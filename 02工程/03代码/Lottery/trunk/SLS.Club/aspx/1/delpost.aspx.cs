using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 删除帖子页
    /// </summary>
    public partial class delpost : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid;
        /// <summary>
        /// 帖子信息
        /// </summary>
        public PostInfo post;
        /// <summary>
        /// 所属主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 所属主题Id
        /// </summary>
        public int topicid;
        /// <summary>
        /// 所属主题标题
        /// </summary>
        public string topictitle;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 版块信息
        /// </summary>
        public ForumInfo forum;
        private bool ismoder = false;
        #endregion

        // 是否允许删除帖子, 初始false为不允许
        bool allowdelpost = false;

        protected override void ShowPage()
        {
            
            // 获取帖子ID
            topicid = DNTRequest.GetInt("topicid", -1);
            postid = DNTRequest.GetInt("postid", -1);
            // 如果主题ID非数字
            if (postid == -1)
            {
                AddErrLine("无效的帖子ID");
                return;
            }

            // 获取该帖子的信息
            post = Posts.GetPostInfo(topicid, postid);
            // 如果该帖子不存在
            if (post == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);

            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            if (topicid != post.Tid)
            {
                AddErrLine("主题ID无效");
                return;
            }

            topictitle = topic.Title;
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name;
            pagetitle = "删除" + post.Title;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            int opinion = DNTRequest.GetInt("opinion", -1);
            if (!CheckPermission(post,opinion))
                return;

            if (!allowdelpost)
            {
                AddErrLine("当前不允许删帖");
                return;
            }

            int Losslessdel = Utils.StrDateDiffHours(post.Postdatetime, config.Losslessdel * 24);

            // 通过验证的用户可以删除帖子，如果是主题贴则另处理
            if (post.Layer == 0)
            {
                TopicAdmins.DeleteTopics(topicid.ToString(), byte.Parse(forum.Recyclebin.ToString()), false);
                //重新统计论坛帖数
                Forums.SetRealCurrentTopics(forum.Fid);

                ForumTags.DeleteTopicTags(topicid);
            }
            else
            {
              int reval;
                if (topic.Special == 4)
                {
                    string opiniontext = "";

                    if (opinion != 1 && opinion != 2)
                    {
                        AddErrLine("参数错误");
                        return;
                    }
                    reval = Posts.DeletePost(Posts.GetPostTableID(topicid), postid, false,true);
                    switch (opinion)
                    {
                        case 1:
                            opiniontext = "positivediggs";
                            break;
                        case 2:
                            opiniontext = "negativediggs";
                            break;

                    }
                    Discuz.Data.DatabaseProvider.GetInstance().DeleteDebatePost(topicid, opiniontext, postid);

                }
                else
                { 
                  reval = Posts.DeletePost(Posts.GetPostTableID(topicid), postid, false,true);
                
                }

                // 删除主题游客缓存
                ForumUtils.DeleteTopicCacheFile(topicid);

                //再次确保回复数精确
                Topics.UpdateTopicReplies(topic.Tid);

                //更新指定版块的最新发帖数信息
                Forums.UpdateLastPost(forum);

                if (reval > 0 && Losslessdel < 0)
                {
                    UserCredits.UpdateUserCreditsByPosts(post.Posterid, -1);
                }
            }


            SetUrl(Urls.ShowTopicAspxRewrite(post.Tid, 1));
            if (post.Layer == 0)
            {
                SetUrl(base.ShowForumAspxRewrite(post.Fid, 0));
            }
            SetMetaRefresh();
            SetShowBackLink(false);
            AddMsgLine("删除帖子成功, 返回主题");
        }

        private bool CheckPermission(PostInfo post, int opinion)
        {
            ismoder = Moderators.IsModer(useradminid, userid, forumid);
            if (userid == post.Posterid && !ismoder)
            {
                if (post.Layer < 1 && topic.Replies > 0)
                {
                    AddErrLine("已经被回复过的主帖不能被删除");
                    return false;
                }
                if (Utils.StrDateDiffMinutes(post.Postdatetime, config.Edittimelimit) > 0 || post.Posterid != userid)//不是作者或者超过编辑时限
                {
                    AddErrLine("已经超过了编辑帖子时限,不能删除帖子");
                    return false;
                }
                else
                {
                    allowdelpost = true;
                }

            }
            else
            {
                AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
                if (admininfo != null)
                {
                    // 如果所属管理组有删帖的管理权限
                    if (admininfo.Allowdelpost == 1)
                    {
                        // 如果是管理员或总版主
                        if (Moderators.IsModer(useradminid, userid, forumid))
                        {
                            forumpath = Request.ApplicationPath + forumpath;

                            forumpath = forumpath.Replace("//","/");

                            allowdelpost = true;
                            if (post.Layer == 0)//管理者跳转至删除主题
                            {
                                HttpContext.Current.Response.Redirect(string.Format("{0}topicadmin.aspx?action=moderate&operat=del&forumid={1}&topicid={2}", forumpath, post.Fid, post.Tid));
                                return false;
                            }
                            else//跳转至批量删帖
                            {
                                HttpContext.Current.Response.Redirect(string.Format("{0}topicadmin.aspx?action=moderate&operat=delposts&forumid={1}&topicid={2}&postid={3}&opinion={4}", forumpath, post.Fid, post.Tid, post.Pid, opinion));
                                return false;
                            }
                        }
                    }
                }
                else
                { allowdelpost = false; }

            
            }

            return true;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:20.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:20. 
            */

            base.OnLoad(e);


            if (page_err == 0)
            {

                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("	<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + "\r\n");
                aspxrewriteurl = this.ShowTopicAspxRewrite(topicid, 0);

                templateBuilder.Append("	 &raquo; <a href=\"" + aspxrewriteurl.ToString() + "\">" + topictitle.ToString() + "</a> &raquo; <strong>删除</strong>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");

                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>彩友提示信息</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");

                if (msgbox_url != "")
                {

                    templateBuilder.Append("	<p><a href=\"" + msgbox_url.ToString() + "\">如果浏览器没有转向, 请点击这里.</a></p>\r\n");

                }	//end if

                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("</div>\r\n");



            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>出现了'" + page_err.ToString() + "'个错误</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 &nbsp; &nbsp|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if

            templateBuilder.Append("</div>\r\n");


            if (footerad != "")
            {

                templateBuilder.Append("<!--底部广告显示-->\r\n");
                templateBuilder.Append("<div id=\"ad_footerbanner\">" + footerad.ToString() + "</div>\r\n");
                templateBuilder.Append("<!--底部广告结束-->\r\n");

            }	//end if

            templateBuilder.Append(Discuz.Web.UI.PageElement.Bottom);
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"stats_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx\">基本状况</a></li>\r\n");

            if (config.Statstatus == 1)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=views\">流量统计</a></li>\r\n");
                templateBuilder.Append("	<li><a href=\"stats.aspx?type=client\">客户软件</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<li><a href=\"stats.aspx?type=posts\">发帖量记录</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=forumsrank\">版块排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=topicsrank\">主题排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=postsrank\">发帖排行</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"stats.aspx?type=creditsrank\">金币排行</a></li>\r\n");

            if (config.Oltimespan > 0)
            {

                templateBuilder.Append("	<li><a href=\"stats.aspx?type=onlinetime\">在线时间</a></li>\r\n");

            }	//end if

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup headermenu_popup\" id=\"my_menu\" style=\"display: none\">\r\n");
            templateBuilder.Append("	<li><a href=\"mytopics.aspx\">我的主题</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myposts.aspx\">我的帖子</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">我的精华</a></li>\r\n");
            templateBuilder.Append("	<li><a href=\"myattachment.aspx\">我的附件</a></li>\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("	<li><a href=\"usercpsubscribe.aspx\">我的收藏</a></li>\r\n");

            }	//end if

            templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/mymenu.js\"></" + "script>\r\n");
            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<ul class=\"popupmenu_popup\" id=\"viewpro_menu\" style=\"display: none\">\r\n");

            if (useravatar != "")
            {

                templateBuilder.Append("		<img src=\"" + useravatar.ToString() + "\"/>\r\n");

            }	//end if

            aspxrewriteurl = this.UserInfoAspxRewrite(userid);

            templateBuilder.Append("	<li class=\"popuser\"><a href=\"" + aspxrewriteurl.ToString() + "\">我的资料</a></li>\r\n");

            templateBuilder.Append("</ul>\r\n");
            templateBuilder.Append("<div id=\"quicksearch_menu\" class=\"searchmenu\" style=\"display: none;\">\r\n");
            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='0';document.getElementById('quicksearch').innerHTML='帖子标题';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</div>\r\n");

            templateBuilder.Append("	<div onclick=\"document.getElementById('keywordtype').value='8';document.getElementById('quicksearch').innerHTML='作&nbsp;&nbsp;者';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作&nbsp;&nbsp;者</div>\r\n");
            templateBuilder.Append("</div>\r\n");



            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}
