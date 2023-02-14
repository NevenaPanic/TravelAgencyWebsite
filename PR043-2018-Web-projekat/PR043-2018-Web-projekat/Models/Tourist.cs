using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class Tourist : User
    {
        private bool suspicious;        // da mogu da ga oznacim kao sumljivog
        private bool blocked;
        private int numberOfCancellations;
        private List<Reservation> reservations;
        // made dictionary for all reservations 

        public Tourist()
        {
            this.Reservations = new List<Reservation>();
            this.Suspicious = false;
            this.Blocked = false;
            this.NumberOfCancellations = 0;
        }

        public Tourist(string username, string password, string name, string lastname, Gender gender, string email, DateTime dateOfBirth, Role role) :
            base(username, password, name, lastname, gender, email, dateOfBirth, role)
        {
            this.Reservations = new List<Reservation>();
            this.Suspicious = false;
            this.Blocked = false;
            this.NumberOfCancellations = 0;
        }

        public List<Reservation> Reservations { get => reservations; set => reservations = value; }
        public bool Suspicious { get => suspicious; set => suspicious = value; }
        public bool Blocked { get => blocked; set => blocked = value; }
        public int NumberOfCancellations { get => numberOfCancellations; set => numberOfCancellations = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5}/{6}/{7};{8};{9};{10};{11}", Username, Password, Name, Lastname, Gender, DateOfBirth.Day, DateOfBirth.Month, DateOfBirth.Year, Email, suspicious, blocked, NumberOfCancellations );
        }
    }
}