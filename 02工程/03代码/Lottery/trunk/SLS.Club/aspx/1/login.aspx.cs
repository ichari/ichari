using System;
using System.Data;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 登录
    /// </summary>
    public partial class login : PageBase
    {
        /// <summary>
        /// 登录所使用的用户名
        /// </summary>
        public string postusername;
        /// <summary>
        /// 登陆时的密码验证信息
        /// </summary>
        public string loginauth = DNTRequest.GetString("loginauth");
        /// <summary>
        /// 登陆时提交的密码
        /// </summary>
        public string postpassword = "";
        /// <summary>
        /// 登陆成功后跳转的链接
        /// </summary>
        public string referer = DNTRequest.GetString("referer");
        /// <summary>
        /// 是否跨页面提交
        /// </summary>
        public bool loginsubmit = DNTRequest.GetString("loginsubmit") == "true" ? true : false;

        protected override void ShowPage()
        {
            pagetitle = "用户登录";

            postusername = Utils.UrlDecode(DNTRequest.GetString("postusername")).Trim();

            if (this.userid != -1)
            {
                //SetUrl("/Forum/");
                SetMetaRefresh();
                SetShowBackLink(false);
                AddMsgLine("您已经登录，无须重复登录");
                ispost = true;
                SetLeftMenuRefresh();

                APIConfigInfo apiInfo = APIConfigs.GetConfig();
                if (apiInfo.Enable)
                {
                    APILogin(apiInfo);
                }
            }

            /*
            if (LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), false) >= 5)
            {
                AddMsgLine("您已经多次输入密码错误, 请15分钟后再登录");
                loginsubmit = false;
                return;
            }
            */

            //未提交或跨页提交时
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
                Utils.WriteCookie("reurl", (DNTRequest.GetQueryString("reurl") == "" || DNTRequest.GetQueryString("reurl").IndexOf("login.aspx") > -1) ? r : DNTRequest.GetQueryString("reurl"));
            }

            //如果提交...
            if (DNTRequest.IsPost())
            {
                StringBuilder builder = new StringBuilder();
                foreach (string key in System.Web.HttpContext.Current.Request.QueryString.AllKeys)
                {
                    if (key != "postusername")
                    {
                        builder.Append("&");
                        builder.Append(key);
                        builder.Append("=");
                        builder.Append(DNTRequest.GetQueryString(key));
                    }
                }
                base.SetBackLink("login.aspx?postusername=" + Utils.UrlEncode(DNTRequest.GetString("username")) + builder.ToString());


                //如果没输入验证码就要求用户填写
                if (isseccode && DNTRequest.GetString("vcode") == "")
                {
                    postusername = DNTRequest.GetString("username");
                    loginauth = DES.Encode(DNTRequest.GetString("password"), config.Passwordkey).Replace("+", "[");
                    loginsubmit = true;
                    return;
                }

                bool isExistsUserByName = Discuz.Forum.Users.Exists(DNTRequest.GetString("username"));
                if (!isExistsUserByName)
                {
                    Discuz.Data.DatabaseProvider.GetInstance().ClubLoginLog(1, -1, System.Web.HttpContext.Current.Request.UserHostAddress, 7);
                    AddErrLine("用户不存在");
                }

                if (DNTRequest.GetString("password").Equals("") && DNTRequest.GetString("loginauth") == "")
                {
                    AddErrLine("密码不能为空");
                }

                if (IsErr())
                {
                    return;
                }

                if (!Utils.StrIsNullOrEmpty(loginauth))
                {
                    postpassword = DES.Decode(loginauth.Replace("[", "+"), config.Passwordkey);
                }
                else
                {
                    postpassword = DNTRequest.GetString("password");
                }

                if (postusername == "")
                {
                    postusername = DNTRequest.GetString("username");
                }

                int uid = -1;
                if (config.Passwordmode == 1)
                {
                    if (config.Secques == 1 && (!Utils.StrIsNullOrEmpty(loginauth) || !loginsubmit))
                    {
                        uid = Discuz.Forum.Users.CheckDvBbsPasswordAndSecques(postusername,
                                                               postpassword,
                                                               DNTRequest.GetInt("question", 0),
                                                               DNTRequest.GetString("answer"));
                    }
                    else
                    {
                        uid = Discuz.Forum.Users.CheckDvBbsPassword(postusername, postpassword);
                    }
                }
                else
                {
                    if (config.Secques == 1 && (!Utils.StrIsNullOrEmpty(loginauth) || !loginsubmit))
                    {
                        uid = Discuz.Forum.Users.CheckPasswordAndSecques(postusername,
                                                          postpassword,
                                                          true,
                                                          DNTRequest.GetInt("question", 0),
                                                          DNTRequest.GetString("answer"));
                    }
                    else
                    {
                        uid = Discuz.Forum.Users.CheckPassword(postusername, postpassword, true);
                    }
                }


                if (uid != -1)
                {
                    ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfo(uid);
                    if (userinfo.Groupid == 8)
                    {
                        AddErrLine("抱歉, 您的用户身份尚未得到验证");
                        if (config.Regverify == 1)
                        {
                            AddMsgLine("请您到您的邮箱中点击激活链接来激活您的帐号");
                        }

                        if (config.Regverify == 2)
                        {
                            AddMsgLine("您需要等待一些时间, 待系统管理员审核您的帐户后才可登录使用");
                        }
                        loginsubmit = false;
                    }
                    else
                    {
                        if (!Utils.StrIsNullOrEmpty(userinfo.Secques) && loginsubmit && Utils.StrIsNullOrEmpty(DNTRequest.GetString("loginauth")))
                        {
                            loginauth = DES.Encode(DNTRequest.GetString("password"), config.Passwordkey).Replace("+", "[");
                        }
                        else
                        {

                            LoginLogs.DeleteLoginLog(DNTRequest.GetIP());
                            UserCredits.UpdateUserCredits(uid);
                            ForumUtils.WriteUserCookie(
                                    uid,
                                    Utils.StrToInt(DNTRequest.GetString("expires"), -1),
                                    config.Passwordkey,
                                    DNTRequest.GetInt("templateid", 0),
                                    DNTRequest.GetInt("loginmode", -1));
                            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0);
                            //无延迟更新在线信息
                            oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);
                            olid = oluserinfo.Olid;
                            Discuz.Forum.Users.UpdateUserLastvisit(uid, DNTRequest.GetIP());

                            string reurl = Utils.UrlDecode(ForumUtils.GetReUrl());
                            if (reurl.IndexOf("register.aspx") < 0)
                            {
                                SetUrl(reurl);
                            }
                            else
                            {
                                SetUrl("index.aspx");
                            }

                            APIConfigInfo apiInfo = APIConfigs.GetConfig();
                            if (apiInfo.Enable)
                            {
                                APILogin(apiInfo);
                            }

                            Discuz.Forum.Users.SaveUserIDToCookie(uid);

                            Discuz.Data.DatabaseProvider.GetInstance().ClubLoginLog(1, uid, System.Web.HttpContext.Current.Request.UserHostAddress, 5);
                            AddMsgLine("登录成功, 返回登录前页面");

                            userid = uid;
                            usergroupinfo = UserGroups.GetUserGroupInfo(userinfo.Groupid);
                            // 根据用户组得到相关联的管理组id
                            useradminid = usergroupinfo.Radminid;

                            SetMetaRefresh();
                            SetShowBackLink(false);

                            SetLeftMenuRefresh();

                            loginsubmit = false;
                        }
                    }
                }
                else
                {
                    int errcount = LoginLogs.UpdateLoginLog(DNTRequest.GetIP(), true);
                    if (errcount > 5)
                    {
                        AddErrLine("您已经输入密码5次错误, 请15分钟后再试");
                    }
                    else
                    {
                        if (isExistsUserByName)
                        {
                            uid = Discuz.Data.DatabaseProvider.GetInstance().GetuidByusername(postusername);
                        }
                        Discuz.Data.DatabaseProvider.GetInstance().ClubLoginLog(1, uid, System.Web.HttpContext.Current.Request.UserHostAddress, 6);
                        AddErrLine(string.Format("密码或安全提问第{0}次错误, 您最多有5次机会重试", errcount.ToString()));
                    }
                }
            }
          
        }

        private void APILogin(APIConfigInfo apiInfo)
        {
            ApplicationInfo appInfo = null;
            ApplicationInfoCollection appcollection = apiInfo.AppCollection;
            foreach (ApplicationInfo newapp in appcollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                {
                    appInfo = newapp;
                }
            }
            if (appInfo == null)
                return;

            this.Load += delegate
            {
                RedirectAPILogin(appInfo);
                this.Load += delegate { };
            };

        }

        private void RedirectAPILogin(ApplicationInfo appInfo)
        {
            string expires = DNTRequest.GetString("expires");
            if (expires == string.Empty)
            {
                expires = Request.Cookies["dnt"]["expires"].ToString();
            }
            //CreateToken
            OnlineUsers.UpdateAction(olid, UserAction.Login.ActionID, 0);
            string next = DNTRequest.GetString("next");
            string time = string.Empty;// = DateTime.Parse(OnlineUsers.GetOnlineUser(olid).Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");
            if (OnlineUsers.GetOnlineUser(olid) == null)
            {
                time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                time = DateTime.Parse(OnlineUsers.GetOnlineUser(olid).Lastupdatetime).ToString("yyyy-MM-dd HH:mm:ss");
            }
            string authToken = DES.Encode(string.Format("{0},{1},{2}", olid.ToString(), time, expires), appInfo.Secret.Substring(0, 10)).Replace("+", "[");
            string reurl = string.Format("{0}{1}auth_token={2}{3}", appInfo.CallbackUrl, appInfo.CallbackUrl.IndexOf("?") > 0 ? "&" : "?", authToken, next == string.Empty ? next : "&next=" + next);
            //string reurl = "http://nt.discuz.net/?auth_token=" + authToken;
            Response.Redirect(reurl);
        }

        private void SetLeftMenuRefresh()
        {
            StringBuilder script = new StringBuilder();
            script.Append("if (top.document.getElementById('leftmenu')){");
            script.Append("		top.frames['leftmenu'].location.reload();");
            script.Append("}");

            AddScript(script.ToString());
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:14.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:14. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; 用户登录</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (ispost && !loginsubmit)
            {


                if (page_err == 0)
                {


                    templateBuilder.Append("<div class=\"box message\">\r\n");
                    templateBuilder.Append("	<h1>彩友提示信息</h1>\r\n");
                    templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");

                    if (msgbox_url != "")
                    {

                        templateBuilder.Append("	<p><a href=\"" + config.Forumurl.ToString().Trim() + "\">如果浏览器没有转向, 请点击这里.</a></p>\r\n");

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

                templateBuilder.Append("<form id=\"form1\" name=\"form1\" method=\"post\" \r\n");

                if (loginauth != "")
                {

                    templateBuilder.Append("action=\"login.aspx?loginauth=" + loginauth.ToString() + "&referer=" + referer.ToString() + "\"\r\n");

                }
                else
                {

                    templateBuilder.Append("action=\"\"\r\n");

                }	//end if

                templateBuilder.Append(">\r\n");
                templateBuilder.Append("<div class=\"mainbox formbox\">\r\n");
                templateBuilder.Append("	<h1>用户登录</h1>\r\n");
                templateBuilder.Append("	<table summary=\"会员登录\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                templateBuilder.Append("	<tbody>\r\n");
                templateBuilder.Append("	<tr>\r\n");
                templateBuilder.Append("		<th onclick=\"document.login.username.focus();\">\r\n");
                templateBuilder.Append("			<label>用户名</label>\r\n");
                templateBuilder.Append("		</th>\r\n");
                templateBuilder.Append("		<td>\r\n");
                templateBuilder.Append("			<input name=\"username\" type=\"text\" id=\"username\" size=\"20\" value=\"" + postusername.ToString() + "\" tabindex=\"4\" />  &nbsp; <a href=\"register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\">立即注册</a>\r\n");
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("	</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");

                if (loginauth == "")
                {

                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"password\">密码</label></th>\r\n");
                    templateBuilder.Append("		<td> \r\n");
                    templateBuilder.Append("		<input name=\"password\" type=\"password\" id=\"password\" size=\"20\" tabindex=\"5\" /> &nbsp; <a href=\"getpassword.aspx\" tabindex=\"-1\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\">忘记密码</a>\r\n");
                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");

                }	//end if


                if (isseccode)
                {

                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"formcode\">验证码:</label></th>\r\n");
                    templateBuilder.Append("		<td>\r\n");

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


                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");

                }	//end if


                if (config.Secques == 1)
                {

                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th><label for=\"question\">安全提问</label></th>\r\n");
                    templateBuilder.Append("			<td>\r\n");
                    templateBuilder.Append("				<select name=\"question\" id=\"question\" tabindex=\"6\">\r\n");
                    templateBuilder.Append("				<option value=\"0\" selected=\"selected\">无</option>\r\n");
                    templateBuilder.Append("				<option value=\"1\">母亲的名字</option>\r\n");
                    templateBuilder.Append("				<option value=\"2\">爷爷的名字</option>\r\n");
                    templateBuilder.Append("				<option value=\"3\">父亲出生的城市</option>\r\n");
                    templateBuilder.Append("				<option value=\"4\">您其中一位老师的名字</option>\r\n");
                    templateBuilder.Append("				<option value=\"5\">您个人计算机的型号</option>\r\n");
                    templateBuilder.Append("				<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
                    templateBuilder.Append("				<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
                    templateBuilder.Append("				</select>\r\n");
                    templateBuilder.Append("			</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");
                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th><label for=\"question\">答案</label></th>\r\n");
                    templateBuilder.Append("			<td><input name=\"answer\" type=\"text\" id=\"answer\" size=\"50\" tabindex=\"7\" /><br/>如果您设置了安全提问，请在此输入正确的问题和回答</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");

                }	//end if

                //templateBuilder.Append("	<tbody>\r\n");
                //templateBuilder.Append("		<tr>\r\n");
                //templateBuilder.Append("			<th><label for=\"expires\">有效时间</label></th>\r\n");
                //templateBuilder.Append("			<td>\r\n");
                //templateBuilder.Append("				<input type=\"radio\" name=\"expires\" value=\"5256000\" tabindex=\"8\" />永久\r\n");
                //templateBuilder.Append("				<input name=\"expires\" type=\"radio\" value=\"43200\" checked tabindex=\"9\" />一个月  \r\n");
                //templateBuilder.Append("				<input type=\"radio\" name=\"expires\" value=\"1440\" tabindex=\"10\" />一天  \r\n");
                //templateBuilder.Append("				<input type=\"radio\" name=\"expires\" value=\"60\" tabindex=\"11\" />一小时 \r\n");
                //templateBuilder.Append("				<input type=\"radio\" name=\"expires\" value=\"0\" tabindex=\"12\" />浏览器进程\r\n");
                //templateBuilder.Append("			</td>\r\n");
                //templateBuilder.Append("		</tr>\r\n");
                //templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("	<tbody>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th><label for=\"templateid\">界面风格</label></th>\r\n");
                templateBuilder.Append("			<td class=\"formbody\">\r\n");
                templateBuilder.Append("				<select name=\"templateid\" tabindex=\"13\"><option value=\"0\">- 使用默认 -</option>\r\n");
                templateBuilder.Append("				" + templatelistboxoptions.ToString() + "\r\n");
                templateBuilder.Append("				</select>\r\n");
                templateBuilder.Append("			</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("	<tbody>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("		<th><label for=\"templateid\">&nbsp;</label></th>\r\n");
                templateBuilder.Append("		<td>\r\n");
                templateBuilder.Append("			<input name=\"login\" type=\"submit\" id=\"login\" value=\"登录\" onclick=\"javascript:window.location.replace('?agree=yes')\"/>\r\n");
                templateBuilder.Append("			<input name=\"cancel\" type=\"button\" id=\"cancel\" value=\"取消\" onclick=\"javascript:window.location.replace('./index.aspx')\"/>\r\n");
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("	document.getElementById(\"username\").focus();\r\n");
                templateBuilder.Append("</" + "script>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("</form>\r\n");

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
            templateBuilder.Append("	<li><a href=\"" + spaceurl.ToString() + "space/\">我的空间</a></li>\r\n");

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