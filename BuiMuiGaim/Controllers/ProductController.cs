using BuiMuiGaim_Data;
using BuiMuiGaim_Models;
using BuiMuiGaim_Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BuiMuiGaim_Utility;
using BuiMuiGaim_DataAccess.Repository.IRepository;

namespace BuiMuiGaim.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _prodRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository prodRepo, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _prodRepo = prodRepo;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _prodRepo.GetAll(includeProperties: "Category,ApplicationType");

            return View(objList);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _prodRepo.GetAllDropDownList(WC.CategoryName),
                ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WC.ApplicationTypeName)
            };

            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _prodRepo.Find(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if(ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using(var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _prodRepo.Add(productVM.Product);                    
                }
                else
                {
                    //updating
                    var objFromDb = _prodRepo.FirstOrDefault(x => x.Id == productVM.Product.Id, isTracking: false);

                    if(files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if(System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);

                            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                            {
                                files[0].CopyTo(fileStream);
                            }

                            productVM.Product.Image = fileName + extension;
                        }                      
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _prodRepo.Update(productVM.Product);
                }
                _prodRepo.Save();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _prodRepo.GetAllDropDownList(WC.CategoryName);
            productVM.ApplicationTypeSelectList = _prodRepo.GetAllDropDownList(WC.ApplicationTypeName);

            return View(productVM);
        }


        //Get - Create
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Include - Eager Loading
            Product product = _prodRepo.FirstOrDefault(x => x.Id == id, includeProperties: "Category,ApplicationType");
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST - DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var objFromDb = _prodRepo.Find(id.GetValueOrDefault());
            if (objFromDb == null)
            {
                return NotFound();
            }

            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, objFromDb.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);               
            }

            _prodRepo.Remove(objFromDb);
            _prodRepo.Save();
            return RedirectToAction("Index");
        }
    }
}
