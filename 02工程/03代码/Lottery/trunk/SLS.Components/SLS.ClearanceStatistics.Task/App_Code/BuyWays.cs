using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Sql;

using Shove.Database;

namespace SLS.ClearanceStatistics.Task
{
    public class BuyWays
    {
        private string ConnectionString;// 数据库连接对象

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        private int lCount = 0;
        
        private System.Threading.Thread thread;
        private bool IsOpened = false;
        private long gCount1 = 0;

        public BuyWays(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void Run()
        {
            // 已经启动
            if (State == 1)
            {
                return;
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                new Log("System").Write("AutoMaticBuyWays_Task.");
            }
        }

        public void Do()
        {
            while (true)
            {
                if (State == 2)
                {
                    State = 0;
                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                lCount++;

                #region Config.ini 配置时间单位：秒, 取一次开奖号码并开奖,默认设置为45秒

                if (gCount1 >= 600)
                {
                    gCount1 = 0;

                    try
                    {
                        // 过关统计
                        ClearanceStatistics();
                        // 过关统计 - 擂台 
                        ClearanceStatistics_Challenge();
                    }
                    catch (Exception e)
                    {
                        new Log("System").Write("ClearanceStatistics Fail:" + e.Message);
                    }
                }

                if (lCount >= 1800)
                {
                    lCount = 0;
                    try
                    {
                        // 擂台开奖
                        GetLotteryOpenNumberAndOpenWin();
                    }
                    catch (Exception ex)
                    {
                        new Log("System").Write("LotteryOpenNumberAndOpenWin Fail:" + ex.Message);                        
                    }
                }

                #endregion
            }
        }

        public void Exit()
        {
            State = 2;
        }

        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        public void ClearanceStatistics()
        {
            DataTable dt1 = new DAL.Tables.T_Schemes().Open(ConnectionString, "ID, LotteryNumber,SchemeNumber,InitiateUserID,PlayTypeID, Multiple", "Buyed = 1 and not exists(select ID from T_BuyWays where T_Schemes.ID = T_BuyWays.SchemeID and Type = 1) and PlayTypeID between 7200 and 7300", "");

            if (dt1 == null)
            {
                new Log("System").Write("T_Schemes表繁忙，请稍候再读");


                return;
            }

            if (dt1.Rows.Count < 1)
            {
                return;
            }

            string InitiateUserID = "";
            string LotteryNumber = "";
            string SchemeNumber = "";
            int SchemeLength = 0;
            int PlayTypeID = 0;
            string BuyWays = "";

            DataTable dtMatch1 = null;
            DataRow[] drMatch1 = null;

            foreach (DataRow dr in dt1.Rows)
            {
                LotteryNumber = dr["LotteryNumber"].ToString();

                PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 7201);

                SchemeNumber = dr["SchemeNumber"].ToString();

                SchemeLength = LotteryNumber.Split(';').Length;

                if (SchemeLength < 3)
                {
                    new Log("System").Write("方案内容错误");//写错误日志

                    continue;
                }

                BuyWays = GetPassWay(LotteryNumber);

                string BuyNumber = LotteryNumber.Trim().Split(';')[1].ToString();

                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                int GamesNumber = Numbers.Split('|').Length;    //选择场次

                string Locate = "";
                string[] Matchs = new string[GamesNumber];
                string[] BuyResutl = new string[GamesNumber];
                string Matchids1 = "";
                string Result1 = "";
                int GamesNumber1 = 0;

                for (int i = 0; i < GamesNumber; i++)
                {
                    Locate = Numbers.Split('|')[i];

                    Matchs[i] = Locate.Substring(0, Locate.IndexOf('('));
                    BuyResutl[i] = Locate.Substring(Locate.IndexOf('(') + 1, (Locate.IndexOf(')') - Locate.IndexOf('(') - 1));

                    Matchids1 += Locate.Substring(0, Locate.IndexOf('(')) + ",";
                }

                if (Matchids1.EndsWith(","))
                {
                    Matchids1 = Matchids1.Substring(0, Matchids1.Length - 1);
                }

                if (string.IsNullOrEmpty(Matchids1))
                {
                    continue;
                }

                dtMatch1 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult", "id in (" + Matchids1 + ") and isnull(SPFResult, '') <> '' and IsOpened = 1", ""); // 查询字段

                if (dtMatch1 == null)
                {
                    new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                    continue;
                }

                if (dtMatch1.Rows.Count < 1)
                {
                    new Log("System").Write("T_Match表没数据");

                    // 写日志
                    continue;
                }

                if (dtMatch1.Rows.Count != Matchids1.Split(',').Length)
                {
                    continue;
                }

                string MatchResult = " ";

                #region 循环比较赛果
                for (int j = 0; j < Matchs.Length; j++)
                {
                    drMatch1 = dtMatch1.Select("ID=" + Matchs[j]);

                    if (drMatch1.Length < 1)
                    {
                        continue;
                    }

                    switch (PlayTypeID)
                    {
                        case 7201:
                            Result1 = drMatch1[0]["SPFResult"].ToString();
                            MatchResult = Get7201(Result1);
                            break;
                        case 7204:
                            Result1 = drMatch1[0]["BQCResult"].ToString();
                            MatchResult = Get7204(Result1);
                            break;
                        case 7203:
                            Result1 = drMatch1[0]["ZJQSResult"].ToString();
                            MatchResult = Get7203(Result1);
                            break;
                        case 7202:
                            Result1 = drMatch1[0]["ZQBFResult"].ToString();
                            MatchResult = Get7202(Result1);
                            break;
                        default:
                            break;
                    }

                    if (BuyResutl[j].Contains(MatchResult))
                    {
                        GamesNumber1++;  //命中场次
                    }

                    // 比较赛果
                }
                #endregion

                string CanonicalNumber = "";
                int count1 = 0;
                DataTable dtMatch2 = null;
                DataRow[] drMatch2 = null;
                string Result2 = "";
                int num = 0;
                int count2 = 0;
                string Locate2 = "";
                string Matchids2 = "";

                double WinMoney = 0;
                double T_WinMoney = 0;

                string[] strs = LotteryNumber.Split('\r');
                string[] LotteryNumbers = null;

                if (strs.Length < 1)
                {
                    continue;
                }

                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str.Replace("\n", "").Replace("\r", "")))
                    {
                        continue;
                    }

                    LotteryNumbers = new Lottery()[72].ToSingle(str, ref CanonicalNumber, PlayTypeID);

                    if (LotteryNumbers.Length < 1)
                    {
                        continue;
                    }

                    bool IsWin = true;

                    count1 = LotteryNumbers.Length;  //注数

                    for (int k = 0; k < count1; k++)
                    {
                        IsWin = true;

                        num = LotteryNumbers[k].Split('|').Length;

                        string[] Screenings = new string[num];

                        string[] LocateBuyResult = new string[num];

                        Matchids2 = " ";

                        for (int l = 0; l < num; l++)
                        {
                            Locate2 = LotteryNumbers[k].Split(';')[1].Substring(1, LotteryNumbers[k].Split(';')[1].Length - 2).Split('|')[l];

                            Screenings[l] = Locate2.Substring(0, Locate2.IndexOf('('));

                            LocateBuyResult[l] = Locate2.Substring(Locate2.IndexOf('(') + 1, (Locate2.IndexOf(')') - Locate2.IndexOf('(') - 1));

                            Matchids2 += Locate2.Substring(0, Locate2.IndexOf('(')) + ",";
                        }

                        if (Matchids2.EndsWith(","))
                        {
                            Matchids2 = Matchids2.Substring(0, Matchids2.Length - 1);
                        }

                        if (string.IsNullOrEmpty(Matchids2))
                        {
                            continue;
                        }

                        dtMatch2 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult, SPFBonus, BQCBonus, ZJQSBonus, ZQBFBonus", "id in (" + Matchids2 + ")", ""); // 查询字段

                        if (dtMatch2 == null)
                        {
                            new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                            continue;
                        }

                        if (dtMatch2.Rows.Count < 1)
                        {
                            new Log("System").Write("T_Match表没数据");// 写日志

                            continue;
                        }
                        string MatchResult2 = "";

                        for (int m = 0; m < Screenings.Length; m++)
                        {
                            drMatch2 = dtMatch2.Select("ID=" + Screenings[m]);

                            if (drMatch2.Length < 1)
                            {
                                continue;
                            }

                            switch (PlayTypeID)
                            {
                                case 7201:
                                    Result2 = drMatch2[0]["SPFResult"].ToString();
                                    MatchResult2 = Get7201(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["SPFBonus"].ToString(), 0);
                                    break;
                                case 7204:
                                    Result2 = drMatch2[0]["BQCResult"].ToString();
                                    MatchResult2 = Get7204(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["BQCBonus"].ToString(), 0);
                                    break;
                                case 7203:
                                    Result2 = drMatch2[0]["ZJQSResult"].ToString();
                                    MatchResult2 = Get7203(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZJQSBonus"].ToString(), 0);
                                    break;
                                case 7202:
                                    Result2 = drMatch2[0]["ZQBFResult"].ToString();
                                    MatchResult2 = Get7202(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZQBFBonus"].ToString(), 0);
                                    break;
                                default:
                                    break;
                            }

                            if (!LocateBuyResult[m].Equals(MatchResult2))
                            {
                                IsWin = false;
                                continue;
                            }
                            else if (BuyWays.Equals("单关"))
                            {
                                WinMoney += T_WinMoney * Shove._Convert.StrToInt(dr["Multiple"].ToString(), 1);
                            }
                        }

                        if (IsWin)
                        {
                            count2++;  //中奖注数
                        }
                    }
                }

                InitiateUserID = dr["InitiateUserID"].ToString();

                DataTable users = new DAL.Tables.T_Users().Open(ConnectionString, "Name", "ID =" + InitiateUserID, "");

                if (users == null)
                {
                    new Log("System").Write("T_Users表繁忙，请稍候再读");

                    return;
                }

                if (users.Rows.Count < 1)
                {
                    new Log("System").Write("T_Users表没数据");

                    continue;
                }

                string Name = users.Rows[0]["Name"].ToString();

                DAL.Tables.T_BuyWays bw = new DAL.Tables.T_BuyWays();
                bw.SchemeID.Value = Shove._Convert.StrToLong(dr["ID"].ToString(), 0);
                bw.SchemeNumber.Value = SchemeNumber;
                bw.Name.Value = Name;
                bw.PlayTypeID.Value = PlayTypeID;
                bw.Count1.Value = count1;
                bw.BuyWays.Value = BuyWays;
                bw.Count2.Value = count2;
                bw.GameNumber.Value = GamesNumber;
                bw.GameNumber2.Value = GamesNumber1;
                bw.Rate.Value = GamesNumber1 * 1.00 / GamesNumber * 1.00;
                bw.UserID.Value = InitiateUserID;
                bw.Type.Value = 1;

                long Result = bw.Insert(ConnectionString);

                if (Result < 0)
                {
                    new Log("System").Write("BuyWays数据表插入数据不成功");
                }

                if (WinMoney > 0)
                {
                    DAL.Tables.T_Schemes t_Schemes = new DAL.Tables.T_Schemes();

                    t_Schemes.WinMoney.Value = WinMoney;
                    t_Schemes.WinMoneyNoWithTax.Value = WinMoney;
                    t_Schemes.WinDescription.Value = "中奖奖金：" + WinMoney.ToString();

                    Result = t_Schemes.Update(ConnectionString, "id=" + dr["ID"].ToString());

                    if (Result < 0)
                    {
                        new Log("System").Write("单关自动开奖出现错误，方案ID：" + Shove._Convert.StrToLong(dr["ID"].ToString(), 0).ToString());
                    }
                }
                else if (count2 < 1)
                {
                    DAL.Tables.T_Schemes t_Schemes = new DAL.Tables.T_Schemes();

                    t_Schemes.WinMoney.Value = WinMoney;
                    t_Schemes.WinMoneyNoWithTax.Value = WinMoney;
                    t_Schemes.WinDescription.Value = "";
                    t_Schemes.isOpened.Value = true;

                    Result = t_Schemes.Update(ConnectionString, "id=" + dr["ID"].ToString());

                    if (Result < 0)
                    {
                        new Log("System").Write("单关自动开奖出现错误，方案ID：" + Shove._Convert.StrToLong(dr["ID"].ToString(), 0).ToString());
                    }
                }
            }
        }

        public void ClearanceStatistics_Challenge()
        {
            // 查询擂台方案表
            DataTable dt1 = new DAL.Tables.T_ChallengeScheme().Open(ConnectionString, "ID, LotteryNumber,SchemeNumber,InitiateUserID,PlayTypeID,1 as Multiple", " not exists(select ID from T_BuyWays where T_ChallengeScheme.ID = T_BuyWays.SchemeID and Type = 2)", "");

            if (dt1 == null)
            {
                new Log("System").Write("T_Schemes表繁忙，请稍候再读");


                return;
            }

            if (dt1.Rows.Count < 1)
            {
                return;
            }

            string InitiateUserID = "";
            string LotteryNumber = "";
            string SchemeNumber = "";
            int SchemeLength = 0;
            int PlayTypeID = 0;
            string BuyWays = "";

            DataTable dtMatch1 = null;
            DataRow[] drMatch1 = null;

            foreach (DataRow dr in dt1.Rows)
            {
                LotteryNumber = dr["LotteryNumber"].ToString();
                PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 7201);

                SchemeNumber = dr["SchemeNumber"].ToString();

                SchemeLength = LotteryNumber.Split(';').Length;

                if (SchemeLength < 3)
                {
                    new Log("System").Write("方案内容错误");//写错误日志

                    continue;
                }

                BuyWays = GetPassWay(LotteryNumber);

                string BuyNumber = LotteryNumber.Trim().Split(';')[1].ToString();

                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                int GamesNumber = Numbers.Split('|').Length;    //选择场次

                string Locate = "";
                string[] Matchs = new string[GamesNumber];
                string[] BuyResutl = new string[GamesNumber];
                string Matchids1 = "";
                string Result1 = "";
                int GamesNumber1 = 0;

                for (int i = 0; i < GamesNumber; i++)
                {
                    Locate = Numbers.Split('|')[i];

                    Matchs[i] = Locate.Substring(0, Locate.IndexOf('('));
                    BuyResutl[i] = Locate.Substring(Locate.IndexOf('(') + 1, (Locate.IndexOf(')') - Locate.IndexOf('(') - 1));

                    Matchids1 += Locate.Substring(0, Locate.IndexOf('(')) + ",";
                }

                if (Matchids1.EndsWith(","))
                {
                    Matchids1 = Matchids1.Substring(0, Matchids1.Length - 1);
                }

                if (string.IsNullOrEmpty(Matchids1))
                {
                    continue;
                }

                dtMatch1 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult", "id in (" + Matchids1 + ") and isnull(SPFResult, '') <> '' and IsOpened = 1", ""); // 查询字段

                if (dtMatch1 == null)
                {
                    new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                    continue;
                }

                if (dtMatch1.Rows.Count < 1)
                {
                    new Log("System").Write("T_Match表没数据");

                    // 写日志
                    continue;
                }

                if (dtMatch1.Rows.Count != Matchids1.Split(',').Length)
                {
                    continue;
                }

                string MatchResult = " ";

                #region 循环比较赛果
                for (int j = 0; j < Matchs.Length; j++)
                {
                    drMatch1 = dtMatch1.Select("ID=" + Matchs[j]);

                    if (drMatch1.Length < 1)
                    {
                        continue;
                    }

                    switch (PlayTypeID)
                    {
                        case 7201:
                            Result1 = drMatch1[0]["SPFResult"].ToString();
                            MatchResult = Get7201(Result1);
                            break;
                        case 7204:
                            Result1 = drMatch1[0]["BQCResult"].ToString();
                            MatchResult = Get7204(Result1);
                            break;
                        case 7203:
                            Result1 = drMatch1[0]["ZJQSResult"].ToString();
                            MatchResult = Get7203(Result1);
                            break;
                        case 7202:
                            Result1 = drMatch1[0]["ZQBFResult"].ToString();
                            MatchResult = Get7202(Result1);
                            break;
                        default:
                            break;
                    }

                    if (BuyResutl[j].Contains(MatchResult))
                    {
                        GamesNumber1++;  //命中场次
                    }

                    // 比较赛果
                }
                #endregion

                string CanonicalNumber = "";
                int count1 = 0;
                DataTable dtMatch2 = null;
                DataRow[] drMatch2 = null;
                string Result2 = "";
                int num = 0;
                int count2 = 0;
                string Locate2 = "";
                string Matchids2 = "";

                double WinMoney = 0;
                double T_WinMoney = 0;

                string[] strs = LotteryNumber.Split('\r');
                string[] LotteryNumbers = null;

                if (strs.Length < 1)
                {
                    continue;
                }

                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str.Replace("\n", "").Replace("\r", "")))
                    {
                        continue;
                    }

                    LotteryNumbers = new Lottery()[72].ToSingle(str, ref CanonicalNumber, PlayTypeID);

                    if (LotteryNumbers.Length < 1)
                    {
                        continue;
                    }

                    bool IsWin = true;

                    count1 = LotteryNumbers.Length;  //注数

                    for (int k = 0; k < count1; k++)
                    {
                        IsWin = true;

                        num = LotteryNumbers[k].Split('|').Length;

                        string[] Screenings = new string[num];

                        string[] LocateBuyResult = new string[num];

                        Matchids2 = " ";

                        for (int l = 0; l < num; l++)
                        {
                            Locate2 = LotteryNumbers[k].Split(';')[1].Substring(1, LotteryNumbers[k].Split(';')[1].Length - 2).Split('|')[l];

                            Screenings[l] = Locate2.Substring(0, Locate2.IndexOf('('));


                            LocateBuyResult[l] = Locate2.Substring(Locate2.IndexOf('(') + 1, (Locate2.IndexOf(')') - Locate2.IndexOf('(') - 1));

                            Matchids2 += Locate2.Substring(0, Locate2.IndexOf('(')) + ",";
                        }

                        if (Matchids2.EndsWith(","))
                        {
                            Matchids2 = Matchids2.Substring(0, Matchids2.Length - 1);
                        }

                        if (string.IsNullOrEmpty(Matchids2))
                        {
                            continue;
                        }

                        dtMatch2 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult, SPFBonus, BQCBonus, ZJQSBonus, ZQBFBonus", "id in (" + Matchids2 + ")", ""); // 查询字段

                        if (dtMatch2 == null)
                        {
                            new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                            continue;
                        }

                        if (dtMatch2.Rows.Count < 1)
                        {
                            new Log("System").Write("T_Match表没数据");// 写日志

                            continue;
                        }
                        string MatchResult2 = "";

                        for (int m = 0; m < Screenings.Length; m++)
                        {
                            drMatch2 = dtMatch2.Select("ID=" + Screenings[m]);

                            if (drMatch2.Length < 1)
                            {
                                continue;
                            }

                            switch (PlayTypeID)
                            {
                                case 7201:
                                    Result2 = drMatch2[0]["SPFResult"].ToString();
                                    MatchResult2 = Get7201(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["SPFBonus"].ToString(), 0);
                                    break;
                                case 7204:
                                    Result2 = drMatch2[0]["BQCResult"].ToString();
                                    MatchResult2 = Get7204(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["BQCBonus"].ToString(), 0);
                                    break;
                                case 7203:
                                    Result2 = drMatch2[0]["ZJQSResult"].ToString();
                                    MatchResult2 = Get7203(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZJQSBonus"].ToString(), 0);
                                    break;
                                case 7202:
                                    Result2 = drMatch2[0]["ZQBFResult"].ToString();
                                    MatchResult2 = Get7202(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZQBFBonus"].ToString(), 0);
                                    break;
                                default:
                                    break;
                            }

                            if (!LocateBuyResult[m].Equals(MatchResult2))
                            {
                                IsWin = false;
                                continue;
                            }
                            else if (BuyWays.Equals("单关"))
                            {
                                WinMoney += T_WinMoney * Shove._Convert.StrToInt(dr["Multiple"].ToString(), 1);
                            }
                        }

                        if (IsWin)
                        {
                            count2++;  //中奖注数
                        }
                    }
                }

                InitiateUserID = dr["InitiateUserID"].ToString();

                DataTable users = new DAL.Tables.T_Users().Open(ConnectionString, "Name", "ID =" + InitiateUserID, "");

                if (users == null)
                {
                    new Log("System").Write("T_Users表繁忙，请稍候再读");

                    return;
                }

                if (users.Rows.Count < 1)
                {
                    new Log("System").Write("T_Users表没数据");

                    continue;
                }

                string Name = users.Rows[0]["Name"].ToString();

                DAL.Tables.T_BuyWays bw = new DAL.Tables.T_BuyWays();
                bw.SchemeID.Value = Shove._Convert.StrToLong(dr["ID"].ToString(), 0);
                bw.SchemeNumber.Value = SchemeNumber;
                bw.Name.Value = Name;
                bw.PlayTypeID.Value = PlayTypeID;
                bw.Count1.Value = count1;
                bw.BuyWays.Value = BuyWays;
                bw.Count2.Value = count2;
                bw.GameNumber.Value = GamesNumber;
                bw.GameNumber2.Value = GamesNumber1;
                bw.Rate.Value = GamesNumber1 * 1.00 / GamesNumber * 1.00;
                bw.UserID.Value = InitiateUserID;
                bw.Type.Value = 2;

                long Result = bw.Insert(ConnectionString);

                if (Result < 0)
                {
                    new Log("System").Write("BuyWays数据表插入数据不成功");
                }
            }
        }

        public void ClearanceStatistics_Challenge_Save()
        {
            // 查询擂台方案表
            DataTable dt1 = new DAL.Tables.T_ChallengeSaveScheme().Open(ConnectionString, "ID, LotteryNumber,SchemeNumber,InitiateUserID,PlayTypeID,1 as Multiple", " not exists(select ID from T_BuyWays where T_ChallengeSaveScheme.ID = T_BuyWays.SchemeID and Type = 3)", "");
            if (dt1 == null)
            {
                new Log("System").Write("T_Schemes表繁忙，请稍候再读");


                return;
            }

            if (dt1.Rows.Count < 1)
            {
                return;
            }

            string InitiateUserID = "";
            string LotteryNumber = "";
            string SchemeNumber = "";
            int SchemeLength = 0;
            int PlayTypeID = 0;
            string BuyWays = "";

            DataTable dtMatch1 = null;
            DataRow[] drMatch1 = null;

            foreach (DataRow dr in dt1.Rows)
            {
                LotteryNumber = dr["LotteryNumber"].ToString();

                PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), 7201);

                SchemeNumber = dr["SchemeNumber"].ToString();

                SchemeLength = LotteryNumber.Split(';').Length;

                if (SchemeLength < 3)
                {
                    new Log("System").Write("方案内容错误");//写错误日志

                    continue;
                }

                BuyWays = GetPassWay(LotteryNumber);

                string BuyNumber = LotteryNumber.Trim().Split(';')[1].ToString();

                string Numbers = BuyNumber.Substring(1, BuyNumber.Length - 1).Substring(0, BuyNumber.Length - 2).ToString().Trim();

                int GamesNumber = Numbers.Split('|').Length;    //选择场次

                string Locate = "";
                string[] Matchs = new string[GamesNumber];
                string[] BuyResutl = new string[GamesNumber];
                string Matchids1 = "";
                string Result1 = "";
                int GamesNumber1 = 0;

                for (int i = 0; i < GamesNumber; i++)
                {
                    Locate = Numbers.Split('|')[i];

                    Matchs[i] = Locate.Substring(0, Locate.IndexOf('('));
                    BuyResutl[i] = Locate.Substring(Locate.IndexOf('(') + 1, (Locate.IndexOf(')') - Locate.IndexOf('(') - 1));

                    Matchids1 += Locate.Substring(0, Locate.IndexOf('(')) + ",";
                }

                if (Matchids1.EndsWith(","))
                {
                    Matchids1 = Matchids1.Substring(0, Matchids1.Length - 1);
                }

                if (string.IsNullOrEmpty(Matchids1))
                {
                    continue;
                }

                dtMatch1 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult", "id in (" + Matchids1 + ") and isnull(SPFResult, '') <> '' and IsOpened = 1", ""); // 查询字段

                if (dtMatch1 == null)
                {
                    new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                    continue;
                }

                if (dtMatch1.Rows.Count < 1)
                {
                    new Log("System").Write("T_Match表没数据");

                    // 写日志
                    continue;
                }

                if (dtMatch1.Rows.Count != Matchids1.Split(',').Length)
                {
                    continue;
                }

                string MatchResult = " ";

                #region 循环比较赛果
                for (int j = 0; j < Matchs.Length; j++)
                {
                    drMatch1 = dtMatch1.Select("ID=" + Matchs[j]);

                    if (drMatch1.Length < 1)
                    {
                        continue;
                    }

                    switch (PlayTypeID)
                    {
                        case 7201:
                            Result1 = drMatch1[0]["SPFResult"].ToString();
                            MatchResult = Get7201(Result1);
                            break;
                        case 7204:
                            Result1 = drMatch1[0]["BQCResult"].ToString();
                            MatchResult = Get7204(Result1);
                            break;
                        case 7203:
                            Result1 = drMatch1[0]["ZJQSResult"].ToString();
                            MatchResult = Get7203(Result1);
                            break;
                        case 7202:
                            Result1 = drMatch1[0]["ZQBFResult"].ToString();
                            MatchResult = Get7202(Result1);
                            break;
                        default:
                            break;
                    }

                    if (BuyResutl[j].Contains(MatchResult))
                    {
                        GamesNumber1++;  //命中场次
                    }

                    // 比较赛果
                }
                #endregion

                string CanonicalNumber = "";
                int count1 = 0;
                DataTable dtMatch2 = null;
                DataRow[] drMatch2 = null;
                string Result2 = "";
                int num = 0;
                int count2 = 0;
                string Locate2 = "";
                string Matchids2 = "";

                double WinMoney = 0;
                double T_WinMoney = 0;

                string[] strs = LotteryNumber.Split('\r');
                string[] LotteryNumbers = null;

                if (strs.Length < 1)
                {
                    continue;
                }

                foreach (string str in strs)
                {
                    if (string.IsNullOrEmpty(str.Replace("\n", "").Replace("\r", "")))
                    {
                        continue;
                    }

                    LotteryNumbers = new Lottery()[72].ToSingle(str, ref CanonicalNumber, PlayTypeID);

                    if (LotteryNumbers.Length < 1)
                    {
                        continue;
                    }

                    bool IsWin = true;

                    count1 = LotteryNumbers.Length;  //注数

                    for (int k = 0; k < count1; k++)
                    {
                        IsWin = true;

                        num = LotteryNumbers[k].Split('|').Length;

                        string[] Screenings = new string[num];

                        string[] LocateBuyResult = new string[num];

                        Matchids2 = " ";

                        for (int l = 0; l < num; l++)
                        {
                            Locate2 = LotteryNumbers[k].Split(';')[1].Substring(1, LotteryNumbers[k].Split(';')[1].Length - 2).Split('|')[l];

                            Screenings[l] = Locate2.Substring(0, Locate2.IndexOf('('));


                            LocateBuyResult[l] = Locate2.Substring(Locate2.IndexOf('(') + 1, (Locate2.IndexOf(')') - Locate2.IndexOf('(') - 1));

                            Matchids2 += Locate2.Substring(0, Locate2.IndexOf('(')) + ",";
                        }

                        if (Matchids2.EndsWith(","))
                        {
                            Matchids2 = Matchids2.Substring(0, Matchids2.Length - 1);
                        }

                        if (string.IsNullOrEmpty(Matchids2))
                        {
                            continue;
                        }

                        dtMatch2 = new DAL.Tables.T_Match().Open(ConnectionString, "ID,SPFResult, BQCResult, ZJQSResult, ZQBFResult, SPFBonus, BQCBonus, ZJQSBonus, ZQBFBonus", "id in (" + Matchids2 + ")", ""); // 查询字段

                        if (dtMatch2 == null)
                        {
                            new Log("System").Write("T_Match表繁忙，请稍候再读");// 写日志

                            continue;
                        }

                        if (dtMatch2.Rows.Count < 1)
                        {
                            new Log("System").Write("T_Match表没数据");// 写日志

                            continue;
                        }
                        string MatchResult2 = "";

                        for (int m = 0; m < Screenings.Length; m++)
                        {
                            drMatch2 = dtMatch2.Select("ID=" + Screenings[m]);

                            if (drMatch2.Length < 1)
                            {
                                continue;
                            }

                            switch (PlayTypeID)
                            {
                                case 7201:
                                    Result2 = drMatch2[0]["SPFResult"].ToString();
                                    MatchResult2 = Get7201(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["SPFBonus"].ToString(), 0);
                                    break;
                                case 7204:
                                    Result2 = drMatch2[0]["BQCResult"].ToString();
                                    MatchResult2 = Get7204(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["BQCBonus"].ToString(), 0);
                                    break;
                                case 7203:
                                    Result2 = drMatch2[0]["ZJQSResult"].ToString();
                                    MatchResult2 = Get7203(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZJQSBonus"].ToString(), 0);
                                    break;
                                case 7202:
                                    Result2 = drMatch2[0]["ZQBFResult"].ToString();
                                    MatchResult2 = Get7202(Result2);
                                    T_WinMoney = Shove._Convert.StrToDouble(drMatch2[0]["ZQBFBonus"].ToString(), 0);
                                    break;
                                default:
                                    break;
                            }

                            if (!LocateBuyResult[m].Equals(MatchResult2))
                            {
                                IsWin = false;
                                continue;
                            }
                            else if (BuyWays.Equals("单关"))
                            {
                                WinMoney += T_WinMoney * Shove._Convert.StrToInt(dr["Multiple"].ToString(), 1);
                            }
                        }

                        if (IsWin)
                        {
                            count2++;  //中奖注数
                        }
                    }
                }

                InitiateUserID = dr["InitiateUserID"].ToString();

                DataTable users = new DAL.Tables.T_Users().Open(ConnectionString, "Name", "ID =" + InitiateUserID, "");

                if (users == null)
                {
                    new Log("System").Write("T_Users表繁忙，请稍候再读");

                    return;
                }

                if (users.Rows.Count < 1)
                {
                    new Log("System").Write("T_Users表没数据");

                    continue;
                }

                string Name = users.Rows[0]["Name"].ToString();


                DAL.Tables.T_BuyWays bw = new DAL.Tables.T_BuyWays();
                bw.SchemeID.Value = Shove._Convert.StrToLong(dr["ID"].ToString(), 0);
                bw.SchemeNumber.Value = SchemeNumber;
                bw.Name.Value = Name;
                bw.PlayTypeID.Value = PlayTypeID;
                bw.Count1.Value = count1;
                bw.BuyWays.Value = BuyWays;
                bw.Count2.Value = count2;
                bw.GameNumber.Value = GamesNumber;
                bw.GameNumber2.Value = GamesNumber1;
                bw.Rate.Value = GamesNumber1 * 1.00 / GamesNumber * 1.00;
                bw.UserID.Value = InitiateUserID;
                bw.Type.Value = 3;

                long Result = bw.Insert(ConnectionString);

                if (Result < 0)
                {
                    new Log("System").Write("BuyWays数据表插入数据不成功");
                }
            }
        }

        #region 类型转换

        private string Get7201(string num)
        {
            string res = string.Empty;
            switch (num)
            {
                case "胜": res = "1";
                    break;
                case "平": res = "2";
                    break;
                case "负": res = "3";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }

        private string Get7202(string num)
        {
            string res = string.Empty;
            switch (num)
            {
                case "1:0": res = "1";
                    break;
                case "2:0": res = "2";
                    break;
                case "2:1": res = "3";
                    break;
                case "3:0": res = "4";
                    break;
                case "3:1": res = "5";
                    break;
                case "3:2": res = "6";
                    break;
                case "4:0": res = "7";
                    break;
                case "4:1": res = "8";
                    break;
                case "4:2": res = "9";
                    break;
                case "5:0": res = "10";
                    break;
                case "5:1": res = "11";
                    break;
                case "5:2": res = "12";
                    break;
                case "胜其它": res = "13";
                    break;
                case "0:0": res = "14";
                    break;
                case "1:1": res = "15";
                    break;
                case "2:2": res = "16";
                    break;
                case "3:3": res = "17";
                    break;
                case "平其它": res = "18";
                    break;
                case "0:1": res = "19";
                    break;
                case "0:2": res = "20";
                    break;
                case "1:2": res = "21";
                    break;
                case "0:3": res = "22";
                    break;
                case "1:3": res = "23";
                    break;
                case "2:3": res = "24";
                    break;
                case "0:4": res = "25";
                    break;
                case "1:4": res = "26";
                    break;
                case "2:4": res = "27";
                    break;
                case "0:5": res = "28";
                    break;
                case "1:5": res = "29";
                    break;
                case "2:5": res = "30";
                    break;
                case "负其它": res = "31";
                    break;


                default:
                    res = "";
                    break;
            }

            return res;
        }

        private string Get7203(string num)
        {
            string res = string.Empty;
            switch (num)
            {
                case "0": res = "1";
                    break;
                case "1": res = "2";
                    break;
                case "2": res = "3";
                    break;
                case "3": res = "4";
                    break;
                case "4": res = "5";
                    break;
                case "5": res = "6";
                    break;
                case "6": res = "7";
                    break;
                case "7+": res = "8";
                    break;
                default:
                    res = "";
                    break;
            }
            return res;
        }

        private string Get7204(string num)
        {
            string res = string.Empty;
            switch (num)
            {
                case "胜胜": res = "1";
                    break;
                case "胜平": res = "2";
                    break;
                case "胜负": res = "3";
                    break;
                case "平胜": res = "4";
                    break;
                case "平平": res = "5";
                    break;
                case "平负": res = "6";
                    break;
                case "负胜": res = "7";
                    break;
                case "负平": res = "8";
                    break;
                case "负负": res = "9";
                    break;
                default:
                    res = "";
                    break;
            }

            return res;
        }

        #endregion

        #region 过关方式

        private static string GetPassWay(string val)
        {
            string Group = "单关,2串1,3串1,3串3,3串4,4串1,4串4,4串5,4串6,4串11,5串1,5串5,5串6,5串10,5串16,5串20,5串26,6串1,6串6,6串7,6串15,6串20,6串22,6串35,6串42,6串50,6串57,7串1,7串7,7串8,7串21,7串35,7串120,8串1,8串8,8串9,8串28,8串56,8串70,8串247";

            string letter = "A0AAABACADAEAFAGAHAIAJAKALAMANAOAPAQARASATAUAVAWAXAYAZBABBBCBDBEBFBGBHBIBJBKBLBMBNBOBPBQ";

            string[] Groups = Group.Split(',');

            int len = val.Split(';').Length;

            if (len < 3)
            {
                return "";
            }

            string curType = val.Split(';')[2].ToString().Replace("]", "").Replace("[", "");

            string way = "";
            string g = "";

            foreach (string ways in curType.Split(','))
            {
                g = ways;

                int i = letter.IndexOf(g.Substring(0, 2));

                if (i == -1)
                {
                    g = "A" + g.Substring(0, 1);
                    i = letter.IndexOf(g.Substring(0, 2));
                }

                way += Groups[i / 2] + ",";
            }

            way = way.EndsWith(",") ? way.Substring(0, way.Length - 1) : way;

            return way;
        }

        #endregion

        #region 辅助方法

        private string[] SplitLotteryNumber(string Number)
        {
            string[] s = Number.Split('\n');
            if (s.Length == 0)
                return null;
            for (int i = 0; i < s.Length; i++)
                s[i] = s[i].Trim();
            return s;
        }

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

        #endregion

        #region 擂台开奖

        public void GetLotteryOpenNumberAndOpenWin() // 获取开奖信息，并计算奖金开奖
        {
            // 读取未开奖的擂台方案表
            DataTable dtSchemeChallenge = new DAL.Tables.T_ChallengeScheme().Open(ConnectionString, "[ID],[InitiateUserID],[LotteryNumber],[Odds], PlayTypeID, Money", "IsOpened = 0 and DateTime < CONVERT(VARCHAR(24),GETDATE(),111)", "");

            if (dtSchemeChallenge == null)
            {
                new Log("SystemGetLotteryOpenNumber").Write("比拼擂台方案表，没有可开奖的数据。");

                return;
            }

            if (dtSchemeChallenge.Rows.Count < 1)
            {
                IsOpened = true;

                return;
            }

            string PlayTypeID = "";
            string SchemeID = "";
            string InitiateUserID = "";
            string LotteryNunber = "";
            string Content = "";
            string WaysNumber = "";
            string Matchs = "";

            // 遍历没有开奖的方案
            foreach (DataRow dr in dtSchemeChallenge.Rows)
            {
                //得到方案ID
                SchemeID = dr["ID"].ToString().Trim();

                //得到方案发起用户ID
                InitiateUserID = dr["InitiateUserID"].ToString().Trim();

                //得到投注号码
                LotteryNunber = dr["LotteryNumber"].ToString().Trim();

                PlayTypeID = dr["PlayTypeID"].ToString().Trim();

                #region 对投注号码进行分析，判断注数

                SLS.Lottery slsLottery = new SLS.Lottery();
                string[] t_lotterys = SplitLotteryNumber(LotteryNunber);

                if ((t_lotterys == null) || (t_lotterys.Length < 1))
                {
                    new Log("SystemGetLotteryOpenNumber").Write("投注号码出现异常，读取格式不正确：" + LotteryNunber);
                    return;
                }

                int ValidNum = 0;

                foreach (string str in t_lotterys)
                {
                    string Number = slsLottery[72].AnalyseScheme(str, Shove._Convert.StrToInt(PlayTypeID, 0));

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



                #endregion

                //解析投注号码
                Content = LotteryNunber.Trim().Split(';')[1].ToString(); // 得到投注内容
                WaysNumber = LotteryNunber.Trim().Split(';')[2].ToString(); // 得到过关玩法

                // 得到投注比赛ID、投注内容
                string[] Numbers = Content.Substring(1, Content.Length - 2).Split('|');

                Matchs = "";

                foreach (string Number in Numbers)
                {
                    Matchs += Number.Substring(0, Number.IndexOf('(')) + ",";
                }

                if (Matchs.EndsWith(","))
                {
                    Matchs = Matchs.Substring(0, Matchs.Length - 1);
                }

                if (string.IsNullOrEmpty(Matchs))
                {
                    continue;
                }

                //通过赛事ID 查找赛事结果
                DataTable dt = new DAL.Tables.T_Match().Open(ConnectionString, "[SPFResult], [ID]", "[ID] in (" + Matchs + ") and IsOpened = 1", "[ID]");

                if (dt == null)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("读取 T_Match 数据时候出现异常");

                    continue;
                }

                if (dt.Rows.Count < 1)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("MatchID：(" + Matchs + ")暂未开奖");

                    continue;
                }

                bool IsWin = true;
                string Result = "";
                int WinCoumt = 0;       // 命中场次
                int i = 0;
                string[] matchsId = Matchs.Split(',');

                foreach (string Number in Numbers)
                {
                    DataRow[] drs = dt.Select("ID = " + matchsId[i]);
                    
                    if (drs.Length < 1)
                    {
                        IsWin = false;
                        break;
                    }
                    string ddx = Number.Substring(Number.IndexOf('(')).Substring(1);
                    if (ddx.EndsWith(")"))
                    {
                        ddx = ddx.Substring(0, ddx.Length - 1);
                    }

                    Result = GetResult(drs[0]["SPFResult"].ToString(), PlayTypeID);

                    if (!Result.Equals(ddx))
                    {
                        IsWin = false;
                    }
                    else
                    {
                        WinCoumt++;
                    }
                    i++;
                }

                string sql = "";
                int result = 0;

                // 计算命中场次
                if (WinCoumt > 0)
                {
                    string WinCountSql = "update T_ChallengeBetRed set WinCount = ISNULL(WinCount,0) + " + WinCoumt + " where UserId = " + InitiateUserID;
                    int WinCountResult = Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, WinCountSql);

                    if (WinCountResult < 0)
                    {
                        new Log("SystemGetLotteryOpenNumber").Write("执行Sql语句失败：" + sql);

                        continue;
                    }
                }

                if (!IsWin)
                {// 没有中奖
                    sql = "update T_ChallengeScheme set IsOpened = 1, OpenOperateId = 1  where ID = " + SchemeID;

                    result = Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, sql);

                    if (result < 0)
                    {
                        new Log("SystemGetLotteryOpenNumber").Write("执行Sql语句失败：" + sql);

                        continue;
                    }

                    continue;
                }

                string Odds = dr["odds"].ToString();

                double WinMoney = 2;

                foreach (string Odd in Odds.Split('|'))
                {
                    WinMoney *= Shove._Convert.StrToDouble(Odd, 0);
                }

                // 将 中奖金额 转换为 整型 以 积分的形式显示出来
                int Score = Convert.ToInt32(WinMoney);

                // 修改方案的 IsOpen , WinMoney ,WinDescription
                sql = "update T_ChallengeScheme set IsOpened = 1,WinMoney = " + Score.ToString() + ",WinDescription='比拼擂台中奖', OpenOperateId = 1 where ID = " + SchemeID;

                sql += " update T_ChallengeBetRed set Score = isnull(Score,0) +" + Score.ToString() + " where UserId = " + InitiateUserID;

                result = Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, sql);

                if (result < 0)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("执行Sql语句失败：" + sql);

                    continue;
                }

                new Log("SystemGetLotteryOpenNumber").Write("开奖成功，SchemeID：" + SchemeID);
            }
        }

        public void GetLotteryOpenNumberAndOpenWin_SaveSchemes() // 获取开奖信息，并计算奖金对保存方案开奖
        {
            // 读取未开奖的擂台方案表
            DataTable dtSchemeChallenge = new DAL.Tables.T_ChallengeSaveScheme().Open(ConnectionString, "[ID],[InitiateUserID],[LotteryNumber],[Odds], PlayTypeID, Money", "IsOpened = 0 and DateTime < CONVERT(VARCHAR(24),GETDATE(),111)", "");

            if (dtSchemeChallenge == null)
            {
                new Log("SystemGetLotteryOpenNumber").Write("(保存)比拼擂台方案保存表，没有可开奖的数据。");

                return;
            }

            if (dtSchemeChallenge.Rows.Count < 1)
            {
                IsOpened = true;

                return;
            }

            string PlayTypeID = "";
            string SchemeID = "";
            string InitiateUserID = "";
            string LotteryNunber = "";
            string Content = "";
            string WaysNumber = "";
            string Matchs = "";

            // 遍历没有开奖的方案
            foreach (DataRow dr in dtSchemeChallenge.Rows)
            {
                //得到方案ID
                SchemeID = dr["ID"].ToString().Trim();

                //得到方案发起用户ID
                InitiateUserID = dr["InitiateUserID"].ToString().Trim();

                //得到投注号码
                LotteryNunber = dr["LotteryNumber"].ToString().Trim();

                PlayTypeID = dr["PlayTypeID"].ToString().Trim();

                #region 对投注号码进行分析，判断注数

                SLS.Lottery slsLottery = new SLS.Lottery();
                string[] t_lotterys = SplitLotteryNumber(LotteryNunber);

                if ((t_lotterys == null) || (t_lotterys.Length < 1))
                {
                    new Log("SystemGetLotteryOpenNumber").Write("(保存)投注号码出现异常，读取格式不正确：" + LotteryNunber);
                    return;
                }

                int ValidNum = 0;

                foreach (string str in t_lotterys)
                {
                    string Number = slsLottery[72].AnalyseScheme(str, Shove._Convert.StrToInt(PlayTypeID, 0));

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



                #endregion

                //解析投注号码
                Content = LotteryNunber.Trim().Split(';')[1].ToString(); // 得到投注内容
                WaysNumber = LotteryNunber.Trim().Split(';')[2].ToString(); // 得到过关玩法

                // 得到投注比赛ID、投注内容
                string[] Numbers = Content.Substring(1, Content.Length - 2).Split('|');

                Matchs = "";

                foreach (string Number in Numbers)
                {
                    Matchs += Number.Substring(0, Number.IndexOf('(')) + ",";
                }

                if (Matchs.EndsWith(","))
                {
                    Matchs = Matchs.Substring(0, Matchs.Length - 1);
                }

                if (string.IsNullOrEmpty(Matchs))
                {
                    continue;
                }

                //通过赛事ID 查找赛事结果
                DataTable dt = new DAL.Tables.T_Match().Open(ConnectionString, "[SPFResult], [ID]", "[ID] in (" + Matchs + ") and IsOpened = 1", "[ID]");

                if (dt == null)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("(保存)读取 T_Match 数据时候出现异常");

                    continue;
                }

                if (dt.Rows.Count < 1)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("(保存)MatchID：(" + Matchs + ")暂未开奖");

                    continue;
                }

                bool IsWin = true;
                string Result = "";
                int WinCoumt = 0;       // 命中场次
                int i = 0;
                string[] matchsId = Matchs.Split(',');

                foreach (string Number in Numbers)
                {
                    DataRow[] drs = dt.Select("ID = " + matchsId[i]);



                    if (drs.Length < 1)
                    {
                        IsWin = false;
                        break;
                    }
                    string ddx = Number.Substring(Number.IndexOf('(')).Substring(1);
                    if (ddx.EndsWith(")"))
                    {
                        ddx = ddx.Substring(0, ddx.Length - 1);
                    }

                    Result = GetResult(drs[0]["SPFResult"].ToString(), PlayTypeID);

                    if (!Result.Equals(ddx))
                    {
                        IsWin = false;
                    }
                    else
                    {
                        WinCoumt++;
                    }
                    i++;
                }

                string sql = "";
                int result = 0;

                // 计算命中场次  ==> WinCount

                if (!IsWin)
                {// 没有中奖
                    sql = "update T_ChallengeSaveScheme set IsOpened = 1, OpenOperateId = 1  where ID = " + SchemeID;

                    result = Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, sql);

                    if (result < 0)
                    {
                        new Log("SystemGetLotteryOpenNumber").Write("(保存)执行Sql语句失败：" + sql);

                        continue;
                    }


                    continue;
                }

                string Odds = dr["odds"].ToString();

                double WinMoney = 2;

                foreach (string Odd in Odds.Split('|'))
                {
                    WinMoney *= Shove._Convert.StrToDouble(Odd, 0);
                }

                // 修改方案的 IsOpen , WinMoney ,WinDescription
                sql = "update T_ChallengeSaveScheme set IsOpened = 1,WinMoney = " + WinMoney.ToString() + ",WinDescription='(保存)擂台中奖', OpenOperateId = 1 where ID = " + SchemeID;

                result = Shove.Database.MSSQL.ExecuteNonQuery(ConnectionString, sql);

                if (result < 0)
                {
                    new Log("SystemGetLotteryOpenNumber").Write("(保存)执行Sql语句失败：" + sql);

                    continue;
                }

                new Log("SystemGetLotteryOpenNumber").Write("(保存)开奖成功，SchemeID：" + SchemeID);
            }
        }

        #endregion
    }
}

