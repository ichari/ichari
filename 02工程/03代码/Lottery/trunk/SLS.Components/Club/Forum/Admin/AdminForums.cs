using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminForumFactory 的摘要说明。
	/// 后台论坛版块管理类
	/// </summary>
	public class AdminForums : Forums
	{
		public AdminForums()
		{
		}


		/// <summary>
		/// 返回所有论坛列表
		/// </summary>
		/// <returns></returns>
		public static DataTable GetAllForumList()
		{
            return DatabaseProvider.GetInstance().GetAllForumList();
		}


		/// <summary>
		///  得到指定论坛版块(分类)的相关信息
		/// </summary>
		/// <param name="fid">指定版块或分类的fid值</param>
		/// <returns></returns>
		public static ForumInfo GetForumInfomation(int fid)
		{
			ForumInfo foruminfo = new ForumInfo();

            DataTable dt = DatabaseProvider.GetInstance().GetForumInformation(fid);
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				foruminfo.Fid = Int32.Parse(dr["fid"].ToString());
				foruminfo.Parentid = Int16.Parse(dr["parentid"].ToString());
				foruminfo.Layer = Int16.Parse(dr["layer"].ToString());
				foruminfo.Name = dr["name"].ToString();
				foruminfo.Pathlist = dr["pathlist"].ToString();
				foruminfo.Parentidlist = dr["parentidlist"].ToString();
				foruminfo.Subforumcount = Int32.Parse(dr["subforumcount"].ToString());
				foruminfo.Status = Int32.Parse(dr["status"].ToString());
				foruminfo.Colcount = Int32.Parse(dr["colcount"].ToString());
				foruminfo.Displayorder = Int32.Parse(dr["displayorder"].ToString());
				foruminfo.Templateid = Int16.Parse(dr["templateid"].ToString());
				foruminfo.Topics = Int32.Parse(dr["topics"].ToString());
				foruminfo.Posts = Int32.Parse(dr["posts"].ToString());
				foruminfo.Todayposts = Int32.Parse(dr["todayposts"].ToString());
				foruminfo.Lastpost = dr["lastpost"].ToString();
				foruminfo.Lastposter = dr["lastposter"].ToString();
				foruminfo.Lastposterid = Int32.Parse(dr["lastposterid"].ToString());
				foruminfo.Lasttitle = dr["lasttitle"].ToString();
				foruminfo.Allowsmilies = Int32.Parse(dr["allowsmilies"].ToString());
				foruminfo.Allowrss = Int32.Parse(dr["allowrss"].ToString());
				foruminfo.Allowhtml = Int32.Parse(dr["allowhtml"].ToString());
				foruminfo.Allowbbcode = Int32.Parse(dr["allowbbcode"].ToString());
				foruminfo.Allowimgcode = Int32.Parse(dr["allowimgcode"].ToString());
				foruminfo.Allowblog = Int32.Parse(dr["allowblog"].ToString());
                foruminfo.Istrade = Int32.Parse(dr["istrade"].ToString());
                foruminfo.Allowpostspecial = Int32.Parse(dr["allowpostspecial"].ToString());
                foruminfo.Allowspecialonly = Int32.Parse(dr["allowspecialonly"].ToString());
				foruminfo.Alloweditrules = Int32.Parse(dr["alloweditrules"].ToString());
				foruminfo.Recyclebin = Int32.Parse(dr["recyclebin"].ToString());
				foruminfo.Modnewposts = Int32.Parse(dr["modnewposts"].ToString());
				foruminfo.Jammer = Int32.Parse(dr["jammer"].ToString());
				foruminfo.Disablewatermark = Int32.Parse(dr["disablewatermark"].ToString());
				foruminfo.Inheritedmod = Int32.Parse(dr["inheritedmod"].ToString());
				foruminfo.Autoclose = Int16.Parse(dr["autoclose"].ToString());
                

				foruminfo.Description = dr["description"].ToString().Trim() != "" ? dr["description"].ToString() : "";
				foruminfo.Password = dr["password"].ToString();
				foruminfo.Icon = dr["icon"].ToString();
				foruminfo.Postcredits = dr["postcredits"].ToString();
				foruminfo.Replycredits = dr["replycredits"].ToString();
				foruminfo.Redirect = dr["redirect"].ToString();
				foruminfo.Attachextensions = dr["attachextensions"].ToString();
				foruminfo.Moderators = dr["moderators"].ToString();
				foruminfo.Rules = dr["rules"].ToString();
				foruminfo.Topictypes = dr["topictypes"].ToString();

				foruminfo.Viewperm = dr["viewperm"].ToString();
				foruminfo.Postperm = dr["postperm"].ToString();
				foruminfo.Replyperm = dr["replyperm"].ToString();
				foruminfo.Getattachperm = dr["getattachperm"].ToString();
				foruminfo.Postattachperm = dr["postattachperm"].ToString();
				foruminfo.Allowthumbnail = Int32.Parse(dr["allowthumbnail"].ToString());
                foruminfo.Allowtag = Int32.Parse(dr["allowtag"].ToString());
                foruminfo.Applytopictype = Int32.Parse(dr["applytopictype"].ToString());
                foruminfo.Postbytopictype = Int32.Parse(dr["postbytopictype"].ToString());
                foruminfo.Viewbytopictype = Int32.Parse(dr["viewbytopictype"].ToString());
                foruminfo.Topictypeprefix = Int32.Parse(dr["topictypeprefix"].ToString());
                foruminfo.Topictypes = dr["topictypes"].ToString();
                foruminfo.Permuserlist = dr["permuserlist"].ToString();

				dt.Dispose();
			}

			return foruminfo;
		}


		/// <summary>
		/// 保存论坛版块(分类)的相关信息
		/// </summary>
		/// <param name="__foruminfo"></param>
		/// <returns></returns>
		public static string SaveForumsInf(ForumInfo __foruminfo)
		{
            DatabaseProvider.GetInstance().SaveForumsInfo(__foruminfo);

			SetForumsPathList();

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/ForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/TopicTypesOption" + __foruminfo.Fid);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/TopicTypesLink" + __foruminfo.Fid);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");

            return SetForumsModerators(__foruminfo.Fid.ToString(), __foruminfo.Moderators, __foruminfo.Inheritedmod);

		}


		/// <summary>
		/// 向版块列表中插入新的版块信息
		/// </summary>
		/// <param name="__foruminfo"></param>
		/// <returns></returns>
		public static string InsertForumsInf(ForumInfo __foruminfo)
		{
            int fid = DatabaseProvider.GetInstance().InsertForumsInf(__foruminfo);

			SetForumsPathList();
            //TopicAdmins.ResetTopTopicList(DatabaseProvider.GetInstance().GetMaxForumId());
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/ForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");

			return SetForumsModerators(fid.ToString(), __foruminfo.Moderators, __foruminfo.Inheritedmod);

		}


		/// <summary>
		/// 设置版块列表中论坛路径(pathlist)字段
		/// </summary>
		public static void SetForumsPathList()
		{
            string extname = GeneralConfigs.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "/config/general.config").Extname;

			SetForumsPathList(true,extname);
		}

        
		/// <summary>
		/// 按指定的文件扩展名称设置版块列表中论坛路径(pathlist)字段
		/// </summary>
		/// <param name="extname">扩展名称,如:aspx , html 等</param>
        public static void SetForumsPathList(bool isaspxrewrite, string extname)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetAllForumList();

			foreach (DataRow dr in dt.Rows)
			{
				string pathlist = "";

				if (dr["parentidlist"].ToString().Trim() == "0")
				{
                    if (isaspxrewrite)
                    {
                        pathlist = "<a href=\"showforum-" + dr["fid"].ToString() + extname + "\">" + dr["name"].ToString().Trim() + "</a>";
                    }
                    else
                    {
                        pathlist = "<a href=\"showforum.aspx?forumid=" + dr["fid"].ToString() + "\">" + dr["name"].ToString().Trim() + "</a>";
                    }
				}
				else
				{
					foreach (string parentid in dr["parentidlist"].ToString().Trim().Split(','))
					{
						if (parentid.Trim() != "")
						{
							DataRow[] drs = dt.Select("[fid]=" + parentid);
							if (drs.Length > 0)
							{
                                if (isaspxrewrite)
                                {
                                    pathlist += "<a href=\"showforum-" + drs[0]["fid"].ToString() + extname + "\">" + drs[0]["name"].ToString().Trim() + "</a>";
                                }
                                else 
                                {
                                    pathlist += "<a href=\"showforum.aspx?forumid=" + drs[0]["fid"].ToString() + "\">" + drs[0]["name"].ToString().Trim() + "</a>";
                                }
							}
						}
					}
                    if (isaspxrewrite)
                    {
                        pathlist += "<a href=\"showforum-" + dr["fid"].ToString() + extname + "\">" + dr["name"].ToString().Trim() + "</a>";
                    }
                    else
                    {
                        pathlist += "<a href=\"showforum.aspx?forumid=" + dr["fid"].ToString() + "\">" + dr["name"].ToString().Trim() + "</a>";
                    }
				}

                DatabaseProvider.GetInstance().SetForumsPathList(pathlist, int.Parse(dr["fid"].ToString()));
			}
		}


		/// <summary>
		/// 设置版块列表中层数(layer)和父列表(parentidlist)字段
		/// </summary>
		public static void SetForumslayer()
		{
            DataTable dt = DatabaseProvider.GetInstance().GetAllForumList();
			foreach (DataRow dr in dt.Rows)
			{
				int layer = 0;
				string parentidlist = "";
				string parentid = dr["parentid"].ToString().Trim();

				//如果是(分类)顶层则直接更新数据库
				if (parentid == "0")
				{
                    DatabaseProvider.GetInstance().SetForumslayer(layer,"0",int.Parse(dr["fid"].ToString()));
					continue;
				}

				do
				{ //更新子版块的层数(layer)和父列表(parentidlist)字段
					string temp = parentid;

                    parentid = DatabaseProvider.GetInstance().GetForumsParentidByFid(int.Parse(parentid)).ToString();
					layer++;
					if (parentid != "0")
					{
						parentidlist = temp + "," + parentidlist;
					}
					else
					{
						parentidlist = temp + "," + parentidlist;
                        DatabaseProvider.GetInstance().SetForumslayer(layer, parentidlist.Substring(0, parentidlist.Length - 1), int.Parse(dr["fid"].ToString()));
						break;
					}
				} while (true);
			}

		}


		/// <summary>
		/// 移动论坛版块
		/// </summary>
		/// <param name="currentfid">当前论坛版块id</param>
		/// <param name="targetfid">目标论坛版块id</param>
		/// <param name="isaschildnode">是否作为子论坛移动</param>
		/// <returns></returns>
		public static bool MovingForumsPos(string currentfid, string targetfid, bool isaschildnode)
		{
            string extname = GeneralConfigs.Deserialize(AppDomain.CurrentDomain.BaseDirectory + "/config/general.config").Extname;

            DatabaseProvider.GetInstance().MovingForumsPos(currentfid, targetfid, isaschildnode, extname);
			AdminForums.SetForumslayer();
			AdminForums.SetForumsSubForumCountAndDispalyorder();
			AdminForums.SetForumsPathList();

            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/ForumList");

			return true;
			
		}


		/// <summary>
		/// 删除指定fid的论坛版块
		/// </summary>
		/// <param name="fid">要删除的论坛版块的fid</param>
		/// <returns></returns>
		public static bool DeleteForumsByFid(string fid)
		{
            if(DatabaseProvider.GetInstance().IsExistSubForum(int.Parse(fid)))
			{
				return false;
			}

         
            DatabaseProvider.GetInstance().DeleteForumsByFid(Posts.GetPostTableName(), fid);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/UI/ForumListBoxOptions");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/ForumList");

			return true;
		}


		/// <summary>
		/// 设置指定论坛版块版主
		/// </summary>
		/// <param name="fid">指定的论坛版块id</param>
		/// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
		/// <param name="inherited">是否使用继承选项 1为使用  0为不使用</param>
		/// <returns></returns>
		public static string SetForumsModerators(string fid, string moderators, int inherited)
		{
			//清除已有论坛的版主设置
            DatabaseProvider.GetInstance().DeleteModeratorByFid(int.Parse(fid));

			//使用继承机制时
			if (inherited == 1)
			{
				string parentid = fid;
				string parendidlist = "-1";
				while (true)
				{
                    DataTable dt = DatabaseProvider.GetInstance().GetParentIdByFid(int.Parse(parentid));
					if (dt.Rows.Count > 0) parentid = dt.Rows[0][0].ToString();
					else break;

					if ((parentid == "0") || (parentid == "")) break;

					parendidlist = parendidlist + "," + parentid;

				}

				int count = 1;
                foreach (DataRow dr in DatabaseProvider.GetInstance().GetUidModeratorByFid(parendidlist).Rows)
				{
                    DatabaseProvider.GetInstance().AddModerator(int.Parse(dr[0].ToString()), int.Parse(fid), count, 1);
					count++;
				}
			}

			InsertForumsModerators(fid, moderators, 1, 0);

			return UpdateUserInfoWithModerator(moderators);

		}


		/// <summary>
		/// 更新当前已设置为指定版块版主的相关用户信息.
		/// </summary>
		/// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
		/// <returns>返回不存在用户的字符串</returns>
		public static string UpdateUserInfoWithModerator(string moderators)
		{
            moderators = moderators == null ? "" : moderators;
			string usernamenoexsit = "";
			DataTable dt = new DataTable();
			foreach (string moderator in moderators.Split(','))
			{
				if (moderator != "")
				{
					//当用户名是系统保留的用户名,请您输入其它的用户名
					if (PrivateMessages.SystemUserName == moderator)
					{
						continue;
					}

                    dt = DatabaseProvider.GetInstance().GetModeratorInfo(moderator);
					if (dt.Rows.Count > 0)
					{
						int groupid = Convert.ToInt32(dt.Rows[0]["groupid"].ToString());
						if ((groupid <= 3) && (groupid > 0)) continue; //当为管理员,超级版主,版主时
						else
						{
                            int radminid = DatabaseProvider.GetInstance().GetRadminidByGroupid(groupid);
                            if (radminid <= 0)
                                    DatabaseProvider.GetInstance().SetModerator(moderator);
                            else continue;
						}
					}
					else
					{
						usernamenoexsit = usernamenoexsit + moderator + ",";
					}
				}

			}

			AdminCaches.ReSetModeratorList();

			return usernamenoexsit;
		}


		/// <summary>
		/// 向版主列表中插入相关的版主信息
		/// </summary>
		/// <param name="fid">指定的论坛版块</param>
		/// <param name="moderators">相关要设置的版主名称(注:用","号分割)</param>
		/// <param name="displayorder">显示顺序</param>
		/// <param name="inherited">是否使用继承机制</param>
		public static void InsertForumsModerators(string fid, string moderators, int displayorder, int inherited)
		{
            moderators = moderators == null ? "" : moderators;
            DatabaseProvider.GetInstance().InsertForumsModerators(fid, moderators, displayorder, inherited);
			AdminCaches.ReSetModeratorList();

		}


		/// <summary>
		/// 对比指定的论坛版块的新老信息,将作出相应的调整
		/// </summary>
		/// <param name="oldmoderators">老版主名称(注:用","号分割)</param>
		/// <param name="newmoderators">新版主名称(注:用","号分割)</param>
		/// <param name="currentfid">当前论坛版块的fid</param>
		public static void CompareOldAndNewModerator(string oldmoderators, string newmoderators, int currentfid)
		{
			if ((oldmoderators == null) || (oldmoderators == ""))
			{
				return;
			}

			//在新的版主名单中查找老的版主是否存在
			foreach (string oldmoderator in oldmoderators.Split(','))
			{
				if ((oldmoderator != "") &&
					("," + newmoderators + ",").IndexOf("," + oldmoderator + ",") < 0) //当不存在，则表示当前老的版主已被删除，则执行删除当前老版主
				{
                    DataTable dt = DatabaseProvider.GetInstance().GetUidAdminIdByUsername(oldmoderator);
					if (dt.Rows.Count > 0) //当前用户存在
					{
						int uid = Convert.ToInt32(dt.Rows[0][0].ToString());
						int radminid = Convert.ToInt32(dt.Rows[0][1].ToString());
                        dt = DatabaseProvider.GetInstance().GetUidInModeratorsByUid(currentfid, uid);

						//在其他版块未曾设置为版主  注:(当大于0时则表示在其它版还有相应的版主身份,则不作任何处理)
						if ((dt.Rows.Count == 0) && (radminid != 1))
						{
							UserInfo __userinfo = Users.GetUserInfo(uid);
							UserGroupInfo __usergroupinfo = UserCredits.GetCreditsUserGroupID(__userinfo.Credits);

                            DatabaseProvider.GetInstance().UpdateUserOnlineInfo(__usergroupinfo.Groupid, uid);
                            DatabaseProvider.GetInstance().UpdateUserOtherInfo(__usergroupinfo.Groupid, uid);
						}
					}
				}
			}
		}

		#region  递归指定论坛版块下的所有子版块

		public static string ChildNode = "0";

		/// <summary>
		/// 递归所有子节点并返回字符串
		/// </summary>
		/// <param name="correntfid">当前</param>
		/// <returns>子版块的集合,格式:1,2,3,4,</returns>
		public static string FindChildNode(string correntfid)
		{
			lock (ChildNode)
			{
				//DataTable dt = DbHelper.ExecuteDataset("Select fid From [" + BaseConfigs.GetTablePrefix + "forums] Where [parentid]=" + correntfid + " ORDER BY [displayorder] ASC").Tables[0];
                DataTable dt = DatabaseProvider.GetInstance().GetFidInForumsByParentid(int.Parse(correntfid));
                
				ChildNode = ChildNode + "," + correntfid;

				if (dt.Rows.Count > 0)
				{
					//有子节点
					foreach (DataRow dr in dt.Rows)
					{
						FindChildNode(dr["fid"].ToString());
					}
					dt.Dispose();
				}
				else
				{
					dt.Dispose();
				}
				return ChildNode;
			}
		}

		#endregion

		/// <summary>
		/// 合并版块
		/// </summary>
		/// <param name="sourcefid">源论坛版块</param>
		/// <param name="targetfid">目标论坛版块</param>
		/// <returns></returns>
		public static bool CombinationForums(string sourcefid, string targetfid)
		{
            if (DatabaseProvider.GetInstance().IsExistSubForum(int.Parse(sourcefid)))
			{
				return false;
			}
			else
			{
                ChildNode = "0";
                string fidlist = ("," + FindChildNode(targetfid)).Replace(",0,", "");
                DatabaseProvider.GetInstance().CombinationForums(sourcefid, targetfid, fidlist);

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/UI/ForumListBoxOptions");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/ForumList");

				return true;
			}
		}


		/// <summary>
		/// 设置论坛字版数和显示顺序
		/// </summary>
		public static void SetForumsSubForumCountAndDispalyorder()
		{
            DataTable dt = DatabaseProvider.GetInstance().GetAllForumList();

			foreach (DataRow dr in dt.Rows)
			{
                DatabaseProvider.GetInstance().UpdateSubForumCount(int.Parse(dt.Select("parentid=" + dr["fid"].ToString()).Length.ToString()), int.Parse(dr["fid"].ToString()));
			}

			if (dt.Rows.Count == 1) return;

			int displayorder = 1;
			string fidlist;
			foreach (DataRow dr in dt.Select("parentid=0"))
			{
				if (dr["parentid"].ToString() == "0")
				{
					ChildNode = "0";
					fidlist = ("," + FindChildNode(dr["fid"].ToString())).Replace(",0,", "");

					foreach (string fidstr in fidlist.Split(','))
					{
                        DatabaseProvider.GetInstance().UpdateDisplayorderInForumByFid(displayorder, int.Parse(fidstr));
						displayorder++;
					}

				}
			}
		}


		/// <summary>
		/// 设置论坛版块的状态
		/// </summary>
		public static void SetForumsStatus()
		{
            DataTable dt = DatabaseProvider.GetInstance().GetMainForum();


			foreach (DataRow dr in dt.Rows)
			{
				ChildNode = "0";
				string fidlist = ("," + FindChildNode(dr["fid"].ToString())).Replace(",0,", "");

				if (dr["status"].ToString() == "0")
				{
                    DatabaseProvider.GetInstance().UpdateStatusByFidlist(fidlist);
				}
				else if (dr["status"].ToString() == "1")
				{
                    DatabaseProvider.GetInstance().UpdateStatusByFidlistOther(fidlist);
				}
				else
				{
                    DatabaseProvider.GetInstance().SetStatusInForum(4,int.Parse(dr["fid"].ToString()));

					int i = 5;
                    foreach (DataRow currentdr in DatabaseProvider.GetInstance().GetForumByParentid(int.Parse(dr["fid"].ToString())).Rows)
					{
                        DatabaseProvider.GetInstance().SetStatusInForum(i, int.Parse(currentdr["fid"].ToString()));
						i++;
					}
				}
			}
		}





		/// <summary>
		/// 批理设置论坛信息
		/// </summary>
		/// <param name="__foruminfo">复制的论坛信息</param>
		/// <param name="bsp">是否要批量设置的信息字段</param>
		/// <param name="fidlist">目标论坛(fid)串</param>
		/// <returns></returns>
        public static bool BatchSetForumInf(ForumInfo __foruminfo, BatchSetParams bsp, string fidlist)
        {
            return DatabaseProvider.GetInstance().BatchSetForumInf(__foruminfo, bsp, fidlist);
        }

	}
}