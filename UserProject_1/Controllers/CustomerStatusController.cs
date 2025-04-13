using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserProject_1.Connection;
using UserProject_1.Models;

namespace UserProject_1.Controllers
{
    public class CustomerStatusController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CustomerStatusController(ApplicationDbContext db)
        {
            this._db = db;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            List<CustomerStatus> customerStatuses = _db.CustStatus
            .Where(x => x.CustomerDetailId == id)
            .ToList();
            ViewData["CustomerId"] = id;
            return View(customerStatuses);
        }

        [HttpGet]
        public IActionResult AddStatus(int customerID)
        {
            if (customerID == 0)
            {
                return NotFound("Customer ID is required.");
            }

            var status = new CustomerStatus
            {
                CustomerDetailId = customerID
            };

            return View(status);
        }

        [HttpPost]
        public IActionResult AddStatus(CustomerStatus details)
        {
            if (details.CustomerDetailId == 0)
            {
                TempData["error"] = "Invalid Customer ID.";
                return View(details);
            }

            // Manually load CustomerDetails from DB
            details.customerDetails = _db.CustDetail
                .FirstOrDefault(c => c.CustomerDetailId == details.CustomerDetailId);

            var customer = _db.CustDetail
            .Include(c => c.CustomerStatuses) // <-- This will load the related statuses
            .FirstOrDefault(c => c.CustomerDetailId == details.CustomerDetailId);

            _db.CustStatus.Add(details);
            _db.SaveChanges();
            TempData["success"] = "Data Added successfully";
            return RedirectToAction("Index");

            return View(details);
        }

        [HttpGet]
        public IActionResult EditStatus(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            CustomerStatus status = _db.CustStatus.First(x => x.Id == id);
            return View(status);
        }
        [HttpPost]
        public IActionResult EditCustomer(CustomerStatus newStatus)
        {
            if (newStatus == null)
            {
                TempData["error"] = "Invalid data submitted!";
                return View(newStatus);
            }

            CustomerStatus oldStatus = _db.CustStatus.FirstOrDefault(x => x.Id == newStatus.Id);

            // Update the properties
            oldStatus.Rent = newStatus.Rent;
            oldStatus.Ebill = newStatus.Ebill;
            oldStatus.Date = newStatus.Date;
            oldStatus.Due = newStatus.Due;
            oldStatus.Status = newStatus.Status;
            _db.SaveChanges();
            TempData["success"] = "Status Updated successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteStatus(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            CustomerStatus detail = _db.CustStatus.First(x => x.Id == id);
            return View(detail);
        }

        [HttpPost, ActionName("DeleteStatus")]
        public IActionResult Delete(int? id)
        {
            if (id != null && id != 0)
            {
                CustomerStatus? status = _db.CustStatus.Find(id);
                if (status == null)
                {
                    return NotFound(status);
                }
                _db.CustStatus.Remove(status);
                _db.SaveChanges();
                TempData["warning"] = "Recors Deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Record Not Deleted !!!";
            return RedirectToAction("Home");
        }
    }
}
