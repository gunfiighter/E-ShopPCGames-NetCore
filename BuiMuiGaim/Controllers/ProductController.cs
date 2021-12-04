using BuiMuiGaim.Data;
using BuiMuiGaim.Models;
using BuiMuiGaim.Models.ViewModels;
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

namespace BuiMuiGaim.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType);

            return View(objList);
        }

        //Get - Upsert
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x =>
                new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.CategoryId.ToString()
                    }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }
                )
            };

            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _db.Product.Find(id);
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

                    _db.Product.Add(productVM.Product);                    
                }
                else
                {
                    //updating
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(x => x.Id == productVM.Product.Id);

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

                    _db.Product.Update(productVM.Product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CategoryId.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }
            );
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
            Product product = _db.Product.Include(x => x.Category).Include(x => x.ApplicationType).FirstOrDefault(x => x.Id == id);
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
            var objFromDb = _db.Product.FirstOrDefault(x => x.Id == id);
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

            _db.Product.Remove(objFromDb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
