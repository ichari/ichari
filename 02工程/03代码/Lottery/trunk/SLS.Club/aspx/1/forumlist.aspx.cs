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
    /// 版块列表(分栏模式)
    /// </summary>
    public partial class forumlist : PageBase
    {
        #region 页面变量
        /// <summary>
        /// 当前登录的用户简要信息
        /// </summary>
        public ShortUserInfo userinfo;
        /// <summary>
        /// 总在线数
        /// </summary>
        public int totalonline;
        /// <summary>
        /// 总在线注册用户数
        /// </summary>
        public int totalonlineuser;
        /// <summary>
        /// 可用的扩展金币显示名称
        /// </summary>
        public string[] score;
        #endregion
        protected override void ShowPage()
        {
            pagetitle = "版块列表";

            if (config.Rssstatus == 1)
            {
                AddLinkRss("tools/rss.aspx", config.Forumtitle + "最新主题");
            }
            userinfo = new ShortUserInfo();
            if (userid != -1)
            {
                userinfo = Discuz.Forum.Users.GetShortUserInfo(userid);
                if (userinfo.Newpm == 0)
                {
                    base.newpmcount = 0;
                }
            }

            OnlineUsers.UpdateAction(olid, UserAction.IndexShow.ActionID, 0, config.Onlinetimeout);
            // 获得统计信息
            totalonline = onlineusercount;
            totalonlineuser = OnlineUsers.GetOnlineUserCount();

            score = Scoresets.GetValidScoreName();
        }

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:07.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:07. 
            */

            base.OnLoad(e);

            templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n");
            templateBuilder.Append("<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n");
            templateBuilder.Append("<head>\r\n");
            templateBuilder.Append("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n");
            templateBuilder.Append("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=7\" />\r\n");
            templateBuilder.Append("" + meta.ToString() + "\r\n");

            if (pagetitle == "首页")
            {

                templateBuilder.Append("<title>" + config.Forumtitle.ToString().Trim() + "</title>\r\n");

            }
            else
            {

                templateBuilder.Append("<title>" + pagetitle.ToString() + " - " + config.Forumtitle.ToString().Trim() + "</title>\r\n");

            }	//end if
            
            templateBuilder.Append("<link rel=\"stylesheet\" href=\"templates/" + templatepath.ToString() + "/dnt.css\" type=\"text/css\" media=\"all\"  />\r\n");
            templateBuilder.Append("" + link.ToString() + "\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_report.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/template_utils.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/common.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\" src=\"javascript/menu.js\"></" + "script>\r\n");
            templateBuilder.Append("<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("	var aspxrewrite = " + config.Aspxrewrite.ToString().Trim() + ";\r\n");
            templateBuilder.Append("</" + "script>\r\n");
            templateBuilder.Append("" + script.ToString() + "\r\n");
            templateBuilder.Append("</head>\r\n");


            templateBuilder.Append("<style>\r\n");
            templateBuilder.Append("body{\r\n");
            templateBuilder.Append("	margin:0px;\r\n");
            templateBuilder.Append("	padding:0px;\r\n");
            templateBuilder.Append("	text-align:left;\r\n");
            //templateBuilder.Append("	background:#F5FAFD url(images/left-bg.jpg) repeat-y top right;\r\n");
            templateBuilder.Append("}\r\n");
            templateBuilder.Append(".collapse { BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(templates/" + templatepath.ToString() + "\r\n");
            templateBuilder.Append("	/images/collapse.gif); WIDTH: 6px; BACKGROUND-REPEAT: no-repeat; POSITION: absolute; HEIGHT: 50px; }\r\n");
            templateBuilder.Append("	.expand { BACKGROUND-POSITION: center center; BACKGROUND-IMAGE: url(templates/" + templatepath.ToString() + "\r\n");
            templateBuilder.Append("	/images/expand.gif); WIDTH: 6px; BACKGROUND-REPEAT: no-repeat; POSITION: absolute; HEIGHT: 50px; }\r\n");
            templateBuilder.Append("</style>\r\n");
            templateBuilder.Append("<body onLoad=\"window_load();\">\r\n");
            templateBuilder.Append("	<script type=\"text/javascript\" src=\"javascript/ajax.js\"></" + "script>\r\n");
            templateBuilder.Append("	<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("			var NoUser = " + userid.ToString() + " == -1 ? true : false;\r\n");
            templateBuilder.Append("			var lastA = null;\r\n");
            templateBuilder.Append("			function window_load(){\r\n");
            templateBuilder.Append("				documentbody = document.documentElement.clientHeight > document.body.clientHeight ? document.documentElement : document.body;\r\n");
            templateBuilder.Append("				var leftbar = document.getElementById('leftbar')\r\n");
            templateBuilder.Append("				leftbar.style.height = documentbody.clientHeight +'px';\r\n");
            templateBuilder.Append("				leftbar.style.left = 0; //document.body.clientWidth - 6;\r\n");
            templateBuilder.Append("				leftbar.style.top = documentbody.scrollTop + 'px'; //document.body.clientWidth - 6;\r\n");
            templateBuilder.Append("				document.onscroll = function(){ \r\n");
            templateBuilder.Append("												leftbar.style.height=documentbody.clientHeight +'px';\r\n");
            templateBuilder.Append("												leftbar.style.top=documentbody.scrollTop + 'px'; \r\n");
            templateBuilder.Append("											}\r\n");
            templateBuilder.Append("				document.onresize = function(){ \r\n");
            templateBuilder.Append("												leftbar.style.height=documentbody.clientHeight +'px';\r\n");
            templateBuilder.Append("												leftbar.style.top=documentbody.scrollTop + 'px'; \r\n");
            templateBuilder.Append("											}\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			function resizediv_onClick(){\r\n");
            templateBuilder.Append("				if (document.getElementById('menubar').style.display != 'none'){\r\n");
            templateBuilder.Append("					top.document.getElementsByTagName('FRAMESET')[0].cols = \"6,*\";\r\n");
            templateBuilder.Append("					document.getElementById('menubar').style.display = 'none';\r\n");
            templateBuilder.Append("					document.getElementById('leftbar').className = \"expand\";\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("				else{\r\n");
            templateBuilder.Append("					top.document.getElementsByTagName('FRAMESET')[0].cols = \"210,*\";\r\n");
            templateBuilder.Append("					document.getElementById('leftbar').className = \"collapse\";\r\n");
            templateBuilder.Append("					document.getElementById('menubar').style.display = '';\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			//↓----------获得版块的树形列表相关脚本-------------------------\r\n");
            templateBuilder.Append("  			function changeExtImg(objImg){\r\n");
            templateBuilder.Append("				if (!objImg){ return; }	\r\n");
            templateBuilder.Append("				var fileName = objImg.src.toLowerCase().substring(objImg.src.lastIndexOf(\"/\"));\r\n");
            templateBuilder.Append("				switch(fileName){\r\n");
            templateBuilder.Append("					case \"/p0.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/m0.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/p1.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/m1.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/p2.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/m2.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/p3.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/m3.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/m0.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/p0.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/m1.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/p1.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/m2.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/p2.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/m3.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/p3.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("  			function changeFolderImg(objImg){\r\n");
            templateBuilder.Append("				if (!objImg){ return; }	\r\n");
            templateBuilder.Append("				var fileName = objImg.src.toLowerCase().substring(objImg.src.lastIndexOf(\"/\"));\r\n");
            templateBuilder.Append("				switch(fileName){\r\n");
            templateBuilder.Append("					case \"/folder.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/folderopen.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("					case \"/folderopen.gif\":\r\n");
            templateBuilder.Append("						objImg.src = \"images/tree/folder.gif\";\r\n");
            templateBuilder.Append("						break;\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			function a_click(objA){\r\n");
            templateBuilder.Append("				if (lastA){\r\n");
            templateBuilder.Append("					lastA.className=''; \r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("				objA.className='bold'; \r\n");
            templateBuilder.Append("				lastA = objA; \r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("  			function writesubforum(objreturn,fid,AtEnd){\r\n");
            templateBuilder.Append("				var process = document.getElementById(\"process_\" + fid);\r\n");
            templateBuilder.Append("				var forum = document.getElementById(\"forum_\" + fid);\r\n");
            templateBuilder.Append("				var dataArray = objreturn.getElementsByTagName('forum');\r\n");
            templateBuilder.Append("				var dataArrayLen = dataArray.length;\r\n");
            templateBuilder.Append("				changeExtImg(document.getElementById(\"forumExt_\" + fid));\r\n");
            templateBuilder.Append("				changeFolderImg(document.getElementById(\"forumFolder_\" + fid));\r\n");
            templateBuilder.Append("				for (i=0;i<dataArrayLen;i++){\r\n");
            templateBuilder.Append("					var thisfid = dataArray[i].getAttribute(\"fid\");\r\n");
            templateBuilder.Append("					var subforumcount = dataArray[i].getAttribute(\"subforumcount\");\r\n");
            templateBuilder.Append("					var thisEnd = i==dataArrayLen-1;\r\n");
            templateBuilder.Append("					var layer = dataArray[i].getAttribute(\"layer\");\r\n");
            templateBuilder.Append("						//显示树型线\r\n");
            templateBuilder.Append("						list = \"\";\r\n");
            templateBuilder.Append("						for (l=1;l<=layer;l++){\r\n");
            templateBuilder.Append("							if (AtEnd && NoUser){\r\n");
            templateBuilder.Append("								list += \"<nobr><img src = \\\"images/tree/L5.gif\\\" align=\\\"absmiddle\\\" />\";\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("							else{\r\n");
            templateBuilder.Append("								list += \"<img src = \\\"images/tree/L4.gif\\\" align=\\\"absmiddle\\\" />\";\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("						if (subforumcount>0){\r\n");
            templateBuilder.Append("							folder = \"folder.gif\";\r\n");
            templateBuilder.Append("							if (layer==0 && thisEnd){\r\n");
            templateBuilder.Append("								if (NoUser){\r\n");
            templateBuilder.Append("									src = \"p2.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("								else{\r\n");
            templateBuilder.Append("									src = \"p1.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("							else{\r\n");
            templateBuilder.Append("								if (thisEnd && layer>0){\r\n");
            templateBuilder.Append("									src = \"P2.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("								else{\r\n");
            templateBuilder.Append("									//if (i==0 && layer==0){\r\n");
            templateBuilder.Append("									//	src = \"P0.gif\";\r\n");
            templateBuilder.Append("									//}\r\n");
            templateBuilder.Append("									//else{\r\n");
            templateBuilder.Append("										src = \"P1.gif\";\r\n");
            templateBuilder.Append("									//}\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("						else{\r\n");
            templateBuilder.Append("							folder = \"file.gif\";\r\n");
            templateBuilder.Append("							if (layer==0 && thisEnd){\r\n");
            templateBuilder.Append("								if (NoUser){\r\n");
            templateBuilder.Append("									src = \"m2.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("								else{\r\n");
            templateBuilder.Append("									src = \"m1.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("							else{\r\n");
            templateBuilder.Append("								if (thisEnd){\r\n");
            templateBuilder.Append("									src = \"L2.gif\";\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("								else{\r\n");
            templateBuilder.Append("									//if (i==0 && layer==0){\r\n");
            templateBuilder.Append("									//	src = \"L0.gif\";\r\n");
            templateBuilder.Append("									//}\r\n");
            templateBuilder.Append("									//else{\r\n");
            templateBuilder.Append("										src = \"L1.gif\";\r\n");
            templateBuilder.Append("									//}\r\n");
            templateBuilder.Append("								}\r\n");
            templateBuilder.Append("							}\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("                        if(" + config.Aspxrewrite.ToString().Trim() + ")\r\n");
            templateBuilder.Append("                        {\r\n");
            templateBuilder.Append("						    list += \"<img id=\\\"forumExt_\" + thisfid + \"\\\" src = \\\"images/tree/\" + src + \"\\\" align=\\\"absmiddle\\\" /><img id=\\\"forumFolder_\" + thisfid + \"\\\" src = \\\"images/tree/\" + folder + \"\\\" align=\\\"absmiddle\\\" /> <a href=\\\"showforum-\" + thisfid + \".aspx\\\" target=\\\"main\\\" title=\\\"\" + dataArray[i].getAttribute(\"name\") + \"\\\" onclick=\\\"a_click(this);\\\">\" + dataArray[i].getAttribute(\"name\") + \"</a></nobr>\";\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("						else\r\n");
            templateBuilder.Append("						{\r\n");
            templateBuilder.Append("						    list += \"<img id=\\\"forumExt_\" + thisfid + \"\\\" src = \\\"images/tree/\" + src + \"\\\" align=\\\"absmiddle\\\" /><img id=\\\"forumFolder_\" + thisfid + \"\\\" src = \\\"images/tree/\" + folder + \"\\\" align=\\\"absmiddle\\\" /> <a href=\\\"showforum.aspx?forumid=\" + thisfid + \"\\\" target=\\\"main\\\" title=\\\"\" + dataArray[i].getAttribute(\"name\") + \"\\\" onclick=\\\"a_click(this);\\\">\" + dataArray[i].getAttribute(\"name\") + \"</a></nobr>\";\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("					var div_forumtitle =  document.createElement(\"DIV\");\r\n");
            templateBuilder.Append("						div_forumtitle.id = \"forumtitle_\" + thisfid;\r\n");
            templateBuilder.Append("						div_forumtitle.className = \"tree_forumtitle\";\r\n");
            templateBuilder.Append("						if (subforumcount>0){\r\n");
            templateBuilder.Append("							div_forumtitle.onclick = new Function(\"getsubforum(\" + thisfid + \",\" + thisEnd + \");\");\r\n");
            templateBuilder.Append("						}\r\n");
            templateBuilder.Append("						div_forumtitle.innerHTML = list;\r\n");
            templateBuilder.Append("						forum.appendChild(div_forumtitle);\r\n");
            templateBuilder.Append("					var div_forum = document.createElement(\"DIV\");\r\n");
            templateBuilder.Append("						div_forum.id = \"forum_\" + thisfid;\r\n");
            templateBuilder.Append("						div_forum.className = \"tree_forum\";\r\n");
            templateBuilder.Append("						forum.appendChild(div_forum);\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("				process.style.display=\"none\";\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			function getsubforum(fid,AtEnd){\r\n");
            templateBuilder.Append("				if (!document.getElementById(\"forum_\" + fid)){\r\n");
            templateBuilder.Append("					document.writeln(\"<div id=\\\"forum_\" + fid + \"\\\"></div>\");\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("				if (!document.getElementById(\"process_\" + fid)){\r\n");
            templateBuilder.Append("					var div = document.createElement(\"DIV\");\r\n");
            templateBuilder.Append("					div.id = \"process_\" + fid;\r\n");
            templateBuilder.Append("					div.className = \"tree_process\";\r\n");
            templateBuilder.Append("					div.innerHTML = \"<img src='images/common/loading.gif' />载入中...\";\r\n");
            templateBuilder.Append("					document.getElementById(\"forum_\" + fid).appendChild(div);\r\n");
            templateBuilder.Append("					ajaxRead(\"tools/ajax.aspx?t=forumtree&fid=\" + fid, \"writesubforum(obj,\" + fid+ \",\" + AtEnd + \");\");\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("				else{\r\n");
            templateBuilder.Append("					changeExtImg(document.getElementById(\"forumExt_\" + fid));\r\n");
            templateBuilder.Append("					changeFolderImg(document.getElementById(\"forumFolder_\" + fid));\r\n");
            templateBuilder.Append("					if (document.getElementById(\"forum_\" + fid).style.display == \"none\"){\r\n");
            templateBuilder.Append("						document.getElementById(\"forum_\" + fid).style.display = \"block\";\r\n");
            templateBuilder.Append("					}\r\n");
            templateBuilder.Append("					else{												\r\n");
            templateBuilder.Append("						document.getElementById(\"forum_\" + fid).style.display = \"none\";\r\n");
            templateBuilder.Append("					}\r\n");
            templateBuilder.Append("				}\r\n");
            templateBuilder.Append("			}\r\n");
            templateBuilder.Append("			//↑----------获得版块的树形列表相关脚本-------------------------\r\n");
            templateBuilder.Append("	</" + "script>\r\n");
            templateBuilder.Append("	<div id=\"leftbar\" class=\"collapse\" onmouseover=\"this.style.backgroundColor='#A7E8F3';\"\r\n");
            templateBuilder.Append("		onmouseout=\"this.style.backgroundColor = '';\" onclick=\"resizediv_onClick()\" style=\"width:6px; cursor:pointer\"\r\n");
            templateBuilder.Append("		title=\"打开/关闭导航\">\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div id=\"menubar\" style=\"white-space:nowrap;\" valign=\"top\">\r\n");
            templateBuilder.Append("	<div id=\"frameback\">\r\n");
            templateBuilder.Append("		<strong><A href=\"###\" onClick=\"resizediv_onClick()\">隐藏侧栏</a></strong><em><A href=\"forumindex.aspx?f=0\" target=\"_top\">平板模式</a></em>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div class=\"framemenu\">\r\n");
            templateBuilder.Append("		<ul>\r\n");

            if (userid != -1)
            {

                templateBuilder.Append("				<li>欢迎访问" + config.Forumtitle.ToString().Trim() + "</li><br />\r\n");
                templateBuilder.Append("				<li><strong>\r\n");
                aspxrewriteurl = this.UserInfoAspxRewrite(userinfo.Uid);

                templateBuilder.Append("				<a href=\"" + aspxrewriteurl.ToString() + "\" target=\"main\" >" + userinfo.Username.ToString().Trim() + "</a> </strong>[ <a href=\"logout.aspx?userkey=" + userkey.ToString() + "&amp;reurl=focuslist.aspx\" target=\"main\">退出</a> ]</li>\r\n");
                templateBuilder.Append("				<li>金币: <strong>" + userinfo.Credits.ToString().Trim() + "</strong>  [<span id=\"creditlist\" onMouseOver=\"showMenu(this.id, false);\" style=\"CURSOR:pointer\">详细金币</span>]\r\n");
                templateBuilder.Append("			</li>\r\n");
                templateBuilder.Append("				<li>头衔: " + usergroupinfo.Grouptitle.ToString().Trim() + "\r\n");

                if (useradminid == 1)
                {

                    templateBuilder.Append("						| <a href=\"admin/index.aspx\" target=\"_blank\">系统设置</a>\r\n");

                }	//end if

                templateBuilder.Append("				</li>\r\n");

            }
            else
            {

                templateBuilder.Append("				<li>头衔: 游客\r\n");
                templateBuilder.Append("					[<a href=\"register.aspx\" target=\"main\">注册</a>] \r\n");
                templateBuilder.Append("			[<a href=\"login.aspx?reurl=focuslist.aspx\" target=\"main\">登录</a>]\r\n");
                templateBuilder.Append("				</li>\r\n");

            }	//end if


            if (newpmcount > 0)
            {

                templateBuilder.Append("			<li>\r\n");
                templateBuilder.Append("				新的短消息<a href=\"usercpinbox.aspx\" target=\"main\"><span id=\"newpmcount\">" + newpmcount.ToString() + "</span></a>条\r\n");
                templateBuilder.Append("			</li>\r\n");

            }	//end if

            templateBuilder.Append("			<li>\r\n");
            templateBuilder.Append("				<img src=\"templates/" + templatepath.ToString() + "/images/home.gif\">\r\n");
            templateBuilder.Append("				<a href=\"focuslist.aspx\" target=\"main\">论坛首页</a>\r\n");
            templateBuilder.Append("			</li>\r\n");
            templateBuilder.Append("			<li>\r\n");
            templateBuilder.Append("				<img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" /><img src=\"templates/" + templatepath.ToString() + "/images/folder_new.gif\" width=\"20\" height=\"20\" />\r\n");
            templateBuilder.Append("				<a href=\"showtopiclist.aspx?type=newtopic&amp;newtopic=" + newtopicminute.ToString() + "&amp;forums=all\" target=\"main\">\r\n");
            templateBuilder.Append("					查看新帖</a>\r\n");
            templateBuilder.Append("			</li>\r\n");
            templateBuilder.Append("			<li>\r\n");
            templateBuilder.Append("				<img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\"><img src=\"templates/" + templatepath.ToString() + "/images/showdigest.gif\" width=\"20\" height=\"20\" />\r\n");
            templateBuilder.Append("				<a href=\"showtopiclist.aspx?type=digest&amp;forums=all\" target=\"main\">精华帖区</a>\r\n");
            templateBuilder.Append("			</li>\r\n");
            templateBuilder.Append("	  </ul>\r\n");
            templateBuilder.Append("			<script type=\"text/javascript\">\r\n");
            templateBuilder.Append("				//生成版块列表\r\n");
            templateBuilder.Append("				getsubforum(0);\r\n");
            templateBuilder.Append("			</" + "script>\r\n");

            if (userid != -1)
            {

                templateBuilder.Append("		<div onClick=\"getsubforum(-1,true);\">\r\n");
                templateBuilder.Append("			<img id=\"forumExt_-1\" src=\"images/tree/M2.gif\" width=\"20\" height=\"20\" /><img id=\"forumFolder_-1\" src=\"templates/" + templatepath.ToString() + "/images/mytopic.gif\" /><span class=\"cursor\">用户功能区</span>\r\n");
                templateBuilder.Append("		</div>\r\n");
                templateBuilder.Append("		<div id=\"process_-1\"></div>\r\n");
                templateBuilder.Append("		<div id=\"forum_-1\" style=\"DISPLAY:block\">\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/folder_mytopic.gif\" width=\"16\" height=\"16\">\r\n");
                templateBuilder.Append("				<a href=\"mytopics.aspx\" target=\"main\">我的主题</a></div>\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/folder.gif\" width=\"16\" height=\"16\">\r\n");
                templateBuilder.Append("				<a href=\"myposts.aspx\" target=\"main\">我的帖子</a></div>\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/digest.gif\">\r\n");
                templateBuilder.Append("				<a href=\"search.aspx?posterid=" + userid.ToString() + "&amp;type=digest\" target=\"main\">我的精华</a></div>\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/favorite.gif\">\r\n");
                templateBuilder.Append("				<a href=\"usercpsubscribe.aspx\" target=\"main\">我的收藏</a></div>\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L1.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/moderate.gif\">\r\n");
                templateBuilder.Append("				<a href=\"usercp.aspx\" target=\"main\">用户中心</a></div>\r\n");
                templateBuilder.Append("			<div><img src=\"images/tree/L5.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"images/tree/L2.gif\" width=\"20\" height=\"20\" border=\"0\"><img src=\"templates/" + templatepath.ToString() + "/images/postpm.gif\" width=\"16\" height=\"16\">\r\n");
                templateBuilder.Append("				<a href=\"usercppostpm.aspx\" target=\"main\">撰写短消息</a></div>\r\n");
                templateBuilder.Append("		</div>\r\n");

            }	//end if

            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("	<div class=\"framemenu\">\r\n");
            templateBuilder.Append("		<ul>\r\n");
            templateBuilder.Append("			<li>在线用户: </li>\r\n");
            templateBuilder.Append("			<li>" + totalonline.ToString() + "人在线  (" + totalonlineuser.ToString() + "位会员) </li>\r\n");

            if (config.Rssstatus != 0)
            {

                templateBuilder.Append("			<li>\r\n");
                templateBuilder.Append("			<a href=\"tools/rss.aspx\" target=\"_blank\"><img src=\"templates/" + templatepath.ToString() + "/images/rss2.gif\" alt=\"RSS订阅全部论坛\"></a>\r\n");
                templateBuilder.Append("			</li>\r\n");

            }	//end if

            templateBuilder.Append("	  </ul>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");


            templateBuilder.Append("<div id=\"creditlist_menu\" class=\"popupmenu_popup\" style=\"display:none;\">\r\n");

            if (score[1].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[1].ToString().Trim() + ": " + userinfo.Extcredits1.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[2].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[2].ToString().Trim() + ": " + userinfo.Extcredits2.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[3].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[3].ToString().Trim() + ": " + userinfo.Extcredits3.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[4].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[4].ToString().Trim() + ": " + userinfo.Extcredits4.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[5].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[5].ToString().Trim() + ": " + userinfo.Extcredits5.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[6].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[6].ToString().Trim() + ": " + userinfo.Extcredits6.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[7].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[7].ToString().Trim() + ": " + userinfo.Extcredits7.ToString().Trim() + "<br />\r\n");

            }	//end if


            if (score[8].ToString().Trim() != "")
            {

                templateBuilder.Append("	" + score[8].ToString().Trim() + ": " + userinfo.Extcredits8.ToString().Trim() + "<br />\r\n");

            }	//end if

            templateBuilder.Append("</div>\r\n");

            Response.Write(templateBuilder.ToString());
        }
    }
}