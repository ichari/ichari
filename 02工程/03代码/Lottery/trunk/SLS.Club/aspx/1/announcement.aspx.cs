using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;

namespace Discuz.Web
{
	/// <summary>
	/// 公告列表
	/// </summary>
	public partial class announcement : PageBase
    {
        #region 变量声明
        /// <summary>
        /// 公告列表
        /// </summary>
		public DataTable announcementlist;
        #endregion 变量声明

        protected override void ShowPage()
		{
			pagetitle = "公告";
			
			announcementlist = Announcements.GetAnnouncementList(nowdatetime,"2099-12-31 23:59:59");
			if (announcementlist.Rows.Count == 0)
			{
				AddErrLine("当前没有任何公告");
				return;
			}
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:13.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:13. 
            */

            base.OnLoad(e);

            if (page_err == 0)
            {

                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");

                int announcement__loop__id = 0;
                foreach (DataRow announcement in announcementlist.Rows)
                {
                    announcement__loop__id++;

                    templateBuilder.Append("<a name=\"" + announcement["id"].ToString().Trim() + "\"></a>\r\n");
                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h3>" + announcement["title"].ToString().Trim() + "</h3>\r\n");
                    templateBuilder.Append("	<div class=\"postmessage\">\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(announcement["posterid"].ToString().Trim());

                    templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">\r\n");
                    templateBuilder.Append("		" + announcement["poster"].ToString().Trim() + "</a> &nbsp; 发表于: " + announcement["starttime"].ToString().Trim() + " \r\n");
                    templateBuilder.Append("		结束于: " + announcement["endtime"].ToString().Trim() + "<br />\r\n");
                    templateBuilder.Append("		" + announcement["message"].ToString().Trim() + "\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");

                }	//end loop

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



            }
            else
            {
                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("<h1>出现了N个错误</h1>");
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


                templateBuilder.Append("</div>\r\n");

            }	//end if


            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
	}
}
