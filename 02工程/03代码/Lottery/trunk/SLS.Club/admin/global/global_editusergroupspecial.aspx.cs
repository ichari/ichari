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
    /// 编辑特殊用户组
    /// </summary>
    
#if NET1
    public class editusergroupspecial : AdminPage
#else
    public partial class editusergroupspecial : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.TextBox groupTitle;
        protected Discuz.Control.DropDownList radminid;
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

            stars.Text = __usergroupinfo.Stars.ToString();
            color.Text = __usergroupinfo.Color;
            groupavatar.Text = __usergroupinfo.Groupavatar;
            readaccess.Text = __usergroupinfo.Readaccess.ToString();
            maxprice.Text = __usergroupinfo.Maxprice.ToString();
            maxpmnum.Text = __usergroupinfo.Maxpmnum.ToString();
            maxsigsize.Text = __usergroupinfo.Maxsigsize.ToString();
            maxattachsize.Text = __usergroupinfo.Maxattachsize.ToString();
            maxsizeperday.Text = __usergroupinfo.Maxsizeperday.ToString();

            radminid.SelectedValue = __usergroupinfo.Radminid == -1 ? "0" : __usergroupinfo.Radminid.ToString();
            ViewState["radminid"] = __usergroupinfo.Radminid;

            //DataTable dt = DbHelper.ExecuteDataset("Select id,extension  From [" + BaseConfigs.GetTablePrefix + "attachtypes]  Order By [id] ASC").Tables[0];
            DataTable dt = DatabaseProvider.GetInstance().GetAttchType().Tables[0];
            attachextensions.SetSelectByID(__usergroupinfo.Attachextensions.Trim());

            //设置用户权限组初始化信息
            //if (__usergroupinfo.Allowvisit == 1) usergroupright.Items[0].Selected = true;
            //if (__usergroupinfo.Allowpost == 1) usergroupright.Items[1].Selected = true;
            //if (__usergroupinfo.Allowreply == 1) usergroupright.Items[2].Selected = true;
            //if (__usergroupinfo.Allowpostpoll == 1) usergroupright.Items[3].Selected = true;
            //if (__usergroupinfo.Allowgetattach == 1) usergroupright.Items[4].Selected = true;
            //if (__usergroupinfo.Allowpostattach == 1) usergroupright.Items[5].Selected = true;
            //if (__usergroupinfo.Allowvote == 1) usergroupright.Items[6].Selected = true;
            //if (__usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true;
            //if (__usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true;
            //if (__usergroupinfo.Allowhidecode == 1) usergroupright.Items[9].Selected = true;
            //if (__usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[10].Selected = true;
            //if (__usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[11].Selected = true;
            //if (__usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[12].Selected = true;
            //if (__usergroupinfo.Allowviewpro == 1) usergroupright.Items[13].Selected = true;
            //if (__usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[14].Selected = true;

            //if (__usergroupinfo.Allowsearch.ToString() == "0") allowsearch.Items[0].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "1") allowsearch.Items[1].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "2") allowsearch.Items[2].Selected = true;

            //if (__usergroupinfo.Allowavatar >= 0) allowavatar.Items[__usergroupinfo.Allowavatar].Selected = true;

            usergrouppowersetting.Bind(__usergroupinfo);
            if (__usergroupinfo.System == 1) DeleteUserGroupInf.Enabled = false;

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 删除用户组
            if (this.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));

                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergroupspecialgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript( "","<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript( "","<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
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
            #region 更新特殊用户组信息

            if (this.CheckCookie())
            {
                __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                __usergroupinfo.System = 0;
                __usergroupinfo.Type = 0;
                __usergroupinfo.Readaccess = Convert.ToInt32(readaccess.Text);

                int selectradminid = radminid.SelectedValue == "0" ? -1 : Convert.ToInt32(radminid.SelectedValue);
                __usergroupinfo.Radminid = selectradminid;

                if (selectradminid.ToString() != ViewState["radminid"].ToString())
                {
                    //DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=" + __usergroupinfo.Radminid + " WHERE [groupid]=" + __usergroupinfo.Groupid);
                    DatabaseProvider.GetInstance().ChangeUserAdminidByGroupid(__usergroupinfo.Radminid, __usergroupinfo.Groupid);
                }

                //__usergroupinfo.Allowvisit = BoolToInt(usergroupright.Items[0].Selected);
                //__usergroupinfo.Allowpost = BoolToInt(usergroupright.Items[1].Selected);
                //__usergroupinfo.Allowreply = BoolToInt(usergroupright.Items[2].Selected);
                //__usergroupinfo.Allowpostpoll = BoolToInt(usergroupright.Items[3].Selected);
                //__usergroupinfo.Allowdirectpost = 1;
                //__usergroupinfo.Allowgetattach = BoolToInt(usergroupright.Items[4].Selected);
                //__usergroupinfo.Allowpostattach = BoolToInt(usergroupright.Items[5].Selected);
                //__usergroupinfo.Allowvote = BoolToInt(usergroupright.Items[6].Selected);
                //__usergroupinfo.Allowmultigroups = 0;
                //__usergroupinfo.Allowsearch = Convert.ToInt32(allowsearch.SelectedValue);
                //__usergroupinfo.Allowcstatus = 0;
                //__usergroupinfo.Allowuseblog = 0;
                //__usergroupinfo.Allowinvisible = 0;
                //__usergroupinfo.Allowtransfer = 0;
                //__usergroupinfo.Allowsetreadperm = BoolToInt(usergroupright.Items[7].Selected);
                //__usergroupinfo.Allowsetattachperm = BoolToInt(usergroupright.Items[8].Selected);
                //__usergroupinfo.Allowhidecode = BoolToInt(usergroupright.Items[9].Selected);
                //__usergroupinfo.Allowhtml = 0;
                //__usergroupinfo.Allowcusbbcode = BoolToInt(usergroupright.Items[10].Selected);
                //__usergroupinfo.Allownickname = 0;
                //__usergroupinfo.Allowsigbbcode = BoolToInt(usergroupright.Items[11].Selected);
                //__usergroupinfo.Allowsigimgcode = BoolToInt(usergroupright.Items[12].Selected);
                //__usergroupinfo.Allowviewpro = BoolToInt(usergroupright.Items[13].Selected);
                //__usergroupinfo.Allowviewstats = 0;
                //__usergroupinfo.Disableperiodctrl = BoolToInt(usergroupright.Items[14].Selected);
                //__usergroupinfo.Reasonpm = 0;

                //__usergroupinfo.Allowavatar = Convert.ToInt16(allowavatar.SelectedValue);
                __usergroupinfo.Grouptitle = groupTitle.Text;

                __usergroupinfo.Stars = Convert.ToInt32(stars.Text);
                __usergroupinfo.Color = color.Text;
                __usergroupinfo.Groupavatar = groupavatar.Text;
                __usergroupinfo.Maxprice = Convert.ToInt32(maxprice.Text);
                __usergroupinfo.Maxpmnum = Convert.ToInt32(maxpmnum.Text);
                __usergroupinfo.Maxsigsize = Convert.ToInt32(maxsigsize.Text);
                __usergroupinfo.Maxattachsize = Convert.ToInt32(maxattachsize.Text);
                __usergroupinfo.Maxsizeperday = Convert.ToInt32(maxsizeperday.Text);
                __usergroupinfo.Attachextensions = attachextensions.GetSelectString(",");

                //GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                usergrouppowersetting.GetSetting(ref __usergroupinfo);
                if (AdminUserGroups.UpdateUserGroupInfo(__usergroupinfo))
                {
                    //GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除特殊用户组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript( "PAGE", "window.location.href='global_usergroupspecialgrid.aspx';");
                }
                else
                {
                    if (AdminUserGroups.opresult != "")
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败,原因:" + AdminUserGroups.opresult + "');window.location.href='global_usergroupspecialgrid.aspx';</script>");
                    }
                    else
                    {
                        base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_usergroupspecialgrid.aspx';</script>");
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

            
            radminid.AddTableData(DatabaseProvider.GetInstance().AddTableData());
            DataTable dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);
            if (DNTRequest.GetString("groupid") != "")
            {
                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("global_sysadminusergroupgrid.aspx");
            }
        }

        #endregion
    }
}