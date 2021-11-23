using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpAnimals.Data;
using HelpAnimals.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace HelpAnimals.Controllers
{
    public class DogsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public DogsController(ApplicationDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
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
        public async Task<IActionResult> Create([Bind("Id, Name, Breed, Age, Gender, Size, Status, ImageName, ImageFile")] Dogs obj)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(obj.ImageFile.FileName);
            string extension = Path.GetExtension(obj.ImageFile.FileName);
            obj.ImageName=fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image/", fileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await obj.ImageFile.CopyToAsync(fileStream);
            }


            _db.Dogs.Add(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        // GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Dogs.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        // POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Dogs obj)
        {
            var existingDog = _db.Dogs.FirstOrDefault(s => s.Id == obj.Id);
            if (obj.ImageFile != null)
            {
                _db.Dogs.Add(existingDog);
                //_db.Entry(existingStudent).State = EntityState.Modified;
                _db.Dogs.Update(existingDog); //You can also use this. Comment out the upper two lines
                _db.SaveChanges();
            }
            else
            {
                // updating student.
                _db.Dogs.Attach(existingDog);
                _db.Entry(existingDog).State = EntityState.Modified;
                _db.Entry(existingDog).Property(x => x.ImageName).IsModified = false;
                _db.SaveChanges();
            }

            /* if (ModelState.IsValid)
             {
                 _db.Dogs.Update(obj);
                 _db.SaveChanges();
                 return RedirectToAction("Index");
             }

             return View(obj);*/

            return View(obj);
        }
    }
}
