using CA_Project.Models;
using CA_Project.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CAShoppingCart.Controllers
{
    public class SearchController : Controller
    {
        private ApplicationDBContext shopContext;
        public SearchController(ApplicationDBContext shopContext)
        {
            this.shopContext = shopContext;
        }

        public IActionResult Index(string searchStr)
        {
            if (searchStr == null)
            {
                searchStr = "";
            }

            List<Product> products = shopContext.Products.Where(x =>
                x.ProductName.Contains(searchStr) || x.Desc.Contains(searchStr)
            ).ToList();


            ViewData["searchStr"] = searchStr;
            ViewData["products"] = products;

            return View();
        }
    }
}
