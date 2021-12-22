using BuiMuiGaim_Data;
using BuiMuiGaim_DataAccess.Repository.IRepository;
using BuiMuiGaim_Models;
using BuiMuiGaim_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class GenreController : Controller
    {
        private readonly IGenreRepository _genreRepo;

        public GenreController(IGenreRepository catRepo)
        {
            _genreRepo = catRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Genre> objList = _genreRepo.GetAll();
            return View(objList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //POST - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Genre obj)
        {
            if(ModelState.IsValid)
            {
                _genreRepo.Add(obj);
                _genreRepo.Save();
                TempData[WC.Success] = "Genre created successfully";
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Error while creating Genre";
            return View(obj);
        }

        //Get - Create
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            //поиск по первичному ключу
            var obj = _genreRepo.Find(id.GetValueOrDefault());
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Genre obj)
        {
            if (ModelState.IsValid)
            {
                _genreRepo.Update(obj);
                _genreRepo.Save();
                TempData[WC.Success] = "Genre updated successfully";
                
                return RedirectToAction("Index");
            }
            TempData[WC.Error] = "Error while editing Genre";
            return View(obj);
        }

        //Get - Create
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //поиск по первичному ключу
            var obj = _genreRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? GenreId)
        {
            var obj = _genreRepo.Find(GenreId.GetValueOrDefault());
            if(obj == null)
            {
                TempData[WC.Error] = "Error while deleting Genre";
                return NotFound();
            }
            _genreRepo.Remove(obj);
            _genreRepo.Save();
            TempData[WC.Success] = "Genre deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
