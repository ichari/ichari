using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 合并用户
    /// </summary>
    
#if NET1
    public class combinationuser : AdminPage
#else
    public partial class combinationuser : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox username1;
        protected Discuz.Control.TextBox username2;
        protected Discuz.Control.TextBox username3;
        protected Discuz.Control.TextBox targetusername;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button CombinationUserInfo;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        { }


        private void CombinationUserInfo_Click(object sender, EventArgs e)
        {
            #region 合并用户

            if (this.CheckCookie())
            {
                int targetuid = AdminUsers.GetuidByusername(targetusername.Text);
                string result = null;
                if (targetuid > 0)
                {
                    int srcuid = 0;
                    if ((username1.Text != "") && (targetusername.Text.Trim() != username1.Text.Trim()))
                    {
                        srcuid = AdminUsers.GetuidByusername(username1.Text);
                        if (srcuid > 0)
                        {
                            AdminUsers.CombinationUser(srcuid, targetuid);
                            AdminUsers.UpdateForumsFieldModerators(username1.Text);
                            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户", "把用户" + username1.Text + " 合并到" + targetusername.Text);
                        }
                        else
                        {
                            result += "用户:" + username1.Text + "不存在!,";
                        }
                    }

                    srcuid = 0;
                    if ((username2.Text != "") && (targetusername.Text.Trim() != username2.Text.Trim()))
                    {
                        srcuid = AdminUsers.GetuidByusername(username2.Text);
                        if (srcuid > 0)
                        {
                            AdminUsers.CombinationUser(srcuid, targetuid);
                            AdminUsers.UpdateForumsFieldModerators(username2.Text);
                            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户", "把用户" + username2.Text + " 合并到" + targetusername.Text);
                        }
                        else
                        {
                            result += "用户:" + username2.Text + "不存在!,";
                        }
                    }

                    srcuid = 0;
                    if ((username3.Text != "") && (targetusername.Text.Trim() != username3.Text.Trim()))
                    {
                        srcuid = AdminUsers.GetuidByusername(username3.Text);
                        if (srcuid > 0)
                        {
                            AdminUsers.CombinationUser(srcuid, targetuid);
                            AdminUsers.UpdateForumsFieldModerators(username3.Text);
                            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "合并用户", "把用户" + username3.Text + " 合并到" + targetusername.Text);
                        }
                        else
                        {
                            result += "用户:" + username3.Text + "不存在!,";
                        }
                    }
                }
                else
                {
                    result += "目标用户:" + targetusername.Text + "不存在!,";
                }

                if (result == null)
                {
                    base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergrid.aspx';");
                }
                else
                {
                    result = result.Replace("'", "’");
                    base.RegisterStartupScript( "", "<script>alert('" + result + "');</script>");
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
            this.CombinationUserInfo.Click += new EventHandler(this.CombinationUserInfo_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

    }
}