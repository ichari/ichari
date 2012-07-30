using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Administrator_Admin_IncomeListByMonth : CpsAdminPageBase
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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Administrator/Admin/Default.aspx";
        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        //获取数据
        string Key = "Cps_Admin_IncomeListByMonth_" + _User.cps.ID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);
        
        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;

            int Result = DAL.Procedures.P_CpsGetIncomeListByMonth(ref ds, _Site.ID, _User.cps.ID, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
                return;
            }

            if (returnValue < 0 || ds == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }

            dt = ds.Tables[0];
           
            Shove._Web.Cache.SetCache(Key, dt, 100000);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"100%\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\" bgcolor=\"#CCCCCC\">")
            .Append("<tr>")
            .Append("<td width=\"19%\" height=\"30\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("时间")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("交易量")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("收入金额")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("收入明细")
            .Append("</td>")
            .Append("<td width=\"15%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("备注")
            .Append("</td>")
            .Append("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            DateTime startDay = Convert.ToDateTime(dr["YearMonth"].ToString() + "-01");    //月头
            DateTime endDay = startDay.AddMonths(1).AddDays(-1);          //月尾

            sb.Append("<tr>")
                .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"black12\">")
                .Append(dr["YearMonth"].ToString())
                .Append("</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"black12\">")
                .Append(Shove._Convert.StrToDouble(dr["BuyMoney"].ToString(),0).ToString("N"))
                .Append("元</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"black12\">")
                .Append(Shove._Convert.StrToDouble(dr["Bonus"].ToString(),0).ToString("N"))
                .Append("元</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red\">")
                .Append("<a href='CpsBuyDetail.aspx?ID="+_User.cps.ID.ToString()+"&StartTime=" + startDay.ToString("yyyy-MM-dd") + "&EndTime=" + endDay.ToString("yyyy-MM-dd")+"'>详情</a>")
                .Append("</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red\">")
                .Append(dr["Memo"].ToString())
                .Append("</td>")
                .Append("</tr>");
        }

        double totalBuyMoney = 0;
        double totalBouns = 0;

        foreach (DataRow dr in dt.Rows)
        {
            totalBuyMoney += Shove._Convert.StrToDouble(dr["BuyMoney"].ToString(), 0);
            totalBouns += Shove._Convert.StrToDouble(dr["Bonus"].ToString(), 0);
        }

        sb.Append("<tr>")
            .Append("<td height=\"28\" align=\"center\" bgcolor=\"#EAF2FE\">")
            .Append("合计")
            .Append("</td>")
            .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(totalBuyMoney.ToString("N"))
            .Append("元</td>")
            .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(totalBouns.ToString("N"))
            .Append("元</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">&nbsp;</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">&nbsp;</td>")
             .Append("</tr>")
             .Append("</table>");

        tdList.InnerHtml = sb.ToString();
    }

}
