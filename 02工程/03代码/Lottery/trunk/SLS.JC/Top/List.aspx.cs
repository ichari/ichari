using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Shove.Database;
using System.Text;

public partial class Top_List : System.Web.UI.Page
{
    private int LotteryID = 0;
    private string LotteryName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string lot = Shove._Web.Utility.GetRequest("lot").ToString();
            string P1 = Shove._Web.Utility.GetRequest("p1").ToString();
            string P2 = Shove._Web.Utility.GetRequest("p2").ToString();
            string P3 = Shove._Web.Utility.GetRequest("p3").ToString();

            if (string.IsNullOrEmpty(lot))
            {
                lot = "jczq";
            }

            switch (lot)
            {
                case "sfc":
                    LotteryID = 74;
                    LotteryName = "胜负彩";
                    break;
                case "rj":
                    LotteryID = 75;
                    LotteryName = "任选九场";
                    break;
                case "6cbq":
                    LotteryID = 15;
                    LotteryName = "六场半全场";
                    break;
                case "4cjq":
                    LotteryID = 2;
                    LotteryName = "四场进球彩";
                    break;
                case "dlt":
                    LotteryID = 39;
                    LotteryName = "超级大乐透";
                    break;
                case "pls":
                    LotteryID = 63;
                    LotteryName = "排列三";
                    break;
                case "qxc":
                    LotteryID = 3;
                    LotteryName = "七星彩";
                    break;
                case "22x5":
                    LotteryID = 9;
                    LotteryName = "22选5";
                    break;
                case "jczq":
                    LotteryID = 72;
                    LotteryName = "竞彩足球";
                    break;
                case "jclq":
                    LotteryID = 73;
                    LotteryName = "竞彩篮球";
                    break;
                default:
                    LotteryID = 72;
                    LotteryName = "竞彩足球";
                    break;
            }

            if (string.IsNullOrEmpty(P1) || P1 == "1")
            {
                P1 = "1";
                BindData(lot, P2, P3);
            }
            else
            {
                BindCustomFriendFollow(lot, P2, P3);
            }
        }
    }

    private void BindData(string lot, string P2, string P3)
    {
        if (string.IsNullOrEmpty(P2))
        {
            P2 = "2";
        }

        if (string.IsNullOrEmpty(P3))
        {
            P3 = DateTime.Now.ToString();
        }

        DateTime SerachDatetime = Shove._Convert.StrToDateTime(P3, DateTime.Now.ToString());

        int Year = SerachDatetime.Year;
        int Month = SerachDatetime.Month;

        int Week = (int)SerachDatetime.DayOfWeek;

        DateTime StartDateTime = SerachDatetime.AddDays(Week * -1);
        DateTime EndDateTime = SerachDatetime.AddDays(7- Week);

        StringBuilder strSQL = new StringBuilder();

        strSQL.Append("select top 20 ROW_NUMBER() OVER (order by SUM(WinMoney) desc) AS id, COUNT(SchemeID) as SchemeCount, Name, SUM(DetailMoney) as DetailMoney, InitiateUserID, SUM(WinMoney) as WinMoney from (");
        strSQL.Append(" select SchemeID, SUM(DetailMoney) as DetailMoney, InitiateUserID, WinMoney, Name  from (");
        strSQL.Append(" select T_BuyDetails.ID, T_BuyDetails.SchemeID, T_BuyDetails.DetailMoney, a.InitiateUserID, a.WinMoney, T_Users.Name from T_BuyDetails");
        strSQL.Append(" inner join(");
        strSQL.Append(" select InitiateUserID, id, WinMoney from T_Schemes where Buyed = 1 and Share > 1 and WinMoney > 0 and exists (");

        if (LotteryID != 72 && LotteryID != 73)
        {
            if (P2 == "1")
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + " and EndTime between '" + StartDateTime.ToString() + "' and '" + EndDateTime.ToString() + "' and T_Schemes.IsuseID = T_Isuses.ID))");
                lbtitle.Text = Year.ToString() + "年" + StartDateTime.ToShortDateString() + " ~ " + EndDateTime.ToShortDateString() + LotteryName + "-发起盈利排行榜";
            }
            else if (P2 == "2")
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + " and " + Year.ToString() + " * 100 + " + Month.ToString() + " = year(EndTime) * 100 + MONTH(EndTime) and T_Schemes.IsuseID = T_Isuses.ID))");
                lbtitle.Text = Year.ToString() + "年" + Month.ToString().PadLeft(2, '0') + "月" + LotteryName + "-发起盈利排行榜";
            }
            else
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + " and " + Year.ToString() + " = year(EndTime) and T_Schemes.IsuseID = T_Isuses.ID))");
                lbtitle.Text = Year.ToString() + "年" + LotteryName + "-发起盈利排行榜";
            }
        }
        else
        {
            if (P2 == "1")
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + " and T_Schemes.IsuseID = T_Isuses.ID) and [Datetime] between '" + StartDateTime.ToString() + "' and '" + EndDateTime.ToString() + "')");
                lbtitle.Text = Year.ToString() + "年" + StartDateTime.ToShortDateString() + " ~ " + EndDateTime.ToShortDateString() + LotteryName + "-发起盈利排行榜";
            }
            else if (P2 == "2")
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + "  and T_Schemes.IsuseID = T_Isuses.ID) and " + Year.ToString() + " * 100 + " + Month.ToString() + " = year([Datetime]) * 100 + MONTH([Datetime]))");
                lbtitle.Text = Year.ToString() + "年" + Month.ToString().PadLeft(2, '0') + "月" + LotteryName + "-发起盈利排行榜";
            }
            else
            {
                strSQL.Append(" select 1 from T_Isuses where LotteryID = " + LotteryID.ToString() + " and T_Schemes.IsuseID = T_Isuses.ID) and " + Year.ToString() + " = year([Datetime]) )");
                lbtitle.Text = Year.ToString() + "年" + LotteryName + "-发起盈利排行榜";
            }
        }

        strSQL.Append(" a on T_BuyDetails.SchemeID = a.ID");
        strSQL.Append(" inner join T_Users on a.InitiateUserID = T_Users.ID");
        strSQL.Append(" and T_BuyDetails.QuashStatus = 0) b group by b.schemeID, b.InitiateUserID, b.WinMoney, b.Name) c group by InitiateUserID, Name");

        DataTable dt = MSSQL.Select(strSQL.ToString());

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>");
        sb.Append("<td class=\"td_t01\">排名</td>");
        sb.Append("<td class=\"td_t01\">发起人</td>");
        sb.Append("<td class=\"td_t01\">方案个数</td>");
        sb.Append("<td class=\"td_t01\">方案总金额</td>");
        sb.Append("<td class=\"td_t01\">中奖总金额</td>");
        sb.Append("<td class=\"td_t01\">方案盈利</td>");
        sb.Append("<td class=\"td_t01\">自动跟单</td>");
        sb.Append("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr class=\"bg_01\" onmouseover='this.className=\"bg_03\";this.style.cursor=\"pointer\";' onmouseout='this.className=\"bg_01\";this.style.cursor=\"none\";'>");
            sb.Append("<td height=\"20\" class=\"bg_01\">").Append(dr["id"].ToString()).Append("</td>");
            sb.Append("<td class=\"bg_01\"><span style=\"text-decoration: underline; cursor: pointer\" onclick=\"ShowMedal('" + dr["InitiateUserID"].ToString() + "','" + LotteryID.ToString() + "','')\">" + dr["Name"].ToString() + "</span></td>");
            sb.Append("<td class=\"bg_01\">" + dr["SchemeCount"].ToString() + "</td>");
            sb.Append("<td>￥" + dr["DetailMoney"].ToString() + "</td>");
            sb.Append("<td>￥" + dr["WinMoney"].ToString() + "</td>");
            sb.Append("<td>￥" + (Shove._Convert.StrToFloat(dr["DetailMoney"].ToString(), 0) - Shove._Convert.StrToFloat(dr["WinMoney"].ToString(), 0)).ToString() + "</td>");

            string dinZhi = "<a href='/Home/Room/FollowFriendSchemeAdd_User.aspx?FollowUserID=" + dr["InitiateUserID"].ToString() + "&FollowUserName=" + HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + LotteryID.ToString() + "' onclick=\"return parent.CreateLogin();\" target=\"_blank\" >";

            sb.Append("<td class=\"bg_01\">" + dinZhi + "定制</a></td>");
            sb.Append("</tr>");
        }

        lbMatch.Text = sb.ToString();
    }

    private void BindCustomFriendFollow(string lot, string P2, string P3)
    {

        DataTable dt = MSSQL.Select("select top 20 ROW_NUMBER() OVER (order by a.count desc) AS Num, T_Users.ID as InitiateUserID, T_Users.Name, T_Users.Level, a.count as SchemeCount, a.LotteryID from (select COUNT(id) as [count], FollowUserID, LotteryID from T_CustomFriendFollowSchemes where LotteryID = " + LotteryID.ToString() + " group by LotteryID, FollowUserID) a inner join T_Users  on a.FollowUserID = T_Users.ID order by a.count desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>");
        sb.Append("<td class=\"td_t01\">排名</td>");
        sb.Append("<td class=\"td_t01\">发起人</td>");
        sb.Append("<td class=\"td_t01\">战绩</td>");
        sb.Append("<td class=\"td_t01\">跟单人气</td>");
        sb.Append("<td class=\"td_t01\">自动跟单</td>");
        sb.Append("</tr>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr class=\"bg_01\" onmouseover='this.className=\"bg_03\";this.style.cursor=\"pointer\";' onmouseout='this.className=\"bg_01\";this.style.cursor=\"none\";'>");
            sb.Append("<td height=\"20\" class=\"bg_01\">").Append(dr["Num"].ToString()).Append("</td>");
            sb.Append("<td class=\"bg_01\"><span style=\"text-decoration: underline; cursor: pointer\" onclick=\"ShowMedal('" + dr["InitiateUserID"].ToString() + "','" + LotteryID.ToString() + "','')\">" + dr["Name"].ToString() + "</span></td>");
            sb.Append("<td class=\"bg_01\">" + dr["SchemeCount"].ToString() + "</td>");

            if (dr["Level"].ToString() == "0")
            {
                sb.Append("<td><a href='../Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + LotteryID.ToString() + "' title='点击查看' target='_blank'>-</a></td>");
            }
            else
            {
                int level = Shove._Convert.StrToInt(dr["Level"].ToString(), 0);

                string img = "";

                for (int i = 1; i <= level; i++)
                {
                    img += "<img src='Images/gold.gif'/>";
                }

                sb.Append("<td><a href='../Home/Web/Score.aspx?id=" + dr["InitiateUserID"].ToString() + "&LotteryID=" + LotteryID.ToString() + "' title='点击查看' target='_blank'>" + img + "</a></td>");
            }

            string dinZhi = "<a href='/Home/Room/FollowFriendSchemeAdd_User.aspx?FollowUserID=" + dr["InitiateUserID"].ToString() + "&FollowUserName=" + HttpUtility.UrlEncode(dr["Name"].ToString()) + "&LotteryID=" + LotteryID.ToString() + "' onclick=\"return parent.CreateLogin();\" target=\"_blank\" >";

            sb.Append("<td class=\"bg_01\">" + dinZhi + "定制</a></td>");
            sb.Append("</tr>");
        }

        lbMatch.Text = sb.ToString();
    }
}
