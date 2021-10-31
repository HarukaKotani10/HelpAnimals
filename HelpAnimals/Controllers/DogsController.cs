using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpAnimals.Data;
using HelpAnimals.Models;

namespace HelpAnimals.Controllers
{
    public class DogsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DogsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Dogs> objList = _db.Dogs;
            return View(objList);
        }

        // GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        // POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Dogs obj)
        {
            _db.Dogs.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
