using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using E_ComerCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_ComerCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/slideshow")]
    public class SlideShowController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        private IHostingEnvironment ihostingEnvironment;

        public SlideShowController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }


        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.SlideShows = db.SlideShows.ToList();
            return View();
        }

        [HttpGet]
        [Route("add")]
        public IActionResult Add()
        {
            var slideshow = new SlideShow();
            return View("Add", slideshow);
        }
        [HttpPost]
        [Route("add")]
        public IActionResult Add(SlideShow slideShow, IFormFile photo)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss")+ photo.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slidesshows", fileName);
            var stream = new FileStream(path, FileMode.Create);
            photo.CopyToAsync(stream);
            slideShow.Name = fileName;
            db.SlideShows.Add(slideShow);
            
            db.SaveChanges();
            return RedirectToAction("index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var slideShow = db.SlideShows.Find(id);
            db.SlideShows.Remove(slideShow);
            db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

        [HttpGet]
        [Route("edit/{id}")]
        public IActionResult Edit(int id)
        {
            var slideShow = db.SlideShows.Find(id);
            return View("Edit", slideShow);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public IActionResult Edit(int id, SlideShow slideShow,IFormFile photo)
        {
            var currentSlide = db.SlideShows.Find(slideShow.Id);
            if(photo != null && !string.IsNullOrEmpty(photo.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + photo.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "slidesshows", fileName);
                var stream = new FileStream(path, FileMode.Create);
                photo.CopyToAsync(stream);
                currentSlide.Name = fileName;
            }
            currentSlide.Status = slideShow.Status;
            currentSlide.Title = slideShow.Title;
            currentSlide.Description = slideShow.Description;
            db.SaveChanges();
            return RedirectToAction("Index", "slideshow", new { area = "admin" });
        }

    }
}