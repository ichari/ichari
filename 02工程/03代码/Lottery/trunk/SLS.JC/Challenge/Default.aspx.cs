using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

public partial class Challenge_Default : RoomPageBase
{
    protected bool state_s = false;
    protected bool state = false;
    protected int indexs = 0;

    protected string NewsHTML = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        ((HtmlInputHidden)WebHead1.FindControl("currentMenu")).Value = "mBattle";
        if (!this.IsPostBack)
        {
            BindPassRate();
            // 绑定新闻
            BindNews();
            // 绑定排行(10天)
            BindRankingScoreToDay();
            // 绑定月 排行
            BindRankingScoreMonth();
            // 绑定总 排行
            BindRankingScoreToMain();
            // 绑定  红人
            BindBetHot();
            // 绑定 热门投注
            BindHotBet();
            // 绑定 最新方案
            BindUserNewBetContent();
            // 绑定 用户信息(已登录)
            BindUserDetails();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    #region Bind
    /// <summary>
    /// 绑定比赛
    /// </summary>
    private void BindPassRate()
    {
        //赛事缓存30分钟
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_PassRate");

        if (dt == null)
        {
          
            dt = Shove.Database.MSSQL.Select("SELECT [MatchID],[MatchNumber],[GameColor],[Game],[MatchDate],[MainTeam],[MainLoseball],[GuestTeam],[EuropeSSP],[EuropePSP],[EuropeFSP],[Win],[Lose],[Flat] FROM [T_PassRate]  where IsHhad = 1 and StopSellTime > GETDATE() and [Day] = CONVERT(VARCHAR(24),GETDATE(),112) order by [Day], MatchNumber");
            
            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(128)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_PassRate", dt, 600);
            }
        }

        gPassRate.DataSource = dt;
        gPassRate.DataBind();
    }
    /// <summary>
    /// 绑定新闻
    /// </summary>
    private void BindNews()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_News");

        if (dt == null)
        {
            dt = new DAL.Tables.T_News().Open("top 5 [ID],[Title],[Content]", "TypeID = 9", "DateTime desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(129)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_News", dt, 1200);
            }
        }
        if (dt == null)
        {
            return;
        }
        if (dt.Rows.Count < 1)
        {
            return;
        }

        Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<li>");
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
            }
            else
            {
                sb.Append("<a href=\"../Home/Web/ShowNews.aspx?Id=" + dr["ID"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
            }
            sb.AppendLine("</li>");
        }

        NewsHTML = sb.ToString();
    }
    /// <summary>
    /// 绑定积分(天)
    /// </summary>
    private void BindRankingScoreToDay()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToDay");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score from T_ChallengeScheme as s,T_Users as u where DateTime > dateadd(DD,-10,getdate()) and u.ID = s.InitiateUserId  and s.WinMoney > 0 group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToDay", dt, 600);
            }
        }
        gSchemesToDay.DataSource = dt;
        gSchemesToDay.DataBind();
    }
    /// <summary>
    /// 绑定积分(月)
    /// </summary>
    private void BindRankingScoreMonth()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToMonth");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score,Count(s.ID) as Counts from T_ChallengeScheme as s,T_Users as u where u.ID = s.InitiateUserId and DateTime > dateadd(MM,-1,getdate()) and s.WinMoney > 0  group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(131)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToMonth", dt, 600);
            }
        }
        gSchemesToMonth.DataSource = dt;
        gSchemesToMonth.DataBind();
    }
    /// <summary>
    /// 绑定积分(总)
    /// </summary>
    private void BindRankingScoreToMain()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToMain");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score from T_ChallengeScheme as s,T_Users as u where u.ID = s.InitiateUserId and s.WinMoney > 0 group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(132)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToMain", dt, 600);
            }

        }
        gSchemesToMain.DataSource = dt;
        gSchemesToMain.DataBind();
    }
    /// <summary>
    /// 绑定擂台红人
    /// </summary>
    private void BindBetHot()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_BetHot");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 4 c.UserId,u.Name,c.BetCount,c.WinCount,c.TotalWinMoney from T_ChallengeBetRed as c ,T_Users as u where c.UserId = u.ID order by BetCount desc ,c.WinCount desc , c.TotalWinMoney desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(133)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_BetHot", dt, 600);
            }
        }

        gBetHot.DataSource = dt;
        gBetHot.DataBind();
    }
    /// <summary>
    /// 绑定热门投注
    /// </summary>
    private void BindHotBet()
    {

        DataTable dt = Shove.Database.MSSQL.Select("select top 5 h.WinningCount,h.DrawCount,h.LostCount,m.MatchNumber,m.Game,m.MainTeam,m.GuestTeam,m.MainLoseBall,(h.WinningCount+h.DrawCount+h.LostCount) as CountSum "
                + "from T_ChallengeHotBet h "
                + "inner join T_PassRate m on h.MatchID = m.MatchID where m.MatchDate > GETDATE()");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(134)", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        gHotBet.DataSource = GetHotBet(dt);
        gHotBet.DataBind();
    }
    /// <summary>
    /// 绑定 最新方案
    /// </summary>
    private void BindUserNewBetContent()
    {

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_BindUserNewBetContent");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 15 s.ID,row_number() over (order by ([DateTime]) desc) as Ranking ,u.Name ,s.LotteryNumber,s.Odds from T_ChallengeScheme as s , T_Users as u where u.ID = s.InitiateUserId order by [DateTime] desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(135)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_BindUserNewBetContent", dt, 300);
            }
        }
        DataTable newDt = dt;
        gNewBetContent.DataSource = GetUserNewBetContent(newDt);
        gNewBetContent.DataBind();
    }
    /// <summary>
    /// 绑定用户
    /// </summary>
    private void BindUserDetails()
    {
        if (_User == null)
        {
            pUserDetails.Visible = false;
            return;
        }

        if (_User.ID < 1)
        {
            return;
        }

        string sql = "select c.UserId,u.Name,c.BetCount,c.WinCount,c.Score,c.TotalWinMoney from T_ChallengeBetRed as c ,T_Users as u where c.UserId = u.ID and c.UserId = " + _User.ID;
        DataTable dtUserDetails = Shove.Database.MSSQL.Select(sql);

        if (dtUserDetails == null)
        {
            return;
        }

        if (dtUserDetails.Rows.Count < 1)
        {
            return;
        }
        // 计算积分
        foreach (DataRow dr in dtUserDetails.Rows)
        {
            dr["Score"] = ((int)Shove._Convert.StrToDouble(dr["Score"].ToString(), 0)).ToString();
        }
        gUserDetails.DataSource = dtUserDetails;
        gUserDetails.DataBind();
        pUserDetails.Visible = true;
    }

    #endregion

    #region Tool
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="ArrayMatchNumber"></param>
    /// <param name="ArrayGame"></param>
    /// <param name="ArrayMainTeam"></param>
    /// <param name="ArrayMainLoseBall"></param>
    /// <param name="ArrayGuestTeam"></param>
    /// <param name="ArrayResult"></param>
    /// <param name="ArrayScale"></param>
    private void Sort(string[] ArrayMatchNumber, string[] ArrayGame, string[] ArrayMainTeam, string[] ArrayMainLoseBall, string[] ArrayGuestTeam, string[] ArrayResult, int[] ArrayScale)
    {
        for (int i = 0; i < ArrayScale.Length; i++)
        {
            for (int j = i; j < ArrayScale.Length; j++)
            {
                if (ArrayScale[i] < ArrayScale[j])
                {
                    // 周二001
                    string MatchNumberTemp = ArrayMatchNumber[i];
                    ArrayMatchNumber[i] = ArrayMatchNumber[j];
                    ArrayMatchNumber[j] = MatchNumberTemp;
                    // 赛事
                    string GameTemp = ArrayGame[i];
                    ArrayGame[i] = ArrayGame[j];
                    ArrayGame[j] = GameTemp;
                    // 主队
                    string MainTeamTemp = ArrayMainTeam[i];
                    ArrayMainTeam[i] = ArrayMainTeam[j];
                    ArrayMainTeam[j] = MainTeamTemp;
                    // 让球
                    string MainLoseBallTemp = ArrayMainLoseBall[i];
                    ArrayMainLoseBall[i] = ArrayMainLoseBall[j];
                    ArrayMainLoseBall[j] = MainLoseBallTemp;
                    // 客队
                    string GuestTeamTemp = ArrayGuestTeam[i];
                    ArrayGuestTeam[i] = ArrayGuestTeam[j];
                    ArrayGuestTeam[j] = GuestTeamTemp;
                    // 结果
                    string resultTemp = ArrayResult[i];
                    ArrayResult[i] = ArrayResult[j];
                    ArrayResult[j] = resultTemp;
                    // 比例
                    int scaleTemp = ArrayScale[i];
                    ArrayScale[i] = ArrayScale[j];
                    ArrayScale[j] = scaleTemp;

                }
            }
        }
    }
    /// <summary>
    /// 计算热门投注
    /// </summary>
    private DataTable GetHotBet(DataTable dtHotBet)
    {
        //int MatchID; //  赛事ID
        double WinCount, DrawCount, LostCount, Sum; // 赢的数量、平的数量、负的数量、总数
        double dWin, dDrwa, dLost, dMax; // 赢的比例、平的比例、负的比例、最大的比例
        string result;  // 结果 "胜|平|负"
        string MatchNumber, Game, MainTeam, GuestTeam, MainLoseBall;

        // 存放比例
        int[] ArrayScale = new int[dtHotBet.Rows.Count];
        // 存放胜平负
        string[] ArrayResult = new string[dtHotBet.Rows.Count];
        // 存放周二001...
        string[] ArrayMatchNumber = new string[dtHotBet.Rows.Count];
        // 存放赛事
        string[] ArrayGame = new string[dtHotBet.Rows.Count];
        // 存放主队
        string[] ArrayMainTeam = new string[dtHotBet.Rows.Count];
        // 存放客队
        string[] ArrayGuestTeam = new string[dtHotBet.Rows.Count];
        // 存放让球
        string[] ArrayMainLoseBall = new string[dtHotBet.Rows.Count];

        // 索引
        int index = 0;
        foreach (DataRow rows in dtHotBet.Rows)
        {
            // 胜
            WinCount = Shove._Convert.StrToDouble(rows["WinningCount"].ToString().Trim(), 0.0);
            // 平
            DrawCount = Shove._Convert.StrToDouble(rows["DrawCount"].ToString().Trim(), 0.0);
            // 负
            LostCount = Shove._Convert.StrToDouble(rows["LostCount"].ToString().Trim(), 0.0);
            // 周二001
            MatchNumber = rows["MatchNumber"].ToString().Trim();
            // 赛事
            Game = rows["Game"].ToString().Trim();
            // 主队
            MainTeam = rows["MainTeam"].ToString().Trim();
            // 客队
            GuestTeam = rows["GuestTeam"].ToString().Trim();
            // 让球
            MainLoseBall = rows["MainLoseBall"].ToString().Trim();

            // 总
            Sum = Shove._Convert.StrToDouble(rows["CountSum"].ToString().Trim(), 0.0);

            // 求出胜平负的各自比例     (如果小于1 就为1000最大的[没人投注的])
            dWin = WinCount < 1 ? -1 : (WinCount / Sum) * 100;
            dDrwa = DrawCount < 1 ? -1 : (DrawCount / Sum) * 100;
            dLost = LostCount < 1 ? -1 : (LostCount / Sum) * 100;

            // 求出最小的
            dMax = Math.Max(dWin, Math.Max(dDrwa, dLost));
            result = dMax == dWin ? "胜" : dMax == dDrwa ? "平" : "负";

            // ArrayMatchID[index] = MatchID;
            ArrayResult[index] = result;                    // 存放胜平负
            ArrayScale[index] = Convert.ToInt32(dMax);      // 存放比例
            ArrayMatchNumber[index] = MatchNumber;          // 周二001
            ArrayGame[index] = Game;                        // 赛事
            ArrayMainTeam[index] = MainTeam;                // 主队
            ArrayGuestTeam[index] = GuestTeam;              // 客队
            ArrayMainLoseBall[index] = MainLoseBall;        // 让球
            index++;
        }

        // 进行排序操作
        Sort(ArrayMatchNumber, ArrayGame, ArrayMainTeam, ArrayMainLoseBall, ArrayGuestTeam, ArrayResult, ArrayScale);

        DataTable dt = new DataTable();
        dt.Columns.Add("MatchNumber");
        dt.Columns.Add("Game");
        dt.Columns.Add("MainTeam");
        dt.Columns.Add("MainLoseBall");
        dt.Columns.Add("GuestTeam");
        dt.Columns.Add("Result");
        dt.Columns.Add("Scale");

        int maxIndex = ArrayMatchNumber.Length < 5 ? ArrayMatchNumber.Length : 5;


        for (int j = 0; j < maxIndex; j++)
        {
            DataRow dr = dt.NewRow();
            object[] objs = { ArrayMatchNumber[j], ArrayGame[j], ArrayMainTeam[j], ArrayMainLoseBall[j], ArrayGuestTeam[j], ArrayResult[j], ArrayScale[j] };
            dr.ItemArray = objs;
            dt.Rows.Add(dr);
        }
        return dt;
    }

    private DataTable GetUserNewBetContent(DataTable dtNewUserMessage)
    {
        try
        {
            dtNewUserMessage.Columns.Add("BetWay", Type.GetType("System.String"));
            dtNewUserMessage.Columns.Add("BetCount", Type.GetType("System.String"));
        }
        catch { }

        foreach (DataRow dr in dtNewUserMessage.Rows)
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

            DataTable dtMatch = new DAL.Tables.T_Match().Open("MatchNumber", "id in (" + MatchIDs + ")", "");
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

        }
        return dtNewUserMessage;
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

    #endregion
}
