using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Shove.Database;

public partial class JCLC_Client_SFC : System.Web.UI.Page
{
    protected string MatchList = "";
    protected string lgList = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            switch (Shove._Web.Utility.GetRequest("id").ToString())
            {
                case "0":
                    Response.Redirect("Buy_SF.aspx");
                    break;
                case "1":
                    Response.Redirect("Buy_RFSF.aspx");
                    break;
                case "2":
                    Response.Redirect("Buy_SFC.aspx");
                    break;
                case "3":
                    Response.Redirect("Buy_DX.aspx");
                    break;
                default:
                    break;
            }

            GetMatchsByIssueName();
        }
    }

    // 根据期号加载赛事信息
    private void GetMatchsByIssueName()
    {
        //第一步：获取比赛
        DataTable matchs = getMatchs();

        //第二步：生成HTML+场次数据
        int jzCount = 0;
        string strHTML = "";
        string strlgList = "";

        createHTML(matchs, ref strHTML, ref jzCount, ref strlgList);

        //jz_changci.Value = jzCount.ToString();

        MatchList = strHTML;
        lgList = strlgList;
    }

    //获取比赛
    private DataTable getMatchs()
    {
        //赛事缓存30分钟
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Matchs_73_IsWnm");

        if (dt == null || dt.Rows.Count == 0)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            dt = Shove.Database.MSSQL.Select("SELECT * FROM [T_PassRateBasket]  where IsWnm = 1 and StopSellTime > GETDATE() order by [Day], MatchNumber");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(128)", this.GetType().BaseType.FullName);

                return null;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return null;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Matchs_73_IsWnm", dt, 600);
            }

        }

        return dt;
    }

    //生成HTML
    private void createHTML(DataTable dt, ref string strHTML, ref int jzCount, ref string strlgList)
    {
        //初始化
        jzCount = 0;
        strHTML = "";
        strlgList = "<ul id=\"lgList\">";

        if (dt.Rows != null && dt.Rows.Count > 0)
        {
            //变量
            StringBuilder sb = new StringBuilder(); //构造HTML
            int i = 0;                              //未截止的赛事计数器
            int m = 0;                              //赛事计数器
            DateTime startDate = DateTime.Now;      //开始时间(分组的开始时间)
            DateTime endDate = DateTime.Now;        //结束时间(分组的结束时间)
            DateTime date = DateTime.Now;           //赛事开始时间
            DateTime saleEndDate = DateTime.Now;      //销售截止时间
            string lineStyle = "";                  //行样式

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //获取开赛时间及停售时间
                    date = Shove._Convert.StrToDateTime(dr["StopSellTime"].ToString(), DateTime.Now.ToString());
                    saleEndDate = Shove._Convert.StrToDateTime(dr["StopSellTime"].ToString(), DateTime.Now.ToString());

                    //分组行的构造
                    if (m == 0)
                    {
                        if (string.Compare(date.ToString("HH:mm"), "12:00") >= 0)
                        {
                            startDate = Shove._Convert.StrToDateTime(date.ToString("yyyy-MM-dd") + " 12:00:00", DateTime.Now.ToString());
                            endDate = Shove._Convert.StrToDateTime(date.AddDays(1).ToString("yyyy-MM-dd") + " 12:00:00", DateTime.Now.ToString());
                        }
                        else
                        {
                            startDate = Shove._Convert.StrToDateTime(date.AddDays(-1).ToString("yyyy-MM-dd") + " 12:00:00", DateTime.Now.ToString());
                            endDate = Shove._Convert.StrToDateTime(date.ToString("yyyy-MM-dd") + " 12:00:00", DateTime.Now.ToString());
                        }

                        sb.AppendLine("<tr class=\"form_tr5\">")
                        .AppendLine("<td colspan=\"14\" height=\"24\"><a style=\"color: rgb(102, 0, 0); cursor: pointer;\" onclick=\"opendate('" + startDate.ToString("yyyy-MM-dd") + "')\"><span class=\"bold\">" + startDate.ToString("yyyy-MM-dd") + getWeekDayName(startDate.DayOfWeek) + "</span><span id=\"img_" + startDate.ToString("yyyy-MM-dd") + "\" alt=\"点击隐藏\" class=\"s_hidden\">点击隐藏 </span></td>")
                        .AppendLine("</tr><tbody id=\"d_" + startDate.ToString("yyyy-MM-dd") + "\">");
                    }
                    else if (date > endDate)
                    {
                        startDate = startDate.AddDays((date - endDate).Days + 1);
                        endDate = endDate.AddDays((date - endDate).Days + 1);

                        sb.AppendLine("</tbody><tr class=\"form_tr5\">")
                        .AppendLine("<td colspan=\"14\" height=\"24\"><a style=\"color: rgb(102, 0, 0); cursor: pointer;\" onclick=\"opendate('" + startDate.ToString("yyyy-MM-dd") + "')\"><span class=\"bold\">" + startDate.ToString("yyyy-MM-dd") + getWeekDayName(startDate.DayOfWeek) + "</span><span id=\"img_" + startDate.ToString("yyyy-MM-dd") + "\" alt=\"点击隐藏\" class=\"s_hidden\">点击隐藏 </span></td>")
                        .AppendLine("</tr><tbody id=\"d_" + startDate.ToString("yyyy-MM-dd") + "\">");
                    }

                    //开始构造赛事行
                    lineStyle = m % 2 == 0 ? "form_tr4" : "form_tr4_2";

                    sb.Append("<tr class='" + lineStyle + "' zid=\"" + dr["MatchID"].ToString() + "\" mid=\"" + dr["MatchID"].ToString() + "1\" pdate=\"" + saleEndDate.ToString("yyyy-MM-dd") + "\"  lg=\"" + dr["Game"].ToString() + "\" odds=" + dr["DifferMain1_5"].ToString() + "," + dr["DifferMain6_10"].ToString() + "," + dr["DifferMain11_15"].ToString() + "," + dr["DifferMain16_20"].ToString() + "," + dr["DifferMain21_25"].ToString() + "," + dr["DifferMain26"].ToString() + "," + dr["DifferGuest1_5"].ToString() + "," + dr["DifferGuest6_10"].ToString() + "," + dr["DifferGuest11_15"].ToString() + "," + dr["DifferGuest16_20"].ToString() + "," + dr["DifferGuest21_25"].ToString() + "," + dr["DifferGuest26"].ToString() + "\">")
                        .AppendLine("<td id=\"pltr_" + dr["MatchID"].ToString() + "1\" rowspan=\"2\"><input name=\"m" + dr["MatchID"].ToString() + "\" type=\"checkbox\" value=\"" + dr["MatchID"].ToString() + "\" checked=\"checked\"/>" + dr["MatchNumber"].ToString() + "</td>")
                        .Append("<td class=\"team1\" bgcolor=\"#FF0000\" rowspan=\"2\"><font color=\"#ffffff\">" + dr["Game"].ToString() + "</font></td>")
                        .AppendLine("<td rowspan=\"2\">" + saleEndDate.ToString("MM-dd HH:mm") + "</td>")
                        .AppendLine("<td rowspan=\"2\">" + dr["GuestTeam"].ToString() + "</td>")
                        .AppendLine("<td rowspan=\"2\">" + dr["MainTeam"].ToString() + "</td>")
                        .AppendLine("<td>客胜</td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"1\" /><span>" + dr["DifferGuest1_5"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"3\" /><span>" + dr["DifferGuest6_10"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"5\" /><span>" + dr["DifferGuest11_15"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"7\" /><span>" + dr["DifferGuest16_20"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"9\" /><span>" + dr["DifferGuest21_25"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"11\" /><span>" + dr["DifferGuest26"].ToString() + "</span></td>")
                        .AppendLine("<td style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" /></td>")
                        .AppendLine("</tr>")
                        .AppendLine("<tr class=\"form_tr4\" c=\"c\" id=\"pltr_" + dr["MatchID"].ToString() + "\" >")
                        .AppendLine("<td>主胜</td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"2\" /><span>" + dr["DifferMain1_5"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"4\" /><span>" + dr["DifferMain6_10"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"6\" /><span>" + dr["DifferMain11_15"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"8\" /><span>" + dr["DifferMain16_20"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"10\" /><span>" + dr["DifferMain21_25"].ToString() + "</span></td>")
                        .AppendLine("<td class=\"sp_bg\" style=\"cursor:pointer; text-align:left\">&nbsp;<input type=\"checkbox\" value=\"12\" /><span>" + dr["DifferMain26"].ToString() + "</span></td>")
                        .AppendLine("<td style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" /></td>")
                        .AppendLine("</tr>");

                    if (strlgList.IndexOf(dr["Game"].ToString()) < 0)
                    {
                        strlgList += "<li><input id=\"lg" + dr["Game"].ToString() + "\" m=\"" + dr["Game"].ToString() + "\" type=\"checkbox\" checked=\"checked\"><label for=\"lg" + dr["Game"].ToString() + "\">" + dr["Game"].ToString() + "</label></li>";
                    }

                    i++;
                    m++;

                }
            }
            catch (Exception ex)
            {
                new Log("TWZT").Write(this.GetType() + ex.Message);

                strHTML = "";
            }

            strlgList += "</ul>";
            sb.AppendLine("</tbody>");
            strHTML = sb.ToString();
        }
    }

    /// <summary>
    /// 得到截止时间
    /// </summary>
    /// <param name="date"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetStopSelltime(string date)
    {
        double time = 0;
        DAL.Tables.T_PlayTypes play = new DAL.Tables.T_PlayTypes();
        DataTable table = play.Open("SystemEndAheadMinute", "id=7301", "");

        if (table == null)
        {
            return date;
        }

        if (table.Rows.Count > 0)
        {
            time = double.Parse(table.Rows[0]["SystemEndAheadMinute"].ToString());
            date = Shove._Convert.StrToDateTime(date, DateTime.Now.ToString()).AddMinutes(-time).ToString("yy-MM-dd HH:mm");
        }

        return date;
    }

    // 返回周几的对应中文名
    private string getWeekDayName(DayOfWeek weekday)
    {
        string weekDayName = "";
        switch (weekday)
        {
            case DayOfWeek.Friday:
                weekDayName = "星期五";
                break;
            case DayOfWeek.Monday:
                weekDayName = "星期一";
                break;
            case DayOfWeek.Saturday:
                weekDayName = "星期六";
                break;
            case DayOfWeek.Sunday:
                weekDayName = "星期日";
                break;
            case DayOfWeek.Thursday:
                weekDayName = "星期四";
                break;
            case DayOfWeek.Tuesday:
                weekDayName = "星期二";
                break;
            case DayOfWeek.Wednesday:
                weekDayName = "星期三";
                break;
        }

        return weekDayName;

    }

}
