using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 短消息基本设置页面
    /// </summary>
    public partial class usercppmset : PageBase
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();

        /// <summary>
        /// 短消息接收设置
        /// </summary>
        public int receivepmsetting;

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Discuz.Forum.Users.GetUserInfo(userid);
            receivepmsetting = (int) user.Newsletter;

            if (DNTRequest.IsPost())
            {
                user.Pmsound = DNTRequest.GetInt("pmsound", 0);


                receivepmsetting = 1;
                foreach (string rpms in DNTRequest.GetString("receivesetting").Split(','))
                {
                    if (rpms != string.Empty)
                    {
                        int tmp = int.Parse(rpms);
                        receivepmsetting = receivepmsetting | tmp;
                    }
                }
                user.Newsletter = (ReceivePMSettingType) receivepmsetting;

                Discuz.Forum.Users.UpdateUserPMSetting(user);

                ForumUtils.WriteCookie("pmsound", user.Pmsound.ToString());

                SetUrl("usercppmset.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("短消息设置已成功更新");
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:07.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:07. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>选项</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"headsearch\">\r\n");
            templateBuilder.Append("		<div id=\"search\">\r\n");

            templateBuilder.Append("			<form method=\"post\" action=\"search.aspx\" target=\"_blank\" onsubmit=\"bind_keyword(this);\">\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"poster\" />\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"keyword\" />\r\n");
            templateBuilder.Append("				<input type=\"hidden\" name=\"type\" value=\"\" />\r\n");
            templateBuilder.Append("				<input id=\"keywordtype\" type=\"hidden\" name=\"keywordtype\" value=\"0\"/>\r\n");
            templateBuilder.Append("				<div id=\"searchbar\">\r\n");
            templateBuilder.Append("					<dl>\r\n");
            templateBuilder.Append("						<dt id=\"quicksearch\" class=\"s2\" onclick=\"showMenu(this.id, false);\" onmouseover=\"MouseCursor(this);\">帖子标题</dt>\r\n");
            templateBuilder.Append("						<dd class=\"textinput\"><input type=\"text\" name=\"keywordf\" value=\"\" class=\"text\"/></dd>\r\n");
            templateBuilder.Append("						<dd><input name=\"searchsubmit\" type=\"submit\" value=\"\" class=\"s3\"/></dd>\r\n");
            templateBuilder.Append("					</dl>\r\n");
            templateBuilder.Append("				</div>\r\n");
            templateBuilder.Append("			</form>\r\n");
            templateBuilder.Append("			<script type=\"text/javascript\">function bind_keyword(form){if(form.keywordtype.value=='8'){form.keyword.value='';form.poster.value=form.keywordf.value; } else { form.poster.value=''; form.keyword.value=form.keywordf.value;if(form.keywordtype.value == '2')form.type.value = 'spacepost';if(form.keywordtype.value == '3')form.type.value = 'album';}}</" + "script>\r\n");


            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--主体-->\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	function checkSetting()\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		if ($('receiveuser').checked)\r\n");
            templateBuilder.Append("		{\r\n");
            templateBuilder.Append("			$('showhint').disabled = false;\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("		else\r\n");
            templateBuilder.Append("		{			\r\n");
            templateBuilder.Append("			$('showhint').checked = false;\r\n");
            templateBuilder.Append("			$('showhint').disabled = true;\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div class=\"controlpannel\">\r\n");

            templateBuilder.Append("<div class=\"pannelmenu\">\r\n");

            if (userid > 0)
            {


                if (pagename == "usercptopic.aspx" || pagename == "usercppost.aspx" || pagename == "usercpdigest.aspx" || pagename == "usercpprofile.aspx"


                  || pagename == "usercpnewpassword.aspx" || pagename == "usercppreference.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\" class=\"current\"><span>个人设置</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\">个人设置</a>\r\n");

                }	//end if


                if (pagename == "usercpinbox.aspx" || pagename == "usercpsentbox.aspx" || pagename == "usercpdraftbox.aspx" || pagename == "usercppostpm.aspx" || pagename == "usercpshowpm.aspx" || pagename == "usercppmset.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\" class=\"current\"><span>短消息</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\">短消息</a>\r\n");

                }	//end if


                if (pagename == "usercpsubscribe.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\" class=\"current\"><span>收藏夹</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\">收藏夹</a>\r\n");

                }	//end if


                if (pagename == "usercpcreditspay.aspx" || pagename == "usercpcreditstransfer.aspx" || pagename == "usercpcreditspayoutlog.aspx" || pagename == "usercpcreditspayinlog.aspx"


                   || pagename == "usercpcreaditstransferlog.aspx")
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\" class=\"current\"><span>金币交易</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\">金币交易</a>\r\n");

                }	//end if


                if (pagename == "usercpforumsetting.aspx")
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\" class=\"current\"><span>论坛设置</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\">论坛设置</a>\r\n");

                }	//end if

            }	//end if

            templateBuilder.Append("	</div>\r\n");


            templateBuilder.Append("	<div class=\"pannelcontent\">\r\n");
            templateBuilder.Append("		<div class=\"pcontent\">\r\n");
            templateBuilder.Append("			<div class=\"panneldetail\">\r\n");

            templateBuilder.Append("				<div class=\"panneltabs\">\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("					<a href=\"usercpinbox.aspx\"\r\n");

                if (pagename == "usercpinbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">收件箱</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpsentbox.aspx\"\r\n");

                if (pagename == "usercpsentbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">发件箱</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpdraftbox.aspx\"\r\n");

                if (pagename == "usercpdraftbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">草稿箱</a>\r\n");
                templateBuilder.Append("					 <a href=\"usercppmset.aspx\"\r\n");

                if (pagename == "usercppmset.aspx")
                {

                    templateBuilder.Append("						class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">选项</a>\r\n");
                templateBuilder.Append("					<a href=\"usercppostpm.aspx\"\r\n");

                if (pagename == "usercppostpm.aspx")
                {

                    templateBuilder.Append("					 class=\"current addbutton\"\r\n");

                }
                else
                {

                    templateBuilder.Append("					 class=\"addbutton\"\r\n");

                }	//end if

                templateBuilder.Append(">写新消息</a>\r\n");

            }	//end if

            templateBuilder.Append("				</div>	\r\n");


            templateBuilder.Append("				<div class=\"pannelbody\">\r\n");
            templateBuilder.Append("					<div class=\"pannellist\">\r\n");

            if (page_err == 0)
            {


                if (ispost)
                {


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

                    templateBuilder.Append("						<form action=\"\" method=\"post\" ID=\"postpmset\">\r\n");
                    templateBuilder.Append("						<label for=\"pmsound\" class=\"labellong2\" style=\"line-height:180%;\">短消息提示音:</label>\r\n");
                    templateBuilder.Append("							<select name=\"pmsound\" id=\"pmsound\">\r\n");
                    templateBuilder.Append("							  <option value=\"0\" \r\n");

                    if (user.Pmsound == 0)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >无</option>							  \r\n");
                    templateBuilder.Append("							  <option value=\"1\" \r\n");

                    if (user.Pmsound == 1)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >提示音1</option>							  \r\n");
                    templateBuilder.Append("							  <option value=\"2\" \r\n");

                    if (user.Pmsound == 2)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >提示音2</option>							  \r\n");
                    templateBuilder.Append("							  <option value=\"3\" \r\n");

                    if (user.Pmsound == 3)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >提示音3</option>			\r\n");
                    templateBuilder.Append("							  <option value=\"4\" \r\n");

                    if (user.Pmsound == 4)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >提示音4</option>			\r\n");
                    templateBuilder.Append("							  <option value=\"5\" \r\n");

                    if (user.Pmsound == 5)
                    {

                        templateBuilder.Append("							  selected=\"selected\"\r\n");

                    }	//end if

                    templateBuilder.Append("							  >提示音5</option>\r\n");
                    templateBuilder.Append("							  </select>\r\n");
                    templateBuilder.Append("							<br/>\r\n");
                    templateBuilder.Append("							<div class=\"compartline\" style=\"margin-top:8px;\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<label for=\"receivesetting\" class=\"labellong2\" style=\"line-height:180%;\">接收设置:</label>\r\n");
                    templateBuilder.Append("							<input id=\"receiveuser\" onclick=\"checkSetting();\" type=\"checkbox\" name=\"receivesetting\" value=\"2\" \r\n");

                    if (receivepmsetting == 2 || receivepmsetting == 3)
                    {

                        templateBuilder.Append("checked=\"checked\"\r\n");

                    }	//end if


                    if (receivepmsetting == 6 || receivepmsetting == 7)
                    {

                        templateBuilder.Append("checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>接收用户短消息\r\n");
                    templateBuilder.Append("							<input id=\"showhint\" onclick=\"checkSetting();\" type=\"checkbox\" name=\"receivesetting\" value=\"4\" \r\n");

                    if (receivepmsetting == 0)
                    {

                        templateBuilder.Append("disabled\r\n");

                    }	//end if


                    if (receivepmsetting == 1 || receivepmsetting == 5)
                    {

                        templateBuilder.Append("disabled\r\n");

                    }	//end if


                    if (receivepmsetting > 4)
                    {

                        templateBuilder.Append("checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>显示短消息提示框\r\n");
                    templateBuilder.Append("							<br/>\r\n");
                    templateBuilder.Append("							<div class=\"compartline\" style=\"margin-top:8px;\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<input type=\"submit\" name=\"Submit\" value=\"确定\" ID=\"Submit1\"/>\r\n");
                    templateBuilder.Append("						</form>\r\n");

                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>错误显示</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		&nbsp; &nbsp; <a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 |&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if

            templateBuilder.Append("				  </div>\r\n");
            templateBuilder.Append("				</div>\r\n");
            templateBuilder.Append("			</div>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--主体-->\r\n");
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