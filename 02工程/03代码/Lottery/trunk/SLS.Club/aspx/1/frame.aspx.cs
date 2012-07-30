using System;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 分栏框架页
    /// </summary>
    public partial class frame : PageBase
    {
        protected override void ShowPage()
        {
            pagetitle = "分栏";
            int toframe = DNTRequest.GetInt("f", 1);
            if (toframe == 1)
            {
                ForumUtils.WriteCookie("isframe", 1);
            }
            else
            {
                toframe = Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1);
                if (toframe == -1)
                {
                    toframe = config.Isframeshow;
                }
            }

            if (toframe == 0)
            {
                HttpContext.Current.Response.Redirect("/");
                HttpContext.Current.Response.End();
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:01.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:01. 
            */

            base.OnLoad(e);

            templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
            templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
            templateBuilder.Append("<head>\r\n");
            templateBuilder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n");
            templateBuilder.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\" />\r\n");
            templateBuilder.Append("" + meta.ToString() + "\r\n");

            if (pagetitle == "首页")
            {

                templateBuilder.Append("<title>" + config.Forumtitle.ToString().Trim() + "</title>\r\n");

            }
            else
            {

                templateBuilder.Append("<title>" + pagetitle.ToString() + " - " + config.Forumtitle.ToString().Trim() + "</title>\r\n");

            }	//end if
            
            templateBuilder.Append("<link rel=\"stylesheet\" href=\"templates/" + templatepath.ToString() + "/dnt.css\" type=\"text/css\" media=\"all\"  />\r\n");
            templateBuilder.Append("" + link.ToString() + "\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_report.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_utils.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/common.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/menu.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	var aspxrewrite = " + config.Aspxrewrite.ToString().Trim() + ";\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("" + script.ToString() + "\r\n");
            templateBuilder.Append("</head>\r\n");


            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	if (top.main){\r\n");
            templateBuilder.Append("		top.main.location = \"focuslist.aspx\";\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div id=\"container\">\r\n");
            templateBuilder.Append("<frameset border=\"0\" name=\"content\" framespacing=\"0\" frameborder=\"0\" cols=\"210,*\">\r\n");
            templateBuilder.Append("	<frame id=\"leftmenu\" name=\"leftmenu\" marginwidth=\"0\" marginheight=\"0\" src=\"forumlist.aspx\" noresize>\r\n");
            templateBuilder.Append("	<frame id=\"main\" name=\"main\" src=\"focuslist.aspx\">\r\n");
            templateBuilder.Append("</frameset><noframes></noframes>\r\n");
            templateBuilder.Append("</div>\r\n");

            Response.Write(templateBuilder.ToString());
        }
    }
}