using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;
using Discuz.Entity;
using Discuz.Config;

namespace Discuz.Web
{
    /// <summary>
    /// 用户个性设置
    /// </summary>
    public partial class usercppreference : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 当前用户信息
        /// </summary>
        public UserInfo user = new UserInfo();

        /// <summary>
        /// 用户头像
        /// </summary>
        public string avatar;

        /// <summary>
        /// 用户头像地址
        /// </summary>
        public string avatarurl;

        /// <summary>
        /// 用户头像类型
        /// </summary>
        public int avatartype;

        /// <summary>
        /// 头像宽度
        /// </summary>
        public int avatarwidth;

        /// <summary>
        /// 头像高度
        /// </summary>
        public int avatarheight;

        /// <summary>
        /// 可用的模板列表
        /// </summary>
        public DataTable templatelist;

        /// <summary>
        /// 系统头像列表
        /// </summary>
        public DataTable avatarfilelist;

        #endregion

        protected override void ShowPage()
        {
            pagetitle = "用户控制面板";
            if (userid == -1)
            {
                AddErrLine("你尚未登录");
                return;
            }
            user = Discuz.Forum.Users.GetUserInfo(userid);            

            avatarwidth = 100;
            avatarheight = 100;

            if (DNTRequest.IsPost())
            {
                int avatartype = DNTRequest.GetInt("avatartype", -1);
                if (avatartype != -1)
                {
                    switch (avatartype)
                    {
                        case 0: //从系统选择
                            avatar = DNTRequest.GetString("usingavatar");
                            avatar = Utils.UrlDecode(avatar.Substring(avatar.IndexOf("avatar")));
                            avatarwidth = 0;
                            avatarheight = 0;
                            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + avatar))
                            {
                                AddErrLine("不存在的头像文件");
                                return;
                            }

                            break;

                        case 1: //上传头像

                            if (usergroupinfo.Allowavatar < 3)
                            {
                                AddErrLine("您当前的身份 \"" + usergroupinfo.Grouptitle + "\" 没有上传头像的权限");
                                return;
                            }
                            avatar = ForumUtils.SaveRequestAvatarFile(userid, config.Maxavatarsize);
                            if (avatar.Equals(""))
                            {
                                AddErrLine(
                                    string.Format("头像图片不合法, 系统要求必须为gif jpg png图片, 且宽高不得超过 {0}x{1}, 大小不得超过 {2} 字节",
                                                  config.Maxavatarwidth, config.Maxavatarheight, config.Maxavatarsize));
                                return;
                            }

                            Thumbnail thumb = new Thumbnail();
                            if (!thumb.SetImage(avatar))
                            {
                                AddErrLine("非法的图片格式");
                                return;
                            }
                            thumb.SaveThumbnailImage(config.Maxavatarwidth, config.Maxavatarheight);
                            avatarwidth = 0;
                            avatarheight = 0;
                            break;

                        case 2: //自定义头像Url

                            if (usergroupinfo.Allowavatar < 2)
                            {
                                AddErrLine(string.Format("您当前的身份 \"{0}\" 没有使用自定义头像的权限", usergroupinfo.Grouptitle));
                                return;
                            }
                            avatar = DNTRequest.GetString("avatarurl").Trim();
                            if (avatar.Length < 10)
                            {
                                AddErrLine("头像路径不合法");

                                return;
                            }
                            if (!avatar.Substring(0, 7).ToLower().Equals("http://"))
                            {
                                AddErrLine("头像路径必须以http://开始");
                                return;
                            }
                            string fileextname = Path.GetExtension(avatar).ToLower();
                            // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                            if (
                                !(fileextname.Equals(".jpg") || fileextname.Equals(".gif") || fileextname.Equals(".png")))
                            {
                                AddErrLine("头像路径必须是.jpg .gif或.png结尾");
                                return;
                            }

                            avatarwidth = DNTRequest.GetInt("avatarwidth", 120);
                            avatarheight = DNTRequest.GetInt("avatarheight", 120);
                            if (avatarwidth <= 0 || avatarwidth > 120|| avatarheight <= 0 ||avatarheight > 120)
                            {
                                AddErrLine("自定义URL地址头像尺寸必须大于0, 且必须小于系统当前设置的最大尺寸 120x120");

                                return;
                            }
                            break;
                    }                    
                }
                else
                { 
                    //当允许使用头像时
                    if (usergroupinfo.Allowavatar > 0)
                    {
                        AddErrLine("请指定新头像的信息<br />");
                        return;
                    }
                }

                //当不允许使用头像时
                if (usergroupinfo.Allowavatar == 0)
                {
                    avatar = user.Avatar;
                    avatarwidth = user.Avatarwidth;
                    avatarheight = user.Avatarheight;
                }
                Discuz.Forum.Users.UpdateUserPreference(userid, avatar, avatarwidth, avatarheight,
                                               DNTRequest.GetInt("templateid", 0));
                SetUrl("usercppreference.aspx");
                SetMetaRefresh();
                SetShowBackLink(true);
                AddMsgLine("修改个性设置完毕");
            }
            else
            {
                templatelist = Templates.GetValidTemplateList();
                avatarfilelist = Caches.GetAvatarList();

                UserInfo __userinfo = Discuz.Forum.Users.GetUserInfo(userid);
                avatar = __userinfo.Avatar;
                avatarurl = "";
                avatartype = 1;
                avatarwidth = 0;
                avatarheight = 0;
                if (Utils.CutString(avatar, 0, 15).ToLower().Equals(@"avatars\common\"))
                {
                    avatartype = 0;
                }
                else if (Utils.CutString(avatar, 0, 7).ToLower().Equals("http://"))
                {
                    avatarurl = avatar;
                    avatartype = 2;
                    avatarwidth = __userinfo.Avatarwidth;
                    avatarheight = __userinfo.Avatarheight;
                }
            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:55:25.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:55:25. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("function changeAvatarOption(currentOption)\r\n");
            templateBuilder.Append("{\r\n");
            templateBuilder.Append("	if ($('enterurl'))\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		$('enterurl').style.display='none';\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("	if ($('uploadfile'))\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		$('uploadfile').style.display='none';\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("	switch (currentOption)\r\n");
            templateBuilder.Append("	{\r\n");
            templateBuilder.Append("		case \"0\":\r\n");
            templateBuilder.Append("			$('templateid').style.display='none';\r\n");
            templateBuilder.Append("			break;\r\n");
            templateBuilder.Append("		case \"1\":\r\n");
            templateBuilder.Append("			$('uploadfile').style.display='block';\r\n");
            templateBuilder.Append("			break;\r\n");
            templateBuilder.Append("		case \"2\":\r\n");
            templateBuilder.Append("			$('enterurl').style.display='block';\r\n");
            templateBuilder.Append("			break;\r\n");
            templateBuilder.Append("	}\r\n");
            templateBuilder.Append("}\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\" class=\"home\">" + config.Forumtitle.ToString().Trim() + "</a>  &raquo; <a href=\"usercp.aspx\">用户中心</a>  &raquo; <strong>个性设置</strong>\r\n");
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

            templateBuilder.Append("				<div class=\"panneltabs\">\r\n");

            if (userid > 0)
            {

                templateBuilder.Append("					<a href=\"usercpprofile.aspx\"\r\n");

                if (pagename == "usercpprofile.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">编辑个人档案</a>\r\n");
                templateBuilder.Append("					<a href=\"usercpnewpassword.aspx\"\r\n");

                if (pagename == "usercpnewpassword.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">更改密码</a>\r\n");
                templateBuilder.Append("					 <a href=\"usercppreference.aspx\"\r\n");

                if (pagename == "usercppreference.aspx")
                {

                    templateBuilder.Append("					 class=\"current\"\r\n");

                }	//end if

                templateBuilder.Append(">个性设置</a>\r\n");

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

                    templateBuilder.Append("						<form action=\"\" method=\"post\" ID=\"Form1\" enctype=\"multipart/form-data\">\r\n");
                    templateBuilder.Append("								<label for=\"templateid\" class=\"labellong2\" style=\"line-height:220%;\">风格:</label>\r\n");
                    templateBuilder.Append("								<select name=\"templateid\" id=\"templateid\" >\r\n");
                    templateBuilder.Append("								<option value=\"0\" \r\n");

                    if (user.Templateid == 0)
                    {

                        templateBuilder.Append("selected\r\n");

                    }	//end if

                    templateBuilder.Append(">默认</option>\r\n");

                    int template__loop__id = 0;
                    foreach (DataRow template in templatelist.Rows)
                    {
                        template__loop__id++;

                        templateBuilder.Append("								<option value=\"" + template["templateid"].ToString().Trim() + "\" \r\n");

                        if (user.Templateid == Utils.StrToInt(template["templateid"].ToString().Trim(), 0))
                        {

                            templateBuilder.Append("selected\r\n");

                        }	//end if

                        templateBuilder.Append("								>" + template["name"].ToString().Trim() + "</option>\r\n");

                    }	//end loop

                    templateBuilder.Append("							    </select>\r\n");
                    templateBuilder.Append("								<br />\r\n");
                    templateBuilder.Append("								<div class=\"photoimg\" style=\"border-top:1px dashed #CCC; margin-bottom:10px; \">\r\n");
                    templateBuilder.Append("								<img src=\"" + user.Avatar.ToString().Trim() + "\" \r\n");

                    if (user.Avatarwidth > 0)
                    {

                        templateBuilder.Append("width=\"" + user.Avatarwidth.ToString().Trim() + "\" height=\"" + user.Avatarheight.ToString().Trim() + "\"\r\n");

                    }	//end if

                    templateBuilder.Append(" id=\"usingavatarimg\" alt=\"形象图\"/>\r\n");

                    if (usergroupinfo.Allowavatar > 0)
                    {

                        templateBuilder.Append("										<ul id=\"avatarbox\">\r\n");

                        if (usergroupinfo.Allowavatar > 1)
                        {

                            templateBuilder.Append("											<li>\r\n");
                            templateBuilder.Append("												<input type=\"radio\" id=\"avatartype1\" name=\"avatartype\" value=\"2\" onclick=\"changeAvatarOption(this.value);\"\r\n");

                            //if (avatartype == 2)
                            //{

                            //    templateBuilder.Append("													checked=\"checked\"\r\n");

                            //}	//end if

                            templateBuilder.Append("/><label for=\"avatartype1\">使用外部图片</label>\r\n");
                            templateBuilder.Append("												<div id=\"enterurl\" style=\"display: none;\">\r\n");
                            templateBuilder.Append("													URL地址: <input name=\"avatarurl\" type=\"text\" id=\"avatarurl\" value=\"" + avatarurl.ToString() + "\" size=\"40\" />\r\n");
                            templateBuilder.Append("													宽度:\r\n");
                            templateBuilder.Append("													<input name=\"avatarwidth\" type=\"text\" id=\"avatarwidth\" readonly='true' value=\"120\" size=\"3\" maxlength=\"3\" style=\"width: 30px;\" /> &nbsp; &nbsp; \r\n");
                            templateBuilder.Append("													高度:\r\n");
                            templateBuilder.Append("													<input name=\"avatarheight\" type=\"text\" id=\"avatarheight\" readonly='true' value=\"120\" size=\"3\" maxlength=\"3\" style=\"width: 30px;\" />	\r\n");
                            templateBuilder.Append("												</div>\r\n");
                            templateBuilder.Append("											</li>\r\n");

                        }	//end if


                        if (usergroupinfo.Allowavatar > 2)
                        {

                            templateBuilder.Append("											<li>\r\n");
                            templateBuilder.Append("												<input type=\"radio\" id=\"avatartype2\" name=\"avatartype\" value=\"1\" onclick=\"changeAvatarOption(this.value);\"\r\n");

                            //if (avatartype == 1)
                            //{

                            //    templateBuilder.Append("													checked=\"checked\"\r\n");

                            //}	//end if

                            templateBuilder.Append("/><label for=\"avatartype2\">上传头像图片</label>\r\n");
                            templateBuilder.Append("												<div id=\"uploadfile\" style=\"display: none;\">\r\n");
                            templateBuilder.Append("												选择本地图片文件: <input name=\"file\" id=\"file\" type=\"file\" size=\"40\"/>\r\n");
                            templateBuilder.Append("												</div>\r\n");
                            templateBuilder.Append("											</li>\r\n");

                        }	//end if

                        templateBuilder.Append("										<li>\r\n");
                        templateBuilder.Append("											<input type=\"radio\" id=\"avatartype3\" name=\"avatartype\" value=\"0\" onclick=\"changeAvatarOption(this.value);BOX_show('fromsystem');\" \r\n");

                        //if (avatartype == 0)
                        //{

                        //    templateBuilder.Append("												checked=\"checked\"\r\n");

                        //}	//end if

                        templateBuilder.Append("											/><label for=\"avatartype3\">使用系统头像 (点击选择...)</label>\r\n");
                        templateBuilder.Append("											<input type=\"hidden\" name=\"usingavatar\" id=\"usingavatar\" value=\"" + avatar.ToString() + "\" />\r\n");
                        templateBuilder.Append("										</li>\r\n");
                        templateBuilder.Append("										</ul>\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("										<ul style=\"margin-top: 40px;\"><li>您所在的用户组 \"" + usergroupinfo.Grouptitle.ToString().Trim() + "\" 没有更改头像的权限</li></ul>\r\n");

                    }	//end if

                    templateBuilder.Append("								</div>\r\n");
                    templateBuilder.Append("								<input id=\"sendmsg\" type=\"submit\" value=\"确定\" name=\"sendmsg\"/>\r\n");
                    templateBuilder.Append("						</form>\r\n");

                }	//end if

                templateBuilder.Append("						</div>\r\n");

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



            if (page_err == 0 && !ispost)
            {

                templateBuilder.Append("	<div id=\"BOX_overlay\" style=\"background: #000; position: absolute; z-index:100; filter:alpha(opacity=50);-moz-opacity: 0.6;opacity: 0.6;\"></div>\r\n");
                templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/template_album.js\"></" + "script>\r\n");
                templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
                templateBuilder.Append("		function usethisavatar(e)\r\n");
                templateBuilder.Append("		{\r\n");
                templateBuilder.Append("			$('usingavatar').value = e.src;\r\n");
                templateBuilder.Append("			var avatars = $('fromsystem').getElementsByTagName('div');\r\n");
                templateBuilder.Append("			for (var i=0; i < avatars.length;i++)\r\n");
                templateBuilder.Append("			{\r\n");
                templateBuilder.Append("				avatars[i].style.border = \"2px dashed white\";\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("			e.parentNode.style.border = \"2px dashed black\";\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("		function selectionborder(e)\r\n");
                templateBuilder.Append("		{\r\n");
                templateBuilder.Append("			if (e.style.border != \"2px dashed black\" && e.style.border != \"black 2px dashed\")\r\n");
                templateBuilder.Append("			{				\r\n");
                templateBuilder.Append("				if (e.style.border == \"2px dashed red\" || e.style.border == \"red 2px dashed\" )\r\n");
                templateBuilder.Append("				{\r\n");
                templateBuilder.Append("					e.style.border = \"2px dashed white\";\r\n");
                templateBuilder.Append("				}\r\n");
                templateBuilder.Append("				else\r\n");
                templateBuilder.Append("				{\r\n");
                templateBuilder.Append("					e.style.border = \"2px dashed red\";\r\n");
                templateBuilder.Append("				}\r\n");
                templateBuilder.Append("			}\r\n");
                templateBuilder.Append("		}\r\n");
                templateBuilder.Append("	</" + "script>\r\n");
                templateBuilder.Append("	<div id=\"fromsystem\" class=\"avatarbackground\" style=\"display: none; position: relative;\">\r\n");
                templateBuilder.Append("			<div class=\"avatarlist\">\r\n");
                templateBuilder.Append("				<ul>\r\n");

                int avatarfile__loop__id = 0;
                foreach (DataRow avatarfile in avatarfilelist.Rows)
                {
                    avatarfile__loop__id++;

                    templateBuilder.Append("						<li>\r\n");
                    templateBuilder.Append("							<div onmouseover=\"selectionborder(this)\" onmouseout=\"selectionborder(this)\" style=\"\r\n");

                    if (avatarfile["filename"].ToString().Trim() == avatar)
                    {

                        templateBuilder.Append("border: 2px dashed black;\r\n");

                    }
                    else
                    {

                        templateBuilder.Append("border: 2px dashed white;\r\n");

                    }	//end if

                    templateBuilder.Append("cursor: pointer; overflow:hidden; zoom:1;\" ><img src=\"" + avatarfile["filename"].ToString().Trim() + "\" onclick=\"usethisavatar(this);\" alt=\"形象图名称\"/></div>\r\n");
                    templateBuilder.Append("						</li>\r\n");

                }	//end loop

                templateBuilder.Append("				</ul>\r\n");
                templateBuilder.Append("			</div>\r\n");
                templateBuilder.Append("			<span class=\"avatarbutton\">\r\n");
                templateBuilder.Append("				<input type=\"button\" name=\"userthisavatar\" id=\"userthisavatar\" value=\"确定\" onclick=\"$('templateid').style.display='';$('usingavatarimg').src=$('usingavatar').value;BOX_remove('fromsystem');\" />\r\n");
                templateBuilder.Append("				<input type=\"button\" name=\"canceluserthisavatar\" id=\"canceluserthisavatar\" value=\"取消\" onclick=\"$('templateid').style.display='';BOX_remove('fromsystem');\" />\r\n");
                templateBuilder.Append("			</span>\r\n");
                templateBuilder.Append("	</div>\r\n");

            }	//end if


            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
    }
}