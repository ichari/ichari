using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户列表
    /// </summary>
     
#if NET1
    public class usergrid : AdminPage
#else
    public partial class usergrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.WebControls.Panel searchtable;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox ispostdatetime;
  
        protected Discuz.Control.TextBox Username;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox islike;
        protected Discuz.Control.TextBox nickname;
        protected Discuz.Control.DropDownList UserGroup;
        protected Discuz.Control.TextBox email;
        protected Discuz.Control.TextBox uid;
        protected Discuz.Control.Calendar joindateStart;
        protected Discuz.Control.Calendar joindateEnd;
        protected Discuz.Control.TextBox credits_start;
        protected Discuz.Control.TextBox credits_end;
        protected Discuz.Control.TextBox lastip;
        protected Discuz.Control.TextBox posts;
        protected Discuz.Control.TextBox digestposts;
        protected Discuz.Control.Button Search;
        protected Discuz.Control.Button StopTalk;
        protected Discuz.Control.Button DeleteUser;
        protected Discuz.Control.CheckBoxList deltype;
        protected Discuz.Control.Button ResetSearchTable;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox Users;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region 实始化控件

                joindateStart.SelectedDate = DateTime.Now.AddDays(-30);

                joindateEnd.SelectedDate = DateTime.Now;

                //UserGroup.AddTableData("SELECT [groupid], [grouptitle] FROM [" + BaseConfigs.GetTablePrefix + "usergroups] ORDER BY [groupid]");
                UserGroup.AddTableData(DatabaseProvider.GetInstance().GetGroupInfo());

                if ((DNTRequest.GetString("username") != null) && (DNTRequest.GetString("username") != ""))
                {
                    ViewState["condition"] = DatabaseProvider.GetInstance().Global_UserGrid_GetCondition(DNTRequest.GetString("username"));
                    //" [" + BaseConfigs.GetTablePrefix + "users].[username]='" + DNTRequest.GetString("username") + "' "
                    searchtable.Visible = false;
                    ResetSearchTable.Visible = true;
                }

                if (ViewState["condition"] != null)
                {
                    searchtable.Visible = false;
                    ResetSearchTable.Visible = true;
                }
                else
                {
                    if (DNTRequest.GetString("condition") != "")
                    {
                        ViewState["condition"] = DNTRequest.GetString("condition").Replace("~^", "'").Replace("~$", "%");
                        searchtable.Visible = false;
                        ResetSearchTable.Visible = true;
                    }
                }
                BindData();

                #endregion
            }
        }

        public void BindData()
        {
            #region 绑定数据

            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();
            DataGrid1.DataSource = buildGridData();
            DataGrid1.DataBind();

            #endregion
        }

        private DataTable buildGridData()
        {
            #region 加载数据

            DataTable dt = new DataTable();
            if (ViewState["condition"] == null)
            {
                dt = UserList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                dt = UserList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
            }

            if ((dt.Rows.Count == 1) && (DNTRequest.GetString("username") != null) && (DNTRequest.GetString("username") != ""))
            {
                Response.Redirect("global_edituser.aspx?uid=" + dt.Rows[0][0].ToString());
            }
            return dt;

            #endregion
        }

        public void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        private int GetRecordCount()
        {
            #region 获取记录数

            if (ViewState["condition"] == null)
            {
                return RecordCount();
            }
            else
            {
                return RecordCount(ViewState["condition"].ToString());
            }

            #endregion
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private void StopTalk_Click(object sender, EventArgs e)
        {
            #region 禁言
            if (DNTRequest.GetString("uid") != "")
            {
                string uidlist = "0" + DNTRequest.GetString("uid");
                string[] uids = uidlist.Split(',');
                foreach (string uid in uids)
                {
                    int iuid = int.Parse(uid);
                    if (iuid != 0)
                    {
                        OnlineUsers.DeleteUserByUid(iuid);
                    }
                }
                DatabaseProvider.GetInstance().SetStopTalkUser(uidlist);
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_usergrid.aspx';");
            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('请选择相应的用户!');window.location.href='global_usergrid.aspx';</script>");
            }

            #endregion
        }

        private bool CheckSponser(int uid)
        {
            #region 检查创建人

            if ((BaseConfigs.GetBaseConfig().Founderuid == uid) && (BaseConfigs.GetBaseConfig().Founderuid != this.userid))
            {
                return false;
            }
            else
            {
                return true;
            }

            #endregion
        }

        private void DeleteUser_Click(object sender, EventArgs e)
        {
            #region 删除相关用户

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("uid") != "")
                {
                    bool delpost = deltype.SelectedValue.IndexOf("1") >= 0 ? false : true;
                    bool delpms = deltype.SelectedValue.IndexOf("2") >= 0 ? false : true;

                    foreach (string uid in DNTRequest.GetString("uid").Split(','))
                    {
                        if (uid != "")
                        {
                            if (CheckSponser(Convert.ToInt32(uid)))
                            {
                                if (Convert.ToInt32(uid) > 1) //判断是不是当前ＵＩＤ是不是系统初始化时生成的ＵＩＤ
                                {
                                    int deluserid = Convert.ToInt32(uid);
                                    if (AdminUsers.DelUserAllInf(deluserid, delpost, delpms))
                                    {
                                        AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "后台删除用户", "用户名:批量用户删除");
                                        base.RegisterStartupScript( "PAGE", "window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('请选择相应的用户!');window.location.href='global_usergrid.aspx?condition=" + DNTRequest.GetString("condition") + "';</script>");
                }
            }

            #endregion
        }


        private void Search_Click(object sender, EventArgs e)
        {
            #region 按指定条件查询用户数据

            if (this.CheckCookie())
            {

                string searchcondition = DatabaseProvider.GetInstance().Global_UserGrid_SearchCondition(islike.Checked, ispostdatetime.Checked, Username.Text, nickname.Text, UserGroup.SelectedValue, email.Text, credits_start.Text, credits_end.Text, lastip.Text, posts.Text, digestposts.Text, uid.Text, joindateStart.SelectedDate.ToString(), joindateEnd.SelectedDate.AddDays(1).ToString());
                //string searchcondition = " [" + BaseConfigs.GetTablePrefix + "users].[uid]>0 ";
                //if (islike.Checked)
                //{
                //    if (Username.Text != "") searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[username] like'%" + Username.Text + "%'";
                //    if (nickname.Text != "") searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[nickname] like'%" + nickname.Text + "%'";
                //}
                //else
                //{
                //    if (Username.Text != "") searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[username] ='" + Username.Text + "'";
                //    if (nickname.Text != "") searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[nickname] ='" + nickname.Text + "'";
                //}

                //if (UserGroup.SelectedValue != "0")
                //{
                //    searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[groupid]=" + UserGroup.SelectedValue;
                //}

                //if (email.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[email] LIKE '%" + email.Text + "%'";
                //}

                //if (credits_start.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] >=" + credits_start.Text;
                //}

                //if (credits_end.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[credits] <=" + credits_end.Text;
                //}

                //if (lastip.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[lastip] LIKE '%" + lastip.Text + "%'";
                //}

                //if (posts.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[posts] >=" + posts.Text;
                //}


                //if (digestposts.Text != "")
                //{
                //    searchcondition += " AND [" + BaseConfigs.GetTablePrefix + "users].[digestposts] >=" + digestposts.Text;
                //}

                //if (uid.Text != "")
                //{
                //    uid.Text = uid.Text.Replace(", ", ",");

                //    if (uid.Text.IndexOf(",") == 0)
                //    {
                //        uid.Text = uid.Text.Substring(1, uid.Text.Length - 1);
                //    }
                //    if (uid.Text.LastIndexOf(",") == (uid.Text.Length - 1))
                //    {
                //        uid.Text = uid.Text.Substring(0, uid.Text.Length - 1);
                //    }

                //    if (uid.Text != "")
                //    {
                //        searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[uid] IN(" + uid.Text + ")";
                //    }

                //}

                //if (ispostdatetime.Checked)
                //{
                //    searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[joindate] >='" + joindateStart.SelectedDate.ToString() + "'";
                //    searchcondition += " And [" + BaseConfigs.GetTablePrefix + "users].[joindate] <='" + joindateEnd.SelectedDate.AddDays(1).ToString() + "'";
                //}



                ViewState["condition"] = searchcondition;

                searchtable.Visible = false;
                ResetSearchTable.Visible = true;

                DataTable dt = DatabaseProvider.GetInstance().Global_UserGrid_Top2(searchcondition);
                    //DbHelper.ExecuteDataset("SELECT TOP 2 [" + BaseConfigs.GetTablePrefix + "users].[uid]  FROM [" + BaseConfigs.GetTablePrefix + "users] WHERE " + searchcondition).Tables[0];
                if (dt.Rows.Count == 1)
                {
                    Response.Redirect("global_edituser.aspx?uid=" + dt.Rows[0][0].ToString() + "&condition=" + ViewState["condition"].ToString().Replace("'", "~^").Replace("%", "~$"));
                }
                else
                {
                    DataGrid1.CurrentPageIndex = 0;
                    BindData();
                }
            }

            #endregion
        }


        public DataTable UserList(int pagesize, int currentpage)
        {
            #region 获得用户列表
            return DatabaseProvider.GetInstance().GetUserList(pagesize, currentpage);

            #endregion
        }

        public DataTable UserList(int pagesize, int currentpage, string condition)
        {
            return DatabaseProvider.GetInstance().UserList(pagesize,currentpage,condition);
        }


        #region 获取记录数

        public int RecordCount()
        {
            return DatabaseProvider.GetInstance().Global_UserGrid_RecordCount();
        }


        public int RecordCount(string condition)
        {
            return DatabaseProvider.GetInstance().Global_UserGrid_RecordCount(condition);
        }

        #endregion

        private void ResetSearchTable_Click(object sender, EventArgs e)
        {
            Response.Redirect("global_usergrid.aspx");
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Search.Click += new EventHandler(this.Search_Click);
            this.StopTalk.Click += new EventHandler(this.StopTalk_Click);
            this.DeleteUser.Click += new EventHandler(this.DeleteUser_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
            this.ResetSearchTable.Click += new EventHandler(this.ResetSearchTable_Click);

            this.Load += new EventHandler(this.Page_Load);
            DataGrid1.TableHeaderName = "用户列表";
            DataGrid1.DataKeyField = "uid";
            DataGrid1.AllowSorting = false;
            DataGrid1.ColumnSpan = 12;
        }

        #endregion

    }
}