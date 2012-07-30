using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Config;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Common;

namespace Discuz.Web
{
    public partial class help : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帮助列表
        /// </summary>
        public ArrayList helplist;

        #region DLL文件的版本信息
      
        public string dllver_discuzaggregation = "";
        public string dllver_discuzcache = "";
        public string dllver_discuzcommon = "";
        public string dllver_discuzconfig = "";
        public string dllver_discuzcontrol = "";
        public string dllver_discuzdata = "";
        public string dllver_discuzdatasqlserver = "";
        public string dllver_discuzdataaccess = "";
        public string dllver_discuzdatamysql = "";
        public string dllver_discuzentity = "";
        public string dllver_discuzforum = "";
        public string dllver_discuzplugin = "";
        public string dllver_discuzpluginsysmail = "";
        public string dllver_discuzspace = "";
        public string dllver_discuzweb = "";
        public string dllver_discuzwebui = "";

        #endregion

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string dbtype = BaseConfigs.GetDbType;
        
        /// <summary>
        /// 产品名称
        /// </summary>
        public string assemblyproductname = Utils.GetAssemblyProductName();
        
        /// <summary>
        /// 版权
        /// </summary>
        public string Assemblycopyright = Utils.GetAssemblyCopyright();

        /// <summary>
        /// 显示版本信息
        /// </summary>
        public int showversion = DNTRequest.GetInt("version", 0);
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "帮助";

            int helpid = DNTRequest.GetInt("hid", 0);
            if (helpid > 0)
            {
                helplist = Helps.GetHelpList(helpid);
            }
            else
            {
                helplist = Helps.GetHelpList();
            }
            if (helplist == null)
            {
                AddErrLine("没有信息可读取！");
                return;
            }
        }

        /// <summary>
        /// 获取指定DLL文件的版本信息
        /// </summary>
        /// <param name="fullfilename"></param>
        /// <returns></returns>
        private string LoadDllVersion(string fullfilename)
        {
            try
            {
                FileVersionInfo AssemblyFileVersion = FileVersionInfo.GetVersionInfo(fullfilename);
                return string.Format("{0}.{1}.{2}.{3}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart, AssemblyFileVersion.FilePrivatePart);
            }
            catch
            {
                return "未能加载dll或该dll文件不存在!";
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:59.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:59. 
            */

            base.OnLoad(e);


            if (page_err == 0)
            {


                if (showversion == 1)
                {

                }
                else
                {

                    templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                    templateBuilder.Append("	<div id=\"nav\">\r\n");
                    templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; <script type=\"text/javascript\">if (getQueryString('hid')!='') document.write(' <strong><a href=\"help.aspx\">帮助</a></strong>');</" + "script>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div class=\"mainbox viewthread specialthread\">	\r\n");
                    templateBuilder.Append("	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"在线帮助\">	\r\n");
                    templateBuilder.Append("		<td class=\"postcontent helpcontent\">\r\n");
                    templateBuilder.Append("			<h3>\r\n");
                    templateBuilder.Append("                <a id=\"live800iconlink\" target=\"_self\" href=\"javascript:;\" onclick=\"try{parent.closeMini();}catch(e){;}this.newWindow = window.open('http://chat10.live800.com/live800/chatClient/chatbox.jsp?companyID=86584&configID=149924&jid=8794095338&enterurl=http%3A%2F%2Flocalhost%3A2003%2FSLS%2EIcaile%2FHome%2FWeb%2FDefault%2Easpx', 'chatbox86584', 'toolbar=0');this.newWindow.focus();this.newWindow.opener=window;return false;\">在线咨询</a>");
                    templateBuilder.Append("			</h3>\r\n");

                    int helpcontent__loop__id = 0;
                    foreach (HelpInfo helpcontent in helplist)
                    {
                        helpcontent__loop__id++;


                        if (helpcontent.Pid == 0)
                        {

                            templateBuilder.Append("						<h2>" + helpcontent.Title.ToString().Trim() + "</h2>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						<ul>\r\n");
                            templateBuilder.Append("							<li class=\"helpsubtitle\"><a name=\"h_" + helpcontent.Pid.ToString().Trim() + "_" + helpcontent.Id.ToString().Trim() + "\"></a>" + helpcontent.Title.ToString().Trim() + "</li>\r\n");
                            templateBuilder.Append("							<li>" + helpcontent.Message.ToString().Trim() + "</li>\r\n");
                            templateBuilder.Append("						</ul>\r\n");

                        }	//end if


                    }	//end loop

                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		<td class=\"postauthor helpmenu\">\r\n");

                    int help__loop__id = 0;
                    foreach (HelpInfo help in helplist)
                    {
                        help__loop__id++;


                        if (help.Pid == 0)
                        {

                            templateBuilder.Append("						<p><strong><a href=\"?hid=" + help.Id.ToString().Trim() + "\">" + help.Title.ToString().Trim() + "</a></strong></p>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						<p><a href=\"#h_" + help.Pid.ToString().Trim() + "_" + help.Id.ToString().Trim() + "\" style=\"padding-left: 8px;\">" + help.Title.ToString().Trim() + "</a></p>\r\n");

                        }	//end if


                    }	//end loop

                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("	</table>\r\n");
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