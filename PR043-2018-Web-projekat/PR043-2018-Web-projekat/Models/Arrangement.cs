using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public enum ArrangmentType { BED_AND_BREAKFAST, HALF_BOARD, FULL_BOARD, ALL_INCLUSIVE_BOARD, RENTED_APARTMENT};
    public enum TransportationType { BUS, BUS_AND_AIRPLANE, AIRPLANE, INDIVIDUAL, OTHER};

    public class Arrangement
    {
        private static uint counter = 0;     // pomocna promenljiva
        private string name;
        private ArrangmentType type;
        private TransportationType transportation;
        private string location;        // will be split with ','
        private DateTime startDate;     // will be "dd/MM/yyyy"
        private DateTime endDate;
        private MeetingPlace meetingSpot;
        private string meetingTime;         // will be in format HH:mm
        private int maxPassengers;
        private string description;
        private string travelPlan;
        private string image;           // name of this image in my file Images
        private Accommodation accommodation;
        private uint id = 0;
        private string managerID;
        private bool deleted;

        public Arrangement() { }

        public Arrangement(string name, ArrangmentType type, TransportationType transportation, string location, DateTime startDate, DateTime endDate,
                            MeetingPlace meetingSpot, string meetingTime, int maxPassengers, string description, string travelPlan, string image, Accommodation accommodation)
        {
            this.Name = name;
            this.Type = type;
            this.Transportation = transportation;
            this.Location = location;
            this.StartDate = startDate;
            this.EndDate = endDate;
            this.MeetingSpot = meetingSpot;
            this.MeetingTime = meetingTime;
            this.MaxPassengers = maxPassengers;
            this.Description = description;
            this.TravelPlan = travelPlan;
            this.Image = image;
            this.Accommodation = accommodation;
            this.Id = counter++;
            this.Deleted = false;
        }

        public string Name { get => name; set => name = value; }
        public ArrangmentType Type { get => type; set => type = value; }
        public TransportationType Transportation { get => transportation; set => transportation = value; }
        public string Location { get => location; set => location = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public MeetingPlace MeetingSpot { get => meetingSpot; set => meetingSpot = value; }
        public string MeetingTime { get => meetingTime; set => meetingTime = value; }
        public int MaxPassengers { get => maxPassengers; set => maxPassengers = value; }
        public string Description { get => description; set => description = value; }
        public string TravelPlan { get => travelPlan; set => travelPlan = value; }
        public string Image { get => image; set => image = value; }
        public Accommodation Accommodation { get => accommodation; set => accommodation = value; }
        public uint Id { get => id; set => id = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public string ManagerID { get => managerID; set => managerID = value; }

        // smisli kako ces ovo da cuvas :(
        public override string ToString()
        {
            // id, name, type, transportation, location, StartDate, endDate, address, lat, long, meeting time, maxPassengers, description, travelPlan, image, accommodation id, deleted, managerID
            return String.Format("{0};{1};{2};{3};{4};{5}/{6}/{7};{8}/{9}/{10};{11};{12};{13};{14};{15};{16};{17};{18};{19};{20};{21}",
                id, name, type, transportation, location, startDate.Day, startDate.Month, startDate.Year, endDate.Day, endDate.Month, endDate.Year,
                meetingSpot.Address, meetingSpot.Latitude, meetingSpot.Longitude, meetingTime, maxPassengers, description, travelPlan, image, 
                Data.accommodations.IndexOf(accommodation), deleted, managerID);
        }
    }
}