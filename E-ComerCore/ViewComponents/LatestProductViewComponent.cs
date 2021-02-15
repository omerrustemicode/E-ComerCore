using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.ViewComponents
{

    [ViewComponent(Name = "LatestProducts")]
    public class LatestProductViewComponent : ViewComponent
    {
        private DatabaseContext db;
        //Ligjerata 11
        public LatestProductViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public IViewComponentResult Invoke()
        {
            List<Product> products = db.Products.OrderByDescending(p => p.Id).Where(p =>
              p.Status).Take(6).ToList();
            return View("Index", products);
        }
    }
}
