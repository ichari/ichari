using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class NewsPapers_NewsPaperList : SitePageBase
{
    private static Dictionary<int, string> LotteryOpenedDay = new Dictionary<int, string>();

    private void LoadOpenDay(string Name)
    {
        LotteryOpenedDay[59] = "每日";
        LotteryOpenedDay[9] = "每日";
        LotteryOpenedDay[63] = "每日";
        LotteryOpenedDay[64] = "每日";
        LotteryOpenedDay[6] = "每日";
        if (Name == "0903")
        {
            LotteryOpenedDay[5] = "二 四 日 ";
            LotteryOpenedDay[39] = "一 三 六 ";
            LotteryOpenedDay[13] = "一 三 五 ";
            LotteryOpenedDay[58] = "一 三 六 ";
            LotteryOpenedDay[3] = "二 五 日 ";
        }
        else
        {
            int[] week = new int[] { 2, 4, 7 };
            LotteryOpenedDay[5] = "二 四 日 " + GetOpenDay(week);

            week = new int[] { 1, 3, 6 };
            LotteryOpenedDay[39] = "一 三 六 " + GetOpenDay(week);

            week = new int[] { 1, 3, 5 };
            LotteryOpenedDay[13] = "一 三 五 " + GetOpenDay(week);

            week = new int[] { 1, 3, 6 };
            LotteryOpenedDay[58] = "一 三 六 " + GetOpenDay(week);
            LotteryOpenedDay[6] = "每日";

            week = new int[] { 2, 5, 7 };
            LotteryOpenedDay[3] = "二 五 日 " + GetOpenDay(week);
        }
    }

    private string GetOpenDay(int[] weeks)
    {
        int week = (int)DateTime.Now.DayOfWeek;

        week = week == 7 ? 0 : week;

        int j = 0;
        foreach (int i in weeks)
        {
            j = i - week;
            if (j >= 0)
            {
                if (j == 0)
                {
                    return "(今日开奖)";
                }
                else if (j == 1)
                {
                    return "(明天开奖)";
                }
                else if (j == 2)
                {
                    return "(后天开奖)";
                }
            }
        }

        return "(今日开奖)";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int IsuseId = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("IsuseId"), 0);

            BindddlIsuses(IsuseId);

            BindNewsPaper();
        }
    }

    private void BindddlIsuses(int IsuseId)
    {
        string Key = "Home_Room_NewsPaper";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            //dt = new DAL.Tables.T_NewsPaperIsuses().Open("", "convert(datetime,StartTime) <= getdate()", " ID desc");
            dt = new DAL.Tables.T_NewsPaperIsuses().Open("", "", " ID desc");
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试！", this.GetType().BaseType.FullName);

                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr["Name"] = dr["Name"].ToString() + "期";

            }

            Shove.ControlExt.FillDropDownList(ddlIsusesID, dt, "Name", "ID");

            ListItem item = ddlIsusesID.Items.FindByValue(IsuseId.ToString());

            if (item != null)
            {
                ddlIsusesID.SelectedIndex = -1;
                item.Selected = true;
            }

        }
    }

    private string BindWinNumber(string Name)
    {
        string Key = "Home_Room_NewsPaper_BindWinNumber";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select  a.Name,WinLotteryNumber,b.Name as LotteryName,TotalMoney,LotteryID from T_Isuses a ")
                .Append("left join T_Lotteries b on a.LotteryID = b.ID ")
                .Append("left join T_TotalMoney c on a.ID = c.IsuseID ")
                .Append("where a.IsOpened = 1 and LotteryID in(3,5,6,9,13,39,58,59,63,64) and a.ID = (select top 1 ID from T_Isuses where LotteryID = a.LotteryID and IsOpened = 1 order by EndTime desc)");
            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据繁忙，请重试！", this.GetType().BaseType.FullName);

                return "";
            }

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        return BindWinTable(dt, Name);
    }

    private string BindWinTable(DataTable dt, string Name)
    {
        StringBuilder sb = new StringBuilder();
        string color = "";
        int lotteryID = 0;
        int i = 0;
        if (Name == "0902")
        {
            sb.Append("<table cellspacing='1' cellpadding='0' width='450' bgcolor='#dddddd' border='0'> ")
                .Append("<tbody>")
                .Append("<tr>")
                .Append("<td class='blue14' align='center' width='75' bgcolor='#ecf9ff' height='30'><strong>彩种</strong></td>")
                .Append("<td class='blue14' align='center' width='62' bgcolor='#ecf9ff'><strong>期号</strong></td>")
                .Append("<td class='blue14' align='center' width='191' bgcolor='#ecf9ff'><strong>开奖号码</strong></td>")
                .Append("<td class='blue14' align='center' width='117' bgcolor='#ecf9ff'><strong>开奖时间</strong></td>")
                .Append("</tr>");


            double totalMoney = 0;


            foreach (DataRow dr in dt.Rows)
            {
                lotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), 0);

                if (LotteryOpenedDay.Keys.Contains(lotteryID))
                {
                    color = "#ffffff";

                    if (i % 2 != 0)
                    {
                        color = "#f7f7f7";
                    }

                    i++;

                    sb.Append("<tr>")
                        .Append("<td height=\"30\" align=\"left\" bgcolor=\"" + color + "\" class=\"hui12\" style=\"padding-left:10px;\">")
                        .Append(dr["LotteryName"].ToString())
                        .Append("</td>")
                        .Append("<td align=\"left\" bgcolor=\"" + color + "\" class=\"hui12\" style=\"padding-left:10px;\">")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td align=\"left\" bgcolor=\"" + color + "\" class=\"hui14_2\" style=\"padding-left:10px;\"");


                    totalMoney = Shove._Convert.StrToDouble(dr["TotalMoney"].ToString(), 0);

                    if (totalMoney > 0)
                    {
                        sb.Append("height=\"60\"");
                    }

                    sb.Append(">")
                        .Append(FormatLotteryWinNumber(lotteryID.ToString(), dr["WinLotteryNumber"].ToString()));

                    if (totalMoney > 0)
                    {
                        sb.Append("<table width=\"92%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">")
                            .Append("<tr>")
                            .Append("<td height=\"8\"><div id=\"hr\"></div></td>")
                            .Append("</tr>")
                            .Append("<tr>")
                            .Append("<td class=\"blue12\">")
                            .Append("奖池滚存：")
                            .Append(totalMoney.ToString("N"))
                            .Append("元</td></tr></table>");
                    }

                    sb.Append("</td>")
                        .Append("<td align=\"center\" bgcolor=\"" + color + "\" class=\"hui12\" >")
                        .Append(LotteryOpenedDay[lotteryID])
                        .Append("</td></tr>");
                }
            }
        }
        else if (Name == "0903")
        {
            sb.Append("<table width='98%' border='0' cellpadding='0' cellspacing='1' style='background-color: #CCC'")
                .Append("<tbody>")
                .Append("<tr  class='blue11'>")
                .Append("<td width='22%' height='24' align='center' valign='middle' bgcolor='#E6F6F8'>彩种</td>")
                .Append("<td width='17%' height='24' align='center' valign='middle' bgcolor='#E6F6F8'>期号</td>")
                .Append("<td width='41%' height='24' align='center' valign='middle' bgcolor='#E6F6F8'>开奖号码</td>")
                .Append("<td width='20%' height='24' align='center' valign='middle' bgcolor='#E6F6F8'>开奖时间</td>")
                .Append("</tr>");

            foreach (DataRow dr in dt.Rows)
            {
                lotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), 0);
                if (LotteryOpenedDay.Keys.Contains(lotteryID))
                {
                    color = "#FFFFFF";

                    if (i % 2 != 0)
                    {
                        color = "#F6F9F9";
                    }

                    i++;

                    sb.Append("<tr>")
                        .Append("<td height='24' align='center' bgcolor=\"" + color + "\" class='hui14_2'>")
                        .Append(dr["LotteryName"].ToString())
                        .Append("</td>")
                        .Append("<td height='24' align='center' bgcolor=\"" + color + "\" class='hui12'>")
                        .Append(dr["Name"].ToString())
                        .Append("</td>")
                        .Append("<td height='24' align='center' bgcolor=\"" + color + "\" class='hui14_2'>")
                        .Append(FormatLotteryWinNumber(lotteryID.ToString(), dr["WinLotteryNumber"].ToString()))
                        .Append("</td>")
                        .Append("<td height='24' align='center' bgcolor=\"" + color + "\" class='hui12'>")
                        .Append(LotteryOpenedDay[lotteryID])
                        .Append("</td>")
                        .Append("</tr>");

                }
            }
            sb.Append("</tbody></table>");
        }

        return sb.ToString();
    }

    private void BindNewsPaper()
    {
        if (ddlIsusesID.SelectedIndex != -1)
        {

            string Key = "Home_Room_NewsPaper_BindNewsPaper_" + ddlIsusesID.SelectedValue;

            DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);
            dt = null;

            if (dt == null)
            {

                dt = new DAL.Tables.T_NewsPaperIsuses().Open("", "ID = " + Shove._Convert.StrToInt(ddlIsusesID.SelectedValue, 0).ToString() + "", "[ID]");

                if (dt == null)
                {

                    PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                    return;
                }


                if (dt.Rows.Count > 0)
                {
                    Shove._Web.Cache.SetCache(Key, dt, 3600);
                }

            }

            if (dt.Rows.Count > 0)
            {
                LoadOpenDay(dt.Rows[0]["Name"].ToString());
                tdIsuseOpenInfo.InnerHtml = dt.Rows[0]["NPMessage"].ToString().Replace("<$Content>", BindWinNumber(dt.Rows[0]["Name"].ToString()));
            }

            string isuseName = ddlIsusesID.SelectedItem.Text;
            lbTime.Text = "今天是：" + DateTime.Now.ToString("yyyy年MM月dd日") + "&nbsp " + "彩友报" + "<span class='red14_2'>" + isuseName.Substring(0, isuseName.Length - 1) + "</span>" + "期";

            //标题和关键字
            this.Page.Title = "彩友报 " + isuseName + " －" + _Site.Name + "主办－买彩票，就上" + _Site.Name ;

            this.key.Content = "彩友报" + isuseName;
            this.des.Content = "彩友报" + isuseName + " 是" + _Site.Name + "为广大彩民定期提供的一份彩票咨询电子期刊。";
        }

    }

    private string FormatLotteryWinNumber(string lotteryID, string lotteryWinNumber)
    {
        lotteryWinNumber = lotteryWinNumber.Trim();

        if (string.IsNullOrEmpty(lotteryWinNumber))
        {
            return lotteryWinNumber;
        }

        string number = "";

        if (lotteryID == "5" || lotteryID == "39" || lotteryID == "13" || lotteryID == "58")
        {
            number = "<span class=\"red14_2\"  style =\"font-weight:normal\" >" + lotteryWinNumber.Split('+')[0] + "</span> +<span class=\"blue14\" style =\"font-weight:normal\">" + lotteryWinNumber.Split('+')[1] + "</span>";
        }
        else if (lotteryID == "59" || lotteryID == "9")
        {
            number = "<span class=\"red14_2\" style =\"font-weight:normal\">" + lotteryWinNumber + "</span>";
        }
        else if (lotteryID == "6" || lotteryID == "3" || lotteryID == "63" || lotteryID == "64")
        {
            number = "<span class=\"red14_2\" style =\"font-weight:normal\">";

            for (int i = 0; i < lotteryWinNumber.Length; i++)
            {
                number += lotteryWinNumber.Substring(i, 1) + "&nbsp;";
            }

            number += "</span>";
        }
        return number;
    }

    protected void ddlIsusesID_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(ddlIsusesID.SelectedValue + ".aspx");
        //BindNewsPaper();
    }
}
