using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;

namespace Discuz.Web
{
	/// <summary>
	/// 在线用户列表页
	/// </summary>
	public partial class onlineuser : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 在线用户列表
		/// </summary>
		public DataTable onlineuserlist;
        /// <summary>
        /// 在线用户数
        /// </summary>
        public int onlineusernumber = 0;
		/// <summary>
        /// 当前页码
		/// </summary>
		public int pageid = 0;
        /// <summary>
        /// 总页数
        /// </summary>
		public int pagecount = 0;
        /// <summary>
        /// 分页页码链接
        /// </summary>
		public string  pagenumbers = "";
        /// <summary>
        /// 在线用户总数
        /// </summary>
		public int totalonline;
        /// <summary>
        /// 在线注册用户数
        /// </summary>
		public int totalonlineuser;
        /// <summary>
        /// 在线游客数
        /// </summary>
		public int totalonlineguest;
        /// <summary>
        /// 在线隐身用户数
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
        #endregion

        private int upp = 16;
	    //开始行数
		private int startrow = 0;
		//结束行数
		private int endrow = 0;
		protected override void ShowPage()
		{
            pagetitle = "在线列表";
			DataTable allonlineuserlist = OnlineUsers.GetOnlineUserList(onlineusercount, out totalonlineguest, out totalonlineuser, out totalonlineinvisibleuser);;
			onlineusernumber = onlineusercount;

			onlineuserlist = CreateUserTable();
		
			//获取总页数
			pagecount = onlineusernumber%upp == 0 ? onlineusernumber/upp : onlineusernumber/upp + 1;
			if (pagecount == 0)
			{
                pagecount = 1;
			}

			//得到当前用户请求的页数
			pageid = DNTRequest.GetInt("page", 1);
			//修正请求页数中可能的错误
			if (pageid <= 1)
			{
				pageid = 1;
				startrow = 0;
				endrow = upp - 1;
			}
			else
			{
				if (pageid > pagecount)
				{
					pageid = pagecount;
				}
                
				startrow = (pageid - 1) * upp;
				endrow = pageid * upp;
			}
		  
            if (startrow >= onlineusernumber) 
				startrow = onlineusernumber - 1;
			if (endrow >= onlineusernumber) 
				endrow = onlineusernumber - 1;
			
			for (;startrow <= endrow; startrow++)
			{

                try
                {
					DataRow newrow = onlineuserlist.NewRow();
				
					newrow["username"] = allonlineuserlist.Rows[startrow]["username"].ToString();
					newrow["userid"] = Convert.ToInt32(allonlineuserlist.Rows[startrow]["userid"].ToString());
					newrow["invisible"] = Convert.ToInt32(allonlineuserlist.Rows[startrow]["invisible"].ToString());
					newrow["lastupdatetime"] = Convert.ToDateTime(allonlineuserlist.Rows[startrow]["lastupdatetime"].ToString());
					string actionid = allonlineuserlist.Rows[startrow]["action"].ToString().Trim();
					if (actionid != "")
					{
						newrow["action"] = UserAction.GetActionDescriptionByID(Convert.ToInt32(actionid));
					}
			
					newrow["forumid"] = Convert.ToInt32(allonlineuserlist.Rows[startrow]["forumid"].ToString());
					newrow["forumname"] = allonlineuserlist.Rows[startrow]["forumname"].ToString();
					newrow["topicid"] = Convert.ToInt32(allonlineuserlist.Rows[startrow]["titleid"].ToString());
					newrow["title"] = allonlineuserlist.Rows[startrow]["title"].ToString();
				
					onlineuserlist.Rows.Add(newrow);
					onlineuserlist.AcceptChanges();
                }
                catch
                { ;}
			}

			//得到页码链接
			if (DNTRequest.GetString("search") == "")
			{
				pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "onlineuser.aspx", 8);
			}
			else
			{
				pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "onlineuser.aspx", 8);
			}

			totalonline = onlineusercount;
			highestonlineusercount = Statistics.GetStatisticsRowItem("highestonlineusercount");
			highestonlineusertime = Statistics.GetStatisticsRowItem("highestonlineusertime");
	   	}


		public DataTable CreateUserTable()
		{
			DataTable dt = new DataTable("onlineuser");
		
			dt.Columns.Add("userid", System.Type.GetType("System.Int32"));
			dt.Columns["userid"].AllowDBNull = false;

			dt.Columns.Add("invisible", System.Type.GetType("System.Int32"));
			dt.Columns["invisible"].AllowDBNull = false;


			dt.Columns.Add("username", System.Type.GetType("System.String"));
			dt.Columns["username"].AllowDBNull = false;
			dt.Columns["username"].MaxLength = 20;
			dt.Columns["username"].DefaultValue = "";
			
			dt.Columns.Add("lastupdatetime", System.Type.GetType("System.DateTime"));
		
			dt.Columns.Add("action", System.Type.GetType("System.String"));
			dt.Columns["action"].MaxLength = 40;
			dt.Columns["action"].DefaultValue = "";
		
			dt.Columns.Add("forumid", System.Type.GetType("System.Int32"));
			dt.Columns.Add("forumname", System.Type.GetType("System.String"));
			dt.Columns["forumname"].MaxLength = 50;
			dt.Columns["forumname"].DefaultValue = "";

			dt.Columns.Add("topicid", System.Type.GetType("System.Int32"));
			dt.Columns.Add("title", System.Type.GetType("System.String"));
			dt.Columns["title"].MaxLength = 80;
			dt.Columns["title"].DefaultValue = "";

            return dt;
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:19.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:19. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; <strong>在线用户列表</strong>\r\n");
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
            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
            templateBuilder.Append("	<div class=\"pages\">\r\n");
            templateBuilder.Append("		<em>共" + totalonline.ToString() + "人在线</em> - " + totalonlineuser.ToString() + "位会员 \r\n");

            if (totalonlineinvisibleuser > 0)
            {

                templateBuilder.Append("		" + totalonlineinvisibleuser.ToString() + "隐身\r\n");
                templateBuilder.Append("		,\r\n");

            }	//end if

            templateBuilder.Append("" + totalonlineguest.ToString() + "位游客 | 最高纪录是 " + highestonlineusercount.ToString() + " 于 " + highestonlineusertime.ToString() + "\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<div class=\"mainbox\">\r\n");
            templateBuilder.Append("	<h3>在线用户列表</h3>\r\n");
            templateBuilder.Append("	<table summary=\"在线用户列表\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
            templateBuilder.Append("		<thead class=\"category\">\r\n");
            templateBuilder.Append("			<tr>\r\n");
            templateBuilder.Append("			<th>&nbsp;</th>\r\n");
            templateBuilder.Append("			<th>用户名</th>\r\n");
            templateBuilder.Append("			<th>时间</th>\r\n");
            templateBuilder.Append("			<th>当前动作</th>\r\n");
            templateBuilder.Append("			<th>所在论坛</th>\r\n");
            templateBuilder.Append("			<th>所在主题</th>\r\n");
            templateBuilder.Append("			</tr>\r\n");
            templateBuilder.Append("		</thead>\r\n");

            int onlineuserinfo__loop__id = 0;
            foreach (DataRow onlineuserinfo in onlineuserlist.Rows)
            {
                onlineuserinfo__loop__id++;

                templateBuilder.Append("		<tbody>\r\n");
                templateBuilder.Append("			<tr>\r\n");
                templateBuilder.Append("				<td><img src=\"templates/" + templatepath.ToString() + "/images/member.gif\" alt=\"用户\" /></td>\r\n");
                templateBuilder.Append("				<td>\r\n");

                if (onlineuserinfo["userid"].ToString().Trim() == "-1")
                {

                    templateBuilder.Append("						 游客\r\n");

                }
                else
                {


                    if (onlineuserinfo["invisible"].ToString().Trim() == "1")
                    {

                        templateBuilder.Append("						 (隐身用户)\r\n");

                    }
                    else
                    {

                        aspxrewriteurl = this.UserInfoAspxRewrite(onlineuserinfo["userid"].ToString().Trim());

                        templateBuilder.Append("						 <a href=\"" + aspxrewriteurl.ToString() + "\">" + onlineuserinfo["username"].ToString().Trim() + "</a>\r\n");

                    }	//end if


                }	//end if

                templateBuilder.Append("				</td>\r\n");
                templateBuilder.Append("				<td>" + onlineuserinfo["lastupdatetime"].ToString().Trim() + "</td>\r\n");
                templateBuilder.Append("				<td>" + onlineuserinfo["action"].ToString().Trim() + "&nbsp;</td>\r\n");
                aspxrewriteurl = this.ShowForumAspxRewrite(onlineuserinfo["forumid"].ToString().Trim(), 0);

                templateBuilder.Append("				<td><a href=\"" + aspxrewriteurl.ToString() + "\">" + onlineuserinfo["forumname"].ToString().Trim() + "</a>&nbsp;</td>\r\n");
                aspxrewriteurl = this.ShowTopicAspxRewrite(onlineuserinfo["topicid"].ToString().Trim(), 0);

                templateBuilder.Append("				<td><a href=\"" + aspxrewriteurl.ToString() + "\">" + onlineuserinfo["title"].ToString().Trim() + "</a>&nbsp;</td>\r\n");
                templateBuilder.Append("			</tr>\r\n");
                templateBuilder.Append("		</tbody>\r\n");

            }	//end loop

            templateBuilder.Append("	</table>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<div class=\"pages_btns\">\r\n");
            templateBuilder.Append("	<div class=\"pages\">\r\n");
            templateBuilder.Append("		<em>共" + onlineusernumber.ToString() + "名用户</em><strong>" + pagecount.ToString() + "页</strong>" + pagenumbers.ToString() + "\r\n");
            templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
            templateBuilder.Append("	window.location='onlineuser.aspx?page='+this.value;}\"  size=\"4\" maxlength=\"9\" class=\"colorblue2\"/>页\r\n");
            templateBuilder.Append("		</kbd>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
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
