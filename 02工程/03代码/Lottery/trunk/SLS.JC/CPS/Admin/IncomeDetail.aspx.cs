using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

public partial class Cps_Admin_IncomeDetail : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindUserAccount();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=IncomeDetail.aspx";

        base.OnInit(e);
    }

    #endregion

    private void BindUserAccount()
    {
        //获取参数:当前查询的日期2009-01-01
        string strDt = Shove._Web.Utility.GetRequest("dt");
        DateTime fromDate = DateTime.Today;
        DateTime toDate = DateTime.Today;

        try
        {
            fromDate = Convert.ToDateTime(strDt + " 00:00:00");
            toDate = Convert.ToDateTime(strDt + " 23:59:59");
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "指定的查询日期无效!");

            return;
        }

        string Key = "Cps_Admin_IncomeDetail_" + _User.cps.ID + "_" + strDt;
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;

            int Result = DAL.Procedures.P_GetCpsBuyDetailByDate(ref ds, _Site.ID, _User.cps.ID, fromDate, toDate, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }

            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(Key, dt, 100000);
        }

        //绑定数据
        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindUserAccount();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindUserAccount();
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lbtnViewScheme = (LinkButton)e.Item.FindControl("lbtnViewScheme");
            if (lbtnViewScheme != null)
            {
                lbtnViewScheme.Text = e.Item.Cells[2].Text;
            }
        }
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "ViewScheme")
        {
            string schemeID = GetDataGridCellValue(g, e.Item.ItemIndex, "schemeID"); //方案号

            Response.Write("<script language='javascript'>window.open('../../Home/Room/Scheme.aspx?id=" + schemeID + "');</script>"); 
        }
    }

    private string GetDataGridCellValue(DataGrid g, int ItemIndex, string dataField)
    {
        bool isFind = false;
        string returnValue = "";
        for (int i = 0; i < g.Columns.Count; i++)
        {

            BoundColumn boundColumn = g.Columns[i] as BoundColumn;
            if (boundColumn != null)
            {
                if (boundColumn.DataField.ToLower() == dataField.ToLower())
                {
                    isFind = true;
                    returnValue = g.Items[ItemIndex].Cells[i].Text;

                    break;
                }
            }
        }
        if (!isFind)
        {
            //Shove._Web.JavaScript.Alert(this.Page, "GetDataGridCellValue 找不到指定的值");
            PF.GoError(-111, "找不到指定的列值,请联系技术员", this.GetType().Name);
        }
        return returnValue;
    }
}
