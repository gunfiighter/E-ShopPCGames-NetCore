using BuiMuiGaim_Data;
using BuiMuiGaim_DataAccess.Repository.IRepository;
using BuiMuiGaim_Models;
using BuiMuiGaim_Utility;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiMuiGaim_DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<SelectListItem> GetAllDropDownList(string obj)
        {
            if(obj == WC.GenreName)
            {
               return _db.Genre.Select(x =>
                   new SelectListItem
                   {
                       Text = x.Name,
                       Value = x.GenreId.ToString()
                   }
               );
            }
            if(obj == WC.PublisherName)
            {
                return _db.Publisher.Select(x =>
                    new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id.ToString()
                    }
                );
            }
            return null;
        }

        public void Update(Product obj)
        {
            _db.Product.Update(obj);
        }
    }
}
