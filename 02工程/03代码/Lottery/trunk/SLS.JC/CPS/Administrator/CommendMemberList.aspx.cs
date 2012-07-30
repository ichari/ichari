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

public partial class Cps_Administrator_CommendMemberList: AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long id = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);

            if (id < 1 )
            {
                Shove._Web.JavaScript.Alert(this.Page,"参数无效!");

                return;
            }

            hfID.Value = id.ToString();

            Users commender = new Users(_Site.ID);
            commender.ID = id;
            lblName.Text = commender.Name;

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

    private void BindData()
    {
        long id = Shove._Convert.StrToLong(hfID.Value, -1);
        if (id < 1  )
        {
            Shove._Web.JavaScript.Alert(this.Page, "无效的参数列表.");

            return;
        }

        //获取数据
        string cacheKey = "Cps_Administrator_CpsUsersList_" + id.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);

        if (dt == null)
        {
            dt = new DAL.Views.V_Users().Open("", "CommenderID = " + id.ToString() + " and not exists (select 1 from T_Cps where V_Users.ID =T_Cps .OwnerUserID )", "[RegisterTime]");
            Shove._Web.Cache.SetCache(cacheKey, dt, 100000);
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (hfFilterExpress.Value != "")
        {
            DataView dv = new DataView(dt, hfFilterExpress.Value, "Name", DataViewRowState.OriginalRows);
            PF.DataGridBindData(g, dv, gPager);
        }
        else
        {
            PF.DataGridBindData(g, dt, gPager);
        }       
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[8].Text = e.Item.Cells[8].Text.Trim() == "2" ? "<font color='red'>高级</font>" : e.Item.Cells[8].Text.Trim() == "3" ? "<font color='blue'>VIP</font>" : "普通";
        }
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        hfFilterExpress.Value = "";
        if (tbUserName.Text.Trim() != "")
        {
            hfFilterExpress.Value = "Name like '" + tbUserName.Text + "%'";
        }

        BindData();
    }
}
