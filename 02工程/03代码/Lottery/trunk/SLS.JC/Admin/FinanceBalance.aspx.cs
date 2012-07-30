using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Admin_FinanceBalance : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForYearMonth();

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        if (ddlYear.Items.Count == 0)
        {
            return;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";

        DataSet ds = null;
        int Results = -1;
            Results = DAL.Procedures.P_GetAccount(ref ds, _Site.ID, int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceAddMoney");

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_FinanceAddMoney");

            return;
        }

        PF.DataGridBindData(g, ds.Tables[0], gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            for (int i = 3; i <= 9; i++)
            {
                double Money = Shove._Convert.StrToDouble(e.Item.Cells[i].Text, 0);

                e.Item.Cells[i].Text = (Money == 0) ? "" : Money.ToString("N");
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
