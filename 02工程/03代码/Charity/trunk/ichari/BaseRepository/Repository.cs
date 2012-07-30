using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;

namespace BaseRepository
{
    public class Repository<TEntity> : BaseRepository<System.Data.Objects.ObjectContext>,IRepositoryBase<TEntity>
        where TEntity : BaseEntity
        
    {
        private IObjectSet<TEntity> _objectSet;

        #region 构造器
        public Repository()
        {
            _objectSet = base.DBContext.CreateObjectSet<TEntity>();
        }

        public Repository(ObjectContext dbContext)
        {
            this._dbContext = dbContext;
            _objectSet = base.DBContext.CreateObjectSet<TEntity>();
        }

        #endregion

        #region 方法
        public virtual IEnumerable<TEntity> GetList()
        {
            return _objectSet.ToList();
        }

        public virtual IQueryable<TEntity> GetQueryList()
        {
            return _objectSet;
        }

        public virtual IQueryable<TEntity> GetQueryList(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }

        public virtual IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate).ToList();
        }

        public virtual int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate).Count();
        }

        public virtual TEntity Get(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {
            return _objectSet.Where(predicate).FirstOrDefault();
        }

        public virtual void Add(TEntity entity)
        {
            _objectSet.AddObject(entity);
        }

        

        public virtual void Delete(TEntity entity)
        {
            _objectSet.DeleteObject(entity);
        }
        #endregion
    }
}
