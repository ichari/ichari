using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;
using Discuz.Entity;


namespace Discuz.Web.Admin
{

#if NET1
    public class forum_userrights : AdminPage
#else
    public partial class forum_userrights : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList dupkarmarate;
        protected Discuz.Control.TextBox maxpolloptions;
        protected Discuz.Control.TextBox maxattachments;
        protected Discuz.Control.TextBox minpostsize;
        protected Discuz.Control.TextBox maxpostsize;
        protected Discuz.Control.TextBox maxfavorites;
        protected Discuz.Control.TextBox maxavatarsize;
        protected Discuz.Control.TextBox maxavatarpixel;
        protected Discuz.Control.TextBox maxavatarwidth;
        protected Discuz.Control.TextBox maxavatarheight;
        protected Discuz.Control.TextBox karmaratelimit;
        protected Discuz.Control.RadioButtonList moderactions;
        protected Discuz.Control.CheckBoxList UserGroup;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                LoadUserGroup();
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                foreach (string groupid in __configinfo.Htmltitleusergroup.Split(','))
                {
                   for(int i = 0 ; i < UserGroup.Items.Count ; i++)
                   {
                       if(UserGroup.Items[i].Value == groupid)
                       {
                           UserGroup.Items[i].Selected = true;
                           break;
                       }
                   }
                }
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息

            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));

            dupkarmarate.SelectedValue = __configinfo.Dupkarmarate.ToString();
            minpostsize.Text = __configinfo.Minpostsize.ToString();
            maxpostsize.Text = __configinfo.Maxpostsize.ToString();
            maxfavorites.Text = __configinfo.Maxfavorites.ToString();
            maxavatarsize.Text = __configinfo.Maxavatarsize.ToString();
            maxavatarwidth.Text = __configinfo.Maxavatarwidth.ToString();
            maxavatarheight.Text = __configinfo.Maxavatarheight.ToString();
            maxpolloptions.Text = __configinfo.Maxpolloptions.ToString();
            maxattachments.Text = __configinfo.Maxattachments.ToString();
            karmaratelimit.Text = __configinfo.Karmaratelimit.ToString();
            moderactions.SelectedValue = __configinfo.Moderactions.ToString();
            #endregion
        }

        private void LoadUserGroup()
        {
            #region 加载用户组
            UserGroup.DataSource = DatabaseProvider.GetInstance().GetUserGroupsTitle();
            UserGroup.DataValueField = "groupid";
            UserGroup.DataTextField = "grouptitle";
            UserGroup.DataBind();
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                if (Convert.ToInt32(minpostsize.Text) > 9999999 || (Convert.ToInt32(minpostsize.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('帖子最小字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxpostsize.Text) > 9999999 || (Convert.ToInt32(maxpostsize.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('帖子最大字数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxfavorites.Text) > 9999999 || (Convert.ToInt32(maxfavorites.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('收藏夹容量只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxavatarsize.Text) > 9999999 || (Convert.ToInt32(maxavatarsize.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('最大签名高度只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxavatarwidth.Text) > 9999999 || (Convert.ToInt32(maxavatarwidth.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('头像最大宽度只能在165-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxavatarheight.Text) > 9999999 || (Convert.ToInt32(maxavatarheight.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('头像最大宽度只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxpolloptions.Text) > 9999999 || (Convert.ToInt32(maxpolloptions.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('最大签名高度只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }

                if (Convert.ToInt32(maxattachments.Text) > 9999999 || (Convert.ToInt32(maxattachments.Text) < 0))
                {
                    base.RegisterStartupScript( "", "<script>alert('投票最大选项数只能在0-9999999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }
                if (Convert.ToInt32(karmaratelimit.Text) > 9999 || (Convert.ToInt32(karmaratelimit.Text) < 0))
                {
                    base.RegisterStartupScript("", "<script>alert('评分时间限制只能在0-9999之间');window.location.href='forum_userrights.aspx';</script>");
                    return;
                }


                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));

                __configinfo.Dupkarmarate = Convert.ToInt16(dupkarmarate.SelectedValue);
                __configinfo.Minpostsize = Convert.ToInt32(minpostsize.Text);
                __configinfo.Maxpostsize = Convert.ToInt32(maxpostsize.Text);
                __configinfo.Maxfavorites = Convert.ToInt32(maxfavorites.Text);
                __configinfo.Maxavatarsize = Convert.ToInt32(maxavatarsize.Text);
                __configinfo.Maxavatarwidth = Convert.ToInt32(maxavatarwidth.Text);
                __configinfo.Maxavatarheight = Convert.ToInt32(maxavatarheight.Text);
                __configinfo.Maxpolloptions = Convert.ToInt32(maxpolloptions.Text);
                __configinfo.Maxattachments = Convert.ToInt32(maxattachments.Text);
                __configinfo.Karmaratelimit = Convert.ToInt16(karmaratelimit.Text);
                __configinfo.Moderactions = Convert.ToInt16(moderactions.SelectedValue);

                string groupList = "";
                for (int i = 0; i < UserGroup.Items.Count; i++)
                {
                    if (UserGroup.Items[i].Selected)
                    {
                        groupList += UserGroup.Items[i].Value + ",";
                    }
                }
                __configinfo.Htmltitleusergroup = groupList.TrimEnd(',');

                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "用户权限设置", "");
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_userrights.aspx';");
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}
