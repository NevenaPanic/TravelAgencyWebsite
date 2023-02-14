using PR043_2018_Web_projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace PR043_2018_Web_projekat.CommonFunctions
{
    public class ReadWriteFunctions
    {
        //======================================== Tourist writer ================================================
        public static void WriteUser(Tourist tourist)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Tourists.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(tourist);
                    fs.SetLength(fs.Position);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverwriteTouristsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Tourists.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (User user in Data.users.Values.ToList().FindAll(x => x.Role == Role.TOURIST))
                        writer.WriteLine((Tourist)user);
                    writer.Close();
                }
                fs.Close();
            }
        }


        public static void WriteManager(Manager menager)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Managers.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(menager);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void WriteReservation(Reservation r)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Reservations.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(r);
                    fs.SetLength(fs.Position);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverwriteReservationsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Reservations.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (Reservation r in Data.reservationsPerUser.Values)
                        writer.WriteLine(r);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverwriteManagersTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Managers.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (User user in Data.users.Values.ToList().FindAll(x => x.Role == Role.MANAGER))
                        writer.WriteLine((Manager)user);
                    writer.Close();
                }
                fs.Close();
            }

            path = HostingEnvironment.MapPath("~/App_Data/ManagerArrangements.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (User user in Data.users.Values.ToList().FindAll(x => x.Role == Role.MANAGER))
                    {
                        if (((Manager)user).CreatedArrangements.Count > 0)
                        {
                            string line = user.Username;
                            foreach (Arrangement a in ((Manager)user).CreatedArrangements)
                                line += ";" + a.Id;
                            writer.WriteLine(line);
                        }
                    }
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void AsignArrangementsToManagers()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/ManagerArrangements.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                for (int i = 1; i < parts.Count() - 1; i++)
                    ((Manager)Data.users[parts[0]]).CreatedArrangements.Add(Data.arrangements[Int32.Parse(parts[i])]);

            }
        }

        public static void OverwriteAdminsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Admins.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (User user in Data.users.Values.ToList().FindAll(x => x.Role == Role.ADMIN))
                         writer.WriteLine((Admin)user);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void WriteAccommodation(Accommodation accommodation)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Accommodations.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(accommodation);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void WriteAccommodationUnit(AccommodationUnit accommodationUnit)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/AccommodationUnits.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(accommodationUnit);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void WriteArrangement(Arrangement arrangement)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Arrangements.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(arrangement);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverrideArrangementsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Arrangements.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (Arrangement arrangement in Data.arrangements)
                        writer.WriteLine(arrangement);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverrideAccommodationsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Accommodations.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (Accommodation accommodation in Data.accommodations)
                        writer.WriteLine(accommodation);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverrideAccommodationUnitsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/AccommodationUnits.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (Accommodation accommodation in Data.accommodations)
                    {
                        foreach (AccommodationUnit unit in accommodation.AllUnits)
                            writer.WriteLine(unit);
                    }
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void ReadAdmins()
        {
            // procita sve admine i ako zadovoljaju sve uslove dodaje ih kao admine
            string path = HostingEnvironment.MapPath("~/App_Data/Admins.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                if (!Data.users.ContainsKey(parts[0]))
                {
                    // Username, Password, Name, Lastname, Gender, DateOfBirth.Day, DateOfBirth.Month, DateOfBirth.Year, Email
                    string[] date_parts = parts[5].Split('/');
                    DateTime date = new DateTime(Int32.Parse(date_parts[2]), Int32.Parse(date_parts[1]), Int32.Parse(date_parts[0]));
                    Admin admin = new Admin(parts[0], parts[1], parts[2], parts[3], (Gender)Enum.Parse(typeof(Gender),parts[4]), parts[6], date, Role.ADMIN);
                    Data.users.Add(parts[0], admin);
                }
            }
        }

        public static void ReadManagers()
        {
            // procita sve menagere i ako zadovoljaju sve uslove dodaje ih kao admine
            string path = HostingEnvironment.MapPath("~/App_Data/Managers.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                if (!Data.users.ContainsKey(parts[0]))
                {
                    // Username, Password, Name, Lastname, Gender, DateOfBirth.Day, DateOfBirth.Month, DateOfBirth.Year, Email
                    string[] date_parts = parts[5].Split('/');
                    DateTime date = new DateTime(Int32.Parse(date_parts[2]), Int32.Parse(date_parts[1]), Int32.Parse(date_parts[0]));
                    Manager manager = new Manager(parts[0], parts[1], parts[2], parts[3], (Gender)Enum.Parse(typeof(Gender), parts[4]), parts[6], date, Role.MANAGER);
                    Data.users.Add(parts[0], manager);
                }
            }
        }

        public static void ReadTourists()
        { 
            // procita sve menagere i ako zadovoljaju sve uslove dodaje ih kao admine
            string path = HostingEnvironment.MapPath("~/App_Data/Tourists.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                if (!Data.users.ContainsKey(parts[0]))
                {
                    // Username, Password, Name, Lastname, Gender, DateOfBirth.Day, DateOfBirth.Month, DateOfBirth.Year, Email, suspicious, blocked, numberOfCancelation
                    string[] date_parts = parts[5].Split('/');
                    DateTime date = new DateTime(Int32.Parse(date_parts[2]), Int32.Parse(date_parts[1]), Int32.Parse(date_parts[0]));
                    Tourist tourist = new Tourist(parts[0], parts[1], parts[2], parts[3], (Gender)Enum.Parse(typeof(Gender), parts[4]), parts[6], date, Role.TOURIST);
                    tourist.Suspicious = bool.Parse(parts[7]);
                    tourist.Blocked = bool.Parse(parts[8]);
                    tourist.NumberOfCancellations = Int32.Parse(parts[9]);
                    Data.users.Add(parts[0], tourist);
                }
            }
        }

        public static void ReadAccommodations()
        {
            // procita sve smestaje
            string path = HostingEnvironment.MapPath("~/App_Data/Accommodations.txt");
            string[] lines = File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                // type , name, stars, pool , spa , hf, wifi, deleted, id aranzmana 
                // public Accommodation(AccomodationType type, string name, int stars, bool pool, bool spa, bool handicapFriendly, bool wiFi)
                Accommodation read = new Accommodation((AccomodationType)Enum.Parse(typeof(AccomodationType), parts[0]), parts[1], Int32.Parse(parts[2]), bool.Parse(parts[3]), bool.Parse(parts[4]), bool.Parse(parts[6]), bool.Parse(parts[5]));
                read.Deleted = bool.Parse(parts[7]);
                read.Id = Int32.Parse(parts[8]);
                Data.accommodations.Add(read);
            }
        }

        public static void ReadReservations()
        {
            // procita sve smestaje
            string path = HostingEnvironment.MapPath("~/App_Data/Reservations.txt");
            string[] lines = File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                // id, uusername, status, id arrangementa, id accommodation unit-a
                // Reservation(string reservationID, Tourist personReserving, ReservationStatus status, Arrangement selectedArrangement, AccommodationUnit accommodationUnit)
                Reservation readOne = new Reservation(parts[0], (Tourist)Data.users[parts[1]], (ReservationStatus)Enum.Parse(typeof(ReservationStatus), parts[2]), 
                    Data.arrangements[Int32.Parse(parts[3])], Data.arrangements[Int32.Parse(parts[3])].Accommodation.AllUnits[Int32.Parse(parts[4])]);
                ((Tourist)Data.users[parts[1]]).Reservations.Add(readOne);
                Data.reservationsPerUser.Add(parts[0], readOne);
            }
        }

        public static void ReadAccommodationUnits()
        {
            // sad se procitaju sve smestajne jedinice
            string path = HostingEnvironment.MapPath("~/App_Data/AccommodationUnits.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                // id acoomodation unit it belongs to, price, beds, petsAllowd, booked, deleted
                //  public AccommodationUnit(bool petsAllowed, int priceForUnit, int numberOfGuests, uint id)
                int accomodationId = Int32.Parse(parts[0]);
                AccommodationUnit readUnit = new AccommodationUnit(bool.Parse(parts[3]), Int32.Parse(parts[1]), Int32.Parse(parts[2]), (UInt32)accomodationId);
                readUnit.Deleted = bool.Parse(parts[5]);
                readUnit.Booked = bool.Parse(parts[4]);
                Data.accommodations[accomodationId].AllUnits.Add(readUnit);
            }
        }

        public static void ReadComments()
        {
            // sad se procitaju sve smestajne jedinice
            string path = HostingEnvironment.MapPath("~/App_Data/Comments.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                /*
                 Id, approved, commentText, grade, commentator.Username, commentedArrangement.Id
                 public Comment(Tourist commentator, Arrangement commentedArrangement, string commentText, int grade)*/
                Comment newComment = new Comment(Data.users[parts[4]] as Tourist, Data.arrangements[Int32.Parse(parts[5])], parts[2], Int32.Parse(parts[3]));
                newComment.Approved = bool.Parse(parts[1]);
                newComment.Id = Int32.Parse(parts[0]);
                Data.comments.Add(newComment);
                
            }
        }

        public static void WriteComment(Comment comment)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Comments.txt");
            using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fs.Seek(0, SeekOrigin.End);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(comment);
                    fs.SetLength(fs.Position);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void OverwriteCommentsTxt()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/Comments.txt");
            using (FileStream fs = new FileStream(path, FileMode.Truncate))
            {
                fs.Seek(0, SeekOrigin.Begin);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    foreach (Comment com in Data.comments)
                        writer.WriteLine(com);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public static void ReadArrangements()
        {
            // sad se procitaju sve smestajne jedinice
            string path = HostingEnvironment.MapPath("~/App_Data/Arrangements.txt");
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
            {
                string[] parts = s.Split(';');
                string[] sd = parts[5].Split('/');
                string[] ed = parts[6].Split('/');
                // id, name, type, transportation, location, StartDate, endDate, address, lat, long, meeting time, maxPassengers, description, travelPlan, image, accommodation id, deleted, managerID
                //  public Arrangement(string name, ArrangmentType type, TransportationType transportation, string location, DateTime startDate, DateTime endDate,
                // MeetingPlace meetingSpot, string meetingTime, int maxPassengers, string description, string travelPlan, string image, Accommodation accommodation)

                Arrangement readArrangement = new Arrangement(parts[1], (ArrangmentType)Enum.Parse(typeof(ArrangmentType),parts[2]), (TransportationType)Enum.Parse(typeof(TransportationType),parts[3]),
                    parts[4], new DateTime(Int32.Parse(sd[2]), Int32.Parse(sd[1]), Int32.Parse(sd[0])), new DateTime(Int32.Parse(ed[2]), Int32.Parse(ed[1]), Int32.Parse(ed[0])),
                    new MeetingPlace(parts[7], Double.Parse(parts[8]), Double.Parse(parts[9])), parts[10], Int32.Parse(parts[11]), parts[12], parts[13], parts[14], Data.accommodations[Int32.Parse(parts[15])]);
                readArrangement.Deleted = bool.Parse(parts[16]);
                readArrangement.ManagerID = parts[17];
                // dodavanje aranzamana u menadzera koji ga je kreirao
                ((Manager)Data.users[parts[17]]).CreatedArrangements.Add(readArrangement);

                Data.arrangements.Add(readArrangement);
            }
        }
    }
}