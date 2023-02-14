using PR043_2018_Web_projekat.Models;
using System;
using PR043_2018_Web_projekat.CommonFunctions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace PR043_2018_Web_projekat.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult HomeAdmin()
        {
            displayUsers = new List<User>(Data.users.Values.ToList());
            ViewBag.users = displayUsers;

            return View();
        }

        public ActionResult AddManager()
        {
            return View();
        }
        private static List<User> displayUsers = new List<User>();

        [HttpPost]
        public ActionResult AddManager(string username, string password, string name, string lastname, string gender, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastname) ||
                string.IsNullOrEmpty(email) || dateOfBirth == DateTime.MinValue)
            {
                ViewBag.AddAdminError = "Please fill in all fileds!";
                return View();
            }

            if (Data.users.ContainsKey(username))
            {
                ViewBag.AddAdminError = "Username taken, please choose another one!";
                return View();
            }
            else
            {
                Manager newManager = new Manager(username, password, name, lastname, (Gender)Enum.Parse(typeof(Gender), gender), email, dateOfBirth, Role.MANAGER);
                Data.users.Add(username, newManager);
                ReadWriteFunctions.WriteManager(newManager);
            }

            return RedirectToAction("HomeAdmin");
        }

        public ActionResult SortUsers(string type, string way)
        {
            switch (type)
            {
                case "name":
                    if (way.Equals("asc"))
                        displayUsers = displayUsers.OrderBy(x => x.Name).ToList();
                    else
                        displayUsers = displayUsers.OrderByDescending(x => x.Name).ToList();
                        break;
                case "lastname":
                    if (way.Equals("asc"))
                        displayUsers = displayUsers.OrderBy(x => x.Lastname).ToList();
                    else
                        displayUsers = displayUsers.OrderByDescending(x => x.Lastname).ToList();
                    break;
                case "role":
                    if (way.Equals("asc"))
                        displayUsers = displayUsers.OrderBy(x => x.Role).ToList();
                    else
                        displayUsers = displayUsers.OrderByDescending(x => x.Role).ToList();
                    break;
            }

            ViewBag.users = displayUsers;

            return View("HomeAdmin");
        }

        public ActionResult SearchUsers(string name, string lastname, string role, string search)
        {
            if (search.Equals("Show all"))
                return RedirectToAction("HomeAdmin");

            displayUsers = new List<User>(Data.users.Values.ToList());
            List<Tuple<string, string>> keys = new List<Tuple<string, string>>();
            if (!string.IsNullOrEmpty(name))
                keys.Add(new Tuple<string, string>("name", name));
            if (!string.IsNullOrEmpty(lastname))
                keys.Add(new Tuple<string, string>("lastname", lastname));
            if (!string.IsNullOrEmpty(role))
                keys.Add(new Tuple<string, string>("role", role));
            foreach (Tuple<string, string> key in keys)
            {
                switch (key.Item1)
                {
                    case "name":
                        foreach (User user in displayUsers.ToList())
                        {
                            if (!user.Name.ToLower().Contains(key.Item2.ToLower()))
                                displayUsers.Remove(user);
                        }
                        break;
                    case "lastname":
                        foreach (User user in displayUsers.ToList())
                        {
                            if (!user.Lastname.ToLower().Contains(key.Item2.ToLower()))
                                displayUsers.Remove(user);
                        }
                        break;
                    case "role":
                        foreach (User user in displayUsers.ToList())
                        {
                            if (user.Role.ToString() != key.Item2)
                                displayUsers.Remove(user);
                        }
                        break;
                }
            }
            ViewBag.users = displayUsers;

            return View("HomeAdmin");
        }

        public ActionResult BlockUser(string block, string unblock)
        {
            if (!string.IsNullOrEmpty(block))
                (Data.users[block] as Tourist).Blocked = true;
            else
                (Data.users[unblock] as Tourist).Blocked = false;
            ReadWriteFunctions.OverwriteTouristsTxt();

            return RedirectToAction("HomeAdmin");
        }

    }
}