using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Cps_Administrator_CpsSiteBuyDetail: AdminPageBase 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //处理请求查看的日期范围
            DateTime fromDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);//默认为今日
            DateTime toDateTime = DateTime.Now;
            if (Request["FromDay"] != null && Request["ToDay"] != null)
            {
                fromDateTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("FromDay") + " 0:0:0", DateTime.Now.ToString());
                toDateTime = Shove._Convert.StrToDateTime(Shove._Web.Utility.GetRequest("ToDay") + " 23:59:59", DateTime.Now.ToString());
            }

            tbBeginTime.Text = fromDateTime.ToString("yyyy-MM-dd");
            tbEndTime.Text = toDateTime.ToString("yyyy-MM-dd");

            //绑定数据
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
        //获取当前查看的CPS ID参数
        long cpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), -1);
        if (cpsID < 1)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "参数错误", this.GetType().BaseType.FullName);
            return;
        }
       
        //处理日期
        DateTime fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);// 默认查询本月明细
        DateTime toDate = DateTime.Now;
        if (tbBeginTime.Text != "" && tbEndTime.Text != "")
        {
            fromDate = Shove._Convert.StrToDateTime( tbBeginTime.Text.Trim() + " 0:0:0", DateTime.Now.ToString());
            toDate = Shove._Convert.StrToDateTime( tbEndTime.Text.Trim() + " 23:59:59", DateTime.Now.ToString());
        }

        tbBeginTime.Text = fromDate.ToString("yyyy-MM-dd");
        tbEndTime.Text = toDate.ToString("yyyy-MM-dd");

        //获取数据
        string key = "Cps_Administrator_CpsSiteBuyDetail_" + cpsID + tbBeginTime.Text + tbEndTime.Text;
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(key);
        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_GetCpsBuyDetailByDate(ref ds, _Site.ID, cpsID, fromDate, toDate, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(key, dt, 100000);

        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if(!Shove._String.Valid.isDate(tbBeginTime.Text) ||!Shove._String.Valid.isDate(tbEndTime.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输正确的查询日期格式:2008-01-01");

            return;
        }

        BindData();
    }

    protected void ddlBuyFromSystem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();

    }
}
