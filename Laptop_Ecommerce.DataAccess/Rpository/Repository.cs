using Laptop_Ecommerce.DataAccess.Data;
using Laptop_Ecommerce.DataAccess.Rpository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Laptop_Ecommerce.DataAccess.Rpository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet=_context.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includePropties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter!=null)
            {
                query = query.Where(filter);
            }
            if(includePropties!=null)
            {
                foreach (var includeProp in includePropties.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderby = null, string includePropties = null)
        {
            IQueryable<T> query = dbSet;
            if(filter!=null)
            {
                query = query.Where(filter);
            }
            if(includePropties!=null)
            {
                foreach (var includeProp in includePropties.Split(new[]{ ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderby != null)
                return orderby(query).ToList();
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity= Get(id);
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear();
            dbSet.Update(entity);
        }
    }
}
