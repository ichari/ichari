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

public partial class Cps_Administrator_PerDayTradeSumOfMonthByType : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string strYearMonth = Shove._Web.Utility.GetRequest("YearMonth");                       //当前查看的年月份,格式:2009-01
           
            DateTime temp;
            if (!DateTime.TryParse(strYearMonth, out  temp))
            {
                Shove._Web.JavaScript.Alert(this.Page ,"参数错误!");

                return;
            }
            else
            {
                hfYearMonth.Value = temp.ToString("yyyy-MM");
              
                lblMonth.Text = temp.ToString("yyyy年MM份");
            }

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
        DateTime viewMonthDate=Shove._Convert.StrToDateTime(hfYearMonth.Value+"-01", "");

        string cacheKeyTradeData = "Cps_Administrator_PerDayTradeSumOfMonthByType_Data_" + "_" + viewMonthDate.ToString("yyyyMMdd");
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKeyTradeData);

        if (dt == null)
        {
            int returnValue=-1;
            string returnDescription="";
            DataSet ds = null;

            int Result = DAL.Procedures.P_CpsGetPerDayTradeSumOfMonthByType(ref ds, viewMonthDate, ref returnValue, ref returnDescription);

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
            Shove._Web.Cache.SetCache(cacheKeyTradeData, dt, 100000);
        }

        g.DataSource = dt;
        g.DataBind();
    }
}
