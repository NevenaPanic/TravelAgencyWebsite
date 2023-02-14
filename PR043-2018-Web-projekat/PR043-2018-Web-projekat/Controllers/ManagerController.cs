using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PR043_2018_Web_projekat.Models;
using System.Web.Mvc;
using PR043_2018_Web_projekat.CommonFunctions;
using System.IO;

namespace PR043_2018_Web_projekat.Controllers
{
    public class ManagerController : Controller
    {
        static ManagerController()
        {
        }

        private static List<string> imageExtentions = new List<string>() { ".png", ".jpg", ".jpeg", ".jfif", ".bmp", ".tif", ".tiff", ".gif" };

        private bool CheckIfImage(string filename)
        {
            if (imageExtentions.Contains(Path.GetExtension(filename)))
                return true;
            return false;
        }

        // GET: Manager
        public ActionResult Arrangements()
        {
            return View();
        }

        public ActionResult AddArrangement()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddArrangement(string name, string type, string transoprtation, string destinationCity, string destinationState, DateTime startDate, DateTime endDate, string lat,
            string lon, string hour, string minutes, string passengers, string description, string plan, HttpPostedFileBase image, string accommodation, string address, string city, string zipCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(destinationCity) || string.IsNullOrEmpty(destinationState) ||
                string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lon) || string.IsNullOrEmpty(hour) || string.IsNullOrEmpty(minutes) ||
                string.IsNullOrEmpty(passengers) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(plan) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(city) || string.IsNullOrEmpty(zipCode) || image.ContentLength <= 0)
            {
                ViewBag.AddArrangementError = "Must fill in all the fields!";
                return View();
            }

            if (endDate < startDate || startDate < DateTime.Now)
            {
                ViewBag.AddArrangementError = "Dates are not valid!";
                return View();
            }

            if (CheckIfImage(image.FileName) == false)
            {
                ViewBag.AddArrangementError = "Bad file, please choose image files! [  .png, .jpg, .jpeg, .jfif, .bmp, .tif, .tiff, .gif ]";
                return View();
            }

            int h, m, numberOfPassengers;
            string time = "";

            if (Int32.TryParse(hour, out h) && Int32.TryParse(minutes, out m))
            {
                if (h < 0 || h > 23 || m < 0 || m > 59)
                {
                    ViewBag.AddArrangementError = "Invalid meeting time";
                    return View();
                }
                else
                    time = (h < 10 ? "0" + hour : hour) + ":" + (m < 10 ? "0" + minutes : minutes);
            }

            if (Int32.TryParse(passengers, out numberOfPassengers))
            {
                if (numberOfPassengers <= 0)
                {
                    ViewBag.AddArrangementError = "Invalid number of passengers";
                    return View();
                }
            }

            string filename = Path.GetFileName(image.FileName);                      // da uzmes ime fajla koje ti je poslato
            string path = Path.Combine(Server.MapPath("~/Images/"), filename);      // da napravis putanju do tvog foldera sa slikama
            image.SaveAs(path);                                                      // Da ga sacuvas  u tom folderu sa tim imenom


            Arrangement newArrangement = new Arrangement(name, (ArrangmentType)Enum.Parse(typeof(ArrangmentType), type), (TransportationType)Enum.Parse(typeof(TransportationType), transoprtation),
                destinationCity + "," + destinationState, startDate, endDate, new MeetingPlace(address + ", " + city + ", " + zipCode, Double.Parse(lon), Double.Parse(lat)),
                time, Int32.Parse(passengers), description, plan, filename, Data.accommodations[Int32.Parse(accommodation)]);

            Manager currentUser = (Manager)Session["user"];
            newArrangement.ManagerID = currentUser.Username;
            ((Manager)Data.users[currentUser.Username]).CreatedArrangements.Add(newArrangement);
            // moras da prepises menagera i da cuvas listu aranzmana koju je on napravio


            Data.arrangements.Add(newArrangement);
            Data.accommodations[Int32.Parse(accommodation)].Id = Data.arrangements.Count - 1;
            ReadWriteFunctions.WriteArrangement(newArrangement);
            ReadWriteFunctions.OverrideAccommodationsTxt();

            return RedirectToAction("Arrangements");
        }

        // GET: Manager
        private static List<Accommodation> currentAccommodations;
        public ActionResult Accommodations()
        {
            currentAccommodations = new List<Accommodation>(Data.accommodations);
            ViewBag.Accommodations = currentAccommodations;
            return View();
        }

        // GET: Manager
        public ActionResult AddAccommodation()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAccommodation(string name, string accommodationType, string stars, string pool, string spa, string handicapFriendly, string wiFi)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(accommodationType) || string.IsNullOrEmpty(pool) || string.IsNullOrEmpty(spa) ||
                string.IsNullOrEmpty(wiFi) || string.IsNullOrEmpty(handicapFriendly))
            {
                ViewBag.Error = "Fill in all fileds, please!";
                return View();
            }

            AccomodationType type = (AccomodationType)Enum.Parse(typeof(AccomodationType), accommodationType);
            int numOfStars = -1;
            if (type == AccomodationType.HOTEL)
            {
                if (string.IsNullOrEmpty(stars))
                {
                    ViewBag.Error = "Fill in all fileds, please!";
                    return View();
                }
                else
                {
                    numOfStars = Int32.Parse(stars);
                }
            }

            Accommodation newAccommodation = new Accommodation(type, name, numOfStars, bool.Parse(pool), bool.Parse(spa), bool.Parse(handicapFriendly), bool.Parse(wiFi));
            ReadWriteFunctions.WriteAccommodation(newAccommodation);
            Data.accommodations.Add(newAccommodation);
            currentAccommodations = Data.accommodations;
            ViewBag.Accommodations = currentAccommodations;

            return RedirectToAction("Accommodations");
        }

        // ovo puca
        public ActionResult AccommodationDelete()
        {
            if (Request["id"] == null)
            {
                ViewBag.Accommodations = currentAccommodations;
                return View("Accommodations");
            }

            int index = Int32.Parse(Request["id"]);

            Data.accommodations[index].Deleted = true;
            ReadWriteFunctions.OverrideAccommodationsTxt();
            currentAccommodations = new List<Accommodation>(Data.accommodations);
            ViewBag.Accommodations = currentAccommodations;

            return RedirectToAction("Accommodations");
        }

        private static int editedAccommodation;
        public ActionResult AccommodationEdit()
        {
            if (Request["id"] == null)
            {
                return View("Accommodations");
            }
            editedAccommodation = Int32.Parse(Request["id"]);
            ViewBag.editedAccommodation = editedAccommodation;

            return View();
        }

        [HttpPost]
        public ActionResult AccommodationEdit(string name, string accommodationType, string stars, string pool, string spa, string handicapFriendly, string wiFi)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(accommodationType) || string.IsNullOrEmpty(pool) || string.IsNullOrEmpty(spa) ||
                 string.IsNullOrEmpty(wiFi) || string.IsNullOrEmpty(handicapFriendly))
            {
                ViewBag.EditAccommodationError = "Fill in all fileds, please!";
                ViewBag.editedAccommodation = editedAccommodation;
                return View();
            }

            Accommodation edited = Data.accommodations[editedAccommodation];

            edited.Type = (AccomodationType)Enum.Parse(typeof(AccomodationType), accommodationType);
            if (edited.Type == AccomodationType.HOTEL)
            {
                if (!string.IsNullOrEmpty(stars))
                    edited.Stars = Int32.Parse(stars);
                else
                {
                    ViewBag.EditAccommodationError = "Fill in all fileds, please!";
                    ViewBag.editedAccommodation = editedAccommodation;
                    return View();
                }
            }

            edited.Name = name;
            edited.Pool = bool.Parse(pool);
            edited.Spa = bool.Parse(spa);
            edited.HandicapFriendly = bool.Parse(handicapFriendly);
            edited.WiFi = bool.Parse(wiFi);

            Data.accommodations[editedAccommodation] = edited;
            ReadWriteFunctions.OverrideAccommodationsTxt();
            currentAccommodations = Data.accommodations;
            ViewBag.Accommodations = currentAccommodations;

            return View("Accommodations");
        }


        // global id
        public static int AccommodationID = -1;

        [HttpGet]
        public ActionResult AccommodationDetails()
        {
            if (Request["id"] == null)
            {
                ViewBag.units = displayUnits;
                ViewBag.accommodation = Data.accommodations[AccommodationID];
                return View();
            }

            AccommodationID = Int32.Parse(Request["id"]);
            displayUnits = new List<AccommodationUnit>(Data.accommodations[AccommodationID].AllUnits);
            ViewBag.accommodation = Data.accommodations[AccommodationID];
            ViewBag.units = displayUnits;
            return View();
        }

        [HttpPost]
        public ActionResult AddUnit(string pets, string guests, string price)
        {
            if (string.IsNullOrEmpty(pets) || string.IsNullOrEmpty(guests) || string.IsNullOrEmpty(price))
            {
                ViewBag.DetailsError = "Please fill in all the fields!";
                return RedirectToAction("AccommodationDetails");
            }

            int unitePrice, uniteGuests;
            if (Int32.TryParse(price, out unitePrice) && Int32.TryParse(guests, out uniteGuests) && unitePrice > 0 && uniteGuests > 0)
            {
                AccommodationUnit newUnit = new AccommodationUnit(bool.Parse(pets), unitePrice, uniteGuests, (UInt32)AccommodationID);
                Data.accommodations[AccommodationID].AllUnits.Add(newUnit);
                ReadWriteFunctions.WriteAccommodationUnit(newUnit);
                return RedirectToAction("AccommodationDetails");
            }
            else
            {
                ViewBag.DetailsError = "All numbers must be positive!";
                return RedirectToAction("AccommodationDetails");
            }
        }

        private static int a, u;
        [HttpPost]
        public ActionResult ChangeUnit(string delete, string edit)
        {
            if (!string.IsNullOrEmpty(delete))
            {
                // dodati proveru da li je ova jedinica rezervisana za neki od buducih aranzmana
                string[] deleteParts = delete.Split('_');
                // logicki obrisane
                ViewBag.units = displayUnits;
                if (Data.reservationsPerUser.Values.ToList().Find(x => x.AccommodationUnit == Data.accommodations[Int32.Parse(deleteParts[0])].AllUnits[Int32.Parse(deleteParts[1])]) != null)
                {
                    return RedirectToAction("AccommodationDetails");
                }
                Data.accommodations[Int32.Parse(deleteParts[0])].AllUnits[Int32.Parse(deleteParts[1])].Deleted = true;
                ReadWriteFunctions.OverrideAccommodationUnitsTxt();

                return RedirectToAction("AccommodationDetails");
            }
            else
            {
                string[] editedParts = edit.Split('_');
                a = Int32.Parse(editedParts[0]);
                ViewBag.editedAccommodation = a;
                u = Int32.Parse(editedParts[1]);
                ViewBag.editedUnit = u;
                return View("EditUnit");
            }
        }

        public ActionResult EditUnit()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditUnit(string pets, string guests, string price)
        {
            if (string.IsNullOrEmpty(pets) || string.IsNullOrEmpty(guests) || string.IsNullOrEmpty(price))
            {
                ViewBag.EditUnitError = "Must fill in all the fileds!";
                return View();
            }

            if (!string.IsNullOrEmpty(pets))
                Data.accommodations[a].AllUnits[u].PetsAllowed = bool.Parse(pets);
            if (!string.IsNullOrEmpty(price))
                Data.accommodations[a].AllUnits[u].PriceForUnit = Int32.Parse(price);

            if (!Data.accommodations[a].AllUnits[u].Booked)
            {
                if (!string.IsNullOrEmpty(guests))
                    Data.accommodations[a].AllUnits[u].NumberOfGuests = Int32.Parse(guests);

            }
            ReadWriteFunctions.OverrideAccommodationUnitsTxt();

            return RedirectToAction("AccommodationDetails");
        }

        [HttpPost]
        public ActionResult SortAccommodations(string order, string type)
        {
            if (string.IsNullOrEmpty(order) || string.IsNullOrEmpty(type))
                return RedirectToAction("Accommodations");

            switch (type)
            {
                case "name":
                    if (order == "asc")
                        ViewBag.Accommodations = currentAccommodations.OrderBy(x => x.Name).ToList();
                    else
                        ViewBag.Accommodations = currentAccommodations.OrderByDescending(x => x.Name).ToList();
                    break;
                case "total":
                    if (order == "asc")
                        ViewBag.Accommodations = currentAccommodations.OrderBy(x => x.AllUnits.Count).ToList();
                    else
                        ViewBag.Accommodations = currentAccommodations.OrderByDescending(x => x.AllUnits.Count).ToList();
                    break;
                case "free":
                    List<Tuple<Int32, Accommodation>> helpList = new List<Tuple<int, Accommodation>>();
                    int freeUnits = 0;
                    foreach (Accommodation a in currentAccommodations)
                    {
                        foreach (AccommodationUnit unit in a.AllUnits)
                        {
                            if (!unit.Booked)
                                freeUnits++;
                        }
                        helpList.Add(new Tuple<int, Accommodation>(freeUnits, a));
                        freeUnits = 0;
                    }
                    if (order == "asc")
                        helpList = helpList.OrderBy(x => x.Item1).ToList();
                    else
                        helpList = helpList.OrderByDescending(x => x.Item1).ToList();

                    currentAccommodations.Clear();
                    foreach (var item in helpList)
                        currentAccommodations.Add(item.Item2);

                    ViewBag.Accommodations = currentAccommodations;
                    break;

            }
            return View("Accommodations");
        }

        [HttpPost]
        public ActionResult SearchAccommodations(string spa, string pool, string wiFi, string name, string type, string handicap, string search)
        {

            if (search.Equals("Show all"))
            {
                currentAccommodations = Data.accommodations;
                ViewBag.Accommodations = currentAccommodations;
                return View("Accommodations");
            }

            List<Accommodation> alreadySearched = new List<Accommodation>(currentAccommodations);
            List<Tuple<string, string>> searchKeys = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(spa))
                searchKeys.Add(new Tuple<string, string>("spa", spa));
            if (!string.IsNullOrEmpty(pool))
                searchKeys.Add(new Tuple<string, string>("pool", pool));
            if (!string.IsNullOrEmpty(wiFi))
                searchKeys.Add(new Tuple<string, string>("wiFi", wiFi));
            if (!string.IsNullOrEmpty(handicap))
                searchKeys.Add(new Tuple<string, string>("handicap", handicap));
            if (!string.IsNullOrEmpty(type))
                searchKeys.Add(new Tuple<string, string>("type", type));
            if (!string.IsNullOrEmpty(name))
                searchKeys.Add(new Tuple<string, string>("name", name));

            foreach (var key in searchKeys)
            {
                switch (key.Item1)
                {
                    case "spa":
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (a.Spa != bool.Parse(key.Item2))
                                alreadySearched.Remove(a);
                        }
                        break;
                    case "pool":
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (a.Pool != bool.Parse(key.Item2))
                                alreadySearched.Remove(a);
                        }
                        break;
                    case "wiFi":
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (a.WiFi != bool.Parse(key.Item2))
                                alreadySearched.Remove(a);
                        }
                        break;
                    case "handicap":
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (a.HandicapFriendly != bool.Parse(key.Item2))
                                alreadySearched.Remove(a);
                        }
                        break;
                    case "type":
                        AccomodationType typeFilter = (AccomodationType)Enum.Parse(typeof(AccomodationType), type);
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (a.Type != typeFilter)
                                alreadySearched.Remove(a);
                        }
                        break;
                    case "name":
                        foreach (Accommodation a in alreadySearched.ToList())
                        {
                            if (!a.Name.ToLower().Contains(key.Item2.ToLower()))
                                alreadySearched.Remove(a);
                        }
                        break;
                }
            }

            currentAccommodations = alreadySearched;
            ViewBag.Accommodations = currentAccommodations;

            return View("Accommodations");
        }

        //============================================== Accommodation units search and sort ================================================================================
        private static List<AccommodationUnit> displayUnits;
        public ActionResult SorthUnits(string sortType, string way)
        {
            ViewBag.accommodation = Data.accommodations[AccommodationID];
            switch (sortType)
            {
                case "guests":
                    if (way.Equals("asc"))
                        ViewBag.units = displayUnits.OrderBy(x => x.NumberOfGuests).ToList();
                    else
                        ViewBag.units = displayUnits.OrderByDescending(x => x.NumberOfGuests).ToList();
                    break;
                case "price":
                    if (way.Equals("asc"))
                        ViewBag.units = displayUnits.OrderBy(x => x.PriceForUnit).ToList();
                    else
                        ViewBag.units = displayUnits.OrderByDescending(x => x.PriceForUnit).ToList();
                    break;
            }

            return View("AccommodationDetails");
        }

        public ActionResult SearchUnits(string minGuests, string maxGuests, string pets, string minPrice, string maxPrice, string search)
        {
            if (search.Equals("Show all"))
                return RedirectToAction("AccommodationDetails");

            List<Tuple<string, string>> keys = new List<Tuple<string, string>>();

            if (!string.IsNullOrEmpty(minGuests))
                keys.Add(new Tuple<string, string>("minGuests", minGuests));
            if (!string.IsNullOrEmpty(maxGuests))
                keys.Add(new Tuple<string, string>("maxGuests", maxGuests));
            if (!string.IsNullOrEmpty(pets))
                keys.Add(new Tuple<string, string>("pets", pets));
            if (!string.IsNullOrEmpty(minPrice))
                keys.Add(new Tuple<string, string>("minPrice", minPrice));
            if (!string.IsNullOrEmpty(maxPrice))
                keys.Add(new Tuple<string, string>("maxPrice", maxPrice));

            int temp;
            bool tempB;
            foreach (Tuple<string, string> key in keys)
            {
                switch (key.Item1)
                {
                    case "minGuests":
                        temp = Int32.Parse(minGuests);
                        displayUnits = displayUnits.FindAll(x => x.NumberOfGuests >= temp).ToList();
                        break;
                    case "maxGuests":
                        temp = Int32.Parse(maxGuests);
                        displayUnits = displayUnits.FindAll(x => x.NumberOfGuests <= temp).ToList();
                        break;
                    case "pets":
                        tempB = bool.Parse(pets);
                        displayUnits = displayUnits.FindAll(x => x.PetsAllowed == tempB).ToList();
                        break;
                    case "minPrice":
                        temp = Int32.Parse(minPrice);
                        displayUnits = displayUnits.FindAll(x => x.PriceForUnit >= temp).ToList();
                        break;
                    case "maxPrice":
                        temp = Int32.Parse(maxPrice);
                        displayUnits = displayUnits.FindAll(x => x.PriceForUnit <= temp).ToList();
                        break;
                }
            }

            ViewBag.accommodation = Data.accommodations[AccommodationID];
            ViewBag.units = displayUnits;
            return View("AccommodationDetails");
        }
        
        public ActionResult DeleteArrangement()
        {
            if (Request["id"] != null)
            {
                int index = Int32.Parse(Request["id"]);
                Data.arrangements[index].Accommodation.Id = -1;
                Data.arrangements[index].Deleted = true;
                ReadWriteFunctions.OverrideArrangementsTxt();
            }
            return RedirectToAction("Arrangements");
        }

        public ActionResult EditArrangement()
        {
            if (Request["id"] != null)
            {
                int id = Int32.Parse(Request["id"]);
                ViewBag.ID = id;
                ViewBag.editedArrangement = Data.arrangements[id];
                return View();
            }
            return RedirectToAction("Arrangements");
        }

        [HttpPost]
        public ActionResult EditArrangement(string identificator, string name, string type, string transoprtation, string destinationCity, string destinationState, DateTime startDate, DateTime endDate, string lat,
            string lon, string hour, string minutes, string passengers, string description, string plan, HttpPostedFileBase image, string accommodation, string address, string city, string zipCode)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(destinationCity) || string.IsNullOrEmpty(destinationState) ||
                string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lon) || string.IsNullOrEmpty(hour) || string.IsNullOrEmpty(minutes) ||
                string.IsNullOrEmpty(passengers) || string.IsNullOrEmpty(description) || string.IsNullOrEmpty(plan) || string.IsNullOrEmpty(address) ||
                string.IsNullOrEmpty(city) || string.IsNullOrEmpty(zipCode))
            {
                ViewBag.EditArrangementError = "Must fill in all the fields!";
                return View();
            }

            if (endDate < startDate || startDate < DateTime.Now)
            {
                ViewBag.EditArrangementError = "Dates are not valid!";
                return View();
            }

            if (CheckIfImage(image.FileName) == false)
            {
                ViewBag.EditArrangementError = "Bad file, please choose image files! [  .png, .jpg, .jpeg, .jfif, .bmp, .tif, .tiff, .gif ]";
                return View();
            }

            int h, m, numberOfPassengers;
            string time = "";

            if (Int32.TryParse(hour, out h) && Int32.TryParse(minutes, out m))
            {
                if (h < 0 || h > 23 || m < 0 || m > 59)
                {
                    ViewBag.EditArrangementError = "Invalid meeting time";
                    return View();
                }
                else
                    time = (h < 10 ? "0" + h.ToString() : h.ToString()) + ":" + (m < 10 ? "0" + m.ToString() : m.ToString());
            }

            if (Int32.TryParse(passengers, out numberOfPassengers))
            {
                if (numberOfPassengers <= 0)
                {
                    ViewBag.EditArrangementError = "Invalid number of passengers";
                    return View();
                }
            }

            string filename = Path.GetFileName(image.FileName);                      // da uzmes ime fajla koje ti je poslato
            string path = Path.Combine(Server.MapPath("~/Images/"), filename);      // da napravis putanju do tvog foldera sa slikama
            image.SaveAs(path);                                                      // Da ga sacuvas  u tom folderu sa tim imenom

            Data.arrangements[Int32.Parse(identificator)].Name = name;
            Data.arrangements[Int32.Parse(identificator)].Type = (ArrangmentType)Enum.Parse(typeof(ArrangmentType), type);
            Data.arrangements[Int32.Parse(identificator)].Transportation = (TransportationType)Enum.Parse(typeof(TransportationType), transoprtation);
            Data.arrangements[Int32.Parse(identificator)].Location = destinationCity + "," + destinationState;
            Data.arrangements[Int32.Parse(identificator)].StartDate = startDate;
            Data.arrangements[Int32.Parse(identificator)].EndDate = endDate;
            Data.arrangements[Int32.Parse(identificator)].MeetingSpot = new MeetingPlace(address + ", " + city + ", " + zipCode, Double.Parse(lon), Double.Parse(lat));
            Data.arrangements[Int32.Parse(identificator)].MeetingTime = time;
            Data.arrangements[Int32.Parse(identificator)].MaxPassengers = Int32.Parse(passengers);
            Data.arrangements[Int32.Parse(identificator)].Description = description;
            Data.arrangements[Int32.Parse(identificator)].TravelPlan = plan;
            Data.arrangements[Int32.Parse(identificator)].Image = filename;
            if (Data.accommodations.IndexOf(Data.arrangements[Int32.Parse(identificator)].Accommodation) != Int32.Parse(accommodation))
                Data.arrangements[Int32.Parse(identificator)].Accommodation.Id = -1;
            Data.arrangements[Int32.Parse(identificator)].Accommodation = Data.accommodations[Int32.Parse(accommodation)];

            ReadWriteFunctions.OverrideArrangementsTxt();

            return RedirectToAction("Arrangements");
        }


        public ActionResult Comments()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ApproveComment(string id)
        {
            Data.comments[Int32.Parse(id)].Approved = true;
            ReadWriteFunctions.OverwriteCommentsTxt();
            return RedirectToAction("Comments");
        }
    }
}