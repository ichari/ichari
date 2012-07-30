using System;
using System.Collections;
using System.Data;
using System.Text;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Web.UI;
using Discuz.Data;
using System.IO;
using Discuz.Common.Generic;
using System.Text.RegularExpressions;

namespace Discuz.Web
{
    /// <summary>
    /// 编辑帖子页面
    /// </summary>
    public partial class editpost : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 帖子所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 帖子所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 帖子信息
        /// </summary>
        public PostInfo postinfo;
        /// <summary>
        /// 帖子所属主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 投票选项列表
        /// </summary>
        public DataTable polloptionlist;
        /// <summary>
        /// 投票帖类型
        /// </summary>
        public PollInfo pollinfo;
        /// <summary>
        /// 附件列表
        /// </summary>
        public DataTable attachmentlist;
        /// <summary>
        /// 附件数
        /// </summary>
        public int attachmentcount;
        /// <summary>
        /// 投票截止时间
        /// </summary>
        //public string pollenddatetime;
        /// <summary>
        /// 帖子内容
        /// </summary>
        public string message;
        /// <summary>
        /// 表情的JavaScript数组
        /// </summary>
        public string smilies;
        /// <summary>
        /// 自定义编辑器按钮
        /// </summary>
        public string customeditbuttons;
        /// <summary>
        /// 主题图标
        /// </summary>
        public string topicicons;
        /// <summary>
        /// 是否进行URL解析
        /// </summary>
        public int parseurloff;
        /// <summary>
        /// 是否进行表情解析
        /// </summary>
        public int smileyoff;
        /// <summary>
        /// 是否进行Discuz!NT代码解析
        /// </summary>
        public int bbcodeoff;
        /// <summary>
        /// 是否使用签名
        /// </summary>
        public int usesig;
        /// <summary>
        /// 是否允许[img]代码
        /// </summary>
        public int allowimg;
        /// <summary>
        /// 是否受发帖控制限制
        /// </summary>
        public int disablepostctrl;
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
        /// 当前版块的主题类型选项
        /// </summary>
        public string topictypeselectoptions;
        /// <summary>
        /// 表情分类
        /// </summary>
        public DataTable smilietypes;
        /// <summary>
        /// 相册列表
        /// </summary>
        public DataTable albumlist;
        /// <summary>
        /// 是否允许上传附件
        /// </summary>
        public bool canpostattach;
        /// <summary>
        /// 是否允许将图片放入相册
        /// </summary>
        public bool caninsertalbum;
        /// <summary>
        /// 是否显示下载链接
        /// </summary>		
        public bool allowviewattach = false;
        /// <summary>
        /// 是否有Html标题的权限
        /// </summary>
        public bool canhtmltitle = false;
        /// <summary>
        /// 当前帖的Html标题
        /// </summary>
        public string htmltitle = "";
        /// <summary>
        /// 第一页表情的JSON
        /// </summary>
        public string firstpagesmilies = string.Empty;
        /// <summary>
        /// 主题所用标签
        /// </summary>
        public string topictags = string.Empty;
        /// <summary>
        /// 本版是否启用了Tag
        /// </summary>
        public bool enabletag = false;

        #endregion
        // 是否允许编辑帖子, 初始false为不允许
        bool alloweditpost = false;

        protected override void ShowPage()
        {

            forumnav = "";
            //maxprice = usergroupinfo.Maxprice > Scoresets.GetMaxIncPerTopic() ? Scoresets.GetMaxIncPerTopic() : usergroupinfo.Maxprice;
            maxprice = usergroupinfo.Maxprice;
            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
            firstpagesmilies = Caches.GetSmiliesFirstPageCache();
            this.disablepostctrl = 0;
            if (admininfo != null)
            {
                this.disablepostctrl = admininfo.Disablepostctrl;
            }

            int topicid = DNTRequest.GetInt("topicid", -1);
            int postid = DNTRequest.GetInt("postid", -1);

            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }

            // 如果帖子ID非数字
            if (postid == -1)
            {
                //
                AddErrLine("无效的帖子ID");

                return;
            }

            postinfo = Posts.GetPostInfo(topicid, postid);
            // 如果帖子不存在
            if (postinfo == null)
            {
                //
                AddErrLine("不存在的帖子ID");

                return;
            }


            // 获取主题ID
            if (topicid != postinfo.Tid)
            {
                AddErrLine("主题ID无效");
                return;
            }

            // 如果主题ID非数字
            if (topicid == -1)
            {
                //
                AddErrLine("无效的主题ID");

                return;
            }

            // 获取该主题的信息
            topic = Topics.GetTopicInfo(topicid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");

                return;
            }

            //非创始人且作者与当前编辑者不同时
            if (postinfo.Posterid != userid && BaseConfigs.GetFounderUid != userid)
            {
                if (postinfo.Posterid == BaseConfigs.GetFounderUid)
                {
                    AddErrLine("您无权编辑创始人的帖子");
                    return;
                }
                if (postinfo.Posterid != -1)
                {
                    UserGroupInfo postergroup = UserGroups.GetUserGroupInfo(Discuz.Forum.Users.GetShortUserInfo(postinfo.Posterid).Groupid);
                    if (postergroup.Radminid > 0 && postergroup.Radminid < useradminid)
                    {
                        AddErrLine("您无权编辑更高权限人的帖子");
                        return;
                    }
                }
            }

            pagetitle = postinfo.Title;

            ///得到所在版块信息
            forumid = topic.Fid;
            forum = Forums.GetForumInfo(forumid);


            // 如果该版块不存在
            if (forum == null)
            {
                AddErrLine("版块已不存在");
                forum = new ForumInfo();
                return;
            }

            if (forum.Password != "" && Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                SetBackLink(base.ShowForumAspxRewrite(forumid, 0));
                return;
            }

            forumname = forum.Name;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            if (forum.Applytopictype == 1)  //启用主题分类
            {
                topictypeselectoptions = Forums.GetCurrentTopicTypesOption(forum.Fid, forum.Topictypes);
            }

            //得到用户可以上传的文件类型
            System.Text.StringBuilder sbAttachmentTypeSelect = new StringBuilder();
            if (!usergroupinfo.Attachextensions.Trim().Equals(""))
            {
                sbAttachmentTypeSelect.Append("[id] in (");
                sbAttachmentTypeSelect.Append(usergroupinfo.Attachextensions);
                sbAttachmentTypeSelect.Append(")");
            }

            if (!forum.Attachextensions.Trim().Equals(""))
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
            //得到今天允许用户上传的附件总大上(字节)
            int MaxTodaySize = 0;
            if (userid > 0)
            {
                MaxTodaySize = Attachments.GetUploadFileSizeByuserid(userid);		//今天已上传大小
            }
            attachsize = usergroupinfo.Maxsizeperday - MaxTodaySize;


            //-------------设置帖子的可用功能allhtml,smileyoff,parseurlof,bbcodeoff
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append("var Allowhtml=1;\r\n"); //+ allhtml.ToString() + "

            parseurloff = 0;

            smileyoff = 1 - forum.Allowsmilies;
            //sb.Append("var Allowsmilies=" + (1-smileyoff).ToString() + ";\r\n");


            bbcodeoff = 1;
            if (forum.Allowbbcode == 1)
            {
                if (usergroupinfo.Allowcusbbcode == 1)
                {
                    bbcodeoff = 0;
                }
            }
            //sb.Append("var Allowbbcode=" + (1-bbcodeoff).ToString() + ";\r\n");

            usesig = 1;

            allowimg = forum.Allowimgcode;
            //sb.Append("var Allowimgcode=" + allowimg.ToString() + ";\r\n");



            //AddScript(sb.ToString());

            //---------------


            parseurloff = postinfo.Parseurloff;

            if (!DNTRequest.IsPost())
            {
                smileyoff = postinfo.Smileyoff;
            }

            bbcodeoff = 1;
            if (usergroupinfo.Allowcusbbcode == 1)
            {
                bbcodeoff = postinfo.Bbcodeoff;
            }
            usesig = postinfo.Usesig;


            if (!Forums.AllowViewByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (forum.Viewperm == null || forum.Viewperm == string.Empty)//当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                        return;
                    }
                }
                else//当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该版块的权限");
                        return;
                    }
                }
            }


            //当前用户是否有允许下载附件权限
            if (Forums.AllowGetAttachByUserID(forum.Permuserlist, userid))
            {
                allowviewattach = true;
            }
            else
            {
                if (forum.Getattachperm == null || forum.Getattachperm == string.Empty)//权限设置为空时，根据用户组权限判断
                {
                    // 验证用户是否有有允许下载附件权限
                    if (usergroupinfo.Allowgetattach == 1)
                    {
                        allowviewattach = true;
                    }
                }
                else if (Forums.AllowGetAttach(forum.Getattachperm, usergroupid))
                {
                    allowviewattach = true;
                }
            }


            //是否有上传附件的权限
            if (Forums.AllowPostAttachByUserID(forum.Permuserlist, userid))
            {
                canpostattach = true;
            }
            else
            {
                if (forum.Postattachperm == null || forum.Postattachperm == "")
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


            // 判断当前用户是否有修改权限
            // 检查是否具有版主的身份
            //if (!Moderators.IsModer(useradminid, userid, forumid) || AdminGroups.GetAdminGroupInfo(useradminid) == null)
            if (!Moderators.IsModer(useradminid, userid, forumid))
            {
                if (postinfo.Posterid != userid)
                {
                    AddErrLine("你并非作者, 且你当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有修改该帖的权限");

                    return;
                }
                else if (config.Edittimelimit > 0 && Utils.StrDateDiffMinutes(postinfo.Postdatetime, config.Edittimelimit) > 0)
                {
                    AddErrLine("抱歉, 系统规定只能在帖子发表" + config.Edittimelimit.ToString() + "分钟内才可以修改");
                    return;

                }

            }



            userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetCreditsTrans());

            //bool allowvote = false;
            if (topic.Special == 1 && postinfo.Layer == 0)
            {
                pollinfo = Polls.GetPollInfo(topicid);

                //if (Polls.GetVoters(topicid, userid, username, out allowvote).Equals(""))
                {
                    polloptionlist = Polls.GetPollOptionList(topicid);
                }
            }
            if (postinfo.Layer == 0)
            {
                canhtmltitle = config.Htmltitle == 1 && Utils.InArray(usergroupid.ToString(), config.Htmltitleusergroup);
            }

            attachmentlist = Attachments.GetAttachmentListByPid(postid);
            attachmentcount = attachmentlist.Rows.Count;

            ShortUserInfo user = Discuz.Forum.Users.GetShortUserInfo(userid);

            caninsertalbum = false;

            if (Topics.GetMagicValue(topic.Magic, MagicType.HtmlTitle) == 1)
            {
                htmltitle = Topics.GetHtmlTitle(topic.Tid).Replace("\"", "\\\"").Replace("'", "\\'");
            }

            enabletag = (config.Enabletag & forum.Allowtag) == 1;
            if (enabletag && Topics.GetMagicValue(topic.Magic, MagicType.TopicTag) == 1)
            {
                List<TagInfo> tags = ForumTags.GetTagsListByTopic(topic.Tid);

                foreach (TagInfo tag in tags)
                {
                    if (tag.Orderid > -1)
                    {
                        topictags += string.Format(" {0}", tag.Tagname);
                    }
                }
                topictags = topictags.Trim();
            }

            if (!ispost)
            {
                AddLinkCss("/" + "templates/" + templatepath + "/editor.css", "css");

                // 帖子内容
                message = postinfo.Message;

                smilies = Caches.GetSmiliesCache();
                smilietypes = Caches.GetSmilieTypesCache();
                customeditbuttons = Caches.GetCustomEditButtonList();
                topicicons = Caches.GetTopicIconsCache();
            }
            else
            {

                SetBackLink("editpost.aspx?topicid=" + postinfo.Tid + "&postid=" + this.postinfo.Pid.ToString());

                if (ForumUtils.IsCrossSitePost())
                {
                    AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
                    return;
                }

                if (postinfo.Layer == 0 && forum.Applytopictype == 1 && forum.Postbytopictype == 1 && topictypeselectoptions != string.Empty)
                {
                    if (DNTRequest.GetString("typeid").Trim().Equals("") || DNTRequest.GetString("typeid").Trim().Equals("0"))
                    {
                        AddErrLine("主题类型不能为空");
                        return;
                    }

                    if (!Forums.IsCurrentForumTopicType(DNTRequest.GetString("typeid").Trim(), forum.Topictypes))
                    {
                        AddErrLine("错误的主题类型");
                        return;
                    }
                }

                ///删除附件
                if (DNTRequest.GetInt("isdeleteatt", 0) == 1)
                {
                    int aid = DNTRequest.GetFormInt("aid", 0);
                    if (aid > 0)
                    {
                        if (Attachments.DeleteAttachment(aid) > 0)
                        {
                            attachmentlist = Attachments.GetAttachmentListByPid(postid);
                            attachmentcount = Attachments.GetAttachmentCountByPid(postid);
                        }
                    }

                    AddLinkCss("/" + "templates/" + templatepath + "/editor.css", "css");

                    // 帖子内容
                    message = postinfo.Message;

                    smilies = Caches.GetSmiliesCache();
                    customeditbuttons = Caches.GetCustomEditButtonList();
                    topicicons = Caches.GetTopicIconsCache();

                    ispost = false;

                    return;
                }

                if (DNTRequest.GetString("title").Trim().Equals("") && postinfo.Layer == 0)
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

                string postmessage = DNTRequest.GetString("message");
                if (postmessage.Equals(""))
                {
                    AddErrLine("内容不能为空");
                }


                if (admininfo != null && this.disablepostctrl != 1)
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


                string enddatetime = DNTRequest.GetString("enddatetime");
                string[] pollitem = { };

                if (!DNTRequest.GetString("updatepoll").Equals("") && topic.Special == 1)
                {

                    pollinfo.Multiple = DNTRequest.GetInt("multiple", 0);

                    // 验证用户是否有发布投票的权限
                    if (usergroupinfo.Allowpostpoll != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有发布投票的权限");
                        return;
                    }


                    //createpoll = true;
                    pollitem = Utils.SplitString(DNTRequest.GetString("PollItemname"), "\r\n");
                    if (pollitem.Length < 2)
                    {
                        AddErrLine("投票项不得少于2个");
                    }
                    else if (pollitem.Length > config.Maxpolloptions)
                    {
                        AddErrLine("系统设置为投票项不得多于" + config.Maxpolloptions + "个");
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
                }

                int topicprice = 0;
                string tmpprice = DNTRequest.GetString("topicprice");

                if (Regex.IsMatch(tmpprice, "^[0-9]*[0-9][0-9]*$") || tmpprice == string.Empty)
                {
                    if (topic.Special != 2)
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
                    if (topic.Special != 2)
                    {
                        AddErrLine("主题售价只能为整数");
                    }
                    else
                    {
                        AddErrLine("悬赏价格只能为整数");
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


                string curdatetime = Utils.GetDateTime();


                if (useradminid == 1)
                {
                    postinfo.Title = Utils.HtmlEncode(DNTRequest.GetString("title"));
                    postinfo.Message = Shove._Web.Utility.FilteSqlInfusion(DNTRequest.GetString("message")).Replace("<hide>", "[hide]").Replace("</hide>", "[/hide]");
                    //postinfo.Message = Utils.HtmlEncode(DNTRequest.GetString("message"));
                }
                else
                {
                    postinfo.Title = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("title")));
                    postinfo.Message = Shove._Web.Utility.FilteSqlInfusion(DNTRequest.GetString("message")).Replace("<hide>", "[hide]").Replace("</hide>", "[/hide]");
                    //postinfo.Message = Utils.HtmlEncode(ForumUtils.BanWordFilter(DNTRequest.GetString("message")));
                }


                if (ForumUtils.HasBannedWord(postinfo.Title) || ForumUtils.HasBannedWord(postinfo.Message))
                {
                    AddErrLine("对不起, 您提交的内容包含不良信息, 因此无法提交, 请返回修改!");
                    return;
                }

                if (useradminid != 1)
                {
                    if (ForumUtils.HasAuditWord(postinfo.Title) || ForumUtils.HasAuditWord(postinfo.Message))
                    {
                        AddErrLine("对不起, 管理员设置了需要对发帖进行审核, 您没有权力编辑已通过审核的帖子, 请返回修改!");
                        return;
                        //postinfo.Invisible = 1;

                        //if (postinfo.Layer == 0) //当为主题帖时
                        //{
                        //    topic.Displayorder = -2;
                        //}
                    }
                }
                //如果是不是管理员组,或者编辑间隔超过60秒,则附加编辑信息
                if (Utils.StrDateDiffSeconds(postinfo.Postdatetime, 60) > 0 && config.Editedby == 1 && useradminid != 1)
                {
                    postinfo.Lastedit = username + " 最后编辑于 " + Utils.GetDateTime();
                }
                postinfo.Usesig = Utils.StrToInt(DNTRequest.GetString("usesig"), 0);
                postinfo.Htmlon = 1;
                postinfo.Smileyoff = smileyoff;
                if (smileyoff == 0)
                {
                    postinfo.Smileyoff = Utils.StrToInt(DNTRequest.GetString("smileyoff"), 0);
                }

                postinfo.Bbcodeoff = 1;
                if (usergroupinfo.Allowcusbbcode == 1)
                {
                    postinfo.Bbcodeoff = Utils.StrToInt(DNTRequest.GetString("bbcodeoff"), 0);
                }
                postinfo.Parseurloff = Utils.StrToInt(DNTRequest.GetString("parseurloff"), 0);

                // 如果所在管理组存在且所在管理组有删帖的管理权限
                if (admininfo != null && admininfo.Alloweditpost == 1 && Moderators.IsModer(useradminid, userid, forumid))
                {
                    alloweditpost = true;
                }
                else if (userid != postinfo.Posterid)
                {
                    AddErrLine("您当前的身份不是作者");
                    return;
                }
                else
                {
                    alloweditpost = true;
                }


                if (alloweditpost)
                {

                    if (postinfo.Layer == 0)
                    {

                        ///修改投票信息
                        StringBuilder itemvaluelist = new StringBuilder("");
                        if (topic.Special == 1)
                        {
                            string pollItemname = Utils.HtmlEncode(DNTRequest.GetFormString("PollItemname"));

                            if (pollItemname != "")
                            {
                                int multiple = DNTRequest.GetString("multiple") == "on" ? 1 : 0;
                                int maxchoices = 0;

                                if (multiple == 1)
                                {
                                    maxchoices = DNTRequest.GetInt("maxchoices", 0);
                                    if (maxchoices > pollitem.Length)
                                    {
                                        maxchoices = pollitem.Length;
                                    }
                                }

                                if (!Polls.UpdatePoll(topicid, multiple, pollitem.Length, DNTRequest.GetFormString("PollOptionID").Trim(), pollItemname.Trim(), DNTRequest.GetFormString("PollOptionDisplayOrder").Trim(), enddatetime, maxchoices, DNTRequest.GetString("visiblepoll") == "on" ? 1 : 0))
                                {
                                    AddErrLine("投票错误,请检查显示顺序");
                                    return;
                                }
                            }
                            else
                            {
                                AddErrLine("投票项为空");
                                return;
                            }
                        }


                        int iconid = DNTRequest.GetInt("iconid", 0);
                        if (iconid > 15 || iconid < 0)
                        {
                            iconid = 0;
                        }

                        topic.Iconid = iconid;
                        topic.Title = postinfo.Title;

                        //悬赏差价处理
                        if (topic.Special == 2)
                        {
                            int pricediff = topicprice - topic.Price;
                            if (pricediff > 0)
                            {
                            }
                            else if (pricediff < 0)
                            {
                                AddErrLine("不能降低悬赏价格");
                                return;
                            }
                        }
                        else if (topic.Special == 0)//普通主题,出售
                        {
                            topic.Price = topicprice;
                        }
                        if (usergroupinfo.Allowsetreadperm == 1)
                        {
                            int topicreadperm = DNTRequest.GetInt("topicreadperm", 0);
                            topicreadperm = topicreadperm > 255 ? 255 : topicreadperm;
                            topic.Readperm = topicreadperm;
                        }
                        if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                        {
                            topic.Hide = 1;
                        }

                        topic.Typeid = DNTRequest.GetFormInt("typeid", 0);

                        string htmltitle = DNTRequest.GetString("htmltitle").Trim();
                        if (htmltitle != string.Empty && Utils.HtmlDecode(htmltitle).Trim() != topic.Title)
                        {
                            topic.Magic = 11000;
                            //按照  附加位/htmltitle(1位)/magic(3位)/以后扩展（未知位数） 的方式来存储
                            //例： 11001
                        }
                        else
                        {
                            topic.Magic = 0;
                        }

                        ForumTags.DeleteTopicTags(topic.Tid);
                        Topics.DeleteRelatedTopics(topic.Tid);
                        string tags = DNTRequest.GetString("tags").Trim();
                        string[] tagArray = null;
                        if (enabletag && tags != string.Empty)
                        {
                            if (ForumUtils.HasBannedWord(tags))
                            {
                                AddErrLine("标签中含有系统禁止词语,请修改");
                                return;
                            }

                            tagArray = Utils.SplitString(tags, " ", true, 10);
                            if (tagArray.Length > 0)
                            {
                                topic.Magic = Topics.SetMagicValue(topic.Magic, MagicType.TopicTag, 1);
                                ForumTags.CreateTopicTags(tagArray, topic.Tid, userid, curdatetime);
                            }
                        }

                        Topics.UpdateTopic(topic);

                        //保存htmltitle
                        if (canhtmltitle && htmltitle != string.Empty && htmltitle != topic.Title)
                        {
                            Topics.WriteHtmlTitleFile(htmltitle, topic.Tid);
                        }


                    }
                    else
                    {
                        if (ForumUtils.IsHidePost(postmessage) && usergroupinfo.Allowhidecode == 1)
                        {
                            topic.Hide = 1;
                        }

                        Topics.UpdateTopicHide(topicid);

                    }

                    // 通过验证的用户可以编辑帖子
                    Posts.UpdatePost(postinfo);






                    sb = new StringBuilder();
                    sb.Remove(0, sb.Length);

                    //编辑帖子时如果进行了批量删除附件
                    string delAttId = DNTRequest.GetFormString("deleteaid");
                    if (delAttId != string.Empty)
                    {
                        if (Utils.IsNumericArray(delAttId.Split(',')))//如果要删除的附件ID列表为数字数组
                        {
                            Attachments.DeleteAttachment(delAttId);

                        }
                    }
                    //编辑帖子时如果进行了更新附件操作
                    string updatedAttId = DNTRequest.GetFormString("attachupdatedid");//被更新的附件Id列表
                    string updateAttId = DNTRequest.GetFormString("attachupdateid");//所有已上传的附件Id列表
                    string[] descriptionArray = DNTRequest.GetFormString("attachupdatedesc").Split(',');//所有已上传的附件的描述
                    string[] readpermArray = DNTRequest.GetFormString("attachupdatereadperm").Split(',');//所有已上传得附件的阅读权限

                    ArrayList updateAttArrayList = new ArrayList();
                    if (updateAttId != string.Empty)
                    {
                        foreach (string s in updateAttId.Split(','))
                        {
                            if (!Utils.InArray(s, delAttId, ","))//已上传的附件Id不在被删除的附件Id列表中时
                            {
                                updateAttArrayList.Add(s);
                            }
                        }
                    }

                    string[] updateAttArray = (string[])updateAttArrayList.ToArray(typeof(string));

                    if (updateAttId != string.Empty)//原来有附件
                    {
                        //						if (updatedAttId != string.Empty)//原来的附件有更新
                        //						{
                        int watermarkstate = config.Watermarkstatus;

                        if (forum.Disablewatermark == 1)
                            watermarkstate = 0;

                        string[] updatedAttArray = updatedAttId.Split(',');

                        string filekey = "attachupdated";

                        //保存新的文件
                        AttachmentInfo[] attArray =
                            ForumUtils.SaveRequestFiles(forumid, config.Maxattachments + updateAttArray.Length,
                                                        usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize,
                                                        attachextensions, watermarkstate, config, filekey);

                        if (Utils.IsNumericArray(updateAttArray))
                        {
                            for (int i = 0; i < updateAttArray.Length; i++) //遍历原来所有附件
                            {
                                string attachmentId = updateAttArray[i];
                                if (Utils.InArray(attachmentId, updatedAttArray)) //附件文件被更新
                                {
                                    if (Utils.InArray(attachmentId, delAttId, ","))//附件进行了删除操作, 则不操作此附件,即使其也被更新
                                    {
                                        continue;
                                    }
                                    //更新附件
                                    int attachmentUpdatedIndex = GetAttachmentUpdatedIndex(attachmentId, updatedAttArray);//获取此次上传的被更新附件在数组中的索引
                                    if (attachmentUpdatedIndex > -1)//附件索引存在
                                    {
                                        if (attArray[attachmentUpdatedIndex].Sys_noupload.Equals(string.Empty)) //由此属性为空可以判断上传成功
                                        {
                                            //获取将被更新的附件信息
                                            AttachmentInfo attachmentInfo =
                                                Attachments.GetAttachmentInfo(Utils.StrToInt(updatedAttArray[attachmentUpdatedIndex], 0));
                                            if (attachmentInfo != null)
                                            {
                                                if (attachmentInfo.Filename.Trim().ToLower().IndexOf("http") < 0)
                                                {
                                                    //删除原来的文件
                                                    File.Delete(
                                                        Utils.GetMapPath("/" + "upload/" + attachmentInfo.Filename));
                                                }

                                                //记住Aid以便稍后更新
                                                attArray[attachmentUpdatedIndex].Aid = attachmentInfo.Aid;
                                                attArray[attachmentUpdatedIndex].Description = descriptionArray[i];
                                                int att_readperm = Utils.StrToInt(readpermArray[i], 0);
                                                att_readperm = att_readperm > 255 ? 255 : att_readperm;
                                                attArray[attachmentUpdatedIndex].Readperm = att_readperm;

                                                Attachments.UpdateAttachment(attArray[attachmentUpdatedIndex]);
                                            }
                                        }
                                        else //上传失败的附件，稍后提示
                                        {
                                            sb.Append("<tr><td align=\"left\">");
                                            sb.Append(attArray[attachmentUpdatedIndex].Attachment);
                                            sb.Append("</td>");
                                            sb.Append("<td align=\"left\">");
                                            sb.Append(attArray[attachmentUpdatedIndex].Sys_noupload);
                                            sb.Append("</td></tr>");
                                        }
                                    }
                                }
                                else //仅修改了阅读权限和描述等
                                {
                                    if (Utils.InArray(updateAttArray[i], delAttId, ","))
                                    {
                                        continue;
                                    }
                                    if ((attachmentlist.Rows[i]["readperm"].ToString() != readpermArray[i]) ||
                                        (attachmentlist.Rows[i]["description"].ToString().Trim() != descriptionArray[i]))
                                    {
                                        int att_readperm = Utils.StrToInt(readpermArray[i], 0);
                                        att_readperm = att_readperm > 255 ? 255 : att_readperm;
                                        Attachments.UpdateAttachment(Utils.StrToInt(updateAttArray[i], 0), att_readperm,
                                                                     descriptionArray[i]);
                                    }
                                }
                            }
                        }
                        //						}
                    }

                    ///上传附件
                    int watermarkstatus = config.Watermarkstatus;
                    if (forum.Disablewatermark == 1)
                    {
                        watermarkstatus = 0;
                    }
                    AttachmentInfo[] attachmentinfo = ForumUtils.SaveRequestFiles(forumid, config.Maxattachments - updateAttArray.Length, usergroupinfo.Maxsizeperday, usergroupinfo.Maxattachsize, MaxTodaySize, attachextensions, watermarkstatus, config, "postfile");
                    if (attachmentinfo != null)
                    {
                        if (attachmentinfo.Length > config.Maxattachments - updateAttArray.Length)
                        {
                            AddErrLine("系统设置为附件不得多于" + config.Maxattachments + "个");
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

                    //编辑后跳转地址
                    if (topic.Special == 4)
                    {
                        //辩论地址
                        SetUrl(Urls.ShowDebateAspxRewrite(topicid));
                    }
                    else if (DNTRequest.GetQueryString("referer") != "")//ajax快速回复将传递referer参数
                    {
                        //SetUrl(Utils.UrlDecode(DNTRequest.GetQueryString("referer")));
                        SetUrl(string.Format("showtopic.aspx?page=end&topicid={0}#{1}", topicid.ToString().Trim(), postinfo.Pid));
                    }
                    else if (DNTRequest.GetQueryString("pageid") != "")//如果不是ajax,则应该是带pageid的参数
                    {
                        if (config.Aspxrewrite == 1)
                        {
                            SetUrl(string.Format("showtopic-{0}-{2}{1}#{3}", topicid.ToString(), config.Extname, DNTRequest.GetString("pageid"), postinfo.Pid));
                        }
                        else
                        {
                            SetUrl(string.Format("showtopic.aspx?&topicid={0}&page={2}#{1}", topicid.ToString(), postinfo.Pid, DNTRequest.GetString("pageid")));
                        }
                    }
                    else//如果都为空.就跳转到第一页(以免意外情况)
                    {
                        if (config.Aspxrewrite == 1)
                        {
                            SetUrl(string.Format("showtopic-{0}{1}", topicid.ToString(), config.Extname));
                        }
                        else
                        {
                            SetUrl(string.Format("showtopic.aspx?&topicid={0}", topicid.ToString()));
                        }
                    }

                    if (sb.Length > 0)
                    {
                        SetMetaRefresh(5);
                        SetShowBackLink(true);
                        sb.Insert(0, "<table cellspacing=\"0\" cellpadding=\"4\" border=\"0\"><tr><td colspan=2 align=\"left\"><span class=\"bold\"><nobr>编辑帖子成功,但以下附件上传失败:</nobr></span><br /></td></tr>");
                        sb.Append("</table>");
                        AddMsgLine(sb.ToString());
                    }
                    else
                    {
                        SetMetaRefresh();
                        SetShowBackLink(false);
                        AddMsgLine("编辑帖子成功, 返回该主题");
                    }
                    // 删除主题游客缓存
                    if (postinfo.Layer == 0)
                    {
                        ForumUtils.DeleteTopicCacheFile(topicid);
                    }
                    return;
                }
                else
                {
                    AddErrLine("您当前的身份没有编辑帖子的权限");
                    return;
                }

            }

        }

        private int GetAttachmentUpdatedIndex(string attachmentId, string[] updatedAttArray)
        {
            for (int i = 0; i < updatedAttArray.Length; i++)
            {
                if (updatedAttArray[i] == attachmentId)
                {
                    return i;
                }
            }

            return -1;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:15.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:15. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_calendar.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("var postminchars = parseInt(" + config.Minpostsize.ToString().Trim() + ");\r\n");
            templateBuilder.Append("var postmaxchars = parseInt(" + config.Maxpostsize.ToString().Trim() + ");\r\n");
            templateBuilder.Append("var disablepostctrl = parseInt(" + disablepostctrl.ToString() + ");\r\n");
            templateBuilder.Append("var tempaccounts = false;\r\n");
            templateBuilder.Append("</" + "script>\r\n");

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

                    templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                    templateBuilder.Append("	<div id=\"nav\">\r\n");
                    templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + " &raquo; <a href=\"showtree.aspx?topicid=" + topic.Tid.ToString().Trim() + "&postid=" + postinfo.Pid.ToString().Trim() + "\">" + topic.Title.ToString().Trim() + "</a>&raquo; 编辑帖子\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>	\r\n");
                    templateBuilder.Append("<div id=\"previewtable\" style=\"display: none\" class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h3>预览帖子</h3>\r\n");
                    templateBuilder.Append("	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"预览帖子\">\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("		<td>" + username.ToString() + "-" + nowdatetime.ToString() + "</td>\r\n");
                    templateBuilder.Append("		<td>\r\n");
                    templateBuilder.Append("			<div id=\"previewmessage\"></span>\r\n");
                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h3>修改帖子</h3>\r\n");
                    templateBuilder.Append("	<form method=\"post\" name=\"postform\" id=\"postform\" action=\"\" enctype=\"multipart/form-data\" onsubmit=\"return validate(this);\">\r\n");
                    templateBuilder.Append("	<table cellspacing=\"0\" cellpadding=\"0\" summary=\"修改帖子\">\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th width=\"200\">用户名</th>\r\n");
                    templateBuilder.Append("			<td>\r\n");

                    if (userid > 0)
                    {

                        templateBuilder.Append("				" + username.ToString() + " [<a href=\"logout.aspx?userkey=" + userkey.ToString() + "\">退出登录</a>]\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("				匿名 [<a href=\"login.aspx\">登录</a>] [<a href=\"register.aspx\">注册</a>]\r\n");

                    }	//end if

                    templateBuilder.Append("			</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th><label for=\"title\">标题</label></th>\r\n");
                    templateBuilder.Append("			<td>\r\n");

                    if (topic.Special == 4)
                    {

                        templateBuilder.Append("			<select id=\"debateopinion\" name=\"debateopinion\">\r\n");
                        templateBuilder.Append("			<option selected=\"\" value=\"0\"/>\r\n");
                        templateBuilder.Append("			<option value=\"1\">正方</option>\r\n");
                        templateBuilder.Append("			<option value=\"2\">反方</option>\r\n");
                        templateBuilder.Append("			</select>\r\n");
                        templateBuilder.Append("			<script type=\"text/javascript\">$('debateopinion').selectedIndex = parseInt(getQueryString(\"debate\"));</" + "script>\r\n");

                    }	//end if


                    if (postinfo.Layer == 0 && forum.Applytopictype == 1)
                    {


                        if (topictypeselectoptions != "")
                        {

                            templateBuilder.Append("				<select name=\"typeid\" id=\"typeid\">" + topictypeselectoptions.ToString() + "</select>\r\n");
                            templateBuilder.Append("				<script>document.getElementById('typeid').value='" + topic.Typeid.ToString().Trim() + "';</" + "script>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("			<input name=\"title\" type=\"text\" id=\"title\" value='" + postinfo.Title.ToString().Trim() + "' size=\"60\" title=\"标题最多为60个字符\" />\r\n");

                    if (postinfo.Layer > 0)
                    {

                        templateBuilder.Append("			<em class=\"tips\">(可选)</em>\r\n");

                    }	//end if


                    if (canhtmltitle)
                    {

                        templateBuilder.Append("			<a href=\"###\" id=\"titleEditorButton\" onclick=\"\">高级编辑</a>\r\n");
                        templateBuilder.Append("			<script type=\"text/javascript\" src=\"javascript/dnteditor.js\"></" + "script>\r\n");
                        templateBuilder.Append("			<div id=\"titleEditorDiv\" style=\"display: none;\">\r\n");
                        templateBuilder.Append("				<textarea name=\"htmltitle\" id=\"htmltitle\" cols=\"80\" rows=\"10\"></textarea>\r\n");
                        templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                        templateBuilder.Append("					var forumpath = '" + forumpath.ToString() + "';\r\n");
                        templateBuilder.Append("					var templatepath = '" + templatepath.ToString() + "';\r\n");
                        templateBuilder.Append("					var temptitle = $('faketitle');\r\n");
                        templateBuilder.Append("					var titleEditor = null;\r\n");
                        templateBuilder.Append("					function AdvancedTitleEditor() {\r\n");
                        templateBuilder.Append("						$('title').style.display = 'none';\r\n");
                        templateBuilder.Append("						$('titleEditorDiv').style.display = '';\r\n");
                        templateBuilder.Append("						$('titleEditorButton').style.display = 'none';\r\n");
                        templateBuilder.Append("						titleEditor = new DNTeditor('htmltitle', '500', '50', '" + htmltitle.ToString() + "'==''?$('title').value:'" + htmltitle.ToString() + "');\r\n");
                        templateBuilder.Append("						titleEditor.OnChange = function(){\r\n");
                        templateBuilder.Append("							//temptitle.innerHTML = html2bbcode(titleEditor.GetHtml().replace(/<[^>]*>/ig, ''));\r\n");
                        templateBuilder.Append("						}\r\n");
                        templateBuilder.Append("						titleEditor.Basic = true;\r\n");
                        templateBuilder.Append("						titleEditor.IsAutoSave = false;\r\n");
                        templateBuilder.Append("						titleEditor.Style = forumpath + 'templates/' + templatepath + '/editor.css';\r\n");
                        templateBuilder.Append("						titleEditor.BasePath = forumpath;\r\n");
                        templateBuilder.Append("						titleEditor.ReplaceTextarea();\r\n");
                        templateBuilder.Append("					}\r\n");
                        templateBuilder.Append("					$('titleEditorButton').onclick = function(){\r\n");
                        templateBuilder.Append("						AdvancedTitleEditor();\r\n");
                        templateBuilder.Append("					};\r\n");
                        templateBuilder.Append("				</" + "script>\r\n");
                        templateBuilder.Append("			</div>\r\n");

                        if (htmltitle != "")
                        {

                            templateBuilder.Append("			<script type=\"text/javascript\">				\r\n");
                            templateBuilder.Append("				AdvancedTitleEditor();\r\n");
                            templateBuilder.Append("			</" + "script>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                    if (postinfo.Layer == 0)
                    {


                        if (topic.Special == 1)
                        {

                            templateBuilder.Append("		<!--投票开始-->		\r\n");
                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th><label for=\"enddatetime\">投票结束日期:</label></th>\r\n");
                            templateBuilder.Append("			<td>				\r\n");
                            templateBuilder.Append("				<input name=\"enddatetime\" type=\"text\" id=\"enddatetime\" size=\"10\" value=\"" + pollinfo.Expiration.ToString().Trim() + "\" style=\"cursor:default\" onClick=\"showcalendar(event, 'enddatetime', 'cal_startdate', 'cal_enddate', '" + nowdate.ToString() + "');\" readonly=\"readonly\" /></span>\r\n");
                            templateBuilder.Append("				<input type=\"hidden\" name=\"cal_startdate\" id=\"cal_startdate\" size=\"10\"  value=\"" + nowdate.ToString() + "\">\r\n");
                            templateBuilder.Append("				<input type=\"hidden\" name=\"cal_enddate\" id=\"cal_enddate\" size=\"10\"  value=\"\">					\r\n");
                            templateBuilder.Append("			</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</tbody>\r\n");
                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th>\r\n");
                            templateBuilder.Append("			   <input name=\"updatepoll\" type=\"hidden\" id=\"updatepoll\" value=\"1\" />\r\n");
                            templateBuilder.Append("			   <input type=\"checkbox\" name=\"visiblepoll\" \r\n");

                            if (pollinfo.Visible == 1)
                            {

                                templateBuilder.Append("checked=\"checked\"\r\n");

                            }	//end if

                            templateBuilder.Append(" /> 提交投票后结果才可见<br />\r\n");
                            templateBuilder.Append("			   <input \r\n");

                            if (pollinfo.Multiple == 1)
                            {

                                templateBuilder.Append("checked=\"checked\"\r\n");

                            }	//end if

                            templateBuilder.Append(" type=\"checkbox\" name=\"multiple\"  onclick=\"this.checked?$('maxchoicescontrol').style.display='':$('maxchoicescontrol').style.display='none';\" class=\"checkinput\" /> 多选投票<br />\r\n");
                            templateBuilder.Append("			   <span id=\"maxchoicescontrol\" \r\n");

                            if (pollinfo.Multiple == 0)
                            {

                                templateBuilder.Append("style=\"display: none;\"\r\n");

                            }	//end if

                            templateBuilder.Append(" >最多可选项数: <input type=\"text\" name=\"maxchoices\" value=\"" + pollinfo.Maxchoices.ToString().Trim() + "\" size=\"5\"></span>\r\n");
                            templateBuilder.Append("			</th>\r\n");
                            templateBuilder.Append("			<td>\r\n");
                            templateBuilder.Append("				<div id=\"divPoll\">\r\n");
                            templateBuilder.Append("				  显示顺序<input name=\"button\" type=\"button\" onclick=\"clonePoll('" + config.Maxpolloptions.ToString().Trim() + "')\" value=\"增加投票项\" />\r\n");
                            templateBuilder.Append("				  <input name=\"button\" type=\"button\" onclick=\"if(!delObj(document.getElementById('polloptions'), (is_ie ? 2 : 4))){alert('投票项不能少于2个');}\" value=\"删除投票项\" />\r\n");
                            templateBuilder.Append("				  <input id=\"PollItemname\" type=\"hidden\" name=\"PollItemname\" value=\"\" />\r\n");
                            templateBuilder.Append("				  <input id=\"PollOptionDisplayOrder\" type=\"hidden\" name=\"PollOptionDisplayOrder\" value=\"\" />\r\n");
                            templateBuilder.Append("				  <input id=\"PollOptionID\" type=\"hidden\" name=\"PollOptionID\" value=\"\" />\r\n");
                            templateBuilder.Append("				  <div id=\"polloptions\">\r\n");

                            int poll__loop__id = 0;
                            foreach (DataRow poll in polloptionlist.Rows)
                            {
                                poll__loop__id++;

                                templateBuilder.Append("					<div  \r\n");

                                if (poll__loop__id == 1)
                                {

                                    templateBuilder.Append("id=\"divPollItem\"\r\n");

                                }	//end if

                                templateBuilder.Append(" name=\"PollItem\" style=\"padding-top:4px\">\r\n");
                                templateBuilder.Append("					  <input type=\"hidden\" name=\"optionid\" value=\"" + poll["polloptionid"].ToString().Trim() + "\">\r\n");
                                templateBuilder.Append("					  <input type=\"text\" size=\"4\" name=\"displayorder\" maxlength=\"4\" class=\"colorblue\" onfocus=\"this.className='colorfocus';\" onblur=\"this.className='colorblue';\" value=\"" + poll["displayorder"].ToString().Trim() + "\"  />\r\n");
                                templateBuilder.Append("					  <input type=\"text\" size=\"70\"  name=\"pollitemid\" class=\"colorblue\" onfocus=\"this.className='colorfocus';\" onblur=\"this.className='colorblue';\" value=\"" + poll["name"].ToString().Trim() + "\" />\r\n");
                                templateBuilder.Append("					</div>\r\n");

                            }	//end loop

                            templateBuilder.Append("				  </div>\r\n");
                            templateBuilder.Append("				</div>\r\n");
                            templateBuilder.Append("			</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</tbody>				\r\n");
                            templateBuilder.Append("		<!--投票结束-->	      	\r\n");

                        }	//end if


                        if (topic.Special == 2)
                        {

                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th><label for=\"topicprice\">悬赏价格</label></th>\r\n");
                            templateBuilder.Append("			<td>\r\n");
                            templateBuilder.Append("			<input name=\"topicprice\" type=\"text\" id=\"topicprice\" value=\"" + topic.Price.ToString().Trim() + "\" size=\"5\" onkeyup=\"getrealprice(this.value);\" />\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				&nbsp;&nbsp;\r\n");
                            templateBuilder.Append("				<script type=\"text/javascript\">\r\n");
                            templateBuilder.Append("					function getrealprice(price)\r\n");
                            templateBuilder.Append("					{\r\n");
                            templateBuilder.Append("						var oldprice = " + topic.Price.ToString().Trim() + ";\r\n");
                            templateBuilder.Append("						if(!price.search(/^\\d+$/) ) {\r\n");
                            templateBuilder.Append("							n = parseInt(price) - oldprice;\r\n");
                            templateBuilder.Append("							if (price < oldprice) {\r\n");
                            templateBuilder.Append("								$('realprice').innerHTML = '<b>不能降低悬赏金币</b>';\r\n");
                            templateBuilder.Append("							}else if (price < " + usergroupinfo.Minbonusprice.ToString().Trim() + " || price > " + usergroupinfo.Maxbonusprice.ToString().Trim() + ") {\r\n");
                            templateBuilder.Append("								$('realprice').innerHTML = '<b>悬赏超出范围</b>';\r\n");
                            templateBuilder.Append("							} else {\r\n");
                            templateBuilder.Append("								$('realprice').innerHTML = '追加悬赏: ' + n + ' " + userextcreditsinfo.Unit.ToString().Trim() + "" + userextcreditsinfo.Name.ToString().Trim() + "';\r\n");
                            templateBuilder.Append("							}\r\n");
                            templateBuilder.Append("						}else{\r\n");
                            templateBuilder.Append("							$('realprice').innerHTML = '<b>填写无效</b>';\r\n");
                            templateBuilder.Append("						}\r\n");
                            templateBuilder.Append("					}\r\n");
                            templateBuilder.Append("				</" + "script>\r\n");
                            templateBuilder.Append("				<span id=\"realprice\"></span>\r\n");
                            templateBuilder.Append("			</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</tbody>\r\n");

                        }
                        else if (topic.Special == 3)
                        {

                            templateBuilder.Append("		<tbody>\r\n");
                            templateBuilder.Append("		<tr>\r\n");
                            templateBuilder.Append("			<th>悬赏价格</th>\r\n");
                            templateBuilder.Append("			<td>\r\n");
                            templateBuilder.Append("				" + topic.Price.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Unit.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				" + userextcreditsinfo.Name.ToString().Trim() + "\r\n");
                            templateBuilder.Append("				&nbsp;&nbsp;[已经结帖无法修改悬赏金额]\r\n");
                            templateBuilder.Append("			</td>\r\n");
                            templateBuilder.Append("		</tr>\r\n");
                            templateBuilder.Append("		</tbody>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");

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
                    templateBuilder.Append("</th>\r\n");
                    templateBuilder.Append("<td valign=\"top\">\r\n");

                    templateBuilder.Append("        <link href=\"fckeditor/_samples/sample.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n");
                    templateBuilder.Append("        <script type=\"text/javascript\" src=\"fckeditor/fckeditor.js\"></script>\r\n");
                    templateBuilder.Append("        <script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("            var oFCKeditor = new FCKeditor('message');\r\n");
                    //string tableid = Posts.GetPostTableID(postinfo.Tid);
                    //int lastPostID = Discuz.Data.DatabaseProvider.GetInstance().GetLastPostID(new System.DateTime(2009, 10, 15, 23, 29, 29), tableid);
                    string showMessage = "";
                    if (postinfo.Pid <= 129952)
                    {
                        PostpramsInfo postpramsinfo = new PostpramsInfo();
                        postpramsinfo.Smileyoff = postinfo.Smileyoff;
                        postpramsinfo.Bbcodeoff = postinfo.Bbcodeoff;
                        postpramsinfo.Parseurloff = postinfo.Parseurloff;
                        postpramsinfo.Allowhtml = postinfo.Htmlon;
                        postpramsinfo.Sdetail = postinfo.Message;
                        postpramsinfo.Pid = postinfo.Pid;
                        showMessage = UBB.UBBToHTML(postpramsinfo);
                        showMessage = showMessage.Replace("[:em", "<img alt=\"\" src=\"" + Discuz.Common.XmlConfig.GetCpsClubUrl() + "/fckeditor/editor/images/smiley/msn/").Replace(":]", ".gif\" />");
                        showMessage = showMessage.Replace("\r\n", "").Replace("\n", "");
                        //showMessage = showMessage.Replace("<br />", "<p>&nbsp;</p>");
                    }
                    else
                    {
                        showMessage = postinfo.Message.Replace("\r\n", "").Replace("\n", "");
                        //showMessage = postinfo.Message.Replace("\r\n", "").Replace("\n", "").Replace("<br />", "<p>&nbsp;</p>");
                        //showMessage = postinfo.Message;
                        
                    }
                    
                    templateBuilder.Append("            oFCKeditor.Value='" + showMessage + "';\r\n");
                    templateBuilder.Append("            oFCKeditor.Height	= 350 ;");
                    templateBuilder.Append("            oFCKeditor.BasePath = '" + Discuz.Common.XmlConfig.GetCpsClubUrl() + "/fckeditor/';\r\n");
                    templateBuilder.Append("            oFCKeditor.Config[\"AutoDetectLanguage\"] = false;\r\n");
                    templateBuilder.Append("            oFCKeditor.Config[\"DefaultLanguage\"] = 'zh-cn';\r\n");
                    templateBuilder.Append("            oFCKeditor.Config['SkinPath'] = oFCKeditor.BasePath+'editor/skins/office2003/';\r\n");
                    templateBuilder.Append("            oFCKeditor.Create();\r\n");
                    templateBuilder.Append("        </script>\r\n");


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

                                if (caninsertalbum)
                                {

                                    templateBuilder.Append("    tags = findtags(newnode, 'select');\r\n");
                                    templateBuilder.Append("    for(i in tags)\r\n");
                                    templateBuilder.Append("    {\r\n");
                                    templateBuilder.Append("        if(tags[i].name == 'albums')\r\n");
                                    templateBuilder.Append("        {\r\n");
                                    templateBuilder.Append("            tags[i].id = 'albums' + id;\r\n");
                                    templateBuilder.Append("        }\r\n");
                                    templateBuilder.Append("    }\r\n");

                                }	//end if

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

                                if (caninsertalbum)
                                {

                                    templateBuilder.Append("    if(isimg == 2)\r\n");
                                    templateBuilder.Append("    {\r\n");
                                    templateBuilder.Append("        $('albums' + id).style.display='';\r\n");
                                    templateBuilder.Append("        $('albums' + id).disabled = tempaccounts;\r\n");
                                    templateBuilder.Append("    }\r\n");

                                }	//end if

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


                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                    if (enabletag)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>标签(Tags):</th>\r\n");
                        templateBuilder.Append("			<td><input type=\"text\" name=\"tags\" id=\"tags\" value=\"" + topictags.ToString() + "\" size=\"55\" />&nbsp;<input type=\"button\" onclick=\"relatekw();\" value=\"可用标签\" />(用空格隔开多个标签，最多可填写 5 个)</td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if


                    if (canpostattach && postinfo.Attachment > 0)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                        templateBuilder.Append("			<td><input type=\"button\" value=\"查看已上传附件>>\" onclick=\"expandoptions('attachfilelist');\"/></td>\r\n");
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");
                        templateBuilder.Append("		<!--附件列表开始-->\r\n");
                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("		<td colspan=\"2\">\r\n");
                        templateBuilder.Append("		<div id=\"attachfilelist\" style=\"display:none\">\r\n");

                        if (postinfo.Attachment > 0)
                        {

                            templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
                            templateBuilder.Append("			var attachments = new Array();\r\n");
                            templateBuilder.Append("			var attachimgurl = new Array();\r\n");
                            templateBuilder.Append("			function restore(aid) {\r\n");
                            templateBuilder.Append("			obj = $('attach' + aid);\r\n");
                            templateBuilder.Append("			objupdate = $('attachupdate' + aid);\r\n");
                            templateBuilder.Append("			obj.style.display = '';\r\n");
                            templateBuilder.Append("			objupdate.innerHTML = '<input type=\"file\" name=\"attachupdated\" style=\"display: none;\">';\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			function attachupdate(aid) {\r\n");
                            templateBuilder.Append("			obj = $('attach' + aid);\r\n");
                            templateBuilder.Append("			objupdate = $('attachupdate' + aid);\r\n");
                            templateBuilder.Append("			obj.style.display = 'none';\r\n");
                            templateBuilder.Append("			objupdate.innerHTML = '<input type=\"file\" name=\"attachupdated\" class=\"colorblue\" onfocus=\"this.className=\\'colorfocus\\';\" onblur=\"this.className=\\'colorblue\\';\"  size=\"15\" /><input type=\"hidden\" value=\"' + aid + '\" name=\"attachupdatedid\" /> <input  onfocus=\"this.className=\\'colorfocus\\';\" onblur=\"this.className=\\'colorblue\\';\" class=\"colorblue\" type=\"button\" value=\"取 &nbsp; 消\" onclick=\"restore(\\'' + aid + '\\')\" />';\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			function insertAttachTag(aid) {\r\n");
                            templateBuilder.Append("			if (bbinsert && wysiwyg) {\r\n");
                            templateBuilder.Append("			insertText('[attach]' + aid + '[/attach]', false);\r\n");
                            templateBuilder.Append("			} else {\r\n");
                            templateBuilder.Append("			AddText('[attach]' + aid + '[/attach]');\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			function insertAttachimgTag(aid) {\r\n");
                            templateBuilder.Append("			if (bbinsert && wysiwyg) {\r\n");
                            templateBuilder.Append("			var attachimgurl = eval('attachimgurl_' + aid);\r\n");
                            templateBuilder.Append("			insertText('<img src=\"' + attachimgurl[0] + '\" border=\"0\" aid=\"attachimg_' + aid + '\" alt=\"\" />', false);\r\n");
                            templateBuilder.Append("			} else {\r\n");
                            templateBuilder.Append("			AddText('[attachimg]' + aid + '[/attachimg]');\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			}\r\n");
                            templateBuilder.Append("			</" + "script>\r\n");
                            templateBuilder.Append("			<table border=\"0\" align=\"center\" cellpadding=\"4\" cellspacing=\"0\" summary=\"附件\">\r\n");
                            templateBuilder.Append("			<tr>\r\n");
                            templateBuilder.Append("				<td width=\"5%\" align=\"center\">删除</td>\r\n");
                            templateBuilder.Append("				<td width=\"5%\" >附件ID</td>\r\n");
                            templateBuilder.Append("				<td width=\"25%\">文件名</td>\r\n");
                            templateBuilder.Append("				<td width=\"15%\">时间</td>\r\n");
                            templateBuilder.Append("				<td width=\"10%\">附件大小</td>\r\n");
                            templateBuilder.Append("				<td width=\"10%\">下载次数</td>\r\n");
                            templateBuilder.Append("				<td width=\"5%\">阅读权限</td>\r\n");
                            templateBuilder.Append("				<td width=\"20%\">描述</td>\r\n");
                            templateBuilder.Append("			</tr>\r\n");

                            int attachment__loop__id = 0;
                            foreach (DataRow attachment in attachmentlist.Rows)
                            {
                                attachment__loop__id++;


                                if (Utils.StrToInt(attachment["pid"].ToString().Trim(), 0) == postinfo.Pid)
                                {

                                    templateBuilder.Append("			<tr onmouseover=\"this.style.backgroundColor='#fff'\" onmouseout=\"this.style.backgroundColor='#F5FBFF'\">\r\n");
                                    templateBuilder.Append("			<td align=\"center\"><input class=\"checkbox\" name=\"deleteaid\" value=\"" + attachment["aid"].ToString().Trim() + "\" type=\"checkbox\"><!--<a href=\"javascript:deleteatt(" + attachment["aid"].ToString().Trim() + ");\">删除</a>--></td>\r\n");
                                    templateBuilder.Append("			<td >" + attachment["aid"].ToString().Trim() + "<input type=\"hidden\" value=\"" + attachment["aid"].ToString().Trim() + "\" name=\"attachupdateid\" /></td>\r\n");
                                    templateBuilder.Append("			<td>\r\n");
                                    templateBuilder.Append("				<div id=\"attach" + attachment["aid"].ToString().Trim() + "\">\r\n");
                                    templateBuilder.Append("					<a href=\"###\" onclick=\"attachupdate('" + attachment["aid"].ToString().Trim() + "')\">[更新]</a>\r\n");
                                    templateBuilder.Append("					<a href=\"###\" onclick=\"\r\n");

                                    if (attachment["filetype"].ToString().Trim().IndexOf("image") > -1)
                                    {

                                        templateBuilder.Append("insertAttachimgTag('" + attachment["aid"].ToString().Trim() + "');\r\n");

                                    }
                                    else
                                    {

                                        templateBuilder.Append("insertAttachTag('" + attachment["aid"].ToString().Trim() + "');\r\n");

                                    }	//end if

                                    templateBuilder.Append("\" title=\"点击这里将本附件插入帖子内容中当前光标的位置\">[插入]</a>\r\n");
                                    templateBuilder.Append("					<script type=\"text/javascript\">attachimgurl_" + attachment["aid"].ToString().Trim() + " = ['attachment.aspx?attachmentid=" + attachment["aid"].ToString().Trim() + "', 400];</" + "script>\r\n");
                                    templateBuilder.Append("					<span id=\"imgpreview_" + attachment__loop__id.ToString() + "\" \r\n");

                                    if (attachment["filetype"].ToString().Trim().IndexOf("image") > -1)
                                    {

                                        templateBuilder.Append("onmouseover=\"if($('imgpreview_" + attachment__loop__id.ToString() + "_image').width > 400)$('imgpreview_" + attachment__loop__id.ToString() + "_image').width = 400;showMenu(this.id, 0, 0, 1, 0);\"\r\n");

                                    }	//end if

                                    templateBuilder.Append("><a href=\"attachment.aspx?attachmentid=" + attachment["aid"].ToString().Trim() + "\">" + attachment["attachment"].ToString().Trim() + "</a></span>\r\n");
                                    templateBuilder.Append("				</div>\r\n");
                                    templateBuilder.Append("				<span id=\"attachupdate" + attachment["aid"].ToString().Trim() + "\"><input type=\"file\" name=\"attachupdated\" style=\"display: none;\"></span>\r\n");

                                    if (attachment["filetype"].ToString().Trim().IndexOf("image") > -1)
                                    {

                                        templateBuilder.Append("				<div class=\"popupmenu_popup\" id=\"imgpreview_" + attachment__loop__id.ToString() + "_menu\" style=\"display: none;width:420px;\"><img id=\"imgpreview_" + attachment__loop__id.ToString() + "_image\" src=\"upload/" + attachment["filename"].ToString().Trim() + "\" onerror=\"this.onerror=null;this.src='" + attachment["filename"].ToString().Trim() + "';\" />\r\n");
                                        templateBuilder.Append("				</div>\r\n");

                                    }	//end if

                                    templateBuilder.Append("			</td>\r\n");
                                    templateBuilder.Append("			<td>" + attachment["postdatetime"].ToString().Trim() + "</td>\r\n");
                                    templateBuilder.Append("			<td>" + attachment["filesize"].ToString().Trim() + " 字节</td>\r\n");
                                    templateBuilder.Append("			<td>" + attachment["downloads"].ToString().Trim() + "</td>\r\n");
                                    templateBuilder.Append("			<td><input type=\"text\" name=\"attachupdatereadperm\" size=\"1\" value=" + attachment["readperm"].ToString().Trim() + " /></td>\r\n");
                                    templateBuilder.Append("			<td><input type=\"text\" name=\"attachupdatedesc\" size=\"25\" value=" + attachment["description"].ToString().Trim() + " /></td>\r\n");
                                    templateBuilder.Append("			</tr>\r\n");

                                }	//end if


                            }	//end loop

                            templateBuilder.Append("			</table>\r\n");

                        }	//end if

                        templateBuilder.Append("		</div>\r\n");

                    }	//end if

                    templateBuilder.Append("		</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<!--附件列表结束-->\r\n");

                    if (isseccode)
                    {

                        templateBuilder.Append("		<tbody>\r\n");
                        templateBuilder.Append("		<tr>\r\n");
                        templateBuilder.Append("			<th>验证码</th>\r\n");
                        templateBuilder.Append("			<td>\r\n");

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
                        templateBuilder.Append("		</tr>\r\n");
                        templateBuilder.Append("		</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("		<tbody class=\"divoption\">\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>					\r\n");
                    templateBuilder.Append("				<td><a href=\"###\" id=\"advoption\" onclick=\"expandoptions('divAdvOption');\">其他选项<img src=\"templates/" + templatepath.ToString() + "/images/dot-down2.gif\" /></a></td>	\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody  id=\"divAdvOption\" style=\"display:none\">\r\n");

                    if (userid != -1 && usergroupinfo.Allowsetreadperm == 1)
                    {


                        if (postinfo.Layer == 0)
                        {

                            templateBuilder.Append("			<tr>\r\n");
                            templateBuilder.Append("				<th><label for=\"topicreadperm\">阅读权限</label></th>\r\n");
                            templateBuilder.Append("				<td>\r\n");
                            templateBuilder.Append("					<input name=\"topicreadperm\" type=\"text\" id=\"topicreadperm\" value=\"" + topic.Readperm.ToString().Trim() + "\" size=\"5\" />\r\n");
                            templateBuilder.Append("				</td>\r\n");
                            templateBuilder.Append("			</tr>\r\n");

                        }	//end if


                    }	//end if


                    if (topic.Special == 0)
                    {


                        if (postinfo.Layer == 0)
                        {


                            if (Scoresets.GetCreditsTrans() != 0 && usergroupinfo.Maxprice > 0)
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


                        }	//end if

                        templateBuilder.Append("				<tr>\r\n");
                        templateBuilder.Append("					<th><label for=\"iconid\">图标</label></th>\r\n");
                        templateBuilder.Append("					<td>\r\n");
                        templateBuilder.Append("					<input name=\"iconid\" type=\"radio\" value=\"0\" \r\n");

                        if (topic.Iconid == 0)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/>无\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"1\" \r\n");

                        if (topic.Iconid == 1)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/1.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"2\" \r\n");

                        if (topic.Iconid == 2)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/2.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"3\" \r\n");

                        if (topic.Iconid == 3)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/3.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"4\" \r\n");

                        if (topic.Iconid == 4)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/4.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"5\" \r\n");

                        if (topic.Iconid == 5)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/5.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"6\" \r\n");

                        if (topic.Iconid == 6)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/6.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"7\" \r\n");

                        if (topic.Iconid == 7)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/7.gif\"/> <br />\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"8\" \r\n");

                        if (topic.Iconid == 8)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/8.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"9\" \r\n");

                        if (topic.Iconid == 9)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/9.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"10\" \r\n");

                        if (topic.Iconid == 10)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/10.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"11\" \r\n");

                        if (topic.Iconid == 11)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/11.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"12\" \r\n");

                        if (topic.Iconid == 12)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/12.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"13\" \r\n");

                        if (topic.Iconid == 13)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/13.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"14\" \r\n");

                        if (topic.Iconid == 14)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/><img src=\"images/posticons/14.gif\"/>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" name=\"iconid\" value=\"15\" \r\n");

                        if (topic.Iconid == 15)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append("/>\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");

                    }	//end if

                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("			<td>\r\n");

                    if (postinfo.Layer == 0 && forum.Applytopictype == 1)
                    {

                        templateBuilder.Append("			<input type=\"hidden\" id=\"postbytopictype\" name=\"postbytopictype\" value=\"" + forum.Postbytopictype.ToString().Trim() + "\" tabindex=\"3\" >\r\n");

                    }	//end if

                    templateBuilder.Append("			<input name=\"editsubmit\" type=\"submit\" id=\"postsubmit\" value=\"编辑帖子\" />	[完成后可按 Ctrl + Enter 发布]\r\n");
                    templateBuilder.Append("			</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		</table>\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"aid\" id=\"aid\" value=\"0\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"isdeleteatt\" id=\"isdeleteatt\" value=\"0\">\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("			isfirstpost  = " + postinfo.Layer.ToString().Trim() + " == 0 ? 1 : 0;\r\n");
                    templateBuilder.Append("			$('postform').onsubmit = function() { return validate($('postform'));};\r\n");
                    templateBuilder.Append("			function deleteatt(aid){\r\n");
                    templateBuilder.Append("				document.getElementById('isdeleteatt').value = 1;\r\n");
                    templateBuilder.Append("				document.getElementById('aid').value = aid;\r\n");
                    templateBuilder.Append("				document.getElementById('isdeleteatt').form.submit();\r\n");
                    templateBuilder.Append("			}\r\n");
                    templateBuilder.Append("		</" + "script>\r\n");
                    templateBuilder.Append("		<p class=\"textmsg\" id=\"divshowuploadmsg\" style=\"display:none\"></p>\r\n");
                    templateBuilder.Append("		<p class=\"textmsg succ\" id=\"divshowuploadmsgok\" style=\"display:none\"></p>\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"uploadallowmax\" value=\"10\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"uploadallowtype\" value=\"jpg,gif\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"thumbwidth\" value=\"300\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"thumbheight\" value=\"250\">\r\n");
                    templateBuilder.Append("		<input type=\"hidden\" name=\"noinsert\" value=\"0\">\r\n");
                    templateBuilder.Append("</form>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("</div>\r\n");

                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>出现了'" + page_err.ToString() + "'个错误</h1>\r\n");
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
