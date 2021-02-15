using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using E_ComerCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_ComerCore.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "Admin_Schema")]
    [Area("admin")]
    [Route("admin/dashboard")]
    public class DashboardController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        public DashboardController(DatabaseContext _db)
        {
            db = _db;
        }
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            ViewBag.countInvoices = db.Invoices.Count(i=>i.Status == 1);
            ViewBag.countProducts = db.Products.Count();
            ViewBag.countCustomer = db.RoleAccount.Count(ra => ra.RoleId == 2);
            ViewBag.countCategories = db.Category.Count(c => c.ParentId == null);
            return View();
        }
    }
}