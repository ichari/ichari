using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;

namespace Ichari.Uow
{
    public interface IUnitOfWork
    {
        //ChariDbContainner DbContext { get; }
        System.Data.Objects.ObjectContext DbContext {get;}
        void Initialize();
        int Commit();

        /// <summary>
        /// 使用反射创建Uow的实例，使用当前调用对象的ObjectContext
        /// </summary>
        /// <typeparam name="T1">some of IUnitOfWork</typeparam>
        /// <typeparam name="T2">some of UnitOfWork</typeparam>
        /// <returns></returns>
        T1 GetUow<T1, T2>()
            where T2 : class,T1;
            

    }
}
