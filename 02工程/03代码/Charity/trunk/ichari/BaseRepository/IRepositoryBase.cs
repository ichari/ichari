using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Objects;

namespace BaseRepository
{
    public interface IRepositoryBase
    { }

    public interface IRepositoryBase<TEntity> : IRepositoryBase where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetQueryList();
        IQueryable<TEntity> GetQueryList(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetList();
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate);

        int GetCount(Expression<Func<TEntity, bool>> predicate);

        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity entity);
        //void Update(TEntity entity);
        void Delete(TEntity entity);

        void Save();
    }
}
