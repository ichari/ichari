using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Objects.DataClasses;



namespace BaseRepository
{
    /// <summary>
    /// repository基类
    /// </summary>
    /// <typeparam name="T">ObjectContext的子类</typeparam>
    public abstract class BaseRepository<T> where T : ObjectContext
    {
        protected T _dbContext;
        public T DBContext
        {
            get
            {
                return _dbContext;
            }
            set
            {
                _dbContext = value;
            }
        }
        /// <summary>
        /// Repository基类构造函数
        /// </summary>
        public BaseRepository()
        {
            //_dbContext = default(T);
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
