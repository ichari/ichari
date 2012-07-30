using System;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 短消息搜索
    /// </summary>

#if NET1
    public class searchsm : AdminPage
#else
    public partial class searchsm : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox isnew;
        protected Discuz.Control.TextBox postdatetime;
        protected Discuz.Control.TextBox msgfromlist;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox lowerupper;
        protected Discuz.Control.TextBox subject;
        protected Discuz.Control.TextBox message;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveSearchInfo;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox isupdateusernewpm;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        { }

        private void SaveSearchInfo_Click(object sender, EventArgs e)
        {
            #region 按指定条件进行短消息消除

            if (this.CheckCookie())
            {
                string sqlstring = DatabaseProvider.GetInstance().DeletePrivateMessages(isnew.Checked, postdatetime.Text, msgfromlist.Text, lowerupper.Checked, subject.Text, message.Text, isupdateusernewpm.Checked);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量清除短消息", "删除条件是:" + sqlstring);

                base.RegisterStartupScript( "PAGE", "window.location.href='forum_searchsm.aspx';");

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
            this.SaveSearchInfo.Click += new EventHandler(this.SaveSearchInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}