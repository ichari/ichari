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
    /// 添加特殊用户组
    /// </summary>
    
#if NET1
    public class addusergroupspecial : AdminPage
#else
    public partial class addusergroupspecial : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
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
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button AddUserGroupInf;
        protected Discuz.Control.TextBox groupavatar;
        #endregion
#endif


        public UserGroupInfo __usergroupinfo = new UserGroupInfo();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                usergrouppowersetting.Bind();
                if (DNTRequest.GetString("groupid") != "")
                {
                    SetGroupRights(DNTRequest.GetString("groupid"));
                }
            }
        }

        public void SetGroupRights(string groupid)
        {
            #region 设置组权限相关信息

            //DataRow usergrouprights = DbHelper.ExecuteDataset("SELECT TOP 1 * From  [" + BaseConfigs.GetTablePrefix + "usergroups] WHERE [groupid]=" + groupid).Tables[0].Rows[0];
            DataRow usergrouprights = DatabaseProvider.GetInstance().GetUserGroupInfoByGroupid(int.Parse(groupid)).Rows[0];
            stars.Text = usergrouprights["stars"].ToString();
            color.Text = usergrouprights["color"].ToString();

            groupavatar.Text = usergrouprights["groupavatar"].ToString();
            readaccess.Text = usergrouprights["readaccess"].ToString();
            maxprice.Text = usergrouprights["maxprice"].ToString();
            maxpmnum.Text = usergrouprights["maxpmnum"].ToString();


            maxsigsize.Text = usergrouprights["maxsigsize"].ToString();
            maxattachsize.Text = usergrouprights["maxattachsize"].ToString();
            maxsizeperday.Text = usergrouprights["maxsizeperday"].ToString();


            //设置用户权限组初始化信息
            //if (usergrouprights["allowvisit"].ToString() == "1") usergroupright.Items[0].Selected = true;
            //if (usergrouprights["allowpost"].ToString() == "1") usergroupright.Items[1].Selected = true;
            //if (usergrouprights["allowreply"].ToString() == "1") usergroupright.Items[2].Selected = true;
            //if (usergrouprights["allowpostpoll"].ToString() == "1") usergroupright.Items[3].Selected = true;
            //if (usergrouprights["allowgetattach"].ToString() == "1") usergroupright.Items[4].Selected = true;
            //if (usergrouprights["allowpostattach"].ToString() == "1") usergroupright.Items[5].Selected = true;
            //if (usergrouprights["allowvote"].ToString() == "1") usergroupright.Items[6].Selected = true;
            //if (usergrouprights["allowsetreadperm"].ToString() == "1") usergroupright.Items[7].Selected = true;
            //if (usergrouprights["allowsetattachperm"].ToString() == "1") usergroupright.Items[8].Selected = true;
            //if (usergrouprights["allowhidecode"].ToString() == "1") usergroupright.Items[9].Selected = true;
            //if (usergrouprights["allowcusbbcode"].ToString() == "1") usergroupright.Items[10].Selected = true;
            //if (usergrouprights["allowsigbbcode"].ToString() == "1") usergroupright.Items[11].Selected = true;
            //if (usergrouprights["allowsigimgcode"].ToString() == "1") usergroupright.Items[12].Selected = true;
            //if (usergrouprights["allowviewpro"].ToString() == "1") usergroupright.Items[13].Selected = true;
            //if (usergrouprights["disableperiodctrl"].ToString() == "1") usergroupright.Items[14].Selected = true;

            //if (usergrouprights["allowsearch"].ToString() == "0") allowsearch.Items[0].Selected = true;
            //if (usergrouprights["allowsearch"].ToString() == "1") allowsearch.Items[1].Selected = true;
            //if (usergrouprights["allowsearch"].ToString() == "2") allowsearch.Items[2].Selected = true;

            //if (usergrouprights["allowavatar"].ToString() == "0") allowavatar.Items[0].Selected = true;
            //if (usergrouprights["allowavatar"].ToString() == "1") allowavatar.Items[1].Selected = true;
            //if (usergrouprights["allowavatar"].ToString() == "2") allowavatar.Items[2].Selected = true;
            //if (usergrouprights["allowavatar"].ToString() == "3") allowavatar.Items[3].Selected = true;

            #endregion
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: 该调用是 ASP.NET Web 窗体设计器所必需的.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器修改
        /// 此方法的内容.
        /// </summary>
        private void InitializeComponent()
        {
            this.TabControl1.InitTabPage();
            this.AddUserGroupInf.Click += new EventHandler(this.AddUserGroupInf_Click);
            this.Load += new EventHandler(this.Page_Load);

            DataTable dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);
        }

        #endregion

        public int BoolToInt(bool a)
        {
            return a ? 1 : 0;
        }


        private void AddUserGroupInf_Click(object sender, EventArgs e)
        {
            #region 插入相关组信息数据

            if (this.CheckCookie())
            {

                __usergroupinfo.System = 0;
                __usergroupinfo.Type = 0;
                __usergroupinfo.Readaccess = Convert.ToInt32(readaccess.Text == "" ? "0" : readaccess.Text);

                __usergroupinfo.Allowdirectpost = 1;
                __usergroupinfo.Allowmultigroups = 0;
                __usergroupinfo.Allowcstatus = 0;
                __usergroupinfo.Allowuseblog = 0;
                __usergroupinfo.Allowinvisible = 0;
                __usergroupinfo.Allowtransfer = 0;
                __usergroupinfo.Allowhtml = 0;
                __usergroupinfo.Allownickname = 0;
                __usergroupinfo.Allowviewstats = 0;

                __usergroupinfo.Radminid = -1;
                __usergroupinfo.Grouptitle = groupTitle.Text;
                __usergroupinfo.Creditshigher = 0;
                __usergroupinfo.Creditslower = 0;

                __usergroupinfo.Stars = Convert.ToInt32(stars.Text);
                __usergroupinfo.Color = color.Text;
                __usergroupinfo.Groupavatar = groupavatar.Text;
                __usergroupinfo.Maxprice = Convert.ToInt32(maxprice.Text);
                __usergroupinfo.Maxpmnum = Convert.ToInt32(maxpmnum.Text);
                __usergroupinfo.Maxsigsize = Convert.ToInt32(maxsigsize.Text);
                __usergroupinfo.Maxattachsize = Convert.ToInt32(maxattachsize.Text);
                __usergroupinfo.Maxsizeperday = Convert.ToInt32(maxsizeperday.Text);
                __usergroupinfo.Attachextensions = attachextensions.GetSelectString(",");
                __usergroupinfo.Raterange = "";
                usergrouppowersetting.GetSetting(ref __usergroupinfo);
                if (AdminUserGroups.AddUserGroupInfo(__usergroupinfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加特殊用户组", "组名:" + groupTitle.Text);

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
    }
}