using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminForumStatFactory 的摘要说明。
	/// 后台论坛统计功能类
	/// </summary>
	public class AdminForumStats
	{
		/// <summary>
		/// 得到论坛中用户总数
		/// </summary>
		/// <returns>用户总数</returns>
		public static int GetUserCount()
		{
			return Users.GetUserCount();
		}


		/// <summary>
		/// 得到论坛中帖子总数;
		/// </summary>
		/// <returns>帖子总数</returns>
		public static int GetPostsCount()
		{
			int postcount = 0;
			foreach (DataRow dr in Posts.GetAllPostTableName().Rows)
			{
                postcount += DatabaseProvider.GetInstance().GetPostsCount(dr["id"].ToString());
			}
			return postcount;
		}

		/// <summary>
		/// 得到论坛中帖子总数;
		/// </summary>
		/// <param name="tid">主题ID</param>
		/// <returns>帖子总数</returns>
		public static int GetPostsCountByTid(int tid)
		{
            return DatabaseProvider.GetInstance().GetPostsCount(Posts.GetPostTableID(tid));
            
        }

		/// <summary>
		/// 得到论坛中帖子总数;
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <param name="todaypostcount">输出参数,根据返回指定版块今日发帖总数</param>
		/// <returns>帖子总数</returns>
		public static int GetPostsCountByFid(int fid, out int todaypostcount)
		{
			int postcount = 0;
			todaypostcount = 0;

			///得到指定版块的最大和最小主题ID
			int maxtid = 0;
			int mintid = 0;
            IDataReader readerGetTid = DatabaseProvider.GetInstance().GetMaxAndMinTid(fid);
			if (readerGetTid != null)
			{
				if (readerGetTid.Read())
				{
					maxtid = Utils.StrToInt(readerGetTid["maxtid"], 0);
					mintid = Utils.StrToInt(readerGetTid["mintid"], 0);
				}

				readerGetTid.Close();
			}

			if (mintid + maxtid == 0)
			{
                postcount = DatabaseProvider.GetInstance().GetPostCount(fid, Posts.GetPostTableName());
                todaypostcount = DatabaseProvider.GetInstance().GetTodayPostCount(fid, Posts.GetPostTableName());
			}
			else
			{
				string[] posttableidA = Posts.GetPostTableIDList(mintid, maxtid);
				foreach (string posttableid in posttableidA)
				{
                    postcount = postcount + DatabaseProvider.GetInstance().GetPostCount(fid, Utils.StrToInt(posttableid, 0));
                    todaypostcount = todaypostcount + DatabaseProvider.GetInstance().GetTodayPostCount(fid, Utils.StrToInt(posttableid, 0));

				}
			}
			return postcount;
		}

		/// <summary>
		/// 返回指定版块的发帖总数
		/// </summary>
		/// <param name="fid">指定的版块id</param>
		/// <returns></returns>
		public static int GetPostsCountByFid(int fid)
		{
			int todaypostcount = 0;
			return GetPostsCountByFid(fid, out todaypostcount);
		}

		/// <summary>
		/// 得到论坛中帖子总数;
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="todaypostcount">输出参数,根据返回指定版块今日发帖总数</param>
		/// <returns>帖子总数</returns>
		public static int GetPostsCountByUid(int uid, out int todaypostcount)
		{
			int postcount = 0;
			todaypostcount = 0;

			
			///得到指定版块的最大和最小主题ID
			///
			int maxtid = 0;
			int mintid = 0;
            IDataReader readerGetTid = DatabaseProvider.GetInstance().GetMaxAndMinTidByUid(uid);
            if (readerGetTid != null)
			{
				if (readerGetTid.Read())
				{
					maxtid = Utils.StrToInt(readerGetTid["maxtid"], 0);
					mintid = Utils.StrToInt(readerGetTid["mintid"], 0);
				}

				readerGetTid.Close();
			}

			if (mintid + maxtid == 0)
			{
                postcount = DatabaseProvider.GetInstance().GetPostCountByUid(uid, Posts.GetPostTableName());
                todaypostcount = DatabaseProvider.GetInstance().GetTodayPostCountByUid(uid, Posts.GetPostTableName());
			}
			else
			{
				string[] posttableidA = Posts.GetPostTableIDList(mintid, maxtid);
				foreach (string posttableid in posttableidA)
				{
                    postcount = postcount + DatabaseProvider.GetInstance().GetPostCountByUid(uid, BaseConfigs.GetTablePrefix + "posts" + posttableid);
                    todaypostcount = todaypostcount + DatabaseProvider.GetInstance().GetTodayPostCountByUid(uid, BaseConfigs.GetTablePrefix + "posts" + posttableid);
				}
			}
			return postcount;
		}

		/// <summary>
		/// 返回指定版块的发帖总数
		/// </summary>
		/// <param name="uid">指定用户id</param>
		/// <returns></returns>
		public static int GetPostsCountByUid(int uid)
		{
			int todaypostcount = 0;
			return GetPostsCountByUid(uid, out todaypostcount);
		}


		/// <summary>
		/// 得到论坛中主题总数;
		/// </summary>
		/// <returns>主题总数</returns>
		public static int GetTopicsCount()
		{
            return DatabaseProvider.GetInstance().GetTopicsCount();
		}


		/// <summary>
		/// 得到论坛中最后注册的用户ID和用户名
		/// </summary>
		/// <param name="lastuserid">输出参数：最后注册的用户ID</param>
		/// <param name="lastusername">输出参数：最后注册的用户名</param>
		/// <returns>存在返回true,不存在返回false</returns>
		public static bool GetLastUserInfo(out string lastuserid, out string lastusername)
		{
            return DatabaseProvider.GetInstance().GetLastUserInfo(out lastuserid, out lastusername);
		}

		/// <summary>
		/// 重设论坛统计数据
		/// </summary>
		public static void ReSetStatistic()
		{
			int UserCount = GetUserCount();
			int TopicsCount = GetTopicsCount();
			int PostCount = GetPostsCount();
			string lastuserid;
			string lastusername;

			if (!GetLastUserInfo(out lastuserid, out lastusername))
			{
				lastuserid = "";
				lastusername = "";
			}

            DatabaseProvider.GetInstance().ReSetStatistic(UserCount, TopicsCount, PostCount, lastuserid, lastusername);

		}


		/// <summary>
		/// 重设置用户精华帖数
		/// </summary>
		/// <param name="statcount">要设置的用户数量</param>
		/// <param name="lastuid">输出参数: 最后一个用户ID</param>
		public static void ReSetUserDigestPosts(int statcount, ref int lastuid)
		{
			if (statcount < 1)
			{
				lastuid = -1;
				return;
			}

            IDataReader reader = DatabaseProvider.GetInstance().GetTopUsers(statcount, lastuid);

			lastuid = -1;

			if (reader != null)
			{
				while (reader.Read())
				{
					lastuid = Utils.StrToInt(reader["uid"], -1);

                    DatabaseProvider.GetInstance().ResetUserDigestPosts(lastuid);

				}
				reader.Close();
			}

		}

		/// <summary>
		/// 重设置用户精华帖数
		/// </summary>
		/// <param name="start_uid">开始统计的用户id </param>
		/// <param name="end_uid">结束统计时的用 id </param>
		public static void ReSetUserDigestPosts(int start_uid, int end_uid)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetUsers(start_uid, end_uid);

			int current_uid = start_uid;

			if (reader != null)
			{
				while (reader.Read())
				{
					current_uid = Utils.StrToInt(reader["uid"], -1);

                    DatabaseProvider.GetInstance().ResetUserDigestPosts(current_uid);
				}
				reader.Close();
			}
		}


		/// <summary>
		/// 重设置用户发帖数
		/// </summary>
		/// <param name="statcount">要设置的用户数量</param>
		/// <param name="lastuid">输出参数：最后一个用户ID</param>
		public static void ReSetUserPosts(int statcount, ref int lastuid)
		{
			if (statcount < 1)
			{
				lastuid = -1;
				return;
			}

            IDataReader reader = DatabaseProvider.GetInstance().GetTopUsers(statcount, lastuid);
			lastuid = -1;

			if (reader != null)
			{
				while (reader.Read())
				{
					lastuid = Utils.StrToInt(reader["uid"], -1);

					int postcount = GetPostsCountByUid(lastuid);

                    DatabaseProvider.GetInstance().UpdateUserPostCount(postcount, lastuid);	
                }
				reader.Close();
			}
		}


		/// <summary>
		/// 重设置指定uid范围的用户发帖数
		/// </summary>
		/// <param name="start_uid">开始统计的用户id </param>
		/// <param name="end_uid">结束统计时的用 id </param>
		public static void ReSetUserPosts(int start_uid, int end_uid)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetUsers(start_uid, end_uid);

			int current_uid = start_uid;

			if (reader != null)
			{
				while (reader.Read())
				{
					current_uid = Utils.StrToInt(reader["uid"], -1);

					int postcount = GetPostsCountByUid(current_uid);

                    DatabaseProvider.GetInstance().UpdateUserPostCount(postcount, current_uid);					
				}
				reader.Close();
			}
		}


		/// <summary>
		/// 重建主题帖数
		/// </summary>
		/// <param name="statcount">要设置的主题数量</param>
		/// <param name="lasttid">输出参数：最后一个主题ID</param>
		public static void ReSetTopicPosts(int statcount, ref int lasttid)
		{
			if (statcount < 1)
			{
				lasttid = -1;
				return;
			}

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicTids(statcount, lasttid);
			lasttid = -1;

			if (reader != null)
			{
				int postcount = 0;

				string posttableid = "";
				while (reader.Read())
				{
					lasttid = Utils.StrToInt(reader["tid"], -1);
					posttableid = Posts.GetPostTableID(lasttid);

					postcount = Posts.GetPostCount(lasttid);

                    IDataReader readerTopic = DatabaseProvider.GetInstance().GetLastPost(lasttid, Utils.StrToInt(posttableid,0));
					if (readerTopic != null)
					{
						if (readerTopic.Read())
						{
							if (Utils.StrToInt(readerTopic["pid"], 0) != 0)
							{
                                DatabaseProvider.GetInstance().UpdateTopic(lasttid, postcount, Utils.StrToInt(readerTopic["pid"], 0), readerTopic["postdatetime"].ToString(), Utils.StrToInt(readerTopic["posterid"], 0), readerTopic["poster"].ToString());
							}
							else
							{
                                DatabaseProvider.GetInstance().UpdateTopicLastPosterId(lasttid);								
                            }
						}
                        readerTopic.Close();
					}
				}
				reader.Close();
			}
		}


		/// <summary>
		/// 重建主题帖数
		/// </summary>
		/// <param name="start_tid">开始统计的主题id </param>
		/// <param name="end_tid">结束统计时的主题id </param>
		public static void ReSetTopicPosts(int start_tid, int end_tid)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetTopics(start_tid, end_tid);
			int current_tid = start_tid;

			if (reader != null)
			{
				int postcount = 0;
				string posttableid = "";
				while (reader.Read())
				{
					current_tid = Utils.StrToInt(reader["tid"], -1);
					posttableid = Posts.GetPostTableID(current_tid);

					postcount = Posts.GetPostCount(current_tid);

                    IDataReader readerTopic = DatabaseProvider.GetInstance().GetLastPost(current_tid, Utils.StrToInt(posttableid, 0));
                    if (readerTopic != null)
					{
						if (readerTopic.Read())
						{
							if (Utils.StrToInt(readerTopic["pid"], 0) != 0)
							{
                                DatabaseProvider.GetInstance().UpdateTopic(current_tid, postcount, Utils.StrToInt(readerTopic["pid"], 0), readerTopic["postdatetime"].ToString(), Utils.StrToInt(readerTopic["posterid"], 0), readerTopic["poster"].ToString());								
                            }
							else
							{
                                DatabaseProvider.GetInstance().UpdateTopicLastPosterId(current_tid);
							}
						}
						readerTopic.Close();
					}
				}
				reader.Close();
			}
		}


		/// <summary>
		/// 重建论坛帖数
		/// </summary>
		/// <param name="statcount">要设置的版块数量</param>
		/// <param name="lastfid">输出参数：最后一个版块ID</param>
		public static void ReSetFourmTopicAPost(int statcount, ref int lastfid)
		{
			if (statcount < 1)
			{
				lastfid = -1;
				return;
			}


			Forums.SetRealCurrentTopics(lastfid);


            IDataReader reader = DatabaseProvider.GetInstance().GetTopForumFids(lastfid, statcount);
			lastfid = -1;
			if (reader != null)
			{
				int topiccount = 0;
				int postcount = 0;
                int todaypostcount = 0;

				while (reader.Read())
				{
					lastfid = Utils.StrToInt(reader["fid"], -1);

					topiccount = Topics.GetAllTopicCount(lastfid);

					postcount = GetPostsCountByFid(lastfid, out todaypostcount);
                    int lasttid = 0;
                    string lasttitle = "";
                    string lastpost = "1900-1-1";
                    int lastposterid = 0;
                    string lastposter = "";

                    IDataReader postreader = DatabaseProvider.GetInstance().GetForumLastPost(lastfid, Posts.GetPostTableName(), topiccount, postcount, 0, "", "1900-1-1", 0, "", todaypostcount);
					if (postreader.Read())
					{
                        lasttid = Utils.StrToInt(postreader["tid"], 0);
                        lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
                        lastpost = postreader["postdatetime"].ToString();
                        lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                        lastposter = postreader["poster"].ToString();
					}
					postreader.Close();

                    DatabaseProvider.GetInstance().UpdateForum(lastfid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
			    }
				reader.Close();
			}
		}


		/// <summary>
		/// 重建论坛帖数
		/// </summary>
		/// <param name="start_fid">要设置的起始版块</param>
        /// <param name="end_fid">要设置的终止版块</param>
		public static void ReSetFourmTopicAPost(int start_fid, int end_fid)
		{
            IDataReader reader = DatabaseProvider.GetInstance().GetForums(start_fid, end_fid);
			int current_fid = start_fid;
			if (reader != null)
			{
				int topiccount = 0;
				int postcount = 0;
				int todaypostcount = 0;
                int lasttid = 0;
                string lasttitle = "";
                string lastpost = "";
                int lastposterid = 0;
                string lastposter = "";

				while (reader.Read())
				{
					current_fid = Utils.StrToInt(reader["fid"], -1);

					Forums.SetRealCurrentTopics(current_fid);

					topiccount = Topics.GetAllTopicCount(current_fid);

					postcount = GetPostsCountByFid(current_fid, out todaypostcount);

                    IDataReader postreader = DatabaseProvider.GetInstance().GetForumLastPost(current_fid, Posts.GetPostTableName(), topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);

                    if (postreader.Read())
                    {
                        lasttid = Utils.StrToInt(postreader["tid"], 0);
                        lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
                        lastpost = postreader["postdatetime"].ToString();
                        lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                        lastposter = postreader["poster"].ToString();
                        DatabaseProvider.GetInstance().UpdateForum(current_fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);	
                    }
                    postreader.Close();	
				}
				reader.Close();
			}
		}

		/// <summary>
		/// 重建指定论坛帖数
		/// </summary>
		public static void ReSetFourmTopicAPost(int fid)
		{
			if (fid < 1)
			{
				return;
			}

            int topiccount = 0;
            int postcount = 0;
            int lasttid = 0;
            string lasttitle = "";
            string lastpost = "1900-1-1";
            int lastposterid = 0;
            string lastposter = "";
            int todaypostcount = 0;
			topiccount = Topics.GetAllTopicCount(fid);

			postcount = GetPostsCountByFid(fid, out todaypostcount);

            IDataReader postreader = DatabaseProvider.GetInstance().GetLastPostByFid(fid, Posts.GetPostTableName());
			
			if (postreader.Read())
			{
                lasttid = Utils.StrToInt(postreader["tid"], 0);
                lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
                lastpost = postreader["postdatetime"].ToString();
                lastposterid = Utils.StrToInt(postreader["posterid"], 0);
                lastposter = postreader["poster"].ToString();
			}

			postreader.Close();

            DatabaseProvider.GetInstance().UpdateForum(fid, topiccount, postcount, lasttid, lasttitle, lastpost, lastposterid, lastposter, todaypostcount);
		}

		/// <summary>
		/// 重建指定论坛帖数
		/// </summary>
		public static void ReSetFourmTopicAPost(int fid, out int topiccount, out int postcount, out int lasttid, out string lasttitle, out string lastpost, out int lastposterid, out string lastposter, out int todaypostcount)
		{
			topiccount = 0;
			postcount = 0;
			lasttid = 0;
			lasttitle = "";
			lastpost = "";
			lastposterid = 0;
			lastposter = "";
			todaypostcount = 0;
			if (fid < 1)
			{
				return;
			}

			topiccount = Topics.GetAllTopicCount(fid);
			postcount = GetPostsCountByFid(fid, out todaypostcount);

            IDataReader postreader = DatabaseProvider.GetInstance().GetLastPostByFid(fid, Posts.GetPostTableName());
			if (postreader.Read())
			{
				lasttid = Utils.StrToInt(postreader["tid"], 0);
                lasttitle = Topics.GetTopicInfo(lasttid).Title;//postreader["title"].ToString();
				lastpost = postreader["postdatetime"].ToString();
				lastposterid = Utils.StrToInt(postreader["posterid"], 0);
				lastposter = postreader["poster"].ToString();
			}

			postreader.Close();

		}


		/// <summary>
		/// 清除主题里面已经移走的主题
		/// </summary>
		public static void ReSetClearMove()
		{
            DatabaseProvider.GetInstance().ReSetClearMove();
		}

	}
}