using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository category { get; }
        ICoverTypeRepository coverType { get; }
        ISPCALL SPCALL { get; }
        IProductRepository product { get; }
        ICompanyRepository company { get; }
        IApplicationUserRepository applicationUser {get;}
        IShoppingCartRepository shoppingCart { get; }
        IOrderHeaderRepository orderHeader { get; }
        IOrderDetailRepository orderDetail { get; }
        

        void Save();
    }
}
