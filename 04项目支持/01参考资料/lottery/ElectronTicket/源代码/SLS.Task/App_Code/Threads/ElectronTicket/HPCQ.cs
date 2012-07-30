using System;
using System.Data;
using System.Configuration;
using System.Net;
using System.IO;

using Shove.Database;

namespace SLS.Task
{
    /// <summary>
    /// HPCQ 的摘要说明
    /// </summary>
    public class ElectronTicket_HPCQ
    {
        private const int TimeoutSeconds = 100;
        private long gCount1 = 0;

        private System.Threading.Thread thread;
        private string ConnectionString;

        private Message msg = new Message("ElectronTicket_HPCQ");
        private Log log = new Log("ElectronTicket_HPCQ");

        public int State = 0;   // 0 停止 1 运行中 2 置为停止

        public string ElectronTicket_HPCQ_UserName;
        public string ElectronTicket_HPCQ_UserPassword;
        public string ElectronTicket_HPCQ_Getway;

        public ElectronTicket_HPCQ(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public void Run()
        {
            SystemOptions so = new SystemOptions(ConnectionString);

            if (!so["ElectronTicket_HPCQ_Status_ON"].ToBoolean(false))
            {
                return;
            }

            if ((ElectronTicket_HPCQ_Getway == "") || (ElectronTicket_HPCQ_UserName == "") || (ElectronTicket_HPCQ_UserPassword == ""))
            {
                log.Write("ElectronTicket_HPCQ Task 参数配置不完整.");

                return;
            }

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

                log.Write("ElectronTicket_HPCQ Task Start.");
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
                    msg.Send("ElectronTicket_HPCQ Task Stop.");
                    log.Write("ElectronTicket_HPCQ Task Stop.");

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
                        WriteTickets(); // 满员方案拆分为票

                        Send();         // 循环发送出票请求
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

        //把方案拆分成票，并写入数据库
        private void WriteTickets()
        {
            DataTable dt = new DAL.Views.V_SchemeSchedules().Open(ConnectionString, "ID, LotteryID, PlayTypeID, LotteryNumber, Multiple, Money", "Buyed = 0 and PrintOutType = 101 and (GetDate() between StartTime and EndTime) and Schedule >= 100 and not [ID] in (select SchemeID from T_SchemesSendToCenter)", "[ID]");

            if (dt == null)
            {
                log.Write("向恒朋-重庆电子票网关发送数据出错：读取方案错误。");

                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                long SchemeID = Shove._Convert.StrToLong(dr["ID"].ToString(), -1);
                int LotteryID = Shove._Convert.StrToInt(dr["LotteryID"].ToString(), -1);
                string LotterNumber = dr["LotteryNumber"].ToString();
                int PlayTypeID = Shove._Convert.StrToInt(dr["PlayTypeID"].ToString(), -1);
                int Multiple = Shove._Convert.StrToInt(dr["Multiple"].ToString(), -1);

                if ((SchemeID < 0) || (LotteryID < 0) || (PlayTypeID < 0) || (Multiple < 1))
                {
                    log.Write("向恒朋-重庆电子票网关发送数据出错：读取方案错误。方案号：" + SchemeID.ToString());

                    continue;
                }

                double Money = 0;
                SLS.Lottery.Ticket[] Tickets = new SLS.Lottery()[LotteryID].ToElectronicTicket_HPCQ(PlayTypeID, LotterNumber, Multiple, 200, ref Money);

                if (Tickets == null)
                {
                    log.Write("向恒朋-重庆电子票网关发送数据出错：读取方案错误。方案号：" + SchemeID.ToString());

                    continue;
                }

                if (Money != Shove._Convert.StrToDouble(dr["Money"].ToString(), -1))
                {
                    log.Write("向恒朋-重庆电子票网关发送数据出错：异常警告！！！！。方案号：" + SchemeID.ToString() + "的购买金额与实际票的金额不符合！！！！");

                    continue;
                }

                int Count = 0;

                foreach (SLS.Lottery.Ticket ticket in Tickets)
                {
                    int TicketID = -1;
                    //long ID = -1;
                    string ReturnDescription = "";

                    Count++;

                    if (TicketID < 0)
                    {
                        log.Write("向恒朋-重庆电子票网关发送数据出错：部分票写入错误：" + ReturnDescription + "，方案号：" + SchemeID.ToString());

                        continue;
                    }
                }
            }
        }

        //向系统发送票的XML格式  发送类型103(投注请求)
        private void Send()
        {
            DataTable dt = new DAL.Views.V_SchemesSendToCenter().Open(ConnectionString, "", "Buyed = 0 and PrintOutType = 101 and (GetDate() between StartTime and EndTime) and ((Sends = 0) or (Sends < 3 and HandleResult = 0 and datediff(minute, [Datetime], GetDate()) > 3))", "[ID]");

            if (dt == null)
            {
                log.Write("向恒朋-重庆电子票网关发送数据出错：读取方案错误。");

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            int SendCount = 0;

            if ((dt.Rows.Count % 500) != 0)
            {
                SendCount = (dt.Rows.Count - (dt.Rows.Count % 500)) / 500 + 1;
            }
            else
            {
                SendCount = dt.Rows.Count / 500;
            }

            for (int i = 0; i < SendCount; i++)
            {
                DateTime Now = DateTime.Now;
                string SendedIDList = "";

                string Body = "<body><lotteryRequest>";

                for (int j = i * 500; j < (i * 500) + 500; j++)
                {
                    if (j >= dt.Rows.Count)
                    {
                        break;
                    }

                    DataRow dr = dt.Rows[j];

                    Body += "<ticket id=\"" + ElectronTicket_HPCQ_UserName + Now.ToString("yyyyMMdd") + dr["TicketID"].ToString().PadLeft(8, '0') + "\"";
                    Body += " playType=\"" + dr["PlayTypeID"].ToString() + "\" money=\"" + double.Parse(dr["Money"].ToString()).ToString("N") + "\" amount=\"" + dr["Multiple"].ToString() + "\">";
                    Body += "<issue number=\"" + dr["IsuseName"].ToString() + "\" gameName=\"" + GetGameName(int.Parse(dr["LotteryID"].ToString())) + "\"/>";
                    Body += "<userProfile cardType=\"1\" mail=\"" + dr["Email"].ToString() + "\" cardNumber=\"" + dr["IDCardNumber"].ToString() + "\" mobile=\"" + dr["Mobile"].ToString() + "\" realName=\"" + dr["RealityName"].ToString() + "\" bonusPhone=\"" + dr["Telephone"].ToString() + "\"/>";

                    string LotteryNumber = dr["Ticket"].ToString();
                    string[] strs = LotteryNumber.Split('\n');

                    foreach (string str in strs)
                    {
                        if (str.Trim() == "")
                        {
                            continue;
                        }

                        Body += "<anteCode>" + str + "</anteCode>";
                    }

                    Body += "</ticket>";

                    SendedIDList += ((SendedIDList != "") ? "," : "") + dr["ID"].ToString();
                }

                Body += "</lotteryRequest></body>";

                string MessageID = ElectronTicket_HPCQ_UserName + Now.ToString("yyyyMMdd") + Now.ToString("HHmmss") + i.ToString().PadLeft(2, '0');
                string TimeStamp = Now.ToString("yyyyMMdd") + Now.ToString("HHmmss");

                string Message = "<?xml version=\"1.0\" encoding=\"GBK\"?>";
                Message += "<message version=\"1.0\" id=\"" + MessageID + "\">";
                Message += "<header>";
                Message += "<messengerID>" + ElectronTicket_HPCQ_UserName + "</messengerID>";
                Message += "<timestamp>" + TimeStamp + "</timestamp>";
                Message += "<transactionType>103</transactionType>";
                Message += "<digest>" + Shove._Security.Encrypt.MD5(MessageID + TimeStamp + ElectronTicket_HPCQ_UserPassword + Body, "gb2312") + "</digest>";
                Message += "</header>";
                Message += Body;
                Message += "</message>";

                PublicFunction.Post(ElectronTicket_HPCQ_Getway, "transType=103&transMessage=" + Message, TimeoutSeconds);

                MSSQL.ExecuteNonQuery(ConnectionString, "update T_SchemesSendToCenter set Sends = Sends + 1, [DateTime] = GetDate() where [ID] in (" + SendedIDList + ")");
            }
        }

        #endregion

        private string GetGameName(int LotteryID)
        {
            switch (LotteryID)
            {
                case 5:
                    return "ssq";
                case 28:
                    return "ssc";
                default:
                    return "";
            }
        }
    }
}