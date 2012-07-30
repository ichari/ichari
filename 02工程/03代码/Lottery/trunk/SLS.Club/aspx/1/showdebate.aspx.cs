using System;
using System.Data;
using System.Text;
using System.Web;
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
using System.Collections;

namespace Discuz.Web
{
    /// <summary>
    /// 查看辩论主题页面
    /// </summary>
    public partial class showdebate : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 主题信息
        /// </summary>
        public TopicInfo topic;

#if NET1
		public ShowtopicPagePostInfoCollection postlist;
        public ShowtopicPageAttachmentInfoCollection attachmentlist;
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 正方帖子列表
        /// </summary>
        public List<ShowtopicPagePostInfo> positivepostlist;
        /// <summary>
        /// 反方帖子列表
        /// </summary>
        public List<ShowtopicPagePostInfo> negativepostlist;
        /// <summary>
        /// 附件列表
        /// </summary>
        public List<ShowtopicPageAttachmentInfo> attachmentlist;

        /// <summary>
        /// 悬赏给分日志
        /// </summary>
        public List<BonusLogInfo> bonuslogs;
#endif

        /// <summary>
        /// 投票选项列表
        /// </summary>
        public DataTable polllist;

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
        /// 所属版块Id
        /// </summary>
        public int forumid;

        /// <summary>
        /// 所属版块名称
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
        /// 是否是投票帖
        /// </summary>
        public bool ispoll;

        /// <summary>
        /// 是否允许投票
        /// </summary>
        public bool allowvote;

        /// <summary>
        /// 是否允许评分
        /// </summary>
        public bool allowrate;

        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle;

        /// <summary>
        /// 回复标题
        /// </summary>
        public string replytitle;

        /// <summary>
        /// 主题魔法表情
        /// </summary>
        public string topicmagic = "";

        /// <summary>
        /// 主题浏览量
        /// </summary>
        public int topicviews;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;

        /// <summary>
        /// 回复帖子数
        /// </summary>
        public int postcount;

        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;

        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 表情Javascript数组
        /// </summary>
        public string smilies;

        /// <summary>
        /// 参与投票的用户列表
        /// </summary>
        public string voters;

        /// <summary>
        /// 论坛跳转链接选项
        /// </summary>
        public string forumlistboxoptions;

        /// <summary>
        /// 是否是管理者
        /// </summary>
        public int ismoder = 0;

        /// <summary>
        /// 上一次进行的管理操作
        /// </summary>
        public string moderactions;

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
        /// 是否允许 [img]标签
        /// </summary>
        public int allowimg;

        /// <summary>
        /// 是否显示评分记录
        /// </summary>
        public int showratelog;

        /// <summary>
        /// 可用的扩展金币名称列表
        /// </summary>
        public string[] score;

        /// <summary>
        /// 可用的扩展金币单位列表
        /// </summary>
        public string[] scoreunit;

        //public string ignorelink;
        /// <summary>
        /// 是否受发贴灌水限制
        /// </summary>
        public int disablepostctrl;

        /// <summary>
        /// 金币策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;

        /// <summary>
        /// 主题鉴定信息
        /// </summary>
        public TopicIdentify topicidentify;

        /// <summary>
        /// 用户的管理组信息
        /// </summary>
        public AdminGroupInfo admininfo = null;

        /// <summary>
        /// 当前版块信息
        /// </summary>
        public ForumInfo forum;

        /// <summary>
        /// 是否只查看楼主贴子 1:只看楼主  0:显示全部
        /// </summary>
        public string onlyauthor = "0";

        /// <summary>
        /// 当前的主题类型
        /// </summary>
        public string topictypes = "";

        /// <summary>
        /// 表情分类列表
        /// </summary>
        public DataTable smilietypes = new DataTable();

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
        /// 当为投票帖时有用,0=单选，1=多选
        /// </summary>
        public int polltype = -1;

        /// <summary>
        /// 投票结束时间
        /// </summary>
        public string pollenddatetime = "";

        /// <summary>
        /// 论坛弹出导航菜单HTML代码
        /// </summary>
        public string navhomemenu = "";

        /// <summary>
        /// 帖间通栏广告
        /// </summary>
        public string postleaderboardad = string.Empty;

        /// <summary>
        /// 是否显示短消息列表
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
        /// 每页帖子数
        /// </summary>
        public int ppp;

        /// <summary>
        /// 是否显示需要登录后访问的错误提示
        /// </summary>
        public bool needlogin = false;

        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = string.Empty;

        /// <summary>
        /// 相关主题集合
        /// </summary>
        public List<TopicInfo> relatedtopics;
        /// <summary>
        /// 本版是否启用了Tag
        /// </summary>
        public bool enabletag = false;

        /// <summary>
        /// 通过TID得到帖子观点
        /// </summary>
        public Dictionary<int, int> debateList;

        /// <summary>
        /// 作为辩论主题的扩展属性
        /// </summary>
        public DebateInfo debateexpand;
        /// <summary>
        /// 已经结束的辩论
        /// </summary>
        public bool isenddebate = false;
        public bool isdiggs = false;

        public ShowtopicPagePostInfo debatepost;

        public float positivepercent;
        public float negativepercent;

        public string positivepagenumbers;
        public string negativepagenumbers;

        public int positivepagecount;
        public int negativepagecount;
        #endregion

        private int pagesize;

        protected override void ShowPage()
        {
            headerad = "";
            footerad = "";
            postleaderboardad = "";

            doublead = "";
            floatad = "";

            allowrate = false;
            disablepostctrl = 0;
            pagesize = config.Debatepagesize;
            // 获取主题ID
            topicid = DNTRequest.GetInt("topicid", -1);
            // 获取该主题的信息
            string go = DNTRequest.GetString("go").Trim().ToLower();
            int fid = DNTRequest.GetInt("forumid", 0);
            firstpagesmilies = Caches.GetSmiliesFirstPageCache();

            if (go == "")
            {
                fid = 0;
            }
            else if (fid == 0)
            {
                go = "";
            }

            string errmsg = "";
            switch (go)
            {
                case "prev":
                    topic = Topics.GetTopicInfo(topicid, fid, 1);
                    errmsg = "没有更旧的主题, 请返回";
                    break;
                case "next":
                    topic = Topics.GetTopicInfo(topicid, fid, 2);
                    errmsg = "没有更新的主题, 请返回";
                    break;
                default:
                    topic = Topics.GetTopicInfo(topicid);
                    errmsg = "该主题不存在";
                    break;
            }

            if (topic == null)
            {
                AddErrLine(errmsg);

                headerad = Advertisements.GetOneHeaderAd("", 0);
                footerad = Advertisements.GetOneFooterAd("", 0);
                pagewordad = Advertisements.GetPageWordAd("", 0);
                doublead = Advertisements.GetDoubleAd("", 0);
                floatad = Advertisements.GetFloatAd("", 0);
                return;
            }

            //当为投票帖时
            if (topic.Special == 1)
            {
                polltype = Polls.GetPollType(topic.Tid);
                pollenddatetime = Polls.GetPollEnddatetime(topic.Tid);
            }

            if (topic.Identify > 0)
            {
                topicidentify = Caches.GetTopicIdentify(topic.Identify);
            }
            topicid = topic.Tid;
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);

            forumname = forum.Name;
            pagetitle = topic.Title;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
            navhomemenu = Caches.GetForumListMenuDivCache(usergroupid, userid, config.Extname);

            fid = forumid;

            ///得到广告列表
            ///头部
            headerad = Advertisements.GetOneHeaderAd("", fid);
            footerad = Advertisements.GetOneFooterAd("", fid);
            postleaderboardad = Advertisements.GetOnePostLeaderboardAD("", fid);

            pagewordad = Advertisements.GetPageWordAd("", fid);
            doublead = Advertisements.GetDoubleAd("", fid);
            floatad = Advertisements.GetFloatAd("", fid);

            // 检查是否具有版主的身份
            if (useradminid != 0)
            {
                ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
                //得到管理组信息
                admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
                if (admininfo != null)
                {
                    disablepostctrl = admininfo.Disablepostctrl;
                }
            }
            //验证不通过则返回
            if (!IsConditionsValid())
                return;

            showratelog = GeneralConfigs.GetConfig().DisplayRateCount > 0 ? 1 : 0;


            topictitle = topic.Title.Trim();
            replytitle = topictitle;
            if (replytitle.Length >= 50)
            {
                replytitle = Utils.CutString(replytitle, 0, 50) + "...";
            }

            //topicmagic = ForumUtils.ShowTopicMagic(topic.Magic);
            topicviews = topic.Views + 1 +
                         (config.TopicQueueStats == 1 ? TopicStats.GetStoredTopicViewCount(topic.Tid) : 0);
            smilies = Caches.GetSmiliesCache();
            smilietypes = Caches.GetSmilieTypesCache();


            //编辑器状态
            StringBuilder sb = new StringBuilder();
            sb.Append("var Allowhtml=1;\r\n"); //+ allhtml.ToString() + "

            parseurloff = 0;

            smileyoff = 1 - forum.Allowsmilies;
            sb.Append("var Allowsmilies=" + (1 - smileyoff).ToString() + ";\r\n");


            bbcodeoff = 1;
            if (forum.Allowbbcode == 1)
            {
                if (usergroupinfo.Allowcusbbcode == 1)
                {
                    bbcodeoff = 0;
                }
            }
            sb.Append("var Allowbbcode=" + (1 - bbcodeoff).ToString() + ";\r\n");

            usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;

            allowimg = forum.Allowimgcode;
            sb.Append("var Allowimgcode=" + allowimg.ToString() + ";\r\n");

            AddScript(sb.ToString());
            int price = 0;
            if (topic.Special == 0 || topic.Special == 2)//普通主题或正在进行的悬赏
            {
                HttpContext.Current.Response.Redirect(forumpath + this.ShowTopicAspxRewrite(topic.Tid, 1));
                return;
            }

            if (topic.Special == 3)//已给分的悬赏帖
            {
                HttpContext.Current.Response.Redirect(forumpath + this.ShowBonusAspxRewrite(topic.Tid, 1));
                return;
            }

            if (topic.Moderated > 0)
            {
                moderactions = TopicAdmins.GetTopicListModeratorLog(topicid);
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


            // 获取帖子总数
            //postcount = Posts.GetPostCount(topicid);
            onlyauthor = DNTRequest.GetString("onlyauthor");
            if (onlyauthor == "" || onlyauthor == "0")
            {
                postcount = topic.Replies + 1;
            }
            else
            {
                postcount = DatabaseProvider.GetInstance().GetPostCount(Posts.GetPostTableID(topicid), topicid, topic.Posterid);
            }

            // 得到Ppp设置
            ppp = Utils.StrToInt(ForumUtils.GetCookie("ppp"), config.Ppp);


            if (ppp <= 0)
            {
                ppp = config.Ppp;
            }

            //获取总页数
            pagecount = postcount % ppp == 0 ? postcount / ppp : postcount / ppp + 1;
            if (pagecount == 0)
            {
                pagecount = 1;
            }
            // 得到当前用户请求的页数
            if (DNTRequest.GetString("page").ToLower().Equals("end"))
            {
                pageid = pagecount;
            }
            else
            {
                pageid = DNTRequest.GetInt("page", 1);
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
            //判断是否为回复可见帖, hide=0为不解析[hide]标签, hide>0解析为回复可见字样, hide=-1解析为以下内容回复可见字样显示真实内容
            //将逻辑判断放入取列表的循环中处理,此处只做是否为回复人的判断，主题作者也该可见
            int hide = 1;
            if (topic.Hide == 1 && (Posts.IsReplier(topicid, userid) || ismoder == 1))
            {
                hide = -1;
            }


            //获取当前页主题列表

            DataSet ds = new DataSet();
            PostpramsInfo postpramsInfo = new PostpramsInfo();
            postpramsInfo.Fid = forum.Fid;
            postpramsInfo.Tid = topicid;
            postpramsInfo.Jammer = forum.Jammer;
            postpramsInfo.Pagesize = pagesize;
            postpramsInfo.Pageindex = pageid;
            postpramsInfo.Getattachperm = forum.Getattachperm;
            postpramsInfo.Usergroupid = usergroupid;
            postpramsInfo.Attachimgpost = config.Attachimgpost;
            postpramsInfo.Showattachmentpath = config.Showattachmentpath;
            postpramsInfo.Hide = hide;
            postpramsInfo.Price = price;
            postpramsInfo.Usergroupreadaccess = usergroupinfo.Readaccess;
            if (ismoder == 1)
                postpramsInfo.Usergroupreadaccess = int.MaxValue;
            postpramsInfo.CurrentUserid = userid;
            postpramsInfo.Showimages = forum.Allowimgcode;
            postpramsInfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
            postpramsInfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
            postpramsInfo.Smiliesmax = config.Smiliesmax;
            postpramsInfo.Bbcodemode = config.Bbcodemode;
            postpramsInfo.CurrentUserGroup = usergroupinfo;
            if (!(onlyauthor.Equals("") || onlyauthor.Equals("0")))
            {
                postpramsInfo.Condition =
                    string.Format(" {0}.posterid={1}", Posts.GetPostTableName(topicid), topic.Posterid);
            }

            postpramsInfo.Pid = Posts.GetFirstPostId(topic.Tid);
            debatepost = Posts.GetSinglePost(postpramsInfo, out attachmentlist, ismoder == 1);
            postpramsInfo.Pid = 0;


            positivepostlist = Debates.GetPositivePostList(postpramsInfo, out attachmentlist, ismoder == 1);
            negativepostlist = Debates.GetNegativePostList(postpramsInfo, out attachmentlist, ismoder == 1);

            if (topic.Special == 4)
            {

                debateexpand = Debates.GetDebateTopic(topicid);
                debateList = Debates.GetPostDebateList(topicid);//通过TID得到帖子观点
                if (debateexpand.Terminaltime < DateTime.Now)
                {
                    isenddebate = true;
                }

                //positivepagecount = (debateexpand.Positivediggs % pagesize == 0) ? (debateexpand.Positivediggs / pagesize) : (debateexpand.Positivediggs / pagesize + 1);
                //negativepagecount = (debateexpand.Negativediggs % pagesize == 0) ? (debateexpand.Negativediggs / pagesize) : (debateexpand.Negativediggs / pagesize + 1);
                int positivepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 1);
                int negativepostlistcount = Debates.GetDebatesPostCount(postpramsInfo, 2);

                positivepagecount = (positivepostlistcount % pagesize == 0) ? (positivepostlistcount / pagesize) : (positivepostlistcount / pagesize + 1);
                negativepagecount = (negativepostlistcount % pagesize == 0) ? (negativepostlistcount / pagesize) : (negativepostlistcount / pagesize + 1);

                positivepagenumbers = Utils.GetAjaxPageNumbers(1, positivepagecount, "showdebatepage('" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=1&tid=" + topic.Tid + "&{0}'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",'" + isenddebate + "',1," + userid + "," + topicid + ")", 8);
                negativepagenumbers = Utils.GetAjaxPageNumbers(1, negativepagecount, "showdebatepage('" + forumpath + "tools/ajax.aspx?t=getdebatepostpage&opinion=2&tid=" + topic.Tid + "&{0}'," + parseurloff + ", " + smileyoff + ", " + bbcodeoff + ",'" + isenddebate + "',2," + userid + "," + topicid + ")", 8);

                //防止无人参与时0做除数
                if (debateexpand.Negativediggs + debateexpand.Positivediggs == 0)
                {
                    positivepercent = 50;
                    negativepercent = 50;
                }
                else
                {
                    positivepercent = (float)debateexpand.Positivediggs / (float)(debateexpand.Negativediggs + debateexpand.Positivediggs) * 100;
                    negativepercent = 100 - positivepercent;
                }



                foreach (ShowtopicPagePostInfo postlistinfo in positivepostlist)
                {
                    //设置POST的观点属性
                    if (debateList != null && debateList.ContainsKey(postlistinfo.Pid))
                        postlistinfo.Debateopinion = Convert.ToInt32(debateList[postlistinfo.Pid]);
                }
            }
            //加载帖内广告
            inpostad = Advertisements.GetInPostAd("", fid, templatepath, positivepostlist.Count > ppp ? ppp : positivepostlist.Count);
            //快速发帖广告
            quickeditorad = Advertisements.GetQuickEditorAD("", fid);

            //更新页面Meta中的Description项, 提高SEO友好性
            string metadescritpion = Utils.RemoveHtml(debatepost.Message);
            metadescritpion = metadescritpion.Length > 100 ? metadescritpion.Substring(0, 100) : metadescritpion;
            UpdateMetaInfo(config.Seokeywords, metadescritpion, config.Seohead);

            //获取相关主题集合
            enabletag = (config.Enabletag & forum.Allowtag) == 1;
            if (enabletag)
            {
                relatedtopics = Topics.GetRelatedTopics(topicid, 5);
            }

            //更新查看次数
            //Topics.UpdateTopicViews(topicid);
            TopicStats.Track(topicid, 1);

            OnlineUsers.UpdateAction(olid, UserAction.ShowTopic.ActionID, forumid, forumname, topicid, topictitle,
                                     config.Onlinetimeout);
            forumlistboxoptions = Caches.GetForumListBoxOptionsCache();

            score = Scoresets.GetValidScoreName();
            scoreunit = Scoresets.GetValidScoreUnit();

            string oldtopic = ForumUtils.GetCookie("oldtopic") + "D";
            if (oldtopic.IndexOf("D" + topic.Tid.ToString() + "D") == -1 &&
                DateTime.Now.AddMinutes(-1 * 600) < DateTime.Parse(topic.Lastpost))
            {
                oldtopic = "D" + topic.Tid.ToString() + Utils.CutString(oldtopic, 0, oldtopic.Length - 1);
                if (oldtopic.Length > 3000)
                {
                    oldtopic = Utils.CutString(oldtopic, 0, 3000);
                    oldtopic = Utils.CutString(oldtopic, 0, oldtopic.LastIndexOf("D"));
                }
                ForumUtils.WriteCookie("oldtopic", oldtopic);
            }
        }

        private bool IsConditionsValid()
        {
            // 如果包含True, 则必然允许某项扩展金币的评分
            if (usergroupinfo.Raterange.IndexOf("True") > -1)
            {
                allowrate = true;
            }

            // 如果主题ID非数字
            if (topicid == -1)
            {
                AddErrLine("无效的主题ID");

                return false;
            }

            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return false;
            }

            if (topic.Closed > 1)
            {
                topicid = topic.Closed;
                topic = Topics.GetTopicInfo(topicid);

                // 如果该主题不存在
                if (topic == null || topic.Closed > 1)
                {
                    AddErrLine("不存在的主题ID");
                    return false;
                }
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 &&
                ismoder != 1)
            {
                AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.Readperm.ToString(), usergroupinfo.Grouptitle));
                if (userid == -1)
                {
                    needlogin = true;
                }
                return false;
            }

            if (topic.Displayorder == -1)
            {
                AddErrLine("此主题已被删除！");
                return false;
            }

            if (topic.Displayorder == -2)
            {
                AddErrLine("此主题未经审核！");
                return false;
            }

            if (forum.Password != "" &&
                Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                //SetBackLink("showforum-" + forumid.ToString() + config.Extname);
                HttpContext.Current.Response.Redirect("/" + "showforum-" + forumid.ToString() + config.Extname, true);
                return false;
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
                        return false;
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
                        return false;
                    }
                }
            }

            //是否显示回复链接
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

            if ((topic.Closed == 0 && canreply) || ismoder == 1)
            {
                canreply = true;
            }
            else
            {
                canreply = false;
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

            //判断是否有发主题的权限
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

            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                {
                    canposttopic = false;
                }
            }

            //是否有回复的权限
            if (topic.Closed == 0 && userid > -1)
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
            return true;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:17.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:17. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("var templatepath = \"" + templatepath.ToString() + "\";\r\n");
            templateBuilder.Append("var postminchars = parseInt(" + config.Minpostsize.ToString().Trim() + ");\r\n");
            templateBuilder.Append("var postmaxchars = parseInt(" + config.Maxpostsize.ToString().Trim() + ");\r\n");
            templateBuilder.Append("var disablepostctrl = parseInt(" + disablepostctrl.ToString() + ");\r\n");
            templateBuilder.Append("var forumpath = \"" + forumpath.ToString() + "\";\r\n");
            templateBuilder.Append("var ismoder = " + ismoder.ToString() + ";\r\n");
            templateBuilder.Append("var userid = parseInt('" + userid.ToString() + "');\r\n");
            templateBuilder.Append("var forumallowhtml =true;\r\n");
            templateBuilder.Append("</" + "script>\r\n");

            if (enabletag)
            {

                templateBuilder.Append("<script type=\"text/javascript\" src=\"cache/tag/closedtags.txt\"></" + "script>\r\n");
                templateBuilder.Append("<script type=\"text/javascript\" src=\"cache/tag/colorfultags.txt\"></" + "script>\r\n");

            }	//end if

            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_showtopic.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/bbcode.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/post.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_debate.js\"></" + "script>\r\n");

            if (page_err == 0)
            {

                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("		<a id=\"forumlist\" href=\"" + config.Forumurl.ToString().Trim() + "\" \r\n");

                if (config.Forumjump == 1)
                {

                    templateBuilder.Append("		onmouseover=\"showMenu(this.id);\" onmouseout=\"showMenu(this.id);\"\r\n");

                }	//end if

                templateBuilder.Append("		>" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + "\r\n");
                int ishtmltitle = Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle);


                if (ishtmltitle == 1)
                {

                    templateBuilder.Append("		  &raquo; <strong>" + Topics.GetHtmlTitle(topic.Tid).ToString().Trim() + "</strong>\r\n");

                }
                else
                {

                    templateBuilder.Append("		  &raquo; <strong>" + topictitle.ToString() + "</strong>\r\n");

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

                if (config.Forumjump == 1)
                {

                    templateBuilder.Append("	" + navhomemenu.ToString() + "\r\n");

                }	//end if

                templateBuilder.Append("<div class=\"mainbox viewthread specialthread specialthread_5\">\r\n");
                templateBuilder.Append("	<h3>\r\n");

                if (forum.Applytopictype == 1 && forum.Topictypeprefix == 1)
                {

                    templateBuilder.Append("		" + topictypes.ToString() + " \r\n");

                }	//end if

                templateBuilder.Append("辩论主题\r\n");
                templateBuilder.Append("	</h3>\r\n");
                templateBuilder.Append("	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"辩论主题\">\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("		<td class=\"postcontent\">\r\n");
                templateBuilder.Append("			<h1>" + debatepost.Title.ToString().Trim() + " </h1>\r\n");
                templateBuilder.Append("			<div class=\"postmessage\">\r\n");
                templateBuilder.Append("				<div id=\"firstpost\">\r\n");
                templateBuilder.Append("					" + debatepost.Message.ToString().Trim() + "\r\n");
                templateBuilder.Append("				</div>\r\n");

                if (enabletag)
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
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("		<td class=\"postauthor\">\r\n");

                if (debatepost.Posterid != -1)
                {

                    templateBuilder.Append("				<cite>\r\n");
                    templateBuilder.Append("					<a href=\"#\" target=\"_blank\" id=\"memberinfo_topic\" class=\"dropmenu\"  onmouseover=\"showMenu(this.id,false)\">\r\n");

                    if (debatepost.Onlinestate == 1)
                    {

                        templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/useronline.gif\" alt=\"在线\" title=\"在线\"/>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("					<img src=\"templates/" + templatepath.ToString() + "/images/useroutline.gif\"  alt=\"离线\" title=\"离线\"/>\r\n");

                    }	//end if

                    templateBuilder.Append("					<em>发起人:</em>" + debatepost.Poster.ToString().Trim() + "\r\n");
                    templateBuilder.Append("					</a>\r\n");
                    templateBuilder.Append("				</cite>\r\n");

                    if (config.Showavatars == 1)
                    {

                        templateBuilder.Append("				<div class=\"avatar\">\r\n");

                        if (debatepost.Avatar != "")
                        {

                            templateBuilder.Append("					<img src=\"" + debatepost.Avatar.ToString().Trim() + "\" onerror=\"this.onerror=null;this.src='templates/" + templatepath.ToString() + "/images/noavatar.gif';\" \r\n");

                            if (debatepost.Avatarwidth > 0)
                            {

                                templateBuilder.Append(" width=\"" + debatepost.Avatarwidth.ToString().Trim() + "\" height=\"" + debatepost.Avatarheight.ToString().Trim() + "\" \r\n");

                            }	//end if

                            templateBuilder.Append(" alt=\"头像\"/>			\r\n");

                        }	//end if

                        templateBuilder.Append("				</div>\r\n");

                    }	//end if


                }
                else
                {

                    templateBuilder.Append("				<div class=\"ipshow\"><strong>" + debatepost.Poster.ToString().Trim() + "</strong>  " + debatepost.Ip.ToString().Trim() + "\r\n");

                    if (useradminid > 0 && admininfo.Allowviewip == 1)
                    {

                        templateBuilder.Append("						<a href=\"getip.aspx?pid=" + debatepost.Pid.ToString().Trim() + "&topicid=" + topicid.ToString() + "\" title=\"查看IP\"><img src=\"templates/" + templatepath.ToString() + "/images/ip.gif\" alt=\"查看IP\"/></a>\r\n");

                    }	//end if

                    templateBuilder.Append("				</div>\r\n");
                    templateBuilder.Append("				<!--guest-->\r\n");
                    templateBuilder.Append("				<div class=\"noregediter\">\r\n");
                    templateBuilder.Append("					未注册\r\n");
                    templateBuilder.Append("				</div>\r\n");

                }	//end if

                templateBuilder.Append("			<p>开始时间&nbsp; \r\n");
                templateBuilder.Append(Convert.ToDateTime(debatepost.Postdatetime).ToString(" yyyy-MM-dd HH:mm"));
                templateBuilder.Append("</p>\r\n");
                templateBuilder.Append("			<p>结束时间&nbsp;\r\n");
                templateBuilder.Append(Convert.ToDateTime(debateexpand.Terminaltime).ToString(" yyyy-MM-dd HH:mm"));
                templateBuilder.Append("</p>\r\n");
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("		<td class=\"postcontent\">\r\n");
                templateBuilder.Append("		<div class=\"postactions\">\r\n");

                if (userid != -1)
                {


                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("    show_report_button();\r\n");
                    templateBuilder.Append("</" + "script>\r\n");


                    templateBuilder.Append("|\r\n");

                }	//end if

                templateBuilder.Append("			<a href=\"favorites.aspx?topicid=" + topicid.ToString() + "\">收藏</a>|\r\n");

                if (ismoder == 1)
                {

                    templateBuilder.Append("				<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + debatepost.Pid.ToString().Trim() + "\">编辑</a>|\r\n");
                    templateBuilder.Append("				<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + debatepost.Pid.ToString().Trim() + "\" onclick=\"return confirm('确定要删除吗?');\">删除</a>|\r\n");

                    if (debatepost.Posterid != -1)
                    {

                        templateBuilder.Append("					<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'" + debatepost.Pid.ToString().Trim() + "');\">评分</a>\r\n");

                        if (debatepost.Ratetimes > 0)
                        {

                            templateBuilder.Append("					<a href=\"###\" onclick=\"action_onchange('cancelrate',$('moderate'),'" + debatepost.Pid.ToString().Trim() + "');\">撤销评分</a>|\r\n");

                        }	//end if


                    }	//end if


                    if (debatepost.Layer == 0 && topic.Special == 4)
                    {


                        if (isenddebate == true && userid == debatepost.Posterid)
                        {

                            templateBuilder.Append("|<a href=\"###\" onClick=\"showMenu(this.id)\" id=\"commentdebates\" name=\"commentdebates\">点评</a>\r\n");

                        }	//end if


                    }	//end if


                }
                else
                {


                    if (debatepost.Posterid != -1 && userid == debatepost.Posterid)
                    {


                        if (topic.Closed == 0)
                        {

                            templateBuilder.Append("						<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + debatepost.Pid.ToString().Trim() + "\">编辑</a>|\r\n");

                        }	//end if

                        templateBuilder.Append("					<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + debatepost.Pid.ToString().Trim() + "\" onclick=\"return confirm('确定要删除吗?');\">删除</a>|\r\n");

                    }	//end if


                    if (usergroupinfo.Raterange != "" && debatepost.Posterid != -1)
                    {

                        templateBuilder.Append("<a href=\"###\" onclick=\"action_onchange('rate',$('moderate'),'" + debatepost.Pid.ToString().Trim() + "');\">评分</a>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("		</td>\r\n");
                templateBuilder.Append("		<td class=\"postauthor\">&nbsp;</td>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<div id=\"commentdebates_menu\" style=\"display: none; width:270px;\" class=\"popupmenu_popup\">\r\n");
                templateBuilder.Append("	<form id=\"commentform\" >\r\n");
                templateBuilder.Append("		<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n");
                templateBuilder.Append("		  <tr>\r\n");
                templateBuilder.Append("   		 <td><textarea name=\"commentdebatesmsg\" cols=\"43\" rows=\"6\" id=\"commentdebatesmsg\"></textarea></td>\r\n");
                templateBuilder.Append("		  </tr>                                                      \r\n");
                templateBuilder.Append("		  <tr>\r\n");
                templateBuilder.Append("			<td><input type=\"button\" value=\"提交\"  onclick=\"commentdebates(" + topic.Tid.ToString().Trim() + ",'firstpost')\"/></td>\r\n");
                templateBuilder.Append("		  </tr>\r\n");
                templateBuilder.Append("		</table>\r\n");
                templateBuilder.Append("	</form>\r\n");
                templateBuilder.Append("</div>\r\n");

                if (debatepost.Posterid != -1)
                {

                    templateBuilder.Append("	<!-- member menu -->\r\n");
                    templateBuilder.Append("	<div class=\"popupmenu_popup userinfopanel\" id=\"memberinfo_topic_menu\" style=\"display: none; z-index: 50; filter: progid:dximagetransform.microsoft.shadow(direction=135,color=#cccccc,strength=2); left: 19px; clip: rect(auto auto auto auto); position absolute; top: 253px; width:150px;\" initialized ctrlkey=\"userinfo2\" h=\"209\">\r\n");
                    templateBuilder.Append("		<p class=\"recivemessage\"><a href=\"usercppostpm.aspx?msgtoid=" + debatepost.Posterid.ToString().Trim() + "\" target=\"_blank\">发送短消息</a></p>\r\n");

                    if (useradminid > 0)
                    {


                        if (admininfo.Allowviewip == 1)
                        {

                            templateBuilder.Append("		<p class=\"seeip\"><a href=\"getip.aspx?pid=" + debatepost.Pid.ToString().Trim() + "&topicid=" + topicid.ToString() + "\" title=\"查看IP\">查看IP</a></p>\r\n");

                        }	//end if


                        if (admininfo.Allowbanuser == 1)
                        {

                            templateBuilder.Append("		<p><a href=\"useradmin.aspx?action=banuser&uid=" + debatepost.Posterid.ToString().Trim() + "\" title=\"禁止用户\">禁止用户</a></p>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("		<p>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(debatepost.Posterid);

                    templateBuilder.Append("			<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">查看公共资料</a>\r\n");
                    templateBuilder.Append("		</p>\r\n");

                    if (debatepost.Nickname != "")
                    {

                        templateBuilder.Append("		<p>昵称<em>:" + debatepost.Nickname.ToString().Trim() + "</em></p>\r\n");

                    }	//end if

                    templateBuilder.Append("		<p>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("			ShowStars(" + debatepost.Stars.ToString().Trim() + ", " + config.Starthreshold.ToString().Trim() + ");\r\n");
                    templateBuilder.Append("		</" + "script>\r\n");
                    templateBuilder.Append("		</p>\r\n");

                    if (debatepost.Medals != "")
                    {

                        templateBuilder.Append("		<div class=\"medals\">" + debatepost.Medals.ToString().Trim() + "</div>\r\n");

                    }	//end if

                    templateBuilder.Append("		<ul>\r\n");

                    if (config.Userstatusby == 1)
                    {

                        templateBuilder.Append("			<li>组别:" + debatepost.Status.ToString().Trim() + "</li>\r\n");

                    }	//end if

                    templateBuilder.Append("			<li>性别:<script type=\"text/javascript\">document.write(displayGender(" + debatepost.Gender.ToString().Trim() + "));</" + "script></span></li>\r\n");

                    if (debatepost.Bday != "")
                    {

                        templateBuilder.Append("			<li>生日:" + debatepost.Bday.ToString().Trim() + "</li>\r\n");

                    }	//end if

                    templateBuilder.Append("			<li>来自:" + debatepost.Location.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("			<li>金币:" + debatepost.Credits.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("			<li>帖子:" + debatepost.Posts.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("			<li>注册:\r\n");

                    if (debatepost.Joindate != "")
                    {

                        templateBuilder.Append(Convert.ToDateTime(debatepost.Joindate).ToString("yyyy-MM-dd"));

                    }	//end if

                    templateBuilder.Append("</li>\r\n");
                    templateBuilder.Append("			<li>UID:" + debatepost.Posterid.ToString().Trim() + "</li>\r\n");
                    templateBuilder.Append("		</ul>\r\n");
                    templateBuilder.Append("		<p>状态:\r\n");

                    if (debatepost.Onlinestate == 1)
                    {

                        templateBuilder.Append("在线\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("离线\r\n");

                    }	//end if

                    templateBuilder.Append("</p>\r\n");
                    templateBuilder.Append("		<ul class=\"tools\">\r\n");

                    if (debatepost.Msn != "")
                    {

                        templateBuilder.Append("			<li>\r\n");
                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/msnchat.gif\" alt=\"MSN Messenger: " + debatepost.Msn.ToString().Trim() + "\"/>\r\n");
                        templateBuilder.Append("				<a href=\"mailto:" + debatepost.Msn.ToString().Trim() + "\" target=\"_blank\">" + debatepost.Msn.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("			</li>\r\n");

                    }	//end if


                    if (debatepost.Skype != "")
                    {

                        templateBuilder.Append("			<li>\r\n");
                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/skype.gif\" alt=\"Skype: " + debatepost.Skype.ToString().Trim() + "\"/>\r\n");
                        templateBuilder.Append("				<a href=\"skype:" + debatepost.Skype.ToString().Trim() + "\" target=\"_blank\">" + debatepost.Skype.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("			</li>\r\n");

                    }	//end if


                    if (debatepost.Icq != "")
                    {

                        templateBuilder.Append("			<li>\r\n");
                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/icq.gif\" alt=\"ICQ: " + debatepost.Icq.ToString().Trim() + "\" />\r\n");
                        templateBuilder.Append("				<a href=\"http://wwp.icq.com/scripts/search.dll?to=" + debatepost.Icq.ToString().Trim() + "\" target=\"_blank\">" + debatepost.Icq.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("			</li>\r\n");

                    }	//end if


                    if (debatepost.Qq != "")
                    {

                        templateBuilder.Append("			<li>\r\n");
                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/qq.gif\" alt=\"QQ: " + debatepost.Qq.ToString().Trim() + "\"/>\r\n");
                        templateBuilder.Append("				<a href=\"http://wpa.qq.com/msgrd?V=1&Uin=" + debatepost.Qq.ToString().Trim() + "&Site=" + config.Forumtitle.ToString().Trim() + "&Menu=yes\" target=\"_blank\">" + debatepost.Qq.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("			</li>\r\n");

                    }	//end if


                    if (debatepost.Yahoo != "")
                    {

                        templateBuilder.Append("			<li>\r\n");
                        templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/yahoo.gif\" width=\"16\" alt=\"Yahoo Messenger: " + debatepost.Yahoo.ToString().Trim() + "\"/>\r\n");
                        templateBuilder.Append("				<a href=\"http://edit.yahoo.com/config/send_webmesg?.target=" + debatepost.Yahoo.ToString().Trim() + "&.src=pg\" target=\"_blank\">" + debatepost.Yahoo.ToString().Trim() + "</a>\r\n");
                        templateBuilder.Append("			</li>\r\n");

                    }	//end if

                    templateBuilder.Append("		</ul>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	<!-- member menu -->\r\n");

                }	//end if

                templateBuilder.Append("<div id=\"ajaxdebateposts\">\r\n");
                templateBuilder.Append("<div class=\"box specialpostcontainer\">\r\n");
                templateBuilder.Append("	<ul class=\"tabs\">\r\n");
                aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                templateBuilder.Append("		 <li class=\"current\" style=\"padding:0 8px;\">辩论详情</li><li><a href=\"" + aspxrewriteurl.ToString() + "\">普通模式</a></li>\r\n");
                templateBuilder.Append("	</ul>\r\n");
                templateBuilder.Append("	<div class=\"talkbox\">\r\n");
                templateBuilder.Append("		<div class=\"specialtitle\">\r\n");
                templateBuilder.Append("			<div class=\"squaretitle\">\r\n");
                templateBuilder.Append("				<p>正方观点</p>\r\n");
                templateBuilder.Append("				" + debateexpand.Positiveopinion.ToString().Trim() + "\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("			<div class=\"sidetitle\">\r\n");
                templateBuilder.Append("				<p>反方观点</p>\r\n");
                templateBuilder.Append("				" + debateexpand.Negativeopinion.ToString().Trim() + "\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("		<div class=\"balance\">\r\n");
                templateBuilder.Append("			<span class=\"scalevalue1\"><b id=\"positivediggs\">" + debateexpand.Positivediggs.ToString().Trim() + "</b></span>\r\n");
                templateBuilder.Append("			<span class=\"scalevalue\"><b id=\"negativediggs\">" + debateexpand.Negativediggs.ToString().Trim() + "</b></span>\r\n");
                templateBuilder.Append("			<div id=\"positivepercent\" class=\"squareboll\" style=\"width:" + positivepercent.ToString() + "%;\"></div>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("		<div class=\"talkinner\">\r\n");
                templateBuilder.Append("			<div class=\"squarebox\">\r\n");

                if (!isenddebate)
                {

                    templateBuilder.Append("				<div class=\"buttoncontrol\"><button onclick=\"$('positivepostform').style.display='';this.style.display='none';\">加入正方</button></div>\r\n");
                    templateBuilder.Append("				<div id=\"positivepostform\" style=\"display: none;\">\r\n");
                    templateBuilder.Append("					<form method=\"post\" name=\"postform_" + topicid.ToString() + "\" id=\"postform_" + topicid.ToString() + "\" action=\"postreply.aspx?topicid=" + topicid.ToString() + "\"	enctype=\"multipart/form-data\" onsubmit=\"return validate(this);\" >\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" id=\"title\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"-1\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"debateopinion\" value=\"1\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"parseurloff\" value=\"" + parseurloff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"smileyoff\" value=\"" + smileyoff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"bbcodeoff\" value=\"" + bbcodeoff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n");
                    templateBuilder.Append("						<table cellspacing=\"0\" cellpadding=\"0\" summary=\"正方观点\">\r\n");
                    templateBuilder.Append("							<tr><td>我的意见：</td></tr>\r\n");
                    templateBuilder.Append("							<tr>\r\n");
                    templateBuilder.Append("								<td>\r\n");
                    templateBuilder.Append("									<textarea name=\"message\" cols=\"6\" rows=\"4\" class=\"autosave\" id=\"message\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id)\";></textarea>\r\n");
                    templateBuilder.Append("								</td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");
                    templateBuilder.Append("							<tr>\r\n");
                    templateBuilder.Append("								<td>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("<div id=\"debate_vcode\" name=\"debate_vcode\"><p class=\"formcode\">验证码:\r\n");

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


                        templateBuilder.Append("</p></div>\r\n");

                    }	//end if

                    templateBuilder.Append("<input type=\"submit\" name=\"replysubmit\" value=\"我要发表\" class=\"submitbutton\"/>\r\n");
                    templateBuilder.Append("								</td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");
                    templateBuilder.Append("						</table>\r\n");
                    templateBuilder.Append("					</form>\r\n");
                    templateBuilder.Append("				</div>\r\n");

                }
                else
                {

                    templateBuilder.Append("				<div class=\"buttoncontrol\"></div>\r\n");

                }	//end if


                if (positivepostlist.Count > 0)
                {

                    templateBuilder.Append("					<div id=\"positive_pagenumbers_top\" class=\"debatepages\">" + positivepagenumbers.ToString() + "</div>\r\n");
                    templateBuilder.Append("					<div id=\"positivepage_owner\">\r\n");

                    int positivepost__loop__id = 0;
                    foreach (ShowtopicPagePostInfo positivepost in positivepostlist)
                    {
                        positivepost__loop__id++;

                        templateBuilder.Append("							<div class=\"square\">\r\n");
                        templateBuilder.Append("								<table cellspacing=\"0\" cellpadding=\"0\" summary=\"正方观点\">\r\n");
                        templateBuilder.Append("								<tbody>\r\n");
                        templateBuilder.Append("								<tr>\r\n");
                        templateBuilder.Append("								<td class=\"supportbox\">\r\n");
                        templateBuilder.Append("										<p style=\"background: rgb(255, 255, 255) none repeat scroll 0% 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">支持度\r\n");
                        templateBuilder.Append("										<span class=\"talknum\" id=\"diggs" + positivepost.Pid.ToString().Trim() + "\">" + positivepost.Diggs.ToString().Trim() + "</span>\r\n");

                        if (!isenddebate && positivepost.Posterid != userid)
                        {


                            if (!positivepost.Digged)
                            {

                                templateBuilder.Append("										<span class=\"cliktalk\" id=\"cliktalk" + positivepost.Pid.ToString().Trim() + "\"><a href=\"javascript:void(0);\" onclick=\"digg(" + positivepost.Pid.ToString().Trim() + "," + topic.Tid.ToString().Trim() + ",1)\">支持</a></span>\r\n");

                            }	//end if


                        }	//end if

                        templateBuilder.Append("										</p>\r\n");
                        templateBuilder.Append("								</td>\r\n");
                        templateBuilder.Append("								<td class=\"comment\">\r\n");
                        templateBuilder.Append("									<h3><span>时间:\r\n");
                        templateBuilder.Append(Convert.ToDateTime(positivepost.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
                        templateBuilder.Append("</span>发表者:<a id=\"poster" + positivepost.Pid.ToString().Trim() + "\" href=\"" + UserInfoAspxRewrite(positivepost.Posterid).ToString().Trim() + "\">" + positivepost.Poster.ToString().Trim() + "</a>\r\n");

                        if (ismoder == 1)
                        {

                            templateBuilder.Append("												<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + positivepost.Pid.ToString().Trim() + "&debate=1\">编辑</a>|\r\n");
                            templateBuilder.Append("												<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + positivepost.Pid.ToString().Trim() + "&opinion=1\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n");

                        }
                        else
                        {


                            if (positivepost.Posterid != -1 && userid == positivepost.Posterid)
                            {

                                templateBuilder.Append("												<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + positivepost.Pid.ToString().Trim() + "&debate=1\">编辑</a>|\r\n");
                                templateBuilder.Append("												<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + positivepost.Pid.ToString().Trim() + "&opinion=1\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n");

                            }	//end if


                        }	//end if

                        templateBuilder.Append("									</h3>\r\n");
                        templateBuilder.Append("									<div class=\"debatemessage\"  id=\"message" + positivepost.Pid.ToString().Trim() + "\">\r\n");
                        templateBuilder.Append("									" + positivepost.Message.ToString().Trim() + "\r\n");
                        templateBuilder.Append("									</div>\r\n");

                        if (!isenddebate && positivepost.Posterid != userid)
                        {

                            templateBuilder.Append("									<input name=\"hiddendpid" + positivepost.Pid.ToString().Trim() + "\" type=\"hidden\" id=\"hiddendpid" + positivepost.Pid.ToString().Trim() + "\" value=\"" + positivepost.Ubbmessage.ToString().Trim() + "\" />\r\n");
                            templateBuilder.Append("									<p class=\"othertalk\"><a id=\"reply_btn_" + positivepost.Pid.ToString().Trim() + "\" href=\"###\" onclick=\"showDebatReplyBox(" + topic.Tid.ToString().Trim() + ", " + positivepost.Pid.ToString().Trim() + ", 2, " + parseurloff.ToString() + ", " + smileyoff.ToString() + ", " + bbcodeoff.ToString() + ");this.style.display='none';\">我不同意</a><div id=\"reply_box_owner_" + positivepost.Pid.ToString().Trim() + "\"></div>\r\n");
                            templateBuilder.Append("									</p>\r\n");

                        }	//end if

                        templateBuilder.Append("								</td>\r\n");
                        templateBuilder.Append("								</tr>\r\n");
                        templateBuilder.Append("								</tbody>\r\n");
                        templateBuilder.Append("								</table>\r\n");
                        templateBuilder.Append("							</div>\r\n");

                    }	//end loop

                    templateBuilder.Append("					</div>\r\n");
                    templateBuilder.Append("					<div id=\"positive_pagenumbers_buttom\" class=\"debatepages\">" + positivepagenumbers.ToString() + "</div>\r\n");

                    if (!isenddebate)
                    {

                        templateBuilder.Append("					<div class=\"buttoncontrol\"><button onclick=\"$('positivepostform2').innerHTML=$('positivepostform').innerHTML;$('positivepostform2').style.display='';this.style.display='none';\">加入正方</button></div>\r\n");
                        templateBuilder.Append("					<div id=\"positivepostform2\" style=\"display:none;\"></div>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("			<div class=\"oppositionbox\">\r\n");

                if (!isenddebate)
                {

                    templateBuilder.Append("				<div class=\"buttoncontrol\"><button onclick=\"$('negativepostform').style.display='';this.style.display='none';\">加入反方</button></div>\r\n");
                    templateBuilder.Append("				<div id=\"negativepostform\" style=\"display: none;\" >\r\n");
                    templateBuilder.Append("					<form method=\"post\" name=\"postform_" + topicid.ToString() + "\" id=\"postform_" + topicid.ToString() + "\" action=\"postreply.aspx?topicid=" + topicid.ToString() + "\"	enctype=\"multipart/form-data\" onsubmit=\"return validate(this);\" >\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" id=\"title\" name=\"title\" size=\"84\" tabindex=\"1\" value=\"\"/>\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" id=\"postid\" name=\"postid\" value=\"-1\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"debateopinion\" value=\"2\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"parseurloff\" value=\"" + parseurloff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"smileyoff\" value=\"" + smileyoff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"bbcodeoff\" value=\"" + bbcodeoff.ToString() + "\" />\r\n");
                    templateBuilder.Append("						<input type=\"hidden\" name=\"usesig\" value=\"0\" />\r\n");
                    templateBuilder.Append("						<table cellspacing=\"0\" cellpadding=\"0\" summary=\"反方观点\">\r\n");
                    templateBuilder.Append("							<tr>\r\n");
                    templateBuilder.Append("								<td>我的意见：</td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");
                    templateBuilder.Append("							<tr>\r\n");
                    templateBuilder.Append("								<td>\r\n");
                    templateBuilder.Append("									<textarea name=\"message\" cols=\"6\" rows=\"4\" class=\"autosave\" id=\"message\" tabindex=\"2\" onkeydown=\"debatequickreply(event, this.form);\" onfocus=\"textareachange(this.form.id);\"></textarea>\r\n");
                    templateBuilder.Append("								</td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");
                    templateBuilder.Append("							<tr>\r\n");
                    templateBuilder.Append("								<td>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("<div id=\"debate_vcode\" name=\"debate_vcode\"><p class=\"formcode\">验证码:\r\n");

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


                        templateBuilder.Append("</p></div>\r\n");

                    }	//end if

                    templateBuilder.Append("<input type=\"submit\" name=\"replysubmit\" value=\"我要发表\" class=\"submitbutton\"/>\r\n");
                    templateBuilder.Append("								</td>\r\n");
                    templateBuilder.Append("							</tr>\r\n");
                    templateBuilder.Append("						</table>\r\n");
                    templateBuilder.Append("					</form>\r\n");
                    templateBuilder.Append("				</div>\r\n");

                }
                else
                {

                    templateBuilder.Append("				<div class=\"buttoncontrol\"></div>\r\n");

                }	//end if


                if (negativepostlist.Count > 0)
                {

                    templateBuilder.Append("					<div id=\"negative_pagenumbers_top\" class=\"debatepages\">" + negativepagenumbers.ToString() + "</div>\r\n");
                    templateBuilder.Append("					<div id=\"negativepage_owner\">\r\n");

                    int negativepost__loop__id = 0;
                    foreach (ShowtopicPagePostInfo negativepost in negativepostlist)
                    {
                        negativepost__loop__id++;

                        templateBuilder.Append("							<div class=\"square\">\r\n");
                        templateBuilder.Append("								<table cellspacing=\"0\" cellpadding=\"0\" summary=\"反方观点\">\r\n");
                        templateBuilder.Append("								<tbody>\r\n");
                        templateBuilder.Append("								<tr>\r\n");
                        templateBuilder.Append("								<td class=\"supportbox\">\r\n");
                        templateBuilder.Append("								        <p style=\"background: rgb(255, 255, 255) none repeat scroll 0% 0%; -moz-background-clip: -moz-initial; -moz-background-origin: -moz-initial; -moz-background-inline-policy: -moz-initial;\">支持度\r\n");
                        templateBuilder.Append("										<span class=\"talknum\" id=\"diggs" + negativepost.Pid.ToString().Trim() + "\">" + negativepost.Diggs.ToString().Trim() + "</span>\r\n");

                        if (!isenddebate && negativepost.Posterid != userid)
                        {


                            if (!negativepost.Digged)
                            {

                                templateBuilder.Append("										<span class=\"cliktalk\" id=\"cliktalk" + negativepost.Pid.ToString().Trim() + "\"><a href=\"javascript:void(0);\" onclick=\"digg(" + negativepost.Pid.ToString().Trim() + "," + topic.Tid.ToString().Trim() + ",2)\">支持</a></span>\r\n");

                            }	//end if


                        }	//end if

                        templateBuilder.Append("									</p>\r\n");
                        templateBuilder.Append("								</td>\r\n");
                        templateBuilder.Append("								<td class=\"comment\">\r\n");
                        templateBuilder.Append("									<h3><span>时间:\r\n");
                        templateBuilder.Append(Convert.ToDateTime(negativepost.Postdatetime).ToString("yyyy-MM-dd HH:mm"));
                        templateBuilder.Append("</span>发表者:<a id=\"poster" + negativepost.Pid.ToString().Trim() + "\" href=\"" + UserInfoAspxRewrite(negativepost.Posterid).ToString().Trim() + "\">" + negativepost.Poster.ToString().Trim() + "</a>\r\n");

                        if (ismoder == 1)
                        {

                            templateBuilder.Append("												<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + negativepost.Pid.ToString().Trim() + "&debate=1\">编辑</a>|\r\n");
                            templateBuilder.Append("												<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + negativepost.Pid.ToString().Trim() + "&opinion=2\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n");

                        }
                        else
                        {


                            if (negativepost.Posterid != -1 && userid == negativepost.Posterid)
                            {

                                templateBuilder.Append("												<a href=\"editpost.aspx?topicid=" + topicid.ToString() + "&postid=" + negativepost.Pid.ToString().Trim() + "&debate=1\">编辑</a>|\r\n");
                                templateBuilder.Append("												<a href=\"delpost.aspx?topicid=" + topicid.ToString() + "&postid=" + negativepost.Pid.ToString().Trim() + "&opinion=2\" onclick=\"return confirm('确定要删除吗?');\">删除</a>\r\n");

                            }	//end if


                        }	//end if

                        templateBuilder.Append("									</h3>\r\n");
                        templateBuilder.Append("									<div class=\"debatemessage\" id=\"message" + negativepost.Pid.ToString().Trim() + "\">\r\n");
                        templateBuilder.Append("									" + negativepost.Message.ToString().Trim() + "\r\n");
                        templateBuilder.Append("									</div>\r\n");

                        if (!isenddebate && negativepost.Posterid != userid)
                        {

                            templateBuilder.Append("										<input name=\"hiddendpid" + negativepost.Pid.ToString().Trim() + "\" type=\"hidden\" id=\"hiddendpid" + negativepost.Pid.ToString().Trim() + "\" value=\"" + negativepost.Ubbmessage.ToString().Trim() + "\" />\r\n");
                            templateBuilder.Append("									<p class=\"othertalk\"><a href=\"###\" id=\"reply_btn_" + negativepost.Pid.ToString().Trim() + "\" onclick=\"showDebatReplyBox(" + topic.Tid.ToString().Trim() + ", " + negativepost.Pid.ToString().Trim() + ", 1, " + parseurloff.ToString() + ", " + smileyoff.ToString() + ", " + bbcodeoff.ToString() + ");this.style.display='none';\">我不同意</a><div id=\"reply_box_owner_" + negativepost.Pid.ToString().Trim() + "\"></div>\r\n");
                            templateBuilder.Append("									</p>\r\n");

                        }	//end if

                        templateBuilder.Append("								</td>\r\n");
                        templateBuilder.Append("								</tr>\r\n");
                        templateBuilder.Append("								</tbody>\r\n");
                        templateBuilder.Append("								</table>\r\n");
                        templateBuilder.Append("							</div>\r\n");

                    }	//end loop

                    templateBuilder.Append("					</div>\r\n");
                    templateBuilder.Append("					<div id=\"negative_pagenumbers_buttom\" class=\"debatepages\">" + negativepagenumbers.ToString() + "</div>\r\n");

                    if (!isenddebate)
                    {

                        templateBuilder.Append("					<div class=\"buttoncontrol\"><button onclick=\"$('negativepostform2').innerHTML=$('negativepostform').innerHTML;$('negativepostform2').style.display='';this.style.display='none';\">加入反方</button></div>\r\n");
                        templateBuilder.Append("					<div id=\"negativepostform2\" style=\"display:none;\"></div>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("	</div>\r\n");
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