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
    /// 管理用户组编辑
    /// </summary>
    public partial class editadminusergroup : AdminPage
    {
        public AdminGroupInfo __admingroupinfo = new AdminGroupInfo();
        public UserGroupInfo __usergroupinfo = new UserGroupInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (DNTRequest.GetString("groupid") != "")
                {
                    LoadUserGroupInf(DNTRequest.GetInt("groupid", -1));
                }
                else
                {
                    Response.Redirect("global_adminusergroupgrid.aspx");
                    return;
                }
            }
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

            if (groupid > 0 && groupid <= 3) radminid.Enabled = false;

            radminid.SelectedValue = __usergroupinfo.Radminid.ToString();

            attachextensions.SetSelectByID(__usergroupinfo.Attachextensions.Trim());

            //设置用户权限组初始化信息
            __admingroupinfo = AdminUserGroups.AdminGetAdminGroupInfo(__usergroupinfo.Groupid);
            usergrouppowersetting.Bind(__usergroupinfo);

            if (__admingroupinfo != null)
            {
                //设置管理权限组初始化信息
                admingroupright.SelectedIndex = -1;
                admingroupright.Items[0].Selected = __admingroupinfo.Alloweditpost == 1;
                admingroupright.Items[1].Selected = __admingroupinfo.Alloweditpoll == 1;
                admingroupright.Items[2].Selected = __admingroupinfo.Allowdelpost == 1;
                admingroupright.Items[3].Selected = __admingroupinfo.Allowmassprune == 1;
                admingroupright.Items[4].Selected = __admingroupinfo.Allowviewip == 1;
                admingroupright.Items[5].Selected = __admingroupinfo.Allowedituser == 1;
                admingroupright.Items[6].Selected = __admingroupinfo.Allowviewlog == 1;
                admingroupright.Items[7].Selected = __admingroupinfo.Disablepostctrl == 1;
                admingroupright.Items[8].Selected = __admingroupinfo.Allowviewrealname == 1;
                admingroupright.Items[9].Selected = __admingroupinfo.Allowbanuser == 1;
                admingroupright.Items[10].Selected = __admingroupinfo.Allowbanip == 1;
                GeneralConfigInfo configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                admingroupright.Items[11].Selected = ("," + configinfo.Reportusergroup + ",").IndexOf("," + groupid + ",") != -1; //是否允许接收举报信息
                admingroupright.Items[12].Selected = ("," + configinfo.Photomangegroups + ",").IndexOf("," + groupid + ",") != -1;//是否允许管理图片评论
                if (__admingroupinfo.Allowstickthread.ToString() != "") allowstickthread.SelectedValue = __admingroupinfo.Allowstickthread.ToString();

            }

            if (radminid.SelectedValue == "1")
            {
                allowstickthread.Enabled = false;
                allowstickthread.SelectedValue = "3";
            }

            #endregion
        }

        private void DeleteUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 删除相关组信息

            if (this.CheckCookie())
            {
                if (AdminUserGroups.DeleteUserGroupInfo(DNTRequest.GetInt("groupid", -1)))
                {
                    //删除举报组
                    GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));                    
                    string tempstr = "";
                    foreach (string report in __configinfo.Reportusergroup.Split(','))
                    {
                        if (report != __usergroupinfo.Groupid.ToString())
                        {
                            if (tempstr == "")
                                tempstr = report;
                            else
                                tempstr += "," + report;
                        }
                    }
                    __configinfo.Reportusergroup = tempstr;
                    tempstr = "";
                    foreach (string photomangegroup in __configinfo.Photomangegroups.Split(','))
                    {
                        if (photomangegroup != __usergroupinfo.Groupid.ToString())
                        {
                            if (tempstr == "")
                                tempstr = photomangegroup;
                            else
                                tempstr += "," + photomangegroup;
                        }
                    }
                    __configinfo.Photomangegroups = tempstr;
                    GeneralConfigs.Serialiaze(__configinfo, AppDomain.CurrentDomain.BaseDirectory + "config/general.config");
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");
                    AdminGroups.GetAdminGroupList();
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除管理组", "组ID:" + DNTRequest.GetInt("groupid", -1));
                    base.RegisterStartupScript("PAGE", "window.location.href='global_adminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
                }
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
            #region 更新管理组信息

            if (this.CheckCookie())
            {
                __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(DNTRequest.GetInt("groupid", -1));
                __usergroupinfo.System = 0;
                __usergroupinfo.Type = 0;
                __usergroupinfo.Readaccess = Convert.ToInt32(readaccess.Text);

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
                        __admingroupinfo.Allowmodpost = 0;
                        __admingroupinfo.Allowdelpost = BoolToByte(admingroupright.Items[2].Selected);
                        __admingroupinfo.Allowmassprune = BoolToByte(admingroupright.Items[3].Selected);
                        __admingroupinfo.Allowrefund = 0;
                        __admingroupinfo.Allowcensorword = 0; ;
                        __admingroupinfo.Allowviewip = BoolToByte(admingroupright.Items[4].Selected);
                        __admingroupinfo.Allowbanip = 0;
                        __admingroupinfo.Allowedituser = BoolToByte(admingroupright.Items[5].Selected);
                        __admingroupinfo.Allowmoduser = 0;
                        __admingroupinfo.Allowbanuser = 0;
                        __admingroupinfo.Allowpostannounce = 0;
                        __admingroupinfo.Allowviewlog = BoolToByte(admingroupright.Items[6].Selected);
                        __admingroupinfo.Disablepostctrl = BoolToByte(admingroupright.Items[7].Selected);
                        __admingroupinfo.Allowviewrealname = BoolToByte(admingroupright.Items[8].Selected);
                        __admingroupinfo.Allowbanuser = BoolToByte(admingroupright.Items[9].Selected);
                        __admingroupinfo.Allowbanip = BoolToByte(admingroupright.Items[10].Selected);

                        //当已有记录时
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
                
                usergrouppowersetting.GetSetting(ref __usergroupinfo);


                if (AdminUserGroups.UpdateUserGroupInfo(__usergroupinfo))
                {
                    #region 是否允许接收举报信息和管理图片评论
                    GeneralConfigInfo configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                    //是否允许接收举报信息
                    int groupid = __usergroupinfo.Groupid;
                    if (admingroupright.Items[11].Selected)
                    {
                        if (("," + configinfo.Reportusergroup + ",").IndexOf("," + groupid + ",") == -1)
                        {
                            if (configinfo.Reportusergroup == "")
                            {
                                configinfo.Reportusergroup = groupid.ToString();
                            }
                            else
                            {
                                configinfo.Reportusergroup += "," + groupid.ToString();
                            }
                        }
                    }
                    else
                    {
                        string tempstr = "";
                        foreach (string report in configinfo.Reportusergroup.Split(','))
                        {
                            if (report != groupid.ToString())
                            {
                                if (tempstr == "")
                                {
                                    tempstr = report;
                                }
                                else
                                {
                                    tempstr += "," + report;
                                }
                            }
                        }
                        configinfo.Reportusergroup = tempstr;
                    }

                    GeneralConfigs.Serialiaze(configinfo, AppDomain.CurrentDomain.BaseDirectory + "config/general.config");
                    #endregion
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AdminGroupList");

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台更新管理组", "组名:" + groupTitle.Text);
                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_adminusergroupgrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('操作失败');window.location.href='global_adminusergroupgrid.aspx';</script>");
                }
            }

            #endregion
        }


        private void radminid_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region 绑定关联组
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
                if (dr["allowviewrealname"].ToString() == "1") admingroupright.Items[8].Selected = true;
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
            this.radminid.SelectedIndexChanged += new EventHandler(this.radminid_SelectedIndexChanged);
            this.UpdateUserGroupInf.Click += new EventHandler(this.UpdateUserGroupInf_Click);
            this.DeleteUserGroupInf.Click += new EventHandler(this.DeleteUserGroupInf_Click);
            //this.Load += new EventHandler(this.Page_Load);

            radminid.AddTableData(DatabaseProvider.GetInstance().AddTableData());
            DataTable dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);
        }

        #endregion
    }
}