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
	/// 交易日志操作类
	/// </summary>
	public class PaymentLogs
	{

		/// <summary>
		/// 购买主题
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="tid">主题id</param>
		/// <param name="posterid">发帖者用户id</param>
		/// <param name="price">价格</param>
		/// <param name="netamount"></param>
		/// <returns></returns>
		public static int BuyTopic(int uid, int tid, int posterid, int price, float netamount)
		{

			int tmpprice = price;
            if (price > Scoresets.GetMaxIncPerTopic())
			{
                tmpprice = Scoresets.GetMaxIncPerTopic();
			}

			

			IDataReader  reader = Users.GetShortUserInfoToReader(uid);
			if (reader == null)
			{
				return -2;
			}

			if (!reader.Read())
			{
				reader.Close();
				return -2;
			}

            if (Utils.StrToFloat(reader["extcredits" + Scoresets.GetCreditsTrans().ToString()], 0) < price)
			{
				reader.Close();
				return -1;
			}
			reader.Close();

            DatabaseProvider.GetInstance().BuyTopic(uid, tid, posterid, price, netamount, Scoresets.GetCreditsTrans());
            UserCredits.UpdateUserCredits(uid);
			UserCredits.UpdateUserCredits(posterid);
            return DatabaseProvider.GetInstance().AddPaymentLog(uid, tid, posterid, price, netamount);
			
		}

		/// <summary>
		/// 判断用户是否已购买主题
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static bool IsBuyer(int tid, int uid)
		{
            return DatabaseProvider.GetInstance().IsBuyer(tid, uid);
		}


		/// <summary>
		/// 获取指定用户的交易日志
		/// </summary>
		/// <param name="pagesize">每页条数</param>
		/// <param name="currentpage">当前页</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static DataTable GetPayLogInList(int pagesize,int currentpage , int uid)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetPayLogInList(pagesize, currentpage, uid);
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
					if(dr["fid"].ToString().Trim() != "")
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
		/// 获取指定用户的收入日志记录数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static int GetPaymentLogInRecordCount(int uid)
		{
            return DatabaseProvider.GetInstance().GetPaymentLogInRecordCount(uid);	
        }

		/// <summary>
		/// 返回指定用户的支出日志记录数
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页</param>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static DataTable GetPayLogOutList(int pagesize,int currentpage , int uid)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetPayLogOutList(pagesize, currentpage, uid);
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
					if(dr["fid"].ToString().Trim() != "")
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
		/// 返回指定用户支出日志总数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns></returns>
		public static int GetPaymentLogOutRecordCount(int uid)
		{
            return DatabaseProvider.GetInstance().GetPaymentLogOutRecordCount(uid);	
        }

		/// <summary>
		/// 获取指定主题的购买记录
		/// </summary>
		/// <param name="pagesize">每页记录数</param>
		/// <param name="currentpage">当前页数</param>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public static DataTable GetPaymentLogByTid(int pagesize,int currentpage , int tid)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetPaymentLogByTid(pagesize, currentpage, tid);
			if (dt==null)
			{
				dt=new DataTable();
			}
			return dt;
		}

		/// <summary>
		/// 主题购买总次数
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns></returns>
		public static int GetPaymentLogByTidCount(int tid)
		{
            return DatabaseProvider.GetInstance().GetPaymentLogByTidCount(tid);	
        }


	}
}
