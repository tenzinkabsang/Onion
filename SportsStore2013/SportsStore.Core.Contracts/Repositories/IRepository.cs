using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SportsStore.Core.Contracts.Models;

namespace SportsStore.Core.Contracts.Repositories
{
    public interface IRepository<T> where T : TEntity
    {
        IList<T> GetAll();

        IList<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        //IList<T> GetAllFull();

        T GetById(int id);

        T GetByIdIncluding(int id, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(int id);
    }
}
