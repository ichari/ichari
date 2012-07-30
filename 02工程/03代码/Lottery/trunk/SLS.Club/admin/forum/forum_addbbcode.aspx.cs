using System;
using System.Text.RegularExpressions;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加Discuz!NT代码
    /// </summary>

#if NET1
    public class addbbcode : AdminPage
#else
    public partial class addbbcode : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList available;
        protected Discuz.Control.UpFile icon;
        protected Discuz.Control.TextBox tag;
        protected Discuz.Web.Admin.TextareaResize replacement;
        protected Discuz.Web.Admin.TextareaResize example;
        protected Discuz.Web.Admin.TextareaResize explanation;
        protected Discuz.Control.TextBox param;
        protected Discuz.Control.TextBox nest;
        protected Discuz.Web.Admin.TextareaResize paramsdescript;
        protected Discuz.Web.Admin.TextareaResize paramsdefvalue;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button AddAdInfo;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                icon.UpFilePath = Server.MapPath(icon.UpFilePath);
            }
        }

        /// <summary>
        /// 增加Discuz!NT代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAdInfo_Click(object sender, EventArgs e)
        {
            #region 添加Discuz!NT代码

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().AddBBCCode(int.Parse(available.SelectedValue), Regex.Replace(tag.Text.Replace("<", "").Replace(">", ""), @"^[\>]|[\{]|[\}]|[\[]|[\]]|[\']|[\.]", ""),
                    icon.UpdateFile(), replacement.Text, example.Text, explanation.Text, param.Text, nest.Text, paramsdescript.Text, paramsdefvalue.Text);

                AdminCaches.ReSetCustomEditButtonList();

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加Discuz!NT代码", "TAG为:" + tag.Text);

                base.RegisterStartupScript("", "<script>window.location.href='forum_bbcodegrid.aspx';</script>");
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
            this.AddAdInfo.Click += new EventHandler(this.AddAdInfo_Click);
        }

        #endregion
    }
}