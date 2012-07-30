using System;
using System.IO;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Web.Configuration;
using System.Text;
using System.Security.Cryptography;
using System.Net;

using Shove.Alipay;

public partial class Cps_Administrator_MonthTradeSum : AdminPageBase
{
    
    SystemOptions so = new SystemOptions();

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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        string cacheKey = "Cps_Administrator_MonthTradeSum_BindData";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);

        if (dt == null)
        {
            int returnValue=-1;
            string returnDecription="";
            DataSet ds = null;

            int Result = DAL.Procedures.P_CpsAdminAccountByMonth(ref ds, _Site.ID, ref returnValue, ref returnDecription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错:" + returnDecription);

                return;
            }

            if (ds.Tables.Count < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "出错#:" + returnDecription);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey, dt, 100000);
            
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
        Summation(dt);
    }

    public void Summation(DataTable dt)
    {
        double totalBuyMoney = 0;
        double totalBouns = 0;

        for (int row = 0; row < dt.Rows.Count; row++)
        {
            totalBuyMoney += Shove._Convert.StrToDouble(dt.Rows[row]["BuyMoney"].ToString(), 0);
            totalBouns += Shove._Convert.StrToDouble(dt.Rows[row]["PayBonus"].ToString(), 0);
        }

        lbTotalBuyMoney.Text = totalBuyMoney.ToString("f2");
        lbTotalBonus.Text = totalBouns.ToString("f2");
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
