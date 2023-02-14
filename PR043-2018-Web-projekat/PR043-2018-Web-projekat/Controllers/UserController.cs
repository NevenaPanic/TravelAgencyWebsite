using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using PR043_2018_Web_projekat.Models;
using PR043_2018_Web_projekat.CommonFunctions;

namespace PR043_2018_Web_projekat.Controllers
{
    public class UserController : Controller
    {
        static UserController()
        {
            ReadWriteFunctions.ReadAdmins();
            ReadWriteFunctions.ReadManagers();
            ReadWriteFunctions.ReadTourists();
            ReadWriteFunctions.ReadAccommodations();
            ReadWriteFunctions.ReadAccommodationUnits();
            ReadWriteFunctions.ReadArrangements();
            ReadWriteFunctions.AsignArrangementsToManagers();
            ReadWriteFunctions.ReadReservations();
            ReadWriteFunctions.ReadComments();
        }

        // GET: User
        public ActionResult Home()
        {
            List<Arrangement> arrangements = (from Arrangement in Data.arrangements
                                              where Arrangement.StartDate > DateTime.Now
                                              select Arrangement).ToList();
            displayArrangements = arrangements;
            ViewBag.arrangements = arrangements.OrderBy( x => x.StartDate).ToList();

            return View();
        }

        public ActionResult PastArrangements()
        {
            List<Arrangement> arrangements = (from Arrangement in Data.arrangements
                                              where Arrangement.StartDate < DateTime.Now
                                              select Arrangement).ToList();
            displayArrangements = arrangements;
            ViewBag.pastArrangements = arrangements.OrderBy( x => x.EndDate).ToList();

            return View();
        }

        [HttpPost]
        public ActionResult SearchPastArrangements(string startFrom, string startTo, string endFrom, string endTo, string name, string transportation, string type, string search)
        {
            if (search.Equals("Show all"))
                return RedirectToAction("PastArrangements");

            List<Tuple<string, string>> keys = new List<Tuple<string, string>>();
            List<Tuple<DateTime, DateTime>> dates = new List<Tuple<DateTime, DateTime>>();


            dates.Add(new Tuple<DateTime, DateTime>(string.IsNullOrEmpty(startFrom) ? DateTime.MinValue : DateTime.Parse(startFrom), string.IsNullOrEmpty(startTo) ? DateTime.MaxValue : DateTime.Parse(startTo)));
            dates.Add(new Tuple<DateTime, DateTime>(string.IsNullOrEmpty(endFrom) ? DateTime.MinValue : DateTime.Parse(endFrom), string.IsNullOrEmpty(endTo) ? DateTime.MaxValue : DateTime.Parse(endTo)));

            if (!string.IsNullOrEmpty(name))
                keys.Add(new Tuple<string, string>("name", name));
            if (!string.IsNullOrEmpty(type))
                keys.Add(new Tuple<string, string>("type", type));
            if (!string.IsNullOrEmpty(transportation))
                keys.Add(new Tuple<string, string>("transportation", transportation));


            if (!string.IsNullOrEmpty(startFrom) || !string.IsNullOrEmpty(startTo))
            {
                foreach (Arrangement a in displayArrangements.ToList())
                {
                    if (a.StartDate < dates[0].Item1 || a.StartDate > dates[0].Item2)
                        displayArrangements.Remove(a);
                }
            }
            if (endFrom != null || endTo != null)
            {
                foreach (Arrangement a in displayArrangements.ToList())
                {
                    if (a.EndDate < dates[1].Item1 || a.EndDate > dates[1].Item2)
                        displayArrangements.Remove(a);
                }
            }

            foreach (Tuple<string, string> key in keys)
            {
                switch (key.Item1)
                {
                    case "name":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Name.ToLower().Contains(key.Item2.ToLower()))
                                displayArrangements.Remove(a);
                        }
                        break;
                    case "type":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Type.ToString().Equals(key.Item2))
                                displayArrangements.Remove(a);
                        }
                        break;
                    case "transportation":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Transportation.ToString().Equals(key.Item2))
                                displayArrangements.Remove(a);
                        }
                        break;
                }
            }

            ViewBag.pastArrangements = displayArrangements;

            return View("PastArrangements");
        }

        // =================================================================== Sort arrangement ==============================================================================
        [HttpPost]
        public ActionResult SortPastArrangements(string way, string sortType)
        {
            if (string.IsNullOrEmpty(way) || string.IsNullOrEmpty(sortType))
                return RedirectToAction("PastArrangements");
            switch (sortType)
            {
                case "name":
                    if (way == "asc")
                        ViewBag.pastArrangements = displayArrangements.OrderBy(x => x.Name).ToList();
                    else
                        ViewBag.pastArrangements = displayArrangements.OrderByDescending(x => x.Name).ToList();
                    break;
                case "start":
                    if (way == "asc")
                        ViewBag.pastArrangements = displayArrangements.OrderBy(x => x.StartDate).ToList();
                    else
                        ViewBag.pastArrangements = displayArrangements.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "end":
                    if (way == "asc")
                        ViewBag.pastArrangements = displayArrangements.OrderBy(x => x.EndDate).ToList();
                    else
                        ViewBag.pastArrangements = displayArrangements.OrderByDescending(x => x.EndDate).ToList();
                    break;
            }
            return View("PastArrangements");
        }


        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.LoginError = "Please fill all fileds!";
                return View();
            }

            if (!Data.users.ContainsKey(username))
            {
                ViewBag.LoginError = "User with that username doesn't exist!";
                return View();
            }
            else
            {
                if (!password.Equals(Data.users[username].Password))
                {
                    ViewBag.LoginError = "Wrong password!";
                    return View();
                }
                else
                {
                    if (Data.users[username].Role== Role.TOURIST && (Data.users[username] as Tourist).Blocked)
                    {
                        ViewBag.LoginError = "This user is blocked by admins!";
                        return View();
                    }
                    Session["user"] = Data.users[username];
                    return RedirectToAction("Home");
                }
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastname) ||
                string.IsNullOrEmpty(email) || dateOfBirth == DateTime.MinValue)
            {
                ViewBag.RegistrationError = "Please fill in all fileds!";
                return View();
            }

            if (Data.users.ContainsKey(username))
            {
                ViewBag.RegistrationError = "Username taken, please choose another one!";
                return View();
            }
            else
            {
                Tourist newTourist = new Tourist(username, password, name, lastname, (Gender)Enum.Parse(typeof(Gender), gender), email, dateOfBirth, Role.TOURIST);
                Data.users.Add(username, newTourist);
                Session["user"] = newTourist;
                ReadWriteFunctions.WriteUser(newTourist);
            }

            return RedirectToAction("Home");
        }

        public ActionResult LogOut()
        {
            Session["user"] = null;
            return View("LogIn");
        }

        public ActionResult ProfilePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditProfile(string name, string lastname, string password, DateTime dateOfBirth, string email, Gender gender)
        {
            string username = ((User)Session["user"]).Username;
            Role role = ((User)Session["user"]).Role;
            if (!string.IsNullOrEmpty(name) && Data.users[username].Name != name)
                Data.users[username].Name = name;
            if (!string.IsNullOrEmpty(lastname) && Data.users[username].Lastname != lastname)
                Data.users[username].Lastname = lastname;
            if (!string.IsNullOrEmpty(password) && Data.users[username].Password != password)
                Data.users[username].Password = password;
            if (!string.IsNullOrEmpty(email) && Data.users[username].Email != email)
                Data.users[username].Email = email;
            if(Data.users[username].Gender != gender)
                Data.users[username].Gender = gender;
            if (Data.users[username].DateOfBirth != dateOfBirth)
                Data.users[username].DateOfBirth = dateOfBirth;

            switch (role)
            {
                case Role.ADMIN:
                    ReadWriteFunctions.OverwriteAdminsTxt();
                    break;
                case Role.TOURIST:
                    ReadWriteFunctions.OverwriteTouristsTxt();
                    break;
                case Role.MANAGER:
                    ReadWriteFunctions.OverwriteManagersTxt();
                    break;
            }

            return View("ProfilePage");
        }

        // ================================== Search arrangements =========================================================================================
        private static List<Arrangement> displayArrangements = new List<Arrangement>(Data.arrangements);
        
        public ActionResult SearchArrangements(string startFrom, string startTo, string endFrom, string endTo, string name, string transportation, string type, string search)
        {

            if (search.Equals("Show all"))
            {
                displayArrangements = (from Arrangement in Data.arrangements
                                       where Arrangement.StartDate > DateTime.Now
                                       select Arrangement).ToList();
                ViewBag.arrangements = displayArrangements;

                return View("Home");
            }

            List<Tuple<string, string>> keys = new List<Tuple<string, string>>();
            List<Tuple<DateTime, DateTime>> dates = new List<Tuple<DateTime, DateTime>>();


            dates.Add( new Tuple<DateTime,DateTime>( string.IsNullOrEmpty(startFrom) ? DateTime.MinValue : DateTime.Parse(startFrom), string.IsNullOrEmpty(startTo) ? DateTime.MaxValue : DateTime.Parse(startTo)));        
            dates.Add(new Tuple<DateTime, DateTime>(string.IsNullOrEmpty(endFrom) ? DateTime.MinValue : DateTime.Parse(endFrom), string.IsNullOrEmpty(endTo) ? DateTime.MaxValue : DateTime.Parse(endTo)));

            if (!string.IsNullOrEmpty(name))
                keys.Add(new Tuple<string, string>("name", name));
            if (!string.IsNullOrEmpty(type))
                keys.Add(new Tuple<string, string>("type", type));
            if (!string.IsNullOrEmpty(transportation))
                keys.Add(new Tuple<string, string>("transportation", transportation));


            if (!string.IsNullOrEmpty(startFrom)|| !string.IsNullOrEmpty(startTo))
            {
                foreach (Arrangement a in displayArrangements.ToList())
                {
                    if(a.StartDate < dates[0].Item1 || a.StartDate > dates[0].Item2)
                        displayArrangements.Remove(a);
                }
            }
            if (endFrom != null || endTo != null)
            {
                foreach (Arrangement a in displayArrangements.ToList())
                {
                    if (a.EndDate < dates[1].Item1 || a.EndDate > dates[1].Item2)
                        displayArrangements.Remove(a);
                }
            }

            foreach (Tuple<string, string> key in keys)
            {
                switch (key.Item1)
                {
                    case "name":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Name.ToLower().Contains(key.Item2.ToLower()))
                                displayArrangements.Remove(a);
                        }
                        break;
                    case "type":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Type.ToString().Equals(key.Item2))
                                displayArrangements.Remove(a);
                        }
                        break;
                    case "transportation":
                        foreach (Arrangement a in displayArrangements.ToList())
                        {
                            if (!a.Transportation.ToString().Equals(key.Item2))
                                displayArrangements.Remove(a);
                        }
                        break;
                }
            }

            ViewBag.arrangements = displayArrangements;

            return View("Home");
        }
        
        // =================================================================== Sort arrangement ==============================================================================
        [HttpPost]
        public ActionResult SortArrangements(string way, string sortType)
        {
            if (string.IsNullOrEmpty(way) || string.IsNullOrEmpty(sortType))
                return RedirectToAction("Home");
            switch (sortType)
            {
                case "name":
                    if(way == "asc")
                        ViewBag.arrangements = displayArrangements.OrderBy( x => x.Name).ToList();
                    else
                        ViewBag.arrangements = displayArrangements.OrderByDescending( x => x.Name).ToList();
                    break;
                case "start":
                    if (way == "asc")
                        ViewBag.arrangements = displayArrangements.OrderBy(x => x.StartDate).ToList();
                    else
                        ViewBag.arrangements = displayArrangements.OrderByDescending(x => x.StartDate).ToList();
                    break;
                case "end":
                    if (way == "asc")
                        ViewBag.arrangements = displayArrangements.OrderBy(x => x.EndDate).ToList();
                    else
                        ViewBag.arrangements = displayArrangements.OrderByDescending(x => x.EndDate).ToList();
                    break;
            }
            return View("Home");
        }

        private static int index;
        [HttpGet]
        public ActionResult ArrangementDetails()
        {
            index = Int32.Parse(Request["id"]);
            ViewBag.arrangementDetails = Data.arrangements.Find( x => x.Id == index);

            return View("ArrangementDetails");
        }

        // ================================================== Accommodation details =============================================================================================
        private static List<AccommodationUnit> displayUnits;
        private static int accommodationDetails;
        public ActionResult AccommodationDetails()
        {
            if(Request["id"] != null)
                accommodationDetails = Int32.Parse(Request["id"]);
            displayUnits = new List<AccommodationUnit>(Data.accommodations[accommodationDetails].AllUnits);
            ViewBag.accommodation = Data.accommodations[accommodationDetails];
            ViewBag.units = displayUnits.ToList();

            return View();
        }


        public ActionResult SorthUnits( string sortType, string way)
        {
            ViewBag.accommodation = Data.accommodations[accommodationDetails];
            switch (sortType)
            {
                case "guests":
                    if (way.Equals("asc"))
                        ViewBag.units = displayUnits.OrderBy( x => x.NumberOfGuests).ToList();
                    else
                        ViewBag.units = displayUnits.OrderByDescending( x => x.NumberOfGuests).ToList();
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
            foreach (Tuple<string,string> key in keys)
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

            ViewBag.accommodation = Data.accommodations[accommodationDetails];
            ViewBag.units = displayUnits;
            return View("AccommodationDetails");
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnoprstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public ActionResult Reservations()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Reserve( string reserve)
        {
            Tourist t = (Tourist)Session["user"];
            // accommodation id _ unit id
            string[] IDs = reserve.Split('_');

            Data.accommodations[Int32.Parse(IDs[0])].AllUnits[Int32.Parse(IDs[1])].Booked = true;
            string reservationID = "";
            do
            {
                reservationID = RandomString(15);
            } while (Data.reservationsPerUser.ContainsKey(reservationID));
            // public Reservation(string reservationID, Tourist personReserving, ReservationStatus status, Arrangement selectedArrangement, AccommodationUnit accommodationUnit)
            Reservation newReservation = new Reservation(reservationID, t , ReservationStatus.ACTIVE, Data.arrangements[index], Data.accommodations[Int32.Parse(IDs[0])].AllUnits[Int32.Parse(IDs[1])]);
            t.Reservations.Add(newReservation);
            Data.reservationsPerUser.Add(reservationID, newReservation);
            ViewBag.accommodation = Data.accommodations[accommodationDetails];
            ViewBag.units = new List<AccommodationUnit>(Data.accommodations[accommodationDetails].AllUnits);
            ReadWriteFunctions.OverrideAccommodationUnitsTxt();
            ReadWriteFunctions.WriteReservation(newReservation);

            return View("AccommodationDetails");
        }

        [HttpPost]
        public ActionResult CancelReservation(string cancel)
        {
            // cancel = reservation ID
            Data.reservationsPerUser[cancel].AccommodationUnit.Booked = false;
            Data.reservationsPerUser[cancel].Status = ReservationStatus.CANCELED;
            Data.reservationsPerUser[cancel].PersonReserving.NumberOfCancellations++;
            ReadWriteFunctions.OverrideAccommodationUnitsTxt();
            ReadWriteFunctions.OverwriteTouristsTxt();
            ReadWriteFunctions.OverwriteReservationsTxt();

            return View("Reservations");
        }

        // ========================================== Comments ==============================================
        [HttpPost]
        public ActionResult Comment(string comment, string grade)
        {
            if(!string.IsNullOrEmpty(comment) && !string.IsNullOrEmpty(grade))
                Data.comments.Add(new Comment(Session["user"] as Tourist, Data.arrangements[index], comment, Int32.Parse(grade)));
            ReadWriteFunctions.OverwriteCommentsTxt();
            ViewBag.arrangementDetails = Data.arrangements[index];

            return View("ArrangementDetails");
        }
    }
}