using E_ComerCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_ComerCore.ViewComponents
{

    [ViewComponent(Name = "SlideShow")]
    public class SlideShowViewComponent : ViewComponent
    {
        private DatabaseContext db;
        //Ligjerata 11
        public SlideShowViewComponent(DatabaseContext _db)
        {
            this.db = _db;
        }
        public IViewComponentResult Invoke()
        {
            List<SlideShow> slideShows = db.SlideShows.Where(c => c.Status).ToList();
            return View("Index", slideShows);
        }
    }
}
