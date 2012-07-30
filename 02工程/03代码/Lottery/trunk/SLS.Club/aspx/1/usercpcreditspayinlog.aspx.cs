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
	/// 收入记录
	/// </summary>
	public partial class usercpcreditspayinlog : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前页码
        /// </summary>
		public int pageid;
        /// <summary>
        /// 金币收入日志数
        /// </summary>
		public int payinlogcount;
        /// <summary>
        /// 分页页数
        /// </summary>
		public int pagecount;
        /// <summary>
        /// 分页页码链接
        /// </summary>
		public string pagenumbers;
        /// <summary>
        /// 金币收入日志列表
        /// </summary>
        public DataTable payloglist = new DataTable();
        /// <summary>
        /// 当前用户信息
        /// </summary>
		public UserInfo user = new UserInfo();
        #endregion

        private int pagesize = 16;
		protected override void ShowPage()
		{
			pagetitle = "用户控制面板";

			if (userid == -1)
			{
				AddErrLine("你尚未登录");
				
				return;
			}

			user = Discuz.Forum.Users.GetUserInfo(userid);
			//得到当前用户请求的页数
			pageid = DNTRequest.GetInt("page", 1);
			//获取主题总数
			payinlogcount = PaymentLogs.GetPaymentLogInRecordCount(userid);
			//获取总页数
			pagecount = payinlogcount%pagesize==0 ? payinlogcount/pagesize : payinlogcount/pagesize + 1;
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
			payloglist = PaymentLogs.GetPayLogInList(pagesize,pageid,userid);

			pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "usercpcreditspayinlog.aspx", 8);
				

		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:46.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:46. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<!--header end-->\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>金币收入记录</strong>\r\n");
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

            templateBuilder.Append("<div class=\"panneltabs\">\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("	<a href=\"usercpcreditspay.aspx\"\r\n");

                if (pagename == "usercpcreditspay.aspx")
                {

                    templateBuilder.Append("		class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">金币兑换</a>\r\n");
                templateBuilder.Append("	<a href=\"usercpcreditstransfer.aspx\"\r\n");

                if (pagename == "usercpcreditstransfer.aspx")
                {

                    templateBuilder.Append("		class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">金币转帐</a>\r\n");
                templateBuilder.Append("	<a href=\"usercpcreditspayoutlog.aspx\"\r\n");

                if (pagename == "usercpcreditspayoutlog.aspx")
                {

                    templateBuilder.Append("		class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">支出记录</a>\r\n");
                templateBuilder.Append("	<a href=\"usercpcreditspayinlog.aspx\"\r\n");

                if (pagename == "usercpcreditspayinlog.aspx")
                {

                    templateBuilder.Append("		class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">收入记录</a>\r\n");
                templateBuilder.Append("	<a href=\"usercpcreaditstransferlog.aspx\"\r\n");

                if (pagename == "usercpcreaditstransferlog.aspx")
                {

                    templateBuilder.Append("		class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">兑换与转帐记录</a>\r\n");

            }	//end if

            templateBuilder.Append("</div>	\r\n");


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

                    templateBuilder.Append("				  		<form id=\"form1\" name=\"form1\" method=\"post\" action=\"\">\r\n");
                    templateBuilder.Append("						<table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                    templateBuilder.Append("				  		<tr>\r\n");
                    templateBuilder.Append("							<th width=\"28%\">标题</th>\r\n");
                    templateBuilder.Append("							<th width=\"10%\">作者</th>\r\n");
                    templateBuilder.Append("							<th width=\"13%\">发表时间</th>\r\n");
                    templateBuilder.Append("							<th width=\"17%\">论坛</th>\r\n");
                    templateBuilder.Append("							<th width=\"13%\">付费时间</th>\r\n");
                    templateBuilder.Append("							<th width=\"5%\">售价</th>\r\n");
                    templateBuilder.Append("							<th width=\"7%\">作者所得</th>\r\n");
                    templateBuilder.Append("					  </tr>\r\n");

                    int paylog__loop__id = 0;
                    foreach (DataRow paylog in payloglist.Rows)
                    {
                        paylog__loop__id++;

                        templateBuilder.Append("						<tbody>\r\n");
                        templateBuilder.Append("						<tr class=\"messagetable\" onmouseover=\"this.className='messagetableon'\" onmouseout=\"this.className='messagetable'\">\r\n");
                        templateBuilder.Append("                          <td width=\"28%\" style=\"text-align:left;\">\r\n");
                        aspxrewriteurl = this.ShowTopicAspxRewrite(paylog["tid"].ToString().Trim(), 0);

                        templateBuilder.Append("						  <a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + paylog["title"].ToString().Trim() + "</a></td>\r\n");
                        templateBuilder.Append("                          <td width=\"10%\">\r\n");
                        aspxrewriteurl = this.UserInfoAspxRewrite(paylog["uid"].ToString().Trim());

                        templateBuilder.Append("						  <a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + paylog["authorname"].ToString().Trim() + "</a></td>\r\n");
                        templateBuilder.Append("                          <td width=\"13%\">" + paylog["postdatetime"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("                          <td width=\"17%\">\r\n");
                        aspxrewriteurl = this.ShowForumAspxRewrite(paylog["fid"].ToString().Trim(), 0);

                        templateBuilder.Append("						  <a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + paylog["forumname"].ToString().Trim() + "</a></td>\r\n");
                        templateBuilder.Append("                          <td width=\"13%\">" + paylog["buydate"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("                          <td width=\"5%\">" + paylog["amount"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("                          <td width=\"7%\">" + paylog["netamount"].ToString().Trim() + "</td>\r\n");
                        templateBuilder.Append("						</tr>\r\n");
                        templateBuilder.Append("						</tbody>\r\n");

                    }	//end loop

                    templateBuilder.Append("						</table>\r\n");
                    templateBuilder.Append("						</form>\r\n");
                    templateBuilder.Append("						</div>\r\n");
                    templateBuilder.Append("						<div class=\"pages_btns\">\r\n");
                    templateBuilder.Append("							<div class=\"pages\">\r\n");
                    templateBuilder.Append("								<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                    templateBuilder.Append("								<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                    templateBuilder.Append("							window.location='usercpcreditspayinlog.aspx?page=' + this.value;}\"  size=\"4\" maxlength=\"9\"/>页</kbd>\r\n");
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
