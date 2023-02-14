using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public enum Gender { MALE, FEMALE, OTHER};
    public enum Role { ADMIN, MANAGER, TOURIST};

    public abstract class User
    {
        private string username;        // unique
        private string password;
        private string name;
        private string lastname;
        private Gender gender;
        private string email;
        private DateTime dateOfBirth;
        private Role role;

        public User()
        { }

        public User(string username, string password, string name, string lastname, Gender gender, string email, DateTime dateOfBirth, Role role)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Lastname = lastname;
            this.Gender = gender;
            this.Email = email;
            this.DateOfBirth = dateOfBirth;
            this.Role = role;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Name { get => name; set => name = value; }
        public string Lastname { get => lastname; set => lastname = value; }
        public Gender Gender { get => gender; set => gender = value; }
        public string Email { get => email; set => email = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public Role Role { get => role; set => role = value; }
    }
}