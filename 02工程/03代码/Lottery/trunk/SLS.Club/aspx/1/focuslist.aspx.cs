using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// 分栏模式首页
	/// </summary>
	public partial class focuslist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 精华主题列表
        /// </summary>
		public DataTable digesttopiclist;
        /// <summary>
        /// 热门主题列表
        /// </summary>
		public DataTable hottopiclist;
        /// <summary>
        /// 当前登录用户上次访问时间
        /// </summary>
		public string lastvisit = "";
        /// <summary>
        /// 在线用户列表
        /// </summary>
		public DataTable onlineuserlist;
        /// <summary>
        /// 获取友情链接列表
        /// </summary>
		public DataTable forumlinklist;

#if NET1
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 当前登录用户的短消息列表
        /// </summary>
        public Discuz.Common.Generic.List<PrivateMessageInfo> pmlist ;//= new Discuz.Common.Generic.List<PrivateMessageInfo>();
#endif		
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 页内文字广告
        /// </summary>
		public string[] pagewordad;
        /// <summary>
        /// 对联广告
        /// </summary>
		public string doublead;
        /// <summary>
        /// 浮动广告
        /// </summary>
		public string floatad;
        /// <summary>
        /// 公告数量
        /// </summary>
		public int announcementcount;
        /// <summary>
        /// 在线图例列表
        /// </summary>
		public string onlineiconlist = "";
        /// <summary>
        /// 当前登录用户简要信息
        /// </summary>
		public ShortUserInfo userinfo;
        /// <summary>
        /// 总主题数
        /// </summary>
		public int totaltopic;
        /// <summary>
        /// 总帖子数
        /// </summary>
		public int totalpost;
        /// <summary>
        /// 总用户数
        /// </summary>
		public int totalusers;
        /// <summary>
        /// 今日帖数
        /// </summary>
		public int todayposts;
        /// <summary>
        /// 友情链接数
        /// </summary>
		public int forumlinkcount;
        /// <summary>
        /// 最后注册的用户的用户名
        /// </summary>
		public string lastusername;
        /// <summary>
        /// 最后注册的用户的用户Id
        /// </summary>
		public int lastuserid;
        /// <summary>
        /// 总在线用户数
        /// </summary>
		public int totalonline;
        /// <summary>
        /// 总在线注册用户数
        /// </summary>
		public int totalonlineuser;
        /// <summary>
        /// 总在线游客数
        /// </summary>
		public int totalonlineguest;
        /// <summary>
        /// 总在线隐身用户数
        /// </summary>
		public int totalonlineinvisibleuser;
        /// <summary>
        /// 最高在线用户数
        /// </summary>
		public string highestonlineusercount;
        /// <summary>
        /// 最高在线用户数发生时间
        /// </summary>
		public string highestonlineusertime;
        /// <summary>
        /// 是否显示在线列表
        /// </summary>
		public bool showforumonline;
        /// <summary>
        /// 可用的扩展金币显示名称
        /// </summary>
		public string[] score;
        /// <summary>
        /// 是否显示短消息提示
        /// </summary>
        public bool showpmhint = false;
        #endregion

        protected override void ShowPage()
		{
			pagetitle = "首页";

            score = Scoresets.GetValidScoreName();

			if (config.Rssstatus == 1)
			{
				AddLinkRss("tools/rss.aspx", string.Format("{0} 最新主题", config.Forumtitle));
			}

			OnlineUsers.UpdateAction(olid, UserAction.IndexShow.ActionID, 0, config.Onlinetimeout);

			if (newpmcount > 0)
			{
				pmlist = PrivateMessages.GetPrivateMessageCollectionForIndex(userid,5,1,1);
			}

			userinfo = new ShortUserInfo();
			if (userid != -1)
			{
				userinfo = Discuz.Forum.Users.GetShortUserInfo(userid);
				if (userinfo.Newpm == 0)
				{
					base.newpmcount = 0;
				}
				lastvisit = userinfo.Lastvisit.ToString();
                showpmhint = Convert.ToInt32(userinfo.Newsletter) > 4;
			}

			Statistics.GetPostCountFromForum(0,out totaltopic,out totalpost,out todayposts);
			digesttopiclist = Focuses.GetDigestTopicList(16);
            hottopiclist = Focuses.GetHotTopicList(16, 30);
			forumlinklist = Caches.GetForumLinkList();
			forumlinkcount = forumlinklist.Rows.Count;

			// 获得统计信息
			totalusers = Utils.StrToInt(Statistics.GetStatisticsRowItem("totalusers"), 0);
			lastusername = Statistics.GetStatisticsRowItem("lastusername");
			lastuserid = Utils.StrToInt(Statistics.GetStatisticsRowItem("lastuserid"), 0);

			totalonline = onlineusercount;

			showforumonline = false;
			if (totalonline < config.Maxonlinelist || DNTRequest.GetString("showonline") == "yes")
			{
				showforumonline = true;
				onlineuserlist = OnlineUsers.GetOnlineUserList(onlineusercount, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);
				onlineiconlist = Caches.GetOnlineGroupIconList();
			}

			if (DNTRequest.GetString("showonline") == "no")
			{
				showforumonline = false;
			}

			highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
			highestonlineusertime = Statistics.GetStatisticsRowItem("highestonlineusertime");

			// 得到公告
			announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
			announcementcount = 0;
			if (announcementlist != null)
			{
				announcementcount = announcementlist.Rows.Count;
			}

			///得到广告列表
			headerad = Advertisements.GetOneHeaderAd("indexad",0);
			footerad = Advertisements.GetOneFooterAd("indexad",0);
			pagewordad = Advertisements.GetPageWordAd("indexad",0);
			doublead = Advertisements.GetDoubleAd("indexad",0);
			floatad = Advertisements.GetFloatAd("indexad",0);
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:09.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:09. 
            */

            base.OnLoad(e);

            
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"userinfo\">\r\n");
            templateBuilder.Append("		<div id=\"nav\">\r\n");
            templateBuilder.Append("		<p>金币: <strong>" + userinfo.Credits.ToString().Trim() + "</strong> / 头衔:<strong> " + usergroupinfo.Grouptitle.ToString().Trim() + "</strong> / 你上次访问是在 " + lastvisit.ToString() + "</p>\r\n");
            templateBuilder.Append("		<p>共 <strong>" + totalusers.ToString() + "</strong> 位会员 / 欢迎新会员 <strong>\r\n");
            aspxrewriteurl = this.UserInfoAspxRewrite(lastuserid);

            templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">" + lastusername.ToString() + "</a></strong></p>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>	\r\n");
            templateBuilder.Append("	<div id=\"forumstats\">\r\n");
            templateBuilder.Append("		<p>共 <strong>" + totaltopic.ToString() + "</strong>  篇主题 /<strong> " + totalpost.ToString() + "</strong>  个帖子 / 今日<strong> " + todayposts.ToString() + "</strong>  个帖子</p>\r\n");
            templateBuilder.Append("		<p>\r\n");

            if (userid != -1)
            {

                templateBuilder.Append("		<a href=\"mytopics.aspx\">我的主题</a>\r\n");
                templateBuilder.Append("		<a href=\"myposts.aspx\">我的帖子</a>\r\n");
                templateBuilder.Append("		<a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">我的精华</a>\r\n");

            }	//end if

            templateBuilder.Append("		<a href=\"showtopiclist.aspx?type=newtopic&amp;newtopic=" + newtopicminute.ToString() + "&amp;forums=all\">查看新帖</a>\r\n");
            templateBuilder.Append("		<a href=\"showtopiclist.aspx?type=digest&amp;forums=all\">精华帖区</a>\r\n");

            if (config.Rssstatus != 0)
            {

                templateBuilder.Append("		<a href=\"tools/rss.aspx\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/rss.gif\" alt=\"Rss\"/></a>\r\n");

            }	//end if

            templateBuilder.Append("	</p>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");


            if (pagewordad.Length > 0)
            {

                templateBuilder.Append("<!--adtext-->\r\n");
                templateBuilder.Append("<div id=\"ad_text\" class=\"ad_text\">\r\n");
                templateBuilder.Append("	<table cellspacing=\"1\" cellpadding=\"0\" summary=\"Text Ad\">\r\n");
                templateBuilder.Append("	<tbody>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                int adindex = 0;


                int pageword__loop__id = 0;
                foreach (string pageword in pagewordad)
                {
                    pageword__loop__id++;


                    if (adindex < 4)
                    {

                        templateBuilder.Append("				<td>" + pageword.Replace("http://club.icaile.com", Shove._Web.Utility.GetUrl()) + "</td>\r\n");
                        adindex = adindex + 1;


                    }
                    else
                    {

                        templateBuilder.Append("				</tr><tr>\r\n");
                        templateBuilder.Append("				<td>" + pageword.Replace("http://club.icaile.com", Shove._Web.Utility.GetUrl()) + "</td>\r\n");
                        adindex = 1;


                    }	//end if


                }	//end loop


                if (pagewordad.Length % 4 > 0)
                {


                    for (int j = 0; j < (4 - pagewordad.Length % 4); j++)
                    {

                        templateBuilder.Append("				<td>&nbsp;</td>\r\n");

                    }


                }	//end if

                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<!--adtext-->\r\n");

            }	//end if





            if (newpmcount > 0 && showpmhint)
            {

                templateBuilder.Append("<!--短信息 area start-->\r\n");
                templateBuilder.Append("<div class=\"mainbox\">\r\n");

                if (pmsound > 0)
                {

                    templateBuilder.Append("		<bgsound src=\"sound/pm" + pmsound.ToString() + ".wav\" />\r\n");

                }	//end if

                templateBuilder.Append("	<span class=\"headactions\"><a href=\"usercpinbox.aspx\" target=\"_blank\">查看详情</a> <a href=\"###\" onclick=\"document.getElementById('frmnewpm').submit();\">不再提示</a></span>\r\n");
                templateBuilder.Append("	<h3>您有 " + newpmcount.ToString() + " 条新的短消息</h3>\r\n");
                templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n");

                int pm__loop__id = 0;
                foreach (PrivateMessageInfo pm in pmlist)
                {
                    pm__loop__id++;

                    templateBuilder.Append("	<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<td style=\"width:53px;text-align:center;\"><img src=\"templates/" + templatepath.ToString() + "/images/message_" + pm.New.ToString().Trim() + ".gif\" alt=\"短信息\"/></td>\r\n");
                    templateBuilder.Append("			<th><a href=\"usercpshowpm.aspx?pmid=" + pm.Pmid.ToString().Trim() + "\">" + pm.Subject.ToString().Trim() + "</a></th>\r\n");
                    templateBuilder.Append("			<td>\r\n");
                    templateBuilder.Append("				<a href=\"userinfo.aspx?userid=" + pm.Msgfromid.ToString().Trim() + "\" target=\"_blank\">" + pm.Msgfrom.ToString().Trim() + "</a>\r\n");
                    templateBuilder.Append("				<span class=\"fontfamily\">" + pm.Postdatetime.ToString().Trim() + "</span>\r\n");
                    templateBuilder.Append("			</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("	</tbody>\r\n");

                }	//end loop

                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("	<form id=\"frmnewpm\" name=\"frmnewpm\" method=\"post\" action=\"#\">\r\n");
                templateBuilder.Append("		<input id=\"ignore\" name=\"ignore\" type=\"hidden\" value=\"yes\" />\r\n");
                templateBuilder.Append("	</form>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<!--短信息 area end-->\r\n");

            }	//end if



            templateBuilder.Append("<div id=\"forumfocus\">\r\n");
            templateBuilder.Append("	<div class=\"focuslistleft\">\r\n");
            templateBuilder.Append("		<div class=\"mainbox\">\r\n");
            templateBuilder.Append("			<h3>最新精华主题</h3>\r\n");
            templateBuilder.Append("			<ul class=\"navfocuslist\">\r\n");

            int digesttopic__loop__id = 0;
            foreach (DataRow digesttopic in digesttopiclist.Rows)
            {
                digesttopic__loop__id++;

                aspxrewriteurl = this.ShowTopicAspxRewrite(digesttopic["tid"].ToString().Trim(), 0);


                if (digesttopic["iconid"].ToString().Trim() != "0")
                {

                    templateBuilder.Append("						<li><img src=\"images/posticons/" + digesttopic["iconid"].ToString().Trim() + ".gif\" alt=\"smile\"/><a href=\"" + aspxrewriteurl.ToString() + "\" target=\"main\">" + digesttopic["title"].ToString().Trim() + "</a></li>\r\n");

                }
                else
                {

                    templateBuilder.Append("						<li class=\"listspace\"><a href=\"" + aspxrewriteurl.ToString() + "\" target=\"main\">" + digesttopic["title"].ToString().Trim() + "</a> </li>\r\n");

                }	//end if


            }	//end loop

            templateBuilder.Append("			</ul>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div class=\"focuslistright\">\r\n");
            templateBuilder.Append("		<div class=\"mainbox\">\r\n");
            templateBuilder.Append("			<h3>最新热门主题</h3>\r\n");
            templateBuilder.Append("			<ul class=\"navfocuslist\">\r\n");

            int hottopic__loop__id = 0;
            foreach (DataRow hottopic in hottopiclist.Rows)
            {
                hottopic__loop__id++;

                aspxrewriteurl = this.ShowTopicAspxRewrite(hottopic["tid"].ToString().Trim(), 0);


                if (hottopic["iconid"].ToString().Trim() != "0")
                {

                    templateBuilder.Append("						<li><img src=\"images/posticons/" + hottopic["iconid"].ToString().Trim() + ".gif\" alt=\"smile\"/><a href=\"" + aspxrewriteurl.ToString() + "\" target=\"main\">" + hottopic["title"].ToString().Trim() + "</a></li>\r\n");

                }
                else
                {

                    templateBuilder.Append("						<li class=\"listspace\"><a href=\"" + aspxrewriteurl.ToString() + "\" target=\"main\">" + hottopic["title"].ToString().Trim() + "</a> </li>\r\n");

                }	//end if


            }	//end loop

            templateBuilder.Append("			</ul>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!---bbs-list area end--->\r\n");

            if (forumlinkcount > 0)
            {

                templateBuilder.Append("<div class=\"box\">\r\n");
                templateBuilder.Append("	<span class=\"headactions\"><img id=\"forumlinks_img\" src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\" alt=\"\" onClick=\"toggle_collapse('forumlinks');\" /></span>\r\n");
                templateBuilder.Append("	<h4>友情链接</h4>\r\n");
                templateBuilder.Append("	<table id=\"forumlinks\" cellspacing=\"0\" cellpadding=\"0\" style=\"table-layout: fixed;\" summary=\"友情链接\">\r\n");

                int forumlink__loop__id = 0;
                foreach (DataRow forumlink in forumlinklist.Rows)
                {
                    forumlink__loop__id++;

                    templateBuilder.Append("		<tbody>	\r\n");
                    templateBuilder.Append("		<tr>	\r\n");

                    if (forumlink["logo"].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\"><img src=\"" + forumlink["logo"].ToString().Trim() + "\" alt=\"" + forumlink["name"].ToString().Trim() + "\"  class=\"forumlink_logo\"/></a>\r\n");
                        templateBuilder.Append("				<h5><a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\">" + forumlink["name"].ToString().Trim() + "</a></h5>\r\n");
                        templateBuilder.Append("				<p>" + forumlink["note"].ToString().Trim() + "</p>\r\n");
                        templateBuilder.Append("			</td>\r\n");

                    }
                    else if (forumlink["name"].ToString().Trim() != "$$otherlink$$")
                    {

                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				<h5>\r\n");
                        templateBuilder.Append("					<a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\">" + forumlink["name"].ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("				</h5>\r\n");
                        templateBuilder.Append("				<p>" + forumlink["note"].ToString().Trim() + "</p>\r\n");
                        templateBuilder.Append("			</td>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("			<td>\r\n");
                        templateBuilder.Append("				" + forumlink["note"].ToString().Trim() + "\r\n");
                        templateBuilder.Append("			</td>\r\n");

                    }	//end if

                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                }	//end loop

                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");

            }	//end if


            if (config.Whosonlinestatus != 0 && config.Whosonlinestatus != 2)
            {

                templateBuilder.Append("<div class=\"box\" id=\"online\">\r\n");
                templateBuilder.Append("	<span class=\"headactions\">\r\n");

                if (DNTRequest.GetString("showonline") == "no")
                {

                    templateBuilder.Append("			<a href=\"?showonline=yes#online\"><img src=\"templates/" + templatepath.ToString() + "/images/collapsed_yes.gif\" alt=\"展开/收起\" />\r\n");

                }
                else
                {

                    templateBuilder.Append("			<a href=\"?showonline=no#online\"><img src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\" alt=\"展开/收起\" />\r\n");

                }	//end if

                templateBuilder.Append("		</a>\r\n");
                templateBuilder.Append("	</span>\r\n");
                templateBuilder.Append("	<h4>\r\n");
                templateBuilder.Append("		<strong><a href=\"" + forumurl.ToString() + "onlineuser.aspx\">在线用户</a></strong>- <em>" + totalonline.ToString() + "</em> 人在线 \r\n");

                if (showforumonline)
                {

                    templateBuilder.Append("- " + totalonlineuser.ToString() + " 会员<span id=\"invisible\"></span>, " + totalonlineguest.ToString() + " 游客\r\n");

                }	//end if

                templateBuilder.Append("- 最高记录是 <em>" + highestonlineusercount.ToString() + "</em> 于 <em>" + highestonlineusertime.ToString() + "</em>\r\n");
                templateBuilder.Append("	</h4>\r\n");
                templateBuilder.Append("	<dl id=\"onlinelist\">\r\n");
                templateBuilder.Append("		<dt>" + onlineiconlist.ToString() + "</dt>\r\n");
                templateBuilder.Append("		<dd class=\"onlineusernumber\">\r\n");
                templateBuilder.Append("			共<strong>" + totalusers.ToString() + "</strong>位会员 <span class=\"newuser\">新会员:\r\n");
                aspxrewriteurl = this.UserInfoAspxRewrite(lastuserid);

                templateBuilder.Append("			<a href=\"" + aspxrewriteurl.ToString() + "\">" + lastusername.ToString() + "</a></span>\r\n");
                templateBuilder.Append("		</dd>\r\n");
                templateBuilder.Append("		<dd>\r\n");
                templateBuilder.Append("			<ul class=\"userlist\">\r\n");

                if (showforumonline)
                {

                    int invisiblecount = 0;


                    int onlineuser__loop__id = 0;
                    foreach (DataRow onlineuser in onlineuserlist.Rows)
                    {
                        onlineuser__loop__id++;


                        if (onlineuser["invisible"].ToString().Trim() == "1")
                        {

                            invisiblecount = invisiblecount + 1;

                            templateBuilder.Append("				<li>(隐身会员)</li>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					<li>" + onlineuser["olimg"].ToString().Trim() + "\r\n");

                            if (onlineuser["userid"].ToString().Trim() == "-1")
                            {

                                templateBuilder.Append("							" + onlineuser["username"].ToString().Trim() + "\r\n");

                            }
                            else
                            {

                                aspxrewriteurl = this.UserInfoAspxRewrite(onlineuser["userid"].ToString().Trim());

                                templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + onlineuser["username"].ToString().Trim() + "</a>\r\n");

                            }	//end if

                            templateBuilder.Append("				</li>\r\n");

                        }	//end if


                    }	//end loop


                }
                else
                {

                    templateBuilder.Append("				<li style=\"width: auto\"><a href=\"?showonline=yes#online\">点击查看在线列表</a></li>\r\n");

                }	//end if

                templateBuilder.Append("			</ul>\r\n");
                templateBuilder.Append("		</dd>\r\n");
                templateBuilder.Append("	</dl>\r\n");
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




            if (floatad != "")
            {

                templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/template_floatadv.js\"></" + "script>\r\n");
                templateBuilder.Append("	" + floatad.ToString() + "\r\n");
                templateBuilder.Append("	<script type=\"text/javascript\">theFloaters.play();</" + "script>\r\n");

            }
            else if (doublead != "")
            {

                templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/template_floatadv.js\"></" + "script>\r\n");
                templateBuilder.Append("	" + doublead.ToString() + "\r\n");
                templateBuilder.Append("	<script type=\"text/javascript\">theFloaters.play();</" + "script>\r\n");

            }	//end if




            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
	}
}
