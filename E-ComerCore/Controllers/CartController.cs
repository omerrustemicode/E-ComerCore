using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_ComerCore.Models;
using E_ComerCore.Helpers;
using System.Security.Claims;

namespace E_ComerCore.Controllers
{
    [Route("cart")]
    public class CartController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CartController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {

            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            ViewBag.Cart = cart;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                ViewBag.countItems = 0;
                ViewBag.Total = 0;

            }
            else
            {

                ViewBag.countItems = cart.Count;
                ViewBag.Total = cart.Sum(it => it.Price * it.Quantity);
            }
    
            return View();
        }

        [HttpGet]
        [Route("buy/{id}")]
        public IActionResult Buy(int id)
        {
         
            var product = db.Products.Find(id);
            var photo = product.Photos.SingleOrDefault(ph => ph.Status && ph.Featured);
            var photoName = photo == null ? "noimage.gif" : photo.Name;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            { 
                 var cart = new List<Item>();
                cart.Add(new Item {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Photo = photoName,
                Quantity = 1
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = exist(id, cart);
                if(index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Photo = photoName,
                        Quantity = 1
                    });
                }
                else
                {
                    cart[index].Quantity++;
                 
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("index", "Cart");
        }

        [HttpPost]
        [Route("buy/{id}")]
        public IActionResult Buy(int id,int quantity)
        {
            var product = db.Products.Find(id);
            var photo = product.Photos.SingleOrDefault(ph => ph.Status && ph.Featured);
            var photoName = photo == null ? "noimage.gif" : photo.Name;
            if (SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart") == null)
            {
                var cart = new List<Item>();
                cart.Add(new Item
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Photo = photoName,
                    Quantity = quantity
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                int index = exist(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                        Photo = photoName,
                        Quantity = quantity
                    });
                }
                else
                {
                    cart[index].Quantity += quantity;

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("index", "Cart");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
            int index = exist(id, cart);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("index", "Cart");
        }

        [Route("checkout")]
        public IActionResult Checkout()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            if(user == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var customer = db.Account.SingleOrDefault(a => a.Username.Equals(user.Value));
                //invoice  ktu..
                var invoice = new Invoice()
                {
                    Name = "Invoice Online",
                    Created = DateTime.Now,
                    Status = 1,
                    AccountId = customer.Id
                };
                db.Invoices.Add(invoice);
                db.SaveChanges();
                //invoice details ktu..
                List<Item> cart = SessionHelper.GetObjectFromJson<List<Item>>(HttpContext.Session, "cart");
                foreach(var item in cart)
                {
                    var invoiceDetails = new InvoiceDetails
                    {
                        InvoiceId = invoice.Id,
                        ProductId = item.Id,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    db.InvoiceDetails.Add(invoiceDetails);
                    db.SaveChanges();
                }
                //Heq Sessionin pas blerjes ...
                HttpContext.Session.Remove("cart");

            }
            return RedirectToAction("Thanks", "Cart");
        }

        [Route("thanks")]
        public IActionResult Thanks()
        {
            return View("Thanks");
        }

        private int exist(int id,List<Item> cart)
        {
            for(var i =0; i < cart.Count; i++)
            {
                if (cart[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
