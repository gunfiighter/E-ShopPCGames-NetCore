using BuiMuiGaim_DataAccess.Repository.IRepository;
using BuiMuiGaim_Models;
using BuiMuiGaim_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IInquiryHeaderRepository _inqHRepo;
        private readonly IInquiryDetailRepository _inqDRepo;
        [BindProperty]
        public InquiryVM InquiryVM { get; set; }
        public InquiryController(IInquiryDetailRepository inqDRepo, IInquiryHeaderRepository inqHRepo)
        {
            _inqDRepo = inqDRepo;
            _inqHRepo = inqHRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                InquiryHeader = _inqHRepo.FirstOrDefault(x => x.Id == id),
                InquiryDetail = _inqDRepo.GetAll(x => x.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _inqHRepo.GetAll() });
        }
        #endregion
    }
}
