using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Entity;




namespace Discuz.Web.Admin
{
    /// <summary>
    /// 附件类型列表.
    /// </summary>
 
#if NET1
    public class attachtypesgrid : AdminPage
#else
    public partial class attachtypesgrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button DelRec;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox extension;
        protected Discuz.Control.TextBox maxsize;
        protected Discuz.Control.Button AddNewRec;
		protected Discuz.Control.Button SaveAttachType;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "上传附件类型列表";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetAttchTypeSql());
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
            #region 绑定附件类型显示方式

            if (e.Item.ItemType == ListItemType.EditItem)
            {
                TextBox t = (TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "255");
                t.Attributes.Add("size", "30");

                t = (TextBox)e.Item.Cells[4].Controls[0];
                t.Attributes.Add("maxlength", "9");
                t.Attributes.Add("size", "10");
            }

            if ((e.Item.ItemType == ListItemType.AlternatingItem) || (e.Item.ItemType == ListItemType.Item))
            {
                if (e.Item.Cells[3].Text.ToString().Length > 40)
                {
                    e.Item.Cells[3].Text = e.Item.Cells[3].Text.Substring(0, 40) + "…";
                }
            }

            #endregion
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除相关的附件类型

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    string idlist = DNTRequest.GetString("id");
                    DatabaseProvider.GetInstance().DeleteAttchType(idlist);
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除附件类型", "删除附件类型,ID为:" + DNTRequest.GetString("id").Replace("0 ", ""));

                    Response.Redirect("forum_attachtypesgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_attachtypesgrid.aspx';</script>");
                }
            }

            #endregion
        }

        private void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 添加新的附件信息

            if (extension.Text == "")
            {
                base.RegisterStartupScript( "", "<script>alert('要添加的附件扩展名不能为空');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }

            if ((maxsize.Text == "") || (Convert.ToInt32(maxsize.Text) <= 0))
            {
                base.RegisterStartupScript( "", "<script>alert('要添加的附件最大尺寸不能为空且要大于0');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }


            //if (DbHelper.ExecuteDataset("Select Top 1  * From [" + BaseConfigs.GetTablePrefix + "attachtypes] WHERE [extension]='" + extension.Text + "'").Tables[0].Rows.Count > 0)
            if(DatabaseProvider.GetInstance().IsExistExtensionInAttachtypes(extension.Text))
            {
                base.RegisterStartupScript( "", "<script>alert('数据库中已存在相同的附件扩展名');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }

            //string sql = string.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "attachtypes] ([extension], [maxsize]) VALUES ('{0}','{1}')",
            //    extension.Text,
            //    maxsize.Text
            //    );
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加附件类型", "添加附件类型,扩展名为:" + extension.Text);
            try
            {
                //DataGrid1.Insert(sql);
                DatabaseProvider.GetInstance().AddAttchType(extension.Text, maxsize.Text);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
                Attachments.GetAttachmentType();
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
                return;
            }
            catch
            {
                base.RegisterStartupScript( "", "<script>alert('无法更新数据库.');window.location.href='forum_attachtypesgrid.aspx';</script>");
                return;
            }

            #endregion
        }

        private void SaveAttachType_Click(object sender, EventArgs e)
        {
            #region 保存附件类型修改
            int rowid = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string id = o.ToString();
                string extension = DataGrid1.GetControlValue(rowid, "extension").Trim();
                string maxsize = DataGrid1.GetControlValue(rowid, "maxsize").Trim();
                if ((extension == "") || (maxsize == ""))
                {
                    error = true;
                    continue;
                }
                DatabaseProvider.GetInstance().UpdateAttchType(extension, maxsize, int.Parse(id));
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑附件类型", "编辑附件类型,扩展名为:" + extension);
                rowid++;
            }
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/ForumSetting/AttachmentType");
            if(error)
                base.RegisterStartupScript("", "<script>alert('某些记录取值不正确，未能被更新！');window.location.href='forum_attachtypesgrid.aspx';</script>");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_attachtypesgrid.aspx';");
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
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.SaveAttachType.Click += new EventHandler(this.SaveAttachType_Click);
            this.Load += new EventHandler(this.Page_Load);
            DataGrid1.ColumnSpan = 4;
        }

        #endregion
    }
}