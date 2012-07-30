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

public partial class Home_Room_MyChallenge : RoomPageBase
{
    protected string uId = "";    

    protected void Page_Load(object sender, EventArgs e)
    {
        // 初始化AjaxPro
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_MyChallenge), this.Page);
        if (!this.IsPostBack)
        {
            if (_User == null)
            {
                Response.Redirect("/UserLogin.aspx?RequestLoginPage=/Home/Room/MyChallenge.aspx");   
            }            
            this.userId.Value = _User.ID.ToString();
            uId = _User.ID.ToString();
            // 计算总积分
            SetSumScoreAndSumNotes();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion
    
    #region 擂台记录

    private void SetSumScoreAndSumNotes()
    {
        if (string.IsNullOrEmpty(uId.Trim()))
        {
            return;
        }
        object o = Shove.Database.MSSQL.ExecuteScalar("select [Score] from T_ChallengeBetRed where UserId = " + uId);
        if (o == null) return;
        this.lblSumWinMoney.Text = ((int)Shove._Convert.StrToDouble(o.ToString(), 0)).ToString();

        object save = Shove.Database.MSSQL.ExecuteScalar("select Count(*) from T_ChallengeSaveScheme where InitiateUserId = " + uId);
        if (save == null) save = "0";
        this.lblRewardCount.Text = save.ToString();
    }

    [AjaxPro.AjaxMethod]
    public string GetPage(string userId, string Year, string Month, int pageIndex)
    {
        uId = userId;

        string html = "";

        string sql = "";
        if (string.IsNullOrEmpty(Year) && string.IsNullOrEmpty(Month))
        {
            sql = string.Format("select [ID],row_number() over (order by ([DateTime]) desc) as Ranking ,[LotteryNumber],[WinMoney] as score,[Odds],[DateTime] as DateTimes from T_ChallengeScheme where InitiateUserId = {0} and [DateTime] >= '" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-01' order by ([DateTime]) desc", uId);
        }
        else
        {
            if (Shove._Convert.StrToInt(Year, 0) < 1990 || Shove._Convert.StrToInt(Year, 0) > DateTime.Now.Year)
            {
                return "日期格式不正确";
            }
            if (Shove._Convert.StrToInt(Month, 0) < 1 || Shove._Convert.StrToInt(Month, 0) > 12)
            {
                return "日期格式不正确";
            }
            sql = string.Format("select [ID],row_number() over (order by ([DateTime]) desc) as Ranking ,[LotteryNumber],[WinMoney] as score,[Odds],[DateTime] as DateTimes from T_ChallengeScheme where InitiateUserId = {0} and [DateTime] >'{1}-{2}-1'  and [DateTime] < '{3}-{4}-1' order by ([DateTime]) desc"
                    , uId, Year, Month, Year, (Shove._Convert.StrToInt(Month, DateTime.Now.Month) + 1).ToString());
        }

        System.Data.DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (dt == null)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }

        if (dt.Rows.Count < 1)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }

        try
        {
           dt.Columns.Add("EndTime", Type.GetType("System.String"));
        }
        catch { }

        dt = GetLotteryNumberAnalyse(dt);

        string[] Colums = { "DateTime", "BetCount", "BetWay", "LotteryNumber", "score", "ID"};

        _Pages(dt, Colums, 20, pageIndex, ref html);

        return html;
    }

    public string _Pages(DataTable dt, string[] column, int pageRow, int pageIndex, ref string returnHtml)
    {
        if (dt == null)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }
        if (dt.Rows.Count < 1)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }
        if (column == null)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }
        if (column.Length < 1)
        {
            return "<tr><td colspan=\"6\">没有您的投注记录</td></tr>";
        }
        if (pageRow < 1)
        {
            pageRow = 10;
        }


        returnHtml = "<tr><td colspan=\"6\">没有您的投注记录</td></tr>"; //
        int SumPageCount = 0;   // 总页数


        // 总行
        SumPageCount = dt.Rows.Count;
        int xRows = 0;
        // 一行多少页
        if (pageIndex * pageRow > SumPageCount)
        {
            xRows = pageRow - (pageRow - SumPageCount % pageRow);
        }
        else
        {
            xRows = pageRow;
        }
                
        // 拼接Html
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int row = (pageIndex - 1) * pageRow; row < (pageIndex - 1) * pageRow + xRows; row++)
        {
            sb.AppendLine("<tr>");
            for (int cols = 0; cols < column.Length; cols++)
            {
                if (column.Length - 1 == cols)
                {// 执行最后一列 ，我要投注
                    DateTime time = Shove._Convert.StrToDateTime(dt.Rows[row]["EndTime"].ToString(), "2011-08-14 10:00:00");
                   
                    if (DateTime.Compare(time,DateTime.Now) > 0)
                    {
                        sb.AppendLine("<td height=\"30px\"><a href=\"BuyConfirm.aspx?id=" + dt.Rows[row][column[cols]] + "&type=1\"><img src=\"/Home/Room/Images/bet.png\" /></a></td>");
                    }
                    else
                    {
                        sb.AppendLine("<td height=\"30px\">----</td>");
                    }                    
                }
                else
                {
                    sb.AppendLine("<td height=\"30px\">" + dt.Rows[row][column[cols]] + "</td>");
                }
            }
            sb.AppendLine("</tr>");
        }
        // 一共有多少页
        int SumPageNumber = SumPageCount % pageRow == 0 ? SumPageCount / pageRow : SumPageCount / pageRow + 1;
        // 制作 选也按钮
        sb.AppendLine("<tr style=\"height:20px;\">");
        sb.AppendLine("<td colspan=\"" + (column.Length) + "\" align=\"center\"  style=\"border:0px;\">");

        // 显示分页的索引
        int index = pageIndex < 2 ? 1 : pageIndex - 1;

        //Top
        if (pageIndex > 1)
        { // 当前页大于1
            sb.AppendLine("<a style=\"\" href=\"javascript:Changepage(1)\">&lt;&lt;</a>");
            sb.AppendLine("<a style=\"\" href=\"javascript:Changepage(" + (pageIndex - 1).ToString() + ")\">上一页</a>");
        }
        else
        {
            sb.AppendLine("<span style=\"color:Gray\">&lt;&lt;</span>");
            sb.AppendLine("<span style=\"color:Gray\">上一页</span>");
        }

        for (int i = 0; i < 6; i++)
        {
            if (SumPageNumber < index)
            {
                break;
            }
            if (index < 1)
            {
                index = 1;
            }
            if (SumPageCount - index > 1 && i == 5)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\"><a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a> ..</span>");
            }
            else if (index > 1 && i == 0)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\">.. <a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a></span>");
            }
            else if (SumPageNumber == 1)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\">1</span>");
            }
            else
            {

                if (pageIndex == index)
                {
                    sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;color:Gray;\">" + index + "</span>");
                }
                else
                {
                    sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\"><a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a></span>");
                }
            }

            index++;
        }

        Shove._Web.JavaScript.Alert(this.Page, "pageIndex :" + pageIndex + "  ,,SumPageNumber" + SumPageNumber);
        // Down
        if (pageIndex == SumPageNumber)
        {
            sb.AppendLine("<span style=\"color:Gray\">下一页</span>");
            sb.AppendLine("<span style=\"color:Gray\">&gt;&gt;</span>");
        }
        else
        {
            sb.AppendLine("<a href=\"javascript:Changepage(" + (pageIndex + 1).ToString() + ")\">下一页</a>");
            sb.AppendLine("<a href=\"javascript:Changepage(" + SumPageNumber + ")\">&gt;&gt;</a>");
        }

        sb.AppendLine("</td></tr>");

        // 返回的Html代码
        returnHtml = sb.ToString();

        return returnHtml;
    }

    // 保存记录
    [AjaxPro.AjaxMethod]
    public string GetPages_Save(string userId, int pageIndex)
    {
        string sql = "select top 100 ID,DateTime,IsuseID,PlayTypeID,LotteryNumber,IsOpened,Odds from T_ChallengeSaveScheme where InitiateUserId = " + userId + " order by DateTime desc";
        DataTable dt = Shove.Database.MSSQL.Select(sql);
        string innerhtml = "";
        try
        {
            dt.Columns.Add("EndTime", Type.GetType("System.String"));
        }
        catch { }

        string[] Colums = { "DateTime", "BetCount", "BetWay", "LotteryNumber", "ID" };

        dt = GetLotteryNumberAnalyse_Save(dt);

        _SaveSchemePages(dt, Colums, 18, pageIndex, ref innerhtml);

        return innerhtml;
    }

    public string _SaveSchemePages(DataTable dt, string[] column, int pageRow, int pageIndex, ref string returnHtml)
    {
        if (dt == null)
        {
            returnHtml = "<tr align=\"center\"><td colspan=\"6\">没有您的投注记录</td></tr>";
            return "";
        }
        if (dt.Rows.Count < 1)
        {
            returnHtml = "<tr align=\"center\"><td colspan=\"6\">没有您的投注记录</td></tr>";
            return "";
        }
        if (column == null)
        {
            returnHtml = "<tr align=\"center\"><td colspan=\"6\">没有您的投注记录</td></tr>";
            return "";
        }
        if (column.Length < 1)
        {
            returnHtml = "<tr align=\"center\"><td colspan=\"6\">没有您的投注记录</td></tr>";
            return "";
        }
        if (pageRow < 1)
        {
            pageRow = 10;
        }


        returnHtml = "<tr align=\"center\"><td colspan=\"6\">没有您的投注记录</td></tr>"; //
        int SumPageCount = 0;   // 总页数


        // 总行
        SumPageCount = dt.Rows.Count;
        int xRows = 0;
        // 一行多少页
        if (pageIndex * pageRow > SumPageCount)
        {
            xRows = pageRow - (pageRow - SumPageCount % pageRow);
        }
        else
        {
            xRows = pageRow;
        }
                
        // 拼接Html
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int row = (pageIndex - 1) * pageRow; row < (pageIndex - 1) * pageRow + xRows; row++)
        {
            sb.AppendLine("<tr align=\"center\">");
            for (int cols = 0; cols < column.Length; cols++)
            {
                if (column.Length - 1 == cols)
                {
                    DateTime time = Shove._Convert.StrToDateTime(dt.Rows[row]["EndTime"].ToString(), "2011-08-14 10:00:00");

                    if (DateTime.Compare(time, DateTime.Now) > 0)
                    {
                        sb.AppendLine("<td height=\"30px\"><a href=\"BuyConfirm.aspx?id=" + dt.Rows[row][column[cols]] + "&type=2\"><img src=\"/Home/Room/Images/bet.png\" /></a></td>");
                    }
                    else
                    {
                        sb.AppendLine("<td height=\"30px\">----</td>");
                    }  
                }
                else
                {
                    sb.AppendLine("<td height=\"30px\">" + dt.Rows[row][column[cols]] + "</td>");
                }
            }
            sb.AppendLine("</tr>");
        }
        // 一共有多少页
        int SumPageNumber = SumPageCount % pageRow == 0 ? SumPageCount / pageRow : SumPageCount / pageRow + 1;
        // 制作 选也按钮
        sb.AppendLine("<tr style=\"height:20px;\">");
        sb.AppendLine("<td colspan=\"" + (column.Length) + "\" align=\"center\"  style=\"border:0px;\">");

        // 显示分页的索引
        int index = pageIndex < 2 ? 1 : pageIndex - 1;

        //Top
        if (pageIndex > 1)
        { // 当前页大于1
            sb.AppendLine("<a style=\"\" href=\"javascript:SaveSchemes(1)\">&lt;&lt;</a>");
            sb.AppendLine("<a style=\"\" href=\"javascript:SaveSchemes(" + (pageIndex - 1).ToString() + ")\">上一页</a>");
        }
        else
        {
            sb.AppendLine("<span style=\"color:Gray\">&lt;&lt;</span>");
            sb.AppendLine("<span style=\"color:Gray\">上一页</span>");
        }

        for (int i = 0; i < 6; i++)
        {
            if (SumPageNumber < index)
            {
                break;
            }
            if (index < 1)
            {
                index = 1;
            }
            if (SumPageCount - index > 1 && i == 5)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\"><a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a> ..</span>");
            }
            else if (index > 1 && i == 0)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\">.. <a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a></span>");
            }
            else if (SumPageNumber == 1)
            {
                sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\">1</span>");
            }
            else
            {

                if (pageIndex == index)
                {
                    sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;color:Gray;\">" + index + "</span>");
                }
                else
                {
                    sb.AppendLine("<span class=\"currentPage\" style=\"margin-right:5px;\"><a href=\"javascript:Changepage(" + index + ")\" style=\"cursor:pointer;\">" + index + "</a></span>");
                }
            }

            index++;
        }

        // Down
        if (pageIndex == SumPageNumber)
        {
            sb.AppendLine("<span style=\"color:Gray\">下一页</span>");
            sb.AppendLine("<span style=\"color:Gray\">&gt;&gt;</span>");
        }
        else
        {
            sb.AppendLine("<a href=\"javascript:SaveSchemes(" + (pageIndex + 1).ToString() + ")\">下一页</a>");
            sb.AppendLine("<a href=\"javascript:SaveSchemes(" + SumPageNumber + ")\">&gt;&gt;</a>");
        }

        sb.AppendLine("</td></tr>");

        // 返回的Html代码
        returnHtml = sb.ToString();

        return returnHtml;
    }

    #region Tool
    private DataTable GetLotteryNumberAnalyse(DataTable dtScheme)
    {
        if (dtScheme == null)
        {
            return null;
        }
        try
        {
            dtScheme.Columns.Add("BetCount", Type.GetType("System.String"));
            dtScheme.Columns.Add("BetWay", Type.GetType("System.String"));
            dtScheme.Columns.Add("DateTime", Type.GetType("System.String"));
        }
        catch { }
        foreach (DataRow dr in dtScheme.Rows)
        {
            string LotteryNumber = "";

            int BetCount = 0;// 投注场次
            string Odds = "";// 投注赔率
            try
            {
                LotteryNumber = dr["LotteryNumber"].ToString();

                Odds = dr["Odds"].ToString();
            }
            catch
            {
                continue;

            }
            string LotteryNumberResult = "";    // 处理后的结果

            if (String.IsNullOrEmpty(LotteryNumber))
            {
                continue;
            }
            if (LotteryNumber.Split(';').Length != 3)
            {
                continue;
            }

            string PlayType, Content, Way = "";

            try
            {
                PlayType = LotteryNumber.Split(';')[0];
                Content = LotteryNumber.Split(';')[1];
                Way = LotteryNumber.Split(';')[2];
            }
            catch
            {
                continue;
            }

            // 解析Content 
            string[] MatchsAndResult = Content.Substring(1, Content.Length - 1).Split('|');
            // MatchID
            string MatchIDs = "";
            // 胜平负
            string result = "";
            // 周一001
            string MatchNumber = "";

            foreach (string val in MatchsAndResult)
            {
                MatchIDs += val.Split('(')[0] + ",";
                // 去掉尾部')'
                result += val.Split('(')[1].Substring(0, val.Split('(')[1].Length - 1) + ",";
            }

            if (MatchIDs.EndsWith(","))
            {
                MatchIDs = MatchIDs.Substring(0, MatchIDs.Length - 1);
            }
            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }
            if (result.EndsWith(")"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            if (string.IsNullOrEmpty(MatchIDs))
            {
                continue;
            }


            // 通过MatchID 查找内容

            DataTable dtMatch = new DAL.Tables.T_Match().Open("StopSellingTime , MatchNumber", "id in (" + MatchIDs + ")", "");
            if (dtMatch == null)
            {
                continue;
            }
            if (dtMatch.Rows.Count < 1)
            {
                continue;
            }

            foreach (DataRow mdr in dtMatch.Rows)
            {
                if (mdr["MatchNumber"] != null)
                {
                    MatchNumber += mdr["MatchNumber"].ToString() + ",";
                }
                dr["EndTime"] = mdr["StopSellingTime"].ToString();
            }

            if (MatchNumber.EndsWith(","))
            {
                MatchNumber = MatchNumber.Substring(0, MatchNumber.Length - 1);
            }

            // 得到投注场次
            BetCount = MatchIDs.Split(',').Length;

            // 拼接LotteryNumber            
            for (int x = 0; x < MatchIDs.Split(',').Length; x++)
            {
                string m = MatchNumber.Split(',')[x];
                string r = GetResult(result.Split(',')[x], PlayType);
                string o = Odds.Split('|')[x];

                LotteryNumberResult += (m + "[" + r + "]" + o + "  ");
            }

            Way = Way.Split(']')[0].Substring(1);
            Way = Way.Substring(0, Way.Length - 1);
            dr["BetWay"] = GetWay(Way);
            dr["LotteryNumber"] = LotteryNumberResult;
            dr["BetCount"] = BetCount + "";
            string day = Shove._Convert.StrToDateTime(dr["DateTimes"].ToString(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")).ToString("yyyy-MM-dd hh:mm:ss");

            dr["DateTime"] = day + "";
            dr["score"] = Convert.ToInt32(dr["score"]);
        }
        return dtScheme;
    }

    private DataTable GetLotteryNumberAnalyse_Savess(DataTable dtScheme)
    {
        if (dtScheme == null)
        {
            return null;
        }
        try
        {
            dtScheme.Columns.Add("IsOpen", Type.GetType("System.String"));
        }
        catch { }
        foreach (DataRow dr in dtScheme.Rows)
        {
            string LotteryNumber = "";

            int BetCount = 0;// 投注场次
            string Odds = "";// 投注赔率
            try
            {
                LotteryNumber = dr["LotteryNumber"].ToString();

                Odds = dr["Odds"].ToString();
            }
            catch
            {
                continue;

            }
            string LotteryNumberResult = "";    // 处理后的结果

            if (String.IsNullOrEmpty(LotteryNumber))
            {
                continue;
            }
            if (LotteryNumber.Split(';').Length != 3)
            {
                continue;
            }

            string PlayType, Content, Way = "";

            try
            {
                PlayType = LotteryNumber.Split(';')[0];
                Content = LotteryNumber.Split(';')[1];
                Way = LotteryNumber.Split(';')[2];
            }
            catch
            {
                continue;
            }

            // 解析Content 
            string[] MatchsAndResult = Content.Substring(1, Content.Length - 1).Split('|');
            // MatchID
            string MatchIDs = "";
            // 胜平负
            string result = "";
            // 周一001
            string MatchNumber = "";

            foreach (string val in MatchsAndResult)
            {
                MatchIDs += val.Split('(')[0] + ",";
                // 去掉尾部')'
                result += val.Split('(')[1].Substring(0, val.Split('(')[1].Length - 1) + ",";
            }

            if (MatchIDs.EndsWith(","))
            {
                MatchIDs = MatchIDs.Substring(0, MatchIDs.Length - 1);
            }
            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }
            if (result.EndsWith(")"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            if (string.IsNullOrEmpty(MatchIDs))
            {
                continue;
            }


            // 通过MatchID 查找内容

            DataTable dtMatch = new DAL.Tables.T_Match().Open("StopSellingTime , MatchNumber", "id in (" + MatchIDs + ")", "");
            if (dtMatch == null)
            {
                continue;
            }
            if (dtMatch.Rows.Count < 1)
            {
                continue;
            }

            foreach (DataRow mdr in dtMatch.Rows)
            {
                if (mdr["MatchNumber"] != null)
                {
                    MatchNumber += mdr["MatchNumber"].ToString() + ",";
                }   
                dr["EndTime"] = mdr["StopSellingTime"].ToString();
            }

            if (MatchNumber.EndsWith(","))
            {
                MatchNumber = MatchNumber.Substring(0, MatchNumber.Length - 1);
            }

            // 得到投注场次
            BetCount = MatchIDs.Split(',').Length;

            // 拼接LotteryNumber            
            for (int x = 0; x < MatchIDs.Split(',').Length; x++)
            {
                string m = MatchNumber.Split(',')[x];
                string r = GetResult(result.Split(',')[x], PlayType);
                string o = Odds.Split('|')[x];

                LotteryNumberResult += (m + "[" + r + "]" + o + "  ");
            }

            dr["LotteryNumber"] = LotteryNumberResult;

            string day = Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")).ToString("yyyy-MM-dd hh:mm:ss");

            string isOpen = dr["IsOpened"].ToString();
            if (isOpen.Equals("False"))
            {
                dr["IsOpen"] = "否";
            }
            else
            {
                dr["IsOpen"] = "是";
            }

            dr["DateTime"] = day + "";
        }
        return dtScheme;
    }

    private DataTable GetLotteryNumberAnalyse_Save(DataTable dtScheme)
    {
        if (dtScheme == null)
        {
            return null;
        }
        try
        {
            dtScheme.Columns.Add("BetCount", Type.GetType("System.String"));
            dtScheme.Columns.Add("BetWay", Type.GetType("System.String"));
        }
        catch { }
        foreach (DataRow dr in dtScheme.Rows)
        {
            string LotteryNumber = "";

            int BetCount = 0;// 投注场次
            string Odds = "";// 投注赔率
            try
            {
                LotteryNumber = dr["LotteryNumber"].ToString();
                Odds = dr["Odds"].ToString();
            }
            catch
            {
                continue;

            }
            string LotteryNumberResult = "";    // 处理后的结果

            if (String.IsNullOrEmpty(LotteryNumber))
            {
                continue;
            }
            if (LotteryNumber.Split(';').Length != 3)
            {
                continue;
            }

            string PlayType, Content, Way = "";

            try
            {
                PlayType = LotteryNumber.Split(';')[0];
                Content = LotteryNumber.Split(';')[1];
                Way = LotteryNumber.Split(';')[2];
            }
            catch
            {
                continue;
            }

            // 解析Content 
            string[] MatchsAndResult = Content.Substring(1, Content.Length - 1).Split('|');
            // MatchID
            string MatchIDs = "";
            // 胜平负
            string result = "";
            // 周一001
            string MatchNumber = "";

            foreach (string val in MatchsAndResult)
            {
                MatchIDs += val.Split('(')[0] + ",";
                // 去掉尾部')'
                result += val.Split('(')[1].Substring(0, val.Split('(')[1].Length - 1) + ",";
            }

            if (MatchIDs.EndsWith(","))
            {
                MatchIDs = MatchIDs.Substring(0, MatchIDs.Length - 1);
            }
            if (result.EndsWith(","))
            {
                result = result.Substring(0, result.Length - 1);
            }
            if (result.EndsWith(")"))
            {
                result = result.Substring(0, result.Length - 1);
            }

            if (string.IsNullOrEmpty(MatchIDs))
            {
                continue;
            }


            // 通过MatchID 查找内容

            DataTable dtMatch = new DAL.Tables.T_Match().Open("StopSellingTime , MatchNumber", "id in (" + MatchIDs + ")", "");
            if (dtMatch == null)
            {
                continue;
            }
            if (dtMatch.Rows.Count < 1)
            {
                continue;
            }

            foreach (DataRow mdr in dtMatch.Rows)
            {
                if (mdr["MatchNumber"] != null)
                {
                    MatchNumber += mdr["MatchNumber"].ToString() + ",";
                }
                dr["EndTime"] = mdr["StopSellingTime"].ToString();
            }

            if (MatchNumber.EndsWith(","))
            {
                MatchNumber = MatchNumber.Substring(0, MatchNumber.Length - 1);
            }

            // 得到投注场次
            BetCount = MatchIDs.Split(',').Length;

            // 拼接LotteryNumber            
            for (int x = 0; x < MatchIDs.Split(',').Length; x++)
            {
                string m = MatchNumber.Split(',')[x];
                string r = GetResult(result.Split(',')[x], PlayType);
                string o = Odds.Split('|')[x];

                LotteryNumberResult += (m + "[" + r + "]" + o + "  ");
            }

            Way = Way.Split(']')[0].Substring(1);
            Way = Way.Substring(0, Way.Length - 1);
            dr["BetWay"] = GetWay(Way);
            dr["LotteryNumber"] = LotteryNumberResult;
            dr["BetCount"] = BetCount + "";
            string day = Shove._Convert.StrToDateTime(dr["DateTime"].ToString(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")).ToString("yyyy-MM-dd hh:mm:ss");

            dr["DateTime"] = day + "";
            //dr["score"] = Convert.ToInt32(dr["score"]);
        }
        return dtScheme;
    }

    #endregion

    #region Lottery
    private string GetResult(string Num, string PlayTypeID)
    {
        switch (PlayTypeID)
        {
            case "7201":
                return Get7201(Num);
            case "7202":
                return Get7202(Num);
            case "7203":
                return Get7203(Num);
            case "7204":
                return Get7204(Num);
            default:
                return "";
        }
    }

    /// <summary>
    /// 胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7201(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜";
                break;
            case "2": res = "平";
                break;
            case "3": res = "负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 比分
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7202(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "1:0";
                break;
            case "2": res = "2:0";
                break;
            case "3": res = "2:1";
                break;
            case "4": res = "3:0";
                break;
            case "5": res = "3:1";
                break;
            case "6": res = "3:2";
                break;
            case "7": res = "4:0";
                break;
            case "8": res = "4:1";
                break;
            case "9": res = "4:2";
                break;
            case "10": res = "5:0";
                break;
            case "11": res = "5:1";
                break;
            case "12": res = "5:2";
                break;
            case "13": res = "胜其它";
                break;
            case "14": res = "0:0";
                break;
            case "15": res = "1:1";
                break;
            case "16": res = "2:2";
                break;
            case "17": res = "3:3";
                break;
            case "18": res = "平其它";
                break;
            case "19": res = "0:1";
                break;
            case "20": res = "0:2";
                break;
            case "21": res = "1:2";
                break;
            case "22": res = "0:3";
                break;
            case "23": res = "1:3";
                break;
            case "24": res = "2:3";
                break;
            case "25": res = "0:4";
                break;
            case "26": res = "1:4";
                break;
            case "27": res = "2:4";
                break;
            case "28": res = "0:5";
                break;
            case "29": res = "1:5";
                break;
            case "30": res = "2:5";
                break;
            case "31": res = "负其它";
                break;


            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 总进球
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7203(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "0";
                break;
            case "2": res = "1";
                break;
            case "3": res = "2";
                break;
            case "4": res = "3";
                break;
            case "5": res = "4";
                break;
            case "6": res = "5";
                break;
            case "7": res = "6";
                break;
            case "8": res = "7+";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    /// <summary>
    /// 半全场胜平负
    /// </summary>
    /// <param name="num"></param>
    /// <returns></returns>
    private string Get7204(string num)
    {
        string res = string.Empty;
        switch (num)
        {
            case "1": res = "胜胜";
                break;
            case "2": res = "胜平";
                break;
            case "3": res = "胜负";
                break;
            case "4": res = "平胜";
                break;
            case "5": res = "平平";
                break;
            case "6": res = "平负";
                break;
            case "7": res = "负胜";
                break;
            case "8": res = "负平";
                break;
            case "9": res = "负负";
                break;
            default:
                res = "";
                break;
        }

        return res;
    }

    public string GetWay(string way)
    {
        if (way == "AA")
        {
            return "2串1";
        }
        if (way == "AB")
        {
            return "3串1";
        }
        if (way == "AC")
        {
            return "4串1";
        }
        if (way == "AD")
        {
            return "5串1";
        }
        if (way == "AQ")
        {
            return "6串1";
        }
        if (way == "BA")
        {
            return "7串1";
        }
        if (way == "BG")
        {
            return "8串1";
        }
        return "暂无";
    }
    #endregion

    #endregion
}
