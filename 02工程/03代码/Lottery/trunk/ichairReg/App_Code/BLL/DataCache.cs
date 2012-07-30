using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Shove.Database;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
///Cache 的摘要说明
/// </summary>
public class DataCache
{
    public readonly static string IsusesInfo = "DataCache_IsusesInfo";
    public readonly static string WinInfo = "DataCache_WinInfo";
    public readonly static string ProfitInfo = "DataCache_ProfitInfo";
    public readonly static string WinNumber = "DataCache_WinNumber";
    public readonly static string LotteryNews = "DataCache_LotteryNews";
    public readonly static string AccountFreezeDetail = "DataCache_AccountFreezeDetail";
    public readonly static string AcountDistill = "DataCache_AcountDistill";
    public readonly static string WinNews = "DataCache_WinNews";
    public readonly static string ZCExpert = "DataCache_ZCExpert";

    #region 常量

    public static Dictionary<short, string> Banks = new Dictionary<short, string>();
    public static Dictionary<string, int> Citys = new Dictionary<string, int>();
    public static Dictionary<int, string> Lotteries = new Dictionary<int, string>();
    public static Dictionary<int, Dictionary<int, string>> PlayTypes = new Dictionary<int, Dictionary<int, string>>();
    public static Dictionary<int, int> LotteryEndAheadMinute = new Dictionary<int, int>();
    public static string[] SecurityQuestions = new string[8];
    public static Dictionary<int, string> LotterieBuyPage = new Dictionary<int, string>();

    #endregion

    static DataCache()
    {
        #region 银行

        DataTable dt = new DAL.Tables.T_Banks().Open("ID,Name", "", "");

        if (dt == null)
        {
            new Log("SNS").Write("PublicFunction 读取 T_Banks 出错");
        }

        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Banks[short.Parse(dr["ID"].ToString())] = dr["Name"].ToString();
            }
        }

        #endregion

        #region 城市

        dt = new DAL.Tables.T_Citys().Open("ID,Name", "", "");

        if (dt == null)
        {
            new Log("SNS").Write("PublicFunction 读取 T_Citys 出错");
        }

        if (dt != null)
        {
            foreach (DataRow dr in dt.Rows)
            {
                Citys[dr["Name"].ToString()] = int.Parse(dr["ID"].ToString());
            }
        }

        #endregion

        #region 安全问题

        SecurityQuestions[0] = "我婆婆或岳母的名字叫什么？";
        SecurityQuestions[1] = "我爸爸的出生地在哪里？";
        SecurityQuestions[2] = "我妈妈的出生地在哪里？";
        SecurityQuestions[3] = "我的小学校名是什么？";
        SecurityQuestions[4] = "我的中学校名是什么？";
        SecurityQuestions[5] = "我的一个好朋友的手机号码是什么？";
        SecurityQuestions[6] = "我的一个好朋友的爸爸名字是什么？";
        SecurityQuestions[7] = "自定义问题";

        #endregion

        #region 彩种玩法
        Lotteries[3] = "七星彩";
        Lotteries[5] = "双色球";
        Lotteries[6] = "福彩3D";
        Lotteries[28] = "重庆时时彩";
        Lotteries[29] = "时时乐";
        Lotteries[39] = "大乐透";
        Lotteries[58] = "东方6+1";
        Lotteries[59] = "15选5";
        Lotteries[13] = "七乐彩";
        Lotteries[9] = "22选5";
        Lotteries[14] = "29选7";
        Lotteries[63] = "排列3";
        Lotteries[64] = "排列5";
        Lotteries[19] = "篮彩";
        Lotteries[1] = "胜负彩";
        Lotteries[2] = "四场进球彩";
        Lotteries[15] = "六场半全场";
        Lotteries[61] = "江西时时彩";
        Lotteries[62] = "十一运夺金";
        Lotteries[65] = "31选7";
        Lotteries[41] = "浙江体彩6+1";
        Lotteries[70] = "11选5";   //江西11选5
        Lotteries[72] = "竞彩足彩";
        Lotteries[73] = "竞彩蓝彩";
        Lotteries[74] = "胜负彩";
        Lotteries[75] = "任九场";
        Lotteries[69] = "河南福彩22选5";
        Lotteries[68] = "快赢481";
        Lotteries[77] = "河南11选5";
        Lotteries[78] = "广东11选5";

        Dictionary<int, string> tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[301] = "单式";
        tempPlayTypes[302] = "复式";
        PlayTypes[3] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();
        tempPlayTypes[501] = "单式";
        tempPlayTypes[502] = "复式";
        tempPlayTypes[503] = "胆拖";

        PlayTypes[5] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[601] = "直选单式";
        tempPlayTypes[602] = "直选复式";
        tempPlayTypes[603] = "组选单式";
        tempPlayTypes[604] = "组6复式";
        tempPlayTypes[605] = "组3复式";
        tempPlayTypes[606] = "直选包点";
        tempPlayTypes[607] = "复选包点";

        PlayTypes[6] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[2801] = "单式";
        tempPlayTypes[2802] = "复式";
        tempPlayTypes[2803] = "组选";
        tempPlayTypes[2804] = "猜大小";
        tempPlayTypes[2805] = "五星通选单式";
        tempPlayTypes[2806] = "五星通选复式";
        tempPlayTypes[2807] = "二星组选单式";
        tempPlayTypes[2808] = "二星组选复式";
        tempPlayTypes[2809] = "二星组选分位";
        tempPlayTypes[2810] = "二星组选包点";
        tempPlayTypes[2811] = "二星组选包胆";
        tempPlayTypes[2812] = "三星包点";
        tempPlayTypes[2813] = "三星组3单式";
        tempPlayTypes[2814] = "三星组3复式";
        tempPlayTypes[2815] = "三星组6单式";
        tempPlayTypes[2816] = "三星组6复式";
        tempPlayTypes[2817] = "三星直选组合复式";
        tempPlayTypes[2818] = "三星组选包胆";
        tempPlayTypes[2819] = "三星组选包点";

        PlayTypes[61] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[2901] = "直选单式";
        tempPlayTypes[2902] = "直选复式";
        tempPlayTypes[2903] = "组选单式";
        tempPlayTypes[2904] = "组选6";
        tempPlayTypes[2905] = "组选3";
        tempPlayTypes[2906] = "直选";
        tempPlayTypes[2907] = "组选";
        tempPlayTypes[2908] = "前二单式";
        tempPlayTypes[2909] = "前二复式";
        tempPlayTypes[2910] = "后二单式";
        tempPlayTypes[2911] = "后二复式";
        tempPlayTypes[2912] = "前一单式";
        tempPlayTypes[2913] = "前一复式";
        tempPlayTypes[2914] = "后一单式";
        tempPlayTypes[2915] = "后一复式";

        PlayTypes[29] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[3901] = "单式";
        tempPlayTypes[3902] = "复式";
        tempPlayTypes[3903] = "追加单式";
        tempPlayTypes[3904] = "追加复式";
        tempPlayTypes[3905] = "12选2单式";
        tempPlayTypes[3906] = "12选2复式";

        PlayTypes[39] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[5801] = "单式";
        tempPlayTypes[5802] = "复式";

        PlayTypes[58] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[1301] = "单式";
        tempPlayTypes[1302] = "复式";

        PlayTypes[13] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[5901] = "单式";
        tempPlayTypes[5902] = "复式";

        PlayTypes[59] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[901] = "单式";
        tempPlayTypes[902] = "复式";

        PlayTypes[9] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[1401] = "单式";
        tempPlayTypes[1402] = "复式";

        PlayTypes[14] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6301] = "排列3直选单式";
        tempPlayTypes[6302] = "排列3直选复式";
        tempPlayTypes[6303] = "排列3组选单式";
        tempPlayTypes[6304] = "排列3组选6复式";
        tempPlayTypes[6305] = "排列3组选3复式";
        tempPlayTypes[6306] = "排列3直选和值";
        tempPlayTypes[6307] = "排列3组选和值";

        PlayTypes[63] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6401] = "排列5单式";
        tempPlayTypes[6402] = "排列5复式";

        PlayTypes[64] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[1901] = "单式";
        tempPlayTypes[1902] = "复式";

        PlayTypes[19] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[101] = "单式";
        tempPlayTypes[102] = "复式";
        tempPlayTypes[103] = "任九场单式";
        tempPlayTypes[104] = "任九场复式";

        PlayTypes[1] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[201] = "单式";
        tempPlayTypes[202] = "复式";

        PlayTypes[2] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[1501] = "单式";
        tempPlayTypes[1502] = "复式";

        PlayTypes[15] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6201] = "任选一";
        tempPlayTypes[6202] = "任选二";
        tempPlayTypes[6203] = "任选三";
        tempPlayTypes[6204] = "任选四";
        tempPlayTypes[6205] = "任选五";
        tempPlayTypes[6206] = "任选六";
        tempPlayTypes[6207] = "任选七";
        tempPlayTypes[6208] = "任选八";
        tempPlayTypes[6209] = "前二直选";
        tempPlayTypes[6210] = "前三直选";
        tempPlayTypes[6211] = "前二组选";
        tempPlayTypes[6212] = "前三组选";

        PlayTypes[62] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6501] = "单式";
        tempPlayTypes[3502] = "复式";

        PlayTypes[65] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6501] = "单式";
        tempPlayTypes[3502] = "复式";

        PlayTypes[65] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6101] = "单式";
        tempPlayTypes[6102] = "复式";
        tempPlayTypes[6103] = "组选";
        tempPlayTypes[6104] = "猜大小";
        tempPlayTypes[6105] = "五星通选单式";
        tempPlayTypes[6106] = "五星通选复式";
        tempPlayTypes[6107] = "二星组选单式";
        tempPlayTypes[6108] = "二星组选复式";
        tempPlayTypes[6109] = "二星组选分位";
        tempPlayTypes[6110] = "二星组选包点";
        tempPlayTypes[6111] = "二星组选包胆";
        tempPlayTypes[6112] = "二星直选包点";

        PlayTypes[61] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7001] = "任选一";
        tempPlayTypes[7002] = "任选二";
        tempPlayTypes[7003] = "任选三";
        tempPlayTypes[7004] = "任选四";
        tempPlayTypes[7005] = "任选五";
        tempPlayTypes[7006] = "任选六";
        tempPlayTypes[7007] = "任选七";
        tempPlayTypes[7008] = "任选八";
        tempPlayTypes[7009] = "前二直选";
        tempPlayTypes[7010] = "前三直选";
        tempPlayTypes[7011] = "前二组选";
        tempPlayTypes[7012] = "前三组选";

        PlayTypes[70] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[4101] = "单式";
        tempPlayTypes[4102] = "复式";

        PlayTypes[41] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7201] = "单式";
        tempPlayTypes[7202] = "复式";
        tempPlayTypes[7203] = "复式";
        tempPlayTypes[7204] = "复式";

        PlayTypes[72] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7301] = "单式";
        tempPlayTypes[7302] = "复式";
        tempPlayTypes[7303] = "复式";
        tempPlayTypes[7304] = "复式";

        PlayTypes[73] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7401] = "单式";
        tempPlayTypes[7402] = "复式";

        PlayTypes[74] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7501] = "单式";
        tempPlayTypes[7502] = "复式";

        PlayTypes[75] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6901] = "单式";
        tempPlayTypes[6902] = "复式";
        tempPlayTypes[6903] = "好运2";
        tempPlayTypes[6904] = "好运3";
        tempPlayTypes[6905] = "好运4";

        PlayTypes[69] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[6801] = "任选一";
        tempPlayTypes[6802] = "任选二";
        tempPlayTypes[6803] = "任选三";
        tempPlayTypes[6804] = "直选单式";
        tempPlayTypes[6805] = "直选复式";
        tempPlayTypes[6806] = "组选24单式";
        tempPlayTypes[6807] = "组选24复式";
        tempPlayTypes[6808] = "组选12单式";
        tempPlayTypes[6809] = "组选12复式";
        tempPlayTypes[6810] = "组选6单式";
        tempPlayTypes[6811] = "组选6复式";
        tempPlayTypes[6812] = "组选4单式";
        tempPlayTypes[6813] = "组选4复式";

        PlayTypes[68] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7701] = "任选一";
        tempPlayTypes[7702] = "任选二";
        tempPlayTypes[7703] = "任选三";
        tempPlayTypes[7704] = "任选四";
        tempPlayTypes[7705] = "任选五";
        tempPlayTypes[7706] = "任选六";
        tempPlayTypes[7707] = "任选七";
        tempPlayTypes[7708] = "任选八";
        tempPlayTypes[7709] = "前二直选";
        tempPlayTypes[7710] = "前三直选";
        tempPlayTypes[7711] = "前二组选";
        tempPlayTypes[7712] = "前三组选";

        PlayTypes[77] = tempPlayTypes;

        tempPlayTypes = new Dictionary<int, string>();

        tempPlayTypes[7801] = "任选一";
        tempPlayTypes[7802] = "任选二";
        tempPlayTypes[7803] = "任选三";
        tempPlayTypes[7804] = "任选四";
        tempPlayTypes[7805] = "任选五";
        tempPlayTypes[7806] = "任选六";
        tempPlayTypes[7807] = "任选七";
        tempPlayTypes[7808] = "任选八";
        tempPlayTypes[7809] = "前二直选";
        tempPlayTypes[7810] = "前三直选";
        tempPlayTypes[7811] = "前二组选";
        tempPlayTypes[7812] = "前三组选";

        PlayTypes[78] = tempPlayTypes;
        #endregion

        #region
        LotterieBuyPage[3] = "Buy_QXC.aspx";
        LotterieBuyPage[5] = "Buy_SSQ.aspx";
        LotterieBuyPage[6] = "Buy_3D.aspx";
        LotterieBuyPage[28] = "Buy_CQSSC.aspx";
        LotterieBuyPage[29] = "Buy_SSL.aspx";
        LotterieBuyPage[39] = "Buy_CJDLT.aspx";
        LotterieBuyPage[58] = "Buy_DF6J1.aspx";
        LotterieBuyPage[59] = "Buy_15X5.aspx";
        LotterieBuyPage[13] = "Buy_QLC.aspx";
        LotterieBuyPage[9] = "Buy_22X5.aspx";
        LotterieBuyPage[14] = "Buy_31X7.aspx";
        LotterieBuyPage[63] = "Buy_PL3.aspx";
        LotterieBuyPage[64] = "Buy_PL5.aspx";
        LotterieBuyPage[19] = "Buy.aspx";
        LotterieBuyPage[1] = "Buy.aspx";
        LotterieBuyPage[2] = "Buy_JQC.aspx";
        LotterieBuyPage[15] = "Buy_LCBQC.aspx";
        LotterieBuyPage[61] = "Buy_SSC.aspx";
        LotterieBuyPage[62] = "Buy_SYYDJ.aspx";
        LotterieBuyPage[65] = "Buy_31X7.aspx";
        LotterieBuyPage[41] = "Buy_ZJTC6J1.aspx";
        LotterieBuyPage[70] = "Buy_JX11X5.aspx";   //江西11选5
        LotterieBuyPage[72] = "JCZC/Buy_SPF.aspx";
        LotterieBuyPage[73] = "JCLC/Buy_SF.aspx";
        LotterieBuyPage[74] = "Buy_SFC.aspx";
        LotterieBuyPage[75] = "Buy_SFC_9_D.aspx";
        LotterieBuyPage[69] = "Buy_HN22X5.aspx";
        LotterieBuyPage[68] = "Buy_KY481.aspx";
        LotterieBuyPage[77] = "Buy_HN11X5.aspx";
        LotterieBuyPage[78] = "Buy_GD11X5.aspx";
        #endregion

        #region 提前截止时间

        string strLottery = "";

        foreach (KeyValuePair<int, string> kvp in Lotteries)
        {
            strLottery += kvp.Key + ",";
        }

        strLottery = strLottery.Remove(strLottery.Length - 1, 1);

        DataTable dtPlayTypes = new DAL.Tables.T_PlayTypes().Open("LotteryID,SystemEndAheadMinute", "LotteryID in (" + strLottery + ")", "");

        foreach (DataRow dr in dtPlayTypes.Rows)
        {
            LotteryEndAheadMinute[int.Parse(dr["LotteryID"].ToString())] = int.Parse(dr["SystemEndAheadMinute"].ToString());
        }

        #endregion
    }

    /// <summary>
    /// 期信息
    /// </summary>
    /// <returns></returns>
    public static DataTable GetIsusesInfo(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(IsusesInfo + LotteryID.ToString());
        
        if (dt == null || dt.Rows.Count == 0)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT * FROM (SELECT TOP 1 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryID=" + LotteryID + " AND   ISNULL(WinLotteryNumber, '')<>'' ORDER BY EndTime DESC)a")
                .Append(" UNION ")
                .Append("SELECT * FROM (SELECT TOP 1 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryID=" + LotteryID + " AND GETDATE()>EndTime  ORDER BY EndTime DESC)a")
                .Append(" UNION ")
                .Append("SELECT TOP 1 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryID=" + LotteryID + " and IsOpened = 0 AND GETDATE() BETWEEN StartTime AND EndTime")
                .Append(" UNION ")
                .Append("SELECT * FROM (SELECT TOP 49 ID,Name,WinLotteryNumber,StartTime,EndTime,IsOpened FROM dbo.T_Isuses WHERE LotteryID=" + LotteryID + " AND getdate()<StartTime ORDER BY StartTime)a");

            dt = MSSQL.Select(sql.ToString(), new MSSQL.Parameter[0]);

            if (dt != null)
            {
                Shove._Web.Cache.SetCache(IsusesInfo + LotteryID.ToString(), dt, (LotteryID == 62 || LotteryID == 29 || LotteryID == 70 || LotteryID == 77) ? 60 : 600);
            }
        }

        return dt;
    }

    /// <summary>
    /// 中奖排行(按彩种区分)
    /// </summary>
    /// <returns></returns>
    public static DataTable GetWinInfo(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(WinInfo + LotteryID.ToString());

        StringBuilder sb = new StringBuilder();

        if (dt == null || dt.Rows.Count == 0)
        {
            sb.Append("select b.InitiateUserID, c.Name as InitiateName, Win,LotteryID,NickName from ")
                .Append("( select top 9  InitiateUserID, SUM(WinMoney) as Win,LotteryID from  ")
                .Append("( select InitiateUserID, WinMoney,LotteryID from T_Schemes a WITH (nolock)  ");

            sb.Append("inner join T_PlayTypes b WITH (nolock) on a.PlayTypeID = b.ID ")
                .Append("where WinMoney > 0 and LotteryID = @LotteryID)d group by InitiateUserID,LotteryID order by Win desc )as b  ")
                .Append("inner join T_Users c WITH (nolock) on b.InitiateUserID = c.ID order by Win desc");
             
            dt = Shove.Database.MSSQL.Select(sb.ToString(), new MSSQL.Parameter("LotteryID", SqlDbType.Int, 0, ParameterDirection.Input, LotteryID));

            if (dt != null && dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(WinInfo + LotteryID.ToString(), dt, (LotteryID == 62 || LotteryID == 29 || LotteryID == 61) ? 300 : 3600);
            }
        }

        return dt;
    }

    /// <summary>
    /// 每彩种的最近100期开奖号码
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    public static DataTable GetWinNumber(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(WinNumber + LotteryID.ToString());

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Tables.T_Isuses().Open("top 100 Name, WinLotteryNumber, EndTime", "LotteryID=" + LotteryID + " and IsOpened = 1 and IsNull(WinLotteryNumber,'')<>''", "EndTime Desc");

            if (dt != null)
            {
                dt.Columns.Add("ID", typeof(int));

                int i = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["ID"] = i;

                    i++;

                    dr["WinLotteryNumber"] = FormatWinNumber(dr["WinLotteryNumber"].ToString(), LotteryID);
                }

                Shove._Web.Cache.SetCache(WinNumber + LotteryID.ToString(), dt, (LotteryID == 62 || LotteryID == 29) ? 120 : 3600);
            }
        }

        return dt;
    }

    private static string FormatWinNumber(string WinNumber, int LotteryID)
    {
        String rightNumber = "";
        if (LotteryID == 6 || LotteryID == 63 || LotteryID == 64 || LotteryID == 3 || LotteryID == 29)
        {
            for (int j = 0; j < WinNumber.Length; j++)
            {
                rightNumber += "<span style='padding-left:5px'>" + WinNumber.Substring(j, 1) + "</span>";
            }
        }
        else if (LotteryID == 5 || LotteryID == 13)
        {
            string winNumbers = WinNumber.Replace(" + ", " ");
            string[] numbers = winNumbers.Split(' ');
            rightNumber = "";
            for (int i = 0; i < numbers.Length; i++)
            {
                if (i < numbers.Length - 1)
                {
                    rightNumber += numbers[i] + " ";
                }
                else
                {
                    rightNumber += "<span class=\"blue12\">" + numbers[i] + "</span>";
                }
            }
        }
        else if (LotteryID == 58)
        {
            string winNumbers = WinNumber.Replace("+", " ");
            for (int j = 0; j < winNumbers.Length - 1; j++)
            {
                rightNumber += "<span style='padding-left:5px'>" + winNumbers.Substring(j, 1) + "</span>";
            }
            rightNumber += "<span class=\"blue12\">" + winNumbers.Substring(winNumbers.Length - 1, 1) + "</span>";
        }
        else if (LotteryID == 39)
        {
            string winNumbers = WinNumber.Replace("+", "");
            string[] numbers = winNumbers.Split(' ');
            rightNumber = "";
            for (int i = 0; i < numbers.Length; i++)
            {
                if (i < numbers.Length - 2)
                {
                    rightNumber += numbers[i] + " ";
                }
                else
                {
                    rightNumber += "<span style='padding-left:5px' class=\"blue12\">" + numbers[i] + "</span>";
                }
            }
        }
        else if (LotteryID == 65)
        {
            rightNumber = WinNumber.Replace("+", "");
            rightNumber = rightNumber.Substring(0, rightNumber.Length - 2) + "<span class=\"blue12\">" + rightNumber.Substring(rightNumber.Length - 2, 2) + "</span>";
        }
        else
        {
            rightNumber = WinNumber;
        }

        return rightNumber;
    }

    /// <summary>
    /// 高频新闻(数据变化时，已清掉缓存)
    /// </summary>
    /// <param name="LotteryID"></param>
    /// <returns></returns>
    public static string GetLotteryNews(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(LotteryNews + LotteryID.ToString());

        StringBuilder sb = new StringBuilder();

        if (dt == null || dt.Rows.Count == 0)
        {
            string NewsType = "";

            if (LotteryID == 28)
            {
                NewsType = "重庆时时彩资讯";
            }

            if (LotteryID == 29)
            {
                NewsType = "时时乐资讯";
            }

            if (LotteryID == 62)
            {
                NewsType = "十一运夺金资讯";
            }
            if (LotteryID == 5)
            {
                NewsType = "双色球资讯";
            }
            if (LotteryID == 6)
            {
                NewsType = "3D资讯";
            }
            if (LotteryID == 39)
            {
                NewsType = "超级大乐透资讯";
            }
            if (LotteryID == 61)
            {
                NewsType = "时时彩咨询";
            }
            if (LotteryID == 63 || LotteryID == 64)
            {
                NewsType = "排列3/5资讯";
            }
            if (LotteryID == 1 || LotteryID == 15 || LotteryID == 2 || LotteryID == 74 || LotteryID == 75)
            {
                NewsType = "足彩资讯";
            }
            if (LotteryID == 70)
            {
                NewsType = "11选5资讯";
            }
            if (LotteryID == 77)
            {
                NewsType = "河南11选5资讯";
            }
            if (LotteryID == 68)
            {
                NewsType = "快赢481资讯";
            }
            if (LotteryID == 41)
            {
                NewsType = "浙江体彩6+1资讯";
            }

            dt = new DAL.Views.V_News().Open("top 8 ID, Title,Content", "isShow = 1  and [TypeName] = '" + NewsType + "'", "isCommend,DateTime desc");

            if (dt != null && dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(LotteryNews + LotteryID.ToString(), dt, 120);
            }
        }


        string Title = "";
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["Title"].ToString().IndexOf("</font>") > -1 && dr["Title"].ToString().IndexOf("</font>") > -1)
            {
                Title = dr["Title"].ToString();
                if (Title.IndexOf("class=red12") > 0)
                {
                    Title = (Title.Substring(Title.IndexOf(">") + 1, Title.IndexOf("</font>") - Title.IndexOf(">") - 1));
                    sb.Append("<tr>")
                    .Append("<td width=\"5%\" height=\"24\" align=\"center\" class=\"hui14\">")
                    .Append("&#9642;&nbsp;")
                    .Append("</td>")
                    .Append("<td width=\"95%\" height=\"22\" class=\"blue12\">")
                    .Append("<a href='");

                    Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(dr["Content"].ToString());

                    if (m.Success)
                    {
                        sb.Append(dr["Content"].ToString());
                    }
                    else
                    {
                        string URL = Shove._Web.Utility.GetUrl();

                        sb.Append(URL + "/Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                    }

                    sb.Append("' target='_blank'>")
                    .Append("<font class = 'red12'>")
                    .Append(Shove._String.Cut(Title, 15))
                    .Append("</font>")
                    .Append("</a>")
                    .Append("</td>")
                    .Append("</tr>");
                }
                else
                {
                    Title = (Title.Substring(Title.IndexOf(">") + 1, Title.IndexOf("</font>") - Title.IndexOf(">") - 1));
                    sb.Append("<tr>")
                    .Append("<td width=\"5%\" height=\"24\" align=\"center\" class=\"hui14\">")
                    .Append("&#9642;&nbsp;")
                    .Append("</td>")
                    .Append("<td width=\"95%\" height=\"22\" class=\"blue12\">")
                    .Append("<a href='");

                    Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    Match m = regex.Match(dr["Content"].ToString());

                    if (m.Success)
                    {
                        sb.Append(dr["Content"].ToString());
                    }
                    else
                    {
                        string URL = Shove._Web.Utility.GetUrl();

                        sb.Append(URL + "/Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                    }

                    sb.Append("' target='_blank'>")
                    .Append("<font class = 'black12'>")
                    .Append(Shove._String.Cut(Title, 17))
                    .Append("</font>")
                    .Append("</a>")
                    .Append("</td>")
                    .Append("</tr>");
                }

                //Append(Shove._String.Cut(dr["Title"].ToString(), 17))
            }
            else
            {
                sb.Append("<tr>")
                .Append("<td width=\"5%\" height=\"24\" align=\"center\" class=\"hui14\">")
                .Append("&#9642;&nbsp;")
                .Append("</td>")
                .Append("<td width=\"95%\" height=\"22\" class=\"blue12\">")
                .Append("<a href='");

                Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(dr["Content"].ToString());

                if (m.Success)
                {
                    sb.Append(dr["Content"].ToString());
                }
                else
                {
                    string URL = Shove._Web.Utility.GetUrl();

                    sb.Append(URL + "/Home/Web/ShowNews.aspx?ID=" + dr["ID"].ToString());
                }

                sb.Append("' target='_blank'>")
                .Append(Shove._String.Cut(dr["Title"].ToString(), 15))
                .Append("</a>")
                .Append("</td>")
                .Append("</tr>");
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 中奖喜报
    /// </summary>
    /// <returns></returns>
    public static DataTable GetWinNews()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(WinNews);

        if (dt == null)
        {
            dt = new DAL.Views.V_Schemes().Open("top 9 ID,InitiateName,NickName,WinMoney,WinDescription,LotteryName,PlayTypeName,IsuseName", "Buyed=1 and QuashStatus=0 and WinMoney>0 and LotteryID in(5,6,29,39)", "ID desc");

            if (dt != null && dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache(WinNews, dt, 600);
            }
        }

        return dt;
    }

    public static void ClearCache(string CaheKey)
    {
        Shove._Web.Cache.ClearCache(CaheKey);
    }

    public static DataTable GetZCExpertList(int LotteryID)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(ZCExpert + LotteryID.ToString());

        if (dt == null || dt.Rows.Count == 0)
        {
            dt = new DAL.Views.V_Experts().Open("UserName,UserID,LotteryName", "[ON]=1 and [isCommend]=1 and LotteryID=" + LotteryID, "");

            if (dt != null)
            {
                dt.Columns.Add("ID", typeof(int));

                int i = 1;

                foreach (DataRow dr in dt.Rows)
                {
                    dr["ID"] = i;

                    i++;
                }

                Shove._Web.Cache.SetCache(ZCExpert + LotteryID.ToString(), dt, 60 * 3);
            }
        }

        return dt;
    }
}
