using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using Project_Ecomm_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_DataAccess.Repository
{
    public class CoverTypeRepository:Repository<CoverType>,ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context):base(context) 
        {
            _context = context;
        }
    }
}
