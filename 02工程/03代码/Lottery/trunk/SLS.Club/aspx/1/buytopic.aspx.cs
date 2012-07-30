using System;
using System.Data;
using System.Web;
using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;
using System.Data.Common;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 购买主题页面类
    /// </summary>
    public partial class buytopic : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 所要购买的主题信息
        /// </summary>
        public TopicInfo topic;
        /// <summary>
        /// 最后5个回复列表
        /// </summary>
        public DataTable lastpostlist;
        /// <summary>
        /// 已购买的支付记录列表
        /// </summary>
        public DataTable paymentloglist;
        /// <summary>
        /// 用户积分策略信息
        /// </summary>
        public UserExtcreditsInfo userextcreditsinfo;
        /// <summary>
        /// 所属版块Id
        /// </summary>
        public int forumid;
        /// <summary>
        /// 所属版块名称
        /// </summary>
        public string forumname;
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;
        /// <summary>
        /// 主题购买总次数
        /// </summary>
        public int buyers;
        /// <summary>
        /// 分页总数
        /// </summary>
        public int pagecount;
        /// <summary>
        /// 页码链接字串
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle;
        /// <summary>
        /// 是否显示购买信息列表
        /// </summary>
        public int showpayments;
        /// <summary>
        /// 在判断此值等于1时显示点击购买主题后的确认购买界面
        /// </summary>
        public int buyit;
        /// <summary>
        /// 主题售价
        /// </summary>
        public int topicprice;
        /// <summary>
        /// 作者所得
        /// </summary>
        public float netamount;
        /// <summary>
        /// 单个主题最高收入
        /// </summary>
        public int maxincpertopic;
        /// <summary>
        /// 单个主题最高出售时限(小时)
        /// </summary>
        public int maxchargespan;
        /// <summary>
        /// 积分交易税
        /// </summary>
        public float creditstax;
        /// <summary>
        /// 主题售价
        /// </summary>
        public int price;
        /// <summary>
        /// 主题作者Id
        /// </summary>
        public int posterid;
        /// <summary>
        /// 主题作者用户名
        /// </summary>
        public string poster;
        /// <summary>
        /// 购买后余额
        /// </summary>
        public float userlastprice;

        /// <summary>
        /// 帖内广告
        /// </summary>
        public string inpostad = string.Empty;
        #endregion

        private int ismoder = 0;
        private int pagesize = 16;
        private const string NO_PERMISSION = "您无权购买本主题";
        private const string UNKNOWN_REASON = "未知原因,交易无法进行,给您带来的不方便我们很抱歉";
        private const string NOT_ENOUGH_MONEY = "对不起,您的账户余额少于交易额,无法进行交易";
        private const string PURCHASE_SUCCESS = "购买主题成功,返回该主题";
        private const string WRONG_TOPIC = "无效的主题ID";
        private const string NOT_EXIST_TOPIC = "不存在的主题ID";
        private const string NOT_NEED_TO_PURCHASE = "此主题无需购买";
        private const string NOT_ENOUGH_MONEY_TO = "对不起,您的账户余额 <span class=\"bold\">{0}</span> 少于交易额 {1} ,无法进行交易";

        protected override void ShowPage()
        {
            topictitle = "";
            forumnav = "";

            ////加载帖内广告
            //inpostad = Advertisements.GetInPostAd("", fid, templatepath, postlist.Count > ppp ? ppp : postlist.Count);

            //AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);

            showpayments = DNTRequest.GetInt("showpayments", 0);
            buyit = DNTRequest.GetInt("buyit", 0);
            topicid = DNTRequest.GetInt("topicid", -1);
            // 如果主题ID非数字
            if (topicid == -1)
            {
                AddErrLine(WRONG_TOPIC);
                return;
            }

            // 获取该主题的信息
            TopicInfo topic = Topics.GetTopicInfo(topicid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine(NOT_EXIST_TOPIC);
                return;
            }

            if (topic.Posterid == userid)
            {
                HttpContext.Current.Response.Redirect(base.ShowTopicAspxRewrite(topic.Tid, 0));
                return;
            }

            if (topic.Price <= 0)
            {
                HttpContext.Current.Response.Redirect(base.ShowTopicAspxRewrite(topic.Tid, 0));
                return;
            }

            topictitle = topic.Title.Trim();
            topicprice = topic.Price;
            poster = topic.Poster;
            posterid = topic.Posterid;
            pagetitle = topictitle.Trim();
            forumid = topic.Fid;
            ForumInfo forum = Forums.GetForumInfo(forumid);
            forumname = forum.Name.Trim();
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            //判断是否为回复可见帖, price=0为非购买可见(正常), price>0 为购买可见, price=-1为购买可见但当前用户已购买
            price = 0;
            if (topic.Price > 0)
            {
                price = topic.Price;
                if (PaymentLogs.IsBuyer(topicid, userid) || (Utils.StrDateDiffHours(topic.Postdatetime, Scoresets.GetMaxChargeSpan()) > 0 && Scoresets.GetMaxChargeSpan() != 0))//判断当前用户是否已经购买
                {
                    price = -1;
                }
            }

            if (useradminid != 0)
            {
                ismoder = Moderators.IsModer(useradminid, userid, forumid) ? 1 : 0;
            }

            if (topic.Readperm > usergroupinfo.Readaccess && topic.Posterid != userid && useradminid != 1 &&
                ismoder != 1)
            {
                AddErrLine(string.Format("本主题阅读权限为: {0}, 您当前的身份 \"{1}\" 阅读权限不够", topic.Readperm.ToString(), usergroupinfo.Grouptitle));
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

            if (forum.Password != "" &&
                Utils.MD5(forum.Password) != ForumUtils.GetCookie("forum" + forumid.ToString() + "password"))
            {
                AddErrLine("本版块被管理员设置了密码");
                //SetBackLink("showforum-" + forumid.ToString() + config.Extname);
                HttpContext.Current.Response.Redirect("showforum-" + forumid.ToString() + config.Extname, true);
                return;
            }

            if (!Forums.AllowViewByUserID(forum.Permuserlist, userid)) //判断当前用户在当前版块浏览权限
            {
                if (forum.Viewperm == null || forum.Viewperm == string.Empty) //当板块权限为空时，按照用户组权限
                {
                    if (usergroupinfo.Allowvisit != 1)
                    {
                        AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有浏览该版块的权限");
                        return;
                    }
                }
                else //当板块权限不为空，按照板块权限
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该版块的权限");
                        return;
                    }
                }
            }

            userextcreditsinfo = Scoresets.GetScoreSet(Scoresets.GetCreditsTrans());
            maxincpertopic = Scoresets.GetMaxIncPerTopic();
            maxchargespan = Scoresets.GetMaxChargeSpan();
            creditstax = Scoresets.GetCreditsTax() * 100;

            netamount = topicprice - topicprice * creditstax / 100;
            if (topicprice > maxincpertopic)
            {
                netamount = maxincpertopic - maxincpertopic * creditstax / 100;
            }

            if (price != -1)
            {
                IDataReader reader = Users.GetUserInfoToReader(userid);
                if (reader == null)
                {
                    AddErrLine(NO_PERMISSION);
                    return;
                }

                if (!reader.Read())
                {
                    AddErrLine(NO_PERMISSION);
                    reader.Close();
                    return;
                }

                if (Utils.StrToFloat(reader["extcredits" + Scoresets.GetCreditsTrans().ToString()], 0) < topic.Price)
                {
                    AddErrLine(string.Format(NOT_ENOUGH_MONEY_TO, Utils.StrToFloat(reader["extcredits" + Scoresets.GetCreditsTrans().ToString()], 0), topic.Price));
                    reader.Close();

                    return;
                }

                userlastprice = Utils.StrToFloat(reader["extcredits" + Scoresets.GetCreditsTrans().ToString()], 0) - topic.Price;
                reader.Close();
            }



            //如果不是提交...
            if (!ispost)
            {
                buyers = PaymentLogs.GetPaymentLogByTidCount(topic.Tid);

                //显示购买信息列表
                if (showpayments == 1)
                {
                    //得到当前用户请求的页数
                    pageid = DNTRequest.GetInt("page", 1);
                    //获取主题总数
                    //获取总页数
                    pagecount = buyers % pagesize == 0 ? buyers / pagesize : buyers / pagesize + 1;
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

                    //获取收入记录并分页显示
                    paymentloglist = PaymentLogs.GetPaymentLogByTid(pagesize, pageid, topic.Tid);
                }

                //判断是否为回复可见帖, hide=0为非回复可见(正常), hide>0为回复可见, hide=-1为回复可见但当前用户已回复
                int hide = 0;
                if (topic.Hide == 1)
                {
                    hide = topic.Hide;
                    if (Posts.IsReplier(topicid, userid))
                    {
                        hide = -1;
                    }
                }

                PostpramsInfo _postpramsinfo = new PostpramsInfo();
                _postpramsinfo.Fid = forum.Fid;
                _postpramsinfo.Tid = topicid;
                _postpramsinfo.Jammer = forum.Jammer;
                _postpramsinfo.Pagesize = 5;
                _postpramsinfo.Pageindex = 1;
                _postpramsinfo.Getattachperm = forum.Getattachperm;
                _postpramsinfo.Usergroupid = usergroupid;
                _postpramsinfo.Attachimgpost = config.Attachimgpost;
                _postpramsinfo.Showattachmentpath = config.Showattachmentpath;
                _postpramsinfo.Hide = hide;
                _postpramsinfo.Price = price;
                _postpramsinfo.Ubbmode = false;

                _postpramsinfo.Showimages = forum.Allowimgcode;
                _postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
                _postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
                _postpramsinfo.Smiliesmax = config.Smiliesmax;
                _postpramsinfo.Bbcodemode = config.Bbcodemode;

                lastpostlist = Posts.GetLastPostList(_postpramsinfo);
            }
            else
            {

                int reval = PaymentLogs.BuyTopic(userid, topic.Tid, topic.Posterid, topic.Price, netamount);
                if (reval > 0)
                {
                    SetUrl(base.ShowTopicAspxRewrite(topic.Tid, 0));

                    SetMetaRefresh();
                    SetShowBackLink(false);
                    AddMsgLine(PURCHASE_SUCCESS);
                    return;
                }
                else
                {
                    SetBackLink(base.ShowForumAspxRewrite(topic.Fid, 0));

                    if (reval == -1)
                    {
                        AddErrLine(NOT_ENOUGH_MONEY);
                        return;
                    }
                    else if (reval == -2)
                    {
                        AddErrLine(NO_PERMISSION);
                        return;
                    }
                    else
                    {
                        AddErrLine(UNKNOWN_REASON);
                        return;
                    }
                }
            }

        }


        override protected void OnLoad(EventArgs e)
        {

            /* 
		        This page was created by Discuz!NT Template Engine at 2007-12-24 10:21:51.
		        本页面代码由Discuz!NT模板引擎生成于 2007-12-24 10:21:51. 
	        */

            base.OnLoad(e);

            if (page_err == 0)
            {

                templateBuilder.Append("	<!--1-->\r\n");

                if (ispost)
                {

                    templateBuilder.Append("<!--2-->\r\n");

                    templateBuilder.Append("<div class=\"forumtrue\">\r\n");
                    templateBuilder.Append("			<div class=\"navforumtrue\">" + msgbox_text.ToString() + "\r\n");

                    if (msgbox_url != "")
                    {

                        templateBuilder.Append("					<p class=\"errorback\">\r\n");
                        templateBuilder.Append("						<a href=\"" + msgbox_url.ToString() + "\">如果浏览器没有转向, 请点击这里.</a>\r\n");
                        templateBuilder.Append("					</p>\r\n");

                    }	//end if

                    templateBuilder.Append("			</div>\r\n");
                    templateBuilder.Append("</div>\r\n");



                }
                else
                {

                    templateBuilder.Append("<!--3-->\r\n");

                    if (buyit == 1)
                    {

                        templateBuilder.Append("<!--4-->\r\n");
                        templateBuilder.Append("<div class=\"ntforumbox\">\r\n");
                        templateBuilder.Append("		<div class=\"titlebar\">\r\n");
                        templateBuilder.Append("			<h3>购买主题</h3>\r\n");
                        templateBuilder.Append("		</div>\r\n");
                        templateBuilder.Append("		<div class=\"forumform\">\r\n");
                        templateBuilder.Append("   		 <form id=\"form1\" name=\"form1\" method=\"post\" action=\"\">\r\n");
                        templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">用户名:</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">" + username.ToString() + " [<a href=\"logout.aspx?userkey=" + userkey.ToString() + "\">退出登录</a>]</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">作者:</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">\r\n");
                        aspxrewriteurl = this.UserInfoAspxRewrite(posterid);

                        templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\">" + poster.ToString() + "</a></td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">标题:</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">\r\n");
                        aspxrewriteurl = this.ShowTopicAspxRewrite(topicid, 0);

                        templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\">" + topictitle.ToString() + "</a></td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">售价(" + userextcreditsinfo.Name.ToString().Trim() + "):</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">" + topicprice.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">作者所得(" + userextcreditsinfo.Name.ToString().Trim() + "):</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">" + netamount.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<th class=\"formlabel\">购买后余额(" + userextcreditsinfo.Name.ToString().Trim() + "):</th>\r\n");
                        templateBuilder.Append("					<td class=\"formbody\">" + userlastprice.ToString() + "</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("				<tr class=\"list\">\r\n");
                        templateBuilder.Append("					<td colspan=\"2\" class=\"formarea\">\r\n");
                        templateBuilder.Append("					<input class=\"sbutton\" type=\"submit\" name=\"paysubmit\" value=\"提交\">\r\n");
                        templateBuilder.Append("					</td>\r\n");
                        templateBuilder.Append("				</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");
                        templateBuilder.Append("			</table>\r\n");
                        templateBuilder.Append("		 </form>\r\n");
                        templateBuilder.Append("		</div>\r\n");
                        templateBuilder.Append("</div>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("	<!--5-->\r\n");

                        if (showpayments == 1)
                        {

                            templateBuilder.Append("		<!--6-->\r\n");
                            templateBuilder.Append("		<div class=\"ntforumbox\">\r\n");
                            templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n");
                            templateBuilder.Append("				<thead><tr>\r\n");
                            templateBuilder.Append("						<th >" + userextcreditsinfo.Name.ToString().Trim() + "</th>\r\n");
                            templateBuilder.Append("						<th>用户名</th>\r\n");
                            templateBuilder.Append("						<th>时间</th>\r\n");
                            templateBuilder.Append("				</tr></thead>\r\n");

                            int paymentlog__loop__id = 0;
                            foreach (DataRow paymentlog in paymentloglist.Rows)
                            {
                                paymentlog__loop__id++;

                                templateBuilder.Append("				<tbody>\r\n");
                                templateBuilder.Append("					<tr class=\"list\" onmouseover=\"this.className='liston'\" onmouseout=\"this.className='list'\">\r\n");
                                templateBuilder.Append("						<td class=\"topicicon\">&nbsp;</td>\r\n");
                                templateBuilder.Append("						<th class=\"topictitle\">\r\n");
                                aspxrewriteurl = this.UserInfoAspxRewrite(paymentlog["uid"].ToString().Trim());

                                templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + paymentlog["username"].ToString().Trim() + "</a></th>\r\n");
                                templateBuilder.Append("						<td><div class=\"ForumBuyTopicLeft\">" + paymentlog["amount"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("						<td>" + paymentlog["buydate"].ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("					</tr>\r\n");
                                templateBuilder.Append("				</tbody>\r\n");

                            }	//end loop

                            templateBuilder.Append("			</table>\r\n");
                            templateBuilder.Append("		 </div>\r\n");

                        }	//end if


                        if (price > 0)
                        {

                            templateBuilder.Append("	<!--7-->\r\n");
                            templateBuilder.Append("		<div class=\"ntforumbox\">\r\n");
                            templateBuilder.Append("				<div class=\"titlebar\">\r\n");
                            templateBuilder.Append("					<h3>标题:" + topictitle.ToString() + "</h3>\r\n");
                            templateBuilder.Append("				</div>\r\n");
                            templateBuilder.Append("				<div class=\"navformcommend\" style=\"border-bottom:none;\">本主题需向作者支付相应积分才能浏览，您可根据作者信誉、出售价格及已购买用户的评价确定购买与否。</div>\r\n");
                            templateBuilder.Append("				<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">售价(" + userextcreditsinfo.Name.ToString().Trim() + ")</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + topicprice.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">积分交易税</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + creditstax.ToString() + "%</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("				<div class=\"NtForumForm\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">作者所得(" + userextcreditsinfo.Name.ToString().Trim() + ")</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + netamount.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">已购买人数</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + buyers.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">作者最高收入(" + userextcreditsinfo.Name.ToString().Trim() + ")</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + maxincpertopic.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">作者最高收入(" + userextcreditsinfo.Name.ToString().Trim() + ")</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + maxincpertopic.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">作者最高收入(" + userextcreditsinfo.Name.ToString().Trim() + ")</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + maxincpertopic.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">强制免费期限(小时)</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">" + maxchargespan.ToString() + "</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				<tbody>\r\n");
                            templateBuilder.Append("					<tr class=\"list\">\r\n");
                            templateBuilder.Append("						<th class=\"formlabel\">&nbsp;</th>\r\n");
                            templateBuilder.Append("						<td class=\"formbody\">\r\n");
                            templateBuilder.Append("						<a href=\"buytopic.aspx?topicid=" + topicid.ToString() + "&showpayments=1\">[查看付款记录]</a><a href=\"buytopic.aspx?topicid=" + topicid.ToString() + "&buyit=1\">[购买主题]</a> <a href=\"#\" onclick=\"history.go(-1)\">[返回上一页]</a>\r\n");
                            templateBuilder.Append("						</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("				</tbody>\r\n");
                            templateBuilder.Append("				</table>\r\n");
                            templateBuilder.Append("		</div>\r\n");
                            templateBuilder.Append("		<div class=\"ntforumbox\">\r\n");
                            templateBuilder.Append("			<div class=\"titlebar\">\r\n");
                            templateBuilder.Append("				<h3>最后5帖</h3>\r\n");
                            templateBuilder.Append("			</div>\r\n");

                            int lastpost__loop__id = 0;
                            foreach (DataRow lastpost in lastpostlist.Rows)
                            {
                                lastpost__loop__id++;

                                templateBuilder.Append("					<div class=\"list\">\r\n");
                                aspxrewriteurl = this.UserInfoAspxRewrite(lastpost["posterid"].ToString().Trim());

                                templateBuilder.Append("					<a href=\"" + aspxrewriteurl.ToString() + "\"><strong>" + lastpost["poster"].ToString().Trim() + "</strong></a>&nbsp;&nbsp;" + lastpost["postdatetime"].ToString().Trim() + "</div>\r\n");
                                templateBuilder.Append("					<p class=\"fontfamily\">" + lastpost["message"].ToString().Trim() + "</p>\r\n");

                            }	//end loop

                            templateBuilder.Append("		</div>\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("				此主题无需购买\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("</div>\r\n");

                    templateBuilder.Append("" + inpostad.ToString() + "\r\n");
                    if (footerad != "")
                    {

                        templateBuilder.Append("<!--底部广告显示-->\r\n");
                        templateBuilder.Append("<div id=\"bottomad\">" + footerad.ToString() + "</div>\r\n");
                        templateBuilder.Append("<!--底部广告结束-->\r\n");

                    }	//end if

                    templateBuilder.Append("<div id=\"forumfooter\">\r\n");
                    templateBuilder.Append("	<div class=\"footercopy\">\r\n");
                    templateBuilder.Append("	 	<p>\r\n");
                    templateBuilder.Append("			版权所有 <a href=\"" + config.Weburl.ToString().Trim() + "\" target=\"_blank\">" + config.Webtitle.ToString().Trim() + "</a>&nbsp; " + config.Linktext.ToString().Trim() + "\r\n");

                    if (config.Sitemapstatus == 1)
                    {

                        templateBuilder.Append("&nbsp;<a href=\"tools/sitemap.aspx\" target=\"_blank\" title=\"百度论坛收录协议\">Sitemap</a>\r\n");

                    }	//end if

                    templateBuilder.Append("		</p>\r\n");
                    templateBuilder.Append("		<span>Powered by <a href=\"http://nt.discuz.net\" target=\"_blank\" title=\"Discuz!NT 2.0 (.net Framework 2.x/3.x)\"><em>Discuz!NT</em></a>&nbsp;<em>2.0.1214 \r\n");

                    if (config.Licensed == 1)
                    {

                        templateBuilder.Append("				(<a href=\"\" onclick=\"this.href='http://nt.discuz.net/certificate/?host='+location.href.substring(0, location.href.lastIndexOf('/'))\" target=\"_blank\">Licensed</a>)\r\n");

                    }	//end if

                    templateBuilder.Append("				&nbsp;&nbsp; " + config.Forumcopyright.ToString().Trim() + "</em>\r\n");

                    if (config.Debug != 0)
                    {

                        templateBuilder.Append("<br/>\r\n");
                        templateBuilder.Append("				Processed in " + this.Processtime.ToString().Trim() + " second(s)\r\n");

                        if (isguestcachepage == 1)
                        {

                            templateBuilder.Append("						(Cached).\r\n");

                        }
                        else if (querycount > 1)
                        {

                            templateBuilder.Append("				        , " + querycount.ToString() + " queries.\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("				        , " + querycount.ToString() + " query.\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("				" + config.Icp.ToString().Trim() + "\r\n");
                    templateBuilder.Append("		 </span>\r\n");
                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("	<div class=\"footergoto\">\r\n");
                    templateBuilder.Append("		<span><img src=\"templates/" + templatepath.ToString() + "/images/gototop2.gif\" alt=\"返顶部\" onclick=\"window.scrollTo(0,0)\" /></span>\r\n");

                    if (config.Stylejump == 1)
                    {


                        if (userid != -1 || config.Guestcachepagetimeout <= 0)
                        {

                            templateBuilder.Append("		<select onchange=\"if(this.options[this.selectedIndex].value != '') {window.location=('showtemplate.aspx?templateid='+this.options[this.selectedIndex].value) }\">\r\n");
                            templateBuilder.Append("			<option selected>切换界面...</option>\r\n");
                            templateBuilder.Append("			" + templatelistboxoptions.ToString() + "\r\n");
                            templateBuilder.Append("		</select>\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("	</div>\r\n");
                    templateBuilder.Append("</div>\r\n");
                    templateBuilder.Append("<div id=\"my_menu\" class=\"ntdropmenu\" style=\"display: none;\">\r\n");
                    templateBuilder.Append("		<a href=\"mytopics.aspx\">我的主题</a>\r\n");
                    templateBuilder.Append("		<a href=\"myposts.aspx\">我的帖子</a>\r\n");
                    templateBuilder.Append("		<a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\">我的精华</a>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/mymenu.js\"></" + "script>\r\n");
                    templateBuilder.Append("</div>\r\n");



                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"ntforumbox\">\r\n");
                templateBuilder.Append("	<div class=\"titlebar\">\r\n");
                templateBuilder.Append("		<h3>出现了" + page_err.ToString() + "个错误</h3>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<div class=\"forumerror\">\r\n");
                templateBuilder.Append("				" + msgbox_text.ToString() + "\r\n");
                templateBuilder.Append("				<p class=\"errorback\">\r\n");
                templateBuilder.Append("						<script language=\"javascript\">\r\n");
                templateBuilder.Append("							if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("							{\r\n");
                templateBuilder.Append("								document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|&nbsp; &nbsp  \");\r\n");
                templateBuilder.Append("							}\r\n");
                templateBuilder.Append("						</" + "script>\r\n");
                templateBuilder.Append("						<a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("						 &nbsp; &nbsp|&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                }	//end if

                templateBuilder.Append("				</p>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");


                templateBuilder.Append("</div>\r\n");

            }	//end if


            templateBuilder.Append("<div id=\"quicksearch_menu\" class=\"searchmenu\" style=\"display: none;\">\r\n");
            templateBuilder.Append("				<div onclick=\"document.getElementById('keywordtype').value='0';document.getElementById('quicksearch').innerHTML='帖子标题';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">帖子标题</div>\r\n");
            templateBuilder.Append("				<div onclick=\"document.getElementById('keywordtype').value='2';document.getElementById('quicksearch').innerHTML='空间日志';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">空间日志</div>\r\n");
            templateBuilder.Append("				<div onclick=\"document.getElementById('keywordtype').value='3';document.getElementById('quicksearch').innerHTML='相册标题';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">相册标题</div>\r\n");
            templateBuilder.Append("				<div onclick=\"document.getElementById('keywordtype').value='8';document.getElementById('quicksearch').innerHTML='作&nbsp;&nbsp;者';document.getElementById('quicksearch_menu').style.display='none';\" onmouseover=\"MouseCursor(this);\">作&nbsp;&nbsp;者</div>\r\n");
            templateBuilder.Append("			</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}
