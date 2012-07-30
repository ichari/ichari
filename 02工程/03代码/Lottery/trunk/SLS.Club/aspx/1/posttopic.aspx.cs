using System;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 发表主题页面
    /// </summary>
    public partial class posttopic : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 所属板块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 主题内容
        /// </summary>
        public string message;
        /// <summary>
        /// 是否允许发表主题
        /// </summary>
        public bool allowposttopic;
        /// <summary>
        /// 表情Javascript数组
        /// </summary>
        public string smilies;
        /// <summary>
        /// 主题图标
        /// </summary>
        public string topicicons;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = string.Empty;
        /// <summary>
        /// 编辑器自定义按钮
        /// </summary>
        public string customeditbuttons;
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
        /// 是否允许 [img] 标签
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受发贴灌水限制
        /// </summary>
        public int disablepost;
        /// <summary>
        /// 允许的附件类型和大小数组
        /// </summary>
        public string attachextensions;
        /// <summary>
        /// 允许的附件类型
        /// </summary>
        public string attachextensionsnosize;
        /// <summary>
        /// 今天可上传附件大小
        /// </summary>
        public int attachsize;
        /// <summary>
        /// 金币策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 最高售价
        /// </summary>
        public int maxprice;
        /// <summary>
        /// 所属版块信息
        /// </summary>
        public ForumInfo forum;
        /// <summary>
        /// 主题分类选项字串
        /// </summary>
        public string topictypeselectoptions;
        /// <summary>
        /// 表情列表
        /// </summary>
        public DataTable smilietypes;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach;
        /// <summary>
        /// 交易金币
        /// </summary>
        public int creditstrans;
        /// <summary>
        /// 投票截止时间
        /// </summary>
        public string enddatetime = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
        /// <summary>
        /// 是否允许Html标题
        /// </summary>
        public bool canhtmltitle = false;

        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = string.Empty;

        /// <summary>
        /// 发帖人的个人空间Id
        /// </summary>
        public int spaceid = 0;
        /// <summary>
        /// 本版是否可用Tag
        /// </summary>
        public bool enabletag = false;
        /// <summary>
        /// 发帖的类型，如普通帖、悬赏帖等。。
        /// </summary>
        public string type;
        /// <summary>
        /// 当前登录用户的交易金币值, 仅悬赏帖时有效
        /// </summary>
        public float mycurrenttranscredits;

        /// <summary>
        /// 是否需要登录
        /// </summary>
        public bool needlogin = false;
        #endregion

        protected override void ShowPage()
        {
            #region 临时帐号发帖
            int realuserid = -1;
            string tempusername = DNTRequest.GetString("tempusername");
            if (tempusername != "" && tempusername != username)
            {
                string temppassword = DNTRequest.GetString("temppassword");
                int question = DNTRequest.GetInt("question", 0);
                string answer = DNTRequest.GetString("answer");
                if (config.Passwordmode == 1)
                {
                    if (config.Secques == 1)
                    {
                        realuserid = Discuz.Forum.Users.CheckDvBbsPasswordAndSecques(tempusername, temppassword, question, answer);
                    }
                    else
                    {
                        realuserid = Discuz.Forum.Users.CheckDvBbsPassword(tempusername, temppassword);
                    }
                }
                else
                {
                    if (config.Secques == 1)
                    {
                        realuserid = Discuz.Forum.Users.CheckPasswordAndSecques(tempusername, temppassword, true, question, answer);
                    }
                    else
                    {
                        realuserid = Discuz.Forum.Users.CheckPassword(tempusername, temppassword, true);
                    }
                }
                if (realuserid == -1)
                {
                    AddErrLine("临时帐号登录失败，无法继续发帖。");
                    return;
                }
                else
                {
                    userid = realuserid;
                    username = tempusername;
                    usergroupinfo = UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(userid).Groupid);
                    usergroupid = usergroupinfo.Groupid;
                    useradminid = Discuz.Forum.Users.GetShortUserInfo(userid).Adminid;
                }
            }
            #endregion

            canhtmltitle = config.Htmltitle == 1 && Utils.InArray(usergroupid.ToString(), config.Htmltitleusergroup);
            firstpagesmilies = Caches.GetSmiliesFirstPageCache();
            bool createpoll = false;
            string[] pollitem = { };

            //内容设置为空;  
            message = "";
            //maxprice = usergroupinfo.Maxprice > Scoresets.GetMaxIncPerTopic() ? Scoresets.GetMaxIncPerTopic() : usergroupinfo.Maxprice;
            maxprice = usergroupinfo.Maxprice;

            forumid = DNTRequest.GetInt("forumid", -1);

            allowposttopic = true;

            if (forumid == -1)
            {
                allowposttopic = false;
                AddErrLine("错误的论坛ID");
                forumnav = "";
                return;
            }
            else
            {
                forum = Forums.GetForumInfo(forumid);
                if (forum == null || forum.Layer == 0)
                {
                    allowposttopic = false;
                    AddErrLine("错误的论坛ID");
                    forumnav = "";
                    return;
                }
                forumname = forum.Name;
                pagetitle = Utils.RemoveHtml(forum.Name);
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);
                enabletag = (config.Enabletag & forum.Allowtag) == 1;
                if (forum.Applytopictype == 1)  //启用主题分类
                {
                    topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);
                }
            }

            //得到用户可以上传的文件类型
            StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!usergroupinfo.Attachextensions.Trim().Equals(""))
            {
                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }

            if (!forum.Attachextensions.Equals(""))
            {
                if (sbAttachmentTypeSelect.Length > 0)
                {
                    sbAttachmentTypeSelect.Append(" AND ");
                }
                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(forum.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }
            attachextensions = Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString());
            attachextensionsnosize = Attachments.GetAttachmentTypeString(sbAttachmentTypeSelect.ToString());

            //得到今天允许用户上传的附件总大小(字节)
            int MaxTodaySize = 0;
            if (userid > 0)
            {
                MaxTodaySize = Attachments.GetUploadFileSizeByuserid(userid);		//今天已上传大小
            }
            attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;//今天可上传得大小



            StringBuilder sb = new StringBuilder();
            //sb.Append("var allowhtml=1;\r\n"); //+ allhtml.ToString() + "

            parseurloff = 0;

            smileyoff = 1 - forum.Allowsmilies;
            //sb.Append("var allowsmilies=" + (1-smileyoff).ToString() + ";\r\n");


            bbcodeoff = 1;
            if (forum.Allowbbcode == 1 && usergroupinfo.Allowcusbbcode == 1)
            {
                bbcodeoff = 0;
            }
            //sb.Append("var allowbbcode=" + (1-bbcodeoff).ToString() + ";\r\n");

            usesig = ForumUtils.GetCookie("sigstatus") == "0" ? 0 : 1;

            allowimg = forum.Allowimgcode;
            //sb.Append("var allowimgcode=" + allowimg.ToString() + ";\r\n");



            //AddScript(sb.ToString());


            //　如果当前用户非管理员并且论坛设定了禁止发帖时间段，当前时间如果在其中的一个时间段内，不允许用户发帖
            if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
            {
                string visittime = "";
                if (Scoresets.BetweenTime(config.Postbanperiods, out visittime))
                {
                    AddErrLine("在此时间段( " + visittime + " )内用户不可以发帖");
                    return;
                }
            }

            if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                SetBackLink(base.ShowForumAspxRewrite(forumid, 0));
                return;
            }


            if (!Forums.AllowViewByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (string.IsNullOrEmpty(forum.Viewperm))//当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                        needlogin = true;
                        return;
                    }
                }
                else//当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该版块的权限");
                        needlogin = true;
                        return;
                    }
                }
            }

            if (!Forums.AllowPostByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块发主题权限
            {
                if (forum.Postperm == null || forum.Postperm == string.Empty)//权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有发表主题的权限
                    if (usergroupinfo.Allowpost != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有发表主题的权限");
                        needlogin = true;
                        return;
                    }
                }
                else//权限设置不为空时,根据板块权限判断
                {
                    if (!Forums.AllowPost(forum.Postperm, usergroupid))
                    {
                        AddErrLine("您没有在该版块发表主题的权限");
                        needlogin = true;
                        return;
                    }
                }
            }



            //是否有上传附件的权限
            if (Forums.AllowPostAttachByUserID(forum.Permuserlist, userid))
            {
                canpostattach = true;
            }
            else
            {
                if (forum.Postattachperm == "")
                {
                    if (usergroupinfo.Allowpostattach == 1)
                    {
                        canpostattach = true;
                    }
                }
                else
                {
                    if (Forums.AllowPostAttach(forum.Postattachperm, usergroupid))
                    {
                        canpostattach = true;
                    }
                }
            }

            ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(userid);

            // 如果是受灌水限制用户, 则判断是否是灌水
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
            disablepost = 0;
            if (admininfo != null)
            {
                disablepost = admininfo.Disablepostctrl;
            }
            if (admininfo == null || admininfo.Disablepostctrl != 1)
            {

                int Interval = Utils.StrDateDiffSeconds(lastposttime, config.Postinterval);
                if (Interval < 0)
                {
                    AddErrLine("系统规定发帖间隔为" + config.Postinterval.ToString() + "秒, 您还需要等待 " + (Interval * -1).ToString() + " 秒");
                    return;
                }
                else if (userid != -1)
                {
                    string joindate = Discuz.Forum.Users.GetUserJoinDate(userid);
                    if (joindate == "")
                    {
                        AddErrLine("您的用户资料出现错误");
                        return;
                    }

                    Interval = Utils.StrDateDiffMinutes(joindate, config.Newbiespan);
                    if (Interval < 0)
                    {
                        AddErrLine("系统规定新注册用户必须要在" + config.Newbiespan.ToString() + "分钟后才可以发帖, 您还需要等待 " + (Interval * -1).ToString() + " 分");
                        return;
                    }

                }
            }


            creditstrans = Scoresets.GetCreditsTrans();
            userextcreditsinfo = Scoresets.GetScoreSet(creditstrans);

            //message = ForumUtils.GetCookie("postmessage");
            if (userid > 0)
            {
                spaceid = Discuz.Forum.Users.GetShortUserInfo(userid).Spaceid;
            }

            type = DNTRequest.GetString("type").ToLower();

            //int specialpost = 0;
            if (forum.Allowspecialonly > 0 && Utils.StrIsNullOrEmpty(type))
            {
                AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表普通主题", forum.Name));
                return;
            }

            if (forum.Allowpostspecial > 0)
            {
                if (type == "poll" && (forum.Allowpostspecial & 1) != 1)
                {
                    AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表投票", forum.Name));
                    return;
                }

                if (type == "bonus" && (forum.Allowpostspecial & 4) != 4)
                {
                    AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表悬赏", forum.Name));
                    return;
                }
                if (type == "debate" && (forum.Allowpostspecial & 16) != 16)
                {
                    AddErrLine(string.Format("当前版块 \"{0}\" 不允许发表辩论", forum.Name));
                    return;
                }
            }

            // 验证用户是否有发布投票的权限
            if (type == "poll" && usergroupinfo.Allowpostpoll != 1)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", usergroupinfo.Grouptitle));
                needlogin = true;
                return;
            }

            // 验证用户是否有发布悬赏的权限
            if (type == "bonus" && usergroupinfo.Allowbonus != 1)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发布悬赏的权限", usergroupinfo.Grouptitle));
                needlogin = true;
                return;
            }

            // 验证用户是否有发起辩论的权限
            if (type == "debate" && usergroupinfo.Allowdebate != 1)
            {
                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", usergroupinfo.Grouptitle));
                needlogin = true;
                return;
            }

            if (type == "bonus")
            {
                //当“交易金币设置”有效时(1-8的整数):
                int creditTrans = Scoresets.GetCreditsTrans();
                if (creditTrans <= 0)
                {
                    AddErrLine(string.Format("系统未设置\"交易金币设置\", 无法判断当前要使用的(扩展)金币字段, 暂时无法发布悬赏", usergroupinfo.Grouptitle));
                    return;
                }
                mycurrenttranscredits = Discuz.Forum.Users.GetUserExtCredits(userid, creditTrans);
            }


            //如果不是提交...
            if (!ispost)
            {
                AddLinkCss("/templates/" + templatepath + "/editor.css", "css");

                smilies = Caches.GetSmiliesCache();
                smilietypes = Caches.GetSmilieTypesCache();
                customeditbuttons = Caches.GetCustomEditButtonList();
                topicicons = Caches.GetTopicIconsCache();
            }
            else
            {
                SetBackLink(string.Format("posttopic.aspx?forumid={0}&restore=1&type={1}", forumid, type));

                string postmessage = DNTRequest.GetString("message");
                postmessage = postmessage.Replace(Shove._Web.Utility.GetUrl(), Discuz.Common.XmlConfig.GetCpsClubUrl().ToString());
                ForumUtils.WriteCookie("postmessage", postmessage);

                message = postmessage;

                #region 常规项验证

                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }


                if (forum.Applytopictype == 1 && forum.Postbytopictype == 1 && topictypeselectoptions != string.Empty)
                {
                    if (DNTRequest.GetString("typeid").Trim().Equals(""))
                    {
                        AddErrLine("主题类型不能为空");
                    }
                    //检测所选主题分类是否有效
                    if (!Forums.IsCurrentForumTopicType(DNTRequest.GetString("typeid").Trim(), forum.Topictypes))
                    {
                        AddErrLine("错误的主题类型");
                    }
                }
                if (DNTRequest.GetString("title").Trim().Equals(""))
                {
                    AddErrLine("标题不能为空");
                }
                else if (DNTRequest.GetString("title").IndexOf("　") != -1)
                {
                    AddErrLine("标题不能包含全角空格符");
                }
                else if (DNTRequest.GetString("title").Length > 60)
                {
                    AddErrLine("标题最大长度为60个字符,当前为 " + DNTRequest.GetString("title").Length.ToString() + " 个字符");
                }

                if (postmessage.Equals(""))
                {
                    AddErrLine("内容不能为空");
                }

                if (admininfo != null && admininfo.Disablepostctrl != 1)
                {
                    if (postmessage.Length < config.Minpostsize)
                    {
                        AddErrLine("您发表的内容过少, 系统设置要求帖子内容不得少于 " + config.Minpostsize.ToString() + " 字多于 " + config.Maxpostsize.ToString() + " 字");
                    }
                    else if (postmessage.Length > config.Maxpostsize)
                    {
                        AddErrLine("您发表的内容过多, 系统设置要求帖子内容不得少于 " + config.Minpostsize.ToString() + " 字多于 " + config.Maxpostsize.ToString() + " 字");
                    }
                }



                //新用户广告强力屏蔽检查

                if ((config.Disablepostad == 1) && useradminid < 1 || userid == -1)  //如果开启新用户广告强力屏蔽检查或是游客
                {
                    if (userid == -1 || (config.Disablepostadpostcount != 0 && user.Posts <= config.Disablepostadpostcount) ||
                        (config.Disablepostadregminute != 0 && DateTime.Now.AddMinutes(-config.Disablepostadregminute) <= Convert.ToDateTime(user.Joindate)))
                    {
                        foreach (string regular in config.Disablepostadregular.Replace("\r", "").Split('\n'))
                        {
                            if (Posts.IsAD(regular, DNTRequest.GetString("title"), postmessage))
                            {
                                AddErrLine("发帖失败，内容中似乎有广告信息，请检查标题和内容，如有疑问请与管理员联系");
                                return;
                            }
                        }
                    }
                }


                if (IsErr())
                {
                    return;
                }



                // 如果用户上传了附件,则检测用户是否有上传附件的权限
                if (ForumUtils.IsPostFile())
                {
                    if (Attachments.GetAttachmentTypeArray(sbAttachmentTypeSelect.ToString()).Trim() == "")
                    {
                        AddErrLine("系统不允许上传附件");
                    }

                    if (!Forums.AllowPostAttachByUserID(forum.Permuserlist, userid))
                    {
                        if (!Forums.AllowPostAttach(forum.Postattachperm, usergroupid))
                        {
                            AddErrLine("您没有在该版块上传附件的权限");
                        }
                        else if (usergroupinfo.Allowpostattach != 1)
                        {
                            AddErrLine(string.Format("您当前的身份 \"{0}\" 没有上传附件的权限", usergroupinfo.Grouptitle));
                        }
                    }
                }

                #endregion

                #region 投票验证
                if (!DNTRequest.GetString("createpoll").Equals(""))
                {
                    // 验证用户是否有发布投票的权限
                    if (usergroupinfo.Allowpostpoll != 1)
                    {
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发布投票的权限", usergroupinfo.Grouptitle));
                        return;
                    }


                    createpoll = true;
                    pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
                    if (pollitem.Length < 2)
                    {
                        AddErrLine("投票项不得少于2个");
                    }
                    else if (pollitem.Length > config.Maxpolloptions)
                    {
                        AddErrLine(string.Format("系统设置为投票项不得多于{0}个", config.Maxpolloptions));
                    }
                    else
                    {
                        for (int i = 0; i < pollitem.Length; i++)
                        {
                            if (pollitem[i].Trim().Equals(""))
                            {
                                AddErrLine("投票项不能为空");
                            }
                        }
                    }

                    enddatetime = DNTRequest.GetString("enddatetime");
                    if (!Utils.IsDateString(enddatetime))
                    {
                        AddErrLine("投票结束日期格式错误");
                    }
                }
                #endregion

                bool isbonus = type == "bonus";

                #region 悬赏/售价验证

                int topicprice = 0;
                string tmpprice = DNTRequest.GetString("topicprice");

                if (Regex.IsMatch(tmpprice, "^[0-9]*[0-9][0-9]*$") || tmpprice == string.Empty)
                {
                    if (!isbonus)
                    {
                        topicprice = Utils.StrToInt(tmpprice, 0);

                        if (topicprice > maxprice && maxprice > 0)
                        {
                            if (userextcreditsinfo.Unit.Equals(""))
                            {
                                AddErrLine(string.Format("主题售价不能高于 {0} {1}", maxprice.ToString(), userextcreditsinfo.Name));
                            }
                            else
                            {
                                AddErrLine(string.Format("主题售价不能高于 {0} {1}({2})", maxprice.ToString(), userextcreditsinfo.Name, userextcreditsinfo.Unit));
                            }
                        }
                        else if (topicprice > 0 && maxprice <= 0)
                        {
                            AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许出售主题", usergroupinfo.Grouptitle));
                        }
                        else if (topicprice < 0)
                        {
                            AddErrLine("主题售价不能为负数");
                        }
                    }
                    else
                    {
                        topicprice = Utils.StrToInt(tmpprice, 0);

                        if (usergroupinfo.Allowbonus == 0)
                        {
                            AddErrLine(string.Format("您当前的身份 \"{0}\" 未被允许进行悬赏", usergroupinfo.Grouptitle));
                        }

                        if (topicprice < usergroupinfo.Minbonusprice || topicprice > usergroupinfo.Maxbonusprice)
                        {
                            AddErrLine(string.Format("悬赏价格超出范围, 您应在 {0} - {1} {2}{3} 范围内进行悬赏", usergroupinfo.Minbonusprice, usergroupinfo.Maxbonusprice,
                                userextcreditsinfo.Unit, userextcreditsinfo.Name));
                        }
                    }
                }
                else
                {
                    if (!isbonus)
                    {
                        AddErrLine("主题售价只能为整数");
                    }
                    else
                    {
                        AddErrLine("悬赏价格只能为整数");
                    }
                }
                #endregion

                string positiveopinion = DNTRequest.GetString("positiveopinion");
                string negativeopinion = DNTRequest.GetString("negativeopinion");
                string terminaltime = DNTRequest.GetString("terminaltime");

                if (type == "debate")
                {
                    if (usergroupinfo.Allowdebate != 1)
                    {
                        AddErrLine(string.Format("您当前的身份 \"{0}\" 没有发起辩论的权限", usergroupinfo.Grouptitle));
                        return;

                    }

                    if (positiveopinion == string.Empty)
                    {
                        AddErrLine("正方观点不能为空");
                    }
                    if (negativeopinion == string.Empty)
                    {
                        AddErrLine("反方观点不能为空");
                    }
                    if (!Utils.IsDateString(terminaltime))
                    {
                        AddErrLine("结束日期格式不正确");
                    }

                }

                if (IsErr())
                {
                    return;
                }


                int iconid = DNTRequest.GetInt("iconid", 0);
                if (iconid > 15 || iconid < 0)
                {
                    iconid = 0;
                }
                int hide = 1;
                //if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                //{
                //    hide = 1;
                //}

                string curdatetime = Utils.GetDateTime();

                TopicInfo topicinfo = new TopicInfo();
                topicinfo.Fid = forumid;
                topicinfo.Iconid = iconid;
                if (useradminid == 1)
                {
                    topicinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                    //message = Utils.HtmlEncode(postmessage);
                }
                else
                {
                    topicinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
                    //message = Utils.HtmlEncode(ForumUtils.BanWordFilter(postmessage));
                }


                if (ForumUtils.HasBannedWord(topicinfo.Title) || ForumUtils.HasBannedWord(message))
                {
                    AddErrLine("对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!");
                    return;
                }

                topicinfo.Typeid = DNTRequest.GetInt("typeid", 0);
                if (usergroupinfo.Allowsetreadperm == 1)
                {
                    int topicreadperm = DNTRequest.GetInt("topicreadperm", 0);
                    topicreadperm = topicreadperm > 255 ? 255 : topicreadperm;
                    topicinfo.Readperm = topicreadperm;
                }
                else
                {
                    topicinfo.Readperm = 0;
                }
                topicinfo.Price = topicprice;
                topicinfo.Poster = username;
                topicinfo.Posterid = userid;
                topicinfo.Postdatetime = curdatetime;
                topicinfo.Lastpost = curdatetime;
                topicinfo.Lastposter = username;
                topicinfo.Views = 0;
                topicinfo.Replies = 0;

                if (forum.Modnewposts == 1 && useradminid != 1)
                {
                    if (useradminid > 1)
                    {
                        if (disablepost == 1)
                        {
                            topicinfo.Displayorder = 0;
                        }
                        else
                        {
                            topicinfo.Displayorder = -2;
                        }
                    }
                    else
                    {
                        topicinfo.Displayorder = -2;
                    }
                }
                else
                {
                    topicinfo.Displayorder = 0;
                }

                if (useradminid != 1)
                {
                    if (Scoresets.BetweenTime(config.Postmodperiods) || ForumUtils.HasAuditWord(topicinfo.Title) || ForumUtils.HasAuditWord(message))
                    {
                        topicinfo.Displayorder = -2;
                    }
                }


                topicinfo.Highlight = "";
                topicinfo.Digest = 0;
                topicinfo.Rate = 0;
                topicinfo.Hide = hide;
                //topicinfo.Poll = 0;
                topicinfo.Attachment = 0;
                topicinfo.Moderated = 0;
                topicinfo.Closed = 0;

                string htmltitle = DNTRequest.GetString("htmltitle").Trim();
                if (htmltitle != string.Empty && Utils.HtmlDecode(htmltitle).Trim() != topicinfo.Title)
                {
                    topicinfo.Magic = 11000;
                    //按照  附加位/htmltitle(1位)/magic(3位)/以后扩展（未知位数） 的方式来存储
                    //例： 11001
                }

                //标签(Tag)操作                
                string tags = DNTRequest.GetString("tags").Trim();
                string[] tagArray = null;
                if (enabletag && tags != string.Empty)
                {
                    tagArray = Utils.SplitString(tags, " ", true, 2, 10);
                    if (tagArray.Length > 0 && tagArray.Length <= 5)
                    {
                        if (topicinfo.Magic == 0)
                        {
                            topicinfo.Magic = 10000;
                        }
                        topicinfo.Magic = Utils.StrToInt(topicinfo.Magic.ToString() + "1", 0);
                    }
                    else
                    {

                        AddErrLine("超过标签数的最大限制，最多可填写 5 个标签");
                        return;
                    }
                }

                if (isbonus)
                {
                    topicinfo.Special = 2;

                    //检查金币是否足够
                    if (mycurrenttranscredits < topicprice)
                    {
                        AddErrLine("您的金币不足, 无法进行悬赏");
                        return;
                    }
                    else
                    {
                        Discuz.Forum.Users.UpdateUserExtCredits(topicinfo.Posterid, Scoresets.GetCreditsTrans(), -topicprice);
                    }
                }

                if (type == "poll")
                {
                    topicinfo.Special = 1;
                }
                //辩论帖
                if (type == "debate")
                {
                    topicinfo.Special = 4;
                }

                int topicid = Topics.CreateTopic(topicinfo);
                //保存htmltitle
                if (canhtmltitle && htmltitle != string.Empty && htmltitle != topicinfo.Title)
                {
                    Topics.WriteHtmlTitleFile(htmltitle, topicid);
                }

                if (enabletag && tagArray != null && tagArray.Length > 0)
                {
                    if (ForumUtils.HasBannedWord(tags))
                    {
                        AddErrLine("标签中含有系统禁止词语,请修改");
                        return;
                    }

                    ForumTags.CreateTopicTags(tagArray, topicid, userid, curdatetime);
                }

                if (type == "debate")
                {
                    DebateInfo debatetopic = new DebateInfo();
                    debatetopic.Tid = topicid;
                    debatetopic.Positiveopinion = positiveopinion;
                    debatetopic.Negativeopinion = negativeopinion;
                    //debatetopic.Positivecolor = DNTRequest.GetString("positivecolor");
                    //debatetopic.Negativecolor = DNTRequest.GetString("negativecolor");
                    debatetopic.Terminaltime = Convert.ToDateTime(DNTRequest.GetString("terminaltime"));
                    Topics.AddDebateTopic(debatetopic);
                }

                PostInfo postinfo = new PostInfo();
                postinfo.Fid = forumid;
                postinfo.Tid = topicid;
                postinfo.Parentid = 0;
                postinfo.Layer = 0;
                postinfo.Poster = username;
                postinfo.Posterid = userid;
                if (useradminid == 1)
                {
                    postinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                }
                else
                {
                    postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
                }

                postinfo.Postdatetime = curdatetime;
                postinfo.Message = message.Replace("<hide>","[hide]").Replace("</hide>","[/hide]");
                postinfo.Ip = DNTRequest.GetIP();
                postinfo.Lastedit = "";

                if (ForumUtils.HasAuditWord(postinfo.Message))
                {
                    postinfo.Invisible = 1;
                }

                if (forum.Modnewposts == 1 && useradminid != 1)
                {
                    if (useradminid > 1)
                    {
                        if (disablepost == 1)
                        {
                            postinfo.Invisible = 0;
                        }
                        else
                        {
                            postinfo.Invisible = 1;
                        }
                    }
                    else
                    {
                        postinfo.Invisible = 1;
                    }
                }
                else
                {
                    postinfo.Invisible = 0;
                }
                //　如果当前用户非管理员并且论坛设定了发帖审核时间段，当前时间如果在其中的一个时间段内，则用户所发帖均为待审核状态
                if (useradminid != 1)
                {
                    if (Scoresets.BetweenTime(config.Postmodperiods))
                    {
                        postinfo.Invisible = 0;
                    }
                }



                postinfo.Usesig = Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
                postinfo.Htmlon = 1;

                postinfo.Smileyoff = smileyoff;
                if (smileyoff == 0 && forum.Allowsmilies == 1)
                {
                    postinfo.Smileyoff = Utils.StrToInt(DNTRequest.GetString("smileyoff"), 0);
                }

                postinfo.Bbcodeoff = 1;
                if (usergroupinfo.Allowcusbbcode == 1 && forum.Allowbbcode == 1)
                {
                    postinfo.Bbcodeoff = Utils.StrToInt(DNTRequest.GetString("bbcodeoff"), 0);
                }
                postinfo.Parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);
                postinfo.Attachment = 0;
                postinfo.Rate = 0;
                postinfo.Ratetimes = 0;
                postinfo.Topictitle = topicinfo.Title;

                int postid = 0;

                try
                {
                    postid = Posts.CreatePost(postinfo);
                }
                catch
                {
                    TopicAdmins.DeleteTopics(topicid.ToString(), false);
                    AddErrLine("帖子保存出现异常");
                    return;
                }

                Topics.AddParentForumTopics(forum.Parentidlist.Trim(), 1, 1);


                //设置用户的金币
                ///首先读取版块内自定义金币
                ///版设置了自定义金币则使用，否则使用论坛默认金币
                float[] values = null;
                if (!forum.Postcredits.Equals(""))
                {
                    int index = 0;
                    float tempval = 0;
                    values = new float[8];
                    foreach (string ext in Utils.SplitString(forum.Postcredits, ","))
                    {

                        if (index == 0)
                        {
                            if (!ext.Equals("True"))
                            {
                                values = null;
                                break;
                            }
                            index++;
                            continue;
                        }
                        tempval = Utils.StrToFloat(ext, 0);
                        values[index - 1] = tempval;
                        index++;
                        if (index > 8)
                        {
                            break;
                        }
                    }
                }

                //if (values != null)
                //{
                //    ///使用版块内金币
                //    UserCredits.UpdateUserCreditsByPostTopic(userid, values);
                //}
                //else
                //{
                //    ///使用默认金币
                //    UserCredits.UpdateUserCreditsByPostTopic(userid);
                //}

                StringBuilder itemvaluelist = new StringBuilder("");
                if (createpoll)
                {
                    // 生成以回车换行符为分割的项目与结果列
                    for (int i = 0; i < pollitem.Length; i++)
                    {
                        itemvaluelist.Append("0\r\n");
                    }

                    string PollItemname = Utils.HtmlEncode(DNTRequest.GetFormString("PollItemname"));
                    if (PollItemname != "")
                    {
                        int multiple = DNTRequest.GetString("multiple") == "on" ? 1 : 0;
                        int maxchoices = 0;
                        if (multiple <= 0)
                        {
                            multiple = 0;
                        }

                        if (multiple == 1)
                        {
                            maxchoices = DNTRequest.GetInt("maxchoices", 1);
                            if (maxchoices > pollitem.Length)
                            {
                                maxchoices = pollitem.Length;
                            }
                        }

                        if (!Polls.CreatePoll(topicid, multiple, pollitem.Length, PollItemname.Trim(), itemvaluelist.ToString().Trim(), enddatetime, userid, maxchoices, DNTRequest.GetString("visiblepoll") == "on" ? 1 : 0))
                        {
                            AddErrLine("投票错误");
                            return;
                        }
                    }
                    else
                    {
                        AddErrLine("投票项为空");
                        return;
                    }
                }


                sb = new StringBuilder();
                sb.Remove(0, sb.Length);

                int watermarkstatus = config.Watermarkstatus;
                if (forum.Disablewatermark == 1)
                {
                    watermarkstatus = 0;
                }
                AttachmentInfo[] attachmentinfo = ForumUtils.SaveRequestFiles(forumid, config.Maxattachments, usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize, attachextensions, watermarkstatus, config, "postfile");
                if (attachmentinfo != null)
                {
                    if (attachmentinfo.Length > config.Maxattachments)
                    {
                        AddErrLine("系统设置为每个帖子附件不得多于" + config.Maxattachments + "个");
                        return;
                    }
                    int errorAttachment = Attachments.BindAttachment(attachmentinfo, postid, sb, topicid, userid);
                    int[] aid = Attachments.CreateAttachments(attachmentinfo);
                    string tempMessage = Attachments.FilterLocalTags(aid, attachmentinfo, postinfo.Message);

                    if (!tempMessage.Equals(postinfo.Message))
                    {
                        postinfo.Message = tempMessage;
                        postinfo.Pid = postid;
                        Posts.UpdatePost(postinfo);
                    }

                    UserCredits.UpdateUserCreditsByUploadAttachment(userid, aid.Length - errorAttachment);
                }

                OnlineUsers.UpdateAction(olid, UserAction.PostTopic.ActionID, forumid, forumname, -1, "", config.Onlinetimeout);
                // 更新在线表中的用户最后发帖时间
                OnlineUsers.UpdatePostTime(olid);

                if (sb.Length > 0)
                {
                    SetUrl(base.ShowTopicAspxRewrite(topicid, 0));
                    SetMetaRefresh(5);
                    SetShowBackLink(true);
                    sb.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>发表主题成功,但以下附件上传失败:</nobr></span><br /></td></tr>");
                    sb.Append("</table>");
                    AddMsgLine(sb.ToString());
                }
                else
                {

                    SetShowBackLink(false);
                    if (useradminid != 1)
                    {
                        bool needaudit = false; //是否需要审核

                        if (Scoresets.BetweenTime(config.Postmodperiods))
                        {
                            needaudit = true;
                        }
                        else
                        {
                            if (forum.Modnewposts == 1 && useradminid != 1)
                            {
                                if (useradminid > 1)
                                {
                                    if (disablepost == 1 && topicinfo.Displayorder != -2)
                                    {
                                        if (useradminid == 3 && !Moderators.IsModer(useradminid, userid, forumid))
                                        {
                                            needaudit = true;
                                        }
                                        else
                                        {
                                            needaudit = false;
                                        }
                                    }
                                    else
                                    {
                                        needaudit = true;
                                    }
                                }
                                else
                                {
                                    needaudit = true;
                                }
                            }
                            else
                            {
                                if (useradminid != 1 && topicinfo.Displayorder == -2)
                                {
                                    needaudit = true;
                                }
                            }
                        }
                        if (needaudit)
                        {
                            SetUrl(base.ShowForumAspxRewrite(forumid, 0));
                            SetMetaRefresh();
                            AddMsgLine("发表主题成功, 但需要经过审核才可以显示. 返回该版块");
                        }
                        else
                        {
                            PostTopicSucceed(values, topicinfo, topicid);
                        }
                    }
                    else
                    {
                        PostTopicSucceed(values, topicinfo, topicid);
                    }
                }
                ForumUtils.WriteCookie("postmessage", "");


                //如果已登录就不需要再登录
                if (needlogin && userid > 0)
                    needlogin = false;
            }
        }

        /// <summary>
        /// 发帖成功
        /// </summary>
        /// <param name="values">版块金币设置</param>
        /// <param name="topicinfo">主题信息</param>
        /// <param name="topicid">主题ID</param>
        private void PostTopicSucceed(float[] values, TopicInfo topicinfo, int topicid)
        {
            if (values != null)
            {
                ///使用版块内金币
                UserCredits.UpdateUserCreditsByPostTopic(userid, values);
            }
            else
            {
                ///使用默认金币
                UserCredits.UpdateUserCreditsByPostTopic(userid);
            }
            SetUrl(topicinfo.Special == 4 ? ShowDebateAspxRewrite(topicid) : ShowTopicAspxRewrite(topicid, 0));
            SetMetaRefresh();
            AddMsgLine("发表主题成功, 返回该主题<br />(<a href=\"" + base.ShowForumAspxRewrite(forumid, 0) + "\">点击这里返回 " + forumname + "</a>)<br />");
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:08.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:08. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("var postminchars = parseInt('" + config.Minpostsize.ToString().Trim() + "');\r\n");
            templateBuilder.Append("var postmaxchars = parseInt('" + config.Maxpostsize.ToString().Trim() + "');\r\n");
            templateBuilder.Append("var disablepostctrl = parseInt('" + disablepost.ToString() + "');\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + "  &raquo; \r\n");

            if (type == "bonus")
            {

                templateBuilder.Append("				发表新悬赏\r\n");

            }
            else if (type == "poll")
            {

                templateBuilder.Append("				发表新投票\r\n");

            }
            else if (type == "debate")
            {

                templateBuilder.Append("				发起新辩论\r\n");

            }
            else
            {

                templateBuilder.Append("				发表新主题\r\n");

            }	//end if

            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

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


                    templateBuilder.Append("		<script type=\"text/javascript\">setcookie(\"dnt_title\", '', 1);</" + "script>\r\n");

                }
                else
                {

                    templateBuilder.Append("	<div class=\"mainbox viewthread\" id=\"previewtable\" style=\"display: none\">\r\n");
                    templateBuilder.Append("			<h1>预览帖子</h1>\r\n");
                    templateBuilder.Append("			<table summary=\"预览帖子\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<td class=\"postauthor\">" + username.ToString() + "</td>\r\n");
                    templateBuilder.Append("					<td class=\"postcontent\">\r\n");
                    templateBuilder.Append("						<span class=\"fontfamily\">" + nowdatetime.ToString() + "</span>\r\n");
                    templateBuilder.Append("						<span id=\"previewmessage\"></span>\r\n");
                    templateBuilder.Append("					</td>\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	<form method=\"post\" name=\"postform\" id=\"postform\" action=\"\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this);\">\r\n");
                    templateBuilder.Append("	<div class=\"mainbox formbox\">\r\n");
                    templateBuilder.Append("		<h1>\r\n");

                    if (type == "bonus")
                    {

                        templateBuilder.Append("			发表新悬赏\r\n");

                    }
                    else if (type == "poll")
                    {

                        templateBuilder.Append("			发表新投票\r\n");

                    }
                    else if (type == "debate")
                    {

                        templateBuilder.Append("			发起新辩论\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("			发表新主题\r\n");

                    }	//end if

                    templateBuilder.Append("		</h1>\r\n");
                    templateBuilder.Append("		<table summary=\"post\" cellspacing=\"0\" cellpadding=\"0\" id=\"newpost\">\r\n");

                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("var tempaccounts = false;\r\n");
                    templateBuilder.Append("function showusername()\r\n");
                    templateBuilder.Append("{\r\n");
                    templateBuilder.Append("    $(\"usernamelayer\").innerHTML = \"<input name='tempusername' type='text' id='tempusername' size='20' class='colorblue' onfocus=\\\"this.className='colorfocus';\\\" onblur=\\\"this.className='colorblue';\\\" value='" + username.ToString() + "' onkeyup=\\\"$('passwordlayer').style.display='';\\\">&nbsp;[<a href='javascript:;' onclick='resetusername()'>恢复</a>]\";\r\n");
                    templateBuilder.Append("    tempaccounts = true;\r\n");
                    templateBuilder.Append("    var i = 1;\r\n");
                    templateBuilder.Append("    while(true)\r\n");
                    templateBuilder.Append("    {\r\n");
                    templateBuilder.Append("        var obj = $(\"albums\" + i);\r\n");
                    templateBuilder.Append("        if(obj == null) break;\r\n");
                    templateBuilder.Append("        obj.options[0].selected = true;\r\n");
                    templateBuilder.Append("        obj.disabled = true\r\n");
                    templateBuilder.Append("        i++;\r\n");
                    templateBuilder.Append("    }\r\n");
                    templateBuilder.Append("}\r\n");
                    templateBuilder.Append("function resetusername()\r\n");
                    templateBuilder.Append("{\r\n");
                    templateBuilder.Append("    $('passwordlayer').style.display='none';\r\n");
                    templateBuilder.Append("    $(\"usernamelayer\").innerHTML = \"" + username.ToString() + "&nbsp;[<a href='javascript:;' onclick='showusername()'>切换临时帐号</a>]\";\r\n");
                    templateBuilder.Append("    tempaccounts = false;\r\n");
                    templateBuilder.Append("    var i = 1;\r\n");
                    templateBuilder.Append("    while(true)\r\n");
                    templateBuilder.Append("    {\r\n");
                    templateBuilder.Append("        var obj = $(\"albums\" + i);\r\n");
                    templateBuilder.Append("        if(obj == null) break;\r\n");
                    templateBuilder.Append("        obj.disabled = false\r\n");
                    templateBuilder.Append("        i++;\r\n");
                    templateBuilder.Append("    }\r\n");
                    templateBuilder.Append("}\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
                    templateBuilder.Append("<tbody>\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th>用户名</th>\r\n");
                    templateBuilder.Append("		<td><h5 id=\"usernamelayer\">\r\n");

                    if (userid > 0)
                    {

                        templateBuilder.Append("		" + username.ToString() + "&nbsp;[<a href='javascript:;' onclick='showusername()'>切换临时帐号</a>]\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("			匿名 [<a href=\"login.aspx\">登录</a>] [<a href=\"register.aspx\">注册</a>]\r\n");

                    }	//end if

                    templateBuilder.Append("</h5>\r\n");
                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("</tbody>\r\n");
                    templateBuilder.Append("<tbody id=\"passwordlayer\" style=\"display:none\">\r\n");
                    templateBuilder.Append("	<tr>\r\n");
                    templateBuilder.Append("		<th><label for=\"temppassword\">密码</label></th>\r\n");
                    templateBuilder.Append("		<td>\r\n");
                    templateBuilder.Append("			<input name=\"temppassword\" type=\"password\" id=\"temppassword\" size=\"20\" />\r\n");
                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"vcode\">验证码</label></th>\r\n");
                        templateBuilder.Append("		<td>\r\n");

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
                        templateBuilder.Append("	</tr>\r\n");

                    }	//end if


                    if (config.Secques == 1)
                    {

                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"question\">安全问题</label></th>\r\n");
                        templateBuilder.Append("		<td>\r\n");
                        templateBuilder.Append("			<select name=\"question\" id=\"question\">\r\n");
                        templateBuilder.Append("			<option value=\"0\" selected=\"selected\">无</option>\r\n");
                        templateBuilder.Append("			<option value=\"1\">母亲的名字</option>\r\n");
                        templateBuilder.Append("			<option value=\"2\">爷爷的名字</option>\r\n");
                        templateBuilder.Append("			<option value=\"3\">父亲出生的城市</option>\r\n");
                        templateBuilder.Append("			<option value=\"4\">您其中一位老师的名字</option>\r\n");
                        templateBuilder.Append("			<option value=\"5\">您个人计算机的型号</option>\r\n");
                        templateBuilder.Append("			<option value=\"6\">您最喜欢的餐馆名称</option>\r\n");
                        templateBuilder.Append("			<option value=\"7\">驾驶执照的最后四位数字</option>\r\n");
                        templateBuilder.Append("			</select>\r\n");
                        templateBuilder.Append("		</td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");
                        templateBuilder.Append("	<tr>\r\n");
                        templateBuilder.Append("		<th><label for=\"answer\">答案</label></th>\r\n");
                        templateBuilder.Append("		<td><input name=\"answer\" type=\"text\" id=\"answer\" size=\"50\" /><br/>如果您设置了安全提问，请在此输入正确的问题和回答</td>\r\n");
                        templateBuilder.Append("	</tr>\r\n");

                    }	//end if

                    templateBuilder.Append("</tbody>\r\n");


                    templateBuilder.Append("			<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th><label for=\"title\">标题</label></th>\r\n");
                    templateBuilder.Append("				<td>\r\n");

                    if (forum.Applytopictype == 1 && topictypeselectoptions != "")
                    {

                        templateBuilder.Append("						<select name=\"typeid\" id=\"typeid\">" + topictypeselectoptions.ToString() + "</select>\r\n");

                    }	//end if

                    templateBuilder.Append("					<input name=\"title\" type=\"text\" id=\"title\" size=\"60\" title=\"标题最多为60个字符\" />\r\n");

                    if (canhtmltitle)
                    {

                        templateBuilder.Append("					<a id=\"titleEditorButton\" href=\"###\" onclick=\"\">高级编辑</a>\r\n");
                        templateBuilder.Append("					<script type=\"text/javascript\" src=\"javascript/dnteditor.js\"></" + "script>\r\n");
                        templateBuilder.Append("					<div id=\"titleEditorDiv\" style=\"display: none;\">\r\n");
                        templateBuilder.Append("					<textarea name=\"htmltitle\" id=\"htmltitle\" cols=\"80\" rows=\"10\"></textarea>\r\n");
                        templateBuilder.Append("					<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("						var forumpath = '" + forumpath.ToString() + "';\r\n");
                        templateBuilder.Append("						var templatepath = '" + templatepath.ToString() + "';\r\n");
                        templateBuilder.Append("						var temptitle = $('faketitle');\r\n");
                        templateBuilder.Append("						var titleEditor = null;\r\n");
                        templateBuilder.Append("						function AdvancedTitleEditor() {\r\n");
                        templateBuilder.Append("							$('title').style.display = 'none';\r\n");
                        templateBuilder.Append("							$('titleEditorDiv').style.display = '';\r\n");
                        templateBuilder.Append("							$('titleEditorButton').style.display = 'none';\r\n");
                        templateBuilder.Append("							titleEditor = new DNTeditor('htmltitle', '500', '50', $('title').value);\r\n");
                        templateBuilder.Append("							titleEditor.OnChange = function(){\r\n");
                        templateBuilder.Append("								//temptitle.innerHTML = html2bbcode(titleEditor.GetHtml().replace(/<[^>]*>/ig, ''));\r\n");
                        templateBuilder.Append("							}\r\n");
                        templateBuilder.Append("							titleEditor.Basic = true;\r\n");
                        templateBuilder.Append("							titleEditor.IsAutoSave = false;\r\n");
                        templateBuilder.Append("							titleEditor.Style = forumpath + 'templates/' + templatepath + '/editor.css';\r\n");
                        templateBuilder.Append("							titleEditor.BasePath = forumpath;\r\n");
                        templateBuilder.Append("							titleEditor.ReplaceTextarea();\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						$('titleEditorButton').onclick = function(){\r\n");
                        templateBuilder.Append("							AdvancedTitleEditor();\r\n");
                        templateBuilder.Append("						};\r\n");
                        templateBuilder.Append("					</" + "script>\r\n");
                        templateBuilder.Append("					</div>\r\n");

                    }	//end if

                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			</tbody>\r\n");

                    if (type == "bonus")
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><input id=\"isbonus\" type=\"hidden\" name=\"isbonus\" value=\"1\" />悬赏价格</th>\r\n");
                        templateBuilder.Append("					<td>\r\n");
                        templateBuilder.Append("					<input name=\"topicprice\" type=\"text\" id=\"topicprice\" value=\"" + usergroupinfo.Minbonusprice.ToString().Trim() + "\" size=\"5\" />\r\n");
                        templateBuilder.Append("					" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                        templateBuilder.Append("				" + userextcreditsinfo.Name.ToString().Trim() + "[ 悬赏范围 " + usergroupinfo.Minbonusprice.ToString().Trim() + " - " + usergroupinfo.Maxbonusprice.ToString().Trim() + "  \r\n");
                        templateBuilder.Append("						" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                        templateBuilder.Append("					" + userextcreditsinfo.Name.ToString().Trim() + ", 当前可用 " + mycurrenttranscredits.ToString() + " " + userextcreditsinfo.Unit.ToString().Trim() + "" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                        templateBuilder.Append("			 ](只允许正整数)\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");

                    }	//end if


                    if (usergroupinfo.Allowpostpoll == 1 && type == "poll")
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"enddatetime\">投票结束日期</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input name=\"enddatetime\" type=\"text\" id=\"enddatetime\" size=\"10\" value=\"" + enddatetime.ToString() + "\" style=\"cursor:default\" onClick=\"showcalendar(event, 'enddatetime', 'cal_startdate', 'cal_enddate', '" + enddatetime.ToString() + "');\" readonly=\"readonly\" /></span>\r\n");
                        templateBuilder.Append("				<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" size=\"10\"  value=\"" + enddatetime.ToString() + "\">\r\n");
                        templateBuilder.Append("				<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" size=\"10\"  value=\"\">\r\n");
                        templateBuilder.Append("				 </td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			</tbody> \r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>\r\n");
                        templateBuilder.Append("					<input name=\"createpoll\" type=\"hidden\" id=\"createpoll\" value=\"1\" />\r\n");
                        templateBuilder.Append("                   		 	<input type=\"checkbox\" name=\"visiblepoll\"  /> 提交投票后结果才可见<br />\r\n");
                        templateBuilder.Append("                    			<input type=\"checkbox\" name=\"multiple\"  onclick=\"this.checked?$('maxchoicescontrol').style.display='':$('maxchoicescontrol').style.display='none';\" class=\"checkinput\" /> 多选投票<br />\r\n");
                        templateBuilder.Append("           	        		<span id=\"maxchoicescontrol\" style=\"display: none\">最多可选项数: <input type=\"text\" name=\"maxchoices\" value=\"2\" size=\"5\"><br /></span>\r\n");
                        templateBuilder.Append("				</th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input name=\"button\" type=\"button\" onclick=\"clonePoll('" + config.Maxpolloptions.ToString().Trim() + "')\" value=\"增加投票项\" />\r\n");
                        templateBuilder.Append("					<input name=\"button\" type=\"button\" onclick=\"if(!delObj(document.getElementById('polloptions'),2)){alert('投票项不能少于2个');}\" value=\"删除投票项\" />\r\n");
                        templateBuilder.Append("					<input id=\"PollItemname\" type=\"hidden\" name=\"PollItemname\" value=\"\" />\r\n");
                        templateBuilder.Append("					<input id=\"PollItemvalue\" type=\"hidden\" name=\"PollItemvalue\" value=\"\" />\r\n");
                        templateBuilder.Append("					<div id=\"polloptions\">\r\n");
                        templateBuilder.Append("						<div id=\"divPollItem\"><input type=\"text\" size=\"70\" id=\"pollitemid\" name=\"pollitemid\" maxlength=\"80\" /></div>\r\n");
                        templateBuilder.Append("						<div><input type=\"text\" size=\"70\" id=\"pollitemid\" name=\"pollitemid\" maxlength=\"80\" /></div>\r\n");
                        templateBuilder.Append("					</div>\r\n");
                        templateBuilder.Append("				 </td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tbody>\r\n");

                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/common.js\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/menu.js\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/bbcode.js\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("var lang						= new Array();\r\n");
                    templateBuilder.Append("lang['post_discuzcode_code'] = '请输入要插入的代码';\r\n");
                    templateBuilder.Append("lang['post_discuzcode_quote'] = '请输入要插入的引用';\r\n");
                    templateBuilder.Append("lang['post_discuzcode_free'] = '请输入要插入的免费信息';\r\n");
                    templateBuilder.Append("lang['post_discuzcode_hide'] = '请输入要插入的隐藏内容';\r\n");
                    templateBuilder.Append("var editorcss = 'templates/" + templatepath.ToString() + "/editor.css';\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
                    templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	var typerequired = parseInt('0');\r\n");
                    templateBuilder.Append("//		var bbinsert = parseInt('1');\r\n");
                    templateBuilder.Append("	var seccodecheck = parseInt('0');\r\n");
                    templateBuilder.Append("	var secqaacheck = parseInt('0');\r\n");
                    templateBuilder.Append("	var special = parseInt('0');\r\n");
                    templateBuilder.Append("	var isfirstpost = 1;\r\n");
                    templateBuilder.Append("	var allowposttrade = parseInt('1');\r\n");
                    templateBuilder.Append("	var allowpostreward = parseInt('1');\r\n");
                    templateBuilder.Append("	var allowpostactivity = parseInt('1');\r\n");
                    templateBuilder.Append("	lang['board_allowed'] = '系统限制';\r\n");
                    templateBuilder.Append("	lang['lento'] = '到';\r\n");
                    templateBuilder.Append("	lang['bytes'] = '字节';\r\n");
                    templateBuilder.Append("	lang['post_curlength'] = '当前长度';\r\n");
                    templateBuilder.Append("	lang['post_title_and_message_isnull'] = '请完成标题或内容栏。';\r\n");
                    templateBuilder.Append("	lang['post_title_toolong'] = '您的标题超过 60 个字符的限制。';\r\n");
                    templateBuilder.Append("	lang['post_message_length_invalid'] = '您的帖子长度不符合要求。';\r\n");
                    templateBuilder.Append("	lang['post_type_isnull'] = '请选择主题对应的分类。';\r\n");
                    templateBuilder.Append("	lang['post_reward_credits_null'] = '对不起，您输入悬赏金币。';\r\n");
                    templateBuilder.Append("	var bbinsert = parseInt('1');\r\n");
                    templateBuilder.Append("	var editorid = 'posteditor';\r\n");
                    templateBuilder.Append("	var allowhtml = parseInt('0');\r\n");
                    templateBuilder.Append("	var forumallowhtml = parseInt('0');\r\n");
                    templateBuilder.Append("	var allowsmilies = 1 - parseInt('" + smileyoff.ToString() + "');\r\n");
                    templateBuilder.Append("	var allowbbcode = 1 - parseInt('" + bbcodeoff.ToString() + "');\r\n");
                    templateBuilder.Append("	var allowimgcode = parseInt('" + allowimg.ToString() + "');\r\n");
                    templateBuilder.Append("	var wysiwyg = (is_ie || is_moz || (is_opera && opera.version() >= 9)) && parseInt('" + config.Defaulteditormode.ToString().Trim() + "') && allowbbcode == 1 ? 1 : 0;//bbinsert == 1 ? 1 : 0;\r\n");
                    templateBuilder.Append("	var allowswitcheditor = parseInt('" + config.Allowswitcheditor.ToString().Trim() + "') && allowbbcode == 1 ;\r\n");
                    templateBuilder.Append("	//var Editor				= new Array();\r\n");
                    templateBuilder.Append("	lang['enter_tag_option']		= \"请输入 %1 标签的选项:\";\r\n");
                    templateBuilder.Append("	//lang['enter_list_item']			= \"输入一个列表项目.\\r\\n留空或者点击'取消'完成此列表.\";\r\n");
                    templateBuilder.Append("	//lang['enter_link_url']			= \"请输入链接的地址:\";\r\n");
                    templateBuilder.Append("	//lang['enter_image_url']			= \"请输入图片链接地址:\";\r\n");
                    templateBuilder.Append("	//lang['enter_email_link']		= \"请输入此链接的邮箱地址:\";\r\n");
                    templateBuilder.Append("	lang['enter_table_rows']		= \"请输入行数，最多 30 行:\";\r\n");
                    templateBuilder.Append("	lang['enter_table_columns']		= \"请输入列数，最多 30 列:\";\r\n");
                    templateBuilder.Append("	//lang['fontname']			= \"字体\";\r\n");
                    templateBuilder.Append("	//lang['fontsize']			= \"大小\";\r\n");
                    templateBuilder.Append("	var custombbcodes = { " + customeditbuttons.ToString() + " };\r\n");
                    templateBuilder.Append("	var smileyinsert = parseInt('1');\r\n");
                    templateBuilder.Append("	//var editor_id = 'posteditor';　//编辑器ID\r\n");
                    templateBuilder.Append("	var smiliesCount = 12;//显示表情总数\r\n");
                    templateBuilder.Append("	var colCount = 4; //每行显示表情个数\r\n");
                    templateBuilder.Append("	var title = \"\";				   //标题\r\n");
                    templateBuilder.Append("	var showsmiliestitle = 1;        //是否显示标题（0不显示 1显示）\r\n");
                    templateBuilder.Append("	var smiliesIsCreate = 0;		   //编辑器是否已被创建(0否，1是）\r\n");
                    templateBuilder.Append("	var smilies_HASH = {};//得到表情符号信息\r\n");
                    templateBuilder.Append("	var smiliePageSize = 16; //表情每页显示数量 (共4列)\r\n");
                    templateBuilder.Append("	//table变量\r\n");
                    templateBuilder.Append("	var msgheader = \"margin:0 2em; font: 11px Arial, Tahoma; font-weight: bold; background: #F3F8D7; padding: 5px;\";\r\n");
                    templateBuilder.Append("	var msgborder = \"margin: 0 2em; padding: 10px; border: 1px solid #dbddd3; word-break: break-all; background-color: #fdfff2;\";\r\n");
                    templateBuilder.Append("	var INNERBORDERCOLOR = \"#D6E0EF\";\r\n");
                    templateBuilder.Append("	var BORDERWIDTH = \"1\";\r\n");
                    templateBuilder.Append("	var BORDERCOLOR = \"#7ac4ea\";\r\n");
                    templateBuilder.Append("	var ALTBG2 = \"#ffffff\";\r\n");
                    templateBuilder.Append("	var FONTSIZE = \"12px\";\r\n");
                    templateBuilder.Append("	var FONT = \"Tahoma, Verdana\";\r\n");
                    templateBuilder.Append("	//var fontoptions = new Array(\"仿宋_GB2312\", \"黑体\", \"楷体_GB2312\", \"宋体\", \"新宋体\", \"Tahoma\", \"Arial\", \"Impact\", \"Verdana\", \"Times New Roman\");\r\n");
                    templateBuilder.Append("	var altbg1 = '#f5fbff';\r\n");
                    templateBuilder.Append("	var altbg2 = 'background: #ffffff;font: 12px Tahoma, Verdana;';\r\n");
                    templateBuilder.Append("	var tableborder = 'background: #D6E0EF;border: 1px solid #7ac4ea;';\r\n");
                    templateBuilder.Append("	//var lang = new Array();\r\n");
                    templateBuilder.Append("	if(is_ie >= 5 || is_moz >= 2) {\r\n");
                    templateBuilder.Append("		window.onbeforeunload = function () {saveData(wysiwyg && bbinsert ? editdoc.body.innerHTML : textobj.value)};\r\n");
                    templateBuilder.Append("		lang['post_autosave_none'] = \"没有可以恢复的数据\";\r\n");
                    templateBuilder.Append("		lang['post_autosave_confirm'] = \"本操作将覆盖当前帖子内容，确定要恢复数据吗？\";\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("	var maxpolloptions = parseInt('" + config.Maxpolloptions.ToString().Trim() + "');\r\n");
                    templateBuilder.Append("</" + "script>\r\n");
                    templateBuilder.Append("<th valign=\"top\">\r\n");
                    templateBuilder.Append("	<label for=\"posteditor_textarea\">\r\n");
                    templateBuilder.Append("		内容\r\n");
                    templateBuilder.Append("	</label>\r\n");
                    //if (templateid == 0)
                    //{
                    //    templateBuilder.Append("	<div id=\"posteditor_left\" >\r\n");
                    //    templateBuilder.Append("		<input type=\"checkbox\" value=\"1\" name=\"parseurloff\" id=\"parseurloff\" \r\n");

                    //    if (parseurloff == 1)
                    //    {

                    //        templateBuilder.Append("checked\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("> 禁用 URL 识别<br />\r\n");
                    //    templateBuilder.Append("		<input type=\"checkbox\" value=\"1\" name=\"smileyoff\" id=\"smileyoff\" \r\n");

                    //    if (smileyoff == 1)
                    //    {

                    //        templateBuilder.Append("checked\r\n");

                    //    }	//end if


                    //    if (forum.Allowsmilies != 1)
                    //    {

                    //        templateBuilder.Append("disabled\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("> 禁用 表情<br />\r\n");
                    //    templateBuilder.Append("		<input type=\"checkbox\" value=\"1\" name=\"bbcodeoff\" id=\"bbcodeoff\"\r\n");

                    //    if (bbcodeoff == 1)
                    //    {

                    //        templateBuilder.Append("				checked\r\n");

                    //    }	//end if


                    //    if (usergroupinfo.Allowcusbbcode != 1)
                    //    {

                    //        templateBuilder.Append("				disabled\r\n");

                    //    }
                    //    else
                    //    {


                    //        if (forum.Allowbbcode != 1)
                    //        {

                    //            templateBuilder.Append("					disabled\r\n");

                    //        }	//end if


                    //    }	//end if

                    //    templateBuilder.Append("		> 禁用代码模式<br />\r\n");
                    //    templateBuilder.Append("		<input type=\"checkbox\" value=\"1\" name=\"usesig\" id=\"usesig\" \r\n");

                    //    if (usesig == 1)
                    //    {

                    //        templateBuilder.Append("checked\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("> 使用个人签名\r\n");

                    //    if (pagename == "postreply.aspx")
                    //    {

                    //        templateBuilder.Append("<br />\r\n");
                    //        templateBuilder.Append("			<input type=\"checkbox\" name=\"emailnotify\" id=\"emailnotify\" /> 发送邮件通知楼主\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("<!--表情符号列表-->\r\n");

                    //    if (config.Smileyinsert == 1)
                    //    {

                    //        string defaulttypname = string.Empty;

                    //        templateBuilder.Append("		<div class=\"editorsmiles\">\r\n");
                    //        templateBuilder.Append("			<div class=\"smiliepanel\">\r\n");
                    //        templateBuilder.Append("				<div id=\"scrollbar\" class=\"scrollbar\">\r\n");
                    //        templateBuilder.Append("					<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\">\r\n");
                    //        templateBuilder.Append("						<tr>\r\n");

                    //        int stype__loop__id = 0;
                    //        foreach (DataRow stype in smilietypes.Rows)
                    //        {
                    //            stype__loop__id++;


                    //            if (stype__loop__id == 1)
                    //            {

                    //                defaulttypname = stype["code"].ToString().Trim();


                    //            }	//end if

                    //            templateBuilder.Append("							<td id=\"t_s_" + stype__loop__id.ToString() + "\"><div id=\"s_" + stype__loop__id.ToString() + "\" onclick=\"showsmiles(" + stype__loop__id.ToString() + ", '" + stype["code"].ToString().Trim() + "');\" \r\n");

                    //            if (stype__loop__id != 1)
                    //            {

                    //                templateBuilder.Append("style=\"display:none;\"\r\n");

                    //            }
                    //            else
                    //            {

                    //                templateBuilder.Append("class=\"lian\"\r\n");

                    //            }	//end if

                    //            templateBuilder.Append(">" + stype["code"].ToString().Trim() + "</div></td>\r\n");

                    //        }	//end loop

                    //        templateBuilder.Append("						</tr>\r\n");
                    //        templateBuilder.Append("					</table>\r\n");
                    //        templateBuilder.Append("				</div>\r\n");
                    //        templateBuilder.Append("				<div id=\"scrlcontrol\">\r\n");
                    //        templateBuilder.Append("					<img src=\"editor/images/smilie_prev_default.gif\" alt=\"向前\" onmouseover=\"if($('scrollbar').scrollLeft>0)this.src=this.src.replace(/_default|_selected/, '_hover');\" onmouseout=\"this.src=this.src.replace(/_hover|_selected/, '_default');\" onmousedown=\"if($('scrollbar').scrollLeft>0){this.src=this.src.replace(/_hover|_default/, '_selected');this.boder=1;}\" onmouseup=\"if($('scrollbar').scrollLeft>0)this.src=this.src.replace(/_selected/, '_hover');else{this.src=this.src.replace(/_selected|_hover/, '_default');}this.border=0;\" onclick=\"scrollSmilieTypeBar($('scrollbar'), 1-$('t_s_1').clientWidth);\"/>\r\n");
                    //        templateBuilder.Append("					<img src=\"editor/images/smilie_next_default.gif\" alt=\"向后\" onmouseover=\"if($('scrollbar').scrollLeft<scrMaxLeft)this.src=this.src.replace(/_default|_selected/, '_hover');\" onmouseout=\"this.src=this.src.replace(/_hover|_selected/, '_default');\" onmousedown=\"if($('scrollbar').scrollLeft<scrMaxLeft){this.src=this.src.replace(/_hover|_default/, '_selected');this.boder=1;}\" onmouseup=\"if($('scrollbar').scrollLeft<scrMaxLeft)this.src=this.src.replace(/_selected/, '_hover');else{this.src=this.src.replace(/_selected|_hover/, '_default');}this.border=0;\" onclick=\"scrollSmilieTypeBar($('scrollbar'), $('t_s_1').clientWidth);\" />\r\n");
                    //        templateBuilder.Append("				</div>\r\n");
                    //        templateBuilder.Append("	 </div>\r\n");
                    //        templateBuilder.Append("	 <div id=\"showsmilie\"><img src=\"images/common/loading_wide.gif\" width=\"90%\" style=\" margin-top:20px;\" alt=\"加载表情\"/><p>正在加载表情...</p></div>\r\n");
                    //        templateBuilder.Append("	 <div id=\"showsmilie_pagenum\">&nbsp;</div>\r\n");
                    //        templateBuilder.Append("    </div>\r\n");
                    //        templateBuilder.Append("		<script src=\"javascript/post.js\" type=\"text/javascript\"></" + "script>\r\n");
                    //        templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    //        templateBuilder.Append("			var scrMaxLeft; //表情滚动条宽度\r\n");
                    //        templateBuilder.Append("			var firstpagesmilies_json ={ " + firstpagesmilies.ToString() + " };\r\n");
                    //        templateBuilder.Append("			showFirstPageSmilies(firstpagesmilies_json, '" + defaulttypname.ToString() + "',  16);\r\n");
                    //        templateBuilder.Append("			function getSmilies(func){\r\n");
                    //        templateBuilder.Append("				var c=\"tools/ajax.aspx?t=smilies\";\r\n");
                    //        templateBuilder.Append("				_sendRequest(c,function(d){var e={};try{e=eval(\"(\"+d+\")\")}catch(f){e={}}var h=e?e:null;func(h);e=null;func=null},false,true)\r\n");
                    //        templateBuilder.Append("			}\r\n");
                    //        templateBuilder.Append("			getSmilies(function(obj){ \r\n");
                    //        templateBuilder.Append("				smilies_HASH = obj; \r\n");
                    //        templateBuilder.Append("				showsmiles(1, '" + defaulttypname.ToString() + "');\r\n");
                    //        templateBuilder.Append("			});\r\n");
                    //        templateBuilder.Append("			window.onload = function() {\r\n");
                    //        templateBuilder.Append("				$('scrollbar').scrollLeft = 10000;\r\n");
                    //        templateBuilder.Append("				scrMaxLeft = $('scrollbar').scrollLeft;\r\n");
                    //        templateBuilder.Append("				$('scrollbar').scrollLeft = 1;	\r\n");
                    //        templateBuilder.Append("				if ($('scrollbar').scrollLeft > 0)\r\n");
                    //        templateBuilder.Append("				{\r\n");
                    //        templateBuilder.Append("					$('scrlcontrol').style.display = '';\r\n");
                    //        templateBuilder.Append("					$('scrollbar').scrollLeft = 0;	\r\n");
                    //        templateBuilder.Append("				}\r\n");
                    //        templateBuilder.Append("			}\r\n");
                    //        templateBuilder.Append("		</" + "script>\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("<!-- / 表情符号列表-->\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("</td>\r\n");
                    //    templateBuilder.Append("<td valign=\"top\">\r\n");
                    //    templateBuilder.Append("<div id=\"posteditor\" class=\"editor\">\r\n");
                    //    templateBuilder.Append("	<div id=\"posteditor_controls\">\r\n");
                    //    templateBuilder.Append("		<div class=\"editorrow\">\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_bold\" title=\"粗体\">B</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_italic\" title=\"斜体\">I</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_underline\" title=\"下划线\">U</a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_popup_fontname\" title=\"字体\"><span style=\"width: 110px; display: block; white-space: nowrap;\" id=\"posteditor_font_out\">字体</span></a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_popup_fontsize\" title=\"大小\"><span style=\"width: 30px; display: block;\" id=\"posteditor_size_out\">大小</span></a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_popup_forecolor\" title=\"颜色\"><span style=\"width: 30px; display: block;\"><img alt=\"color\" src=\"editor/images/bb_color.gif\"/><br/><img width=\"21\" height=\"4\" style=\"background-color: Black;\" alt=\"\" id=\"posteditor_color_bar\" src=\"editor/images/bb_clear.gif\"/></span></a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_justifyleft\" title=\"居左\">Align Left</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_justifycenter\" title=\"居中\">Align Center</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_justifyright\" title=\"居右\">Align Right</a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_createlink\" title=\"插入链接\">Url</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_email\" title=\"插入邮箱链接\">Email</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_insertimage\" title=\"插入图片\">Image</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_popup_media\" title=\"插入在线视频\">Media</a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_quote\" title=\"插入引用\">Quote</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_code\" title=\"插入代码\">Code</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_buttonctrl\" class=\"plugeditor editormode\">简单功能</a>\r\n");
                    //    templateBuilder.Append("		</div>\r\n");
                    //    templateBuilder.Append("		<div class=\"editorrow\" id=\"posteditor_morebuttons\" >\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_removeformat\" title=\"清除文本格式\">Remove Format</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_unlink\" title=\"移除链接\">Unlink</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_undo\" title=\"撤销\">Undo</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_redo\" title=\"重做\">Redo</a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_insertorderedlist\" title=\"排序的列表\">Ordered List</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_insertunorderedlist\" title=\"未排序列表\">Unordered List</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_outdent\" title=\"减少缩进\">Outdent</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_indent\" title=\"增加缩进\">Indent</a>\r\n");
                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_popup_table\" title=\"插入表格\">Table</a>\r\n");
                    //    templateBuilder.Append("			<a id=\"posteditor_cmd_free\" href=\"###\" title=\"插入免费信息\">Free</a>\r\n");

                    //    if (usergroupinfo.Allowhidecode == 1)
                    //    {

                    //        templateBuilder.Append("			<a id=\"posteditor_cmd_hide\" title=\"插入隐藏内容\">Hide</a>\r\n");

                    //    }	//end if

                    //    templateBuilder.Append("			<em></em>\r\n");
                    //    templateBuilder.Append("			<!--<a class=\"plugeditor\" id=\"posteditor_cmd_custom1_qq\"><img src=\"editor/images/bb_qq.gif\" title=\"显示 QQ 在线状态，点这个图标可以和他（她）聊天\" alt=\"qq\" /></a>-->		\r\n");
                    //    templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
                    //    templateBuilder.Append("				//自定义按扭显示\r\n");
                    //    templateBuilder.Append("				if(typeof(custombbcodes) != 'undefined') {\r\n");
                    //    templateBuilder.Append("					//document.writeln('<td><img src=\"editor/images/separator.gif\" width=\"6\" height=\"23\"></td>');\r\n");
                    //    templateBuilder.Append("					for (var id in custombbcodes){\r\n");
                    //    templateBuilder.Append("						if (custombbcodes[id][1] == '')\r\n");
                    //    templateBuilder.Append("						{\r\n");
                    //    templateBuilder.Append("							continue;\r\n");
                    //    templateBuilder.Append("						}\r\n");
                    //    templateBuilder.Append("						document.writeln('<a id=\"posteditor_cmd_custom' + custombbcodes[id][5] + '_' + custombbcodes[id][0] + '\" style=\"background: transparent url(editor/images/' + custombbcodes[id][1] + ') no-repeat 0 0;\"><img title=\"' + custombbcodes[id][2] + '\" alt=\"' + custombbcodes[id][2] + '\" src = \"editor/images/' + custombbcodes[id][1] + '\" width=\"21\" height=\"20\" /></a>');\r\n");
                    //    templateBuilder.Append("					}\r\n");
                    //    templateBuilder.Append("				}\r\n");
                    //    templateBuilder.Append("			</" + "script>\r\n");
                    //    templateBuilder.Append("		</div>\r\n");
                    //    templateBuilder.Append("		<div id=\"posteditor_switcher\" class=\"editor_switcher_bar\">\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" id=\"bbcodemode\">代码模式</button>\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" id=\"wysiwygmode\">所见即所得模式</button>\r\n");
                    //    templateBuilder.Append("		</div>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("</div>\r\n");
                    //    templateBuilder.Append("<div class=\"editortoolbar\">\r\n");
                    //    templateBuilder.Append("	<div class=\"popupmenu_popup fontname_menu\" id=\"posteditor_popup_fontname_menu\" style=\"display: none\">\r\n");
                    //    templateBuilder.Append("		<ul unselectable=\"on\">\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '仿宋_GB2312');hideMenu()\" style=\"font-family: 仿宋_GB2312\" unselectable=\"on\">仿宋_GB2312</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '黑体');hideMenu()\" style=\"font-family: 黑体\" unselectable=\"on\">黑体</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '楷体_GB2312');hideMenu()\" style=\"font-family: 楷体_GB2312\" unselectable=\"on\">楷体_GB2312</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '宋体');hideMenu()\" style=\"font-family: 宋体\" unselectable=\"on\">宋体</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '新宋体');hideMenu()\" style=\"font-family: 新宋体\" unselectable=\"on\">新宋体</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', '微软雅黑');hideMenu()\" style=\"font-family: 微软雅黑\" unselectable=\"on\">微软雅黑</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Trebuchet MS');hideMenu()\" style=\"font-family: Trebuchet MS\" unselectable=\"on\">Trebuchet MS</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Tahoma');hideMenu()\" style=\"font-family: Tahoma\" unselectable=\"on\">Tahoma</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Arial');hideMenu()\" style=\"font-family: Arial\" unselectable=\"on\">Arial</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Impact');hideMenu()\" style=\"font-family: Impact\" unselectable=\"on\">Impact</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Verdana');hideMenu()\" style=\"font-family: Verdana\" unselectable=\"on\">Verdana</li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontname', 'Times New Roman');hideMenu()\" style=\"font-family: Times New Roman\" unselectable=\"on\">Times New Roman</li>\r\n");
                    //    templateBuilder.Append("		</ul>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("	<div class=\"popupmenu_popup fontsize_menu\" id=\"posteditor_popup_fontsize_menu\" style=\"display: none\">\r\n");
                    //    templateBuilder.Append("		<ul unselectable=\"on\">\r\n");
                    //    templateBuilder.Append("		<li onclick=\"discuzcode('fontsize', 1);hideMenu()\" unselectable=\"on\"><font size=\"1\" unselectable=\"on\">1</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 2);hideMenu()\" unselectable=\"on\"><font size=\"2\" unselectable=\"on\">2</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 3);hideMenu()\" unselectable=\"on\"><font size=\"3\" unselectable=\"on\">3</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 4);hideMenu()\" unselectable=\"on\"><font size=\"4\" unselectable=\"on\">4</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 5);hideMenu()\" unselectable=\"on\"><font size=\"5\" unselectable=\"on\">5</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 6);hideMenu()\" unselectable=\"on\"><font size=\"6\" unselectable=\"on\">6</font></li>\r\n");
                    //    templateBuilder.Append("			<li onclick=\"discuzcode('fontsize', 7);hideMenu()\" unselectable=\"on\"><font size=\"7\" unselectable=\"on\">7</font></li>\r\n");
                    //    templateBuilder.Append("		</ul>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("	<div class=\"popupmenu_popup colorbar\" id=\"posteditor_popup_forecolor_menu\" style=\"display: none\">\r\n");
                    //    templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" unselectable=\"on\" style=\"width: auto;\">\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Black');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Black\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Sienna');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Sienna\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkOliveGreen');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkOliveGreen\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkGreen');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkGreen\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkSlateBlue');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkSlateBlue\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Navy');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Navy\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Indigo');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Indigo\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkSlateGray');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkSlateGray\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkRed');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkRed\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkOrange');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkOrange\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Olive');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Olive\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Green');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Green\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Teal');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Teal\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Blue');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Blue\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'SlateGray');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: SlateGray\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DimGray');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DimGray\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Red');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Red\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'SandyBrown');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: SandyBrown\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'YellowGreen');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: YellowGreen\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'SeaGreen');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: SeaGreen\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'MediumTurquoise');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: MediumTurquoise\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'RoyalBlue');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: RoyalBlue\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Purple');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Purple\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Gray');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Gray\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Magenta');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Magenta\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Orange');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Orange\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Yellow');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Yellow\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Lime');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Lime\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Cyan');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Cyan\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DeepSkyBlue');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DeepSkyBlue\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'DarkOrchid');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: DarkOrchid\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Silver');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Silver\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Pink');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Pink\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Wheat');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Wheat\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'LemonChiffon');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: LemonChiffon\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'PaleGreen');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: PaleGreen\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'PaleTurquoise');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: PaleTurquoise\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'LightBlue');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: LightBlue\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'Plum');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: Plum\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("			<td class=\"editor_colornormal\" onclick=\"discuzcode('forecolor', 'White');hideMenu()\" unselectable=\"on\" onmouseover=\"colorContext(this, 'mouseover')\" onmouseout=\"colorContext(this, 'mouseout')\"><div style=\"background-color: White\" unselectable=\"on\"></div></td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("	</table>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("	<div class=\"popupmenu_popup\" id=\"posteditor_popup_table_menu\" style=\"display: none\">\r\n");
                    //    templateBuilder.Append("		<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" unselectable=\"on\">\r\n");
                    //    templateBuilder.Append("		  <tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("				<td nowrap>表格行数:</td>\r\n");
                    //    templateBuilder.Append("				<td nowrap><input type=\"text\" id=\"posteditor_table_rows\" size=\"3\" value=\"2\" /></td>\r\n");
                    //    templateBuilder.Append("				<td nowrap>表格列数:</td>\r\n");
                    //    templateBuilder.Append("				<td nowrap><input type=\"text\" id=\"posteditor_table_columns\" size=\"3\" value=\"2\" /></td>\r\n");
                    //    templateBuilder.Append("		  </tr>\r\n");
                    //    templateBuilder.Append("		  <tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("				<td nowrap>表格宽度:</td>\r\n");
                    //    templateBuilder.Append("				<td nowrap><input type=\"text\" id=\"posteditor_table_width\" size=\"3\" value=\"\" /></td>\r\n");
                    //    templateBuilder.Append("				<td nowrap>背景颜色:</td>\r\n");
                    //    templateBuilder.Append("				<td nowrap><input type=\"text\" id=\"posteditor_table_bgcolor\" size=\"3\" /></td>\r\n");
                    //    templateBuilder.Append("		  </tr>\r\n");
                    //    templateBuilder.Append("		  <tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("				<td colspan=\"2\" align=\"right\"><input type=\"button\" onclick=\"discuzcode('table')\" value=\"提交\" /></td>\r\n");
                    //    templateBuilder.Append("				<td colspan=\"2\" align=\"left\"><input type=\"button\" onclick=\"hideMenu()\" value=\"取消\" /></td>\r\n");
                    //    templateBuilder.Append("		  </tr>\r\n");
                    //    templateBuilder.Append("		</table>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("	<div class=\"popupmenu_popup\" id=\"posteditor_popup_media_menu\" style=\"width: 240px;display: none\">\r\n");
                    //    templateBuilder.Append("	<input type=\"hidden\" id=\"posteditor_mediatype\" value=\"ra\">\r\n");
                    //    templateBuilder.Append("	<input type=\"hidden\" id=\"posteditor_mediaautostart\" value=\"0\">\r\n");
                    //    templateBuilder.Append("	<table cellpadding=\"4\" cellspacing=\"0\" border=\"0\" unselectable=\"on\">\r\n");
                    //    templateBuilder.Append("	<tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("		<td nowrap>\r\n");
                    //    templateBuilder.Append("			请输入在线视频的地址:<br />\r\n");
                    //    templateBuilder.Append("			<input id=\"posteditor_mediaurl\" size=\"40\" value=\"\" onkeyup=\"setmediatype('posteditor')\" />\r\n");
                    //    templateBuilder.Append("		</td>\r\n");
                    //    templateBuilder.Append("	</tr>\r\n");
                    //    templateBuilder.Append("	<tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("		<td nowrap>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_ra\" onclick=\"$('posteditor_mediatype').value = 'ra'\" checked=\"checked\">RA</label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_wma\" onclick=\"$('posteditor_mediatype').value = 'wma'\">WMA</label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_mp3\" onclick=\"$('posteditor_mediatype').value = 'mp3'\">MP3</label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_rm\" onclick=\"$('posteditor_mediatype').value = 'rm'\">RM/RMVB</label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_wmv\" onclick=\"$('posteditor_mediatype').value = 'wmv'\">WMV</label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"radio\" name=\"posteditor_mediatyperadio\" id=\"posteditor_mediatyperadio_mov\" onclick=\"$('posteditor_mediatype').value = 'mov'\">MOV</label>\r\n");
                    //    templateBuilder.Append("		</td>\r\n");
                    //    templateBuilder.Append("	</tr>\r\n");
                    //    templateBuilder.Append("	<tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("		<td nowrap>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\">宽: <input id=\"posteditor_mediawidth\" size=\"5\" value=\"400\" /></label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\">高: <input id=\"posteditor_mediaheight\" size=\"5\" value=\"300\"/></label>\r\n");
                    //    templateBuilder.Append("			<label style=\"float: left; width: 32%\"><input type=\"checkbox\" onclick=\"$('posteditor_mediaautostart').value =this.checked ? 1 : 0\"> 自动播放</label>\r\n");
                    //    templateBuilder.Append("		</td>\r\n");
                    //    templateBuilder.Append("	</tr>\r\n");
                    //    templateBuilder.Append("	<tr class=\"popupmenu_option\">\r\n");
                    //    templateBuilder.Append("		<td align=\"center\" colspan=\"2\"><input type=\"button\" size=\"8\" value=\"提交\" onclick=\"setmediacode('posteditor')\"> <input type=\"button\" onclick=\"hideMenu()\" value=\"取消\" /></td>\r\n");
                    //    templateBuilder.Append("	</tr>\r\n");
                    //    templateBuilder.Append("	</table>\r\n");
                    //    templateBuilder.Append("	</div>\r\n");
                    //    templateBuilder.Append("</div>\r\n");
                    //    templateBuilder.Append("<table class=\"editor_text\" summary=\"Message Textarea\" cellpadding=\"0\" cellspacing=\"0\" style=\"table-layout: fixed;\">\r\n");
                    //    templateBuilder.Append("<tr>\r\n");
                    //    templateBuilder.Append("	<td>\r\n");
                    //    templateBuilder.Append("		<textarea class=\"autosave\" name=\"message\" rows=\"10\" cols=\"60\" style=\"width:99%; height:250px\" tabindex=\"100\" id=\"posteditor_textarea\"  onSelect=\"javascript: storeCaret(this);\" onClick=\"javascript: storeCaret(this);\" onKeyUp=\"javascript:storeCaret(this);\" onKeyDown=\"ctlent(event);\">" + message.ToString() + "</textarea><input type=\"hidden\" name=\"sposteditor_mode\" id=\"posteditor_mode\" value=\"" + config.Defaulteditormode.ToString().Trim() + "\" />\r\n");
                    //    templateBuilder.Append("	</td>\r\n");
                    //    templateBuilder.Append("</tr>\r\n");
                    //    templateBuilder.Append("</table>\r\n");
                    //    templateBuilder.Append("		<div id=\"posteditor_bottom\" >\r\n");
                    //    templateBuilder.Append("		<table summary=\"Enitor Buttons\" cellpadding=\"0\" cellspacing=\"0\" class=\"editor_button\" style=\"border-top: none;\">\r\n");
                    //    templateBuilder.Append("		<tr>\r\n");
                    //    templateBuilder.Append("			<td style=\"border-top: none;\">\r\n");
                    //    templateBuilder.Append("				<div class=\"editor_textexpand\">\r\n");
                    //    templateBuilder.Append("					<img src=\"editor/images/contract.gif\" width=\"11\" height=\"21\" title=\"收缩编辑框\" alt=\"收缩编辑框\" id=\"posteditor_contract\" /><img src=\"editor/images/expand.gif\" width=\"12\" height=\"21\" title=\"扩展编辑框\" alt=\"扩展编辑框\" id=\"posteditor_expand\" />\r\n");
                    //    templateBuilder.Append("				</div>\r\n");
                    //    templateBuilder.Append("				</td>\r\n");
                    //    templateBuilder.Append("			<td align=\"right\" style=\"border-top: none;\">\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" name=\"restoredata\" id=\"restoredata\">恢复数据</button>\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" id=\"checklength\">字数检查</button>\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" name=\"previewbutton\" id=\"previewbutton\" tabindex=\"102\">预览帖子</button>\r\n");
                    //    templateBuilder.Append("				<button type=\"button\" tabindex=\"103\" id=\"clearcontent\">清空内容</button>\r\n");
                    //    templateBuilder.Append("			</td>\r\n");
                    //    templateBuilder.Append("		</tr>\r\n");
                    //    templateBuilder.Append("		</table>\r\n");
                    //}
                    //else
                    //{ 
                        templateBuilder.Append("</th>\r\n");
                        templateBuilder.Append("<td valign=\"top\">\r\n");
                        //templateBuilder.Append("<table class=\"editor_text\" summary=\"Message Textarea\" cellpadding=\"0\" cellspacing=\"0\" style=\"table-layout: fixed;border:1px solid white;padding-left:0px;\">\r\n");
                        //templateBuilder.Append("<tr>\r\n");
                        //templateBuilder.Append("	<td>\r\n");
                        templateBuilder.Append("        <link href=\"fckeditor/_samples/sample.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
                        templateBuilder.Append("        <script type=\"text/javascript\" src=\"fckeditor/fckeditor.js\"></script>\r\n");
                        templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("            var oFCKeditor = new FCKeditor('message');\r\n");
                        templateBuilder.Append("            oFCKeditor.Height	= 350 ;");
                        templateBuilder.Append("            oFCKeditor.BasePath = '" + Discuz.Common.XmlConfig.GetCpsClubUrl() + "/fckeditor/';\r\n");
                        templateBuilder.Append("            oFCKeditor.Config[\"AutoDetectLanguage\"] = false;\r\n");
                        templateBuilder.Append("            oFCKeditor.Config[\"DefaultLanguage\"] = 'zh-cn';\r\n");
                        templateBuilder.Append("            oFCKeditor.Config['SkinPath'] = oFCKeditor.BasePath+'editor/skins/office2003/';\r\n");
                        templateBuilder.Append("            oFCKeditor.Create();\r\n");
                        templateBuilder.Append("        </script>\r\n");
                        //templateBuilder.Append("	</td>\r\n");
                        //templateBuilder.Append("</tr>\r\n");
                        //templateBuilder.Append("</table>\r\n");
                    //}

                    if (canpostattach)
                    {



                        if (attachsize > 0)
                        {


                            if (attachextensions != "")
                            {

                                templateBuilder.Append("<div class=\"box\" style=\"padding:0;\">\r\n");
                                templateBuilder.Append("<table cellspacing=\"0\" cellpadding=\"0\" summary=\"Upload\">\r\n");
                                templateBuilder.Append("	<thead>\r\n");
                                templateBuilder.Append("		<tr>\r\n");
                                templateBuilder.Append("			<th><img src=\"templates/" + templatepath.ToString() + "/images/attachment.gif\" alt=\"附件\"/>上传附件</th>\r\n");

                                if (userid != -1 && usergroupinfo.Allowsetattachperm == 1)
                                {

                                    templateBuilder.Append("			<td class=\"nums\">阅读权限</td>\r\n");

                                }	//end if

                                templateBuilder.Append("			<td>描述</td>\r\n");
                                templateBuilder.Append("		</tr>\r\n");
                                templateBuilder.Append("	</thead>\r\n");
                                templateBuilder.Append("	<tbody style=\"display: none;\" id=\"attachbodyhidden\"><tr>\r\n");
                                templateBuilder.Append("		<td>\r\n");
                                templateBuilder.Append("			<input type=\"file\" name=\"postfile\"/>\r\n");
                                templateBuilder.Append("			<span id=\"localfile[]\"></span>&nbsp;\r\n");
                                templateBuilder.Append("			<input type=\"hidden\" name=\"localid\" />\r\n");
                                templateBuilder.Append("		</td>\r\n");

                                if (userid != -1 && usergroupinfo.Allowsetattachperm == 1)
                                {

                                    templateBuilder.Append("		<td class=\"nums\"><input type=\"text\" name=\"readperm\" value=\"0\" size=\"1\"/></td>\r\n");

                                }	//end if

                                templateBuilder.Append("		<td><input type=\"text\" name=\"attachdesc\" size=\"25\"/>\r\n");

                                templateBuilder.Append("		</td>\r\n");
                                templateBuilder.Append("	</tr>\r\n");
                                templateBuilder.Append("	</tbody>\r\n");
                                templateBuilder.Append("	<tbody id=\"attachbody\"></tbody>\r\n");
                                templateBuilder.Append("	<tbody>\r\n");
                                templateBuilder.Append("	<tr>\r\n");
                                templateBuilder.Append("		<td style=\"border-bottom: medium none;\" colspan=\"5\">\r\n");
                                templateBuilder.Append("			单个附件大小: <strong><script type=\"text/javascript\">ShowFormatBytesStr(" + usergroupinfo.Maxattachsize.ToString().Trim() + ");</" + "script></strong><br/>\r\n");
                                templateBuilder.Append("			今天可上传大小: <strong><script type=\"text/javascript\">ShowFormatBytesStr(" + attachsize.ToString() + ");</" + "script></strong><br/>\r\n");
                                templateBuilder.Append("			附件类型: <strong>" + attachextensionsnosize.ToString() + "</strong><br/>\r\n");
                                templateBuilder.Append("		</td>\r\n");
                                templateBuilder.Append("	</tr>\r\n");
                                templateBuilder.Append("	</tbody>\r\n");
                                templateBuilder.Append("</table>\r\n");
                                templateBuilder.Append("</div>\r\n");
                                templateBuilder.Append("<img id=\"img_hidden\" alt=\"1\" style=\"position:absolute;top:-100000px;filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='image');width:400;height:300\"></img>\r\n");
                                templateBuilder.Append("<script type=\"text/javascript\">\r\n");
                                templateBuilder.Append("var aid = 1;\r\n");
                                templateBuilder.Append("var thumbwidth = parseInt(400);\r\n");
                                templateBuilder.Append("var thumbheight = parseInt(300);\r\n");
                                templateBuilder.Append("var attachexts = new Array();\r\n");
                                templateBuilder.Append("var attachwh = new Array();\r\n");
                                templateBuilder.Append("function delAttach(id) \r\n");
                                templateBuilder.Append("{\r\n");
                                templateBuilder.Append("    var curattlength = $('attachbody').childNodes.length;\r\n");
                                templateBuilder.Append("    $('attachbody').removeChild($('attach_' + id).parentNode.parentNode);\r\n");
                                templateBuilder.Append("    $('attachbody').innerHTML == '' && addAttach();\r\n");
                                templateBuilder.Append("    if (curattlength == " + config.Maxattachments.ToString().Trim() + " && $(\"attach_\" + (aid-1)).value != \"\")\r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("	    addAttach();\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("	if ($('localimgpreview_' + id + '_menu'))\r\n");
                                templateBuilder.Append("	{\r\n");
                                templateBuilder.Append("		document.body.removeChild($('localimgpreview_' + id + '_menu'));\r\n");
                                templateBuilder.Append("	}    \r\n");
                                templateBuilder.Append("}\r\n");
                                templateBuilder.Append("function addAttach() \r\n");
                                templateBuilder.Append("{\r\n");
                                templateBuilder.Append("    if ($('attachbody').childNodes.length > " + config.Maxattachments.ToString().Trim() + " - 1)\r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("	    return;\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    newnode = $('attachbodyhidden').firstChild.cloneNode(true);\r\n");
                                templateBuilder.Append("    var id = aid;\r\n");
                                templateBuilder.Append("    var tags;\r\n");
                                templateBuilder.Append("    tags = findtags(newnode, 'input');\r\n");
                                templateBuilder.Append("    for(i in tags) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        if(tags[i].name == 'postfile') \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("            tags[i].id = 'attach_' + id;\r\n");
                                templateBuilder.Append("            tags[i].onchange = function() \r\n");
                                templateBuilder.Append("            {\r\n");
                                templateBuilder.Append("	            insertAttach(id);\r\n");
                                templateBuilder.Append("            };\r\n");
                                templateBuilder.Append("            tags[i].unselectable = 'on';\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        if(tags[i].name == 'localid') \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("            tags[i].value = id;\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    tags = findtags(newnode, 'span');\r\n");
                                templateBuilder.Append("    for(i in tags) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        if(tags[i].id == 'localfile[]') \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("            tags[i].id = 'localfile_' + id;\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("    }\r\n");


                                templateBuilder.Append("    aid++;\r\n");
                                templateBuilder.Append("    $('attachbody').appendChild(newnode);\r\n");
                                templateBuilder.Append("}\r\n");
                                templateBuilder.Append("addAttach();\r\n");
                                templateBuilder.Append("function insertAttach(id) \r\n");
                                templateBuilder.Append("{\r\n");
                                templateBuilder.Append("    var localimgpreview = '';\r\n");
                                templateBuilder.Append("    var path = $('attach_' + id).value;\r\n");
                                templateBuilder.Append("    var extensions = '" + attachextensionsnosize.ToString() + "';\r\n");
                                templateBuilder.Append("    var ext = path.lastIndexOf('.') == -1 ? '' : path.substr(path.lastIndexOf('.') + 1, path.length).toLowerCase();\r\n");
                                templateBuilder.Append("    var re = new RegExp(\"(^|\\\\s|,)\" + ext + \"($|\\\\s|,)\", \"ig\");\r\n");
                                templateBuilder.Append("    var localfile = $('attach_' + id).value.substr($('attach_' + id).value.replace(/\\\\/g, '/').lastIndexOf('/') + 1);\r\n");
                                templateBuilder.Append("    if(path == '') \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        return;\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    if(extensions != '' && (re.exec(extensions) == null || ext == '')) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        alert('对不起，不支持上传此类扩展名的附件。');\r\n");
                                templateBuilder.Append("        return;\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    attachexts[id] = is_ie && in_array(ext, ['gif', 'jpg', 'jpeg', 'png', 'bmp']) ? 2 : 1;\r\n");
                                templateBuilder.Append("    var err = false;\r\n");
                                templateBuilder.Append("    if(attachexts[id] == 2) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        $('img_hidden').alt = id;\r\n");
                                templateBuilder.Append("        try \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("	        $('img_hidden').filters.item(\"DXImageTransform.Microsoft.AlphaImageLoader\").sizingMethod = 'image';\r\n");
                                templateBuilder.Append("        } \r\n");
                                templateBuilder.Append("        catch (e) \r\n");
                                templateBuilder.Append("        {err = true;}\r\n");
                                templateBuilder.Append("        try \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("            $('img_hidden').filters.item(\"DXImageTransform.Microsoft.AlphaImageLoader\").src = $('attach_' + id).value;\r\n");
                                templateBuilder.Append("        } \r\n");
                                templateBuilder.Append("        catch (e) \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("			alert('无效的图片文件。');\r\n");
                                templateBuilder.Append("			delAttach(id);\r\n");
                                templateBuilder.Append("			err = true;		\r\n");
                                templateBuilder.Append("            return;\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        var wh = {'w' : $('img_hidden').offsetWidth, 'h' : $('img_hidden').offsetHeight};\r\n");
                                templateBuilder.Append("        var aid = $('img_hidden').alt;\r\n");
                                templateBuilder.Append("        if(wh['w'] >= thumbwidth || wh['h'] >= thumbheight) \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("            wh = thumbImg(wh['w'], wh['h']);\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        attachwh[id] = wh;\r\n");
                                templateBuilder.Append("        $('img_hidden').style.width = wh['w']\r\n");
                                templateBuilder.Append("        $('img_hidden').style.height = wh['h'];\r\n");
                                templateBuilder.Append("        try \r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("	        $('img_hidden').filters.item(\"DXImageTransform.Microsoft.AlphaImageLoader\").sizingMethod = 'scale';\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        catch (e)\r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        if (err == true)\r\n");
                                templateBuilder.Append("        {\r\n");
                                templateBuilder.Append("	        $('img_hidden').src = $('attach_' + id).value;\r\n");
                                templateBuilder.Append("        }\r\n");
                                templateBuilder.Append("        div = document.createElement('div');\r\n");
                                templateBuilder.Append("        div.id = 'localimgpreview_' + id + '_menu';\r\n");
                                templateBuilder.Append("        div.style.display = 'none';\r\n");
                                templateBuilder.Append("        div.style.marginLeft = '20px';\r\n");
                                templateBuilder.Append("        div.className = 'popupmenu_popup';\r\n");
                                templateBuilder.Append("        document.body.appendChild(div);\r\n");
                                templateBuilder.Append("        div.innerHTML = '<img style=\"filter:progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod=\\'scale\\',src=\\'' + $('attach_' + id).value.replace(')','%29').replace('(','%28') +'\\');width:'+wh['w']+';height:'+wh['h']+'\" src=\\'images/common/none.gif\\' border=\"0\" aid=\"attach_'+ aid +'\" alt=\"\" />';\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    var isimg = in_array(ext, ['gif', 'jpg', 'jpeg', 'png', 'bmp']) ? 2 : 1;\r\n");


                                templateBuilder.Append("    $('localfile_' + id).innerHTML = '<a href=\"###delAttach\" onclick=\"delAttach(' + id + ')\">[删除]</a> <a href=\"###insertAttach\" title=\"点击这里将本附件插入帖子内容中当前光标的位置\" onclick=\"insertAttachtext(' + id + ', ' + err + ');return false;\">[插入]</a> ' +\r\n");
                                templateBuilder.Append("    (attachexts[id] == 2 ? '<span id=\"localimgpreview_' + id + '\" onmouseover=\"showMenu(this.id, 0, 0, 1, 0)\"> <span class=\"smalltxt\">[' + id + ']</span> <a href=\"###attachment\" onclick=\"insertAttachtext(' + id + ', ' + err + ');return false;\">' + localfile + '</a></span>' : '<span class=\"smalltxt\">[' + id + ']</span> ' + localfile);\r\n");
                                templateBuilder.Append("    $('attach_' + id).style.display = 'none';\r\n");
                                templateBuilder.Append("    /*if(isimg == 2)\r\n");
                                templateBuilder.Append("        insertAttachtext(id, err);*/\r\n");
                                templateBuilder.Append("    addAttach();\r\n");
                                templateBuilder.Append("}\r\n");
                                templateBuilder.Append("function insertAttachtext(id, iserr) \r\n");
                                templateBuilder.Append("{\r\n");
                                templateBuilder.Append("    if(!attachexts[id]) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        return;\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    if(attachexts[id] == 2) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        bbinsert && wysiwyg && iserr == false ? insertText($('localimgpreview_' + id + '_menu').innerHTML, false) : AddText('[localimg=' + attachwh[id]['w'] + ',' + attachwh[id]['h'] + ']' + id + '[/localimg]');\r\n");
                                templateBuilder.Append("    } \r\n");
                                templateBuilder.Append("    else \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        bbinsert && wysiwyg ? insertText('[local]' + id + '[/local]', false) : AddText('[local]' + id + '[/local]');\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("}\r\n");
                                templateBuilder.Append("function thumbImg(w, h) \r\n");
                                templateBuilder.Append("{\r\n");
                                templateBuilder.Append("    var x_ratio = thumbwidth / w;\r\n");
                                templateBuilder.Append("    var y_ratio = thumbheight / h;\r\n");
                                templateBuilder.Append("    var wh = new Array();\r\n");
                                templateBuilder.Append("    if((x_ratio * h) < thumbheight) \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        wh['h'] = Math.ceil(x_ratio * h);\r\n");
                                templateBuilder.Append("        wh['w'] = thumbwidth;\r\n");
                                templateBuilder.Append("    } \r\n");
                                templateBuilder.Append("    else \r\n");
                                templateBuilder.Append("    {\r\n");
                                templateBuilder.Append("        wh['w'] = Math.ceil(y_ratio * w);\r\n");
                                templateBuilder.Append("        wh['h'] = thumbheight;\r\n");
                                templateBuilder.Append("    }\r\n");
                                templateBuilder.Append("    return wh;\r\n");
                                templateBuilder.Append("}\r\n");
                                templateBuilder.Append("</" + "script>\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("		<div class=\"hintinfo\">							\r\n");
                                templateBuilder.Append("				你没有上传附件的权限.\r\n");
                                templateBuilder.Append("		</div>\r\n");

                            }	//end if


                        }
                        else
                        {

                            templateBuilder.Append("	<div class=\"hintinfo\">\r\n");

                            if (usergroupinfo.Maxsizeperday > 0 && usergroupinfo.Maxattachsize > 0)
                            {

                                templateBuilder.Append("			你目前可上传的附件大小为 0 字节.\r\n");

                            }
                            else
                            {

                                templateBuilder.Append("			你没有上传附件的权限.\r\n");

                            }	//end if

                            templateBuilder.Append("	</div>\r\n");

                        }	//end if




                    }	//end if

                    templateBuilder.Append("		</div>\r\n");
                    templateBuilder.Append("	</td>\r\n");
                    templateBuilder.Append("	</tr>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("		if(!(is_ie >= 5 || is_moz >= 2)) {\r\n");
                    templateBuilder.Append("			$('restoredata').style.display = 'none';\r\n");
                    templateBuilder.Append("		}\r\n");
                    templateBuilder.Append("		var editorid = 'posteditor';\r\n");
                    templateBuilder.Append("		var textobj = $(editorid + '_textarea');\r\n");
                    templateBuilder.Append("		var special = parseInt('0');\r\n");
                    templateBuilder.Append("		var charset = 'utf-8';\r\n");
                    templateBuilder.Append("	</" + "script>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("		var thumbwidth = parseInt(400);\r\n");
                    templateBuilder.Append("		var thumbheight = parseInt(300);\r\n");
                    templateBuilder.Append("		var extensions = '';\r\n");
                    templateBuilder.Append("		lang['post_attachment_ext_notallowed']	= '对不起，不支持上传此类扩展名的附件。';\r\n");
                    templateBuilder.Append("		lang['post_attachment_img_invalid']		= '无效的图片文件。';\r\n");
                    templateBuilder.Append("		lang['post_attachment_deletelink']		= '删除';\r\n");
                    templateBuilder.Append("		lang['post_attachment_insert']			= '点击这里将本附件插入帖子内容中当前光标的位置';\r\n");
                    templateBuilder.Append("		lang['post_attachment_insertlink']		= '插入';\r\n");
                    templateBuilder.Append("	</" + "script>\r\n");
                    templateBuilder.Append("	<script src=\"javascript/post.js\" type=\"text/javascript\"></" + "script>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("		var fontoptions = new Array(\"仿宋_GB2312\", \"黑体\", \"楷体_GB2312\", \"宋体\", \"新宋体\", \"微软雅黑\", \"Trebuchet MS\", \"Tahoma\", \"Arial\", \"Impact\", \"Verdana\", \"Times New Roman\");\r\n");
                    templateBuilder.Append("		lang['enter_list_item']			= \"输入一个列表项目.\\r\\n留空或者点击取消完成此列表.\";\r\n");
                    templateBuilder.Append("		lang['enter_link_url']			= \"请输入链接的地址:\";\r\n");
                    templateBuilder.Append("		lang['enter_image_url']			= \"请输入图片链接地址:\";\r\n");
                    templateBuilder.Append("		lang['enter_email_link']		= \"请输入此链接的邮箱地址:\";\r\n");
                    templateBuilder.Append("		lang['fontname']				= \"字体\";\r\n");
                    templateBuilder.Append("		lang['fontsize']				= \"大小\";\r\n");
                    templateBuilder.Append("		lang['post_advanceeditor']		= \"全部功能\";\r\n");
                    templateBuilder.Append("		lang['post_simpleeditor']		= \"简单功能\";\r\n");
                    templateBuilder.Append("		lang['submit']					= \"提交\";\r\n");
                    templateBuilder.Append("		lang['cancel']					= \"取消\";\r\n");
                    templateBuilder.Append("	</" + "script>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/editor.js\"></" + "script>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/post_editor.js\"></" + "script>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                    //templateBuilder.Append("		newEditor(wysiwyg);\r\n");
                    //templateBuilder.Append("		$(editorid + '_contract').onclick = function() { resizeEditor(-100) };\r\n");
                    //templateBuilder.Append("		$(editorid + '_expand').onclick = function() { resizeEditor(100) };\r\n");
                    //templateBuilder.Append("		$('checklength').onclick = function() { checklength($('postform')) };\r\n");
                    //templateBuilder.Append("		$('previewbutton').onclick = function() { previewpost() };\r\n");
                    //templateBuilder.Append("		$('clearcontent').onclick = function() { clearcontent() };\r\n");
                    //templateBuilder.Append("		$('restoredata').onclick = function() { loadData() };\r\n");
                    templateBuilder.Append("		$('postform').onsubmit = function() { return validate(this); };\r\n");
                    templateBuilder.Append("		try{ $('title').focus(); }catch(e){ }\r\n");
                    templateBuilder.Append("	</" + "script>\r\n");


                    templateBuilder.Append("			</tbody>\r\n");

                    if (type == "debate")
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"positiveopinion\">正方观点</label></th>\r\n");
                        templateBuilder.Append("					<td><input name=\"positiveopinion\" type=\"text\" id=\"positiveopinion\" style=\"margin-bottom:-1px;\" value=\"\" size=\"80\" maxlength=\"200\" />\r\n");
                        templateBuilder.Append("					(最多 200 字)</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"negativeopinion\">反方观点</label></th>\r\n");
                        templateBuilder.Append("					<td><input name=\"negativeopinion\" type=\"text\" id=\"negativeopinion\" style=\"margin-bottom:-1px;\" value=\"\" size=\"80\" maxlength=\"200\" />\r\n");
                        templateBuilder.Append("					(最多 200 字)</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"terminaltime\">结束时间</label></th>\r\n");
                        templateBuilder.Append("					<td><input type=\"text\" name=\"terminaltime\" id=\"terminaltime\" style=\"margin-bottom:-1px;cursor:pointer;\" size=\"16\" value=\"" + enddatetime.ToString() + "\" onclick=\"showcalendar(event, 'terminaltime', 'cal_startdate', 'cal_enddate', '" + enddatetime.ToString() + "');\" readonly />(日期格式 年-月-日, 如 2008-8-8)\r\n");
                        templateBuilder.Append("					<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" value=\"" + enddatetime.ToString() + "\">\r\n");
                        templateBuilder.Append("					<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" value=\"\">\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("				function doadvdebate()\r\n");
                        templateBuilder.Append("				{\r\n");
                        templateBuilder.Append("					var adv_open = $('advdebate_open');\r\n");
                        templateBuilder.Append("					var adv_close = $('advdebate_close');\r\n");
                        templateBuilder.Append("					if (adv_open && adv_close)\r\n");
                        templateBuilder.Append("					{\r\n");
                        templateBuilder.Append("						if (adv_open.style.display != 'none')\r\n");
                        templateBuilder.Append("						{\r\n");
                        templateBuilder.Append("							adv_open.style.display = 'none';\r\n");
                        templateBuilder.Append("							adv_close.style.display = '';\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						else\r\n");
                        templateBuilder.Append("						{\r\n");
                        templateBuilder.Append("							adv_open.style.display = '';\r\n");
                        templateBuilder.Append("							adv_close.style.display = 'none';\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("				}\r\n");
                        templateBuilder.Append("			</" + "script>\r\n");

                    }	//end if


                    if (enabletag)
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"positiveopinion\">标签(Tags)</label></th>\r\n");
                        templateBuilder.Append("					<td><input type=\"text\" name=\"tags\" id=\"tags\" value=\"\" style=\"margin-bottom:-1px;\" size=\"55\" />&nbsp;<input type=\"button\" onclick=\"relatekw();\" value=\"可用标签\"/>  (用空格隔开多个标签，最多可填写 5 个)</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tbody class=\"divoption\">\r\n");
                    templateBuilder.Append("				<tr>\r\n");
                    templateBuilder.Append("					<th>&nbsp;</th>					\r\n");
                    templateBuilder.Append("					<td><a href=\"###\" id=\"advoption\" onclick=\"expandoptions('divAdvOption');\">其他选项<img src=\"templates/" + templatepath.ToString() + "/images/dot-down2.gif\" /></a></td>	\r\n");
                    templateBuilder.Append("				</tr>\r\n");
                    templateBuilder.Append("			</tbody>\r\n");
                    templateBuilder.Append("			<tbody  id=\"divAdvOption\" style=\"display:none\">\r\n");

                    if (userid != -1 && usergroupinfo.Allowsetreadperm == 1)
                    {

                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"topicreadperm\">阅读权限</label></th>\r\n");
                        templateBuilder.Append("					<td>\r\n");
                        templateBuilder.Append("						<input name=\"topicreadperm\" type=\"text\" id=\"topicreadperm\" value=\"0\" size=\"5\" />(阅读权限的最大有效值为 255)\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");

                    }	//end if


                    if (type == "")
                    {


                        if (creditstrans != 0 && maxprice > 0)
                        {

                            templateBuilder.Append("				<tr>\r\n");
                            templateBuilder.Append("					<th><label for=\"positiveopinion\">售价</label></th>\r\n");
                            templateBuilder.Append("					<td>\r\n");
                            templateBuilder.Append("					<input name=\"topicprice\" type=\"text\" id=\"topicprice\" value=\"0\" size=\"5\" />\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				&nbsp;&nbsp;[ 主题最高售价 " + maxprice.ToString() + " \r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                            templateBuilder.Append("		 ](售价只允许非负整数, 单个主题最大收入 " + Scoresets.GetMaxIncPerTopic().ToString().Trim() + " " + userextcreditsinfo.Unit.ToString().Trim() + "" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                            templateBuilder.Append("					</td>\r\n");
                            templateBuilder.Append("				</tr>\r\n");

                        }	//end if

                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"title\">图标</label></th>\r\n");
                        templateBuilder.Append("					<td>\r\n");
                        templateBuilder.Append("						<input name=\"iconid\" type=\"radio\" value=\"0\" checked /> 无&nbsp;\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"1\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/1.gif\" alt=\"求助\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"2\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/2.gif\" alt=\"示好\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"3\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/3.gif\" alt=\"贡献\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"4\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/4.gif\" alt=\"音乐\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"5\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/5.gif\" alt=\"光碟\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"6\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/6.gif\" alt=\"游戏\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"7\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/7.gif\" alt=\"照片\"/><br/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"8\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/8.gif\" alt=\"诈唬\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"9\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/9.gif\" alt=\"播放\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"10\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/10.gif\" alt=\"点火\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"11\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/11.gif\" alt=\"体育\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"12\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/12.gif\" alt=\"提示\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"13\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/13.gif\" alt=\"阳光\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"14\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/14.gif\" alt=\"代码脚本\"/>\r\n");
                        templateBuilder.Append("						<input type=\"radio\" name=\"iconid\" value=\"15\" />\r\n");
                        templateBuilder.Append("						<img src=\"images/posticons/15.gif\" alt=\"玫瑰\"/>\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>			\r\n");

                    }	//end if


                    if (isseccode)
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th><label for=\"vcode\">验证码</label></th>\r\n");
                        templateBuilder.Append("				<td>\r\n");

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
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"" + forum.Postbytopictype.ToString().Trim() + "\" tabindex=\"3\">\r\n");
                    templateBuilder.Append("					<input name=\"topicsubmit\" type=\"submit\" id=\"postsubmit\" value=\"发表主题\"/>\r\n");

                    if (spaceid > 0 && type == "")
                    {

                        templateBuilder.Append("<input type=\"checkbox\" name=\"addtoblog\" /> 添加到个人空间\r\n");

                    }	//end if

                    templateBuilder.Append(" [完成后可按 Ctrl + Enter 发布]\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("	</form>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("	if (getQueryString('restore') == '1')\r\n");
                    templateBuilder.Append("	{\r\n");
                    templateBuilder.Append("		loadData(true);\r\n");
                    templateBuilder.Append("	}\r\n");
                    templateBuilder.Append("	</" + "script>\r\n");
                    templateBuilder.Append("	</div>\r\n");

                }	//end if


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


                    templateBuilder.Append("		<script type=\"text/javascript\">setcookie(\"dnt_title\", '', 1);</" + "script>\r\n");

                }	//end if

                templateBuilder.Append("	</div>\r\n");

            }	//end if

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
