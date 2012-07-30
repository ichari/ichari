using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
using System.Text.RegularExpressions;

namespace Discuz.Web
{
    /// <summary>
    /// 更新用户档案页面
    /// </summary>
    public partial class usercpprofile : PageBase
    {
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";

            if (userid == -1)
            {
                AddErrLine("你尚未登录");

                return;
            }
            user = Discuz.Forum.Users.GetUserInfo(userid);

            if (DNTRequest.IsPost())
            {
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
                //实名验证
                if (config.Realnamesystem == 0)
                {
                    string idcard = DNTRequest.GetString("idcard").Trim().Replace(",", "");
                    if (DNTRequest.GetString("realname").Trim() == "")
                    {
                        AddErrLine("真实姓名不能为空");
                    }
                    else if (DNTRequest.GetString("realname").Trim().Length > 10)
                    {
                        AddErrLine("真实姓名不能大于10个字符");
                    }
                    if (idcard == "")
                    {
                        AddErrLine("身份证号码不能为空");
                    }
                    else if (idcard.Length > 20)
                    {
                        AddErrLine("身份证号码不能大于20个字符");
                    }


                    //Regex reTai = new Regex(@"[A-Za-z][12]\d{8}");
                    //Regex reXiang = new Regex(@"[A-Za-z]{1,2}\d{6}\(([0-9]|[Aa])\)");
                    //Regex reXin = new Regex(@"\d{7}[A-JZa-jz]");
                    //Regex reAo = new Regex(@"\d{7}\([0-9]\)");
                    //Regex reDaLu = new Regex(@"\d{17}[0-9Xx]|\d{15}");

                    if (!Shove._String.Valid.isIDCardNumber(idcard) && !Shove._String.Valid.isIDCardNumber_Hongkong(idcard) && !Shove._String.Valid.isIDCardNumber_Macau(idcard) && !Shove._String.Valid.isIDCardNumber_Singapore(idcard) && !Shove._String.Valid.isIDCardNumber_Taiwan(idcard))
                    {
                        AddErrLine("身份证号码格式不正确");
                        return;
                    }
                    if (DNTRequest.GetString("mobile").Trim() == "" && DNTRequest.GetString("phone").Trim() == "")
                    {
                        AddErrLine("移动电话号码和是固定电话号码必须填写其中一项");
                    }
                    if (DNTRequest.GetString("mobile").Trim().Length > 20)
                    {
                        AddErrLine("移动电话号码不能大于20个字符");
                    }
                    if (DNTRequest.GetString("phone").Trim().Length > 20)
                    {
                        AddErrLine("固定电话号码不能大于20个字符");
                    }
                }


                //if (DNTRequest.GetString("idcard").Trim() != "" &&
                //    !Regex.IsMatch(DNTRequest.GetString("idcard").Trim(), @"^[\x20-\x80]+$"))
                //{
                //    AddErrLine("身份证号码中含有非法字符");
                //}

                //if (DNTRequest.GetString("mobile").Trim() != "" &&
                //    !Regex.IsMatch(DNTRequest.GetString("mobile").Trim(), @"^[\d|-]+$"))
                //{
                //    AddErrLine("移动电话号码中含有非法字符");
                //}

                //if (DNTRequest.GetString("phone").Trim() != "" &&
                //    !Regex.IsMatch(DNTRequest.GetString("phone").Trim(), @"^[\d|-]+$"))
                //{
                //    AddErrLine("固定电话号码中含有非法字符");
                //}

                //
                //string email = DNTRequest.GetString("email").Trim().ToLower();

                //if (email.Equals(""))
                //{
                //    AddErrLine("Email不能为空");
                //    return;
                //}

                //else if (!Utils.IsValidEmail(email))
                //{
                //    AddErrLine("Email格式不正确");
                //    return;
                //}
                //else
                //{
                //    int tmpUserid = Discuz.Forum.Users.FindUserEmail(email);
                //    if (tmpUserid != userid && tmpUserid != -1 && config.Doublee == 0)
                //    {
                //        AddErrLine("Email: \"" + email + "\" 已经被其它用户注册使用");
                //        return;
                //    }

                //    string emailhost = Utils.GetEmailHostName(email);
                //    // 允许名单规则优先于禁止名单规则
                //    if (config.Accessemail.Trim() != "")
                //    {
                //        // 如果email后缀 不属于 允许名单
                //        if (!Utils.InArray(emailhost, config.Accessemail.Replace("\r\n", "\n"), "\n"))
                //        {
                //            AddErrLine("Email: \"" + email + "\" 不在本论坛允许范围之类, 本论坛只允许用户使用这些Email地址注册: " +
                //                       config.Accessemail.Replace("\n", ",&nbsp;"));
                //            return;
                //        }
                //    }
                //    else if (config.Censoremail.Trim() != "")
                //    {
                //        // 如果email后缀 属于 禁止名单
                //        if (Utils.InArray(emailhost, config.Censoremail.Replace("\r\n", "\n"), "\n"))
                //        {
                //            AddErrLine("Email: \"" + email + "\" 不允许在本论坛使用, 本论坛不允许用户使用的Email地址包括: " +
                //                       config.Censoremail.Replace("\n", ",&nbsp;"));
                //            return;
                //        }
                //    }
                //    if (DNTRequest.GetString("bio").Length > 500)
                //    {
                //        //如果自我介绍超过500...
                //        AddErrLine("自我介绍不得超过500个字符");
                //        return;
                //    }
                //    if (DNTRequest.GetString("signature").Length > 500)
                //    {
                //        //如果签名超过500...
                //        AddErrLine("签名不得超过500个字符");
                //        return;
                //    }
                //}

                if (page_err == 0)
                {
                    UserInfo __userinfo = new UserInfo();
                    __userinfo.Uid = userid;
                    __userinfo.Nickname = Utils.HtmlEncode(DNTRequest.GetString("nickname"));
                    __userinfo.Gender = DNTRequest.GetInt("gender", 0);
                    __userinfo.Realname = DNTRequest.GetString("realname");
                    __userinfo.Idcard = DNTRequest.GetString("idcard");
                    __userinfo.Mobile = DNTRequest.GetString("mobile");
                    __userinfo.Phone = DNTRequest.GetString("phone");
                    //__userinfo.Email = email;
                    __userinfo.Bday = Utils.HtmlEncode(DNTRequest.GetString("bday"));
                    __userinfo.Showemail = DNTRequest.GetInt("showemail", 1);
                    if (DNTRequest.GetString("website").IndexOf(".") > -1 &&
                        !DNTRequest.GetString("website").ToLower().StartsWith("http"))
                    {
                        __userinfo.Website = Utils.HtmlEncode("http://" + DNTRequest.GetString("website"));
                    }
                    else
                    {
                        __userinfo.Website = Utils.HtmlEncode(DNTRequest.GetString("website"));
                    }
                    __userinfo.Icq = Utils.HtmlEncode(DNTRequest.GetString("icq"));
                    __userinfo.Qq = Utils.HtmlEncode(DNTRequest.GetString("qq"));
                    __userinfo.Yahoo = Utils.HtmlEncode(DNTRequest.GetString("yahoo"));
                    __userinfo.Msn = Utils.HtmlEncode(DNTRequest.GetString("msn"));
                    __userinfo.Skype = Utils.HtmlEncode(DNTRequest.GetString("skype"));
                    __userinfo.Location = Utils.HtmlEncode(DNTRequest.GetString("location"));
                    __userinfo.Bio = Utils.HtmlEncode(DNTRequest.GetString("bio"));

                    Discuz.Forum.Users.UpdateUserProfile(__userinfo);
                    SetUrl("usercpprofile.aspx");
                    SetMetaRefresh();
                    SetShowBackLink(true);
                    AddMsgLine("修改个人档案完毕");
                }
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:24.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:24. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>编辑个人档案</strong>\r\n");
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

                templateBuilder.Append("					<a href=\"usercpprofile.aspx\"\r\n");

                if (pagename == "usercpprofile.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">编辑个人档案</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpnewpassword.aspx\"\r\n");

                if (pagename == "usercpnewpassword.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">更改密码</a>\r\n");
                templateBuilder.Append("					 <a href=\"usercppreference.aspx\"\r\n");

                if (pagename == "usercppreference.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">个性设置</a>\r\n");

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

                    templateBuilder.Append("						<form action=\"\" method=\"post\" ID=\"Form1\">\r\n");
                    templateBuilder.Append("						<label for=\"realname\">真实姓名:</label>\r\n");
                    templateBuilder.Append("							<input name=\"realname\" type=\"text\" id=\"realname\" value=\"" + user.Realname.ToString().Trim() + "\" size=\"10\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"idcard\">身份证号码:</label>\r\n");
                    templateBuilder.Append("							<input name=\"idcard\" type=\"text\" id=\"idcard\" value=\"" + user.Idcard.ToString().Trim() + "\" size=\"20\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"mobile\">移动电话号码:</label>\r\n");
                    templateBuilder.Append("							<input name=\"mobile\" type=\"text\" id=\"mobile\" value=\"" + user.Mobile.ToString().Trim() + "\" size=\"20\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"phone\">固定电话号码:</label>\r\n");
                    templateBuilder.Append("							<input name=\"phone\" type=\"text\" id=\"phone\" value=\"" + user.Phone.ToString().Trim() + "\" size=\"20\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<div style=\"line-height:180%;\">\r\n");
                    templateBuilder.Append("						<label for=\"gender\">性别:</label>\r\n");
                    templateBuilder.Append("							<input type=\"radio\" name=\"gender\" value=\"1\"  class=\"radioinput\" \r\n");

                    if (user.Gender == 1)
                    {

                        templateBuilder.Append("							 checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("							 ID=\"Radio1\"/>\r\n");
                    templateBuilder.Append("							男\r\n");
                    templateBuilder.Append("							<input type=\"radio\" name=\"gender\" value=\"2\"  class=\"radioinput\" \r\n");

                    if (user.Gender == 2)
                    {

                        templateBuilder.Append("							 checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("							 ID=\"Radio2\"/>\r\n");
                    templateBuilder.Append("							女\r\n");
                    templateBuilder.Append("							<input name=\"gender\" type=\"radio\" value=\"0\"   class=\"radioinput\" \r\n");

                    if (user.Gender == 0)
                    {

                        templateBuilder.Append("							 checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("							 ID=\"Radio3\"/>\r\n");
                    templateBuilder.Append("							保密 \r\n");
                    templateBuilder.Append("						</div>\r\n");
                    templateBuilder.Append("						<br style=\"height:1px;line-height:5px;\"/>\r\n");
                    templateBuilder.Append("						<label for=\"nickname\">呢称:</label>\r\n");
                    templateBuilder.Append("						<input name=\"nickname\" type=\"text\" id=\"nickname\" value=\"" + user.Nickname.ToString().Trim() + "\" size=\"30\" /><br />\r\n");
                    //templateBuilder.Append("						<label for=\"EMail\">EMail:</label>\r\n");
                    //templateBuilder.Append("							<input name=\"email\" type=\"text\" id=\"email\" value=\"" + user.Email.ToString().Trim() + "\" size=\"30\"/> \r\n");
                    //templateBuilder.Append("							<input name=\"showemail\" type=\"checkbox\" id=\"showemail\" value=\"0\" \r\n");

                    //if (user.Showemail == 0)
                    //{

                    //    templateBuilder.Append("									checked=\"checked\"\r\n");

                    //}	//end if

                    //templateBuilder.Append("/>\r\n");
                    //templateBuilder.Append("									Email保密\r\n");
                    //templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"website\">主页:</label>\r\n");
                    templateBuilder.Append("						<input name=\"website\" type=\"text\" class=\"colorblue\" id=\"website\" value=\"" + user.Website.ToString().Trim() + "\" size=\"30\" /> \r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"location\">来自:</label>\r\n");
                    templateBuilder.Append("						<input name=\"location\" type=\"text\" id=\"location\" class=\"colorblue\" value=\"" + user.Location.ToString().Trim() + "\" size=\"20\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"bday\">生日:</label>\r\n");
                    templateBuilder.Append("						<input name=\"bday\" type=\"text\" id=\"bday\" size=\"10\" value=\"" + Shove._Convert.StrToDateTime(user.Bday.ToString().Trim(),"1980-01-01").ToString("yyyy-MM-dd") + "\" style=\"cursor:default\" onClick=\"showcalendar(event, 'bday', 'cal_startdate', 'cal_enddate', '1980-01-01');\" readonly=\"readonly\" />&nbsp;<button onclick=\"$('bday').value='';\">清空</button>\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" size=\"10\"  value=\"1900-01-01\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" size=\"10\"  value=\"" + nowdatetime.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"msn\">MSN Messenger:</label>\r\n");
                    templateBuilder.Append("						<input name=\"msn\" type=\"text\" id=\"msn\" value=\"" + user.Msn.ToString().Trim() + "\" size=\"30\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"qq\">QQ:</label>\r\n");
                    templateBuilder.Append("						<input name=\"qq\" type=\"text\" id=\"qq\" value=\"" + user.Qq.ToString().Trim() + "\" size=\"12\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"Skype\">Skype:</label>\r\n");
                    templateBuilder.Append("						<input name=\"skype\" type=\"text\" id=\"skype\" value=\"" + user.Skype.ToString().Trim() + "\" size=\"30\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"ICQ\">ICQ:</label>\r\n");
                    templateBuilder.Append("						<input name=\"icq\" type=\"text\" id=\"icq\" value=\"" + user.Icq.ToString().Trim() + "\" size=\"12\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"yahoo\">Yahoo Messenger:</label>\r\n");
                    templateBuilder.Append("						<input name=\"yahoo\" type=\"text\" class=\"colorblue\" id=\"yahoo\" value=\"" + user.Yahoo.ToString().Trim() + "\" size=\"30\" />\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"bio\">自我介绍:</label>\r\n");
                    templateBuilder.Append("						<textarea name=\"bio\" cols=\"50\" rows=\"6\" id=\"bio\" style=\"width:80%;\">" + user.Bio.ToString().Trim() + "</textarea>\r\n");
                    templateBuilder.Append("						<br />\r\n");
                    templateBuilder.Append("						<label for=\"Submit\">&nbsp;</label><input type=\"submit\" name=\"Submit\" value=\"确定\" onclick=\"if (document.getElementById('bio').value.length > 500) {alert('自我介绍长度最大为500字'); return false;}\" ID=\"Submit1\"/>\r\n");
                    templateBuilder.Append("								</form>\r\n");

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
            templateBuilder.Append("<!--主体-->\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_calendar.js\"></" + "script>\r\n");


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