using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 论坛统计类
	/// </summary>
	public class Statistics
	{
		/// <summary>
		/// 获得统计列
		/// </summary>
		/// <returns>统计列</returns>
		public static DataRow GetStatisticsRow()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataRow dr = cache.RetrieveObject("/Statistics") as DataRow;
			if(dr == null)
			{
                dr = DatabaseProvider.GetInstance().GetStatisticsRow();
				cache.AddObject("/Statistics", dr);
			}
			return dr;
		}

		/// <summary>
		/// 将缓存中的统计列保存到数据库
		/// </summary>
		public static void SaveStatisticsRow()
		{
            DatabaseProvider.GetInstance().SaveStatisticsRow(GetStatisticsRow());	
        }


		/// <summary>
		/// 获取指定版块中的主题帖子统计数据
		/// </summary>
		/// <param name="fid"></param>
		/// <param name="topiccount"></param>
		/// <param name="postcount"></param>
		/// <param name="todaypostcount"></param>
		public static void GetPostCountFromForum(int fid, out int topiccount,out int postcount, out int todaypostcount)
		{
			topiccount = 0;
			postcount = 0;
			todaypostcount = 0;
			IDataReader  reader = null;
			if (fid == 0)
			{
                reader = DatabaseProvider.GetInstance().GetAllForumStatistics();
			}
			else
			{
                reader = DatabaseProvider.GetInstance().GetForumStatistics(fid);
			}

			while (reader.Read())
			{
				topiccount = Utils.StrToInt(reader["topiccount"],0);
				postcount = Utils.StrToInt(reader["postcount"],0);
				todaypostcount = Utils.StrToInt(reader["todaypostcount"],0);
			}
			reader.Close();

		}



		/// <summary>
		/// 获得指定名称的统计项
		/// </summary>
		/// <param name="param">项</param>
		/// <returns>统计值</returns>
		public static string GetStatisticsRowItem(string param)
		{
			return GetStatisticsRow()[param].ToString();
		}


		/// <summary>
		/// 得到上一次执行搜索操作的时间
		/// </summary>
		/// <returns></returns>
		public static string GetStatisticsSearchtime()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			string searchtime = cache.RetrieveObject("/StatisticsSearchtime") as string;
			if (searchtime == null)
			{
				searchtime = DateTime.Now.ToString();
				cache.AddObject("/StatisticsSearchtime", searchtime);
			}
			return searchtime;
		}

		/// <summary>
		/// 得到用户在一分钟内搜索的次数。
		/// </summary>
		/// <returns></returns>
		public static int GetStatisticsSearchcount()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			int searchcount = Utils.StrToInt(cache.RetrieveObject("/StatisticsSearchcount"),0);
			if (searchcount == 0)
			{
				searchcount = 1;
				cache.AddObject("/StatisticsSearchcount", searchcount);
			}
			return Utils.StrToInt(searchcount,1);
		}


		/// <summary>
		/// 重新设置用户上一次执行搜索操作的时间
		/// </summary>
		/// <param name="searchtime">操作时间</param>
		public static void SetStatisticsSearchtime(string searchtime)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			cache.RemoveObject("/StatisticsSearchtime");
			cache.AddObject("/StatisticsSearchtime", searchtime);
		}

		/// <summary>
		/// 设置用户在一分钟内搜索的次数为初始值。
		/// </summary>
		/// <param name="searchcount">初始值</param>
		public static void SetStatisticsSearchcount(int searchcount)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			cache.RemoveObject("/StatisticsSearchcount");
			cache.AddObject("/StatisticsSearchcount", searchcount);
		}

		/// <summary>
		/// 设置指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>统计项</returns>
		public static void SetStatisticsRowItem(string param,int Value)
		{
			GetStatisticsRow()[param] = Value;
		}

		/// <summary>
		/// 设置指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>统计项</returns>
		public static void SetStatisticsRowItem(string param,string Value)
		{
			GetStatisticsRow()[param] = Value;
		}

		/// <summary>
		/// 设置指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>统计项</returns>
		public static void SetStatisticsRowItem(string param,DateTime Value)
		{
			GetStatisticsRow()[param] = Value;
		}



		/// <summary>
		/// 更新指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>更新数</returns>
		public static int UpdateStatistics(string param,int intValue)
		{
            return DatabaseProvider.GetInstance().UpdateStatistics(param, intValue);
		}

		/// <summary>
		/// 更新指定名称的统计项
		/// </summary>
		/// <param name="param">项目名称</param>
		/// <param name="Value">指定项的值</param>
		/// <returns>更新数</returns>
		public static int UpdateStatistics(string param,string strValue)
		{
            return DatabaseProvider.GetInstance().UpdateStatistics(param, strValue);
		}



		/// <summary>
		/// 检查并更新60秒内统计的数量
		/// </summary>
		/// <param name="maxspm">60秒内允许的最大搜索次数</param>
		/// <returns>没有超过最大搜索次数返回true,否则返回false</returns>
		public static bool CheckSearchCount(int maxspm)
		{
			if (maxspm == 0)
				return true;
            string searchtime = GetStatisticsSearchtime();
			int searchcount = GetStatisticsSearchcount();
			if (Utils.StrDateDiffSeconds(searchtime,60) > 0)
			{
				SetStatisticsSearchtime(DateTime.Now.ToString());
				SetStatisticsSearchcount(1);
			}
			
			if (searchcount > maxspm)
			{
				return false;
			}

			SetStatisticsSearchcount(searchcount + 1);
			return true;			

		}

		/// <summary>
		/// 重建统计缓存
		/// </summary>
		public static void ReSetStatisticsCache()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            DataRow dr = DatabaseProvider.GetInstance().GetStatisticsRow();
			cache.RemoveObject("/Statistics");
			cache.AddObject("/Statistics", dr);
		}
	}
}
