using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

using Shove.Database;
using System.Text;

namespace SLS.Resource.Task
{
    /// <summary>
    /// Task 的摘要说明
    /// </summary>
    public class Task
    {

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Log log = new Log("Task");
        private long gCount1 = 0;

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
                    log.Write("Task Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 2秒为单位
                
                gCount1++;

                if (gCount1 >= 30)
                {
                    gCount1 = 0;
                    try
                    {
                        BonusNumber.GetLastWinNumber_CQSSC(ConnectionString, "28");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_CQSSC is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.GetLastWinNumber_JXSSC(ConnectionString, "61");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_JXSSC is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.GetLastWinNumber_SYYDJ(ConnectionString, "62");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_SYYDJ is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.GetLastWinNumber_11X5(ConnectionString, "70");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_11X5 is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.GetLastWinNumber_KY481(ConnectionString, "68");

                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_KY481 is Fail: " + e.Message);
                    }


                    try
                    {
                        BonusNumber.GetLastWinNumber_HN11X5(ConnectionString, "77");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_HN11X5 is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.GetLastWinNumber_GD11X5(ConnectionString, "78");
                    }
                    catch (Exception e)
                    {
                        log.Write("GetLastWinNumber_GD11X5 is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakeSFCFile(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakeSFCFile is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakeRXJCFile(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakeRXJCFile is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakeQXCFile(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakeQXCFile is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakeLCBQCFile(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakeLCBQCFile is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakeJQCFile(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakeJQCFile is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakePL3File(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakePL3File is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.MakePL5File(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("MakePL5File is Fail: " + e.Message);
                    }

                    try
                    {
                        BonusNumber.Make22X5File(ConnectionString);
                    }
                    catch (Exception e)
                    {
                        log.Write("Make22X5File is Fail: " + e.Message);
                    }
                }
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

    }
}