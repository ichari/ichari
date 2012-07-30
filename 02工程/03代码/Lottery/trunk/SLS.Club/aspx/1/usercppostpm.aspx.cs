using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using Discuz.Data;

namespace Discuz.Web
{
    /// <summary>
    /// 撰写短消息页面
    /// </summary>
    public partial class usercppostpm : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 短消息收件人
        /// </summary>
        public string msgto;

        /// <summary>
        /// 短消息标题
        /// </summary>
        public string subject;

        /// <summary>
        /// 短消息内容
        /// </summary>
        public string message;

        /// <summary>
        /// 短消息收件人Id
        /// </summary>
        public int msgtoid = 0;

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();

        #endregion

        protected override void ShowPage()
        {
            pagetitle = "撰写短消息";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");

                return;
            }
            user = Discuz.Forum.Users.GetUserInfo(userid);

            if (!CheckPermission())
            {
                return;
            }

            if (DNTRequest.IsPost())
            {
                if (!CheckPermissionAfterPost())
                {
                    return;
                }

                #region 创建并发送短消息

                PrivateMessageInfo pm = new PrivateMessageInfo();

                string curdatetime = Utils.GetDateTime();
                // 收件箱
                if (useradminid == 1)
                {
                    pm.Message = Utils.HtmlEncode(DNTRequest.GetString("message"));
                    pm.Subject = Utils.HtmlEncode(DNTRequest.GetString("subject"));
                }
                else
                {
                    pm.Message =
                   Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("message")));
                    pm.Subject =
                        Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("subject")));
                }

                if (ForumUtils.HasBannedWord(pm.Message) || ForumUtils.HasBannedWord(pm.Subject))
                {
                    //HasBannedWord 指定的字符串中是否含有禁止词汇

                    AddErrLine("对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!");
                    return;
                }


                if (ForumUtils.HasAuditWord(pm.Message) || ForumUtils.HasAuditWord(pm.Subject))
                {


                    AddErrLine("对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!");
                    return;
                }

                pm.Message = ForumUtils.BanWordFilter(pm.Message);
                pm.Subject = ForumUtils.BanWordFilter(pm.Subject);

                pm.Msgto = DNTRequest.GetString("msgto");
                pm.Msgtoid = msgtoid;
                pm.Msgfrom = username;
                pm.Msgfromid = userid;
                pm.New = 1;
                pm.Postdatetime = curdatetime;


                if (!DNTRequest.GetString("savetousercpdraftbox").Equals(""))
                {
                    // 检查发送人的短消息是否已超过发送人用户组的上限
                    if (PrivateMessages.GetPrivateMessageCount(userid, -1) >= usergroupinfo.Maxpmnum)
                    {
                        AddErrLine("抱歉,您的短消息已达到上限,无法保存到草稿箱");
                        return;
                    }
                    // 只将消息保存到草稿箱
                    pm.Folder = 2;
                    if (UserCredits.UpdateUserCreditsBySendpms(base.userid) == -1)
                    {
                        AddErrLine("您的金币不足, 不能发送短消息");
                        return;
                    }
                    pm.Pmid = PrivateMessages.CreatePrivateMessage(pm, 0);

                    //发送邮件通知
                    if (DNTRequest.GetString("emailnotify") == "on")
                    {
                        SendNotifyEmail(Discuz.Forum.Users.GetUserInfo(msgtoid).Email.Trim(), pm);
                    }

                    SetUrl("usercpdraftbox.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("已将消息保存到草稿箱");
                }
                else if (!DNTRequest.GetString("savetosentbox").Equals(""))
                {
                    // 检查接收人的短消息是否已超过接收人用户组的上限
                    UserInfo touser = Discuz.Forum.Users.GetUserInfo(msgtoid);
                    //管理组不受接收人短消息上限限制
                    int radminId = UserGroups.GetUserGroupInfo(usergroupid).Radminid;
                    if (!(radminId > 0 && radminId <= 3) && PrivateMessages.GetPrivateMessageCount(msgtoid, -1) >=
                        UserGroups.GetUserGroupInfo(touser.Groupid).Maxpmnum)
                    {
                        AddErrLine("抱歉,接收人的短消息已达到上限,无法接收");
                        return;
                    }

                    if (!Utils.InArray(Convert.ToInt32(touser.Newsletter).ToString(), "2,3,6,7"))
                    {
                        AddErrLine("抱歉,接收人拒绝接收短消息");
                        return;
                    }
                    // 检查发送人的短消息是否已超过发送人用户组的上限
                    if (PrivateMessages.GetPrivateMessageCount(userid, -1) >= usergroupinfo.Maxpmnum)
                    {
                        AddErrLine("抱歉,您的短消息已达到上限,无法保存到发件箱");
                        return;
                    }
                    // 发送消息且保存到发件箱
                    pm.Folder = 0;
                    if (UserCredits.UpdateUserCreditsBySendpms(base.userid) == -1)
                    {
                        AddErrLine("您的金币不足, 不能发送短消息");
                        return;
                    }
                    pm.Pmid = PrivateMessages.CreatePrivateMessage(pm, 1);

                    //发送邮件通知
                    if (DNTRequest.GetString("emailnotify") == "on")
                    {
                        SendNotifyEmail(touser.Email.Trim(), pm);
                    }

                    // 更新在线表中的用户最后发帖时间
                    OnlineUsers.UpdatePostPMTime(olid);

                    SetUrl("usercpsentbox.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("发送完毕, 且已将消息保存到发件箱");
                }
                else
                {
                    UserInfo touser = Discuz.Forum.Users.GetUserInfo(msgtoid);
                    // 检查接收人的短消息是否已超过接收人用户组的上限,管理组不受接收人短消息上限限制
                    int radminId = UserGroups.GetUserGroupInfo(usergroupid).Radminid;
                    if (!(radminId > 0 && radminId <=3) && PrivateMessages.GetPrivateMessageCount(msgtoid, -1) >=
                        UserGroups.GetUserGroupInfo(touser.Groupid).Maxpmnum)
                    {
                        AddErrLine("抱歉,接收人的短消息已达到上限,无法接收");
                        return;
                    }
                    if (!Utils.InArray(Convert.ToInt32(touser.Newsletter).ToString(), "2,3,6,7"))
                    {
                        AddErrLine("抱歉,接收人拒绝接收短消息");
                        return;
                    }

                    // 发送消息但不保存到发件箱
                    pm.Folder = 0;
                    if (UserCredits.UpdateUserCreditsBySendpms(base.userid) == -1)
                    {
                        AddErrLine("您的金币不足, 不能发送短消息");
                        return;
                    }
                    pm.Pmid = PrivateMessages.CreatePrivateMessage(pm, 0);

                    //发送邮件通知
                    if (DNTRequest.GetString("emailnotify") == "on")
                    {
                        SendNotifyEmail(touser.Email.Trim(), pm);
                    }

                    SetUrl("usercpinbox.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("发送完毕");
                }

                #endregion
            }

            msgto = Utils.HtmlEncode(DNTRequest.GetString("msgto"));

            msgtoid = DNTRequest.GetInt("msgtoid", 0);
            if (msgtoid > 0)
            {
                msgto = Discuz.Forum.Users.GetUserName(msgtoid).Trim();
            }

            subject = Utils.HtmlEncode(DNTRequest.GetString("subject"));
            message = Utils.HtmlEncode(DNTRequest.GetString("message"));

            string action = DNTRequest.GetQueryString("action").ToLower();
            if (action.CompareTo("re") == 0 || action.CompareTo("fw") == 0) //回复或者转发
            {
                int pmid = DNTRequest.GetQueryInt("pmid", -1);
                if (pmid != -1)
                {
                    PrivateMessageInfo pm = PrivateMessages.GetPrivateMessageInfo(pmid);
                    if (pm != null)
                    {
                        if (pm.Msgtoid == userid || pm.Msgfromid == userid)
                        {
                            if (action.CompareTo("re") == 0)
                            {
                                msgto = Utils.HtmlEncode(pm.Msgfrom);
                            }
                            else
                            {
                                msgto = "";
                            }
                            subject = Utils.HtmlEncode(action) + ":" + pm.Subject;
                            message = Utils.HtmlEncode("> ") + pm.Message.Replace("\n", "\n> ") + "\r\n\r\n";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 提交后的权限检查
        /// </summary>
        /// <returns></returns>
        private bool CheckPermissionAfterPost()
        {
            if (ForumUtils.IsCrossSitePost())
            {
                AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                return false;
            }

            if (DNTRequest.GetString("message").Equals(""))
            {
                AddErrLine("内容不能为空");

                return false;
            }

            if (DNTRequest.GetString("message").Length > 3000)
            {
                AddErrLine("内容不能超过3000字");

                return false;
            }

            if (DNTRequest.GetString("msgto").Equals(""))
            {
                AddErrLine("接收人不能为空");

                return false;
            }

            if (DNTRequest.GetString("subject").Trim().Equals(""))
            {
                AddErrLine("标题不能为空");

                return false;
            }

            if (DNTRequest.GetString("subject").Trim().Length > 60)
            {
                AddErrLine("标题不能超过60字");

                return false;
            }

            // 不能给负责发送新用户注册欢迎信件的用户名称发送消息
            if (DNTRequest.GetString("msgto") == PrivateMessages.SystemUserName)
            {
                AddErrLine("不能给系统发送消息");
                return false;
            }

            msgtoid = Discuz.Forum.Users.GetUserID(DNTRequest.GetString("msgto"));
            if (msgtoid == -1)
            {
                AddErrLine("接收人不是注册用户");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 不论是否提交都有的权限检查
        /// </summary>
        /// <returns></returns>
        private bool CheckPermission()
        {
            // 如果是受灌水限制用户, 则判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {
                int Interval = Utils.StrDateDiffSeconds(lastpostpmtime, config.Postinterval * 2);
                if (Interval < 0)
                {
                    AddErrLine(string.Format("系统规定发帖或发短消息间隔为{0}秒, 您还需要等待 {1} 秒", (config.Postinterval * 2).ToString(), (Interval * -1).ToString()));
                    return false;
                }
            }

            if (!UserCredits.CheckUserCreditsIsEnough(userid, 1, CreditsOperationType.SendMessage, -1))
            {
                AddErrLine("您的金币不足, 不能发送短消息");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送邮件通知
        /// </summary>
        /// <param name="email">接收人邮箱</param>
        /// <param name="privatemessageinfo">短消息对象</param>
        public void SendNotifyEmail(string email, PrivateMessageInfo pm)
        {
            string jumpurl = "http://" + DNTRequest.GetCurrentFullHost() + "/usercpshowpm.aspx?pmid=" + pm.Pmid;
            System.Text.StringBuilder sb_body = new System.Text.StringBuilder("# 论坛短消息: <a href=\"" + jumpurl + "\" target=\"_blank\">" + pm.Subject + "</a>");
            //发送人邮箱
            string cur_email = Discuz.Forum.Users.GetShortUserInfo(userid).Email.Trim();
            sb_body.Append("\r\n");
            sb_body.Append("\r\n");
            sb_body.Append(pm.Message);
            sb_body.Append("\r\n<hr/>");
            sb_body.Append("作 者:" + pm.Msgfrom);
            sb_body.Append("\r\n");
            sb_body.Append("Email:<a href=\"mailto:" + cur_email + "\" target=\"_blank\">" + cur_email + "</a>");
            sb_body.Append("\r\n");
            sb_body.Append("URL:<a href=\"" + jumpurl + "\" target=\"_blank\">" + jumpurl + "</a>");
            sb_body.Append("\r\n");
            sb_body.Append("时 间:" + pm.Postdatetime);
            Emails.SendEmailNotify(email, "[" + config.Forumtitle + "短消息通知]" + pm.Subject, sb_body.ToString());
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:41.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:41. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>撰写短消息</strong>\r\n");
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

                    templateBuilder.Append("						<form id=\"postpm\" name=\"postpm\" method=\"post\" action=\"\">\r\n");
                    templateBuilder.Append("							<label for=\"user\" class=\"labelshort\">收件人:</label>\r\n");
                    templateBuilder.Append("							<input name=\"msgto\" type=\"text\" id=\"msgto\" value=\"" + msgto.ToString() + "\" size=\"20\" /><br />\r\n");
                    templateBuilder.Append("							<label for=\"email\" class=\"labelshort\">标题:</label>\r\n");
                    templateBuilder.Append("							<input name=\"subject\" type=\"text\"id=\"subject\" value=\"" + subject.ToString() + "\" size=\"40\" /><br />\r\n");
                    templateBuilder.Append("							<label for=\"comment\" class=\"labelshort\">内容:</label>\r\n");
                    templateBuilder.Append("							<textarea name=\"message\" cols=\"80\" rows=\"20\" id=\"message\" onkeydown=\"if((event.ctrlKey && event.keyCode == 13) || (event.altKey && event.keyCode == 83)) document.getElementById('postpm').submit();\" style=\"width:80%;\">" + message.ToString() + "</textarea><br/>\r\n");
                    templateBuilder.Append("							<label for=\"savetosentbox\"  class=\"labelshort\">&nbsp;</label>\r\n");
                    templateBuilder.Append("							<input name=\"savetosentbox\" type=\"checkbox\" id=\"Checkbox1\" value=\"1\" style=\"border:0;\" />发送的同时保存到发件箱 \r\n");
                    templateBuilder.Append("							<input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" />邮件通知<br />\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("							<label for=\"savetosentbox\"  class=\"labelshort\">验证码</label>\r\n");

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

                    templateBuilder.Append("							<label for=\"savetosentbox\"  class=\"labelshort\">&nbsp;</label>\r\n");
                    templateBuilder.Append("							<input name=\"sendmsg\" type=\"submit\" id=\"sendmsg\" value=\"立即发送\"/>\r\n");
                    templateBuilder.Append("							<input name=\"savetousercpdraftbox\" type=\"submit\" id=\"savetousercpdraftbox\" value=\"存为草稿\"/>\r\n");
                    templateBuilder.Append("							[完成后可按Ctrl+Enter提交]\r\n");
                    templateBuilder.Append("						</form>\r\n");

                }	//end if

                templateBuilder.Append("						</div>\r\n");

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

            templateBuilder.Append("			  </div>\r\n");
            templateBuilder.Append("			</div>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--主体-->\r\n");


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
    } //class end
}