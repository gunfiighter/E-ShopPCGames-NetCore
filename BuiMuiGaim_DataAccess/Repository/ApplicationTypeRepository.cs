using BuiMuiGaim_Data;
using BuiMuiGaim_DataAccess.Repository.IRepository;
using BuiMuiGaim_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuiMuiGaim_DataAccess.Repository
{
    public class ApplicationTypeRepository : Repository<ApplicationType>, IApplicationTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationTypeRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(ApplicationType obj)
        {
            var objFromDb = base.FirstOrDefault(x => x.Id == obj.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = obj.Name;
            }
        }
    }
}
