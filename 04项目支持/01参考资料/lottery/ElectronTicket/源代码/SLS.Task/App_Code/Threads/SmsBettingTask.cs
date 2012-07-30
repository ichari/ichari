using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Shove.Database;

namespace SLS.Task
{
    /// <summary>
    ///SmsBetting 的摘要说明
    /// </summary>
    public class SmsBettingTask
    {
        private long gCount1 = 0;

        private string Betting_SMS_UserID = "";
        private string Betting_SMS_UserPassword = "";
        private string Betting_SMS_RegCode = "";

        private SMS.Eucp.Gateway.Gateway segg = null;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("SmsBettingTask");
        private Log log = new Log("SmsBettingTask");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public SmsBettingTask(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public void Run()
        {
            // 已经启动
            if (State == 1)
            {
                return;
            }

            SystemOptions so = new SystemOptions(ConnectionString);
            Betting_SMS_UserID = so["Betting_SMS_UserID"].Value.ToString();
            Betting_SMS_UserPassword = so["Betting_SMS_UserPassword"].Value.ToString();
            Betting_SMS_RegCode = so["Betting_SMS_RegCode"].Value.ToString();

            if ((Betting_SMS_UserID == "") || (Betting_SMS_UserPassword == ""))
            {
                State = 0;

                msg.Send("SmsBetting: SMS config error.");
                log.Write("SmsBetting: SMS config error.");

                return;
            }

            segg = new SMS.Eucp.Gateway.Gateway(Betting_SMS_UserID, Betting_SMS_UserPassword);

            if (segg == null)
            {
                State = 0;

                msg.Send("SmsBetting: SMS Gateway open error.");
                log.Write("SmsBetting: SMS Gateway open error.");

                return;
            }

            if (Betting_SMS_RegCode != "")
            {
                segg.SetKey(Betting_SMS_RegCode);
            }

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                msg.Send("SmsBettingTask Start.");
                log.Write("SmsBettingTask Start.");
            }
        }

        public void Exit()
        {
            State = 2;
        }

        public void Do()
        {
            while (true)
            {
                if (State == 2)
                {
                    msg.Send("SmsBettingTask Stop.");
                    log.Write("SmsBettingTask Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region 30 秒, 接收短信，解析投注

                if (gCount1 >= 30)
                {
                    gCount1 = 0;

                    try
                    {
                        Receive();

                        msg.Send("Receive ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("Receive is Fail: " + e.Message);
                        log.Write("Receive is Fail: " + e.Message);
                    }

                    try
                    {
                        Betting();

                        msg.Send("Betting ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("Betting is Fail: " + e.Message);
                        log.Write("Betting is Fail: " + e.Message);
                    }
                }

                #endregion
            }
        }

        private void Stop()
        {
            if (thread != null)
            {
                thread.Abort();
                thread = null;
            }
        }

        #region 定时器执行的事件

        private void Receive()  // 接收短信
        {
            if ((Betting_SMS_UserID == "") || (Betting_SMS_UserPassword == "") || (segg == null))
            {
                return;
            }

            SMS.Eucp.Gateway.CallResult Result = segg.ReceiveSMS();

            if (Result.Code < 0)
            {
                msg.Send("Receive SMS fail." + Result.Description);
                log.Write("Receive SMS fail." + Result.Description);

                return;
            }

            if (segg.rsc.Count < 1)
            {
                return;
            }

            // 写入数据库
            DAL.Tables.T_SmsBettings t_SmsBettings = new DAL.Tables.T_SmsBettings();

            for (int i = 0; i < segg.rsc.Count; i++)
            {
                t_SmsBettings.SMSID.Value = 0;
                t_SmsBettings.From.Value = segg.rsc[i].FromMobile;
                t_SmsBettings.Content.Value = segg.rsc[i].Content;
                t_SmsBettings.HandleResult.Value = 0;

                if (t_SmsBettings.Insert(ConnectionString) < 0)
                {
                    msg.Send("Write SMS fail.");
                    log.Write("Write SMS fail.");

                    continue;
                }
            }
        }

        private void Betting()  // 解析，投注
        {
            DAL.Tables.T_SmsBettings t_SmsBettings = new DAL.Tables.T_SmsBettings();
            DataTable dt = t_SmsBettings.Open(ConnectionString, "", "HandleResult = 0", "[ID]");

            if (dt == null)
            {
                msg.Send("Read SMS fail.");
                log.Write("Read SMS fail.");

                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                string ID = dr["ID"].ToString();
                string Mobile = dr["From"].ToString().Trim();
                string Content = Shove._Convert.ToDBC(dr["Content"].ToString()).Trim();

                if ((Mobile == "") || (Content == ""))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1000)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                string[] Contents = Content.Split(';');

                if ((Contents == null) || (Contents.Length != 6))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1001)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 1;501;1;1;010203040506+01|010203040506+01;#2                          站点ID;玩法ID;总份数;认购份数;号码|号码;#倍数

                long SiteID = Shove._Convert.StrToLong(Contents[0], -1);

                if (SiteID < 0)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1002)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                if (new DAL.Tables.T_Sites().GetCount(ConnectionString, "[ID] = " + SiteID.ToString()) < 0)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1003)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                #region 投注

                if (!Contents[5].StartsWith("#"))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1004)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                int PlayTypeID = Shove._Convert.StrToInt(Contents[1], -1);
                int Share = Shove._Convert.StrToInt(Contents[2], -1);
                int BuyShare = Shove._Convert.StrToInt(Contents[3], -1);
                string LotteryNumber = Contents[4].Trim();
                int Multiple = Shove._Convert.StrToInt(Contents[5].Substring(1, Contents[5].Length - 1), -1);

                if ((PlayTypeID < 0) || (BuyShare < 0) || (Share < BuyShare) || (LotteryNumber == "") || (Multiple < 0))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1005)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 根据手机获取用户
                DataTable dtUsers = new DAL.Tables.T_Users().Open(ConnectionString, "[ID], Balance", "SiteID = " + SiteID.ToString() + " and Mobile = '" + Mobile + "' and isMobileValided = 1", "");

                if ((dtUsers == null) || (dtUsers.Rows.Count != 1))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "用户不存在(1006)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                long UserID = Shove._Convert.StrToLong(dtUsers.Rows[0]["ID"].ToString(), -1);
                double Balance = Shove._Convert.StrToDouble(dtUsers.Rows[0]["Balance"].ToString(), 0);

                if (UserID < 0)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "用户不存在(1007)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 获取彩种、同时也校验了玩法
                DataTable dtLottery = new DAL.Tables.T_PlayTypes().Open(ConnectionString, "LotteryID, Price, MaxMultiple", "[ID] = " + PlayTypeID.ToString(), "");

                if ((dtLottery == null) || (dtLottery.Rows.Count < 1))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(1008)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                int LotteryID = Shove._Convert.StrToInt(dtLottery.Rows[0]["LotteryID"].ToString(), -1);
                double Price = Shove._Convert.StrToDouble(dtLottery.Rows[0]["Price"].ToString(), -1);
                int MaxMultiple = Shove._Convert.StrToInt(dtLottery.Rows[0]["MaxMultiple"].ToString(), -1);

                if ((LotteryID < 0) || (Price < 2) || (Multiple > MaxMultiple))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "短信格式错误(10090)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 校验投注号码，计算注数、金额
                int Num = 0;
                LotteryNumber = GetLotteryNumber(LotteryID, PlayTypeID, LotteryNumber, ref Num);

                if (Num < 1)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "投注号码错误(1010)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                double Money = Num * Price * Multiple;

                // 获取期号
                DataTable dtIsuse = new DAL.Tables.T_Isuses().Open(ConnectionString, "top 1 [ID]", "LotteryID = " + LotteryID.ToString() + " and GetDate() between StartTime and dbo.F_GetIsuseSystemEndTime([ID], " + PlayTypeID.ToString() + ") and IsOpened = 0", "");

                if ((dtIsuse == null) || (dtIsuse.Rows.Count < 1))
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "奖期未开启(1011)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                long IsuseID = Shove._Convert.StrToLong(dtIsuse.Rows[0]["ID"].ToString(), -1);

                if (IsuseID < 0)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "奖期未开启(1012)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 校验余额
                if (Balance < Money)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = "投注卡账户余额不足(1013)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                // 投注
                string ReturnDescription = "";
                long SchemeID = InitiateScheme(SiteID, UserID, IsuseID, PlayTypeID, "本方案由用户手动编写短信代码投注（系统）", "短信ID:" + ID + "\r\n短信内容:" + Content, LotteryNumber, "", Multiple, Money, 0, Share, BuyShare, "", 0, ref ReturnDescription);

                if (SchemeID < 0)
                {
                    t_SmsBettings.HandleResult.Value = -1;
                    t_SmsBettings.HandleDescription.Value = ReturnDescription + "(1015)";
                    t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                    continue;
                }

                t_SmsBettings.SchemeID.Value = SchemeID;
                t_SmsBettings.HandleResult.Value = 1;
                t_SmsBettings.Update(ConnectionString, "[ID] = " + ID);

                #endregion
            }
        }

        #endregion

        private string GetLotteryNumber(int LotteryID, int PlayTypeID, string BettingNumber, ref int Num)   // Num 返回注数
        {
            Num = 0;

            if (String.IsNullOrEmpty(BettingNumber))
            {
                return "";
            }

            BettingNumber = BettingNumber.Replace("|", "\n");
            BettingNumber = ConvertLotteryNumber(LotteryID, BettingNumber);

            SLS.Lottery.LotteryBase lb = new SLS.Lottery()[LotteryID];

            if (lb == null)
            {
                return "";
            }

            string t_Number = lb.AnalyseScheme(BettingNumber, PlayTypeID);

            if (String.IsNullOrEmpty(t_Number))
            {
                return "";
            }

            string[] t_Numbers = t_Number.Split('\n');

            if ((t_Numbers == null) || (t_Numbers.Length < 1))
            {
                return "";
            }

            string Result = "";

            foreach (string str in t_Numbers)
            {
                string t_str = str.Trim();

                if (String.IsNullOrEmpty(t_str))
                {
                    continue;
                }

                string[] t_strs = t_str.Split('|');

                if ((t_strs == null) || (t_strs.Length != 2))
                {
                    continue;
                }

                int t_Num = Shove._Convert.StrToInt(t_strs[1], -1);

                if (String.IsNullOrEmpty(t_strs[0]) || (t_Num < 1))
                {
                    continue;
                }

                Result += t_strs[0] + "\n";
                Num += t_Num;
            }

            if (Result.EndsWith("\n"))
            {
                Result = Result.Substring(0, Result.Length - 1);
            }

            return Result;
        }

        private string ConvertLotteryNumber(int LotteryID, string BettingNumber)
        {
            if (String.IsNullOrEmpty(BettingNumber))
            {
                return "";
            }

            if ((LotteryID != SLS.Lottery.SSQ.ID) && (LotteryID != SLS.Lottery.HD15X5.ID) && (LotteryID != SLS.Lottery.QLC.ID))
            {
                return BettingNumber;
            }

            string[] Numbers = BettingNumber.Replace("\r", "").Split('\n');

            if ((Numbers == null) || (Numbers.Length < 1))
            {
                return "";
            }

            string Result = "";

            foreach (string str in Numbers)
            {
                try
                {
                    if (LotteryID == SLS.Lottery.SSQ.ID)
                    {
                        string[] Parts = str.Split('+');

                        if ((Parts == null) || (Parts.Length != 2))
                        {
                            continue;
                        }

                        string t_str = "";

                        for (int i = 0; i < Parts[0].Length; i += 2)
                        {
                            t_str += Parts[0].Substring(i, 2) + " ";
                        }

                        t_str += "+ ";

                        for (int i = 0; i < Parts[1].Length; i += 2)
                        {
                            t_str += Parts[1].Substring(i, 2) + " ";
                        }

                        t_str = t_str.Trim();
                        Result += t_str + "\n";
                    }
                    else if ((LotteryID == SLS.Lottery.HD15X5.ID) || (LotteryID == SLS.Lottery.QLC.ID))
                    {
                        string t_str = "";

                        for (int i = 0; i < str.Length; i += 2)
                        {
                            t_str += str.Substring(i, 2) + " ";
                        }

                        t_str = t_str.Trim();
                        Result += t_str + "\n";
                    }
                }
                catch
                {

                }
            }

            if (Result.EndsWith("\n"))
            {
                Result = Result.Substring(0, Result.Length - 1);
            }

            return Result;
        }

        private long InitiateScheme(long SiteID, long UserID, long IsuseID, int PlayTypeID, string Title, string Description, string LotteryNumber, string UpdateloadFileContent, int Multiple,
            double Money, double AssureMoney, int Share, int BuyShare, string OpenUsers, short SecrecyLevel, ref string ReturnDescription)
        {
            if ((SiteID < 0) || (UserID < 0))
            {
                throw new Exception("手机短信投注的 InitiateScheme 方法需要提供正确的 SiteID、UserID 参数");
            }

            ReturnDescription = "";

            if ((SecrecyLevel < 0) || (SecrecyLevel > 3))
            {
                SecrecyLevel = 0;
            }

            long SchemeID = -1;

            int Result = DAL.Procedures.P_InitiateScheme(ConnectionString, SiteID, UserID, IsuseID, PlayTypeID, Title, Description, LotteryNumber, UpdateloadFileContent,
                Multiple, Money, AssureMoney, Share, BuyShare, OpenUsers.Replace('，', ','), SecrecyLevel, 0.04,ref SchemeID, ref ReturnDescription);

            if (Result < 0)
            {
                ReturnDescription = "数据库读写错误";

                return -1;
            }

            if (SchemeID < 0)
            {
                return SchemeID;
            }

            return SchemeID;
        }
    }
}