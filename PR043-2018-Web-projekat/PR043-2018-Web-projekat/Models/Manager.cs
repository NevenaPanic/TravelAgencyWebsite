using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class Manager : User
    {
        private List<Arrangement> createdArrangements;


        public Manager(string username, string password, string name, string lastname, Gender gender, string email, DateTime dateOfBirth, Role role) :
            base(username, password, name, lastname, gender, email, dateOfBirth, role)
        {
            this.CreatedArrangements = new List<Arrangement>();
        }

        public Manager()
        {
            this.CreatedArrangements = new List<Arrangement>();
        }

        public List<Arrangement> CreatedArrangements { get => createdArrangements; set => createdArrangements = value; }

        public override string ToString()
        {
            return String.Format("{0};{1};{2};{3};{4};{5}/{6}/{7};{8}", Username, Password, Name, Lastname, Gender, DateOfBirth.Day, DateOfBirth.Month, DateOfBirth.Year, Email);
        }
    }
}