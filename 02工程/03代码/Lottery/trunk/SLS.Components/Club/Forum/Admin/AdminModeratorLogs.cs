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
	/// 前台管理日志管理操作类
	/// </summary>
	public class AdminModeratorLogs
	{
		public AdminModeratorLogs()
		{
		}

		/// <summary>
		/// 插入版主管理日志记录
		/// </summary>
		/// <param name="moderatorname">版主名</param>
		/// <param name="grouptitle">所属组的ID</param>
		/// <param name="ip">客户端的IP</param>
		/// <param name="fname">版块的名称</param>
		/// <param name="title">主题的名称</param>
		/// <param name="actions">动作</param>
		/// <param name="reason">原因</param>
		/// <returns></returns>
		public static bool InsertLog(string moderatoruid, string moderatorname, string groupid, string grouptitle, string ip, string postdatetime, string fid, string fname, string tid, string title, string actions, string reason)
		{
            return DatabaseProvider.GetInstance().InsertModeratorLog(moderatoruid, moderatorname, groupid, grouptitle, ip, postdatetime, fid, fname, tid, title, actions, reason);
		}


		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return DatabaseProvider.GetInstance().DeleteModeratorLog(condition);
		}


		/// <summary>
		/// 得到当前指定页数的前台管理日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage)
		{
			return LogList(pagesize, currentpage, "");
		}


		/// <summary>
		/// 得到当前指定条件和页数的前台管理日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize, int currentpage, string condition)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetModeratorLogList(pagesize, currentpage, condition);

            GeneralConfigInfo config = GeneralConfigs.GetConfig();
			foreach (DataRow dr in dt.Rows)
			{
				if (dr["title"].ToString().Trim() != "")
				{
					try
					{
						if (dr["actions"].ToString().Trim() == "rate")
						{
							dr["title"] = "<a href=../../showtree.aspx?postid=" + dr["tid"] + ">" + dr["title"] + "</a>";
							dr["actions"] = "评分";
						}
						else if (dr["actions"].ToString().Trim() == "delposts")
						{
                            if (config.Aspxrewrite == 1)
                            {
                                dr["title"] = "<a href='../../showtopic-" + dr["tid"] + ".aspx' title='内容摘要:" + dr["title"].ToString().Split('|')[1] + "\r\n点击查看所在主题'>" + dr["title"].ToString().Split('|')[0] + "</a>";
                            }
                            else
                            {
                                dr["title"] = "<a href='../../showtopic.aspx?topicid=" + dr["tid"] + "' title='内容摘要:" + dr["title"].ToString().Split('|')[1] + "\r\n点击查看所在主题'>" + dr["title"].ToString().Split('|')[0] + "</a>";
                            }
							dr["actions"] = "批量删贴";
						}
						else
						{
                            if (config.Aspxrewrite == 1)
                            {
                                dr["title"] = "<a href='../../showtopic-" + dr["tid"] + ".aspx'>" + dr["title"] + "</a>";
                            }
                            else
                            {
                                dr["title"] = "<a href='../../showtopic.aspx?topicid=" + dr["tid"] + "'>" + dr["title"] + "</a>";
                            }
						}
					}
					catch
					{
						dr["title"] = "暂无";
					}
				}
			}
			return dt;
		}


		/// <summary>
		/// 得到前台管理日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return DatabaseProvider.GetInstance().GetModeratorLogListCount();
		}


		/// <summary>
		/// 得到指定查询条件下的前台管理日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetModeratorLogListCount(condition);	
        }
	}
}