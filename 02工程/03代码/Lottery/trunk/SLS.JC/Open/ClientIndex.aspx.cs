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
using Shove.Database;

public partial class Open_ClientIndex : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            tbBeginTime.Text = Shove._Web.Utility.GetRequest("startdate") == "" ? DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd") : Shove._Web.Utility.GetRequest("startdate");
            tbEndTime.Text = Shove._Web.Utility.GetRequest("enddate") == "" ? DateTime.Now.ToString("yyyy-MM-dd") : Shove._Web.Utility.GetRequest("enddate");

            GetOpenMatch();
        }
    }

    private void GetOpenMatch()
    {
        DateTime BeginTime = Shove._Convert.StrToDateTime(tbBeginTime.Text, DateTime.Now.AddDays(-3).ToString());
        DateTime EndTime = Shove._Convert.StrToDateTime(tbEndTime.Text, DateTime.Now.ToString()).AddDays(1);

        string Key = "Open_index_GetOpenMatch" + BeginTime.ToString("yyyy-MM-dd") + EndTime.ToString("yyyy-MM-dd");

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string strCmd = @"select ROW_NUMBER() OVER (ORDER BY StopSellingTime desc, MatchNumber desc) AS id, MatchDate, MatchNumber, Game, GameColor,  MainTeam, GuestTeam, isnull(MainLoseBall, 0) as MainLoseBall, SPFResult, SPFBonus, BQCResult, BQCBonus, ZJQSResult, ZJQSBonus, ZQBFResult, ZQBFBonus, Result from T_Match
                              where StopSellingTime between '" + BeginTime.ToString("yyyy-MM-dd") + "' and '" + EndTime.ToString("yyyy-MM-dd") + "' and isnull(Result, '') <> ''order by  StopSellingTime desc";

            dt = MSSQL.Select(strCmd);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(Key, dt, 600);
            }
        }

        DataTable dtGame = new DataTable();

        dtGame.Columns.Add("Game", typeof(System.String));
        DataRow drGame = dtGame.NewRow();
        drGame["Game"] = "全部联赛";

        dtGame.Rows.Add(drGame);
        dtGame.AcceptChanges();

        object LastValue = null;

        foreach (DataRow dr in dt.Select("", "Game"))
        {
            if (LastValue == null || !(ColumnEqual(LastValue, dr["Game"])))
            {
                LastValue = dr["Game"];

                DataRow drGame1 = dtGame.NewRow();
                drGame1["Game"] = dr["Game"].ToString();
                dtGame.Rows.Add(drGame1);
                dtGame.AcceptChanges();
            }
        }

        Shove.ControlExt.FillDropDownList(ddlleague, dtGame, "Game", "Game");

        int SumPage = dt.Rows.Count;
        string league = Shove._Web.Utility.GetRequest("league");

        if (league != "" && league != "全部联赛")
        {
            SumPage = dt.Select("Game='" + league + "'").Length;
        }

        int Count = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("p"), 1);
        int Last = SumPage % 20 == 0 ? SumPage / 20 : SumPage / 20 + 1;

        string URL = "?";

        if (!string.IsNullOrEmpty(Shove._Web.Utility.GetRequest("startdate")))
        {
            URL += "startdate=" + Shove._Web.Utility.GetRequest("startdate") + "&";
        }

        if (!string.IsNullOrEmpty(Shove._Web.Utility.GetRequest("enddate")))
        {
            URL += "enddate=" + Shove._Web.Utility.GetRequest("enddate") + "&";
        }

        if (!string.IsNullOrEmpty(league))
        {
            URL += "league=" + league + "&";
        }

        lbNum.Text = SumPage.ToString();

        lbPage.Text = "<li class=\"first\"><a href='" + URL + "p=1'>首页</a></li>";
        lbPage.Text += "<li class=\"previous\"><a href='" + URL + "p=" + (Count == 1 ? "1" : (Count - 1).ToString()) + "'>上一页</a></li>";

        for (int i = 1; i < Last + 1; i++)
        {
            lbPage.Text += (Count == i ? "<li class=\"slect_r\">" + i.ToString() + "</li>" : "<li><a href=\"" + URL + "p=" + i.ToString() + "\">" + i.ToString() + "</a></li>");
        }

        lbPage.Text += "<li class='next'><a href='" + URL + "p=" + (Count == Last ? Last.ToString() : (Count + 1).ToString()) + "'>下一页</a></li>";
        lbPage.Text += "<li class='last'><a href='" + URL + "p=" + Last.ToString() + "'>末页</a></li>";
        lbPage.Text += "<li class='totel'>共" + Last.ToString() + "页</li>";

        DataRow[] drs = dt.Select("1=1", "id");

        if (league != "" && league != "全部联赛")
        {
            drs = dt.Select("Game='" + league + "'");
        }

        string lineStyle = "";

        JczqMatch.Text = "<table border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"data_tablebox\">";
        JczqMatch.Text += "<tr class=\"trt1\"><th rowspan=\"2\" width=\"70\">赛事日期</th>";
        JczqMatch.Text += "<th rowspan=\"2\" width=\"70\">赛事编号</th>";
        JczqMatch.Text += "<th rowspan=\"2\" width=\"70\">赛事类型</th>";
        JczqMatch.Text += "<th class=\"th_noline\" rowspan=\"2\"></th>";
        JczqMatch.Text += "<th class=\"th_noline\" rowspan=\"2\">对阵</th>";
        JczqMatch.Text += "<th class=\"\" rowspan=\"2\"></th>";
        JczqMatch.Text += "<th rowspan=\"2\">让球</th>";
        JczqMatch.Text += "<th height=\"24\" colspan=\"2\" class=\"tbottom\">让球胜平负</th>";
        JczqMatch.Text += "<th colspan=\"2\" class=\"tbottom\">总进球数</th>";
        JczqMatch.Text += "<th colspan=\"2\" class=\"tbottom\">比分</th>";
        JczqMatch.Text += "<th colspan=\"2\" class=\"tbottom\">半全场</th></tr>";
        JczqMatch.Text += "<tr class=\"trt2\"><th width=\"35\" height=\"24\">彩果</th>";
        JczqMatch.Text += "<th>奖金</th>";
        JczqMatch.Text += "<th>彩果</th>";
        JczqMatch.Text += "<th>奖金</th>";
        JczqMatch.Text += "<th>彩果</th>";
        JczqMatch.Text += "<th>奖金</th>";
        JczqMatch.Text += "<th>彩果</th>";
        JczqMatch.Text += "<th>奖金</th></tr>";

        for (int i = (Count - 1) * 20; i < (Count * 20 < drs.Length ? Count * 20 : drs.Length); i++)
        {
            DataRow dr = drs[i];

            lineStyle = Shove._Convert.StrToInt(dr["ID"].ToString(), 0) % 2 == 0 ? "tr1" : "tr2";

            JczqMatch.Text += "<tr class=\"" + lineStyle + "\"><td>" + Shove._Convert.StrToDateTime(dr["MatchDate"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd") + "</td>";
            JczqMatch.Text += "<td>" + dr["MatchNumber"].ToString() + "</td>";
            JczqMatch.Text += "<td bgcolor=\"" + dr["GameColor"].ToString() + "\"><span class=\"white12\">" + dr["Game"].ToString() + "</span></td>";
            JczqMatch.Text += "<td >" + dr["MainTeam"].ToString() + "</td>";
            JczqMatch.Text += "<td><span class=\"red\" >" + dr["Result"].ToString() + "</span></td>";
            JczqMatch.Text += "<td>" + dr["GuestTeam"].ToString() + "</td>";
            JczqMatch.Text += "<td><span class=\"bold\">" + dr["MainLoseBall"].ToString().Replace("无让球", "0") + "</span></td>";
            JczqMatch.Text += "<td>" + dr["SPFResult"].ToString() + "</td>";
            JczqMatch.Text += "<td ><span class=\"red\">" + dr["SPFBonus"].ToString() + "</span></td>";
            JczqMatch.Text += "<td>" + dr["ZJQSResult"].ToString() + "</td>";
            JczqMatch.Text += "<td ><span class=\"red\">" + dr["ZJQSBonus"].ToString() + "</span></td>";
            JczqMatch.Text += "<td>" + dr["ZQBFResult"].ToString() + "</td>";
            JczqMatch.Text += "<td ><span class=\"red\">" + dr["ZQBFBonus"].ToString() + "</span></td>";
            JczqMatch.Text += "<td>" + dr["BQCResult"].ToString() + "</td>";
            JczqMatch.Text += "<td ><span class=\"red\">" + dr["BQCBonus"].ToString() + "</span></td></tr>";
            
        }

        if (league != "" && league != "全部联赛")
        {
            ddlleague.SelectedValue = league;
        }

        JczqMatch.Text += "</table>";
    }

    private bool ColumnEqual(object A, object B)
    {
        if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value
            return true;
        if (A == DBNull.Value || B == DBNull.Value) //  only one is DBNull.Value
            return false;
        return (A.Equals(B));  // value type standard comparison
    }

}
