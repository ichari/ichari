using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Uow
{
    public abstract class BaseUnitOfWork : IUnitOfWork, IDisposable
    {
        //protected ChariDbContainner _context;
        protected ObjectContext _context;

       

        private bool disposed = false;

        public ObjectContext DbContext
        {
            get
            {
                return _context;
            }
        }

        public BaseUnitOfWork(ObjectContext context)
        {
            this._context = context;
        }

        public BaseUnitOfWork(string entityTag)
        {
            switch (entityTag)
            {
                case "Admin":
                    _context = new AdminEntities();
                    break;
                case "Chari":
                    _context = new ChariDbContainner();
                    break;
                case "Drawings":
                    _context = new ChariDbContainner();
                    break;
                default:
                    _context = new ChariDbContainner();
                    break;
            }
            
        }

        public abstract void Initialize();

        /// <summary>
        /// 使用反射创建Uow的实例，使用当前调用对象的ObjectContext
        /// </summary>
        /// <typeparam name="T1">some of IUnitOfWork</typeparam>
        /// <typeparam name="T2">some of UnitOfWork</typeparam>
        /// <returns></returns>
        public T1 GetUow<T1,T2>() 
            where T2 : class,T1
        {
            var t = typeof(T2);
            var instance = Activator.CreateInstance(t, this._context);
            return (T1)instance;
        }

        public virtual int Commit()
        {
            return _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
