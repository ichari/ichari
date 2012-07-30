using System;
using System.Data;
using System.Data.Common;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 设置新密码
    /// </summary>
    public partial class setnewpassword : PageBase
    {
        protected override void ShowPage()
        {
            pagetitle = "密码找回";
            username = DNTRequest.GetString("username");
            int uid = DNTRequest.GetInt("uid", 0);
            string Authstr = DNTRequest.GetQueryString("id") != "" ? DNTRequest.GetQueryString("id") : DNTRequest.GetString("authstr");
            DateTime authtime = DateTime.Now.AddDays(-3);

            base.SetBackLink("/");

            //更新激活字段
            UserInfo __userinfo = Discuz.Forum.Users.GetUserInfo(uid);
            if (__userinfo == null)
            {
                AddErrLine("用户名不存在,你无法重设密码");
                return;
            }

            if ((!__userinfo.Authstr.Equals(Authstr)) || Convert.ToDateTime(__userinfo.Authtime) < authtime)
            {
                ReSendMail(__userinfo.Uid, __userinfo.Username, __userinfo.Email.Trim());
                SetUrl("/");
                SetMetaRefresh(5);
                SetShowBackLink(false);
                AddErrLine("验证码已失效,新的验证码已经通过 Email 发送到您的信箱中,<BR />请在 3 天之内到论坛修改您的密码.");
                return;
            }

            //如果提交了用户注册信息...
            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                base.SetBackLink("setnewpassword.aspx?uid=" + uid.ToString() + "&id=" + Authstr);

                if (DNTRequest.GetString("newpassword").Equals(""))
                {
                    AddErrLine("新密码不能为空");
                }

                if (!DNTRequest.GetString("newpassword").Equals(DNTRequest.GetString("confirmpassword")))
                {
                    AddErrLine("两次密码输入不一致");
                }

                if (Utils.StrIsNullOrEmpty(Authstr) || !__userinfo.Authstr.Equals(Authstr))
                {
                    AddErrLine("您所提供的验证码与注册信息不符.");
                }

                if (IsErr())
                {
                    return;
                }

                if (Utils.IsSafeSqlString(DNTRequest.GetString("newpassword")) && Discuz.Forum.Users.UpdateUserPassword(uid, DNTRequest.GetString("newpassword")))
                {
                    Discuz.Forum.Users.UpdateAuthStr(uid, "", 0); //将验证码清空,并设置验证标志为无效

                    SetUrl("login.aspx");
                    SetMetaRefresh(5);
                    SetShowBackLink(false);
                    AddMsgLine("你的密码已被重新设置,请用新密码登录.");
                }
                else
                {
                    AddErrLine("用户名,Email 地址或安全提问不匹配,请返回修改.");
                }
            }
        }


        private void ReSendMail(int uid, string username, string email)
        {
            string Authstr = ForumUtils.CreateAuthStr(20);
            Discuz.Forum.Users.UpdateAuthStr(uid, Authstr, 2);

            string title = config.Forumtitle + " 取回密码说明";
            StringBuilder body = new StringBuilder();
            body.Append(username);
            body.Append("您好!<BR />这封信是由 ");
            body.Append(config.Forumtitle);
            body.Append(" 发送的.<BR /><BR />您收到这封邮件,是因为在我们的论坛上这个邮箱地址被登记为用户邮箱,且该用户请求使用 Email 密码重置功能所致.");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR />重要！");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR /><BR />如果您没有提交密码重置的请求或不是我们论坛的注册用户,请立即忽略并删除这封邮件.只在您确认需要重置密码的情况下,才继续阅读下面的内容.");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR />密码重置说明");
            body.Append("<BR /><BR />----------------------------------------------------------------------");
            body.Append("<BR /><BR />您只需在提交请求后的三天之内,通过点击下面的链接重置您的密码:");
            body.Append("<BR /><BR /><a href=" + GetForumPath() + "/setnewpassword.aspx?uid=" + uid + "&id=" + Authstr +
                        " target=_blank>");
            body.Append(GetForumPath());
            body.Append("/setnewpassword.aspx?uid=");
            body.Append(uid);
            body.Append("&id=");
            body.Append(Authstr);
            body.Append("</a>");

            body.Append("<BR /><BR />(如果上面不是链接形式,请将地址手工粘贴到浏览器地址栏再访问)");
            body.Append("<BR /><BR />上面的页面打开后,输入新的密码后提交,之后您即可使用新的密码登录论坛了.您可以在用户控制面板中随时修改您的密码.");
            body.Append("<BR /><BR />本请求提交者的 IP 为 ");
            body.Append(DNTRequest.GetIP());
            body.Append("<BR /><BR /><BR /><BR />");
            body.Append("<BR />此致 <BR /><BR />");
            body.Append(config.Forumtitle);
            body.Append(" 管理团队.");
            body.Append("<BR />");
            body.Append(GetForumPath());
            body.Append("<BR /><BR />");


            Emails.DiscuzSmtpMailToUser(email, title, body.ToString());
        }

        private string GetForumPath()
        {
            return this.Context.Request.Url.ToString().ToLower().Substring(0, this.Context.Request.Url.ToString().ToLower().IndexOf("/aspx/"));
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:16.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:16. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; 密码重置</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (ispost)
            {


                if (page_err == 0)
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


            }
            else
            {


                if (page_err == 0)
                {

                    templateBuilder.Append("<form id=\"form1\" name=\"form1\" method=\"post\" action=\"setnewpassword.aspx#\">	\r\n");
                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h3>密码重置</h3>\r\n");
                    templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" summary=\"密码重置\">\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th><label for=\"newpassword\">新密码</label></th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<input name=\"newpassword\" type=\"password\" id=\"newpassword\" size=\"25\"  />\r\n");
                    templateBuilder.Append("					<input name=\"uid\" type=\"hidden\" id=\"uid\" value=\"" + DNTRequest.GetString("uid") + "\" />\r\n");
                    templateBuilder.Append("					<input name=\"authstr\" type=\"hidden\" id=\"authstr\" value=\"" + DNTRequest.GetString("id") + "\" />\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th><label for=\"confirmpassword\">确认密码</label></th>\r\n");
                    templateBuilder.Append("			<td class=\"formbody\"><input name=\"confirmpassword\" type=\"password\" id=\"confirmpassword\" size=\"25\" >\r\n");
                    templateBuilder.Append("			</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"vcode\">验证码</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

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


                        templateBuilder.Append("</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<input name=\"login\" type=\"submit\" id=\"login\" value=\"确定\" onclick=\"javascript:location.replace('?agree=yes')\" />\r\n");
                    templateBuilder.Append("					<input name=\"cancel\" type=\"button\" id=\"cancel\" value=\"取消\" onclick=\"javascript:location.replace('/')\" />\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>					\r\n");
                    templateBuilder.Append("</form>\r\n");
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