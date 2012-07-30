using System;
using System.Web;
using System.Collections;
using System.Web.Caching;

namespace Discuz.Cache
{


    abstract public class LogVisitor
    {
        abstract public void WriteLog(DefaultCacheStrategy dcs, string key, object val, CacheItemRemovedReason reason);
        protected static int m_IsWriteCachelog = 1;

        public int IsWriteCacheLog
        {
            set { m_IsWriteCachelog = value; }
            get { return m_IsWriteCachelog; }
        }
    }


	/// <summary>
	/// 默认缓存管理类
	/// </summary>
	public class  DefaultCacheStrategy : ICacheStrategy
	{
		private static readonly DefaultCacheStrategy instance = new DefaultCacheStrategy();

        protected static volatile System.Web.Caching.Cache webCache = System.Web.HttpRuntime.Cache;

        protected int _timeOut = 1440; // 默认缓存存活期为1440分钟(24小时)

		private static object syncObj = new object();

		/// <summary>
		/// 构造函数
		/// </summary>
		static DefaultCacheStrategy()
		{   
            //lock (syncObj)
            //{
            //    //System.Web.HttpContext context = System.Web.HttpContext.Current;
            //    //if(context != null)
            //    //    webCache = context.Cache;
            //    //else
            //    webCache = System.Web.HttpRuntime.Cache;
            //}	
		}

		
		
		/// <summary>
        /// 设置到期相对时间[单位：／分钟] 
		/// </summary>
		public int TimeOut
		{
			set { _timeOut = value > 0 ? value : 6000; }
			get { return _timeOut > 0 ? _timeOut : 6000; }
		}


        public static System.Web.Caching.Cache GetWebCacheObj
		{
			get { return webCache; }
		}
		
		/// <summary>
		/// 加入当前对象到缓存中
		/// </summary>
		/// <param name="objId">对象的键值</param>
		/// <param name="o">缓存的对象</param>
		public void AddObject(string objId, object o)
		{
	
			if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

			if (TimeOut == 6000)
			{
				webCache.Insert(objId, o, null, DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, callBack);
			}
			else
			{
				webCache.Insert(objId, o, null, DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
			}
		}

        /// <summary>
        /// 加入当前对象到缓存中
        /// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
		public void AddObjectWith(string objId, object o)
		{
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

			webCache.Insert(objId,o,null,System.DateTime.Now.AddHours(TimeOut),	System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High,	callBack);
		}



		/// <summary>
		/// 加入当前对象到缓存中,并对相关文件建立依赖
		/// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
		/// <param name="files">监视的路径文件</param>
		public void AddObjectWithFileChange(string objId, object o, string[] files)
		{
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

	   		CacheDependency dep = new CacheDependency(files, DateTime.Now);

			webCache.Insert(objId, o, dep, System.DateTime.Now.AddHours(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
		}
	


		/// <summary>
		/// 加入当前对象到缓存中,并使用依赖键
		/// </summary>
        /// <param name="objId">对象的键值</param>
        /// <param name="o">缓存的对象</param>
		/// <param name="dependKey">依赖关联的键值</param>
		public void AddObjectWithDepend(string objId, object o, string[] dependKey)
		{
            if (objId == null || objId.Length == 0 || o == null)
            {
                return;
            }

            CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(onRemove);

			CacheDependency dep = new CacheDependency(null, dependKey, DateTime.Now);

			webCache.Insert(objId, o, dep, System.DateTime.Now.AddMinutes(TimeOut), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.High, callBack);
		}
	


        /// <summary>
        /// 建立回调委托的一个实例
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="reason"></param>
		public void onRemove(string key, object val,CacheItemRemovedReason reason)
		{

            
                switch (reason)
                {
                    case CacheItemRemovedReason.DependencyChanged:
                            break;
                    case CacheItemRemovedReason.Expired:
                        {
                            //CacheItemRemovedCallback callBack = new CacheItemRemovedCallback(this.onRemove);

                            //webCache.Insert(key, val, null, System.DateTime.Now.AddMinutes(TimeOut),
                            //    System.Web.Caching.Cache.NoSlidingExpiration,
                            //    System.Web.Caching.CacheItemPriority.High,
                            //    callBack);
                            break;
                        }
                    case CacheItemRemovedReason.Removed:
                        {
                            break;
                        }
                    case CacheItemRemovedReason.Underused:
                        {
                            break;
                        }
                    default: break;
                }

			//如需要使用缓存日志,则需要使用下面代码
			//myLogVisitor.WriteLog(this,key,val,reason);
			
		}


		/// <summary>
		/// 删除缓存对象
		/// </summary>
		/// <param name="objId">对象的关键字</param>
		public void RemoveObject(string objId)
		{
			//objectTable.Remove(objId);
            if (objId == null || objId.Length == 0)
            {
                return;
            }
			webCache.Remove(objId);
		}


		/// <summary>
		/// 返回一个指定的对象
		/// </summary>
		/// <param name="objId">对象的关键字</param>
		/// <returns>对象</returns>
		public object RetrieveObject(string objId)
		{
           	//return objectTable[objId];

            if (objId == null || objId.Length == 0)
            {
                return null;
            }
			
			return webCache.Get(objId);
		}


//		private LogVisitor myLogVisitor;
//
//		public void Accept(LogVisitor lv)
//		{
//			myLogVisitor=lv;
//		}
    }






}
