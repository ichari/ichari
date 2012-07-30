using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Plugin.Preview;
using Discuz.Common.Generic;
using System.Web;

namespace Discuz.Forum
{
    /// <summary>
    /// 帖子操作类
    /// </summary>
    public class Posts
    {
        private static Regex regexAttach = new Regex(@"\[attach\](\d+?)\[\/attach\]", RegexOptions.IgnoreCase);

        private static Regex regexHide = new Regex(@"\s*\[hide\][\n\r]*([\s\S]+?)[\n\r]*\[\/hide\]\s*", RegexOptions.IgnoreCase);

        private static Regex regexAttachImg = new Regex(@"\[attachimg\](\d+?)\[\/attachimg\]", RegexOptions.IgnoreCase);


        /// <summary>
        /// 得到用户帖子分表信息
        /// </summary>
        /// <returns>分表记录集</returns>
        public static DataTable GetAllPostTableName()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            DataTable dt = cache.RetrieveObject("/PostTableName") as DataTable;

            if (dt == null)
            {
                DataSet ds = DatabaseProvider.GetInstance().GetAllPostTableName();
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        cache.AddObject("/PostTableName", dt);
                    }
                }
                ds.Dispose();
            }
            return dt;
        }

        /// <summary>
        /// 得到当前所用帖子表分表ID
        /// </summary>
        /// <returns>分表ID</returns>
        public static string GetPostTableID()
        {

            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();

            string id = cache.RetrieveObject("/LastPostTableName") as string;
            if (id == null)
            {
                id = "1";
                DataTable dt = GetAllPostTableName();
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        id = dt.Rows[0]["id"].ToString();
                    }
                }
                dt.Dispose();
                cache.AddObject("/LastPostTableName", id);
            }
            return id;
        }

        /// <summary>
        /// 得到当前所用帖子表分表的表名
        /// </summary>
        /// <returns>分表表名</returns>
        public static string GetPostTableName()
        {
            return string.Format("{0}posts{1}", BaseConfigs.GetTablePrefix, GetPostTableID());
        }

        /// <summary>
        /// 得到指定主题的帖子所在分表ID
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>分表ID</returns>
        public static string GetPostTableID(int tid)
        {
            if (tid < 1)
            {
                return GetPostTableID();
            }

            string id = "1";
            DataTable dt = GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("[mintid]<={0} AND ([maxtid]<=0 OR [maxtid]>={0})", tid.ToString()));

                if (dr != null)
                {
                    if (dr.Length > 0)
                    {
                        id = dr[dr.Length - 1]["id"].ToString();
                    }
                }
            }
            dt.Dispose();
            return id;
        }

        /// <summary>
        /// 得到指定主题的帖子所分表的表名
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <returns>分表表名</returns>
        public static string GetPostTableName(int tid)
        {
            return BaseConfigs.GetTablePrefix + "posts" + GetPostTableID(tid);
        }



        /// <summary>
        /// 得到多个主题的帖子所在分表的ID
        /// </summary>
        /// <param name="topicidlist">主题ID列表</param>
        /// <returns>分表ID字符数组</returns>
        public static string[] GetPostTableIDList(string topicidlist)
        {

            string id = "";
            if (!Utils.IsNumericArray(topicidlist.Split(',')))
            {
                return null;
            }

            string[] reval = topicidlist.Split(',');
            int mintid = Utils.StrToInt(reval[0], 0);
            int maxtid = Utils.StrToInt(reval[0], 0);
            for (int i = 0; i < reval.Length; i++)
            {
                if (mintid > Utils.StrToInt(reval[i], 0))
                {
                    mintid = Utils.StrToInt(reval[i], 0);
                }

                if (maxtid < Utils.StrToInt(reval[i], 0))
                {
                    maxtid = Utils.StrToInt(reval[i], 0);
                }
            }

            DataTable dt = GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select("mintid<=" + maxtid.ToString() + " AND (maxtid<=0 OR maxtid>=" + mintid.ToString() + ")");
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (!id.Equals(""))
                        {
                            id = id + ",";
                        }
                        id = id + dr[dr.Length - 1]["id"].ToString();
                    }
                }
            }
            dt.Dispose();
            return id.Split(',');
        }

        /// <summary>
        /// 根据最大主题ID和最小主题ID得到帖子所在分表的ID
        /// </summary>
        /// <param name="mintid">最小主题ID</param>
        /// <param name="maxtid">最大主题ID</param>
        /// <returns>分表ID字符数组</returns>
        public static string[] GetPostTableIDList(int mintid, int maxtid)
        {

            string id = "";
            if (mintid > maxtid)
            {
                return null;
            }

            DataTable dt = GetAllPostTableName();
            if (dt != null)
            {
                DataRow[] dr = dt.Select(string.Format("[mintid]<={0} AND ([maxtid]<=0 OR [maxtid]>={1})", maxtid.ToString(), mintid.ToString()));
                if (dr != null)
                {
                    for (int i = 0; i < dr.Length; i++)
                    {
                        if (!id.Equals(""))
                        {
                            id = id + ",";
                        }
                        id = id + dr[i]["id"].ToString();
                    }
                }
            }
            dt.Dispose();
            return id.Split(',');
        }

        /// <summary>
        /// 创建帖子
        /// </summary>
        /// <param name="postinfo">帖子信息类</param>
        /// <returns>返回帖子id</returns>
        public static int CreatePost(PostInfo postinfo)
        {
            int pid = DatabaseProvider.GetInstance().CreatePost(postinfo, GetPostTableID(postinfo.Tid));

            //本帖具有正反方立场
            if (postinfo.Debateopinion > 0)
            {
                DebatePostExpandInfo dpei = new DebatePostExpandInfo();
                dpei.Tid = postinfo.Tid;
                dpei.Pid = pid;
                dpei.Opinion = postinfo.Debateopinion;
                dpei.Diggs = 0;
                CreateDebatePostExpand(dpei);
            }

            return pid;
        }

        /// <summary>
        /// 创建参加辩论帖子的扩展信息
        /// </summary>
        /// <param name="dpei">参与辩论的帖子的辩论相关扩展字段实体</param>
        public static void CreateDebatePostExpand(DebatePostExpandInfo dpei)
        {
            DatabaseProvider.GetInstance().CreateDebatePostExpand(dpei);
        }



        /// <summary>
        /// 更新指定帖子信息
        /// </summary>
        /// <param name="postsInfo">帖子信息</param>
        /// <returns>更新数量</returns>
        public static int UpdatePost(PostInfo postsInfo)
        {
            return DatabaseProvider.GetInstance().UpdatePost(postsInfo, GetPostTableID(postsInfo.Tid));
        }


        /// <summary>
        /// 删除指定ID的帖子
        /// </summary>
        /// <param name="posttableid">帖子所在分表Id</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="reserveattach">保留附件</param>
        /// <returns>删除数量</returns>
        public static int DeletePost(string posttableid, int pid, bool reserveattach,bool chanageposts)
        {
            if (!reserveattach)
            {//删除附件 
                Attachments.DeleteAttachmentByPid(pid);
            }

            return DatabaseProvider.GetInstance().DeletePost(posttableid, pid,chanageposts);
        }


        /// <summary>
        /// 获得指定的帖子描述信息
        /// </summary>
        /// <param name="tid">tid</param>
        /// <param name="pid">帖子id</param>
        /// <returns>帖子描述信息</returns>
        public static PostInfo GetPostInfo(int tid, int pid)
        {
            PostInfo postinfo = new PostInfo();

			IDataReader reader = null;

            if (tid < 1)
            {
                foreach (DataRow dr in GetAllPostTableName().Rows)
                {
                    reader = DatabaseProvider.GetInstance().GetPostInfo(dr["id"].ToString(), pid);
                    if (reader != null)
                    {
                        break;
                    }
                }
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetPostInfo(GetPostTableID(tid), pid);
            }
            //if (reader != null)
            //{
                if (reader.Read())
                {
                    postinfo.Pid = pid;
                    postinfo.Fid = Int32.Parse(reader["fid"].ToString());
                    postinfo.Tid = Int32.Parse(reader["tid"].ToString());
                    postinfo.Parentid = Int32.Parse(reader["parentid"].ToString());
                    postinfo.Layer = Int32.Parse(reader["layer"].ToString());
                    postinfo.Poster = reader["poster"].ToString();
                    postinfo.Posterid = Int32.Parse(reader["posterid"].ToString());
                    postinfo.Title = reader["title"].ToString();
                    postinfo.Postdatetime = reader["postdatetime"].ToString();
                    postinfo.Message = reader["message"].ToString();
                    postinfo.Ip = reader["ip"].ToString();
                    postinfo.Lastedit = reader["lastedit"].ToString();
                    postinfo.Invisible = Int32.Parse(reader["invisible"].ToString());
                    postinfo.Usesig = Int32.Parse(reader["usesig"].ToString());
                    postinfo.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                    postinfo.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                    postinfo.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                    postinfo.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                    postinfo.Attachment = Int32.Parse(reader["attachment"].ToString());
                    postinfo.Rate = Int32.Parse(reader["rate"].ToString());
                    postinfo.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                    reader.Close();
                    return postinfo;
                }
                reader.Close();
            //}
            return null;
        }



        /// <summary>
        /// 获得指定主题的帖子列表
        /// </summary>
        /// <param name="topiclist">主题ID列表</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPostList(string topiclist)
        {
            if (!Utils.IsNumericArray(Utils.SplitString(topiclist, ",")))
            {
                return null;
            }

            string[] posttableid = GetPostTableIDList(topiclist);
            if (posttableid == null)
            {
                return null;
            }
            if (posttableid.Length < 1)
            {
                return null;
            }

            DataSet ds = DatabaseProvider.GetInstance().GetPostList(topiclist, posttableid);
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    return ds.Tables[0];
                }
            }

            return null;
        }

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static DataTable GetPostListTitle(int Tid)
        {
            return DatabaseProvider.GetInstance().GetPostListTitle(Tid, GetPostTableName(Tid));
        }


        /// <summary>
        /// 获取加载附件信息的帖子内容
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <param name="allowGetAttach">是否允许获取附件</param>
        /// <param name="attHidArray">隐藏在hide标签中的附件数组</param>
        /// <param name="drpost">帖子信息</param>
        /// <param name="drAttach">附件信息</param>
        /// <param name="message">帖子内容</param>
        /// <returns>帖子内容</returns>
        private static string GetMessageWithAttachInfo(PostpramsInfo postpramsinfo, int allowGetAttach, string[] attHidArray, ShowtopicPagePostInfo postinfo, ShowtopicPageAttachmentInfo attinfo, string message)
        {
            string forumPath = BaseConfigs.GetBaseConfig().Forumpath;
            string filesize;
            string replacement;
            if (Utils.InArray(attinfo.Aid.ToString(), attHidArray))
                return message;
            if ((Utils.StrToInt(attinfo.Readperm, 0) <= postpramsinfo.Usergroupreadaccess || postinfo.Posterid == postpramsinfo.CurrentUserid) && allowGetAttach == 1)
                attinfo.Allowread = 1;
            else
                attinfo.Allowread = 0;

            attinfo.Getattachperm = allowGetAttach;

            attinfo.Filename = attinfo.Filename.ToString().Replace("\\", "/");

            if (message.IndexOf("[attach]" + attinfo.Aid.ToString() + "[/attach]") != -1 || message.IndexOf("[attachimg]" + attinfo.Aid.ToString() + "[/attachimg]") != -1)
            {
                if (allowGetAttach != 1)
                {
                    replacement = "<br /><img src=\"/images/attachicons/attachment.gif\" alt=\"\">&nbsp;附件: <span class=\"attachnotdown\">您所在的用户组无法下载或查看附件</span>";
                }
                else if (attinfo.Allowread == 1)
                {
                    if (attinfo.Filesize > 1024)
                    {
                        filesize = Convert.ToString(Math.Round(Convert.ToDecimal(attinfo.Filesize) / 1024, 2)) + " K";
                    }
                    else
                    {
                        filesize = attinfo.Filesize + " B";
                    }

                    if (Utils.IsImgFilename(attinfo.Attachment))
                    {
                        attinfo.Attachimgpost = 1;

                        if (postpramsinfo.Showattachmentpath == 1)
                        {
                            if (postpramsinfo.Isforspace == 1)
                            {
                                if (attinfo.Filename.ToLower().IndexOf("http") == 0)
                                {
                                    replacement = "<img src=\"" + attinfo.Filename + "\" />";
                                }
                                else
                                {
                                    replacement = "<img src=\"" + forumPath + "upload/" + attinfo.Filename + "\" />";
                                }
                            }
                            else
                            {
                                if (attinfo.Filename.ToLower().IndexOf("http") == 0)
                                {
                                    replacement = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_" + attinfo.Aid + "\"><img border=\"0\" src=\"" + forumPath + "images/attachicons/attachimg.gif\" /></span><img src=\"" + attinfo.Filename + "\" onload=\"attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 0, event)\" /><div id=\"attach_" + attinfo.Aid + "_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"" + forumPath + "images/attachicons/image.gif\" /><a target=\"_blank\" href=\"" + attinfo.Filename + "\"><strong>" + attinfo.Attachment + "</strong></a>(" + filesize + ")<br/><div class=\"t_smallfont\">" + attinfo.Postdatetime + "</div></div>";
                                }
                                else
                                {
                                    replacement = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_" + attinfo.Aid + "\"><img border=\"0\" src=\"" + forumPath + "images/attachicons/attachimg.gif\" /></span><img src=\"" + forumPath + "upload/" + attinfo.Filename + "\" onload=\"attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 0, event)\" /><div id=\"attach_" + attinfo.Aid + "_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"" + forumPath + "images/attachicons/image.gif\" /><a target=\"_blank\" href=\"" + forumPath + "upload/" + attinfo.Filename + "\"><strong>" + attinfo.Attachment + "</strong></a>(" + filesize + ")<br/><div class=\"t_smallfont\">" + attinfo.Postdatetime + "</div></div>";
                                }

                            }
                        }
                        else
                        {
                            if (postpramsinfo.Isforspace == 1)
                            {
                                replacement = "<img src=\"" + forumPath + "attachment.aspx?attachmentid=" + attinfo.Aid.ToString() + "\" />";
                            }
                            else
                            {
                                replacement = "<span style=\"position: absolute; display: none;\" onmouseover=\"showMenu(this.id, 0, 1)\" id=\"attach_" + attinfo.Aid + "\"><img border=\"0\" src=\"" + forumPath + "images/attachicons/attachimg.gif\" /></span><img src=\"" + forumPath + "attachment.aspx?attachmentid=" + attinfo.Aid.ToString() + "\" onload=\"attachimg(this, 'load');\" onmouseover=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 1);attachimg(this, 'mouseover')\" onclick=\"zoom(this);\" onmouseout=\"attachimginfo(this, 'attach_" + attinfo.Aid + "', 0, event)\" /><div id=\"attach_" + attinfo.Aid + "_menu\" style=\"display: none;\" class=\"t_attach\"><img border=\"0\" alt=\"\" class=\"absmiddle\" src=\"" + forumPath + "images/attachicons/image.gif\" /><a target=\"_blank\" href=\"" + forumPath + "attachment.aspx?attachmentid=" + attinfo.Aid.ToString() + "\"><strong>" + attinfo.Attachment + "</strong></a>(" + filesize + ")<br/><div class=\"t_smallfont\">" + attinfo.Postdatetime + "</div></div>";
                            }
                        }
                    }
                    else
                    {
                        attinfo.Attachimgpost = 0;
                        replacement = string.Format("<p><img alt=\"\" src=\"{0}images/attachicons/attachment.gif\" border=\"0\" /><span class=\"bold\">附件</span>: <a href=\"{0}attachment.aspx?attachmentid={1}\" target=\"_blank\">{2}</a> ({3}, {4})<br />该附件被下载次数 {5}</p>", forumPath, attinfo.Aid.ToString(), attinfo.Attachment.ToString().Trim(), attinfo.Postdatetime, filesize, attinfo.Downloads.ToString());
                    }
                }
                else
                {
                    replacement = string.Format("<br /><span class=\"notdown\">你的下载权限 {0} 低于此附件所需权限 {1}, 你无法查看此附件</span>", postpramsinfo.Usergroupreadaccess.ToString(), attinfo.Readperm.ToString());
                }

                Regex r = new Regex(string.Format(@"\[attach\]{0}\[/attach\]|\[attachimg\]{0}\[/attachimg\]", attinfo.Aid));
                message = r.Replace(message, replacement, 1);

                message = message.Replace("[attach]" + attinfo.Aid.ToString() + "[/attach]", string.Empty);
                message = message.Replace("[attachimg]" + attinfo.Aid.ToString() + "[/attachimg]", string.Empty);

                if (attinfo.Pid == postinfo.Pid)
                {
                    attinfo.Pid = 0;
                }
            }
            else
            {
                if (attinfo.Pid == postinfo.Pid)
                {
                    if (Utils.IsImgFilename(attinfo.Attachment))
                    {
                        attinfo.Attachimgpost = 1;
                    }
                    else
                    {
                        attinfo.Attachimgpost = 0;
                    }

                    //加载文件预览类指定方法
                    IPreview preview = PreviewProvider.GetInstance(Path.GetExtension(attinfo.Filename).Remove(0, 1).Trim());

                    if (preview != null)
                    {
                        //当支持FTP上传附件时
                        if (FTPs.GetForumAttachInfo.Allowupload == 1)
                        {
                            preview.UseFTP = true;
                            attinfo.Preview = preview.GetPreview(attinfo.Filename, attinfo);
                        }
                        else
                        {
                            preview.UseFTP = false;
                            attinfo.Preview = preview.GetPreview(AppDomain.CurrentDomain.BaseDirectory + "/upload/" + attinfo.Filename, attinfo);
                        }
                    }
                }
            }
            return message;
        }


        /// <summary>
        /// 获取被包含在[hide]标签内的附件id
        /// </summary>
        /// <param name="content">帖子内容</param>
        /// <param name="hide">隐藏标记</param>
        /// <returns>隐藏的附件id数组</returns>
        private static string[] GetHiddenAttachIdList(string content, int hide)
        {
            if (hide == 0)
            {
                return new string[0];
            }


            StringBuilder tmpStr = new StringBuilder();
            StringBuilder hidContent = new StringBuilder();
            foreach (Match m in regexHide.Matches(content))
            {
                if (hide == 1)
                {
                    hidContent.Append(m.Groups[0].ToString());
                }
            }


            foreach (Match ma in regexAttach.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            foreach (Match ma in regexAttachImg.Matches(hidContent.ToString()))
            {
                tmpStr.Append(ma.Groups[1].ToString());
                tmpStr.Append(",");
            }

            if (tmpStr.Length == 0)
            {
                return new string[0];
            }


            return tmpStr.Remove(tmpStr.Length - 1, 1).ToString().Split(',');
        }


        /// <summary>
        /// 返回指定主题的最后回复帖子
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>指定主题的最后回复帖子</returns>
        public static DataTable GetLastPostByTid(int tid)
        {
            return DatabaseProvider.GetInstance().GetLastPostByTid(tid, GetPostTableName(tid));
        }

        /// <summary>
        /// 获得最后回复的帖子列表
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetLastPostList(PostpramsInfo postpramsinfo)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetLastPostList(postpramsinfo, GetPostTableName(postpramsinfo.Tid));
            if (dt == null)
            {
                dt = new DataTable();
                dt.Columns.Add("adindex", Type.GetType("System.Int32"));
                return dt;
            }


            dt.Columns.Add("adindex", Type.GetType("System.Int32"));
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));
            int adcount = Advertisements.GetInPostAdCount("", postpramsinfo.Fid);

            foreach (DataRow dr in dt.Rows)
            {

                //　ubb转为html代码在页面显示
                postpramsinfo.Smileyoff = Utils.StrToInt(dr["smileyoff"], 0);
                postpramsinfo.Bbcodeoff = Utils.StrToInt(dr["bbcodeoff"], 0);
                postpramsinfo.Parseurloff = Utils.StrToInt(dr["parseurloff"], 0);
                postpramsinfo.Allowhtml = Utils.StrToInt(dr["htmlon"], 0);
                postpramsinfo.Pid = Utils.StrToInt(dr["pid"], 0);
                postpramsinfo.Sdetail = dr["message"].ToString();

                if (postpramsinfo.Price > 0 && Utils.StrToInt(dr["layer"], 0) == 0)
                {
                    dr["message"] = string.Format("<div class=\"paystyle\">此帖为交易帖,要付 {0} <span class=\"bold\">{1}</span>{2} 才可查看</div>", Scoresets.GetScoreSet(Scoresets.GetCreditsTrans()).Name, postpramsinfo.Price, Scoresets.GetScoreSet(Scoresets.GetCreditsTrans()).Unit);
                }
                else
                {
                    if (!postpramsinfo.Ubbmode)
                    {
                        dr["message"] = UBB.UBBToHTML(postpramsinfo);
                    }
                    else
                    {
                        dr["message"] = Utils.HtmlEncode(dr["message"].ToString());
                    }
                }
                dr["adindex"] = random.Next(0, adcount);

                //是不是加干扰码
                if (postpramsinfo.Jammer == 1)
                {
                    dr["message"] = ForumUtils.AddJammer(dr["message"].ToString());
                }

                //是不是隐藏会员email
                if (Utils.StrToInt(dr["showemail"], 0) == 1)
                {
                    dr["email"] = "";
                }

            }

            return dt;
        }




        /// <summary>
        /// 获取指定tid的帖子DataTable
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子DataTable</returns>
        public static DataTable GetPostTree(int tid)
        {
            DataTable dt = DatabaseProvider.GetInstance().GetPostTree(tid, GetPostTableID(tid));
            dt.Columns.Add("spaces", Type.GetType("System.String"));
            foreach (DataRow dr in dt.Rows)
            {
                dr["spaces"] = Utils.GetSpacesString(Utils.StrToInt(dr["layer"].ToString(), 0));
                dr["message"] = Utils.CutString(Utils.HtmlEncode(dr["message"].ToString()), 0, 50);
                if (!dr["message"].Equals(""))
                {
                    dr["title"] = dr["message"];
                }

            }

            return dt;
        }

        /// <summary>
        /// 获取指定tid的帖子总数
        /// </summary>
        /// <param name="tid">帖子的tid</param>
        /// <returns>指定tid的帖子总数</returns>
        public static int GetPostCount(int tid)
        {
            return DatabaseProvider.GetInstance().GetPostCountByTid(tid, GetPostTableName(tid));
        }


        ///// <summary>
        ///// 按条件获取指定tid的帖子总数
        ///// </summary>
        ///// <param name="tid">帖子的tid</param>
        ///// <returns>指定tid的帖子总数</returns>
        //public static int GetPostCount(int tid, string condition)
        //{
        //    return DatabaseProvider.GetInstance().GetPostCount(tid, condition);
        //}

        /// <summary>
        /// 获得指定主题的第一个帖子的id
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <returns>帖子id</returns>
        public static int GetFirstPostId(int tid)
        {
            return DatabaseProvider.GetInstance().GetFirstPostId(tid, GetPostTableID(tid));
        }

        /// <summary>
        /// 判断指定用户是否是指定主题的回复者
        /// </summary>
        /// <param name="tid">主题id</param>
        /// <param name="uid">用户id</param>
        /// <returns>是否是指定主题的回复者</returns>
        public static bool IsReplier(int tid, int uid)
        {
            return DatabaseProvider.GetInstance().IsReplier(tid, uid, GetPostTableID(tid));
        }


        /// <summary>
        /// 更新帖子的评分值
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postidlist">帖子ID列表</param>
        /// <returns>更新的帖子数量</returns>
        public static int UpdatePostRateTimes(int tid, string postidlist,int ratetimes)
        {
            if (!Utils.IsNumericArray(Utils.SplitString(postidlist, ",")))
            {
                return 0;
            }

            return DatabaseProvider.GetInstance().UpdatePostRateTimes(ratetimes, postidlist, GetPostTableID(tid));
        }

        /// <summary>
        /// 获取帖子评分列表
        /// </summary>
        /// <param name="pid">帖子列表</param>
        /// <returns>帖子评分列表</returns>
        public static DataTable GetPostRateList(int pid)
        {
            return DatabaseProvider.GetInstance().GetPostRateList(pid, GeneralConfigs.GetConfig().DisplayRateCount);
        }

        /// <summary>
        /// 得到空间格式的帖子内容
        /// </summary>
        /// <param name="postinfo">帖子描述</param>
        /// <param name="attArray">附件集合</param>
        /// <returns>空间格式</returns>
        public static string GetSpacePostMessage(PostInfo postinfo, AttachmentInfo[] attArray)
        {
            string message = "";
            PostpramsInfo postpramsinfo = new PostpramsInfo();
            //ShowtopicPageAttachmentInfo[] sAtt = (ShowtopicPageAttachmentInfo[])attachmentinfo;
            //处理帖子内容
            postpramsinfo.Smileyoff = postinfo.Smileyoff;
            postpramsinfo.Bbcodeoff = postinfo.Bbcodeoff;
            postpramsinfo.Parseurloff = postinfo.Parseurloff;
            postpramsinfo.Allowhtml = postinfo.Htmlon;
            postpramsinfo.Sdetail = postinfo.Message;
            postpramsinfo.Showimages = 1 - postinfo.Smileyoff;
            postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsinfo.Pid = postinfo.Pid;
            //强制隐藏hide内容
            postpramsinfo.Hide = 1;
            //设定这是为个人空间进行的解析
            postpramsinfo.Isforspace = 1;

            //先简单判断是否是动网兼容模式
            if (!postpramsinfo.Ubbmode)
            {
                message = UBB.UBBToHTML(postpramsinfo);
            }
            else
            {
                message = Utils.HtmlEncode(postinfo.Message);
            }

            if (postpramsinfo.Jammer == 1)
            {
                message = ForumUtils.AddJammer(postinfo.Message);
            }

            if (postinfo.Attachment > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
            {


                //获取在[hide]标签中的附件id
                string[] attHidArray = GetHiddenAttachIdList(postpramsinfo.Sdetail, postpramsinfo.Hide);

                ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();
                info.Posterid = postinfo.Posterid;
                info.Pid = postinfo.Pid;

                for (int i = 0; i < attArray.Length; i++)
                {
                    ShowtopicPageAttachmentInfo sAtt = new ShowtopicPageAttachmentInfo();
                    sAtt.Aid = attArray[i].Aid;
                    sAtt.Attachment = attArray[i].Attachment;
                    sAtt.Description = attArray[i].Description;
                    sAtt.Downloads = attArray[i].Downloads;
                    sAtt.Filename = attArray[i].Filename;
                    sAtt.Filesize = attArray[i].Filesize;
                    sAtt.Filetype = attArray[i].Filetype;
                    sAtt.Pid = attArray[i].Pid;
                    sAtt.Postdatetime = attArray[i].Postdatetime;
                    sAtt.Readperm = attArray[i].Readperm;
                    sAtt.Sys_index = attArray[i].Sys_index;
                    sAtt.Sys_noupload = attArray[i].Sys_noupload;
                    sAtt.Tid = attArray[i].Tid;
                    sAtt.Uid = attArray[i].Uid;
                    message = GetMessageWithAttachInfo(postpramsinfo, 1, attHidArray, info, sAtt, message);
                }

            }
            return message;
        }

#if NET1
    
        #region 帖子集合方法

               /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static ShowtopicPagePostInfoCollection GetPostList(PostpramsInfo _postpramsinfo, out ShowtopicPageAttachmentInfoCollection attcoll, bool ismoder)
        {

            ShowtopicPagePostInfoCollection postcoll = new ShowtopicPagePostInfoCollection();
            attcoll = new ShowtopicPageAttachmentInfoCollection();

            IDataReader reader;
            StringBuilder pidlist = new StringBuilder();
            if (!_postpramsinfo.Condition.Equals(""))
            {
                reader = DatabaseProvider.GetInstance().GetPostListByCondition(_postpramsinfo, GetPostTableName(_postpramsinfo.Tid));
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetPostList(_postpramsinfo, GetPostTableName(_postpramsinfo.Tid));
            }



            if (reader != null)
            {
                int allowGetAttach = 0;
                if (_postpramsinfo.Getattachperm.Equals("") || _postpramsinfo.Getattachperm == null)
                {
                    allowGetAttach = _postpramsinfo.CurrentUserGroup.Allowgetattach;
                }
                else
                {
                    if (Forums.AllowGetAttach(_postpramsinfo.Getattachperm, _postpramsinfo.Usergroupid))
                    {
                        allowGetAttach = 1;
                    }

                }







                //序号(楼层)的初值
                int id = (_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize;

                //保存初始的hide值
                int oldHide = _postpramsinfo.Hide;

                //取帖间广告
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int fid = _postpramsinfo.Fid;
                int adcount = Advertisements.GetInPostAdCount("", fid);

                //用户组
                UserGroupInfo tmpGroupInfo;

                while (reader.Read())
                {
                    //当帖子中的posterid字段为0时, 表示该数据出现异常
                    if (Int32.Parse(reader["posterid"].ToString()) == 0)
                    {
                        continue;
                    }

                    ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();
                    info.Pid = Int32.Parse(reader["pid"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    if (info.Attachment > 0)
                    {
                        pidlist.Append(",");
                        pidlist.Append(reader["pid"].ToString());
                    }
                    info.Fid = fid;
                    info.Title = reader["title"].ToString().Trim();
                    info.Layer = Int32.Parse(reader["layer"].ToString());
                    info.Message = reader["message"].ToString().Trim();
                    info.Lastedit = reader["lastedit"].ToString().Trim();
                    info.Postdatetime = reader["postdatetime"].ToString().Trim();

                    info.Poster = reader["poster"].ToString().Trim();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Invisible = Int32.Parse(reader["invisible"].ToString());
                    info.Usesig = Int32.Parse(reader["usesig"].ToString());
                    info.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                    info.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                    info.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                    info.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                    if (info.Posterid > 0)
                    {
                        info.Nickname = reader["nickname"].ToString().Trim();
                        info.Username = reader["username"].ToString().Trim();
                        info.Groupid = Utils.StrToInt(reader["groupid"], 0);
                        info.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                        info.Gender = Utils.StrToInt(reader["gender"], 2);
                        info.Bday = reader["bday"].ToString().Trim();
                        info.Showemail = Utils.StrToInt(reader["showemail"], 0);
                        info.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                        info.Credits = Utils.StrToInt(reader["credits"], 0);
                        info.Extcredits1 = reader["extcredits1"] == DBNull.Value ? 0 : float.Parse(reader["extcredits1"].ToString());
                        info.Extcredits2 = reader["extcredits2"] == DBNull.Value ? 0 : float.Parse(reader["extcredits2"].ToString());
                        info.Extcredits3 = reader["extcredits3"] == DBNull.Value ? 0 : float.Parse(reader["extcredits3"].ToString());
                        info.Extcredits4 = reader["extcredits4"] == DBNull.Value ? 0 : float.Parse(reader["extcredits4"].ToString());
                        info.Extcredits5 = reader["extcredits5"] == DBNull.Value ? 0 : float.Parse(reader["extcredits5"].ToString());
                        info.Extcredits6 = reader["extcredits6"] == DBNull.Value ? 0 : float.Parse(reader["extcredits6"].ToString());
                        info.Extcredits7 = reader["extcredits7"] == DBNull.Value ? 0 : float.Parse(reader["extcredits7"].ToString());
                        info.Extcredits8 = reader["extcredits8"] == DBNull.Value ? 0 : float.Parse(reader["extcredits8"].ToString());
                        info.Posts = Utils.StrToInt(reader["posts"], 0);
                        info.Joindate = reader["joindate"].ToString().Trim();
                        info.Lastactivity = reader["lastactivity"].ToString().Trim();
                        info.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                        info.Avatar = reader["avatar"].ToString();
                        info.Avatarwidth = Utils.StrToInt(reader["avatarwidth"], 0);
                        info.Avatarheight = Utils.StrToInt(reader["avatarheight"], 0);
                        info.Medals = reader["medals"].ToString();
                        info.Signature = reader["signature"].ToString();
                        info.Location = reader["location"].ToString();
                        info.Customstatus = reader["customstatus"].ToString();
                        info.Website = reader["website"].ToString();
                        info.Icq = reader["icq"].ToString();
                        info.Qq = reader["qq"].ToString();
                        info.Msn = reader["msn"].ToString();
                        info.Yahoo = reader["yahoo"].ToString();
                        info.Skype = reader["skype"].ToString();
                        //部分属性需要根据不同情况来赋值

                        //根据用户自己的设置决定是否显示邮箱地址
                        if (info.Showemail == 0)
                        {
                            info.Email = "";
                        }
                        else
                        {
                            info.Email = reader["email"].ToString().Trim();
                        }


                        // 最后活动时间50分钟内的为在线, 否则不在线
                        if (Utils.StrDateDiffMinutes(info.Lastactivity, 50) < 0)
                        {
                            info.Onlinestate = 1;
                        }
                        else
                        {
                            info.Onlinestate = 0;
                        }


                        //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                        info.Ip = reader["ip"].ToString().Trim();

                        // 勋章
                        if (info.Medals == "")
                        {
                            info.Medals = "";
                        }
                        else
                        {
                            info.Medals = Caches.GetMedalsList(info.Medals);
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                        info.Stars = tmpGroupInfo.Stars;
                        if (tmpGroupInfo.Color.Equals(""))
                        {
                            info.Status = tmpGroupInfo.Grouptitle;
                        }
                        else
                        {
                            info.Status = string.Format("<span style=\"color:{0}\">{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);
                        }
                    }
                    else
                    {
                        info.Nickname = "游客";
                        info.Username = "游客";
                        info.Groupid = 7;
                        info.Showemail = 0;
                        info.Digestposts = 0;
                        info.Credits = 0;
                        info.Extcredits1 = 0;
                        info.Extcredits2 = 0;
                        info.Extcredits3 = 0;
                        info.Extcredits4 = 0;
                        info.Extcredits5 = 0;
                        info.Extcredits6 = 0;
                        info.Extcredits7 = 0;
                        info.Extcredits8 = 0;
                        info.Posts = 0;
                        info.Joindate = "2006-9-1 1:1:1";
                        info.Lastactivity = "2006-9-1 1:1:1"; ;
                        info.Userinvisible = 0;
                        info.Avatar = "";
                        info.Avatarwidth = 0;
                        info.Avatarheight = 0;
                        info.Medals = "";
                        info.Signature = "";
                        info.Location = "";
                        info.Customstatus = "";
                        info.Website = "";
                        info.Icq = "";
                        info.Qq = "";
                        info.Msn = "";
                        info.Yahoo = "";
                        info.Skype = "";
                        //部分属性需要根据不同情况来赋值
                        info.Email = "";
                        info.Onlinestate = 1;
                        info.Medals = "";

                        info.Ip = reader["ip"].ToString().Trim();
                        if (info.Ip.IndexOf('.') > -1)
                        {
                            info.Ip = info.Ip.Substring(0, info.Ip.LastIndexOf(".") + 1) + "*";
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(7);
                        info.Stars = tmpGroupInfo.Stars;
                        info.Status = "游客";

                    }
                    //扩展属性
                    id++;
                    info.Id = id;
                    info.Adindex = random.Next(0, adcount);

                    if (!Utils.InArray(info.Groupid.ToString(), "4,5,6"))
                    {
                        //处理帖子内容
                        _postpramsinfo.Smileyoff = info.Smileyoff;
                        _postpramsinfo.Bbcodeoff = info.Bbcodeoff;
                        _postpramsinfo.Parseurloff = info.Parseurloff;
                        _postpramsinfo.Allowhtml = info.Htmlon;
                        _postpramsinfo.Sdetail = info.Message;
                        //校正hide处理
                        if (tmpGroupInfo.Allowhidecode == 0)
                        {
                            _postpramsinfo.Hide = 0;
                        }


                        //先简单判断是否允许使用Discuz!NT代码,如果不允许,效率起见直接不进行代码转换 (虽然UBB.UBBToHTML方法内也进行判断)
                        if (!_postpramsinfo.Ubbmode)
                        {
                            info.Message = UBB.UBBToHTML(_postpramsinfo);
                        }
                        else
                        {
                            info.Message = Utils.HtmlEncode(info.Message);
                        }

                        if (_postpramsinfo.Jammer == 1)
                        {
                            info.Message = ForumUtils.AddJammer(info.Message);
                        }


                        //恢复hide初值
                        _postpramsinfo.Hide = oldHide;
                    }
                    else//发帖人已经被禁止发言
                    {
						if (ismoder)
						{
							info.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + info.Message;
						}
						else
						{
							info.Message = "该用户帖子内容已被屏蔽";
						}
                    }

                    postcoll.Add(info);
                }
                reader.Close();

                if (pidlist.Length > 0)
                {
                    pidlist.Remove(0, 1);
                    IDataReader attachreader = DatabaseProvider.GetInstance().GetAttachmentListByPid(pidlist.ToString());
                    if (attachreader != null)
                    {
                        while (attachreader.Read())
                        {
                            ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                            //info.Uid = Int32.Parse(reader["uid"].ToString());
                            attinfo.Aid = Int32.Parse(attachreader["aid"].ToString());
                            attinfo.Tid = Int32.Parse(attachreader["tid"].ToString());
                            attinfo.Pid = Int32.Parse(attachreader["pid"].ToString());
                            attinfo.Postdatetime = attachreader["postdatetime"].ToString();
                            attinfo.Readperm = Int32.Parse(attachreader["readperm"].ToString());
                            attinfo.Filename = attachreader["filename"].ToString();
                            attinfo.Description = attachreader["description"].ToString();
                            attinfo.Filetype = attachreader["filetype"].ToString();
                            attinfo.Filesize = Int32.Parse(attachreader["filesize"].ToString());
                            attinfo.Attachment = attachreader["attachment"].ToString();
                            attinfo.Downloads = Int32.Parse(attachreader["downloads"].ToString());
                            attcoll.Add(attinfo);
                        }
                    }
                    attachreader.Close();
                }


                foreach (ShowtopicPagePostInfo info in postcoll)
                {
                    if (info.Groupid != 4 && info.Groupid != 5)
                    {
                        string message = info.Message;
                        if (info.Attachment > 0 || regexAttach.IsMatch(message))
                        {

                            //获取在[hide]标签中的附件id
                            string[] attHidArray = GetHiddenAttachIdList(_postpramsinfo.Sdetail, _postpramsinfo.Hide);

                            for (int i = 0; i < attcoll.Count; i++)
                            {
                                message = GetMessageWithAttachInfo(_postpramsinfo, allowGetAttach, attHidArray, info, attcoll[i], message);
                                if (Utils.InArray(attcoll[i].Aid.ToString(), attHidArray))
                                {
                                    attcoll.RemoveAt(i);
                                }
                            }
                            info.Message = message;

                        }
                    }
                    else
                    {
                        for (int i = 0; i < attcoll.Count; i++)
                        {
                            if (attcoll[i].Pid == info.Pid)
                            {
                                attcoll.RemoveAt(i);
                            }
                        }
                    }
                }
            }

            return postcoll;
        }



        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        
        public static ShowtopicPagePostInfo GetSinglePost(PostpramsInfo _postpramsinfo, out ShowtopicPageAttachmentInfoCollection attcoll, bool ismoder)
        {
            ShowtopicPagePostInfo info = null;
            attcoll = new ShowtopicPageAttachmentInfoCollection();
			IDataReader attachments = null;

            ///得到帖子对应主题的所有附件
            IDataReader reader = DatabaseProvider.GetInstance().GetSinglePost(out attachments,_postpramsinfo, GetPostTableID(_postpramsinfo.Tid));
            int allowGetAttach = 0;

            if (reader != null)
            {

                if (_postpramsinfo.Getattachperm.Equals("") || _postpramsinfo.Getattachperm == null)
                {
                    allowGetAttach = _postpramsinfo.CurrentUserGroup.Allowgetattach;
                }
                else
                {
                    if (Forums.AllowGetAttach(_postpramsinfo.Getattachperm, _postpramsinfo.Usergroupid))
                    {
                        allowGetAttach = 1;
                    }

                }
				if (attachments != null)
				{



					while (attachments.Read())
					{
						ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
						//info.Uid = Int32.Parse(reader["uid"].ToString());
						attinfo.Aid = Int32.Parse(attachments["aid"].ToString());
						attinfo.Tid = Int32.Parse(attachments["tid"].ToString());
						attinfo.Pid = Int32.Parse(attachments["pid"].ToString());
						attinfo.Postdatetime = attachments["postdatetime"].ToString();
						attinfo.Readperm = Int32.Parse(attachments["readperm"].ToString());
						attinfo.Filename = attachments["filename"].ToString();
						attinfo.Description = attachments["description"].ToString();
						attinfo.Filetype = attachments["filetype"].ToString();
						attinfo.Filesize = Int32.Parse(attachments["filesize"].ToString());
						attinfo.Attachment = attachments["attachment"].ToString();
						attinfo.Downloads = Int32.Parse(attachments["downloads"].ToString());
						attcoll.Add(attinfo);
					}



				}
				else

				{ 
                
                
					while (reader.Read())
					{
						ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
						//info.Uid = Int32.Parse(reader["uid"].ToString());
						attinfo.Aid = Int32.Parse(reader["aid"].ToString());
						attinfo.Tid = Int32.Parse(reader["tid"].ToString());
						attinfo.Pid = Int32.Parse(reader["pid"].ToString());
						attinfo.Postdatetime = reader["postdatetime"].ToString();
						attinfo.Readperm = Int32.Parse(reader["readperm"].ToString());
						attinfo.Filename = reader["filename"].ToString();
						attinfo.Description = reader["description"].ToString();
						attinfo.Filetype = reader["filetype"].ToString();
						attinfo.Filesize = Int32.Parse(reader["filesize"].ToString());
						attinfo.Attachment = reader["attachment"].ToString();
						attinfo.Downloads = Int32.Parse(reader["downloads"].ToString());
						attcoll.Add(attinfo);
					}
                
				}

			}



			bool next = false;
			if (attachments != null)
			{
				next = true & reader.Read();

			}
			else
			{
				next = reader.NextResult() && reader.Read();
			}

            //当帖子中的posterid字段为0时, 表示该数据出现异常
            if (reader["posterid"].ToString() == "0")
            {
                return null;
            }

			if (next)
			{

                //取帖间广告
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int fid = Int32.Parse(reader["fid"].ToString());
                int adcount = Advertisements.GetInPostAdCount("", fid);

                //用户组
                UserGroupInfo tmpGroupInfo;

                info = new ShowtopicPagePostInfo();

                info.Pid = Int32.Parse(reader["pid"].ToString());
                info.Fid = fid;
                info.Title = reader["title"].ToString().Trim();
                info.Layer = Int32.Parse(reader["layer"].ToString());
                info.Message = reader["message"].ToString().Trim();
                info.Lastedit = reader["lastedit"].ToString().Trim();
                info.Postdatetime = reader["postdatetime"].ToString().Trim();
                info.Attachment = Int32.Parse(reader["attachment"].ToString());
                info.Poster = reader["poster"].ToString().Trim();
                info.Posterid = Int32.Parse(reader["posterid"].ToString());
                info.Invisible = Int32.Parse(reader["invisible"].ToString());
                info.Usesig = Int32.Parse(reader["usesig"].ToString());
                info.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                info.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                info.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                info.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                info.Rate = Int32.Parse(reader["rate"].ToString());
                info.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                if (info.Posterid > 0)
                {
                    info.Nickname = reader["nickname"].ToString().Trim();
                    info.Username = reader["username"].ToString().Trim();
                    info.Groupid = Int32.Parse(reader["groupid"].ToString());
                    info.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                    info.Gender = Utils.StrToInt(reader["gender"], 2);
                    info.Bday = reader["bday"].ToString().Trim();
                    info.Showemail = Int32.Parse(reader["showemail"].ToString());
                    info.Digestposts = Int32.Parse(reader["digestposts"].ToString());
                    info.Credits = Int32.Parse(reader["credits"].ToString());
                    info.Extcredits1 = float.Parse(reader["extcredits1"].ToString());
                    info.Extcredits2 = float.Parse(reader["extcredits2"].ToString());
                    info.Extcredits3 = float.Parse(reader["extcredits3"].ToString());
                    info.Extcredits4 = float.Parse(reader["extcredits4"].ToString());
                    info.Extcredits5 = float.Parse(reader["extcredits5"].ToString());
                    info.Extcredits6 = float.Parse(reader["extcredits6"].ToString());
                    info.Extcredits7 = float.Parse(reader["extcredits7"].ToString());
                    info.Extcredits8 = float.Parse(reader["extcredits8"].ToString());
                    info.Posts = Int32.Parse(reader["posts"].ToString());
                    info.Joindate = reader["joindate"].ToString().Trim();
                    info.Lastactivity = reader["lastactivity"].ToString().Trim();
                    info.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                    info.Avatar = reader["avatar"].ToString();
                    info.Avatarwidth = Int32.Parse(reader["avatarwidth"].ToString());
                    info.Avatarheight = Int32.Parse(reader["avatarheight"].ToString());
                    info.Medals = reader["medals"].ToString();
                    info.Signature = reader["signature"].ToString();
                    info.Location = reader["location"].ToString();
                    info.Customstatus = reader["customstatus"].ToString();
                    info.Website = reader["website"].ToString();
                    info.Icq = reader["icq"].ToString();
                    info.Qq = reader["qq"].ToString();
                    info.Msn = reader["msn"].ToString();
                    info.Yahoo = reader["yahoo"].ToString();
                    info.Skype = reader["skype"].ToString();
                    //部分属性需要根据不同情况来赋值

                    //根据用户自己的设置决定是否显示邮箱地址
                    if (info.Showemail == 0)
                    {
                        info.Email = "";
                    }
                    else
                    {
                        info.Email = reader["email"].ToString().Trim();
                    }


                    // 最后活动时间50分钟内的为在线, 否则不在线
                    if (Utils.StrDateDiffMinutes(info.Lastactivity, 50) < 0)
                    {
                        info.Onlinestate = 1;
                    }
                    else
                    {
                        info.Onlinestate = 0;
                    }


                    //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                    info.Ip = reader["ip"].ToString().Trim();

                    // 勋章
                    if (info.Medals == "")
                    {
                        info.Medals = "";
                    }
                    else
                    {
                        info.Medals = Caches.GetMedalsList(info.Medals);
                    }

                    tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                    info.Stars = tmpGroupInfo.Stars;
                    if (tmpGroupInfo.Color.Equals(""))
                    {
                        info.Status = tmpGroupInfo.Grouptitle;
                    }
                    else
                    {
                        info.Status = string.Format("<span style=\"color:{0}>{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);
                    }
                }
                else
                {
                    info.Nickname = "游客";
                    info.Username = "游客";
                    info.Groupid = 7;
                    info.Showemail = 0;
                    info.Digestposts = 0;
                    info.Credits = 0;
                    info.Extcredits1 = 0;
                    info.Extcredits2 = 0;
                    info.Extcredits3 = 0;
                    info.Extcredits4 = 0;
                    info.Extcredits5 = 0;
                    info.Extcredits6 = 0;
                    info.Extcredits7 = 0;
                    info.Extcredits8 = 0;
                    info.Posts = 0;
                    info.Joindate = "2006-9-1 1:1:1";
                    info.Lastactivity = "2006-9-1 1:1:1"; ;
                    info.Userinvisible = 0;
                    info.Avatar = "";
                    info.Avatarwidth = 0;
                    info.Avatarheight = 0;
                    info.Medals = "";
                    info.Signature = "";
                    info.Location = "";
                    info.Customstatus = "";
                    info.Website = "";
                    info.Icq = "";
                    info.Qq = "";
                    info.Msn = "";
                    info.Yahoo = "";
                    info.Skype = "";
                    //部分属性需要根据不同情况来赋值
                    info.Email = "";
                    info.Onlinestate = 1;
                    info.Medals = "";

                    info.Ip = reader["ip"].ToString().Trim();
                    if (info.Ip.IndexOf('.') > -1)
                    {
                        info.Ip = info.Ip.Substring(0, info.Ip.LastIndexOf(".") + 1) + "*";
                    }

                    tmpGroupInfo = UserGroups.GetUserGroupInfo(7);
                    info.Stars = tmpGroupInfo.Stars;
                    info.Status = "游客";
                }
                //扩展属性
                info.Id = 1;
                info.Adindex = random.Next(0, adcount);


                if (!Utils.InArray(info.Groupid.ToString(), "4,5,6"))
                {
                    //处理帖子内容
                    _postpramsinfo.Smileyoff = info.Smileyoff;
                    _postpramsinfo.Bbcodeoff = info.Bbcodeoff;
                    _postpramsinfo.Parseurloff = info.Parseurloff;
                    _postpramsinfo.Allowhtml = info.Htmlon;
                    _postpramsinfo.Sdetail = info.Message;
                    //校正hide处理
                    if (tmpGroupInfo.Allowhidecode == 0)
                    {
                        _postpramsinfo.Hide = 0;
                    }

                    //先简单判断是否允许使用Discuz!NT代码,如果不允许,效率起见直接不进行代码转换 (虽然UBB.UBBToHTML方法内也进行判断)
                    if (!_postpramsinfo.Ubbmode)
                    {
                        info.Message = UBB.UBBToHTML(_postpramsinfo);
                    }
                    else
                    {
                        info.Message = Utils.HtmlEncode(info.Message);
                    }

                    if (_postpramsinfo.Jammer == 1)
                    {
                        info.Message = ForumUtils.AddJammer(info.Message);
                    }

                    string message = info.Message;
                    if (info.Attachment > 0 || regexAttach.IsMatch(message))
                    {


                        //获取在[hide]标签中的附件id
                        string[] attHidArray = GetHiddenAttachIdList(_postpramsinfo.Sdetail, _postpramsinfo.Hide);

                        for (int i = 0; i < attcoll.Count; i++)
                        {
                            message = GetMessageWithAttachInfo(_postpramsinfo, allowGetAttach, attHidArray, info, attcoll[i], message);
                        }
                        info.Message = message;

                    }
                }
                else//禁言的发帖人
                {
					if (ismoder)
					{
						info.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + info.Message;
					}
					else
					{
						info.Message = "该用户帖子内容已被屏蔽";
					}
                }


            }

            reader.Close();
            if (attachments != null)
            {
                attachments.Close();
            }

            return info;
        }

        #endregion

#else

        #region 帖子泛型方法

        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static Discuz.Common.Generic.List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo postpramsinfo, out Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo> attcoll, bool ismoder)
        {

            Discuz.Common.Generic.List<ShowtopicPagePostInfo> postcoll = new Discuz.Common.Generic.List<ShowtopicPagePostInfo>();
            attcoll = new Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo>();

            IDataReader reader;
            StringBuilder pidlist = new StringBuilder();
            if (!postpramsinfo.Condition.Equals(""))
            {
                reader = DatabaseProvider.GetInstance().GetPostListByCondition(postpramsinfo, GetPostTableName(postpramsinfo.Tid));
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetPostList(postpramsinfo, GetPostTableName(postpramsinfo.Tid));
            }



            return ParsePostList(postpramsinfo, attcoll, ismoder, postcoll, reader, pidlist);
        }

        public static List<ShowtopicPagePostInfo> ParsePostList(PostpramsInfo postpramsinfo, List<ShowtopicPageAttachmentInfo> attcoll, bool ismoder, List<ShowtopicPagePostInfo> postcoll, IDataReader reader, StringBuilder pidlist)
        {
                string diggslist = string.Empty;
                int allowGetAttach = 0;

                if (Forums.AllowGetAttachByUserID(Forums.GetForumInfo(postpramsinfo.Fid).Permuserlist , postpramsinfo.CurrentUserid))
                {
                    allowGetAttach = 1;
                }
                else
                {
                    if (postpramsinfo.Getattachperm.Equals("") || postpramsinfo.Getattachperm == null)
                    {
                        allowGetAttach = postpramsinfo.CurrentUserGroup.Allowgetattach;
                    }
                    else
                    {
                        if (Forums.AllowGetAttach(postpramsinfo.Getattachperm, postpramsinfo.Usergroupid))
                        {
                            allowGetAttach = 1;
                        }
                    }
                }
                TopicInfo topicinfo = Topics.GetTopicInfo(postpramsinfo.Tid);
                if (topicinfo.Special == 4 && UserGroups.GetUserGroupInfo(7).Allowdiggs != 1)
                {
                    diggslist = Debates.GetUesrDiggs(postpramsinfo.Tid, postpramsinfo.CurrentUserid);
                }
                //序号(楼层)的初值
                int id = (postpramsinfo.Pageindex - 1) * postpramsinfo.Pagesize;

                //保存初始的hide值
                int oldHide = postpramsinfo.Hide;

                //取帖间广告
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int fid = postpramsinfo.Fid;
                int adcount = Advertisements.GetInPostAdCount("", fid);

                //用户组
                UserGroupInfo tmpGroupInfo;

                while (reader.Read())
                {

                    //当帖子中的posterid字段为0时, 表示该数据出现异常
                    if (Int32.Parse(reader["posterid"].ToString()) == 0)
                    {
                        continue;
                    }

                    ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();
                    info.Pid = Int32.Parse(reader["pid"].ToString());
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    if (info.Attachment > 0)
                    {
                        pidlist.Append(",");
                        pidlist.Append(reader["pid"].ToString());
                    }
                    info.Fid = fid;
                    info.Title = reader["title"].ToString().Trim();
                    info.Layer = Int32.Parse(reader["layer"].ToString());
                    info.Message = reader["message"].ToString().Trim();
                    info.Lastedit = reader["lastedit"].ToString().Trim();
                    info.Postdatetime = reader["postdatetime"].ToString().Trim();

                    info.Poster = reader["poster"].ToString().Trim();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Invisible = Int32.Parse(reader["invisible"].ToString());
                    info.Usesig = Int32.Parse(reader["usesig"].ToString());
                    info.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                    info.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                    info.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                    info.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                    info.Ubbmessage = reader["message"].ToString().Trim();
                    if (info.Posterid > 0)
                    {
                        info.Nickname = reader["nickname"].ToString().Trim();
                        info.Username = reader["username"].ToString().Trim();
                        info.Groupid = Utils.StrToInt(reader["groupid"], 0);
                        info.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                        info.Gender = Utils.StrToInt(reader["gender"], 2);
                        info.Bday = reader["bday"].ToString().Trim();
                        info.Showemail = Utils.StrToInt(reader["showemail"], 0);
                        info.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                        info.Credits = Utils.StrToInt(reader["credits"], 0);
                        info.Extcredits1 = reader["extcredits1"] == DBNull.Value ? 0 : float.Parse(reader["extcredits1"].ToString());
                        info.Extcredits2 = reader["extcredits2"] == DBNull.Value ? 0 : float.Parse(reader["extcredits2"].ToString());
                        info.Extcredits3 = reader["extcredits3"] == DBNull.Value ? 0 : float.Parse(reader["extcredits3"].ToString());
                        info.Extcredits4 = reader["extcredits4"] == DBNull.Value ? 0 : float.Parse(reader["extcredits4"].ToString());
                        info.Extcredits5 = reader["extcredits5"] == DBNull.Value ? 0 : float.Parse(reader["extcredits5"].ToString());
                        info.Extcredits6 = reader["extcredits6"] == DBNull.Value ? 0 : float.Parse(reader["extcredits6"].ToString());
                        info.Extcredits7 = reader["extcredits7"] == DBNull.Value ? 0 : float.Parse(reader["extcredits7"].ToString());
                        info.Extcredits8 = reader["extcredits8"] == DBNull.Value ? 0 : float.Parse(reader["extcredits8"].ToString());
                        info.Posts = Utils.StrToInt(reader["posts"], 0);
                        info.Joindate = reader["joindate"].ToString().Trim();
                        info.Lastactivity = reader["lastactivity"].ToString().Trim();
                        info.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                        info.Avatar = reader["avatar"].ToString();
                        info.Avatarwidth = Utils.StrToInt(reader["avatarwidth"], 0);
                        info.Avatarheight = Utils.StrToInt(reader["avatarheight"], 0);
                        info.Medals = reader["medals"].ToString();
                        info.Signature = reader["signature"].ToString();
                        info.Location = reader["location"].ToString();
                        info.Customstatus = reader["customstatus"].ToString();
                        info.Website = reader["website"].ToString();
                        info.Icq = reader["icq"].ToString();
                        info.Qq = reader["qq"].ToString();
                        info.Msn = reader["msn"].ToString();
                        info.Yahoo = reader["yahoo"].ToString();
                        info.Skype = reader["skype"].ToString();
                        
                        //部分属性需要根据不同情况来赋值

                        //根据用户自己的设置决定是否显示邮箱地址
                        if (info.Showemail == 0)
                        {
                            info.Email = "";
                        }
                        else
                        {
                            info.Email = reader["email"].ToString().Trim();
                        }


                        // 最后活动时间50分钟内的为在线, 否则不在线
                        if (Utils.StrDateDiffMinutes(info.Lastactivity, 50) < 0)
                        {
                            info.Onlinestate = 1;
                        }
                        else
                        {
                            info.Onlinestate = 0;
                        }


                        //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                        info.Ip = reader["ip"].ToString().Trim();

                        // 勋章
                        if (info.Medals == "")
                        {
                            info.Medals = "";
                        }
                        else
                        {
                            info.Medals = Caches.GetMedalsList(info.Medals);
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                        info.Stars = tmpGroupInfo.Stars;
                        if (tmpGroupInfo.Color.Equals(""))
                        {
                            info.Status = tmpGroupInfo.Grouptitle;
                        }
                        else
                        {
                            info.Status = string.Format("<span style=\"color:{0}\">{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);
                        }

                        if (topicinfo.Special == 4)
                        {
                            if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 1)
                            {
                                info.Digged = Debates.IsDigged(int.Parse(reader["pid"].ToString()), postpramsinfo.CurrentUserid);
                            }
                            else
                            { 
                             info.Digged = diggslist.Contains(reader["pid"].ToString());
                            }
                           
                        }

                    }
                    else
                    {
                        info.Nickname = "游客";
                        info.Username = "游客";
                        info.Groupid = 7;
                        info.Showemail = 0;
                        info.Digestposts = 0;
                        info.Credits = 0;
                        info.Extcredits1 = 0;
                        info.Extcredits2 = 0;
                        info.Extcredits3 = 0;
                        info.Extcredits4 = 0;
                        info.Extcredits5 = 0;
                        info.Extcredits6 = 0;
                        info.Extcredits7 = 0;
                        info.Extcredits8 = 0;
                        info.Posts = 0;
                        info.Joindate = "2006-9-1 1:1:1";
                        info.Lastactivity = "2006-9-1 1:1:1"; ;
                        info.Userinvisible = 0;
                        info.Avatar = "";
                        info.Avatarwidth = 0;
                        info.Avatarheight = 0;
                        info.Medals = "";
                        info.Signature = "";
                        info.Location = "";
                        info.Customstatus = "";
                        info.Website = "";
                        info.Icq = "";
                        info.Qq = "";
                        info.Msn = "";
                        info.Yahoo = "";
                        info.Skype = "";
                        //部分属性需要根据不同情况来赋值
                        info.Email = "";
                        info.Onlinestate = 1;
                        info.Medals = "";

                        info.Ip = reader["ip"].ToString().Trim();
                        if (info.Ip.IndexOf('.') > -1)
                        {
                            info.Ip = info.Ip.Substring(0, info.Ip.LastIndexOf(".") + 1) + "*";
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(7);
                        info.Stars = tmpGroupInfo.Stars;
                        info.Status = "游客";

                    }
                    //扩展属性
                    id++;
                    info.Id = id;
                    info.Adindex = random.Next(0, adcount);

                    postcoll.Add(info);
                }
                reader.Close();

                if (pidlist.Length > 0)
                {
                    pidlist.Remove(0, 1);
                    IDataReader attachreader = DatabaseProvider.GetInstance().GetAttachmentListByPid(pidlist.ToString());
                    if (attachreader != null)
                    {
                        while (attachreader.Read())
                        {
                            ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                            //info.Uid = Int32.Parse(reader["uid"].ToString());
                            attinfo.Aid = Int32.Parse(attachreader["aid"].ToString());
                            attinfo.Tid = Int32.Parse(attachreader["tid"].ToString());
                            attinfo.Pid = Int32.Parse(attachreader["pid"].ToString());
                            attinfo.Postdatetime = attachreader["postdatetime"].ToString();
                            attinfo.Readperm = Int32.Parse(attachreader["readperm"].ToString());
                            attinfo.Filename = attachreader["filename"].ToString();
                            attinfo.Description = attachreader["description"].ToString();
                            attinfo.Filetype = attachreader["filetype"].ToString();
                            attinfo.Filesize = Int32.Parse(attachreader["filesize"].ToString());
                            attinfo.Attachment = attachreader["attachment"].ToString();
                            attinfo.Downloads = Int32.Parse(attachreader["downloads"].ToString());
                            attcoll.Add(attinfo);
                        }
                    }
                    attachreader.Close();
                }

                foreach (ShowtopicPagePostInfo info in postcoll)
                {
                    if (!Utils.InArray(info.Groupid.ToString(), "4,5,6"))
                    {
                        //处理帖子内容
                        postpramsinfo.Smileyoff = info.Smileyoff;
                        postpramsinfo.Bbcodeoff = info.Bbcodeoff;
                        postpramsinfo.Parseurloff = info.Parseurloff;
                        postpramsinfo.Allowhtml = info.Htmlon;
                        postpramsinfo.Sdetail = info.Message;
                        postpramsinfo.Pid = info.Pid;
                        //校正hide处理
                        tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                        if (tmpGroupInfo.Allowhidecode == 0)
                        {
                            postpramsinfo.Hide = 0;
                        }


                        //先简单判断是否是动网兼容模式
                        if (!postpramsinfo.Ubbmode)
                        {
                            info.Message = UBB.UBBToHTML(postpramsinfo);
                        }
                        else
                        {
                            info.Message = Utils.HtmlEncode(info.Message);
                        }

                        if (postpramsinfo.Jammer == 1)
                        {
                            info.Message = ForumUtils.AddJammer(info.Message);
                        }

                        string message = info.Message;
                        if (info.Attachment > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
                        {
                            //获取在[hide]标签中的附件id
                            string[] attHidArray = GetHiddenAttachIdList(postpramsinfo.Sdetail, postpramsinfo.Hide);
                            List<ShowtopicPageAttachmentInfo> delattlist = new List<ShowtopicPageAttachmentInfo>();

                            foreach (ShowtopicPageAttachmentInfo attach in attcoll)
                            {
                                message = GetMessageWithAttachInfo(postpramsinfo, allowGetAttach, attHidArray, info, attach, message);
                                if (Utils.InArray(attach.Aid.ToString(), attHidArray) || attach.Pid == 0)
                                {
                                    delattlist.Add(attach);
                                }
                            };

                            foreach (ShowtopicPageAttachmentInfo attach in delattlist)
                            {
                                attcoll.Remove(attach);
                            }
                            info.Message = message;

                        }

                        //恢复hide初值
                        postpramsinfo.Hide = oldHide;
                    }
                    else//发帖人已经被禁止发言
                    {
                        if (ismoder)
                        {
                            info.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + info.Message;
                        }
                        else
                        {
                            info.Message = "该用户帖子内容已被屏蔽";
                            List<ShowtopicPageAttachmentInfo> delattlist = new List<ShowtopicPageAttachmentInfo>();

                            foreach (ShowtopicPageAttachmentInfo attach in attcoll)
                            {
                                if (attach.Pid == info.Pid)
                                {
                                    delattlist.Add(attach);
                                }
                            }

                            foreach (ShowtopicPageAttachmentInfo attach in delattlist)
                            {
                                attcoll.Remove(attach);
                            }

                        }
                    }
                }

            return postcoll;
        }

        /*
        
        /// <summary>
        /// 获取指定条件的帖子DataSet
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>指定条件的帖子DataSet</returns>
        public static Discuz.Common.Generic.List<ShowtopicPagePostInfo> GetPostList(PostpramsInfo _postpramsinfo, out Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo> attcoll)
        {

            Discuz.Common.Generic.List<ShowtopicPagePostInfo> postcoll = new Discuz.Common.Generic.List<ShowtopicPagePostInfo>();
            attcoll = new Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo>();

            IDataReader reader;
            IDataReader attachreader = null;

            if (!_postpramsinfo.Condition.Equals(""))
            {
                reader = DatabaseProvider.GetInstance().GetPostListByCondition(_postpramsinfo, GetPostTableName(_postpramsinfo.Tid), out attachreader);
            }
            else
            {
                reader = DatabaseProvider.GetInstance().GetPostList(_postpramsinfo, GetPostTableName(_postpramsinfo.Tid), out attachreader);
            }


            int allowGetAttach = 0;
            if (reader != null)
            {

                if (_postpramsinfo.Getattachperm.Equals("") || _postpramsinfo.Getattachperm == null)
                {
                    allowGetAttach = _postpramsinfo.CurrentUserGroup.Allowgetattach;
                }
                else
                {
                    if (Forums.AllowGetAttach(_postpramsinfo.Getattachperm, _postpramsinfo.Usergroupid))
                    {
                        allowGetAttach = 1;
                    }

                }
                if (attachreader!=null)
                {
                    while (attachreader.Read())
                    {
                        ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                        //info.Uid = Int32.Parse(reader["uid"].ToString());
                        attinfo.Aid = Int32.Parse(attachreader["aid"].ToString());
                        attinfo.Tid = Int32.Parse(attachreader["tid"].ToString());
                        attinfo.Pid = Int32.Parse(attachreader["pid"].ToString());
                        attinfo.Postdatetime = attachreader["postdatetime"].ToString();
                        attinfo.Readperm = Int32.Parse(attachreader["readperm"].ToString());
                        attinfo.Filename = attachreader["filename"].ToString();
                        attinfo.Description = attachreader["description"].ToString();
                        attinfo.Filetype = attachreader["filetype"].ToString();
                        attinfo.Filesize = Int32.Parse(attachreader["filesize"].ToString());
                        attinfo.Attachment = attachreader["attachment"].ToString();
                        attinfo.Downloads = Int32.Parse(attachreader["downloads"].ToString());
                        attcoll.Add(attinfo);
                    }
                }
                else
                {

                    while (reader.Read())
                    {
                        ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                        //info.Uid = Int32.Parse(reader["uid"].ToString());
                        attinfo.Aid = Int32.Parse(reader["aid"].ToString());
                        attinfo.Tid = Int32.Parse(reader["tid"].ToString());
                        attinfo.Pid = Int32.Parse(reader["pid"].ToString());
                        attinfo.Postdatetime = reader["postdatetime"].ToString();
                        attinfo.Readperm = Int32.Parse(reader["readperm"].ToString());
                        attinfo.Filename = reader["filename"].ToString();
                        attinfo.Description = reader["description"].ToString();
                        attinfo.Filetype = reader["filetype"].ToString();
                        attinfo.Filesize = Int32.Parse(reader["filesize"].ToString());
                        attinfo.Attachment = reader["attachment"].ToString();
                        attinfo.Downloads = Int32.Parse(reader["downloads"].ToString());
                        attcoll.Add(attinfo);
                    }



                }

            }


            bool next = false;
            if (attachreader!=null)
            {
                next = true;

            }
            else
            {
                next = reader.NextResult() && reader != null;
            }


            if (next)
            {
                //序号(楼层)的初值
                int id = (_postpramsinfo.Pageindex - 1) * _postpramsinfo.Pagesize;

                //保存初始的hide值
                int oldHide = _postpramsinfo.Hide;

                //取帖间广告
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int fid = _postpramsinfo.Fid;
                int adcount = Advertisements.GetInPostAdCount("", fid);

                //用户组
                UserGroupInfo tmpGroupInfo;

                while (reader.Read())
                {
                    ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();

                    info.Pid = Int32.Parse(reader["pid"].ToString());
                    info.Fid = fid;
                    info.Title = reader["title"].ToString().Trim();
                    info.Layer = Int32.Parse(reader["layer"].ToString());
                    info.Message = reader["message"].ToString().Trim();
                    info.Lastedit = reader["lastedit"].ToString().Trim();
                    info.Postdatetime = reader["postdatetime"].ToString().Trim();
                    info.Attachment = Int32.Parse(reader["attachment"].ToString());
                    info.Poster = reader["poster"].ToString().Trim();
                    info.Posterid = Int32.Parse(reader["posterid"].ToString());
                    info.Invisible = Int32.Parse(reader["invisible"].ToString());
                    info.Usesig = Int32.Parse(reader["usesig"].ToString());
                    info.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                    info.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                    info.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                    info.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                    info.Rate = Int32.Parse(reader["rate"].ToString());
                    info.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                    if (info.Posterid > 0)
                    {
                        info.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                        info.Nickname = reader["nickname"].ToString().Trim();
                        info.Username = reader["username"].ToString().Trim();
                        info.Groupid = Utils.StrToInt(reader["groupid"], 0);
                        info.Showemail = Utils.StrToInt(reader["showemail"], 0);
                        info.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                        info.Credits = Utils.StrToInt(reader["credits"], 0);
                        info.Extcredits1 = reader["extcredits1"] == DBNull.Value ? 0 : float.Parse(reader["extcredits1"].ToString());
                        info.Extcredits2 = reader["extcredits2"] == DBNull.Value ? 0 : float.Parse(reader["extcredits2"].ToString());
                        info.Extcredits3 = reader["extcredits3"] == DBNull.Value ? 0 : float.Parse(reader["extcredits3"].ToString());
                        info.Extcredits4 = reader["extcredits4"] == DBNull.Value ? 0 : float.Parse(reader["extcredits4"].ToString());
                        info.Extcredits5 = reader["extcredits5"] == DBNull.Value ? 0 : float.Parse(reader["extcredits5"].ToString());
                        info.Extcredits6 = reader["extcredits6"] == DBNull.Value ? 0 : float.Parse(reader["extcredits6"].ToString());
                        info.Extcredits7 = reader["extcredits7"] == DBNull.Value ? 0 : float.Parse(reader["extcredits7"].ToString());
                        info.Extcredits8 = reader["extcredits8"] == DBNull.Value ? 0 : float.Parse(reader["extcredits8"].ToString());
                        info.Posts = Utils.StrToInt(reader["posts"], 0);
                        info.Joindate = reader["joindate"].ToString().Trim();
                        info.Lastactivity = reader["lastactivity"].ToString().Trim();
                        info.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                        info.Avatar = reader["avatar"].ToString();
                        info.Avatarwidth = Utils.StrToInt(reader["avatarwidth"], 0);
                        info.Avatarheight = Utils.StrToInt(reader["avatarheight"], 0);
                        info.Medals = reader["medals"].ToString();
                        info.Signature = reader["signature"].ToString();
                        info.Location = reader["location"].ToString();
                        info.Customstatus = reader["customstatus"].ToString();
                        info.Website = reader["website"].ToString();
                        info.Icq = reader["icq"].ToString();
                        info.Qq = reader["qq"].ToString();
                        info.Msn = reader["msn"].ToString();
                        info.Yahoo = reader["yahoo"].ToString();
                        info.Skype = reader["skype"].ToString();
                        //部分属性需要根据不同情况来赋值

                        //根据用户自己的设置决定是否显示邮箱地址
                        if (info.Showemail == 0)
                        {
                            info.Email = "";
                        }
                        else
                        {
                            info.Email = reader["email"].ToString().Trim();
                        }


                        // 最后活动时间50分钟内的为在线, 否则不在线
                        if (Utils.StrDateDiffMinutes(info.Lastactivity, 50) < 0)
                        {
                            info.Onlinestate = 1;
                        }
                        else
                        {
                            info.Onlinestate = 0;
                        }


                        //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                        info.Ip = reader["ip"].ToString().Trim();

                        // 勋章
                        if (info.Medals == "")
                        {
                            info.Medals = "";
                        }
                        else
                        {
                            info.Medals = Caches.GetMedalsList(info.Medals);
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                        info.Stars = tmpGroupInfo.Stars;
                        if (tmpGroupInfo.Color.Equals(""))
                        {
                            info.Status = tmpGroupInfo.Grouptitle;
                        }
                        else
                        {
                            info.Status = string.Format("<span style=\"color:{0}\">{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);
                        }
                    }
                    else
                    {
                        info.Nickname = "游客";
                        info.Username = "游客";
                        info.Groupid = 7;
                        info.Showemail = 0;
                        info.Digestposts = 0;
                        info.Credits = 0;
                        info.Extcredits1 = 0;
                        info.Extcredits2 = 0;
                        info.Extcredits3 = 0;
                        info.Extcredits4 = 0;
                        info.Extcredits5 = 0;
                        info.Extcredits6 = 0;
                        info.Extcredits7 = 0;
                        info.Extcredits8 = 0;
                        info.Posts = 0;
                        info.Joindate = "2006-9-1 1:1:1";
                        info.Lastactivity = "2006-9-1 1:1:1"; ;
                        info.Userinvisible = 0;
                        info.Avatar = "";
                        info.Avatarwidth = 0;
                        info.Avatarheight = 0;
                        info.Medals = "";
                        info.Signature = "";
                        info.Location = "";
                        info.Customstatus = "";
                        info.Website = "";
                        info.Icq = "";
                        info.Qq = "";
                        info.Msn = "";
                        info.Yahoo = "";
                        info.Skype = "";
                        //部分属性需要根据不同情况来赋值
                        info.Email = "";
                        info.Onlinestate = 1;
                        info.Medals = "";

                        info.Ip = reader["ip"].ToString().Trim();
                        if (info.Ip.IndexOf('.') > -1)
                        {
                            info.Ip = info.Ip.Substring(0, info.Ip.LastIndexOf(".") + 1) + "*";
                        }

                        tmpGroupInfo = UserGroups.GetUserGroupInfo(7);
                        info.Stars = tmpGroupInfo.Stars;
                        info.Status = "游客";

                    }
                    //扩展属性
                    id++;
                    info.Id = id;
                    info.Adindex = random.Next(0, adcount);


                    //处理帖子内容
                    _postpramsinfo.Smileyoff = info.Smileyoff;
                    _postpramsinfo.Bbcodeoff = info.Bbcodeoff;
                    _postpramsinfo.Parseurloff = info.Parseurloff;
                    _postpramsinfo.Allowhtml = info.Htmlon;
                    _postpramsinfo.Sdetail = info.Message;
                    //校正hide处理
                    if (tmpGroupInfo.Allowhidecode == 0)
                    {
                        _postpramsinfo.Hide = 0;
                    }                   


                    //先简单判断是否允许使用Discuz!NT代码,如果不允许,效率起见直接不进行代码转换 (虽然UBB.UBBToHTML方法内也进行判断)
                    if (!_postpramsinfo.Ubbmode)
                    {
                        info.Message = UBB.UBBToHTML(_postpramsinfo);
                    }
                    else
                    {
                        info.Message = Utils.HtmlEncode(info.Message);
                    }

                    if (_postpramsinfo.Jammer == 1)
                    {
                        info.Message = ForumUtils.AddJammer(info.Message);
                    }

                    string message = info.Message;
                    if (info.Attachment > 0 || regexAttach.IsMatch(message))
                    {


                        //获取在[hide]标签中的附件id
                        string[] attHidArray = GetHiddenAttachIdList(_postpramsinfo.Sdetail, _postpramsinfo.Hide);

                        for (int i = 0; i < attcoll.Count; i++)
                        {
                            message = GetMessageWithAttachInfo(_postpramsinfo, allowGetAttach, attHidArray, info, attcoll[i], message);
                            if (Utils.InArray(attcoll[i].Aid.ToString(), attHidArray))
                            {
                                attcoll.RemoveAt(i);
                            }
                        }
                        info.Message = message;

                    }

                    //恢复hide初值
                    _postpramsinfo.Hide = oldHide;

                    postcoll.Add(info);
                }
            }
            return postcoll;
        }
        */
        /// <summary>
        /// 通过主题ID得到主帖内容,此方法可继续扩展
        /// </summary>
        /// <param name="tid"></param>
        /// <returns>ShowtopicPagePostInfo</returns>
        public static ShowtopicPagePostInfo GetSinglePost(int tid)
        {
            ShowtopicPagePostInfo info = new ShowtopicPagePostInfo();; 
            IDataReader reader = DatabaseProvider.GetInstance().GetSinglePost(tid,Utils.StrToInt(GetPostTableID(tid),-1));

                while(reader.Read())
                {
                 info.Message = reader["message"].ToString().Trim();
                }

            reader.Close();
            return info;
           
        }
        /// <summary>
        /// 获得单个帖子的信息, 包括发帖人的一般资料
        /// </summary>
        /// <param name="_postpramsinfo">参数列表</param>
        /// <returns>帖子的信息</returns>
        public static ShowtopicPagePostInfo GetSinglePost(PostpramsInfo postpramsinfo, out Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo> attcoll, bool ismoder)
        {
            ShowtopicPagePostInfo info = null;
            IDataReader attachments = null;
            attcoll = new Discuz.Common.Generic.List<ShowtopicPageAttachmentInfo>();


            ///得到帖子对应主题的所有附件
            IDataReader reader = DatabaseProvider.GetInstance().GetSinglePost(out attachments, postpramsinfo, GetPostTableID(postpramsinfo.Tid));
            int allowGetAttach = 0;


            if (reader != null)
            {

                if (postpramsinfo.Getattachperm.Equals("") || postpramsinfo.Getattachperm == null)
                {
                    allowGetAttach = postpramsinfo.CurrentUserGroup.Allowgetattach;
                }
                else
                {
                    if (Forums.AllowGetAttach(postpramsinfo.Getattachperm, postpramsinfo.Usergroupid))
                    {
                        allowGetAttach = 1;
                    }

                }
                if (attachments != null)
                {
                    while (attachments.Read())
                    {
                        ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                        //info.Uid = Int32.Parse(reader["uid"].ToString());
                        attinfo.Aid = Int32.Parse(attachments["aid"].ToString());
                        attinfo.Tid = Int32.Parse(attachments["tid"].ToString());
                        attinfo.Pid = Int32.Parse(attachments["pid"].ToString());
                        attinfo.Postdatetime = attachments["postdatetime"].ToString();
                        attinfo.Readperm = Int32.Parse(attachments["readperm"].ToString());
                        attinfo.Filename = attachments["filename"].ToString();
                        attinfo.Description = attachments["description"].ToString();
                        attinfo.Filetype = attachments["filetype"].ToString();
                        attinfo.Filesize = Int32.Parse(attachments["filesize"].ToString());
                        attinfo.Attachment = attachments["attachment"].ToString();
                        attinfo.Downloads = Int32.Parse(attachments["downloads"].ToString());
                        attcoll.Add(attinfo);
                    }
                }
                else
                {
                    while (reader.Read())
                    {
                        ShowtopicPageAttachmentInfo attinfo = new ShowtopicPageAttachmentInfo();
                        //info.Uid = Int32.Parse(reader["uid"].ToString());
                        attinfo.Aid = Int32.Parse(reader["aid"].ToString());
                        attinfo.Tid = Int32.Parse(reader["tid"].ToString());
                        attinfo.Pid = Int32.Parse(reader["pid"].ToString());
                        attinfo.Postdatetime = reader["postdatetime"].ToString();
                        attinfo.Readperm = Int32.Parse(reader["readperm"].ToString());
                        attinfo.Filename = reader["filename"].ToString();
                        attinfo.Description = reader["description"].ToString();
                        attinfo.Filetype = reader["filetype"].ToString();
                        attinfo.Filesize = Int32.Parse(reader["filesize"].ToString());
                        attinfo.Attachment = reader["attachment"].ToString();
                        attinfo.Downloads = Int32.Parse(reader["downloads"].ToString());
                        attcoll.Add(attinfo);
                    }
                }
            }

            bool next = false;
            if (attachments != null)
            {
                next = true & reader.Read();

            }
            else
            {
                next = reader.NextResult() && reader.Read();
            }

            //当帖子中的posterid字段为0时, 表示该数据出现异常
            if (reader["posterid"].ToString() == "0")
            {
                reader.Close();
                return null;
            }

            if (next)
            {

                //取帖间广告
                Random random = new Random(unchecked((int)DateTime.Now.Ticks));
                int fid = Int32.Parse(reader["fid"].ToString());
                int adcount = Advertisements.GetInPostAdCount("", fid);

                //用户组
                UserGroupInfo tmpGroupInfo;

                info = new ShowtopicPagePostInfo();

                info.Pid = Int32.Parse(reader["pid"].ToString());
                info.Fid = fid;
                info.Title = reader["title"].ToString().Trim();
                info.Layer = Int32.Parse(reader["layer"].ToString());
                info.Message = reader["message"].ToString().Trim();
                info.Lastedit = reader["lastedit"].ToString().Trim();
                info.Postdatetime = reader["postdatetime"].ToString().Trim();
                info.Attachment = Int32.Parse(reader["attachment"].ToString());
                info.Poster = reader["poster"].ToString().Trim();
                info.Posterid = Int32.Parse(reader["posterid"].ToString());
                info.Invisible = Int32.Parse(reader["invisible"].ToString());
                info.Usesig = Int32.Parse(reader["usesig"].ToString());
                info.Htmlon = Int32.Parse(reader["htmlon"].ToString());
                info.Smileyoff = Int32.Parse(reader["smileyoff"].ToString());
                info.Parseurloff = Int32.Parse(reader["parseurloff"].ToString());
                info.Bbcodeoff = Int32.Parse(reader["bbcodeoff"].ToString());
                info.Rate = Int32.Parse(reader["rate"].ToString());
                info.Ratetimes = Int32.Parse(reader["ratetimes"].ToString());
                if (info.Posterid > 0)
                {
                    info.Nickname = reader["nickname"].ToString().Trim();
                    info.Username = reader["username"].ToString().Trim();
                    info.Groupid = Utils.StrToInt(reader["groupid"], 0);
                    info.Spaceid = Utils.StrToInt(reader["spaceid"], 0);
                    info.Gender = Utils.StrToInt(reader["gender"], 2);
                    info.Bday = reader["bday"].ToString().Trim();
                    info.Showemail = Utils.StrToInt(reader["showemail"], 0);
                    info.Digestposts = Utils.StrToInt(reader["digestposts"], 0);
                    info.Credits = Utils.StrToInt(reader["credits"], 0);
                    info.Extcredits1 = reader["extcredits1"] == DBNull.Value ? 0 : float.Parse(reader["extcredits1"].ToString());
                    info.Extcredits2 = reader["extcredits2"] == DBNull.Value ? 0 : float.Parse(reader["extcredits2"].ToString());
                    info.Extcredits3 = reader["extcredits3"] == DBNull.Value ? 0 : float.Parse(reader["extcredits3"].ToString());
                    info.Extcredits4 = reader["extcredits4"] == DBNull.Value ? 0 : float.Parse(reader["extcredits4"].ToString());
                    info.Extcredits5 = reader["extcredits5"] == DBNull.Value ? 0 : float.Parse(reader["extcredits5"].ToString());
                    info.Extcredits6 = reader["extcredits6"] == DBNull.Value ? 0 : float.Parse(reader["extcredits6"].ToString());
                    info.Extcredits7 = reader["extcredits7"] == DBNull.Value ? 0 : float.Parse(reader["extcredits7"].ToString());
                    info.Extcredits8 = reader["extcredits8"] == DBNull.Value ? 0 : float.Parse(reader["extcredits8"].ToString());
                    info.Posts = Utils.StrToInt(reader["posts"], 0);
                    info.Joindate = reader["joindate"].ToString().Trim();
                    info.Lastactivity = reader["lastactivity"].ToString().Trim();
                    info.Userinvisible = Int32.Parse(reader["invisible"].ToString());
                    info.Avatar = reader["avatar"].ToString();
                    info.Avatarwidth = Utils.StrToInt(reader["avatarwidth"], 0);
                    info.Avatarheight = Utils.StrToInt(reader["avatarheight"], 0);
                    info.Medals = reader["medals"].ToString();
                    info.Signature = reader["signature"].ToString();
                    info.Location = reader["location"].ToString();
                    info.Customstatus = reader["customstatus"].ToString();
                    info.Website = reader["website"].ToString();
                    info.Icq = reader["icq"].ToString();
                    info.Qq = reader["qq"].ToString();
                    info.Msn = reader["msn"].ToString();
                    info.Yahoo = reader["yahoo"].ToString();
                    info.Skype = reader["skype"].ToString();
                    //部分属性需要根据不同情况来赋值

                    //根据用户自己的设置决定是否显示邮箱地址
                    if (info.Showemail == 0)
                    {
                        info.Email = "";
                    }
                    else
                    {
                        info.Email = reader["email"].ToString().Trim();
                    }


                    // 最后活动时间50分钟内的为在线, 否则不在线
                    if (Utils.StrDateDiffMinutes(info.Lastactivity, 50) < 0)
                    {
                        info.Onlinestate = 1;
                    }
                    else
                    {
                        info.Onlinestate = 0;
                    }


                    //作者ID为-1即表明作者为游客, 为了区分会直接公开显示游客发帖时的IP, 这里将IP最后一位修改为*
                    info.Ip = reader["ip"].ToString().Trim();

                    // 勋章
                    if (info.Medals == "")
                    {
                        info.Medals = "";
                    }
                    else
                    {
                        info.Medals = Caches.GetMedalsList(info.Medals);
                    }

                    tmpGroupInfo = UserGroups.GetUserGroupInfo(info.Groupid);
                    info.Stars = tmpGroupInfo.Stars;
                    if (tmpGroupInfo.Color.Equals(""))
                    {
                        info.Status = tmpGroupInfo.Grouptitle;
                    }
                    else
                    {
                        info.Status = string.Format("<span style=\"color:{0}>{1}</span>", tmpGroupInfo.Color, tmpGroupInfo.Grouptitle);
                    }
                }
                else
                {
                    info.Nickname = "游客";
                    info.Username = "游客";
                    info.Groupid = 7;
                    info.Showemail = 0;
                    info.Digestposts = 0;
                    info.Credits = 0;
                    info.Extcredits1 = 0;
                    info.Extcredits2 = 0;
                    info.Extcredits3 = 0;
                    info.Extcredits4 = 0;
                    info.Extcredits5 = 0;
                    info.Extcredits6 = 0;
                    info.Extcredits7 = 0;
                    info.Extcredits8 = 0;
                    info.Posts = 0;
                    info.Joindate = "2006-9-1 1:1:1";
                    info.Lastactivity = "2006-9-1 1:1:1"; ;
                    info.Userinvisible = 0;
                    info.Avatar = "";
                    info.Avatarwidth = 0;
                    info.Avatarheight = 0;
                    info.Medals = "";
                    info.Signature = "";
                    info.Location = "";
                    info.Customstatus = "";
                    info.Website = "";
                    info.Icq = "";
                    info.Qq = "";
                    info.Msn = "";
                    info.Yahoo = "";
                    info.Skype = "";
                    //部分属性需要根据不同情况来赋值
                    info.Email = "";
                    info.Onlinestate = 1;
                    info.Medals = "";

                    info.Ip = reader["ip"].ToString().Trim();
                    if (info.Ip.IndexOf('.') > -1)
                    {
                        info.Ip = info.Ip.Substring(0, info.Ip.LastIndexOf(".") + 1) + "*";
                    }

                    tmpGroupInfo = UserGroups.GetUserGroupInfo(7);
                    info.Stars = tmpGroupInfo.Stars;
                    info.Status = "游客";
                }
                //扩展属性
                info.Id = 1;
                info.Adindex = random.Next(0, adcount);

                if (!Utils.InArray(info.Groupid.ToString(), "4,5,6"))
                {
                    //处理帖子内容
                    postpramsinfo.Smileyoff = info.Smileyoff;
                    postpramsinfo.Bbcodeoff = info.Bbcodeoff;
                    postpramsinfo.Parseurloff = info.Parseurloff;
                    postpramsinfo.Allowhtml = info.Htmlon;
                    postpramsinfo.Sdetail = info.Message;
                    postpramsinfo.Pid = info.Pid;
                    //校正hide处理
                    if (tmpGroupInfo.Allowhidecode == 0)
                    {
                        postpramsinfo.Hide = 0;
                    }

                    //先简单判断是否是动网兼容模式
                    if (!postpramsinfo.Ubbmode)
                    {
                        info.Message = UBB.UBBToHTML(postpramsinfo);
                    }
                    else
                    {
                        info.Message = Utils.HtmlEncode(info.Message);
                    }

                    if (postpramsinfo.Jammer == 1)
                    {
                        info.Message = ForumUtils.AddJammer(info.Message);
                    }

                    string message = info.Message;
                    if (info.Attachment > 0 || regexAttach.IsMatch(message) || regexAttachImg.IsMatch(message))
                    {


                        //获取在[hide]标签中的附件id
                        string[] attHidArray = GetHiddenAttachIdList(postpramsinfo.Sdetail, postpramsinfo.Hide);

                        List<ShowtopicPageAttachmentInfo> delattlist = new List<ShowtopicPageAttachmentInfo>();

                        foreach (ShowtopicPageAttachmentInfo attach in attcoll)
                        {
                            message = GetMessageWithAttachInfo(postpramsinfo, allowGetAttach, attHidArray, info, attach, message);
                            if (Utils.InArray(attach.Aid.ToString(), attHidArray) || attach.Pid == 0)
                            {
                                delattlist.Add(attach);
                            }
                        }

                        foreach (ShowtopicPageAttachmentInfo attach in delattlist)
                        {
                            attcoll.Remove(attach);
                        }

                        info.Message = message;

                    }
                }
                else//禁言的发帖人
                {
                    if (ismoder)
                    {
                        info.Message = "<div class='hintinfo'>该用户帖子内容已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>" + info.Message;
                    }
                    else
                    {
                        info.Message = "该用户帖子内容已被屏蔽";
                        List<ShowtopicPageAttachmentInfo> delattlist = new List<ShowtopicPageAttachmentInfo>();

                        foreach (ShowtopicPageAttachmentInfo attach in attcoll)
                        {
                            if (attach.Pid == info.Pid)
                            {
                                delattlist.Add(attach);
                            }
                        }

                        foreach (ShowtopicPageAttachmentInfo attach in delattlist)
                        {
                            attcoll.Remove(attach);
                        }

                    }
                }

            }

            reader.Close();
            if (attachments != null)
            {
                attachments.Close();
            }

            return info;
        }

        #endregion

#endif

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postpramsInfo"></param>
        /// <param name="attachmentlist"></param>
        /// <param name="ismoder"></param>
        /// <returns></returns>
        public static List<ShowbonusPagePostInfo> GetPostListWithBonus(PostpramsInfo postpramsInfo, out List<ShowtopicPageAttachmentInfo> attachmentlist, bool ismoder)
        {
            List<ShowtopicPagePostInfo> sppil = Posts.GetPostList(postpramsInfo, out attachmentlist, ismoder);

            List<ShowbonusPagePostInfo> result = new List<ShowbonusPagePostInfo>();

            Dictionary<int, BonusLogInfo> bonusDetails = Bonus.GetLogsForEachPost(postpramsInfo.Tid);
            foreach (ShowtopicPagePostInfo sppi in sppil)
            {
                ShowbonusPagePostInfo item = new ShowbonusPagePostInfo();
                #region 属性赋值
                item.Adindex = sppi.Adindex;
                item.Attachment = sppi.Attachment;
                item.Avatar = sppi.Avatar;
                item.Avatarheight = sppi.Avatarheight;
                item.Avatarwidth = sppi.Avatarwidth;
                item.Bbcodeoff = sppi.Bbcodeoff;
                item.Bday = sppi.Bday;
                item.Credits = sppi.Credits;
                item.Customstatus = sppi.Customstatus;
                item.Digestposts = sppi.Digestposts;
                item.Email = sppi.Email;
                item.Extcredits1 = sppi.Extcredits1;
                item.Extcredits2 = sppi.Extcredits2;
                item.Extcredits3 = sppi.Extcredits3;
                item.Extcredits4 = sppi.Extcredits4;
                item.Extcredits5 = sppi.Extcredits5;
                item.Extcredits6 = sppi.Extcredits6;
                item.Extcredits7 = sppi.Extcredits7;
                item.Extcredits8 = sppi.Extcredits8;
                item.Fid = sppi.Fid;
                item.Gender = sppi.Gender;
                item.Groupid = sppi.Groupid;
                item.Htmlon = sppi.Htmlon;
                item.Icq = sppi.Icq;
                item.Id = sppi.Id;
                item.Invisible = sppi.Invisible;
                item.Ip = sppi.Ip;
                item.Joindate = sppi.Joindate;
                item.Lastactivity = sppi.Lastactivity;
                item.Lastedit = sppi.Lastedit;
                item.Layer = sppi.Layer;
                item.Location = sppi.Location;
                item.Medals = sppi.Medals;
                item.Message = sppi.Message;
                item.Msn = sppi.Msn;
                item.Nickname = sppi.Nickname;
                item.Onlinestate = sppi.Onlinestate;
                item.Parseurloff = sppi.Parseurloff;
                item.Pid = sppi.Pid;
                item.Postdatetime = sppi.Postdatetime;
                item.Poster = sppi.Poster;
                item.Posterid = sppi.Posterid;
                item.Posts = sppi.Posts;
                item.Qq = sppi.Qq;
                item.Rate = sppi.Rate;
                item.Ratetimes = sppi.Ratetimes;
                item.Showemail = sppi.Showemail;
                item.Signature = sppi.Signature;
                item.Skype = sppi.Skype;
                item.Smileyoff = sppi.Smileyoff;
                item.Spaceid = sppi.Spaceid;
                item.Stars = sppi.Stars;
                item.Status = sppi.Status;
                item.Title = sppi.Title;
                item.Userinvisible = sppi.Userinvisible;
                item.Username = sppi.Username;
                item.Usesig = sppi.Usesig;
                item.Website = sppi.Website;
                item.Yahoo = sppi.Yahoo;

                if (bonusDetails.ContainsKey(item.Pid))
                {
                    item.Bonus = bonusDetails[item.Pid].Bonus;
                    item.Bonusextid = bonusDetails[item.Pid].Extid;
                    item.Isbest = bonusDetails[item.Pid].Isbest;
                }
                #endregion
                if (bonusDetails.ContainsKey(item.Pid) || item.Layer == 0)
                {
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// 屏蔽帖子内容
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="postlist">帖子ID</param>
        /// <param name="invisible">屏蔽还是解除屏蔽</param>
        public static void BanPosts(int tid,string postlist,int invisible)
        {
            int tableid = Utils.StrToInt(GetPostTableID(Utils.StrToInt(tid, -1)), -1);

            DatabaseProvider.GetInstance().SetPostsBanned(tableid, postlist, invisible);
        }
        /// <summary>
        /// 返回帖子列表
        /// </summary>
        /// <param name="postlist">帖子ID</param>
        /// <param name="tid">主题ID</param>
        /// <returns>帖子列表</returns>
        public static DataTable GetPostList(string postlist,string tid)
        {
            if (!Utils.IsNumericArray(postlist.Split(',')))
            {
                return null;
            }
            int tableid=Utils.StrToInt(GetPostTableID(Utils.StrToInt(tid,-1)),-1);
            return DatabaseProvider.GetInstance().GetPostList(postlist,tableid);
        }

        /// <summary>
        /// 检查帖子标题与内容中是否有广告
        /// </summary>
        /// <param name="regular">验证广告正则</param>
        /// <param name="title">帖子标题</param>
        /// <param name="message">帖子内容</param>
        /// <returns>帖子标题与内容中是否有广告</returns>
        public static bool IsAD(string regular, string title, string message)
        {
            if(regular.Trim() == "")
            {
                return false;
            }
            return (Regex.IsMatch(title, regular) || Regex.IsMatch(message, regular));
        }
    }
}


