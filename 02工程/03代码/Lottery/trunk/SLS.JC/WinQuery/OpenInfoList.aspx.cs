using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Shove.Database;

public partial class WinQuery_OpenInfoList : SitePageBase
{
    private string dingZhi = "";
    public string LotteryName = "";
    public int LotteryID = 5;
    public string BuyUrl = "";
    public string TrendUrl = "";
    public string IsusesName = "";
    private string LotteryCode = "";

    private void GoToBuy()
    {
        if (LotteryID == 5)
        {
            BuyUrl = "/Lottery/Buy_SSQ.aspx";
            TrendUrl = "/TrendCharts/SSQ/SSQ_CGXMB.aspx";
            LotteryCode = "ssq";
        }
        else if (LotteryID == 6)
        {
            BuyUrl = "/Lottery/Buy_3D.aspx";
            TrendUrl = "/TrendCharts/FC3D/FC3D_ZHXT.aspx";
            LotteryCode = "fc3d";
        }
        else if (LotteryID == 59)
        {
            BuyUrl = "/Lottery/Buy_15X5.aspx";
            TrendUrl = "/TrendCharts/HD15X5/C15X5_CGXMB.aspx";
            LotteryCode = "15x5";
        }
        else if (LotteryID == 13)
        {
            BuyUrl = "/Lottery/Buy_QLC.aspx";
            TrendUrl = "/TrendCharts/QLC/7LC_CGXMB.aspx";
            LotteryCode = "qlc";
        }
        else if (LotteryID == 58)
        {
            BuyUrl = "/Lottery/Buy_DF6J1.aspx";
            TrendUrl = "/TrendCharts/DF6J1/DF61_ZHFB.aspx";
            LotteryCode = "df6j1";
        }
        else if (LotteryID == 39)
        {
            BuyUrl = "/Lottery/Buy_CJDLT.aspx";
            TrendUrl = "/TrendCharts/CJDLT/Default.aspx";
            LotteryCode = "cjdlt";
        }
        else if (LotteryID == 63)
        {
            BuyUrl = "/Lottery/Buy_PL3.aspx";
            TrendUrl = "/TrendCharts/PL3/Default.aspx";
            LotteryCode = "pl3";
        }
        else if (LotteryID == 3)
        {
            BuyUrl = "/Lottery/Buy_QXC.aspx";
            TrendUrl = "/TrendCharts/7Xing/Default.aspx";
            LotteryCode = "qxc";
        }
        else if (LotteryID == 64)
        {
            BuyUrl = "/Lottery/Buy_PL5.aspx";
            TrendUrl = "/TrendCharts/PL5/Default.aspx";
            LotteryCode = "pl5";
        }
        else if (LotteryID == 9)
        {
            BuyUrl = "/Lottery/Buy_22X5.aspx";
            TrendUrl = "/TrendCharts/TC22X5/Default.aspx";
            LotteryCode = "22x5";
        }
        else if (LotteryID == 65)
        {
            BuyUrl = "/Lottery/Buy_31X7.aspx";
            TrendUrl = "/TrendCharts/SSQ/SSQ_CGXMB.aspx";
            LotteryCode = "31x7";
        }
        else if (LotteryID == 29)
        {
            BuyUrl = "/Lottery/Buy_SSL.aspx";
            TrendUrl = "/TrendCharts/SHSSL/SHSSL_ZHFB.aspx";
            LotteryCode = "ssl";
        }
        else if (LotteryID == 62)
        {
            BuyUrl = "/Lottery/Buy_SYYDJ.aspx";
            TrendUrl = "/TrendCharts/SYYDJ/SYDJ_FBZS.aspx";
            LotteryCode = "syydj";
        }
        else if (LotteryID == 74)
        {
            BuyUrl = "/Lottery/Buy_SFC.aspx";
            TrendUrl = "";
            LotteryCode = "sfc";
        }
        else if (LotteryID == 75)
        {
            BuyUrl = "/Lottery/Buy_SFC_9_D.aspx";
            TrendUrl = "";
            LotteryCode = "rxjc";
        }
        else if (LotteryID == 2)
        {
            BuyUrl = "/Lottery/Buy_JQC.aspx";
            TrendUrl = "/TrendCharts/SSQ/SSQ_CGXMB.aspx";
            LotteryCode = "jqc";
        }
        else if (LotteryID == 15)
        {
            BuyUrl = "/Lottery/Buy_LCBQC.aspx";
            TrendUrl = "/TrendCharts/SSQ/SSQ_CGXMB.aspx";
            LotteryCode = "lcbqc";
        }
        else if (LotteryID == 61)
        {
            BuyUrl = "/Lottery/Buy_SSC.aspx";
            TrendUrl = "/TrendCharts/JXSSC/SSC_5X_ZHFB.aspx";
            LotteryCode = "ssc";
        }
        else if (LotteryID == 61)
        {
            BuyUrl = "/Lottery/Buy_HN22X5.aspx";
            TrendUrl = "/TrendCharts/HN22X5/22X5_HMZST.aspx";
            LotteryCode = "hn22x5";
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), 5);

            if (!DataCache.Lotteries.ContainsKey(LotteryID))
            {
                LotteryID = DataCache.Lotteries.First().Key;
            }
            GoToBuy();
            LotteryName = DataCache.Lotteries[LotteryID];

            HidLotteryID.Value = LotteryID.ToString();

            BindOpenInfoList(LotteryID, LotteryCode);

            long IsuseID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("IsuseID"), 0);
            HidIsuseID.Value = IsuseID.ToString();

            long PlayTypeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("PlayTypeID"), 0);
            HidPlayType.Value = PlayTypeID.ToString();

            string SearchValue = System.Web.HttpUtility.UrlDecode(Shove._Web.Utility.GetRequest("Search"));

            if (SearchValue != "" && SearchValue != "0")
            {
                HidSearch.Value = Shove._Web.Cache.GetCache(SearchValue).ToString();
                tbSearch.Text = HidSearch.Value;
                Shove._Web.Cache.ClearCache(HidSearch.Value);
            }

            BindDataForIsuses(LotteryID);
        }
    }

    private void BindOpenInfoList(int LotteryID, string LotteryCode)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("");

        string IsuseName = "";
        string WinNumber = "";
        DataTable dt = GetWinNumber(LotteryID, LotteryCode);

        sb.Append("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IsuseName = dt.Rows[i]["Name"].ToString();
            WinNumber = dt.Rows[i]["WinLotteryNumber"].ToString();

            if (i % 2 == 0)
            {
                sb.AppendLine("<tr>")
                .AppendLine("<td width='50%' height='28'>")
                .Append("<a href='" + LotteryID + "-" + dt.Rows[i]["ID"].ToString() + "-0.aspx' target='_blank'><span class='hui14'>&#9642;</span>&nbsp;第" + IsuseName + "期&nbsp;&nbsp;&nbsp;&nbsp;")
                .Append("<span class='red12'>" + WinNumber + "</span>")
                .AppendLine("</a></td>");
            }
            else
            {

                sb.AppendLine("<td width='50%' height='28'>")
                 .Append("<a href='" + LotteryID + "-" + dt.Rows[i]["ID"].ToString() + "-0.aspx' target='_blank'><span class='hui14'>&#9642;</span>&nbsp;第" + IsuseName + "期&nbsp;&nbsp;&nbsp;&nbsp;")
                .Append("<span class='red12'>" + WinNumber + "</span>")
                .AppendLine("</a></td>")
                .AppendLine("</tr>");

            }
        }
        sb.Append("</table>");

        LatestOpenInfo.InnerHtml = sb.ToString();
    }

    //绑定开奖号码
    private DataTable GetWinNumber(int LotteryID, string LotteryCode)
    {
        string key = LotteryCode + "WinNumber" + LotteryID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(key);

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_Isuses().Open("top 10 ID, Name, WinLotteryNumber, EndTime", "LotteryID=" + LotteryID + " and IsOpened = 1 and IsNull(WinLotteryNumber,'')<>''", "EndTime Desc");

            if (dt == null)
            {
                return null;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(key, dt, 600);
            }
        }

        return dt;
    }

    private void BindDataForIsuses(int LotteryID)
    {
        string CacheKey = "Isuses";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = MSSQL.Select("select ID,EndTime,WinLotteryNumber,OpenAffiche,Name,LotteryID from T_Isuses where IsOpened = 1 and EndTime < GETDATE() order by EndTime desc");
            if (dt == null)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 3600);
        }

        ddlIsuses.Items.Clear();

        DataRow[] drIsuse = dt.Select("LotteryID=" + LotteryID.ToString());

        foreach (DataRow dr in drIsuse)
        {
            ListItem li = new ListItem(dr["Name"].ToString(), dr["ID"].ToString());

            ddlIsuses.Items.Add(li);
        }

        imgLogo.ImageUrl = "/Home/Room/Images/LotteryWinLogo/" + new SLS.Lottery()[LotteryID].code + ".jpg";

        int IsuseID = Shove._Convert.StrToInt(HidIsuseID.Value, 0);

        if (ddlIsuses.Items.Count > 0)
        {
            if (ddlIsuses.Items.FindByValue(IsuseID.ToString()) != null)
            {
                ddlIsuses.SelectedValue = IsuseID.ToString();
            }

            lbIsuse.Text = ddlIsuses.SelectedItem.Text.Trim();
            IsusesName = lbIsuse.Text;

            HidIsuseID.Value = ddlIsuses.SelectedValue;

            string CacheKeyIsusess = "Isuses";
            DataTable dtIsusess = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyIsusess);

            if (dtIsusess == null)
            {
                dtIsusess = MSSQL.Select("select ID,EndTime,WinLotteryNumber,OpenAffiche,Name,LotteryID from T_Isuses where IsOpened = 1 and EndTime < GETDATE() order by EndTime desc");
                if (dtIsusess == null || dtIsusess.Rows.Count < 1)
                {
                    PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                    return;
                }

                Shove._Web.Cache.SetCache(CacheKeyIsusess, dtIsusess, 0);
            }

            DataRow Isuse = dtIsusess.Select("ID = " + ddlIsuses.SelectedValue)[0];

            tbWinNumber.InnerHtml = FormatWinNumber(Shove._Convert.StrToInt(HidLotteryID.Value, 5), Isuse["WinLotteryNumber"].ToString());
            lbWinInfo.Text = Isuse["OpenAffiche"].ToString();

            BindDataForPlayTypes(int.Parse(HidLotteryID.Value));
        }
    }

    private void BindDataForPlayTypes(int LotteryID)
    {
        //玩法信息缓存 6000 秒
        string CacheKey = "dtPlayTypes_" + LotteryID.ToString();
        DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtPlayTypes == null)
        {
            dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("", "LotteryID in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ") and LotteryID = " + LotteryID.ToString(), "[ID]");

            if (dtPlayTypes == null || dtPlayTypes.Rows.Count < 1)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtPlayTypes, 6000);
        }

        ddlPlayTypes.Items.Clear();
        ddlPlayTypes.Items.Add(new ListItem("全部玩法", "0"));

        foreach (DataRow dr in dtPlayTypes.Rows)
        {
            ListItem li = new ListItem(dr["Name"].ToString(), dr["ID"].ToString());

            ddlPlayTypes.Items.Add(li);
        }

        int PlayTypeID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PlayTypeID"), 0);

        if (ddlPlayTypes.Items.Count > 0)
        {
            if (ddlPlayTypes.Items.FindByValue(PlayTypeID.ToString()) != null)
            {
                ddlPlayTypes.SelectedValue = PlayTypeID.ToString();
            }

            HidPlayType.Value = ddlPlayTypes.SelectedValue;

            BindDataForWinDetail(int.Parse(HidLotteryID.Value), int.Parse(HidIsuseID.Value));
        }
    }

    protected void ddlIsuses_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Shove._Web.Utility.FilteSqlInfusion(HidLotteryID.Value) + "-" + Shove._Web.Utility.FilteSqlInfusion(ddlIsuses.SelectedValue) + "-0.aspx");
    }

    protected void ddlPlayTypes_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(Shove._Web.Utility.FilteSqlInfusion(HidLotteryID.Value) + "-" + Shove._Web.Utility.FilteSqlInfusion(HidIsuseID.Value) + "-" + Shove._Web.Utility.FilteSqlInfusion(ddlPlayTypes.SelectedValue) + ".aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string SearchValue = (tbSearch.Text.Trim() == "(可输入发起人ID或方案编号搜索)" || tbSearch.Text.Trim() == "") ? "" : tbSearch.Text.Trim();
        string CacheKey = System.DateTime.Now.ToString("yyyyMMddhhmmss");
        Shove._Web.Cache.SetCache(CacheKey, SearchValue);
        Response.Redirect(Shove._Web.Utility.FilteSqlInfusion(HidLotteryID.Value) + "-" + Shove._Web.Utility.FilteSqlInfusion(HidIsuseID.Value) + "-" + Shove._Web.Utility.FilteSqlInfusion(ddlPlayTypes.SelectedValue) + "-" + CacheKey + ".aspx");
    }

    //本站中奖情况
    private void BindDataForWinDetail(int LotteryID, int IsuseID)
    {
        rptWinDetail.DataBind();

        int pageNumber = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("PID"), 1);

        if (pageNumber <= 0)
        {
            pageNumber = 1;
        }

        string CacheKey = "WinDetailIsuse_" + IsuseID.ToString() + "_" + HidPlayType.Value + (HidSearch.Value == "" ? "" : " and (InitiateName like '%" + HidSearch.Value + "%' or SchemeNumber like '%" + HidSearch.Value + "%')");
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            StringBuilder sbSQL = new StringBuilder();
            if (Shove._Web.Utility.FilteSqlInfusion(HidPlayType.Value) != "0")    //特定玩法
            {
                sbSQL.AppendLine("select a.*,b.Name as InitiateName,c.Name as PlayTypeName from ")
                    .AppendLine("(")
                    .AppendLine("	select ID,InitiateUserID,PlayTypeID,[Money],SchemeNumber,Share,WinDescription,Multiple,WinMoneyNoWithTax,InitiateBonus  from T_Schemes ")
                    .AppendLine("		where WinMoney > 0 and IsuseID = @IsuseID and  QuashStatus = 0 and PlayTypeID = @PlayTypeID")
                    .AppendLine(") a")
                    .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                    .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                    .AppendLine((HidSearch.Value == "" ? "" : " where (b.Name like @InitiateName or a.SchemeNumber like @InitiateName)"))
                    .AppendLine("order by a.WinMoneyNoWithTax desc");
                dt = MSSQL.Select(sbSQL.ToString(),
                        new MSSQL.Parameter("IsuseID", SqlDbType.Int, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(IsuseID.ToString())),
                        new MSSQL.Parameter("PlayTypeID", SqlDbType.Int, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(HidPlayType.Value)),
                        new MSSQL.Parameter("InitiateName", SqlDbType.VarChar, 0, ParameterDirection.Input, "%" + Shove._Web.Utility.FilteSqlInfusion(HidSearch.Value) + "%"));
            }
            else     //全部玩法
            {
                sbSQL.AppendLine("select a.*,b.Name as InitiateName,c.Name as PlayTypeName from ")
                    .AppendLine("(")
                    .AppendLine("	select ID,InitiateUserID,PlayTypeID,[Money],SchemeNumber,Share,WinDescription,Multiple,WinMoneyNoWithTax,InitiateBonus  from T_Schemes ")
                    .AppendLine("		where WinMoney > 0 and IsuseID = @IsuseID and  QuashStatus = 0 ")
                    .AppendLine(") a")
                    .AppendLine("inner join T_Users b on a.InitiateUserID = b.ID")
                    .AppendLine("inner join T_PlayTypes c on a.PlayTypeID = c.ID")
                    .AppendLine((HidSearch.Value == "" ? "" : " where (b.Name like @InitiateName or a.SchemeNumber like @InitiateName)"))
                    .AppendLine("order by a.WinMoneyNoWithTax desc");

                dt = MSSQL.Select(sbSQL.ToString(),
                        new MSSQL.Parameter("IsuseID", SqlDbType.Int, 0, ParameterDirection.Input, Shove._Web.Utility.FilteSqlInfusion(IsuseID.ToString())),
                        new MSSQL.Parameter("InitiateName", SqlDbType.VarChar, 0, ParameterDirection.Input, "%" + Shove._Web.Utility.FilteSqlInfusion(HidSearch.Value) + "%"));
            }

            if (dt == null)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            dt.Columns.Add("TmpID", Type.GetType("System.Int32"));
            dt.Columns.Add("WinDescription2", System.Type.GetType("System.String"));
            dt.Columns.Add("ShareWinMoney", System.Type.GetType("System.String"));
            dt.Columns.Add("EachMonney", System.Type.GetType("System.String"));

            int c = dt.Rows.Count;

            for (int r = 0; r < c; r++)
            {
                dt.Rows[r]["TmpID"] = r + 1;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 600);
        }

        int perPageRowCount = 20;
        int rowCount = dt.Rows.Count;
        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (pageNumber > pageCount)
        {
            pageNumber = pageCount;
        }


        int showPageCount = 10;
        int pageFirstID = pageNumber % showPageCount == 0 ? showPageCount * (pageNumber / showPageCount - 1) + 1 : showPageCount * (pageNumber / showPageCount) + 1;
        int prePageID = (pageFirstID / showPageCount + 1) == 1 ? 1 : showPageCount * (pageFirstID / showPageCount - 1) + 1;
        int nextPageID = pageCount > showPageCount * (pageNumber / showPageCount + 2) ? showPageCount * (pageNumber / showPageCount + 1) + 1 : pageCount;

        if (pageFirstID + showPageCount > pageCount)
        {
            pageFirstID = pageCount - showPageCount + 1;
        }

        if (pageFirstID <= 0)
        {
            pageFirstID = 1;
        }

        DataRow[] drShow = dt.Select("TmpID > " + (perPageRowCount * (pageNumber - 1)).ToString() + " and TmpID < " + (perPageRowCount * pageNumber + 1).ToString());

        DataTable dtShow = dt.Clone();

        foreach (DataRow dr in drShow)
        {
            dtShow.Rows.Add(dr.ItemArray);
        }

        int Count = dtShow.Rows.Count;

        for (int i = 0; i < Count; i++)
        {
            string str = drShow[i]["WinDescription"].ToString();
            dtShow.Rows[i]["WinDescription2"] = str;
            double WinMoneyNoWithTax = Shove._Convert.StrToDouble(drShow[i]["WinMoneyNoWithTax"].ToString(), 0);
            double InitiateBonus = Shove._Convert.StrToDouble(drShow[i]["InitiateBonus"].ToString(), 0);
            int Share = Shove._Convert.StrToInt(drShow[i]["Share"].ToString(), 0);
            int PlayTypeID = Shove._Convert.StrToInt(drShow[i]["PlayTypeID"].ToString(), 0);
            double Money = Shove._Convert.StrToDouble(drShow[i]["Money"].ToString(), 0);

            if (Share == 1)
            {
                dtShow.Rows[i]["ShareWinMoney"] = WinMoneyNoWithTax.ToString("N");
                dtShow.Rows[i]["EachMonney"] = Money;
            }
            else
            {
                dtShow.Rows[i]["ShareWinMoney"] = Math.Round((WinMoneyNoWithTax - InitiateBonus) / Share, 2).ToString("N");
                dtShow.Rows[i]["EachMonney"] = Convert.ToDouble(Money / Share).ToString("N");
            }

            dtShow.Rows[i]["Money"] = Shove._Convert.StrToDouble(drShow[i]["Money"].ToString(), 0).ToString("N");
            dtShow.Rows[i]["WinMoneyNoWithTax"] = WinMoneyNoWithTax.ToString("N");

            dtShow.AcceptChanges();
        }

        rptWinDetail.DataSource = dtShow;
        rptWinDetail.DataBind();

        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>")
            .Append("<td width='31%' height='36' align='left' class='black12'>")
            .Append("第<span class='red'>").Append(pageNumber.ToString()).Append("/").Append(pageCount.ToString()).Append("</span>页 <span class='red'>").Append(perPageRowCount.ToString()).Append("</span>条/页 共<span class='red'>").Append(rowCount.ToString()).Append("</span>条")
            .Append("</td>")
            .Append("<td width='69%' align='right'>")
            .Append("<table border='0' cellspacing='4' cellpadding='0'>")
            .Append("<tbody style='text-align:center; width:20px;'>")
            .Append("<tr>")
            .Append("<td valign='middle' class='ball' onclick='showPage(1,").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",\"").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("\");'>")
            .Append("<img src='/Home/Room/Images/page_first.gif' width='9' height='8' />")
            .Append("</td>")
            .Append("<td class='ball' onclick='showPage(").Append(prePageID.ToString()).Append(",").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",\"").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("\");'>")
            .Append("<img src='/Home/Room/Images/page_previous.gif' width='9' height='8' />");

        for (int p = pageFirstID; p < pageFirstID + showPageCount && p <= pageCount; p++)
        {
            sb.Append("</td>")
                .Append("<td id='page_").Append(p.ToString()).Append("' class='ball").Append(pageNumber == p ? "_r" : p <= pageCount ? "" : "_c").Append("'");

            if (p <= pageCount)
            {
                sb.Append(" onclick='showPage(").Append(p.ToString()).Append(",").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",\"").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("\");'");
            }
            sb.Append(">")
                .Append(p.ToString())
                .Append("</td>");
        }

        sb.Append("<td style='width:36px;'>")
            .Append("<input type='text' class='ball_50' id='txtgopage' maxlength='").Append(pageCount.ToString().Length.ToString()).Append("' />")
            .Append("</td>")
            .Append("<td class='ball' onclick=\"showPage(document.getElementById('txtgopage').value,").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",'").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("');\">")
            .Append("<img src='/Home/Room/Images/page_go.gif' width='11' height='8' />")
            .Append("</td>")
            .Append("<td class='ball' onclick='showPage(").Append(nextPageID.ToString()).Append(",").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",\"").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("\");'>")
            .Append("<img src='/Home/Room/Images/page_next.gif' width='9' height='8' />")
            .Append("</td>")
            .Append("<td class='ball' onclick='showPage(").Append(pageCount.ToString()).Append(",").Append(LotteryID.ToString()).Append(",").Append(HidIsuseID.Value).Append(",").Append(HidPlayType.Value).Append(",\"").Append(System.Web.HttpUtility.UrlEncode(HidSearch.Value)).Append("\");'>")
            .Append("<img src='/Home/Room/Images/page_last.gif' width='9' height='8' />")
            .Append("</td>")
            .Append("</tr>")
            .Append("</tbody>")
            .Append("</table>")
            .Append("</td>")
            .Append("</tr>");

        tbPaging.InnerHtml = sb.ToString();
    }

    private string FormatWinNumber(int LotteryID, string winNumber)
    {
        StringBuilder sb = new StringBuilder();

        switch (LotteryID)
        {
            case 3:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }
                break;

            case 5:
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 1)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                        }
                    }
                }

                break;

            case 6:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }

                break;

            case 29:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }

                break;

            case 39:
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 2)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                        }
                    }
                }

                break;

            case 58:
                if (winNumber.IndexOf("+") > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        if (i < winNumber.Length - 2)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                        }
                        else
                        {
                            if (i == winNumber.Length - 1)
                            {
                                sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(winNumber.Substring(i, 1));
                            }
                        }
                    }
                }

                break;

            case 59:
                if (winNumber.Length > 0)
                {
                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length && i < 5; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                    }
                }

                break;

            case 13:
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 1)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                        }
                    }
                }

                break;

            case 9:
                if (winNumber.Length > 0)
                {
                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                    }
                }

                break;

            case 14:
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 1)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                        }
                    }
                }

                break;

            case 64:
            case 61:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }

                break;

            case 63:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }

                break;

            case 19:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }

                break;

            case 74:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }
                break;

            case 75:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }
                break;

            case 2:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }
                break;

            case 15:
                if (winNumber.Length > 0)
                {
                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber.Substring(i, 1));
                    }
                }
                break;

            case 62:
                if (winNumber.Length > 0)
                {
                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                    }
                }

                break;

            case 41:
                if (winNumber.IndexOf("-") > 0)
                {
                    winNumber = winNumber.Replace("-", "");

                    for (int i = 0; i < winNumber.Length; i++)
                    {
                        if (i < winNumber.Length - 1)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(winNumber[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(winNumber[i]);
                        }
                    }
                }

                break;

            case 65:
                if (winNumber.IndexOf(" + ") > 0)
                {
                    winNumber = winNumber.Replace(" + ", " ");

                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length; i++)
                    {
                        if (i < number.Length - 1)
                        {
                            sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                        }
                        else
                        {
                            sb.Append("</td><td align='center' class='white14' style='width: 25px;background-image: url(/Home/Room/Images/zfb_blueball.gif)'>").Append(number[i]);
                        }
                    }
                }

                break;

            case 69:
                if (winNumber.Length > 0)
                {
                    string[] number = winNumber.Split(' ');

                    for (int i = 0; i < number.Length && i < 5; i++)
                    {
                        sb.Append("</td><td align='center' class='white14' style='width:25px;background-image: url(/Home/Room/Images/zfb_redball.gif)'>").Append(number[i]);
                    }
                }

                break;


            default:
                break;
        }

        return sb.ToString();
    }

    protected void btn_Single_Click(object sender, EventArgs e)
    {
        ResponseTailor(true, Shove._Convert.StrToLong(((ImageButton)sender).CommandArgument, 0));
    }

    private void ResponseTailor(bool b, long userid)
    {
        int lotteryid = Shove._Convert.StrToInt(HidLotteryID.Value, -1);
        int temp = -1;
        if (b)
        {
            temp = lotteryid;
        }
        string headMethod = "followScheme(" + temp + ");$Id(\"iframeFollowScheme\").src=\"/Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=" + temp + "&DzLotteryID=" + temp;
        if (userid < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(lotteryid))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }
        Users tu = new Users(_Site.ID);
        tu.ID = userid;
        dingZhi = "&FollowUserID=" + userid + "&FollowUserName=" + HttpUtility.UrlEncode(tu.Name) + "\"";
        string ReturnDescription = "";
        if (tu.GetUserInformationByID(ref ReturnDescription) != 0)
        {

            return;
        }


        dingZhi = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), headMethod + dingZhi);

        if ((lotteryid != 72) || (lotteryid != 73))
        {
            Response.Redirect("/Lottery/" + DataCache.LotterieBuyPage[lotteryid] + "?DZ=" + dingZhi + "");
        }
    }

    protected void btn_All_Click(object sender, ImageClickEventArgs e)
    {
        ResponseTailor(false, Shove._Convert.StrToLong(((ImageButton)sender).CommandArgument, 0));
    }
}