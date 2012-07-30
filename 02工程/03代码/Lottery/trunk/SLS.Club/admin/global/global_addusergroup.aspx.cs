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
    /// 添加用户组
    /// </summary>
    
#if NET1
    public class addusergroup : AdminPage
#else
    public partial class addusergroup : AdminPage
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

            DataRow usergrouprights = DatabaseProvider.GetInstance().GetUserGroupInfoByGroupid(int.Parse(groupid)).Rows[0];
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
                __usergroupinfo.Radminid = 0;
                __usergroupinfo.Grouptitle = groupTitle.Text;
                __usergroupinfo.Creditshigher = Convert.ToInt32(creditshigher.Text);
                __usergroupinfo.Creditslower = Convert.ToInt32(creditslower.Text);
                usergrouppowersetting.GetSetting(ref __usergroupinfo);
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
                __usergroupinfo.Raterange = "";

                if (AdminUserGroups.AddUserGroupInfo(__usergroupinfo))
                {
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UserGroupList");
                    UserGroups.GetUserGroupList();

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台添加用户组", "组名:" + groupTitle.Text);

                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergroupgrid.aspx';");
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
    }
}