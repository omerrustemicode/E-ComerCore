using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using E_ComerCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;

namespace E_ComerCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/photo")]
    public class PhotoController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private IHostingEnvironment ihostingEnvironment;

        public PhotoController(DatabaseContext _db, IHostingEnvironment _ihostingEnvironment)
        {
            db = _db;
            ihostingEnvironment = _ihostingEnvironment;
        }

        [Route("index/{id}")]
        public IActionResult Index(int id)
        {
            ViewBag.Product = db.Products.Find(id);
            ViewBag.Photos = db.Photos.Where(p => p.ProductId == id).ToList();
            return View();
        }

        [HttpGet]
        [Route("add/{id}")]
        public IActionResult Add(int id)
        {
            ViewBag.Product = db.Products.Find(id);
            var photo = new Photo
            {
                ProductId = id
            };
            return View("Add", photo);
        }
        [HttpPost]
        [Route("add/{productId}")]
        public IActionResult Add(int productId,Photo photo, IFormFile fileUpload)
        {
            var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + fileUpload.FileName;
            var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
            var stream = new FileStream(path, FileMode.Create);
            fileUpload.CopyToAsync(stream);
            photo.Name = fileName;
            db.Photos.Add(photo);

            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }
        [HttpGet]
        [Route("delete/{id}/productId")]
        public IActionResult Delete(int id, int productId)
        {
            ViewBag.Product = db.Products.Find(id);
            var photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }
        [HttpGet]
        [Route("edit/{id}/{productId}")]
        public IActionResult Edit(int id, int productId)
        {
            ViewBag.Product = db.Products.Find(id);
            var photo = db.Photos.Find(id);

            return View("Edit", photo);
        }
        [HttpPost]
        [Route("edit/{photoIdd}/{productId}")]
        public IActionResult Edit(int photoIdd, int productId,Photo photo, IFormFile fileUpload)
        {
            var currentPhoto = db.Photos.Find(photo.Id);
            if (fileUpload != null && !string.IsNullOrEmpty(fileUpload.FileName))
            {
                var fileName = DateTime.Now.ToString("MMddyyyyhhmmss") + fileUpload.FileName;
                var path = Path.Combine(this.ihostingEnvironment.WebRootPath, "products", fileName);
                var stream = new FileStream(path, FileMode.Create);
                fileUpload.CopyToAsync(stream);
                currentPhoto.Name = fileName;
            }
            currentPhoto.Status = photo.Status;
            currentPhoto.Featured = photo.Featured;
            db.SaveChanges();
            return RedirectToAction("index", "photo", new { area = "admin", id = productId });
        }
    }
}