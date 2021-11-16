using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HelpAnimals.Data;
using HelpAnimals.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

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
    }
}
