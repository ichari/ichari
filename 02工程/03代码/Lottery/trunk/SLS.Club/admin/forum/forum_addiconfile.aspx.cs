using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;



namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加图标文件
    /// </summary>
    
#if NET1
    public class addiconfile : AdminPage
#else
    public partial class addiconfile : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.TextBox code;
        protected Discuz.Control.UpFile url;
        protected Discuz.Control.Button AddIncoInfo;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                url.UpFilePath = Server.MapPath(url.UpFilePath);
            }
        }

        private void AddIncoInfo_Click(object sender, EventArgs e)
        {
            #region 添加图标记录

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().AddSmiles(DatabaseProvider.GetInstance().GetMaxSmiliesId(),
                                                 Utils.StrToInt(displayorder.Text, 0),
                                                 1,
                                                 code.Text,
                                                 url.UpdateFile());

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/IconsList");
                Caches.GetTopicIconsCache();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "表情文件添加", code.Text);

                base.RegisterStartupScript( "PAGE", "window.location.href='forum_iconfilegrid.aspx';");
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
            this.AddIncoInfo.Click += new EventHandler(this.AddIncoInfo_Click);
        }

        #endregion

    }
}