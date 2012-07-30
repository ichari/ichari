using System;
using System.Data;
using System.Data.Common;
using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using System.Text;
using Discuz.Entity;

#if NET1
#else
using Discuz.Common.Generic;
#endif

namespace Discuz.Forum
{
    /// <summary>
    /// 版块操作类
    /// </summary>
    public class Forums
    {

        /// <summary>
        /// 获得默认首页的分类和版块列表
        /// </summary>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="moderstyle">版主显示样式</param>
        /// <param name="topiccount">主题总数</param>
        /// <param name="postcount">帖子总数</param>
        /// <param name="todaycount">今日发帖数</param>
        /// <returns>板块列表的DataTable</returns>
        public static DataTable GetDefalutForumIndexList(int hideprivate, int usergroupid, int moderstyle, out int topiccount, out int postcount, out int todaycount)
        {
            return GetForumIndexList(0, hideprivate, usergroupid, moderstyle, out topiccount, out postcount, out todaycount);
        }

        /// <summary>
        /// 获得分类和版块列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="moderstyle">版主显示样式</param>
        /// <param name="topiccount">主题总数</param>
        /// <param name="postcount">帖子总数</param>
        /// <param name="todaycount">今日发帖数</param>
        /// <returns>板块列表的DataTable</returns>
        public static DataTable GetForumIndexList(int fid, int hideprivate, int usergroupid, int moderstyle, out int topiccount, out int postcount, out int todaycount)
        {
            topiccount = 0;
            postcount = 0;
            todaycount = 0;

            DataTable dt;
            if (fid > 0)
            {
                dt = GetForumList(fid);
            }
            else
            {
                dt = DatabaseProvider.GetInstance().GetForumIndexListTable();
            }


            if (dt != null)
            {
                int status = 0;
                int colcount = 1;
                //int lastparentid = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    if (Int32.Parse(dr["status"].ToString()) > 0)
                    {

                        if (Int32.Parse(dr["parentid"].ToString()) == 0 && Int32.Parse(dr["subforumcount"].ToString()) > 0)
                        {
                            colcount = Int32.Parse(dr["colcount"].ToString());
                            status = colcount;
                            dr["status"] = status + 1;
                        }
                        else
                        {
                            status++;
                            dr["status"] = status;
                            dr["colcount"] = colcount;
                        }
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    if (hideprivate == 1 && dr["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), dr["viewperm"].ToString()))
                    {
                        dr.Delete();
                        continue;

                    }

                    string[] moderatorslist = Utils.SplitString(dr["moderators"].ToString(), ",");
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, sb.Length);
                    }
                    for (int i = 0; i < moderatorslist.Length; i++)
                    {
                        if (moderstyle == 0)
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                if (!sb.ToString().Equals(""))
                                {
                                    sb.Append(",");
                                }
                                sb.Append("<a href=\"userinfo.aspx?username=");
                                sb.Append(Utils.UrlEncode(moderatorslist[i].Trim()));
                                sb.Append("\" target=\"_blank\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</a>");
                            }
                        }
                        else
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                sb.Append("<option value=\"");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</option>");
                            }

                        }
                    }
                    if (!sb.ToString().Equals("") && moderstyle == 1)
                    {
                        sb.Insert(0, "<select style=\"width: 100px;\" onchange=\"window.open('userinfo.aspx?username=' + escape(this.value));\">");
                        sb.Append("</select>");
                    }
                    dr["moderators"] = sb.ToString();


                    if (dr["lastpost"].ToString().Equals(""))
                    {
                        dr["todayposts"] = 0;
                    }
                    else
                    {
                        if (Convert.ToDateTime(dr["lastpost"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            dr["todayposts"] = 0;
                        }
                    }

                    if (Utils.StrToInt(dr["layer"], 0) > 0)
                    {
                        topiccount = topiccount + Utils.StrToInt(dr["topics"], 0);
                        postcount = postcount + Utils.StrToInt(dr["posts"], 0);
                        todaycount = todaycount + Utils.StrToInt(dr["todayposts"], 0);
                    }





                }
                dt.AcceptChanges();
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.AddObject("/NowStatistics", topiccount.ToString() + "," + postcount.ToString() + "," + todaycount.ToString());
                return dt;
            }
            return new DataTable();

        }



        /// <summary>
        /// 获得简介版论坛首页列表
        /// </summary>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <returns>板块列表的DataTable</returns>
        public static DataTable GetArchiverForumIndexList(int hideprivate, int usergroupid)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetArchiverForumIndexList();
            ForumInfo[] forums = GetForumList();
            foreach (DataRow dr in dt.Rows)
            {
                string parentidlist = dr["parentidlist"].ToString().Trim();
                if (dr["status"].ToString() == "0")
                {
                    dr.Delete();
                }
                else
                {
                    foreach (ForumInfo f in forums)
                    {
                        if (Utils.InArray(f.Fid.ToString(), parentidlist) && f.Status == 0)
                        {
                            dr.Delete();
                        }
                    }
                }
            }

            dt.AcceptChanges();
            if (hideprivate != 1)
            {
                return dt;
            }


            foreach (DataRow dr in dt.Rows)
            {
                if (dr["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), dr["viewperm"].ToString()))
                {
                    dr.Delete();
                }
            }
            dt.AcceptChanges();
            return dt;
        }




        /// <summary>
        /// 获得版块下的子版块列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>子版块列表</returns>
        public static DataTable GetForumList(int fid)
        {
            DataTable dt = new DataTable();
            if (fid < 0)
            {
                return dt;
            }

            dt = DatabaseProvider.GetInstance().GetSubForumTable(fid);

            if (dt != null)
            {
                int status = 0;
                int colcount = 1;
                //int lastparentid = 0;
                foreach (DataRow dr in dt.Rows)
                {

                    if (Int32.Parse(dr["status"].ToString()) > 0)
                    {
                        //if (Int32.Parse(dr["subforumcount"].ToString()) == 0 && Int32.Parse(dr["parentid"].ToString()) == lastparentid)
                        if (colcount > 1)
                        {
                            status++;
                            dr["status"] = status;
                            dr["colcount"] = colcount;
                        }
                        else if (Int32.Parse(dr["subforumcount"].ToString()) > 0 && Int32.Parse(dr["colcount"].ToString()) > 0)
                        {
                            //lastparentid = Int32.Parse(dr["fid"].ToString());
                            colcount = Int32.Parse(dr["colcount"].ToString());
                            status = colcount;
                            dr["status"] = status + 1;
                        }
                    }
                }
            }
            return dt;
        }



        /// <summary>
        /// 返回用户所在的用户组是否有权浏览该版块
        /// </summary>
        /// <param name="viewperm">查看权限的用户组id列表</param>
        /// <param name="usergroupid">用户组id</param>
        /// <returns>bool</returns>
        public static bool AllowView(string viewperm, int usergroupid)
        {
            return HasPerm(viewperm, usergroupid);
        }
        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发主题
        /// </summary>
        /// <param name="postperm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns>bool</returns>
        public static bool AllowPost(string postperm, int usergroupid)
        {
            return HasPerm(postperm, usergroupid);
        }
        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发回复
        /// </summary>
        /// <param name="replyperm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns>bool</returns>
        public static bool AllowReply(string replyperm, int usergroupid)
        {
            return HasPerm(replyperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块发主题或恢复
        /// </summary>
        /// <param name="perm">用户组</param>
        /// <param name="usergroupid">用户过在组别</param>
        /// <returns>bool</returns>
        private static bool HasPerm(string perm, int usergroupid)
        {
            if (perm == null || perm.Trim() == "")
            {
                return true;
            }
            return Utils.InArray(usergroupid.ToString(), perm);
        }


        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块下载附件
        /// </summary>
        /// <param name="getattachperm">允许下载附件的用户组id列表</param>
        /// <param name="usergroupid">当前用户组</param>
        /// <returns></returns>
        public static bool AllowGetAttach(string getattachperm, int usergroupid)
        {

            return HasPerm(getattachperm, usergroupid);
        }

        /// <summary>
        /// 返回用户所在的用户组是否有权在该版块上传附件
        /// </summary>
        /// <param name="postattachperm"></param>
        /// <param name="usergroupid"></param>
        /// <returns></returns>
        public static bool AllowPostAttach(string postattachperm, int usergroupid)
        {
            return HasPerm(postattachperm, usergroupid);
        }


        /// <summary>
        /// 返回全部版块列表并缓存
        /// </summary>
        /// <returns>板块信息数组</returns>
        public static ForumInfo[] GetForumList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            ForumInfo[] forumlist = cache.RetrieveObject("/ForumList") as ForumInfo[];
            if (forumlist == null)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetForumsTable();
                if (dt.Rows.Count > 0)
                {
                    forumlist = new ForumInfo[dt.Rows.Count];
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        forumlist[i] = new ForumInfo();
                        forumlist[i].Fid = Int32.Parse(dr["fid"].ToString());
                        forumlist[i].Parentid = Int16.Parse(dr["parentid"].ToString());
                        forumlist[i].Layer = Int16.Parse(dr["layer"].ToString());
                        forumlist[i].Name = dr["name"].ToString();
                        forumlist[i].Pathlist = dr["pathlist"].ToString();
                        forumlist[i].Parentidlist = dr["parentidlist"].ToString();
                        forumlist[i].Subforumcount = Int32.Parse(dr["subforumcount"].ToString());
                        forumlist[i].Status = Int32.Parse(dr["status"].ToString());
                        forumlist[i].Colcount = Int16.Parse(dr["colcount"].ToString());
                        forumlist[i].Displayorder = Int32.Parse(dr["displayorder"].ToString());
                        forumlist[i].Templateid = Int16.Parse(dr["templateid"].ToString());
                        forumlist[i].Topics = Int32.Parse(dr["topics"].ToString());
                        forumlist[i].CurrentTopics = Int32.Parse(dr["curtopics"].ToString());
                        forumlist[i].Posts = Int32.Parse(dr["posts"].ToString());

                        //当前版块的最后发帖日期为空，则表示今日发帖数为0 
                        if (dr["lastpost"].ToString().Equals(""))
                        {
                            dr["todayposts"] = 0;
                        }
                        else
                        {
                            //当系统日期与最发发帖日期不同时，则表示今日发帖数为0 
                            if (Convert.ToDateTime(dr["lastpost"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                            {
                                dr["todayposts"] = 0;
                            }
                        }

                        forumlist[i].Todayposts = Int32.Parse(dr["todayposts"].ToString());
                        forumlist[i].Lastpost = dr["lastpost"].ToString();
                        forumlist[i].Lastposter = dr["lastposter"].ToString();
                        forumlist[i].Lastposterid = Int32.Parse(dr["lastposterid"].ToString());
                        forumlist[i].Lasttitle = dr["lasttitle"].ToString();
                        forumlist[i].Allowsmilies = Int32.Parse(dr["allowsmilies"].ToString());
                        forumlist[i].Allowrss = Int32.Parse(dr["allowrss"].ToString());
                        forumlist[i].Allowhtml = Int32.Parse(dr["allowhtml"].ToString());
                        forumlist[i].Allowbbcode = Int32.Parse(dr["allowbbcode"].ToString());
                        forumlist[i].Allowimgcode = Int32.Parse(dr["allowimgcode"].ToString());
                        forumlist[i].Allowblog = Int32.Parse(dr["allowblog"].ToString());
                        forumlist[i].Istrade = Int32.Parse(dr["istrade"].ToString());
                        forumlist[i].Allowpostspecial = Int32.Parse(dr["allowpostspecial"].ToString());
                        forumlist[i].Allowspecialonly = Int32.Parse(dr["allowspecialonly"].ToString());
                        forumlist[i].Alloweditrules = Int32.Parse(dr["alloweditrules"].ToString());
                        forumlist[i].Allowthumbnail = Int32.Parse(dr["allowthumbnail"].ToString());
                        forumlist[i].Allowtag = Int32.Parse(dr["allowtag"].ToString());
                        forumlist[i].Recyclebin = Int32.Parse(dr["recyclebin"].ToString());
                        forumlist[i].Modnewposts = Int32.Parse(dr["modnewposts"].ToString());
                        forumlist[i].Jammer = Int32.Parse(dr["jammer"].ToString());
                        forumlist[i].Disablewatermark = Int32.Parse(dr["disablewatermark"].ToString());
                        forumlist[i].Inheritedmod = Int32.Parse(dr["inheritedmod"].ToString());
                        forumlist[i].Autoclose = Int16.Parse(dr["autoclose"].ToString());

                        forumlist[i].Description = dr["description"].ToString();
                        forumlist[i].Password = dr["password"].ToString();
                        forumlist[i].Icon = dr["icon"].ToString();
                        forumlist[i].Postcredits = dr["postcredits"].ToString();
                        forumlist[i].Replycredits = dr["replycredits"].ToString();
                        forumlist[i].Redirect = dr["redirect"].ToString();
                        forumlist[i].Attachextensions = dr["attachextensions"].ToString();
                        forumlist[i].Moderators = dr["moderators"].ToString();
                        forumlist[i].Rules = dr["rules"].ToString();
                        forumlist[i].Topictypes = dr["topictypes"].ToString();
                        forumlist[i].Viewperm = dr["viewperm"].ToString();
                        forumlist[i].Postperm = dr["postperm"].ToString();
                        forumlist[i].Replyperm = dr["replyperm"].ToString();
                        forumlist[i].Getattachperm = dr["getattachperm"].ToString();
                        forumlist[i].Postattachperm = dr["postattachperm"].ToString();
                        forumlist[i].Applytopictype = Int16.Parse(dr["applytopictype"] == DBNull.Value ? "0" : dr["applytopictype"].ToString());
                        forumlist[i].Postbytopictype = Int16.Parse(dr["postbytopictype"] == DBNull.Value ? "0" : dr["postbytopictype"].ToString());
                        forumlist[i].Viewbytopictype = Int16.Parse(dr["viewbytopictype"] == DBNull.Value ? "0" : dr["viewbytopictype"].ToString());
                        forumlist[i].Topictypeprefix = Int16.Parse(dr["topictypeprefix"] == DBNull.Value ? "0" : dr["topictypeprefix"].ToString());
                        forumlist[i].Permuserlist = dr["permuserlist"].ToString();
                        i++;
                    }

                    //声明新的缓存策略接口
                    Discuz.Cache.ICacheStrategy ics = new ForumCacheStrategy();
                    ics.TimeOut = 5;
                    cache.LoadCacheStrategy(ics);
                    cache.AddObject("/ForumList", forumlist);
                    cache.LoadDefaultCacheStrategy();
                }
            }

            return forumlist;
        }

        /// <summary>
        /// 获得指定的分类或版块信息
        /// </summary>
        /// <param name="fid">分类或版块ID</param>
        /// <returns>返回分类或版块的信息</returns>
        public static ForumInfo GetForumInfo(int fid)
        {
            ForumInfo[] forumlist = GetForumList();
            if (forumlist == null)
            {
                return new ForumInfo();
            }

            foreach (ForumInfo foruminfo in forumlist)
            {

                if (foruminfo.Fid == fid)
                {
                    foruminfo.Pathlist = foruminfo.Pathlist.Replace("a><a", "a> &raquo; <a");
                    return foruminfo;
                }
            }
            return null;

        }


        /// <summary>
        /// 设置当前版块主题数(不含子版块)
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <returns>主题数</returns>
        public static int SetRealCurrentTopics(int fid)
        {
            return DatabaseProvider.GetInstance().SetRealCurrentTopics(fid);
        }

        /// <summary>
        /// 获得可见的板块列表
        /// </summary>
        /// <returns>返回值是以英文逗号分割的板块ID</returns>
        public static string GetVisibleForum()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            ForumInfo[] forumlist = GetForumList();
            if (forumlist == null)
            {
                return string.Empty;
            }

            foreach (ForumInfo foruminfo in forumlist)
            {

                if (foruminfo.Status > 0)
                {
                    if (foruminfo.Viewperm == null || foruminfo.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                    {
                        if (UserGroups.GetUserGroupInfo(7).Allowvisit != 1)
                        {
                            continue;
                        }
                    }
                    else //当板块权限不为空，按照板块权限
                    {
                        if (!AllowView(foruminfo.Viewperm, 7))
                        {
                            continue;
                        }
                    }
                    result.AppendFormat(",{0}", foruminfo.Fid);
                }
            }

            if (result.Length > 0)
                return result.Remove(0, 1).ToString();
            else
                return string.Empty;
        }


        /// <summary>
        /// 得到当前版块的主题类型选项
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <returns>主题类型字符串</returns>
        public static string GetCurrentTopicTypesOption(int fid, string topictypes)
        {
            //判断当前版块没有相应主题分类时
            if (Utils.StrIsNullOrEmpty(topictypes) || topictypes == "|")
            {
                return "";
            }

            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            string topictypeoptions = cache.RetrieveObject("/TopicTypesOption" + fid) as string;
            if (topictypeoptions != null)
            {
                return topictypeoptions;
            }

            StringBuilder builder = new StringBuilder();
            //if (GetForumInfo(fid).Postbytopictype != 1)
            //{
            //builder.Append("<option value=\"0\"></option>");
            //}
            foreach (string topictype in topictypes.Split('|'))
            {
                if (topictype.Trim() != "")
                {
                    builder.Append("<option value=\"");
                    builder.Append(topictype.Split(',')[0]);
                    builder.Append("\">");
                    builder.Append(topictype.Split(',')[1]);
                    builder.Append("</option>");
                }
            }
            cache.AddObject("/TopicTypesOption" + fid, builder.ToString());

            return builder.ToString();
        }
        /// <summary>
        /// 得到当前版块的主题类型链接串 
        /// </summary>
        /// <param name="fid">板块ID</param>
        /// <returns>当前版块的主题类型链接串</returns>
        public static string GetCurrentTopicTypesLink(int fid, string topictypes, string fullpagename)
        {
            if ((topictypes == null) || (topictypes == ""))
            {
                return "";
            }

            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            string topictypelinks = cache.RetrieveObject("/TopicTypesLink" + fid) as string;
            if (topictypelinks != null)
            {
                return topictypelinks;
            }

            StringBuilder builder = new StringBuilder();
            StringBuilder dropbuilder = new StringBuilder();
            foreach (string topictype in topictypes.Split('|'))
            {
                if (topictype.Trim() != "")
                {
                    if (topictype.Split(',')[2] == "0") //平版模式
                    {
                        builder.Append("<a href=\"");
                        builder.Append(fullpagename + "?forumid=" + fid);
                        builder.Append("&typeid=");
                        builder.Append(topictype.Split(',')[0]);
                        //sb.Append("&search=1");
                        builder.Append("\">");
                        builder.Append(topictype.Split(',')[1]);
                        builder.Append("</a>&nbsp;&nbsp;");
                    }
                    else	//下拉类型
                    {
                        dropbuilder.Append("<p><a href=\"");
                        dropbuilder.Append(fullpagename + "?forumid=" + fid);
                        dropbuilder.Append("&typeid=");
                        dropbuilder.Append(topictype.Split(',')[0]);
                        //dropsb.Append("&search=1");
                        dropbuilder.Append("\">");
                        dropbuilder.Append(topictype.Split(',')[1]);
                        dropbuilder.Append("</a></p>");
                    }
                }
            }

            if (dropbuilder.ToString() != "")
            {
                builder.Append("<span id=\"topictypedrop\" onmouseover=\"showMenu(this.id, true);\" style=\"CURSOR:pointer\"><a href=\"###\">更多分类 ...</a></span>");
                builder.Append("<div class=\"popupmenu_popup popupmenu_topictype\" id=\"topictypedrop_menu\" style=\"DISPLAY: none\">");
                builder.Append("<div class=\"popupmenu_topictypeoption\">");
                builder.Append(dropbuilder.ToString().Trim());
                builder.Append("</div>");
                builder.Append("</div>");
            }

            cache.AddObject("/TopicTypesLink" + fid, builder.ToString());

            return builder.ToString();
        }



        public enum ForumSpecialUserPower
        {
            ViewByUser = 1,
            PostByUser = 2,
            ReplyByUser = 4,
            DownloadAttachByUser = 8,
            PostAttachByUser = 16
        }


        /// <summary>
        /// 获取指定版块特殊用户的权限
        /// </summary>
        /// <param name="Permuserlist"></param>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        private static int GetForumSpecialUserPower(string Permuserlist, int userid)
        {
            foreach (string currentinf in Permuserlist.Split('|'))
            {
                if (currentinf != "")
                {
                    if (currentinf.Split(',')[1] == userid.ToString())
                    {
                        return Int16.Parse(currentinf.Split(',')[2]);
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// 返回用户是否有权浏览该版块
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>BOOL</returns>
        public static bool AllowViewByUserID(string Permuserlist, int userid)
        {
            if (Permuserlist != null || Permuserlist.Trim() != "")
            {
                ForumSpecialUserPower forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(Permuserlist, userid);
                if (((int)(forumspecialuserpower & ForumSpecialUserPower.ViewByUser)) > 0)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 返回用户是否有权在该版块发主题
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowPostByUserID(string Permuserlist, int userid)
        {
            if (Permuserlist != null || Permuserlist.Trim() != "")
            {
                ForumSpecialUserPower __forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(Permuserlist, userid);
                if (((int)(__forumspecialuserpower & ForumSpecialUserPower.PostByUser)) > 0)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 返回用户是否有权在该版块发回复
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowReplyByUserID(string Permuserlist, int userid)
        {
            if (Permuserlist != null || Permuserlist.Trim() != "")
            {
                ForumSpecialUserPower forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(Permuserlist, userid);

                if (((int)(forumspecialuserpower & ForumSpecialUserPower.ReplyByUser)) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回用户是否有权在该版块下载附件
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowGetAttachByUserID(string Permuserlist, int userid)
        {
            if (Permuserlist != null || Permuserlist.Trim() != "")
            {
                ForumSpecialUserPower forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(Permuserlist, userid);
                if (((int)(forumspecialuserpower & ForumSpecialUserPower.DownloadAttachByUser)) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 返回用户是否有权在该版块上传附件
        /// </summary>
        /// <param name="Permuserlist">查看当前版块的相关权限</param>
        /// <param name="userid">查看权限的用户id</param>
        /// <returns>bool</returns>
        public static bool AllowPostAttachByUserID(string Permuserlist, int userid)
        {
            if (Permuserlist != null || Permuserlist.Trim() != "")
            {
                ForumSpecialUserPower forumspecialuserpower = (ForumSpecialUserPower)GetForumSpecialUserPower(Permuserlist, userid);
                if (((int)(forumspecialuserpower & ForumSpecialUserPower.PostAttachByUser)) > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定的主题分类是否属于本版块可用的范围之内
        /// </summary>
        /// <param name="typeid">主题分类Id</param>
        /// <param name="topictypes">本版可用的主题分类</param>
        /// <returns>bool</returns>
        public static bool IsCurrentForumTopicType(string typeid, string topictypes)
        {
            if (topictypes == null || topictypes == string.Empty)
            {
                return true;
            }

            foreach (string topictype in topictypes.Split('|'))
            {
                if (topictype.Trim() != string.Empty)
                {
                    if (typeid.Trim() == topictype.Split(',')[0].Trim())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 获取版块列表
        /// </summary>
        /// <returns>DataTable</returns>
        public static DataTable GetForumListTable()
        {
            return DatabaseProvider.GetInstance().GetForumListTable();
        }

        /// <summary>
        /// 更新指定版块的最新发帖数信息
        /// </summary>
        /// <param name="foruminfo"></param>
        public static void UpdateLastPost(ForumInfo foruminfo)
        {
            PostInfo postinfo = new PostInfo();

            int tid = DatabaseProvider.GetInstance().GetLastPostTid(foruminfo, Forums.GetVisibleForum());
            if (tid > 0)
            {
                DataTable dt = Posts.GetLastPostByTid(tid);
                if (dt.Rows.Count > 0)
                {
                    postinfo.Pid = Convert.ToInt32(dt.Rows[0]["pid"].ToString());
                    postinfo.Tid = Convert.ToInt32(dt.Rows[0]["tid"].ToString());
                    postinfo.Title = dt.Rows[0]["title"].ToString().Trim();
                    postinfo.Postdatetime = dt.Rows[0]["postdatetime"].ToString().Trim();
                    postinfo.Poster = dt.Rows[0]["poster"].ToString().Trim();
                    postinfo.Posterid = Convert.ToInt32(dt.Rows[0]["posterid"].ToString());
                    postinfo.Topictitle = Topics.GetTopicInfo(postinfo.Tid).Title;
                }
                else
                {
                    postinfo.Pid = 0;
                    postinfo.Tid = 0;
                    postinfo.Title = "从未";
                    postinfo.Topictitle = "从未";
                    postinfo.Postdatetime = "1900-1-1";
                    postinfo.Poster = "";
                    postinfo.Posterid = 0;
                }
                dt.Dispose();
            }
            else
            {
                postinfo.Pid = 0;
                postinfo.Tid = 0;
                postinfo.Title = "从未";
                postinfo.Topictitle = "从未";
                postinfo.Postdatetime = "1900-1-1";
                postinfo.Poster = "";
                postinfo.Posterid = 0;
            }

            DatabaseProvider.GetInstance().UpdateLastPost(foruminfo, postinfo);

            if (foruminfo.Layer > 0) //递归调用并更新相应父版块信息
            {
                foruminfo = Forums.GetForumInfo(foruminfo.Parentid);
                UpdateLastPost(foruminfo);
            }
        }



        #region 条件编译的方法
#if NET1


        private static IndexPageForumInfoCollection GetRealForumIndexCollection(IndexPageForumInfoCollection forumIndexCollection)
        {
            IndexPageForumInfoCollection parentforums = new IndexPageForumInfoCollection();
            IndexPageForumInfoCollection subforums = new IndexPageForumInfoCollection();
            IndexPageForumInfoCollection result = new IndexPageForumInfoCollection();
#else
        /// <summary>
        /// 
        /// </summary>
        /// <param name="forumIndexCollection"></param>
        /// <returns></returns>
        private static List<IndexPageForumInfo> GetRealForumIndexCollection(List<IndexPageForumInfo> forumIndexCollection)
        {
            List<IndexPageForumInfo> parentforums = new List<IndexPageForumInfo>();
            List<IndexPageForumInfo> subforums = new List<IndexPageForumInfo>();
            List<IndexPageForumInfo> result = new List<IndexPageForumInfo>();
#endif
            foreach (IndexPageForumInfo forum in forumIndexCollection)
            {
                if (forum.Parentid == 0)
                    parentforums.Add(forum);
                else
                    subforums.Add(forum);
            }

            foreach (IndexPageForumInfo forum in parentforums)
            {
                if (forum.Colcount > 1)
                {
                    int realsubcount = 0;
                    foreach (IndexPageForumInfo sub in subforums)
                    {
                        if (forum.Fid == sub.Parentid)
                        {
                            realsubcount++;
                        }
                    }

                    //forum.Colcount = Math.Min(forum.Colcount, realsubcount);
                }
            }

            foreach (IndexPageForumInfo forum in parentforums)
            {
                result.Add(forum);

                foreach (IndexPageForumInfo sub in subforums)
                {
                    if (sub.Parentid == forum.Fid)
                    {
                        sub.Colcount = forum.Colcount;
                        result.Add(sub);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取首页版块列表集合
        /// </summary>
        /// <param name="fid"></param>
        /// <param name="hideprivate"></param>
        /// <param name="usergroupid"></param>
        /// <param name="moderstyle"></param>
        /// <param name="topiccount"></param>
        /// <param name="postcount"></param>
        /// <param name="todaycount"></param>
        /// <returns></returns>
#if NET1
        public static IndexPageForumInfoCollection GetForumIndexCollection(int hideprivate, int usergroupid, int moderstyle, out int topiccount, out int postcount, out int todaycount)
        {
            IndexPageForumInfoCollection coll = new IndexPageForumInfoCollection();
#else
        public static Discuz.Common.Generic.List<IndexPageForumInfo> GetForumIndexCollection(int hideprivate, int usergroupid, int moderstyle, out int topiccount, out int postcount, out int todaycount)
        {
            Discuz.Common.Generic.List<IndexPageForumInfo> coll = new Discuz.Common.Generic.List<IndexPageForumInfo>();
#endif

            topiccount = 0;
            postcount = 0;
            todaycount = 0;

            IDataReader reader = DatabaseProvider.GetInstance().GetForumIndexList();



            if (reader != null)
            {
                int status = 0;
                int colcount = 1;

                while (reader.Read())
                {
                    IndexPageForumInfo info = new IndexPageForumInfo();
                    //赋值
                    info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Parentid = Int16.Parse(reader["parentid"].ToString());
                    info.Layer = Int16.Parse(reader["layer"].ToString());
                    info.Name = reader["name"].ToString();
                    info.Pathlist = reader["pathlist"].ToString();
                    info.Parentidlist = reader["parentidlist"].ToString();
                    info.Subforumcount = Int32.Parse(reader["subforumcount"].ToString());
                    info.Status = Int32.Parse(reader["status"].ToString());
                    info.Colcount = Int16.Parse(reader["colcount"].ToString());
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Templateid = Int16.Parse(reader["templateid"].ToString());
                    info.Topics = Int32.Parse(reader["topics"].ToString());
                    info.CurrentTopics = Int32.Parse(reader["curtopics"].ToString());
                    info.Posts = Int32.Parse(reader["posts"].ToString());
                    info.Todayposts = Int32.Parse(reader["todayposts"].ToString());
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lasttid = Int32.Parse(reader["lasttid"].ToString());
                    info.Lastposterid = Int32.Parse(reader["lastposterid"].ToString());
                    info.Lasttitle = reader["lasttitle"].ToString();
                    info.Allowsmilies = Int32.Parse(reader["allowsmilies"].ToString());
                    info.Allowrss = Int32.Parse(reader["allowrss"].ToString());
                    info.Allowhtml = Int32.Parse(reader["allowhtml"].ToString());
                    info.Allowbbcode = Int32.Parse(reader["allowbbcode"].ToString());
                    info.Allowimgcode = Int32.Parse(reader["allowimgcode"].ToString());
                    info.Allowblog = Int32.Parse(reader["allowblog"].ToString());
                    info.Istrade = Int32.Parse(reader["istrade"].ToString());
                    info.Allowpostspecial = Int32.Parse(reader["allowpostspecial"].ToString());
                    info.Allowspecialonly = Int32.Parse(reader["allowspecialonly"].ToString());
                    info.Alloweditrules = Int32.Parse(reader["alloweditrules"].ToString());
                    info.Allowthumbnail = Int32.Parse(reader["allowthumbnail"].ToString());
                    info.Recyclebin = Int32.Parse(reader["recyclebin"].ToString());
                    info.Modnewposts = Int32.Parse(reader["modnewposts"].ToString());
                    info.Jammer = Int32.Parse(reader["jammer"].ToString());
                    info.Disablewatermark = Int32.Parse(reader["disablewatermark"].ToString());
                    info.Inheritedmod = Int32.Parse(reader["inheritedmod"].ToString());
                    info.Autoclose = Int16.Parse(reader["autoclose"].ToString());

                    info.Description = reader["description"].ToString();
                    info.Password = reader["password"].ToString();
                    info.Icon = reader["icon"].ToString();
                    info.Postcredits = reader["postcredits"].ToString();
                    info.Replycredits = reader["replycredits"].ToString();
                    info.Redirect = reader["redirect"].ToString();
                    info.Attachextensions = reader["attachextensions"].ToString();
                    info.Moderators = reader["moderators"].ToString();
                    info.Rules = reader["rules"].ToString();
                    info.Topictypes = reader["topictypes"].ToString();
                    info.Viewperm = reader["viewperm"].ToString();
                    info.Postperm = reader["postperm"].ToString();
                    info.Replyperm = reader["replyperm"].ToString();
                    info.Getattachperm = reader["getattachperm"].ToString();
                    info.Postattachperm = reader["postattachperm"].ToString();
                    info.Applytopictype = Int16.Parse(reader["applytopictype"] == DBNull.Value ? "0" : reader["applytopictype"].ToString());
                    info.Postbytopictype = Int16.Parse(reader["postbytopictype"] == DBNull.Value ? "0" : reader["postbytopictype"].ToString());
                    info.Viewbytopictype = Int16.Parse(reader["viewbytopictype"] == DBNull.Value ? "0" : reader["viewbytopictype"].ToString());
                    info.Topictypeprefix = Int16.Parse(reader["topictypeprefix"] == DBNull.Value ? "0" : reader["topictypeprefix"].ToString());
                    info.Permuserlist = reader["permuserlist"].ToString();

                    //扩展属性
                    info.Havenew = reader["havenew"].ToString();

                    //判断是否收起
                    if (info.Layer == 0 && Utils.GetCookie("discuz_collapse").IndexOf("_category_" + info.Fid + "_") > -1)
                    {
                        info.Collapse = "display: none;";
                    }
                    //
                    if (Int32.Parse(reader["status"].ToString()) > 0)
                    {

                        if (Int32.Parse(reader["parentid"].ToString()) == 0 && Int32.Parse(reader["subforumcount"].ToString()) > 0)
                        {
                            colcount = Int32.Parse(reader["colcount"].ToString());
                            status = colcount;
                            info.Status = status + 1;
                        }
                        else
                        {
                            status++;
                            info.Status = status;
                            info.Colcount = colcount;
                        }
                    }

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    //如果当前用户权限不够


                    string[] moderatorslist = Utils.SplitString(reader["moderators"].ToString(), ",");
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, sb.Length);
                    }
                    for (int i = 0; i < moderatorslist.Length; i++)
                    {
                        if (moderstyle == 0)
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                if (!sb.ToString().Equals(""))
                                {
                                    sb.Append(",");
                                }
                                sb.Append("<a href=\"userinfo.aspx?username=");
                                sb.Append(Utils.UrlEncode(moderatorslist[i].Trim()));
                                sb.Append("\" target=\"_blank\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</a>");
                            }
                        }
                        else
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                sb.Append("<option value=\"");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</option>");
                            }

                        }
                    }
                    if (!sb.ToString().Equals("") && moderstyle == 1)
                    {
                        sb.Insert(0, "<select style=\"width: 100px;\" onchange=\"window.open('userinfo.aspx?username=' + escape(this.value));\">");
                        sb.Append("</select>");
                    }
                    info.Moderators = sb.ToString();


                    if (reader["lastpost"].ToString().Equals(""))
                    {
                        info.Todayposts = 0;
                    }
                    else
                    {
                        if (Convert.ToDateTime(reader["lastpost"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            info.Todayposts = 0;
                        }
                    }

                    if (Utils.StrToInt(reader["layer"], 0) > 0)
                    {
                        topiccount = topiccount + Utils.StrToInt(reader["topics"], 0);
                        postcount = postcount + Utils.StrToInt(reader["posts"], 0);
                        todaycount = todaycount + info.Todayposts;
                    }

                    //判断是否为私密论坛
                    if (reader["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), reader["viewperm"].ToString()))
                    {
                        if (hideprivate == 1)
                        {
                            //不显示
                        }
                        else
                        {
                            info.Lasttitle = "";
                            info.Lastposter = "";
                            info.Status = -1;
                            coll.Add(info);
                        }
                    }
                    else
                    {
                        coll.Add(info);
                    }
                }

                reader.Close();

                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.AddObject("/NowStatistics", topiccount.ToString() + "," + postcount.ToString() + "," + todaycount.ToString());

            }
            //return co
            return GetRealForumIndexCollection(coll);

        }

        /// <summary>
        /// 获得子版块列表
        /// </summary>
        /// <param name="fid">版块id</param>
        /// <param name="colcount">每行显示几个版块</param>
        /// <param name="hideprivate">是否显示无权限的版块</param>
        /// <param name="usergroupid">用户组id</param>
        /// <param name="moderstyle">版主显示样式</param>
        /// <returns></returns>
#if NET1
        public static IndexPageForumInfoCollection GetSubForumCollection(int fid, int colcount, int hideprivate, int usergroupid, int moderstyle)
        {
            IndexPageForumInfoCollection coll = new IndexPageForumInfoCollection();
#else
        public static Discuz.Common.Generic.List<IndexPageForumInfo> GetSubForumCollection(int fid, int colcount, int hideprivate, int usergroupid, int moderstyle)
        {
            Discuz.Common.Generic.List<IndexPageForumInfo> coll = new Discuz.Common.Generic.List<IndexPageForumInfo>();
#endif

            if (fid <= 0)
            {
                return coll;
            }

            IDataReader reader = DatabaseProvider.GetInstance().GetSubForumReader(fid);

            if (reader != null)
            {

                while (reader.Read())
                {
                    IndexPageForumInfo info = new IndexPageForumInfo();
                    //赋值
                    info.Fid = Int32.Parse(reader["fid"].ToString());
                    info.Parentid = Int16.Parse(reader["parentid"].ToString());
                    info.Layer = Int16.Parse(reader["layer"].ToString());
                    info.Name = reader["name"].ToString();
                    info.Pathlist = reader["pathlist"].ToString();
                    info.Parentidlist = reader["parentidlist"].ToString();
                    info.Subforumcount = Int32.Parse(reader["subforumcount"].ToString());
                    info.Status = Int32.Parse(reader["status"].ToString());
                    if (colcount > 0)
                    {
                        info.Colcount = colcount;
                    }
                    else
                    {
                        info.Colcount = Int16.Parse(reader["colcount"].ToString());
                    }
                    info.Displayorder = Int32.Parse(reader["displayorder"].ToString());
                    info.Templateid = Int16.Parse(reader["templateid"].ToString());
                    info.Topics = Int32.Parse(reader["topics"].ToString());
                    info.CurrentTopics = Int32.Parse(reader["curtopics"].ToString());
                    info.Posts = Int32.Parse(reader["posts"].ToString());
                    info.Todayposts = Int32.Parse(reader["todayposts"].ToString());
                    info.Lastpost = reader["lastpost"].ToString();
                    info.Lastposter = reader["lastposter"].ToString();
                    info.Lasttid = Int32.Parse(reader["lasttid"].ToString());
                    info.Lastposterid = Int32.Parse(reader["lastposterid"].ToString());
                    info.Lasttitle = reader["lasttitle"].ToString();
                    info.Allowsmilies = Int32.Parse(reader["allowsmilies"].ToString());
                    info.Allowrss = Int32.Parse(reader["allowrss"].ToString());
                    info.Allowhtml = Int32.Parse(reader["allowhtml"].ToString());
                    info.Allowbbcode = Int32.Parse(reader["allowbbcode"].ToString());
                    info.Allowimgcode = Int32.Parse(reader["allowimgcode"].ToString());
                    info.Allowblog = Int32.Parse(reader["allowblog"].ToString());
                    info.Istrade = Int32.Parse(reader["istrade"].ToString());
                    info.Allowpostspecial = Int32.Parse(reader["allowpostspecial"].ToString());
                    info.Allowspecialonly = Int32.Parse(reader["allowspecialonly"].ToString());
                    info.Alloweditrules = Int32.Parse(reader["alloweditrules"].ToString());
                    info.Allowthumbnail = Int32.Parse(reader["allowthumbnail"].ToString());
                    info.Recyclebin = Int32.Parse(reader["recyclebin"].ToString());
                    info.Modnewposts = Int32.Parse(reader["modnewposts"].ToString());
                    info.Jammer = Int32.Parse(reader["jammer"].ToString());
                    info.Disablewatermark = Int32.Parse(reader["disablewatermark"].ToString());
                    info.Inheritedmod = Int32.Parse(reader["inheritedmod"].ToString());
                    info.Autoclose = Int16.Parse(reader["autoclose"].ToString());

                    info.Description = reader["description"].ToString();
                    info.Password = reader["password"].ToString();
                    info.Icon = reader["icon"].ToString();
                    info.Postcredits = reader["postcredits"].ToString();
                    info.Replycredits = reader["replycredits"].ToString();
                    info.Redirect = reader["redirect"].ToString();
                    info.Attachextensions = reader["attachextensions"].ToString();
                    info.Moderators = reader["moderators"].ToString();
                    info.Rules = reader["rules"].ToString();
                    info.Topictypes = reader["topictypes"].ToString();
                    info.Viewperm = reader["viewperm"].ToString();
                    info.Postperm = reader["postperm"].ToString();
                    info.Replyperm = reader["replyperm"].ToString();
                    info.Getattachperm = reader["getattachperm"].ToString();
                    info.Postattachperm = reader["postattachperm"].ToString();
                    info.Applytopictype = Int16.Parse(reader["applytopictype"].ToString());
                    info.Postbytopictype = Int16.Parse(reader["postbytopictype"].ToString());
                    info.Viewbytopictype = Int16.Parse(reader["viewbytopictype"].ToString());
                    info.Topictypeprefix = Int16.Parse(reader["topictypeprefix"].ToString());
                    info.Permuserlist = reader["permuserlist"].ToString();

                    //扩展属性
                    info.Havenew = reader["havenew"].ToString();

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();


                    string[] moderatorslist = Utils.SplitString(reader["moderators"].ToString(), ",");
                    if (sb.Length > 0)
                    {
                        sb.Remove(0, sb.Length);
                    }
                    for (int i = 0; i < moderatorslist.Length; i++)
                    {
                        if (moderstyle == 0)
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                if (!sb.ToString().Equals(""))
                                {
                                    sb.Append(",");
                                }
                                sb.Append("<a href=\"userinfo.aspx?username=");
                                sb.Append(Utils.UrlEncode(moderatorslist[i].Trim()));
                                sb.Append("\" target=\"_blank\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</a>");
                            }
                        }
                        else
                        {
                            if (!moderatorslist[i].Trim().Equals(""))
                            {
                                sb.Append("<option value=\"");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("\">");
                                sb.Append(moderatorslist[i].Trim());
                                sb.Append("</option>");
                            }

                        }
                    }
                    if (!sb.ToString().Equals("") && moderstyle == 1)
                    {
                        sb.Insert(0, "<select style=\"width: 100px;\" onchange=\"window.open('userinfo.aspx?username=' + escape(this.value));\">");
                        sb.Append("</select>");
                    }
                    info.Moderators = sb.ToString();


                    if (reader["lastpost"].ToString().Equals(""))
                    {
                        info.Todayposts = 0;
                    }
                    else
                    {
                        if (Convert.ToDateTime(reader["lastpost"]).ToString("yyyy-MM-dd") != DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            info.Todayposts = 0;
                        }
                    }

                    if (reader["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), reader["viewperm"].ToString()))
                    {
                        if (hideprivate == 1)
                        {
                            //不显示
                        }
                        else
                        {
                            info.Lasttitle = "";
                            info.Lastposter = "";
                            info.Status = -1;
                            coll.Add(info);
                        }
                    }
                    else
                    {
                        coll.Add(info);
                    }






                }
                reader.Close();

            }
            return coll;
        }

        #endregion
    }
}
