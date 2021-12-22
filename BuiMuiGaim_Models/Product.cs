using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BuiMuiGaim_Models
{
    public class Product
    {

        public Product()
        {
            TempAmount = 1;
        }

        [Key]
        public int Id{ get; set; }
        [Required]
        public string Name{ get; set; }
        public string ShortDesc { get; set; }
        public string Description{ get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int Price { get; set; }

        public string Image{ get; set; }

        [Display(Name = "Genre Type")]
        public int GenreId{ get; set; }
        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }
        [ForeignKey("PublisherId")]
        public virtual Publisher Publisher{ get; set; }

        [NotMapped]
        [Range(1,10000)]
        public int TempAmount { get; set; }
    }
}
