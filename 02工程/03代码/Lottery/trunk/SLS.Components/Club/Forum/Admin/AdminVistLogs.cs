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
	/// AdminVistLogFactory 的摘要说明。
	/// 后台访问日志管理操作类
	/// </summary>
	public class AdminVistLogs
	{
		public AdminVistLogs()
		{
		}


		/// <summary>
		/// 插入版主管理日志记录
		/// </summary>
		/// <param name="uid">用户UID</param>
		/// <param name="username">用户名</param>
		/// <param name="groupid">所属组ID</param>
		/// <param name="grouptitle">所属组名称</param>
		/// <param name="ip">IP地址</param>
		/// <param name="actions">动作</param>
		/// <param name="others"></param>
		/// <returns></returns>
		public static bool InsertLog(int uid, string username, int groupid, string grouptitle, string ip, string actions, string others)
		{
			try
			{
                DatabaseProvider.GetInstance().AddVisitLog(uid, username, groupid, grouptitle, ip, actions, others);
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
			try
			{
                DatabaseProvider.GetInstance().DeleteVisitLogs();
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
			try
			{
                DatabaseProvider.GetInstance().DeleteVisitLogs(condition);
				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 得到当前指定页数的后台访问日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage)
		{
            return DatabaseProvider.GetInstance().GetVisitLogList(pagesize, currentpage);
		}


		/// <summary>
		/// 得到当前指定条件和页数的后台访问日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            return DatabaseProvider.GetInstance().GetVisitLogList(pagesize, currentpage, condition);
		}


		/// <summary>
		/// 得到后台访问日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return DatabaseProvider.GetInstance().GetVisitLogCount();
		}


		/// <summary>
		/// 得到指定查询条件下的后台访问日志记录数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetVisitLogCount(condition);
        }


	}
}