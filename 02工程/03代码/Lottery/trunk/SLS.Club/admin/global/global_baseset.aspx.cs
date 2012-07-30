using System;
using System.Web.UI;
using System.Data;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 基本设置
    /// </summary>
    public partial class baseset : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息

            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            forumtitle.Text = __configinfo.Forumtitle.ToString();
            forumurl.Text = __configinfo.Forumurl.ToString();
            webtitle.Text = __configinfo.Webtitle.ToString();
            weburl.Text = __configinfo.Weburl.ToString();
            licensed.SelectedValue = __configinfo.Licensed.ToString();
            icp.Text = __configinfo.Icp.ToString();
            rssttl.Text = __configinfo.Rssttl.ToString();
            sitemapttl.Text = __configinfo.Sitemapttl.ToString();
            nocacheheaders.SelectedValue = __configinfo.Nocacheheaders.ToString();
            debug.SelectedValue = __configinfo.Debug.ToString();
            rssstatus.SelectedValue = __configinfo.Rssstatus.ToString();
            sitemapstatus.SelectedValue = __configinfo.Sitemapstatus.ToString();
            cachelog.SelectedValue = "0";
            fulltextsearch.SelectedValue = __configinfo.Fulltextsearch.ToString();
            passwordmode.SelectedValue = __configinfo.Passwordmode.ToString();
            bbcodemode.SelectedValue = __configinfo.Bbcodemode.ToString();
            extname.Text = __configinfo.Extname.Trim();
            enablesilverlight.SelectedValue = __configinfo.Silverlight.ToString();
            CookieDomain.Text = __configinfo.CookieDomain.ToString();
            memliststatus.SelectedValue = __configinfo.Memliststatus.ToString();
            Indexpage.SelectedIndex = Convert.ToInt32(__configinfo.Indexpage.ToString());
            Linktext.Text = __configinfo.Linktext;
            Statcode.Text = __configinfo.Statcode;
            aspxrewrite.SelectedValue = __configinfo.Aspxrewrite.ToString();

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存信息
            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                __configinfo.Forumtitle = forumtitle.Text;
                __configinfo.Forumurl = forumurl.Text;
                if ((__configinfo.Forumurl == "") || !__configinfo.Forumurl.EndsWith(".aspx"))
                {
                    base.RegisterStartupScript("", "<script>alert('论坛URL地址不能为空且必须以\".aspx结尾\"!');</script>");
                    return;
                }


                __configinfo.Webtitle = webtitle.Text;
                __configinfo.Weburl = weburl.Text;
                __configinfo.Licensed = Convert.ToInt16(licensed.SelectedValue);
                __configinfo.Icp = icp.Text;
                __configinfo.Rssttl = Convert.ToInt32(rssttl.Text);
                __configinfo.Sitemapttl = Convert.ToInt32(sitemapttl.Text);
                __configinfo.Nocacheheaders = Convert.ToInt16(nocacheheaders.SelectedValue);
                __configinfo.Debug = Convert.ToInt16(debug.SelectedValue);
                __configinfo.Rewriteurl = "";
                __configinfo.Maxmodworksmonths = 3;
                __configinfo.Rssstatus = Convert.ToInt16(rssstatus.SelectedValue);
                __configinfo.Sitemapstatus = Convert.ToInt16(sitemapstatus.SelectedValue);
                __configinfo.Cachelog = 0;
                if (fulltextsearch.SelectedValue == "1")
                {
                    __configinfo.Fulltextsearch = 1;
                    foreach (DataRow dr in Posts.GetAllPostTableName().Rows)
                    {
                        try
                        {
                            DatabaseProvider.GetInstance().TestFullTextIndex(Utils.StrToInt(dr["id"], 0));
                        }
                        catch
                        {
                            base.RegisterStartupScript("", "<script>alert('您的数据库帖子表[" + BaseConfigs.GetTablePrefix + "posts" + dr["id"] + "]中暂未进行全文索引设置,因此使用数据库全文搜索无效');</script>");
                            __configinfo.Fulltextsearch = 0;
                            break;
                        }
                    }
                }
                else
                {
                    __configinfo.Fulltextsearch = 0;
                }
                __configinfo.Passwordmode = Convert.ToInt16(passwordmode.SelectedValue);
                __configinfo.Bbcodemode = Convert.ToInt16(bbcodemode.SelectedValue);

                if (extname.Text.Trim() == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('您未输入相应的伪静态url扩展名!');</script>");
                    return;
                }

                //if (__configinfo.Extname != extname.Text.Trim())
                //{
                //    AdminForums.SetForumsPathList(true, extname.Text.Trim());
                //}

                __configinfo.Extname = extname.Text.Trim();
                __configinfo.Silverlight = Convert.ToInt32(enablesilverlight.SelectedValue);
                __configinfo.CookieDomain = CookieDomain.Text;
                __configinfo.Memliststatus = Convert.ToInt32(memliststatus.SelectedValue);
                __configinfo.Indexpage = Convert.ToInt32(Indexpage.SelectedValue);
                __configinfo.Linktext = Linktext.Text;
                __configinfo.Statcode = Statcode.Text;

                __configinfo.Aspxrewrite = Convert.ToInt16(aspxrewrite.SelectedValue);

                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                if (__configinfo.Aspxrewrite == 1)
                {
                    AdminForums.SetForumsPathList(true, __configinfo.Extname);
                }
                else 
                {
                    AdminForums.SetForumsPathList(false, __configinfo.Extname);
                }
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumList");

                TopicStats.SetQueueCount();
                AdminCaches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "基本设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_baseset.aspx';");
            }
            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);

            forumtitle.IsReplaceInvertedComma = false;
            forumurl.IsReplaceInvertedComma = false;
            webtitle.IsReplaceInvertedComma = false;
            weburl.IsReplaceInvertedComma = false;
            icp.IsReplaceInvertedComma = false;
        }

        #endregion

    }
}