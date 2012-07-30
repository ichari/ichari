using System;
using System.Web.UI.WebControls;
using System.Collections;

using Discuz.Forum;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户组列表
    /// </summary>
    
#if NET1
    public class usergroupgrid : AdminPage
#else
    public partial class usergroupgrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected CheckBox chkConfirmInsert;
        protected CheckBox chkConfirmUpdate;
        protected CheckBox chkConfirmDelete;
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
            #region 绑定用户组列表
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "用户组列表";
            DataGrid1.Attributes.Add("borderStyle", "2");
            DataGrid1.DataKeyField = "groupid";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetUserGroup());
            DataGrid1.Sort = "creditshigher";
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

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
            DataGrid1.DataKeyField = "groupid";
            DataGrid1.ColumnSpan = 12;
        }

        #endregion

        protected void EditUserGroup_Click(object sender, EventArgs e)
        {
            #region 编辑用户组
            try
            {
                int row = 0;
                ArrayList creditshigherArray = new ArrayList();
                ArrayList creditslowerArray = new ArrayList();
                ArrayList updateArray = new ArrayList();
                foreach (object o in DataGrid1.GetKeyIDArray())
                {
                    int groupid = int.Parse(o.ToString());
                    string grouptitle = DataGrid1.GetControlValue(row, "grouptitle");
                    if (grouptitle.Trim() == "")
                    {
                        base.RegisterStartupScript("", "<script>alert('组标题未输入,请检查!');window.location.href='global_usergroupgrid.aspx';</script>");
                        return;
                    }
                    int creditshigher = int.Parse(DataGrid1.GetControlValue(row, "creditshigher"));
                    int creditslower = int.Parse(DataGrid1.GetControlValue(row, "creditslower"));
                    if (creditshigher >= creditslower)
                    {
                        base.RegisterStartupScript("", "<script>alert('" + grouptitle + "组的金币下限超过上限,请检查!');window.location.href='global_usergroupgrid.aspx';</script>");
                        return;
                    }
                    creditshigherArray.Add(creditshigher);
                    creditslowerArray.Add(creditslower);
                    updateArray.Add(new UserGroup(groupid, grouptitle, creditshigher, creditslower));
                    row++;
                }
                creditshigherArray.Sort();
                creditslowerArray.Sort();
                for (int i = 1; i < creditshigherArray.Count; i++)
                {
                    if (creditshigherArray[i].ToString() != creditslowerArray[i - 1].ToString())
                    {
                        base.RegisterStartupScript("", "<script>alert('金币下限与上限取值不连续,请检查!');window.location.href='global_usergroupgrid.aspx';</script>");
                        return;
                    }
                }
                for (int i = 0; i < updateArray.Count; i++)
                {
                    UserGroup ug = (UserGroup)updateArray[i];
                    DatabaseProvider.GetInstance().UpdateUserGroupTitleAndCreditsByGroupid(ug.id, ug.grouptitle, ug.creditslower, ug.creditshigher);
                }
                base.RegisterStartupScript("", "<script>window.location.href='global_usergroupgrid.aspx';</script>");
            }
            catch
            {
                base.RegisterStartupScript("", "<script>alert('金币下限或是上限输入的数值不合法,请检查!');window.location.href='global_usergroupgrid.aspx';</script>");
            }
            #endregion
        }

        protected void DataGrid1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            //for (int i = 0; i < DataGrid1.Columns.Count - 1; i++)
            //{
            //    TextBox tb = e.Item.Cells[i].Controls[0] as TextBox;
            //    if(tb != null)
            //        tb.Attributes.Add("size", "8");
            //}
        }
    }

    struct UserGroup
    {
        public int id;
        public string grouptitle;
        public int creditshigher;
        public int creditslower;

        public UserGroup(int id,string grouptitle,int creditshigher,int creditslower)
        {
            this.id = id;
            this.grouptitle = grouptitle;
            this.creditshigher = creditshigher;
            this.creditslower = creditslower;
        }
    }

}