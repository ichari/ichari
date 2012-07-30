using System;
using System.Data;
using System.Data.Common;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminModeratorManageLogFactory 的摘要说明。
	/// 勋章日志操作管理类
	/// </summary>
	public class AdminMedalLogs
	{
		public AdminMedalLogs()
		{
		}

		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
            return DatabaseProvider.GetInstance().DeleteMedalLog();
		}


		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return DatabaseProvider.GetInstance().DeleteMedalLog(condition);
		}


		/// <summary>
		/// 得到当前指定页数的勋章日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage)
		{
            return DatabaseProvider.GetInstance().GetMedalLogList(pagesize, currentpage);
		}


		/// <summary>
		/// 得到当前指定条件和页数的勋章日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return DatabaseProvider.GetInstance().GetMedalLogList(pagesize, currentpage, condition);
		}


		/// <summary>
		/// 得到缓存日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return DatabaseProvider.GetInstance().GetMedalLogListCount();
		}


		/// <summary>
		/// 得到指定查询条件下的勋章日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetMedalLogListCount(condition);
		}
	}
}