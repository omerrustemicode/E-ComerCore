using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.ViewComponents
{

    [ViewComponent(Name = "Search")]
    public class SearchViewComponent : ViewComponent
    {
        private DatabaseContext db;
    
        public SearchViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public IViewComponentResult Invoke()
        {
            List<Category> categories = db.Category.Where(c => c.Status).ToList();
            return View("Index", categories);
        }
    }
}
