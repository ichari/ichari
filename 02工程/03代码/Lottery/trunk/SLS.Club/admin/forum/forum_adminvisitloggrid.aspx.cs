using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;


using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 后台访问日志列表
    /// </summary>
    
#if NET1
    public class adminvisitloggrid : AdminPage
#else
    public partial class adminvisitloggrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Calendar postdatetimeStart;
        protected Discuz.Control.Calendar postdatetimeEnd;
        protected Discuz.Control.TextBox others;
        protected Discuz.Control.TextBox Username;
        protected Discuz.Control.Button SearchLog;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox deleteNum;
        protected Discuz.Control.Calendar deleteFrom;
        protected Discuz.Control.Button DelRec;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                postdatetimeStart.SelectedDate = DateTime.Now.AddDays(-30);
                postdatetimeEnd.SelectedDate = DateTime.Now;
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = true;
            DataGrid1.VirtualItemCount = GetRecordCount();
            DataGrid1.DataSource = buildGridData();
            DataGrid1.DataBind();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.CurrentPageIndex = e.NewPageIndex;
            BindData();
        }

        public void GoToPagerButton_Click(object sender, EventArgs e)
        {
            BindData();
        }

        private DataTable buildGridData()
        {
            if (ViewState["condition"] == null)
            {
                return AdminVistLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1);
            }
            else
            {
                return AdminVistLogs.LogList(DataGrid1.PageSize, DataGrid1.CurrentPageIndex + 1, ViewState["condition"].ToString());
            }
        }

        private int GetRecordCount()
        {
            #region 得到日志记录数

            if (ViewState["condition"] == null)
            {
                return AdminVistLogs.RecordCount();
            }
            else
            {
                return AdminVistLogs.RecordCount(ViewState["condition"].ToString());
            }

            #endregion
        }


        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除指定条件的日志信息

            if (this.CheckCookie())
            {
                string condition = "";
                //switch (Request.Form["deleteMode"])
                //{
                //    case "chkall":
                //        if (DNTRequest.GetString("visitid") != "")
                //            condition = " [visitid] IN(" + DNTRequest.GetString("visitid") + ")";
                //        break;
                //    case "deleteNum":
                //        if (deleteNum.Text != "" && Utils.IsNumeric(deleteNum.Text))
                //            condition = " [visitid] not in (select top " + deleteNum.Text + " [visitid] from [" + BaseConfigs.GetTablePrefix + "adminvisitlog] order by [visitid] desc)";
                //        break;
                //    case "deleteFrom":
                //        if (deleteFrom.SelectedDate.ToString() != "")
                //            condition = " [postdatetime]<'" + deleteFrom.SelectedDate.ToString() + "'";
                //        break;
                //}
                condition = DatabaseProvider.GetInstance().DelVisitLogCondition(Request.Form["deleteMode"].ToString(), DNTRequest.GetString("visitid").ToString(), deleteNum.Text, deleteFrom.SelectedDate.ToString());
                if (condition != "")
                {
                    AdminVistLogs.DeleteLog(condition);
                    Response.Redirect("forum_adminvisitloggrid.aspx");
                }                      
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项或输入参数错误！');window.location.href='forum_adminvisitloggrid.aspx';</script>");
                }                                                                                                                                           
            }

            #endregion
        }

        public string BoolStr(string closed)
        {
            #region 返回图标
            if (closed == "1")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            else
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
            #endregion
        }

        private void SearchLog_Click(object sender, EventArgs e)
        {
            #region 按指定条件搜索后台访问日志

            if (this.CheckCookie())
            {
                string sqlstring = DatabaseProvider.GetInstance().SearchVisitLog(postdatetimeStart.SelectedDate, postdatetimeEnd.SelectedDate, Username.Text, others.Text);               

                ViewState["condition"] = sqlstring;
                DataGrid1.CurrentPageIndex = 0;
                BindData();
            }

            #endregion
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SearchLog.Click += new EventHandler(this.SearchLog_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.TableHeaderName = "后台访问记录";
            DataGrid1.AllowSorting = false;
            DataGrid1.DataKeyField = "visitid";
            DataGrid1.ColumnSpan = 7;
        }

        #endregion
    }
}