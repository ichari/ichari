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
	/// AdminRateLogFactory 的摘要说明。
	/// 后台评分日志管理操作类
	/// </summary>
	public class AdminRateLogs
	{

		/// <summary>
		/// 添加评分记录
		/// </summary>
		/// <param name="postidlist">被评分帖子pid</param>
		/// <param name="userid">评分者uid</param>
		/// <param name="username">评分者用户名</param>
		/// <param name="extid">分的积分类型</param>
		/// <param name="score">积分数值</param>
		/// <param name="reason">评分理由</param>
		/// <returns>更新数据行数</returns>
		public static int InsertLog(string postidlist, int userid, string username, int extid, int score, string reason)
		{
            int reval = 0;
            foreach (string pid in Utils.SplitString(postidlist, ","))
            {
                reval += DatabaseProvider.GetInstance().InsertRateLog(Utils.StrToInt(pid, 0), userid, username, extid, score, reason);
            }
            return reval;
		}


		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
            return DatabaseProvider.GetInstance().DeleteRateLog();
		}



		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return DatabaseProvider.GetInstance().DeleteRateLog(condition);

		}



		/// <summary>
		/// 得到当前指定页数的评分日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage)
		{
            return DatabaseProvider.GetInstance().RateLogList(pagesize, currentpage, Posts.GetPostTableName());
		}



		/// <summary>
		/// 得到当前指定条件和页数的评分日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return DatabaseProvider.GetInstance().RateLogList(pagesize, currentpage, Posts.GetPostTableName(), condition);
		}



		/// <summary>
		/// 得到评分日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return DatabaseProvider.GetInstance().GetRateLogCount();
		}



		/// <summary>
		/// 得到指定查询条件下的评分日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetRateLogCount(condition);
		}
    }
}