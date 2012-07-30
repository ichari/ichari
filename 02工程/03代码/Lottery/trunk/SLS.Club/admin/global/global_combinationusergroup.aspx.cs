using System;
using System.Web.UI;


using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 合并用户组
    /// </summary>
    
#if NET1
    public class combinationusergroup : AdminPage
#else
    public partial class combinationusergroup : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DropDownList sourceusergroup;
        protected Discuz.Control.DropDownList targetusergroup;
        protected Discuz.Control.Button ComUsergroup;
        protected Discuz.Control.DropDownList sourceadminusergroup;
        protected Discuz.Control.DropDownList targetadminusergroup;
        protected Discuz.Control.Button ComAdminUsergroup;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                sourceusergroup.AddTableData(DatabaseProvider.GetInstance().GetUserGroupTitle());
                targetusergroup.AddTableData(DatabaseProvider.GetInstance().GetUserGroupTitle());
                sourceadminusergroup.AddTableData(DatabaseProvider.GetInstance().GetAdminUserGroupTitle());
                targetadminusergroup.AddTableData(DatabaseProvider.GetInstance().GetAdminUserGroupTitle());
            }
        }

        private void ComUsergroup_Click(object sender, EventArgs e)
        {
            #region 合并用户组

            if (this.CheckCookie())
            {
                if ((sourceusergroup.SelectedIndex == 0) || (targetusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,请您选择有效的用户组!');</script>");
                    return;
                }

                if (sourceusergroup.SelectedValue == targetusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,同一个用户组不能够合并!');</script>");
                    return;
                }
                int sourceusergroupcreditslower = UserGroups.GetUserGroupInfo(int.Parse(sourceusergroup.SelectedValue)).Creditslower;
                int targetusergroupcreditshigher = UserGroups.GetUserGroupInfo(int.Parse(targetusergroup.SelectedValue)).Creditshigher;
                if (sourceusergroupcreditslower != targetusergroupcreditshigher)
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败,要合并的用户组必须是金币相连的两个用户组!');</script>");
                    return;
                }

                //合并用户金币上下限
                DatabaseProvider.GetInstance().CombinationUsergroupScore(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));
                //删除被合并的源用户组
                DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceusergroup.SelectedValue));

                //更新用户组中的信息
                DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceusergroup.SelectedValue), int.Parse(targetusergroup.SelectedValue));

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户组", "把组ID:" + sourceusergroup.SelectedIndex + " 合并到组ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
            }

            #endregion
        }

        private void ComAdminUsergroup_Click(object sender, EventArgs e)
        {
            #region 合并管理组

            if (this.CheckCookie())
            {
                if ((sourceadminusergroup.SelectedIndex == 0) || (targetadminusergroup.SelectedIndex == 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,请您选择有效的管理组!');</script>");
                    return;
                }

                if ((Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3) || (Convert.ToInt32(sourceadminusergroup.SelectedValue) <= 3))
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,您选择的组为系统初始化的管理组,这些组不允许合并!');</script>");
                    return;
                }

                if (sourceadminusergroup.SelectedValue == targetadminusergroup.SelectedValue)
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败,同一个管理组不能够合并!');</script>");
                    return;
                }

                //删除被合并的源用户组
                DatabaseProvider.GetInstance().DeleteAdminGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));

                //删除被合并的源用户组
                DatabaseProvider.GetInstance().DeleteUserGroupInfo(int.Parse(sourceadminusergroup.SelectedValue));
             
                //更新用户组中的信息
                DatabaseProvider.GetInstance().UpdateAdminUsergroup(targetadminusergroup.SelectedValue.ToString(), sourceadminusergroup.SelectedValue.ToString());
                DatabaseProvider.GetInstance().ChangeUsergroup(int.Parse(sourceadminusergroup.SelectedValue), int.Parse(targetadminusergroup.SelectedValue));

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并管理组", "把组ID:" + sourceusergroup.SelectedIndex + " 合并到组ID:" + targetusergroup.SelectedIndex);
                base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
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
            this.ComUsergroup.Click += new EventHandler(this.ComUsergroup_Click);
            this.ComAdminUsergroup.Click += new EventHandler(this.ComAdminUsergroup_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion


    }
}