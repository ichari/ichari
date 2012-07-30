using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 论坛公告操作类
	/// </summary>
	public class Announcements
	{
		/// <summary>
		/// 获得全部指定时间段内的公告列表
		/// </summary>
		/// <param name="starttime">开始时间</param>
		/// <param name="endtime">结束时间</param>
		/// <returns>公告列表</returns>
		public static DataTable GetAnnouncementList(string starttime, string endtime)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataTable dt = cache.RetrieveObject("/AnnouncementList") as DataTable;
			
			if(dt == null)
			{
                dt = DatabaseProvider.GetInstance().GetAnnouncementList(starttime, endtime);
                cache.AddObject("/AnnouncementList", dt);
			}
			return dt;
		}

		/// <summary>
		/// 获得全部指定时间段内的第一条公告列表
		/// </summary>
		/// <param name="starttime">开始时间</param>
		/// <param name="endtime">结束时间</param>
		/// <returns>公告列表</returns>
		public static DataTable GetSimplifiedAnnouncementList(string starttime, string endtime)
		{
			return GetSimplifiedAnnouncementList(starttime, endtime, -1);
		}

		/// <summary>
		/// 获得全部指定时间段内的前n条公告列表
		/// </summary>
		/// <param name="starttime">开始时间</param>
		/// <param name="endtime">结束时间</param>
		/// <param name="maxcount">最大记录数,小于0返回全部</param>
		/// <returns>公告列表</returns>
		public static DataTable GetSimplifiedAnnouncementList(string starttime, string endtime, int maxcount)
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			DataTable dt = cache.RetrieveObject("/SimplifiedAnnouncementList") as DataTable;
			
			if(dt == null)
			{
                dt = DatabaseProvider.GetInstance().GetSimplifiedAnnouncementList(starttime, endtime, maxcount);
				cache.AddObject("/SimplifiedAnnouncementList", dt);
			}
			return dt;
		}

	}
}
