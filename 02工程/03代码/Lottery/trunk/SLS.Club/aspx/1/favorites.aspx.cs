using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using System;

namespace Discuz.Web
{
	/// <summary>
	/// 添加收藏页
	/// </summary>
	public partial class favorites : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 将要收藏的主题信息
        /// </summary>
		public TopicInfo topic;
        /// <summary>
        /// 主题所属版块
        /// </summary>
		public int forumid;
        /// <summary>
        /// 主题所属版块名称
        /// </summary>
		public string forumname;
        /// <summary>
        /// 主题Id
        /// </summary>
		public int topicid;
        /// <summary>
        /// 主题标题
        /// </summary>
		public string topictitle;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
		public string forumnav;
        /// <summary>
        /// 将要收藏的相册Id
        /// </summary>
        public int albumid;
        /// <summary>
        /// 将要收藏的日志Id
        /// </summary>
        public int blogid;
        /// <summary>
        /// 将要收藏的商品Id
        /// </summary>
        public int goodsid;
        /// <summary>
        /// 主题所属版块
        /// </summary>
        public ForumInfo forum;
       
        #endregion

        protected override void ShowPage()
		{
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
			
			string referer = ForumUtils.GetCookie("referer");

			// 获取主题ID
			topicid = DNTRequest.GetInt("topicid", -1);
            albumid = DNTRequest.GetInt("albumid", -1);
            blogid = DNTRequest.GetInt("postid", -1);
            goodsid = DNTRequest.GetInt("goodsid", -1);
            
            if (topicid != -1)//收藏的是主题
            {
                // 获取该主题的信息
                TopicInfo topic = Topics.GetTopicInfo(topicid);
                // 如果该主题不存在
                if (topic == null)
                {
                    AddErrLine("不存在的主题ID");
                    return;
                }

                topictitle = topic.Title;
                forumid = topic.Fid;
                forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                forumnav = forum.Pathlist;

                // 检查用户是否拥有足够权限                
                if (config.Maxfavorites <= Favorites.GetFavoritesCount(userid))
                {
                    AddErrLine("您收藏的主题数目已经达到系统设置的数目上限");
                    return;
                }

                if (Favorites.CheckFavoritesIsIN(userid, topicid) != 0)
                {
                    AddErrLine("您过去已经收藏过这个主题,请返回");
                    return;
                }

                if (Favorites.CreateFavorites(userid, topicid) > 0)
                {
                    AddMsgLine("指定主题已成功添加到收藏夹中,现在将回到上一页");
                    SetUrl(referer);
                    SetMetaRefresh();
                    SetShowBackLink(false);
                }
            }
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:10.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:10. 
            */

            base.OnLoad(e);


            if (page_err > 0)
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>出现了" + page_err.ToString() + "个错误</h1>\r\n");
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



            }
            else
            {

                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");

                if (topicid != -1)
                {

                    templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + " &raquo; \r\n");
                    aspxrewriteurl = this.ShowForumAspxRewrite(forumid, 0);

                    templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">" + topictitle.ToString() + "</a> &raquo; <strong>收藏主题</strong>\r\n");

                }	//end if

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
