using System;
using System.Data;
using System.Text;
using Discuz.Common;
#if NET1
#else
using Discuz.Common.Generic;
#endif
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Data;

namespace Discuz.Web
{
    /// <summary>
    /// 查看新帖、精华帖
    /// </summary>
    public partial class showtopiclist : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 在线用户列表
        /// </summary>
        public DataTable onlineuserlist;

        /// <summary>
        /// 在线用户图例
        /// </summary>
        public string onlineiconlist;

#if NET1
		public ShowforumPageTopicInfoCollection topiclist;
        public IndexPageForumInfoCollection subforumlist;
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 主题列表
        /// </summary>
        public Topics.ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo> topiclist;

        //= new Topics.ShowforumPageTopicInfoCollection<ShowforumPageTopicInfo>();

        /// <summary>
        /// 子版块列表
        /// </summary>
        public List<IndexPageForumInfo> subforumlist; //= new Discuz.Common.Generic.List<IndexPageForumInfo>();

        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist; //= new Discuz.Common.Generic.List<PrivateMessageInfo>();
#endif

        /// <summary>
        /// 公告列表
        /// </summary>
        public DataTable announcementlist;

        /// <summary>
        /// 版块信息
        /// </summary>
        public ForumInfo forum;

        /// <summary>
        /// 当前用户管理组信息
        /// </summary>
        public AdminGroupInfo admingroupinfo;

        /// <summary>
        /// 论坛在线总数
        /// </summary>
        public int forumtotalonline;

        /// <summary>
        /// 论坛在线注册用户总数
        /// </summary>
        public int forumtotalonlineuser;

        /// <summary>
        /// 论坛在线游客数
        /// </summary>
        public int forumtotalonlineguest;

        /// <summary>
        /// 论坛在线隐身用户数
        /// </summary>
        public int forumtotalonlineinvisibleuser;

        /// <summary>
        /// 版块Id
        /// </summary>
        public int forumid;

        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumname = "";

        /// <summary>
        /// 子版块数
        /// </summary>
        public int subforumcount;

        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";

        /// <summary>
        /// 是否显示版块密码提示 1为显示, 0不显示
        /// </summary>
        public int showforumlogin;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;

        /// <summary>
        /// 主题总数
        /// </summary>
        public int topiccount = 0;

        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount = 0;

        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers = "";

        /// <summary>
        /// 置顶主题数
        /// </summary>
        public int toptopiccount = 0;

        /// <summary>
        /// 版块跳转链接选项
        /// </summary>
        public string forumlistboxoptions;

        /// <summary>
        /// 最近访问的版块选项
        /// </summary>
        public string visitedforumsoptions;

        /// <summary>
        /// 是否允许Rss订阅
        /// </summary>
        public int forumallowrss;

        /// <summary>
        /// 是否显示在线用户列表
        /// </summary>
        public bool showforumonline;

        //public string ignorelink;


        /// <summary>
        /// 排序方式
        /// </summary>
        public int order = 2; //排序字段

        public int direct = 1; //排序方向[默认：降序]

        /// <summary>
        /// 查看方式,digest=精华帖,其他值=新帖
        /// </summary>
        public string type = "";

        /// <summary>
        /// 新帖时限
        /// </summary>
        public int newtopic = 120;

        /// <summary>
        /// 用户选择的版块
        /// </summary>
        public string forums = "";

        /// <summary>
        /// 论坛选择多选框列表
        /// </summary>
        public string forumcheckboxlist = "";
        /// <summary>
        /// 获取绑定相关版块的商品分类信息
        /// </summary>
        public string goodscategoryfid = "{}";
        #endregion

        //后台指定的最大查询贴数
        private int maxseachnumber = 10000;
        private string condition = ""; //查询条件

        protected override void ShowPage()
        {
            pagetitle = "查看新帖";
            forumallowrss = 0;
            forumid = DNTRequest.GetInt("forumid", -1);
            forum = new ForumInfo();
            admingroupinfo = new AdminGroupInfo();
            if (userid > 0 && useradminid > 0)
            {
                admingroupinfo = AdminGroups.GetAdminGroupInfo(useradminid);
            }

            if (config.Rssstatus == 1)
            {
                AddLinkRss("tools/rss.aspx", "最新主题");
            }

            //当所选论坛为多个时或全部时
        
            if (forumid == -1)
            {
                //用户点选相应的论坛
                if ((DNTRequest.GetString("fidlist").Trim() != ""))
                {
                    forums = DNTRequest.GetString("fidlist");
                }
                else
                {
                    forums = DNTRequest.GetString("forums");
                }
                //获得已选取的论坛列表
                forumcheckboxlist = GetForumCheckBoxListCache(forums);

                //获得有权限的fid
                //string allowviewforums = "";
                if (forums.ToLower() == "all" || forums == "")//如果是选择全部版块
                {
                    //取得所有列表
                    forums = "";//先清空
                    ForumInfo[] objForumInfoList = Forums.GetForumList();
                    foreach (ForumInfo objForumInfo in objForumInfoList)
                    {
                        forums += string.Format(",{0}", objForumInfo.Fid);
                    }
                    forums = GetAllowviewForums(forums.Trim(','));
                }
                else//如果是选择指定版块
                {
                    forums = GetAllowviewForums(forums);
                }
            }
            

            #region 对搜索条件进行检索

            string orderStr = "";

            if (DNTRequest.GetString("search").Trim() != "") //进行指定查询
            {
                //排序的字段
                order = DNTRequest.GetInt("order", 2);
                switch (order)
                {
                    case 1:
                        orderStr = "lastpostid";
                        break;
                    case 2:
                        orderStr = "tid";
                        break;

                    default:
                        orderStr = "tid";
                        break;
                }

                direct = DNTRequest.GetInt("direct", 1);
            }


            //if (DNTRequest.GetString("type").Trim() == "digest")
            //{
            //    type = "digest";
            //    condition += " AND digest>0 ";
            //}

            //if (DNTRequest.GetString("type").Trim() == "newtopic")
            //{
            //    type = "newtopic";

            //    if ((DNTRequest.GetString("newtopic").Trim() != null) && (DNTRequest.GetString("newtopic").Trim() != ""))
            //    {
            //        newtopic = DNTRequest.GetString("newtopic").Trim();
            //        condition += " AND postdatetime>='" + DateTime.Now.AddMinutes(-Convert.ToInt32(newtopic)).ToString() + "'";
            //    }
            //}
            newtopic = DNTRequest.GetInt("newtopic", 120);
            condition =
                DatabaseProvider.GetInstance().GetTopicCountCondition(out type, DNTRequest.GetString("type").ToString(),
                                                                      newtopic);
            if (type == "digest")
            {
                pagetitle = "查看精华";
            }
            if (forums != "")
            {
                //验证重新生成的版块id列表是否合法(需要放入sql语句查询)                
                if (!Utils.IsNumericArray(forums.Split(',')))
                {
                    AddErrLine("错误的Url");
                    return;
                }
                condition += " AND fid IN(" + forums + ")";                
            }
            else if (forumid > 0)
            {
                condition += " AND fid =" + forumid;
            }
            else//无可访问的版块fid存留
            {
                AddErrLine("没有可访问的版块,或者Url参数错误!<br >如果是需要登录的版块,请登录后再访问.");
                return;
            }

            #endregion

            if (forumid > 0)
            {
                forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                subforumcount = forum.Subforumcount;
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

                // 是否显示版块密码提示 1为显示, 0不显示
                showforumlogin = 1;
                // 如果版块未设密码
                if (forum.Password == "")
                {
                    showforumlogin = 0;
                }
                else
                {
                    // 如果检测到相应的cookie正确
                    if (Utils.MD5(forum.Password) == ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
                    {
                        showforumlogin = 0;
                    }
                    else
                    {
                        // 如果用户提交的密码正确则保存cookie
                        if (forum.Password == DNTRequest.GetString("forumpassword"))
                        {
                            ForumUtils.WriteCookie("forum" + forumid.ToString() + "password", Utils.MD5(forum.Password));
                            showforumlogin = 0;
                        }
                    }
                }

                if (!Forums.AllowView(forum.Viewperm, usergroupid))
                {
                    AddErrLine("您没有浏览该版块的权限");
                    return;
                }
                // 得到子版块列表
                subforumlist =
                    Forums.GetSubForumCollection(forumid, forum.Colcount, config.Hideprivate, usergroupid, config.Moddisplay);
            }

            if (base.newpmcount > 0)
            {
                pmlist = PrivateMessages.GetPrivateMessageCollectionForIndex(userid, 5, 1, 1);
            }
            
            // 得到公告
            announcementlist = Announcements.GetSimplifiedAnnouncementList(nowdatetime, "2999-01-01 00:00:00");

            //得到当前用户请求的页数
            pageid = DNTRequest.GetInt("page", 1);
            //获取主题总数
            topiccount = Topics.GetTopicCount(condition);

            //防止查询数超过系统规定的最大值
            topiccount = maxseachnumber > topiccount ? topiccount : maxseachnumber;

            // 得到Tpp设置
            int tpp = Utils.StrToInt(ForumUtils.GetCookie("tpp"), config.Tpp);
            if (tpp <= 0)
            {
                tpp = config.Tpp;
            }
            //得到用户设置的每页显示主题数
            if (userid != -1)
            {
                ShortUserInfo userinfo = Discuz.Forum.Users.GetShortUserInfo(userid);
                if (userinfo != null)
                {
                    if (userinfo.Tpp > 0)
                    {
                        tpp = userinfo.Tpp;
                    }

                    if (userinfo.Newpm == 0)
                    {
                        base.newpmcount = 0;
                    }
                }
            }


            //获取总页数
            pagecount = topiccount%tpp == 0 ? topiccount/tpp : topiccount/tpp + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            //修正请求页数中可能的错误
            if (pageid < 1)
            {
                pageid = 1;
            }
            if (pageid > pagecount)
            {
                pageid = pagecount;
            }
            //如果当前页面的返回结果超过系统规定的的范围时，则进行相应删剪
            if ((pageid*tpp) > topiccount)
            {
                tpp = tpp - (pageid*tpp - topiccount);
            }
            if (orderStr == "")
            {
                topiclist =
                    Topics.GetTopicCollectionByType(tpp, pageid, 0, 10, config.Hottopic, forum.Autoclose,
                                                    forum.Topictypeprefix, condition, direct);
            }
            else
            {
                topiclist =
                    Topics.GetTopicCollectionByTypeDate(tpp, pageid, 0, 10, config.Hottopic, forum.Autoclose,
                                                        forum.Topictypeprefix, condition, orderStr, direct);
            }

            //得到页码链接
            if ("".Equals(DNTRequest.GetString("search")))
            {
                pagenumbers =
                    Utils.GetPageNumbers(pageid, pagecount,
                                         string.Format(
                                             "showtopiclist.aspx?type={0}&newtopic={1}&forumid={2}&forums={3}", type,
                                             newtopic, forumid, forums), 8);
            }
            else
            {
                pagenumbers =
                    Utils.GetPageNumbers(pageid, pagecount,
                                         string.Format(
                                             "showtopiclist.aspx?search=1&type={0}&newtopic={1}&order={2}&direct={3}&forumid={4}&forums={5}",
                                             type, newtopic, DNTRequest.GetString("order"),
                                             DNTRequest.GetString("direct"), forumid.ToString(), forums), 8);
            }

            forumlistboxoptions = Caches.GetForumListBoxOptionsCache();
            OnlineUsers.UpdateAction(olid, UserAction.ShowForum.ActionID, forumid, config.Onlinetimeout);

            showforumonline = false;
            if (forumtotalonline < 300 || DNTRequest.GetString("showonline") != "")
            {
                showforumonline = true;
            }

            ForumUtils.UpdateVisitedForumsOptions(forumid);
            visitedforumsoptions = ForumUtils.GetVisitedForumsOptions(config.Visitedforums);
            forumallowrss = forum.Allowrss;
        }

        /// <summary>
        /// 取得当前用户有权访问的版块列表
        /// </summary>
        /// <param name="forums">原始版块列表(用逗号分隔的fid)</param>
        /// <returns>有权访问的版块列表(用逗号分隔的fid)</returns>
        private string GetAllowviewForums(string forums)
        {
            //验证版块id列表是否合法的数字列表                
            if (!Utils.IsNumericArray(forums.Split(',')))
            {
                return "";
            }
            string allowviewforums = "";
            string[] fidlist = forums.Split(',');

            foreach (string strfid in fidlist)
            {                
                int fid = Utils.StrToInt(strfid, 0);
                if (Forums.AllowView(Forums.GetForumInfo(fid).Viewperm, usergroupid))
                {
                    if (Forums.GetForumInfo(fid).Password.Trim() == "" || Utils.MD5(Forums.GetForumInfo(fid).Password.Trim()) == ForumUtils.GetCookie("forum" + strfid.Trim() + "password"))
                    {
                        allowviewforums += string.Format(",{0}", fid);
                    }
                }
            }

            return allowviewforums.Trim(',');
        }


        /// <summary>
        /// 获得已选取的论坛列表
        /// </summary>
        /// <returns>列表内容的html</returns>
        public static string GetForumCheckBoxListCache(string forums)
        {
            StringBuilder sb = new StringBuilder();

            /*
			sb.Append("<script language=\"JavaScript\">\r\n");
            sb.Append("function CheckAll(form)\r\n");
			sb.Append("{\r\n");
		    sb.Append("  for (var i=0;i<form.elements.length;i++)\r\n");
            sb.Append("{\r\n");
            sb.Append("var e = form.elements[i];\r\n");
            sb.Append("if (e.name != 'chkall')\r\n");
            sb.Append("e.checked = form.chkall.checked;\r\n");
            sb.Append("}\r\n");
            sb.Append("}\r\n");

            sb.Append("function SH_SelectOne()\r\n");
            sb.Append("{\r\n");
	        sb.Append("var obj = window.event.srcElement;\r\n");
	        sb.Append("if ( obj.checked == false)\r\n");
	        sb.Append("{\r\n");
		    sb.Append("  document.all.chkall.checked = obj.chcked;\r\n");
		    sb.Append("}\r\n");
            sb.Append("}\r\n");
    		sb.Append("</script>\r\n");
            */
            forums = "," + forums + ",";

            DataTable dt = Forums.GetForumListTable();
            int count = 1;
            foreach (DataRow dr in dt.Rows)
            {
                if (forums.ToLower() == ",all,")
                {
                    sb.AppendFormat(
                        "<td><input id=\"fidlist\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\"	name=\"fidlist\"  checked/> {1}</td>\r\n",
                        dr["fid"].ToString(), dr["name"].ToString());
                }
                else
                {
                    if (forums.IndexOf("," + dr["fid"].ToString() + ",") >= 0)
                    {
                        sb.AppendFormat(
                            "<td><input id=\"fidlist\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\"	name=\"fidlist\"  checked/> {1}</td>\r\n",
                            dr["fid"].ToString(), dr["name"].ToString());
                    }
                    else
                    {
                        sb.AppendFormat(
                            "<td><input id=\"fidlist\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\"	name=\"fidlist\"  /> {1}</td>\r\n",
                            dr["fid"].ToString(), dr["name"].ToString());
                    }
                }

                if (count > 3)
                {
                    sb.Append("			  </tr>\r\n");
                    sb.Append("			  <tr>\r\n");
                    count = 0;
                }
                count++;
            }
            return sb.ToString();
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:06.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:06. 
            */

            base.OnLoad(e);


            if (page_err == 0)
            {

                templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("var templatepath = \"" + templatepath.ToString() + "\";\r\n");
                templateBuilder.Append("var fid = parseInt(" + forum.Fid.ToString().Trim() + ");\r\n");
                templateBuilder.Append("</" + "script>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_showforum.js\"></" + "script>\r\n");
                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + "\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div id=\"forumstats\">\r\n");
                templateBuilder.Append("		<p>\r\n");

                if (forumid == -1)
                {

                    templateBuilder.Append("			<a href=\"forumindex.aspx\">全部</a>\r\n");
                    templateBuilder.Append("			<a href=\"showtopiclist.aspx?type=digest&amp;forums=" + forums.ToString() + "\">精华帖区</a>\r\n");

                }
                else
                {

                    aspxrewriteurl = this.ShowForumAspxRewrite(forumid, 0);

                    templateBuilder.Append("			<a href=\"" + aspxrewriteurl.ToString() + "\">全部</a>\r\n");
                    templateBuilder.Append("			<a href=\"showtopiclist.aspx?forumid=" + forumid.ToString() + "&type=digest\">精华帖区</a>\r\n");

                }	//end if


                if (config.Rssstatus != 0)
                {

                    templateBuilder.Append("			<a href=\"tools/rss.aspx\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/rss.gif\" alt=\"Rss\"/></a>\r\n");

                }	//end if

                templateBuilder.Append("		</p>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");

                if (showforumlogin == 1)
                {

                    templateBuilder.Append("	<div class=\"mainbox formbox\">\r\n");
                    templateBuilder.Append("		<h3>本版块已经被管理员设置了密码</h3>\r\n");
                    templateBuilder.Append("		<form id=\"forumlogin\" name=\"forumlogin\" method=\"post\" action=\"\">\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<th><label for=\"forumpassword\">请输入密码</label></th>\r\n");
                    templateBuilder.Append("					<td><input name=\"forumpassword\" type=\"password\" id=\"forumpassword\" size=\"20\"/></td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("				<tbody>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"vcode\">输入验证码</label></th>\r\n");
                        templateBuilder.Append("					<td>\r\n");

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
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("				<tbody>\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("					<td>\r\n");
                    templateBuilder.Append("						<input type=\"submit\"  value=\"确定\"/>\r\n");
                    templateBuilder.Append("					</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("				</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</form>\r\n");
                    templateBuilder.Append("	</div>\r\n");

                }
                else
                {

                    templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                    templateBuilder.Append("	<div class=\"pages\">\r\n");
                    templateBuilder.Append("		<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "		\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div class=\"mainbox threadlist\">\r\n");
                    templateBuilder.Append("	<h3>\r\n");

                    if (forumid > 0)
                    {

                        aspxrewriteurl = this.ShowForumAspxRewrite(forum.Fid, 0);

                        templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\">" + forum.Name.ToString().Trim() + "</a>\r\n");

                    }
                    else if (type == "digest")
                    {

                        templateBuilder.Append("			精华帖\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("			新帖\r\n");

                    }	//end if

                    templateBuilder.Append("	</h3>\r\n");
                    templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" summary=\"帖子\">\r\n");
                    templateBuilder.Append("		<thead class=\"category\">\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td class=\"folder\">&nbsp;</td>\r\n");
                    templateBuilder.Append("				<td class=\"icon\">&nbsp;</td>\r\n");
                    templateBuilder.Append("				<th>标题</th>\r\n");
                    templateBuilder.Append("				<td class=\"author\">作者</td>\r\n");
                    templateBuilder.Append("				<td class=\"nums\">回复/查看</td>\r\n");
                    templateBuilder.Append("				<td class=\"lastpost\">最后发表</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</thead>\r\n");
                    templateBuilder.Append("	<!--announcement area start-->\r\n");

                    int announcement__loop__id = 0;
                    foreach (DataRow announcement in announcementlist.Rows)
                    {
                        announcement__loop__id++;

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<td style=\"line-height:28px;\"><img src=\"templates/" + templatepath.ToString() + "/images/announcement.gif\" alt=\"announcement\"/></td>\r\n");
                        templateBuilder.Append("			<td>&nbsp;</td>\r\n");
                        templateBuilder.Append("			<th><a href=\"announcement.aspx#" + announcement["id"].ToString().Trim() + "\">" + announcement["title"].ToString().Trim() + "</a></th>\r\n");
                        templateBuilder.Append("			<td colspan=\"3\">\r\n");

                        if (Utils.StrToInt(announcement["posterid"].ToString().Trim(), 0) == -1)
                        {

                            templateBuilder.Append("					游客\r\n");

                        }
                        else
                        {

                            aspxrewriteurl = this.UserInfoAspxRewrite(announcement["posterid"].ToString().Trim());

                            templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\">" + announcement["poster"].ToString().Trim() + "</a>\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end loop

                    templateBuilder.Append("	<!--announcement area end-->\r\n");
                    templateBuilder.Append("	<!--showtopiclist start-->\r\n");

                    int topic__loop__id = 0;
                    foreach (ShowforumPageTopicInfo topic in topiclist)
                    {
                        topic__loop__id++;

                        templateBuilder.Append("		<tbody id=\"normalthread_" + topic.Tid.ToString().Trim() + "\" >\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<td class=\"folder\">\r\n");

                        if (topic.Folder != "")
                        {

                            aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                            templateBuilder.Append("				<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/folder_" + topic.Folder.ToString().Trim() + ".gif\" alt=\"主题图标\"/></a>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("				&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			<td class=\"icon\">\r\n");

                        if (topic.Iconid != 0)
                        {

                            templateBuilder.Append("					<img src=\"images/posticons/" + topic.Iconid.ToString().Trim() + ".gif\" alt=\"示图\"/>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			<th class=\"common\">	\r\n");
                        templateBuilder.Append("				<label>\r\n");

                        if (topic.Digest > 0)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/digest" + topic.Digest.ToString().Trim() + ".gif\" alt=\"digtest\"/>\r\n");

                        }	//end if


                        if (topic.Special == 1)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/pollsmall.gif\" alt=\"投票\" />\r\n");

                        }	//end if


                        if (topic.Special == 2 || topic.Special == 3)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/bonus.gif\" alt=\"悬赏\"/>\r\n");

                        }	//end if


                        if (topic.Special == 4)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/debatesmall.gif\" alt=\"辩论\"/>\r\n");

                        }	//end if


                        if (topic.Attachment > 0)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/attachment.gif\" alt=\"附件\"/>\r\n");

                        }	//end if

                        templateBuilder.Append("				</label>\r\n");
                        templateBuilder.Append("				<span>\r\n");

                        if (topic.Replies > 0)
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/topItem_exp.gif\" id=\"imgButton_" + topic.Tid.ToString().Trim() + "\" onclick=\"showtree(" + topic.Tid.ToString().Trim() + "," + config.Ppp.ToString().Trim() + ");\" class=\"cursor\" alt=\"展开帖子列表\" title=\"展开帖子列表\" />\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/no-sublist.gif\" id=\"imgButton_" + topic.Tid.ToString().Trim() + "\"/>\r\n");

                        }	//end if

                        aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                        templateBuilder.Append("				<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Title.ToString().Trim() + "</a>\r\n");

                        if (topic.Replies / config.Ppp > 0)
                        {

                            templateBuilder.Append("					<script type=\"text/javascript\">getpagenumbers(\"" + config.Extname.ToString().Trim() + "\", " + topic.Replies.ToString().Trim() + "," + config.Ppp.ToString().Trim() + ",0,\"\"," + topic.Tid.ToString().Trim() + ",\"\",\"\"," + config.Aspxrewrite.ToString().Trim() + ");</" + "script>\r\n");

                        }	//end if

                        templateBuilder.Append("				</span>\r\n");
                        templateBuilder.Append("			</th>\r\n");
                        templateBuilder.Append("			<td class=\"author\">\r\n");
                        templateBuilder.Append("				<cite>\r\n");

                        if (topic.Posterid == -1)
                        {

                            templateBuilder.Append("					游客\r\n");

                        }
                        else
                        {

                            aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);

                            templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Poster.ToString().Trim() + "</a>\r\n");

                        }	//end if

                        templateBuilder.Append("</cite>\r\n");
                        templateBuilder.Append("				<em>\r\n");
                        templateBuilder.Append(Convert.ToDateTime(topic.Postdatetime).ToString("yy-MM-dd HH:mm"));
                        templateBuilder.Append("</em>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			<td class=\"nums\"><em>" + topic.Replies.ToString().Trim() + "</em> / " + topic.Views.ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("			<td class=\"lastpost\">\r\n");
                        templateBuilder.Append("					<em><a href=\"showtopic.aspx?topicid=" + topic.Tid.ToString().Trim() + "&page=end#lastpost\">\r\n");
                        templateBuilder.Append(Convert.ToDateTime(topic.Lastpost).ToString("yy-MM-dd HH:mm"));
                        templateBuilder.Append("</a></em>\r\n");
                        templateBuilder.Append("					<cite>by\r\n");

                        if (topic.Lastposterid == -1)
                        {

                            templateBuilder.Append("						游客\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						<a href=\"" + UserInfoAspxRewrite(topic.Lastposterid).ToString().Trim() + "\" target=\"_blank\">" + topic.Lastposter.ToString().Trim() + "</a>\r\n");

                        }	//end if

                        templateBuilder.Append("					</cite>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<td colspan=\"6\" style=\"border:none; padding:0;\"><div id=\"divTopic" + topic.Tid.ToString().Trim() + "\"></div></td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end loop

                    templateBuilder.Append("	<!--showtopiclist end-->\r\n");
                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                    templateBuilder.Append("	<div class=\"pages\">\r\n");
                    templateBuilder.Append("		<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "		\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("function CheckAll(form)\r\n");
                    templateBuilder.Append("{\r\n");
                    templateBuilder.Append("	for (var i = 0; i < form.elements.length; i++)\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		var e = form.elements[i];\r\n");
                    templateBuilder.Append("		if (e.id == \"fidlist\"){\r\n");
                    templateBuilder.Append("		   e.checked = form.chkall.checked;\r\n");
                    templateBuilder.Append("		}\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("}\r\n");
                    templateBuilder.Append("function SH_SelectOne(obj)\r\n");
                    templateBuilder.Append("{\r\n");
                    templateBuilder.Append("	for (var i = 0; i < document.getElementById(\"LookBySearch\").elements.length; i++)\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		var e = document.getElementById(\"LookBySearch\").elements[i];\r\n");
                    templateBuilder.Append("		if (e.id == \"fidlist\"){\r\n");
                    templateBuilder.Append("		   if (!e.checked){\r\n");
                    templateBuilder.Append("			document.getElementById(\"chkall\").checked = false;\r\n");
                    templateBuilder.Append("			return;\r\n");
                    templateBuilder.Append("		   }\r\n");
                    templateBuilder.Append("		}\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("	document.getElementById(\"chkall\").checked = true;\r\n");
                    templateBuilder.Append("}\r\n");
                    templateBuilder.Append("function ShowDetailGrid(tid)\r\n");
                    templateBuilder.Append("{\r\n");
                    templateBuilder.Append("   if(" + config.Aspxrewrite.ToString().Trim() + ")\r\n");
                    templateBuilder.Append("   {\r\n");
                    templateBuilder.Append("       window.location.href = \"showforum-\" + tid + \"" + config.Extname.ToString().Trim() + "\";\r\n");
                    templateBuilder.Append("   }\r\n");
                    templateBuilder.Append("   else\r\n");
                    templateBuilder.Append("   {\r\n");
                    templateBuilder.Append("       window.location.href = \"showforum.aspx?forumid=\" + tid ;\r\n");
                    templateBuilder.Append("   }\r\n");
                    templateBuilder.Append("}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");

                    if (forumid == -1)
                    {

                        templateBuilder.Append("<form name=\"LookBySearch\" method=\"post\" action=\"showtopiclist.aspx?search=1&forumid=" + forumid.ToString() + "&type=" + type.ToString() + "&newtopic=" + newtopic.ToString() + "&forums=" + forums.ToString() + "\" ID=\"LookBySearch\">\r\n");
                        templateBuilder.Append("<div class=\"box\" style=\"padding-bottom:0;\">\r\n");
                        templateBuilder.Append("	<h4>以下论坛版块:</h4>\r\n");
                        templateBuilder.Append("	<table width=\"100%\" border=\"0\" cellspacing=\"3\" cellpadding=\"0\">\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			 " + forumcheckboxlist.ToString() + "\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("	<div style=\"padding:6px 0; border:none; border-top:1px solid #CCC; background:#EEE;\">\r\n");
                        templateBuilder.Append("		<span style=\"float:right;\">\r\n");
                        templateBuilder.Append("			排序方式\r\n");
                        templateBuilder.Append("			<select name=\"order\" id=\"order\">\r\n");
                        templateBuilder.Append("			  <option value=\"1\" \r\n");

                        if (order == 1)
                        {

                            templateBuilder.Append("selected\r\n");

                        }	//end if

                        templateBuilder.Append(">最后回复时间</option>\r\n");
                        templateBuilder.Append("			  <option value=\"2\" \r\n");

                        if (order == 2)
                        {

                            templateBuilder.Append("selected\r\n");

                        }	//end if

                        templateBuilder.Append(">发布时间</option>\r\n");
                        templateBuilder.Append("			</select>	\r\n");
                        templateBuilder.Append("			<select name=\"direct\" id=\"direct\">\r\n");
                        templateBuilder.Append("			  <option value=\"0\" \r\n");

                        if (direct == 0)
                        {

                            templateBuilder.Append("selected\r\n");

                        }	//end if

                        templateBuilder.Append(">按升序排列</option>\r\n");
                        templateBuilder.Append("			  <option value=\"1\" \r\n");

                        if (direct == 1)
                        {

                            templateBuilder.Append("selected\r\n");

                        }	//end if

                        templateBuilder.Append(">按降序排列</option>\r\n");
                        templateBuilder.Append("			</select>\r\n");
                        templateBuilder.Append("			<button type=\"submit\" onclick=\"document.LookBySearch.submit();\">提交</button>\r\n");
                        templateBuilder.Append("		</span>\r\n");
                        templateBuilder.Append("		<input title=\"选中/取消选中 本页所有Case\" onclick=\"CheckAll(this.form)\" type=\"checkbox\" name=\"chkall\" id=\"chkall\">全选/取消全选\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("</div>\r\n");
                        templateBuilder.Append("</form>\r\n");
                        templateBuilder.Append("<div id=\"footfilter\" class=\"box\">\r\n");

                        if (config.Forumjump == 1)
                        {

                            templateBuilder.Append("    <select onchange=\"if(this.options[this.selectedIndex].value != '') { jumpurl(this.options[this.selectedIndex].value," + config.Aspxrewrite.ToString().Trim() + ",'" + config.Extname.ToString().Trim() + "');}\">\r\n");
                            templateBuilder.Append("		<option>论坛跳转...</option>\r\n");
                            templateBuilder.Append("		" + forumlistboxoptions.ToString() + "\r\n");
                            templateBuilder.Append("	</select>\r\n");

                        }	//end if


                        if (config.Visitedforums > 0)
                        {

                            templateBuilder.Append("    <select name=\"select2\" onchange=\"if(this.options[this.selectedIndex].value != '') {jumpurl(this.options[this.selectedIndex].value," + config.Aspxrewrite.ToString().Trim() + ",'" + config.Extname.ToString().Trim() + "');}\">\r\n");
                            templateBuilder.Append("		<option>最近访问...</option>" + visitedforumsoptions.ToString() + "\r\n");
                            templateBuilder.Append("	</select>\r\n");

                        }	//end if

                        templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("	var categorydata = " + goodscategoryfid.ToString() + ";		\r\n");
                        templateBuilder.Append("	function jumpurl(fid, aspxrewrite, extname) {\r\n");
                        templateBuilder.Append("		for(var i in categorydata) {\r\n");
                        templateBuilder.Append("		   if(categorydata[i].fid == fid) {\r\n");
                        templateBuilder.Append("			   if(aspxrewrite) {\r\n");
                        templateBuilder.Append("				   window.location='showgoodslist-' +categorydata[i].categoryid + extname;\r\n");
                        templateBuilder.Append("			   }\r\n");
                        templateBuilder.Append("			   else {\r\n");
                        templateBuilder.Append("				   window.location='showgoodslist.aspx?categoryid=' +categorydata[i].categoryid;\r\n");
                        templateBuilder.Append("			   }\r\n");
                        templateBuilder.Append("			   return;\r\n");
                        templateBuilder.Append("		   } \r\n");
                        templateBuilder.Append("		}\r\n");
                        templateBuilder.Append("		if(aspxrewrite) {\r\n");
                        templateBuilder.Append("			window.location='showforum-' + fid + extname;\r\n");
                        templateBuilder.Append("		}\r\n");
                        templateBuilder.Append("		else {\r\n");
                        templateBuilder.Append("			window.location='showforum.aspx?forumid=' + fid ;\r\n");
                        templateBuilder.Append("		}\r\n");
                        templateBuilder.Append("	}\r\n");
                        templateBuilder.Append("	</" + "script>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }	//end if


                }	//end if

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