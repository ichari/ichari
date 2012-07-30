using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 用户列表页面
    /// </summary>
    public partial class showuser : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 用户列表
        /// </summary>
        public DataTable userlist;

        /// <summary>
        /// 当前页码
        /// </summary>
        public int pageid;

        /// <summary>
        /// 总用户数
        /// </summary>
        public int totalusers;

        /// <summary>
        /// 分页页数
        /// </summary>
        public int pagecount;

        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;

        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户";

            userlist = new DataTable();
            if (config.Memliststatus != 1)
            {
                AddErrLine("系统不允许查看用户列表");
                return;
            }

            string orderby = DNTRequest.GetString("orderby").Trim();
            //进行参数过滤
            if (!Utils.StrIsNullOrEmpty(orderby) && !Utils.InArray(orderby, "uid,username,credits,posts,admin,joindate,lastactivity"))
            {
                orderby = "uid";
            }
            
            string ordertype = DNTRequest.GetString("ordertype").Trim();
            //进行参数过滤      
            if (!ordertype.Equals("desc") && !ordertype.Equals("asc") )
            {
                ordertype = "desc";
            }

            //得到当前用户请求的页数
            pageid = DNTRequest.GetInt("page", 1);
            if (DNTRequest.GetString("orderby").Equals("admin"))
            {
                //获管理组用户总数
                totalusers = Discuz.Forum.Users.GetUserCountByAdmin();
            }
            else
            {
                //获取用户总数
                totalusers = Discuz.Forum.Users.GetUserCount();
            }
            //获取总页数
            pagecount = totalusers%20 == 0 ? totalusers/20 : totalusers/20 + 1;
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


            //获取当前页主题列表
            userlist = Discuz.Forum.Users.GetUserList(20, pageid, orderby, ordertype);
            //得到页码链接
            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, string.Format("showuser.aspx{0}", string.Format("?orderby={0}&ordertype={1}", orderby, ordertype)), 8);
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:01.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:01. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; 用户列表</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (page_err == 0)
            {

                templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                templateBuilder.Append("	<div class=\"pages\">\r\n");
                templateBuilder.Append("		<em>共" + totalusers.ToString() + "名用户</em><strong>" + pagecount.ToString() + "页</strong>" + pagenumbers.ToString() + "\r\n");
                templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                templateBuilder.Append("		window.location='showuser.aspx?page='+this.value;}\"  size=\"4\" maxlength=\"9\"  class=\"colorblue2\"/>页\r\n");
                templateBuilder.Append("		</kbd>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<span class=\"postbtn\">\r\n");
                templateBuilder.Append("	<input type=\"text\" size=\"22\" id=\"username\" name=\"username\" onKeyDown=\"javascript:if(event.keyCode==13) window.location.href='userinfo.aspx?username='+this.value;\" />\r\n");
                templateBuilder.Append("	<input type=\"button\" value=\"查看用户\" onclick=\"if(document.getElementById('username').value==''){return false;}else{window.location.href='userinfo.aspx?username=' + document.getElementById('username').value;}\" />\r\n");
                templateBuilder.Append("	</span>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<div class=\"mainbox\">\r\n");
                templateBuilder.Append("	<h3>会员列表</h3>\r\n");
                templateBuilder.Append("	<table summary=\"用户列表\" cellspacing=\"0\" cellpadding=\"0\">\r\n");
                templateBuilder.Append("		<thead class=\"category\">\r\n");
                templateBuilder.Append("		<tr>\r\n");
                templateBuilder.Append("			<th>&nbsp;</th>\r\n");
                templateBuilder.Append("			<th>用户名</th>\r\n");
                templateBuilder.Append("			<th>组别</th>\r\n");
                templateBuilder.Append("			<th>金币</th>\r\n");
                templateBuilder.Append("			<th>发帖数</th>\r\n");
                templateBuilder.Append("			<th>来自</th>\r\n");
                templateBuilder.Append("			<th>注册时间</th>\r\n");
                templateBuilder.Append("			<th>最后访问</th>\r\n");
                templateBuilder.Append("		</tr>\r\n");
                templateBuilder.Append("		</thead>\r\n");

                int user__loop__id = 0;
                foreach (DataRow user in userlist.Rows)
                {
                    user__loop__id++;

                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("		<tr>\r\n");
                    templateBuilder.Append("			<td>" + user["olimg"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("			<th>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(user["uid"].ToString().Trim());

                    templateBuilder.Append("						<a href=\"" + aspxrewriteurl.ToString() + "\" class=\"bold\">" + user["username"].ToString().Trim() + "</a>\r\n");

                    if (user["nickname"].ToString().Trim() != "")
                    {


                        if (user["nickname"].ToString().Trim() != user["username"].ToString().Trim())
                        {

                            templateBuilder.Append("&nbsp;&nbsp;(" + user["nickname"].ToString().Trim() + ")\r\n");

                        }	//end if


                    }	//end if

                    templateBuilder.Append("			</th>\r\n");
                    templateBuilder.Append("			<td>" + user["grouptitle"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("			<td>" + user["credits"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("			<td>" + user["posts"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("			<td>" + user["location"].ToString().Trim() + "&nbsp;</td>\r\n");
                    templateBuilder.Append("			<td>" + user["joindate"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("			<td>" + user["lastactivity"].ToString().Trim() + "</td>\r\n");
                    templateBuilder.Append("		</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");

                }	//end loop

                templateBuilder.Append("	</table>\r\n");
                templateBuilder.Append("</div>\r\n");
                templateBuilder.Append("<div class=\"pages_btns\">\r\n");
                templateBuilder.Append("	<div class=\"pages\">\r\n");
                templateBuilder.Append("		<em>共" + totalusers.ToString() + "名用户</em><strong>" + pagecount.ToString() + "页</strong>" + pagenumbers.ToString() + "\r\n");
                templateBuilder.Append("		<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) {\r\n");
                templateBuilder.Append("		window.location='showuser.aspx?page='+this.value;}\"  size=\"4\" maxlength=\"9\"  class=\"colorblue2\"/>页\r\n");
                templateBuilder.Append("		</kbd>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("	<span class=\"postbtn\">\r\n");
                templateBuilder.Append("		<form method=\"get\" action=\"\">\r\n");
                templateBuilder.Append("			按:\r\n");
                templateBuilder.Append("			<input id=\"page\" type=\"hidden\" value=\"" + pageid.ToString() + "\" name=\"page\" />\r\n");
                templateBuilder.Append("			<select name=\"orderby\" id=\"orderby\">\r\n");
                templateBuilder.Append("			  <option value=\"\"></option>\r\n");
                templateBuilder.Append("			  <option value=\"uid\">用户ID</option>\r\n");
                templateBuilder.Append("			  <option value=\"username\">用户名</option>\r\n");
                templateBuilder.Append("			  <option value=\"credits\">金币</option>\r\n");
                templateBuilder.Append("			  <option value=\"posts\">发帖数</option>\r\n");
                templateBuilder.Append("			  <option value=\"admin\">管理权限</option>\r\n");
                templateBuilder.Append("			  <option value=\"joindate\">注册日期</option>\r\n");
                templateBuilder.Append("			  <option value=\"lastactivity\">最后访问日期</option>\r\n");
                templateBuilder.Append("			</select>\r\n");
                templateBuilder.Append("			<select name=\"ordertype\" id=\"ordertype\">\r\n");
                templateBuilder.Append("			  <option value=\"asc\">升序</option>\r\n");
                templateBuilder.Append("			  <option value=\"desc\">降序</option>\r\n");
                templateBuilder.Append("			</select>\r\n");
                templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("				document.getElementById('orderby').value=\"" + DNTRequest.GetString("orderby") + "\";\r\n");
                templateBuilder.Append("				document.getElementById('ordertype').value=\"" + DNTRequest.GetString("ordertype") + "\";\r\n");
                templateBuilder.Append("			</" + "script>\r\n");
                templateBuilder.Append("			&nbsp;\r\n");
                templateBuilder.Append("			<input type=\"submit\" name=\"Submit\" value=\"排序\" class=\"lightbutton\" onclick=\"document.getElementById('username').value='';this.form.submit();\" />\r\n");
                templateBuilder.Append("		</form>\r\n");
                templateBuilder.Append("	</span>\r\n");
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