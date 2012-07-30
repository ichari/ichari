using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Threading;

namespace GetSportteryService
{
    class Main
    {
        private string ConnectionString = "";
        private string Source = "";
        private System.Threading.Thread thread;
        private Log log = new Log("Sporttery");
        private long gCount1 = 0;
        private long gCount2 = 0;

        private int MatchSpaceTime = 0;
        private int RateSpaceTime = 0;
        public int State = 0;   // 0 停止 1 运行中 2 置为停止
        public bool isFirst = true;
        Clutch clu = new Clutch();

        public Main(string conn, int MatchSpaceTime, int RateSpaceTime, string Source, string PathName)
        {
            this.ConnectionString = conn;
            this.MatchSpaceTime = MatchSpaceTime;
            this.RateSpaceTime = RateSpaceTime;
            this.Source = Source;
            clu.Source = Source;

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


                log.Write("Main Start.");
            }
        }

        public void Exit()
        {
            State = 2;
        }

        public void Sleeping()
        {
            try
            {
                SqlConnection conn = Shove.Database.MSSQL.CreateDataConnection<System.Data.SqlClient.SqlConnection>(ConnectionString);
                while (conn.State != ConnectionState.Open)
                {
                    conn.Open();

                    System.Threading.Thread.Sleep(1000);
                }

                conn.Close();
            }
            catch (Exception e)
            {
                new Log("Sporttery").Write("数据库连接失败,原因描述：" + e.Message);
            }
        }

        public void Do()
        {
            if (isFirst)
            {
                Sleeping();
                isFirst = false;
            }

            while (true)
            {
                if (State == 2)
                {
                    log.Write("Main Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                if (gCount1 % (MatchSpaceTime * 21) == 0)
                {
                    gCount1 = 1;

                    try
                    {
                        clu.GetMatchResult();       //足彩比赛结果
                    }
                    catch
                    { }

                    try
                    {
                        clu.GetMatchBasketResult(); //篮彩比赛结果
                    }
                    catch
                    { }

                    try
                    {
                        clu.GetCompensationRate();
                    }
                    catch { }

                    try
                    {
                        clu.WriteToFile();
                    }
                    catch
                    { }
                }

                gCount2++;

                if (gCount2 % (1 * 20) == 0)
                {
                    gCount2 = 1;

                    try
                    {
                        clu.GetPassRate();          //足彩过关奖金
                    }
                    catch { }

                    try
                    {
                        clu.GetSingleRate();        //足彩单场奖金
                    }
                    catch { }

                    try
                    {
                        clu.GetPassRateBasket();    //篮彩过关奖金
                    }
                    catch { }

                    try
                    {
                        clu.GetSingleRateBasket();      //篮彩单场奖金
                    }
                    catch { }

                    try
                    {
                        clu.GetOkoooInformation();
                    }
                    catch { }
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
