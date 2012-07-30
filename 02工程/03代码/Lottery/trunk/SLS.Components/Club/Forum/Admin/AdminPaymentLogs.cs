using System;
using System.Data;
using System.Data.Common;

using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminPaymentLogFactory 的摘要说明。
	/// 积分交易日志管理操作类
	/// </summary>
	public class AdminPaymentLogs
	{
		public AdminPaymentLogs()
		{
		}

		
		/// <summary>
		/// 删除日志
		/// </summary>
		/// <returns></returns>
		public static bool DeleteLog()
		{
            return DatabaseProvider.GetInstance().DeletePaymentLog();
		}



		/// <summary>
		/// 按指定条件删除日志
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static bool DeleteLog(string condition)
		{
            return DatabaseProvider.GetInstance().DeletePaymentLog(condition);
		}



		/// <summary>
		/// 得到当前指定页数的积分交易日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize,int currentpage)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetPaymentLogList(pagesize, currentpage);
			if (dt!=null)
			{

				DataColumn dc=new DataColumn();
				dc.ColumnName="forumname";
				dc.DataType=System.Type.GetType("System.String");
				dc.DefaultValue="";
				dc.AllowDBNull=false;
				dt.Columns.Add(dc);
                DataTable ForumList = DatabaseProvider.GetInstance().GetForumList();
				foreach(DataRow dr in dt.Rows)
				{
					if(dr["fid"].ToString().Trim()!="")
					{
						foreach(DataRow forumdr in ForumList.Select("fid="+dr["fid"].ToString()))
						{
							dr["forumname"]=forumdr["name"].ToString();
							break;
						}
					}
				}
			}
			return dt;
		}

		

		/// <summary>
		/// 得到当前指定条件和页数的积分交易日志记录(表)
		/// </summary>
		/// <param name="pagesize">当前分页的尺寸大小</param>
		/// <param name="currentpage">当前页码</param>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static DataTable LogList(int pagesize,int currentpage , string condition)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetPaymentLogList(pagesize, currentpage, condition);
			if (dt!=null)
			{

				DataColumn dc=new DataColumn();
				dc.ColumnName="forumname";
				dc.DataType=System.Type.GetType("System.String");
				dc.DefaultValue="";
				dc.AllowDBNull=false;
				dt.Columns.Add(dc);
                DataTable ForumList = DatabaseProvider.GetInstance().GetForumList();
				foreach(DataRow dr in dt.Rows)
				{
					if(dr["fid"].ToString().Trim()!="")
					{
						foreach(DataRow forumdr in ForumList.Select("fid="+dr["fid"].ToString()))
						{
							dr["forumname"]=forumdr["name"].ToString();
							break;
						}
					}
				}
			}
			return dt;
		}



		/// <summary>
		/// 得到积分交易日志记录数
		/// </summary>
		/// <returns></returns>
		public static int RecordCount()
		{
            return DatabaseProvider.GetInstance().GetPaymentLogListCount();
		}


		/// <summary>
		/// 得到指定查询条件下的积分交易日志数
		/// </summary>
		/// <param name="condition">查询条件</param>
		/// <returns></returns>
		public static int RecordCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetPaymentLogListCount(condition);
		}
	}
}
