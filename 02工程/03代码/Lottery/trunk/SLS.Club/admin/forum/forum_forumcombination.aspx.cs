using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Common;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 论坛版块合并
    /// </summary>
    
#if NET1
    public class forumcombination : AdminPage
#else
    public partial class forumcombination : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DropDownTreeList sourceforumid;
        protected Discuz.Control.DropDownTreeList targetforumid;
        protected Discuz.Control.Button SaveCombinationInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            sourceforumid.BuildTree(DatabaseProvider.GetInstance().GetForumsTree());
            targetforumid.BuildTree(DatabaseProvider.GetInstance().GetForumsTree());
            
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("fid") != "")
                {
                    sourceforumid.SelectedValue = DNTRequest.GetString("fid");
                }
            }
        }


        private void SaveCombinationInfo_Click(object sender, EventArgs e)
        {
            #region 合并论坛版块

            if (this.CheckCookie())
            {
                if (sourceforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript( "", "<script>alert('请选择相应的源论坛!');</script>");
                    return;
                }

                if (targetforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript( "", "<script>alert('请选择相应的目标论坛!');</script>");
                    return;
                }

                DataTable dt = DatabaseProvider.GetInstance().GetTopForum(Utils.StrToInt(targetforumid.SelectedValue, 0));
                if (dt.Rows.Count > 0)
                {
                    base.RegisterStartupScript( "", "<script>alert('您所选择的目标论坛是\"论坛分类\"而不是\"论坛版块\",因此合并无效!');</script>");
                    return;
                }

                string result;
                if (!AdminForums.CombinationForums(sourceforumid.SelectedValue, targetforumid.SelectedValue))
                {
                    result = "<script>alert('当前节点下面有子结点,因此合并无效!');window.location.href='forum_forumcombination.aspx';</script>";
                    base.RegisterStartupScript( "", result);
                    return;
                }
                else
                {
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并论坛版块", "合并论坛版块" + sourceforumid.SelectedValue + "到" + targetforumid.SelectedValue);

                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_forumstree.aspx';");
                    return;
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
            this.SaveCombinationInfo.Click += new EventHandler(this.SaveCombinationInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}