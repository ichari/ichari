using System;
using System.Web;
using System.Threading;
using System.Web.UI;
using System.Xml;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;



namespace Discuz.Web.Admin
{
    public partial class managerbody : AdminPage
    {
        public int olid;

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
