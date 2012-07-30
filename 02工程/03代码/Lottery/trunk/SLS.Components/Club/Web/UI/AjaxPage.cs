using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using System.Collections;
using Discuz.Cache;
using Discuz.Data;
using Discuz.Config;
using System.Web;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using Discuz.Common.Generic;
using Newtonsoft.Json;

namespace Discuz.Web.UI
{
    /// <summary>
    /// Ajax相关功能操作类
    /// </summary>
    public class AjaxPage : PageBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            string type = DNTRequest.GetString("t");
            switch (type)
            {
                case "forumtree":
                    GetForumTree();		//获得指定版块的子版块信息,以xml文件输出
                    break;
                case "topictree":
                    GetTopicTree();		//获得指定主题的回复信息,以xml文件输出
                    break;
                case "checkusername":
                    CheckUserName();    //检查用户名是否存在
                    break;
                case "quickreply":
                    QuickReply();	//快速回复主题
                    break;
                case "report":  //举报功能
                    Report();
                    break;
                case "checkrewritename":
                    CheckRewriteName();
                    break;
                case "ratelist":
                    GetRateLogList();	//帖子评分记录
                    break;
                case "smilies":
                    GetSmilies();
                    break;
                case "relatekw":
                    GetRelateKeyword();
                    break;
                case "gettopictags":
                    GetTopicTags();
                    break;
                case "topicswithsametag":
                    GetTopicsWithSameTag();
                    break;
                case "getforumhottags":
                    GetForumHotTags();
                    break;
                case "gethotdebatetopic":
                    Getdebatesjsonlist("gethotdebatetopic", DNTRequest.GetString("tidlist"));
                    break;
                case "recommenddebates":
                    Getdebatesjsonlist("recommenddebates", DNTRequest.GetString("tidlist"));
                    break;
                case "addcommentdebates":
                    AddCommentDebates();
                    break;
                case "diggdebates":
                    DiggDebates();
                    break;
                case "getdebatepostpage":
                    GetDebatePostPage();
                    break;
            }
        }

        /// <summary>
        /// 获取分页的辩论帖子数据
        /// </summary>
        private void GetDebatePostPage()
        {
            int opinion = DNTRequest.GetInt("opinion", 0);
            int pageid = DNTRequest.GetInt("page", 1);
            int topicid = DNTRequest.GetInt("tid", 0);
            string errormsg = string.Empty;
            int ismoder = 0;
            AdminGroupInfo admininfo;
            byte disablepostctrl;
            List<ShowtopicPageAttachmentInfo> positiveattatchments;
            List<ShowtopicPageAttachmentInfo> negativeattatchments;
            int pagesize = config.Debatepagesize;

            TopicInfo topic = Topics.GetTopicInfo(topicid);
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            int hide = 1;

            if (topic == null)
            {
                errormsg = "该主题不存在";
                ResponseText(errormsg);
                return;
            }

            ForumInfo forum = Forums.GetForumInfo(topic.Fid);
            if (ValidatePurview() == string.Empty)
            {
                // 检查是否具有版主的身份
                if (useradminid != 0)
                {
                    ismoder = Moderators.IsModer(useradminid, userid, forum.Fid) ? 1 : 0;
                    //得到管理组信息
                    admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
                    if (admininfo != null)
                    {
                        disablepostctrl = admininfo.Disablepostctrl;
                    }
                }
            }
            if (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1))
            {
                hide = -1;
            }


            if (topic.Closed > 1)
            {
                topicid = topic.Closed;
                topic = Topics.GetTopicInfo(topicid);

                // 如果该主题不存在
                if (topic == null || topic.Closed > 1)
                {
                    ResponseText("不存在的主题ID");
                    return;
                }
            }


            if (topic.Displayorder == -1)
            {
                ResponseText("此主题已被删除！");
                return;
            }

            if (topic.Displayorder == -2)
            {
                ResponseText("此主题未经审核！");
                return;
            }

            if (forum.Password != "" &&
                Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forum.Fid.ToString() + "password"))
            {
                ResponseText("本版块被管理员设置了密码");
                //SetBackLink("showforum-" + forumid.ToString() + config.Extname);
                HttpContext.Current.Response.Redirect("/showforum-" + forum.Fid.ToString() + config.Extname, true);
                return;
            }


            if (!Forums.AllowViewByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        ResponseText("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                        return;
                    }
                }
                else //当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        ResponseText("您没有浏览该版块的权限");

                        return;
                    }
                }
            }

            if (topic.Special != 4)
            {
                ResponseText("本主题不是辩论主题");
                return;
            }



            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = pagesize;
            postpramsInfo.Pageindex = pageid;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = 0;
            postpramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;
            if (ismoder == 1)
                postpramsInfo.Usergroupreadaccess = int.MaxValue;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            List<ShowtopicPagePostInfo> postlist = new List<ShowtopicPagePostInfo>();
            if (opinion == 1)
            {
                postlist = Debates.GetPositivePostList(postpramsInfo, out positiveattatchments, ismoder == 1);
            }
            else if (opinion == 2)
            {
                postlist = Debates.GetNegativePostList(postpramsInfo, out negativeattatchments, ismoder == 1);
            }
            DebateInfo debateExpand = Debates.GetDebateTopic(topic.Tid);

            //int positivepagecount = (debateExpand.Positivediggs % pagesize == 0) ? (debateExpand.Positivediggs / pagesize) : (debateExpand.Positivediggs / pagesize + 1);
            //int negativepagecount = (debateExpand.Negativediggs % pagesize == 0) ? (debateExpand.Negativediggs / pagesize) : (debateExpand.Negativediggs / pagesize + 1);

            int positivepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 1);
            int negativepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 2);

            int positivepagecount = (positivepostlistcount % pagesize == 0) ? (positivepostlistcount / pagesize) : (positivepostlistcount / pagesize + 1);
            int negativepagecount = (negativepostlistcount % pagesize == 0) ? (negativepostlistcount / pagesize) : (negativepostlistcount / pagesize + 1);

            bool isenddebate = false;
            if (debateExpand.Terminaltime < DateTime.Now)
            {
                isenddebate = true;
            }

            int bbcodeoff = 1;
            if (forum.Allowbbcode == 1)
            {
                if (usergroupinfo.Allowcusbbcode == 1)
                {
                    bbcodeoff = 0;
                }
            }
            int smileyoff = 1 - forum.Allowsmilies;
            int parseurloff = 0;
            StringBuilder builder = new StringBuilder("{\"postlist\":");

            builder.Append(JavaScriptConvert.SerializeObject(postlist));
            builder.Append(",'debateexpand':");
            builder.Append(JavaScriptConvert.SerializeObject(debateExpand));
            builder.Append(",'pagenumbers':'");
            if (opinion == 1)
            {
                builder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, positivepagecount, "showdebatepage(\\'" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=1&tid=" + topic.Tid + "&{0}\\'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",\\'" + isenddebate + "\\',1," + userid + "," + topicid + ")", 8));
            }
            else
            {
                builder.Append(Utils.GetAjaxPageNumbers(postpramsInfo.Pageindex, negativepagecount, "showdebatepage(\\'" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=2&tid=" + topic.Tid + "&{0}\\'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",\\'" + isenddebate + "\\',2," + userid + "," + topicid + ")", 8));
            }

            builder.Append("'");
            builder.Append("}");

            ResponseText(builder);
        }

        private void ResponseText(string text)
        {
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(text);
            HttpContext.Current.Response.End();
        }

        private void ResponseText(StringBuilder builder)
        {
            ResponseText(builder.ToString());
        }

        private string ValidatePurview()
        {
            return string.Empty;
        }

        #region debate
        /// <summary>
        /// 顶辩论贴
        /// </summary>
        private void DiggDebates()
        {


            int pid = DNTRequest.GetInt("pid", 0);
            int tid = DNTRequest.GetInt("tid", 0);
            int type = DNTRequest.GetInt("type", -1);
            StringBuilder xmlnode = IsValidCouDeba(tid, pid, type);
            if (!xmlnode.ToString().Contains("<error>"))
            {

                Debates.AddDebateDigg(tid, pid, type, userid);

                if (UserGroups.GetUserGroupInfo(7).Allowdiggs == 1)
                {
                    Debates.WriteCookies(pid);
                }
            }

            ResponseXML(xmlnode);

        }

        /// <summary>
        /// 关于辩论贴的验证
        /// </summary>
        /// <param name="tid">主题ID</param>
        /// <param name="pid">帖子ID</param>
        /// <param name="CountenanceType">观点</param>
        /// <returns>返回错误信息</returns>
        private StringBuilder IsValidCouDeba(int tid, int pid, int CountenanceType)
        {
            StringBuilder xmlnode = new StringBuilder();


            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }

            string tip;
            bool allowdiggs = Debates.AllowDiggs(userid, out tip);
            if (!allowdiggs)
            {
                xmlnode.Append("<error>" + tip + "</error>");
                return xmlnode;
            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);
            if (tid == 0 || topicinfo.Special != 4 || pid == 0)
            {
                xmlnode.Append("<error>本主题不是辩论贴，无法支持</error>");
                return xmlnode;
            }
            if (Debates.GetDebateTopic(tid).Terminaltime < DateTime.Now)
            {
                xmlnode.Append("<error>本辩论贴结束时间已到，无法再参与</error>");
                return xmlnode;
            }




            if (CountenanceType != 1 && CountenanceType != 2)
            {
                xmlnode.Append("<error>支持方不能为空</error>");
                return xmlnode;
            }


            if (Debates.IsDigged(pid, userid))
            {
                xmlnode.Append("<error>投过票了</error>");
                return xmlnode;
            }
            return xmlnode;
        }

        private StringBuilder IsValidDebates(int tid, string message)
        {
            StringBuilder xmlnode = new StringBuilder();


            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return xmlnode;
            }


            string reg = @"\[area=([\s\S]+?)\]([\s\S]+?)\[/area\]";
            Regex r = new Regex(reg, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            MatchCollection m = r.Matches(message);
            if (m.Count == 0)
            {
                xmlnode.Append("<error>评论内容不能为空</error>");
                return xmlnode;

            }

            TopicInfo topicinfo = Topics.GetTopicInfo(tid);

            if (tid == 0 || topicinfo.Special != 4)
            {
                xmlnode.Append("<error>本主题不是辩论贴，无法点评</error>");
                return xmlnode;
            }

            if (Debates.GetDebateTopic(tid).Terminaltime > DateTime.Now)
            {
                xmlnode.Append("<error>本辩论贴结束时间未到，无法点评</error>");
                return xmlnode;
            }

            return xmlnode;
        }

        /// <summary>
        /// 点评辩论主题
        /// </summary>
        private void AddCommentDebates()
        {
            string message = DNTRequest.GetString("commentdebates");
            int tid = DNTRequest.GetInt("tid", 0);
            StringBuilder xmlnode = IsValidDebates(tid, message);

            if (!xmlnode.ToString().Contains("<error>"))
            {
                xmlnode.Append("<message>" + message + "</message>");
                Debates.CommentDabetas(tid, Utils.HtmlEncode(ForumUtils.BanWordFilter(message)));
            }
            ResponseXML(xmlnode);
        }
        #endregion
        
        /// <summary>
        /// 获取论坛热门标签
        /// </summary>
        private void GetForumHotTags()
        {
            string filename = Utils.GetMapPath("/" + ForumTags.ForumHotTagJSONPCacheFileName);
            string tags;
            if (!File.Exists(filename))
            {
                ForumTags.WriteHotTagsListForForumJSONPCacheFile(60);
            }

            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    tags = sr.ReadToEnd();
                }
            }

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(tags);
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 获取根据Tag的相关主题
        /// </summary>
        private void GetTopicsWithSameTag()
        {
            int tagid = DNTRequest.GetInt("tagid", 0);
            if (tagid > 0)
            {
                TagInfo tag = Tags.GetTagInfo(tagid);
                if (tag != null)
                {
                    List<MyTopicInfo> topics = Topics.GetTopicsWithSameTag(tagid, config.Tpp);
                    StringBuilder builder = new StringBuilder();
                    builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");
                    builder.Append("<root><![CDATA[ \r\n");
                    builder.Append(@"<div class=""tagthread"">
                                <a class=""close"" href=""javascript:;hideMenu()"" title=""关闭""><img src=""images/common/close.gif"" alt=""关闭"" /></a>
                                <h4>标签: ");
                    builder.Append(string.Format("<font color='{1}'>{0}</font>", tag.Tagname, tag.Color));
                    builder.Append("</h4>\r\n<ul>\r\n");
                    foreach (TopicInfo topic in topics)
                    {
                        builder.Append(string.Format(@"<li><a href=""{0}"" target=""_blank"">{1}</a></li>", base.ShowTopicAspxRewrite(topic.Tid, 1), topic.Title));
                    }
                    builder.Append(string.Format(@"<li class=""more""><a href=""tags.aspx?tagid={0}"" target=""_blank"">查看更多</a></li>", tag.Tagid));
                    builder.Append("</ul>\r\n");
                    builder.Append(@"</div>
                                ]]></root>");

                    ResponseXML(builder);
                }
            }
        }

        /// <summary>
        /// 读取主题标签缓存文件
        /// </summary>
        private void GetTopicTags()
        {
            int topicid = DNTRequest.GetInt("topicid", 0);
            if (topicid > 0)
            {
                StringBuilder dir = new StringBuilder();
                dir.Append("/cache/topic/magic/");
                dir.Append((topicid / 1000 + 1).ToString());
                dir.Append("/");
                string filename = Utils.GetMapPath(dir.ToString() + topicid.ToString() + "_tags.config");
                string tags;
                if (!File.Exists(filename))
                {
                    ForumTags.WriteTopicTagsCacheFile(topicid);
                }

                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                    {
                        tags = sr.ReadToEnd();
                    }
                }


                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(tags);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// 获取关键字分词
        /// </summary>
        private void GetRelateKeyword()
        {
            string title = Utils.UrlEncode(Utils.RemoveHtml(UBB.ClearUBB(DNTRequest.GetString("titleenc").Trim())));
            string content = Utils.UrlEncode(Utils.RemoveHtml(UBB.ClearUBB(DNTRequest.GetString("contentenc").Trim())));

            string xmlContent = Utils.GetSourceTextByUrl(string.Format("http://keyword.discuz.com/related_kw.html?title={0}&content={1}&ics=utf-8&ocs=utf-8", title, content.Length < 500 ? content : content.Substring(0, 500)));

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xmlContent);

            XmlNodeList xnl = xmldoc.GetElementsByTagName("kw");
            StringBuilder builder = new StringBuilder();
            foreach (XmlNode node in xnl)
            {
                builder.AppendFormat("{0} ", node.InnerText);

            }

            StringBuilder xmlBuilder = new StringBuilder(string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                                            <root><![CDATA[
                                            <script type=""text/javascript"">
                                            var tagsplit = $('tags').value.split(' ');
                                            var inssplit = '{0}';
                                            var returnsplit = inssplit.split(' ');
                                            var result = '';
                                            for(i in tagsplit) {{
                                                for(j in returnsplit) {{
                                                    if(tagsplit[i] == returnsplit[j]) {{
                                                        tagsplit[i] = '';break;
                                                    }}
                                                }}
                                            }}

                                            for(i in tagsplit) {{
                                                if(tagsplit[i] != '') {{
                                                    result += tagsplit[i] + ' ';
                                                }}
                                            }}
                                            $('tags').value = result + '{0}';
                                            </script>
                                            ]]></root>", builder.ToString()));

            ResponseXML(xmlBuilder);
        }

        /// <summary>
        /// 输出表情字符串
        /// </summary>
        private void GetSmilies()
        {
            //如果不是提交...
            if (ForumUtils.IsCrossSitePost())
            {
                return;
            }

            string smilies = Caches.GetSmiliesCache();
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write("{" + smilies + "}");
            HttpContext.Current.Response.End();

        }

        /// <summary>
        /// 检查Rewritename是否存在
        /// </summary>
        private void CheckRewriteName()
        {

            if (userid == -1)
            {
                return;
            }

            string rewriteName = DNTRequest.GetString("rewritename").Trim();
            string result = "0";
            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlnode.Append("<result>");
            xmlnode.Append(result);
            xmlnode.Append("</result>");
            ResponseXML(xmlnode);
        }

        #region QuickReply

        /// <summary>
        /// 验证回帖的条件
        /// </summary>
        /// <param name="xmlnode">xml节点</param>
        /// <param name="topicid">主题id</param>
        /// <param name="topic">主题信息</param>
        /// <param name="forum">版块信息</param>
        /// <param name="admininfo">管理组信息</param>
        /// <param name="postmessage">帖子内容</param>
        /// <returns>验证结果</returns>
        private bool IsQuickReplyValid(StringBuilder xmlnode, int topicid, TopicInfo topic, ForumInfo forum, AdminGroupInfo admininfo, string postmessage)
        {
            //如果不是提交...
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                return false;
            }


            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段,当前时间如果在其中的一个时间段内,不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                {
                    xmlnode.AppendFormat("<error>在此时间段( {0} )内用户不可以发帖</error>", visittime);
                    return false;
                }
            }

            // 如果主题ID非数字
            if (topicid == -1)
            {
                xmlnode.Append("<error>无效的主题ID</error>");
                return false;
            }


            // 如果该主题不存在
            if (topic == null)
            {
                xmlnode.Append("<error>不存在的主题ID</error>");
                return false;
            }



            //　如果当前用户非管理员并且该主题已关闭,不允许用户发帖
            if (admininfo == null || !Moderators.IsModer(admininfo.Admingid, userid, forum.Fid))
            {
                if (topic.Closed == 1)
                {
                    xmlnode.Append("<error>主题已关闭无法回复</error>");
                    return false;
                }
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && !Utils.InArray(username, forum.Moderators.Split(',')))
            {
                xmlnode.AppendFormat("<error>本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够</error>", topic.Readperm.ToString(), usergroupinfo.Grouptitle);
                return false;
            }

            if (forum.Replyperm == null || forum.Replyperm == string.Empty)//板块权限设置为空时,根据用户组权限判断
            {
                // 验证用户是否有发表主题的权限
                if (usergroupinfo.Allowreply != 1)
                {
                    xmlnode.AppendFormat("<error>您当前的身份 \"{0}\" 没有发表回复的权限</error>", usergroupinfo.Grouptitle);
                    return false;
                }
            }
            else//板块权限不为空,根据板块权限判断
            {
                if (!Forums.AllowReply(forum.Replyperm, usergroupid))
                {
                    xmlnode.Append("<error>您没有在该版块发表回复的权限</error>");
                    return false;
                }
            }

            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {

                int Interval = Utils.StrDateDiffSeconds(lastposttime, config.Postinterval);
                if (Interval < 0)
                {
                    xmlnode.AppendFormat("<error>系统规定发帖间隔为{0}秒, 您还需要等待 {1} 秒</error>", config.Postinterval.ToString(), (Interval * -1).ToString());
                    return false;
                }
                else if (userid != -1)
                {
                    string joindate = Discuz.Forum.Users.GetUserJoinDate(userid);
                    if (joindate == "")
                    {
                        xmlnode.Append("<error>您的用户资料出现错误</error>");
                        return false;
                    }

                    Interval = Utils.StrDateDiffMinutes(joindate, config.Newbiespan);
                    if (Interval < 0)
                    {
                        xmlnode.AppendFormat("<error>系统规定新注册用户必须要在{0}分钟后才可以发帖, 您还需要等待 {1} 分</error>", config.Newbiespan.ToString(), (Interval * -1).ToString());
                        return false;
                    }

                }
            }

            //if (DNTRequest.GetString("title").Trim().Equals(""))
            //{
            //    xmlnode.Append("<error>主题不能为空</error>");
            //    return false;
            //}
            if (DNTRequest.GetString("title").IndexOf("　") != -1)
            {
                xmlnode.Append("<error>主题不能包含全角空格符</error>");
                return false;
            }
            if (DNTRequest.GetString("title").Length > 60)
            {
                xmlnode.AppendFormat("<error>主题最大长度为60个字符,当前为 {0} 个字符</error>", DNTRequest.GetString("title").Length.ToString());
                return false;
            }


            if ("".Equals(postmessage))
            {
                xmlnode.Append("<error>内容不能为空</error>");
                return false;
            }

            if (admininfo != null && admininfo.Disablepostctrl != 1)
            {
                if (postmessage.Length < config.Minpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过少, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
                else if (postmessage.Length > config.Maxpostsize)
                {
                    xmlnode.AppendFormat("<error>您发表的内容过多, 系统设置要求帖子内容不得少于 {0} 字多于 {1} 字</error>", config.Minpostsize.ToString(), config.Maxpostsize.ToString());
                    return false;
                }
            }

            if (topic.Special == 4 && DNTRequest.GetInt("debateopinion", 0) == 0)
            {
                xmlnode.AppendFormat("<error>请选择您在辩论中的观点</error>");
                return false;
            }

            return true;
        }


        /// <summary>
        /// 快速回复
        /// </summary>
        private void QuickReply()
        {
            TopicInfo topic;
            int forumid;
            string forumname;
            int topicid;
            int postid = 0;
            string topictitle;

            int smileyoff;

            ForumInfo forum;

            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            // 获取主题ID
            topicid = DNTRequest.GetInt("topicid", -1);
            PostInfo postinfo;
            int layer = 1;
            int parentid = 0;
            topictitle = "";

            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);

            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);

            forumid = topic.Fid;

            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name.Trim();

            string postmessage = DNTRequest.GetString("message").Trim();


            if (!IsQuickReplyValid(xmlnode, topicid, topic, forum, admininfo, postmessage))
            {
                ResponseXML(xmlnode);
                return;
            }



            smileyoff = 1 - forum.Allowsmilies;

            // 在线状态如果为精确, 则更新用户动作, 否则不更新
            OnlineUsers.UpdateAction(olid, UserAction.PostReply.ActionID, forumid, forumname, topicid, topictitle, config.Onlinetimeout);




            //			int iconid = DNTRequest.GetInt("iconid", 0);
            //			if (iconid > 15)
            //			{
            //				iconid = 0;
            //			}

            int hide = 0;
            if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
            {
                hide = 1;
            }

            string curdatetime = Utils.GetDateTime();

            postinfo = new PostInfo();
            postinfo.Fid = forumid;
            postinfo.Tid = topicid;
            postinfo.Parentid = parentid;
            postinfo.Layer = layer;
            postinfo.Poster = username;
            postinfo.Posterid = userid;


            postinfo.Postdatetime = curdatetime;
            if (useradminid == 1)
            {
                postinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                postinfo.Message = Utils.HtmlEncode(postmessage);
            }
            else
            {
                postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
                postinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
            }

            if (ForumUtils.HasBannedWord(postinfo.Title) || ForumUtils.HasBannedWord(postinfo.Message))
            {
                ResponseXML(xmlnode.Append("<error>对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!</error>"));
                return;
            }


            postinfo.Ip = DNTRequest.GetIP();
            postinfo.Lastedit = "";

            int disablepost = 0;
            if (admininfo != null)
            {
                disablepost = admininfo.Disablepostctrl;
            }
            //判断当前版块以及用户所属组的审核设置来确定贴子是否需要审核
            if (forum.Modnewposts == 1 && useradminid != 1)
            {
                postinfo.Invisible = 1;
            }
            else
            {
                postinfo.Invisible = 0;
            }

            //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
            if (useradminid != 1)
            {
                if (Scoresets.BetweenTime(config.Postmodperiods))
                {
                    postinfo.Invisible = 1;
                }

                if (ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                {
                    postinfo.Invisible = 1;
                }
            }



            //　如果当前用户非管理员并且论坛设定了发帖审核时间段,当前时间如果在其中的一个时间段内,则用户所发帖均为待审核状态
            if (useradminid != 1)
            {
                if (Scoresets.BetweenTime(config.Postmodperiods))
                {
                    postinfo.Invisible = 1;
                }

                if (ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                {
                    postinfo.Invisible = 1;
                }
            }


            postinfo.Usesig = Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
            postinfo.Htmlon = 1;

            postinfo.Smileyoff = 1;
            if (smileyoff == 0)
            {
                postinfo.Smileyoff = Utils.StrToInt(DNTRequest.GetString("smileyoff"), 0);
            }

            postinfo.Bbcodeoff = 1;
            if (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1)
            {
                postinfo.Bbcodeoff = Utils.StrToInt(DNTRequest.GetString("bbcodeoff"), 0);
            }
            postinfo.Parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);
            postinfo.Attachment = 0;
            postinfo.Rate = 0;
            postinfo.Ratetimes = 0;
            postinfo.Debateopinion = DNTRequest.GetInt("debateopinion", 0);
            postinfo.Topictitle = topic.Title;

            // 产生新帖子
            try
            {
                postid = Posts.CreatePost(postinfo);
            }
            catch
            {
                ResponseXML(xmlnode.Append("<error>提交失败,请稍后重试！</error>"));
                return;
            }

            if (postinfo.Invisible == 1)
            {
                ResponseXML(xmlnode.Append("<error>发表回复成功, 但需要经过审核才可以显示!</error>"));
                return;
            }

            if (hide == 1)
            {
                topic.Hide = hide;
                Topics.UpdateTopicHide(topicid);
            }
            Topics.UpdateTopicReplies(topicid);
            Topics.AddParentForumTopics(forum.Parentidlist.Trim(), 0, 1);



            //设置用户的积分
            ///首先读取版块内自定义积分
            ///版设置了自定义积分则使用,否则使用论坛默认积分
            float[] values = null;
            if (!forum.Replycredits.Equals(""))
            {
                int index = 0;
                float tempval;
                values = new float[8];
                foreach (string ext in Utils.SplitString(forum.Replycredits, ","))
                {

                    if (index == 0)
                    {
                        if (!ext.Equals("True"))
                        {
                            values = null;
                            break;
                        }
                        index++;
                        continue;
                    }
                    tempval = Utils.StrToFloat(ext, 0);
                    values[index - 1] = tempval;
                    index++;
                    if (index > 8)
                    {
                        break;
                    }
                }
            }

            if (values != null)
            {
                ///使用版块内积分
                UserCredits.UpdateUserCreditsByPosts(userid, values);

            }
            else
            {
                ///使用默认积分
                UserCredits.UpdateUserCreditsByPosts(userid);
            }





            // 更新在线表中的用户最后发帖时间
            OnlineUsers.UpdatePostTime(olid);



            xmlnode = GetNewPostXML(xmlnode, postinfo, forum, topic, postid);

            // 删除主题游客缓存
            if (topic.Replies < (config.Ppp + 10))
            {
                ForumUtils.DeleteTopicCacheFile(topicid);
            }

            ResponseXML(xmlnode);

        }


        /// <summary>
        /// 得到输出xml字符串
        /// </summary>
        /// <param name="xmlnode">xml节点</param>
        /// <param name="postinfo">帖子信息</param>
        /// <param name="forum">版块信息</param>
        /// <param name="topic">主题信息</param>
        /// <param name="postid">帖子id</param>
        /// <returns>xml结果</returns>
        private StringBuilder GetNewPostXML(StringBuilder xmlnode, PostInfo postinfo, ForumInfo forum, TopicInfo topic, int postid)
        {
            int hide = 1;
            if (topic.Hide == 1 || ForumUtils.IsHidePost(postinfo.Message))
            {
                hide = -1;
            }
            if (usergroupinfo.Allowhidecode == 0)
                hide = 0;
            //判断是否为回复可见帖, price=0为非购买可见(正常), price > 0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topic.Price > 0)
            {
                price = topic.Price;
                if (PaymentLogs.IsBuyer(topic.Tid, userid))//判断当前用户是否已经购买
                {
                    price = -1;
                }
            }

            PostpramsInfo _postpramsinfo = new PostpramsInfo();
            _postpramsinfo.Fid = forum.Fid;
            _postpramsinfo.Tid = postinfo.Tid;
            _postpramsinfo.Pid = postinfo.Pid;
            _postpramsinfo.Jammer = forum.Jammer;
            _postpramsinfo.Pagesize = 1;
            _postpramsinfo.Pageindex = 1;
            _postpramsinfo.Getattachperm = forum.Getattachperm;
            _postpramsinfo.Usergroupid = usergroupid;
            _postpramsinfo.Attachimgpost = config.Attachimgpost;
            _postpramsinfo.Showattachmentpath = config.Showattachmentpath;
            _postpramsinfo.Hide = hide;
            _postpramsinfo.Price = price;
            _postpramsinfo.Ubbmode = false;

            _postpramsinfo.Showimages = forum.Allowimgcode;
            _postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            _postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            _postpramsinfo.Smiliesmax = config.Smiliesmax;
            _postpramsinfo.Bbcodemode = config.Bbcodemode;
            _postpramsinfo.Smileyoff = postinfo.Smileyoff;
            _postpramsinfo.Bbcodeoff = postinfo.Bbcodeoff;
            _postpramsinfo.Parseurloff = postinfo.Parseurloff;
            _postpramsinfo.Allowhtml = postinfo.Htmlon;

            _postpramsinfo.Sdetail = postinfo.Message;

            string message = UBB.UBBToHTML(_postpramsinfo);

            UserInfo userinfo = Discuz.Forum.Users.GetUserInfo(userid);

            int adcount = Advertisements.GetInPostAdCount("", postinfo.Fid);
            Random random = new Random(unchecked((int)DateTime.Now.Ticks));

            //头衔、星星
            UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(Utils.StrToInt(usergroupid, UserCredits.GetCreditsUserGroupID(Utils.StrToInt(userinfo.Credits.ToString(), 0)).Groupid));
            string status = tmpUserGroupInfo.Grouptitle;
            if (!tmpUserGroupInfo.Color.Equals(""))
            {
                status = "<font color=\"" + tmpUserGroupInfo.Color + "\">" + tmpUserGroupInfo.Grouptitle + "</font>";
            }
            int stars = tmpUserGroupInfo.Stars;
            string medals;
            medals = "".Equals(userinfo.Medals.Trim()) ? "" : Caches.GetMedalsList(userinfo.Medals);


            xmlnode.Append("<post>\r\n\t");

            xmlnode.AppendFormat("<ismoder>{0}</ismoder>", Moderators.IsModer(useradminid, userid, topic.Fid) ? 1 : 0);
            xmlnode.AppendFormat(Advertisements.GetInPostAdXMLByFloor("", postinfo.Fid, templatepath, (topic.Replies + 2) % 15));
            xmlnode.AppendFormat("<id>{0}</id>\r\n\t", topic.Replies + 2);
            xmlnode.AppendFormat("<status><![CDATA[{0}]]></status>\r\n\t", status);
            xmlnode.AppendFormat("<stars>{0}</stars>\r\n\t", stars);

            xmlnode.AppendFormat("<fid>{0}</fid>\r\n\t", postinfo.Fid);
            //xmlnode.AppendFormat("<bbcodeoff>{0}</bbcodeoff>\r\n\t", postinfo.Bbcodeoff);
            xmlnode.AppendFormat("<invisible>{0}</invisible>\r\n\t", postinfo.Invisible);
            xmlnode.AppendFormat("<ip>{0}</ip>\r\n\t", postinfo.Ip);
            xmlnode.AppendFormat("<lastedit>{0}</lastedit>\r\n\t", postinfo.Lastedit);
            xmlnode.AppendFormat("<layer>{0}</layer>\r\n\t", postinfo.Layer);
            xmlnode.AppendFormat("<message><![CDATA[{0}]]></message>\r\n\t", message);
            xmlnode.AppendFormat("<parentid>{0}</parentid>\r\n\t", postinfo.Parentid);
            xmlnode.AppendFormat("<pid>{0}</pid>\r\n\t", postid);
            xmlnode.AppendFormat("<postdatetime>{0}</postdatetime>\r\n\t", postinfo.Postdatetime.Substring(0, postinfo.Postdatetime.Length - 3));
            xmlnode.AppendFormat("<poster>{0}</poster>\r\n\t", postinfo.Poster);
            xmlnode.AppendFormat("<posterid>{0}</posterid>\r\n\t", postinfo.Posterid);
            xmlnode.AppendFormat("<smileyoff>{0}</smileyoff>\r\n\t", postinfo.Smileyoff);
            xmlnode.AppendFormat("<topicid>{0}</topicid>\r\n\t", postinfo.Tid);
            xmlnode.AppendFormat("<title>{0}</title>\r\n\t", Utils.HtmlEncode(postinfo.Title));
            xmlnode.AppendFormat("<usesig>{0}</usesig>\r\n", postinfo.Usesig);
            xmlnode.AppendFormat("<debateopinion>{0}</debateopinion>", postinfo.Debateopinion);

            xmlnode.AppendFormat("<uid>{0}</uid>\r\n\t", userinfo.Uid);
            xmlnode.AppendFormat("<accessmasks>{0}</accessmasks>\r\n\t", userinfo.Accessmasks);
            xmlnode.AppendFormat("<adminid>{0}</adminid>\r\n\t", userinfo.Adminid);
            xmlnode.AppendFormat("<avatar>{0}</avatar>\r\n\t", userinfo.Avatar.Replace("\\", "/"));
            xmlnode.AppendFormat("<avatarheight>{0}</avatarheight>\r\n\t", userinfo.Avatarheight);
            xmlnode.AppendFormat("<avatarshowid>{0}</avatarshowid>\r\n\t", userinfo.Avatarshowid);
            xmlnode.AppendFormat("<avatarwidth>{0}</avatarwidth>\r\n\t", userinfo.Avatarwidth);
            xmlnode.AppendFormat("<bday>{0}</bday>\r\n\t", userinfo.Bday);
            //xmlnode.AppendFormat("<bio>{0}</bio>\r\n\t", userinfo.Bio);
            xmlnode.AppendFormat("<credits>{0}</credits>\r\n\t", userinfo.Credits);
            xmlnode.AppendFormat("<digestposts>{0}</digestposts>\r\n\t", userinfo.Digestposts);
            xmlnode.AppendFormat("<email>{0}</email>\r\n\t", userinfo.Email.Trim());


            string[] score = Scoresets.GetValidScoreName();
            xmlnode.AppendFormat("<score1>{0}</score1>\r\n\t", score[1]);
            xmlnode.AppendFormat("<score2>{0}</score2>\r\n\t", score[2]);
            xmlnode.AppendFormat("<score3>{0}</score3>\r\n\t", score[3]);
            xmlnode.AppendFormat("<score4>{0}</score4>\r\n\t", score[4]);
            xmlnode.AppendFormat("<score5>{0}</score5>\r\n\t", score[5]);
            xmlnode.AppendFormat("<score6>{0}</score6>\r\n\t", score[6]);
            xmlnode.AppendFormat("<score7>{0}</score7>\r\n\t", score[7]);
            xmlnode.AppendFormat("<score8>{0}</score8>\r\n\t", score[8]);
            string[] scoreunit = Scoresets.GetValidScoreUnit();
            xmlnode.AppendFormat("<scoreunit1>{0}</scoreunit1>\r\n\t", scoreunit[1]);
            xmlnode.AppendFormat("<scoreunit2>{0}</scoreunit2>\r\n\t", scoreunit[2]);
            xmlnode.AppendFormat("<scoreunit3>{0}</scoreunit3>\r\n\t", scoreunit[3]);
            xmlnode.AppendFormat("<scoreunit4>{0}</scoreunit4>\r\n\t", scoreunit[4]);
            xmlnode.AppendFormat("<scoreunit5>{0}</scoreunit5>\r\n\t", scoreunit[5]);
            xmlnode.AppendFormat("<scoreunit6>{0}</scoreunit6>\r\n\t", scoreunit[6]);
            xmlnode.AppendFormat("<scoreunit7>{0}</scoreunit7>\r\n\t", scoreunit[7]);
            xmlnode.AppendFormat("<scoreunit8>{0}</scoreunit8>\r\n\t", scoreunit[8]);

            xmlnode.AppendFormat("<extcredits1>{0}</extcredits1>\r\n\t", userinfo.Extcredits1);
            xmlnode.AppendFormat("<extcredits2>{0}</extcredits2>\r\n\t", userinfo.Extcredits2);
            xmlnode.AppendFormat("<extcredits3>{0}</extcredits3>\r\n\t", userinfo.Extcredits3);
            xmlnode.AppendFormat("<extcredits4>{0}</extcredits4>\r\n\t", userinfo.Extcredits4);
            xmlnode.AppendFormat("<extcredits5>{0}</extcredits5>\r\n\t", userinfo.Extcredits5);
            xmlnode.AppendFormat("<extcredits6>{0}</extcredits6>\r\n\t", userinfo.Extcredits6);
            xmlnode.AppendFormat("<extcredits7>{0}</extcredits7>\r\n\t", userinfo.Extcredits7);
            xmlnode.AppendFormat("<extcredits8>{0}</extcredits8>\r\n\t", userinfo.Extcredits8);
            xmlnode.AppendFormat("<extgroupids>{0}</extgroupids>\r\n\t", userinfo.Extgroupids.Trim());
            xmlnode.AppendFormat("<gender>{0}</gender>\r\n\t", userinfo.Gender);
            xmlnode.AppendFormat("<icq>{0}</icq>\r\n\t", userinfo.Icq);
            //xmlnode.AppendFormat("<invisible>{0}</invisible>\r\n\t", userinfo.Invisible);
            xmlnode.AppendFormat("<joindate>{0}</joindate>\r\n\t", userinfo.Joindate);
            xmlnode.AppendFormat("<lastactivity>{0}</lastactivity>\r\n\t", userinfo.Lastactivity);
            xmlnode.AppendFormat("<medals><![CDATA[{0}]]></medals>\r\n\t", medals);
            xmlnode.AppendFormat("<nickname>{0}</nickname>\r\n\t", userinfo.Nickname);
            xmlnode.AppendFormat("<oltime>{0}</oltime>\r\n\t", userinfo.Oltime);
            xmlnode.AppendFormat("<onlinestate>{0}</onlinestate>\r\n\t", userinfo.Onlinestate);
            xmlnode.AppendFormat("<showemail>{0}</showemail>\r\n\t", userinfo.Showemail);
            //xmlnode.AppendFormat("<sightml>{0}</sightml>", userinfo.Sightml);
            xmlnode.AppendFormat("<signature><![CDATA[{0}]]></signature>\r\n\t", userinfo.Sightml);
            xmlnode.AppendFormat("<sigstatus>{0}</sigstatus>\r\n\t", userinfo.Sigstatus);
            xmlnode.AppendFormat("<skype>{0}</skype>\r\n\t", userinfo.Skype);
            //xmlnode.AppendFormat("<username>{0}</username>\r\n\t", userinfo.Username);
            xmlnode.AppendFormat("<website>{0}</website>\r\n\t", userinfo.Website);
            xmlnode.AppendFormat("<yahoo>{0}</yahoo>\r\n", userinfo.Yahoo);
            xmlnode.AppendFormat("<qq>{0}</qq>\r\n", userinfo.Qq);
            xmlnode.AppendFormat("<msn>{0}</msn>\r\n", userinfo.Msn);
            xmlnode.AppendFormat("<posts>{0}</posts>\r\n", userinfo.Posts);
            xmlnode.AppendFormat("<location>{0}</location>\r\n", userinfo.Location);

            xmlnode.AppendFormat("<showavatars>{0}</showavatars>\r\n", config.Showavatars);
            xmlnode.AppendFormat("<userstatusby>{0}</userstatusby>\r\n", config.Userstatusby);
            xmlnode.AppendFormat("<starthreshold>{0}</starthreshold>\r\n", config.Starthreshold);
            xmlnode.AppendFormat("<forumtitle>{0}</forumtitle>\r\n", config.Forumtitle);
            xmlnode.AppendFormat("<showsignatures>{0}</showsignatures>\r\n", config.Showsignatures);
            xmlnode.AppendFormat("<maxsigrows>{0}</maxsigrows>\r\n", config.Maxsigrows);
            xmlnode.Append("</post>\r\n");

            return xmlnode;

        }


        #endregion

        #region 举报功能

        private void Report()
        {
            if (ForumUtils.IsCrossSitePost())
            {
                return;
            }

            if (userid == -1)
            {
                return;
            }

            string reportUrl = DNTRequest.GetString("report_url");
            string reportmessage = DNTRequest.GetString("reportmessage");

            StringBuilder xmlnode = new StringBuilder();


            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            if (reportmessage == string.Empty || reportmessage.Length < 15)
            {
                xmlnode.Append("<error>您的理由必须多余15个字</error>");

            }
            else
            {

                if (reportUrl != string.Empty)
                {
                    PrivateMessageInfo pm = new PrivateMessageInfo();
                    string message = string.Format(@"下面的链接地址被举报,<br /><a href='{0}' target='_blank'>{0}</a><br />请检查<br/>举报理由：{1}", reportUrl, reportmessage);
                    string curdate = Utils.GetDateTime();
                    Hashtable ht = Discuz.Forum.Users.GetReportUsers();
                    foreach (DictionaryEntry de in ht)
                    {
                        pm.Message = message;
                        pm.Subject = "举报通知";
                        pm.Msgto = de.Value.ToString();
                        pm.Msgtoid = Utils.StrToInt(de.Key, 0);
                        pm.Msgfrom = username;
                        pm.Msgfromid = userid;
                        pm.New = 1;
                        pm.Postdatetime = curdate;
                        pm.Folder = 0;
                        PrivateMessages.CreatePrivateMessage(pm, 0);
                    }
                }
            }
            ResponseXML(xmlnode);
        }

        #endregion

        /// <summary>
        /// 获得指定版块的子版块信息,以xml文件输出
        /// </summary>
        public void GetForumTree()
        {
            StringBuilder xmlnode = new StringBuilder();

            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlnode.Append("<data>\n");

            int fid = DNTRequest.GetInt("fid", 0);
            DataTable dt = Forums.GetForumList(fid);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (config.Hideprivate == 1 && dr["viewperm"].ToString() != "" && !Utils.InArray(usergroupid.ToString(), dr["viewperm"].ToString()))
                            continue;

                        xmlnode.Append("<forum name=\"");
                        xmlnode.Append(Utils.RemoveHtml(dr["name"].ToString().Trim()).Replace("&", "&amp;"));
                        xmlnode.Append("\" fid=\"");
                        xmlnode.Append(dr["fid"]);
                        xmlnode.Append("\" subforumcount=\"");
                        xmlnode.Append(dr["subforumcount"]);
                        xmlnode.Append("\" layer=\"");
                        xmlnode.Append(dr["layer"]);
                        xmlnode.Append("\" parentid=\"");
                        xmlnode.Append(dr["parentid"]);
                        xmlnode.Append("\" parentidlist=\"");
                        xmlnode.Append(dr["parentidlist"].ToString().Trim());
                        xmlnode.Append("\" />\n");
                    }
                }
            }
            xmlnode.Append("</data>\n");

            //向页面输出xml内容
            ResponseXML(xmlnode);

        }

        /// <summary>
        /// 获得指定主题的回复信息,以xml文件输出
        /// </summary>
        public void GetTopicTree()
        {
            int topicid = DNTRequest.GetInt("topicid", 0);

            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");

            TopicInfo topic = Topics.GetTopicInfo(topicid);
            ForumInfo forum = Forums.GetForumInfo(topic.Fid);
            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 && !Utils.InArray(username, forum.Moderators.Split(',')))
            {
                xmlnode.Append("<error>本主题阅读权限为: " + topic.Readperm.ToString() + ", 您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 阅读权限不够</error>");
                ResponseXML(xmlnode);
                return;
            }

            xmlnode.Append("<data>\n");


            DataTable dt = Posts.GetPostTree(topicid);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Select("layer>0", "pid asc"))
                    {
                        if (Utils.StrToInt(dr["layer"], 0) != 0)
                        {
                            xmlnode.Append("<post title=\"");
                            xmlnode.Append(UBB.ClearBR(UBB.ClearUBB(dr["title"].ToString())));
                            xmlnode.Append("\" pid=\"");
                            xmlnode.Append(dr["pid"]);
                            xmlnode.Append("\" message=\"");
                            if (UBB.ClearBR(UBB.ClearUBB(dr["message"].ToString())).Equals(""))
                            {
                                xmlnode.Append(dr["title"]);
                            }
                            else
                            {
                                string tt = dr["message"].ToString();
                                if (tt.IndexOf("[hide]") > -1)
                                {
                                    xmlnode.Append("*** 隐藏帖 ***");
                                }
                                else
                                {
                                    xmlnode.Append(UBB.ClearBR(UBB.ClearUBB(dr["message"].ToString())));
                                }
                            }

                            xmlnode.Append("\" postdatetime=\"");
                            xmlnode.Append(DateTime.Parse(dr["postdatetime"].ToString()).ToString("yyyy-MM-dd HH:mm"));
                            xmlnode.Append("\" poster=\"");
                            xmlnode.Append(Utils.HtmlEncode(dr["poster"].ToString()));
                            xmlnode.Append("\" posterid=\"");
                            xmlnode.Append(dr["posterid"]);
                            xmlnode.Append("\" />\n");
                        }
                    }
                    if (xmlnode.Length > 0)
                    {
                        xmlnode = xmlnode.Replace("&", "");
                    }
                }
            }

            xmlnode.Append("</data>\n");
            ResponseXML(xmlnode);

        }


        /// <summary>
        /// 查询用户名是否存在
        /// </summary>
        public void CheckUserName()
        {
            if(config==null)
            {
                config = GeneralConfigs.GetConfig();
            }

            if (DNTRequest.GetString("username").Trim() == "")
                return;
            string result = "0";
            string tmpUsername = DNTRequest.GetString("username").Trim();
            if (tmpUsername.IndexOf("　") != -1)
            {
                //AddErrLine("用户名中不允许包含全角空格符");
                result = "1";
            }
            else if (tmpUsername.IndexOf(" ") != -1)
            {
                //AddErrLine("用户名中不允许包含空格");
                result = "1";
            }
            else if (tmpUsername.IndexOf(":") != -1)
            {
                //AddErrLine("用户名中不允许包含冒号");
                result = "1";
            }
            else if (Users.Exists(tmpUsername))
            {
                //AddErrLine("该用户名已存在");
                result = "1";
            }
            else if ((!Utils.IsSafeSqlString(tmpUsername)) || (!Utils.IsSafeUserInfoString(tmpUsername)))
            {
                //AddErrLine("用户名中存在非法字符");
                result = "1";
            }
            // 如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同...
            else if (tmpUsername.Trim() == PrivateMessages.SystemUserName || ForumUtils.IsBanUsername(tmpUsername, config.Censoruser))
            {
                //AddErrLine("用户名 \"" + tmpUsername + "\" 不允许在本论坛使用, 本论坛不允许用户名使用这些词语: " + config.Censoruser.Replace("\n", ",").Replace("\r", ""));
                result = "1";
            }


            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n");
            xmlnode.Append("<result>");
            xmlnode.Append(result);
            xmlnode.Append("</result>");
            ResponseXML(xmlnode);

        }

        /// <summary>
        /// 获得帖子评分列表
        /// </summary>
        public void GetRateLogList()
        {
            System.Text.StringBuilder xmlnode = new System.Text.StringBuilder();
            xmlnode.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n");

            //如果不是提交...
            if (!ispost || ForumUtils.IsCrossSitePost())
            {
                xmlnode.Append("<error>您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。</error>");
                ResponseXML(xmlnode);
                return;
            }

            int pid = DNTRequest.GetFormInt("pid", 0);

            DataTable rateList = Posts.GetPostRateList(pid);
            if (rateList == null || rateList.Rows.Count == 0)
            {
                xmlnode.Append("<error>该帖没有评分记录</error>");
                ResponseXML(xmlnode);
                return;
            }
            xmlnode.Append("<data>\r\n");

            string[] scorename = Scoresets.GetValidScoreName();
            string[] scoreunit = Scoresets.GetValidScoreUnit();


            foreach (DataRow rate in rateList.Rows)
            {
                xmlnode.Append("<ratelog>");

                xmlnode.AppendFormat("\r\n\t<rateid>{0}</rateid>", rate["id"]);
                xmlnode.AppendFormat("\r\n\t<uid>{0}</uid>", rate["uid"]);
                xmlnode.AppendFormat("\r\n\t<username>{0}</username>", rate["username"].ToString().Trim());
                xmlnode.AppendFormat("\r\n\t<extcredits>{0}</extcredits>", rate["extcredits"]);
                xmlnode.AppendFormat("\r\n\t<extcreditsname>{0}</extcreditsname>", scorename[Convert.ToInt32(rate["extcredits"])]);
                xmlnode.AppendFormat("\r\n\t<extcreditsunit>{0}</extcreditsunit>", scoreunit[Convert.ToInt32(rate["extcredits"])]);
                xmlnode.AppendFormat("\r\n\t<postdatetime>{0}</postdatetime>", Convert.ToDateTime(rate["postdatetime"]).ToString("yyyy-MM-dd HH:mm:ss"));
                xmlnode.AppendFormat("\r\n\t<score>{0}</score>", Convert.ToInt32(rate["score"]) > 0 ? ("+" + rate["score"]) : rate["score"]);
                xmlnode.AppendFormat("\r\n\t<reason>{0}</reason>", rate["reason"].ToString().Trim());


                xmlnode.Append("\r\n</ratelog>\r\n");
            }
            xmlnode.Append("</data>");

            ResponseXML(xmlnode);




        }

        //private void GetDebateTopic(int tid)
        //{
        //    HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        //    HttpContext.Current.Response.Expires = -1;
        //    HttpContext.Current.Response.Clear();
        //    HttpContext.Current.Response.Write(Debates.GetDebatesJson(tid));
        //    HttpContext.Current.Response.End();
        //}

        private void Getdebatesjsonlist(string callback, string tidllist)
        {
            HttpContext.Current.Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            HttpContext.Current.Response.Expires = -1;
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(Debates.GetDebatesJsonList(callback, tidllist));
            HttpContext.Current.Response.End();
        }


        #region Helper
        /// <summary>
        /// 向页面输出xml内容
        /// </summary>
        /// <param name="xmlnode">xml内容</param>
        private void ResponseXML(System.Text.StringBuilder xmlnode)
        {
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "Text/XML";
            System.Web.HttpContext.Current.Response.Expires = 0;

            System.Web.HttpContext.Current.Response.Cache.SetNoStore();
            System.Web.HttpContext.Current.Response.Write(xmlnode.ToString());
            System.Web.HttpContext.Current.Response.End();
        }
        #endregion


    } //class
} //namespace
