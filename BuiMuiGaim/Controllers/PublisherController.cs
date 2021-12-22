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
    public class PublisherController : Controller
    {
        private readonly IPublisherRepository _publisherRepo;

        public PublisherController(IPublisherRepository appTypeRepo)
        {
            _publisherRepo = appTypeRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Publisher> objList = _publisherRepo.GetAll();
            return View(objList);
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //Get - Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Publisher obj)
        {
            _publisherRepo.Add(obj);
            _publisherRepo.Save();
            TempData[WC.Success] = "Publisher created succesfully";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)            
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _publisherRepo.Find(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Publisher obj)
        {
            _publisherRepo.Update(obj);
            _publisherRepo.Save();
            TempData[WC.Success] = "Publisher updated succesfully";
            return RedirectToAction("Index");
        }

        //Post - Delete
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _publisherRepo.Find(id.GetValueOrDefault());
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //Post - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _publisherRepo.Find(id.GetValueOrDefault());
            if(obj == null)
            {
                TempData[WC.Error] = "Error while deleting publisher";
                return NotFound();
            }

            _publisherRepo.Remove(obj);
            _publisherRepo.Save();
            TempData[WC.Success] = "Publisher deleted succesfully";
            return RedirectToAction("Index");
        }
    }

}
