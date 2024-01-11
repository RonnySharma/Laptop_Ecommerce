using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.DataAccess.Rpository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            LaptopCompany = new LaptopCompanyRepository(_context);
            Processor = new ProcessorRepository(_context);
            GraphicsCard = new GraphicsCardRepository(_context);
            Laptop = new LaptopRepository(_context);
            Company = new CompanyRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
            ShoppingCart = new ShoppingCartRepository(_context);
            OrderHeader = new OrderHeaderRepository(_context);
            OrderDetails = new OrderDetailsRepository(_context);
        }
        public ILaptopCompanyRepository LaptopCompany { get; private set; }

        public IProcessorRepository Processor { get; private set; }
        public IGraphicsCardRepository GraphicsCard { get; private set; }
        public ILaptopRepository Laptop { get; private set; }

        public ICompanyRepository Company { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
