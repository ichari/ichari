using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Shove.Database;

public partial class JCZC_Client_SPF_DG : System.Web.UI.Page
{
    protected string MatchList = "";
    protected string lgList = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Shove._Web.Utility.GetRequest("id").ToString() == "1")
            {
                Response.Redirect("Buy_ZJQS.aspx");
            }
            if (Shove._Web.Utility.GetRequest("id").ToString() == "2")
            {
                Response.Redirect("Buy_ZQBF.aspx");
            }
            if (Shove._Web.Utility.GetRequest("id").ToString() == "3")
            {
                Response.Redirect("Buy_BQC.aspx");
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
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Matchs_72_IsHhad_dg");

        if (dt == null || dt.Rows.Count == 0)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            dt = Shove.Database.MSSQL.Select("SELECT * FROM [T_singleRate]  where IsHhad = 1 and StopSellTime > GETDATE() order by [Day], MatchNumber");

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
                Shove._Web.Cache.SetCache("DataCache_Matchs_72_IsHhad_dg", dt, 600);
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

        int RqCount = 0;
        int NoRqCount = 0;

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
            string Day = "";
            string OldDay = "";

            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    //获取开赛时间及停售时间
                    date = Shove._Convert.StrToDateTime(dr["StopSellTime"].ToString(), DateTime.Now.ToString());
                    saleEndDate = Shove._Convert.StrToDateTime(dr["StopSellTime"].ToString(), DateTime.Now.ToString());
                    Day = dr["Day"].ToString();

                    if (!OldDay.Equals(Day))
                    {
                        if (!string.IsNullOrEmpty(OldDay))
                        {
                            sb.Append("</tbody>");
                        }

                        sb.AppendLine("<tr class=\"form_tr5\">")
                        .AppendLine("<td colspan=\"16\" height=\"24\"><a style=\"color: rgb(102, 0, 0); cursor: pointer;\" onclick=\"opendate('" + Day.Insert(6, "-").Insert(4, "-") + "')\"><span class=\"bold\">" + Day.Insert(6, "-").Insert(4, "-") + getWeekDayName(Shove._Convert.StrToDateTime(Day.Insert(6, "-").Insert(4, "-"), DateTime.Now.ToString()).DayOfWeek) + "</span><span id=\"img_" + Day.Insert(6, "-").Insert(4, "-") + "\" alt=\"点击隐藏\" class=\"s_hidden\">点击隐藏 </span></td>")
                        .AppendLine("</tr><tbody id=\"d_" + Day.Insert(6, "-").Insert(4, "-") + "\">");
                    }

                    OldDay = Day;

                    //开始构造赛事行
                    lineStyle = m % 2 == 0 ? "form_tr4" : "form_tr4_2";

                    sb.Append("<tr class='" + lineStyle + "' zid=\"" + dr["MatchID"].ToString() + "\" mid=\"" + dr["MatchID"].ToString() + "\" pdate=\"" + saleEndDate.ToString("yyyy-MM-dd") + "\" pname=\"" + (((int)startDate.DayOfWeek) == 0 ? 7 : ((int)startDate.DayOfWeek)).ToString() + dr["MatchNumber"].ToString().Substring(dr["MatchNumber"].ToString().Length - 3) + "\" lg=\"" + dr["Game"].ToString() + "\" rq=\"" + dr["mainloseball"].ToString() + "\" win=\"" + dr["Win"].ToString() + "\" draw=\"" + dr["Flat"].ToString() + "\" lost=\"" + dr["Lose"].ToString() + "\">")
                        .AppendLine("<td><input name=\"m" + dr["MatchID"].ToString() + "\" type=\"checkbox\" value=\"" + dr["MatchID"].ToString() + "\" checked=\"checked\"/>" + dr["MatchNumber"].ToString() + "</td>")
                        .Append("<td class=\"team1\" bgcolor=\"" + dr["GameColor"].ToString() + "\"><font color=\"#ffffff\">" + dr["Game"].ToString() + "</font></a></td>")
                        .AppendLine("<td>" + saleEndDate.ToString("MM-dd HH:mm") + "</td>")
                        .AppendLine("<td>" + dr["MainTeam"].ToString() + "</td>");

                    if (dr["mainloseball"].ToString().IndexOf("-") >= 0)
                    {
                        sb.AppendLine("<td class=\"green\">");
                    }
                    else if (Shove._Convert.StrToInt(dr["mainloseball"].ToString(), 0) > 0)
                    {
                        sb.AppendLine("<td class=\"red\">");
                    }
                    else
                    {
                        sb.AppendLine("<td>");
                    }

                    sb.AppendLine( dr["mainloseball"].ToString() + "</td>")
                      .AppendLine("<td>" + dr["GuestTeam"].ToString() + "</td>")
                     .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropeSSP"].ToString(), 0).ToString("F2") + "</td>")
                     .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropePSP"].ToString(), 0).ToString("F2") + "</td>")
                     .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropeFSP"].ToString(), 0).ToString("F2") + "</td>")
                     .AppendLine("<td><a href=\"http://hbty.shovesoft.com/zc/sls_MatchReportLastMatch.aspx?MatchID=" + dr["InforMationID"].ToString() + "\" target=\"_blank\">析</a>")
                     .AppendLine("<a href=\"http://hbty.shovesoft.com/zc/sls_MatchReportHandicap.aspx?MatchID=" + dr["InforMationID"].ToString() + "\" target=\"_blank\">亚</a>")
                     .AppendLine("<a href=\"http://hbty.shovesoft.com/zc/sls_MatchReportLatestOdds.aspx?MatchID=" + dr["InforMationID"].ToString() + "\" target=\"_blank\">欧</a></td>")
                     .AppendLine("<td class=\"sp_bg\" align=\"center\" style=\"cursor:pointer\">&nbsp;<input type=\"checkbox\" value=\"3\" /><span>" + (Shove._Convert.StrToDouble(dr["Win"].ToString(), 0) > 0 ? Shove._Convert.StrToDouble(dr["Win"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                     .AppendLine("<td class=\"sp_bg\" align=\"center\" style=\"cursor:pointer\">&nbsp;<input type=\"checkbox\" value=\"1\" /><span>" + (Shove._Convert.StrToDouble(dr["Flat"].ToString(), 0) > 0 ? Shove._Convert.StrToDouble(dr["Flat"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                     .AppendLine("<td class=\"sp_bg\" align=\"center\" style=\"cursor:pointer\">&nbsp;<input type=\"checkbox\" value=\"0\" /><span>" + (Shove._Convert.StrToDouble(dr["Lose"].ToString(), 0) > 0 ? Shove._Convert.StrToDouble(dr["Lose"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                     .AppendLine("<td style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" /></td>")
                     .AppendLine("</tr>");

                    if (strlgList.IndexOf(dr["Game"].ToString()) < 0)
                    {
                        strlgList += "<li><input id=\"lg" + dr["Game"].ToString() + "\" m=\"" + dr["Game"].ToString() + "\" type=\"checkbox\" checked=\"checked\"><label for=\"lg" + dr["Game"].ToString() + "\">" + dr["Game"].ToString() + "</label></li>";
                    }

                    if (Shove._Convert.StrToInt(dr["mainloseball"].ToString(), 0) == 0)
                    {
                        NoRqCount += 1;
                    }
                    else
                    {
                        RqCount += 1;
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

            lbShowRq.Text = "让球(" + RqCount.ToString() + "场)";

            lbShowRoRaq.Text = "非让球(" + NoRqCount.ToString() + "场)";

            strlgList += "</ul>";
            sb.AppendLine("</tbody>");
            strHTML = sb.ToString();
            //noMatch.Value = i.ToString();
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
        DataTable table = play.Open("SystemEndAheadMinute", "id=7201", "");

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
