using System;
using System.IO;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Specialized;
using System.Web;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// 主题时间类型
	/// </summary>
	public enum TopicTimeType
	{
        /// <summary>
        /// 一天
        /// </summary>
		Day = 1,
        /// <summary>
        /// 一周
        /// </summary>
		Week = 2,
        /// <summary>
        /// 一个月
        /// </summary>
		Month = 3,
        /// <summary>
        /// 六个月
        /// </summary>
        SixMonth = 4,
        /// <summary>
        /// 一年
        /// </summary>
        Year = 5,
		All = 0
	}

	/// <summary>
	/// 主题排序类型
	/// </summary>
	public enum TopicOrderType
	{
		ID = 0,
        /// <summary>
        /// 浏览量
        /// </summary>
		Views = 1,
        /// <summary>
        /// 最后回复
        /// </summary>
		LastPost = 2,
        /// <summary>
        /// 按最新主题查
        /// </summary>
        PostDataTime = 3,
        /// <summary>
        /// 按精华主题查
        /// </summary>
        Digest = 4,
        /// <summary>
        /// 按回复数
        /// </summary>
        Replies = 5
	}

	/// <summary>
	/// 焦点数据操作类
	/// </summary>
	public class Focuses
	{
		/// <summary>
		/// 精华主题列表
		/// </summary>
		/// <param name="count">获取数</param>
		/// <returns></returns>
		public static DataTable GetDigestTopicList(int count)
		{

			return GetTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, true, 20, false);
		}

		/// <summary>
		/// 获取精华主题列表
		/// </summary>
		/// <param name="count">获取数</param>
		/// <param name="fid">版块id</param>
		/// <param name="timetype">时间类型</param>
		/// <param name="ordertype">排序类型</param>
		/// <returns></returns>
		public static DataTable GetDigestTopicList(int count, int fid, TopicTimeType timetype, TopicOrderType ordertype)
		{
			return GetTopicList(count, -1, fid, timetype, ordertype, true, 20, false);
		}


		/// <summary>
		/// 获得热门主题列表
		/// </summary>
		/// <param name="count">获取数</param>
		/// <param name="views">查看数</param>
		/// <returns></returns>
		public static DataTable GetHotTopicList(int count, int views)
		{

			return GetTopicList(count, views, 0, TopicTimeType.All, TopicOrderType.ID, false, 20, false);
		}

		/// <summary>
		/// 获得热门主题列表
		/// </summary>
		/// <param name="count">获取数</param>
		/// <param name="views">查看数</param>
		/// <param name="fid">版块id</param>
		/// <param name="timetype">时间类型</param>
		/// <param name="ordertype">排序类型</param>
		/// <returns></returns>
		public static DataTable GetHotTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype)
		{
			return GetTopicList(count, views, fid, timetype, ordertype, false, 20, false);
		}

		/// <summary>
		/// 获得最新发表主题列表
		/// </summary>
		/// <param name="count"></param>
		/// <returns></returns>
		public static DataTable GetRecentTopicList(int count)
		{

			return GetTopicList(count, -1, 0, TopicTimeType.All, TopicOrderType.ID, false, 20, false);
		}

		/// <summary>
		/// 获得帖子列表
		/// </summary>
		/// <param name="count">数量</param>
		/// <param name="views">最小浏览量</param>
		/// <param name="fid">板块ID</param>
		/// <param name="timetype">期限类型,一天、一周、一月、不限制</param>
		/// <param name="ordertype">排序类型,时间倒序、浏览量倒序、最后回复倒序</param>
		/// <param name="isdigest">是否精华</param>
		/// <param name="cachetime">缓存的有效期(单位:分钟)</param>
		/// <returns></returns>
		public static DataTable GetTopicList(int count, int views, int fid, TopicTimeType timetype, TopicOrderType ordertype, bool isdigest, int cachetime, bool onlyimg)
		{
			//防止恶意行为
			if (cachetime == 0)
				cachetime = 1;
			if (count > 50)
				count = 50;
			if (count < 1)
				count = 1;
		
			string cacheKey = "/TopicList-{0}-{1}-{2}-{3}-{4}-{5}-{6}";
			cacheKey = string.Format(cacheKey,
				count,
				views,
				fid,
				timetype,
				ordertype,
				isdigest,
				onlyimg
				);

			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable dt = cache.RetrieveObject(cacheKey) as DataTable;
            if (dt == null)
            {
                dt = DatabaseProvider.GetInstance().GetFocusTopicList(count, views, fid, GetStartDate(timetype), GetFieldName(ordertype), Forums.GetVisibleForum(), isdigest, onlyimg);

                //声明新的缓存策略接口
                Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                ics.TimeOut = cachetime;
                cache.LoadCacheStrategy(ics);
                cache.AddObject(cacheKey, dt);
                cache.LoadDefaultCacheStrategy();    
            }
        
            return dt;
		}

		#region Helper
		/// <summary>
		/// 获取开始日期
		/// </summary>
		/// <param name="type">日期类型</param>
		/// <returns></returns>
		public static string GetStartDate(TopicTimeType type)
		{
			DateTime dtnow = DateTime.Now;
			switch (type)
			{
				case TopicTimeType.Day:
					return dtnow.AddDays(-1).ToString();
				case TopicTimeType.Week:
					return dtnow.AddDays(-7).ToString();
				case TopicTimeType.Month:
					return dtnow.AddDays(-30).ToString();
                case TopicTimeType.SixMonth:
                    return dtnow.AddMonths(-6).ToString();
                case TopicTimeType.Year:
                    return dtnow.AddYears(-1).ToString();
				default: return "1754-1-1";
			}
		}

		/// <summary>
		/// 获取字段名
		/// </summary>
		/// <param name="type">排序类型</param>
		/// <returns></returns>
		public static string GetFieldName(TopicOrderType type)
		{
			switch (type)
			{
				case TopicOrderType.Views: 
					return "views";
				case TopicOrderType.LastPost:
					return "lastpost";
                case TopicOrderType.PostDataTime:
                    return "postdatetime";
                case TopicOrderType.Digest:
                    return "digest";
                case TopicOrderType.Replies:
                    return "replies";
                    
				default: return "tid";
			}
		}
		#endregion

	}

}
