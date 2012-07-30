using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;
#if NET1
#else
using Discuz.Common.Generic;
#endif
using Discuz.Data;


namespace Discuz.Web
{
    /// <summary>
    /// 帖子管理页面
    /// </summary>
    public partial class topicadmin : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 操作标题
        /// </summary>
        public string operationtitle = "";

        /// <summary>
        /// 操作类型
        /// </summary>
        public string operation = "";

        /// <summary>
        /// 操作类型参数
        /// </summary>
        public string action = "";

        /// <summary>
        /// 主题列表
        /// </summary>
        public string topiclist = "0";

        /// <summary>
        /// 帖子Id列表
        /// </summary>
        public string postidlist = "0";

        /// <summary>
        /// 版块名称
        /// </summary>
        public string forumname = "";

        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";

        /// <summary>
        /// 帖子标题
        /// </summary>
        public string title = "";

        /// <summary>
        /// 帖子作者用户名
        /// </summary>
        public string poster = "";

        /// <summary>
        /// 版块Id
        /// </summary>
        public int forumid = 0;

        /// <summary>
        /// 版块列表
        /// </summary>
        public string forumlist = "";

        /// <summary>
        /// 主题置顶状态
        /// </summary>
        public int displayorder = 0;

        /// <summary>
        /// 主题精华状态
        /// </summary>
        public int digest = 0;

        /// <summary>
        /// 高亮颜色
        /// </summary>
        public string highlight_color = "";

        /// <summary>
        /// 是否加粗
        /// </summary>
        public string highlight_style_b = "";

        /// <summary>
        /// 是否斜体
        /// </summary>
        public string highlight_style_i = "";

        /// <summary>
        /// 是否带下划线
        /// </summary>
        public string highlight_style_u = "";

        /// <summary>
        /// 关闭主题, 0=打开,1=关闭 
        /// </summary>
        public int close = 0;

        /// <summary>
        /// 移动主题时的目标版块Id
        /// </summary>
        public int moveto = 0;

        /// <summary>
        /// 移动方式
        /// </summary>
        public string type = ""; //移动方式

        /// <summary>
        /// 后续操作
        /// </summary>
        public int donext = 0;

        /// <summary>
        /// 帖子列表
        /// </summary>
        public DataTable postlist;

        /// <summary>
        /// 可用金币列表
        /// </summary>
        public DataTable scorelist;

#if NET1
        public TopicIdentifyCollection identifylist;
#else
        /// <summary>
        /// 主题鉴定类型列表
        /// </summary>
        public List<TopicIdentify> identifylist;
#endif

        /// <summary>
        /// 主题鉴定js数组
        /// </summary>
        public string identifyjsarray;

        /// <summary>
        /// 主题分类选项
        /// </summary>
        public string topictypeselectoptions; //当前版块的主题类型选项

        /// <summary>
        /// 当前帖子评分日志列表
        /// </summary>
        public DataTable ratelog = new DataTable();

        /// <summary>
        /// 当前帖子评分日志记录数
        /// </summary>
        public int ratelogcount = 0;
        /// <summary>
        /// 当前的主题
        /// </summary>
        public TopicInfo topicinfo;
        public int opinion = -1;

        #endregion

        // 是否允许管理主题, 初始false为不允许
        protected bool ismoder = false;
        protected int RateIsReady = 0;
        private ForumInfo forum;
        private int highlight = 0;
        public bool issendmessage = false;
        public bool isreason = false;

        protected override void ShowPage()
        {
            ValidatePermission();

            if (!BindTitle())
            {
                return;
            }
        }

        /// <summary>
        /// 验证权限
        /// </summary>
        private void ValidatePermission()
        {
            opinion = DNTRequest.GetInt("opinion", -1);
            if (userid == -1)
            {
                AddErrLine("请先登录");
                return;
            }
            UserGroupInfo usergroupinfo = UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetUserInfo(userid).Groupid);
            switch (usergroupinfo.Reasonpm)
            {

                case 1:
                    isreason = true;
                    break;
                case 2:
                    issendmessage = true;
                    break;
                case 3:
                    isreason = true;
                    issendmessage = true;
                    break;

                default:
                    break;
            }
            action = DNTRequest.GetQueryString("action");
            if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()) || action == "")
            {
                AddErrLine("非法提交");
                return;
            }

            forumid = DNTRequest.GetInt("forumid", -1);
            // 检查是否具有版主的身份
            ismoder = Moderators.IsModer(useradminid, userid, forumid);
            // 如果拥有管理组身份
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);

            operation = DNTRequest.GetQueryString("operation").ToLower();
            if (!operation.Equals("rate") && !operation.Equals("bonus") && !operation.Equals("banpost") && !DNTRequest.GetString("operat").Equals("rate") && !DNTRequest.GetString("operat").Equals("bonus") && !DNTRequest.GetString("operat").Equals("banpost"))
            {
                // 如果所属管理组不存在
                if (admininfo == null)
                {
                    AddErrLine("你没有管理权限");
                    return;
                }
            }

            operationtitle = "操作提示";

            SetUrl(base.ShowForumAspxRewrite(forumid, 0));


            topiclist = DNTRequest.GetString("topicid");
            postidlist = DNTRequest.GetString("postid");

            if (action == "")
            {
                AddErrLine("操作类型参数为空");
                return;
            }

            if (forumid == -1)
            {
                //
                AddErrLine("版块ID必须为数字");
                return;
            }

            if (DNTRequest.GetFormString("topicid") != "" && !Topics.InSameForum(topiclist, forumid))
            {
                AddErrLine("无法对非本版块主题进行管理操作");
                return;
            }
            displayorder = TopicAdmins.GetDisplayorder(topiclist);
            digest = TopicAdmins.GetDigest(topiclist);
            forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name;
            topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);

            if (!Forums.AllowView(forum.Viewperm, usergroupid))
            {
                AddErrLine("您没有浏览该版块的权限");
                return;
            }

            pagetitle = Utils.RemoveHtml(forumname);
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            if (topiclist.CompareTo("") == 0)
            {
                AddErrLine("您没有选择主题或相应的管理操作,请返回修改");
                return;
            }

            if (operation.CompareTo("") != 0)
            {
                // DoOperations执行管理操作
                if (!DoOperations(forum, admininfo, config.Reasonpm))
                {
                    return;
                }
                // 删除主题游客缓存
                ForumUtils.DeleteTopicCacheFile(topiclist);
            }


            if (action.CompareTo("moderate") != 0)
            {
                if (
                    "del,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,delposts,banpost".IndexOf(
                        operation) == -1)
                {
                    AddErrLine("你无权操作此功能");
                    return;
                }
                operation = action;
            }
            else
            {
                if (operation.CompareTo("") == 0)
                {
                    operation = DNTRequest.GetString("operat");
                }

                if (operation.CompareTo("") == 0)
                {
                    //
                    AddErrLine("您没有选择主题或相应的管理操作,请返回修改");
                    return;
                }
            }

        }

        /// <summary>
        /// 绑定操作的标题
        /// </summary>
        /// <returns></returns>
        private bool BindTitle()
        {
            switch (operation)
            {
                case "del":
                    operationtitle = "删除主题";
                    break;

                case "move":
                    operationtitle = "移动主题";
                    forumlist = Caches.GetForumListBoxOptionsCache();
                    break;

                case "type":
                    operationtitle = "主题分类";
                    break;

                case "highlight": //设置高亮
                    operationtitle = "高亮显示";
                    donext = 1;
                    break;

                case "close":
                    operationtitle = "关闭/打开主题";
                    donext = 1;
                    break;

                case "displayorder": //设置置顶
                    operationtitle = "置顶/解除置顶";
                    donext = 1;
                    break;

                case "digest": //设置精华
                    operationtitle = "加入/解除精华 ";
                    donext = 1;
                    break;

                case "copy":
                    operationtitle = "复制主题";
                    forumlist = Caches.GetForumListBoxOptionsCache();
                    break;

                case "split":
                    {
                        operationtitle = "分割主题";
                        int tid = Utils.StrToInt(topiclist, 0);
                        if (tid <= 0)
                        {
                            AddErrLine(string.Format("您当前的身份 \"{0}\" 没有分割主题的权限", usergroupinfo.Grouptitle));
                            return false;
                        }
                        postlist = Posts.GetPostListTitle(tid);
                        if (postlist != null)
                        {
                            if (postlist.Rows.Count > 0)
                            {
                                postlist.Rows[0].Delete();
                                postlist.AcceptChanges();
                            }
                        }
                        break;
                    }
                case "merge":
                    operationtitle = "合并主题";
                    break;

                case "bump":
                    operationtitle = "提升/下沉主题";
                    break;

                case "repair":
                    operationtitle = "修复主题";
                    break;

                case "rate":
                    {
                        if (!CheckRatePermission())
                        {
                            return false;
                        }

                        string repost = TopicAdmins.CheckRateState(postidlist, userid);
                        //if (!repost.Equals("") && RateIsReady != 1)
                        if (config.Dupkarmarate != 1 && !repost.Equals("") && RateIsReady != 1)
                        {
                            AddErrLine(string.Format("对不起,您不能对帖子ID 为({0}) 的帖子重复评分,请返回.", repost));
                            return false;
                        }
                        scorelist = UserGroups.GroupParticipateScore(userid, usergroupid);
                        if (scorelist.Rows.Count < 1)
                        {
                            AddErrLine(string.Format("您当前的身份 \"{0}\" 没有评分的权限, 或者今日可用评分已经用完", usergroupinfo.Grouptitle));
                            return false;
                        }
                        PostInfo postinfo =
                            Posts.GetPostInfo(Utils.StrToInt(topiclist, 0), Utils.StrToInt(postidlist, 0));
                        if (postinfo == null)
                        {
                            AddErrLine("您没有选择要评分的帖子, 请返回修改.");
                            return false;
                        }
                        poster = postinfo.Poster;
                        if (postinfo.Posterid == userid)
                        {
                            AddErrLine("对不起,您不能对自已的帖子评分,请返回.");
                            return false;
                        }

                        title = postinfo.Title;
                        topiclist = postinfo.Tid.ToString();
                        operationtitle = "参与评分";
                        break;
                    }
                case "cancelrate":
                    {
                        PostInfo postinfo = Posts.GetPostInfo(Utils.StrToInt(topiclist, 0), Utils.StrToInt(postidlist, 0));
                        if (postinfo == null)
                        {
                            AddErrLine("您没有选择要撤消评分的帖子, 请返回修改.");
                            return false;
                        }

                        if (!ismoder)
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有撤消评分的权限");
                            return false;
                        }
                        poster = postinfo.Poster;

                        title = postinfo.Title;
                        topiclist = postinfo.Tid.ToString();
                        operationtitle = "撤销评分";

                        ratelogcount = AdminRateLogs.RecordCount("pid = " + postidlist);
                        ratelog = AdminRateLogs.LogList(ratelogcount, 1, "pid = " + postidlist);
                        ratelog.Columns.Add("extcreditname", Type.GetType("System.String"));
                        DataTable scorePaySet = Scoresets.GetScoreSet();

                        //绑定金币名称属性
                        foreach (DataRow dr in ratelog.Rows)
                        {
                            int extcredits = Utils.StrToInt(dr["extcredits"].ToString(), 0);
                            if ((extcredits > 0) && (extcredits < 9))
                            {
                                if (scorePaySet.Columns.Count > extcredits + 1)
                                {
                                    dr["extcreditname"] = scorePaySet.Rows[0][extcredits + 1].ToString();
                                }
                                else
                                {
                                    dr["extcreditname"] = "";
                                }
                            }
                            else
                            {
                                dr["extcreditname"] = "";
                            }
                        }

                        break;
                    }
                case "delposts":
                    operationtitle = "批量删贴";
                    break;
                case "identify":
                    operationtitle = "鉴定主题";
                    identifylist = Caches.GetTopicIdentifyCollection();
                    identifyjsarray = Caches.GetTopicIdentifyFileNameJsArray();
                    break;
                case "bonus":
                    {
                        operationtitle = "结帖";
                        int tid = Utils.StrToInt(topiclist, 0);
                        postlist = Posts.GetPostListTitle(tid);
                        if (postlist != null)
                        {
                            if (postlist.Rows.Count > 0)
                            {
                                postlist.Rows[0].Delete();
                                postlist.AcceptChanges();
                            }
                        }

                        if (postlist.Rows.Count == 0)
                        {
                            AddErrLine("无法对没有回复的悬赏进行结帖");
                        }

                        topicinfo = Topics.GetTopicInfo(tid);
                        if (topicinfo.Special == 3)
                        {
                            AddMsgLine("本主题的悬赏已经结束");
                        }
                        break;
                    }
                case "banpost":
                    operationtitle = "单贴屏蔽";
                    break;
                default:
                    operationtitle = "未知操作";
                    break;
            }
            return true;
        }

        /// <summary>
        /// 检查评分权限
        /// </summary>
        /// <returns></returns>
        private bool CheckRatePermission()
        {
            if (usergroupinfo.Raterange.Equals(""))
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有评分的权限", usergroupinfo.Grouptitle));
                return false;
            }
            else
            {
                string[] rolesByScoreType = usergroupinfo.Raterange.Split('|');
                bool hasExtcreditsCanRate = false;
                foreach (string roleByScoreType in rolesByScoreType)
                {
                    string[] role = roleByScoreType.Split(',');
                    //数组结构:  扩展金币编号,参与评分,金币代号,金币名称,评分最小值,评分最大值,24小时最大评分数
                    //				0			1			2		3		4			5			6
                    if (Utils.StrToBool(role[1], false))
                    {
                        hasExtcreditsCanRate = true;
                    }
                }
                if (!hasExtcreditsCanRate)
                {
                    AddErrLine(string.Format("您当前的身份 \"{0}\" 没有评分的权限", usergroupinfo.Grouptitle));
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 进行相关操作
        /// </summary>
        /// <param name="forum"></param>
        /// <param name="admininfo"></param>
        /// <param name="reasonpm"></param>
        /// <returns></returns>
        private bool DoOperations(ForumInfo forum, AdminGroupInfo admininfo, int reasonpm)
        {
            string operationName = "";
            string next = DNTRequest.GetFormString("next");
            string referer = DNTRequest.GetFormString("referer");

            DataTable dt = null;

            #region DoOperation

            string reason = DNTRequest.GetString("reason");
            int sendmsg = DNTRequest.GetFormInt("sendmessage", 0);
            if (issendmessage && sendmsg == 0)
            {
                AddErrLine("您所在的用户组需要在操作时发送短消息");
                return false;
            }


            if (operation != "identify" && operation != "bonus")
            {

                if (isreason)
                {
                    if (reason.Equals(""))
                    {
                        AddErrLine("操作原因不能为空");
                        return false;
                    }
                    else
                    {
                        if (reason.Length >= 20)
                        {
                            AddErrLine("操作原因不能多于20个字符");
                            return false;
                        }
                    }
                }
            }

            if (
                "del,move,type,highlight,close,displayorder,digest,copy,split,merge,bump,repair,rate,cancelrate,delposts,identify,bonus,banpost"
                    .IndexOf(operation) == -1)
            {
                AddErrLine("未知的操作参数");
                return false;
            }
            //执行提交操作
            if (next.Trim() != "")
            {
                referer = string.Format("topicadmin.aspx?action={0}&forumid={1}&topicid={2}", next, forumid.ToString(), topiclist);
            }
            //else
            //{
            //    referer = string.Format(base.ShowForumAspxRewrite(forumid, 0));
            //}
            int operationid = 0;

            //if (!operation.Equals("rate") && !operation.Equals("delposts"))
            //{
            //    dt = Topics.GetTopicList(topiclist, -1);
            //}

            bool istopic = false;
            string subjecttype;
            string postoperations = "rate,delposts,banpost";
            if (postoperations.Contains(operation))
            {
                dt = Posts.GetPostList(postidlist, topiclist);
                subjecttype = "帖子";
            }
            else
            {
                dt = Topics.GetTopicList(topiclist, -1);
                istopic = true;
                subjecttype = "主题";

            }


            #region switch operation

            switch (operation)
            {
                case "del":

                    #region delete
                    operationName = "删除主题";
                    if (!DoDeleteOperation(forum))
                        return false;
                    operationid = 1;
                    #endregion

                    break;

                case "move":

                    #region move
                    operationName = "移动主题";
                    if (!DoMoveOperation())
                        return false;
                    operationid = 2;
                    #endregion

                    break;

                case "type":
                    #region type
                    operationName = "主题分类";
                    if (!DoTypeOperation())
                        return false;
                    operationid = 3;
                    #endregion

                    break;

                case "highlight": //设置高亮
                    #region highlight
                    operationName = "设置高亮";
                    if (!DoHighlightOperation())
                        return false;
                    operationid = 4;
                    #endregion

                    break;

                case "close":

                    #region close
                    operationName = "关闭主题/取消";
                    if (!DoCloseOperation())
                        return false;
                    operationid = 5;
                    #endregion

                    break;

                case "displayorder":
                    #region displayorder
                    operationName = "主题置顶/取消";
                    if (!DoDisplayOrderOperation(admininfo))
                        return false;
                    operationid = 6;
                    #endregion

                    break;

                case "digest": //设置精华

                    #region digest
                    operationName = "设置精华/取消";
                    if (!DoDigestOperation())
                        return false;
                    operationid = 7;
                    #endregion

                    break;

                case "copy": //复制主题";

                    #region copy
                    operationName = "复制主题";
                    if (!DoCopyOperation())
                        return false;
                    operationid = 8;
                    #endregion

                    break;

                case "split":

                    #region split
                    operationName = "分割主题";
                    if (!DoSplitOperation())
                        return false;
                    operationid = 9;
                    #endregion

                    break;

                case "merge":

                    #region merge
                    operationName = "合并主题";
                    if (!DoMergeOperation())
                        return false;
                    operationid = 10;
                    #endregion

                    break;

                case "bump": //提升主题
                    operationName = "提升/下沉主题";
                    if (!DoBumpTopicsOperation())
                        return false;
                    operationid = 11;
                    break;

                case "repair": //修复主题
                    operationName = "修复主题";
                    TopicAdmins.RepairTopicList(topiclist);
                    operationid = 12;
                    break;

                case "rate":

                    #region rate
                    operationName = "帖子评分";
                    if (!DoRateOperation(reason))
                        return false;
                    operationid = 13;
                    #endregion

                    break;

                case "delposts":
                    operationName = "批量删贴";
                    if (!ismoder)
                    {
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 没有删除的权限", usergroupinfo.Grouptitle));
                        return false;
                    }
                    if (!DoDelpostsOperation(reason, forum))
                        return false;
                    operationid = 14;
                    break;

                case "identify":
                    operationName = "鉴定主题";
                    if (!ismoder)
                    {
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 没有鉴定的权限", usergroupinfo.Grouptitle));
                        return false;
                    }
                    if (!DoIndentifyOperation())
                        return false;
                    operationid = 15;
                    break;

                case "cancelrate":

                    #region cancelrate
                    operationName = "撤销评分";
                    if (!DoCancelRateOperation(reason))
                        return false;
                    operationid = 16;
                    #endregion

                    break;
                case "bonus":
                    operationName = "结帖";

                    if (!DoBonusOperation())
                        return false;
                    operationid = 16;
                    break;

                case "banpost":
                    operationName = "屏蔽帖子";
                    if (!DoBanPostOperatopn())
                        return false;
                    operationid = 17;
                    break;
                default:
                    operationName = "未知操作";
                    break;
            }

            #endregion

            if (next.CompareTo("") == 0)
            {
                AddMsgLine("管理操作成功,现在将转入主题列表");
            }
            else
            {
                AddMsgLine("管理操作成功,现在将转入后续操作");
            }


            //if (operation.Equals("rate"))
            //{
            //    Posts.UpdatePostRate(Utils.StrToInt(topiclist, 0), postidlist);
            //    if (config.Modworkstatus == 1)
            //    {
            //        foreach (string postid in postidlist.Split(','))
            //        {
            //            PostInfo postinfo = Posts.GetPostInfo(Utils.StrToInt(topiclist, 0), Utils.StrToInt(postid, 0));
            //            AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
            //                                         usergroupinfo.Grouptitle, Utils.GetRealIP(),
            //                                         Utils.GetDateTime(), forumid.ToString(), forumname,
            //                                         postid, postinfo.Title, operation, reason);
            //        }
            //    }
            //}else 
            //if (operation.Equals("delposts"))
            //{
            //    // do nothing, already logged in method DoDelpostsOperation()
            //}
            //else
            //{
                if (!operation.Equals("rate"))
                {
                    //Posts.UpdatePostRate(Utils.StrToInt(topiclist, 0), postidlist);
                    if (config.Modworkstatus == 1)
                    {
                        if (postidlist.Equals(""))
                        {
                            foreach (string tid in topiclist.Split(','))
                            {
                                string title;
                                if (operation == "del")
                                {
                                    title = "";
                                }
                                else
                                {
                                    TopicInfo topicinfo = Topics.GetTopicInfo(Utils.StrToInt(tid, -1));
                                    title = topicinfo.Title;
                                }
                                    AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                                   usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                                   Utils.GetDateTime(), forumid.ToString(), forumname,
                                                                   tid, title , operation, reason);
                            }
                        }
                        else
                        {
                            string [] postarray = postidlist.Split(',');
                            TopicInfo topinfo = Topics.GetTopicInfo(Utils.StrToInt(topiclist, -1));
                            foreach (string postid in postarray)
                            {
                                PostInfo postinfo = Posts.GetPostInfo(Utils.StrToInt(topiclist, 0), Utils.StrToInt(postid, 0));
                                string postitle;
                                if (postinfo == null)
                                {
                                    postitle = topinfo.Title;

                                }
                                else
                                { 
                                 postitle = postinfo.Title == "" ? topinfo.Title : postinfo.Title;
                                }
                               
                              
                                AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                             usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                             Utils.GetDateTime(), forumid.ToString(), forumname,
                                                             postid, postitle, operation, reason);
                            }
                        }
                    }
                }
                SendMessage(operationid, dt, istopic, operationName, reason, sendmsg, subjecttype);
            //}


            //执行完某一操作后转到后续操作
            SetUrl(referer);
            if (next != string.Empty)
            {
                HttpContext.Current.Response.Redirect("/" + referer, false);
            }
            else
            {
                AddScript("window.setTimeout('redirectURL()', 2000);function redirectURL() {window.location='" + referer +
                          "';}");
            }
            //HttpContext.Current.Response.Write("<script type='text/javascript'>window.setTimeout('redirectURL()', 2000);function redirectURL() {window.location='" + referer + "';}</script>");
            //HttpContext.Current.Response.Redirect(referer);
            //SetMetaRefresh();
            SetShowBackLink(false);

            #endregion

            return true;
        }

        private void SendMessage(int operationid, DataTable dt, bool istopic, string operationName, string reason, int sendmsg, string subjecttype)
        {
            ////Topics.UpdateTopicModerated(topiclist, operationid);
            ////if (config.Modworkstatus == 1)
            ////{
            ////    if (dt != null)
            ////    {
            ////        foreach (DataRow dr in dt.Rows)
            ////        {
            ////            //string topicStr = dr["tid"].ToString() + "\r\n" + dr["title"].ToString();
            ////            AdminModeratorLogs.InsertLog(this.userid.ToString(), username, usergroupid.ToString(),
            ////                                         this.usergroupinfo.Grouptitle, Utils.GetRealIP(),
            ////                                         Utils.GetDateTime(), this.forumid.ToString(), this.forumname,
            ////                                         dr["tid"].ToString(), dr["title"].ToString(), operationName,
            ////                                         reason);

            ////            if (reasonpm == 1)
            ////            {
            ////                int posterid = Utils.StrToInt(dr["posterid"], -1);
            ////                if (posterid != -1) //是游客，管理操作就不发短消息了
            ////                {
            ////                    if (PrivateMessages.GetPrivateMessageCount(posterid, -1) <
            ////                        UserGroups.GetUserGroupInfo(Users.GetShortUserInfo(posterid).Groupid).Maxpmnum)
            ////                    {
            ////                        PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();

            ////                        string curdatetime = Utils.GetDateTime();
            ////                        // 收件箱
            ////                        privatemessageinfo.Message =
            ////                            Utils.HtmlEncode(
            ////                                string.Format(
            ////                                    "这是由论坛系统自动发送的通知短消息。\r\n以下您所发表的主题被 {0} {1} 执行 {2} 操作。\r\n\r\n主题: {3} \r\n发表时间: {4}\r\n所在论坛: {5}\r\n操作理由: {6}\r\n\r\n如果您对本管理操作有异议，请与我取得联系。",
            ////                                    Utils.RemoveHtml(this.usergroupinfo.Grouptitle), username,
            ////                                    operationName, dr["title"].ToString().Trim(),
            ////                                    dr["postdatetime"].ToString(), this.forumname, reason));
            ////                        privatemessageinfo.Subject = Utils.HtmlEncode("您发表的主题被执行管理操作");
            ////                        privatemessageinfo.Msgto = dr["poster"].ToString();
            ////                        privatemessageinfo.Msgtoid = posterid;
            ////                        privatemessageinfo.Msgfrom = username;
            ////                        privatemessageinfo.Msgfromid = userid;
            ////                        privatemessageinfo.New = 1;
            ////                        privatemessageinfo.Postdatetime = curdatetime;
            ////                        privatemessageinfo.Folder = 0;
            ////                        PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);
            ////                    }
            ////                }
            ////            }
            ////        }
            ////        dt.Dispose();
            ////    }
            ////}
            //--------------------------------------------------------------------------------------------------------------
            if (istopic)
            {
                Topics.UpdateTopicModerated(topiclist, operationid);

            }

            if (dt != null)
            {
                if (ForumUtils.HasBannedWord(reason))
                {
                    AddErrLine("对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!");
                    return;
                }
                else
                {
                    reason = ForumUtils.BanWordFilter(reason);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    //string topicStr = dr["tid"].ToString() + "\r\n" + dr["title"].ToString();
                    if (istopic)
                    {
                        AdminModeratorLogs.InsertLog(this.userid.ToString(), username, usergroupid.ToString(),
                                                     this.usergroupinfo.Grouptitle, Utils.GetRealIP(),
                                                     Utils.GetDateTime(), this.forumid.ToString(), this.forumname,
                                                     dr["tid"].ToString(), dr["title"].ToString(), operationName,
                                                     reason);
                    }


                    if (sendmsg == 1)
                    {
                        //int posterid = Utils.StrToInt(dr["posterid"], -1);
                        //if (posterid != -1) //是游客，管理操作就不发短消息了
                        //{

                        //    PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();

                        //    string curdatetime = Utils.GetDateTime();
                        //    // 收件箱
                        //    privatemessageinfo.Message =
                        //        Utils.HtmlEncode(
                        //            string.Format(
                        //                "这是由论坛系统自动发送的通知短消息。\r\n以下您所发表的{7}被 {0} {1} 执行 {2} 操作。\r\n\r\n{7}: {3} \r\n发表时间: {4}\r\n所在论坛: {5}\r\n操作理由: {6}\r\n\r\n如果您对本管理操作有异议，请与我取得联系。",
                        //                Utils.RemoveHtml(this.usergroupinfo.Grouptitle), username,
                        //                operationName, dr["title"].ToString().Trim(),
                        //                dr["postdatetime"].ToString(), this.forumname, reason,subjecttype));
                        //    privatemessageinfo.Subject = Utils.HtmlEncode(string.Format("您发表的{0}被执行管理操作",subjecttype));
                        //    privatemessageinfo.Msgto = dr["poster"].ToString();
                        //    privatemessageinfo.Msgtoid = posterid;
                        //    privatemessageinfo.Msgfrom = username;
                        //    privatemessageinfo.Msgfromid = userid;
                        //    privatemessageinfo.New = 1;
                        //    privatemessageinfo.Postdatetime = curdatetime;
                        //    privatemessageinfo.Folder = 0;
                        //    PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);

                        //}
                        MessagePost(dr, operationName, subjecttype, reason);
                    }

                }
                dt.Dispose();
            }



        }

        #region Operations

        private void MessagePost(DataRow dr, string operationName, string subjecttype, string reason)
        {
            int posterid = Utils.StrToInt(dr["posterid"], -1);
            if (posterid != -1) //是游客，管理操作就不发短消息了
            {

                PrivateMessageInfo privatemessageinfo = new PrivateMessageInfo();

                string curdatetime = Utils.GetDateTime();
                // 收件箱
                privatemessageinfo.Message =
                    Utils.HtmlEncode(
                        string.Format(
                            "这是由论坛系统自动发送的通知短消息。\r\n以下您所发表的{7}被 {0} {1} 执行 {2} 操作。\r\n\r\n{7}: {3} \r\n发表时间: {4}\r\n所在论坛: {5}\r\n操作理由: {6}\r\n\r\n如果您对本管理操作有异议，请与我取得联系。",
                            Utils.RemoveHtml(this.usergroupinfo.Grouptitle), username,
                            operationName, dr["title"].ToString().Trim(),
                            dr["postdatetime"].ToString(), this.forumname, reason, subjecttype));
                privatemessageinfo.Subject = Utils.HtmlEncode(string.Format("您发表的{0}被执行管理操作", subjecttype));
                privatemessageinfo.Msgto = dr["poster"].ToString();
                privatemessageinfo.Msgtoid = posterid;
                privatemessageinfo.Msgfrom = username;
                privatemessageinfo.Msgfromid = userid;
                privatemessageinfo.New = 1;
                privatemessageinfo.Postdatetime = curdatetime;
                privatemessageinfo.Folder = 0;
                PrivateMessages.CreatePrivateMessage(privatemessageinfo, 0);


            }


        }

        /// <summary>
        /// 评分
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        private bool DoRateOperation(string reason)
        {
            if (!CheckRatePermission())
            {
                return false;
            }
            string score = DNTRequest.GetFormString("score");
            string extcredits = DNTRequest.GetFormString("extcredits");

            if (postidlist.Equals(""))
            {
                AddErrLine("您没有选择要评分的帖子, 请返回修改.");
                return false;
            }
            //"[uid]=" + userid + " AND [pid] = " + Utils.StrToInt(postidlist,0).ToString()
            if (config.Dupkarmarate != 1 &&
                AdminRateLogs.RecordCount(DatabaseProvider.GetInstance().GetRateLogCountCondition(userid, postidlist)) >
                0)
            {
                AddErrLine("您不能对本帖重复评分.");
                return false;
            }


            scorelist = UserGroups.GroupParticipateScore(userid, usergroupid);
            string[] scoreArr = Utils.SplitString(score, ",");
            string[] extcreditsArr = Utils.SplitString(extcredits, ",");
            string cscoreArr = string.Empty;
            string cextcreditsArr = string.Empty;
            int ratetimes = 0;
            for (int i = 0; i < scoreArr.Length; i++)
            {
                if (Utils.IsNumeric(scoreArr[i].ToString()) && scoreArr[i].ToString() != "0" && !scoreArr[i].ToString().Contains("."))
                {
                    cscoreArr = cscoreArr + scoreArr[i] + ",";

                    cextcreditsArr = cextcreditsArr + extcreditsArr[i] + ",";
                    ratetimes++;
                }

            }

            if (cscoreArr.Length == 0)
            {
                AddErrLine("请至少为一个评分项目设置非零整数值");
                return false;
            }

            Posts.UpdatePostRateTimes(Utils.StrToInt(topiclist, 0), postidlist, ratetimes);
            //TopicAdmins.RatePosts(Utils.StrToInt(topiclist, 0), postidlist, score, extcredits, userid, username, reason);
            TopicAdmins.RatePosts(Utils.StrToInt(topiclist, 0), postidlist, cscoreArr, cextcreditsArr, userid, username, reason);
            RateIsReady = 1;
            return true;
        }

        /// <summary>
        /// 撤消评分
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        private bool DoCancelRateOperation(string reason)
        {
            if (!CheckRatePermission())
            {
                return false;
            }

            if (postidlist.Equals(""))
            {
                AddErrLine("您没有选择要撤销评分的帖子, 请返回修改.");
                return false;
            }

            if (!ismoder)
            {
                AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有撤消评分的权限");
                return false;
            }

            if (DNTRequest.GetFormString("ratelogid").Equals(""))
            {
                AddErrLine("您未选取要撤消的评分记录");
                return false;
            }
            int ratetimes = (DNTRequest.GetFormString("ratelogid").Split(',').Length) * -1;
            Posts.UpdatePostRateTimes(Utils.StrToInt(topiclist, 0), postidlist, ratetimes);
            TopicAdmins.CancelRatePosts(DNTRequest.GetFormString("ratelogid"), Utils.StrToInt(topiclist, 0), postidlist, userid, username, usergroupinfo.Groupid, usergroupinfo.Grouptitle, forumid, forumname, reason);
            return true;
        }

        /// <summary>
        /// 合并主题
        /// </summary>
        /// <returns></returns>
        private bool DoMergeOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有合并主题的权限", usergroupinfo.Grouptitle));
                return false;
            }
            int othertid = DNTRequest.GetFormInt("othertid", 0);
            if (othertid == 0)
            {
                AddErrLine("您没有输入要合并的主题ID, 请返回修改!");
                return false;
            }

            //同一主题,不能合并
            if (othertid == Utils.StrToInt(topiclist, 0))
            {
                AddErrLine("不能对同一主题进行合并操作");
                return false;
            }

            TopicAdmins.MerrgeTopics(topiclist, othertid);
            return true;
        }

        /// <summary>
        /// 分割主题
        /// </summary>
        /// <returns></returns>
        private bool DoSplitOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有分割主题的权限", usergroupinfo.Grouptitle));
                return false;
            }

            string subject = DNTRequest.GetString("subject");
            if (subject.Equals(""))
            {
                AddErrLine("您没有输入标题, 请返回修改!");
                return false;
            }

            if (subject.Length > 60)
            {
                AddErrLine("您的标题过长, 标题最大长度为 60 字, 请返回修改!");
                return false;
            }

            if (postidlist.Equals(""))
            {
                AddErrLine("您没有选择要分割入新主题的帖子, 请返回修改.");
                return false;
            }

            TopicAdmins.SplitTopics(postidlist, subject, topiclist);
            return true;
        }

        /// <summary>
        /// 复制主题
        /// </summary>
        /// <returns></returns>
        private bool DoCopyOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有复制主题的权限", usergroupinfo.Grouptitle));
                return false;
            }
            int copyto = DNTRequest.GetFormInt("copyto", 0);
            if (copyto == 0)
            {
                AddMsgLine("您没有选择目标论坛/分类");
                return false;
            }

            TopicAdmins.CopyTopics(topiclist, copyto);
            return true;
        }

        /// <summary>
        /// 精华操作
        /// </summary>
        /// <returns></returns>
        private bool DoDigestOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有设置精华的权限", usergroupinfo.Grouptitle));
                return false;
            }
            digest = DNTRequest.GetFormInt("level", -1);
            if (digest > 3 || digest < 0)
            {
                digest = -1;
            }
            if (digest == -1)
            {
                //
                AddErrLine("您没有选择精华级别");
                return false;
            }

            TopicAdmins.SetDigest(topiclist, short.Parse(digest.ToString()));
            return true;
        }

        /// <summary>
        /// 置顶操作
        /// </summary>
        /// <param name="admininfo"></param>
        /// <returns></returns>
        private bool DoDisplayOrderOperation(AdminGroupInfo admininfo)
        {
            displayorder = DNTRequest.GetFormInt("level", -1);
            if (displayorder < 0 || displayorder > 3)
            {
                //
                AddErrLine("置顶参数只能是0到3之间的值");
                return false;
            }
            // 检查用户所在管理组是否具有置顶的管理权限
            if (admininfo.Admingid != 1)
            {
                if (admininfo.Allowstickthread < displayorder)
                {
                    AddErrLine(string.Format("您没有{0}级置顶的管理权限", displayorder));
                    return false;
                }
            }

            //			if (admininfo.Allowstickthread < 0)
            //			{
            //				AddErrLine("您没有" + displayorder.ToString() + "级置顶的管理权限");
            //				return false;
            //			}

            TopicAdmins.SetTopTopicList(forumid, topiclist, short.Parse(displayorder.ToString()));
            return true;
        }

        /// <summary>
        /// 关闭主题
        /// </summary>
        /// <returns></returns>
        private bool DoCloseOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有关闭主题的权限", usergroupinfo.Grouptitle));
                return false;
            }
            close = DNTRequest.GetFormInt("close", -1);
            if (close == -1)
            {
                //
                AddErrLine("您没有选择操作类型(打开/关闭)");
                return false;
            }

            int reval = TopicAdmins.SetClose(topiclist, short.Parse(close.ToString()));
            if (reval < 1)
            {
                AddErrLine("要(打开/关闭)的主题未找到");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 高亮主题
        /// </summary>
        /// <returns></returns>
        private bool DoHighlightOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有设置高亮的权限", usergroupinfo.Grouptitle));
                return false;
            }
            highlight_color = DNTRequest.GetFormString("highlight_color");
            highlight_style_b = DNTRequest.GetFormString("highlight_style_b");
            highlight_style_i = DNTRequest.GetFormString("highlight_style_i");
            highlight_style_u = DNTRequest.GetFormString("highlight_style_u");

            string highlightStyle = "";
            //<span style="text-decoration:underline;font-style:italic;font-weight:bold;color:#FF0000">abc</span>

            //加粗
            if (!highlight_style_b.Equals(""))
            {
                highlightStyle = highlightStyle + "font-weight:bold;";
            }


            //加斜
            if (!highlight_style_i.Equals(""))
            {
                highlightStyle = highlightStyle + "font-style:italic;";
            }

            //加下划线
            if (!highlight_style_u.Equals(""))
            {
                highlightStyle = highlightStyle + "text-decoration:underline;";
            }

            //设置颜色
            if (!highlight_color.Equals(""))
            {
                highlightStyle = highlightStyle + "color:" + highlight_color + ";";
            }


            //highlight=(highlight_style_b + highlight_style_i + highlight_style_u) *10 + highlight_color;

            if (highlight == -1)
            {
                //
                AddErrLine("您没有选择字体样式及颜色");
                return false;
            }

            TopicAdmins.SetHighlight(topiclist, highlightStyle);
            return true;
        }

        /// <summary>
        /// 修改主题分类
        /// </summary>
        /// <returns></returns>
        private bool DoTypeOperation()
        {
            //			if (!ismoder)
            //			{
            //				AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有分类的权限");
            //				return false;
            //			}
            //	
            //			close = DNTRequest.GetFormInt("close", -1);
            //			if (close == -1)
            //			{
            //				AddErrLine("您没有选择相应的管理操作");
            //				return false;
            //							
            //			}
            //	
            //			TopicAdmins.SetClose(topiclist,short.Parse(close.ToString()));
            //			return true;

            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有分类的权限", usergroupinfo.Grouptitle));
                return false;
            }

            int typeid = DNTRequest.GetFormInt("typeid", 0);
            if (typeid == 0)
            {
                AddErrLine("你没有选择相应的主题分类");
                return false;
            }
            TopicAdmins.ResetTopicTypes(typeid, topiclist);
            return true;
        }

        /// <summary>
        /// 移动主题
        /// </summary>
        /// <returns></returns>
        private bool DoMoveOperation()
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有移动的权限", usergroupinfo.Grouptitle));
                return false;
            }

            moveto = DNTRequest.GetFormInt("moveto", 0);
            type = DNTRequest.GetFormString("type");
            if (moveto == 0 || type.CompareTo("") == 0 || ",normal,redirect,".IndexOf("," + type.Trim() + ",") == -1)
            {
                AddMsgLine(string.Format("{0}  {1}您没有选择目标论坛/分类或移动方式", moveto.ToString(), type));
                return false;
            }

            if (moveto == forumid)
            {
                AddErrLine("主题不能在相同论坛/分类内移动");
                return false;
            }

            ForumInfo movetoinfo = Forums.GetForumInfo(moveto);
            if (movetoinfo == null)
            {
                AddErrLine("目标版块不存在");
                return false;
            }

            if (movetoinfo.Layer == 0)
            {
                AddErrLine("主题只能在版块间移动,你当前选择的是分类");
                return false;
            }


            bool savelink = false;
            if (type.CompareTo("redirect") == 0)
            {
                savelink = true;
            }

            TopicAdmins.MoveTopics(topiclist, moveto, forumid, savelink);
            return true;
        }

        /// <summary>
        /// 删除主题
        /// </summary>
        /// <param name="forum"></param>
        /// <returns></returns>
        private bool DoDeleteOperation(ForumInfo forum)
        {
            if (!ismoder)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有删除的权限", usergroupinfo.Grouptitle));
                return false;
            }
            TopicAdmins.DeleteTopics(topiclist, byte.Parse(forum.Recyclebin.ToString()), DNTRequest.GetInt("reserveattach", 0) == 1);
            Forums.SetRealCurrentTopics(forum.Fid);

            //更新指定版块的最新发帖数信息
            Forums.UpdateLastPost(forum);

            return true;
        }

        private bool DoBumpTopicsOperation()
        {
            if (!Utils.IsNumericArray(topiclist.Split(',')))
            {
                AddErrLine("非法的主题ID");
                return false;
            }
            int bumptype = DNTRequest.GetFormInt("bumptype", 0);
            if (Math.Abs(bumptype) != 1)
            {
                AddErrLine("错误的参数");
                return false;
            }

            TopicAdmins.BumpTopics(topiclist, bumptype);
            return true;
        }

        /// <summary>
        /// 单帖屏蔽
        /// </summary>
        /// <returns></returns>
        private bool DoBanPostOperatopn()
        {
            if (!Utils.IsNumeric(topiclist))
            {
                AddErrLine("无效的主题ID");
                return false;
            }
            TopicInfo topic = Topics.GetTopicInfo(Utils.StrToInt(topiclist, 0));
            if (topic == null)
            {
                AddErrLine("不存在的主题");
                return false;
            }

            if (!Utils.IsNumericArray(postidlist.Split(',')))
            {
                AddErrLine("非法的帖子ID");
                return false;
            }
            int banposttype = DNTRequest.GetFormInt("banpost", -1);

            if (banposttype != -1 && (banposttype == 0 || banposttype == -2))
            {
                Posts.BanPosts(topic.Tid, postidlist, banposttype);
                return true;
            }


            return false;
        }

        /// <summary>
        /// 批量删帖
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="forum"></param>
        /// <returns></returns>
        private bool DoDelpostsOperation(string reason, ForumInfo forum)
        {
            if (!Utils.IsNumeric(topiclist))
            {
                AddErrLine("无效的主题ID");
                return false;
            }
            TopicInfo topic = Topics.GetTopicInfo(Utils.StrToInt(topiclist, 0));
            if (topic == null)
            {
                AddErrLine("不存在的主题");
                return false;
            }

            if (!Utils.IsNumericArray(postidlist.Split(',')))
            {
                AddErrLine("非法的帖子ID");
                return false;
            }
            
            bool flag = false;
            foreach (string postid in postidlist.Split(','))
            {
                PostInfo post = Posts.GetPostInfo(topic.Tid, Utils.StrToInt(postid, 0));
                if (post == null || (post.Layer <= 0 && topic.Replies > 0) || topic.Tid != post.Tid)
                {
                    AddErrLine(string.Format("ID为{0}的帖子因为对应主题无效或者已被回复,所以无法删除", postid));
                    continue;
                }
                int Losslessdel = Utils.StrDateDiffHours(post.Postdatetime, config.Losslessdel * 24);
                // 通过验证的用户可以删除帖子
                if (post.Layer == 0)
                {
                    TopicAdmins.DeleteTopics(topic.Tid.ToString(), byte.Parse(forum.Recyclebin.ToString()), DNTRequest.GetInt("reserveattach", 0) == 1);
                    break;
                }
                else
                {
                    int reval = Posts.DeletePost(Posts.GetPostTableID(topic.Tid), post.Pid, DNTRequest.GetInt("reserveattach", 0) == 1,true);
                    if (reval > 0 && config.Modworkstatus == 1)
                    {
                        string newTitle = post.Message.Replace(" ", string.Empty).Replace("|", string.Empty);
                        if (newTitle.Length > 100)
                            newTitle = newTitle.Substring(0, 100) + "...";
                        newTitle = "(pid:" + postid + ")" + post.Title + "|" + newTitle;
                        AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(),
                                                     usergroupinfo.Grouptitle, Utils.GetRealIP(), Utils.GetDateTime(),
                                                     forumid.ToString(), forumname, topic.Tid.ToString(), newTitle,
                                                     operation, reason);
                    }
                    if (topic.Special == 4)
                    {
                        string opiniontext = "";

                        if (opinion != 1 && opinion != 2)
                        {
                            AddErrLine("参数错误");
                            return false;
                        }
                        switch (opinion)
                        {
                            case 1:
                                opiniontext = "positivediggs";
                                break;
                            case 2:
                                opiniontext = "negativediggs";
                                break;

                        }
                        Discuz.Data.DatabaseProvider.GetInstance().DeleteDebatePost(topic.Tid, opiniontext, Utils.StrToInt(postid,-1));

                    }

                    if (reval > 0 && Losslessdel < 0)
                    {
                        UserCredits.UpdateUserCreditsByPosts(post.Posterid, -1);
                    }
                }
                flag = true;
            }
            //确保回复数精确

            Topics.UpdateTopicReplies(topic.Tid);

            //更新指定版块的最新发帖数信息
            Forums.UpdateLastPost(forum);
            return flag;
        }

        /// <summary>
        /// 鉴定主题
        /// </summary>
        /// <returns></returns>
        private bool DoIndentifyOperation()
        {
            int identify = DNTRequest.GetInt("selectidentify", 0);

            if (identify > 0 || identify == -1)
            {
                TopicAdmins.IdentifyTopic(topiclist, identify);
                return true;
            }
            else
            {
                AddErrLine("请选择签定类型");
                return false;
            }
        }

        /// <summary>
        /// 悬赏结帖
        /// </summary>
        /// <returns></returns>
        private bool DoBonusOperation()
        {
            //身份验证
            int topicid = DNTRequest.GetInt("topicid", 0);
            topicinfo = Topics.GetTopicInfo(topicid);

            if (topicinfo.Special == 3)
            {
                AddErrLine("本主题的悬赏已经结束");
            }

            if (topicinfo.Posterid <= 0)
            {
                AddErrLine("无法结束游客发布的悬赏");
            }

            if (topicinfo.Posterid != userid && !ismoder)//不是作者或管理者
            {
                AddErrLine("您没有权限结束此悬赏");
            }

            int costBonus = 0;
            string[] costBonusArray = DNTRequest.GetString("postbonus").Split(',');

            foreach (string s in costBonusArray)
            {
                costBonus += Utils.StrToInt(s, 0);
            }

            if (costBonus != topicinfo.Price)
            {
                AddErrLine("分数总和与悬赏总分不相符");
            }

            string[] addonsArray = DNTRequest.GetFormString("addons").Split(',');
            int[] winneridArray = new int[addonsArray.Length];
            int[] postidArray = new int[addonsArray.Length];
            string[] winnernameArray = new string[addonsArray.Length];
            int[] isbestArray = new int[addonsArray.Length];

            //string[] winerIdArray = DNTRequest.GetString("posterids").Split(',');

            foreach (string addon in addonsArray)
            {
                int winnerid = Utils.StrToInt(addon.Split('|')[0], 0);
                if (winnerid == topicinfo.Posterid)
                {
                    AddErrLine("不能向悬赏者发放金币奖励");
                    break;
                }
            }

            if (costBonusArray.Length != addonsArray.Length)
            {
                AddErrLine("获奖者数量与金币奖励数量不一致");
            }

            string[] valuableAnswerArray = DNTRequest.GetFormString("valuableAnswers").Split(',');
            int bestAnswer = DNTRequest.GetFormInt("bestAnswer", 0);
            if (IsErr())
            {
                return false;
            }

            for (int i = 0; i < addonsArray.Length; i++)
            {
                winneridArray[i] = Utils.StrToInt(addonsArray[i].Split('|')[0], 0);
                postidArray[i] = Utils.StrToInt(addonsArray[i].Split('|')[1], 0);
                winnernameArray[i] = addonsArray[i].Split('|')[2];
            }

            Bonus.CloseBonus(topicinfo, userid, postidArray, winneridArray, winnernameArray, costBonusArray, valuableAnswerArray, bestAnswer);

            //Bonus.AddLog(topicinfo.Tid, topicinfo.Posterid, winerIdArray, costBonusArray);
            return true;
        }


        #endregion

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:43.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:43. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo;  " + forumnav.ToString() + "  &raquo;  <strong>" + operationtitle.ToString() + "</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (DNTRequest.GetString("operation") == "")
            {


                if (page_err == 0)
                {

                    templateBuilder.Append("<div class=\"mainbox formbox\">\r\n");
                    templateBuilder.Append("<form id=\"moderate\" name=\"moderate\" method=\"post\" action=\"topicadmin.aspx?action=moderate&operation=" + operation.ToString() + "\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"topicid\" value=\"" + topiclist.ToString() + "\" />\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"forumid\" value=\"" + forumid.ToString() + "\" />\r\n");

                    if (config.Aspxrewrite == 1)
                    {

                        templateBuilder.Append("		<input type=\"hidden\" id=\"referer\" name=\"referer\" value=\"showforum-" + forumid.ToString() + "" + config.Extname.ToString().Trim() + "\" />\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("		<input type=\"hidden\" id=\"referer\" name=\"referer\" value=\"showforum.aspx?forumid=" + forumid.ToString() + "\">\r\n");

                    }	//end if

                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	var re = getQueryString(\"referer\");\r\n");
                    templateBuilder.Append("	if (re != \"\")\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		$(\"referer\").value = unescape(re);\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
                    templateBuilder.Append("	<h3>" + operationtitle.ToString() + "</h3>\r\n");
                    templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>用户名</th>\r\n");
                    templateBuilder.Append("				<td>" + username.ToString() + "&nbsp;<a href=\"logout.aspx?userkey=" + userkey.ToString() + "\">退出登录</a></td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                    if (operation == "highlight")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>字体样式</th>\r\n");
                        templateBuilder.Append("				<td><input type=\"checkbox\" name=\"highlight_style_b\" value=\"B\" /> <strong>粗体</strong> <input type=\"checkbox\" name=\"highlight_style_i\" value=\"I\" /> <em>斜体</em><input type=\"checkbox\" name=\"highlight_style_u\" value=\"U\" /> <u>下划线</u>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>字体颜色:</th>\r\n");
                        templateBuilder.Append("				<td><!--colorpicker层显示开始-->						\r\n");
                        templateBuilder.Append("				<script type=\"text/javascript\" src=\"javascript/template_colorpicker.js\"></" + "script>\r\n");
                        templateBuilder.Append("				<input type=\"text\" value=\"\" name=\"highlight_color\" id=\"highlight_color\"  size=\"7\" class=\"colorblue\" onfocus=\"this.className='colorfocus';\" onblur=\"this.className='colorblue';\" />\r\n");
                        templateBuilder.Append("				<select name=\"highlight_colorselect\" id=\"highlight_colorselect\" onChange=\"selectoptioncolor(this)\" style=\"margin-bottom:2px;\">\r\n");
                        templateBuilder.Append("					<option value=\"\">默认</option>  \r\n");
                        templateBuilder.Append("					<option style=\"background:#FF0000\" value=\"#FF0000\"></option>  \r\n");
                        templateBuilder.Append("					<option style=\"background:#FF8000\" value=\"#FF8000\"></option> \r\n");
                        templateBuilder.Append("					<option style=\"background:#00FF00\" value=\"#00FF00\"></option> \r\n");
                        templateBuilder.Append("					<option style=\"background:#0080ff\" value=\"#0080ff\"></option> \r\n");
                        templateBuilder.Append("					<option style=\"background:#0000A0\" value=\"#0000A0\"></option> \r\n");
                        templateBuilder.Append("					<option style=\"background:#8000FF\" value=\"#8000FF\"></option> \r\n");
                        //templateBuilder.Append("					<option style=\"background:#800080\" value=\"#800080\"></option> \r\n");
                        //templateBuilder.Append("					<option style=\"background:#808080\" value=\"#808080\"></option>\r\n");
                        templateBuilder.Append("				</select>\r\n");
                        templateBuilder.Append("				<img class=\"img\" title=\"选择颜色\" src=\"templates/" + templatepath.ToString() + "/images/colorpicker.gif\" id=s_bgcolor onclick=\"IsShowColorPanel(this);\" style=\"cursor:hand; border:0px;\" />\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "displayorder")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"level\">级别</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

                        if (displayorder > 0)
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"0\" name=\"level\" />解除置顶\r\n");

                        }	//end if

                        templateBuilder.Append("				<input name=\"level\" type=\"radio\" value=\"1\"\r\n");

                        if (displayorder <= 1)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /> <input type=\"radio\" value=\"2\" name=\"level\"\r\n");

                        if (displayorder == 2)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" />\r\n");
                        templateBuilder.Append("				<input type=\"radio\" value=\"3\" name=\"level\"\r\n");

                        if (displayorder == 3)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" />\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "digest")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"level\">级别</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

                        if (digest > 0)
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"0\" name=\"level\" />解除精华\r\n");

                        }	//end if

                        templateBuilder.Append("				<input name=\"level\" type=\"radio\" value=\"1\"\r\n");

                        if (digest <= 1)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /> <input type=\"radio\" value=\"2\" name=\"level\"\r\n");

                        if (digest == 2)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" />\r\n");
                        templateBuilder.Append("				<input type=\"radio\" value=\"3\" name=\"level\"\r\n");

                        if (digest == 3)
                        {

                            templateBuilder.Append("				 checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("				 /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" /><img src=\"templates/" + templatepath.ToString() + "/images/star_level1.gif\" width=\"16\" height=\"16\" />\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "move")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"moveto\">目标版块</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<select name=\"moveto\">\r\n");
                        templateBuilder.Append("						" + forumlist.ToString() + "\r\n");
                        templateBuilder.Append("					</select>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"type\">移动方式</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" checked=\"checked\" value=\"normal\" name=\"type\" />\r\n");
                        templateBuilder.Append("				移动主题&nbsp;&nbsp;<input type=\"radio\" value=\"redirect\" name=\"type\" /> 移动主题并在原来的论坛中保留转向\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "close")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"close\">操作</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" checked=\"checked\" value=\"0\" name=\"close\" />\r\n");
                        templateBuilder.Append("				打开主题&nbsp;&nbsp; <input type=\"radio\" value=\"1\" name=\"close\" /> 关闭主题\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "banpost")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"bandpost\">操作</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input id=\"banpost1\" name=\"banpost\" type=\"radio\" value=\"0\" />取消屏蔽\r\n");
                        templateBuilder.Append("					<input id=\"banpost2\" name=\"banpost\" type=\"radio\" value=\"-2\" checked/>屏蔽帖子\r\n");
                        templateBuilder.Append("					<input type=\"hidden\" name=\"postid\" id=\"postid\" value=\"" + postidlist.ToString() + "\" />\r\n");
                        templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("					var status = getQueryString(\"banstatus\");\r\n");
                        templateBuilder.Append("					if (status == \"0\") {\r\n");
                        templateBuilder.Append("						$(\"banpost1\").checked = true;\r\n");
                        templateBuilder.Append("						$(\"banpost2\").checked = false;\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("					else {\r\n");
                        templateBuilder.Append("						$(\"banpost2\").checked = true;\r\n");
                        templateBuilder.Append("						$(\"banpost1\").checked = false;\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("				</" + "script>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "bump")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"bumptype\">操作</lable></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input id=\"bumptype1\" name=\"bumptype\" type=\"radio\" value=\"1\"  checked/>主题提升\r\n");
                        templateBuilder.Append("					&nbsp;&nbsp; \r\n");
                        templateBuilder.Append("					<input id=\"bumptype2\" name=\"bumptype\" type=\"radio\" value=\"-1\"/>主题下沉\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "copy")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"copyto\">目标论坛/分类</label></div>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<select name=\"copyto\">\r\n");
                        templateBuilder.Append("						" + forumlist.ToString() + "\r\n");
                        templateBuilder.Append("					</select>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "split")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"subject\">新主题的标题</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input type=\"text\" id=\"\" name=\"subject\" size=\"45\" value=\"\" />\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "merge")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"othertid\">主题tid</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<INPUT size=\"10\" name=\"othertid\" ID=\"othertid\" />&nbsp;\r\n");

                        if (config.Aspxrewrite == 1)
                        {

                            templateBuilder.Append("					<SPAN class=\"smalltxt\">即将与这个主题合并的主题id,如showtopic-22.aspx，tid 为 22</SPAN>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					<SPAN class=\"smalltxt\">即将与这个主题合并的主题id,如showtopic.aspx?topicid=22，tid 为 22</SPAN>\r\n");

                        }	//end if

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "type")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"typeid\">目标分类</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<select name=\"typeid\" ID=\"typeid\">" + topictypeselectoptions.ToString() + "</select>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "rate")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"postid\">作者</label></th>\r\n");
                        templateBuilder.Append("				<td>" + poster.ToString() + "<INPUT type=\"hidden\" size=\"10\" name=\"postid\" ID=\"postid\" value=\"" + postidlist.ToString() + "\" /></td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>标题:</th>\r\n");
                        templateBuilder.Append("				<td>" + title.ToString() + "</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"score\">评分</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

                        int score__loop__id = 0;
                        foreach (DataRow score in scorelist.Rows)
                        {
                            score__loop__id++;

                            templateBuilder.Append("					<div style=\"padding-left:3px;margin-top:3px;\">\r\n");
                            templateBuilder.Append("					<select name=\"select\" onchange=\"this.form.score" + score["ScoreCode"].ToString().Trim() + ".value=this.value\">\r\n");
                            templateBuilder.Append("					  <option value=\"0\" selected=\"selected\">" + score["ScoreName"].ToString().Trim() + "</option>\r\n");
                            templateBuilder.Append("					  <option value=\"0\">----</option>\r\n");
                            templateBuilder.Append("					  " + score["options"].ToString().Trim() + "\r\n");
                            templateBuilder.Append("					</select>\r\n");
                            templateBuilder.Append("					<input size=\"3\" value=\"0\" name=\"score\" id=\"score" + score["ScoreCode"].ToString().Trim() + "\" />\r\n");
                            templateBuilder.Append("					<input type=\"hidden\" value=\"" + score["ScoreCode"].ToString().Trim() + "\" name=\"extcredits\" /> (今日还能评分 " + score["MaxInDay"].ToString().Trim() + " )\r\n");
                            templateBuilder.Append("					</div>\r\n");

                        }	//end loop

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "cancelrate")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"postid\">作者</label></th>\r\n");
                        templateBuilder.Append("				<td>" + poster.ToString() + "<input type=\"hidden\" size=\"10\" name=\"postid\" value=\"" + postidlist.ToString() + "\" /></td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>标题:</th>\r\n");
                        templateBuilder.Append("				<td>" + title.ToString() + "</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "identify")
                    {

                        templateBuilder.Append("		" + identifyjsarray.ToString() + "\r\n");
                        templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("			function changeindentify(imgid)\r\n");
                        templateBuilder.Append("			{\r\n");
                        templateBuilder.Append("				if (imgid != \"0\" && imgid != \"-1\")\r\n");
                        templateBuilder.Append("				{\r\n");
                        templateBuilder.Append("					$(\"identify_preview\").src = \"images/identify/\" + topicidentify[imgid];\r\n");
                        templateBuilder.Append("					$(\"identify_preview\").style.display = \"\";\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("				else\r\n");
                        templateBuilder.Append("				{\r\n");
                        templateBuilder.Append("					$(\"identify_preview\").style.display = \"none\";\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("			}\r\n");
                        templateBuilder.Append("		</" + "script>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"selectidentify\">鉴定</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<select name=\"selectidentify\" id=\"selectidentify\" onchange=\"changeindentify(this.value)\">\r\n");
                        templateBuilder.Append("						<option value=\"0\" selected=\"selected\">请选择</option>\r\n");
                        templateBuilder.Append("						<option value=\"-1\">* 取消鉴定 *</option>\r\n");

                        int identify__loop__id = 0;
                        foreach (TopicIdentify identify in identifylist)
                        {
                            identify__loop__id++;

                            templateBuilder.Append("						<option value=\"" + identify.Identifyid.ToString().Trim() + "\">" + identify.Name.ToString().Trim() + "</option>						  \r\n");

                        }	//end loop

                        templateBuilder.Append("					</select>		\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>图例预览</th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<img id=\"identify_preview\" style=\"display: none;\" />\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "delposts")
                    {

                        templateBuilder.Append("<input type=\"hidden\" size=\"10\" name=\"postid\" ID=\"postid\" value=\"" + postidlist.ToString() + "\" /><input type=\"hidden\" size=\"10\" name=\"opinion\" ID=\"opinion\" value=\"" + opinion.ToString() + "\" />\r\n");

                    }	//end if


                    if (operation != "identify" && operation != "bonus")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"selectreason\">操作原因:</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<select id=\"selectreason\" name=\"selectreason\" size=\"6\" style=\"width: 8em;height:8em; \" onchange=\"this.form.reason.value=this.value\">\r\n");
                        templateBuilder.Append("                  <option value=\"\">自定义</option>\r\n");
                        templateBuilder.Append("                  <option value=\"\">--------</option>\r\n");
                        templateBuilder.Append("                  <option value=\"广告/SPAM\">广告</option>\r\n");
                        templateBuilder.Append("                  <option value=\"恶意灌水\">恶意灌水</option>\r\n");
                        templateBuilder.Append("                  <option value=\"违规内容\">违规内容</option>\r\n");
                        templateBuilder.Append("                  <option value=\"发错版块\">发错版块</option>\r\n");
                        templateBuilder.Append("                  <option value=\"文不对题\">文不对题</option>\r\n");
                        templateBuilder.Append("                  <option value=\"重复发帖\">重复发帖</option>\r\n");
                        templateBuilder.Append("                  <option value=\"屡教不改\">屡教不改</option>\r\n");
                        templateBuilder.Append("                  <option value=\"已经过期\">已经过期</option>\r\n");
                        templateBuilder.Append("                  <option value=\"\">--------</option>\r\n");
                        templateBuilder.Append("                  <option value=\"我很赞同\">我很赞同</option>\r\n");
                        templateBuilder.Append("                  <option value=\"精品文章\">精品文章</option>\r\n");
                        templateBuilder.Append("                  <option value=\"原创内容\">原创内容</option>\r\n");
                        templateBuilder.Append("				  <option value=\"鼓励分享\">鼓励分享</option>\r\n");
                        templateBuilder.Append("                </select>\r\n");
                        templateBuilder.Append("				<textarea name=\"reason\" style=\"height: 8em; width:20em; margin-bottom:-2px;\" class=\"colorblue\" onkeydown=\"if(this.value.length>=20){ alert('操作原因不能多于20个字符');return false; }\"></textarea>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "split")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"postid\">选择内容</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

                        int post__loop__id = 0;
                        foreach (DataRow post in postlist.Rows)
                        {
                            post__loop__id++;

                            templateBuilder.Append("<input name=\"postid\" type=\"checkbox\" value=\"" + post["pid"].ToString().Trim() + "\" /><strong>" + post["poster"].ToString().Trim() + "</strong><br />\r\n");
                            templateBuilder.Append("						" + post["message"].ToString().Trim() + "<br />\r\n");

                        }	//end loop

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "bonus")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"postbonus\">给分情况</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<div style=\"position: relative;\">\r\n");
                        templateBuilder.Append("					<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("						var reg = /^\\d+$/i;\r\n");
                        templateBuilder.Append("						$('moderate').onsubmit = function (){\r\n");
                        templateBuilder.Append("							if (getCostBonus() != " + topicinfo.Price.ToString().Trim() + ")\r\n");
                        templateBuilder.Append("							{\r\n");
                        templateBuilder.Append("								alert('分数总和与悬赏总分不相符');\r\n");
                        templateBuilder.Append("								return false;\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							return true;\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function getCostBonus()\r\n");
                        templateBuilder.Append("						{\r\n");
                        templateBuilder.Append("							var bonusboxs = document.getElementsByName('postbonus');\r\n");
                        templateBuilder.Append("							var costbonus = 0;\r\n");
                        templateBuilder.Append("							for (var i = 0; i < bonusboxs.length ; i ++ )\r\n");
                        templateBuilder.Append("							{\r\n");
                        templateBuilder.Append("								var bonus = isNaN(parseInt(bonusboxs[i].value)) ? 0 : parseInt(bonusboxs[i].value);\r\n");
                        templateBuilder.Append("								costbonus += bonus;\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							return costbonus;\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function checkInt(obj)\r\n");
                        templateBuilder.Append("						{				\r\n");
                        templateBuilder.Append("							if (!reg.test(obj.value))\r\n");
                        templateBuilder.Append("							{\r\n");
                        templateBuilder.Append("								obj.value = 0;\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function bonushint(obj)\r\n");
                        templateBuilder.Append("						{							\r\n");
                        templateBuilder.Append("							var costbonus = getCostBonus();\r\n");
                        templateBuilder.Append("							var leftbonus = " + topicinfo.Price.ToString().Trim() + " - costbonus;\r\n");
                        templateBuilder.Append("							$('bonus_menu').innerHTML = '总悬赏分: ' + " + topicinfo.Price.ToString().Trim() + " + '<br />当前可用: ' + leftbonus;\r\n");
                        templateBuilder.Append("							$('bonus_menu').style.left = obj.offsetLeft + obj.offsetWidth/2 + 'px';\r\n");
                        templateBuilder.Append("							$('bonus_menu').style.top = obj.offsetTop + obj.offsetHeight + 'px';\r\n");
                        templateBuilder.Append("							$('bonus_menu').style.display = '';\r\n");
                        templateBuilder.Append("							obj.focus();\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function closebonushint(obj)\r\n");
                        templateBuilder.Append("						{\r\n");
                        templateBuilder.Append("							$('bonus_menu').style.display = 'none';\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						var originalColor = '';\r\n");
                        templateBuilder.Append("						var valuableColor = '#cce2f8';\r\n");
                        templateBuilder.Append("						var bestColor = '#ff9d25';\r\n");
                        templateBuilder.Append("						function rgbToColor(forecolor) {\r\n");
                        templateBuilder.Append("							if(forecolor == null) {\r\n");
                        templateBuilder.Append("								forecolor = '';\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							if(!is_moz && !is_opera) {\r\n");
                        templateBuilder.Append("								if (forecolor.indexOf('#') == 0)\r\n");
                        templateBuilder.Append("								{\r\n");
                        templateBuilder.Append("									forecolor = forecolor.replace('#', '0x');	\r\n");
                        templateBuilder.Append("								}\r\n");
                        templateBuilder.Append("								return rgbhexToColor(((forecolor >> 16) & 0xFF).toString(16), ((forecolor >> 8) & 0xFF).toString(16), (forecolor & 0xFF).toString(16));\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							if(forecolor.toLowerCase().indexOf('rgb') == 0) {\r\n");
                        templateBuilder.Append("								var matches = forecolor.match(/^rgb\\s*\\(([0-9]+),\\s*([0-9]+),\\s*([0-9]+)\\)$/);\r\n");
                        templateBuilder.Append("								if(matches) {\r\n");
                        templateBuilder.Append("									return rgbhexToColor((matches[1] & 0xFF).toString(16), (matches[2] & 0xFF).toString(16), (matches[3] & 0xFF).toString(16));\r\n");
                        templateBuilder.Append("								} else {\r\n");
                        templateBuilder.Append("									return rgbToColor(null);\r\n");
                        templateBuilder.Append("								}\r\n");
                        templateBuilder.Append("							} else {\r\n");
                        templateBuilder.Append("								return forecolor;\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function rgbhexToColor(r, g, b) {\r\n");
                        templateBuilder.Append("							var coloroptions = {'#000000' : 'Black', '#a0522d' : 'Sienna', '#556b2f' : 'DarkOliveGreen', '#006400' : 'DarkGreen', '#483d8b' : 'DarkSlateBlue', '#000080' : 'Navy', '#4b0082' : 'Indigo', '#2f4f4f' : 'DarkSlateGray', '#8b0000' : 'DarkRed', '#ff8c00' : 'DarkOrange', '#808000' : 'Olive', '#008000' : 'Green', '#008080' : 'Teal', '#0000ff' : 'Blue', '#708090' : 'SlateGray', '#696969' : 'DimGray', '#ff0000' : 'Red', '#f4a460' : 'SandyBrown', '#9acd32' : 'YellowGreen', '#2e8b57' : 'SeaGreen', '#48d1cc' : 'MediumTurquoise', '#4169e1' : 'RoyalBlue', '#800080' : 'Purple', '#808080' : 'Gray', '#ff00ff' : 'Magenta', '#ffa500' : 'Orange', '#ffff00' : 'Yellow', '#00ff00' : 'Lime', '#00ffff' : 'Cyan', '#00bfff' : 'DeepSkyBlue', '#9932cc' : 'DarkOrchid', '#c0c0c0' : 'Silver', '#ffc0cb' : 'Pink', '#f5deb3' : 'Wheat', '#fffacd' : 'LemonChiffon', '#98fb98' : 'PaleGreen', '#afeeee' : 'PaleTurquoise', '#add8e6' : 'LightBlue', '#dda0dd' : 'Plum', '#ffffff' : 'White'};\r\n");
                        templateBuilder.Append("							var color = '#' + (str_pad(r, 2, 0) + str_pad(g, 2, 0) + str_pad(b, 2, 0));\r\n");
                        templateBuilder.Append("							return coloroptions[color] ? coloroptions[color] : color;\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function str_pad(text, length, padstring) {\r\n");
                        templateBuilder.Append("							text += '';\r\n");
                        templateBuilder.Append("							padstring += '';\r\n");
                        templateBuilder.Append("							if(text.length < length) {\r\n");
                        templateBuilder.Append("								padtext = padstring;\r\n");
                        templateBuilder.Append("								while(padtext.length < (length - text.length)) {\r\n");
                        templateBuilder.Append("									padtext += padstring;\r\n");
                        templateBuilder.Append("								}\r\n");
                        templateBuilder.Append("								text = padtext.substr(0, (length - text.length)) + text;\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							return text;\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						function setValuableOrBestAnswer(obj, pid)\r\n");
                        templateBuilder.Append("						{\r\n");
                        templateBuilder.Append("							switch (rgbToColor(obj.style.backgroundColor))\r\n");
                        templateBuilder.Append("							{\r\n");
                        templateBuilder.Append("								case valuableColor:				\r\n");
                        templateBuilder.Append("									var valuableAnswers = $('valuableAnswers').value.split(',');\r\n");
                        templateBuilder.Append("									$('valuableAnswers').value = '';\r\n");
                        templateBuilder.Append("									for (var i = 0; i < valuableAnswers.length ; i++)\r\n");
                        templateBuilder.Append("									{\r\n");
                        templateBuilder.Append("										if (valuableAnswers[i] != pid && valuableAnswers[i] != '')\r\n");
                        templateBuilder.Append("										{\r\n");
                        templateBuilder.Append("											$('valuableAnswers').value += ',' + valuableAnswers[i];\r\n");
                        templateBuilder.Append("										}\r\n");
                        templateBuilder.Append("									}\r\n");
                        templateBuilder.Append("									var options = document.getElementsByName('answeroption');\r\n");
                        templateBuilder.Append("									for (var i = 0; i < options.length ; i++ )\r\n");
                        templateBuilder.Append("									{\r\n");
                        templateBuilder.Append("										if (options[i].style.backgroundColor == bestColor)\r\n");
                        templateBuilder.Append("										{\r\n");
                        templateBuilder.Append("											options[i].style.backgroundColor = valuableColor;\r\n");
                        templateBuilder.Append("											$('valuableAnswers').value += ',' + $('bestAnswer').value;\r\n");
                        templateBuilder.Append("										}										\r\n");
                        templateBuilder.Append("									}\r\n");
                        templateBuilder.Append("									obj.style.backgroundColor = bestColor;\r\n");
                        templateBuilder.Append("									$('bestAnswer').value = pid;\r\n");
                        templateBuilder.Append("									break;\r\n");
                        templateBuilder.Append("								case bestColor:\r\n");
                        templateBuilder.Append("									obj.style.backgroundColor = originalColor;\r\n");
                        templateBuilder.Append("									$('bestAnswer').value= '';\r\n");
                        templateBuilder.Append("									break;\r\n");
                        templateBuilder.Append("								default:\r\n");
                        templateBuilder.Append("									obj.style.backgroundColor = valuableColor;\r\n");
                        templateBuilder.Append("									if (!in_array(pid, $('valuableAnswers').value.split(',')))\r\n");
                        templateBuilder.Append("									{\r\n");
                        templateBuilder.Append("										$('valuableAnswers').value += ',' + pid;\r\n");
                        templateBuilder.Append("									}\r\n");
                        templateBuilder.Append("									break;\r\n");
                        templateBuilder.Append("							}							\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("					</" + "script>\r\n");
                        templateBuilder.Append("					提示: 每次点击答案可以切换\"最佳答案\"与\"有价值的答案\"的颜色状态.&nbsp;&nbsp;&nbsp;&nbsp;颜色含义:<script type=\"text/javascript\">document.write('<span style=\"padding: 3px; color: #fff;background-color: ' + bestColor + ';\">最佳答案</span><span style=\"margin-left: 3px;padding: 3px; color: #fff;background-color: ' + valuableColor + ';\">有价值的答案</span><br /><br />');</" + "script>\r\n");
                        templateBuilder.Append("					<input type=\"hidden\" id=\"bestAnswer\" name=\"bestAnswer\" value=\"\" />\r\n");
                        templateBuilder.Append("					<input type=\"hidden\" id=\"valuableAnswers\" name=\"valuableAnswers\" value=\"\" />\r\n");

                        int post__loop__id = 0;
                        foreach (DataRow post in postlist.Rows)
                        {
                            post__loop__id++;

                            templateBuilder.Append("					<div name=\"answeroption\" \r\n");

                            if (Utils.StrToInt(post["posterid"].ToString().Trim(), 0) != topicinfo.Posterid)
                            {

                                templateBuilder.Append("onclick=\"setValuableOrBestAnswer(this, " + post["pid"].ToString().Trim() + ");\" style=\"cursor: pointer; width: 100%;\"\r\n");

                            }	//end if

                            templateBuilder.Append(">\r\n");
                            templateBuilder.Append("					<strong>" + post["poster"].ToString().Trim() + "</strong>&nbsp; \r\n");

                            if (Utils.StrToInt(post["posterid"].ToString().Trim(), 0) != topicinfo.Posterid)
                            {

                                templateBuilder.Append("得分: <input name=\"postbonus\" id=\"bonus_" + post["pid"].ToString().Trim() + "\" type=\"text\" value=\"0\" size=\"3\" maxlength=\"9\" onblur=\"checkInt(this);\" onmouseover=\"bonushint(this);\" onmouseout=\"closebonushint(this);\" /><input name=\"addons\" type=\"hidden\" value=\"" + post["posterid"].ToString().Trim() + "|" + post["pid"].ToString().Trim() + "|" + post["poster"].ToString().Trim() + "\" />\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("不能给自己分\r\n");

                            }	//end if

                            templateBuilder.Append("<br />\r\n");
                            templateBuilder.Append("						" + post["message"].ToString().Trim() + "<br />\r\n");
                            templateBuilder.Append("					</div><br />\r\n");

                        }	//end loop

                        templateBuilder.Append("					<div id=\"bonus_menu\" style=\"position: absolute; z-index: 50; background: yellow;\"></div>\r\n");
                        templateBuilder.Append("				</div>\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (operation == "del" || operation == "delposts")
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"reserveattach\">附件</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<input name=\"reserveattach\" type=\"checkbox\" value=\"1\" />保留附件(附件可能正在被相册使用, 如果希望保留, 请选中此选项)				\r\n");
                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (donext == 1)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"next\">后续</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("				<input name=\"next\" type=\"radio\" checked=\"checked\" value=\"\" />无\r\n");

                        if (operation != "highlight")
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"highlight\" name=\"next\" />高亮显示\r\n");

                        }	//end if


                        if (operation != "displayorder")
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"displayorder\" name=\"next\" />置顶/解除置顶\r\n");

                        }	//end if


                        if (operation != "digest")
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"digest\" name=\"next\" />加入/解除精华\r\n");

                        }	//end if


                        if (operation != "close")
                        {

                            templateBuilder.Append("				<input type=\"radio\" value=\"close\" name=\"next\" />打开/关闭主题\r\n");

                        }	//end if

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<td>&nbsp;</td>\r\n");
                    templateBuilder.Append("				<td>\r\n");

                    if (issendmessage)
                    {

                        templateBuilder.Append("				<input type=\"checkbox\" disabled checked=\"checked\"/>\r\n");
                        templateBuilder.Append("				<input name=\"sendmessage\" type=\"hidden\" id=\"sendmessage\" value=\"1\"/>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("				<input name=\"sendmessage\" type=\"checkbox\" id=\"sendmessage\" value=\"1\"/>\r\n");

                    }	//end if

                    templateBuilder.Append("				发短消息通知作者\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>	\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<input type=\"submit\" value=\"提  交\" name=\"modsubmit\"/>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                    if (operation == "cancelrate")
                    {


                        if (ratelogcount > 0)
                        {

                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("			<tr>\r\n");
                            templateBuilder.Append("			<td colspan=\"6\">\r\n");
                            templateBuilder.Append("				<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                            templateBuilder.Append("				<thead>\r\n");
                            templateBuilder.Append("					<tr>\r\n");
                            templateBuilder.Append("						<td colspan=\"6\" align=\"left\">评分日志</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</thead>\r\n");
                            templateBuilder.Append("				<tr>\r\n");
                            templateBuilder.Append("					<td><input name=\"chkall\" type=\"checkbox\"  onclick=\"checkall(this.form, 'ratelogid')\" />删除</td>\r\n");
                            templateBuilder.Append("					<td>用户名</td>\r\n");
                            templateBuilder.Append("					<td>时间</td>\r\n");
                            templateBuilder.Append("					<td>评分单位</td>\r\n");
                            templateBuilder.Append("					<td>评分分值</td>\r\n");
                            templateBuilder.Append("					<td>理由</td>\r\n");
                            templateBuilder.Append("				</tr>\r\n");

                            int rateloginfo__loop__id = 0;
                            foreach (DataRow rateloginfo in ratelog.Rows)
                            {
                                rateloginfo__loop__id++;

                                templateBuilder.Append("				<tr>\r\n");
                                templateBuilder.Append("					<td><input name=\"ratelogid\" type=\"checkbox\"  value=\"" + rateloginfo["id"].ToString().Trim() + "\" /></td>\r\n");
                                templateBuilder.Append("					<td>" + rateloginfo["username"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("					<td>" + rateloginfo["postdatetime"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("					<td>" + rateloginfo["extcreditname"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("					<td>" + rateloginfo["score"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("					<td>" + rateloginfo["reason"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("				</tr>\r\n");

                            }	//end loop

                            templateBuilder.Append("				</table>\r\n");
                            templateBuilder.Append("			</td>\r\n");
                            templateBuilder.Append("			</tr>\r\n");
                            templateBuilder.Append("			</tbody>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</form>\r\n");
                    templateBuilder.Append("</div>\r\n");

                    if (operation == "highlight")
                    {

                        templateBuilder.Append("		<div  id=\"ColorPicker\" title=\"ColorPicker\" style=\"display:none;cursor:crosshair;border: black 1px solid;position: absolute; z-index: 10;background-color: aliceblue; width:250px;background: #FFFFFF;padding: 4px; margin-left:150px;\" onmouseover=\"ShowColorPanel();\">\r\n");
                        templateBuilder.Append("						<table border=\"0\" cellPadding=\"0\" cellSpacing=\"10\" onmouseover=\"ShowColorPanel();\">\r\n");
                        templateBuilder.Append("						<tr>\r\n");
                        templateBuilder.Append("						<td>\r\n");
                        templateBuilder.Append("						<table border=\"0\" cellPadding=\"0\" cellSpacing=\"0\" id=\"ColorTable\" style=\"cursor:crosshair;\"  onmouseover=\"ShowColorPanel();\">\r\n");
                        templateBuilder.Append("						<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("						function wc(r, g, b, n){\r\n");
                        templateBuilder.Append("							r = ((r * 16 + r) * 3 * (15 - n) + 0x80 * n) / 15;\r\n");
                        templateBuilder.Append("							g = ((g * 16 + g) * 3 * (15 - n) + 0x80 * n) / 15;\r\n");
                        templateBuilder.Append("							b = ((b * 16 + b) * 3 * (15 - n) + 0x80 * n) / 15;\r\n");
                        templateBuilder.Append("							document.write('<td BGCOLOR=#' + ToHex(r) + ToHex(g) + ToHex(b) + ' title=\"#' + ToHex(r) + ToHex(g) + ToHex(b) + '\" height=8 width=8 onmouseover=\"ColorTableMouseOver(this)\" onmousedown=\"ColorTableMouseDown(this)\"  onmouseout=\"ColorTableMouseOut(this)\" ></TD>');\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						var cnum = new Array(1, 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 0, 0);\r\n");
                        templateBuilder.Append("						for(i = 0; i < 16; i ++){\r\n");
                        templateBuilder.Append("							document.write('<TR>');\r\n");
                        templateBuilder.Append("							for(j = 0; j < 30; j ++){\r\n");
                        templateBuilder.Append("								n1 = j % 5;\r\n");
                        templateBuilder.Append("								n2 = Math.floor(j / 5) * 3;\r\n");
                        templateBuilder.Append("								n3 = n2 + 3;\r\n");
                        templateBuilder.Append("								wc((cnum[n3] * n1 + cnum[n2] * (5 - n1)),\r\n");
                        templateBuilder.Append("								(cnum[n3 + 1] * n1 + cnum[n2 + 1] * (5 - n1)),\r\n");
                        templateBuilder.Append("								(cnum[n3 + 2] * n1 + cnum[n2 + 2] * (5 - n1)), i);\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							document.writeln('</TR>');\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						</" + "script>\r\n");
                        templateBuilder.Append("						</table></td>\r\n");
                        templateBuilder.Append("						<td>\r\n");
                        templateBuilder.Append("						<table border=\"0\" cellPadding=\"0\" cellSpacing=\"0\" id=\"GrayTable\" style=\"CURSOR: hand;cursor:crosshair;\"  onmouseover=\"ShowColorPanel();\">\r\n");
                        templateBuilder.Append("						<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("						for(i = 255; i >= 0; i -= 8.5)\r\n");
                        templateBuilder.Append("						document.write('<tr BGCOLOR=#' + ToHex(i) + ToHex(i) + ToHex(i) + '><td TITLE=' + Math.floor(i * 16 / 17) + ' height=4 width=20 onmouseover=\"GrayTableMouseOver(this)\" onmousedown=\"GrayTableMouseDown(this)\"  onmouseout=\"GrayTableMouseOut(this)\" ></td></tr>');\r\n");
                        templateBuilder.Append("						</" + "script>\r\n");
                        templateBuilder.Append("						</table></td></tr></table>\r\n");
                        templateBuilder.Append("						<table border=\"0\" cellPadding=\"0\" cellSpacing=\"10\" onmouseover=\"ShowColorPanel();\">\r\n");
                        templateBuilder.Append("						<tr>\r\n");
                        templateBuilder.Append("						<td rowSpan=\"2\">选中色彩\r\n");
                        templateBuilder.Append("						<table border=\"1\" cellPadding=\"0\" cellSpacing=\"0\" height=\"30\" id=\"ShowColor\" width=\"40\" bgcolor=\"\">\r\n");
                        templateBuilder.Append("						<tr>\r\n");
                        templateBuilder.Append("						<td></td></tr></table></td>\r\n");
                        templateBuilder.Append("						<td rowSpan=2>基色: <span id=\"RGB\"></span><br />亮度: <span id=\"GRAY\">120</span><br />代码: <input id=\"SelColor\" size=\"7\" value=\"\" border=\"0\" name=\"SelColor\" /></TD>\r\n");
                        templateBuilder.Append("						<td><input type=\"button\" onclick=\"javascript:ColorPickerOK();\" value=\"确定\" ID=\"ok\"/></td></tr>\r\n");
                        templateBuilder.Append("						<tr>\r\n");
                        templateBuilder.Append("						<td><input type=\"button\" onclick=\"javascript:document.getElementById('highlight_color').value='';document.getElementById('s_bgcolor').style.background='#FFFFFF';HideColorPanel();\" value=\"取消\" ID=\"Button2\" NAME=\"Button2\"/></td></tr></table>\r\n");
                        templateBuilder.Append("</div>\r\n");
                        templateBuilder.Append("						<!--colorpicker层显示结束-->\r\n");

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


            }
            else
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



            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}
