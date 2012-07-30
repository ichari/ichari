using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    public partial class addhelp : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if ((this.username != null) && (this.username != ""))
                {
                    poster.Text = this.username;
                    type.AddTableData(Helps.bindhelptype());
                    Addhelp.ValidateForm = true;
                    tbtitle.AddAttributes("maxlength", "200");
                    tbtitle.AddAttributes("rows", "2");
                    type.DataBind();
                }
            }
        }

        protected void Addhelp_Click(object sender, EventArgs e)
        {
            #region 增加帮助项
            if (this.CheckCookie())
            {
                if (int.Parse(type.SelectedItem.Value) == 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='global_addhelp.aspx';</script>");
                }
                else
                {
                    Helps.addhelp(tbtitle.Text, message.Text, int.Parse(type.SelectedItem.Value));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加帮助", "添加帮助,标题为:" + tbtitle.Text);
            #if NET1
                    if (!base.IsStartupScriptRegistered("page"))
                    {
                        base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
                    }
            #else
                    base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
            #endif
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
            this.Addhelp.Click += new EventHandler(this.Addhelp_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion
    }
}
