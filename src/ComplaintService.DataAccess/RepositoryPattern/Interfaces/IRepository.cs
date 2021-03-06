﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ComplaintService.DataAccess.RepositoryPattern.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }
        T Get(string id);
        IEnumerable<T> GetAll();

        T Find(Expression<Func<T, bool>> predicate);

        void Insert(T entity, bool saveNow = true);
        void InsertRange(IEnumerable<T> entities, bool saveNow = true);

        IQueryable<T> Fetch(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            int? page = null, int? pageSize = null);

        int Count(Expression<Func<T, bool>> predicate);

        void Update(T entity, bool isSaveNow = true);

        void Remove(object entity, bool saveNow = true);

        void Remove(T entity, bool saveNow = true);
        void RemoveRange(IEnumerable<T> entities, bool save = true);
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}