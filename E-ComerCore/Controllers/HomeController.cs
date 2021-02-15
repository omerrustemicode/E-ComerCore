using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_ComerCore.Models;

namespace E_ComerCore.Controllers
{
    [Route("home")]
    public class HomeController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public HomeController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        [Route("~/")]
        public IActionResult Index()
        {
            ViewBag.isHome = true;
            var FeaturedProducts = db.Products.OrderByDescending(p => p.Id).Where(p => p.Status && p.Featured).ToList();
            ViewBag.FeaturedProducts = FeaturedProducts;
            ViewBag.CountFeaturedProducts = FeaturedProducts.Count;
            ViewBag.LatestProducts = db.Products.OrderByDescending(p => p.Id).Where(p =>
              p.Status).Take(6).ToList();
            return View();
        }

        public IActionResult Privacy()
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
