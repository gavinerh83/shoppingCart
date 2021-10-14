﻿using Microsoft.AspNetCore.Mvc;
using System;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CA_Project.Data;

namespace CA_Project.Controllers
{
    public class CartController : Controller
    {
        private readonly SessionDictionary _sessionDict;
        private readonly ApplicationDBContext _db;
        public CartController(ApplicationDBContext db, SessionDictionary sessiondict)
        {
            _sessionDict = sessiondict;
            _db = db;
        }
        public IActionResult Index()
        {
            if (!CheckLoginStatus())
            {
                return RedirectToAction("index", "home");
            }
            //get prodname, prod descrip and prod img, product price and quantity from cart_products table

            //find userid
            string sessionid = GetSessionID();
            Session sess1 = _db.Sessions.FirstOrDefault(x =>
                x.Id.ToString() == sessionid
            );

            //use userid to query for the cartid
            Cart cart1 = _db.Carts.FirstOrDefault(x =>
                x.User.Id == sess1.User.Id
            );

            List<CartProduct> cartProducts = (List<CartProduct>) cart1.CartProducts;
            ViewData["cartProducts"] = cartProducts;
            return View();
        }

        public IActionResult UpdateQuantity([FromBody] CartProduct cp)
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }
            CartProduct cartProduct = _db.CartProducts.FirstOrDefault(x =>
                x.Id == cp.Id
            );
            if(cartProduct == null)
            {
                return Json(null);
            }
            cartProduct.Quantity = cp.Quantity;
            _db.SaveChanges();
            return Json(new
            {
                success = true
            });

        }

        public IActionResult RemoveProduct([FromBody] CartProduct cp)
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("home/index");
            }
            CartProduct cartProduct = _db.CartProducts.FirstOrDefault(x =>
                x.Id == cp.Id
            );
            if(cartProduct == null)
            {
                return Json(null);
            }
            _db.CartProducts.Remove(cartProduct);
            _db.SaveChanges();
            return Json(new
            {
                delete = true
            });
        }


        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
        }

        private bool CheckLoginStatus()
        {
            string sessionid = Request.Cookies["sessionid"];
            if (sessionid == null)
            {
                return false;
            }
            long? time = _sessionDict.CheckSessionPresence(sessionid);
            if (time == null)
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
    }
}
