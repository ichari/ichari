using System;
using System.Data;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 退出页面
    /// </summary>
    public partial class logout : PageBase
    {
        protected override void ShowPage()
        {
            pagetitle = "用户退出";
            username = "游客";
            int uid = userid;
            userid = -2;
            StringBuilder script = new StringBuilder();
            script.Append("if (top.document.getElementById('leftmenu')){");
            script.Append("		top.frames['leftmenu'].location.reload();");
            script.Append("}");

            base.AddScript(script.ToString());

            string referer = DNTRequest.GetQueryString("reurl");
            if (!DNTRequest.IsPost() || referer != "")
            {
                string r = "";
                if (referer != "")
                {
                    r = referer;
                }
                else
                {
                    if ((DNTRequest.GetUrlReferrer() == "") || (DNTRequest.GetUrlReferrer().IndexOf("login") > -1) ||
                        DNTRequest.GetUrlReferrer().IndexOf("logout") > -1)
                    {
                        r = "index.aspx";
                    }
                    else
                    {
                        r = DNTRequest.GetUrlReferrer();
                    }
                }
                Utils.WriteCookie("reurl", (referer == "" || referer.IndexOf("login.aspx") > -1) ? r : referer);
            }


            SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            
            SetMetaRefresh();
            SetShowBackLink(false);
            if (DNTRequest.GetString("userkey") == userkey || IsApplicationLogout())
            {
                AddMsgLine("已经清除了您的登录信息, 稍后您将以游客身份返回首页");
                //Users.UpdateOnlineTime(uid);
                OnlineUsers.DeleteRows(olid);
                ForumUtils.ClearUserCookie();
                Utils.WriteCookie(Utils.GetTemplateCookieName(), "", -999999);

                System.Web.HttpCookie cookie = new System.Web.HttpCookie("dntadmin");
                System.Web.HttpContext.Current.Response.AppendCookie(cookie);

                //System.Web.Security.FormsAuthentication.SignOut();

            }
            else
            {
                AddMsgLine("无法确定您的身份, 稍后返回首页");
            }

            Discuz.Forum.Users.RemoveUserIDFromCookie();
        }

        /// <summary>
        /// 是否是来自应用程序的登出
        /// </summary>
        /// <returns></returns>
        private bool IsApplicationLogout()
        {
            APIConfigInfo apiconfig = APIConfigs.GetConfig();
            if(!apiconfig.Enable)
                return false;

            int confirm = DNTRequest.GetFormInt("confirm", -1);
            if (confirm != 1)
                return false;        

            return true;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:29.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:29. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; 用户退出</div>\r\n");
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