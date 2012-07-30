using System;
using System.Data;
using System.Web;
using Discuz.Common;
#if NET1
#else
using Discuz.Common.Generic;
#endif
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;
using System.Text.RegularExpressions;
using System.Collections;

namespace Discuz.Web
{
    /// <summary>
    /// showforum 的摘要说明.
    /// </summary>
    public partial class showtree : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo topic;

        /// <summary>
        /// 主题所属帖子
        /// </summary>
        public ShowtopicPagePostInfo post;

        /// <summary>
        /// 投票选项列表
        /// </summary>
        public DataTable polloptionlist;

        /// <summary>
        /// 投票帖类型
        /// </summary>
        public PollInfo pollinfo;

        /// <summary>
        /// 是否显示投票结果
        /// </summary>
        public bool showpollresult = true;

        //public DataRow attachment;

#if NET1		
        public ShowtopicPageAttachmentInfoCollection attachmentlist;
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<ShowtopicPageAttachmentInfo> attachmentlist = new List<ShowtopicPageAttachmentInfo>();

        /// <summary>
        /// 短消息列表
        /// </summary>
        public List<PrivateMessageInfo> pmlist = new List<PrivateMessageInfo>();


        /// <summary>
        /// 悬赏给分日志
        /// </summary>
        public List<BonusLogInfo> bonuslogs;
#endif

        /// <summary>
        /// 主题回复列表
        /// </summary>
        public DataTable posttree;

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
        /// 快速编辑器背景广告
        /// </summary>
        public string quickbgadimg = string.Empty;
        public string quickbgadlink = string.Empty;

        /// <summary>
        /// 版块Id
        /// </summary>
        public int forumid;

        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumname;

        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;

        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid;

        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid;

        /// <summary>
        /// 是否是投票帖
        /// </summary>
        public bool ispoll;

        /// <summary>
        /// 是否允许投票
        /// </summary>
        public bool allowvote;

        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle;

        /// <summary>
        /// 回复标题
        /// </summary>
        public string replytitle;

        /// <summary>
        /// 主题浏览量
        /// </summary>
        public int topicviews;

        /// <summary>
        /// 主题帖子数
        /// </summary>
        public int postcount;

        /// <summary>
        /// 表情Javascript数组
        /// </summary>
        public string smilies;

        /// <summary>
        /// 参与投票的用户列表
        /// </summary>
        public string voters;

        /// <summary>
        /// 当前用户是否是管理者
        /// </summary>
        public int ismoder;

        /// <summary>
        /// 是否显示评分记录
        /// </summary>
        public int showratelog;

        /// <summary>
        /// 是否解析URL
        /// </summary>
        public int parseurloff;

        /// <summary>
        /// 是否解析表情
        /// </summary>
        public int smileyoff;

        /// <summary>
        /// 是否解析 Discuz!NT 代码
        /// </summary>
        public int bbcodeoff;

        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig;

        /// <summary>
        /// 可用的扩展金币名称
        /// </summary>
        public string[] score;

        /// <summary>
        /// 可用的扩展金币单位
        /// </summary>
        public string[] scoreunit;

        /// <summary>
        /// 
        /// </summary>
        public string ignorelink;

        /// <summary>
        /// 主题鉴定信息
        /// </summary>
        public int quickpost;

        /// <summary>
        /// 是否受发贴灌水限制
        /// </summary>
        public int disablepostctrl;

        /// <summary>
        /// 主题鉴定信息
        /// </summary>
        public TopicIdentify topicidentify;

        /// <summary>
        /// 当前用户所属的管理组信息
        /// </summary>
        public AdminGroupInfo admininfo;

        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum;

        /// <summary>
        /// 是否允许下载附件
        /// </summary>
        public DataTable smilietypes = new DataTable();

        /// <summary>
        /// 是否显示回复链接
        /// </summary>
        public bool isshowreplaylink = false;

        /// <summary>
        /// 是否显示下载链接
        /// </summary>
        public bool allowdownloadattach = false;

        /// <summary>
        /// 是否有发表主题的权限
        /// </summary>
        public bool canposttopic = false;

        /// <summary>
        /// 是否有回复的权限
        /// </summary>
        public bool canreply = false;

        /// <summary>
        /// 论坛弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = "";

        /// <summary>
        /// 是否显示短消息提示
        /// </summary>
        public bool showpmhint = false;

        /// <summary>
        /// 帖内广告
        /// </summary>
        public string inpostad = string.Empty;

        /// <summary>
        /// 快速发帖广告
        /// </summary>
        public string quickeditorad = string.Empty;

        /// <summary>
        /// 主题分类列表
        /// </summary>
        public string topictypes = "";

        /// <summary>
        /// 金币策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;

        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;

        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = string.Empty;

        /// <summary>
        /// 本版是否启用Tag
        /// </summary>
        public bool enabletag = false;
        #endregion

       
		protected override void ShowPage()
		{
			ismoder = 0;
			disablepostctrl = 0;
			quickpost = 1;
			forumnav = "";
			topictitle = "";
			ignorelink = "";
            forumid = 0;
            firstpagesmilies = Caches.GetSmiliesFirstPageCache();
            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                {
                    quickpost = 0;
                }
            }

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", forumid);
            footerad = Advertisements.GetOneFooterAd("", forumid);

            pagewordad = Advertisements.GetPageWordAd("", forumid);
            doublead = Advertisements.GetDoubleAd("", forumid);
            floatad = Advertisements.GetFloatAd("", forumid);
            inpostad = Advertisements.GetInPostAd("", forumid, templatepath, 1);
            //快速发帖广告
            quickeditorad = Advertisements.GetQuickEditorAD("", forumid);

            //快速编辑器背景广告
            string[] quickbgad = Advertisements.GetQuickEditorBgAd("", forumid);

            if (quickbgad.Length > 1)
            {
                quickbgadimg = quickbgad[0];
                quickbgadlink = quickbgad[1];
            }

            topicid = DNTRequest.GetInt("topicid", -1);
            postid = DNTRequest.GetInt("postid", -1);
            // 如果主题ID非数字则判断帖子ID
            if (topicid != -1)
            {
                postid = DNTRequest.GetInt("postid", -1);
                if (postid == -1)
                {
                    // 如果只有主题id则现实主题的第一个帖子的内容
                    postid = Posts.GetFirstPostId(topicid);
                }
            }
            //"+BaseConfigs.GetTablePrefix+"getsinglepost
            PostInfo __postinfo = Posts.GetPostInfo(topicid, postid);
            if (__postinfo == null)
            {
                //
                AddErrLine("错误的主题");
                return;
            }
            else
            {
                if (topicid == -1)
                {
                    topicid = __postinfo.Tid;
                }
            }
            if (topicid != __postinfo.Tid)
            {
                AddErrLine("主题ID无效");
                return;
            }
            smilies = Caches.GetSmiliesCache();
            smilietypes = Caches.GetSmilieTypesCache();

            //编辑器状态
            parseurloff = 0;

            usesig = config.Showsignatures;


            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            if (topic.Displayorder == -1)
            {
                AddErrLine("此主题已被删除！");
                return;
            }

            if (topic.Displayorder == -2)
            {
                AddErrLine("此主题未经审核！");
                return;
            }

            if (topic.Identify > 0)
            {
                topicidentify = Caches.GetTopicIdentify(topic.Identify);
                //原来的鉴定项已被移除，则将其恢复成未被鉴定的主题
                if (topic.Identify != topicidentify.Identifyid)
                {
                    TopicAdmins.IdentifyTopic(topicid.ToString(), 0);
                }
            }

            forumid = topic.Fid;
            // 检查是否具有版主的身份
            ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
            admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
            if (admininfo != null)
            {
                disablepostctrl = admininfo.Disablepostctrl;
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 &&
                ismoder == 0)
            {
                AddErrLine("本主题阅读权限为: " + topic.Readperm.ToString() + ", 您当前的身份 \"" + usergroupinfo.Grouptitle +
                           "\" 阅读权限不够");
                if (userid == -1)
                {
                    needlogin = true;
                }
                return;
            }


            topictitle = topic.Title.Trim();
            replytitle = topictitle;
            if (replytitle.Length >= 50)
            {
                replytitle = Utils.CutString(replytitle, 0, 50) + "...";
            }

            topicviews = topic.Views + 1;

            forum = Forums.GetForumInfo(forumid);
            enabletag = (config.Enabletag & forum.Allowtag) == 1;
            forumname = forum.Name;
            pagetitle = topic.Title;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);
            smileyoff = 1 - forum.Allowsmilies;
            bbcodeoff = 1;
            if (forum.Allowbbcode == 1)
            {
                if (usergroupinfo.Allowcusbbcode == 1)
                {
                    bbcodeoff = 0;
                }
            }

            if (forum.Password != "" &&
                Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                //SetBackLink("showforum-" + forumid.ToString() + config.Extname);
                HttpContext.Current.Response.Redirect("/" + "showforum-" + forumid.ToString() + config.Extname, true);
                return;
            }


            if (!Forums.AllowViewByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                        if (userid == -1)
                        {
                            needlogin = true;
                        }
                        return;
                    }
                }
                else //当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该版块的权限");
                        if (userid == -1)
                        {
                            needlogin = true;
                        }
                        return;
                    }
                }
            }

            //是否显示回复链接
            if (Forums.AllowReplyByUserID(forum.Permuserlist, userid))
            {
                isshowreplaylink = true;
            }
            else
            {
                if (forum.Replyperm == null || forum.Replyperm == string.Empty) //权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有发表主题的权限
                    if (usergroupinfo.Allowreply == 1)
                    {
                        isshowreplaylink = true;
                    }
                }
                else if (Forums.AllowReply(forum.Replyperm, usergroupid))
                {
                    isshowreplaylink = true;
                }
            }

            //当前用户是否有允许下载附件权限
            if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userid))
            {
                allowdownloadattach = true;
            }
            else
            {
                if (forum.Getattachperm == null || forum.Getattachperm == string.Empty) //权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有有允许下载附件权限
                    if (usergroupinfo.Allowgetattach == 1)
                    {
                        allowdownloadattach = true;
                    }
                }
                else if (Forums.AllowGetAttach(forum.Getattachperm, usergroupid))
                {
                    allowdownloadattach = true;
                }
            }
            try
            {
                topictypes = Caches.GetTopicTypeArray()[topic.Typeid].ToString();
                topictypes = topictypes != "" ? "[" + topictypes + "]" : "";
            }
            catch
            {
                ;
            }

            userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetCreditsTrans());

            //判断是否有发主题的权限
            if (quickpost == 1)
            {
                if (userid > -1 && Forums.AllowPostByUserID(forum.Permuserlist, userid))
                {
                    canposttopic = true;
                }

                if (forum.Postperm == null || forum.Postperm == string.Empty) //权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有发表主题的权限
                    if (usergroupinfo.Allowpost == 1)
                    {
                        canposttopic = true;
                    }
                }
                else if (Forums.AllowPost(forum.Postperm, usergroupid))
                {
                    canposttopic = true;
                }
            }


            //是否有回复的权限
            if (topic.Closed == 0 || ismoder == 1)
            {
                if (config.Fastpost == 2 || config.Fastpost == 3)
                {
                    if (Forums.AllowReplyByUserID(forum.Permuserlist, userid))
                    {
                        canreply = true;
                    }
                    else
                    {
                        if (forum.Replyperm == null || forum.Replyperm == string.Empty) //权限设置为空时，根据用户组权限判断
                        {
                            // 验证用户是否有发表主题的权限
                            if (usergroupinfo.Allowreply == 1)
                            {
                                canreply = true;
                            }
                        }
                        else if (Forums.AllowReply(forum.Replyperm, usergroupid))
                        {
                            canreply = true;
                        }
                    }
                }
            }


            showratelog = GeneralConfigs.GetConfig().DisplayRateCount > 0 ? 1 : 0;


            //购买帖子操作
            //判断是否为回复可见帖, price=0为非购买可见(正常), price>0 为购买可见, price=-1为购买可见但当前用户已购买
            int price = 0;
            if (topic.Special == 0)//普通主题
            {
                if (topic.Price > 0 && userid != topic.Posterid && ismoder != 1)
                {
                    price = topic.Price;
                    //时间乘以-1是因为当Configs.GetMaxChargeSpan()==0时,帖子始终为购买帖
                    if (PaymentLogs.IsBuyer(topicid, userid) ||
                        (Utils.StrDateDiffHours(topic.Postdatetime, Scoresets.GetMaxChargeSpan()) > 0 &&
                         Scoresets.GetMaxChargeSpan() != 0)) //判断当前用户是否已经购买
                    {
                        price = -1;
                    }
                }
                if (price > 0)
                {
                    HttpContext.Current.Response.Redirect("/" + "buytopic.aspx?topicid=" + topic.Tid);
                    return;
                }

            }

            if (newpmcount > 0)
            {
                pmlist = PrivateMessages.GetPrivateMessageCollectionForIndex(userid, 5, 1, 1);
                if (DNTRequest.GetUrl().IndexOf("?") != -1)
                {
                    ignorelink = DNTRequest.GetRawUrl() + "?ignore=yes";
                }
                else
                {
                    ignorelink = DNTRequest.GetRawUrl() + "&ignore=yes";
                }

                showpmhint = Convert.ToInt32(Discuz.Forum.Users.GetShortUserInfo(userid).Newsletter) > 4;
            }


            ispoll = false;
            allowvote = false;
            if (topic.Special == 1)
            {
                ispoll = true;
                polloptionlist = Polls.GetPollOptionList(topicid);
                pollinfo = Polls.GetPollInfo(topicid);
                voters = Polls.GetVoters(topicid, userid, username, out allowvote);

                if (pollinfo.Visible == 1 && //当为投票才可见时
                  (allowvote || (userid == -1 && Utils.InArray(topicid.ToString(), ForumUtils.GetCookie("dnt_polled")))))//当允许投票或为游客时
                {
                    showpollresult = false;
                }
            }

            if (topic.Special == 3)//已给分的悬赏帖
            {
                bonuslogs = Bonus.GetLogs(topic.Tid);
            }


            if (ispoll && DateTime.Parse(pollinfo.Expiration) < DateTime.Now)
            {
                allowvote = false;
            }

            postcount = topic.Replies + 1;


            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断
            int hide = 1;
            if (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1))
            {
                hide = -1;
            }

            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Tid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Pid = postid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = 1;
            postpramsInfo.Pageindex = 1;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = price;
            postpramsInfo.Ubbmode = false;
            postpramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;

            if (ismoder == 1)
                postpramsInfo.Usergroupreadaccess = int.MaxValue;

            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.CurrentUserGroup = usergroupinfo;


            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;

            post = Posts.GetSinglePost(postpramsInfo, out attachmentlist, ismoder == 1);

            if (post == null)
            {
                AddErrLine("读取信息失败");
                return;
            }

            posttree = Posts.GetPostTree(topicid);

            //更新查看次数
            //Topics.UpdateTopicViews(topicid);
            TopicStats.Track(topicid, 1);

            OnlineUsers.UpdateAction(olid, UserAction.ShowTopic.ActionID, forumid, forumname, topicid, topictitle,
                                     config.Onlinetimeout);
            score = Scoresets.GetValidScoreName();
            scoreunit = Scoresets.GetValidScoreUnit();

            //得到用户设置的每页显示帖子数各短消息数量
            if (userid != -1)
            {
                ShortUserInfo __userinfo = Discuz.Forum.Users.GetShortUserInfo(userid);
                if (__userinfo != null)
                {
                    if (__userinfo.Newpm == 0)
                    {
                        newpmcount = 0;
                    }
                }
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:44.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:44. 
            */

            base.OnLoad(e);


            int loopi = 1;


            if (page_err == 0)
            {


                if (enabletag)
                {

                    templateBuilder.Append("<script type=\"text/javascript\" src=\"cache/tag/closedtags.txt\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\" src=\"cache/tag/colorfultags.txt\"></" + "script>\r\n");

                }	//end if

                templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_showtopic.js\"></" + "script>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("var templatepath = \"" + templatepath.ToString() + "\";\r\n");
                templateBuilder.Append("var postminchars = parseInt(" + config.Minpostsize.ToString().Trim() + ");\r\n");
                templateBuilder.Append("var postmaxchars = parseInt(" + config.Maxpostsize.ToString().Trim() + ");\r\n");
                templateBuilder.Append("var disablepostctrl = parseInt(" + disablepostctrl.ToString() + ");\r\n");
                templateBuilder.Append("var forumpath = \"" + forumpath.ToString() + "\";\r\n");
                templateBuilder.Append("</" + "script>\r\n");
                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + " &raquo; \r\n");
                int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);


                if (ishtmltitle == 1)
                {

                    templateBuilder.Append("			<strong>" + Topics.GetHtmlTitle(topic.Tid).ToString().Trim() + "</strong>\r\n");

                }
                else
                {

                    templateBuilder.Append("			<strong>" + topictitle.ToString() + "</strong>\r\n");

                }	//end if

                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div id=\"headsearch\">\r\n");
                templateBuilder.Append("		<div id=\"search\">\r\n");

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

                templateBuilder.Append("		</div>\r\n");
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





                if (ispoll)
                {

                    templateBuilder.Append("<!--投票区开始-->\r\n");
                    templateBuilder.Append("<div class=\"box pollpanel\">\r\n");
                    templateBuilder.Append("	<form id=\"formpoll\" name=\"formpoll\" method=\"post\" action=\"poll.aspx?topicid=" + topicid.ToString() + "\">\r\n");
                    templateBuilder.Append("		<h4>\r\n");
                    templateBuilder.Append("		投票：<strong>" + topictitle.ToString() + "</strong>\r\n");

                    if (pollinfo.Multiple == 1)
                    {

                        templateBuilder.Append("		-多选(最多可选" + pollinfo.Maxchoices.ToString().Trim() + "项)\r\n");

                    }	//end if

                    templateBuilder.Append("		-截止时间：" + pollinfo.Expiration.ToString().Trim() + "\r\n");
                    templateBuilder.Append("		</h4>\r\n");
                    templateBuilder.Append("	<div class=\"postmessage\">\r\n");
                    templateBuilder.Append("		<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" summary=\"pollpanel\">\r\n");

                    int polloption__loop__id = 0;
                    foreach (DataRow polloption in polloptionlist.Rows)
                    {
                        polloption__loop__id++;

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<td style=\"padding:3px 0;\">\r\n");

                        if (allowvote)
                        {


                            if (pollinfo.Multiple == 1)
                            {

                                templateBuilder.Append("					<input type=\"checkbox\" name=\"pollitemid\" value=\"" + polloption["polloptionid"].ToString().Trim() + "\" onclick='checkbox(this)'/>\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("					<input type=\"radio\" name=\"pollitemid\"  value=\"" + polloption["polloptionid"].ToString().Trim() + "\"  />\r\n");

                            }	//end if


                        }	//end if

                        templateBuilder.Append("				" + polloption["name"].ToString().Trim() + "\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			<td width=\"420\">\r\n");

                        if (showpollresult)
                        {

                            templateBuilder.Append("				<div class=\"optionbar\">\r\n");
                            templateBuilder.Append("					<div style=\"width:" + polloption["percentwidth"].ToString().Trim() + "px\"></div>\r\n");
                            templateBuilder.Append("				</div>				\r\n");
                            templateBuilder.Append("				<div id=\"voterlist" + polloption["polloptionid"].ToString().Trim() + "\" style=\"display:none;clear:both;\">\r\n");

                            if (useradminid == 1 && polloption["value"].ToString().Trim() != "0")
                            {

                                templateBuilder.Append("					 " + polloption["votername"].ToString().Trim() + "\r\n");

                            }	//end if

                            templateBuilder.Append("				</div>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("				&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			<td>\r\n");

                        if (showpollresult)
                        {

                            templateBuilder.Append("			<strong>" + polloption["value"].ToString().Trim() + "</strong>票 / " + polloption["percent"].ToString().Trim() + "\r\n");

                            if (useradminid == 1 && polloption["value"].ToString().Trim() != "0")
                            {

                                templateBuilder.Append("				( <a href=\"javascript:void(0);\" onclick=\"javascript:displayvoter($('voterlist" + polloption["polloptionid"].ToString().Trim() + "'));\">查看该项投票人</a> )\r\n");

                            }	//end if


                        }
                        else
                        {

                            templateBuilder.Append("			&nbsp;\r\n");

                        }	//end if

                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end loop

                    templateBuilder.Append("		<script language=\"javascript\">\r\n");
                    templateBuilder.Append("		var max_obj = " + pollinfo.Maxchoices.ToString().Trim() + ";\r\n");
                    templateBuilder.Append("	    var p = 0;\r\n");
                    templateBuilder.Append("	    function checkbox(obj) {\r\n");
                    templateBuilder.Append("	        if(obj.checked) {\r\n");
                    templateBuilder.Append("		        p++;\r\n");
                    templateBuilder.Append("		        for (var i = 0; i < $('formpoll').elements.length; i++) {\r\n");
                    templateBuilder.Append("			        var e = $('formpoll').elements[i];\r\n");
                    templateBuilder.Append("			        if(p == max_obj) {\r\n");
                    templateBuilder.Append("				        if(e.name.match('pollitemid') && !e.checked) {\r\n");
                    templateBuilder.Append("					        e.disabled = true;\r\n");
                    templateBuilder.Append("				        }\r\n");
                    templateBuilder.Append("			        }\r\n");
                    templateBuilder.Append("		        }\r\n");
                    templateBuilder.Append("	        } else {\r\n");
                    templateBuilder.Append("		        p--;\r\n");
                    templateBuilder.Append("		        for (var i = 0; i < $('formpoll').elements.length; i++) {\r\n");
                    templateBuilder.Append("			        var e = $('formpoll').elements[i];\r\n");
                    templateBuilder.Append("			        if(e.name.match('pollitemid') && e.disabled) {\r\n");
                    templateBuilder.Append("				        e.disabled = false;\r\n");
                    templateBuilder.Append("			        }\r\n");
                    templateBuilder.Append("		        }\r\n");
                    templateBuilder.Append("	        }\r\n");
                    templateBuilder.Append("        }	   \r\n");
                    templateBuilder.Append("	    function displayvoter(objid) {\r\n");
                    templateBuilder.Append("	        if(objid.style.display == 'block') {\r\n");
                    templateBuilder.Append("	           objid.style.display = 'none';\r\n");
                    templateBuilder.Append("	        }\r\n");
                    templateBuilder.Append("	        else {\r\n");
                    templateBuilder.Append("	           objid.style.display = 'block';\r\n");
                    templateBuilder.Append("	        }\r\n");
                    templateBuilder.Append("	    }	    \r\n");
                    templateBuilder.Append("	    </" + "script>\r\n");

                    if (config.Silverlight == 1)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("			<td colspan=\"3\"><iframe src=\"silverlight/piechart/index.html?topicid=" + topicid.ToString() + "&bg=F5FAFE\" allowtransparency=\"yes\" width=\"95%\" height=\"425\" border=\"0\" scrolling=\"no\" marginheight=\"0\" marginwidth=\"0\" frameborder=\"0\" ></iframe>\r\n");
                        templateBuilder.Append("			</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<td colspan=\"3\" style=\"padding:4px 0;\">\r\n");

                    if (useradminid == 1 && voters != "")
                    {

                        templateBuilder.Append("					<button onclick=\"expandoptions('ticketvoterlist');\" />查看投票用户名单>></button>\r\n");

                    }	//end if


                    if (usergroupinfo.Allowvote == 1)
                    {


                        if (allowvote)
                        {

                            templateBuilder.Append("						<input type=\"submit\" name=\"Submit\" value=\"投票\"/>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						提示: 您已经投过票或者投票已经过期\r\n");

                        }	//end if


                    }
                    else
                    {

                        templateBuilder.Append("					抱歉,您没有参与投票的权限\r\n");

                    }	//end if

                    templateBuilder.Append("			 </td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		</table>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	</form>\r\n");
                    templateBuilder.Append("</div>\r\n");

                    if (useradminid == 1 && voters != "")
                    {

                        templateBuilder.Append("<div id=\"ticketvoterlist\" style=\"display:none;\" class=\"box\">\r\n");
                        templateBuilder.Append("	<h4>投票用户名单</h4>\r\n");
                        templateBuilder.Append("	<div class=\"postmessage\">\r\n");
                        templateBuilder.Append("	<table width=\"96%\" border=\"0\" cellpadding=\"4\" cellspacing=\"0\" summary=\"投票用户名单\">\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<td>" + voters.ToString() + "</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("	</table>\r\n");
                        templateBuilder.Append("	</div>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }	//end if

                    templateBuilder.Append("<!--投票区结束-->\r\n");

                }	//end if



                templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                templateBuilder.Append("	<span class=\"postbtn\">	\r\n");

                if (userid < 0 || canposttopic)
                {

                    templateBuilder.Append("	    <span onmouseover=\"$('newspecial').id = 'newspecialtmp';this.id = 'newspecial';if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"newspecial\" class=\"postbtn\"><a title=\"发新话题\" id=\"newtopic\" href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "\" onmouseover=\"if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\"><img alt=\"发新话题\" src=\"templates/" + templatepath.ToString() + "/images/newtopic.gif\"/></a></span>\r\n");

                }	//end if


                if (canreply)
                {

                    templateBuilder.Append("	    <SPAN class=\"replybtn\"><a href=\"postreply.aspx?topicid=" + topicid.ToString() + "\"><img src=\"templates/" + templatepath.ToString() + "/images/reply.gif\" border=\"0\" alt=\"回复该主题\" /></a></span>\r\n");

                }	//end if

                templateBuilder.Append("	</span>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<div class=\"showTable viewthread\">\r\n");
                templateBuilder.Append("	<h3>\r\n");

                if (forum.Applytopictype == 1 && forum.Topictypeprefix == 1)
                {

                    templateBuilder.Append("			" + topictypes.ToString() + " \r\n");

                }	//end if

                templateBuilder.Append("			" + topictitle.ToString() + "\r\n");

                if (topic.Price > 0 && topic.Posterid != userid)
                {

                    templateBuilder.Append("			<a href=\"buytopic.aspx?topicid=" + topicid.ToString() + "&showpayments=1\">浏览需支付 " + userextcreditsinfo.Name.ToString().Trim() + " " + topic.Price.ToString().Trim() + " " + userextcreditsinfo.Unit.ToString().Trim() + "</a>\r\n");

                }	//end if

                templateBuilder.Append("	</h3>\r\n");
                templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                templateBuilder.Append("		<tbody>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("		<td class=\"postauthor\" id=\"" + post.Pid.ToString().Trim() + "\">\r\n");
                templateBuilder.Append("		<!--register user-->\r\n");

                if (post.Posterid != -1)
                {

                    templateBuilder.Append("			<cite>\r\n");
                    templateBuilder.Append("				<a href=\"#\" target=\"_blank\" id=\"memberinfo_" + loopi.ToString() + "\" class=\"dropmenu\" onmouseover=\"showMenu(this.id)\">\r\n");

                    if (post.Onlinestate == 1)
                    {

                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/useronline.gif\" alt=\"在线\" title=\"在线\"/>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/useroutline.gif\"  alt=\"离线\" title=\"离线\"/>\r\n");

                    }	//end if

                    templateBuilder.Append("				" + post.Poster.ToString().Trim() + "\r\n");
                    templateBuilder.Append("				</a>\r\n");
                    templateBuilder.Append("			</cite>\r\n");

                    if (config.Showavatars == 1 && post.Avatar != "")
                    {

                        templateBuilder.Append("			<div class=\"avatar\">\r\n");
                        templateBuilder.Append("				<img class=\"avatar\" src=\"" + post.Avatar.ToString().Trim() + "\" onerror=\"this.onerror=null;this.src='templates/" + templatepath.ToString() + "/images/noavatar.gif';\" \r\n");

                        if (post.Avatarwidth > 0)
                        {

                            templateBuilder.Append("							width=\"" + post.Avatarwidth.ToString().Trim() + "\"\r\n");
                            templateBuilder.Append("							height=\"" + post.Avatarheight.ToString().Trim() + "\"\r\n");

                        }	//end if

                        templateBuilder.Append(" alt=\"头像\"\r\n");
                        templateBuilder.Append("					/>	\r\n");
                        templateBuilder.Append("			</div>		\r\n");

                    }	//end if


                    if (post.Nickname != "")
                    {

                        templateBuilder.Append("			<p><em>" + post.Nickname.ToString().Trim() + "</em></p>\r\n");

                    }	//end if

                    templateBuilder.Append("			<p>\r\n");
                    templateBuilder.Append("			<script language=\"javascript\" type=\"text/javascript\">\r\n");
                    templateBuilder.Append("				ShowStars(" + post.Stars.ToString().Trim() + ", " + config.Starthreshold.ToString().Trim() + ");\r\n");
                    templateBuilder.Append("			</" + "script>\r\n");
                    templateBuilder.Append("			</p>\r\n");

                    templateBuilder.Append("			<ul class=\"otherinfo\">\r\n");

                    if (config.Userstatusby == 1)
                    {

                        templateBuilder.Append("				<li>组别:" + post.Status.ToString().Trim() + "</li>\r\n");

                    }	//end if

                    templateBuilder.Append("				<li>性别:<script type=\"text/javascript\">document.write(displayGender(" + post.Gender.ToString().Trim() + "));</" + "script></li>\r\n");

                    if (post.Bday != "")
                    {

                        templateBuilder.Append("				<li>生日:" + post.Bday.ToString().Trim() + "</li>\r\n");

                    }	//end if

                    templateBuilder.Append("				<li>来自:" + post.Location.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("				<li>金币:" + post.Credits.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("				<li>帖子:" + post.Posts.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("				<li>注册:\r\n");

                    if (post.Joindate != "")
                    {

                        templateBuilder.Append(Convert.ToDateTime(post.Joindate).ToString("yyyy-MM-dd"));

                    }	//end if

                    templateBuilder.Append("</li>\r\n");
                    templateBuilder.Append("				</ul>\r\n");
                    templateBuilder.Append("			</div>\r\n");

                    if (post.Medals != "")
                    {

                        templateBuilder.Append("			<div class=\"medals\">" + post.Medals.ToString().Trim() + "</div>\r\n");

                    }	//end if


                }
                else
                {

                    templateBuilder.Append("				<div class=\"ipshow\"><strong>" + post.Poster.ToString().Trim() + "</strong>  " + post.Ip.ToString().Trim() + "\r\n");

                    if (useradminid > 0 && admininfo.Allowviewip == 1)
                    {

                        templateBuilder.Append("					<a href=\"getip.aspx?pid=" + post.Pid.ToString().Trim() + "&topicid=" + topicid.ToString() + "\" title=\"查看IP\"><img src=\"templates/" + templatepath.ToString() + "/images/ip.gif\" alt=\"查看IP\" /></a>\r\n");

                    }	//end if

                    templateBuilder.Append("				</div>\r\n");
                    templateBuilder.Append("			<!--guest-->\r\n");
                    templateBuilder.Append("				<div class=\"noregediter\">\r\n");
                    templateBuilder.Append("					未注册\r\n");
                    templateBuilder.Append("				</div>\r\n");

                }	//end if

                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("		<td class=\"postcontent\">\r\n");
                templateBuilder.Append("			<div class=\"postinfo\">\r\n");
                templateBuilder.Append("				<em>\r\n");
                templateBuilder.Append(Convert.ToDateTime(post.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
                templateBuilder.Append("</em>\r\n");
                aspxrewriteurl = this.ShowTopicAspxRewrite(topicid, 0);

                templateBuilder.Append("				<a href=\"" + aspxrewriteurl.ToString() + "\">平板</a>|\r\n");
                templateBuilder.Append("				<a href=\"favorites.aspx?topicid=" + topicid.ToString() + "\">收藏</a>|\r\n");

                if (ismoder == 1)
                {

                    templateBuilder.Append("					<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + post.Pid.ToString().Trim() + "\">编辑</a>|\r\n");
                    templateBuilder.Append("					<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + post.Pid.ToString().Trim() + "\" onclick=\"return confirm('确定要删除吗?');\">删除</a>|\r\n");

                    if (post.Posterid != -1)
                    {

                        templateBuilder.Append("						<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'" + post.Pid.ToString().Trim() + "');\">评分</a>|\r\n");

                        if (post.Ratetimes > 0)
                        {

                            templateBuilder.Append("						<a href=\"###\" onclick=\"action_onchange('cancelrate',$('moderate'),'" + post.Pid.ToString().Trim() + "');\">撤销评分</a>&nbsp;\r\n");

                        }	//end if


                        if (post.Invisible == -2)
                        {

                            templateBuilder.Append("					<a href=\"###\" onclick=\"action_onchange('banpost',$('moderate'),'" + post.Pid.ToString().Trim() + "','0');\">撤销屏蔽</a>|\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					<a href=\"###\" onclick=\"action_onchange('banpost',$('moderate'),'" + post.Pid.ToString().Trim() + "','-2');\">屏蔽帖子</a>|\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("					<input name=\"postid\" id=\"postid\" value=\"" + post.Pid.ToString().Trim() + "\" type=\"checkbox\" />\r\n");

                }
                else
                {


                    if (post.Posterid != -1 && userid == post.Posterid)
                    {


                        if (topic.Closed == 0)
                        {

                            templateBuilder.Append("							<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + post.Pid.ToString().Trim() + "\">编辑</a>|\r\n");

                        }	//end if

                        templateBuilder.Append("						<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + post.Pid.ToString().Trim() + "\" onclick=\"return confirm('确定要删除吗?');\">删除</a>|\r\n");

                    }	//end if


                    if (usergroupinfo.Raterange != "" && post.Posterid != -1)
                    {

                        templateBuilder.Append("<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'" + post.Pid.ToString().Trim() + "');\">评分</a>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("				<a href=\"###\" class=\"t_number\" onclick=\"$('message" + post.Pid.ToString().Trim() + "').className='t_smallfont'\">小</a>			\r\n");
                templateBuilder.Append("				<a href=\"###\" class=\"t_number\" onclick=\"$('message" + post.Pid.ToString().Trim() + "').className='t_msgfont'\">中</a>\r\n");
                templateBuilder.Append("				<a href=\"###\" class=\"t_number\" onclick=\"$('message" + post.Pid.ToString().Trim() + "').className='t_bigfont'\">大</a>\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("			<div id=\"ad_thread2_1\"></div>\r\n");
                templateBuilder.Append("			<div class=\"postmessage defaultpost viewthreadShowTable\">\r\n");

                if (post.Invisible != -2 || ismoder == 1)
                {


                    if (topic.Identify > 0)
                    {

                        templateBuilder.Append("				<div class=\"ntprint\" onclick=\"this.style.display='none';\"><img onload=\"setIdentify(this.parentNode);\" src=\"images/identify/" + topicidentify.Filename.ToString().Trim() + "\" alt=\"点击关闭鉴定图章\" title=\"点击关闭鉴定图章\" /></div>\r\n");

                    }	//end if


                    if (ismoder == 1 && post.Invisible == -2)
                    {

                        templateBuilder.Append("						<!--管理组有权查看内容开始-->\r\n");
                        templateBuilder.Append("						<div class='hintinfo'>提示: 该帖已被屏蔽, 您拥有管理权限, 以下是帖子内容</div>		              \r\n");

                    }	//end if

                    templateBuilder.Append("				<h2>" + post.Title.ToString().Trim() + "</h2>\r\n");
                    templateBuilder.Append("				<div id=\"message" + post.Pid.ToString().Trim() + "\" class=\"t_msgfont\">\r\n");
                    templateBuilder.Append("					<div id=\"ad_thread3_1\"></div>\r\n");

                    if (post.Layer == 0 && enabletag)
                    {

                        templateBuilder.Append("						<div id=\"firstpost\">\r\n");

                    }	//end if

                    //处理内部错误链接地址(CPS商家的链接)
                    string showMessage = post.Message.ToString().Trim();

                    //处理旧帖图片跟斜体
                    showMessage = showMessage.Replace("editor/images/smilies/旺旺表情/", "fckeditor/editor/images/smiley/msn/").Replace("editor/images/smilies/默认表情1/", "fckeditor/editor/images/smiley/msn/").Replace("<em>", "<I>").Replace("</em>", "</I>");

                    //处理外链(先查找出帖子中所有的超链接[a标签]，然后 逐一过滤，非我们网站的链接一律替换成 彩友双色球购彩大厅)

                    Regex regex = new Regex(@"<A[^>]*?HREF\s*=\s*[""']?([^'""   >]+?)[   '""]?>", RegexOptions.IgnoreCase);

                    MatchCollection matchCollections = regex.Matches(showMessage);

                    ArrayList arrHrefs = new ArrayList();

                    IEnumerator ienum = matchCollections.GetEnumerator();
                    while (ienum.MoveNext())
                    {
                        Match m = (Match)ienum.Current;
                        CaptureCollection cc = m.Captures;
                        for (int k = 0; k < cc.Count; k++)
                        {
                            Capture c = cc[k];
                            arrHrefs.Add(c.ToString());
                        }
                    }

                    string tempHref = "";
                    foreach (string str in arrHrefs)
                    {
                        tempHref = str.ToLower();
                        string ClubUrl = Discuz.Common.XmlConfig.GetCpsClubUrl().ToString();
                        string SiteUrl = Discuz.Common.XmlConfig.GetCpsProperty("SiteUrl","").ToString();
                        if (tempHref.IndexOf(ClubUrl) == -1 && tempHref.IndexOf(SiteUrl) == -1)
                        {
                            showMessage = showMessage.Replace(str, "<a href='" + SiteUrl + "/Lottery/Buy_SSQ.aspx'>");
                        }
                    }


                    templateBuilder.Append("						" + showMessage + "\r\n");
                    if (post.Layer == 0 && enabletag)
                    {

                        templateBuilder.Append("						</div>\r\n");

                    }	//end if

                    templateBuilder.Append("				</div>\r\n");

                    if (attachmentlist.Count > 0)
                    {

                        int currentattachcount = 0;


                        int attachtemp__loop__id = 0;
                        foreach (ShowtopicPageAttachmentInfo attachtemp in attachmentlist)
                        {
                            attachtemp__loop__id++;


                            if (attachtemp.Pid == post.Pid)
                            {

                                currentattachcount = currentattachcount + 1;


                            }	//end if


                        }	//end loop


                        if (currentattachcount > 0)
                        {

                            int getattachperm = attachmentlist[0].Getattachperm;


                            if (getattachperm == 1)
                            {

                                templateBuilder.Append("					<div class=\"box postattachlist\">\r\n");
                                templateBuilder.Append("						<h4>附件</h4>\r\n");

                                int attachment__loop__id = 0;
                                foreach (ShowtopicPageAttachmentInfo attachment in attachmentlist)
                                {
                                    attachment__loop__id++;


                                    if (attachment.Pid == post.Pid)
                                    {

                                        templateBuilder.Append("							<!--附件开始-->\r\n");

                                        if (attachment.Allowread == 1)
                                        {


                                            templateBuilder.Append("										<dl class=\"t_attachlist\">\r\n");
                                            templateBuilder.Append("											<dt>\r\n");
                                            templateBuilder.Append("											<img class=\"absmiddle\" border=\"0\" alt=\"\" src=\"images/attachicons/image.gif\"/>\r\n");
                                            templateBuilder.Append("											<a class=\"bold\" target=\"_blank\" href=\"attachment.aspx?attachmentid=" + attachment.Aid.ToString().Trim() + "\">" + attachment.Attachment.ToString().Trim() + "</a>\r\n");
                                            templateBuilder.Append("											<em>(<script type=\"text/javascript\">ShowFormatBytesStr(" + attachment.Filesize.ToString().Trim() + ");</" + "script>)</em>\r\n");
                                            templateBuilder.Append("											</dt>\r\n");
                                            templateBuilder.Append("											<dd>\r\n");
                                            templateBuilder.Append("											<p> " + attachment.Postdatetime.ToString().Trim() + " </p>\r\n");

                                            if (attachment.Preview != "")
                                            {

                                                templateBuilder.Append("											<p>" + attachment.Preview.ToString().Trim() + "</p>\r\n");

                                            }	//end if

                                            templateBuilder.Append("											<p>\r\n");

                                            if (allowdownloadattach)
                                            {

                                                templateBuilder.Append("<!--当用户有下载附件权限时-->\r\n");

                                                if (config.Showimages == 1)
                                                {


                                                    if (config.Attachimgpost == 1)
                                                    {


                                                        if (attachment.Attachimgpost == 1)
                                                        {

                                                            templateBuilder.Append("														<img alt=\"" + attachment.Attachment.ToString().Trim() + "\" \r\n");

                                                            if (config.Showattachmentpath == 1)
                                                            {

                                                                templateBuilder.Append("															src=\"upload/" + attachment.Filename.ToString().Trim() + "\"\r\n");

                                                            }
                                                            else
                                                            {

                                                                templateBuilder.Append("															src=\"attachment.aspx?attachmentid=" + attachment.Aid.ToString().Trim() + "\"\r\n");

                                                            }	//end if

                                                            templateBuilder.Append("													onmouseover=\"attachimg(this, 'mouseover')\" onload=\"attachimg(this, 'load');\" onclick=\"zoom(this, this.src);\" />\r\n");

                                                        }	//end if


                                                    }	//end if

                                                    templateBuilder.Append("												<br />\r\n");

                                                }	//end if


                                            }
                                            else
                                            {

                                                templateBuilder.Append("											<span class=\\\"attachnotdown\\\">您所在的用户组无法下载或查看附件</span>\r\n");

                                            }	//end if

                                            templateBuilder.Append("											</p>\r\n");
                                            templateBuilder.Append("											</dd>										\r\n");
                                            templateBuilder.Append("										</dl>\r\n");



                                        }
                                        else
                                        {

                                            templateBuilder.Append("								<span class=\"notdown\">你的下载权限 " + usergroupinfo.Readaccess.ToString().Trim() + " 低于此附件所需权限 " + attachment.Readperm.ToString().Trim() + ", 你无权查看此附件</span>\r\n");

                                        }	//end if

                                        templateBuilder.Append("							<!--附件结束-->\r\n");

                                    }	//end if


                                }	//end loop

                                templateBuilder.Append("					</div>\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("					<div class=\"notice\" style=\"width: 500px;\">\r\n");
                                templateBuilder.Append("					附件:<em>您所在的用户组无法下载或查看附件</em>\r\n");
                                templateBuilder.Append("					</div>\r\n");

                            }	//end if


                        }	//end if


                    }	//end if


                    if (post.Ratetimes > 0)
                    {


                        templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("function ratevalveimg(rate,ratevalveset)\r\n");
                        templateBuilder.Append("{\r\n");
                        templateBuilder.Append("    valveimg = '';\r\n");
                        templateBuilder.Append("	if(rate) {\r\n");
                        templateBuilder.Append("		image = rate > 0 ? 'templates/" + templatepath.ToString() + "/images/agree.gif' : 'templates/" + templatepath.ToString() + "/images/disagree.gif';\r\n");
                        templateBuilder.Append("		var  ratevalve = ratevalveset.split(\",\");\r\n");
                        templateBuilder.Append("		for(i = 0; i < ratevalve.length; i++) {\r\n");
                        templateBuilder.Append("		    if(Math.abs(rate)>ratevalve[i]){ \r\n");
                        templateBuilder.Append("			    valveimg += '<img src=\"' + image + '\" border=\"0\" alt=\"\" />';\r\n");
                        templateBuilder.Append("			}\r\n");
                        templateBuilder.Append("			else{\r\n");
                        templateBuilder.Append("			    break;\r\n");
                        templateBuilder.Append("			}\r\n");
                        templateBuilder.Append("		}\r\n");
                        templateBuilder.Append("	}\r\n");
                        templateBuilder.Append("	return valveimg;\r\n");
                        templateBuilder.Append("}\r\n");
                        templateBuilder.Append("</" + "script>\r\n");
                        templateBuilder.Append("	<div class=\"ratelog\">\r\n");

                        if (showratelog == 1)
                        {

                            templateBuilder.Append("		<div id=\"rate_" + post.Pid.ToString().Trim() + "_fake\"><script type=\"text/javascript\">document.write(ratevalveimg(" + post.Rate.ToString().Trim() + ",'" + config.Ratevalveset.ToString().Trim() + "'));</" + "script> <a href=\"###\" title=\"查看/收起 评分记录\" onclick=\"showrate(" + post.Pid.ToString().Trim() + "," + config.Aspxrewrite.ToString().Trim() + ")\">本帖被评分 " + post.Ratetimes.ToString().Trim() + " 次</a></div>\r\n");
                            templateBuilder.Append("		<div id=\"rate_" + post.Pid.ToString().Trim() + "_real\" style=\"display:none\">\r\n");
                            templateBuilder.Append("		<fieldset style=\"width: 600px;\">\r\n");
                            templateBuilder.Append("			<legend><script>document.write(ratevalveimg(" + post.Rate.ToString().Trim() + ",'" + config.Ratevalveset.ToString().Trim() + "'));</" + "script> <a href=\"###\" title=\"查看/收起 评分记录\" onclick=\"showrate(" + post.Pid.ToString().Trim() + "," + config.Aspxrewrite.ToString().Trim() + ")\">本帖被评分 " + post.Ratetimes.ToString().Trim() + " 次</a> </legend>\r\n");
                            templateBuilder.Append("			<div id=\"rate_" + post.Pid.ToString().Trim() + "\" style=\"margin: 5px; height: 77px; overflow: auto;\"></div>\r\n");
                            templateBuilder.Append("		</fieldset>\r\n");
                            templateBuilder.Append("		</div>\r\n");
                            templateBuilder.Append("	<script type=\"text/javascript\">_attachEvent(window, \"load\", function(){ showrate(" + post.Pid.ToString().Trim() + "," + config.Aspxrewrite.ToString().Trim() + "); })</" + "script>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("	<script type=\"text/javascript\">document.write(ratevalveimg(" + post.Rate.ToString().Trim() + ",'" + config.Ratevalveset.ToString().Trim() + "'));</" + "script>本帖被评分 " + post.Ratetimes.ToString().Trim() + " 次\r\n");

                        }	//end if

                        templateBuilder.Append("	</div>\r\n");



                    }	//end if


                    if (post.Lastedit != "")
                    {

                        templateBuilder.Append("				<!--最后编辑开始-->\r\n");
                        templateBuilder.Append("				<div class=\"lastediter\">\r\n");
                        templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/lastedit.gif\" alt=\"最后编辑\"/>" + post.Lastedit.ToString().Trim() + "\r\n");
                        templateBuilder.Append("				</div>\r\n");
                        templateBuilder.Append("				<!--最后编辑结束-->\r\n");

                    }	//end if


                    if (post.Id == 1)
                    {


                        if (topic.Special == 3)
                        {

                            templateBuilder.Append("						<div class=\"quote\">\r\n");
                            templateBuilder.Append("							<div class=\"text\"><p>本帖得分:</p>\r\n");
                            templateBuilder.Append("								<div class=\"attachmentinfo\">\r\n");

                            int bonuslog__loop__id = 0;
                            foreach (BonusLogInfo bonuslog in bonuslogs)
                            {
                                bonuslog__loop__id++;

                                aspxrewriteurl = this.UserInfoAspxRewrite(bonuslog.Answerid);

                                string unit = scoreunit[bonuslog.Extid];

                                string name = score[bonuslog.Extid];

                                templateBuilder.Append("										<a href=\"" + aspxrewriteurl.ToString() + "\">" + bonuslog.Answername.ToString().Trim() + "</a>(" + bonuslog.Bonus.ToString().Trim() + " " + unit.ToString() + "" + name.ToString() + ")\r\n");

                                if (bonuslog__loop__id != bonuslogs.Count)
                                {

                                    templateBuilder.Append("											,\r\n");

                                }	//end if


                            }	//end loop

                            templateBuilder.Append("								</div>\r\n");
                            templateBuilder.Append("							</div>\r\n");
                            templateBuilder.Append("						</div>\r\n");

                        }	//end if


                    }	//end if


                    if (forum.Allowtag == 1 && post.Layer == 0)
                    {

                        templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("					function forumhottag_callback(data)\r\n");
                        templateBuilder.Append("					{\r\n");
                        templateBuilder.Append("						tags = data;\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("				</" + "script>\r\n");
                        templateBuilder.Append("				<script type=\"text/javascript\" src=\"cache/hottags_forum_cache_jsonp.txt\"></" + "script>\r\n");
                        templateBuilder.Append("				<div id=\"topictag\">\r\n");
                        int hastag = Topics.GetMagicValue(topic.Magic, MagicType.TopicTag);


                        if (hastag == 1)
                        {

                            templateBuilder.Append("						<script type=\"text/javascript\">getTopicTags(" + topic.Tid.ToString().Trim() + ");</" + "script>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("						<script type=\"text/javascript\">parsetag();</" + "script>\r\n");

                        }	//end if

                        templateBuilder.Append("				</div>\r\n");

                    }	//end if

                    templateBuilder.Append("			</div>\r\n");

                }
                else
                {

                    templateBuilder.Append("			<div id=\"message" + post.Pid.ToString().Trim() + "\" class=\"t_msgfont\">\r\n");
                    templateBuilder.Append("			          <div class='hintinfo'>提示: 该帖被管理员或版主屏蔽</div>\r\n");
                    templateBuilder.Append("			</div>\r\n");

                }	//end if


                if (config.Showsignatures == 1)
                {


                    if (post.Usesig == 1)
                    {


                        if (post.Signature != "")
                        {

                            templateBuilder.Append("			<!--签名开始-->\r\n");
                            templateBuilder.Append("				<div class=\"postertext\">\r\n");

                            if (config.Maxsigrows > 0)
                            {

                                int ieheight = config.Maxsigrows * 12;

                                templateBuilder.Append("						<div class=\"signatures\" style=\"max-height: " + config.Maxsigrows.ToString().Trim() + "em;maxHeightIE:" + ieheight.ToString() + "px\">" + post.Signature.ToString().Trim() + "</div>\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("						" + post.Signature.ToString().Trim() + "\r\n");

                            }	//end if

                            templateBuilder.Append("				</div>\r\n");
                            templateBuilder.Append("			<!--签名结束-->\r\n");

                        }	//end if


                    }	//end if


                }	//end if

                templateBuilder.Append("	</td>\r\n");
                templateBuilder.Append("	</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("	<tbody>\r\n");
                templateBuilder.Append("	<tr>\r\n");
                templateBuilder.Append("	<!--用户信息结束-->\r\n");
                templateBuilder.Append("		<td class=\"postauthor\">&nbsp;</td>\r\n");
                templateBuilder.Append("		<td class=\"postcontent\">\r\n");
                templateBuilder.Append("			<div class=\"postactions\">\r\n");
                templateBuilder.Append("				<p>\r\n");

                if (userid != -1)
                {


                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("    show_report_button();\r\n");
                    templateBuilder.Append("</" + "script>\r\n");


                    templateBuilder.Append("|\r\n");

                }	//end if


                if (canreply)
                {

                    templateBuilder.Append("					<a href=\"postreply.aspx?topicid=" + topicid.ToString() + "&postid=" + post.Pid.ToString().Trim() + "&quote=yes\">引用</a>|\r\n");

                    if (userid != -1)
                    {

                        templateBuilder.Append("					<a href=\"###\" onclick=\"replyToFloor('" + post.Id.ToString().Trim() + "', '" + post.Poster.ToString().Trim() + "', '" + post.Pid.ToString().Trim() + "')\">回复</a>|\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("					<a href=\"###\" onclick=\"window.scrollTo(0,0)\">TOP</a>\r\n");
                templateBuilder.Append("				</p>\r\n");
                templateBuilder.Append("				<div id=\"ad_thread1_1\"></div>\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("	</tr>\r\n");
                templateBuilder.Append("	</tbody>\r\n");
                templateBuilder.Append("</table>\r\n");
                templateBuilder.Append("</div>\r\n");

                if (post.Posterid != -1)
                {

                    templateBuilder.Append("<!-- member menu -->\r\n");
                    templateBuilder.Append("<div class=\"popupmenu_popup userinfopanel\" id=\"memberinfo_" + loopi.ToString() + "_menu\" style=\"display: none; z-index: 50; filter: progid:dximagetransform.microsoft.shadow(direction=135,color=#cccccc,strength=2); left: 19px; clip: rect(auto auto auto auto); position absolute; top: 253px; width:150px;\" initialized ctrlkey=\"userinfo2\" h=\"209\">\r\n");
                    templateBuilder.Append("	<p class=\"recivemessage\"><a href=\"usercppostpm.aspx?msgtoid=" + post.Posterid.ToString().Trim() + "\" target=\"_blank\">发送短消息</a></p>\r\n");

                    if (useradminid > 0)
                    {


                        if (admininfo.Allowviewip == 1)
                        {

                            templateBuilder.Append("			<p  class=\"seeip\"><a href=\"getip.aspx?pid=" + post.Pid.ToString().Trim() + "&topicid=" + topicid.ToString() + "\" title=\"查看IP\">查看IP</a></p>\r\n");

                        }	//end if


                        if (admininfo.Allowbanuser == 1)
                        {

                            templateBuilder.Append("			<p><a href=\"useradmin.aspx?action=banuser&uid=" + post.Posterid.ToString().Trim() + "\" title=\"禁止用户\">禁止用户</a></p>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("	<p>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);

                    templateBuilder.Append("		<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">查看公共资料</a></p>\r\n");
                    templateBuilder.Append("	<p><a href=\"search.aspx?posterid=" + post.Posterid.ToString().Trim() + "\">查找该会员全部帖子</a></p>\r\n");
                    templateBuilder.Append("	<ul>\r\n");
                    templateBuilder.Append("		<li>UID:" + post.Posterid.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("		<li>精华:\r\n");

                    if (post.Digestposts > 0)
                    {

                        templateBuilder.Append("<a href=\"search.aspx?posterid=" + post.Posterid.ToString().Trim() + "&type=digest\">" + post.Digestposts.ToString().Trim() + "</a>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("" + post.Digestposts.ToString().Trim() + "\r\n");

                    }	//end if

                    templateBuilder.Append("</li>\r\n");

                    if (score[1].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[1].ToString().Trim() + ":" + post.Extcredits1.ToString().Trim() + " " + scoreunit[1].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[2].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[2].ToString().Trim() + ":" + post.Extcredits2.ToString().Trim() + " " + scoreunit[2].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[3].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[3].ToString().Trim() + ":" + post.Extcredits3.ToString().Trim() + " " + scoreunit[3].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[4].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[4].ToString().Trim() + ":" + post.Extcredits4.ToString().Trim() + " " + scoreunit[4].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[5].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[5].ToString().Trim() + ":" + post.Extcredits5.ToString().Trim() + " " + scoreunit[5].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[6].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[6].ToString().Trim() + ":" + post.Extcredits6.ToString().Trim() + " " + scoreunit[6].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[7].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[7].ToString().Trim() + ":" + post.Extcredits7.ToString().Trim() + " " + scoreunit[7].ToString().Trim() + "</li>\r\n");

                    }	//end if


                    if (score[8].ToString().Trim() != "")
                    {

                        templateBuilder.Append("			<li>" + score[8].ToString().Trim() + ":" + post.Extcredits8.ToString().Trim() + " " + scoreunit[8].ToString().Trim() + "</li>\r\n");

                    }	//end if

                    templateBuilder.Append("	</ul>\r\n");
                    templateBuilder.Append("	<p>状态:\r\n");

                    if (post.Onlinestate == 1)
                    {

                        templateBuilder.Append("				在线\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("				离线\r\n");

                    }	//end if

                    templateBuilder.Append("	</p>\r\n");
                    templateBuilder.Append("	<ul class=\"tools\">\r\n");

                    if (post.Msn != "")
                    {

                        templateBuilder.Append("		<li>\r\n");
                        templateBuilder.Append("			<img src=\"templates/" + templatepath.ToString() + "/images/msnchat.gif\" title=\"MSN Messenger: " + post.Msn.ToString().Trim() + "\" alt=\"MSN Messenger: " + post.Msn.ToString().Trim() + "\" />\r\n");
                        templateBuilder.Append("			<a href=\"mailto:" + post.Msn.ToString().Trim() + "\" target=\"_blank\">" + post.Msn.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("		</li>\r\n");

                    }	//end if


                    if (post.Skype != "")
                    {

                        templateBuilder.Append("		<li>\r\n");
                        templateBuilder.Append("			<img src=\"templates/" + templatepath.ToString() + "/images/skype.gif\" title=\"Skype: " + post.Skype.ToString().Trim() + "\" alt=\"Skype: " + post.Skype.ToString().Trim() + "\" />\r\n");
                        templateBuilder.Append("			<a href=\"skype:" + post.Skype.ToString().Trim() + "\" target=\"_blank\">" + post.Skype.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("		</li>\r\n");

                    }	//end if


                    if (post.Icq != "")
                    {

                        templateBuilder.Append("		<li>\r\n");
                        templateBuilder.Append("			<img src=\"templates/" + templatepath.ToString() + "/images/icq.gif\" alt=\"ICQ: " + post.Icq.ToString().Trim() + "\" alt=\"ICQ: " + post.Icq.ToString().Trim() + "\" />\r\n");
                        templateBuilder.Append("			<a href=\"http://wwp.icq.com/scripts/search.dll?to=" + post.Icq.ToString().Trim() + "\" target=\"_blank\">" + post.Icq.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("		</li>\r\n");

                    }	//end if


                    if (post.Qq != "")
                    {

                        templateBuilder.Append("		<li>\r\n");
                        templateBuilder.Append("			<img src=\"templates/" + templatepath.ToString() + "/images/qq.gif\" alt=\"QQ: " + post.Qq.ToString().Trim() + "\" alt=\"QQ: " + post.Qq.ToString().Trim() + "\" />\r\n");
                        templateBuilder.Append("			<a href=\"http://wpa.qq.com/msgrd?V=1&Uin=" + post.Qq.ToString().Trim() + "&Site=" + config.Forumtitle.ToString().Trim() + "&Menu=yes\" target=\"_blank\">" + post.Qq.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("		</li>\r\n");

                    }	//end if


                    if (post.Yahoo != "")
                    {

                        templateBuilder.Append("				<li>\r\n");
                        templateBuilder.Append("			<img src=\"templates/" + templatepath.ToString() + "/images/yahoo.gif\" width=\"16\" alt=\"Yahoo Messenger: " + post.Yahoo.ToString().Trim() + "\"/>\r\n");
                        templateBuilder.Append("			<a href=\"http://edit.yahoo.com/config/send_webmesg?.target=" + post.Yahoo.ToString().Trim() + "&.src=pg\" target=\"_blank\">" + post.Yahoo.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("				</li>\r\n");

                    }	//end if

                    templateBuilder.Append("	</ul>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<!-- member menu -->\r\n");

                }	//end if

                templateBuilder.Append("<!--ntforumbox end-->\r\n");
                templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                templateBuilder.Append("	<span class=\"postbtn\">	\r\n");

                if (userid < 0 || canposttopic)
                {

                    templateBuilder.Append("	    <span onmouseover=\"$('newspecial').id = 'newspecialtmp';this.id = 'newspecial';if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\" id=\"Span1\" class=\"postbtn\"><a title=\"发新话题\" id=\"A1\" href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "\" onmouseover=\"if($('newspecial_menu').childNodes.length>0)  showMenu(this.id);\"><img alt=\"发新话题\" src=\"templates/" + templatepath.ToString() + "/images/newtopic.gif\"/></a></span>\r\n");

                }	//end if


                if (canreply)
                {

                    templateBuilder.Append("	    <SPAN class=\"replybtn\"><a href=\"postreply.aspx?topicid=" + topicid.ToString() + "\"><img src=\"templates/" + templatepath.ToString() + "/images/reply.gif\" border=\"0\" alt=\"回复该主题\" /></a></span>\r\n");

                }	//end if

                templateBuilder.Append("	</span>\r\n");
                templateBuilder.Append("</div>\r\n");

                if (canreply)
                {

                    templateBuilder.Append("<!--quickreply-->\r\n");
                    bool isenddebate = true;



                    if (quickeditorad != "")
                    {

                        templateBuilder.Append("<div class=\"leaderboard\">" + quickeditorad.ToString() + "</div>\r\n");

                    }	//end if

                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/post.js\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/bbcode.js\"></" + "script>\r\n");
                    templateBuilder.Append("<form method=\"post\" name=\"postform\" id=\"postform\" action=\"postreply.aspx?topicid=" + topicid.ToString() + "\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this);\">\r\n");
                    templateBuilder.Append("<div id=\"quickpost\" class=\"box\">\r\n");
                    templateBuilder.Append("	<h4>快速回复帖子</h4>\r\n");
                    //templateBuilder.Append("	<div class=\"postoptions\">\r\n");
                    //templateBuilder.Append("		<h5>选项</h5>\r\n");
                    //templateBuilder.Append("		<p><label><input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" \r\n");

                    //if (parseurloff == 1)
                    //{

                    //    templateBuilder.Append("checked \r\n");

                    //}	//end if

                    //templateBuilder.Append(" /> 禁用</label> URL 识别</label></p>\r\n");
                    //templateBuilder.Append("		<p><label><input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" \r\n");

                    //if (smileyoff == 1)
                    //{

                    //    templateBuilder.Append(" checked disabled \r\n");

                    //}	//end if

                    //templateBuilder.Append(" /> 禁用 </label>表情</p>\r\n");
                    //templateBuilder.Append("		<p><label><input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\" \r\n");

                    //if (bbcodeoff == 1)
                    //{

                    //    templateBuilder.Append(" checked disabled \r\n");

                    //}	//end if

                    //templateBuilder.Append(" /> 禁用 </label>代码模式</p>\r\n");
                    //templateBuilder.Append("		<p><label><input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\"\r\n");

                    //if (usesig == 1)
                    //{

                    //    templateBuilder.Append("checked\r\n");

                    //}	//end if

                    //templateBuilder.Append("/> 使用个人签名</label></p>\r\n");
                    //templateBuilder.Append("		<p><label><input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" /> 发送邮件通知楼主</label></p>	\r\n");
                    //templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	<div class=\"postform\">\r\n");
                    templateBuilder.Append("		<h5><label for=\"subject\">标题</label>\r\n");

                    if (isenddebate == false)
                    {

                        templateBuilder.Append("			<select name=\"debateopinion\" id=\"debateopinion\">\r\n");
                        templateBuilder.Append("				<option value=\"0\" selected></option>\r\n");
                        templateBuilder.Append("				<option value=\"1\">正方</option>\r\n");
                        templateBuilder.Append("				<option value=\"2\">反方</option>\r\n");
                        templateBuilder.Append("			</select>\r\n");

                    }	//end if

                    templateBuilder.Append("			<input type=\"text\" id=\"title\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\" /><input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"-1\" />\r\n");
                    templateBuilder.Append("		</h5>\r\n");
                    templateBuilder.Append("		<p><label>内容</label>\r\n");
                    //templateBuilder.Append("		<textarea rows=\"7\" cols=\"80\" name=\"message\" id=\"message\" onKeyDown=\"ctlent(event,false);\" tabindex=\"2\" class=\"autosave\" style=\"background:url(" + quickbgadimg.ToString() + ") no-repeat 0 0; \" \r\n");

                    //if (quickbgadlink != "")
                    //{

                    //    templateBuilder.Append(" onfocus=\"$('adlinkbtn').style.display='';$('closebtn').style.display='';this.onfocus=null;\"\r\n");

                    //}	//end if

                    //templateBuilder.Append("></textarea>\r\n");
                    templateBuilder.Append("<table class=\"editor_text\" summary=\"Message Textarea\" cellpadding=\"0\" cellspacing=\"0\" style=\"table-layout: fixed;border:1px solid white;padding-left:30px;\">\r\n");
                    templateBuilder.Append("<tr>\r\n");
                    templateBuilder.Append("	<td>\r\n");
                    templateBuilder.Append("        <link href=\"fckeditor/_samples/sample.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
                    templateBuilder.Append("        <script type=\"text/javascript\" src=\"fckeditor/fckeditor.js\"></script>\r\n");
                    templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("            var oFCKeditor = new FCKeditor('message');\r\n");
                    templateBuilder.Append("            oFCKeditor.Height	= 250 ;");
                    templateBuilder.Append("            oFCKeditor.BasePath = '" + Discuz.Common.XmlConfig.GetCpsClubUrl() + "/fckeditor/';\r\n");
                    templateBuilder.Append("            oFCKeditor.Config[\"AutoDetectLanguage\"] = false;\r\n");
                    templateBuilder.Append("            oFCKeditor.Config[\"DefaultLanguage\"] = 'zh-cn';\r\n");
                    templateBuilder.Append("            oFCKeditor.Config['SkinPath'] = oFCKeditor.BasePath+'editor/skins/office2003/';\r\n");
                    templateBuilder.Append("            oFCKeditor.Create();\r\n");
                    templateBuilder.Append("        </script>\r\n");
                    templateBuilder.Append("	</td>\r\n");
                    templateBuilder.Append("</tr>\r\n");
                    templateBuilder.Append("</table>\r\n");
                    templateBuilder.Append("		</p>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("		<p>验证码:\r\n");

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


                        templateBuilder.Append("</p>\r\n");

                    }	//end if

                    templateBuilder.Append("		<p class=\"btns\">\r\n");
                    templateBuilder.Append("			<a href=\"###\" id=\"closebtn\" style=\"display:none;\" onclick=\"$('message').style.background='';this.style.display='none';$('adlinkbtn').style.display='none';\">关闭广告</a>\r\n");
                    templateBuilder.Append("			<a href=\"" + quickbgadlink.ToString() + "\" id=\"adlinkbtn\" style=\"display:none;\" target=\"_blank\" onclick=\"\">进入广告</a>\r\n");
                    templateBuilder.Append("			<button type=\"submit\" id=\"postsubmit\" name=\"replysubmit\" value=\"发表帖子\" tabindex=\"3\">发表帖子</button>\r\n");
                    //templateBuilder.Append("			<input  name=\"topicsreset\" id=\"restoredata\" value=\"清空内容\" tabindex=\"6\" title=\"清空内容\" onclick=\"javascript:document.postform.reset(); return true; \" type=\"reset\" />\r\n");
                    //templateBuilder.Append("			<input name=\"restoredata\" id=\"restoredata\" value=\"恢复数据\" tabindex=\"5\" title=\"恢复上次自动保存的数据\" onclick=\"loadData();\" type=\"button\" />\r\n");
                    //templateBuilder.Append("			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"" + forum.Postbytopictype.ToString().Trim() + "\" tabindex=\"3\" />\r\n");
                    templateBuilder.Append("		</p>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    //templateBuilder.Append("	<div class=\"smilies\">\r\n");
                    //templateBuilder.Append("		 <script type=\"text/javascript\">\r\n");
                    //templateBuilder.Append("					var textobj = $('message');\r\n");
                    //templateBuilder.Append("					var lang = new Array();\r\n");
                    //templateBuilder.Append("					if(is_ie >= 5 || is_moz >= 2) {\r\n");
                    //templateBuilder.Append("						window.onbeforeunload = function () {\r\n");
                    //templateBuilder.Append("							saveData(textobj.value);\r\n");
                    //templateBuilder.Append("						};\r\n");
                    //templateBuilder.Append("						lang['post_autosave_none'] = \"没有可以恢复的数据\";\r\n");
                    //templateBuilder.Append("						lang['post_autosave_confirm'] = \"本操作将覆盖当前帖子内容，确定要恢复数据吗？\";\r\n");
                    //templateBuilder.Append("					}\r\n");
                    //templateBuilder.Append("					else {\r\n");
                    //templateBuilder.Append("						$('restoredata').style.display = 'none';\r\n");
                    //templateBuilder.Append("					}\r\n");
                    //templateBuilder.Append("					var bbinsert = parseInt('1');\r\n");
                    //templateBuilder.Append("					var smileyinsert = parseInt('1');\r\n");
                    //templateBuilder.Append("					var editor_id = 'posteditor';\r\n");
                    //templateBuilder.Append("					var smiliesCount = 9;\r\n");
                    //templateBuilder.Append("					var colCount = 3;\r\n");
                    //templateBuilder.Append("					var showsmiliestitle = 0;\r\n");
                    //templateBuilder.Append("					var smiliesIsCreate = 0;\r\n");
                    //templateBuilder.Append("					var scrMaxLeft; //表情滚动条宽度\r\n");
                    //templateBuilder.Append("			var smilies_HASH = {};\r\n");
                    //templateBuilder.Append("		</" + "script>\r\n");
                    //string defaulttypname = string.Empty;

                    //templateBuilder.Append("		<div class=\"navcontrol\">\r\n");
                    //templateBuilder.Append("			<div class=\"smiliepanel\">\r\n");
                    //templateBuilder.Append("				<div id=\"scrollbar\" class=\"scrollbar\">\r\n");
                    //templateBuilder.Append("					<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
                    //templateBuilder.Append("						<tr>\r\n");

                    //int stype__loop__id = 0;
                    //foreach (DataRow stype in smilietypes.Rows)
                    //{
                    //    stype__loop__id++;


                    //    if (stype__loop__id == 1)
                    //    {

                    //        defaulttypname = stype["code"].ToString().Trim();


                    //    }	//end if

                    //    templateBuilder.Append("							<td id=\"t_s_" + stype__loop__id.ToString() + "\"><div id=\"s_" + stype__loop__id.ToString() + "\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\" \r\n");

                    //    if (stype__loop__id != 1)
                    //    {

                    //        templateBuilder.Append("style=\"display:none;\"\r\n");

                    //    }
                    //    else
                    //    {

                    //        templateBuilder.Append("class=\"lian\"\r\n");

                    //    }	//end if

                    //    templateBuilder.Append(">" + stype["code"].ToString().Trim() + "</div></td>\r\n");

                    //}	//end loop

                    //templateBuilder.Append("						</tr>\r\n");
                    //templateBuilder.Append("					</table>\r\n");
                    //templateBuilder.Append("				</div>\r\n");
                    //templateBuilder.Append("				<div id=\"scrlcontrol\">\r\n");
                    //templateBuilder.Append("					<img src=\"editor/images/smilie_prev_default.gif\" alt=\"向前\" onmouseover=\"if($('scrollbar').scrollLeft>0)this.src=this.src.replace(/_default|_selected/, '_hover');\" onmouseout=\"this.src=this.src.replace(/_hover|_selected/, '_default');\" onmousedown=\"if($('scrollbar').scrollLeft>0){this.src=this.src.replace(/_hover|_default/, '_selected');this.boder=1;}\" onmouseup=\"if($('scrollbar').scrollLeft>0)this.src=this.src.replace(/_selected/, '_hover');else{this.src=this.src.replace(/_selected|_hover/, '_default');}this.border=0;\" onclick=\"scrollSmilieTypeBar($('scrollbar'), 1-$('t_s_1').clientWidth);\"/>&nbsp;\r\n");
                    //templateBuilder.Append("					<img src=\"editor/images/smilie_next_default.gif\" alt=\"向后\"  onmouseover=\"if($('scrollbar').scrollLeft<scrMaxLeft)this.src=this.src.replace(/_default|_selected/, '_hover');\" onmouseout=\"this.src=this.src.replace(/_hover|_selected/, '_default');\" onmousedown=\"if($('scrollbar').scrollLeft<scrMaxLeft){this.src=this.src.replace(/_hover|_default/, '_selected');this.boder=1;}\" onmouseup=\"if($('scrollbar').scrollLeft<scrMaxLeft)this.src=this.src.replace(/_selected/, '_hover');else{this.src=this.src.replace(/_selected|_hover/, '_default');}this.border=0;\" onclick=\"scrollSmilieTypeBar($('scrollbar'), $('t_s_1').clientWidth);\" />\r\n");
                    //templateBuilder.Append("				</div>	\r\n");
                    //templateBuilder.Append("			</div>\r\n");
                    //templateBuilder.Append("			<div id=\"showsmilie\"><img src=\"images/common/loading_wide.gif\" width=\"90%\" alt=\"表情加载\"/><p>正在加载表情...</p></div>\r\n");
                    //templateBuilder.Append("			<div id=\"showsmilie_pagenum\">&nbsp;</div>\r\n");
                    //templateBuilder.Append("		</div>\r\n");
                    //templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    //templateBuilder.Append("			var firstpagesmilies_json ={ " + firstpagesmilies.ToString() + " };\r\n");
                    //templateBuilder.Append("			showFirstPageSmilies(firstpagesmilies_json, '" + defaulttypname.ToString() + "', 12);\r\n");
                    //templateBuilder.Append("			function getSmilies(func){\r\n");
                    //templateBuilder.Append("				var c=\"tools/ajax.aspx?t=smilies\";\r\n");
                    //templateBuilder.Append("				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true)\r\n");
                    //templateBuilder.Append("			}\r\n");
                    //templateBuilder.Append("			getSmilies(function(obj){ \r\n");
                    //templateBuilder.Append("			smilies_HASH = obj; \r\n");
                    //templateBuilder.Append("			showsmiles(1, '" + defaulttypname.ToString() + "');\r\n");
                    //templateBuilder.Append("			});\r\n");
                    //templateBuilder.Append("			window.onload = function() {\r\n");
                    //templateBuilder.Append("				$('scrollbar').scrollLeft = 10000;\r\n");
                    //templateBuilder.Append("				scrMaxLeft = $('scrollbar').scrollLeft;\r\n");
                    //templateBuilder.Append("				$('scrollbar').scrollLeft = 1;	\r\n");
                    //templateBuilder.Append("				if ($('scrollbar').scrollLeft > 0)\r\n");
                    //templateBuilder.Append("				{\r\n");
                    //templateBuilder.Append("					$('scrlcontrol').style.display = '';\r\n");
                    //templateBuilder.Append("					$('scrollbar').scrollLeft = 0;	\r\n");
                    //templateBuilder.Append("				}\r\n");
                    //templateBuilder.Append("			}\r\n");
                    //templateBuilder.Append("		</" + "script>\r\n");
                    //templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</form>\r\n");


                    templateBuilder.Append("<!--quickreply-->\r\n");

                }	//end if

                templateBuilder.Append("<!--replay-->\r\n");

                if (userid < 0 || canposttopic)
                {

                    templateBuilder.Append("	<ul class=\"popupmenu_popup newspecialmenu\" id=\"newspecial_menu\" style=\"display: none\">\r\n");

                    if (forum.Allowspecialonly <= 0)
                    {

                        templateBuilder.Append("		<li><a href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "\">发新主题</a></li>\r\n");

                    }	//end if

                    int specialpost = forum.Allowpostspecial & 1;


                    if (specialpost == 1 && usergroupinfo.Allowpostpoll == 1)
                    {

                        templateBuilder.Append("		<li class=\"poll\"><a href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "&type=poll\">发布投票</a></li>\r\n");

                    }	//end if

                    specialpost = forum.Allowpostspecial & 4;


                    if (specialpost == 4 && usergroupinfo.Allowbonus == 1)
                    {

                        templateBuilder.Append("			<li class=\"reward\"><a href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "&type=bonus\">发布悬赏</a></li>\r\n");

                    }	//end if

                    specialpost = forum.Allowpostspecial & 16;


                    if (specialpost == 16 && usergroupinfo.Allowdebate == 1)
                    {

                        templateBuilder.Append("			<li class=\"debate\"><a href=\"posttopic.aspx?forumid=" + forum.Fid.ToString().Trim() + "&type=debate\">发起辩论</a></li>\r\n");

                    }	//end if

                    templateBuilder.Append("	</ul>\r\n");

                }	//end if

                templateBuilder.Append("<div class=\"mainbox\">\r\n");
                templateBuilder.Append("	<h3>标题: " + topictitle.ToString() + "</h3>\r\n");
                templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");

                int posttreeitem__loop__id = 0;
                foreach (DataRow posttreeitem in posttree.Rows)
                {
                    posttreeitem__loop__id++;

                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<td>\r\n");
                    templateBuilder.Append("		" + posttreeitem["spaces"].ToString().Trim() + "\r\n");
                    templateBuilder.Append("		·<a href=\"showtree.aspx?topicid=" + topicid.ToString() + "&postid=" + posttreeitem["pid"].ToString().Trim() + "\">\r\n");

                    if (Utils.StrToInt(posttreeitem["pid"].ToString().Trim(), 0) == postid)
                    {

                        templateBuilder.Append("			" + posttreeitem["title"].ToString().Trim() + "\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("			" + posttreeitem["title"].ToString().Trim() + "\r\n");

                    }	//end if

                    templateBuilder.Append("		</a>\r\n");
                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		<td>\r\n");

                    if (Utils.StrToInt(posttreeitem["posterid"].ToString().Trim(), 0) == -1)
                    {

                        templateBuilder.Append("			游客\r\n");

                    }
                    else
                    {

                        aspxrewriteurl = this.UserInfoAspxRewrite(posttreeitem["posterid"].ToString().Trim());

                        templateBuilder.Append("<a href=\"" + aspxrewriteurl.ToString() + "\">" + posttreeitem["poster"].ToString().Trim() + "</a>\r\n");

                    }	//end if

                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		<td>发表于 " + posttreeitem["postdatetime"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");

                }	//end loop

                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<!--reply-->\r\n");

                if (useradminid > 0 || usergroupinfo.Raterange != "" || config.Forumjump == 1)
                {

                    templateBuilder.Append("<!--forumjumping start-->\r\n");
                    templateBuilder.Append("<div id=\"footfilter\" class=\"box\">\r\n");

                    if (useradminid > 0)
                    {

                        templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("			function action_onchange(value,objfrm,postid){\r\n");
                        templateBuilder.Append("				if (value != ''){\r\n");
                        templateBuilder.Append("					objfrm.operat.value = value;\r\n");
                        templateBuilder.Append("					objfrm.postid.value = postid;\r\n");
                        templateBuilder.Append("					if(value != 'delpost'){\r\n");
                        templateBuilder.Append("						objfrm.submit();\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("					else{\r\n");
                        templateBuilder.Append("						$('delpost').submit();\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("			}\r\n");
                        templateBuilder.Append("		</" + "script>\r\n");
                        templateBuilder.Append("		<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=" + forumid.ToString() + "\">\r\n");
                        templateBuilder.Append("			<input name=\"forumid\" type=\"hidden\" value=\"" + forumid.ToString() + "\" />\r\n");
                        templateBuilder.Append("			<input name=\"topicid\" type=\"hidden\" value=\"" + topicid.ToString() + "\" />\r\n");
                        templateBuilder.Append("			<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n");
                        templateBuilder.Append("			<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n");
                        templateBuilder.Append("			<select id=\"operatSel\" onchange=\"action_onchange(this.options[this.selectedIndex].value,this.form,0);\" name=\"operatSel\">\r\n");
                        templateBuilder.Append("				<option value=\"\" selected=\"selected\">管理选项</option>\r\n");
                        templateBuilder.Append("				<option value=\"delete\">删除主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"banpost\">屏蔽单贴</option>\r\n");
                        templateBuilder.Append("				<option value=\"delpost\">批量删帖</option>\r\n");
                        templateBuilder.Append("				<option value=\"close\">关闭主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"move\">移动主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"copy\">复制主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"highlight\">高亮显示</option>\r\n");
                        templateBuilder.Append("				<option value=\"digest\">设置精华</option>\r\n");
                        templateBuilder.Append("				<option value=\"identify\">鉴定主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"displayorder\">主题置顶</option>\r\n");
                        templateBuilder.Append("				<option value=\"split\">分割主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"merge\">合并主题</option>\r\n");
                        templateBuilder.Append("				<option value=\"repair\">修复主题</option>\r\n");
                        templateBuilder.Append("			</select>		\r\n");
                        templateBuilder.Append("		</form>\r\n");

                    }
                    else if (usergroupinfo.Raterange != "")
                    {

                        templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("			function action_onchange(value,objfrm,postid){\r\n");
                        templateBuilder.Append("				if (value != ''){\r\n");
                        templateBuilder.Append("					objfrm.operat.value = value;\r\n");
                        templateBuilder.Append("					objfrm.postid.value = postid;\r\n");
                        templateBuilder.Append("					if(value != 'delpost'){\r\n");
                        templateBuilder.Append("						objfrm.submit();\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("					else{\r\n");
                        templateBuilder.Append("						$('delpost').submit();\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("			}\r\n");
                        templateBuilder.Append("		</" + "script>\r\n");
                        templateBuilder.Append("		<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&forumid=" + forumid.ToString() + "\">\r\n");
                        templateBuilder.Append("			<input name=\"forumid\" type=\"hidden\" value=\"" + forumid.ToString() + "\" />\r\n");
                        templateBuilder.Append("			<input name=\"topicid\" type=\"hidden\" value=\"" + topicid.ToString() + "\" />\r\n");
                        templateBuilder.Append("			<input name=\"postid\" type=\"hidden\" value=\"\" />\r\n");
                        templateBuilder.Append("			<input name=\"operat\" type=\"hidden\" value=\"\" />\r\n");
                        templateBuilder.Append("		</form>\r\n");

                    }	//end if

                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<!--forumjumping end-->\r\n");

                }	//end if

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


            }	//end if

            templateBuilder.Append("" + inpostad.ToString() + "\r\n");


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
