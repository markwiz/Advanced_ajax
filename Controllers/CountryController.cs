using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AdvancedAjax.Models;
using AdvancedAjax.Data;

namespace AdvancedAjax.Controllers
{
    public class CountryController : Controller
    {

        private readonly AppDbContext _context;

        public CountryController(AppDbContext context)
        {
            _context = context;           
        }

        public IActionResult Index()
        {
            List<Country> countries;
            countries = _context.Countries.ToList();
            return View(countries);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Country country = new Country { Code = string.Empty, Name = string.Empty };            
            return View(country);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                _context.SaveChanges();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, country = new { id = country.Id, name = country.Name } });
                }
                return RedirectToAction(nameof(Index));
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreateModalForm", country);
            }
            return View(country);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Country? country = GetCountry(Id);
            if (country == null)
                return NotFound();
            return View(country);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            Country? country = GetCountry(Id);
            if (country == null)
                return NotFound();
            return View(country);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(Country country)
        {
            _context.Attach(country);
            _context.Entry(country).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private Country? GetCountry(int id)
        {
            return _context.Countries.FirstOrDefault(c => c.Id == id);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Country? country = GetCountry(Id);
            if (country == null)
                return NotFound();
            return View(country);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(Country country)
        {
            try
            {
                _context.Attach(country);
                _context.Entry(country).State = EntityState.Deleted;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {     
                _context.Entry(country).Reload();                         
                ModelState.AddModelError("", ex.InnerException.Message);
                return View(country);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CreateModalForm()
        {
            Country country = new Country { Code = string.Empty, Name = string.Empty };
            return PartialView("_CreateModalForm", country);
        }

    }
}