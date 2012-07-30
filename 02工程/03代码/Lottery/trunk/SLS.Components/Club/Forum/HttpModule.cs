using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Xml;
using Discuz.Common;
using Discuz.Config;
using Discuz.Config.Provider;
using Discuz.Forum.ScheduledEvents;

namespace Discuz.Forum
{
    /// <summary>
    /// 论坛HttpModule类
    /// </summary>
    public class HttpModule : System.Web.IHttpModule
    {
        //public readonly static Mutex m=new Mutex();
        static Timer eventTimer;

        /// <summary>
        /// 实现接口的Init方法
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            try
            {
                OnlineUsers.ResetOnlineList();
                context.BeginRequest += new EventHandler(ReUrl_BeginRequest);

                if (eventTimer == null && ScheduleConfigs.GetConfig().Enabled)
                {
                    EventManager.RootPath = AppDomain.CurrentDomain.BaseDirectory;
                    eventTimer = new Timer(new TimerCallback(ScheduledEventWorkCallback), context.Context, 60000, EventManager.TimerMinutesInterval * 60000);
                }
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog("Discuz.Forum.HttpModule Init:" + e.Message);
            }
        }

        private void ScheduledEventWorkCallback(object sender)
        {
            try
            {
                if (ScheduleConfigs.GetConfig().Enabled)
                {
                    EventManager.Execute();
                }
            }
            catch(Exception e)
            {
                EventLogs.WriteFailedLog("Failed ScheduledEventCallBack:" + e.Message);
            }

        }

        public void Application_OnError(Object sender, EventArgs e)
        {
            
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            EventLogs.WriteFailedLog("Discuz.Forum.HttpModule Application_OnError: " + context.Server.GetLastError().Message);
            ////if (context.Server.GetLastError().GetBaseException() is MyException)
            //{
            //    //MyException ex = (MyException) context.Server.GetLastError().GetBaseException();
            //    context.Response.Write("<html><body style=\"font-size:14px;\">");
            //    context.Response.Write("Discuz!NT Error:<br />");
            //    context.Response.Write("<textarea name=\"errormessage\" style=\"width:80%; height:200px; word-break:break-all\">");
            //    context.Response.Write(System.Web.HttpUtility.HtmlEncode(context.Server.GetLastError().ToString()));
            //    context.Response.Write("</textarea>");
            //    context.Response.Write("</body></html>");
            //    context.Response.End();
            //}

        }

        /// <summary>
        /// 实现接口的Dispose方法
        /// </summary>
        public void Dispose()
        {
            eventTimer = null;
        }


        /// <summary>
        /// 重写Url
        /// </summary>
        /// <param name="sender">事件的源</param>
        /// <param name="e">包含事件数据的 EventArgs</param>
        private void ReUrl_BeginRequest(object sender, EventArgs e)
        {
            BaseConfigInfo baseconfig = new BaseConfigInfo();
            if (baseconfig == null)
            {
                return;
            }
            GeneralConfigInfo config = GeneralConfigs.GetConfig();
            HttpContext context = ((HttpApplication)sender).Context;
            string forumPath = (context.Request.ApplicationPath).ToLower();

            //if (context.Request.Url.AbsoluteUri.IndexOf("localhost") > 0 || context.Request.Url.AbsoluteUri.IndexOf("127.0.0.1") > 0)
            //{
            //    forumPath = forumPath + "/";
            //}

            forumPath = forumPath + "/";

            string requestPath = context.Request.Path.ToLower();

            if (requestPath.StartsWith(forumPath) && !requestPath.EndsWith(".axd"))
            {
                if (requestPath.Substring(forumPath.Length).IndexOf("/") == -1)
                {                    // 当前样式id   
                    string strTemplateid = config.Templateid.ToString();
                    if (Utils.InArray(Utils.GetCookie(Utils.GetTemplateCookieName()), Templates.GetValidTemplateIDList()))
                    {
                        strTemplateid = Utils.GetCookie(Utils.GetTemplateCookieName());
                    }

                    if (requestPath.EndsWith("/index.aspx"))
                    {
                        if (config.Indexpage == 0)
                        {
                            if (config.BrowseCreateTemplate == 1)
                            {
                                CreateTemplate(forumPath, Templates.GetTemplateItem(int.Parse(strTemplateid)).Directory, "forumindex.aspx", int.Parse(strTemplateid));
                            }
                            context.RewritePath(forumPath + "aspx/" + strTemplateid + "/forumindex.aspx");
                        }
                        else
                        {
                            if (config.BrowseCreateTemplate == 1)
                            {
                                CreateTemplate(forumPath, Templates.GetTemplateItem(int.Parse(strTemplateid)).Directory, "website.aspx", int.Parse(strTemplateid));
                            }
                            context.RewritePath(forumPath + "aspx/" + strTemplateid + "/website.aspx");
                        }

                        return;
                    }


                    //当使用伪aspx, 如:showforum-1.aspx等.
                    if (config.Aspxrewrite == 1)
                    {
                        foreach (SiteUrls.URLRewrite url in SiteUrls.GetSiteUrls().Urls)
                        {
                            if (Regex.IsMatch(requestPath, url.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                            {
                                string newUrl = Regex.Replace(requestPath.Substring(context.Request.Path.LastIndexOf("/")), url.Pattern, url.QueryString, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);
                                if (config.BrowseCreateTemplate == 1)
                                {
                                    CreateTemplate(forumPath, Templates.GetTemplateItem(int.Parse(strTemplateid)).Directory, url.Page.Replace("/", ""), int.Parse(strTemplateid));
                                }
                                //通过参数设置指定模板
                                if (config.Specifytemplate > 0)
                                {
                                    strTemplateid = SelectTemplate(strTemplateid, url.Page, newUrl);
                                }
                                context.RewritePath(forumPath + "aspx/" + strTemplateid + url.Page, string.Empty, newUrl + "&selectedtemplateid=" + strTemplateid);

                                return;
                            }
                        }
                    }

                    if (config.BrowseCreateTemplate == 1)
                    {
                        if (requestPath.IndexOf("showtemplate.aspx") != -1)
                        {
                            CreateTemplate(forumPath,
                                Templates.GetTemplateItem(DNTRequest.GetInt("templateid", 1)).Directory,
                                config.Indexpage == 0 ? "forumindex.aspx" : "website.aspx",
                                DNTRequest.GetInt("templateid", 1)); //当跳转模板页时，生成目标文件
                        }
                        CreateTemplate(forumPath, Templates.GetTemplateItem(int.Parse(strTemplateid)).Directory, requestPath.Substring(context.Request.Path.LastIndexOf("/")).Replace("/", ""), int.Parse(strTemplateid));

                    }
                    //通过参数设置指定模板
                    if (config.Specifytemplate > 0)
                    {
                        strTemplateid = SelectTemplate(strTemplateid, requestPath, context.Request.QueryString.ToString());
                    }
                    context.RewritePath(forumPath + "aspx/" + strTemplateid + requestPath.Substring(context.Request.Path.LastIndexOf("/")), string.Empty, context.Request.QueryString.ToString() + "&selectedtemplateid=" + strTemplateid);
                }
                else if (requestPath.StartsWith(forumPath + "archiver/"))
                {
                    //当使用伪aspx, 如:showforum-1.aspx等.
                    if (config.Aspxrewrite == 1)
                    {
                        string path = requestPath.Substring(forumPath.Length + 8);
                        foreach (SiteUrls.URLRewrite url in SiteUrls.GetSiteUrls().Urls)
                        {
                            if (Regex.IsMatch(path, url.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                            {
                                string newUrl = Regex.Replace(path, url.Pattern, url.QueryString, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);

                                context.RewritePath(forumPath + "archiver" + url.Page, string.Empty, newUrl);
                                return;
                            }
                        }

                    }
                    return;
                }
                else if (requestPath.StartsWith(forumPath + "tools/"))
                {
                    //当使用伪aspx, 如:showforum-1.aspx等.
                    if (config.Aspxrewrite == 1)
                    {
                        string path = requestPath.Substring(forumPath.Length + 5);
                        foreach (SiteUrls.URLRewrite url in SiteUrls.GetSiteUrls().Urls)
                        {
                            if (Regex.IsMatch(path, url.Pattern, Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase))
                            {
                                string newUrl = Regex.Replace(path, url.Pattern, Utils.UrlDecode(url.QueryString), Utils.GetRegexCompiledOptions() | RegexOptions.IgnoreCase);

                                context.RewritePath(forumPath + "tools" + url.Page, string.Empty, newUrl);
                                return;
                            }
                        }
                    }
                    return;
                }
                else if (requestPath.StartsWith(forumPath + "upload/") || requestPath.StartsWith(forumPath + "space/upload/"))
                {
                    context.RewritePath(forumPath + "index.aspx");
                    return;
                }
            }
        }

        /// <summary>
        /// 根据参数信息选择相应的模板
        /// </summary>
        /// <param name="strTemplateid">默认的模板ID</param>
        /// <param name="pagename">请求的页面名称</param>
        /// <param name="newUrl">请求参数</param>
        /// <returns>返回相应的模板ID</returns>
        public string SelectTemplate(string strTemplateid, string pagename, string newUrl)
        {
            string pagenamelist = "showforum,showtopic,showdebate,showbonus,posttopic,postreply,showtree,editpost,delpost,topicadmin";

            int forumid = 0;
            //要截取的字段串的开始位置
            int startindex = pagename.LastIndexOf("/") + 1;
            //如果是指定的页面则进行模板查询

            if (Utils.InArray(pagename.Substring(startindex, pagename.LastIndexOf(".") - startindex), pagenamelist))
            {
                foreach (string urlvalue in newUrl.Split('&'))
                {
                    if ((urlvalue.IndexOf("forumid=") >= 0) && (urlvalue.Split('=')[1] != ""))
                    {
                        forumid = Utils.StrToInt(urlvalue.Split('=')[1], 0);
                    }
                    else
                    {
                        if ((urlvalue.IndexOf("topicid=") >= 0) && (urlvalue.Split('=')[1] != ""))
                        {
                            Discuz.Entity.TopicInfo topicinfo = Topics.GetTopicInfo(Utils.StrToInt(urlvalue.Split('=')[1], 0));
                            //主题存在时
                            if (topicinfo != null)
                            {
                                forumid = topicinfo.Fid;
                            }
                        }
                        else
                        {
                            forumid = DNTRequest.GetInt("forumid", 0);
                        }
                    }

                    if (forumid > 0)
                    {
                        int templateid = Forums.GetForumInfo(forumid).Templateid;

                        //当前版块未指定模板时(使用用户选择的模版或系统默认模板)
                        if (templateid <= 0)
                        {
                            //从cookie中获取用户选择的模板
                            if (Utils.InArray(Utils.GetCookie(Utils.GetTemplateCookieName()), Templates.GetValidTemplateIDList()))
                            {
                                templateid = Utils.StrToInt(Utils.GetCookie(Utils.GetTemplateCookieName()), GeneralConfigs.GetConfig().Templateid);
                            }

                            //使用系统默认模板
                            if (templateid == 0)
                            {
                                templateid = GeneralConfigs.GetConfig().Templateid;
                            }
                        }
                        strTemplateid = templateid.ToString();
                        break;
                    }
                }

            }

            return strTemplateid;
        }

        private void CreateTemplate(string forumpath, string templatepath, string pagename, int templateid)
        {
            if (!Directory.Exists(Utils.GetMapPath(forumpath + "aspx/" + templateid)))
            {
                Directory.CreateDirectory(Utils.GetMapPath(forumpath + "aspx/" + templateid));
            }
            if (!File.Exists(Utils.GetMapPath(forumpath + "aspx/" + templateid + "/" + pagename)))   //当前模板文件不存在
            {
                ForumPageTemplate forumpagetemplate = new ForumPageTemplate();
                forumpagetemplate.GetTemplate(forumpath, templatepath, pagename.Split('.')[0], 1, templateid);
            }
        }
    }



    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 站点伪Url信息类
    /// </summary>
    public class SiteUrls
    {
        #region 内部属性和方法
        private static object lockHelper = new object();
        private static volatile SiteUrls instance = null;

        string SiteUrlsFile = AppDomain.CurrentDomain.BaseDirectory + "/config/urls.config";
        private System.Collections.ArrayList _Urls;
        public System.Collections.ArrayList Urls
        {
            get
            {
                return _Urls;
            }
            set
            {
                _Urls = value;
            }
        }

        private System.Collections.Specialized.NameValueCollection _Paths;
        public System.Collections.Specialized.NameValueCollection Paths
        {
            get
            {
                return _Paths;
            }
            set
            {
                _Paths = value;
            }
        }

        private SiteUrls()
        {
            Urls = new System.Collections.ArrayList();
            Paths = new System.Collections.Specialized.NameValueCollection();

            XmlDocument xml = new XmlDocument();

            xml.Load(SiteUrlsFile);

            XmlNode root = xml.SelectSingleNode("urls");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "rewrite")
                {
                    XmlAttribute name = n.Attributes["name"];
                    XmlAttribute path = n.Attributes["path"];
                    XmlAttribute page = n.Attributes["page"];
                    XmlAttribute querystring = n.Attributes["querystring"];
                    XmlAttribute pattern = n.Attributes["pattern"];

                    if (name != null && path != null && page != null && querystring != null && pattern != null)
                    {
                        Paths.Add(name.Value, path.Value);
                        Urls.Add(new URLRewrite(name.Value, pattern.Value, page.Value.Replace("^", "&"), querystring.Value.Replace("^", "&")));
                    }
                }
            }
        }
        #endregion

        public static SiteUrls GetSiteUrls()
        {
            if (instance == null)
            {
                lock (lockHelper)
                {
                    if (instance == null)
                    {
                        instance = new SiteUrls();
                    }
                }
            }
            return instance;

        }

        public static void SetInstance(SiteUrls anInstance)
        {
            if (anInstance != null)
                instance = anInstance;
        }

        public static void SetInstance()
        {
            SetInstance(new SiteUrls());
        }


        /// <summary>
        /// 重写伪地址
        /// </summary>
        public class URLRewrite
        {
            #region 成员变量
            private string _Name;
            public string Name
            {
                get
                {
                    return _Name;
                }
                set
                {
                    _Name = value;
                }
            }

            private string _Pattern;
            public string Pattern
            {
                get
                {
                    return _Pattern;
                }
                set
                {
                    _Pattern = value;
                }
            }

            private string _Page;
            public string Page
            {
                get
                {
                    return _Page;
                }
                set
                {
                    _Page = value;
                }
            }

            private string _QueryString;
            public string QueryString
            {
                get
                {
                    return _QueryString;
                }
                set
                {
                    _QueryString = value;
                }
            }
            #endregion

            #region 构造函数
            public URLRewrite(string name, string pattern, string page, string querystring)
            {
                _Name = name;
                _Pattern = pattern;
                _Page = page;
                _QueryString = querystring;
            }
            #endregion
        }

    }







}
