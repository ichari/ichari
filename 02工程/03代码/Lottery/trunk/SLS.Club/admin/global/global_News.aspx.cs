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
using Discuz.Web.Admin;

public partial class admin_global_global_News : AdminPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            
           BindDataForType();
            

            BindData();
            //btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.EditNews));
            btnAdd.Visible = (usergroupid == 1);
            g.Columns[7].Visible = btnAdd.Visible;
        }
    }
    /// <summary>
    /// 绑定新闻类型树
    /// </summary>
    private void BindDataForType()
    {
        DataTable dt = new DAL.Tables.T_NewsTypes().Open("", "SiteID = 1" , "[ID]");

        //if (dt == null)
        //{
        //    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

        //    return;
        //}

        tv.DataTable = dt;
        tv.DataBind();

        foreach (TreeNode tn in tv.Nodes)
        {
            tn.NavigateUrl = "";
        }

        string TypeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("TypeID"), -1).ToString();

        if (TypeID != "-1")
        {
            Shove.ControlExt.SetTreeViewSelectedFromValue(tv, TypeID);
        }
        else if (tv.Nodes.Count > 0)
        {
            tv.Nodes[0].Select();
        }
    }

    private void BindData()
    {
        if (tv.Nodes.Count < 1)
        {
            return;
        }

        string TypeID = tv.SelectedValue;

        if (TypeID == "")
        {
            TypeID = tv.Nodes[0].Value;
        }

        DataTable dt = new DAL.Tables.T_News().Open("", "SiteID = 1 and TypeID = " + Shove._Web.Utility.FilteSqlInfusion(TypeID), "[DateTime] desc");

        //if (dt == null)
        //{
        //    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

        //    return;
        //}



        DataGridBindData(g, dt, gPager);
    }

    public static void DataGridBindData(DataGrid g, DataTable dt, Shove.Web.UI.ShoveGridPager gPager)
    {
        g.DataSource = dt;

        try
        {
            g.DataBind();
        }
        catch
        {
            g.CurrentPageIndex = 0;
            gPager.PageIndex = 0;

            g.DataBind();
        }

        gPager.Visible = (dt.Rows.Count > 0);
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
            this.Response.Redirect("global_News_Edit.aspx?TypeID=" + tv.SelectedValue + "&ID=" + e.Item.Cells[8].Text, true);
            return;
        }

        if (e.CommandName == "Del")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_NewsDelete(1, long.Parse(e.Item.Cells[8].Text), ref ReturnValue, ref ReturnDescription);

            //if (Results < 0)
            //{
            //    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            //    return;
            //}

            //if (ReturnValue < 0)
            //{
            //    PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

            //    return;
            //}


            BindData();

            return;
        }
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        this.Response.Redirect("global_News_Add.aspx?TypeID=" + tv.SelectedValue, true);
       
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void tv_SelectedNodeChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
