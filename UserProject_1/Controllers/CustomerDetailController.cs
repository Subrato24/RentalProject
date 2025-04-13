using Microsoft.AspNetCore.Mvc;
using UserProject_1.Connection;
using UserProject_1.Models;

namespace UserProject_1.Controllers
{
    public class CustomerDetailController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerDetailController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this._db = db;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            string username = HttpContext.Session.GetString("Username");
            string phone = HttpContext.Session.GetString("Phone");

            List<CustomerDetails> customerDetails;

            if (username == "Admin")
            {
                customerDetails = _db.CustDetail.ToList();
            }
            else
            {
                customerDetails = _db.CustDetail.Where(c => (c.Phone == phone) && c.Name == username).ToList();
            }
          
            return View(customerDetails);
        }

        [HttpGet]   
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCustomer(CustomerDetails details, IFormFile? ProfileImage, IFormFile? IdProof)
        {
            try
            {
                if (ProfileImage != null && IdProof != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    string IdProofFilePath = Guid.NewGuid().ToString() + Path.GetExtension(IdProof.FileName);
                    string IdProofUploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "Idproof");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    if (!Directory.Exists(IdProofUploadPath))
                    {
                        Directory.CreateDirectory(IdProofUploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);
                    string IdproofPath = Path.Combine(IdProofUploadPath, IdProofFilePath);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(fileStream);
                    }

                    using (var IdFileStream = new FileStream(IdproofPath, FileMode.Create))
                    {
                        IdProof.CopyTo(IdFileStream);
                    }

                    details.ProfileImageUrl = "/uploads/" + fileName;
                    details.IdProof = "/Idproof/" + IdProofFilePath;
                }
                else
                {
                    details.ProfileImageUrl = "/lib/images.png"; // Default image
                }

                _db.CustDetail.Add(details);
                _db.SaveChanges();
                TempData["success"] = "Customer Added successfully";
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                TempData["error"] = "An error occurred while uploading files: " + e.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult EditCustomer(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            CustomerDetails detail = _db.CustDetail.First(x => x.CustomerDetailId == id);
            return View(detail);
        }

        [HttpPost]
        public IActionResult EditCustomer(CustomerDetails newDetail, IFormFile? ProfileImage)
        {
            CustomerDetails oldDetail = _db.CustDetail.FirstOrDefault(x => x.CustomerDetailId == newDetail.CustomerDetailId);

            if (oldDetail != null)
            {
                if (ProfileImage != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                    string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        ProfileImage.CopyTo(fileStream);
                    }

                    oldDetail.ProfileImageUrl = "/uploads/" + fileName;
                }

                oldDetail.Name = newDetail.Name;
                oldDetail.Email = newDetail.Email;
                oldDetail.Phone = newDetail.Phone;
                oldDetail.Address = newDetail.Address;
                oldDetail.Deposit = newDetail.Deposit;
                oldDetail.JoiningDate = newDetail.JoiningDate;
                _db.SaveChanges();
                TempData["success"] = "Customer Updated successfully";
                return RedirectToAction("Index");
            }

            TempData["error"] = "Customer Not Updated !!!";
            return View(newDetail);
        }

        [HttpGet]
        public IActionResult DeleteCustomer(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            CustomerDetails detail = _db.CustDetail.First(x => x.CustomerDetailId == id);
            return View(detail);
        }

        [HttpPost, ActionName("DeleteCustomer")]
        public IActionResult Delete(int? id)
        {
            if (id != null && id != 0)
            {
                CustomerDetails? detail = _db.CustDetail.Find(id);
                if (detail == null)
                {
                    return NotFound(detail);
                }

                // Delete Image from Server
                if (!string.IsNullOrEmpty(detail.ProfileImageUrl) && detail.ProfileImageUrl != "/lib/default-profile.png")
                {
                    string filePath = Path.Combine(_webHostEnvironment.WebRootPath, detail.ProfileImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _db.CustDetail.Remove(detail);
                _db.SaveChanges();
                TempData["warning"] = "Customer Deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "Customer Not Deleted !!!";
            return RedirectToAction("Home");
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var customer = _db.CustDetail.FirstOrDefault(c => c.CustomerDetailId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
    }
}
