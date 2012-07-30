using System;
using System.Web;
using System.Web.UI;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// index 的摘要说明.
    /// </summary>
    public partial class index : Page
    {
        protected internal GeneralConfigInfo config;

        public int olid;

        protected void Page_Load(object sender, EventArgs e)
        {
            config = GeneralConfigs.GetConfig();

            // 如果IP访问列表有设置则进行判断
            if (config.Adminipaccess.Trim() != "")
            {
                string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                {
                    Context.Response.Redirect("syslogin.aspx");
                    return;
                }
            }

            #region 进行权限判断

            int userid = Discuz.Forum.Users.GetUserIDFromCookie();

            if (userid <= 0)
            {
                Context.Response.Redirect("syslogin.aspx");
                return;
            }

            UserInfo u = Discuz.Forum.Users.GetUserInfo(userid);

            if (u.Adminid > 0 && u.Groupid > 0)
            {
                return;
            }
            else
            {
                Context.Response.Redirect("syslogin.aspx");
                return;
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
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}