using System.Linq;
using E_ComerCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_ComerCore.Areas.Admin.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace E_ComerCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public ProductController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.Products = db.Products.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var productViewModel = new ProductView();
            productViewModel.Product = new Product();
            productViewModel.Category = new List<SelectListItem>();
            var categories = db.Category.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };
                if (category.InverseParent != null && category.InverseParent.Count > 0)
                {

                    foreach (var subCategory in category.InverseParent)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };
                        productViewModel.Category.Add(selectListItem);
                    }
                }
            }
            return View("Add", productViewModel);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Add(ProductView productView)
        {
            db.Products.Add(productView.Product);
            db.SaveChanges();
            //Default foto per produktin 
            var defaultPhoto = new Photo
            {
                Name = "noimage.gif",
                Status = true,
                ProductId = productView.Product.Id,
                Featured = true
            };
            db.Photos.Add(defaultPhoto);
            db.SaveChanges();
            return RedirectToAction("index", "product", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
              var product = db.Products.Find(id);
              db.Products.Remove(product);
              db.SaveChanges();
            }
            catch(Exception e)
            {
                TempData["error"] = e.Message;
            }
        
            return RedirectToAction("index", "product", new { area = "admin" });
        }

       
        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            
            var productViewModel = new ProductView();
            productViewModel.Product = db.Products.Find(id);
            productViewModel.Category = new List<SelectListItem>();
            var categories = db.Category.ToList();
            foreach (var category in categories)
            {
                var group = new SelectListGroup { Name = category.Name };
                if (category.InverseParent != null && category.InverseParent.Count > 0)
                {

                    foreach (var subCategory in category.InverseParent)
                    {
                        var selectListItem = new SelectListItem
                        {
                            Text = subCategory.Name,
                            Value = subCategory.Id.ToString(),
                            Group = group
                        };
                        productViewModel.Category.Add(selectListItem);
                    }
                }
            }
            return View("Edit", productViewModel);
        }
        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, ProductView productView)
        {
            db.Entry(productView.Product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index", "product", new { area = "admin" });
        }
    }
}