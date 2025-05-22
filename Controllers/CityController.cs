using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using AdvancedAjax.Models;
using AdvancedAjax.Data;

namespace AdvancedAjax.Controllers
{
    public class CityController : Controller
    {
        private readonly AppDbContext _context;

        public CityController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cities = _context.Cities.Include(c => c.Country).ToList();
            return View(cities);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Countries = _context.Countries.ToList();
            return View(new City { Code = string.Empty, Name = string.Empty });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                _context.SaveChanges();
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, city = new { id = city.Id, name = city.Name } });
                }
                return RedirectToAction(nameof(Index));
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_CreateModalForm", city);
            }
            ViewBag.Countries = _context.Countries.ToList();
            return View(city);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            var city = GetCity(Id);
            if (city == null)
                return NotFound();
            return View(city);
        }

        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var city = GetCity(Id);
            if (city == null)
                return NotFound();
            ViewBag.Countries = _context.Countries.ToList();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Edit(City city)
        {
            if (ModelState.IsValid)
            {
                _context.Attach(city);
                _context.Entry(city).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Countries = _context.Countries.ToList();
            return View(city);
        }

        private City? GetCity(int id)
        {
            return _context.Cities.Include(c => c.Country).FirstOrDefault(c => c.Id == id);
        }

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            var city = GetCity(Id);
            if (city == null)
                return NotFound();
            return View(city);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Delete(City city)
        {
            _context.Attach(city);
            _context.Entry(city).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public JsonResult GetCitiesByCountry(int countryId)
        {
            var cities = _context.Cities
                .Where(c => c.CountryId == countryId)
                .Select(c => new { c.Id, c.Name })
                .ToList();
            return Json(cities);
        }

        [HttpGet]
        public IActionResult CreateModalForm()
        {
            ViewBag.Countries = _context.Countries.Select(c => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return PartialView("_CreateModalForm", new City { Code = string.Empty, Name = string.Empty });
        }
    }
} 