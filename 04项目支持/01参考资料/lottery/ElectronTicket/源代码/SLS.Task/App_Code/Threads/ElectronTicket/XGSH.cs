using System;
using System.Data;
using System.Configuration;

namespace SLS.Task
{
    /// <summary>
    /// XGSH 的摘要说明
    /// </summary>
    public class ElectronTicket_XGSH
    {
        private long gCount1 = 0;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("ElectronTicket_XGSH");
        private Log log = new Log("ElectronTicket_XGSH");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public ElectronTicket_XGSH(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public void Run()
        {
            //if ((ElectronTicket_HPCQ_Getway == "") || (ElectronTicket_HPCQ_UserName == "") || (ElectronTicket_HPCQ_UserPassword == ""))
            //{
            //    log.Write("ElectronTicket_XGSH Task 参数配置不完整.");

            //    return;
            //}

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

                log.Write("ElectronTicket_XGSH Task Start.");
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
                    msg.Send("ElectronTicket_XGSH Stop.");
                    log.Write("ElectronTicket_XGSH Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region 10 秒，发送电子票数据

                if (gCount1 >= 10)
                {
                    gCount1 = 0;

                    try
                    {
                        Send();
                    }
                    catch { }
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

        private void Send()
        {
        }

        #endregion
    }
}