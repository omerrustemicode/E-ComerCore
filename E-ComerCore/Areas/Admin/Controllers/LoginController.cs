using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using E_ComerCore.Models;
using E_ComerCore.Security;
using Microsoft.AspNetCore.Mvc;

namespace E_ComerCore.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/login")]
    public class LoginController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();
        public LoginController(DatabaseContext _db)
        {
            db = _db;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        [Route("process")]
        public IActionResult Process(string username, string password)
        {
            var account = processLogin(username, password);
            if(account != null)
            {
                securityManager.SignIn(this.HttpContext, account, "Admin_Schema");
                return RedirectToAction("index","Dashboard", new { area = "Admin" });
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("Index");
            }
          
        }

        private Account processLogin(string username, string password)
        {
            var account = db.Account.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);
            if(account != null)
            {
                var roleOfAcc = account.RoleAccount.FirstOrDefault();
                if (roleOfAcc.RoleId == 1 && BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }

        [Route("")]
        [Route("signout")]
        public IActionResult SignOut()
        {
            securityManager.SignOut(this.HttpContext, "Admin_Schema");
            return RedirectToAction("index","login", new { area = "admin" });
        }

        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var username = user.Value;
            var account = db.Account.SingleOrDefault(a => a.Username.Equals(username));
            return View("Profile",account);
        }

        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var currentAccount = db.Account.SingleOrDefault(a => a.Id == account.Id);
            if (!string.IsNullOrEmpty(account.Password))
            {
                currentAccount.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);

            }
            currentAccount.Username = account.Username;
            currentAccount.Email = account.Email;
            currentAccount.FullName = account.FullName;
            db.SaveChanges();
            ViewBag.msg = "Done";
            return View("Profile");
        }

        [Route("")]
        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}