using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim_Models.ViewModels
{
    public class ProductVM
    {
        public Product Product{ get; set; }
        public IEnumerable<SelectListItem> GenreSelectList { get; set; }
        public IEnumerable<SelectListItem> PublisherSelectList { get; set; }
    }
}
