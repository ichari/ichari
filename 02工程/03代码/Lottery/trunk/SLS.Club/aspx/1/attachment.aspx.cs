using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Web.UI;
using Discuz.Entity;

namespace Discuz.Web
{
	/// <summary>
	/// attachment页的类派生于BasePage类
	/// </summary>
	public partial class attachment : PageBase
    {
        #region 变量声明
        /// <summary>
        /// 附件所属主题信息
        /// </summary>
		public TopicInfo topic;
        /// <summary>
        /// 附件信息
        /// </summary>
		public AttachmentInfo attachmentinfo;
        /// <summary>
        /// 附件所属版块Id
        /// </summary>
		public int forumid;
        /// <summary>
        /// 附件所属版块名称
        /// </summary>
		public string forumname;
        /// <summary>
        /// 附件所属主题Id
        /// </summary>
		public int topicid;
        /// <summary>
        /// 附件Id
        /// </summary>
		public int attachmentid;
        /// <summary>
        /// 附件所属主题标题
        /// </summary>
		public string topictitle;
        /// <summary>
        /// 论坛导航信息
        /// </summary>
		public string forumnav;
        /// <summary>
        /// 是否需要登录后进行下载
        /// </summary>
	    public bool needlogin = false;
             /// <summary>
        #endregion 变量声明

        protected override void ShowPage()
		{
            pagetitle = "附件下载";

			//　如果当前用户非管理员并且论坛设定了禁止下载附件时间段，当前时间如果在其中的一个时间段内，则不允许用户下载附件
			if (useradminid != 1 && usergroupinfo.Disableperiodctrl != 1)
			{
				string visittime = "";
                if (Scoresets.BetweenTime(config.Attachbanperiods, out visittime))
				{
					AddErrLine("在此时间段( " + visittime + " )内用户不可以下载附件");
					return;
				}
			}

			// 获取附件ID
			attachmentid = DNTRequest.GetInt("attachmentid", -1);
			// 如果附件ID非数字
			if(attachmentid == -1)
			{
				AddErrLine("无效的附件ID");
				return;
			}

            if (DNTRequest.GetString("goodsattach") == "yes")
            {

            }
            else
            {
                // 获取该附件的信息
                attachmentinfo = Attachments.GetAttachmentInfo(attachmentid);
                // 如果该附件不存在
                if (attachmentinfo == null)
                {
                    AddErrLine("不存在的附件ID");
                    return;
                }
                topicid = attachmentinfo.Tid;

                // 获取该主题的信息
                topic = Topics.GetTopicInfo(topicid);
                // 如果该主题不存在
                if (topic == null)
                {
                    AddErrLine("不存在的主题ID");
                    return;
                }

                topictitle = topic.Title;
                forumid = topic.Fid;
                ForumInfo forum = Forums.GetForumInfo(forumid);
                forumname = forum.Name;

                pagetitle = Utils.RemoveHtml(forum.Name);
                forumnav = ForumUtils.UpdatePathListExtname(forum.Pathlist.Trim(), config.Extname);

                //添加判断特殊用户的代码
                if (!Forums.AllowViewByUserID(forum.Permuserlist, userid))
                {
                    if (!Forums.AllowView(forum.Viewperm, usergroupid))
                    {
                        AddErrLine("您没有浏览该版块的权限");
                        if (userid == -1)
                        {
                            needlogin = true;
                        }
                        return;
                    }
                }

                 //添加判断特殊用户的代码
                if (!Forums.AllowGetAttachByUserID(forum.Permuserlist, userid))
                {
                    if (forum.Getattachperm == "" || forum.Getattachperm == null)
                    {
                        // 验证用户是否有下载附件的权限
                        if (usergroupinfo.Allowgetattach != 1)
                        {
                            AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有下载或查看附件的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                    else
                    {
                        if (!Forums.AllowGetAttach(forum.Getattachperm, usergroupid))
                        {
                            AddErrLine("您没有在该版块下载附件的权限");
                            if (userid == -1)
                            {
                                needlogin = true;
                            }
                            return;
                        }
                    }
                }

                // 检查用户是否拥有足够的阅读权限
                if ((attachmentinfo.Readperm > usergroupinfo.Readaccess) && (attachmentinfo.Uid != userid) && (!Moderators.IsModer(useradminid, userid, forumid)))
                {
                    AddErrLine("您的阅读权限不够");
                    if (userid == -1)
                    {
                        needlogin = true;
                    }
                    return;
                }
                //如果图片是不直接显示(作为附件显示) 并且不是作者本人下载都会扣分
                if (config.Showimages != 1 || !Utils.IsImgFilename(attachmentinfo.Filename.Trim()) && userid != attachmentinfo.Uid)
                {
                    if (UserCredits.UpdateUserCreditsByDownloadAttachment(userid) == -1)
                    {
                        AddErrLine("您的金币不足");
                        return;
                    }

                }

                if (attachmentinfo.Filename.IndexOf("http") < 0)
                {
                    if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "upload/" + attachmentinfo.Filename))
                    {
                        AddErrLine("该附件文件不存在或已被删除");
                        return;
                    }
                }

                Attachments.UpdateAttachmentDownloads(attachmentid);

                if (attachmentinfo.Filename.IndexOf("http") < 0)
                {
                    Utils.ResponseFile(AppDomain.CurrentDomain.BaseDirectory + "upload/" + attachmentinfo.Filename, System.IO.Path.GetFileName(attachmentinfo.Attachment), attachmentinfo.Filetype);
                    //ResponseFile("/Forum/" + "upload/" + attachmentinfo.Filename.Trim(), attachmentinfo.Filetype);
                }
                else
                {
                    //Utils.ResponseFile(attachmentinfo.Filename.Trim(), System.IO.Path.GetFileName(attachmentinfo.Filename).Trim(), attachmentinfo.Filetype);
                    ResponseFile(attachmentinfo.Filename.Trim(), attachmentinfo.Filetype);
                }
            }

		}

        public static void ResponseFile(string url, string filetype)
        {
            HttpContext.Current.Response.Redirect(url);
            return;
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:42.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:42. 
            */

            base.OnLoad(e);


            if (page_err == 0)
            {

                templateBuilder.Append("<div id=\"foruminfo\">\r\n");
                templateBuilder.Append("	<div id=\"nav\">\r\n");
                templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; " + forumnav.ToString() + " &raquo; \r\n");
                aspxrewriteurl = this.ShowTopicAspxRewrite(topicid, 0);

                templateBuilder.Append("	    <a href=\"" + aspxrewriteurl.ToString() + "\">" + topictitle.ToString() + "</a><strong>附件</strong>\r\n");
                templateBuilder.Append("	</div>\r\n");
                templateBuilder.Append("</div>\r\n");

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


            }	//end if

            templateBuilder.Append("</div>\r\n");

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
	}
}
