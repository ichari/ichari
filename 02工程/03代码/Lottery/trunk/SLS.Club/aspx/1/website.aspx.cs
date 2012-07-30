using System;
using System.Data;
using Discuz.Common;

#if NET1
#else
using Discuz.Common.Generic;
#endif

using Discuz.Forum;
using Discuz.Entity;
using Discuz.Aggregation;

namespace Discuz.Web
{
    /// <summary>
    /// 聚合首页
    /// </summary>
    public partial class website : PageBase
    {
        /// <summary>
        /// 论坛推荐主题
        /// </summary>
        public PostInfo[] postlist;
        /// <summary>
        /// 论坛聚合主题
        /// </summary>
        public DataTable topiclist;
        /// <summary>
        /// 用户聚合数据
        /// </summary>
        public DataTable userlist;
        /// <summary>
        /// 论坛新帖
        /// </summary>
        public DataTable newtopiclist;
        /// <summary>
        /// 论坛热帖
        /// </summary>
        public DataTable hottopiclist;
        /// <summary>
        /// 论坛热门版块
        /// </summary>
        public DataTable hotforumlist;
        /// <summary>
        /// 友情链接列表
        /// </summary>
        public DataTable forumlinklist;
        /// <summary>
        /// 友情链接数量
        /// </summary>
        public int forumlinkcount;
        /// <summary>
        /// 公告数量
        /// </summary>
        public int announcementcount;
        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;
        /// <summary>
        /// 图片轮显数据
        /// </summary>
        public string rotatepicdata = "";

        public ForumAggregationData forumagg = Discuz.Aggregation.AggregationFacade.ForumAggregation;

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
        /// 推荐的版块id列表
        /// </summary>
        public int[] forumidarray;
        /// <summary>
        /// 标签列表
        /// </summary>
        public TagInfo[] taglist;

        /// <summary>
        /// 分类间广告
        /// </summary>
        public string inforumad;
        /// <summary>
        /// 对联广告
        /// </summary>
        public string doublead;
        /// <summary>
        /// 浮动广告
        /// </summary>
        public string floatad;

        protected override void ShowPage()
        {
            pagetitle = "首页";

            postlist = AggregationFacade.ForumAggregation.GetPostListFromFile("Website");

            topiclist = AggregationFacade.ForumAggregation.GetForumTopicList();

            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");
            announcementcount = 0;
            if (announcementlist != null)
            {
                announcementcount = announcementlist.Rows.Count;
            }

            // 友情链接
            forumlinklist = Caches.GetForumLinkList();

            rotatepicdata = AggregationFacade.BaseAggregation.GetRotatePicData();

            Forums.GetForumIndexCollection(config.Hideprivate, usergroupid, config.Moddisplay, out totaltopic, out totalpost, out todayposts);

            // 获得统计信息
            totalusers = Utils.StrToInt(Statistics.GetStatisticsRowItem("totalusers"), 0);
            lastusername = Statistics.GetStatisticsRowItem("lastusername");
            lastuserid = Utils.StrToInt(Statistics.GetStatisticsRowItem("lastuserid"), 0);
            yesterdayposts = Utils.StrToInt(Statistics.GetStatisticsRowItem("yesterdayposts"), 0);
            highestposts = Utils.StrToInt(Statistics.GetStatisticsRowItem("highestposts"), 0);
            highestpostsdate = Statistics.GetStatisticsRowItem("highestpostsdate").ToString().Trim();
            if (todayposts > highestposts)
            {
                highestposts = todayposts;
                highestpostsdate = DateTime.Now.ToString("yyyy-M-d");
            }
            totalonline = onlineusercount;
            OnlineUsers.GetOnlineUserCollection(out totalonline, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);

            highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
            highestonlineusertime = Statistics.GetStatisticsRowItem("highestonlineusertime");


            forumidarray = AggregationFacade.ForumAggregation.GetRecommendForumID();

            forumlinklist = Caches.GetForumLinkList();
            forumlinkcount = forumlinklist.Rows.Count;

            if (config.Enabletag == 1)
            {
                taglist = ForumTags.GetCachedHotForumTags(config.Hottagcount);
            }
            else
            {
                taglist = new TagInfo[0];
            }

            doublead = Advertisements.GetDoubleAd("indexad", 0);
            floatad = Advertisements.GetFloatAd("indexad", 0);

        }

        override protected void OnLoad(EventArgs e)
{

	/* 
		This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:13.
		本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:13. 
	*/

	base.OnLoad(e);

            
            templateBuilder.Append("<div id=\"infobox\">	\r\n");
            templateBuilder.Append("	<div id=\"ntforumposition\">\r\n");
            templateBuilder.Append("	<div class=\"ntforumnote\">\r\n");
            templateBuilder.Append("		<dl>\r\n");
            templateBuilder.Append("			<dd style=\"font-weight:bold;\"><a href=\"" + forumurl.ToString() + "forumindex.aspx\">" + config.Forumtitle.ToString().Trim() + "</a></dd>\r\n");

            if (announcementcount > 0)
            {

                templateBuilder.Append("			<dt>公告:</dt>\r\n");
                templateBuilder.Append("			<dd style=\"width:400px;\">\r\n");
                templateBuilder.Append("				<marquee width=\"98%\" direction=\"left\" scrollamount=\"2\" scrolldelay=\"1\" onmouseover=\"this.stop();\" onmouseout=\"this.start();\">\r\n");

                int announcement__loop__id = 0;
                foreach (DataRow announcement in announcementlist.Rows)
                {
                    announcement__loop__id++;

                    templateBuilder.Append("					<a href=\"" + forumurl.ToString() + "announcement.aspx#" + announcement["id"].ToString().Trim() + "\">" + announcement["title"].ToString().Trim() + "</a><cite>\r\n");
                    templateBuilder.Append(Convert.ToDateTime(announcement["starttime"].ToString().Trim()).ToString("MM.dd"));
                    templateBuilder.Append("</cite>\r\n");

                }	//end loop

                templateBuilder.Append("				</marquee>			\r\n");
                templateBuilder.Append("			</dd>\r\n");

            }	//end if

            templateBuilder.Append("		</dl>\r\n");
            templateBuilder.Append("	</div>		    \r\n");
            templateBuilder.Append("	<div class=\"ntforumsearch\">\r\n");
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
            templateBuilder.Append("    </div>\r\n");
            templateBuilder.Append("</div>\r\n");



            if (page_err == 0)
            {


                /*
                聚合首面方法说明
	
                ///////////////////////////////////////////////////////////////////////////////////////////////
	
                方法名称: GetForumTopicList(count, views, forumid, timetype, ordertype, isdigest, onlyimg)
                方法说明: 返回指定条件的主题列表信息
                参数说明:
                          count : 返回的主题数 
                          views : 浏览量 [返回等于或大于当前浏览量的主题]
                          forumid : 版块ID [默认值 0 为所有版块]
                          timetype : 指定时间段内的主题 [ TopicTimeType.Day(一天内)  , TopicTimeType.Week(一周内),   TopicTimeType.Month(一个月内),   TopicTimeType.SixMonth(六个月内),  TopicTimeType.Year(一年内),  TopicTimeType.All(默认 从1754-1-1至今的所有主题)
                          ordertype : 排序字段(降序) [TopicOrderType.ID(默认 主题ID) , TopicOrderType.Views(浏览量),   TopicOrderType.LastPost(最后回复),    TopicOrderType.PostDataTime(按最新主题查),    TopicOrderType.Digest(按精华主题查),    TopicOrderType.Replies(按回复数)]  
                          isdigest : 是否精化 [true(仅返回精华主题)   false(不加限制)]
                          onlyimg : 是否包含附件 [true(仅返回包括图片附件的主题)   false(不加限制)]
	      
                //////////////////////////////////////////////////////////////////////////////////////////////    
	
                方法名称: GetHotForumList(count)   
                方法说明: 返回指定数量的热门版块列表
                参数说明:
                          count : 返回的版块数
	    
                //////////////////////////////////////////////////////////////////////////////////////////////      
	
                方法名称: GetForumList(forumid)   
                方法说明: 返回指定版块下的所有子段块列表
                参数说明:
                          forumid : 指定的版块id
	      
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetLastPostList(forumid, count)   
                方法说明: 返回指定版块下的最新回帖列表
                参数说明:
                          forumid : 指定的版块id     
                          count : 返回的回帖数
	 
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetAlbumList(photoconfig.Focusalbumshowtype, count, days)   
                方法说明: 返回指定条件的相册列表
                参数说明:
                          photoconfig.Focusalbumshowtype : 排序字段(降序) [1(浏览量), 2(照片数), 3(创建时间)]    注:管理后台聚合设置项
                          count : 返回的相册数
                          days :有效天数 [指定天数内的相册]
	      
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetWeekHotPhotoList(photoconfig.Weekhot)
                方法说明: 返回指定数量的热门图片
                参数说明:
                          photoconfig.Weekhot : 返回的热图数量  注:管理后台聚合设置项
	          
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetSpaceTopComments(count)
                方法说明: 返回指定数量的空间最新评论
                参数说明:
                          count : 返回的评论数
	          
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetRecentUpdateSpaceList(count)
                方法说明: 返回指定数量的最新更新空间列表
                参数说明:
                          count : 返回的空间信息数
	
	
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetGoodsList(condition, orderby, categoryid, count)
                方法说明: 返回指定数量的最新更新空间列表
                参数说明:
                          condition : 条件 [recommend(仅返回推荐商品, 商城模式下可用) , quality_new(仅返回全新(状态)商品),    quality_old(仅返回二手(状态)商品)]  
                          orderby: 排序字段(降序) [viewcount(按浏览量排序),    hotgoods(按商品交易量排序),  newgoods(按发布商品先后顺序排序) ]
                          categoryid : 商品所属分类id [默认值 0 为不加限制]
                          count : 返回的商品数
	          
	 
                //////////////////////////////////////////////////////////////////////////////////////////////  
	
                方法名称: GetUserList(count, orderby)
                方法说明: 返回指定数量及排序方式的用户列表
                参数说明:
                          count : 返回的用户数       
                          orderby: 排序字段(降序) [credits(用户金币), posts(用户发帖数), lastactivity(最后活动时间), joindate(注册时间), oltime(在线时间)]
                */

                templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_website.js\"></" + "script>\r\n");
                templateBuilder.Append("<div class=\"mainbox\">\r\n");
                templateBuilder.Append("	<div class=\"box firstbox\">\r\n");
                templateBuilder.Append("		<div class=\"focusbox\">\r\n");

                if (rotatepicdata != null && rotatepicdata != "")
                {

                    templateBuilder.Append("			<div class=\"modulebox sidebox\" style=\"padding:1px;\">\r\n");
                    templateBuilder.Append("				<script type='text/javascript'>\r\n");
                    templateBuilder.Append("				var imgwidth = 237;\r\n");
                    templateBuilder.Append("				var imgheight = 210;\r\n");
                    templateBuilder.Append("				</" + "script>			\r\n");
                    templateBuilder.Append("				<!--图片轮换代码开始-->\r\n");
                    templateBuilder.Append("				<script type=\"text/javascript\" src=\"javascript/template_rotatepic.js\"></" + "script>\r\n");
                    templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("				var data = { };\r\n");
                    templateBuilder.Append("				" + rotatepicdata.ToString() + "\r\n");
                    templateBuilder.Append("				var ri = new MzRotateImage();\r\n");
                    templateBuilder.Append("				ri.dataSource = data;\r\n");
                    templateBuilder.Append("				ri.width = 237;\r\n");
                    templateBuilder.Append("				ri.height = 210;\r\n");
                    templateBuilder.Append("				ri.interval = 3000;\r\n");
                    templateBuilder.Append("				ri.duration = 2000;\r\n");
                    templateBuilder.Append("				document.write(ri.render());\r\n");
                    templateBuilder.Append("				</" + "script>\r\n");
                    templateBuilder.Append("				<!--图片轮换代码结束-->\r\n");
                    templateBuilder.Append("			</div>\r\n");

                }	//end if

                templateBuilder.Append("        <!--\r\n");
                templateBuilder.Append("			<div id=\"focusimg\"><img src=\"images/gather/img.gif\"/></div>\r\n");
                templateBuilder.Append("			<h3>春节前将为大家准备一个大礼包</h3>\r\n");
                templateBuilder.Append("			<div class=\"focuspage\"><a href=\"#\" class=\"current\">1</a><a href=\"#\">2</a><a href=\"#\">3</a><a href=\"#\">4</a><a href=\"#\">5</a></div>\r\n");
                templateBuilder.Append("		--->\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"box newtopicbox\">\r\n");
                templateBuilder.Append("		<h2>最新主题</h2>\r\n");

                if (postlist.Length > 0)
                {

                    templateBuilder.Append("		<dl>\r\n");

                    int __postinfo__loop__id = 0;
                    foreach (PostInfo __postinfo in postlist)
                    {
                        __postinfo__loop__id++;


                        if (__postinfo__loop__id == 1)
                        {

                            aspxrewriteurl = this.ShowTopicAspxRewrite(__postinfo.Tid, 0);

                            templateBuilder.Append("				<dt><strong><a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\">" + __postinfo.Title.ToString().Trim() + "</a></strong>\r\n");
                            templateBuilder.Append("				<cite>\r\n");

                            if (__postinfo.Posterid > 0)
                            {

                                aspxrewriteurl = this.UserInfoAspxRewrite(__postinfo.Posterid);

                                templateBuilder.Append("				    <a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + __postinfo.Poster.ToString().Trim() + "</a>\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("				    " + __postinfo.Poster.ToString().Trim() + "\r\n");

                            }	//end if

                            templateBuilder.Append("				      " + __postinfo.Postdatetime.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				</cite></dt>\r\n");
                            aspxrewriteurl = this.ShowTopicAspxRewrite(__postinfo.Tid, 0);

                            templateBuilder.Append("				<dd>" + __postinfo.Message.ToString().Trim() + " <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">详细</a></dd>\r\n");

                        }	//end if


                    }	//end loop

                    templateBuilder.Append("		</dl>\r\n");

                }	//end if

                templateBuilder.Append("		<ul class=\"topiclist\">\r\n");

                if (postlist.Length > 0)
                {

                    topiclist = forumagg.GetForumTopicList(6, 0, 0, TopicTimeType.All, TopicOrderType.PostDataTime, false, false);


                }
                else
                {

                    topiclist = forumagg.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.PostDataTime, false, false);


                }	//end if


                int __newtopicinfo__loop__id = 0;
                foreach (DataRow __newtopicinfo in topiclist.Rows)
                {
                    __newtopicinfo__loop__id++;

                    templateBuilder.Append("            <li>\r\n");
                    templateBuilder.Append("                <cite>\r\n");
                    aspxrewriteurl = this.ShowForumAspxRewrite(__newtopicinfo["fid"].ToString().Trim(), 0);

                    templateBuilder.Append("                    <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + __newtopicinfo["name"].ToString().Trim() + "</a>\r\n");
                    templateBuilder.Append("                </cite>\r\n");
                    aspxrewriteurl = this.ShowTopicAspxRewrite(__newtopicinfo["tid"].ToString().Trim(), 0);

                    templateBuilder.Append("                <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                    templateBuilder.Append(Utils.GetUnicodeSubString(__newtopicinfo["title"].ToString().Trim(), 43, "..."));
                    templateBuilder.Append("</a>\r\n");
                    templateBuilder.Append("           </li>            \r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"box sidebox\">\r\n");
                templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\" id=\"li_hotforum\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_hotforum'));\">热门版块</a></li><li id=\"li_bbsmessage\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_bbsmessage'));\">论坛信息</a></li>\r\n");

                if (config.Enabletag == 1)
                {

                    templateBuilder.Append("<li id=\"li_hottags\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_hottags'));\">标签</a></li>\r\n");

                }	//end if

                templateBuilder.Append("</ul></div>\r\n");
                templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                templateBuilder.Append("		<ul id=\"hotforum\">\r\n");
                hotforumlist = forumagg.GetHotForumList(10);


                int __foruminfo__loop__id = 0;
                foreach (DataRow __foruminfo in hotforumlist.Rows)
                {
                    __foruminfo__loop__id++;

                    aspxrewriteurl = this.ShowForumAspxRewrite(__foruminfo["fid"].ToString().Trim(), 0);

                    templateBuilder.Append("			<li><em>" + __foruminfo["topics"].ToString().Trim() + "</em><cite \r\n");

                    if (__foruminfo__loop__id == 1)
                    {

                        templateBuilder.Append("class=\"first\"\r\n");

                    }	//end if


                    if (__foruminfo__loop__id == 2)
                    {

                        templateBuilder.Append("class=\"second\"\r\n");

                    }	//end if


                    if (__foruminfo__loop__id == 3)
                    {

                        templateBuilder.Append("class=\"third\"\r\n");

                    }	//end if

                    templateBuilder.Append(" > " + __foruminfo__loop__id.ToString() + "</cite><a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + __foruminfo["name"].ToString().Trim() + "</a></li>\r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		<ul id=\"bbsmessage\"  style=\"display:none;\">\r\n");
                templateBuilder.Append("			<li>会员总数: <i>" + totalusers.ToString() + "</i>人</li>\r\n");
                templateBuilder.Append("			<li>最新注册会员:<i>\r\n");
                aspxrewriteurl = this.UserInfoAspxRewrite(lastuserid);

                templateBuilder.Append("<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + lastusername.ToString() + "</a></i></li>\r\n");
                templateBuilder.Append("			<li>主题数:<i>" + totaltopic.ToString() + "</i>主题</li>\r\n");
                templateBuilder.Append("			<li>帖子数:<i>" + totalpost.ToString() + "</i> 个(含回帖) </li>\r\n");
                templateBuilder.Append("			<li>今  日:<i>" + todayposts.ToString() + "</i>帖  昨 日: <i>" + yesterdayposts.ToString() + "</i> 帖</li>\r\n");

                if (highestpostsdate != "")
                {

                    templateBuilder.Append("			    <li>	\r\n");
                    templateBuilder.Append("		            最高日:<i>" + highestposts.ToString() + "</i>帖\r\n");
                    templateBuilder.Append("		        </li>\r\n");
                    templateBuilder.Append("		        <li>	\r\n");
                    templateBuilder.Append("		            最高发帖日:<i>" + highestpostsdate.ToString() + "</i>\r\n");
                    templateBuilder.Append("		        </li>\r\n");

                }	//end if

                templateBuilder.Append("			<li>在线总数:<i>" + totalonline.ToString() + "</i>人</li>\r\n");
                templateBuilder.Append("			<li>最高在线:<i>" + highestonlineusercount.ToString() + "</i> 人 </li>\r\n");
                templateBuilder.Append("			<li>最高在线发生于:<i>" + highestonlineusertime.ToString() + "</i></li>\r\n");
                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		<div id=\"hottags\"  class=\"forumtag\" style=\"display:none;\">\r\n");

                if (taglist.Length > 0)
                {


                    int tag__loop__id = 0;
                    foreach (TagInfo tag in taglist)
                    {
                        tag__loop__id++;

                        templateBuilder.Append("				<a \r\n");

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

                        templateBuilder.Append("					title=\"" + tag.Fcount.ToString().Trim() + "\">" + tag.Tagname.ToString().Trim() + "</a>\r\n");

                    }	//end loop


                }
                else
                {

                    templateBuilder.Append("		    暂无数据!\r\n");

                }	//end if

                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("var reco_topic = " + forumagg.GetTopicJsonFromFile().ToString().Trim() + ";\r\n");
                templateBuilder.Append("var templatepath = \"" + templatepath.ToString() + "\";\r\n");
                templateBuilder.Append("var aspxrewrite = " + config.Aspxrewrite.ToString().Trim() + ";\r\n");
                templateBuilder.Append("</" + "script>\r\n");

                int forumid__loop__id = 0;
                foreach (int forumid in forumidarray)
                {
                    forumid__loop__id++;

                    ForumInfo foruminfo = Forums.GetForumInfo(forumid);


                    if (foruminfo != null)
                    {

                        templateBuilder.Append("<div class=\"mainbox\">\r\n");
                        templateBuilder.Append("	<div class=\"box topicbox\">\r\n");
                        templateBuilder.Append("		<span>\r\n");

                        int sub_forum__loop__id = 0;
                        foreach (DataRow sub_forum in Forums.GetForumList(forumid).Rows)
                        {
                            sub_forum__loop__id++;


                            if (sub_forum__loop__id <= 5)
                            {

                                aspxrewriteurl = this.ShowForumAspxRewrite(sub_forum["fid"].ToString().Trim(), 0);

                                templateBuilder.Append("		    <a href=\"" + aspxrewriteurl.ToString() + "\" tabindex=\"_blank\">" + sub_forum["name"].ToString().Trim() + "</a>\r\n");

                            }	//end if


                        }	//end loop

                        aspxrewriteurl = this.ShowForumAspxRewrite(forumid, 0);

                        templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\" tabindex=\"_blank\">更多&gt;&gt;</a>\r\n");
                        templateBuilder.Append("		</span>\r\n");
                        templateBuilder.Append("		<h2><a href=\"" + aspxrewriteurl.ToString() + "\" tabindex=\"_blank\">" + foruminfo.Name.ToString().Trim() + "</a></h2>\r\n");
                        templateBuilder.Append("		<script type=\"text/javascript\">document.write(showtopicinfo(" + forumid.ToString() + ", " + forumid__loop__id.ToString() + "-1));</" + "script>\r\n");
                        templateBuilder.Append("		<ul class=\"topiclist\">\r\n");
                        topiclist = forumagg.GetForumTopicList(8, 0, forumid, TopicTimeType.All, TopicOrderType.PostDataTime, false, false);


                        int newtopicinfo__loop__id = 0;
                        foreach (DataRow newtopicinfo in topiclist.Rows)
                        {
                            newtopicinfo__loop__id++;

                            templateBuilder.Append("		   <li>\r\n");
                            templateBuilder.Append("                <cite>\r\n");
                            aspxrewriteurl = this.ShowForumAspxRewrite(newtopicinfo["fid"].ToString().Trim(), 0);

                            templateBuilder.Append("                    <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + newtopicinfo["name"].ToString().Trim() + "</a>\r\n");
                            templateBuilder.Append("                </cite>\r\n");
                            aspxrewriteurl = this.ShowTopicAspxRewrite(newtopicinfo["tid"].ToString().Trim(), 0);

                            templateBuilder.Append("                <a title=\"" + newtopicinfo["title"].ToString().Trim() + "\" href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                            templateBuilder.Append(Utils.GetUnicodeSubString(newtopicinfo["title"].ToString().Trim(), 43, "..."));
                            templateBuilder.Append("</a>\r\n");
                            templateBuilder.Append("           </li>   \r\n");

                        }	//end loop

                        templateBuilder.Append("		</ul>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("	<div class=\"box sidebox\">\r\n");
                        templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\" id=\"li_forum_" + forumid.ToString() + "_topic\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_" + forumid.ToString() + "_topic')," + forumid.ToString() + ");\">最热主题</a></li><li id=\"li_forum_" + forumid.ToString() + "_reply\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_" + forumid.ToString() + "_reply'), " + forumid.ToString() + ");\">最新回复</a></li><li id=\"li_forum_" + forumid.ToString() + "_digest\"><a href=\"javascript:;\" onclick=\"javascript:tabselect($('li_forum_" + forumid.ToString() + "_digest'), " + forumid.ToString() + ");\">精华</a></li></ul></div>\r\n");
                        templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                        templateBuilder.Append("		<ul id=\"forum_" + forumid.ToString() + "_topic\" class=\"topicdot\">\r\n");
                        topiclist = forumagg.GetForumTopicList(8, 0, forumid, TopicTimeType.All, TopicOrderType.Replies, false, false);


                        if (topiclist.Rows.Count > 0)
                        {


                            int hottopicinfo__loop__id = 0;
                            foreach (DataRow hottopicinfo in topiclist.Rows)
                            {
                                hottopicinfo__loop__id++;

                                aspxrewriteurl = this.ShowTopicAspxRewrite(hottopicinfo["tid"].ToString().Trim(), 0);

                                templateBuilder.Append("            <li><em>" + hottopicinfo["replies"].ToString().Trim() + "</em><a title=\"" + hottopicinfo["title"].ToString().Trim() + "\" href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Utils.GetUnicodeSubString(hottopicinfo["title"].ToString().Trim(), 28, "..."));
                                templateBuilder.Append("</a></li> \r\n");

                            }	//end loop


                        }
                        else
                        {

                            templateBuilder.Append("		    暂无数据!\r\n");

                        }	//end if

                        templateBuilder.Append("		</ul>\r\n");
                        templateBuilder.Append("		<ul id=\"forum_" + forumid.ToString() + "_reply\" class=\"topicdot\" style=\"display:none;\">\r\n");
                        topiclist = forumagg.GetForumTopicList(8, 0, forumid, TopicTimeType.All, TopicOrderType.LastPost, false, false);


                        if (topiclist.Rows.Count > 0)
                        {


                            int replytopic__loop__id = 0;
                            foreach (DataRow replytopic in topiclist.Rows)
                            {
                                replytopic__loop__id++;

                                templateBuilder.Append("            <li><a title=\"" + replytopic["title"].ToString().Trim() + "\" href=\"" + forumurl.ToString() + "showtopic.aspx?topicid=" + replytopic["tid"].ToString().Trim() + "&page=end#lastpost\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Utils.GetUnicodeSubString(replytopic["title"].ToString().Trim(), 28, "..."));
                                templateBuilder.Append("</a></li> \r\n");

                            }	//end loop


                        }
                        else
                        {

                            templateBuilder.Append("		    暂无数据!\r\n");

                        }	//end if

                        templateBuilder.Append("		</ul>\r\n");
                        templateBuilder.Append("		<ul id=\"forum_" + forumid.ToString() + "_digest\" class=\"topicdot\" style=\"display:none;\">\r\n");
                        topiclist = forumagg.GetForumTopicList(8, 0, forumid, TopicTimeType.All, TopicOrderType.LastPost, true, false);


                        if (topiclist.Rows.Count > 0)
                        {


                            int replytopic__loop__id = 0;
                            foreach (DataRow replytopic in topiclist.Rows)
                            {
                                replytopic__loop__id++;

                                templateBuilder.Append("            <li><a title=\"" + replytopic["title"].ToString().Trim() + "\" href=\"" + forumurl.ToString() + "showtopic.aspx?topicid=" + replytopic["tid"].ToString().Trim() + "&page=end#lastpost\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Utils.GetUnicodeSubString(replytopic["title"].ToString().Trim(), 28, "..."));
                                templateBuilder.Append("</a></li> \r\n");

                            }	//end loop


                        }
                        else
                        {

                            templateBuilder.Append("		    暂无数据!\r\n");

                        }	//end if

                        templateBuilder.Append("		</ul>\r\n");
                        templateBuilder.Append("		</div>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }	//end if


                }	//end loop

                templateBuilder.Append("<!--<div class=\"adinner\"><img src=\"images/gather/ad.gif\" alt=\"广告\"/></div>-->\r\n");
                
                templateBuilder.Append("<!--<div class=\"adinner\"><img src=\"images/gather/ad.gif\" alt=\"广告\"/></div>-->\r\n");
                templateBuilder.Append("<div id=\"statistics\" class=\"mainbox\">\r\n");
                templateBuilder.Append("	<div class=\"box sidebox\">\r\n");
                templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\"><a href=\"#\">论坛点击量排行</a></li></ul></div>\r\n");
                templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                templateBuilder.Append("		<ul id=\"topic1\">\r\n");
                topiclist = forumagg.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.Views, false, false);


                int views_topicinfo__loop__id = 0;
                foreach (DataRow views_topicinfo in topiclist.Rows)
                {
                    views_topicinfo__loop__id++;

                    templateBuilder.Append("            <li>\r\n");
                    templateBuilder.Append("                <em>" + views_topicinfo["views"].ToString().Trim() + "</em><cite \r\n");

                    if (views_topicinfo__loop__id == 1)
                    {

                        templateBuilder.Append("class=\"first\"\r\n");

                    }	//end if


                    if (views_topicinfo__loop__id == 2)
                    {

                        templateBuilder.Append("class=\"second\"\r\n");

                    }	//end if


                    if (views_topicinfo__loop__id == 3)
                    {

                        templateBuilder.Append("class=\"third\"\r\n");

                    }	//end if

                    templateBuilder.Append(">" + views_topicinfo__loop__id.ToString() + "</cite>\r\n");
                    aspxrewriteurl = this.ShowTopicAspxRewrite(views_topicinfo["tid"].ToString().Trim(), 0);

                    templateBuilder.Append("                <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                    templateBuilder.Append(Utils.GetUnicodeSubString(views_topicinfo["title"].ToString().Trim(), 20, "..."));
                    templateBuilder.Append("</a>\r\n");
                    templateBuilder.Append("           </li>            \r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"box sidebox\">\r\n");
                templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\"><a href=\"#\">论坛精华排行</a></li></ul></div>\r\n");
                templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                templateBuilder.Append("		<ul id=\"topic2\">\r\n");
                topiclist = forumagg.GetForumTopicList(10, 0, 0, TopicTimeType.All, TopicOrderType.Replies, true, false);


                int digest_topicinfo__loop__id = 0;
                foreach (DataRow digest_topicinfo in topiclist.Rows)
                {
                    digest_topicinfo__loop__id++;

                    templateBuilder.Append("           <li>\r\n");
                    templateBuilder.Append("                <em>" + digest_topicinfo["views"].ToString().Trim() + "</em><cite \r\n");

                    if (digest_topicinfo__loop__id == 1)
                    {

                        templateBuilder.Append("class=\"first\"\r\n");

                    }	//end if


                    if (digest_topicinfo__loop__id == 2)
                    {

                        templateBuilder.Append("class=\"second\"\r\n");

                    }	//end if


                    if (digest_topicinfo__loop__id == 3)
                    {

                        templateBuilder.Append("class=\"third\"\r\n");

                    }	//end if

                    templateBuilder.Append(">" + digest_topicinfo__loop__id.ToString() + "</cite>\r\n");
                    aspxrewriteurl = this.ShowTopicAspxRewrite(digest_topicinfo["tid"].ToString().Trim(), 0);

                    templateBuilder.Append("                <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                    templateBuilder.Append(Utils.GetUnicodeSubString(digest_topicinfo["title"].ToString().Trim(), 20, "..."));
                    templateBuilder.Append("</a>\r\n");
                    templateBuilder.Append("           </li>            \r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"box sidebox\">\r\n");
                templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\"><a href=\"#\">用户金币排行</a></li></ul></div>\r\n");
                templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                templateBuilder.Append("		<ul id=\"hottopic\">\r\n");
                userlist = forumagg.GetUserList(10, "credits");


                int credits_user__loop__id = 0;
                foreach (DataRow credits_user in userlist.Rows)
                {
                    credits_user__loop__id++;

                    templateBuilder.Append("		   <li>\r\n");
                    templateBuilder.Append("                <em>" + credits_user["credits"].ToString().Trim() + "</em><cite \r\n");

                    if (credits_user__loop__id == 1)
                    {

                        templateBuilder.Append("class=\"first\"\r\n");

                    }	//end if


                    if (credits_user__loop__id == 2)
                    {

                        templateBuilder.Append("class=\"second\"\r\n");

                    }	//end if


                    if (credits_user__loop__id == 3)
                    {

                        templateBuilder.Append("class=\"third\"\r\n");

                    }	//end if

                    templateBuilder.Append(">" + credits_user__loop__id.ToString() + "</cite>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(credits_user["uid"].ToString().Trim());

                    templateBuilder.Append("                <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                    templateBuilder.Append(Utils.GetUnicodeSubString(credits_user["username"].ToString().Trim(), 20, "..."));
                    templateBuilder.Append("</a>\r\n");
                    templateBuilder.Append("           </li> \r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"box sidebox\" style=\"float:right; margin-right:0;\">\r\n");
                templateBuilder.Append("		<div class=\"titlebar\"><ul><li class=\"current\"><a href=\"#\">在线时长排行</a></li></ul></div>\r\n");
                templateBuilder.Append("		<div class=\"sideinner\">\r\n");
                templateBuilder.Append("		<ul id=\"hottopic\">\r\n");
                userlist = forumagg.GetUserList(10, "oltime");


                int oltime_user__loop__id = 0;
                foreach (DataRow oltime_user in userlist.Rows)
                {
                    oltime_user__loop__id++;

                    templateBuilder.Append("		   <li>\r\n");
                    templateBuilder.Append("                <em>" + oltime_user["oltime"].ToString().Trim() + "</em><cite \r\n");

                    if (oltime_user__loop__id == 1)
                    {

                        templateBuilder.Append("class=\"first\"\r\n");

                    }	//end if


                    if (oltime_user__loop__id == 2)
                    {

                        templateBuilder.Append("class=\"second\"\r\n");

                    }	//end if


                    if (oltime_user__loop__id == 3)
                    {

                        templateBuilder.Append("class=\"third\"\r\n");

                    }	//end if

                    templateBuilder.Append(">" + oltime_user__loop__id.ToString() + "</cite>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(oltime_user["uid"].ToString().Trim());

                    templateBuilder.Append("                <a href=\"" + forumurl.ToString() + "" + aspxrewriteurl.ToString() + "\" target=\"_blank\">\r\n");
                    templateBuilder.Append(Utils.GetUnicodeSubString(oltime_user["username"].ToString().Trim(), 20, "..."));
                    templateBuilder.Append("</a>\r\n");
                    templateBuilder.Append("           </li> \r\n");

                }	//end loop

                templateBuilder.Append("		</ul>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");

                if (forumlinkcount > 0)
                {

                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("<div class=\"box\"  style=\"margin-right:0;\">\r\n");
                    templateBuilder.Append("	<h2>友情链接</h2>\r\n");
                    templateBuilder.Append("	<table id=\"forumlinks\" cellspacing=\"0\" cellpadding=\"0\" style=\"table-layout: fixed; margin-bottom:-1px;\" summary=\"友情链接\">\r\n");

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
                    templateBuilder.Append("</div>\r\n");

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


                templateBuilder.Append("</div>\r\n");

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