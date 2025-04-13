using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using UserProject_1.Connection;
using UserProject_1.Models;

namespace UserProject_1.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public PaymentController(IConfiguration configuration, ApplicationDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {
            try
            {
                string key = _configuration["Razorpay:Key"];
                string secret = _configuration["Razorpay:Secret"];

                RazorpayClient client = new RazorpayClient(key, secret);

                string phone = HttpContext.Session.GetString("Phone");
                CustomerStatus status = _context.CustStatus.FirstOrDefault(x => x.customerDetails.Phone == phone);
                double rent = (status.Rent)*100;   // Amount in paise (10000 paise = ₹100)

                if (rent == null)
                {
                    return NotFound(new { message = "Rent data not found." });
                }

                Dictionary<string, object> options = new Dictionary<string, object>
                {
                    { "amount", rent }, 
                    { "currency", "INR" },
                    { "receipt", "order_rcptid_11" },
                    { "payment_capture", 1 } // Auto capture payment
                };

                Order order = client.Order.Create(options);
                string orderId = order["id"].ToString();

                ViewData["RazorpayKey"] = key;
                return View("CreateOrder", orderId);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOrder(int amount)
        {
            try
            {
                string key = _configuration["Razorpay:Key"];
                string secret = _configuration["Razorpay:Secret"];

                RazorpayClient client = new RazorpayClient(key, secret);

                Dictionary<string, object> options = new Dictionary<string, object>
        {
            { "amount", amount * 100 }, // Convert ₹ to paise
            { "currency", "INR" },
            { "receipt", Guid.NewGuid().ToString() },
            { "payment_capture", 1 }
        };

                Order order = client.Order.Create(options);
                string orderId = order["id"].ToString();

                ViewData["RazorpayKey"] = key;
                return View("PaymentPage", orderId);
            }
            catch (Exception ex)
            {
                return Content("Error: " + ex.Message);
            }
        }


        [HttpGet]
        // ✅ Payment Success Callback Method
        public IActionResult Success(string paymentId)
        {
            TempData["success"] = "Payment Successful! Payment ID: " + paymentId;
            return RedirectToAction("Index", "CustomerDetail");
        }
    }
}
