using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_User : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        string sql = @"select a.ID,Name,RealityName,IDCardNumber,Email,QQ,Telephone,Mobile,RegisterTime,BankName,BankCardNumber from  T_Users a 
  where   RegisterTime between '2009-10-01 00:00:00' and '2009-10-08 23:59:59' and ID in(select UserID from T_UserPayDetails where Money >= 60 and DateTime  between '2009-10-01 00:00:00' and '2009-10-08 23:59:59' and Result = 1)";

        DataTable dt = Shove.Database.MSSQL.Select(sql);

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=1&ID=" + e.Item.Cells[10].Text + "'>" + e.Item.Cells[0].Text + "</a>";
        }
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
