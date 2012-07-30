using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;

public partial class Admin_SupperCoBuy : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

             BindData();

            g.Columns[6].Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindData()
    {
        string strCmd = "select top 10 SchemeNumber, InitiateName, Money, PlayTypeName, Share, Schedule, ID from V_Schemes with (nolock) where SiteID = "
            + _Site.ID.ToString() + "  and Share > BuyedShare and QuashStatus = 0  and LotteryID = " + ddlLottery.SelectedValue + " and datediff(mi,getdate(),SystemEndTime) > 0 order by [Money] desc";

        DataTable dt = MSSQL.Select(strCmd);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试 101", "Admin_SchemeAtTop");

            return;
        }
        try
        {
            PF.DataGridBindData(g, dt, gPager);
        }
        catch { }
    }

    protected void g_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[6].FindControl("cbCommend");

            cb.CheckedChanged += new System.EventHandler(this.g_ItemCheckedChanged);
        }
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[6].FindControl("cbCommend");

            cb.Checked = IsCheck(e.Item.Cells[7].Text);
        }
    }

    private bool IsCheck(string schemeID)
    {
        bool isCheck = false;

        string type = Shove._Web.Utility.GetRequest("TypeState");
        type = string.IsNullOrEmpty(type) ? "" : "TypeState=" + type;

        DataTable dt = new DAL.Tables.T_SchemeSupperCobuy().Open("",  type , "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试 102", "Admin_SchemeAtTop");

            return isCheck;
        }

        foreach (DataRow dr in dt.Rows)
        {
            if (dr["SchemeID"].ToString() == schemeID)
            {
                isCheck = true;
                break;
            }
        }

        return isCheck;
    }

    protected void g_ItemCheckedChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem item = (DataGridItem)cb.Parent.Parent;
        long id = Shove._Convert.StrToLong(item.Cells[7].Text, 0);
        bool Commend = cb.Checked;
        int Type = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("TypeState"), 1);
        string cmd = "";

        if (Commend)
        {
            if (!IsCheck(id.ToString()))
            {
                cmd = "insert T_SchemeSupperCobuy values(" + id + "," + Type + ")";
            }
        }
        else
        {
            cmd = "delete T_SchemeSupperCobuy where SchemeID=" + id + " and TypeState = "+Type+"";
        }

        if (MSSQL.ExecuteNonQuery(cmd) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试 104", "Admin_SupperCoBuy");

            return;
        }

        Shove._Web.Cache.ClearCache(CacheKey.SupperCobuy);
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
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
