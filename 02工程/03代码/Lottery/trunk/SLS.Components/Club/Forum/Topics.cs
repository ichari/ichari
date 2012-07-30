using System;
using System.Data;
using System.Data.Common;

using System.Text;
using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using System.IO;
using Discuz.Common.Generic;
using System.Text.RegularExpressions;

namespace Discuz.Forum
{
    public enum MagicType
    { 
        //顺序按此排列
        /// <summary>
        /// html标题 占位1
        /// </summary>
        HtmlTitle = 1,
        /// <summary>
        /// 魔法主题 占位3
        /// </summary>
        MagicTopic = 2,    
        /// <summary>
        /// 标签, 占位1
        /// </summary>
        TopicTag = 3,


    }
	/// <summary>
	/// 主题操作类
	/// </summary>
	public class Topics
	{
	    	
		//private static object syncObj = new object();


		/// <summary>
		/// 按照用户Id获取其回复过的主题总数
		/// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>回复过的主题总数</returns>
		public static int GetTopicsCountbyReplyUserId(int userId)
		{
            return DatabaseProvider.GetInstance().GetTopicsCountbyReplyUserId(userId);	
        }

	

		/// <summary>
		/// 按照用户Id获取主题总数
		/// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>主题总数</returns>
		public static int GetTopicsCountbyUserId(int userId)
		{
            return DatabaseProvider.GetInstance().GetTopicsCountbyUserId(userId);	
        }


     
		/// <summary>
		/// 创建新主题
		/// </summary>
		/// <param name="topicinfo">主题信息</param>
		/// <returns>返回主题ID</returns>
		public static int CreateTopic(TopicInfo topicinfo)
		{
            return DatabaseProvider.GetInstance().CreateTopic(topicinfo);
        }

		
		/// <summary>
		/// 增加父版块的主题数
		/// </summary>
		/// <param name="fpidlist">父板块id列表</param>
		/// <param name="topics">主题数</param>
		/// <param name="posts">贴子数</param>
		public static void AddParentForumTopics(string fpidlist, int topics, int posts)
		{
            if (fpidlist != "")
            {
                DatabaseProvider.GetInstance().AddParentForumTopics(fpidlist, topics, posts);
            }
        }

		/// <summary>
		/// 获得主题信息
		/// </summary>
		/// <param name="tid">要获得的主题ID</param>
		public static TopicInfo GetTopicInfo(int tid)
		{
			return GetTopicInfo(tid, 0, 0);
		}
		
		/// <summary>
		/// 获得主题信息
		/// </summary>
		/// <param name="tid">要获得的主题ID</param>
		/// <param name="tid">版块ID</param>
		/// <param name="mode">模式选择, 0=当前主题, 1=上一主题, 2=下一主题</param>
		public static TopicInfo GetTopicInfo(int tid, int fid, byte mode)
		{
			TopicInfo topicinfo = new TopicInfo();

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicInfo(tid, fid, mode);
			if (reader.Read())
			{
				topicinfo.Tid = Int32.Parse(reader["tid"].ToString());
				topicinfo.Fid = Int32.Parse(reader["fid"].ToString());
				topicinfo.Iconid = Int32.Parse(reader["iconid"].ToString());
				topicinfo.Title = reader["title"].ToString();
				topicinfo.Typeid = Int32.Parse(reader["typeid"].ToString());
				topicinfo.Readperm = Int32.Parse(reader["readperm"].ToString());
				topicinfo.Price = Int32.Parse(reader["price"].ToString());
				topicinfo.Poster = reader["poster"].ToString();
				topicinfo.Posterid = Int32.Parse(reader["posterid"].ToString());
				topicinfo.Postdatetime = reader["postdatetime"].ToString();
				topicinfo.Lastpost = reader["lastpost"].ToString();
				topicinfo.Lastposter = reader["lastposter"].ToString();
				topicinfo.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
				topicinfo.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
				topicinfo.Views = Int32.Parse(reader["views"].ToString());
				topicinfo.Replies = Int32.Parse(reader["replies"].ToString());
				topicinfo.Displayorder = Int32.Parse(reader["displayorder"].ToString());
				topicinfo.Highlight = reader["highlight"].ToString();
				topicinfo.Digest = Int32.Parse(reader["digest"].ToString());
				topicinfo.Rate = Int32.Parse(reader["rate"].ToString());
				topicinfo.Hide = Int32.Parse(reader["hide"].ToString());
				//topicinfo.Poll = Int32.Parse(reader["poll"].ToString());
				topicinfo.Attachment = Int32.Parse(reader["attachment"].ToString());
				topicinfo.Moderated = Int32.Parse(reader["moderated"].ToString());
				topicinfo.Closed = Int32.Parse(reader["closed"].ToString());
				topicinfo.Magic = Int32.Parse(reader["magic"].ToString());
                topicinfo.Identify = Int32.Parse(reader["identify"].ToString());
                topicinfo.Special = byte.Parse(reader["special"].ToString());
				reader.Close();
				return topicinfo;
			}
			else
			{
				reader.Close();
				return null;
			}
		}


       
        /// <summary>
		/// 获得主题列表
		/// </summary>
		/// <param name="topiclist">主题id列表</param>
		/// <returns>主题列表</returns>
		public static DataTable GetTopicList(string topiclist)
		{
			return GetTopicList(topiclist, -10);
		}

		/// <summary>
		/// 获得指定的主题列表
		/// </summary>
		/// <param name="topiclist">主题ID列表</param>
		/// <param name="displayorder">order的下限( WHERE [displayorder]>此值)</param>
        /// <returns>主题列表</returns>
		public static DataTable GetTopicList(string topiclist, int displayorder)
		{
			if (!Utils.IsNumericArray(topiclist.Split(',')))
			{
				return null;
			}
            return DatabaseProvider.GetInstance().GetTopicList(topiclist, displayorder);
		}

		/// <summary>
		/// 得到置顶主题信息
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>置顶主题</returns>
		public static DataRow GetTopTopicListID(int fid)
		{
			DataRow dr = null;
			string xmlPath = AppDomain.CurrentDomain.BaseDirectory + "cache/topic/" + fid.ToString() + ".xml";
			if (Utils.FileExists(xmlPath))
			{
				//xmlPath=Utils.GetMapPath(xmlPath);
				DataSet ds = new DataSet();
				ds.ReadXml(@xmlPath,XmlReadMode.ReadSchema);
				if (ds.Tables.Count > 0)
				{
					if (ds.Tables[0].Rows.Count > 0)
					{
						dr = ds.Tables[0].Rows[0];
						if (Utils.CutString(dr["tid"].ToString(), 0, 1).Equals(","))
						{
							dr["tid"] = Utils.CutString(dr["tid"].ToString(), 1);
						}
					}
				}
				
				ds.Dispose();

			}
			return dr;
		}


		/// <summary>
		/// 列新主题的回复数
		/// </summary>
		/// <param name="tid">主题ID</param>
		public static void UpdateTopicReplies(int tid)
		{
            DatabaseProvider.GetInstance().UpdateTopicReplies(tid, Posts.GetPostTableID(tid));
		}


		/// <summary>
		/// 更新主题显示数
		/// </summary>
		/// <param name="tid">主题ID</param>
//		public static void UpdateTopicViews(int tid)
//		{
//			DbParameter[] parms = {
//									   DbHelper.MakeInParam("@tid",(DbType)(DbType)SqlDbType.Int,4,tid)								   
//								   };
//			DbHelper.ExecuteDataset(CommandType.Text, "UPDATE [" + BaseConfigs.GetTablePrefix + "topics] SET [views]= [views] + 1 WHERE [tid]=@tid", parms);
////			lock (syncObj)
////			{
////				string filePath=Utils.GetMapPath(BaseConfigs.GetForumPath + "topic/views.config");
////				if (Utils.FileExists(filePath))
////				{
////					using (StreamWriter sw = File.AppendText(filePath)) 
////					{
////						sw.WriteLine(tid);
////					}
////				}
////			}
//
//		}
//
		/// <summary>
		/// 得到当前版块内正常(未关闭)主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
		public static int GetTopicCount(int fid)
		{
            return DatabaseProvider.GetInstance().GetTopicCount(fid);
        }

		/// <summary>
		/// 得到当前版块内(包括子版)正常(未关闭)主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
		public static int GetAllTopicCount(int fid)
		{
            return DatabaseProvider.GetInstance().GetAllTopicCount(fid);	
        }

		/// <summary>
		/// 得到当前版块内正常(未关闭)主题总数
		/// </summary>
		/// <param name="fid">版块ID</param>
		/// <returns>主题总数</returns>
        public static int GetTopicCount(int fid, string condition)
		{
            return GetTopicCount(fid, condition, false);	
        }

        /// <summary>
        /// 得到当前版块内主题总数
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <returns>主题总数</returns>
        public static int GetTopicCount(int fid, string condition, bool includeclosedtopic)
        {
            return DatabaseProvider.GetInstance().GetTopicCount(fid, includeclosedtopic ? -1 : 0, condition);
        }

		/// <summary>
		/// 得到符合条件的主题总数
		/// </summary>
		/// <param name="condition">条件</param>
		/// <returns>主题总数</returns>
		public static int GetTopicCount(string condition)
		{
            return DatabaseProvider.GetInstance().GetTopicCount(condition);	
        }


		/// <summary>
		/// 更新主题标题
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="topictitle">新标题</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicTitle(int tid, string topictitle)
		{
            return DatabaseProvider.GetInstance().UpdateTopicTitle(tid, topictitle);	
        }

		/// <summary>
		/// 更新主题图标id
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="iconid">主题图标id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicIconID(int tid, int iconid)
		{
            return DatabaseProvider.GetInstance().UpdateTopicIconID(tid, iconid);
        }

		/// <summary>
		/// 更新主题价格
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicPrice(int tid)
		{
            return DatabaseProvider.GetInstance().UpdateTopicPrice(tid);	
        }

		/// <summary>
		/// 更新主题价格
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="price">价格</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicPrice(int tid,int price)
		{
            return DatabaseProvider.GetInstance().UpdateTopicPrice(tid, price);	
        }

		/// <summary>
		/// 更新主题阅读权限
		/// </summary>
		/// <param name="tid">主题id</param>
		/// <param name="readperm">阅读权限</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicReadperm(int tid,int readperm)
		{
            return DatabaseProvider.GetInstance().UpdateTopicReadperm(tid, readperm);	
        }

		/// <summary>
		/// 更新主题为已被管理
		/// </summary>
		/// <param name="topiclist">主题id列表</param>
		/// <param name="moderated">管理操作id</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopicModerated(string topiclist,int moderated)
		{
			if (!Utils.IsNumericArray(Utils.SplitString(topiclist,",")))
			{
				return 0;
			}

            return DatabaseProvider.GetInstance().UpdateTopicModerated(topiclist, moderated);
		}

		/// <summary>
		/// 更新主题
		/// </summary>
		/// <param name="topicinfo">主题信息</param>
		/// <returns>成功返回1，否则返回0</returns>
		public static int UpdateTopic(TopicInfo topicinfo)
		{
            return DatabaseProvider.GetInstance().UpdateTopic(topicinfo);
		}

		/// <summary>
		/// 判断帖子列表是否都在当前板块
		/// </summary>
		/// <param name="topicidlist">主题序列</param>
		/// <param name="fid">板块ID</param>
		/// <returns>是则返回TREU,反则FLASH</returns>
		public static bool InSameForum(string topicidlist, int fid)
		{
			if (!Utils.IsNumericArray(Utils.SplitString(topicidlist, ",")))
			{
				return false;
			}
            return DatabaseProvider.GetInstance().InSameForum(topicidlist, fid);	
        }

		/// <summary>
		/// 将主题设置为隐藏主题
		/// </summary>
		/// <param name="tid">主题ID</param>
		/// <returns></returns>
		public static int UpdateTopicHide(int tid)
		{
            return DatabaseProvider.GetInstance().UpdateTopicHide(tid);
		}

	

        /// <summary>
        /// 获得主题列表
        /// </summary>
        /// <param name="forumid">板块ID</param>
        /// <param name="pageid">当前页数</param>
        /// <param name="tpp">每页主题数</param>
        /// <returns>主题列表</returns>
        public static DataTable GetTopicList(int forumid, int pageid, int tpp)
        {
            return DatabaseProvider.GetInstance().GetTopicList(forumid, pageid, tpp);
        }


#if NET1

        #region 主题集合类操作

        //将得到的主题列表中加入主题类型名称字段
        public static DataTable GetTopicTypeName(DataTable __topiclist)
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = "topictypename";
            dc.DataType = Type.GetType("System.String");
            dc.DefaultValue = "";
            dc.AllowDBNull = true;
            __topiclist.Columns.Add(dc);

            System.Collections.SortedList __topictypearray = Caches.GetTopicTypeArray();
            object typictypename = null;
            foreach (DataRow dr in __topiclist.Rows)
            {
                typictypename = __topictypearray[Int32.Parse(dr["typeid"].ToString())];
                dr["topictypename"] = (typictypename != null && typictypename.ToString().Trim() != "") ? "[" + typictypename.ToString().Trim() + "]" : "";
            }
            return __topiclist;
        }

        /// <summary>
        /// 按照用户Id获取其回复过的主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static MyTopicInfoCollection GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize, int newmin, int hot)
        {
            MyTopicInfoCollection coll = new MyTopicInfoCollection();

            if (pageIndex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByReplyUserId(userId, pageIndex, pageSize);
            if(reader != null)
            {
                while (reader.Read())
                {
                    MyTopicInfo topic = LoadSingleMyTopic(newmin, hot, reader);

                    coll.Add(topic);
                }
                reader.Close();
            }

            return coll;
        }

        /// <summary>
        /// 按照用户Id获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static MyTopicInfoCollection GetTopicsByUserId(int userId, int pageIndex, int pageSize, int newmin, int hot)
        {
            MyTopicInfoCollection coll = new MyTopicInfoCollection();

            if (pageIndex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByUserId(userId, pageIndex, pageSize);

            if(reader != null)
            {
                while (reader.Read())
                {
                    MyTopicInfo topic = LoadSingleMyTopic(newmin, hot, reader);

                    coll.Add(topic);
                }
                reader.Close();
            }

            return coll;
        }



        /// <summary>
        /// 获得置顶主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="pagesize">每页显示主题数</param>
        /// <param name="pageindex">当前页数</param>
        /// <param name="tids">主题id列表</param>
        /// <returns>主题信息列表</returns>
        public static ShowforumPageTopicInfoCollection GetTopTopicCollection(int fid, int pagesize, int pageindex, string tids, int autoclose, int topictypeprefix)
        {
            ShowforumPageTopicInfoCollection coll = new ShowforumPageTopicInfoCollection();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopTopics(fid, pagesize, pageindex, tids);
            if (reader != null)
            {

                System.Collections.SortedList TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.SortAdd(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }

            return coll;


        }

        /// <summary>
        /// 获得一般主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="condition">条件</param>
        /// <param name="pagesize">每页显示主题数</param>
        /// <param name="pageindex">当前页数</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <returns>主题信息列表</returns>
        public static ShowforumPageTopicInfoCollection GetTopicCollection(int fid, int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition)
        {
            ShowforumPageTopicInfoCollection coll = new ShowforumPageTopicInfoCollection();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopics(fid, pagesize, pageindex, startnum, condition);
            if (reader != null)
            {

                System.Collections.SortedList TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;

        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="ascdesc">升/降序</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection GetTopicCollection(int fid, int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, string orderby, int ascdesc)
        {
            ShowforumPageTopicInfoCollection coll = new ShowforumPageTopicInfoCollection();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByDate(fid, pagesize, pageindex, startnum, condition, orderby, ascdesc);
            if (reader != null)
            {

                System.Collections.SortedList TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }

        /// <summary>
        /// 对符合新贴,精华贴的页面显示进行查询的函数
        /// </summary>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection GetTopicCollectionByType(int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, int ascdesc)
        {
            ShowforumPageTopicInfoCollection coll = new ShowforumPageTopicInfoCollection();

            if (pageindex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByType(pagesize, pageindex, startnum, condition, ascdesc);
            if (reader != null)
            {

                System.Collections.SortedList TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }

		private static MyTopicInfo LoadSingleMyTopic(int newmin, int hot, IDataReader reader)
		{
			MyTopicInfo topic = new MyTopicInfo();
			topic.Tid = Int32.Parse(reader["tid"].ToString());
			topic.Fid = Int32.Parse(reader["fid"].ToString());
			topic.Iconid = Int32.Parse(reader["iconid"].ToString());
			topic.Forumname = Forums.GetForumInfo(topic.Fid).Name;
			topic.Title = reader["title"].ToString();
			topic.Typeid = Int32.Parse(reader["typeid"].ToString());
			topic.Readperm = Int32.Parse(reader["readperm"].ToString());
			topic.Price = Int32.Parse(reader["price"].ToString());
			topic.Poster = reader["poster"].ToString();
			topic.Posterid = Int32.Parse(reader["posterid"].ToString());
			topic.Postdatetime = reader["postdatetime"].ToString();
			topic.Lastpost = reader["lastpost"].ToString();
			topic.Lastposter = reader["lastposter"].ToString();
			topic.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
			topic.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
			topic.Views = Int32.Parse(reader["views"].ToString());
			topic.Replies = Int32.Parse(reader["replies"].ToString());
			topic.Displayorder = Int32.Parse(reader["displayorder"].ToString());
			topic.Highlight = reader["highlight"].ToString();
			topic.Digest = Int32.Parse(reader["digest"].ToString());
			//topic.Rate = Int32.Parse(reader["rate"].ToString());
			topic.Hide = Int32.Parse(reader["hide"].ToString());
			//topic.Poll = Int32.Parse(reader["poll"].ToString());
            topic.Special = byte.Parse(reader["special"].ToString());
			topic.Attachment = Int32.Parse(reader["attachment"].ToString());
			//topic.Moderated = Int32.Parse(reader["moderated"].ToString());
			topic.Closed = Int32.Parse(reader["closed"].ToString());
			//info.Magic = Int32.Parse(reader["magic"].ToString());
			//处理关闭标记
			if (topic.Closed == 0)
			{
				string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
				if (oldtopic.IndexOf("D" + topic.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(topic.Lastpost))
				{
					topic.Folder = "new";
				}
				else
				{
					topic.Folder = "old";
				}


				if (topic.Replies >= hot)
				{
					topic.Folder += "hot";
				}
			}
			else
			{
				topic.Folder = "closed";
				if (topic.Closed > 1)
				{
					topic.Tid = topic.Closed;
					topic.Folder = "move";
				}
			}

			if (topic.Highlight != "")
			{
				topic.Title = "<span style=\"" + topic.Highlight + "\">" + topic.Title + "</span>";
			}
			return topic;
		}
        /// <summary>
        /// 对符合新贴,精华贴的页面显示进行查询的函数
        /// </summary>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="ascdesc">升/降序</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection GetTopicCollectionByTypeDate(int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, string orderby, int ascdesc)
        {
            ShowforumPageTopicInfoCollection coll = new ShowforumPageTopicInfoCollection();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByTypeDate(pagesize, pageindex, startnum, condition, orderby, ascdesc);
            if (reader != null)
            {

                System.Collections.SortedList TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }

        #endregion

#else

        #region 主题泛型类操作

        //将得到的主题列表中加入主题类型名称字段
        public static DataTable GetTopicTypeName(DataTable __topiclist)
        {
            DataColumn dc = new DataColumn();
            dc.ColumnName = "topictypename";
            dc.DataType = Type.GetType("System.String");
            dc.DefaultValue = "";
            dc.AllowDBNull = true;
            __topiclist.Columns.Add(dc);

            Discuz.Common.Generic.SortedList<int, object> __topictypearray = Caches.GetTopicTypeArray();
            object typictypename = null;
            foreach (DataRow dr in __topiclist.Rows)
            {
                typictypename = __topictypearray[Int32.Parse(dr["typeid"].ToString())];
                dr["topictypename"] = (typictypename != null && typictypename.ToString().Trim() != "") ? "[" + typictypename.ToString().Trim() + "]" : "";
            }
            return __topiclist;
        }

        /// <summary>
        /// 按照用户Id获取其回复过的主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<MyTopicInfo> GetTopicsByReplyUserId(int userId, int pageIndex, int pageSize, int newmin, int hot)
        {
            Discuz.Common.Generic.List<MyTopicInfo> coll = new Discuz.Common.Generic.List<MyTopicInfo>();

            if (pageIndex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByReplyUserId(userId, pageIndex, pageSize);

            if (reader != null)
            {
                while (reader.Read())
                {
                    MyTopicInfo topic = LoadSingleMyTopic(newmin, hot, reader);

                    coll.Add(topic);
                }
                reader.Close();
            }
            return coll;
        }

        private static MyTopicInfo LoadSingleMyTopic(int newmin, int hot, IDataReader reader)
        {
            MyTopicInfo topic = new MyTopicInfo();
            topic.Tid = Int32.Parse(reader["tid"].ToString());
            topic.Fid = Int32.Parse(reader["fid"].ToString());
            topic.Iconid = Int32.Parse(reader["iconid"].ToString());
            topic.Forumname = Forums.GetForumInfo(topic.Fid).Name;
            topic.Title = reader["title"].ToString();
            topic.Typeid = Int32.Parse(reader["typeid"].ToString());
            topic.Readperm = Int32.Parse(reader["readperm"].ToString());
            topic.Price = Int32.Parse(reader["price"].ToString());
            topic.Poster = reader["poster"].ToString();
            topic.Posterid = Int32.Parse(reader["posterid"].ToString());
            topic.Postdatetime = reader["postdatetime"].ToString();
            topic.Lastpost = reader["lastpost"].ToString();
            topic.Lastposter = reader["lastposter"].ToString();
            topic.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
            topic.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
            topic.Views = Int32.Parse(reader["views"].ToString());
            topic.Replies = Int32.Parse(reader["replies"].ToString());
            topic.Displayorder = Int32.Parse(reader["displayorder"].ToString());
            topic.Highlight = reader["highlight"].ToString();
            topic.Digest = Int32.Parse(reader["digest"].ToString());
            //topic.Rate = Int32.Parse(reader["rate"].ToString());
            topic.Hide = Int32.Parse(reader["hide"].ToString());
            //topic.Poll = Int32.Parse(reader["poll"].ToString());
            topic.Attachment = Int32.Parse(reader["attachment"].ToString());
            //topic.Moderated = Int32.Parse(reader["moderated"].ToString());
            topic.Closed = Int32.Parse(reader["closed"].ToString());
            topic.Special = byte.Parse(reader["special"].ToString());
            //info.Magic = Int32.Parse(reader["magic"].ToString());
            //处理关闭标记
            if (topic.Closed == 0)
            {
                string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                if (oldtopic.IndexOf("D" + topic.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(topic.Lastpost))
                {
                    topic.Folder = "new";
                }
                else
                {
                    topic.Folder = "old";
                }


                if (topic.Replies >= hot)
                {
                    topic.Folder += "hot";
                }
            }
            else
            {
                topic.Folder = "closed";
                if (topic.Closed > 1)
                {
                    topic.Tid = topic.Closed;
                    topic.Folder = "move";
                }
            }

            if (topic.Highlight != "")
            {
                topic.Title = "<span style=\"" + topic.Highlight + "\">" + topic.Title + "</span>";
            }
            return topic;
        }

        /// <summary>
        /// 按照用户Id获取主题列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static Discuz.Common.Generic.List<MyTopicInfo> GetTopicsByUserId(int userId, int pageIndex, int pageSize, int newmin, int hot)
        {
            Discuz.Common.Generic.List<MyTopicInfo> coll = new Discuz.Common.Generic.List<MyTopicInfo>();

            if (pageIndex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByUserId(userId, pageIndex, pageSize);

            if (reader != null)
            {
                while (reader.Read())
                {
                    MyTopicInfo topic = LoadSingleMyTopic(newmin, hot, reader);

                    coll.Add(topic);
                }
                reader.Close();
            }
            return coll;
        }



        /// <summary>
        /// 获得置顶主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="pagesize">每页显示主题数</param>
        /// <param name="pageindex">当前页数</param>
        /// <param name="tids">主题id列表</param>
        /// <returns>主题信息列表</returns>
        public static ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> GetTopTopicCollection(int fid, int pagesize, int pageindex, string tids, int autoclose, int topictypeprefix)
        {
            ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> coll = new ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();
            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopTopics(fid, pagesize, pageindex, tids);
            if (reader != null)
            {

                Discuz.Common.Generic.SortedList<int, object> TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    info.Magic = Int32.Parse(reader["magic"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.SortAdd(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }

            return coll;


        }

        /// <summary>
        /// 获得一般主题信息列表
        /// </summary>
        /// <param name="fid">版块ID</param>
        /// <param name="condition">条件</param>
        /// <param name="pagesize">每页显示主题数</param>
        /// <param name="pageindex">当前页数</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <returns>主题信息列表</returns>
        public static ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> GetTopicCollection(int fid, int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition)
        {
            ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> coll = new ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopics(fid, pagesize, pageindex, startnum, condition);
            if (reader != null)
            {

                Discuz.Common.Generic.SortedList<int, object> TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    info.Magic = Int32.Parse(reader["magic"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;

        }

        /// <summary>
        /// 获取主题列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="ascdesc">升/降序</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> GetTopicCollection(int fid, int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, string orderby, int ascdesc)
        {
            ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> coll = new ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByDate(fid, pagesize, pageindex, startnum, condition, orderby, ascdesc);
            if (reader != null)
            {

                Discuz.Common.Generic.SortedList<int, object> TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());                    
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }

        /// <summary>
        /// 对符合新贴,精华贴的页面显示进行查询的函数
        /// </summary>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> GetTopicCollectionByType(int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, int ascdesc)
        {
            ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> coll = new ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();

            if (pageindex <= 0)
            {
                return coll;
            }
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByType(pagesize, pageindex, startnum, condition, ascdesc);
            if (reader != null)
            {

                Discuz.Common.Generic.SortedList<int, object> TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    //info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }

        /// <summary>
        /// 对符合新贴,精华贴的页面显示进行查询的函数
        /// </summary>
        /// <param name="pagesize">每个分页的主题数</param>
        /// <param name="pageindex">分页页数</param>
        /// <param name="startnum">置顶帖数量</param>
        /// <param name="newmin">最近多少分钟内的主题认为是新主题</param>
        /// <param name="hot">多少个帖子认为是热门主题</param>
        /// <param name="condition">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="ascdesc">升/降序</param>
        /// <returns>主题列表</returns>
        public static ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> GetTopicCollectionByTypeDate(int pagesize, int pageindex, int startnum, int newmin, int hot, int autoclose, int topictypeprefix, string condition, string orderby, int ascdesc)
        {
            ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> coll = new ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();

            if (pageindex <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetTopicsByTypeDate(pagesize, pageindex, startnum, condition, orderby, ascdesc);
            if (reader != null)
            {

                Discuz.Common.Generic.SortedList<int, object> TopicTypeArray = Caches.GetTopicTypeArray();
                StringBuilder closeid = new StringBuilder();
                object TopicTypeName = null;

                while (reader.Read())
                {
                    ShowforumPageTopicInfo info = new ShowforumPageTopicInfo();
                    info.Tid = Int32.Parse(reader["tid"].ToString());
                    //info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Iconid = Int32.Parse(reader["iconid"].ToString());
                    info.Title = reader["title"].ToString();
                    info.Typeid = Int32.Parse(reader["typeid"].ToString());
                    info.Readperm = Int32.Parse(reader["readperm"].ToString());
                    info.Price = Int32.Parse(reader["price"].ToString());
                    info.Poster = reader["poster"].ToString();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Postdatetime = reader["postdatetime"].ToString();
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lastposterid = Int32.Parse(reader["LastposterID"].ToString());
                    info.Lastpostid = Int32.Parse(reader["LastpostID"].ToString());
                    info.Views = Int32.Parse(reader["views"].ToString());
                    info.Replies = Int32.Parse(reader["replies"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Highlight = reader["highlight"].ToString();
                    info.Digest = Int32.Parse(reader["digest"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Hide = Int32.Parse(reader["hide"].ToString());
                    //info.Poll = Int32.Parse(reader["poll"].ToString());
                    info.Special = byte.Parse(reader["special"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    //info.Moderated = Int32.Parse(reader["moderated"].ToString());
                    info.Closed = Int32.Parse(reader["closed"].ToString());
                    //info.Magic = Int32.Parse(reader["magic"].ToString());
                    //处理关闭标记
                    if (info.Closed == 0)
                    {
                        string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
                        if (oldtopic.IndexOf("D" + info.Tid.ToString() + "D") == -1 && DateTime.Now.AddMinutes(-1 * newmin) < DateTime.Parse(info.Lastpost))
                        {
                            info.Folder = "new";
                        }
                        else
                        {
                            info.Folder = "old";
                        }


                        if (info.Replies >= hot)
                        {
                            info.Folder += "hot";
                        }

                        if (autoclose > 0)
                        {
                            if (Utils.StrDateDiffHours(info.Postdatetime, autoclose * 24) > 0)
                            {
                                info.Closed = 1;
                                if (closeid.Length > 0)
                                {
                                    closeid.Append(",");
                                }
                                closeid.Append(info.Tid.ToString());
                                info.Folder = "closed";
                            }
                        }


                    }
                    else
                    {
                        info.Folder = "closed";
                        if (info.Closed > 1)
                        {
                            info.Tid = info.Closed;
                            info.Folder = "move";
                        }
                    }

                    if (info.Highlight != "")
                    {
                        info.Title = "<span style=\"" + info.Highlight + "\">" + info.Title + "</span>";
                    }

                    //扩展属性
                    if (topictypeprefix > 0 && info.Typeid > 0)
                    {
                        TopicTypeName = TopicTypeArray[info.Typeid];
                        if (TopicTypeName != null && TopicTypeName.ToString().Trim() != "")
                        {
                            info.Topictypename = TopicTypeName.ToString().Trim();
                        }
                    }
                    else
                    {
                        info.Topictypename = "";
                    }
                    //

                    coll.Add(info);
                }
                reader.Close();
                if (closeid.Length > 0)
                {
                    TopicAdmins.SetClose(closeid.ToString(), 1);
                }
            }
            return coll;
        }



        public class ShowforumPageTopicInfoCollection<T> : Discuz.Common.Generic.List<T> where T : ShowforumPageTopicInfo, new()
        {
            public ShowforumPageTopicInfoCollection() : base() { }

            public ShowforumPageTopicInfoCollection(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

            public ShowforumPageTopicInfoCollection(int capacity) : base(capacity) { }

            public void SortAdd(T value)
            {
                if (this.Count <= 0)
                {
                    this.Add(value);
                }
                else
                {
                    for (int i = 0; i < this.Count; i++)
                    {
                        if ((value.Displayorder) > (this[i].Displayorder))
                        {
                            this.Insert(i, value);
                            return ;
                        }
                    }
                    this.Add(value);

                }
           
            }

        }
        
        #endregion

#endif


        public static void UpdateTopic(int tid, string title, int posterid, string poster)
        {
            DatabaseProvider.GetInstance().UpdateTopic(tid, title, posterid, poster);
        }

        /// <summary>
        /// 输出htmltitle
        /// </summary>
        /// <param name="htmltitle"></param>
        /// <param name="topicid"></param>
        public static void WriteHtmlTitleFile(string htmltitle, int topicid)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append("/cache/topic/magic/");

            if (!Directory.Exists(Utils.GetMapPath(dir.ToString())))
            {
                Utils.CreateDir(Utils.GetMapPath(dir.ToString()));
            }

            dir.Append((topicid / 1000 + 1).ToString());
            dir.Append("/");

            if (!Directory.Exists(Utils.GetMapPath(dir.ToString())))
            {
                Utils.CreateDir(Utils.GetMapPath(dir.ToString()));
            }


            string filename = Utils.GetMapPath(dir.ToString() + topicid.ToString() + "_htmltitle.config");
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    Byte[] info = System.Text.Encoding.UTF8.GetBytes(Utils.RemoveUnsafeHtml(htmltitle));
                    fs.Write(info, 0, info.Length);
                    fs.Close();
                }

            }
            catch
            {
            }


        }

        /// <summary>
        /// 获得相应的magic值
        /// </summary>
        /// <param name="magic">数据库中magic值</param>
        /// <param name="magicType">magic类型</param>
        /// <returns></returns>
        public static int GetMagicValue(int magic, MagicType magicType)
        {
            if (magic == 0)
                return 0;
            string m = magic.ToString();
            switch (magicType)
            { 
                case MagicType.HtmlTitle:
                    if (m.Length >= 2)
                        return Convert.ToInt32(m.Substring(1, 1));
                    break;
                case MagicType.MagicTopic:
                    if (m.Length >= 5)
                        return Convert.ToInt32(m.Substring(2, 3));
                    break;
                case MagicType.TopicTag:
                    if (m.Length >= 6)
                        return Convert.ToInt32(m.Substring(5, 1));
                    break;
        
            }
            return 0;

        }

        /// <summary>
        /// 设置相应的magic值
        /// </summary>
        /// <param name="magic">原始magic值</param>
        /// <param name="magicType"></param>
        /// <param name="bonusstat"></param>
        /// <returns></returns>
        public static int SetMagicValue(int magic, MagicType magicType, int newmagicvalue)
        {
            string[] m = Utils.SplitString(magic.ToString(), "");
            switch (magicType)
            { 
                case MagicType.HtmlTitle:
                    if (m.Length >= 2)
                    {
                        m[1] = newmagicvalue.ToString().Substring(0, 1);
                        return Utils.StrToInt(string.Join("", m), magic);
                    }
                    else
                    {
                        return Utils.StrToInt(string.Format("1{0}", newmagicvalue.ToString().Substring(0,1)), magic);
                    }
                case MagicType.MagicTopic:
                    if (m.Length >= 5)
                    {
                        string[] t = Utils.SplitString(newmagicvalue.ToString().PadLeft(3, '0'), "");
                        m[2] = t[0];
                        m[3] = t[1];
                        m[4] = t[2];
                        return Utils.StrToInt(string.Join("", m), magic);
                    }
                    else
                    {
                        return Utils.StrToInt(string.Format("1{0}{1}", GetMagicValue(magic, MagicType.HtmlTitle), newmagicvalue.ToString().PadLeft(3, '0').Substring(0, 3)), magic);
                    }
                case MagicType.TopicTag:
                    if (m.Length >= 6)
                    {
                        m[5] = newmagicvalue.ToString().Substring(0, 1);
                        return Utils.StrToInt(string.Join("", m), magic);
                    }
                    else
                    {
                        return Utils.StrToInt(string.Format("1{0}{1}{2}", GetMagicValue(magic, MagicType.HtmlTitle), GetMagicValue(magic, MagicType.MagicTopic).ToString("000"), newmagicvalue.ToString().Substring(0, 1)), magic);
                    }
        
            }
            return magic;
        }

        /// <summary>
        /// 读取指定帖子的html标题
        /// </summary>
        /// <param name="topicid"></param>
        /// <returns></returns>
        public static string GetHtmlTitle(int topicid)
        {
            StringBuilder dir = new StringBuilder();
            dir.Append("/cache/topic/magic/");
            dir.Append((topicid / 1000 + 1).ToString());
            dir.Append("/");
            string filename = Utils.GetMapPath(dir.ToString() + topicid.ToString() + "_htmltitle.config");
            if (!File.Exists(filename))
                return "";

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    return sr.ReadToEnd();
                }
            }

        }

        /// <summary>
        /// 根据主题的Tag获取相关主题(游客可见级别的)
        /// </summary>
        /// <param name="topicid">主题Id</param>
        /// <returns></returns>
        public static List<TopicInfo> GetRelatedTopics(int topicid, int count)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetRelatedTopics(topicid, count);

            List<TopicInfo> topics = new List<TopicInfo>();
            while (reader.Read())
            {
                TopicInfo topic = new TopicInfo();
                topic.Tid = Utils.StrToInt(reader["linktid"], 0);
                topic.Title = reader["linktitle"].ToString();
                topics.Add(topic);
            }

            reader.Close();
            return topics;
        }

        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <returns></returns>
        public static int GetTopicsCountWithSameTag(int tagid)
        {
            return DatabaseProvider.GetInstance().GetTopicsCountByTag(tagid);
        }

        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <returns></returns>
        public static List<MyTopicInfo> GetTopicsWithSameTag(int tagid, int count)
        {
            return GetTopicsWithSameTag(tagid, 1, count);
        }

        /// <summary>
        /// 获取使用同一tag的主题列表
        /// </summary>
        /// <param name="tagid">TagId</param>
        /// <param name="pageindex">页码</param>
        /// <param name="pagesize">页大小</param>
        /// <returns></returns>
        public static List<MyTopicInfo> GetTopicsWithSameTag(int tagid, int pageindex, int pagesize)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTopicListByTag(tagid, pageindex, pagesize);

            List<MyTopicInfo> topics = new List<MyTopicInfo>();

            while (reader.Read())
            {
                MyTopicInfo topic = new MyTopicInfo();
                topic.Tid = Utils.StrToInt(reader["tid"], 0);
                topic.Title = reader["title"].ToString();
                topic.Poster = reader["poster"].ToString();
                topic.Posterid = Utils.StrToInt(reader["posterid"], -1);
                topic.Fid = Utils.StrToInt(reader["fid"], 0);
                topic.Forumname = Forums.GetForumInfo(Utils.StrToInt(reader["fid"], 0)).Name;
                topic.Postdatetime = reader["postdatetime"].ToString();
                topic.Replies = Utils.StrToInt(reader["replies"], 0);
                topic.Views = Utils.StrToInt(reader["views"], 0);
                topic.Lastposter = reader["lastposter"].ToString();
                topic.Lastposterid = Utils.StrToInt(reader["lastposterid"], -1);
                topic.Lastpost = reader["lastpost"].ToString();
                topics.Add(topic);
            }

            reader.Close();

            return topics;
        }

        /// <summary>
        /// 整理相关主题表
        /// </summary>
        public static void NeatenRelateTopics()
        {
            DatabaseProvider.GetInstance().NeatenRelateTopics();
        }
        /// <summary>
        /// 删除主题的相关主题记录
        /// </summary>
        /// <param name="topicid"></param>
        public static void DeleteRelatedTopics(int topicid)
        {
            DatabaseProvider.GetInstance().DeleteRelatedTopics(topicid);
        }

        /// <summary>
        /// 修改指定主题的Magic为指定值
        /// </summary>
        /// <param name="tid">主题Id</param>
        /// <param name="magic">Magic值</param>
        public static void UpdateMagicValue(int tid, int magic)
        {
            DatabaseProvider.GetInstance().UpdateMagicValue(tid, magic);
        }

        /// <summary>
        /// 增加辩论主题扩展信息
        /// </summary>
        /// <param name="debatetopic"></param>
        public static void AddDebateTopic(DebateInfo debatetopic)
        {
            //debatetopic = ReviseDebateTopicColor(debatetopic);
            DatabaseProvider.GetInstance().AddDebateTopic(debatetopic);
        }

        /// <summary>
        /// 更新主题类型标识字段
        /// </summary>
        /// <param name="topicinfo"></param>
        public static void UpdateSpecial(TopicInfo topicinfo)
        {
            DatabaseProvider.GetInstance().UpdateTopicSpecial(topicinfo.Tid, topicinfo.Special);
        }

        /// <summary>
        /// 修正主题的颜色为6位16进制的合法颜色值，
        /// </summary>
        /// <param name="debatetopic"></param>
        /// <returns></returns>
        //private static DebateInfo ReviseDebateTopicColor(DebateInfo debatetopic)
        //{
        //    if (Utils.CheckColorValue(debatetopic.Positivecolor))
        //    {
        //        debatetopic.Positivecolor = debatetopic.Positivecolor.Trim().Trim('#');
        //    }
        //    else
        //    {
        //        debatetopic.Positivecolor = GeneralConfigs.GetConfig().Defaultpositivecolor;
        //    }

        //    if (Utils.CheckColorValue(debatetopic.Negativecolor))
        //    {
        //        debatetopic.Negativecolor = debatetopic.Negativecolor.Trim().Trim('#');
        //    }
        //    else
        //    {
        //        debatetopic.Negativecolor = GeneralConfigs.GetConfig().Defaultnegativecolor;
        //    }

        //    if (Utils.CheckColorValue(debatetopic.Positivebordercolor))
        //    {
        //        debatetopic.Positivebordercolor = debatetopic.Positivebordercolor.Trim().Trim('#');
        //    }
        //    else
        //    {
        //        debatetopic.Positivebordercolor = GeneralConfigs.GetConfig().Defaultpositivebordercolor;
        //    }

        //    if (Utils.CheckColorValue(debatetopic.Negativebordercolor))
        //    {
        //        debatetopic.Negativebordercolor = debatetopic.Negativebordercolor.Trim().Trim('#');
        //    }
        //    else
        //    {
        //        debatetopic.Negativebordercolor = GeneralConfigs.GetConfig().Defaultnegativebordercolor;
        //    }

        //    return debatetopic;
        //}
    }
}
