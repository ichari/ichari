<%@ WebHandler Language="C#" Class="Buy_Challenge_Handler" %>

using System;
using System.Web;
using System.Text;
using Microsoft.JScript;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.Data;

public class Buy_Challenge_Handler : IHttpHandler, IReadOnlySessionState
{
    private Users _User;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        Buy(context);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 购买彩票
    /// </summary>
    private void Buy(HttpContext context)
    {

        _User = Users.GetCurrentUser(1);      
       
        
        if (_User == null)
        {
            context.Response.Write("[竞彩擂台]您的登录信息丢失，请重新登录。");
            context.Response.End();
        }

        // 效验用户是否效验手机
        if (!_User.isMobileValided)
        { // 手机未效验
            context.Response.Write("[竞彩擂台]对不起，只有绑定手机才可以进行投注");
            context.Response.End();
        }

        // 类别  1 = 投注  , 2 = 保存
        string subType = Shove._Web.Utility.GetRequest(context, "type");

        if (subType == "1")
        {
            if (!checkeBuyDateTime())
            {   // 今天已购买
                context.Response.Write("[竞彩擂台]对不起，每天仅能提交1个竞猜方案");
                context.Response.End();
            }
        }
        else
        {
            if (!checkeMaxSave())
            { // 保存数量限制了
                context.Response.Write("[竞彩擂台]对不起，每天仅能提交1个竞猜方案");
                context.Response.End();
            }
        }

        #region 获取表单数据

        string lotId = Shove._Web.Utility.GetRequest(context, "lotid");                         //彩票编号
        string playid = Shove._Web.Utility.GetRequest(context, "playid");                     //玩法
        string ggtypeid = Shove._Web.Utility.GetRequest(context, "ggtypeid");                   //过关方式（M串N）

        string codes = Shove._Web.Utility.GetRequest(context, "codes");                         //投注内容
        string zhushu = Shove._Web.Utility.GetRequest(context, "zhushu");                       //基本注数
        string multiple = Shove._Web.Utility.GetRequest(context, "beishu");                   //投注倍数

        string Odds = Shove._Web.Utility.GetRequest(context, "odds"); ;                    // 投注赔率
        
        #endregion


        #region 数据验证区

        //************局部变量

        //彩种信息
        int LotteryID = 0;              //彩种
        int PlayTypeID = 0;             //玩法
        double Price = 2.00;                  //彩票单价

        //方案信息
        int BaseCount = 1;              //投注基本注数
        int Multiple = 0;               //投注倍数，       
        int SumNum = 0;                 //总的注数（基本注数*倍数）
        double SumMoney = 2;            //方案总金额

        //验证数据格式是否正确
        try
        {
            LotteryID = int.Parse(lotId);
            PlayTypeID = int.Parse(playid);

            Multiple = int.Parse(multiple);
            SumNum = BaseCount;
        }
        catch
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]输入有错误，请仔细检查。");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案有错误，请仔细检查。");
            }
            context.Response.End();
        }


        //验证彩种和玩法
        if (LotteryID != 72 || (PlayTypeID != 7201 && PlayTypeID != 7202 && PlayTypeID != 7203 && PlayTypeID != 7204))
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]输入有错误，请仔细检查。");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案有误，请仔细检查。");
            }
            context.Response.End();
        }

        string BuyNumber = codes.Trim().Split(';')[1].ToString();

        string FireTeam = BuyNumber.Substring(1, BuyNumber.IndexOf('(') - 1).Trim();

        // 读取 预计截止销售时间
        System.Data.DataTable dt = Shove.Database.MSSQL.Select("SELECT StopSellTime from T_PassRate where MatchID=" + FireTeam);

        if ((dt == null) || (dt.Rows.Count == 0))
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]数据读取错误,请稍后再访问!");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案读取错误,请稍后再访问!");
            }
            context.Response.End();
        }

        if (Shove._Convert.StrToDateTime(dt.Rows[0]["StopSellTime"].ToString(), DateTime.Now.AddDays(-1).ToString()).CompareTo(DateTime.Now) < 0)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]您所选择的场次里面包含了已截止场次, 请重新选择!");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案错误, 请重新选择!");
            }
            context.Response.End();
        }

        if (codes.Trim().Split(';').Length == 4)
        {
            try
            {
                FireTeam = BuyNumber.Split('[')[2].Substring(0, BuyNumber.Split('[')[2].IndexOf('(')).Trim();
            }
            catch
            { }

            if (FireTeam == "")
            {
                if (subType == "1")
                {
                    context.Response.Write("[竞彩擂台]您所选择的号码出现异常，请重新选择投注!");
                }
                else
                {
                    context.Response.Write("[竞彩擂台]您所保存的号码出现异常，请重新选择投注!");
                }
                context.Response.End();
            }

            dt = Shove.Database.MSSQL.Select("SELECT StopSellTime from T_PassRate where MatchID=" + FireTeam);

            if ((dt == null) || (dt.Rows.Count == 0))
            {
                if (subType == "1")
                {
                    context.Response.Write("[竞彩擂台]数据读取错误,请稍后再访问!");
                }
                else
                {
                    context.Response.Write("[竞彩擂台]数据保存错误,请稍后再访问!");
                }
                context.Response.End();
            }

            if (Shove._Convert.StrToDateTime(dt.Rows[0]["StopSellTime"].ToString(), DateTime.Now.AddDays(-1).ToString()).CompareTo(DateTime.Now) < 0)
            {
                if (subType == "1")
                {
                    context.Response.Write("[竞彩擂台]您所选择的场次里面包含了已截止场次, 请重新选择!");
                }
                else
                {
                    context.Response.Write("[竞彩擂台]您所保存的场次里面包含了已截止场次, 请重新选择!");
                }
                context.Response.End();
            }
        }

        //验证方案基本信息
        if (Multiple < 1 || SumMoney < 2 || SumNum < 1)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]输入有错误，请仔细检查。");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存内容有错误，请仔细检查。");
            }
            context.Response.End();
        }

        if (SumMoney != BaseCount * Multiple * Price)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]输入有错误，请仔细检查。");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存内容有错误，请仔细检查。");
            }
            context.Response.End();
        }

       
        if (SumMoney > 2)
        {
            //SaveDataForAliBuy();
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]投注错误，请重新投注！");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存内容有错误，请仔细检查。");
            }
            context.Response.End();
        }
       
        if (SumMoney > 2)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]投注错误，请重新投注！");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存内容有错误，请仔细检查。");
            }
            context.Response.End();
        }
        #endregion
        #region 对彩票号码进行分析，判断注数

        SLS.Lottery slsLottery = new SLS.Lottery();
        string[] t_lotterys = SplitLotteryNumber(codes);

        if ((t_lotterys == null) || (t_lotterys.Length < 1))
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]选号发生异常，请重新选择号码投注，谢谢。(-694)");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案异常，请重新选择号码投注，谢谢。(-694)");
            }
            context.Response.End();
        }

        int ValidNum = 0;

        foreach (string str in t_lotterys)
        {
            string Number = slsLottery[72].AnalyseScheme(str, PlayTypeID);

            if (string.IsNullOrEmpty(Number))
            {
                continue;
            }

            string[] str_s = Number.Split('|');

            if (str_s == null || str_s.Length < 1)
            {
                continue;
            }

            ValidNum += Shove._Convert.StrToInt(str_s[str_s.Length - 1], 0);
        }

        if (ValidNum != SumNum)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]选号发生异常，请重新选择号码投注，谢谢。");
            }
            else
            {
                context.Response.Write("[竞彩擂台]保存方案异常，请重新选择号码投注，谢谢。");
            }
            context.Response.End();
        }

        #endregion

        if (PlayTypeID == 7201)
        {
            int SNumberCount = 0;
            int PNumberCount = 0;
            int FNumberCount = 0;
            string t_str = "";
            long MatchID = 0;

            string[] t_Number = codes.Trim().Replace("][", "|").Split(';')[1].ToString().Split('|');

            StringBuilder sb = new StringBuilder();

            foreach (string str in t_Number)
            {
                SNumberCount = 0;
                PNumberCount = 0;
                FNumberCount = 0;

                if (str.IndexOf("[") >= 0)
                {
                    MatchID = Shove._Convert.StrToLong(str.Substring(str.IndexOf("[") + 1).Split('(')[0], 0);
                }
                else
                {
                    MatchID = Shove._Convert.StrToLong(str.Split('(')[0], 0);
                }

                t_str = str.Split('(')[1];

                if (t_str.IndexOf("1") >= 0)
                {
                    SNumberCount += 1;
                }

                if (t_str.IndexOf("2") >= 0)
                {
                    PNumberCount += 1;
                }

                if (t_str.IndexOf("3") >= 0)
                {
                    FNumberCount += 1;
                }

                if (subType == "1")
                {
                    // 执行存储过程，插入相关数据【擂台红人】
                    int HotBet_Result = DAL.Procedures.P_ChallengeHotBetAdd(MatchID, SNumberCount, PNumberCount, FNumberCount);

                    if (HotBet_Result < 0)
                    {
                        context.Response.Write("[竞彩擂台]擂台红人处理异常，执行存储过程：P_ChallengeHotBetAdd");
                        context.Response.End();
                    }
                }
            }
            if (subType == "1")
            {
                // 执行存储过程，累计投注场次
                int BetRed_Result = DAL.Procedures.P_ChallengeBetRedAdd(_User.ID);
                if (BetRed_Result < 0)
                {
                    context.Response.Write("[竞彩擂台]擂台红人处理异常，执行存储过程：P_ChallengeBetRedAdd");
                    context.Response.End();
                }
            }
        }

        #region 投注区域

        long IsuseID = 0;

        dt = new DAL.Tables.T_Isuses().Open("ID", "LotteryID= 72", "EndTime desc");

        if (dt == null)
        {
            context.Response.Write("数据读取错误,请稍后再访问!");

            context.Response.End();
        }

        if (dt.Rows.Count < 1)
        {
            context.Response.Write("期号不存在，请添加期号！");

            context.Response.End();
        }

        IsuseID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), 0);
        
        string sql = "";
        
        //第二步：保存到数据库
        if (subType == "1")
        {
            sql = string.Format("insert into T_ChallengeScheme(InitiateUserId,IsuseID,PlayTypeID,Money,LotteryNumber,IsOpened,Odds) values({0},{1},{2},{3},'{4}',{5},'{6}')"
                , _User.ID, IsuseID, PlayTypeID, 0.00, codes, 0, Odds);
        }
        else
        {
            sql = string.Format("insert into T_ChallengeSaveScheme(InitiateUserId,IsuseID,PlayTypeID,Money,LotteryNumber,IsOpened,Odds) values({0},{1},{2},{3},'{4}',{5},'{6}')"
                , _User.ID, IsuseID, PlayTypeID, 0.00, codes, 0, Odds);

            DataTable dt22 = Shove.Database.MSSQL.Select("select * from T_ChallengeSaveScheme");
        }
        
        int Scheme_Challenge = Shove.Database.MSSQL.ExecuteNonQuery(sql.ToString());
        if (Scheme_Challenge < 0)
        {
            if (subType == "1")
            {
                context.Response.Write("[竞彩擂台]用户投注出错，执行语句：" + sql);
            }
            else
            {
                context.Response.Write("[竞彩擂台]用户保存方案出错，执行语句：" + sql);
            }
            context.Response.End();
        }
        if (subType == "1")
        {
            context.Response.Write("true");
        }
        else
        {
            context.Response.Write("ok");
        }

        #endregion
    }


    private string[] SplitLotteryNumber(string Number)
    {
        string[] s = Number.Split('\n');
        if (s.Length == 0)
            return null;
        for (int i = 0; i < s.Length; i++)
            s[i] = s[i].Trim();
        return s;
    }

    /// <summary>
    /// 今天是否已经投注
    /// </summary>
    /// <returns></returns> 
    private bool checkeBuyDateTime()
    {
        string sql = "select top 1 DateTime from T_ChallengeScheme where InitiateUserId =" + _User.ID + " order by DateTime desc";
        DataTable dt = Shove.Database.MSSQL.Select(sql);

        if (dt == null)
        {
            return false;
        }

        if (dt.Rows.Count < 1)
        {
            return true;
        }
        
        string date = Shove._Convert.StrToDateTime(dt.Rows[0][0].ToString(),"").ToString("yyyyMMdd");
        if(string.IsNullOrEmpty(date))
        {
            return false;
        }

        string toDay = DateTime.Now.Year + GetDateZero(DateTime.Now.Month) + GetDateZero(DateTime.Now.Day);
        
        if(date.Equals(toDay))
        {
            return false;
        }
        return true;        
    }

    /// <summary>
    /// 保存方案是否已经超出范围
    /// </summary>
    /// <returns></returns> 
    private bool checkeMaxSave()
    {
        // 最大保存方案数量
        int MaxSaveScheme = 300;
        
        if (_User == null) return false;
        if (_User.ID < 1) return false;

        int count = Shove._Convert.StrToInt(Shove.Database.MSSQL.ExecuteScalar("select Count(*) from T_ChallengeSaveScheme where InitiateUserId = " + _User.ID).ToString(), -1001);
        
        
        if (0 < MaxSaveScheme)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    private string GetDateZero(int date)
    {
        return date < 10 ? "0" + date : date + "";
    }
}