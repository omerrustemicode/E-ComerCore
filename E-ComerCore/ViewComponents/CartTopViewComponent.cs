using E_ComerCore.Helpers;
using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.ViewComponents
{

    [ViewComponent(Name = "CartTop")]
    public class CartTopViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.countItems = 0;
                ViewBag.Total = 0;

            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                ViewBag.countItems = cart.Count;
                ViewBag.Total = cart.Sum(it => it.Price * it.Quantity);
            }
            return View("Index");
        }
    }
}
