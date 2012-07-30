using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 搜索帖子
    /// </summary>

#if NET1
    public class searchpost : AdminPage
#else
    public partial class searchpost : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DropDownTreeList forumid;
        protected Discuz.Web.Admin.DropDownPost postlist;
        protected Discuz.Control.Calendar postdatetimeStart;
        protected Discuz.Control.Calendar postdatetimeEnd;
        protected Discuz.Control.TextBox poster;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox lowerupper;
        protected Discuz.Control.TextBox Ip;
        protected Discuz.Control.TextBox message;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveConditionInf;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30);
                postdatetimeEnd.SelectedDate = DateTime.Now;
            }
            forumid.BuildTree(DatabaseProvider.GetInstance().GetForumsTree());
            forumid.TypeID.Items.RemoveAt(0);
            forumid.TypeID.Items.Insert(0, new ListItem("全部", "0"));
        }


        private void SaveConditionInf_Click(object sender, EventArgs e)
        {
            #region 生成查询条件

            if (this.CheckCookie())
            {
                string sqlstring = DatabaseProvider.GetInstance().SearchPost(Utils.StrToInt(forumid.SelectedValue, 0), postlist.SelectedValue, postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, poster.Text, lowerupper.Checked, Ip.Text, message.Text);
                Discuz.Common.Utils.WriteCookie("seachpost_fid", forumid.SelectedValue, 4*60);
                //Session["posttablename"] = BaseConfigs.GetTablePrefix + "posts" + postlist.SelectedValue;
                Discuz.Common.Utils.WriteCookie("posttablename", BaseConfigs.GetTablePrefix + "posts" + DNTRequest.GetString("postlist:postslist"), 4*60);
                Discuz.Common.Utils.WriteCookie("postswhere", sqlstring, 4*60);
                Response.Redirect("forum_postgridmanage.aspx");

            }

            #endregion
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SaveConditionInf.Click += new EventHandler(this.SaveConditionInf_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}