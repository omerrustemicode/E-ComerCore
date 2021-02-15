using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.ViewComponents
{

    [ViewComponent(Name = "Category")]
    public class CategoryViewComponent : ViewComponent
    {
        private DatabaseContext db;
        //Ligjerata 11
        public CategoryViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public IViewComponentResult Invoke()
        {
            List<Category> categories = db.Category.Where(c => c.Status && c.Parent == null).ToList();
            return View("Index", categories);
        }
    }
}
