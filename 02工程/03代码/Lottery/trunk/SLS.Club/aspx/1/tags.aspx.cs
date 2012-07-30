using Discuz.Common.Generic;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Common;
using System;

namespace Discuz.Web
{
    /// <summary>
    /// 标签列表页
    /// </summary>
    public partial class tags : PageBase
    {
        #region 页面变量

        /// <summary>
        /// 使用了指定标签的主题列表
        /// </summary>
        public List<MyTopicInfo> topiclist;
        /// <summary>
        /// 访问所请求的TagId
        /// </summary>
        public int tagid;
        /// <summary>
        /// Tag的详细信息
        /// </summary>
        public TagInfo tag;
        /// <summary>
        /// 页码
        /// </summary>
        public int pageid = 1;
        /// <summary>
        /// 主题数
        /// </summary>
        public int topiccount = 0;
        /// <summary>
        /// 日志数
        /// </summary>
        public int spacepostcount = 0;
        /// <summary>
        /// 图片数
        /// </summary>
        public int photocount = 0;
        /// <summary>
        /// 页数
        /// </summary>
        public int pagecount = 1;
        /// <summary>
        /// 分页页码链接
        /// </summary>
        public string pagenumbers;
        /// <summary>
        /// 当前列表类型,topic=主题列表,spacepost=日志列表,photo=图片列表
        /// </summary>
        public string listtype;
        /// <summary>
        /// 标签数组
        /// </summary>
        public TagInfo[] taglist;
        #endregion

        protected override void ShowPage()
        {
            
            if (config.Enabletag != 1)
            {
                AddErrLine("没有启用Tag功能");
                return;
            }

            tagid = DNTRequest.GetInt("tagid", 0);

            if (tagid > 0)
            {
                tag = Tags.GetTagInfo(tagid);
                if (tag == null)
                {
                    AddErrLine("指定的标签不存在");
                    return;
                }

                if (tag.Orderid < 0)
                {
                    AddErrLine("指定的标签已被关闭");
                }

                if (IsErr())
                {
                    return;
                }

                listtype = DNTRequest.GetString("t");

                pageid = DNTRequest.GetInt("page", 1);
                if (pageid < 1)
                {
                    pageid = 1;
                }
                pagetitle = tag.Tagname;
                if (listtype.Equals(""))
                {
                    listtype = "topic";
                
                }
                switch (listtype)
                {

                    case "topic":
                        topiccount = Topics.GetTopicsCountWithSameTag(tagid);
                        pagecount = topiccount % config.Tpp == 0 ? topiccount / config.Tpp : topiccount / config.Tpp + 1;

                        if (pagecount == 0)
                        {
                            pagecount = 1;
                        }
                        if (pageid > pagecount)
                        {
                            pageid = pagecount;
                        }

                        if (topiccount > 0)
                        {
                            topiclist = Topics.GetTopicsWithSameTag(tagid, pageid, config.Tpp);
                            pagenumbers = Utils.GetPageNumbers(pageid, pagecount, "tags.aspx?t=topic&tagid=" + tagid, 8);
                        }
                        else
                        {
                            AddMsgLine("该标签下暂无主题");
                        }
                        break;


                }
            }
            else
            {
                pagetitle = "标签";

                taglist = ForumTags.GetCachedHotForumTags(100);

            }
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:50.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:50. 
            */

            base.OnLoad(e);


            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("var aspxrewrite = " + config.Aspxrewrite.ToString().Trim() + ";\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\"><a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a> &raquo; <a href=\"tags.aspx\">标签</a> &raquo; \r\n");

            if (page_err == 0 && tagid > 0)
            {

                templateBuilder.Append("" + tag.Tagname.ToString().Trim() + "\r\n");

            }	//end if

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            if (page_err == 0)
            {


                if (tagid > 0)
                {

                    templateBuilder.Append("		<script type=\"text/javascript\">\r\n");
                    templateBuilder.Append("			function changeTab(obj)\r\n");
                    templateBuilder.Append("			{\r\n");
                    templateBuilder.Append("				if (obj.className == 'currenttab')\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					obj.className = '';\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("				else\r\n");
                    templateBuilder.Append("				{\r\n");
                    templateBuilder.Append("					obj.className = 'currenttab';\r\n");
                    templateBuilder.Append("				}\r\n");
                    templateBuilder.Append("			}\r\n");
                    templateBuilder.Append("		</" + "script>\r\n");
                    templateBuilder.Append("		<div class=\"searchtab\">\r\n");
                    templateBuilder.Append("			<a id=\"tab_forum\" \r\n");

                    if (listtype == "topic")
                    {

                        templateBuilder.Append("class=\"currenttab\" \r\n");

                    }
                    else
                    {

                        templateBuilder.Append(" onmouseout=\"changeTab(this)\" onmouseover=\"changeTab(this)\" style=\"cursor: pointer;\" href=\"\r\n");

                        if (config.Aspxrewrite == 1)
                        {

                            templateBuilder.Append("topictag-" + tagid.ToString() + ".aspx\r\n");

                        }
                        else
                        {

                            templateBuilder.Append("tags.aspx?tagid=" + tagid.ToString() + "\r\n");

                        }	//end if

                        templateBuilder.Append("\"\r\n");

                    }	//end if

                    templateBuilder.Append(">主题</a>\r\n");

                    templateBuilder.Append("		</div>\r\n");

                    if (listtype == "topic")
                    {


                        if (topiccount == 0)
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

                            templateBuilder.Append("				<DIV class=\"mainbox forumlist\">\r\n");
                            templateBuilder.Append("				<TABLE cellSpacing=\"0\" cellPadding=\"0\" summary=\"主题标签结果\">\r\n");
                            templateBuilder.Append("					<thead>\r\n");
                            templateBuilder.Append("					<tr>\r\n");
                            templateBuilder.Append("						<th>标题</th>\r\n");
                            templateBuilder.Append("						<th>所在版块</th>\r\n");
                            templateBuilder.Append("						<td>作者</td>\r\n");
                            templateBuilder.Append("						<td class=\"nums\">回复</td>\r\n");
                            templateBuilder.Append("						<td class=\"nums\">查看</td>\r\n");
                            templateBuilder.Append("						<td>最后发表</td>\r\n");
                            templateBuilder.Append("					</tr>\r\n");
                            templateBuilder.Append("					</thead>\r\n");

                            int topic__loop__id = 0;
                            foreach (MyTopicInfo topic in topiclist)
                            {
                                topic__loop__id++;

                                templateBuilder.Append("					<tbody>\r\n");
                                templateBuilder.Append("						<tr>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                aspxrewriteurl = this.ShowTopicAspxRewrite(topic.Tid, 0);

                                templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"_blank\">" + topic.Title.ToString().Trim() + "</a></td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                aspxrewriteurl = this.ShowForumAspxRewrite(topic.Fid, 0);

                                templateBuilder.Append("								<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Forumname.ToString().Trim() + "</a>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                templateBuilder.Append("								<p>\r\n");

                                if (Utils.StrToInt(topic.Posterid, 0) == -1)
                                {

                                    templateBuilder.Append("									游客\r\n");

                                }
                                else
                                {

                                    aspxrewriteurl = this.UserInfoAspxRewrite(topic.Posterid);

                                    templateBuilder.Append("									<a href=\"" + aspxrewriteurl.ToString() + "\">" + topic.Poster.ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("</p>\r\n");
                                templateBuilder.Append("								<em>\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic.Postdatetime).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</em>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("							<td class=\"nums\">" + topic.Replies.ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("							<td class=\"nums\">" + topic.Views.ToString().Trim() + "</td>\r\n");
                                templateBuilder.Append("							<td>\r\n");
                                templateBuilder.Append("									<em><a href=\"showtopic.aspx?topicid=" + topic.Tid.ToString().Trim() + "&page=end\" target=\"_blank\">\r\n");
                                templateBuilder.Append(Convert.ToDateTime(topic.Lastpost).ToString("yyyy.MM.dd HH:mm"));
                                templateBuilder.Append("</a></em>\r\n");
                                templateBuilder.Append("									<cite>by\r\n");

                                if (topic.Lastposterid == -1)
                                {

                                    templateBuilder.Append("										游客\r\n");

                                }
                                else
                                {

                                    templateBuilder.Append("										<a href=\"" + UserInfoAspxRewrite(topic.Lastposterid).ToString().Trim() + "\" target=\"_blank\">" + topic.Lastposter.ToString().Trim() + "</a>\r\n");

                                }	//end if

                                templateBuilder.Append("									</cite>\r\n");
                                templateBuilder.Append("							</td>\r\n");
                                templateBuilder.Append("						</tr>\r\n");
                                templateBuilder.Append("					</tbody>\r\n");

                            }	//end loop

                            templateBuilder.Append("					</table>			\r\n");
                            templateBuilder.Append("				</div>\r\n");
                            templateBuilder.Append("				<div class=\"pages_btns\">\r\n");
                            templateBuilder.Append("					<div class=\"pages\">\r\n");
                            templateBuilder.Append("						<em>" + pageid.ToString() + "/" + pagecount.ToString() + "页</em>" + pagenumbers.ToString() + "\r\n");
                            templateBuilder.Append("						<kbd>跳转<input name=\"gopage\" type=\"text\" id=\"gopage\" onKeyDown=\"if(event.keyCode==13) { window.location='tags.aspx?t=topic&tagid=" + tagid.ToString() + "&page=' + (parseInt(this.value) > 0 ? parseInt(this.value) : 1);}\" size=\"4\" maxlength=\"9\" class=\"colorblue2\"/>页</kbd>\r\n");
                            templateBuilder.Append("					</div>\r\n");
                            templateBuilder.Append("				</div>\r\n");

                        }	//end if


                    }

                }
                else
                {

                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"cache/tag/closedtags.txt\"></" + "script>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"cache/tag/colorfultags.txt\"></" + "script>\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/template_showtopic.js\"></" + "script>	\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/template_tags.js\"></" + "script>	\r\n");
                    templateBuilder.Append("		<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
                    templateBuilder.Append("		<div class=\"mainbox\">\r\n");
                    templateBuilder.Append("			<h3>论坛热门标签</h3>\r\n");
                    templateBuilder.Append("			<ul id=\"forumhottags\" class=\"taglist\">\r\n");

                    int tag__loop__id = 0;
                    foreach (TagInfo tag in taglist)
                    {
                        tag__loop__id++;

                        templateBuilder.Append("					<li><a \r\n");

                        if (config.Aspxrewrite == 1)
                        {

                            templateBuilder.Append("					href=\"topictag-" + tag.Tagid.ToString().Trim() + ".aspx\" \r\n");

                        }
                        else
                        {

                            templateBuilder.Append("					href=\"tags.aspx?t=topic&tagid=" + tag.Tagid.ToString().Trim() + "\" \r\n");

                        }	//end if


                        if (tag.Color != "")
                        {

                            templateBuilder.Append("					style=\"color: #" + tag.Color.ToString().Trim() + ";\"\r\n");

                        }	//end if

                        templateBuilder.Append("					title=\"" + tag.Fcount.ToString().Trim() + "\">" + tag.Tagname.ToString().Trim() + "</a></li>\r\n");

                    }	//end loop

                    templateBuilder.Append("			</ul>\r\n");
                    templateBuilder.Append("		</div>\r\n");

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

            templateBuilder.Append("	</div>\r\n");


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
