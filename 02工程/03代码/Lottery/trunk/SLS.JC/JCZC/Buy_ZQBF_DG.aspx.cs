using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using Shove.Database;

public partial class JCZC_Buy_ZQBF_DG : System.Web.UI.Page
{
    protected string MatchList = "";
    protected string lgList = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
        	 Response.Expires = 0;
            Response.Buffer = true;
            Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
            Response.AddHeader("pragma", "no-cache");
            Response.CacheControl = "no-cache";
            
            GetMatchsByIssueName();
            GetSchemeBonusScalec();
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
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Matchs_72_IsCrs_dg");

        if (dt == null || dt.Rows.Count == 0)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            dt = Shove.Database.MSSQL.Select("SELECT * FROM [T_singleRate]  where IsCrs = 1 and DATEADD(minute, (select SystemEndAheadMinute from T_PlayTypes where id = 7202) * -1, StopSellTime) > GETDATE() order by [Day], MatchNumber");

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
                Shove._Web.Cache.SetCache("DataCache_Matchs_72_IsCrs_dg", dt, 0);
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

        DAL.Tables.T_PlayTypes play = new DAL.Tables.T_PlayTypes();

        System.Data.DataTable table = play.Open("SystemEndAheadMinute", "id=7202", "");

        if (table == null)
        {
            return;
        }

        double time = 0;

        if (table.Rows.Count > 0)
        {
            time = double.Parse(table.Rows[0]["SystemEndAheadMinute"].ToString());
        }

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

                    sb.Append("<tr class='" + lineStyle + "' zid=\"" + dr["MatchID"].ToString() + "\" mid=\"" + dr["MatchID"].ToString() + "\" pdate=\"" + OldDay.Insert(6, "-").Insert(4, "-") + "\" pname=\"" + (((int)startDate.DayOfWeek) == 0 ? 7 : ((int)startDate.DayOfWeek)).ToString() + dr["MatchNumber"].ToString().Substring(dr["MatchNumber"].ToString().Length - 3) + "\" lg=\"" + dr["Game"].ToString() + "\" odds=\"" + dr["Sother"].ToString() + "," + dr["S10"].ToString() + "," + dr["S20"].ToString() + "," + dr["S21"].ToString() + "," + dr["S30"].ToString() + "," + dr["S31"].ToString() + "," + dr["S32"].ToString() + "," + dr["S40"].ToString() + "," + dr["S41"].ToString() + "," + dr["S42"].ToString() + "," + dr["S50"].ToString() + "," + dr["S51"].ToString() + "," + dr["S52"].ToString() + "," + dr["Pother"].ToString() + "," + dr["P00"].ToString() + "," + dr["P11"].ToString() + "," + dr["P22"].ToString() + "," + dr["P33"].ToString() + "," + dr["Fother"].ToString() + "," + dr["F01"].ToString() + "," + dr["F02"].ToString() + "," + dr["F12"].ToString() + "," + dr["F03"].ToString() + "," + dr["F13"].ToString() + "," + dr["F23"].ToString() + "," + dr["F04"].ToString() + "," + dr["F14"].ToString() + "," + dr["F24"].ToString() + "," + dr["F05"].ToString() + "," + dr["F15"].ToString() + "," + dr["F25"].ToString() + "\">")
                         .AppendLine("<td><input name=\"m" + dr["MatchID"].ToString() + "\" type=\"checkbox\" value=\"" + dr["MatchID"].ToString() + "\" checked=\"checked\"/>" + dr["MatchNumber"].ToString() + "</td>")
                        .Append("<td class=\"team1\" bgcolor=\"" + dr["GameColor"].ToString() + "\"><font color=\"#FFFFFF\">" + dr["Game"].ToString() + "</font></td>")
                        .AppendLine("<td>" + saleEndDate.AddMinutes(time * -1).ToString("MM-dd HH:mm") + "</td>")
                        .AppendLine("<td>" + dr["MainTeam"].ToString() + "</td>")
                        .AppendLine("<td>" + dr["GuestTeam"].ToString() + "</td>")
                        .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropeSSP"].ToString(), 0).ToString("F2") + "</td>")
                        .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropePSP"].ToString(), 0).ToString("F2") + "</td>")
                        .AppendLine("<td class=\"odds\">" + Shove._Convert.StrToDouble(dr["EuropeFSP"].ToString(), 0).ToString("F2") + "</td>")
                        .AppendLine("<td>析 亚 欧</td>");

                    if (m == 0)
                    {
                        sb.Append("<td class=\"sp\"><a onclick=\"openclose(" + dr["MatchID"].ToString() + ")\"><img src=\"images/btn_sp.gif\" alt=\"隐藏选项\" id=\"img_" + dr["MatchID"].ToString() + "\" /></a></td>")
                            .Append("</tr>")
                            .Append("<tr id=\"pltr_" + dr["MatchID"].ToString() + "\">");
                    }
                    else
                    {
                        sb.Append("<td class=\"sp\"><a onclick=\"openclose(" + dr["MatchID"].ToString() + ")\"><img src=\"images/btn_spadd.gif\" alt=\"展开选项\" id=\"img_" + dr["MatchID"].ToString() + "\" /></a></td>")
                            .Append("</tr>")
                            .Append("<tr id=\"pltr_" + dr["MatchID"].ToString() + "\" style=\"display:none;\">");
                    }

                    sb.Append("<td colspan=\"11\">")
                    .Append("<div class=\"sp_box\">")
                    .Append("<table class=\"sp_table\" width=\"100%\" border=\"0\" cellpadding=\"2\" cellspacing=\"1\">")
                    .Append("<tbody>")
                    .Append("<tr class=\"sp_3\">")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"3A\" /><strong>胜其它</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["Sother"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["Sother"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"10\" /><strong>1:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S10"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S10"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"20\" /><strong>2:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S20"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S20"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"21\" /><strong>2:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S21"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S21"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"30\" /><strong>3:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S30"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S30"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"31\" /><strong>3:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S31"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S31"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"32\" /><strong>3:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S32"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S32"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"40\" /><strong>4:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S40"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S40"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"41\" /><strong>4:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S41"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S41"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"42\" /><strong>4:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S42"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S42"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"50\" /><strong>5:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S50"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S50"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"51\" /><strong>5:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S51"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S51"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"52\" /><strong>5:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["S52"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["S52"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" />包</td></tr><tr class=\"sp_1\">")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"1A\" /><strong>平其它</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["Pother"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["Pother"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"00\" /><strong>0:0</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["P00"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["P00"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"11\" /><strong>1:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["P11"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["P11"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"22\" /><strong>2:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["P22"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["P22"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"33\" /><strong>3:3</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["P33"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["P33"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td colspan=\"8\">&nbsp;</td><td align=\"center\" style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" />包</td>")
                    .Append("</tr>")
                    .Append("<tr class=\"sp_0\">")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"0A\" /><strong>负其它</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["Fother"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["Fother"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"01\" /><strong>0:1</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F01"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F01"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"02\" /><strong>0:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F02"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F02"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"12\" /><strong>1:2</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F12"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F12"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"03\" /><strong>0:3</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F03"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F03"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"13\" /><strong>1:3</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F13"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F13"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"23\" /><strong>2:3</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F23"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F23"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"04\" /><strong>0:4</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F04"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F04"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"14\" /><strong>1:4</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F14"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F14"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"24\" /><strong>2:4</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F24"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F24"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"05\" /><strong>0:5</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F05"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F05"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"15\" /><strong>1:5</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F15"].ToString(), 0)> 0 ? Shove._Convert.StrToDouble(dr["F15"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\" ><input type=\"checkbox\" value=\"25\" /><strong>2:5</strong><br /><span>" + (Shove._Convert.StrToDouble(dr["F25"].ToString(), 0) > 0 ? Shove._Convert.StrToDouble(dr["F25"].ToString(), 0).ToString("F2") : "") + "</span></td>")
                    .Append("<td align=\"center\" style=\"cursor:pointer\"><input type=\"checkbox\" name=\"ck" + dr["MatchID"].ToString() + "\" />包</td>")
                    .Append("</tr>")
                    .Append("</tbody>")
                    .Append("</table>")
                    .Append("</div>")
                    .Append("</td>")
                    .Append("</tr>");

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

    private void GetSchemeBonusScalec()
    {
        string bScalec;
        //获得站点选项中的佣金比率
        DataTable dt = new DAL.Tables.T_Sites().Open("Opt_InitiateSchemeBonusScale,Opt_InitiateSchemeLimitLowerScaleMoney,Opt_InitiateSchemeLimitLowerScale,Opt_InitiateSchemeLimitUpperScaleMoney,Opt_InitiateSchemeLimitUpperScale", "", "");
        //把佣金比率换成整数
        bScalec = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeBonusScale"].ToString(), 0) * 100).ToString();

        //发起方案条件
        string Opt_InitiateSchemeLimitLowerScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScaleMoney"].ToString(), 100)).ToString();
        string Opt_InitiateSchemeLimitLowerScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitLowerScale"].ToString(), 0.2)).ToString();
        string Opt_InitiateSchemeLimitUpperScaleMoney = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScaleMoney"].ToString(), 10000)).ToString();
        string Opt_InitiateSchemeLimitUpperScale = (Shove._Convert.StrToDouble(dt.Rows[0]["Opt_InitiateSchemeLimitUpperScale"].ToString(), 0.05)).ToString();

        SchemeSchemeBonusScalec.Value = "{'bScalec': '" + bScalec + "','LScaleMoney': '" + Opt_InitiateSchemeLimitLowerScaleMoney + "','LScale': '" + Opt_InitiateSchemeLimitLowerScale + "' ,'UScaleMoney': '" + Opt_InitiateSchemeLimitUpperScaleMoney + "', 'UpperScale': '" + Opt_InitiateSchemeLimitUpperScale + "'}";
    }

}
