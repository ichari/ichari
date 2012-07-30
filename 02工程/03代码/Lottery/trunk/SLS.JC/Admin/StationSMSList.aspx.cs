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

public partial class Admin_StationSMSList : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            g.Columns[4].Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.SendMessage));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SendMessage,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_StationSMS().Open("", "SiteID = " + _Site.ID.ToString(), "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_StationSMSList");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemCreated(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[5].FindControl("cbisShow");
            cb.CheckedChanged += new System.EventHandler(this.g_ItemCheckedChanged);
        }
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[5].FindControl("cbisShow");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[7].Text, true);
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Del")
        {
            long id = Shove._Convert.StrToLong(e.Item.Cells[6].Text, 0);

            if (new DAL.Tables.T_StationSMS().Delete("ID = " + id.ToString()) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_StationSMSList");

                return;
            }

            BindData();

            return;
        }
    }

    protected void g_ItemCheckedChanged(object sender, System.EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        DataGridItem item = (DataGridItem)cb.Parent.Parent;
        long id = Shove._Convert.StrToLong(item.Cells[6].Text, 0);

        DAL.Tables.T_StationSMS T_StationSMS = new DAL.Tables.T_StationSMS();

        T_StationSMS.isShow.Value = cb.Checked;

        if (T_StationSMS.Update("ID = " + id.ToString()) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_StationSMSList");

            return;
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
