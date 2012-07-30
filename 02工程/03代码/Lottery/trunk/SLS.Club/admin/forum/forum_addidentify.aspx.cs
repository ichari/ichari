using System;
using System.Web.UI;


using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Cache;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加鉴定
    /// </summary>
    
#if NET1
    public class addidentify : AdminPage
#else
    public partial class addidentify : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox name;
        protected Discuz.Control.UpFile uploadfile;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button AddIdentifyInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                uploadfile.UpFilePath = Server.MapPath("../../images/identify/");
            }
        }

        private void AddIdentifyInfo_Click(object sender, EventArgs e)
        {
            #region 添加鉴定记录

            if (this.CheckCookie())
            {
                string fileName = uploadfile.UpdateFile();
                if (fileName.Trim() == "")
                {
                    base.RegisterStartupScript("PAGE", "alert('没有选择鉴定图片');window.location.href='forum_identifymanage.aspx';");
                }
                if (DatabaseProvider.GetInstance().AddIdentify(name.Text, fileName))
                {
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicIdentifys");
                    DNTCache.GetCacheService().RemoveObject("/Forum/TopicIndentifysJsArray");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "鉴定文件添加", name.Text);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_identifymanage.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("PAGE", "alert('插入失败，可能名称与原有的相同');window.location.href='forum_identifymanage.aspx';");
                }
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddIdentifyInfo.Click += new EventHandler(this.AddIdentifyInfo_Click);
        }

        #endregion


    }
}