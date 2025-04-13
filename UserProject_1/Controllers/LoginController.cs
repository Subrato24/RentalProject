using Microsoft.AspNetCore.Mvc;
using UserProject_1.Connection;
using UserProject_1.Models;

namespace UserProject_1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LoginController(ApplicationDbContext db) { 
            this._db = db;  
        }
        [HttpGet]
        public IActionResult Loggedin()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Loggedin(CustomerDetails logInDetails)
        {
            if (logInDetails.Name == "Admin" && logInDetails.Phone == "7869494484")
            {
                HttpContext.Session.SetString("Username", logInDetails.Name);
                HttpContext.Session.SetString("Phone", logInDetails.Phone);

                return RedirectToAction("Index", "Home");
            }
            else {
                var customer = _db.CustDetail.FirstOrDefault(c => (c.Name == logInDetails.Name) && (c.Phone == logInDetails.Phone));
                if (customer == null)
                {
                    return NotFound("Customer not found !!!");
                }
                if (logInDetails.Name == customer.Name && logInDetails.Phone == customer.Phone)
                {
                    HttpContext.Session.SetString("Username", logInDetails.Name);
                    HttpContext.Session.SetString("Phone",logInDetails.Phone);
                    return RedirectToAction("Index", "Home");
                }
                else {
                    ViewBag.ErrorMessage = "Invalid login. Please try again.";
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout() { 
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
