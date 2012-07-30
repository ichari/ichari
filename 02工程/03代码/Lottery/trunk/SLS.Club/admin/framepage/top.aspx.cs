using System;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Threading;
using System.Xml;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
using Discuz.Common.Xml;


namespace Discuz.Web.Admin
{
    public partial class top : AdminPage
    {
        public StringBuilder sb = new StringBuilder();

        public int menucount = 0;

        public int olid;
        public string showmenuid;
        public string toptabmenuid;
        public string mainmenulist;
        public string defaulturl;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                config = GeneralConfigs.GetConfig();
                string sysloginPage = Shove._Web.Utility.GetUrl() + "admin/syslogin.aspx";

                // 如果IP访问列表有设置则进行判断
                if (config.Adminipaccess.Trim() != "")
                {
                    string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                    {
                        Context.Response.Redirect(sysloginPage);
                        return;
                    }
                }

                #region 进行权限判断

                int userid = Discuz.Forum.Users.GetUserIDFromCookie();

                if (userid <= 0)
                {
                    Context.Response.Redirect(sysloginPage);
                    return;
                }

                UserInfo u = Discuz.Forum.Users.GetUserInfo(userid);

                if (u.Adminid < 1 || u.Groupid < 1)
                {
                    Context.Response.Redirect(sysloginPage);
                    return;
                }

                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(u.Groupid);
                if (usergroupinfo.Radminid != 1)
                {
                    Context.Response.Redirect(sysloginPage);
                    return;
                }

                this.userid = u.Uid;
                this.username = u.Username;
                this.usergroupid = u.Groupid;
                this.useradminid = (short)usergroupinfo.Radminid;
                this.grouptitle = usergroupinfo.Grouptitle;
                this.ip = DNTRequest.GetIP();

                #endregion
            }
        }
    }
}
