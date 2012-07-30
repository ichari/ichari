using System;
using System.Data;
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
    /// 编辑系统管理组
    /// </summary>
    
#if NET1
    public class editsysadminusergroup : AdminPage
#else
    public partial class editsysadminusergroup : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.DropDownList radminid;
        protected Discuz.Control.TextBox groupTitle;
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
        protected Discuz.Control.TabPage tabPage33;
        protected Discuz.Control.CheckBoxList admingroupright;
        protected Discuz.Control.DropDownList allowstickthread;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button UpdateUserGroupInf;
        protected Discuz.Control.TextBox creditshigher;
        protected Discuz.Control.TextBox creditslower;
        protected Discuz.Control.TextBox groupavatar;
        #endregion
#endif


        public AdminGroupInfo __admingroupinfo = new AdminGroupInfo();
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
            stars.Text = __usergroupinfo.Stars.ToString();
            color.Text = __usergroupinfo.Color;
            groupavatar.Text = __usergroupinfo.Groupavatar;
            readaccess.Text = __usergroupinfo.Readaccess.ToString();
            maxprice.Text = __usergroupinfo.Maxprice.ToString();
            maxpmnum.Text = __usergroupinfo.Maxpmnum.ToString();
            maxsigsize.Text = __usergroupinfo.Maxsigsize.ToString();
            maxattachsize.Text = __usergroupinfo.Maxattachsize.ToString();
            maxsizeperday.Text = __usergroupinfo.Maxsizeperday.ToString();

            attachextensions.SetSelectByID(__usergroupinfo.Attachextensions.Trim());

            if (groupid > 0 && groupid <= 3) radminid.Enabled = false;
            radminid.SelectedValue = __usergroupinfo.Radminid.ToString();

            //if (__usergroupinfo.Allowvisit == 1) usergroupright.Items[0].Selected = true;
            //if (__usergroupinfo.Allowpost == 1) usergroupright.Items[1].Selected = true;
            //if (__usergroupinfo.Allowreply == 1) usergroupright.Items[2].Selected = true;
            //if (__usergroupinfo.Allowpostpoll == 1) usergroupright.Items[3].Selected = true;
            //if (__usergroupinfo.Allowgetattach == 1) usergroupright.Items[4].Selected = true;
            //if (__usergroupinfo.Allowpostattach == 1) usergroupright.Items[5].Selected = true;
            //if (__usergroupinfo.Allowvote == 1) usergroupright.Items[6].Selected = true;

            //if (groupid != 7) //不是游客时
            //{
            //    //设置用户权限组初始化信息
            //    if (__usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true;
            //    if (__usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true;
            //    if (__usergroupinfo.Allowhidecode == 1) usergroupright.Items[9].Selected = true;
            //    if (__usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[10].Selected = true;
            //    if (__usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[11].Selected = true;
            //    if (__usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[12].Selected = true;
            //    if (__usergroupinfo.Allowviewpro == 1) usergroupright.Items[13].Selected = true;
            //    if (__usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[14].Selected = true;
            //}
            //else
            //{
            //    //设置用户权限组初始化信息
            //    if (__usergroupinfo.Allowsetreadperm == 1) usergroupright.Items[7].Selected = true;
            //    if (__usergroupinfo.Allowsetattachperm == 1) usergroupright.Items[8].Selected = true;
            //    if (__usergroupinfo.Allowcusbbcode == 1) usergroupright.Items[9].Selected = true;
            //    if (__usergroupinfo.Allowsigbbcode == 1) usergroupright.Items[10].Selected = true;
            //    if (__usergroupinfo.Allowsigimgcode == 1) usergroupright.Items[11].Selected = true;
            //    if (__usergroupinfo.Allowviewpro == 1) usergroupright.Items[12].Selected = true;
            //    if (__usergroupinfo.Disableperiodctrl == 1) usergroupright.Items[13].Selected = true;

            //}

            //if (__usergroupinfo.Allowsearch.ToString() == "0") allowsearch.Items[0].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "1") allowsearch.Items[1].Selected = true;
            //if (__usergroupinfo.Allowsearch.ToString() == "2") allowsearch.Items[2].Selected = true;

            //if (__usergroupinfo.Allowavatar >= 0) allowavatar.Items[__usergroupinfo.Allowavatar].Selected = true;

            __admingroupinfo = AdminUserGroups.AdminGetAdminGroupInfo(__usergroupinfo.Groupid);
            usergrouppowersetting.Bind(__usergroupinfo);
            if (__admingroupinfo != null)
            {
                if (groupid != 7)
                {
                    //设置管理权限组初始化信息
                    admingroupright.SelectedIndex = -1;

                    if (__admingroupinfo.Alloweditpost.ToString() == "1") admingroupright.Items[0].Selected = true;
                    if (__admingroupinfo.Alloweditpoll.ToString() == "1") admingroupright.Items[1].Selected = true;
                    if (__admingroupinfo.Allowstickthread.ToString() != "") allowstickthread.SelectedValue = __admingroupinfo.Allowstickthread.ToString();
                    if (__admingroupinfo.Allowdelpost.ToString() == "1") admingroupright.Items[2].Selected = true;
                    if (__admingroupinfo.Allowmassprune.ToString() == "1") admingroupright.Items[3].Selected = true;
                    if (__admingroupinfo.Allowviewip.ToString() == "1") admingroupright.Items[4].Selected = true;
                    if (__admingroupinfo.Allowedituser.ToString() == "1") admingroupright.Items[5].Selected = true;
                    if (__admingroupinfo.Allowviewlog.ToString() == "1") admingroupright.Items[6].Selected = true;
                    if (__admingroupinfo.Disablepostctrl.ToString() == "1") admingroupright.Items[7].Selected = true;
                }
            }


            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }

            #endregion
        }


        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }


        public byte BoolToByte(bool a)
        {
            if (a) return 1;
            else return 0;
        }


        private void UpdateUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 更新系统管理组信息

            if (this.CheckCookie())
            {
                __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                __usergroupinfo.System = 0;
                __usergroupinfo.Type = 0;
                __usergroupinfo.Readaccess = Convert.ToInt32(readaccess.Text);

                //__usergroupinfo.Allowvisit = BoolToInt(usergroupright.Items[0].Selected);
                //__usergroupinfo.Allowpost = BoolToInt(usergroupright.Items[1].Selected);
                //__usergroupinfo.Allowreply = BoolToInt(usergroupright.Items[2].Selected);
                //__usergroupinfo.Allowpostpoll = BoolToInt(usergroupright.Items[3].Selected);
                //__usergroupinfo.Allowdirectpost = 1; //BoolToInt(usergroupright.Items[4].Selected);
                //__usergroupinfo.Allowgetattach = BoolToInt(usergroupright.Items[4].Selected);
                //__usergroupinfo.Allowpostattach = BoolToInt(usergroupright.Items[5].Selected);
                //__usergroupinfo.Allowvote = BoolToInt(usergroupright.Items[6].Selected);
                //__usergroupinfo.Allowsetreadperm = BoolToInt(usergroupright.Items[7].Selected);
                //__usergroupinfo.Allowsetattachperm = BoolToInt(usergroupright.Items[8].Selected);

                //if (__usergroupinfo.Groupid != 7)
                //{
                //    __usergroupinfo.Allowhidecode = BoolToInt(usergroupright.Items[9].Selected);
                //    __usergroupinfo.Allowcusbbcode = BoolToInt(usergroupright.Items[10].Selected);
                //    __usergroupinfo.Allowsigbbcode = BoolToInt(usergroupright.Items[11].Selected);
                //    __usergroupinfo.Allowsigimgcode = BoolToInt(usergroupright.Items[12].Selected);
                //    __usergroupinfo.Allowviewpro = BoolToInt(usergroupright.Items[13].Selected);
                //    __usergroupinfo.Disableperiodctrl = BoolToInt(usergroupright.Items[14].Selected);
                //}
                //else
                //{
                //    __usergroupinfo.Allowhidecode = 0;
                //    __usergroupinfo.Allowcusbbcode = BoolToInt(usergroupright.Items[9].Selected);
                //    __usergroupinfo.Allowsigbbcode = BoolToInt(usergroupright.Items[10].Selected);
                //    __usergroupinfo.Allowsigimgcode = BoolToInt(usergroupright.Items[11].Selected);
                //    __usergroupinfo.Allowviewpro = BoolToInt(usergroupright.Items[12].Selected);
                //    __usergroupinfo.Disableperiodctrl = BoolToInt(usergroupright.Items[13].Selected);
                //}
                //__usergroupinfo.Allowsearch = Convert.ToInt32(allowsearch.SelectedValue);


                __usergroupinfo.Allowviewstats = 0;
                __usergroupinfo.Allownickname = 0;
                __usergroupinfo.Allowhtml = 0;
                __usergroupinfo.Allowcstatus = 0;
                __usergroupinfo.Allowuseblog = 0;
                __usergroupinfo.Allowinvisible = 0;
                __usergroupinfo.Allowtransfer = 0;
                __usergroupinfo.Allowmultigroups = 0;
                __usergroupinfo.Reasonpm = 0;

                //__usergroupinfo.Allowavatar = Convert.ToInt16(allowavatar.SelectedValue);

                if (radminid.SelectedValue == "0") //当未选取任何管理模板时
                {
                    AdminGroups.DeleteAdminGroupInfo((short)__usergroupinfo.Groupid);
                    __usergroupinfo.Radminid = 0;
                }
                else //当选取相应的管理模板时
                {
                    int selectradminid = Convert.ToInt32(radminid.SelectedValue);
                    ///对于当前用户组中,有管理权限的,则设置管理权限
                    if (selectradminid > 0 && selectradminid <= 3)
                    {
                        __admingroupinfo = new AdminGroupInfo();
                        __admingroupinfo.Admingid = (short)__usergroupinfo.Groupid;
                        //插入相应的管理组
                        __admingroupinfo.Alloweditpost = BoolToByte(admingroupright.Items[0].Selected);
                        __admingroupinfo.Alloweditpoll = BoolToByte(admingroupright.Items[1].Selected);
                        __admingroupinfo.Allowstickthread = (byte)Convert.ToInt16(allowstickthread.SelectedValue);
                        __admingroupinfo.Allowmodpost = 0; ;
                        __admingroupinfo.Allowdelpost = BoolToByte(admingroupright.Items[2].Selected);
                        __admingroupinfo.Allowmassprune = BoolToByte(admingroupright.Items[3].Selected);
                        __admingroupinfo.Allowrefund = 0;
                        __admingroupinfo.Allowcensorword = 0;
                        __admingroupinfo.Allowviewip = BoolToByte(admingroupright.Items[4].Selected);
                        __admingroupinfo.Allowbanip = 0;
                        __admingroupinfo.Allowedituser = BoolToByte(admingroupright.Items[5].Selected);
                        __admingroupinfo.Allowmoduser = 0;
                        __admingroupinfo.Allowbanuser = 0;
                        __admingroupinfo.Allowpostannounce = 0;
                        __admingroupinfo.Allowviewlog = BoolToByte(admingroupright.Items[6].Selected);
                        __admingroupinfo.Disablepostctrl = BoolToByte(admingroupright.Items[7].Selected);

                        //当已有记录时
                        //if (DbHelper.ExecuteDataset("SELECT [admingid]  FROM [" + BaseConfigs.GetTablePrefix + "admingroups] WHERE [admingid]=" + __usergroupinfo.Groupid.ToString()).Tables[0].Rows.Count > 0)
                        if (DatabaseProvider.GetInstance().GetAdmingid(__usergroupinfo.Groupid).Rows.Count > 0)
                        {
                            //更新相应的管理组
                            AdminGroups.SetAdminGroupInfo(__admingroupinfo);
                        }
                        else
                        { //建立相应的用户组
                            AdminGroups.CreateAdminGroupInfo(__admingroupinfo);
                        }
                        __usergroupinfo.Radminid = selectradminid;
                    }
                    else
                    {
                        __usergroupinfo.Radminid = 0;
                    }
                }

                //DbHelper.ExecuteNonQuery("UPDATE [" + BaseConfigs.GetTablePrefix + "users] SET [adminid]=" + __usergroupinfo.Radminid + " WHERE [groupid]=" + __usergroupinfo.Groupid);
                DatabaseProvider.GetInstance().ChangeUserAdminidByGroupid(__usergroupinfo.Radminid, __usergroupinfo.Groupid);
                __usergroupinfo.Grouptitle = groupTitle.Text;
                __usergroupinfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                __usergroupinfo.Creditslower = Convert.ToInt32(creditslower.Text);
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
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新系统组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_sysadminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_sysadminusergroupgrid.aspx';</script>");
                }
            }

            #endregion
        }


        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 绑定关联用户组信息
            DataTable usergrouprightstable = DatabaseProvider.GetInstance().GetUserGroupInfoByGroupid(int.Parse(radminid.SelectedValue));
            if (usergrouprightstable.Rows.Count > 0)
            {
                //设置管理组初始化信息
                DataRow usergrouprights = usergrouprightstable.Rows[0];
                creditshigher.Text = usergrouprights["creditshigher"].ToString();
                creditslower.Text = usergrouprights["creditslower"].ToString();
                stars.Text = usergrouprights["stars"].ToString();
                color.Text = usergrouprights["color"].ToString();
                groupavatar.Text = usergrouprights["groupavatar"].ToString();
                readaccess.Text = usergrouprights["readaccess"].ToString();
                maxprice.Text = usergrouprights["maxprice"].ToString();
                maxpmnum.Text = usergrouprights["maxpmnum"].ToString();
                maxsigsize.Text = usergrouprights["maxsigsize"].ToString();
                maxattachsize.Text = usergrouprights["maxattachsize"].ToString();
                maxsizeperday.Text = usergrouprights["maxsizeperday"].ToString();

                DataTable dt = DatabaseProvider.GetInstance().GetAttchType().Tables[0];
                attachextensions.AddTableData(dt, usergrouprights["attachextensions"].ToString().Trim());
            }

            DataTable admingrouprights = DatabaseProvider.GetInstance().GetAdmingroupByAdmingid(int.Parse(radminid.SelectedValue));

            if (admingrouprights.Rows.Count > 0)
            {
                //设置管理权限组初始化信息
                DataRow dr = admingrouprights.Rows[0];
                admingroupright.SelectedIndex = -1;
                if (dr["alloweditpost"].ToString() == "1") admingroupright.Items[0].Selected = true;
                if (dr["alloweditpoll"].ToString() == "1") admingroupright.Items[1].Selected = true;
                if (dr["allowdelpost"].ToString() == "1") admingroupright.Items[2].Selected = true;
                if (dr["allowmassprune"].ToString() == "1") admingroupright.Items[3].Selected = true;
                if (dr["allowviewip"].ToString() == "1") admingroupright.Items[4].Selected = true;
                if (dr["allowedituser"].ToString() == "1") admingroupright.Items[5].Selected = true;
                if (dr["allowviewlog"].ToString() == "1") admingroupright.Items[6].Selected = true;
                if (dr["disablepostctrl"].ToString() == "1") admingroupright.Items[7].Selected = true;
            }

            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }
            else
            {
                allowstickthread.Enabled = true;
            }
            admingrouprights.Dispose();
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
            this.Load += new EventHandler(this.Page_Load);
            radminid.AddTableData(DatabaseProvider.GetInstance().AddTableData());
            DataTable dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);

            string groupid = DNTRequest.GetString("groupid");
            if (groupid != "")
            {
                //if (groupid == "7")
                //{
                //    usergroupright.Items.RemoveAt(10);
                //}

                LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
            }
            else
            {
                Response.Redirect("sysglobal_sysadminusergroupgrid.aspx");
            }

        }

        #endregion
    }
}