using System;
using System.Data;
using System.Web;
using System.Xml;
using System.Text.RegularExpressions;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Common.Generic;

namespace Discuz.Web
{
    /// <summary>
    /// 论坛首页
    /// </summary>
    public partial class forumindex : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 论坛版块列表
        /// </summary>
        public Discuz.Common.Generic.List<IndexPageForumInfo> forumlist;//= new Discuz.Common.Generic.List<IndexPageForumInfo>();
        /// <summary>
        /// 在线用户列表
        /// </summary>
        public Discuz.Common.Generic.List<OnlineUserInfo> onlineuserlist;// = new Discuz.Common.Generic.List<OnlineUserInfo>();
        /// <summary>
        /// 当前登录的用户短消息列表
        /// </summary>
        public Discuz.Common.Generic.List<PrivateMessageInfo> pmlist;//= new Discuz.Common.Generic.List<PrivateMessageInfo>();
        /// <summary>
        /// 当前用户最后访问时间
        /// </summary>
        public string lastvisit = "未知";
        /// <summary>
        /// 友情链接列表
        /// </summary>
        public DataTable forumlinklist;
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
        ///  Silverlight广告
        /// </summary>
        public string mediaad;
        /// <summary>
        /// 分类间广告
        /// </summary>
        public string inforumad;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount;
        /// <summary>
        /// 在线图例列表
        /// </summary>
        public string onlineiconlist = "";
        /// <summary>
        /// 当前登录用户的简要信息
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
        /// 昨日帖数
        /// </summary>
        public int yesterdayposts;
        /// <summary>
        /// 最高日帖数
        /// </summary>
        public int highestposts;
        /// <summary>
        /// 最高发帖日
        /// </summary>
        public string highestpostsdate;
        /// <summary>
        /// 友情链接数
        /// </summary>
        public int forumlinkcount;
        /// <summary>
        /// 最新注册的用户名
        /// </summary>
        public string lastusername;
        /// <summary>
        /// 最新注册的用户Id
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
        /// 是否已经拥有个人空间
        /// </summary>
        public bool isactivespace;
        /// <summary>
        /// 是否允许申请个人空间
        /// </summary>
        public bool isallowapply;
        /// <summary>
        /// 可用的扩展金币显示名称
        /// </summary>
        public string[] score;
        /// <summary>
        /// 弹出导航菜单的HTML代码
        /// </summary>
        public string navhomemenu = "";
        /// <summary>
        /// 是否显示短消息
        /// </summary>
        public bool showpmhint = false;
        /// <summary>
        /// 标签列表
        /// </summary>
        public TagInfo[] taglist;
        #endregion



        protected override void ShowPage()
        {
            
            pagetitle = "首页";
            if(Discuz.Common.XmlConfig.GetCpsProperty("SiteUrl","") == null)
            {
                Shove._Web.JavaScript.Alert(this.Page,"a");
            }
            else
            {
                Shove._Web.JavaScript.Alert(this.Page,"b");
            }

            score = Scoresets.GetValidScoreName();

            int toframe = DNTRequest.GetInt("f", 1);
            if (toframe == 0)
            {
                ForumUtils.WriteCookie("isframe", 1);
            }
            else
            {
                toframe = Utils.StrToInt(ForumUtils.GetCookie("isframe"), -1);
                if (toframe == -1)
                {
                    toframe = config.Isframeshow;
                }
            }

            if (toframe == 2)
            {
                HttpContext.Current.Response.Redirect("frame.aspx");
                HttpContext.Current.Response.End();
                return;
            }


            if (config.Rssstatus == 1)
            {
                AddLinkRss("tools/rss.aspx", "最新主题");
            }

            OnlineUsers.UpdateAction(olid, UserAction.IndexShow.ActionID, 0, config.Onlinetimeout);

            if (newpmcount > 0)
            {
                pmlist = PrivateMessages.GetPrivateMessageCollectionForIndex(userid, 5, 1, 1);
            }

            userinfo = new ShortUserInfo();
            if (userid != -1)
            {
                userinfo = Discuz.Forum.Users.GetShortUserInfo(userid);
                if (userinfo == null)
                {
                    userid = -1;
                    ForumUtils.ClearUserCookie("dnt");
                }
                else
                {
                    if (userinfo.Newpm == 0)
                    {
                        base.newpmcount = 0;
                    }
                    lastvisit = userinfo.Lastvisit.ToString();
                    showpmhint = Convert.ToInt32(userinfo.Newsletter) > 4;
                }
            }

            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            totaltopic = 0;
            totalpost = 0;
            todayposts = 0;

            forumlist = Forums.GetForumIndexCollection(config.Hideprivate, usergroupid, config.Moddisplay, out totaltopic, out totalpost, out todayposts);
            forumlinklist = Caches.GetForumLinkList();
            forumlinkcount = forumlinklist.Rows.Count;

            string ChacheKey = "totaltopics";
            totaltopic = Shove._Web.Cache.GetCacheAsInt(ChacheKey, 0);
            if (totaltopic == 0)
            {
                totaltopic = Shove._Convert.StrToInt(new DAL.Tables.dnt_topics().GetCount("").ToString(), 0);

                Shove._Web.Cache.SetCache(ChacheKey, totaltopic, 120);
            }

            ChacheKey = "totalposts";
            totalpost = Shove._Web.Cache.GetCacheAsInt(ChacheKey, 0);
            if (totalpost == 0)
            {
                totalpost = Shove._Convert.StrToInt(new DAL.Tables.dnt_posts1().GetCount("").ToString(), 0);

                Shove._Web.Cache.SetCache(ChacheKey, totalpost, 120);
            }

            // 获得统计信息
            totalusers = Utils.StrToInt(Statistics.GetStatisticsRowItem("totalusers"), 0);
            lastusername = Statistics.GetStatisticsRowItem("lastusername");
            lastuserid = Utils.StrToInt(Statistics.GetStatisticsRowItem("lastuserid"), 0);
            yesterdayposts = Utils.StrToInt(Statistics.GetStatisticsRowItem("yesterdayposts"), 0);
            highestposts = Utils.StrToInt(Statistics.GetStatisticsRowItem("highestposts"), 0);
            highestpostsdate = Shove._Convert.StrToDateTime(Statistics.GetStatisticsRowItem("highestpostsdate").ToString().Trim(), DateTime.Now.ToString()).ToString("yyyy-MM-dd");
            if (todayposts > highestposts)
            {
                highestposts = todayposts;
                highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            totalonline = onlineusercount;
            showforumonline = false;
            onlineiconlist = Caches.GetOnlineGroupIconList();
            if (totalonline < config.Maxonlinelist || DNTRequest.GetString("showonline") == "yes")
            {
                showforumonline = true;
                //获得在线用户列表和图标
                onlineuserlist = OnlineUsers.GetOnlineUserCollection(out totalonline, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);
                totalonlineguest *= 10;
                totalonline = totalonlineuser + totalonlineguest;
            }

            if (DNTRequest.GetString("showonline") == "no")
            {
                showforumonline = false;
            }

            highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
            highestonlineusercount = (Shove._Convert.StrToInt(highestonlineusercount, 500) * 10).ToString();
            highestonlineusertime = DateTime.Parse(Statistics.GetStatisticsRowItem("highestonlineusertime")).ToString("yyyy-MM-dd HH:mm");
            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            announcementcount = 0;
            if (announcementlist != null)
            {
                announcementcount = announcementlist.Rows.Count;
            }

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("indexad", 0);
            footerad = Advertisements.GetOneFooterAd("indexad", 0);

            List<IndexPageForumInfo> topforum = new List<IndexPageForumInfo>();

            foreach (IndexPageForumInfo f in forumlist)
            {
                if (f.Layer == 0)
                {
                    topforum.Add(f);
                }
            }

            if (config.Enabletag == 1)
            {
                taglist = ForumTags.GetCachedHotForumTags(config.Hottagcount);
            }
            else
            {
                taglist = new TagInfo[0];
            }

            inforumad = Advertisements.GetInForumAd("indexad", 0, topforum, templatepath);

            pagewordad = Advertisements.GetPageWordAd("indexad", 0);
            doublead = Advertisements.GetDoubleAd("indexad", 0);
            floatad = Advertisements.GetFloatAd("indexad", 0);
            mediaad = Advertisements.GetMediaAd(templatepath, "indexad", 0);
        }



        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:49.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:49. 
            */

            base.OnLoad(e);

            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"userinfo\">\r\n");
            templateBuilder.Append("		<div id=\"nav\">\r\n");
            templateBuilder.Append("		<p><a id=\"forumlist\" href=\"" + config.Forumurl.ToString().Trim() + "\" \r\n");

            if (config.Forumjump == 1)
            {

                templateBuilder.Append("onmouseover=\"showMenu(this.id);\" onmouseout=\"showMenu(this.id);\"\r\n");

            }	//end if

            templateBuilder.Append(">" + config.Forumtitle.ToString().Trim() + "</a>		主题:<em>" + totaltopic.ToString() + "</em>, 帖子:<em>" + totalpost.ToString() + "</em> \r\n");
            templateBuilder.Append("		</p>\r\n");
            templateBuilder.Append("		<p>\r\n");

            if (userid == -1)
            {

                templateBuilder.Append("		<form id=\"loginform\" name=\"login\" method=\"post\" action=\"login.aspx?loginsubmit=true\">\r\n");
                templateBuilder.Append("			<input type=\"hidden\" name=\"referer\" value=\"index.aspx\" />\r\n");
                templateBuilder.Append("			<input onclick=\"if(this.value=='用户名')this.value = ''\" value=\"用户名\" maxlength=\"40\" size=\"15\" name=\"username\" id=\"username\" \r\n");
                templateBuilder.Append("	type=\"text\" />\r\n");
                templateBuilder.Append("			<input type=\"password\" size=\"10\" name=\"password\" id=\"password\" />\r\n");
                templateBuilder.Append("			<button value=\"true\" type=\"submit\" name=\"userlogin\" onclick=\"javascript:window.location.replace('?agree=yes')\">登录</button>\r\n");
                templateBuilder.Append("		</form>\r\n");

            }
            else
            {

                templateBuilder.Append("		您上次访问是在: " + userinfo.Lastvisit.ToString().Trim() + " 		\r\n");

                templateBuilder.Append("			<a href=\"showtopiclist.aspx\">查看新帖</a>\r\n");

            }	//end if

            templateBuilder.Append("		</p>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"forumstats\">\r\n");

            if (usergroupinfo.Allowsearch > 0)
            {


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



            }	//end if

            templateBuilder.Append("		<p>\r\n");
            templateBuilder.Append("		今日:<em>" + todayposts.ToString() + "</em>, 昨日:<em>" + yesterdayposts.ToString() + "</em>, \r\n");

            if (highestpostsdate != "")
            {

                templateBuilder.Append("		最高日:<em>" + highestposts.ToString() + "</em>(" + highestpostsdate.ToString() + ")\r\n");

            }	//end if

            templateBuilder.Append("			<a href=\"showtopiclist.aspx?type=digest&amp;forums=all\">精华区</a>\r\n");

            if (config.Rssstatus != 0)
            {

                templateBuilder.Append("			<a href=\"tools/rss.aspx\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/rss.gif\" alt=\"rss\"/></a>\r\n");

            }	//end if

            templateBuilder.Append("		</p>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (announcementcount > 0)
            {

                templateBuilder.Append("<div onmouseout=\"annstop = 0\" onmouseover=\"annstop = 1\" id=\"announcement\">\r\n");
                templateBuilder.Append("	<div id=\"announcementbody\">\r\n");
                templateBuilder.Append("		<ul>		\r\n");

                int announcement__loop__id = 0;
                foreach (DataRow announcement in announcementlist.Rows)
                {
                    announcement__loop__id++;

                    templateBuilder.Append("        <li><a href=\"announcement.aspx#" + announcement["id"].ToString().Trim() + "\">" + announcement["title"].ToString().Trim() + "<em>" + announcement["starttime"].ToString().Trim() + "</em></a></li>\r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("	var anndelay = 3000;\r\n");
                templateBuilder.Append("	var annst = 0;\r\n");
                templateBuilder.Append("	var annstop = 0;\r\n");
                templateBuilder.Append("	var annrowcount = 0;\r\n");
                templateBuilder.Append("	var anncount = 0;\r\n");
                templateBuilder.Append("	var annlis = $('announcementbody').getElementsByTagName(\"LI\");\r\n");
                templateBuilder.Append("	var annrows = new Array();\r\n");
                templateBuilder.Append("	var annstatus;\r\n");
                templateBuilder.Append("	function announcementScroll() {\r\n");
                templateBuilder.Append("		if(annstop) {\r\n");
                templateBuilder.Append("			annst = setTimeout('announcementScroll()', anndelay);\r\n");
                templateBuilder.Append("			return;\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("		if(!annst) {\r\n");
                templateBuilder.Append("			var lasttop = -1;\r\n");
                templateBuilder.Append("			for(i = 0;i < annlis.length;i++) {\r\n");
                templateBuilder.Append("				if(lasttop != annlis[i].offsetTop) {\r\n");
                templateBuilder.Append("					if(lasttop == -1) {\r\n");
                templateBuilder.Append("						lasttop = 0;\r\n");
                templateBuilder.Append("					}\r\n");
                templateBuilder.Append("					annrows[annrowcount] = annlis[i].offsetTop - lasttop;\r\n");
                templateBuilder.Append("					annrowcount++;\r\n");
                templateBuilder.Append("				}\r\n");
                templateBuilder.Append("				lasttop = annlis[i].offsetTop;\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("			if(annrows.length == 1) {\r\n");
                templateBuilder.Append("				$('announcement').onmouseover = $('announcement').onmouseout = null;\r\n");
                templateBuilder.Append("			} else {\r\n");
                templateBuilder.Append("				annrows[annrowcount] = annrows[1];\r\n");
                templateBuilder.Append("				$('announcementbody').innerHTML += '<br style=\"clear:both\" />' + $('announcementbody').innerHTML;\r\n");
                templateBuilder.Append("				annst = setTimeout('announcementScroll()', anndelay);\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("			annrowcount = 1;\r\n");
                templateBuilder.Append("			return;\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("		if(annrowcount >= annrows.length) {\r\n");
                templateBuilder.Append("			$('announcementbody').scrollTop = 0;\r\n");
                templateBuilder.Append("			annrowcount = 1;\r\n");
                templateBuilder.Append("			annst = setTimeout('announcementScroll()', anndelay);\r\n");
                templateBuilder.Append("		} else {\r\n");
                templateBuilder.Append("			anncount = 0;\r\n");
                templateBuilder.Append("			announcementScrollnext(annrows[annrowcount]);\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("	}\r\n");
                templateBuilder.Append("	function announcementScrollnext(time) {\r\n");
                templateBuilder.Append("		$('announcementbody').scrollTop++;\r\n");
                templateBuilder.Append("		anncount++;\r\n");
                templateBuilder.Append("		if(anncount != time) {\r\n");
                templateBuilder.Append("			annst = setTimeout('announcementScrollnext(' + time + ')', 10);\r\n");
                templateBuilder.Append("		} else {\r\n");
                templateBuilder.Append("			annrowcount++;\r\n");
                templateBuilder.Append("			annst = setTimeout('announcementScroll()', anndelay);\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("	}\r\n");
                templateBuilder.Append("	announcementScroll();\r\n");
                templateBuilder.Append("</" + "script>\r\n");

            }	//end if



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

            templateBuilder.Append("<!-- 主题排行开始 -->\r\n");
            templateBuilder.Append("<link rel=\"stylesheet\" href=\"Richie/topic.css\" type=\"text/css\" media=\"all\"  />\r\n");
            templateBuilder.Append("<div class=\"t\">\r\n");
            templateBuilder.Append("<table width=\"100%\" class=\"topicref\" cellspacing=\"1\" cellpadding=\"1\">\r\n");
            templateBuilder.Append("  <tr class=\"topicbanner\">\r\n");
            templateBuilder.Append("    <td width=\"18%\" height:65px background=\"/templates/default/images/header_bg.gif\" class=\"textstyle\"><font style=\"color:#DD0000\">社区最新图片</font></td>\r\n");
            templateBuilder.Append("    <td width=\"22%\" height:65px background=\"/templates/default/images/header_bg.gif\" class=\"textstyle\"><font style=\"color:#DD0000\">最新发表主题</font></td>\r\n");
            templateBuilder.Append("    <td width=\"22%\" height:65px background=\"/templates/default/images/header_bg.gif\" class=\"textstyle\"><font style=\"color:#DD0000\">最新回复主题</font></td>\r\n");
            templateBuilder.Append("    <td width=\"22%\" height:65px background=\"/templates/default/images/header_bg.gif\" class=\"textstyle\"><font style=\"color:#DD0000\">社区今日热贴</font></td>\r\n");
            templateBuilder.Append("  </tr>\r\n");
            templateBuilder.Append("  <tr class=\"tr3\">\r\n");
            templateBuilder.Append("    <td align=\"center\">\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"tools/showtopics.aspx?count=5&template=3&type=1&onlyimg=1&imgsize=200&encoding=utf-8&timespan=0\"> </" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"tools/showtopics.aspx?count=5&template=5&type=2&onlyimg=1&encoding=utf-8&timespan=0\"> </" + "script>\r\n");
            templateBuilder.Append("    <script type=\"text/javascript\">\r\n");
            templateBuilder.Append("    <!--\r\n");
            templateBuilder.Append("    function ShoveWebUI_ShoveImagePlayerFlash_Onload(ID, FocusWidth, FocusHeight, TextHeight, Pics, Links, Texts, FlashAddress, TitleBgColor)\r\n");
            templateBuilder.Append("    {\r\n");
            templateBuilder.Append("        var ShoveWebUI_ShoveImagePlayer_FocusWidth = FocusWidth; \r\n");
            templateBuilder.Append("        var ShoveWebUI_ShoveImagePlayer_FocusHeight = FocusHeight\r\n");
            templateBuilder.Append("        var ShoveWebUI_ShoveImagePlayer_TextHeight = TextHeight;\r\n");
            templateBuilder.Append("        var ShoveWebUI_ShoveImagePlayer_SwfHeight = ShoveWebUI_ShoveImagePlayer_FocusHeight + ShoveWebUI_ShoveImagePlayer_TextHeight;\r\n");
            templateBuilder.Append("        document.write('<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" width=\"'+ ShoveWebUI_ShoveImagePlayer_FocusWidth +'\" height=\"'+ ShoveWebUI_ShoveImagePlayer_SwfHeight +'\">');");
            templateBuilder.Append("        document.write('<param name=\"allowScriptAccess\" value=\"sameDomain\"><param name=\"movie\" value=\"' + FlashAddress + '\"> <param name=\"quality\" value=\"high\"><param name=\"bgcolor\" value=\"' + TitleBgColor + '\">');");
            templateBuilder.Append("        document.write('<param name=\"menu\" value=\"false\"><param name=wmode value=\"opaque\">');");
            templateBuilder.Append("        document.write('<param name=\"FlashVars\" value=\"pics='+ Pics +'&links='+ Links +'&texts='+ Texts +'&borderwidth='+ ShoveWebUI_ShoveImagePlayer_FocusWidth +'&borderheight='+ ShoveWebUI_ShoveImagePlayer_FocusHeight +'&textheight='+ ShoveWebUI_ShoveImagePlayer_TextHeight +'\">');");
            templateBuilder.Append("        document.write('<embed src=\"' + FlashAddress + '\" wmode=\"opaque\" FlashVars=\"pics='+ Pics +'&links='+ Links +'&texts='+ Texts +'&borderwidth='+ ShoveWebUI_ShoveImagePlayer_FocusWidth +'&borderheight='+ ShoveWebUI_ShoveImagePlayer_FocusHeight +'&textheight='+ ShoveWebUI_ShoveImagePlayer_TextHeight +'\" menu=\"false\" bgcolor=\"#ffffff\" quality=\"high\" width=\"'+ ShoveWebUI_ShoveImagePlayer_FocusWidth +'\" height=\"'+ ShoveWebUI_ShoveImagePlayer_SwfHeight +'\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />');");
            templateBuilder.Append("        document.write('</object>');");
            templateBuilder.Append("    }\r\n");
            templateBuilder.Append("    ShoveWebUI_ShoveImagePlayerFlash_Onload('aa',260,190,0,pics,links,'','ShoveWebUI_client/Images/PixviewerYellow.swf','#F4FBFF');");

            //templateBuilder.Append("      document.write('<object classid=\"clsid:d27cdb6e-ae6d-11cf-96b8-444553540000\" codebase=\"http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0\" width=\"'+ swf_width +'\" height=\"'+ swf_height +'\">');\r\n");
            //templateBuilder.Append("      document.write('<param name=\"allowScriptAccess\" value=\"sameDomain\"><param name=\"movie\" value=\"Richie/pixviewer.swf\"><param name=\"quality\" value=\"high\"><param name=\"bgcolor\" value=\"#F4FBFF\">');\r\n");
            //templateBuilder.Append("      document.write('<param name=\"menu\" value=\"false\"><param name=wmode value=\"opaque\">');\r\n");
            //templateBuilder.Append("      document.write('<param name=\"FlashVars\" value=\"pics='+pics+'&links='+links+'&borderwidth='+focus_width+'&borderheight='+focus_height+'\">');\r\n");
            //templateBuilder.Append("      document.write('<embed src=\"Richie/pixviewer.swf\" wmode=\"opaque\" FlashVars=\"pics='+pics+'&links='+links+'&borderwidth='+focus_width+'&borderheight='+focus_height+'\" menu=\"false\" bgcolor=\"#F4FBFF\" quality=\"high\" width=\"'+ swf_width +'\" height=\"'+ swf_height +'\" allowScriptAccess=\"sameDomain\" type=\"application/x-shockwave-flash\" pluginspage=\"http://www.macromedia.com/go/getflashplayer\" />');\r\n");
            //templateBuilder.Append("      document.write('</object>');\r\n");
            templateBuilder.Append("    //-->\r\n");
            templateBuilder.Append("    </" + "script>\r\n");
            templateBuilder.Append("    </td>\r\n");
            templateBuilder.Append("    <!--最新发表主题-->\r\n");
            templateBuilder.Append("    <td class=\"f_two\" style=\"vertical-align:top;\">\r\n");
            templateBuilder.Append("        <ul>\r\n");
            templateBuilder.Append("      <script type=\"text/javascript\" src=\"tools/showtopics.aspx?template=7&order=0&count=10&length=25&encoding=utf-8&timespan=0\"></" + "script>\r\n");
            templateBuilder.Append("    </ul>\r\n");
            templateBuilder.Append("</td>\r\n");
            templateBuilder.Append("<!--最新回复主题-->\r\n");
            templateBuilder.Append("    <td style=\"vertical-align:top;\">\r\n");
            templateBuilder.Append("  <ul>\r\n");
            templateBuilder.Append("      <script type=\"text/javascript\" src=\"tools/showtopics.aspx?template=7&order=2&count=10&length=25&encoding=utf-8&timespan=0\"></" + "script>\r\n");
            templateBuilder.Append("  </ul>\r\n");
            templateBuilder.Append("</td>\r\n");
            templateBuilder.Append("<!--是否精华-->\r\n");
            templateBuilder.Append("<td style=\"vertical-align:top;\">\r\n");
            templateBuilder.Append("  <ul>\r\n");
            templateBuilder.Append("      <script type=\"text/javascript\" src=\"tools/showtopics.aspx?template=7&order=1&time=1&count=10&length=25&encoding=utf-8&timespan=0\"></" + "script>\r\n");
            templateBuilder.Append("  </ul>\r\n");
            templateBuilder.Append("</td>\r\n");
            templateBuilder.Append("<!--今日发贴排行-->\r\n");
            templateBuilder.Append("  </tr>\r\n");
            templateBuilder.Append("</table>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!-- 主题排行结束 -->\r\n");
            templateBuilder.Append("<!--forumtopic-->\r\n");
            templateBuilder.Append("<!--topic-->\r\n");


            int lastforumlayer = -1;

            int lastcolcount = 1;

            int lastforumid = 0;

            int subforumcount = 0;


            int forum__loop__id = 0;
            foreach (IndexPageForumInfo forum in forumlist)
            {
                forum__loop__id++;


                if (forum.Layer == 0)
                {


                    if (lastforumlayer > -1)
                    {


                        if (lastcolcount != 1)
                        {


                            if (subforumcount != 0)
                            {

                                for (int i = 0; i < lastcolcount - subforumcount; i++)
                                {
                                    templateBuilder.Append("<td>&nbsp;</td>");
                                }

                                templateBuilder.Append("		</tr>\r\n");

                            }	//end if

                            templateBuilder.Append("		</table>\r\n");
                            templateBuilder.Append("</div>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("		</table>\r\n");
                            templateBuilder.Append("	</div>			\r\n");

                        }	//end if

                        templateBuilder.Append("<div id=\"ad_intercat_" + lastforumid.ToString() + "\"></div>\r\n");

                    }	//end if


                    if (forum.Colcount == 1)
                    {

                        templateBuilder.Append("<div class=\"mainbox forumlist\">\r\n");
                        templateBuilder.Append("	<span class=\"headactions\">\r\n");

                        if (forum.Moderators != "")
                        {

                            templateBuilder.Append("			分类版主: " + forum.Moderators.ToString().Trim() + "\r\n");

                        }	//end if

                        templateBuilder.Append("<img id=\"category_" + forum.Fid.ToString().Trim() + "_img\"  \r\n");

                        if (forum.Collapse != "")
                        {

                            templateBuilder.Append("		src=\"templates/" + templatepath.ToString() + "/images/collapsed_yes.gif\"\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("		src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\"\r\n");

                        }	//end if

                        templateBuilder.Append("		 alt=\"展开/收起\" onclick=\"toggle_collapse('category_" + forum.Fid.ToString().Trim() + "');\"/>\r\n");
                        templateBuilder.Append("	</span>\r\n");
                        templateBuilder.Append("	<h3>\r\n");
                        aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid, 0);

                        templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">" + forum.Name.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("	</h3>	\r\n");
                        templateBuilder.Append("	<table id=\"category_" + forum.Fid.ToString().Trim() + "\" summary=\"category_" + forum.Fid.ToString().Trim() + "\" cellspacing=\"0\" cellpadding=\"0\"  style=\"" + forum.Collapse.ToString().Trim() + "\">\r\n");
                        templateBuilder.Append("	<thead class=\"category\">\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>版块</th>\r\n");
                        templateBuilder.Append("			<td class=\"nums\">主题</td>\r\n");
                        templateBuilder.Append("			<td class=\"nums\">帖子</td>\r\n");
                        templateBuilder.Append("			<td class=\"lastpost\">最后发表</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("	</thead>\r\n");

                    }
                    else
                    {

                        subforumcount = 0;

                        templateBuilder.Append("<div class=\"mainbox forumlist\">\r\n");
                        templateBuilder.Append("	<span class=\"headactions\">\r\n");

                        if (forum.Moderators != "")
                        {

                            templateBuilder.Append("			分类版主: " + forum.Moderators.ToString().Trim() + "\r\n");

                        }	//end if

                        templateBuilder.Append("		<img id=\"category_" + forum.Fid.ToString().Trim() + "_img\"\r\n");

                        if (forum.Collapse != "")
                        {

                            templateBuilder.Append("		src=\"templates/" + templatepath.ToString() + "/images/collapsed_yes.gif\"\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("		src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\"\r\n");

                        }	//end if

                        templateBuilder.Append("		alt=\"展开/收起\" onclick=\"toggle_collapse('category_" + forum.Fid.ToString().Trim() + "');\"/>\r\n");
                        templateBuilder.Append("	</span>\r\n");
                        templateBuilder.Append("	<h3>\r\n");
                        aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid, 0);

                        templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">" + forum.Name.ToString().Trim() + "</a>					\r\n");
                        templateBuilder.Append("	</h3>\r\n");
                        templateBuilder.Append("	<table id=\"category_" + forum.Fid.ToString().Trim() + "\" summary=\"category_" + forum.Fid.ToString().Trim() + "\" cellspacing=\"0\" cellpadding=\"0\"  style=\"" + forum.Collapse.ToString().Trim() + "\">	\r\n");

                    }	//end if

                    lastforumlayer = 0;

                    lastcolcount = forum.Colcount;

                    lastforumid = forum.Fid;


                }
                else
                {


                    if (forum.Colcount == 1)
                    {

                        templateBuilder.Append("		<tbody id=\"forum" + forum.Fid.ToString().Trim() + "\">\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        aspxrewriteurl = this.ShowTopicAspxRewrite(forum.Lasttid, 0);

                        templateBuilder.Append("				<th \r\n");

                        if (forum.Havenew == "new")
                        {

                            templateBuilder.Append("class=\"new\"\r\n");

                        }	//end if

                        templateBuilder.Append(">\r\n");

                        if (forum.Icon != "")
                        {

                            templateBuilder.Append("					<img src=\"" + forum.Icon.ToString().Trim() + "\" border=\"0\" align=\"left\" hspace=\"5\" alt=\"" + forum.Name.ToString().Trim() + "\"/>\r\n");

                        }	//end if

                        templateBuilder.Append("					<h2>\r\n");

                        if (forum.Redirect == "")
                        {

                            aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid, 0);

                            templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\">\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						<a href=\"" + forum.Redirect.ToString().Trim() + "\" target=\"_blank\">\r\n");

                        }	//end if

                        templateBuilder.Append("					" + forum.Name.ToString().Trim() + "</a>\r\n");

                        if (forum.Todayposts > 0)
                        {

                            templateBuilder.Append("<em>(" + forum.Todayposts.ToString().Trim() + ")</em>\r\n");

                        }	//end if

                        templateBuilder.Append("					</h2>\r\n");

                        if (forum.Description != "")
                        {

                            templateBuilder.Append("<p>" + forum.Description.ToString().Trim() + "</p>\r\n");

                        }	//end if


                        if (forum.Moderators != "")
                        {

                            templateBuilder.Append("<p class=\"moderators\">版主: " + forum.Moderators.ToString().Trim() + "</p>\r\n");

                        }	//end if

                        templateBuilder.Append("				</th>\r\n");
                        templateBuilder.Append("				<td class=\"nums\">\r\n");

                        if (forum.Istrade != 1)
                        {

                            templateBuilder.Append("" + forum.Topics.ToString().Trim() + "\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("</td>\r\n");
                        templateBuilder.Append("				<td class=\"nums\">\r\n");

                        if (forum.Istrade != 1)
                        {

                            templateBuilder.Append("" + forum.Posts.ToString().Trim() + "\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("</td>\r\n");
                        templateBuilder.Append("				<td class=\"lastpost\">\r\n");

                        if (forum.Istrade != 1)
                        {


                            if (forum.Status == -1)
                            {

                                templateBuilder.Append("					私密版块\r\n");

                            }
                            else
                            {


                                if (forum.Lasttid != 0)
                                {

                                    templateBuilder.Append("					<p>\r\n");
                                    aspxrewriteurl = this.ShowTopicAspxRewrite(forum.Lasttid, 0);

                                    templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\">" + forum.Lasttitle.ToString().Trim() + "</a>\r\n");
                                    templateBuilder.Append("					</p>\r\n");
                                    templateBuilder.Append("					<div class=\"topicbackwriter\">by\r\n");

                                    if (forum.Lastposter != "")
                                    {


                                        if (forum.Lastposterid == -1)
                                        {

                                            templateBuilder.Append("								游客\r\n");

                                        }
                                        else
                                        {

                                            aspxrewriteurl = this.UserInfoAspxRewrite(forum.Lastposterid);

                                            templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + forum.Lastposter.ToString().Trim() + "</a>\r\n");

                                        }	//end if


                                    }
                                    else
                                    {

                                        templateBuilder.Append("							匿名\r\n");

                                    }	//end if

                                    templateBuilder.Append("						- <a href=\"showtopic.aspx?topicid=" + forum.Lasttid.ToString().Trim() + "&page=end#lastpost\" title=\"" + forum.Lastpost.ToString().Trim() + "\"><span><script type=\"text/javascript\">document.write(convertdate('" + forum.Lastpost.ToString().Trim() + "'));</" + "script></span></a>\r\n");
                                    templateBuilder.Append("					</div>\r\n");

                                }
                                else
                                {

                                    templateBuilder.Append("						从未\r\n");

                                }	//end if


                            }	//end if


                        }
                        else
                        {

                            templateBuilder.Append("				   <p>" + forum.Description.ToString().Trim() + "</p>\r\n");

                        }	//end if

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }
                    else
                    {

                        subforumcount = subforumcount + 1;

                        double colwidth = 99.9 / forum.Colcount;


                        if (subforumcount == 1)
                        {

                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("		<tr>\r\n");

                        }	//end if

                        templateBuilder.Append("			<th style=\"width:" + colwidth.ToString() + "%;\"\r\n");

                        if (forum.Havenew == "new")
                        {

                            templateBuilder.Append("class=\"new\"\r\n");

                        }	//end if

                        templateBuilder.Append(">\r\n");
                        templateBuilder.Append("				<h2>\r\n");

                        if (forum.Icon != "")
                        {

                            templateBuilder.Append("					<img src=\"" + forum.Icon.ToString().Trim() + "\" border=\"0\" align=\"left\" hspace=\"5\" alt=\"" + forum.Name.ToString().Trim() + "\"/>\r\n");

                        }	//end if


                        if (forum.Redirect == "")
                        {

                            aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid, 0);

                            templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\">\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					<a href=\"" + forum.Redirect.ToString().Trim() + "\" target=\"_blank\">\r\n");

                        }	//end if

                        templateBuilder.Append("				" + forum.Name.ToString().Trim() + "</a>\r\n");

                        if (forum.Todayposts > 0)
                        {

                            templateBuilder.Append("				<em>(" + forum.Todayposts.ToString().Trim() + ")</em>\r\n");

                        }	//end if

                        templateBuilder.Append("				</h2>\r\n");
                        templateBuilder.Append("				<p>\r\n");

                        if (forum.Istrade != 1)
                        {

                            templateBuilder.Append("主题:" + forum.Topics.ToString().Trim() + ", 帖数:" + forum.Posts.ToString().Trim() + "\r\n");

                        }	//end if

                        templateBuilder.Append("</p>\r\n");

                        if (forum.Istrade != 1)
                        {


                            if (forum.Status == -1)
                            {

                                templateBuilder.Append("				<p>私密版块</p>\r\n");

                            }
                            else
                            {


                                if (forum.Lasttid != 0)
                                {

                                    templateBuilder.Append("						<p>最后: <a href=\"showtopic.aspx?topicid=" + forum.Lasttid.ToString().Trim() + "&page=end#lastpost\" title=\"" + forum.Lasttitle.ToString().Trim() + "\"><span><script type=\"text/javascript\">document.write(convertdate('" + forum.Lastpost.ToString().Trim() + "'));</" + "script></span></a> by \r\n");

                                    if (forum.Lastposter != "")
                                    {


                                        if (forum.Lastposterid == -1)
                                        {

                                            templateBuilder.Append("									游客\r\n");

                                        }
                                        else
                                        {

                                            aspxrewriteurl = this.UserInfoAspxRewrite(forum.Lastposterid);

                                            templateBuilder.Append("									<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + forum.Lastposter.ToString().Trim() + "</a>\r\n");

                                        }	//end if


                                    }
                                    else
                                    {

                                        templateBuilder.Append("								匿名\r\n");

                                    }	//end if

                                    templateBuilder.Append("						</p>\r\n");

                                }	//end if


                            }	//end if


                        }
                        else
                        {

                            templateBuilder.Append("				  <p>" + forum.Description.ToString().Trim() + "</p>\r\n");

                        }	//end if

                        templateBuilder.Append("			</th>\r\n");

                        if (subforumcount == forum.Colcount)
                        {

                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</tbody>\r\n");
                            subforumcount = 0;


                        }	//end if


                    }	//end if

                    lastforumlayer = 1;

                    lastcolcount = forum.Colcount;


                }	//end if


            }	//end loop


            if (lastcolcount != 1 && subforumcount != 0)
            {

                for (int i = 0; i < lastcolcount - subforumcount; i++)
                {
                    templateBuilder.Append("<td>&nbsp;</td>");
                }

                templateBuilder.Append("		</tr>\r\n");

            }	//end if

            templateBuilder.Append("	</table>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--end topic-->\r\n");


            if (config.Enabletag == 1)
            {

                templateBuilder.Append("<!--tag-->\r\n");
                templateBuilder.Append("<table cellspacing=\"1\" cellpadding=\"0\" class=\"portalbox\" summary=\"HeadBox\">\r\n");
                templateBuilder.Append("<tbody>\r\n");
                templateBuilder.Append("	<tr>\r\n");
                templateBuilder.Append("	<td>\r\n");
                templateBuilder.Append("		<div id=\"hottags\">\r\n");
                templateBuilder.Append("			<h3><a target=\"_blank\" href=\"tags.aspx\">热门标签</a></h3>\r\n");
                templateBuilder.Append("			<ul id=\"forumhottags\">\r\n");

                int tag__loop__id = 0;
                foreach (TagInfo tag in taglist)
                {
                    tag__loop__id++;

                    templateBuilder.Append("					<li><a \r\n");

                    if (config.Aspxrewrite == 1)
                    {

                        templateBuilder.Append("					href=\"topictag-" + tag.Tagid.ToString().Trim() + ".aspx\" \r\n");

                    }
                    else
                    {

                        templateBuilder.Append("					href=\"tags.aspx?t=topic&tagid=" + tag.Tagid.ToString().Trim() + "\" \r\n");

                    }	//end if


                    if (tag.Color != "")
                    {

                        templateBuilder.Append("					style=\"color: #" + tag.Color.ToString().Trim() + ";\"\r\n");

                    }	//end if

                    templateBuilder.Append("					title=\"" + tag.Fcount.ToString().Trim() + "\">" + tag.Tagname.ToString().Trim() + "</a></li>\r\n");

                }	//end loop

                templateBuilder.Append("			</ul>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</td>\r\n");
                templateBuilder.Append("	</tr>\r\n");
                templateBuilder.Append("</tbody> \r\n");
                templateBuilder.Append("</table>\r\n");
                templateBuilder.Append("<!--tag end-->\r\n");

            }	//end if




            if (forumlinkcount > 0)
            {

                templateBuilder.Append("<div class=\"box\">\r\n");
                templateBuilder.Append("	<span class=\"headactions\"><img id=\"forumlinks_img\" src=\"templates/" + templatepath.ToString() + "/images/collapsed_no.gif\" alt=\"\" onclick=\"toggle_collapse('forumlinks');\"/></span>\r\n");
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
                        //templateBuilder.Append("				<a href=\"" + forumlink["url"].ToString().Trim() + "\" target=\"_blank\"><img src=\"" + forumlink["logo"].ToString().Trim() + "\" alt=\"" + forumlink["name"].ToString().Trim() + "\"  class=\"forumlink_logo\"/></a>\r\n");
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

                templateBuilder.Append("</a>\r\n");
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
                    foreach (OnlineUserInfo onlineuser in onlineuserlist)
                    {
                        onlineuser__loop__id++;

                        if (onlineuser.Invisible == 1)
                        {
                            invisiblecount = invisiblecount + 1;
                            templateBuilder.Append("				<li>(隐身会员)</li>\r\n");
                        }
                        else
                        {
                            templateBuilder.Append("				<li>" + onlineuser.Olimg.ToString().Trim() + "\r\n");
                            if (onlineuser.Userid == -1)
                            {
                                templateBuilder.Append("							" + onlineuser.Username.ToString().Trim() + "\r\n");
                            }
                            else
                            {
                                aspxrewriteurl = this.UserInfoAspxRewrite(onlineuser.Userid);

                                templateBuilder.Append("							<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\" title=\"时间: " + onlineuser.Lastupdatetime.ToString().Trim() + "\r\n");
                                templateBuilder.Append("操作: " + onlineuser.Actionname.ToString().Trim() + "\r\n");

                                if (onlineuser.Forumname != "")
                                {
                                    templateBuilder.Append("版块: " + onlineuser.Forumname.ToString().Trim() + "\r\n");
                                }	//end if
                                templateBuilder.Append("	\">" + onlineuser.Username.ToString().Trim() + "</a>\r\n");
                            }	//end if
                            templateBuilder.Append("				</li>\r\n");
                        }	//end if
                    }	//end loop

                    if (GeneralConfigs.GetConfig().Whosonlinecontract == 0 && totalonlineguest != 0)
                    {
                        int wrongCount = totalonline - totalonlineuser - (totalonlineguest / 10);
                        for (int i = 0; i < wrongCount; i++)
                        {
                            templateBuilder.Append("				<li><img src=\"images/groupicons/guest.gif\" />\r\n游客");


                            templateBuilder.Append("				</li>\r\n");

                        }
                    }

                    if (invisiblecount > 0)
                    {
                        templateBuilder.Append("					<script type=\"text/javascript\">$('invisible').innerHTML = '(" + invisiblecount.ToString() + "' + \" 隐身)\";</" + "script>\r\n");
                    }	//end if
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

            templateBuilder.Append("<div class=\"legend\">\r\n");
            templateBuilder.Append("	<label><img src=\"templates/" + templatepath.ToString() + "/images/forum_new.gif\" alt=\"有新帖的版块\" />有新帖的版块</label>\r\n");
            templateBuilder.Append("	<label><img src=\"templates/" + templatepath.ToString() + "/images/forum.gif\" alt=\"无新帖的版块\" />无新帖的版块</label>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (config.Forumjump == 1)
            {
                templateBuilder.Append("	" + navhomemenu.ToString() + "\r\n");
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

            templateBuilder.Append("" + mediaad.ToString() + "\r\n");
            templateBuilder.Append("" + inforumad.ToString() + "\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");

            Response.Write(templateBuilder.ToString());
        }
    }
}
