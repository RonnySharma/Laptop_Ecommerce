using Microsoft.EntityFrameworkCore;
using Project_Ecomm.DataAccess.Data;
using Project_Ecomm_DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project_Ecomm_DataAccess.Repository
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

        public T FristOrDefault(Expression<Func<T, bool>> filter = null, string IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if (IncludeProperties != null)
            {
                foreach (var includeprop in IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeprop);
                }
            }
            return query.FirstOrDefault();
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> Orderby = null, string IncludeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
                query = query.Where(filter);
            if(IncludeProperties!=null)
            {
                foreach(var includeprop in IncludeProperties.Split(new[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query=query.Include(includeprop);
                }
            }
            if (Orderby != null)
                return Orderby(query).ToList();
            return query.ToList();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            var entity = dbSet.Find(id);
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
