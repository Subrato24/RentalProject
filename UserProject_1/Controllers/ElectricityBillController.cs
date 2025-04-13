using Microsoft.AspNetCore.Mvc;

namespace UserProject_1.Controllers
{
    public class ElectricityBillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateBill(int currentReading, int previousReading, decimal chargePerUnit)
        {
            // Calculate the total units consumed
            int unitsConsumed = currentReading - previousReading;

            // Calculate the electricity bill
            decimal billAmount = unitsConsumed * chargePerUnit;

            // Pass the bill amount to the view
            ViewBag.BillAmount = billAmount;

            return View("Index"); // Return to the same view
        }
    }
}
