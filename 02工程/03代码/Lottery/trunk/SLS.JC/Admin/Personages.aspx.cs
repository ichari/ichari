using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Personages : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindLotteryType();
            BindData();

            hlAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
            g.Columns[4].Visible = hlAdd.Visible;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent,Competences.QueryData);

        base.OnInit(e);
    }

    private void BindLotteryType()
    {
        string LotteryID = Shove._Web.Utility.GetRequest("LotteryID");

        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-46)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        ddlLotteries.DataSource = dtLotteries;
        ddlLotteries.DataTextField = "Name";
        ddlLotteries.DataValueField = "ID";
        ddlLotteries.DataBind();

        if (ddlLotteries.Items.FindByValue(LotteryID) != null)
        {
            ddlLotteries.SelectedValue = LotteryID;

        }
    }

    private void BindData()
    {
        hlAdd.NavigateUrl = "PersonagesAdd.aspx?LotteryID=" + ddlLotteries.SelectedValue;

        string Key = "Admin_Personages";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Personages().Open("", "", "[Order]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(59)");

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 6000);
        }

        DataRow[] drs = dt.Select("LotteryID=" + ddlLotteries.SelectedValue + "");

        DataTable dtData = dt.Clone();

        foreach (DataRow dr in drs)
        {
            dtData.Rows.Add(dr.ItemArray);
        }

        g.DataSource = dtData;
        g.DataBind();
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[3].FindControl("cbisShow");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[6].Text, true);
        }
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("PersonagesEdit.aspx?ID=" + e.Item.Cells[5].Text + "", true);
        }

        if (e.CommandName == "Deletes")
        {
            string id = e.Item.Cells[5].Text;

            DAL.Tables.T_Personages p = new DAL.Tables.T_Personages();

            p.Delete("ID=" + id);

            Shove._Web.Cache.ClearCache("Admin_Personages");
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Collects");
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Star");
            Shove._Web.Cache.ClearCache("DataCache_CelebrityHall_Recommends");
            BindData();
        }
    }

    protected void ddlLotteries_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
