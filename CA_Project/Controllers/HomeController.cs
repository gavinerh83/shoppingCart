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
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db, SessionDictionary sessionDict)
        {
            _logger = logger;
            this.db = db;
            _sessionDict = sessionDict;
        }

        public IActionResult Index() 
        {
            if (CheckLoginStatus())
            {
                return LocalRedirect("/producthome/index");
            }
            List<Session> sessions = db.Sessions.ToList();
            _sessionDict.InputStoredSessions(sessions);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Create(string id, string password, string email, string fullname)
        {
            db.Users.Add(new Models.User
            {
                Username = id,
                Password = password,
                Email = email,
                FullName = fullname
            });
            db.SaveChanges();
            return LocalRedirect("/home/signupsuccess");
            //return RedirectToAction("Index","User");
        }
        public IActionResult CheckUser([FromBody] User user)
        {
            List<User> u = db.Users.Where(x => x.Id != null).ToList();
            if (u != null)
            {
                foreach (User id in u)
                {


                    if (user.Username == id.Username)
                    {
                        return Json(new { isOkay = false });
                    }
                }
            }

            return Json(new { isOkay = true });
        }
        public IActionResult Authenticate([FromBody] User user)
        {
            User u = db.Users.FirstOrDefault(x => x.Id == user.Id);
            if (u != null)
            {
                if (user.Id == u.Id && user.Password == u.Password)
                {

                    return Json(new { isOkay = true });


                }
                return Json(new { isOkay = false });
            }
            return Json(new { isOkay = false });
        }

        public IActionResult Register()
        {
            return View();
        }


        private bool CheckLoginStatus()
        {
            string sessionid = Request.Cookies["sessionid"];
            if(sessionid == null)
            {
                return false;
            }
            long? time =_sessionDict.CheckSessionPresence(sessionid);
            if(time == null)
            {
                return false;
            }
            else
            {
                //check if time is expired
                if ((time + Session.timeout) < DateTimeOffset.Now.ToUnixTimeSeconds())
                {
                    return false;
                }
            }
            return true;
        }


        public IActionResult Logout()
        {
            string s = Request.Cookies["sessionid"];
            if(s == null)
            {
                return LocalRedirect("/home/index");
            }
            Session sess = db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == s
            );
            Response.Cookies.Delete("sessionid");
            Response.Cookies.Delete("name");
            db.Sessions.Remove(sess);
            db.SaveChanges();

            _sessionDict.RemoveSession(s);
            return LocalRedirect("/home/index");
        }

        public IActionResult LoginFailed()
        {
            return View();
        }

        public IActionResult Login(string username, string password)
        {
            if (CheckLoginStatus())
            {
                return LocalRedirect("/producthome/index");
            }
            //validate username and password
            User user1 = db.Users.FirstOrDefault(x =>
                x.Username == username && x.Password == password
            );
            if(user1 == null)
            {
                return LocalRedirect("/home/LoginFailed");
            }
            else
            {
                //create a session
                Session userSession = new Session();
                userSession.User = user1;

                //add session to db
                db.Sessions.Add(userSession);
                db.SaveChanges();

                //add session to sessiondict
                _sessionDict.AddSession(userSession.Id.ToString(), userSession.TimeStamp);

                //set cookie
                Response.Cookies.Append("sessionid", userSession.Id.ToString());
                Response.Cookies.Append("name", user1.FullName);
                ViewData["sessionTime"] = userSession.TimeStamp;
            }
            return LocalRedirect("/ProductHome/Index");
        }

        public IActionResult CheckUserDuplicate([FromBody] User user)
        {
            User usr = db.Users.FirstOrDefault(x =>
                x.Username == user.Username
            );
            if(usr != null)
            {
                return Json(new
                {
                    unique = false
                });
            }
            return Json(new
            {
                unique = true
            });
        }

        public IActionResult CheckEmailDuplicate([FromBody] User user)
        {
            User user1 = db.Users.FirstOrDefault(x =>
                x.Email == user.Email
            );
            if(user1 != null)
            {
                return Json(new
                {
                    unique = false
                });
            }
            return Json(new
            {
                unique = true
            });
        }

        public IActionResult SignupSuccess()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
