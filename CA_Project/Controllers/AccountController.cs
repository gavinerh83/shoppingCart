using CA_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using CA_Project.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CA_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDBContext db;
        public AccountController(ApplicationDBContext db)
        { 
            this.db = db;
        }
        public IActionResult MyAccount()
        {
            if (Request.Cookies["name"] != null)
            {
                string s = Request.Cookies["name"];
                User user = db.Users.FirstOrDefault(x =>
                    x.FullName == s
                );
                ViewData["account"] = user;
                return View();
            }
            return RedirectToAction("Index", "Home");
            
        }
        public IActionResult ChangePW()
        {
            if (Request.Cookies["name"] != null) { return View(); }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Validate(string cpassword, string newpassword)
        {
            string s = Request.Cookies["name"];
            User user = db.Users.FirstOrDefault(x =>
                x.FullName == s
            );
            if (user.Password == cpassword)
            {
                user.Password = newpassword;
                db.SaveChanges();
                return View();
            }

            return RedirectToAction("Invalid", "Account");
        }
        public IActionResult Invalid()
        {
            return View();
        }

    }
}
