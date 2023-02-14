using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public enum AccomodationType { HOTEL, MOTEL, VILLA };

    public class Accommodation
    {
        private AccomodationType type;
        private string name;
        private int stars;
        private bool pool;
        private bool spa;
        private bool handicapFriendly;
        private bool wiFi;
        private bool deleted;
        private int id;     // id aranzmana kom pripada
        private List<AccommodationUnit> allUnits;

        public Accommodation() { }

        public Accommodation(AccomodationType type, string name, int stars, bool pool, bool spa, bool handicapFriendly, bool wiFi)
        {
            this.Type = type;
            this.Name = name;
            if(type == AccomodationType.HOTEL)
                this.Stars = stars;
            else
                this.Stars = 0;
            this.Pool = pool;
            this.Spa = spa;
            this.HandicapFriendly = handicapFriendly;
            this.WiFi = wiFi;
            this.Deleted = false;
            this.AllUnits = new List<AccommodationUnit>();
            this.Id = -1;
        }

        public AccomodationType Type { get => type; set => type = value; }
        public string Name { get => name; set => name = value; }
        public int Stars { get => stars; set => stars = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool Spa { get => spa; set => spa = value; }
        public bool HandicapFriendly { get => handicapFriendly; set => handicapFriendly = value; }
        public bool WiFi { get => wiFi; set => wiFi = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public List<AccommodationUnit> AllUnits { get => allUnits; set => allUnits = value; }
        public int Id { get => id; set => id = value; }

        public override string ToString()
        {
            // type , name, stars, pool , spa , hf, wifi, deleted, id aranzmana 
            return String.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8}", Type, Name, Stars, Pool, Spa, HandicapFriendly, WiFi, deleted, Id);
        }
    }
}