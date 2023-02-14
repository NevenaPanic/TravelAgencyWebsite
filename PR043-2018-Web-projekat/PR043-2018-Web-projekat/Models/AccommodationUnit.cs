using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class AccommodationUnit
    {
        private bool petsAllowed;
        private int priceForUnit;
        private int numberOfGuests;
        private uint id;         // ID ACCOMMODATION WITCH CONTAINS THIS UNIT
        private bool deleted;
        private bool booked;

        public AccommodationUnit() { }

        public AccommodationUnit(bool petsAllowed, int priceForUnit, int numberOfGuests, uint id)
        {
            this.PetsAllowed = petsAllowed;
            this.PriceForUnit = priceForUnit;
            this.NumberOfGuests = numberOfGuests;
            this.Id = id;
            this.Deleted = false;
            this.Booked = false;
        }

        public bool PetsAllowed { get => petsAllowed; set => petsAllowed = value; }
        public int PriceForUnit { get => priceForUnit; set => priceForUnit = value; }
        public int NumberOfGuests { get => numberOfGuests; set => numberOfGuests = value; }
        public uint Id { get => id; set => id = value; }
        public bool Deleted { get => deleted; set => deleted = value; }
        public bool Booked { get => booked; set => booked = value; }

        public override string ToString()
        {
            // id acoomodation unit it belongs to, price, beds, petsAllowd, booked, deleted
            return String.Format("{0};{1};{2};{3};{4};{5}", Id, PriceForUnit, NumberOfGuests, PetsAllowed, Booked, Deleted);
        }
    }
}