using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_ComerCore.Models;
using E_ComerCore.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_ComerCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public CategoryController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.categories = db.Category.Where(c => c.Parent == null).ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var category = new Category();
            return View("Add", category);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add(Category category)
        {
            category.Parent = null;
            db.Category.Add(category);
            db.SaveChanges();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var category = db.Category.Find(id);
            db.Category.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var category = db.Category.Find(id);
            return View("Edit", category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, Category category)
        {
            var currentCategory = db.Category.Find(id);
            currentCategory.Name = category.Name;
            currentCategory.Status = category.Status;
            db.SaveChanges();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }

        [HttpGet]
        [Route("addsubcategory/{id}")]
        public IActionResult AddSubcategory(int id)
        {
            var subcategory = new Category()
            {
                ParentId = id

            };
            return View("AddSubcategory", subcategory);
        }

        [HttpPost]
        [Route("addsubcategory/{categoryId}")]
        public IActionResult AddSubcategory(int categoryId, Category subcategory)
        {
            db.Category.Add(subcategory);
            db.SaveChanges();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }
    }
}