using System.Linq;
using System.Security.Claims;
using E_ComerCore.Models;
using E_ComerCore.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace E_ComerCore.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private SecurityManager securityManager = new SecurityManager();
        public AccountController(DatabaseContext _db)
        {
            db = _db;
        }
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            if (user == null)
            {
                var account = new Account();
                return View("Register", account);
            }
            else
            {
                return RedirectToAction("Dashboard", "Account");
            }
        
          
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(Account account)
        {
            var exist = db.Account.Count(a => a.Username.Equals(account.Username)) > 0;
            account.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            if (!exist)
            {
                account.Status = true;
                db.Account.Add(account);
                db.SaveChanges();
                //Shto Rolin ne accountin e ri
                var roleAccount = new RoleAccount()
                {
                    RoleId = 2,
                    AccountId = account.Id,
                    Status = true
                };
                db.RoleAccount.Add(roleAccount);
                db.SaveChanges();
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.error = "Username exists";
                return View("Register", account);
            }
        }


        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            if (user == null)
            {
                return View("login");
            }
            else
            {
                return RedirectToAction("Dashboard", "Account");
            }
        }

        [HttpPost]
        [Route("")]
        [Route("process")]
        public IActionResult Process(string username, string password)
        {
            var account = ProcessLogin(username, password);
            if (account != null)
            {
                securityManager.SignIn(this.HttpContext, account, "User_Schema");
                return RedirectToAction("Dashboard", "Account");
            }
            else
            {
                ViewBag.error = "Invalid Account";
                return View("login");
            }

        }

        private Account ProcessLogin(string username, string password)
        {
            var account = db.Account.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);
            if (account != null)
            {
                var roleOfAcc = account.RoleAccount.FirstOrDefault();
                if (roleOfAcc.RoleId == 2 && roleOfAcc.Status == true && BCrypt.Net.BCrypt.Verify(password, account.Password))
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
            securityManager.SignOut(this.HttpContext, "User_Schema");
            return RedirectToAction("login", "Account");
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpGet]
        [Route("profile")]
        public IActionResult Profile()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Account.SingleOrDefault(a => a.Username.Equals(user.Value));
            return View("Profile",customer);
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [HttpPost]
        [Route("profile")]
        public IActionResult Profile(Account account)
        {
            var currentCustomer = db.Account.Find(account.Id);
            if (!string.IsNullOrEmpty(account.Password))
            {
                currentCustomer.Password = BCrypt.Net.BCrypt.HashPassword(account.Password);
            }
            currentCustomer.FullName = account.FullName;
            currentCustomer.Email = account.Email;
            currentCustomer.Address = account.Address;
            currentCustomer.Phone = account.Phone;
            db.SaveChanges();
            return View("Profile", account);
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [Route("")]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View("dashboard");
        }

        [Route("")]
        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [Route("orderhistory")]
        public IActionResult Orderhistory()
        {
            var user = User.FindFirst(ClaimTypes.Name);
            var customer = db.Account.SingleOrDefault(a => a.Username.Equals(user.Value));
            ViewBag.invoices = customer.Invoices.OrderByDescending(i=>i.Id).ToList();
            return View("orderhistory");
        }

        [Authorize(Roles = "Customer", AuthenticationSchemes = "User_Schema")]
        [Route("details/{id}")]
        public IActionResult Details(int id)
        {

            ViewBag.invoiceDetails = db.InvoiceDetails.Where(i => i.InvoiceId == id).ToList();

            return View("Details");
        }
    }
}