using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Shove.Database;

namespace SLS.Score.Task
{
    /// <summary>
    /// Task 的摘要说明
    /// </summary>
    public class Task
    {
        private long gCount1 = 0;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("Task");
        private Log log = new Log("Task");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public Task(string connectionstring)
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

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                msg.Send("Task Start.");
                log.Write("Task Start.");
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
                    msg.Send("Task Stop.");
                    log.Write("Task Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region 半小时, 计算积分

                if (gCount1 >= 60 * 60)
                {
                    gCount1 = 0;

                    try
                    {
                        CalculateScore();

                        msg.Send("CalculateScore ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("CalculateScore is Fail: " + e.Message);
                        log.Write("CalculateScore is Fail: " + e.Message);
                    }

                    try
                    {
                        WinScoreScale();

                        msg.Send("WinScoreScale ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("WinScoreScale is Fail: " + e.Message);
                        log.Write("WinScoreScale is Fail: " + e.Message);
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

        private void CalculateScore()	//计算积分
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_CalculateScore(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec CalculateScore: Procedure \"P_CalculateScore\" Fail.");
                log.Write("Exec CalculateScore: Procedure \"P_CalculateScore\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec CalculateScore: Procedure \"P_CalculateScore\" Return: " + ReturnDescription);
                log.Write("Exec CalculateScore: Procedure \"P_CalculateScore\" Return: " + ReturnDescription);
            }
        }

        private void WinScoreScale()
        {
            DataTable dt = MSSQL.Select(ConnectionString, "select id, PlayTypeID, WinMoney, LotteryNumber, IsuseID, Multiple from T_Schemes where not exists (select 1 from T_SchemeIsCalcuteScore where ScoreType = 2 and T_Schemes.ID = T_SchemeIsCalcuteScore.SchemeID) and ID > isnull((select max(SchemeID) - 10000 from T_SchemeIsCalcuteScore where ScoreType = 2), 0) and exists (select 1 from T_Isuses where LotteryID in (6, 28, 29, 60, 61, 62, 63, 64, 68, 70) and T_Isuses.ID = T_Schemes.IsuseID) and isOpened = 1 order by IsuseID desc");

            if (dt == null)
            {
                log.Write("Exec WinScoreScale: DataTable \"WinScoreScale\" Fail.");

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            DAL.Tables.T_SchemeIsCalcuteScore t_SchemeIsCalcuteScore = new DAL.Tables.T_SchemeIsCalcuteScore();

            DataTable dtIsuses = null;
            DataTable dtWinMoney = null;

            foreach (DataRow dr in dt.Rows)
            {
                if (Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0) == 0)
                {
                    t_SchemeIsCalcuteScore.SchemeID.Value = dr["ID"].ToString();
                    t_SchemeIsCalcuteScore.ScoreType.Value = 2;
                    t_SchemeIsCalcuteScore.Insert(ConnectionString);

                    continue;
                }

                dtIsuses = new DAL.Tables.T_Isuses().Open(ConnectionString, "WinLotteryNumber, LotteryID", "ID=" + dr["IsuseID"].ToString(), "");

                if (dtIsuses == null)
                {
                    log.Write("Exec WinScoreScale: DataTable \"T_Isuses\" Fail.期号为:" + dr["IsuseID"].ToString());

                    continue;
                }

                if (dtIsuses.Rows.Count < 1)
                {
                    continue;
                }

                dtWinMoney = MSSQL.Select(ConnectionString, "select DefaultMoney * a.ScoreScale as DefaultMoney, DefaultMoneyNoWithTax * a.ScoreScale as DefaultMoneyNoWithTax, a.WinMoney as WinMoney from T_WinTypes inner join T_WinScoreScale a on T_WinTypes.ID = a.WinTypeID where exists (select * from T_PlayTypes where ID = " + dr["PlayTypeID"].ToString() + " and T_WinTypes.LotteryID = T_PlayTypes.LotteryID)  order by [Order]");

                if (dtWinMoney == null)
                {
                    log.Write("Exec WinScoreScale: DataTable \"dtWinMoney\" Fail.期号为:" + dr["IsuseID"].ToString());

                    continue;
                }

                if (dtWinMoney.Rows.Count < 1)
                {
                    continue;
                }

                double[] WinMoneyList = new double[dtWinMoney.Rows.Count * 2];

                double MaxMoney = 0;

                for (int i = 0; i < dtWinMoney.Rows.Count; i++)
                {
                    WinMoneyList[i * 2] = Shove._Convert.StrToDouble(dtWinMoney.Rows[i]["DefaultMoney"].ToString(), 0);
                    WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(dtWinMoney.Rows[i]["DefaultMoneyNoWithTax"].ToString(), 0);

                    if (Shove._Convert.StrToDouble(dtWinMoney.Rows[i]["WinMoney"].ToString(), 0) > MaxMoney)
                    {
                        MaxMoney = Shove._Convert.StrToDouble(dtWinMoney.Rows[i]["WinMoney"].ToString(), 0);
                    }
                }

                string LotteryNumber = dr["LotteryNumber"].ToString();
                string Description = "";

                double WinMoneyNoWithTax = 0;
                double WinMoney = 0;

                try
                {
                    WinMoney = new SLS.Lottery()[int.Parse(dtIsuses.Rows[0]["LotteryID"].ToString())].ComputeWin(LotteryNumber, dtIsuses.Rows[0]["WinLotteryNumber"].ToString(), ref Description, ref WinMoneyNoWithTax, int.Parse(dr["PlayTypeID"].ToString()), WinMoneyList);
                }
                catch
                {
                    WinMoney = 0;
                }

                log.Write("方案号:" + dr["ID"].ToString() + "-------中奖金额:" + (Shove._Convert.StrToDouble(dr["Multiple"].ToString(), 0) * WinMoney).ToString() + "-----------最大金额:" + MaxMoney.ToString());

                if (DAL.Procedures.P_SchemeWinCalculatedScore(ConnectionString, Shove._Convert.StrToLong(dr["ID"].ToString(), 0), Shove._Convert.StrToDouble(dr["Multiple"].ToString(), 0) * WinMoney, MaxMoney) < 0)
                {
                    log.Write("Exec WinScoreScale: Procedures \"P_SchemeWinCalculatedScore\" Fail.");

                    continue;
                }
            }

        #endregion
        }
    }
}