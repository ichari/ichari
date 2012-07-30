using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// 查看用户信息页
	/// </summary>
	public partial class userinfo : PageBase
	{
        /// <summary>
        /// 当前用户信息
        /// </summary>
		public UserInfo user;
        /// <summary>
        /// 当前用户用户组信息
        /// </summary>
		public UserGroupInfo group;
        /// <summary>
        /// 当前用户管理组信息
        /// </summary>
        public AdminGroupInfo admininfo;
        /// <summary>
        /// 可用的扩展金币名称列表
        /// </summary>
		public string[] score;

        /// <summary>
        /// 是否需要快速登录
        /// </summary>
        public bool needlogin = false;

		protected override void ShowPage()
		{
			pagetitle = "查看用户信息";
			
			if (usergroupinfo.Allowviewpro != 1)
			{
				AddErrLine(string.Format("您当前的身份 \"{0}\" 没有查看用户资料的权限", usergroupinfo.Grouptitle));
                if (userid < 1)
                    needlogin = true;
				return;
			}

			if (DNTRequest.GetString("username").Trim() == "" && DNTRequest.GetString("userid").Trim() == "")
			{
				AddErrLine("错误的URL链接");
				return;
			}

			int id = DNTRequest.GetInt("userid", -1);
			
			if (id == -1)
			{
				id = Discuz.Forum.Users.GetUserID(Utils.UrlDecode(DNTRequest.GetString("username")));
			}

			if (id == -1)
			{
				AddErrLine("该用户不存在");
				return;
			}

			user = Discuz.Forum.Users.GetUserInfo(id);
			if (user == null)
			{
				AddErrLine("该用户不存在");
				return;
			}

			//用户设定Email保密时，清空用户的Email属性以避免被显示
			if (user.Showemail != 1)
			{
				user.Email = "";
			}
			//获取金币机制和用户组信息，底层有缓存
            score = Scoresets.GetValidScoreName();
			group = UserGroups.GetUserGroupInfo(user.Groupid);
            admininfo = AdminUserGroups.AdminGetAdminGroupInfo(usergroupid);
            
		}

        override protected void OnLoad(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:13.
		本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:13. 
	*/

	base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>查看用户信息</strong>\r\n");
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
                templateBuilder.Append("<div class=\"mainbox viewthread specialthread\">\r\n");
                templateBuilder.Append("<table cellspacing=\"0\" cellpadding=\"0\" summary=\"辩论主题\">\r\n");
                templateBuilder.Append("	<tr>\r\n");
                templateBuilder.Append("	<td class=\"postcontent\">\r\n");
                templateBuilder.Append("		<h3>用户信息" + user.Username.ToString().Trim() + "</h3>\r\n");
                templateBuilder.Append("		<table cellspacing=\"0\" cellpadding=\"0\" summary=\"辩论主题\">\r\n");
                templateBuilder.Append("		<thead>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th class=\"infotitle\">论坛信息</th>\r\n");
                templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		</thead> \r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th class=\"usertitle\">用户名</th>\r\n");
                templateBuilder.Append("			<td class=\"navname\">" + user.Username.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>用户ID</th>\r\n");
                templateBuilder.Append("			<td>" + user.Uid.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>昵称</th>\r\n");
                templateBuilder.Append("			<td>" + user.Nickname.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>自定义头衔</th>\r\n");
                templateBuilder.Append("			<td>" + user.Customstatus.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>用户组</th>\r\n");
                templateBuilder.Append("			<td>" + group.Grouptitle.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>阅读权限</th>\r\n");
                templateBuilder.Append("			<td>" + group.Readaccess.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>		\r\n");
                //templateBuilder.Append("		<tr>\r\n");
                //templateBuilder.Append("			<th>金币</th>\r\n");
                //templateBuilder.Append("			<td>" + user.Credits.ToString().Trim() + "</td>\r\n");
                //templateBuilder.Append("		</tr>\r\n");

                if (score[1].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[1].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits1.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[2].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[2].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits2.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[3].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[3].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits3.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[4].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[4].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits4.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[5].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[5].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits5.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[6].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[6].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits6.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[7].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[7].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits7.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if


                if (score[8].ToString().Trim() != "")
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>" + score[8].ToString().Trim() + "</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Extcredits8.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if

                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>发帖量</th>\r\n");
                templateBuilder.Append("			<td>" + user.Posts.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>精华帖数</th>\r\n");
                templateBuilder.Append("			<td>" + user.Digestposts.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>在线时间</th>\r\n");
                templateBuilder.Append("			<td>" + user.Oltime.ToString().Trim() + "分</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th class=\"infotitle\">个人信息</th>\r\n");
                templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>来自</th>\r\n");
                templateBuilder.Append("			<td>" + user.Location.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>性别</th>\r\n");
                templateBuilder.Append("			<td>\r\n");

                if (user.Gender == 0)
                {

                    templateBuilder.Append("保密\r\n");

                }	//end if


                if (user.Gender == 1)
                {

                    templateBuilder.Append("男\r\n");

                }	//end if


                if (user.Gender == 2)
                {

                    templateBuilder.Append("女\r\n");

                }	//end if

                templateBuilder.Append("			</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>生日</th>\r\n");
                templateBuilder.Append("			<td>" + user.Bday.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");

                if (admininfo != null && admininfo.Allowviewip == 1)
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>注册IP</th>\r\n");
                    templateBuilder.Append("			<td>" + user.Regip.ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if

                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>注册日期</th>\r\n");
                templateBuilder.Append("			<td>" + user.Joindate.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>个人主页</th>\r\n");
                templateBuilder.Append("			<td>" + user.Website.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>自我介绍</th>\r\n");
                templateBuilder.Append("			<td>" + user.Bio.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");

                ////if (admininfo != null && admininfo.Allowviewrealname == 1)
                //{

                //    templateBuilder.Append("		<tr>\r\n");
                //    templateBuilder.Append("			<th>真实姓名</th>\r\n");
                //    templateBuilder.Append("			<td>" + user.Realname.ToString().Trim() + "</td>\r\n");
                //    templateBuilder.Append("		</tr>\r\n");
                //    templateBuilder.Append("		<tr>\r\n");
                //    templateBuilder.Append("			<th>身份证号码</th>\r\n");
                //    templateBuilder.Append("			<td>" + user.Idcard.ToString().Trim() + "</td>\r\n");
                //    templateBuilder.Append("		</tr>\r\n");
                //    templateBuilder.Append("		<tr>\r\n");
                //    templateBuilder.Append("			<th>移动电话号码</th>\r\n");
                //    templateBuilder.Append("			<td>" + user.Mobile.ToString().Trim() + "</td>\r\n");
                //    templateBuilder.Append("		</tr>		\r\n");
                //    templateBuilder.Append("		<tr>\r\n");
                //    templateBuilder.Append("			<th>固定电话号码</th>\r\n");
                //    templateBuilder.Append("			<td>" + user.Phone.ToString().Trim() + "</td>\r\n");
                //    templateBuilder.Append("		</tr>\r\n");

                //}	//end if


                if (user.Showemail == 1)
                {

                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>E-Mail</th>\r\n");
                    templateBuilder.Append("			<td><a herf=\"#\" onclick=\"javascript:location.href='mailto:" + user.Email.ToString().Trim() + "';\">" + user.Email.ToString().Trim() + "</a></td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");

                }	//end if

                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>QQ</th>\r\n");
                templateBuilder.Append("			<td>" + user.Qq.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>MSN Messenger</th>\r\n");
                templateBuilder.Append("			<td>" + user.Msn.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>Yahoo Messenger</th>\r\n");
                templateBuilder.Append("			<td>" + user.Yahoo.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>Skype</th>\r\n");
                templateBuilder.Append("			<td>" + user.Skype.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>ICQ</th>\r\n");
                templateBuilder.Append("			<td>" + user.Icq.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th class=\"infotitle\">发帖情况</th>\r\n");
                templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>最后发帖</th>\r\n");
                templateBuilder.Append("			<td class=\"userlink\"><a href=\"showtree.aspx?postid=" + user.Lastpostid.ToString().Trim() + "\" target=\"_blank\">" + user.Lastposttitle.ToString().Trim() + "</a> <span>" + user.Lastpost.ToString().Trim() + "</span></td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>最后访问(登录)</th>\r\n");
                templateBuilder.Append("			<td>" + user.Lastvisit.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>最后活动</th>\r\n");
                templateBuilder.Append("			<td>" + user.Lastactivity.ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                templateBuilder.Append("			<td class=\"userlink\"><a href=\"search.aspx?posterid=" + user.Uid.ToString().Trim() + "\">搜索该用户发表的主题及相关内容</a></td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		</table>\r\n");
                templateBuilder.Append("	</td>\r\n");
                templateBuilder.Append("	<td class=\"postauthor\">\r\n");

                if (user.Avatar != "")
                {

                    templateBuilder.Append("		<div class=\"avatar\">\r\n");
                    templateBuilder.Append("			<img class=\"avatar\" src=\"" + user.Avatar.ToString().Trim() + "\"\r\n");

                    if (user.Avatarwidth > 0)
                    {

                        templateBuilder.Append("					width=\"" + user.Avatarwidth.ToString().Trim() + "\"\r\n");
                        templateBuilder.Append("					height=\"" + user.Avatarheight.ToString().Trim() + "\"\r\n");

                    }	//end if

                    templateBuilder.Append("			/>\r\n");
                    templateBuilder.Append("		</div>\r\n");

                }	//end if
                if (user.Showemail == 1)
                {

                    templateBuilder.Append("		<p class=\"usermail\"><a href=\"mailto:" + user.Email.ToString().Trim() + "\">给该用户发送Email</a></p>\r\n");

                }	//end if

                templateBuilder.Append("		<p class=\"userpm\"><a href=\"usercppostpm.aspx?msgtoid=" + user.Uid.ToString().Trim() + "\">给该用户发送短消息</a></p>\r\n");

                if (useradminid > 0 && admininfo.Allowbanuser == 1)
                {

                    templateBuilder.Append("			<p class=\"userban\"><a href=\"useradmin.aspx?action=banuser&uid=" + user.Uid.ToString().Trim() + "\" title=\"禁止用户\">禁言用户</a></p>\r\n");

                }	//end if

                templateBuilder.Append("	</td>\r\n");
                templateBuilder.Append("	</tr>\r\n");
                templateBuilder.Append("</table>\r\n");
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


                if (needlogin)
                {


                    templateBuilder.Append("<div class=\"box message\">\r\n");
                    templateBuilder.Append("	<h1>" + config.Forumtitle.ToString().Trim() + " 提示信息</h1>\r\n");
                    templateBuilder.Append("	<p>您无权进行当前操作，这可能因以下原因之一造成</p>\r\n");
                    templateBuilder.Append("	<p><b>" + msgbox_text.ToString() + "</b></p>\r\n");
                    templateBuilder.Append("	<p>您还没有登录，请填写下面的登录表单后再尝试访问。</p>\r\n");
                    templateBuilder.Append("	<form id=\"formlogin\" name=\"formlogin\" method=\"post\" action=\"login.aspx\" onsubmit=\"submitLogin(this);\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" value=\"2592000\" name=\"cookietime\"/>\r\n");
                    templateBuilder.Append("	<div class=\"box\" style=\"margin: 10px auto; width: 60%;\">\r\n");
                    templateBuilder.Append("		<table cellpadding=\"4\" cellspacing=\"0\" width=\"100%\">\r\n");
                    templateBuilder.Append("		<thead>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td colspan=\"2\">会员登录</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</thead>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>用户名</td>\r\n");
                    templateBuilder.Append("				<td><input type=\"text\" id=\"username\" name=\"username\" size=\"25\" maxlength=\"40\" tabindex=\"2\" />  <a href=\"register.aspx\" tabindex=\"-1\" accesskey=\"r\" title=\"立即注册 (ALT + R)\">立即注册</a>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>密码</td>\r\n");
                    templateBuilder.Append("				<td><input type=\"password\" name=\"password\" size=\"25\" tabindex=\"3\" /> <a href=\"getpassword.aspx\" tabindex=\"-1\" accesskey=\"g\" title=\"忘记密码 (ALT + G)\">忘记密码</a>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");

                    if (config.Secques == 1)
                    {

                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<td>安全问题</td>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<select name=\"questionid\" tabindex=\"4\">\r\n");
                        templateBuilder.Append("					<option value=\"0\">&nbsp;</option>\r\n");
                        templateBuilder.Append("					<option value=\"1\">母亲的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"2\">爷爷的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"3\">父亲出生的城市</option>\r\n");
                        templateBuilder.Append("					<option value=\"4\">您其中一位老师的名字</option>\r\n");
                        templateBuilder.Append("					<option value=\"5\">您个人计算机的型号</option>\r\n");
                        templateBuilder.Append("					<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
                        templateBuilder.Append("					<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
                        templateBuilder.Append("					</select>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<td>答案</td>\r\n");
                        templateBuilder.Append("				<td><input type=\"text\" name=\"answer\" size=\"25\" tabindex=\"5\" /></td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>&nbsp;</td>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<button class=\"submit\" type=\"submit\" name=\"loginsubmit\" id=\"loginsubmit\" value=\"true\" tabindex=\"6\">会员登录</button>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		</table>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	</form>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	document.getElementById(\"username\").focus();\r\n");
                    templateBuilder.Append("	function submitLogin(loginForm)\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		loginForm.action = 'login.aspx?loginsubmit=true&reurl=' + escape(window.location);\r\n");
                    templateBuilder.Append("		loginForm.submit();\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
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

                templateBuilder.Append("</div>\r\n");

            }	//end if


            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
	}
}
