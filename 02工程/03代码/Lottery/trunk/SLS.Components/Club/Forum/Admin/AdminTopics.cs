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
	/// AdminTopicFactory 的摘要说明。
	/// 后台主题管理操作类
	/// </summary>
	public class AdminTopics : TopicAdmins
	{
		public AdminTopics()
		{
		}


		/// <summary>
		/// 更新主题信息
		/// </summary>
		/// <param name="topicinfo"></param>
		/// <returns></returns>
		public static bool UpdateTopicAllInfo(TopicInfo topicinfo)
		{
            return DatabaseProvider.GetInstance().UpdateTopicAllInfo(topicinfo);
		}


		/// <summary>
		/// 根据主题ID删除相应的主题信息
		/// </summary>
		/// <param name="tid"></param>
		/// <returns></returns>
		public static bool DeleteTopicByTid(int tid)
		{
            return DatabaseProvider.GetInstance().DeleteTopicByTid(tid, Posts.GetPostTableName());

		}


		public static bool SetTypeid(string topiclist, int value)
		{
            return DatabaseProvider.GetInstance().SetTypeid(topiclist, value);
		}


		public static bool SetDisplayorder(string topiclist, int value)
		{
            return DatabaseProvider.GetInstance().SetDisplayorder(topiclist, value);

		}


		/// <summary>
		/// 得到指定分表tid的表记录
		/// </summary>
		/// <param name="tid"></param>
		/// <param name="pagesize"></param>
		/// <param name="pageindex"></param>
		/// <returns></returns>
		public static DataSet AdminGetPostList(int tid, int pagesize, int pageindex)
		{
            DataSet ds = DatabaseProvider.GetInstance().GetPosts(tid, pagesize, pageindex, Posts.GetPostTableName(tid));

			if (ds == null)
			{
				ds = new DataSet();
				ds.Tables.Add("post");
				ds.Tables.Add();
				return ds;
			}

			ds.Tables[0].TableName = "post";

			foreach (DataRow dr in ds.Tables[0].Rows)
			{
				// 处理用户头衔

				if (dr["attachment"].ToString().Equals("1"))
				{
                    dr["attachment"] = DatabaseProvider.GetInstance().GetAttachCount(Utils.StrToInt(dr["pid"], 0));
                }

			}
			return ds;
		}


       

	}
}