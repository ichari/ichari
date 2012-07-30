using System;
using System.Data;
using System.Web.UI;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;

using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 编辑用户组
    /// </summary>
    
#if NET1
    public class editusergroup : AdminPage
#else
    public partial class editusergroup : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.TextBox groupTitle;
        protected Discuz.Control.TextBox creditshigher;
        protected Discuz.Control.TextBox creditslower;
        protected Discuz.Control.ColorPicker color;
        protected Discuz.Control.TextBox stars;
        protected Discuz.Control.TextBox readaccess;
        protected Discuz.Control.TextBox maxprice;
        protected Discuz.Control.TextBox maxpmnum;
        protected Discuz.Control.TextBox maxsigsize;
        protected Discuz.Control.TextBox maxattachsize;
        protected Discuz.Control.TextBox maxsizeperday;
        protected Discuz.Control.TextBox maxspaceattachsize;
        protected Discuz.Control.TextBox maxspacephotosize;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.TabPage tabPage22;
        protected Discuz.Control.RadioButtonList allowavatar;
        protected Discuz.Control.RadioButtonList allowsearch;
        protected Discuz.Control.CheckBoxList usergroupright;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button UpdateUserGroupInf;
        protected Discuz.Control.Button DeleteUserGroupInf;
        protected Discuz.Control.TextBox groupavatar;
        #endregion
#endif

        public UserGroupInfo __usergroupinfo = new UserGroupInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void LoadUserGroupInf(int groupid)
        {
            #region 加载相关组信息

            __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(groupid);

            groupTitle.Text = Utils.RemoveFontTag(__usergroupinfo.Grouptitle);
            creditshigher.Text = __usergroupinfo.Creditshigher.ToString();
            creditslower.Text = __usergroupinfo.Creditslower.ToString();

            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupExceptGroupid(groupid);
            if (dt.Rows.Count == 0)
            {
                creditshigher.Enabled = false;
                creditslower.Enabled = false;
            }

            ViewState["creditshigher"] = __usergroupinfo.Creditshigher.ToString();
            ViewState["creditslower"] = __usergroupinfo.Creditslower.ToString();

            stars.Text = __usergroupinfo.Stars.ToString();
            color.Text = __usergroupinfo.Color;
            groupavatar.Text = __usergroupinfo.Groupavatar;
            readaccess.Text = __usergroupinfo.Readaccess.ToString();
            maxprice.Text = __usergroupinfo.Maxprice.ToString();
            maxpmnum.Text = __usergroupinfo.Maxpmnum.ToString();
            maxsigsize.Text = __usergroupinfo.Maxsigsize.ToString();
            maxattachsize.Text = __usergroupinfo.Maxattachsize.ToString();
            maxsizeperday.Text = __usergroupinfo.Maxsizeperday.ToString();

            dt = DatabaseProvider.GetInstance().GetAttchType().Tables[0];
            attachextensions.SetSelectByID(__usergroupinfo.Attachextensions.Trim());
            //绑定权限信息
            usergrouppowersetting.Bind(__usergroupinfo);

            if (__usergroupinfo.System == 1) DeleteUserGroupInf.Enabled = false;

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 删除相关组信息
            if (this.CheckCookie())
            {
                int groupid = DNTRequest.GetInt("groupid", -1);
                if (AdminUserGroups.DeleteUserGroupInfo(groupid))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_usergroupgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                }
            }
            #endregion
        }

        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }


        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 更新用户组信息

            if (this.CheckCookie())
            {
                if (creditshigher.Enabled == true)
                {
                    if (Convert.ToInt32(creditshigher.Text) < Convert.ToInt32(ViewState["creditshigher"].ToString()) || Convert.ToInt32(creditslower.Text) > Convert.ToInt32(ViewState["creditslower"].ToString()))
                    {
                        base.RegisterStartupScript( "","<script>alert('操作失败, 您所输入的金币上下限范围应在" + ViewState["creditshigher"].ToString() + "至" + ViewState["creditslower"].ToString() + "之间');</script>");
                        return;
                    }
                }

                __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                __usergroupinfo.System = 0;
                __usergroupinfo.Type = 0;
                __usergroupinfo.Readaccess = Convert.ToInt32(readaccess.Text);
                usergrouppowersetting.GetSetting(ref __usergroupinfo);
                __usergroupinfo.Grouptitle = groupTitle.Text;

                __usergroupinfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                __usergroupinfo.Creditslower = Convert.ToInt32(creditslower.Text);

                if (__usergroupinfo.Creditshigher >= __usergroupinfo.Creditslower)
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败, 金币下限必须小于金币上限');</script>");
                    return;
                }
                if (__usergroupinfo.Allowbonus == 1 && (__usergroupinfo.Minbonusprice >= __usergroupinfo.Maxbonusprice))
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败, 最低悬赏价格必须小于最高悬赏价格');</script>");
                    return;
                }

                __usergroupinfo.Stars = Convert.ToInt32(stars.Text);
                __usergroupinfo.Color = color.Text;
                __usergroupinfo.Groupavatar = groupavatar.Text;
                __usergroupinfo.Maxprice = Convert.ToInt32(maxprice.Text);
                __usergroupinfo.Maxpmnum = Convert.ToInt32(maxpmnum.Text);
                __usergroupinfo.Maxsigsize = Convert.ToInt32(maxsigsize.Text);
                __usergroupinfo.Maxattachsize = Convert.ToInt32(maxattachsize.Text);
                __usergroupinfo.Maxsizeperday = Convert.ToInt32(maxsizeperday.Text);
                __usergroupinfo.Attachextensions = attachextensions.GetSelectString(",");

                if (AdminUserGroups.UpdateUserGroupInfo(__usergroupinfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript( "PAGE", "window.location.href='global_usergroupgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_usergroupgrid.aspx';</script>");
                    }

                }
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
            this.TabControl1.InitTabPage();
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);

            this.Load += new EventHandler(this.Page_Load);
          
            DataTable dt = DatabaseProvider.GetInstance().GetAttachTypes();
           
            attachextensions.AddTableData(dt);

            if (DNTRequest.GetString("groupid") != "")
            {
                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("global_usergroupgrid.aspx");
            }
        }
        #endregion
    }
}