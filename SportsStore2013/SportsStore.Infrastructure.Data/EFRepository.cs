using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using SportsStore.Core.Contracts.Models;
using SportsStore.Core.Contracts.Repositories;

namespace SportsStore.Infrastructure.Data
{
    public class EFRepository<T>: IRepository<T> where T: TEntity
    {
        private readonly EFDbContext _context;
        protected DbSet<T> DbSet { get; set; }
        
        public EFRepository(EFDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        } 

        public virtual IList<T> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IList<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return query.ToList();
        }

        public virtual T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public virtual T GetByIdIncluding(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = DbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            T result = query.FirstOrDefault(x => x.Id == id);

            return result;
        }

        public virtual void Add(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
                dbEntityEntry.State = EntityState.Added;
            else
                DbSet.Add(entity);
        }

        public virtual void Update(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State == EntityState.Deleted)
                DbSet.Attach(entity);

            dbEntityEntry.State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _context.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
                dbEntityEntry.State = EntityState.Deleted;
            else
            {
                DbSet.Attach(entity);
                DbSet.Remove(entity);
            }
        }

        public virtual void Delete(int id)
        {
            T entity = GetById(id);
            if (entity == null)
                return; // not found; assume already deleted.
            Delete(entity);
        }
    }
}
