using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class MeetingPlace
    {
        private string address;         // specijalni raspored : ulica i broj, mesto/grad, poštanski broj
        private double longitude;       // duzina
        private double latitude;        // sirina

        public MeetingPlace() { }

        public MeetingPlace(string address, double longitude, double latitude)
        {
            this.Address = address;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        public string Address { get => address; set => address = value; }
        public double Longitude { get => longitude; set => longitude = value; }
        public double Latitude { get => latitude; set => latitude = value; }
    }
}