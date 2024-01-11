using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.DataAccess.Rpository.IRepository
{
    public interface IUnitOfWork
    {
        ILaptopCompanyRepository LaptopCompany { get; }
        IProcessorRepository Processor { get; }
        IGraphicsCardRepository GraphicsCard { get; }
        ILaptopRepository Laptop { get; }
        ICompanyRepository Company { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        void Save();
    }
}
