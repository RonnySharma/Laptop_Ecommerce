using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context= context;
            category = new CategoryRepository(_context);
            coverType = new CoverTypeRepository(_context);
            SPCALL = new SPCALL(_context);
            product=new ProductRepository(_context);
            company=new CompanyRepository(_context);
            applicationUser = new ApplicationUserRepository(_context);
            shoppingCart=new ShoppingCartRepository(_context);
            orderHeader=new OrderHeaderRepository(_context);
            orderDetail = new OrderDetailRepository(_context);
            
        }
        public ICategoryRepository category { get; private set; }

        public ICoverTypeRepository coverType { get; private set; }
        public ISPCALL SPCALL { get; private set; }
        public IProductRepository product { get; private set; }
        public ICompanyRepository company{ get; private set; }
        public IApplicationUserRepository applicationUser { get; private set; }
        public IShoppingCartRepository shoppingCart { get; private set; }
        public IOrderHeaderRepository orderHeader { get; private set; }
        public IOrderDetailRepository orderDetail { get; private set; }
        

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
