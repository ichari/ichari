using System;
using System.Data;
using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using System.Data.Common;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
    /// <summary>
    /// 用户管理页面
    /// </summary>
    public partial class useradmin : PageBase 
    {
        #region 页面变量

        /// <summary>
        /// 操作标题
        /// </summary>
        public string operationtitle;

        /// <summary>
        /// 被操作用户Id
        /// </summary>
        public int operateduid;

        /// <summary>
        /// 被操作用户名
        /// </summary>
        public string operatedusername;

        /// <summary>
        /// 禁止用户类型
        /// </summary>
        public int bantype;

        /// <summary>
        /// 操作类型
        /// </summary>
        public string action;

        #endregion

        private AdminGroupInfo admininfo;
        private ShortUserInfo operateduser;

        protected override void ShowPage()
        {
            pagetitle = "用户管理";
            operationtitle = "操作提示";

            if (userid == -1)
            {
                AddErrLine("请先登录");
                return;
            }
            action = DNTRequest.GetQueryString("action");
            if (ForumUtils.IsCrossSitePost(DNTRequest.GetUrlReferrer(), DNTRequest.GetHost()) || action == "")
            {
                AddErrLine("非法提交");
                return;
            }
            if (action == "")
            {
                AddErrLine("操作类型参数为空");
                return;
            }
            // 如果拥有管理组身份
            admininfo = AdminGroups.GetAdminGroupInfo(useradminid);
            // 如果所属管理组不存在
            if (admininfo == null)
            {
                AddErrLine("你没有管理权限");
                return;
            }
            operateduid = DNTRequest.GetInt("uid", -1);
            if (operateduid == -1)
            {
                AddErrLine("没有选择要操作的用户");
                return;
            }
            operateduser = Discuz.Forum.Users.GetShortUserInfo(operateduid);
            if (operateduser == null)
            {
                AddErrLine("选择的用户不存在");
                return;
            }
            if (operateduser.Adminid > 0)
            {
                AddErrLine("无法对拥有管理权限的用户进行操作, 请管理员登录后台进行操作");
                return;
            }
            operatedusername = operateduser.Username;

            if (!ispost)
            {
                Utils.WriteCookie("reurl", DNTRequest.GetUrlReferrer());
                switch (action)
                {
                    case "banuser":
                        operationtitle = "禁止用户";
                        switch (operateduser.Groupid)
                        {
                            case 4:
                                bantype = 1;
                                break;
                            case 5:
                                bantype = 2;
                                break;
                            case 6:
                                bantype = 3;
                                break;
                            default:
                                bantype = 0;
                                break;
                        }
                        if (!ValidateBanUser())
                        {
                            AddErrLine("您没有禁止用户的权限");
                            return;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                switch (action)
                {
                    case "banuser":
                        operationtitle = "禁止用户";
                        DoBanUserOperation();
                        break;
                    default:
                        break;
                }
            }
        }

        private void DoBanUserOperation()
        {
            string actions = string.Empty;
            string reason = DNTRequest.GetString("reason");

            if (reason == string.Empty)
            {
                AddErrLine("请填写操作原因");
                return;
            }
            switch (DNTRequest.GetInt("bantype", -1))
            {
                case 0:
                    //正常状态
                    Discuz.Forum.Users.UpdateUserGroup(operateduid, UserCredits.GetCreditsUserGroupID(operateduser.Credits).Groupid);
                    actions = "解除禁止用户";
                    AddMsgLine("已根据金币将用户归组, 将返回之前页面");
                    break;
                case 1:
                    //禁止发言
                    Discuz.Forum.Users.UpdateUserGroup(operateduid, 4);
                    actions = "禁止用户发言";
                    AddMsgLine("已成功禁止所选用户发言, 将返回之前页面");
                    break;
                case 2:
                    //禁止发言
                    Discuz.Forum.Users.UpdateUserGroup(operateduid, 5);
                    actions = "禁止用户访问";
                    AddMsgLine("已成功禁止所选用户访问, 将返回之前页面");
                    break;
                default:
                    AddErrLine("错误的禁止类型");
                    return;
            }

            AdminModeratorLogs.InsertLog(userid.ToString(), username, usergroupid.ToString(), usergroupinfo.Grouptitle,
                                         DNTRequest.GetIP(),
                                         DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "0", string.Empty, "0",
                                         string.Empty,
                                         actions, reason);

            RedirectURL();
        }

        private void RedirectURL()
        {
            SetShowBackLink(false);
            SetUrl(Utils.UrlDecode(ForumUtils.GetReUrl()));
            SetMetaRefresh();
        }

        private bool ValidateBanUser()
        {
            if (admininfo.Allowbanuser == 1)
            {
                return true;
            }
            return false;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:56:00.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:56:00. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<!--TheCurrent start-->\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div class=\"userinfo\">\r\n");
            templateBuilder.Append("		<h2><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a><strong>" + operationtitle.ToString() + "</strong></h2>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<!--TheCurrent end-->\r\n");

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

                    templateBuilder.Append("	<div class=\"mainbox formbox\">\r\n");
                    templateBuilder.Append("		<form id=\"moderate\" name=\"moderate\" method=\"post\">\r\n");
                    templateBuilder.Append("			<h3>" + operationtitle.ToString() + "</h3>\r\n");
                    templateBuilder.Append("			<table cellpadding=\"0\" cellspacing=\"0\" border=\"0\">\r\n");
                    templateBuilder.Append("			<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>被操作用户</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    aspxrewriteurl = this.UserInfoAspxRewrite(operateduid);

                    templateBuilder.Append("				<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + operatedusername.ToString() + "</a></td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			</tbody>\r\n");

                    if (action == "banuser")
                    {

                        templateBuilder.Append("			<tbody>\r\n");
                        templateBuilder.Append("			<tr>\r\n");
                        templateBuilder.Append("				<th>禁止类型</th>\r\n");
                        templateBuilder.Append("				<td>\r\n");
                        templateBuilder.Append("					<input type=\"radio\" value=\"0\" name=\"bantype\" \r\n");

                        if (bantype == 0)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append(" />正常状态\r\n");
                        templateBuilder.Append("					<input type=\"radio\" value=\"1\" name=\"bantype\" \r\n");

                        if (bantype == 1)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append(" />禁止发言\r\n");
                        templateBuilder.Append("					<input type=\"radio\" value=\"2\" name=\"bantype\" \r\n");

                        if (bantype == 2)
                        {

                            templateBuilder.Append("checked=\"checked\"\r\n");

                        }	//end if

                        templateBuilder.Append(" />禁止访问\r\n");

                        if (bantype == 3)
                        {

                            templateBuilder.Append("<input type=\"radio\" value=\"3\" name=\"bantype\" checked=\"checked\" />禁止IP\r\n");

                        }	//end if

                        templateBuilder.Append("				</td>\r\n");
                        templateBuilder.Append("			</tr>\r\n");
                        templateBuilder.Append("			</tbody>\r\n");

                    }	//end if

                    templateBuilder.Append("			<tbody>\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>操作原因</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<select id=\"selectreason\" name=\"selectreason\" size=\"6\" style=\"width: 8em; height:8em; \" onchange=\"this.form.reason.value=this.value\">\r\n");
                    templateBuilder.Append("					  <option value=\"\">自定义</option>\r\n");
                    templateBuilder.Append("					  <option value=\"\">--------</option>\r\n");
                    templateBuilder.Append("					  <option value=\"广告/SPAM\">广告</option>\r\n");
                    templateBuilder.Append("					  <option value=\"恶意灌水\">恶意灌水</option>\r\n");
                    templateBuilder.Append("					  <option value=\"违规内容\">违规内容</option>\r\n");
                    templateBuilder.Append("					  <option value=\"文不对题\">文不对题</option>\r\n");
                    templateBuilder.Append("					  <option value=\"重复发帖\">重复发帖</option>\r\n");
                    templateBuilder.Append("					  <option value=\"发错版块\">发错版块</option>\r\n");
                    templateBuilder.Append("					  <option value=\"屡教不改\">屡教不改</option>\r\n");
                    templateBuilder.Append("					  <option value=\"已经过期\">已经过期</option>\r\n");
                    templateBuilder.Append("					  <option value=\"\">--------</option>\r\n");
                    templateBuilder.Append("					  <option value=\"我很赞同\">我很赞同</option>\r\n");
                    templateBuilder.Append("					  <option value=\"精品文章\">精品文章</option>\r\n");
                    templateBuilder.Append("					  <option value=\"原创内容\">原创内容</option>\r\n");
                    templateBuilder.Append("					  <option value=\"鼓励分享\">鼓励分享</option>\r\n");
                    templateBuilder.Append("					</select>\r\n");
                    templateBuilder.Append("					<textarea name=\"reason\" style=\"height: 7.5em; width:20em;\" onkeydown=\"if(this.value.length>200){ alert('操作原因不能多于200个字符');return false; }\"></textarea>\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("			</tbody>\r\n");
                    templateBuilder.Append("			<tbody>	\r\n");
                    templateBuilder.Append("			<tr>\r\n");
                    templateBuilder.Append("				<th>&nbsp;</th>\r\n");
                    templateBuilder.Append("				<td>\r\n");
                    templateBuilder.Append("					<input type=\"submit\" value=\"提  交\" name=\"modsubmit\" />\r\n");
                    templateBuilder.Append("				</td>\r\n");
                    templateBuilder.Append("			</tr>\r\n");
                    templateBuilder.Append("		</tbody>\r\n");
                    templateBuilder.Append("			</table>\r\n");
                    templateBuilder.Append("		</form>	\r\n");
                    templateBuilder.Append("	</div>\r\n");

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