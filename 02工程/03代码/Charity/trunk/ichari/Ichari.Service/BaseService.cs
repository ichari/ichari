using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Ichari.IService;
using BaseRepository;

namespace Ichari.Service
{
    public class BaseService<TEntity> : IService<TEntity>
       where TEntity : BaseEntity
    {
        protected IRepositoryBase<TEntity> _repository;

        #region constructor
        public BaseService()
        { }

        public BaseService(IRepositoryBase<TEntity> repository)
        {
            this._repository = repository;
        }
        public BaseService(System.Data.Objects.ObjectContext context)
        {
            this._repository = new Repository<TEntity>(context);
        }
        #endregion

        public virtual TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this._repository.Get(predicate);
        }

        public virtual void Add(TEntity entity)
        {
            this._repository.Add(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            this._repository.Delete(entity);
        }

        

        public virtual IQueryable<TEntity> GetQueryList()
        {
            return this._repository.GetQueryList();
        }

        public virtual IQueryable<TEntity> GetQueryList(Expression<Func<TEntity, bool>> predicate)
        {
            return this._repository.GetQueryList(predicate);
        }

        public virtual IEnumerable<TEntity> GetList()
        {
            return this._repository.GetList();
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return this._repository.GetList(predicate);
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return this._repository.GetCount(predicate);
        }
    }
}
