using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.DataAccess.Rpository.IRepository
{
    public interface IRepository<T>where T:class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
        void Remove(int id);
        T Get(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null,
            string includePropties = null
            );
       T FirstOrDefault(
             Expression<Func<T, bool>> filter = null,
            string includePropties = null
            );
    }
}
