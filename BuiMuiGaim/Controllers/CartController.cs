using BuiMuiGaim_Data;
using BuiMuiGaim_Models;
using BuiMuiGaim_Models.ViewModels;
using BuiMuiGaim_Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuiMuiGaim.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        public ProductUserVM ProductUserVM { get; set; }

        public CartController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment, IEmailSender emailSender)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            List<ShoppingCart> shoppingCartList= new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(x => x.ProductId).ToList();
            IEnumerable<Product> prodList = _db.Product.Where(x => prodInCart.Contains(x.Id));

            return View(prodList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {

            return RedirectToAction(nameof(Summary));
        }


        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId = User.FindFirstValue(ClaimTypes.Name);

            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            List<int> prodInCart = shoppingCartList.Select(x => x.ProductId).ToList();
            List<Product> prodList = _db.Product.Where(x => prodInCart.Contains(x.Id)).ToList();

            ProductUserVM = new ProductUserVM()
            {
                ApplicationUser = _db.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = prodList
            };

            return View(ProductUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
            
        public async Task<IActionResult> SummaryPost(ProductUserVM ProductUserVM)
        {
            var pathToTemplate = _webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString()
                    + "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            string htmlBody = String.Empty;
            var subject = "New Inquiry";
            using (StreamReader sr = System.IO.File.OpenText(pathToTemplate))
            {
                htmlBody = sr.ReadToEnd();
            }

            StringBuilder productListSB = new StringBuilder();
            foreach (var product in ProductUserVM.ProductList)
            {
                productListSB.Append($"- Name: {product.Name} <span style='font-size:14px;'> (ID: {product.Id})</span><br />");
            }

            string messageBody = string.Format(htmlBody,
                ProductUserVM.ApplicationUser.FullName,
                ProductUserVM.ApplicationUser.Email,
                ProductUserVM.ApplicationUser.PhoneNumber,
                productListSB.ToString()
            );

            await _emailSender.SendEmailAsync(WC.EmailAdmin, subject, messageBody);
            return RedirectToAction(nameof(InquiryConfirmation));
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();
            return View();
        }

        public IActionResult Remove(int id)
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();

            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null &&
                HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)
            {
                //session exists
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Remove(shoppingCartList.FirstOrDefault(x => x.ProductId == id));
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            return RedirectToAction(nameof(Index));
        }
    }
}
