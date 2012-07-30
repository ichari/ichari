using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Administrator_News : AdminPageBase
{
    private string Type = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDataForType();
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForType()
    {
        DataTable dt = new DAL.Tables.T_NewsTypes().Open("", "Name = 'CPS新闻公告' or Name = 'CPS推广指南'", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        ddlType.DataSource = dt;
        ddlType.DataTextField = "Name";
        ddlType.DataValueField = "ID";
        ddlType.DataBind();

        Type = Shove._Web.Utility.GetRequest("Type");

        if (Type == "2")
        {
            Shove.ControlExt.SetDownListBoxText(ddlType, "CPS推广指南");
        }
        else
        {
            Shove.ControlExt.SetDownListBoxText(ddlType, "CPS新闻公告");
        }
    }

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_News().Open("", "SiteID = " + _Site.ID.ToString() + " and TypeID = '"+ddlType.SelectedValue+"'", "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[2].FindControl("cbisShow");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[9].Text, true);

            cb = (CheckBox)e.Item.Cells[3].FindControl("cbisHasImage");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[10].Text, true);

            cb = (CheckBox)e.Item.Cells[4].FindControl("cbisCommend");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[11].Text, true);

            cb = (CheckBox)e.Item.Cells[5].FindControl("cbisHot");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[12].Text, true);
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Edit")
        {
            this.Response.Redirect("NewsEdit.aspx?ID=" + e.Item.Cells[8].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_NewsDelete(_Site.ID, long.Parse(e.Item.Cells[8].Text), ref ReturnValue, ref ReturnDescription);

            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.ClearCache("CPS_Default_BindNews");
            Shove._Web.Cache.ClearCache("CPS_News_BindNews");

            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("NewsAdd.aspx?TypeID=" + ddlType.SelectedValue, true);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
