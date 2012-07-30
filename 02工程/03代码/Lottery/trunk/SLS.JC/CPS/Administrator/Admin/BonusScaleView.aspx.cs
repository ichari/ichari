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

public partial class CPS_Administrator_Admin_BonusScaleView : CpsAdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
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

    private void BindData()
    {
        long Cpsid = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("ID")), -1);
        Cps curCps = new Cps();
        curCps.SiteID = _Site.ID;
        curCps.ID = Cpsid;
        string returndescription = string.Empty;
        curCps.GetCpsInformationByID(ref returndescription);

        string cacheKeyScaleList = "Cps_Admin_BonusScaleView_ScaleList_" + curCps.OwnerUserID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyScaleList);
     
        if (dt == null )
        {
            int returnValue=-1;
            string returnDescription="";
            DataSet ds = null;
            int Result = DAL.Procedures.P_CpsGetUserBonusScaleList(ref ds, curCps.OwnerUserID, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "系统出错!");

                return;

            }
            if (ds == null || ds.Tables.Count < 1)
            {
                Shove._Web.JavaScript.Alert(this.Page, "获取数据出错!");

                return;
            }
            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKeyScaleList, dt, 100000);
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    { 
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
