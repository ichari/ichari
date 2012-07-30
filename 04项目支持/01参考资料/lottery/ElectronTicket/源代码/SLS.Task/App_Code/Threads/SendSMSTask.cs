using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace SLS.Task
{
    /// <summary>
    /// 发送手机短信任务
    /// </summary>
    public class SendSMSTask
    {
        private long gCount1 = 0;
        private long gCount2 = 0;

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

                gCount2++;

                #region 1 分钟, 检索是否有紧急票

                if (gCount2 >= 60 * 1)
                {
                    gCount2 = 0;

                    try
                    {
                        Shove._IO.IniFile ini = new Shove._IO.IniFile(System.AppDomain.CurrentDomain.BaseDirectory + "Config.ini");

                        bool IsSendSMSForSchemeCheck = Shove._Convert.StrToBool(ini.Read("Options", "IsSendSMSForSchemeCheck"), false);

                        if (IsSendSMSForSchemeCheck)
                        {
                            SchemeCheck();

                            msg.Send("SchemeCheck ...... OK.");
                        }
                    }
                    catch (Exception e)
                    {
                        msg.Send("SchemeCheck is Fail: " + e.Message);
                        log.Write("SchemeCheck is Fail: " + e.Message);
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
            DataTable dt = sms.Open(ConnectionString, "[ID], [To], [DateTime], [Content]", "IsSent = 0", "[DateTime]");

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

        private void SchemeCheck()
        {
            if ((DateTime.Now.Hour < 9) || (DateTime.Now.Hour > 21) || ((DateTime.Now.Hour > 21) && (DateTime.Now.Minute > 30)))
            {
                return;
            }

            DataTable dt = new DAL.Views.V_Schemes().Open(ConnectionString, "id", "DATEDIFF(minute, case when UpdateDatetime < StartTime then dateadd(minute, 2, StartTime) else UpdateDatetime end, getdate()) > 4 and Schedule >= 100 and Buyed = 0 and QuashStatus = 0 and State = 1", "");

            if (dt == null)
            {
                msg.Send("读取未出票方案错误(SchemeCheck)。");
                log.Write("读取未出票方案错误(SchemeCheck)。");

                return;
            }

            if (dt.Rows.Count > 0)
            {
                string[] strMobile = new string[1];

                strMobile[0] = "13537697101";

                for (int i = 0; i < strMobile.Length; i++)
                {
                    SMS.Eucp.Gateway.CallResult Result = segg.Send(strMobile[i], "爱彩乐出现紧急票，需要立即处理。");

                    if (Result.Code < 0)
                    {
                        log.Write("Send is Fail: " + Result.Description);
                    }
                }

                Shove._Net.Email.SendEmail("services@icaile.com", "thc@3km.cc", "爱彩乐出现紧急票，需要立即处理。", "爱彩乐出现紧急票，需要立即处理。", "mail.icaile.com", "services@icaile.com", "1314521");
            }
        }

        #endregion
    }
}
