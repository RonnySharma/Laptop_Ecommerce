using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Laptop_Ecommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.DataAccess.Rpository
{
    public class OrderHeaderRepository:Repository<OrderHeader>,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
