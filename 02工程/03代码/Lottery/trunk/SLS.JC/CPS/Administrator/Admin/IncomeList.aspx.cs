using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class CPS_Administrator_Admin_IncomeList : CpsAdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hBeginTime.Value = DateTime.Now.ToShortDateString();
            tbBeginTime.Text = hBeginTime.Value;
            hEndTime.Value = DateTime.Now.ToString("yyyy-MM-dd");
            tbEndTime.Text = hBeginTime.Value;
            BindTransactionList();
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

    private void BindTransactionList()
    {
        DateTime StartTime = Shove._Convert.StrToDateTime(tbBeginTime.Text.Trim() + " 0:0:0", DateTime.Now.ToString());
        DateTime EndTime = Shove._Convert.StrToDateTime(tbEndTime.Text.Trim() + " 23:59:59", DateTime.Now.ToString());

        if (EndTime.CompareTo(StartTime) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间不能大于截止时间。");

            return;
        }

        string CacheKey = "Cps_Admin_IncomeList" + _User.ID.ToString() + this.tbBeginTime.Text + this.tbEndTime.Text;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
        
        if (dt == null)
        {
            DataSet ds = null;

            int ReturnValue = -1;
            string ReturnDescprtion = "";

            int Result = DAL.Procedures.P_GetCpsAccountRevenue(ref ds, _Site.ID, _User.ID, StartTime, EndTime, ref ReturnValue, ref ReturnDescprtion);

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

            if (ds == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 100000);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"100%\" border=\"0\" cellspacing=\"1\" cellpadding=\"0\" bgcolor=\"#CCCCCC\">")
            .Append("<tr>")
            .Append("<td width=\"19%\" height=\"30\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("时间")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("累计会员数")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("新增会员数")
            .Append("</td>")
            .Append("<td width=\"12%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("新增交易会员数")
            .Append("</td>")
            .Append("<td width=\"15%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("新增会员交易量")
            .Append("</td>")
            .Append("<td width=\"11%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("日交易量")
            .Append("</td>")
            .Append("<td width=\"9%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("佣金收入")
            .Append("</td>")
            .Append("<td width=\"10%\" align=\"center\" bgcolor=\"#ECECEC\">")
            .Append("编辑")
            .Append("</td>")
            .Append("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>")
                .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red\">")
                .Append(Shove._Convert.StrToDateTime(dr["DayTime"].ToString(),DateTime.Now.ToString()).ToString("yyyy-MM-dd"))
                .Append("</td>")
                .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(dr["TotalUserCount"].ToString())
                .Append("人</td>")
                .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(dr["DayNewUserCount"].ToString())
                .Append("人</td>")
                .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(dr["DayNewUserPayCount"].ToString())
                .Append("人</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(Shove._Convert.StrToDouble(dr["DayNewUserTradeMoney"].ToString(),0).ToString("N"))
                .Append("元</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(Shove._Convert.StrToDouble(dr["DayMemberTradeMoney"].ToString(),0).ToString("N"))
                .Append("元</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_cc\">")
                .Append(Shove._Convert.StrToDouble(dr["DayBonus"].ToString(),0).ToString("N"))
                .Append("元</td>")
                 .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red\">")
                .Append("<a href='IncomeDetail.aspx?dt=" + Shove._Convert.StrToDateTime(dr["DayTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd") + "'>收入明细</a>")
                .Append("</td>")
                .Append("</tr>");
        }

        dt = summation(dt);

        sb.Append("<tr>")
            .Append("<td height=\"28\" align=\"center\" bgcolor=\"#EAF2FE\">")
            .Append("合计")
            .Append("</td>")
            .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append("&nbsp;")
            .Append("</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(dt.Rows[0]["DayNewUserCount"].ToString())
            .Append("人</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(dt.Rows[0]["DayNewUserPayCount"].ToString())
            .Append("人</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(dt.Rows[0]["DayNewUserTradeMoney"].ToString())
            .Append("元</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
            .Append(dt.Rows[0]["DayMemberTradeMoney"].ToString())
            .Append("元</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"hui_2\">")
           .Append(dt.Rows[0]["DayBonus"].ToString())
            .Append("元</td>")
             .Append("<td height=\"28\" align=\"center\" bgcolor=\"#FFFFFF\" class=\"red\">")
            .Append("&nbsp;")
            .Append("</td>")
             .Append("</tr>")
             .Append("</table>");

        divIncomeList.InnerHtml = sb.ToString();
    }

    public DataTable summation(DataTable dt)
    {

        DataTable dt1 = new DataTable();

        dt1.Columns.Add("DayTime", Type.GetType("System.String"));
        dt1.Columns.Add("TotalUserCount", Type.GetType("System.Double"));
        dt1.Columns.Add("DayNewUserCount", Type.GetType("System.Double"));
        dt1.Columns.Add("DayNewUserPayCount", Type.GetType("System.Double"));
        dt1.Columns.Add("DayNewUserTradeMoney", Type.GetType("System.Double"));
        dt1.Columns.Add("DayMemberTradeMoney", Type.GetType("System.Double"));
        dt1.Columns.Add("DaySiteTradeMoney", Type.GetType("System.Double"));
        dt1.Columns.Add("DayBonus", Type.GetType("System.Double"));

        dt1.Columns.Add("", Type.GetType("System.String"));

        DataRow dr;

        dr = dt1.NewRow();
        dr[0] = "合计：";
        for (int col = 0; col < dt.Columns.Count; col++)
        {
            string curColName = dt.Columns[col].ColumnName;
            if (string.Compare(curColName, "DayNewUserCount", true) == 0
                || string.Compare(curColName, "DayNewUserPayCount", true) == 0
                || string.Compare(curColName, "DayNewUserTradeMoney", true) == 0
                || string.Compare(curColName, "DayMemberTradeMoney", true) == 0
                || string.Compare(curColName, "DaySiteTradeMoney", true) == 0
                || string.Compare(curColName, "DayBonus", true) == 0)
            {
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    dr[curColName] = Shove._Convert.StrToDouble(dr[curColName].ToString(), 0) + Shove._Convert.StrToDouble(dt.Rows[row][col].ToString(), 0);

                }
            }
        }
        dt1.Rows.Add(dr);

        return dt1;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindTransactionList();
    }
}
