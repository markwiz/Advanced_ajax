using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using AdvancedAjax.Models;
using AdvancedAjax.Data;

namespace AdvancedAjax.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _webHost;

        public CustomerController(AppDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.City).ToList();
            return View(customers);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var customer = new Customer();
            ViewBag.Countries = GetCountries();
            ViewBag.Cities = new List<SelectListItem>();
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = GetProfilePhotoFileName(customer);
                if (!string.IsNullOrEmpty(uniqueFileName))
                {
                    customer.PhotoUrl = uniqueFileName;
                }
                _context.Add(customer);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Countries = GetCountries();
            ViewBag.Cities = GetCities(customer.CountryId);
            return View(customer);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var customer = _context.Customers
                .Include(c => c.City)
                .ThenInclude(city => city.Country)
                .FirstOrDefault(c => c.Id == Id);
            return View(customer);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Customer customer = _context.Customers
                .Include(co => co.City)
                .Where(c => c.Id == Id)
                .FirstOrDefault();

            if (customer == null)
                return NotFound();

            customer.CountryId = customer.City.CountryId;
            ViewBag.Countries = GetCountries();
            ViewBag.Cities = GetCities(customer.CountryId);
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers.AsNoTracking().FirstOrDefault(c => c.Id == customer.Id);
                if (existingCustomer != null)
                {
                    // Only update the photo if a new one is uploaded
                    if (customer.ProfilePhoto != null)
                    {
                        string uniqueFileName = GetProfilePhotoFileName(customer);
                        customer.PhotoUrl = uniqueFileName;
                    }
                    else
                    {
                        // Keep the existing photo URL
                        customer.PhotoUrl = existingCustomer.PhotoUrl;
                    }

                    _context.Attach(customer);
                    _context.Entry(customer).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewBag.Countries = GetCountries();
            ViewBag.Cities = GetCities(customer.CountryId);
            return View(customer);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var customer = _context.Customers.Include(c => c.City).FirstOrDefault(c => c.Id == Id);
            return View(customer);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Customer customer)
        {
            _context.Attach(customer);
            _context.Entry(customer).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private List<SelectListItem> GetCountries()
        {
            var countries = _context.Countries.ToList();
            var lstCountries = countries.Select(ct => new SelectListItem()
            {
                Value = ct.Id.ToString(),
                Text = ct.Name
            }).ToList();
            lstCountries.Insert(0, new SelectListItem() { Value = "", Text = "----Select Country----" });
            return lstCountries;
        }

        private List<SelectListItem> GetCities(int countryId)
        {
            List<SelectListItem> cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .OrderBy(n => n.Name)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();

            return cities;
        }


             [HttpGet]
        public JsonResult GetCitiesByCountry(int countryId)
        {
            
                List<SelectListItem> cities = _context.Cities
                  .Where(c => c.CountryId == countryId)
                  .OrderBy(n => n.Name)
                  .Select(n =>
                  new SelectListItem
                  {
                      Value = n.Id.ToString(),
                      Text = n.Name
                  }).ToList();

                return Json(cities);
            
        }

          private string GetProfilePhotoFileName(Customer customer)
        {
            string uniqueFileName = null;

            if (customer.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + customer.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    customer.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
} 