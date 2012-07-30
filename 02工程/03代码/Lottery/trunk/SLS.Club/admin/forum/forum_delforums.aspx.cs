using System;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 删除版块
    /// </summary>
    
#if NET1
    public class delforums : AdminPage
#else
    public partial class delforums : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.HtmlControls.HtmlForm Form1;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region 按FID删除相应的版块

                if (AdminForums.DeleteForumsByFid(DNTRequest.GetString("fid")))
                {
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除论坛版块", "删除论坛版块,fid为:" + DNTRequest.GetString("fid"));
                    base.RegisterStartupScript( "", "<script>window.location.href='forum_ForumsTree.aspx';</script>");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('对不起,当前节点下面还有子结点,因此不能删除！');window.location.href='forum_ForumsTree.aspx';</script>");
                }

                #endregion
            }
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}