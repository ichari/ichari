using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;

using Discuz.Config;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 重设Config文件
    /// </summary>
    
#if NET1
    public class restoresetupinf : AdminPage
#else
    public partial class restoresetupinf : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button RestoreInf;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GeneralConfigs.GetConfig();
            }
        }

        private void RestoreInf_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = AdminConfigs.GetConfig();
                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "恢复论坛初始化设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_restoresetupinf.aspx';");
            }
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.RestoreInf.Click += new EventHandler(this.RestoreInf_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion

    }
}