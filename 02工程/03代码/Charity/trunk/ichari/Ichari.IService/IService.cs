using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Ichari.IService
{
    public interface IService<TEntity>
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate);
        void Add(TEntity entity);
        void Delete(TEntity entity);
       

        IQueryable<TEntity> GetQueryList();
        IQueryable<TEntity> GetQueryList(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetList();
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        int GetCount(Expression<Func<TEntity, bool>> predicate);
    }
}
