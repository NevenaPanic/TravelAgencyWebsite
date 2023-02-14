using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PR043_2018_Web_projekat.Models
{
    public class Data
    {
        public static Dictionary<string, User> users = new Dictionary<string, User>();
        public static List<Arrangement> arrangements = new List<Arrangement>();     // arrangment id all arrangements 
        public static List<Accommodation> accommodations = new List<Accommodation>();
        public static Dictionary<string, Reservation> reservationsPerUser = new Dictionary<string, Reservation>();     // string username, all reservations for specific user
        public static  List<Comment> comments = new List<Comment>();             // all managers will have list of comments for their arrangements
    }
}