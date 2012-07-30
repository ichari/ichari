using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using System;

namespace Discuz.Web
{
    /// <summary>
    /// 显示短消息页面
    /// </summary>
    public partial class usercpshowpm : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 短消息发件人
        /// </summary>
        public string msgfrom = "";

        /// <summary>
        /// 短消息标题
        /// </summary>
        public string subject = "";

        /// <summary>
        /// 短消息内容
        /// </summary>
        public string message = "";

        /// <summary>
        /// 短消息回复标题
        /// </summary>
        public string resubject = "";

        /// <summary>
        /// 短消息回复内容
        /// </summary>
        public string remessage = "";

        /// <summary>
        /// 短消息发送时间
        /// </summary>
        public string postdatetime = "";

        /// <summary>
        /// 短消息Id
        /// </summary>
        public int pmid = 0;

        /// <summary>
        /// 是否能够回复短消息
        /// </summary>
        public bool canreplypm = true;

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();
        #endregion

        protected override void ShowPage()
        {
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }

            pagetitle = "查看短消息";
            user = Discuz.Forum.Users.GetUserInfo(userid);

            pmid = DNTRequest.GetQueryInt("pmid", -1);
            if (pmid <= 0)
            {
                AddErrLine("参数无效");
                return;
            }

            if (!UserCredits.CheckUserCreditsIsEnough(userid, 1, CreditsOperationType.SendMessage, -1))
            {
                canreplypm = false;
            }

            PrivateMessageInfo messageinfo = PrivateMessages.GetPrivateMessageInfo(pmid);

            if (messageinfo == null)
            {
                AddErrLine("无效的短消息ID");
                return;
            }

            if (messageinfo.Msgfrom == "系统" && messageinfo.Msgfromid == 0)
            {
                messageinfo.Message = Utils.HtmlDecode(messageinfo.Message);
            }

            if (messageinfo != null)
            {
                //判断当前用户是否有权阅读此消息
                if (messageinfo.Msgtoid == userid || messageinfo.Msgfromid == userid)
                {
                    string action = DNTRequest.GetQueryString("action");
                    if (action.CompareTo("delete") == 0)
                    {
                        ispost = true;
                        int retval = PrivateMessages.DeletePrivateMessage(userid, pmid);
                        if (retval < 1)
                        {
                            AddErrLine("消息未找到,可能已被删除");
                            return;
                        }
                        else
                        {
                            AddMsgLine("指定消息成功删除,现在将转入消息列表");
                            SetUrl("usercpinbox.aspx");
                            SetMetaRefresh();
                            return;
                        }
                    }

                    if (action.CompareTo("noread") == 0)
                    {
                        PrivateMessages.SetPrivateMessageState(pmid, 1); //将短消息的状态置 1 表示未读
                        ispost = true;
                        if (messageinfo.New != 1 && messageinfo.Folder == 0)
                        {
                            Discuz.Forum.Users.DecreaseNewPMCount(userid, -1); //将用户的未读短信息数据加 1
                            AddMsgLine("指定消息已被置成未读状态,现在将转入消息列表");
                            SetUrl("usercpinbox.aspx");
                            SetMetaRefresh();
                        }
                    }
                    else
                    {
                        PrivateMessages.SetPrivateMessageState(pmid, 0); //将短消息的状态置 0 表示已读

                        if (messageinfo.New == 1 && messageinfo.Folder == 0)
                        {
                            Discuz.Forum.Users.DecreaseNewPMCount(userid); //将用户的未读短信息数据减 1
                        }
                    }

                    msgfrom = messageinfo.Msgfrom;
                    subject = messageinfo.Subject;
                    message = Utils.StrFormat(messageinfo.Message);
                    postdatetime = messageinfo.Postdatetime;
                    resubject = "re:" + messageinfo.Subject;
                    remessage = Utils.HtmlEncode("> ") + messageinfo.Message.Replace("\n", "\n> ") + "\r\n\r\n";
                    return;
                }
            }
            AddErrLine("对不起, 短消息不存在或已被删除.");
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:55.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:55. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>查看短消息</strong>\r\n");
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

            if (page_err == 0)
            {

                templateBuilder.Append("<!--主体-->\r\n");
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

                    templateBuilder.Append("				    <ul>\r\n");
                    templateBuilder.Append("						<li class=\"notetitle\">" + subject.ToString() + "</li>\r\n");
                    templateBuilder.Append("						<li class=\"notetime\">发送人:" + msgfrom.ToString() + "  " + postdatetime.ToString() + "</li>\r\n");
                    templateBuilder.Append("						<li class=\"notecontent\">" + message.ToString() + "</li>\r\n");
                    templateBuilder.Append("						<li class=\"notecontent\"><img src=\"templates/" + templatepath.ToString() + "/images/leftdot.gif\" alt=\"查看信息\"/>\r\n");
                    templateBuilder.Append("							<a href=\"usercpinbox.aspx\">返回列表</a>\r\n");
                    templateBuilder.Append("							<a href=\"usercppostpm.aspx?action=re&amp;pmid=" + pmid.ToString() + "\">回复</a>\r\n");
                    templateBuilder.Append("							<a href=\"usercppostpm.aspx?action=fw&amp;pmid=" + pmid.ToString() + "\">转发</a>\r\n");
                    templateBuilder.Append("							<a href=\"usercpshowpm.aspx?action=noread&amp;pmid=" + pmid.ToString() + "\">标记为未读</a>\r\n");
                    templateBuilder.Append("							<a href=\"usercpshowpm.aspx?action=delete&amp;pmid=" + pmid.ToString() + "\">删除</a>\r\n");
                    templateBuilder.Append("						</li>\r\n");
                    templateBuilder.Append("					</ul>\r\n");

                    if (canreplypm)
                    {

                        templateBuilder.Append("						<form id=\"postpm\" name=\"postpm\" method=\"post\" action=\"usercppostpm.aspx?action=re&amp;pmid=" + pmid.ToString() + "\">\r\n");
                        templateBuilder.Append("						<label for=\"user\" class=\"labelshort\">接件人:</label>\r\n");
                        templateBuilder.Append("						<input name=\"msgto\" type=\"text\" id=\"msgto\" value=\"" + msgfrom.ToString() + "\" size=\"20\" /><br />\r\n");
                        templateBuilder.Append("						<label for=\"email\" class=\"labelshort\">标题:</label>\r\n");
                        templateBuilder.Append("						<input name=\"subject\" type=\"text\" id=\"subject\" value=\"" + resubject.ToString() + "\" size=\"40\" /><br />\r\n");
                        templateBuilder.Append("						<label for=\"comment\" class=\"labelshort\">内容:</label>\r\n");
                        templateBuilder.Append("						<textarea name=\"message\" cols=\"80\" rows=\"20\" id=\"message\" onkeydown=\"if((event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83)) document.getElementById('postpm').submit();\" style=\"width:80%;\">" + remessage.ToString() + "</textarea><br/>\r\n");
                        templateBuilder.Append("						<label for=\"savetosentbox\"  class=\"labelshort\">&nbsp;</label>\r\n");
                        templateBuilder.Append("						<input name=\"savetosentbox\" type=\"checkbox\" id=\"Checkbox1\" value=\"1\" style=\"border:0;\" />发送的同时保存到发件箱\r\n");
                        templateBuilder.Append("						<input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" />邮件通知<br />\r\n");

                        if (isseccode)
                        {

                            templateBuilder.Append("						<label for=\"savetosentbox\"  class=\"labelshort\">验证码</label>\r\n");

                            templateBuilder.Append("<!-- onkeydown=\"return (event.keyCode ? event.keyCode : event.which ? event.which : event.charCode) != 13\"-->\r\n");
                            templateBuilder.Append("<input size=\"10\"  style=\"width:50px;\" class=\"colorblue\" onfocus=\"this.className='colorfocus';\" onblur=\"this.className='colorblue';\" onkeyup=\"changevcode(this.form, this.value);\" />\r\n");
                            templateBuilder.Append("&nbsp;\r\n");
                            templateBuilder.Append("<img src=\"tools/VerifyImagePage.aspx?time=" + Processtime.ToString() + "\" class=\"cursor\" id=\"vcodeimg\" onclick=\"this.src='tools/VerifyImagePage.aspx?id=" + olid.ToString() + "&time=' + Math.random();\" />\r\n");
                            templateBuilder.Append("<input name=\"reloadvcade\" type=\"button\" class=\"colorblue\" id=\"reloadvcade\" value=\"刷新验证码\"  onclick=\"document.getElementById('vcodeimg').src='tools/VerifyImagePage.aspx?time=' + Math.random();\" tabindex=\"-1\"  style=\"color:#99cc00; width:75px;\" />\r\n");
                            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                            templateBuilder.Append("	$('vcodeimg').src='tools/VerifyImagePage.aspx?bgcolor=F5FAFE&time=' + Math.random();\r\n");
                            templateBuilder.Append("	//document.getElementById('vcode').value = \"\";\r\n");
                            templateBuilder.Append("	function changevcode(form, value)\r\n");
                            templateBuilder.Append("	{\r\n");
                            templateBuilder.Append("		if (!$('vcode'))\r\n");
                            templateBuilder.Append("		{\r\n");
                            templateBuilder.Append("			var vcode = document.createElement('input');\r\n");
                            templateBuilder.Append("			vcode.id = 'vcode';\r\n");
                            templateBuilder.Append("			vcode.name = 'vcode';\r\n");
                            templateBuilder.Append("			vcode.type = 'hidden';\r\n");
                            templateBuilder.Append("			vcode.value = value;\r\n");
                            templateBuilder.Append("			form.appendChild(vcode);\r\n");
                            templateBuilder.Append("		}\r\n");
                            templateBuilder.Append("		else\r\n");
                            templateBuilder.Append("		{\r\n");
                            templateBuilder.Append("			$('vcode').value = value;\r\n");
                            templateBuilder.Append("		}\r\n");
                            templateBuilder.Append("	}\r\n");
                            templateBuilder.Append("</" + "script>\r\n");


                            templateBuilder.Append("<br />\r\n");

                        }	//end if

                        templateBuilder.Append("						<label for=\"savetosentbox\"  class=\"labelshort\">&nbsp;</label><input name=\"sendmsg\" type=\"submit\" id=\"sendmsg\" value=\"立即发送\"/>\r\n");
                        templateBuilder.Append("						<input name=\"savetousercpdraftbox\" type=\"submit\" id=\"savetousercpdraftbox\" value=\"存为草稿\"/>\r\n");
                        templateBuilder.Append("						[完成后可按Ctrl+Enter提交]\r\n");
                        templateBuilder.Append("					</form>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("			  </div>\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");
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


            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    } //class end
}