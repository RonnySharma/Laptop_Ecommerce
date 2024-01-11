using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_DataAccess.Repository.IRepository
{
    public interface IRepository<T>where T : class
    {
        void Add(T entity);
        void Remove(T entity);
        void Remove(int id);
        void Update(T entity);
        void RemoveRange(IEnumerable<T> entity);
        T Get (int id);
        IEnumerable<T> GetAll(Expression<Func<T,bool>>filter=null,
            Func<IQueryable<T>,IOrderedQueryable<T>>Orderby=null,
            string IncludeProperties=null);
        T FristOrDefault(
            Expression<Func<T, bool>> filter = null,
            string IncludeProperties = null
            );
    }
}
