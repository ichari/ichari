using System;
using System.Text;
using System.Reflection;
using Discuz.Config;

namespace Discuz.Data
{
	public class DatabaseProvider
	{
		private DatabaseProvider()
		{ }

		private static IDataProvider _instance = null;
		private static object lockHelper = new object();

		static DatabaseProvider()
		{
			GetProvider();
		}

		private static void GetProvider()
		{
			try
			{
                _instance = new Discuz.Data.SqlServer.DataProvider();
			}
			catch(Exception e)
			{
				throw new Exception("请检查DNT.config中Dbtype节点数据库类型是否正确，例如：SqlServer、Access、MySql" + e.Message);
			}
		}

		public static IDataProvider GetInstance()
		{
			if (_instance == null)
			{
				lock (lockHelper)
				{
					if (_instance == null)
					{
						GetProvider();
					}
				}
			}
			return _instance;
		}

        public static void ResetDbProvider()
        {
            _instance = null; 
        }
	}
}
