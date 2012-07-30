using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Administrator_Admin_PromoterList : CpsAdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindPromoterList();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Administrator/Admin/Default.aspx";
        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindPromoterList()
    {
        string CacheKey = "CPS_Admin_PromoterList" + _User.cps.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            DataSet ds = null;

            int ReturnValue = -1;
            string ReturnDescprtion = "";

            int Result = DAL.Procedures.P_GetCpsAccountWithMonth(ref ds, DateTime.Now, DateTime.Now, "", _User.cps.ID, ref ReturnValue, ref ReturnDescprtion);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescprtion);

                return;
            }

            if (ds == null || ds.Tables.Count < 1)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(CacheKey, dt, 100000);
        }

        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindPromoterList();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindPromoterList();
    }
}
