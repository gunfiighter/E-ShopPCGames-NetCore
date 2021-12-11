using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiMuiGaim_Models.ViewModels
{
    public class InquiryVM
    {
        public InquiryHeader InquiryHeader { get; set; }
        public IEnumerable<InquiryDetail> InquiryDetails { get; set; }

    }
}
