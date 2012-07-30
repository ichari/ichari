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

using Shove.Database;

public partial class Admin_SchemeAtTop : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            BindData();

            g.Columns[6].Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

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

        DataTable dt = new DAL.Tables.T_Isuses().Open("", "StartTime < GetDate() and LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

            return;
        }

        ddlIsuse.Items.Clear();

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");
    }

    private void BindData()
    {
        if (ddlIsuse.Items.Count < 1)
        {
            return;
        }

        string Cmd;

        if (cbUserApplication.Checked)
        {
            Cmd = "select * from V_SchemeSchedules where SiteID = " + _Site.ID.ToString() + " and IsuseID = " + ddlIsuse.SelectedValue + " and QuashStatus = 0 and Buyed = 0 and AtTopStatus = 1 order by [Money] desc";
        }
        else
        {
            Cmd = "select * from V_SchemeSchedules where SiteID = " + _Site.ID.ToString() + " and IsuseID = " + ddlIsuse.SelectedValue + " and QuashStatus = 0 and Buyed = 0 order by [Money] desc";
        }

        DataTable dt = MSSQL.Select(Cmd);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[6].FindControl("cbAtTop");

            cb.CheckedChanged += new System.EventHandler(this.g_ItemCheckedChanged);
        }
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[6].FindControl("cbAtTop");

            cb.Checked = (Shove._Convert.StrToShort(e.Item.Cells[8].Text, 0) == 2);
        }
    }

    protected void g_ItemCheckedChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem item = (DataGridItem)cb.Parent.Parent;
        long id = Shove._Convert.StrToLong(item.Cells[7].Text, 0);
        bool AtTop = cb.Checked;

        if (MSSQL.ExecuteNonQuery("update T_Schemes set AtTopStatus = " + (AtTop ? "2" : "0") + " where [ID] = " + id.ToString()) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

            return;
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();

        BindData();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void cbUserApplication_CheckedChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
