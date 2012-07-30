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
    public class CpsTask
    {
        private long gCount1 = 0;
        private long gCount2 = 0;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("Task");
        private Log log = new Log("Task");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public CpsTask(string connectionstring)
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
                gCount2 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                msg.Send("CpsTask Start.");
                log.Write("CpsTask Start.");
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
                    msg.Send("CpsTask Stop.");
                    log.Write("CpsTask Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;
                gCount2++;

                #region 20 分钟, 计算佣金

                if (gCount1 >= 60 * 20)
                {
                    gCount1 = 0;

                    try
                    {
                        CpsCalculateBonus();

                        msg.Send("SchemeSystemDeal ...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("SchemeSystemDeal is Fail: " + e.Message);
                        log.Write("SchemeSystemDeal is Fail: " + e.Message);
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

        private void CpsCalculateBonus()	//计算累计佣金
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            if (DAL.Procedures.P_CpsCalculateBonus(ConnectionString, ref ReturnValue, ref ReturnDescription) < 0)
            {
                msg.Send("Exec CpsCalculateBonus: Procedure \"P_CpsCalculateBonus\" Fail.");
                log.Write("Exec CpsCalculateBonus: Procedure \"P_CpsCalculateBonus\" Fail.");

                return;
            }

            if (ReturnValue < 0)
            {
                msg.Send("Exec CpsCalculateBonus: Procedure \"P_CpsCalculateBonus\" Return: " + ReturnDescription);
                log.Write("Exec CpsCalculateBonus: Procedure \"P_CpsCalculateBonus\" Return: " + ReturnDescription);
            }
        }

        #endregion
    }
}