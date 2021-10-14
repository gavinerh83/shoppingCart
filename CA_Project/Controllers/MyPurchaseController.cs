using Microsoft.AspNetCore.Mvc;
using CA_Project.Data;
using System;
using CA_Project.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace CA_Project.Controllers
{
    public class MyPurchaseController : Controller
    {
        private readonly ApplicationDBContext db;
        private readonly SessionDictionary _sessionDict;
        public MyPurchaseController(ApplicationDBContext db, SessionDictionary sessionDict)
        {
            this.db = db;
            _sessionDict = sessionDict;
        }
        public IActionResult Record()
        {
            if (!CheckLoginStatus())
            {
                return LocalRedirect("/home/index");
            }
            string sessionid=GetSessionID();// Find session

            Session session = db.Sessions.FirstOrDefault(x =>
               x.Id.ToString() == sessionid //find session
           );

            List <Purchase> purchase = db.Purchases.Where(x =>
            x.User.Id ==session.User.Id).ToList(); //List of purchase record with matched user id

            List <PurchaseProduct> Detail = db.PurchaseProducts.Where(x => x.Purchase.User.Id ==session.User.Id).ToList();
           

            /*List<PurchaseProduct> Detail=new List <PurchaseProduct>();
                for (int i = 0; i < purchase.Count; i++) // find purchase detail with purchase id
            {

                Detail.Add((PurchaseProduct)purchase[i].PurchaseProducts);
            }*/

            ViewData["Purchase"] = purchase;
            ViewData["Detail"] = Detail;
            return View();
            //List<Guid> DtPurchases = purchase.Select(x => x.PurchaseProducts.Id).Distinct().ToList();
            //ViewData["PurchasesID"] = DtPurchases;//

        }
        private string GetSessionID()
        {
            return Request.Cookies["sessionid"];
        }

        private bool CheckLoginStatus()
        {
            string sessionid = GetSessionID();
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
    } }
