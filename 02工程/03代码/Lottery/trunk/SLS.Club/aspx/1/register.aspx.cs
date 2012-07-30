using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;
using System.Text.RegularExpressions;

namespace Discuz.Web
{
    /// <summary>
    /// 注册页
    /// </summary>
    public partial class register : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 可用的模板列表
        /// </summary>
        public DataTable templatelist;
        /// <summary>
        /// 此变量等于1时创建用户,否则显示填写用户信息界面
        /// </summary>
        public string createuser;
        /// <summary>
        /// 是否同意注册协议
        /// </summary>
        public string agree;
        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户注册";

            createuser = DNTRequest.GetString("createuser");
            agree = DNTRequest.GetFormString("agree");
            if (config.Rules == 0)
            {
                agree = "true";
            }

            if (userid != -1)
            {
                SetUrl("/");
                SetMetaRefresh();
                SetShowBackLink(false);
                AddMsgLine("不能重复注册用户");
                ispost = true;
                createuser = "1";
                agree = "yes";

                return;
            }


            templatelist = Templates.GetValidTemplateList();

            if (config.Regstatus != 1)
            {
                AddErrLine("论坛当前禁止新用户注册");
                return;
            }

            if (config.Regctrl > 0)
            {
                ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfoByIP(DNTRequest.GetIP());
                if (userinfo != null)
                {
                    int Interval = Utils.StrDateDiffHours(userinfo.Joindate, config.Regctrl);
                    if (Interval <=0)
                    {
                        AddErrLine("抱歉, 系统设置了IP注册间隔限制, 您必须在 " + (Interval * -1).ToString() + " 小时后才可以注册");
                        return;
                    }
                }
            }

            if (config.Ipregctrl.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Ipregctrl, "\n");
                //string[] userip = Utils.SplitString(DNTRequest.GetIP(),".");
                if (Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                {
                    ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfoByIP(DNTRequest.GetIP());
                    if (userinfo != null)
                    {
                        int Interval = Utils.StrDateDiffHours(userinfo.Joindate, 72);
                        if (Interval < 0)
                        {
                            AddErrLine("抱歉, 系统设置了特殊IP注册限制, 您必须在 " + (Interval * -1).ToString() + " 小时后才可以注册");
                            return;
                        }
                    }
                }
            }


            //如果提交了用户注册信息...
            if (!createuser.Equals("") && ispost)
            {
                SetShowBackLink(true);

                string tmpUsername = DNTRequest.GetString("username");

                string email = DNTRequest.GetString("email").Trim().ToLower();

                string tmpBday = DNTRequest.GetString("bday").Trim();
                if (tmpBday == "")
                {
                    tmpBday = DNTRequest.GetString("bday_y").Trim()
                            + "-"
                            + DNTRequest.GetString("bday_m").Trim()
                            + "-"
                            + DNTRequest.GetString("bday_d").Trim();
                }
                if (tmpBday == "--")
                {
                    tmpBday = "";
                }

                ValidateUserInfo(tmpUsername, email, tmpBday);


                if (IsErr())
                {
                    return;
                }


                if (Discuz.Forum.Users.Exists(tmpUsername))
                {
                    //如果用户名符合注册规则, 则判断是否已存在
                    AddErrLine("请不要重复提交！");
                    return;
                }
                // 如果找不到0金币的用户组则用户自动成为待验证用户


                UserInfo userinfo = new UserInfo();
                userinfo.Username = tmpUsername;
                userinfo.Nickname = Utils.HtmlEncode(DNTRequest.GetString("nickname"));
                userinfo.Password = Utils.MD5(DNTRequest.GetString("password"));
                userinfo.Secques = ForumUtils.GetUserSecques(DNTRequest.GetInt("question", 0), DNTRequest.GetString("answer"));
                userinfo.Gender = DNTRequest.GetInt("gender", 0);
                userinfo.Adminid = 0;
                userinfo.Groupexpiry = 0;
                userinfo.Extgroupids = "";
                userinfo.Regip = DNTRequest.GetIP();
                userinfo.Joindate = Utils.GetDateTime();
                userinfo.Lastip = DNTRequest.GetIP();
                userinfo.Lastvisit = Utils.GetDateTime();
                userinfo.Lastactivity = Utils.GetDateTime();
                userinfo.Lastpost = Utils.GetDateTime();
                userinfo.Lastpostid = 0;
                userinfo.Lastposttitle = "";
                userinfo.Posts = 0;
                userinfo.Digestposts = 0;
                userinfo.Oltime = 0;
                userinfo.Pageviews = 0;
                userinfo.Credits = 0;
                userinfo.Extcredits1 = Scoresets.GetScoreSet(1).Init;
                userinfo.Extcredits2 = Scoresets.GetScoreSet(2).Init;
                userinfo.Extcredits3 = Scoresets.GetScoreSet(3).Init;
                userinfo.Extcredits4 = Scoresets.GetScoreSet(4).Init;
                userinfo.Extcredits5 = Scoresets.GetScoreSet(5).Init;
                userinfo.Extcredits6 = Scoresets.GetScoreSet(6).Init;
                userinfo.Extcredits7 = Scoresets.GetScoreSet(7).Init;
                userinfo.Extcredits8 = Scoresets.GetScoreSet(8).Init;
                userinfo.Avatarshowid = 0;
                userinfo.Email = email;
                userinfo.Bday = tmpBday;
                userinfo.Sigstatus = DNTRequest.GetInt("sigstatus", 0);

                if (userinfo.Sigstatus != 0)
                {
                    userinfo.Sigstatus = 1;
                }
                userinfo.Tpp = DNTRequest.GetInt("tpp", 0);
                userinfo.Ppp = DNTRequest.GetInt("ppp", 0);
                userinfo.Templateid = DNTRequest.GetInt("templateid", 0);
                userinfo.Pmsound = DNTRequest.GetInt("pmsound", 0);
                userinfo.Showemail = DNTRequest.GetInt("showemail", 0);

                int receivepmsetting = 1;
                foreach (string rpms in DNTRequest.GetString("receivesetting").Split(','))
                {
                    if (rpms != string.Empty)
                    {
                        int tmp = int.Parse(rpms);
                        receivepmsetting = receivepmsetting | tmp;
                    }
                }

                if (config.Regadvance == 0)
                {
                    receivepmsetting = 7;
                }

                userinfo.Newsletter = (ReceivePMSettingType)receivepmsetting;
                userinfo.Invisible = DNTRequest.GetInt("invisible", 0);
                userinfo.Newpm = 0;
                userinfo.Medals = "";
                if (config.Welcomemsg == 1)
                {
                    userinfo.Newpm = 1;
                }
                userinfo.Accessmasks = DNTRequest.GetInt("accessmasks", 0);
                userinfo.Website = Utils.HtmlEncode(DNTRequest.GetString("website"));
                userinfo.Icq = Utils.HtmlEncode(DNTRequest.GetString("icq"));
                userinfo.Qq = Utils.HtmlEncode(DNTRequest.GetString("qq"));
                userinfo.Yahoo = Utils.HtmlEncode(DNTRequest.GetString("yahoo"));
                userinfo.Msn = Utils.HtmlEncode(DNTRequest.GetString("msn"));
                userinfo.Skype = Utils.HtmlEncode(DNTRequest.GetString("skype"));
                userinfo.Location = Utils.HtmlEncode(DNTRequest.GetString("location"));
                if (usergroupinfo.Allowcstatus == 1)
                {
                    userinfo.Customstatus = Utils.HtmlEncode(DNTRequest.GetString("customstatus"));
                }
                else
                {
                    userinfo.Customstatus = "";
                }
                userinfo.Avatar = @"avatars\common\0.gif";
                userinfo.Avatarwidth = 0;
                userinfo.Avatarheight = 0;
                userinfo.Bio = DNTRequest.GetString("bio");
                userinfo.Signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature")));

                PostpramsInfo postpramsinfo = new PostpramsInfo();
                postpramsinfo.Usergroupid = usergroupid;
                postpramsinfo.Attachimgpost = config.Attachimgpost;
                postpramsinfo.Showattachmentpath = config.Showattachmentpath;
                postpramsinfo.Hide = 0;
                postpramsinfo.Price = 0;
                postpramsinfo.Sdetail = userinfo.Signature;
                postpramsinfo.Smileyoff = 1;
                postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
                postpramsinfo.Parseurloff = 1;
                postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
                postpramsinfo.Allowhtml = 0;
                postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
                postpramsinfo.Smiliesmax = config.Smiliesmax;

                userinfo.Sightml = UBB.UBBToHTML(postpramsinfo);

                //
                userinfo.Authtime = Utils.GetDateTime();

                //邮箱激活链接验证
                if (config.Regverify == 1)
                {
                    userinfo.Authstr = ForumUtils.CreateAuthStr(20);
                    userinfo.Authflag = 1;
                    userinfo.Groupid = 8;
                    SendEmail(tmpUsername, DNTRequest.GetString("password").Trim(), DNTRequest.GetString("email").Trim(), userinfo.Authstr);
                }
                //系统管理员进行后台验证
                else if (config.Regverify == 2)
                {
                    userinfo.Authstr = DNTRequest.GetString("website");
                    userinfo.Groupid = 8;
                    userinfo.Authflag = 1;
                }
                else
                {
                    userinfo.Authstr = "";
                    userinfo.Authflag = 0;
                    userinfo.Groupid = UserCredits.GetCreditsUserGroupID(0).Groupid;
                }
                userinfo.Realname = DNTRequest.GetString("realname");
                userinfo.Idcard = DNTRequest.GetString("idcard");
                userinfo.Mobile = DNTRequest.GetString("mobile");
                userinfo.Phone = DNTRequest.GetString("phone");

                int uid = Discuz.Forum.Users.CreateUser(userinfo);
                userinfo.Uid = uid;
                if (config.Welcomemsg == 1)
                {
                    PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();

                    string curdatetime = Utils.GetDateTime();
                    // 收件箱
                    privatemessageinfo.Message = config.Welcomemsgtxt;
                    privatemessageinfo.Subject = "欢迎您的加入! (请勿回复本信息)";
                    privatemessageinfo.Msgto = userinfo.Username;
                    privatemessageinfo.Msgtoid = uid;
                    privatemessageinfo.Msgfrom = PrivateMessages.SystemUserName;
                    privatemessageinfo.Msgfromid = 0;
                    privatemessageinfo.New = 1;
                    privatemessageinfo.Postdatetime = curdatetime;
                    privatemessageinfo.Folder = 0;
                    PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
                }

                if (config.Regverify == 0)
                {
                    UserCredits.UpdateUserCredits(uid);
                    ForumUtils.WriteUserCookie(userinfo, -1, config.Passwordkey);
                    OnlineUsers.UpdateAction(olid, UserAction.Register.ActionID, 0, config.Onlinetimeout);

                    Discuz.Forum.Users.SaveUserIDToCookie(uid);
                    
                    Statistics.ReSetStatisticsCache();

                    SetUrl("index.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    AddMsgLine("注册成功, 返回登录页");
                }
                else
                {
                    SetUrl("index.aspx");
                    SetMetaRefresh(5);
                    SetShowBackLink(false);
                    Statistics.ReSetStatisticsCache();
                    if (config.Regverify == 1)
                    {
                        AddMsgLine("注册成功, 请您到您的邮箱中点击激活链接来激活您的帐号");
                    }

                    if (config.Regverify == 2)
                    {
                        AddMsgLine("注册成功, 但需要系统管理员审核您的帐户后才可登陆使用");
                    }
                }
                agree = "yes";
            }
        }

        /// <summary>
        /// 验证用户信息
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="birthday"></param>
        private void ValidateUserInfo(string username, string email, string birthday)
        {

            #region CheckUserName
            if (username.Equals(""))
            {
                AddErrLine("用户名不能为空");
                return;
            }
            if (username.Length > 20)
            {
                //如果用户名超过20...
                AddErrLine("用户名不得超过20个字符");
                return;
            }
            if (Utils.GetStringLength(username) < 3)
            {
                AddErrLine("用户名不得小于3个字符");
                return;
            }
            if (username.IndexOf("　") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含全角空格符");
                return;
            }
            if (username.IndexOf(" ") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含空格");
                return;
            }
            if (username.IndexOf(":") != -1)
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("用户名中不允许包含冒号");
                return;
            }
            if (Discuz.Forum.Users.Exists(username))
            {
                //如果用户名符合注册规则, 则判断是否已存在
                AddErrLine("该用户名已存在");
                return;
            }
            if ((!Utils.IsSafeSqlString(username)) || (!Utils.IsSafeUserInfoString(username)))
            {
                AddErrLine("用户名中存在非法字符");
                return;
            }
            // 如果用户名属于禁止名单, 或者与负责发送新用户注册欢迎信件的用户名称相同...
            if (username.Trim() == PrivateMessages.SystemUserName ||
                     ForumUtils.IsBanUsername(username, config.Censoruser))
            {
                AddErrLine("用户名 \"" + username + "\" 不允许在本论坛使用");
                return;
            }
            #endregion

            #region CheckPassword
            // 检查密码
            if (DNTRequest.GetString("password").Equals(""))
            {
                AddErrLine("密码不能为空");
                return;
            }
            if (!DNTRequest.GetString("password").Equals(DNTRequest.GetString("password2")))
            {
                AddErrLine("两次密码输入必须相同");
                return;
            }
            if (DNTRequest.GetString("password").Length < 6)
            {
                AddErrLine("密码不得少于6个字符");
                return;
            }
            #endregion

            #region CheckMail
            if (email.Equals(""))
            {
                AddErrLine("Email不能为空");
                return;
            }

            if (!Utils.IsValidEmail(email))
            {
                AddErrLine("Email格式不正确");
                return;
            }
            if (config.Doublee == 0 && Discuz.Forum.Users.FindUserEmail(email) != -1)
            {
                AddErrLine("Email: \"" + email + "\" 已经被其它用户注册使用");
                return;
            }

            string emailhost = Utils.GetEmailHostName(email);
                // 允许名单规则优先于禁止名单规则
                if (config.Accessemail.Trim() != "")
                {
                    // 如果email后缀 不属于 允许名单
                    if (!Utils.InArray(emailhost, config.Accessemail.Replace("\r\n", "\n"), "\n"))
                    {
                        AddErrLine("Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " +
                                   config.Accessemail.Replace("\n", ",").Replace("\r", ""));
                        return;
                    }
                }
                if (config.Censoremail.Trim() != "")
                {
                    // 如果email后缀 属于 禁止名单
                    if (Utils.InArray(email, config.Censoremail.Replace("\r\n", "\n"), "\n"))
                    {
                        AddErrLine("Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " +
                                   config.Censoremail.Replace("\n", ",").Replace("\r", ""));
                        return;
                    }
                }
            #endregion

            #region CheckRealInfo
            //实名验证
            if (config.Realnamesystem == 1)
            {
                var idcard = DNTRequest.GetString("idcard").Trim().Replace(",","");
                if (DNTRequest.GetString("realname").Trim() == "")
                {
                    AddErrLine("真实姓名不能为空");
                    return;
                }
                if (DNTRequest.GetString("realname").Trim().Length > 10)
                {
                    AddErrLine("真实姓名不能大于10个字符");
                    return;
                }
                //if (idcard == "")
                //{
                //    AddErrLine("身份证号码不能为空");
                //    return;
                //}
                //if (idcard.Length > 20)
                //{
                //    AddErrLine("身份证号码不能大于20个字符");
                //    return;
                //}


                //Regex reTai = new Regex(@"[A-Za-z][12]\d{8}");
                //Regex reXiang = new Regex(@"[A-Za-z]{1,2}\d{6}\(([0-9]|[Aa])\)");
                //Regex reXin = new Regex(@"\d{7}[A-JZa-jz]");
                //Regex reAo = new Regex(@"\d{7}\([0-9]\)");
                //Regex reDaLu = new Regex(@"\d{17}[Xx0-9]|\d{15}");

                //if (!Shove._String.Valid.isIDCardNumber(idcard) && !Shove._String.Valid.isIDCardNumber_Hongkong(idcard) && !Shove._String.Valid.isIDCardNumber_Macau(idcard) && !Shove._String.Valid.isIDCardNumber_Singapore(idcard) && !Shove._String.Valid.isIDCardNumber_Taiwan(idcard))
                //{
                //    AddErrLine("身份证号码格式不正确");
                //    return;
                //}
                
                if (DNTRequest.GetString("mobile").Trim() == "" && DNTRequest.GetString("phone").Trim() == "")
                {
                    AddErrLine("移动电话号码或固定电话号码必须填写其中一项");
                    return;
                }
                if (DNTRequest.GetString("mobile").Trim().Length > 20)
                {
                    AddErrLine("移动电话号码不能大于20个字符");
                    return;
                }
                if (DNTRequest.GetString("phone").Trim().Length > 20)
                {
                    AddErrLine("固定电话号码不能大于20个字符");
                    return;
                }
            }
            #endregion

            //if (DNTRequest.GetString("idcard").Trim() != "" &&
            //    !Regex.IsMatch(DNTRequest.GetString("idcard").Trim(), @"^[\x20-\x80]+$"))
            //{
            //    AddErrLine("身份证号码中含有非法字符");
            //    return;
            //}

            //if (DNTRequest.GetString("mobile").Trim() != "" &&
            //    !Regex.IsMatch(DNTRequest.GetString("mobile").Trim(), @"^[\d|-]+$"))
            //{
            //    AddErrLine("移动电话号码中含有非法字符");
            //    return;
            //}

            //if (DNTRequest.GetString("phone").Trim() != "" &&
            //    !Regex.IsMatch(DNTRequest.GetString("phone").Trim(), @"^[\d|-]+$"))
            //{
            //    AddErrLine("固定电话号码中含有非法字符");
            //    return;
            //}


            //用户注册模板中,生日可以单独用一个名为bday的文本框, 也可以分别用bday_y bday_m bday_d三个文本框, 用户可不填写

            if (!Utils.IsDateString(birthday) && !birthday.Equals(""))
            {
                AddErrLine("生日格式错误, 如果不想填写生日请置空");
                return;
            }
            if (DNTRequest.GetString("bio").Length > 500)
            {
                //如果自我介绍超过500...
                AddErrLine("自我介绍不得超过500个字符");
                return;
            }
            if (DNTRequest.GetString("signature").Length > 500)
            {
                //如果签名超过500...
                AddErrLine("签名不得超过500个字符");
                return;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="emailaddress"></param>
        /// <param name="authstr"></param>
        private void SendEmail(string username, string password, string emailaddress, string authstr)
        {
            Emails.DiscuzSmtpMail(username, emailaddress, password, authstr);
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:48.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:48. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; 用户注册</div>\r\n");
            templateBuilder.Append("<!--reg start-->\r\n");

            if (agree == "")
            {


                if (page_err == 0)
                {


                    if (config.Rules == 1)
                    {

                        templateBuilder.Append("		<form id=\"form1\" name=\"form1\" method=\"post\" action=\"\">\r\n");
                        templateBuilder.Append("		<div class=\"mainbox formbox regbox\">\r\n");
                        templateBuilder.Append("		<h1>用户注册协议</h1>\r\n");
                        templateBuilder.Append("		<table cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" align=\"center\" class=\"register\">\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<textarea name=\"textarea\" cols=\"\" rows=\"\" readonly=\"readonly\" style=\"width:700px;height:320px;margin:10px 0;\">" + config.Rulestxt.ToString().Trim() + "</textarea>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("			<td style=\"padding:2px;\">\r\n");
                        templateBuilder.Append("				<input name=\"agree\" type=\"hidden\" value=\"true\" />\r\n");
                        templateBuilder.Append("			  <input disabled=\"disabled\" type=\"submit\" id=\"btnagree\" value=\"我同意\" />\r\n");
                        templateBuilder.Append("			  &nbsp;&nbsp;\r\n");
                        templateBuilder.Append("			  <input name=\"cancel\" type=\"button\" id=\"cancel\" value=\"不同意\" onClick=\"javascript:location.replace('index.aspx')\" />				  \r\n");
                        templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("				var secs = 5;\r\n");
                        templateBuilder.Append("				var wait = secs * 1000;\r\n");
                        templateBuilder.Append("				$(\"btnagree\").value = \"同 意(\" + secs + \")\";\r\n");
                        templateBuilder.Append("				$(\"btnagree\").disabled = true;\r\n");
                        templateBuilder.Append("				for(i = 1; i <= secs; i++) {\r\n");
                        templateBuilder.Append("						window.setTimeout(\"update(\" + i + \")\", i * 1000);\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("				window.setTimeout(\"timer()\", wait);\r\n");
                        templateBuilder.Append("				function update(num, value) {\r\n");
                        templateBuilder.Append("						if(num == (wait/1000)) {\r\n");
                        templateBuilder.Append("								$(\"btnagree\").value = \"同 意\";\r\n");
                        templateBuilder.Append("						} else {\r\n");
                        templateBuilder.Append("								printnr = (wait / 1000) - num;\r\n");
                        templateBuilder.Append("								$(\"btnagree\").value = \"同 意(\" + printnr + \")\";\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("				function timer() {\r\n");
                        templateBuilder.Append("						$(\"btnagree\").disabled = false;\r\n");
                        templateBuilder.Append("						$(\"btnagree\").value = \"同 意\";\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("				</" + "script>\r\n");
                        templateBuilder.Append("				 </td>\r\n");
                        templateBuilder.Append("			 </tr>\r\n");
                        templateBuilder.Append("			 </tbody>\r\n");
                        templateBuilder.Append("		</table>\r\n");
                        templateBuilder.Append("		</div>\r\n");
                        templateBuilder.Append("		</form>\r\n");

                        /*
                        <script type="text/javascript">
                        location.replace('register.aspx?agree=yes')
                        </" + "script>
                        */


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

                templateBuilder.Append("	</div>\r\n");

            }
            else
            {


                if (createuser == "")
                {

                    templateBuilder.Append("<form id=\"form1\" name=\"form1\" method=\"post\" action=\"?createuser=1\">\r\n");
                    templateBuilder.Append("<div class=\"mainbox formbox\">\r\n");
                    templateBuilder.Append("	<h1>填写注册信息</h1>\r\n");
                    templateBuilder.Append("	<table summary=\"注册\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                    templateBuilder.Append("	<thead>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>基本信息 ( * 为必填项)</th>\r\n");
                    templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("	</thead>\r\n");
                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"username\">用户名 * </label></th>\r\n");
                    templateBuilder.Append("		<td><input name=\"username\" type=\"text\" id=\"username\" maxlength=\"20\" size=\"20\"  onkeyup=\"checkusername(this.value);\" /><span id=\"checkresult\">不超过20个字符</span></td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");
                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"password\">密码 * </label></th>\r\n");
                    templateBuilder.Append("		<td><input name=\"password\" type=\"password\" id=\"password\" size=\"20\" onblur=\"return checkpasswd(this);\" /><span id=\"pshowmsg\"></span></td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>	\r\n");
                    templateBuilder.Append("	<tbody id=\"passwdpower\" style=\"display: none;\">\r\n");
                    templateBuilder.Append("	<tr><th>密码强度</th><td>\r\n");
                    templateBuilder.Append("	 <script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("				var PasswordStrength ={\r\n");
                    templateBuilder.Append("					Level : [\"极佳\",\"一般\",\"较弱\",\"太短\"],\r\n");
                    templateBuilder.Append("					LevelValue : [15,10,5,0],//强度值\r\n");
                    templateBuilder.Append("					Factor : [1,2,5],//字符加数,分别为字母，数字，其它\r\n");
                    templateBuilder.Append("					KindFactor : [0,0,10,20],//密码含几种组成的加数 \r\n");
                    templateBuilder.Append("					Regex : [/[a-zA-Z]/g,/\\d/g,/[^a-zA-Z0-9]/g] //字符正则数字正则其它正则\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("				PasswordStrength.StrengthValue = function(pwd)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					var strengthValue = 0;\r\n");
                    templateBuilder.Append("					var ComposedKind = 0;\r\n");
                    templateBuilder.Append("					for(var i = 0 ; i < this.Regex.length;i++)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						var chars = pwd.match(this.Regex[i]);\r\n");
                    templateBuilder.Append("						if(chars != null)\r\n");
                    templateBuilder.Append("						{\r\n");
                    templateBuilder.Append("							strengthValue += chars.length * this.Factor[i];\r\n");
                    templateBuilder.Append("							ComposedKind ++;\r\n");
                    templateBuilder.Append("						}\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					strengthValue += this.KindFactor[ComposedKind];\r\n");
                    templateBuilder.Append("					return strengthValue;\r\n");
                    templateBuilder.Append("				} \r\n");
                    templateBuilder.Append("				PasswordStrength.StrengthLevel = function(pwd)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					var value = this.StrengthValue(pwd);\r\n");
                    templateBuilder.Append("					for(var i = 0 ; i < this.LevelValue.length ; i ++)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						if(value >= this.LevelValue[i] )\r\n");
                    templateBuilder.Append("							return this.Level[i];\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function checkpasswd(o)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					var pshowmsg;\r\n");
                    templateBuilder.Append("					if(o.value.length<6)  {\r\n");
                    templateBuilder.Append("					   pshowmsg=\"<img src='templates/" + templatepath.ToString() + "/images/check_error.gif'/><span>不得少于6个字符</span>\";\r\n");
                    templateBuilder.Append("					} else {\r\n");
                    templateBuilder.Append("					   var showmsg=PasswordStrength.StrengthLevel(o.value);\r\n");
                    templateBuilder.Append("					   switch(showmsg) {\r\n");
                    templateBuilder.Append("					   case \"太短\": showmsg+=\" <img src='images/level/1.gif' width='88' height='11' />\";break;\r\n");
                    templateBuilder.Append("					   case \"较弱\": showmsg+=\" <img src='images/level/2.gif' width='88' height='11' />\";break;\r\n");
                    templateBuilder.Append("					   case \"一般\": showmsg+=\" <img src='images/level/3.gif' width='88' height='11' />\";break;\r\n");
                    templateBuilder.Append("					   case \"极佳\": showmsg+=\" <img src='images/level/4.gif' width='88' height='11' />\";break;\r\n");
                    templateBuilder.Append("					   }\r\n");
                    templateBuilder.Append("					   pshowmsg=\"<img src='templates/" + templatepath.ToString() + "/images/check_right.gif'/>\";\r\n");
                    templateBuilder.Append("					   $('showmsg').innerHTML = showmsg;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					$('pshowmsg').innerHTML = pshowmsg;\r\n");
                    templateBuilder.Append("					$(\"passwdpower\").style.display = \"\";\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function checkemail(strMail)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("				    var str;\r\n");
                    templateBuilder.Append("					if(strMail.length==0) return false; \r\n");
                    templateBuilder.Append("					var objReg = new RegExp(\"[A-Za-z0-9-_]+@[A-Za-z0-9-_]+[\\.][A-Za-z0-9-_]\") \r\n");
                    templateBuilder.Append("					var IsRightFmt = objReg.test(strMail) \r\n");
                    templateBuilder.Append("					var objRegErrChar = new RegExp(\"[^a-z0-9-._@]\",\"ig\") \r\n");
                    templateBuilder.Append("					var IsRightChar = (strMail.search(objRegErrChar)==-1) \r\n");
                    templateBuilder.Append("					var IsRightLength = strMail.length <= 60 \r\n");
                    templateBuilder.Append("					var IsRightPos = (strMail.indexOf(\"@\",0) != 0 && strMail.indexOf(\".\",0) != 0 && strMail.lastIndexOf(\"@\")+1 != strMail.length && strMail.lastIndexOf(\".\")+1 != strMail.length) \r\n");
                    templateBuilder.Append("					var IsNoDupChar = (strMail.indexOf(\"@\",0) == strMail.lastIndexOf(\"@\")) \r\n");
                    templateBuilder.Append("                    if(IsRightFmt && IsRightChar && IsRightLength && IsRightPos && IsNoDupChar) \r\n");
                    templateBuilder.Append("                     {str=\"<img src='templates/" + templatepath.ToString() + "/images/check_right.gif'/>\"}\r\n");
                    templateBuilder.Append("					 else\r\n");
                    templateBuilder.Append("					 {str=\"<img src='templates/" + templatepath.ToString() + "/images/check_error.gif'/>Email 地址无效，请返回重新填写。\";}\r\n");
                    templateBuilder.Append("                  $('isemail').innerHTML=str;\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function htmlEncode(source, display, tabs)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					function special(source)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						var result = '';\r\n");
                    templateBuilder.Append("						for (var i = 0; i < source.length; i++)\r\n");
                    templateBuilder.Append("						{\r\n");
                    templateBuilder.Append("							var c = source.charAt(i);\r\n");
                    templateBuilder.Append("							if (c < ' ' || c > '~')\r\n");
                    templateBuilder.Append("							{\r\n");
                    templateBuilder.Append("								c = '&#' + c.charCodeAt() + ';';\r\n");
                    templateBuilder.Append("							}\r\n");
                    templateBuilder.Append("							result += c;\r\n");
                    templateBuilder.Append("						}\r\n");
                    templateBuilder.Append("						return result;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					function format(source)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						// Use only integer part of tabs, and default to 4\r\n");
                    templateBuilder.Append("						tabs = (tabs >= 0) ? Math.floor(tabs) : 4;\r\n");
                    templateBuilder.Append("						// split along line breaks\r\n");
                    templateBuilder.Append("						var lines = source.split(/\\r\\n|\\r|\\n/);\r\n");
                    templateBuilder.Append("						// expand tabs\r\n");
                    templateBuilder.Append("						for (var i = 0; i < lines.length; i++)\r\n");
                    templateBuilder.Append("						{\r\n");
                    templateBuilder.Append("							var line = lines[i];\r\n");
                    templateBuilder.Append("							var newLine = '';\r\n");
                    templateBuilder.Append("							for (var p = 0; p < line.length; p++)\r\n");
                    templateBuilder.Append("							{\r\n");
                    templateBuilder.Append("								var c = line.charAt(p);\r\n");
                    templateBuilder.Append("								if (c === '\\t')\r\n");
                    templateBuilder.Append("								{\r\n");
                    templateBuilder.Append("									var spaces = tabs - (newLine.length % tabs);\r\n");
                    templateBuilder.Append("									for (var s = 0; s < spaces; s++)\r\n");
                    templateBuilder.Append("									{\r\n");
                    templateBuilder.Append("										newLine += ' ';\r\n");
                    templateBuilder.Append("									}\r\n");
                    templateBuilder.Append("								}\r\n");
                    templateBuilder.Append("								else\r\n");
                    templateBuilder.Append("								{\r\n");
                    templateBuilder.Append("									newLine += c;\r\n");
                    templateBuilder.Append("								}\r\n");
                    templateBuilder.Append("							}\r\n");
                    templateBuilder.Append("							// If a line starts or ends with a space, it evaporates in html\r\n");
                    templateBuilder.Append("							// unless it's an nbsp.\r\n");
                    templateBuilder.Append("							newLine = newLine.replace(/(^ )|( $)/g, '&nbsp;');\r\n");
                    templateBuilder.Append("							lines[i] = newLine;\r\n");
                    templateBuilder.Append("						}\r\n");
                    templateBuilder.Append("						// re-join lines\r\n");
                    templateBuilder.Append("						var result = lines.join('<br />');\r\n");
                    templateBuilder.Append("						// break up contiguous blocks of spaces with non-breaking spaces\r\n");
                    templateBuilder.Append("						result = result.replace(/  /g, ' &nbsp;');\r\n");
                    templateBuilder.Append("						// tada!\r\n");
                    templateBuilder.Append("						return result;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					var result = source;\r\n");
                    templateBuilder.Append("					// ampersands (&)\r\n");
                    templateBuilder.Append("					result = result.replace(/\\&/g,'&amp;');\r\n");
                    templateBuilder.Append("					// less-thans (<)\r\n");
                    templateBuilder.Append("					result = result.replace(/\\</g,'&lt;');\r\n");
                    templateBuilder.Append("					// greater-thans (>)\r\n");
                    templateBuilder.Append("					result = result.replace(/\\>/g,'&gt;');\r\n");
                    templateBuilder.Append("					if (display)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						// format for display\r\n");
                    templateBuilder.Append("						result = format(result);\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					else\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						// Replace quotes if it isn't for display,\r\n");
                    templateBuilder.Append("						// since it's probably going in an html attribute.\r\n");
                    templateBuilder.Append("						result = result.replace(new RegExp('\"','g'), '&quot;');\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					// special characters\r\n");
                    templateBuilder.Append("					result = special(result);\r\n");
                    templateBuilder.Append("					// tada!\r\n");
                    templateBuilder.Append("					return result;\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				var profile_username_toolong = '<img src=\\'templates/" + templatepath.ToString() + "/images/check_error.gif\\'/>对不起，您的用户名超过 20 个字符，请输入一个较短的用户名。';\r\n");
                    templateBuilder.Append("				var profile_username_tooshort = '<img src=\\'templates/" + templatepath.ToString() + "/images/check_error.gif\\'/>对不起，您输入的用户名小于3个字符, 请输入一个较长的用户名。';\r\n");
                    templateBuilder.Append("				var profile_username_pass = \"<img src='templates/" + templatepath.ToString() + "/images/check_right.gif'/>\";\r\n");
                    templateBuilder.Append("				function checkusername(username)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					var unlen = username.replace(/[^\\x00-\\xff]/g, \"**\").length;\r\n");
                    templateBuilder.Append("					if(unlen < 3 || unlen > 20) {\r\n");
                    templateBuilder.Append("						$(\"checkresult\").innerHTML =(unlen < 3 ? profile_username_tooshort : profile_username_toolong);\r\n");
                    templateBuilder.Append("						return;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					ajaxRead(\"tools/ajax.aspx?t=checkusername&username=\" + escape(username), \"showcheckresult(obj,'\" + username + \"');\");\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function showcheckresult(obj, username)\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					var res = obj.getElementsByTagName('result');\r\n");
                    templateBuilder.Append("					var resContainer = $(\"checkresult\");\r\n");
                    templateBuilder.Append("					var result = \"\";\r\n");
                    templateBuilder.Append("					if (res[0] != null && res[0] != undefined)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						if (res[0].childNodes.length > 1) {\r\n");
                    templateBuilder.Append("							result = res[0].childNodes[1].nodeValue;\r\n");
                    templateBuilder.Append("						} else {\r\n");
                    templateBuilder.Append("							result = res[0].firstChild.nodeValue;    		\r\n");
                    templateBuilder.Append("						}\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					if (result == \"1\")\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						resContainer.innerHTML = \"<img src=\\'templates/" + templatepath.ToString() + "/images/check_error.gif\\'/><font color='#009900'>对不起，您输入的用户名 \\\"\" + htmlEncode(username, true, 4) + \"\\\" 已经被他人使用或被禁用，请选择其他名字后再试。</font>\";\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					else\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						resContainer.innerHTML = profile_username_pass;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function checkSetting()\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					if ($('receiveuser').checked)\r\n");
                    templateBuilder.Append("					{\r\n");
                    templateBuilder.Append("						$('showhint').disabled = false;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("					else\r\n");
                    templateBuilder.Append("					{			\r\n");
                    templateBuilder.Append("						$('showhint').checked = false;\r\n");
                    templateBuilder.Append("						$('showhint').disabled = true;\r\n");
                    templateBuilder.Append("					}\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				function checkdoublepassword()\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("				  var pw1=$('password').value;\r\n");
                    templateBuilder.Append("				  var pw2=$('password2').value;\r\n");
                    templateBuilder.Append("				  if(pw1=='' &&  pw2=='')\r\n");
                    templateBuilder.Append("				  {\r\n");
                    templateBuilder.Append("				  return;\r\n");
                    templateBuilder.Append("				  }\r\n");
                    templateBuilder.Append("				  var str;\r\n");
                    templateBuilder.Append("				 if(pw1!=pw2)\r\n");
                    templateBuilder.Append("				 {\r\n");
                    templateBuilder.Append("				str =\"<img src='templates/" + templatepath.ToString() + "/images/check_error.gif'/>两次输入的密码不一致\";\r\n");
                    templateBuilder.Append("				 }\r\n");
                    templateBuilder.Append("				 else\r\n");
                    templateBuilder.Append("				 {\r\n");
                    templateBuilder.Append("				 str=\"<img src='templates/" + templatepath.ToString() + "/images/check_right.gif'/>\";\r\n");
                    templateBuilder.Append("				 }\r\n");
                    templateBuilder.Append("				$('password2tips').innerHTML=str;\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("			</" + "script>\r\n");
                    templateBuilder.Append("			<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                    templateBuilder.Append("	<strong id=\"showmsg\"></strong></td></tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");
                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"password2\">确认密码 * </label></th>\r\n");
                    templateBuilder.Append("		<td><input name=\"password2\" type=\"password\" id=\"password2\" size=\"20\" onblur=\"checkdoublepassword()\"/><span id=\"password2tips\"></span></td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");
                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"email\">Email * </label></th>\r\n");
                    templateBuilder.Append("		<td><input name=\"email\" type=\"text\" id=\"email\" size=\"20\"  onblur=\"checkemail(this.value)\"/><span id=\"isemail\"></span></td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");

                    if (config.Realnamesystem == 1)
                    {


                        templateBuilder.Append("<tbody>\r\n");
                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"realname\">真实姓名 * </label></th>\r\n");
                        templateBuilder.Append("		<td><input name=\"realname\" type=\"text\" id=\"realname\" size=\"10\" /></td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");
                        templateBuilder.Append("</tbody>\r\n");
                        //templateBuilder.Append("<tbody>\r\n");
                        //templateBuilder.Append("	<tr>\r\n");
                        //templateBuilder.Append("		<th><label for=\"idcard\">身份证号码</label></th>\r\n");
                        //templateBuilder.Append("		<td><input name=\"idcard\" type=\"text\" id=\"idcard\" size=\"20\" /></td>\r\n");
                        //templateBuilder.Append("	</tr>\r\n");
                        //templateBuilder.Append("</tbody>\r\n");
                        templateBuilder.Append("<tbody>\r\n");
                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"mobile\">移动电话号码</label></th>\r\n");
                        templateBuilder.Append("		<td><input name=\"mobile\" type=\"text\" id=\"mobile\" size=\"20\" /></td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");
                        templateBuilder.Append("</tbody>\r\n");
                        templateBuilder.Append("<tbody>\r\n");
                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"phone\">固定电话号码</label></th>\r\n");
                        templateBuilder.Append("		<td><input name=\"phone\" type=\"text\" id=\"phone\" size=\"20\" /></td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");
                        templateBuilder.Append("</tbody>\r\n");



                    }	//end if


                    if (isseccode)
                    {

                        templateBuilder.Append("	<tbody>\r\n");
                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"vcode\">验证码 * </label></th>\r\n");
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


                        templateBuilder.Append("</td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");
                        templateBuilder.Append("	</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("		<td><input name=\"submit\" type=\"submit\" value=\"创建用户\"/>  <input type=\"button\" onclick=\"javascript:window.location.replace('./index.aspx')\" value=\"取消\"/></td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");
                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>\r\n");

                    if (config.Regadvance == 1)
                    {

                        templateBuilder.Append("<div class=\"mainbox formbox\">\r\n");
                        templateBuilder.Append("	<span class=\"headactions\">\r\n");
                        templateBuilder.Append("		<a href=\"###\" onclick=\"toggle_collapse('regoptions');\"><img id= \"regoptions_img\" src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\" alt=\"展开\" /></a>\r\n");
                        templateBuilder.Append("	</span>\r\n");
                        templateBuilder.Append("	<h1>填写可选项</h1>\r\n");
                        templateBuilder.Append("	<table summary=\"注册 高级选项\" cellspacing=\"0\" cellpadding=\"0\" id=\"regoptions\" style=\"display: none;\">\r\n");
                        templateBuilder.Append("		<thead>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>找回密码问题</th>\r\n");
                        templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</thead>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"question\">问题</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"question\" id=\"question\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">无</option>\r\n");
                        templateBuilder.Append("				  <option value=\"1\">母亲的名字</option>\r\n");
                        templateBuilder.Append("				  <option value=\"2\">爷爷的名字</option>\r\n");
                        templateBuilder.Append("				  <option value=\"3\">父亲出生的城市</option>\r\n");
                        templateBuilder.Append("				  <option value=\"4\">您其中一位老师的名字</option>\r\n");
                        templateBuilder.Append("				  <option value=\"5\">您个人计算机的型号</option>\r\n");
                        templateBuilder.Append("				  <option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
                        templateBuilder.Append("				  <option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>		\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"answer\">答案</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"answer\" type=\"text\" id=\"answer\" size=\"30\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<thead>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>个人信息</th>\r\n");
                        templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</thead>\r\n");

                        if (config.Realnamesystem == 1)
                        {


                            templateBuilder.Append("<tbody>\r\n");
                            templateBuilder.Append("	<tr>\r\n");
                            templateBuilder.Append("		<th><label for=\"realname\">真实姓名</label></th>\r\n");
                            templateBuilder.Append("		<td><input name=\"realname\" type=\"text\" id=\"realname\" size=\"10\" /></td>\r\n");
                            templateBuilder.Append("	</tr>\r\n");
                            templateBuilder.Append("</tbody>\r\n");
                            templateBuilder.Append("<tbody>\r\n");
                            templateBuilder.Append("	<tr>\r\n");
                            templateBuilder.Append("		<th><label for=\"idcard\">身份证号码</label></th>\r\n");
                            templateBuilder.Append("		<td><input name=\"idcard\" type=\"text\" id=\"idcard\" size=\"20\" /></td>\r\n");
                            templateBuilder.Append("	</tr>\r\n");
                            templateBuilder.Append("</tbody>\r\n");
                            templateBuilder.Append("<tbody>\r\n");
                            templateBuilder.Append("	<tr>\r\n");
                            templateBuilder.Append("		<th><label for=\"mobile\">移动电话号码</label></th>\r\n");
                            templateBuilder.Append("		<td><input name=\"mobile\" type=\"text\" id=\"mobile\" size=\"20\" /></td>\r\n");
                            templateBuilder.Append("	</tr>\r\n");
                            templateBuilder.Append("</tbody>\r\n");
                            templateBuilder.Append("<tbody>\r\n");
                            templateBuilder.Append("	<tr>\r\n");
                            templateBuilder.Append("		<th><label for=\"phone\">固定电话号码</label></th>\r\n");
                            templateBuilder.Append("		<td><input name=\"phone\" type=\"text\" id=\"phone\" size=\"20\" /></td>\r\n");
                            templateBuilder.Append("	</tr>\r\n");
                            templateBuilder.Append("</tbody>\r\n");



                        }	//end if

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"gender\">性别</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"gender\" value=\"1\" style=\"InPutRadio\"/>男\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"gender\" value=\"2\"  style=\"InPutRadio\"/>女\r\n");
                        templateBuilder.Append("				<input name=\"gender\" type=\"radio\" value=\"0\" checked=\"checked\"  style=\"InPutRadio\"/>保密\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"nickname\">昵称</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"nickname\" type=\"text\" id=\"nickname\" size=\"20\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>	\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"bday\">生日</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"bday_y\" type=\"text\" id=\"bday_y\" size=\"4\" maxlength=\"4\" />年\r\n");
                        templateBuilder.Append("				<input name=\"bday_m\" type=\"text\" id=\"bday_m\" size=\"2\"  maxlength=\"2\"/>月\r\n");
                        templateBuilder.Append("				<input name=\"bday_d\" type=\"text\" id=\"bday_d\" size=\"2\"  maxlength=\"2\"/>日\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"location\">来自</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"location\" type=\"text\" id=\"location\" size=\"20\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"msn\">MSN Messenger</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"msn\" type=\"text\" id=\"msn\" size=\"30\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"yahoo\">Yahoo Messenger</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"yahoo\" type=\"text\" id=\"yahoo\" size=\"30\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"skype\">Skype</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"skype\" type=\"text\" id=\"skype\" size=\"30\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"ICQ\">ICQ</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"icq\" type=\"text\" id=\"icq\" size=\"12\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"qq\">QQ</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"qq\" type=\"text\" id=\"qq\" size=\"12\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"homepage\">主页</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"homepage\" type=\"text\" id=\"homepage\" size=\"30\" />\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"bio\">自我介绍</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<textarea name=\"bio\" cols=\"50\" rows=\"6\" id=\"bio\" style=\"height:95px;width:85%;\"></textarea>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<thead>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>论坛使用喜好设置:</th>\r\n");
                        templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</thead>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"templateid\">风格</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"templateid\" id=\"templateid\" >\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected>默认</option>\r\n");

                        int template__loop__id = 0;
                        foreach (DataRow template in templatelist.Rows)
                        {
                            template__loop__id++;

                            templateBuilder.Append("					<option value=\"" + template["templateid"].ToString().Trim() + "\">" + template["name"].ToString().Trim() + "</option>\r\n");

                        }	//end loop

                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"tpp\">每页主题数</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"tpp\" id=\"tpp\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">默认</option>\r\n");
                        templateBuilder.Append("				  <option value=\"15\">15</option>\r\n");
                        templateBuilder.Append("				  <option value=\"20\">20</option>\r\n");
                        templateBuilder.Append("				  <option value=\"25\">25</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"ppp\">每页帖子数</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"ppp\" id=\"ppp\">\r\n");
                        templateBuilder.Append("				  <option value=\"0\" selected=\"selected\">默认</option>\r\n");
                        templateBuilder.Append("				  <option value=\"10\">10</option>\r\n");
                        templateBuilder.Append("				  <option value=\"15\">15</option>\r\n");
                        templateBuilder.Append("				  <option value=\"20\">20</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"newpm\">是否提示短消息</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"newpm\" type=\"radio\" value=\"radiobutton\" checked=\"checked\"  style=\"InPutRadio\"/>是\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"newpm\" value=\"radiobutton\"  style=\"InPutRadio\"/>否\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"pmsound\">短消息铃声</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<select name=\"pmsound\" id=\"pmsound\">\r\n");
                        templateBuilder.Append("				  <option value=\"1\" selected=\"selected\">默认</option>							  \r\n");
                        templateBuilder.Append("				  <option value=\"1\">提示音1</option>							  \r\n");
                        templateBuilder.Append("				  <option value=\"2\">提示音2</option>							  \r\n");
                        templateBuilder.Append("				  <option value=\"3\">提示音3</option>		\r\n");
                        templateBuilder.Append("				  <option value=\"4\">提示音4</option>		\r\n");
                        templateBuilder.Append("				  <option value=\"5\">提示音5</option>							  \r\n");
                        templateBuilder.Append("				  <option value=\"0\">无</option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"showemail\">是否显示Email</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"showemail\" type=\"radio\" value=\"1\" checked=\"checked\"  style=\"InPutRadio\"/>是\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"showemail\" value=\"0\"  style=\"InPutRadio\"/>否\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"receivesetting\">消息接收设置</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input id=\"receiveuser\" onclick=\"checkSetting();\" type=\"checkbox\" name=\"receivesetting\" value=\"2\" checked=\"checked\" />接收用户短消息\r\n");
                        templateBuilder.Append("				<input id=\"showhint\" onclick=\"checkSetting();\" type=\"checkbox\" name=\"receivesetting\" value=\"4\" checked=\"checked\" />显示短消息提示框\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"invisible\">是否隐身</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input type=\"radio\" name=\"invisible\" value=\"1\"  style=\"InPutRadio\"/>是\r\n");
                        templateBuilder.Append("				<input name=\"invisible\" type=\"radio\" value=\"0\" checked=\"checked\"  style=\"InPutRadio\"/>否\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th><label for=\"signature\">签名</label></th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<textarea name=\"signature\" cols=\"50\" rows=\"6\" id=\"signature\" style=\"height:95px;width:85%;\"></textarea>\r\n");
                        templateBuilder.Append("				<input name=\"sigstatus\" type=\"checkbox\" id=\"sigstatus\" value=\"1\" checked=\"checked\" />使用签名\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<input name=\"submit\" type=\"submit\" value=\"创建用户\"/>  <input type=\"button\" onclick=\"javascript:window.location.replace('./index.aspx')\" value=\"取消\"/></td>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }	//end if

                    templateBuilder.Append("</form>\r\n");
                    templateBuilder.Append("<!--reg end-->\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</div>\r\n");

                }
                else
                {


                    if (createuser != "")
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

                        templateBuilder.Append("	</div>\r\n");

                    }	//end if


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