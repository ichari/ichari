using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 模板列表选择
    /// </summary>
    public partial class showtemplate : PageBase
    {
        /// <summary>
        /// 可用模板列表
        /// </summary>
        public DataTable templatelist;
        protected override void ShowPage()
        {
            pagetitle = "选择模板";

            if (userid == -1 && config.Guestcachepagetimeout > 0)
            {
                AddErrLine("当前的系统设置不允许游客选择模板");
                return;
            }

            int templateid = DNTRequest.GetInt("templateid", 0);
            if (templateid > 0)
            {
                if (!System.IO.Directory.Exists(Utils.GetMapPath("../" + templateid)))
                {
                    AddErrLine("您所选择的模板不存在！");
                    return;
                }
                string strtemplateid = Templates.GetValidTemplateIDList();
                if (!Utils.InArray(templateid.ToString(), strtemplateid))
                {
                    templateid = config.Templateid;
                }
                Utils.WriteCookie(Utils.GetTemplateCookieName(), templateid.ToString(), 999999);

                string rurl = ForumUtils.GetReUrl();
                SetUrl(rurl.IndexOf("logout.aspx") > -1 || rurl.IndexOf("showtemplate.aspx") > -1 ? "index.aspx" : rurl);
                AddMsgLine("切换模板成功, 返回切换模板前页面");
                SetMetaRefresh();
                SetShowBackLink(false);
            }
            else
            {
                templatelist = Templates.GetValidTemplateList();
                if ((DNTRequest.GetUrlReferrer() == "") || (DNTRequest.GetUrlReferrer().IndexOf("showtemplate") > -1))
                {
                    ForumUtils.WriteCookie("reurl", "index.aspx");
                }
                else
                {
                    ForumUtils.WriteCookie("reurl", DNTRequest.GetUrlReferrer());
                }
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:53.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:53. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <strong>切换模板</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (page_err == 0)
            {


                if (DNTRequest.GetString("templateid") != "")
                {

                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	if (top.frames[\"leftmenu\"])\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		top.frames[\"leftmenu\"].location.reload();\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");

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

                    templateBuilder.Append("<form id=\"form1\" name=\"form1\" method=\"post\" action=\"\">\r\n");
                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h1>界面</h1>\r\n");
                    templateBuilder.Append("	<ul id=\"forumtemplate\">\r\n");

                    int template__loop__id = 0;
                    foreach (DataRow template in templatelist.Rows)
                    {
                        template__loop__id++;

                        templateBuilder.Append("		<li><span><img src=\"templates/" + template["directory"].ToString().Trim() + "/about.png\" /></span><br />\r\n");
                        templateBuilder.Append("		  <br /><input name=\"templateid\" type=\"radio\" value=\"" + template["templateid"].ToString().Trim() + "\" \r\n");

                        if (templatepath == template["directory"].ToString().Trim())
                        {

                            templateBuilder.Append("checked \r\n");

                        }	//end if

                        templateBuilder.Append("/>\r\n");
                        templateBuilder.Append("		 " + template["name"].ToString().Trim() + "\r\n");
                        templateBuilder.Append("		</li>\r\n");

                    }	//end loop

                    templateBuilder.Append("	</ul>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div class=\"templatebutton\"><input type=\"submit\" name=\"Submit\" value=\"确定\"/></div>\r\n");
                    templateBuilder.Append("</form>\r\n");
                    templateBuilder.Append("</div>\r\n");

                }	//end if


            }
            else
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


                templateBuilder.Append("</div>\r\n");

            }	//end if



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