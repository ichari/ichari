using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;

public partial class JCZC_Scheme : System.Web.UI.Page
{
    private long SchemeID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            SchemeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
            Bind(SchemeID);
        }
    }

    private void Bind(long SchemeID)
    {
        string LotteryNumber = "";
        string[] LotteryNumbers = null;

        string CacheKey = "JCZC_Scheme_Bind";

        DataTable dtMatch = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (SchemeID < 0)
        {
            string FileName = Request.Cookies["ASP.NET_SessionId"].Value;

            try
            {
                LotteryNumber = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "Temp\\" + FileName + ".txt");
            }
            catch { }

            if (string.IsNullOrEmpty(LotteryNumber))
            {
                Shove._Web.JavaScript.Alert(this.Page, "传递的参数错误，请重新发起操作！");

                return;
            }

            LotteryNumbers = LotteryNumber.Replace("\r", "").Split('\n');

            if (dtMatch == null)
            {
                dtMatch = new DAL.Tables.T_PassRate().Open("MatchID, MatchNumber, StopSellTime", "", "");
            }
        }

        if (string.IsNullOrEmpty(LotteryNumber))
        {
            string SchemeInfo = "";

            try
            {
                SchemeInfo = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "SchemeInfo\\" + SchemeID.ToString() + ".txt");
            }
            catch { }

            if (string.IsNullOrEmpty(SchemeInfo))
            {
                DataTable dt = new DAL.Tables.T_Schemes().Open("", "ID=" + SchemeID.ToString(), "");

                if (dt == null)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "传递的参数错误，请重新发起操作！");

                    return;
                }

                if (dt.Rows.Count < 1)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "传递的参数错误，请重新发起操作！");

                    return;
                }

                LotteryNumber = dt.Rows[0]["LotteryNumber"].ToString();

                ArrayList al = new ArrayList();

                string[] strs = LotteryNumber.Split('\n');

                if (strs == null)
                    return;
                if (strs.Length == 0)
                    return;

                string CanonicalNumber = "";
                int PlayTypeID = Shove._Convert.StrToInt(dt.Rows[0]["PlayTypeID"].ToString(), 7201);

                string CacheKeyNumbers = "Home_Web_DownloadSchemeFile_" + SchemeID.ToString();

                LotteryNumbers = Shove._Web.Cache.GetCacheAsString(CacheKeyNumbers, "").Split('\n');
                string[] strNumbers = null;

                if (LotteryNumbers.Length < 2)
                {
                    for (int i = 0; i < strs.Length; i++)
                    {
                        if (string.IsNullOrEmpty(strs[i]))
                        {
                            continue;
                        }

                        strNumbers = new SLS.Lottery()[Shove._Convert.StrToInt(PlayTypeID.ToString().Substring(0, PlayTypeID.ToString().Length - 2), 72)].ToSingle(strs[i], ref CanonicalNumber, PlayTypeID);

                        if (strNumbers == null)
                        {
                            continue;
                        }

                        for (int j = 0; j < strNumbers.Length; j++)
                        {
                            al.Add(strNumbers[j]);
                        }
                    }

                    LotteryNumbers = new string[al.Count];

                    StringBuilder sbLotteryNumbers = new StringBuilder();

                    for (int i = 0; i < al.Count; i++)
                    {
                        if (i == al.Count)
                        {
                            sbLotteryNumbers.Append(al[i].ToString());
                        }
                        else
                        {
                            sbLotteryNumbers.Append(al[i].ToString() + "\n");
                        }

                        LotteryNumbers[i] = al[i].ToString();
                    }

                    Shove._Web.Cache.SetCache(CacheKeyNumbers, sbLotteryNumbers.ToString(), 3600);
                }

                if (dtMatch == null)
                {
                    dtMatch = new DAL.Tables.T_Match().Open("ID as MatchID, MatchNumber, StopSellingTime as StopSellTime", "", "");
                }
            }
            else
            {
                LotteryNumber = SchemeInfo.Substring(0, SchemeInfo.LastIndexOf('$') - 1);

                LotteryNumbers = LotteryNumber.Replace("\r", "").Split('\n');

                if (dtMatch == null)
                {
                    dtMatch = new DAL.Tables.T_PassRate().Open("MatchID, MatchNumber, StopSellTime", "MatchID in (" + SchemeInfo.Substring(SchemeInfo.LastIndexOf('$')) + ")", "");
                }
            }
        }

        if (dtMatch == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "传递的参数错误，请重新发起操作！");

            return;
        }

        if (dtMatch.Rows.Count < 1)
        {
            return;
        }

        Shove._Web.Cache.SetCache(CacheKey, dtMatch, 3600);

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"60%\" border=\"0\" align=\"center\" cellpadding=\"2\" cellspacing=\"1\" class=\"BgBlue\">");
        sb.Append("<tr align=\"center\" bgcolor=\"#FFFFFF\" class=\"BlueLightBg WhiteWords\">");
        sb.Append("<td width=\"8%\"><strong>序号</strong></td>");
        sb.Append("<td><strong>过关场次</strong></td>");
        sb.Append("<td width=\"10%\"><strong>过关方式</strong></td>");
        sb.Append("<td width=\"10%\"><strong>注数</strong></td>");
        sb.Append("<td width=\"10%\"><strong>投注金额(元)</strong></td></tr>");

        string Number = "";
        int No = 0;
        string BuyWays = "";

        int Multiple = 0;

        int LotID = 0;
        int PlayID = 0;

        PlayID = Shove._Convert.StrToInt(LotteryNumbers[0].Split(';')[0], 7201);
        LotID = Shove._Convert.StrToInt(PlayID.ToString().Substring(0, 2), 72);

        DateTime EndTime = DateTime.Now;

        int pageindex = 1;
        if (!string.IsNullOrEmpty(Shove._Web.Utility.GetRequest("p")))
        {
            pageindex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("p").ToString(), 1);
        }

        int perPageRowCount = 30;

        if (pageindex < 1)
        {
            pageindex = 1;
        }

        foreach (string str in LotteryNumbers)
        {
            if (string.IsNullOrEmpty(str))
            {
                continue;
            }

            No++;

            if (str.Split(';').Length < 3)
            {
                continue;
            }

            try
            {
                Multiple = Shove._Convert.StrToInt(str.Split(';')[2].Substring(1, str.Split(';')[2].Length - 2).Substring(2), 1);
            }
            catch
            { }

            if ((No < (pageindex - 1) * perPageRowCount) || No > (pageindex) * perPageRowCount)
            {
                continue;
            }

            sb.Append("<tr align=\"center\" class=\"" + ((No % 2 == 0) ? "BlueWord WhiteBg" : "BlueLightBg2 BlueWord") + "\">");
            sb.Append("<td>" + No.ToString() + "</td>");

            Number = str.Split(';')[1].Substring(1, str.Split(';')[1].Length - 2);
            string[] Numbers = Number.Split('|');

            if (Numbers.Length < 2)
            {
                continue;
            }

            sb.Append("<td height=\"20\">");

            BuyWays = Numbers.Length.ToString() + "串1";

            long MatchID = 0;

            for (int i = 0; i < Numbers.Length; i++)
            {
                if (Numbers[i].IndexOf("(") < 0)
                {
                    continue;
                }

                MatchID = Shove._Convert.StrToLong(Numbers[i].Substring(0, Numbers[i].IndexOf("(")), 1);

                DataRow[] dr = dtMatch.Select("MatchID=" + MatchID.ToString());

                if (dr.Length < 1)
                {
                    continue;
                }

                sb.Append(dr[0]["MatchNumber"].ToString() + "->" + PF.Getesult(PlayID.ToString(), Numbers[i].Substring(Numbers[i].IndexOf("(") + 1, Numbers[i].IndexOf(")") - Numbers[i].IndexOf("(") - 1)) + ";");
            }

            sb.Append("</td>");
            sb.Append("<td height=\"20\">" + BuyWays + "</td>");
            sb.Append("<td>1</td>");
            sb.Append("<td>" + (2 * Multiple).ToString() + "</td></tr>");
        }

        sb.Append("</table>");
        labLotteryNumber.Text = sb.ToString();

        StringBuilder sbpage = new StringBuilder();

        int rowCount = No;
        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (SchemeID < 0)
        {
            sbpage.Append("<div id=\"Pagination\" class=\"yahoo\" style=\"width: auto;\"><span id=\"first\"><a href=\"Scheme.aspx\">首页</a></span>");

            if (pageindex == 1)
            {
                sbpage.Append("<span class=\"disabled\">« 上一页</span>");
            }
            else
            {
                sbpage.Append("<span><a href=\"Scheme.aspx?p=" + (pageindex - 1).ToString() + "\">« 上一页</a></span>");
            }

            for (int i = 0; i < pageCount; i++)
            {
                if (i == pageindex - 1)
                {
                    sbpage.Append("<span class=\"current\">" + (i + 1).ToString() + "</span>");

                    continue;
                }

                if ((i < pageindex + 4 || i < 9) && (i > pageindex - 6 || i > pageCount - 10))
                {
                    sbpage.Append("<a href=\"Scheme.aspx?p=" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a>");
                }
            }

            if (pageindex == pageCount)
            {
                sbpage.Append("<span class=\"disabled\">下一页 »</span>");
            }
            else
            {
                sbpage.Append("<span><a href=\"Scheme.aspx?p=" + (pageindex + 1).ToString() + "\">下一页 »</a></span>");
            }

            sbpage.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"Scheme.aspx?p=" + (pageCount).ToString() + "\">尾页</a></span><span class=\"jilu\">共" + pageCount.ToString() + "页，" + No.ToString() + "条记录</span></div>");
        }
        else
        {
            sbpage.Append("<div id=\"Pagination\" class=\"yahoo\" style=\"width: auto;\"><span id=\"first\"><a href=\"Scheme.aspx?id="+ SchemeID.ToString() +"\">首页</a></span>");

            if (pageindex == 1)
            {
                sbpage.Append("<span class=\"disabled\">« 上一页</span>");
            }
            else
            {
                sbpage.Append("<span><a href=\"Scheme.aspx?id=" + SchemeID.ToString() + "&p=" + (pageindex - 1).ToString() + "\">« 上一页</a></span>");
            }

            for (int i = 0; i < pageCount; i++)
            {
                if (i == pageindex - 1)
                {
                    sbpage.Append("<span class=\"current\">" + (i + 1).ToString() + "</span>");

                    continue;
                }

                if ((i < pageindex + 4 || i < 9) && (i > pageindex - 6 || i > pageCount - 10))
                {
                    sbpage.Append("<a href=\"Scheme.aspx?id=" + SchemeID.ToString() + "&p=" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a>");
                }
            }

            if (pageindex == pageCount)
            {
                sbpage.Append("<span class=\"disabled\">下一页 »</span>");
            }
            else
            {
                sbpage.Append("<span><a href=\"Scheme.aspx?id=" + SchemeID.ToString() + "&p=" + (pageindex + 1).ToString() + "\">下一页 »</a></span>");
            }

            sbpage.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"Scheme.aspx?id=" + SchemeID.ToString() + "&p=" + (pageCount).ToString() + "\">尾页</a></span><span class=\"jilu\">共" + pageCount.ToString() + "页，" + No.ToString() + "条记录</span></div>");
        }

        labLotteryNumber.Text += sbpage.ToString();
    }
}
