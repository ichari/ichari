using System;
using System.Data;
using System.Data.SqlClient;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
	/// <summary>
	/// 发件箱页面
	/// </summary>
	public partial class usercpsentbox : PageBase
    {
        #region 页面变量
#if NET1        
        public PrivateMessageInfoCollection pmlist = new PrivateMessageInfoCollection();
#else
        /// <summary>
        /// 短消息列表
        /// </summary>
        public Discuz.Common.Generic.List<PrivateMessageInfo> pmlist = new Discuz.Common.Generic.List<PrivateMessageInfo>();
#endif	
        /// <summary>
        /// 当前页码
        /// </summary>
		public int pageid;
        /// <summary>
        /// 短消息数
        /// </summary>
		public int pmcount;
        /// <summary>
        /// 分页页数
        /// </summary>
		public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
		public string pagenumbers;
        /// <summary>
        /// 用户最大短消息数
        /// </summary>
		public int maxmsg;
        /// <summary>
        /// 已使用的短消息数
        /// </summary>
		public int usedmsgcount;
        /// <summary>
        /// 已使用的短消息条宽度
        /// </summary>
		public int usedmsgbarwidth;
        /// <summary>
        /// 未使用的短消息条宽度
        /// </summary>
		public int unusedmsgbarwidth;
        /// <summary>
        /// 当前用户信息
        /// </summary>
		public UserInfo user = new UserInfo();
        #endregion

        protected override void ShowPage()
		{
			pagetitle = "短消息发件箱";
			
			if (userid == -1)
			{
				AddErrLine("你尚未登录");
				return;
			}
			user = Discuz.Forum.Users.GetUserInfo(userid);

			if (DNTRequest.IsPost())
			{
				if (ForumUtils.IsCrossSitePost())
				{
					AddErrLine("您的请求来路不正确，无法提交。如果您安装了某种默认屏蔽来路信息的个人防火墙软件(如 Norton Internet Security)，请设置其不要禁止来路信息后再试。");
					return;
				}

				string[] pmitemid = Utils.SplitString(DNTRequest.GetFormString("pmitemid"), ",");

				int retval = PrivateMessages.DeletePrivateMessage(userid, pmitemid);

				if (retval == -1)
				{
					AddErrLine("参数无效<br />");
					return;
				}

				SetShowBackLink(false);
				AddMsgLine("删除完毕");
			}
			else
			{
				BindItems();
			}
		}

		/// <summary>
		/// 加载用户当前请求页数的短消息列表并装入DataTable中
		/// </summary>
		private void BindItems()
		{
			//得到当前用户请求的页数
			pageid = DNTRequest.GetInt("page", 1);
			//获取主题总数
			pmcount = PrivateMessages.GetPrivateMessageCount(userid,1);
			//获取总页数
			pagecount = pmcount%16==0 ? pmcount/16 : pmcount/16 + 1;
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
	
			//取得用户所有短消息总数，第二个参数就传-1
			usedmsgcount = PrivateMessages.GetPrivateMessageCount(userid, -1);
			maxmsg = usergroupinfo.Maxpmnum;
	
			if (maxmsg <= 0)
			{
				usedmsgbarwidth = 0;
				unusedmsgbarwidth = 0;
			}
			else
			{
				usedmsgbarwidth = usedmsgcount * 100 / maxmsg;
				unusedmsgbarwidth = 100 - usedmsgbarwidth;
			}
	
			pmlist = PrivateMessages.GetPrivateMessageCollection(userid, 1, 16, pageid,2);
			pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpsentbox.aspx", 8);
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:05.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:05. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>发件箱</strong>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"headsearch\">\r\n");
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
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--主体-->\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	function checkCheckBox(form,objtag)\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		if (typeof(objtag.checked) == \"undefined\")\r\n");
            templateBuilder.Append("		{\r\n");
            templateBuilder.Append("			objtag.checked = true;\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("		for(var i = 0; i < form.elements.length; i++) \r\n");
            templateBuilder.Append("		{\r\n");
            templateBuilder.Append("			var e = form.elements[i];\r\n");
            templateBuilder.Append("			if(e.name == \"pmitemid\") \r\n");
            templateBuilder.Append("			{\r\n");
            templateBuilder.Append("				e.checked = objtag.checked;\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("		}\r\n");
            templateBuilder.Append("		objtag.checked = !objtag.checked;\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div class=\"controlpannel\">\r\n");

            templateBuilder.Append("<div class=\"pannelmenu\">\r\n");

            if (userid > 0)
            {


                if (pagename == "usercptopic.aspx" || pagename == "usercppost.aspx" || pagename == "usercpdigest.aspx" || pagename == "usercpprofile.aspx"


                  || pagename == "usercpnewpassword.aspx" || pagename == "usercppreference.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\" class=\"current\"><span>个人设置</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpprofile.aspx\">个人设置</a>\r\n");

                }	//end if


                if (pagename == "usercpinbox.aspx" || pagename == "usercpsentbox.aspx" || pagename == "usercpdraftbox.aspx" || pagename == "usercppostpm.aspx" || pagename == "usercpshowpm.aspx" || pagename == "usercppmset.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\" class=\"current\"><span>短消息</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpinbox.aspx\">短消息</a>\r\n");

                }	//end if


                if (pagename == "usercpsubscribe.aspx")
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\" class=\"current\"><span>收藏夹</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("	   <a href=\"usercpsubscribe.aspx\">收藏夹</a>\r\n");

                }	//end if


                if (pagename == "usercpcreditspay.aspx" || pagename == "usercpcreditstransfer.aspx" || pagename == "usercpcreditspayoutlog.aspx" || pagename == "usercpcreditspayinlog.aspx"


                   || pagename == "usercpcreaditstransferlog.aspx")
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\" class=\"current\"><span>金币交易</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("       <a href=\"usercpcreditspay.aspx\">金币交易</a>\r\n");

                }	//end if


                if (pagename == "usercpforumsetting.aspx")
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\" class=\"current\"><span>论坛设置</span></a>\r\n");

                }
                else
                {

                    templateBuilder.Append("			<a href=\"usercpforumsetting.aspx\">论坛设置</a>\r\n");

                }	//end if
            }	//end if

            templateBuilder.Append("	</div>\r\n");


            templateBuilder.Append("	<div class=\"pannelcontent\">\r\n");
            templateBuilder.Append("		<div class=\"pcontent\">\r\n");
            templateBuilder.Append("			<div class=\"panneldetail\">\r\n");

            templateBuilder.Append("				<div class=\"panneltabs\">\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("					<a href=\"usercpinbox.aspx\"\r\n");

                if (pagename == "usercpinbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">收件箱</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpsentbox.aspx\"\r\n");

                if (pagename == "usercpsentbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">发件箱</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpdraftbox.aspx\"\r\n");

                if (pagename == "usercpdraftbox.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">草稿箱</a>\r\n");
                templateBuilder.Append("					 <a href=\"usercppmset.aspx\"\r\n");

                if (pagename == "usercppmset.aspx")
                {

                    templateBuilder.Append("						class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">选项</a>\r\n");
                templateBuilder.Append("					<a href=\"usercppostpm.aspx\"\r\n");

                if (pagename == "usercppostpm.aspx")
                {

                    templateBuilder.Append("					 class=\"current addbutton\"\r\n");

                }
                else
                {

                    templateBuilder.Append("					 class=\"addbutton\"\r\n");

                }	//end if

                templateBuilder.Append(">写新消息</a>\r\n");

            }	//end if

            templateBuilder.Append("				</div>	\r\n");


            templateBuilder.Append("				<div class=\"pannelbody\">\r\n");
            templateBuilder.Append("					<div class=\"pannellist\">\r\n");

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

                    templateBuilder.Append("						<p class=\"newmessage\"><a href=\"usercppostpm.aspx\" class=\"submitbutton\">写新消息</a></p>\r\n");
                    templateBuilder.Append("				  		<form id=\"pmform\" name=\"pmform\" method=\"post\" action=\"\">\r\n");
                    templateBuilder.Append("						<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" ID=\"Table1\">\r\n");

                    int pm__loop__id = 0;
                    foreach (PrivateMessageInfo pm in pmlist)
                    {
                        pm__loop__id++;

                        templateBuilder.Append("						<tbody>\r\n");
                        templateBuilder.Append("							<tr class=\"messagetable\" onmouseover=\"this.className='messagetableon'\" onmouseout=\"this.className='messagetable'\">\r\n");
                        templateBuilder.Append("							<td width=\"3%\"><img src=\"templates/" + templatepath.ToString() + "/images/message_" + pm.New.ToString().Trim() + ".gif\" /></td>\r\n");
                        templateBuilder.Append("							<td width=\"4%\"><input name=\"pmitemid\" id=\"id" + pm.Pmid.ToString().Trim() + "\" type=\"checkbox\" id=\"pmitemid\" value=\"" + pm.Pmid.ToString().Trim() + "\" style=\"margin-top:-1px;\"/></td>\r\n");
                        templateBuilder.Append("							<td width=\"50%\" style=\"text-align:left;\"><a href=\"usercpshowpm.aspx?pmid=" + pm.Pmid.ToString().Trim() + "\">" + pm.Subject.ToString().Trim() + "</a> </td>\r\n");
                        templateBuilder.Append("							<td width=\"30%\">\r\n");
                        aspxrewriteurl = this.UserInfoAspxRewrite(pm.Msgfromid);

                        templateBuilder.Append("							<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + pm.Msgfrom.ToString().Trim() + "</a>   <span class=\"fontfamily\"><script type=\"text/javascript\">document.write(convertdate('" + pm.Postdatetime.ToString().Trim() + "'));</" + "script></span></td>\r\n");
                        templateBuilder.Append("							<td><a href=\"#\" onclick=\"$('id" + pm.Pmid.ToString().Trim() + "').checked=true;$('pmform').submit();\">删除</a></td>\r\n");
                        templateBuilder.Append("							</tr>\r\n");
                        templateBuilder.Append("						</tbody>\r\n");

                    }	//end loop

                    templateBuilder.Append("						</table>\r\n");
                    templateBuilder.Append("						</form>\r\n");
                    templateBuilder.Append("						</div>\r\n");
                    templateBuilder.Append("						<div class=\"pannelmessage\">\r\n");
                    templateBuilder.Append("									<div class=\"pannelleft\" style=\"width: 160px;\">\r\n");
                    templateBuilder.Append("										<a href=\"javascript:;\"  onclick=\"checkCheckBox($('pmform'),this)\" class=\"selectall\">全选</a>\r\n");
                    templateBuilder.Append("										<a href=\"#\" onclick=\"$('pmform').submit()\" class=\"selectall\">删除</a> \r\n");
                    templateBuilder.Append("									</div>\r\n");
                    templateBuilder.Append("									<div class=\"pannelright\" style=\"width: 70%;\">\r\n");
                    templateBuilder.Append("										<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                    templateBuilder.Append("												<tbody>\r\n");
                    templateBuilder.Append("													<tr>\r\n");
                    templateBuilder.Append("														<th class=\"ticketleft\">\r\n");
                    templateBuilder.Append("															共有短消息:<span  class=\"ticketnumbers\">" + usedmsgcount.ToString() + "</span>条 上限: " + maxmsg.ToString() + "条\r\n");
                    templateBuilder.Append("														</th>\r\n");
                    templateBuilder.Append("														<td width=\"300\">\r\n");
                    templateBuilder.Append("															<div class=\"optionbar\" style=\"float:none;background:none;\">\r\n");
                    templateBuilder.Append("																<div style=\"width:" + usedmsgbarwidth.ToString() + "%; background:#FFF url(templates/" + templatepath.ToString() + "/images/ticket.gif) no-repeat 0 0;\"></div>\r\n");
                    templateBuilder.Append("															</div>\r\n");
                    templateBuilder.Append("														</td>\r\n");
                    templateBuilder.Append("													</tr>\r\n");
                    templateBuilder.Append("												</tbody>\r\n");
                    templateBuilder.Append("										 </table>\r\n");
                    templateBuilder.Append("									</div>\r\n");
                    templateBuilder.Append("						</div>\r\n");
                    templateBuilder.Append("						<div class=\"pages_btns\">\r\n");
                    templateBuilder.Append("							<div class=\"pages\">\r\n");
                    templateBuilder.Append("								<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                    templateBuilder.Append("								<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                    templateBuilder.Append("							window.location='usercpsentbox.aspx?page=' + this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
                    templateBuilder.Append("							</div>\r\n");
                    templateBuilder.Append("						</div>\r\n");

                }	//end if


            }
            else
            {


                templateBuilder.Append("<div class=\"box message\">\r\n");
                templateBuilder.Append("	<h1>错误显示</h1>\r\n");
                templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");
                templateBuilder.Append("	<p class=\"errorback\">\r\n");
                templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("			if(" + msgbox_showbacklink.ToString() + ")\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				document.write(\"<a href=\\\"" + msgbox_backlink.ToString() + "\\\">返回上一步</a> &nbsp; &nbsp;|  \");\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		</" + "script>\r\n");
                templateBuilder.Append("		&nbsp; &nbsp; <a href=\"forumindex.aspx\">论坛首页</a>\r\n");

                if (usergroupid == 7)
                {

                    templateBuilder.Append("		 |&nbsp; &nbsp; <a href=\"register.aspx\">注册</a>\r\n");

                }	//end if

                templateBuilder.Append("	</p>\r\n");
                templateBuilder.Append("</div>\r\n");



            }	//end if

            templateBuilder.Append("			  </div>\r\n");
            templateBuilder.Append("			</div>\r\n");
            templateBuilder.Append("		</div>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--主体-->\r\n");
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

