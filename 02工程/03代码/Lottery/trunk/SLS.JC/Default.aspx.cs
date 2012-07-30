using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Text;
using System.IO;
using System.Security.Cryptography;
using Shove.Database;
using System.Text.RegularExpressions;

public partial class Default : System.Web.UI.Page
{
    public string zcWinlottery = "";
    public string szWinlottery = "";
    public string gpWinlottery = "";
    public string lbSiteAffiches = "";
    public string SportsNews = "";
    public string NumberNews = "";
    public string FocusImage = "";
    public int imgCount = 1;
    public string imgbtn = "";

    public string WinRanking = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ((HtmlInputHidden)WebHead1.FindControl("currentMenu")).Value = "mHome";
        if (!this.IsPostBack)
        {
            BindWinLottery();
            BindSiteAffiches();
            BindSportsNews();
            BindFocusImage();
            // 绑定七天中奖排行
            BindWinRakking();
        }
    }

    private void BindFocusImage()
    {
        string CacheKey = "Default_BindFocusImage";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Tables.T_FocusImageNews().Open("", "", "ID Desc");
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 1200);
        }

        StringBuilder sb = new StringBuilder();
        DataRow[] drs = dt.Select("IsBig=1", "ID desc");
        string slide_a = "";
        sb.Append("<ul>");

        for (int i = 0; i < drs.Length && i < 5; i++)
        {
            if (i == 0)
            {
                slide_a = "<p class=\"p1\"><a id=\"slide_a\" href=\"" + drs[i]["Url"].ToString() + "\" target=\"_blank\">" + drs[i]["Title"].ToString() + "</a></p>";
                sb.Append("<li style='display:'><a href=\"" + drs[i]["Url"].ToString() + "\" txt=\"" + drs[i]["Title"].ToString() + "\">");
            }
            else
            {
                sb.Append("<li style='display:none'><a href=\"" + drs[i]["Url"].ToString() + "\" txt=\"" + drs[i]["Title"].ToString() + "\">");
            }
            sb.Append("<img src='Images/imgloading.gif' alt=\"" + drs[i]["Title"].ToString() + "\"");
            sb.Append(" url=\"Private/1/NewsImages/" + drs[i]["ImageUrl"].ToString() + "\"></a></li>");
            imgCount = i + 1;

            if (i == 0)
            {
                imgbtn += "<li class=\"btn02 btn01\" target=\"0\"><a></a></li>";
            }
            else
            {
                imgbtn += "<li class=\"btn02\" target=\"" + (i).ToString() + "\"><a></a></li>";
            }
        }

        sb.Append("</ul>");
        sb.Append(slide_a);
        FocusImage = sb.ToString();
    }

    private void BindSportsNews()
    {
        StringBuilder sb = new StringBuilder();
        string Key = "Default_BindSportsNews";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = @"select * from
                (select top 6 ID,Title,Content,TypeName, DateTime from V_News where isShow = 1  and [TypeName] = '竞技彩资讯'
                order by isCommend,ID desc) a
                union select * from
                (select top 6 ID,Title,Content,TypeName, DateTime from V_News where isShow = 1  and [TypeName] = '数字彩资讯'
                order by isCommend,ID desc) b ";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null){
                return;
            }

            if (dt.Rows.Count > 0){
                Shove._Web.Cache.SetCache(Key, dt,1200);
            }
        }
        DataRow[] drs = dt.Select("TypeName='竞技彩资讯'", "ID desc");

        foreach (DataRow dr in drs)
        {
            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<li><a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">");
            }
            else
            {
                sb.Append("<li><a href=\"Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString() + "\" target=\"_blank\">");
            }

            sb.Append(Shove._String.Cut(dr["Title"].ToString(), 32));
            sb.AppendLine("</a></li>");
        }

        SportsNews = sb.ToString();
        sb.Remove(0, sb.Length);
        drs = dt.Select("TypeName='数字彩资讯'", "ID desc");

        foreach (DataRow dr in drs)
        {
            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<li><a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">");
            }
            else
            {
                sb.Append("<li><a href=\"Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString() + "\" target=\"_blank\">");
            }

            sb.Append(Shove._String.Cut(dr["Title"].ToString(), 32));
            sb.AppendLine("</a></li>");
        }
        NumberNews = sb.ToString();
    }

    private void BindSiteAffiches()
    {
        StringBuilder sb = new StringBuilder();
        string Key = "Default_GetSiteAffiches";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_SiteAffiches().Open("top 5 Title, ID, DateTime, Content", "isShow = 1", " DateTime desc");
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(Key, dt, 1200);
            }
        }

        foreach (DataRow dr in dt.Rows)
        {
            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<li><a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">");
            }
            else
            {
                sb.Append("<li><a href=\"Home/Web/ShowAffiches.aspx?ID=" + dr["ID"].ToString() + "\" target=\"_blank\">");
            }

            sb.Append(Shove._String.Cut(dr["Title"].ToString(), 22));
            sb.AppendLine("</a></li>");
        }
        lbSiteAffiches = sb.ToString();
    }

    private void BindWinLottery()
    {
        DataTable dt = MSSQL.Select(@"select Name, WinLotteryNumber, T_Isuses.LotteryID from T_Isuses inner join
            (Select   max(EndTime) as EndTime, LotteryID
            From   T_Isuses where isnull(WinLotteryNumber, '') <> '' and LotteryID in (5, 6, 13, 28) Group   By   LotteryID) a on T_Isuses.LotteryID = a.LotteryID and T_Isuses.EndTime = a.EndTime");

        if (dt == null)
        {
            new Log("System").Write("Default_BindWinLottery: 获取开奖号码错误");
            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            switch (dr["LotteryID"].ToString())
            {
                case "5":
                    ucdSSQ.LoadLottoery(dr["Name"].ToString(), dr["WinLotteryNumber"].ToString());
                    break;
                case "6":
                    char[] w = dr["WinLotteryNumber"].ToString().ToCharArray();
                    ucdFC3D.LoadLottoery(dr["Name"].ToString(), w[0].ToString() + " " + w[1].ToString() + " " + w[2].ToString());
                    break;
                case "13":
                    ucdQLC.LoadLottoery(dr["Name"].ToString(), dr["WinLotteryNumber"].ToString());
                    break;
                case "28":
                    char[] x = dr["WinLotteryNumber"].ToString().ToCharArray();
                    ucdCQSSC.LoadLottoery(dr["Name"].ToString(), x[0].ToString() + " " + x[1].ToString() + " " + x[2].ToString() + " " + x[3].ToString() + " " + x[4].ToString());
                    break;
            }
        }
    }

    private void BindWinRakking()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("Default_WinRanking");

        if (dt == null)
        {
            string sql = @"select Top 4 a.*, T_Users.Name as UserName, 
                            (select LotteryID from T_Isuses where ID in (
                            select IsuseID from T_Schemes where ID in (
                            select MAX(SchemeID) from T_BuyDetails  where a.UserID = T_BuyDetails.UserID and T_BuyDetails.WinMoneyNoWithTax > 0))) as LotteryID from (
                            select top 9 UserID,sum(WinMoneyNoWithTax) as Money from T_BuyDetails
                            where [DateTime] > dateadd(dd,-7,getdate())  and WinMoneyNoWithTax > 1 
                            group by UserID order by Money desc ) a
                            inner join T_Users on a.UserID = T_Users.ID order by a.Money desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("Default_WinRanking", dt, 1200);
            }
        }
        WinRanking = "";

        //if (dt.Rows.Count < 1)
        //{
        //    return;
        //}
        //int i = 1;
        //foreach (DataRow dr in dt.Rows)
        //{
        //    WinRanking += "<tr align=\"center\">"
        //        + "<td width=\"15%\"><img src=\"/images/num" + i + ".jpg\" /></td>"
        //        + "<td width=\"31%\" ><span>" + dr["UserName"].ToString() + "</span></td>"
        //        + "<td width=\"35%\" align='right'>￥" + Convert.ToDouble(dr["Money"].ToString()).ToString("F2") + "</td>"
        //        + "<td width=\"19%\"><a href=\"/Home/Web/Score.aspx?id=" + dr["UserID"].ToString() + "&LotteryID=" + dr["LotteryID"].ToString() + "\" target=\"_blank\" class=\"yellow\">查看</a></td>"
        //        + "</tr>";
        //    i++;
        //}
        tblWinner.Rows.Clear();
        for (int c = 0; c < dt.Rows.Count; c++)
        {
            TableRow tr = new TableRow();
            TableCell tc1 = new TableCell();
            TableCell tc2 = new TableCell();
            TableCell tc3 = new TableCell();
            TableCell tc4 = new TableCell();
            tc1.Text = "<img src=\"/images/num" + (c + 1).ToString() + ".jpg\" />";
            tc2.Text = dt.Rows[c]["UserName"].ToString();
            tc3.Text = "￥" + Convert.ToDouble(dt.Rows[c]["Money"].ToString()).ToString("F2");
            tc4.Text = "<a href=\"/Home/Web/Score.aspx?id=" + dt.Rows[c]["UserID"].ToString() + "&LotteryID=" + dt.Rows[c]["LotteryID"].ToString() + "\" target=\"_blank\" class=\"yellow\">查看</a>";
            tr.Cells.Add(tc1);
            tr.Cells.Add(tc2);
            tr.Cells.Add(tc3);
            tr.Cells.Add(tc4);
            tblWinner.Rows.Add(tr);
        }
        for (int k = dt.Rows.Count; k < 4; k++)
        {
            TableRow tr = new TableRow();
            TableCell tc = new TableCell();
            tc.ColumnSpan = 4;
            tc.Text = "&nbsp;";
            tr.Cells.Add(tc);
            tblWinner.Rows.Add(tr);
        }
        //if (dt.Rows.Count < 9)
        //{
        //    for (int x = 0; x < 9 - dt.Rows.Count; x++)
        //    {
        //        WinRanking += "<tr align=\"center\">"
        //        + "<td width=\"15%\"></td>"
        //        + "<td width=\"31%\" ></td>"
        //        + "<td width=\"35%\"></td>"
        //        + "<td width=\"19%\"></td>"
        //        + "</tr>";
        //    }
        //}
    }

    #region Tool
    private string FormatWinNumber(string LotteryID, string winNumber)
    {
        StringBuilder sb = new StringBuilder();

        switch (LotteryID)
        {
            case "5":
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 1)
                        {
                            sb.Append(number[i] + " ");
                        }
                        else
                        {
                            sb.Append("<span class=\"blue\">").Append(number[i]).AppendLine("</span>");
                        }
                    }
                }

                break;

            case "39":
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 2)
                        {
                            sb.Append(number[i] + " ");
                        }
                        else
                        {
                            sb.Append("<span class=\"blue\">").Append(number[i]).AppendLine("</span>");
                        }
                    }
                }
                break;
            case "9":
            case "13":
                sb.Append(winNumber);
                break;
            default:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append(winNumber.Substring(i, 1) + " ");
                    }
                }
                break;
        }

        return sb.ToString();
    }
    #endregion
}