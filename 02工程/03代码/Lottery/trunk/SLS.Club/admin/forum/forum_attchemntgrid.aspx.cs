using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;


using Discuz.Forum;
using Discuz.Common;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Data;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 附件编辑列表
    /// </summary>

#if NET1
    public class attchemntgrid : AdminPage
#else
    public partial class attchemntgrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button DeleteAttachment;
        protected Discuz.Control.Button DeleteAll;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Discuz.Common.Utils.GetCookie("attchmentwhere") != "")
                {
                    ViewState["condition"] = Discuz.Common.Utils.GetCookie("attchmentwhere").ToString();
                }
                else
                {
                    Response.Redirect("forum_searchattchment.aspx");
                    return;
                }
                BindData();
            }
        }

        public void BindData()
        {
            #region
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "附件列表";
            DataGrid1.BindData(DatabaseProvider.GetInstance().AttachDataBind(ViewState["condition"].ToString(), Posts.GetPostTableName().ToString()));
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.Cells[5].Text.ToString().Length > 15)
            {
                e.Item.Cells[5].Text = e.Item.Cells[5].Text.Substring(0, 15) + "…";
            }

            #endregion
        }


        private bool DeleteFile(string filename)
        {
            #region 删除指定文件

            if (Utils.FileExists(Utils.GetMapPath(@"..\..\upload\" + filename)))
            {
                File.Delete(Utils.GetMapPath(@"..\..\upload\" + filename));
                return true;
            }
            return false;

            #endregion
        }

        private DataTable GetAttachDataTable()
        {
            DataTable dt = DatabaseProvider.GetInstance().GetAttachDataTable(ViewState["condition"].ToString(), Posts.GetPostTableName().ToString());
            return dt;
        }

        private void DeleteAttachment_Click(object sender, EventArgs e)
        {
            #region 删除选中的附件

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("aid") != "")
                {
                    string aidlist = DNTRequest.GetString("aid");
                    DataTable dt = GetAttachDataTable();
                    foreach (DataRow dr in dt.Select("aid IN(" + aidlist + ")"))
                    {
                        DeleteFile(dr["filename"].ToString());
                    }
                    //DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [aid] IN(" + aidlist + ")");
                    DatabaseProvider.GetInstance().DeleteAttachment(aidlist);

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件", "ID:" + aidlist);
                    base.RegisterStartupScript("PAGE", "window.location.href='forum_searchattchment.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='forum_searchattchment.aspx';</script>");
                }
            }

            #endregion
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            #region 直接全部删除

            if (this.CheckCookie())
            {
                DataTable dt = GetAttachDataTable();
                string aid = "0";
                foreach (DataRow dr in dt.Rows)
                {
                    DeleteFile(dr["filename"].ToString());
                    aid += "," + dr["aid"].ToString();
                }
                //DbHelper.ExecuteNonQuery("DELETE FROM [" + BaseConfigs.GetTablePrefix + "attachments] WHERE [aid] IN(" + aid + ")");
                DatabaseProvider.GetInstance().DeleteAttachment(aid);

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件", "ID:" + aid);
                base.RegisterStartupScript("PAGE", "window.location.href='forum_searchattchment.aspx';");
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
            this.DeleteAttachment.Click += new EventHandler(this.DeleteAttachment_Click);
            this.DeleteAll.Click += new EventHandler(this.DeleteAll_Click);
            DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.TableHeaderName = "附件列表";
            DataGrid1.ColumnSpan = 7;
            DataGrid1.DataKeyField = "aid";
        }

        #endregion


    }
}