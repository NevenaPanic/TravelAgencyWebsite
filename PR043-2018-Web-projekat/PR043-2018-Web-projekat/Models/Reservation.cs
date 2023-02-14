using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public enum ReservationStatus { ACTIVE, CANCELED};
    public class Reservation
    {
        private string reservationID;       // jedinstven, 15 karaktera
        private Tourist personReserving;  // rezervator aranzmana
        private ReservationStatus status;
        private Arrangement selectedArrangement;
        private AccommodationUnit accommodationUnit;

        public Reservation() { }

        public Reservation(string reservationID, Tourist personReserving, ReservationStatus status, Arrangement selectedArrangement, AccommodationUnit accommodationUnit)
        {
            this.ReservationID = reservationID;
            this.PersonReserving = personReserving;
            this.Status = status;
            this.SelectedArrangement = selectedArrangement;
            this.AccommodationUnit = accommodationUnit;
        }

        public string ReservationID { get => reservationID; set => reservationID = value; }
        public Tourist PersonReserving { get => personReserving; set => personReserving = value; }
        public ReservationStatus Status { get => status; set => status = value; }
        public Arrangement SelectedArrangement { get => selectedArrangement; set => selectedArrangement = value; }
        public AccommodationUnit AccommodationUnit { get => accommodationUnit; set => accommodationUnit = value; }

        public override string ToString()
        {
            // id, uusername, status, id arrangementa, id accommodation unit-a
            return String.Format("{0};{1};{2};{3};{4}", reservationID, personReserving.Username, status, selectedArrangement.Id, 
                                                        selectedArrangement.Accommodation.AllUnits.IndexOf(accommodationUnit));
        }
    }
}