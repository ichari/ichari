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
	/// 论坛设置
	/// </summary>
	public partial class usercpforumsetting : PageBase
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

			if(DNTRequest.IsPost())
			{
                SetBackLink("usercpforumsetting.aspx");
                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }
				UserInfo __userinfo = new UserInfo();
				__userinfo.Uid = userid;
				__userinfo.Tpp = DNTRequest.GetInt("tpp", 0);
				__userinfo.Ppp = DNTRequest.GetInt("ppp", 0);
				__userinfo.Pmsound = DNTRequest.GetInt("pmsound", 0);
				__userinfo.Invisible = DNTRequest.GetInt("invisible", 0);
                __userinfo.Customstatus = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("customstatus")));
                //获取提交的内容并进行脏字和Html处理
                string signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("signature")));

                int sigstatus = DNTRequest.GetInt("sigstatus", 0);
                //错误参数值纠正
                if (sigstatus != 0)
                {
                    sigstatus = 1;
                }

                PostpramsInfo _postpramsinfo = new PostpramsInfo();
                _postpramsinfo.Usergroupid = usergroupid;
                _postpramsinfo.Attachimgpost = config.Attachimgpost;
                _postpramsinfo.Showattachmentpath = config.Showattachmentpath;
                _postpramsinfo.Hide = 0;
                _postpramsinfo.Price = 0;
                _postpramsinfo.Sdetail = signature;
                _postpramsinfo.Smileyoff = 1;
                _postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
                _postpramsinfo.Parseurloff = 1;
                _postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
                _postpramsinfo.Allowhtml = 0;
                _postpramsinfo.Signature = 1;
                _postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                _postpramsinfo.Customeditorbuttoninfo = null;//Editors.GetCustomEditButtonListWithInfo();
                _postpramsinfo.Smiliesmax = config.Smiliesmax;
                _postpramsinfo.Signature = 1;


                string sightml = UBB.UBBToHTML(_postpramsinfo);

                //if (usergroupinfo.Maxsigsize<Utils.ClearHtml(sightml).Length)
                if (DNTRequest.GetString("signature").Length > usergroupinfo.Maxsigsize)
                {
                    AddErrLine(string.Format("您的签名长度超过 {0} 字符的限制，请返回修改。", usergroupinfo.Maxsigsize));
                    return;
                }

                if (sightml.Length >= 1000)
                {
                    AddErrLine("您的签名转换后超出系统最大长度， 请返回修改");
                    return;
                }

                __userinfo.Sigstatus = sigstatus;
                __userinfo.Signature = signature;
                __userinfo.Sightml = sightml;

                Discuz.Forum.Users.UpdateUserForumSetting(__userinfo);
				OnlineUsers.UpdateInvisible(olid, __userinfo.Invisible);

                ForumUtils.WriteCookie("sigstatus", sigstatus);
				ForumUtils.WriteCookie("tpp", __userinfo.Tpp.ToString());
				ForumUtils.WriteCookie("ppp", __userinfo.Ppp.ToString());
				ForumUtils.WriteCookie("pmsound", __userinfo.Pmsound.ToString());

				SetUrl("usercpforumsetting.aspx");
				SetMetaRefresh();
				SetShowBackLink(true);
				AddMsgLine("修改论坛设置完毕");

			}
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:58.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:58. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<!--header end-->\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>论坛设置</strong>\r\n");
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

            templateBuilder.Append("                <div class=\"panneltabs\">\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("						  <a href=\"usercpforumsetting.aspx\"\r\n");

                if (pagename == "usercpforumsetting.aspx")
                {

                    templateBuilder.Append("						 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">论坛设置</a>\r\n");

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
                    templateBuilder.Append("							<label for=\"tpp\"  class=\"labellong2\" style=\"line-height:180%;\">每页主题数:</label>\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"0\" name=\"tpp\"\r\n");

                    if (user.Tpp == 0)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>默认&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"15\" name=\"tpp\"\r\n");

                    if (user.Tpp == 15)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>15&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"20\" name=\"tpp\"\r\n");

                    if (user.Tpp == 20)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>20&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"25\" name=\"tpp\"\r\n");

                    if (user.Tpp == 25)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>25&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<div class=\"compartline\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<label for=\"ppp\"  class=\"labellong2\" style=\"line-height:180%;\">每页帖子数:</label>\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"0\" name=\"ppp\"\r\n");

                    if (user.Ppp == 0)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>默认&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"10\" name=\"ppp\"\r\n");

                    if (user.Ppp == 10)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>10&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"15\" name=\"ppp\"\r\n");

                    if (user.Ppp == 15)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>15&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<input type=\"radio\" value=\"20\" name=\"ppp\"\r\n");

                    if (user.Ppp == 20)
                    {

                        templateBuilder.Append("							  checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("/>20&nbsp;&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<div class=\"compartline\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<label for=\"invisible\" class=\"labellong2\" style=\"line-height: 180%;\">是否隐身:</label>\r\n");
                    templateBuilder.Append("							<input type=\"radio\" name=\"invisible\" value=\"1\" class=\"radioinput\" \r\n");

                    if (user.Invisible == 1)
                    {

                        templateBuilder.Append("								checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("								 ID=\"Radio3\"/>是\r\n");
                    templateBuilder.Append("							<input name=\"invisible\" type=\"radio\" value=\"0\" class=\"radioinput\"  \r\n");

                    if (user.Invisible == 0)
                    {

                        templateBuilder.Append("								checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("								 ID=\"Radio4\"/>否\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<div class=\"compartline\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<label for=\"signature\">签名:</label>\r\n");
                    templateBuilder.Append("							<textarea name=\"signature\" cols=\"60\" rows=\"8\" id=\"signature\">" + user.Signature.ToString().Trim() + "</textarea>\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<label for=\"sigstatus\">&nbsp;</label>\r\n");
                    templateBuilder.Append("							<input name=\"sigstatus\" type=\"checkbox\" id=\"sigstatus\" value=\"1\" \r\n");

                    if (user.Sigstatus == 1)
                    {

                        templateBuilder.Append("									checked=\"checked\"\r\n");

                    }	//end if

                    templateBuilder.Append("								/>使用签名\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<label for=\"newpassword2\">&nbsp;</label>\r\n");
                    templateBuilder.Append("							UBB代码: <strong>\r\n");

                    if (usergroupinfo.Allowsigbbcode == 1)
                    {

                        templateBuilder.Append("										可用\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("										禁用\r\n");

                    }	//end if

                    templateBuilder.Append("									</strong>&nbsp;&nbsp;\r\n");
                    templateBuilder.Append("									[img]代码: <strong>\r\n");

                    if (usergroupinfo.Allowsigimgcode == 1)
                    {

                        templateBuilder.Append("										可用\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("										禁用\r\n");

                    }	//end if

                    templateBuilder.Append("									</strong>\r\n");
                    templateBuilder.Append("							<br />\r\n");
                    templateBuilder.Append("							<div class=\"compartline\">&nbsp;</div>\r\n");
                    templateBuilder.Append("							<input type=\"submit\" name=\"Submit\" value=\"确定\" ID=\"Submit1\"/>\r\n");
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
