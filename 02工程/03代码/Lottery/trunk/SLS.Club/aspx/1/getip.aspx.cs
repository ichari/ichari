using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Web
{
    /// <summary>
    /// 查看IP信息页
    /// </summary>
    public partial class getip : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前版块名称
        /// </summary>
        public string forumname = "";
        /// <summary>
        /// 论坛导航信息
        /// </summary>
        public string forumnav = "";
        /// <summary>
        /// 帖子标题
        /// </summary>
        public string posttitle = "";
        /// <summary>
        /// 主题标题
        /// </summary>
        public string topictitle = "";
        /// <summary>
        /// 帖子Id
        /// </summary>
        public int postid;
        /// <summary>
        /// 主题Id
        /// </summary>
        public int topicid;
        /// <summary>
        /// IP地址
        /// </summary>
        public string ip = "";
        /// <summary>
        /// IP地址所在地查询结果
        /// </summary>
        public string iplocation = "";
        #endregion

        protected override void ShowPage()
        {
            topicid = DNTRequest.GetInt("topicid", -1);
            postid = DNTRequest.GetInt("pid", 0);
            if (postid == 0)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }

            PostInfo __postinfo = Posts.GetPostInfo(topicid, postid);
            if (__postinfo == null)
            {
                base.AddErrLine("指定的主题不存在或已被删除或正在被审核,请返回.");
                return;
            }

            ip = __postinfo.Ip;
            iplocation = IpSearch.GetAddressWithIP(ip);

            // 如果数据库文件不存在
            if (iplocation == null)
            {
                iplocation = "(IP数据库文件不存在,无法查询)";
            }
            else if (iplocation == "") // 如果没有查到
            {
                iplocation = "没有查询到该用户的地理所在地";
            }

            // 获取该主题的信息
            TopicInfo topic = Topics.GetTopicInfo(__postinfo.Tid);
            // 如果该主题不存在
            if (topic == null)
            {
                AddErrLine("不存在的主题ID");
                return;
            }

            topictitle = topic.Title;

            ForumInfo forum = Forums.GetForumInfo(__postinfo.Fid);
            forumname = forum.Name;
            pagetitle = topic.Title;
            forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

            AdminGroupInfo admininfo = AdminGroups.GetAdminGroupInfo(useradminid);

            if (admininfo == null || admininfo.Allowviewip != 1)
            {
                AddErrLine("你没有查看IP的权限");
                return;
            }

            if (DNTRequest.GetString("action") == "ipban")
            {
                if (admininfo.Allowbanip != 1)
                {
                    AddErrLine("你无权禁止用户IP,请返回");
                    return;
                }

                string[] regctrl = Utils.SplitString(config.Ipdenyaccess, "\n");
                if (Utils.InIPArray(DNTRequest.GetString("ip"), regctrl))
                {
                    AddErrLine("IP已在列表中存在,无需重复添加");
                    return;
                }

                if (GeneralConfigs.SetIpDenyAccess(DNTRequest.GetString("ip")))
                {
                    //调整用户到禁止IP组

                    SetUrl(base.ShowTopicAspxRewrite(topic.Tid, 0));
                    SetMetaRefresh();
                    SetShowBackLink(false);
                    base.AddMsgLine("IP已加入到用户禁止列表中");
                    base.ispost = true;
                    return;
                }
                else
                {
                    base.AddErrLine("未知原因,IP无法加到禁止列表中");
                    return;
                }
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:26.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:26. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("	<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + "\r\n");
            aspxrewriteurl = this.ShowTopicAspxRewrite(topicid, 0);

            templateBuilder.Append("	 &raquo; <a href=\"" + aspxrewriteurl.ToString() + "\">" + pagetitle.ToString() + "</a> &raquo;<strong>查看IP</strong>\r\n");
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



                }
                else
                {

                    templateBuilder.Append("<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("	<h3>查看 IP</h3>\r\n");
                    templateBuilder.Append("	<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>此用户的 IP 地址是:</th>\r\n");
                    templateBuilder.Append("				<td>" + ip.ToString() + "</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>所在地查询结果结果是:</th>\r\n");
                    templateBuilder.Append("				<td>" + iplocation.ToString() + "</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("		<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("				<a href=\"###\" onClick=\"if(confirm('你确实要禁止此IP吗?')){window.location.replace('getip.aspx?topicid=" + topicid.ToString() + "&pid=" + postid.ToString() + "&action=ipban&ip=" + ip.ToString() + "')}\">禁止此 IP</a>\r\n");
                    templateBuilder.Append("				<a href=\"###\" onclick=\"history.go(-1)\">返回上一页</a>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("	</table>\r\n");
                    templateBuilder.Append("</div>\r\n");

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