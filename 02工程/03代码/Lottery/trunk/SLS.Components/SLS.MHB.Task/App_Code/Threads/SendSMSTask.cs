using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SLS.MHB.Task
{
    /// <summary>
    /// 发送手机短信任务
    /// </summary>
    public class SendSMSTask
    {
        private long gCount1 = 0;

        private string Betting_SMS_UserID = "";
        private string Betting_SMS_UserPassword = "";
        private string Betting_SMS_RegCode = "";

        private SMS.Eucp.Gateway.Gateway segg = null;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("SendSMSTask");
        private Log log = new Log("SendSMSTask");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public SendSMSTask(string connectionstring)
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

                msg.Send("SendSMSTask: SMS config error.");
                log.Write("SendSMSTask: SMS config error.");

                return;
            }

            segg = new SMS.Eucp.Gateway.Gateway(Betting_SMS_UserID, Betting_SMS_UserPassword);

            if (segg == null)
            {
                State = 0;

                msg.Send("SendSMSTask: SMS Gateway open error.");
                log.Write("SendSMSTask: SMS Gateway open error.");

                return;
            }

            //if (Betting_SMS_RegCode != "")
            //{
            //    segg.SetKey(Betting_SMS_RegCode);
            //}

            lock (this) // 确保临界区被一个 Thread 所占用
            {
                State = 1;

                gCount1 = 0;

                thread = new System.Threading.Thread(new System.Threading.ThreadStart(Do));
                thread.IsBackground = true;

                thread.Start();

                msg.Send("SendSMSTask Start.");
                log.Write("SendSMSTask Start.");
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
                    msg.Send("SendSMSTask Stop.");
                    log.Write("SendSMSTask Stop.");

                    State = 0;

                    Stop();

                    return;
                }

                System.Threading.Thread.Sleep(1000);   // 1秒为单位

                gCount1++;

                #region 5 秒, 遍历待发短信表，发送短信

                if (gCount1 >= 5)
                {
                    gCount1 = 0;

                    int SendCount = 0;

                    try
                    {
                        SendCount = Send();

                        msg.Send("Send (" + SendCount.ToString() + ")...... OK.");
                    }
                    catch (Exception e)
                    {
                        msg.Send("Send is Fail: " + e.Message);
                        log.Write("Send is Fail: " + e.Message);
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

        private int Send()
        {
            DAL.Tables.T_SMS sms = new DAL.Tables.T_SMS();
            DataTable dt = sms.Open(ConnectionString, " top 100 [ID], [To], [DateTime], [Content]", "IsSent = 0", "[DateTime]");

            if (dt == null)
            {
                log.Write("Send is Fail: Data read fail.");

                return -1;
            }

            if (dt.Rows.Count == 0)
            {
                return 0;
            }

            int Count = 0;

            segg = new SMS.Eucp.Gateway.Gateway(Betting_SMS_UserID, Betting_SMS_UserPassword);

            if (segg == null)
            {
                State = 0;

                msg.Send("SendSMSTask: SMS Gateway open error.");
                log.Write("SendSMSTask: SMS Gateway open error.");

                return -1;
            }

            foreach (DataRow dr in dt.Rows)
            {
                SMS.Eucp.Gateway.CallResult Result = segg.Send(dr["To"].ToString(), dr["Content"].ToString());

                if (Result.Code < 0)
                {
                    log.Write("Send is Fail: " + Result.Description);
                }
                else
                {
                    Count++;
                }

                sms.IsSent.Value = true;
                sms.Update(ConnectionString, "[ID] = " + dr["ID"].ToString());

                System.Threading.Thread.Sleep(500);
            }

            return Count;
        }

        #endregion
    }
}
