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
using System.Text;

public partial class Home_Web_DownloadSchemeFile : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    private void BindData()
    {
        long SchemeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (SchemeID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        DataTable dt = new DAL.Tables.T_Schemes().Open("InitiateUserID,LotteryNumber, PlayTypeID", "SiteID = " + _Site.ID.ToString() + " and [ID] = " + SchemeID.ToString(), "");
      
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        long InitiateUserID = Shove._Convert.StrToLong(dt.Rows[0]["InitiateUserID"].ToString(), -1);

        //既不是发起人，也不在招股对象之内
        if (_User != null && !_User.isCanViewSchemeContent(SchemeID))
        {
            PF.GoError(ErrorNumber.Unknow, "对不起，您不在此方案的招股对象之内。", this.GetType().FullName);

            return;
        }

        string LotteryNumber = dt.Rows[0]["LotteryNumber"].ToString();

        if (Shove._Convert.StrToInt(dt.Rows[0]["PlayTypeID"].ToString(), -1) > 7200 && Shove._Convert.StrToInt(dt.Rows[0]["PlayTypeID"].ToString(), -1) < 7300)
        {
            string CacheKey = "JCZC_Scheme_Bind";

            DataTable dtMatch = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dtMatch == null)
            {
                dtMatch = new DAL.Tables.T_Match().Open("ID, MatchNumber, StopSellingTime", "", "");

                if(dtMatch == null)
                {
                    return;
                }

                if (dtMatch.Rows.Count < 1)
                {
                    return;
                }

                Shove._Web.Cache.SetCache(CacheKey, dtMatch, 3600);
            }

            string CanonicalNumber = "";
            int PlayTypeID = Shove._Convert.StrToInt(dt.Rows[0]["PlayTypeID"].ToString(), 7201);

            ArrayList al = new ArrayList();

            string[] strs = LotteryNumber.Split('\n');

            if (strs == null)
                return;
            if (strs.Length == 0)
                return;

            string CacheKeyNumbers = "Home_Web_DownloadSchemeFile_" + SchemeID.ToString();

            string[] LotteryNumbers = Shove._Web.Cache.GetCacheAsString(CacheKeyNumbers, "").Split('\n');
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

                //if (Numbers.Length < 2)
                //{
                //    continue;
                //}

                sb.Append("<td height=\"20\">");

                if (Numbers.Length == 1)
                {
                    BuyWays = "单关";
                }
                else
                {
                    BuyWays = Numbers.Length.ToString() + "串1";
                }

                long MatchID = 0;

                for (int i = 0; i < Numbers.Length; i++)
                {
                    if (Numbers[i].IndexOf("(") < 0)
                    {
                        continue;
                    }

                    MatchID = Shove._Convert.StrToLong(Numbers[i].Substring(0, Numbers[i].IndexOf("(")), 1);

                    DataRow[] dr = dtMatch.Select("ID=" + MatchID.ToString());

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
                sbpage.Append("<div id=\"Pagination\" class=\"yahoo\" style=\"width: auto;\"><span id=\"first\"><a href=\"DownloadSchemeFile.aspx\">首页</a></span>");

                if (pageindex == 1)
                {
                    sbpage.Append("<span class=\"disabled\">« 上一页</span>");
                }
                else
                {
                    sbpage.Append("<span><a href=\"DownloadSchemeFile.aspx?p=" + (pageindex - 1).ToString() + "\">« 上一页</a></span>");
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
                        sbpage.Append("<a href=\"DownloadSchemeFile.aspx?p=" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a>");
                    }
                }

                if (pageindex == pageCount)
                {
                    sbpage.Append("<span class=\"disabled\">下一页 »</span>");
                }
                else
                {
                    sbpage.Append("<span><a href=\"DownloadSchemeFile.aspx?p=" + (pageindex + 1).ToString() + "\">下一页 »</a></span>");
                }

                sbpage.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"DownloadSchemeFile.aspx?p=" + (pageCount).ToString() + "\">尾页</a></span><span class=\"jilu\">共" + pageCount.ToString() + "页，" + No.ToString() + "条记录</span></div>");
            }
            else
            {
                sbpage.Append("<div id=\"Pagination\" class=\"yahoo\" style=\"width: auto;\"><span id=\"first\"><a href=\"DownloadSchemeFile.aspx?id=" + SchemeID.ToString() + "\">首页</a></span>");

                if (pageindex == 1)
                {
                    sbpage.Append("<span class=\"disabled\">« 上一页</span>");
                }
                else
                {
                    sbpage.Append("<span><a href=\"DownloadSchemeFile.aspx?id=" + SchemeID.ToString() + "&p=" + (pageindex - 1).ToString() + "\">« 上一页</a></span>");
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
                        sbpage.Append("<a href=\"DownloadSchemeFile.aspx?id=" + SchemeID.ToString() + "&p=" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a>");
                    }
                }

                if (pageindex == pageCount)
                {
                    sbpage.Append("<span class=\"disabled\">下一页 »</span>");
                }
                else
                {
                    sbpage.Append("<span><a href=\"DownloadSchemeFile.aspx?id=" + SchemeID.ToString() + "&p=" + (pageindex + 1).ToString() + "\">下一页 »</a></span>");
                }

                sbpage.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"DownloadSchemeFile.aspx?id=" + SchemeID.ToString() + "&p=" + (pageCount).ToString() + "\">尾页</a></span><span class=\"jilu\">共" + pageCount.ToString() + "页，" + No.ToString() + "条记录</span></div>");
            }

            labLotteryNumber.Text += sbpage.ToString();

        }
        else
        {
            LotteryNumber = PF.GetScriptResTable(LotteryNumber);

            if (LotteryNumber.IndexOf("table") < 0)
            {
                LotteryNumber = Shove._Convert.ToHtmlCode(LotteryNumber);
            }

            labLotteryNumber.Text = (LotteryNumber == "") ? "未找到相关数据。" : (LotteryNumber + "&nbsp;");
        }
    }
}