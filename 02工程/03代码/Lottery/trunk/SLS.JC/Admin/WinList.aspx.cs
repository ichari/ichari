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

public partial class Admin_WinList : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_WinList");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = null;

        string Condtion = "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue);

        if (Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) != "72" && Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) != "73")
        {
            Condtion += " and EndTime < GetDate() and isOpened = 1";
        }

        dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", Condtion, "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_WinList");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {
            BindData();
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();
    }

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_Schemes().Open("", "[IsuseID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue) + " and QuashStatus = 0 and Buyed = 1 and WinMoney <> 0", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_WinList");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void btnGo_Click(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            e.Item.Cells[2].Text = money.ToString("N");
            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = money.ToString("N");
            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = money.ToString("N");
        }
    }
}
