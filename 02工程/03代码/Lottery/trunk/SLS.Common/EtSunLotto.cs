using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.IO;
using System.Text;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Net;
using SLS.Common;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Collections;

namespace SLS.Common
{
    /// <summary>
    /// Summary description for eTicket
    /// </summary>
    public class EtSunLotto
    {
        #region Lottery Type
        public class SSQ
        {
            public const string ID = "001";
            public class SubCategory
            {
                public const string PuTong = "00";
            }
            public class PlayType
            {
                public const string DanShi = "01";
                public const string FuShi = "02";
            }
        }
        public class QLC
        {
            public const string ID = "003";
            public class SubCategory
            {
                public const string PuTong = "00";
            }
            public class PlayType
            {
                public const string DanShi = "01";
                public const string FuShi = "02";
                public const string DanTuo = "03";
            }
        }
        public class FC3D
        {
            public const string ID = "002";
            public class SubCategory
            {
                public const string ZhiXuan = "01";
                public const string ZuXuan3 = "02";
                public const string ZuXuan6 = "03";
            }
            public class PlayType
            {
                public const string DanShi = "01";
                public const string FuShi = "02";
                public const string DanTuo = "03";
                public const string HeZhi = "04";
            }
        }
        public class CQSSC
        {
            public const string ID = "018";
            public class SubCategory
            {
                public const string YiXing = "01";
                public const string ErXing = "02";
                public const string ErXingZuXuan = "06";
                public const string SanXing = "03";
                public const string SanXingZuSan = "07";
                public const string SanXingZuLiu = "08";
                public const string WuXing = "05";
                public const string WuXingTongXuan = "13";
                public const string DaXiaoDanShuang = "23";
            }
            public class PlayType
            {
                public const string DanShi = "01";
                public const string FuShi = "02";
                public const string DanTuo = "03";
                public const string HeZhi = "04";
                public const string FuXuan = "08";
            }
        }
        #endregion
        private SLS.Common.Log lg = null;
        /// <summary>
        /// eTicket class for provider : Sun Lotto
        /// </summary>
        public class slTicket
        {
            public string SubCategory;
            public string PlayTypeID;
            public string DrawNumber;
            public int Multiple;
            public double Money;

            public slTicket(string subcategory, string playtype_id, string drawnumber, int multiple, double money)
            {
                SubCategory = subcategory;
                PlayTypeID = playtype_id;
                Multiple = multiple;
                DrawNumber = drawnumber;
                Money = money;
            }

            public override string ToString()
            {
                return SubCategory + "-" + PlayTypeID + "-" + DrawNumber + "-" + Multiple + "-" + Money.ToString("0.00");
            }
        }

        public EtSunLotto() { lg = new SLS.Common.Log("eTicket"); }

        #region Order XML
        /// <summary>
        /// prepares the body xml for the selected scheme
        /// </summary>
        /// <param name="IDCardNumber">ID Card of the purchaser</param>
        /// <param name="Mobile">cell phone number</param>
        /// <param name="RealityName">real name as appears on ID card</param>
        /// <param name="lotteryId">system lottery id</param>
        /// <param name="issue">system issue name</param>
        /// <param name="schemeNumber">system scheme number</param>
        /// <param name="playTypeId">system play type id</param>
        /// <param name="lotteryNumber">system lottery number</param>
        /// <param name="multiplier">multiplier of the scheme</param>
        /// <param name="total">total amount to be calculated</param>
        /// <returns>the complete body xml with tickets</returns>
        public string PrepareTickets(string IDCardNumber, string Mobile, string RealityName, int lotteryId, string issue, string schemeNumber, int playTypeId, string lotteryNumber, string multiplier, ref double total)
        {
            //lg.Write("PrepareTickets Started");
            string eLottoId = GetLotteryID(lotteryId);
            if (eLottoId == "000")
                return null;
            string subCat;
            string pType;
            GetPlayType(lotteryId, playTypeId, lotteryNumber, out subCat, out pType);
            if (subCat == "" || pType == "")
                return null;
            string eIssue = GetIssue(lotteryId, issue);
            //order info xml
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            // <body>
            xtw.WriteStartElement("body");
            // <order username="" lotoid="" areaid="" orderno="">
            xtw.WriteStartElement("order");
            xtw.WriteAttributeString("username", ConfigurationManager.AppSettings["SunUserName"].ToString());
            //xtw.WriteAttributeString("lotoid", eLottoId.ToString()); // production use
            xtw.WriteAttributeString("lotoid", "001");
            //xtw.WriteAttributeString("issue", GetIssue(lotteryId, issue)); // production use
            xtw.WriteAttributeString("issue", "2011147"); // testing purposes
            xtw.WriteAttributeString("areaid", "00");
            xtw.WriteAttributeString("orderno", schemeNumber.Replace("-", "").Replace("_", "")); //order number must be unique for each day (use SchemeNumber from T_Schemes)

            // <userinfo realname="" mobile="" email="" cardtype="" cardno="" />
            xtw.WriteStartElement("userinfo");
            xtw.WriteAttributeString("realname", RealityName);
            xtw.WriteAttributeString("mobile", Mobile);
            xtw.WriteAttributeString("email", ""); //optional - not filled in
            xtw.WriteAttributeString("cardtype", "1"); //1身份证 2护照 3军官证
            xtw.WriteAttributeString("cardno", IDCardNumber); //id number
            xtw.WriteEndElement();

            // <ticket seq=""></ticket> (loop all tickets, each ticket is defined as <ticket seq="">子类-选号方式-投注号1&投注号2-倍数-金额</ticket>
            slTicket[] totalTickets = ToElectronicTicket(lotteryId, playTypeId, lotteryNumber, int.Parse(multiplier), ref total);
            for (int i = 0; i < totalTickets.Length; i++)
            {
                xtw.WriteStartElement("ticket");
                xtw.WriteAttributeString("seq", (i + 1).ToString("00000"));
                xtw.WriteString(totalTickets[i].ToString());
                xtw.WriteEndElement();
            }
            // </order>
            xtw.WriteEndElement();
            // </body>
            xtw.WriteEndElement();
            xtw.Flush();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms, Encoding.UTF8);
            string s = sr.ReadToEnd();
            xtw.Close();
            ms.Close();
            sr.Close();
            lg.Write("PrepareTickets Ended");
            return s;
        }
        /// <summary>
        /// writes the body xml of the order
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <param name="IDCardNumber"></param>
        /// <param name="Mobile"></param>
        /// <param name="RealityName"></param>
        /// <param name="lotteryId"></param>
        /// <param name="issue"></param>
        /// <param name="ticketXML"></param>
        /// <returns>XML for the body of the order</returns>
        public string PrepareTicketsXML(string OrderNumber, string IDCardNumber, string Mobile, string RealityName, int lotteryId, string issue, string ticketXML)
        {
            string eLottoId = GetLotteryID(lotteryId);
            if (eLottoId == "000")
                return null;
            string eIssue = GetIssue(lotteryId, issue);
            //order info xml
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            // <body>
            xtw.WriteStartElement("body");
            // <order username="" lotoid="" areaid="" orderno="">
            xtw.WriteStartElement("order");
            xtw.WriteAttributeString("username", ConfigurationManager.AppSettings["SunUserName"].ToString());
            xtw.WriteAttributeString("lotoid", eLottoId.ToString()); // production use
            //xtw.WriteAttributeString("lotoid", "001");
            xtw.WriteAttributeString("issue", GetIssue(lotteryId, issue)); // production use
            //xtw.WriteAttributeString("issue", "2011147"); // testing purposes
            xtw.WriteAttributeString("areaid", "00");
            xtw.WriteAttributeString("orderno", OrderNumber); //order number must be unique for each day 
            
            // <userinfo realname="" mobile="" email="" cardtype="" cardno="" />
            xtw.WriteStartElement("userinfo");
            xtw.WriteAttributeString("realname", RealityName);
            xtw.WriteAttributeString("mobile", Mobile);
            xtw.WriteAttributeString("email", ""); //optional - not filled in
            xtw.WriteAttributeString("cardtype", "1"); //1身份证 2护照 3军官证
            xtw.WriteAttributeString("cardno", IDCardNumber); //id number
            xtw.WriteEndElement();

            // insert ticket XML
            xtw.WriteRaw(ticketXML);
            // </order>
            xtw.WriteEndElement();
            // </body>
            xtw.WriteEndElement();
            xtw.Flush();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms, Encoding.UTF8);
            string s = sr.ReadToEnd();
            xtw.Close();
            ms.Close();
            sr.Close();
            //lg.Write("PrepareTickets Ended");
            return s;
        }
        /// <summary>
        /// gets the XML for each ticket to be sent
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="lotteryId"></param>
        /// <param name="playTypeId"></param>
        /// <param name="lotteryNumber"></param>
        /// <param name="multiplier"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public string ConvertToTicketXML(string seq, int lotteryId, int playTypeId, string lotteryNumber, string multiplier, double total)
        {
            string eLottoId = GetLotteryID(lotteryId);
            if (eLottoId == "000")
                return null;
            string subCat;
            string pType;
            GetPlayType(lotteryId, playTypeId, lotteryNumber, out subCat, out pType);
            if (subCat == "" || pType == "")
                return null;
            // <ticket seq=""></ticket> (loop all tickets, each ticket is defined as <ticket seq="">子类-选号方式-投注号1&投注号2-倍数-金额</ticket>
            string s = "<ticket seq=\"{0}\">{1}-{2}-{3}-{4}-{5}</ticket>";
            string lottoNum = ConvertLottoNum(lotteryId, playTypeId, lotteryNumber);
            return string.Format(s, seq, subCat, pType, lottoNum, multiplier, total);
        }

        public string GenerateOrderXML(string IDCardNumber, string Mobile, string RealityName, int lotteryID, string issueName, int playTypeID, string lotteryNum, string multiplier, double totalDB, string schemeNum)
        {
            //lg.Write("GenerateOrderXML Started");
            // request body XML
            double total = 0;
            string reqXML = PrepareTickets(IDCardNumber, Mobile, RealityName, lotteryID, issueName, schemeNum, playTypeID, lotteryNum, multiplier, ref total);
            //lg.Write("GenerateOrderXML Ended");
            return PrepRequestXML("2001", reqXML, schemeNum);
        }
        /// <summary>
        /// generates the xml to send to provider
        /// </summary>
        /// <param name="orderXML">the body xml of the request</param>
        /// <param name="orderID">the unique id of the request for each day</param>
        /// <returns>the complete xml for the order request</returns>
        public string GenerateOrderXML(string orderBodyXML, string orderID)
        {
            //lg.Write("GenerateOrderXML Started/Ended");
            return PrepRequestXML("2001", orderBodyXML, orderID);
        }
        /// <summary>
        /// Sends the request for eTicket 
        /// </summary>
        /// <param name="usr">user of the ticket</param>
        /// <param name="schemeID">scheme id to request the eTicket</param>
        /// <returns>returns the error code from provider</returns>
        public string OrdereTicketSend(string ticketXML, out string ResponseString)
        {
            Stream resp = SendRequest("2001", ticketXML);
            using (StreamReader sr = new StreamReader(resp))
            {
                ResponseString = sr.ReadToEnd();
            }
            XmlDocument xDoc = new XmlDocument();
            XmlNodeList xNodes = null;
            xDoc.Load(new StringReader(ResponseString));
            xNodes = xDoc.GetElementsByTagName("response");
            if (xNodes.Count != 1)
                return null;
            if (xNodes[0].Attributes.Count < 1)
                return null;
            try{
                return xNodes[0].Attributes["errorcode"].Value;
            }
            catch{
                return null;
            }
        }
        #endregion

        #region Inquery XML
        /// <summary>
        /// request xml for inquerying issue
        /// </summary>
        /// <param name="lotteryId">lottery ID in system</param>
        /// <param name="issueId">issue ID in system</param>
        /// <returns>the body xml for lottery issue inquery</returns>
        private string InqueryLotteryXML(int lotteryId, string issueId)
        {
            string eLottoId;
            eLottoId = GetLotteryID(lotteryId);
            if (eLottoId == "000")
                return null;
            return "<body><loto lotoid=\"" + eLottoId + "\" issue=\"" + GetIssue(lotteryId, issueId) + "\" /></body>";
        }

        /// <summary>
        /// Sends the query request for an issue
        /// </summary>
        /// <param name="lotteryId">system lottery id for the issue</param>
        /// <param name="issueId">issue to query, empty string of requesting current issue</param>
        /// <returns>information on current issue in XML format, otherwise the system eTicket error code</returns>
        public string GetLotteryInfo(int LotteryID, string IssueID, out string ResponseString)
        {
            string reqXML = HttpUtility.HtmlDecode(InqueryLotteryXML(LotteryID, IssueID));
            string x = PrepRequestXML("2000", reqXML, "2000" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            Stream resp = SendRequest("2000", HttpUtility.HtmlDecode(x));
            using (StreamReader sr = new StreamReader(resp))
            {
                ResponseString = sr.ReadToEnd();
            }
            XmlDocument xDoc = new XmlDocument();
            XmlNodeList xNodes = null;
            xDoc.Load(new StringReader(ResponseString));
            xNodes = xDoc.GetElementsByTagName("response");
            if (xNodes.Count != 1)
                return null;
            if (xNodes[0].Attributes.Count < 1)
                return null;
            try
            {
                return xNodes[0].Attributes["errorcode"].Value;
            }
            catch
            {
                return null;
            }
        }

        public string eTicketInqueryXML(string OrderNumber)
        {
            string eXml = "<body><order merchantid=\"" + ConfigurationManager.AppSettings["SunAgentID"] + "\" orderno=\"" + OrderNumber + "\"></body>";
            return eXml;
        }
        #endregion

        #region Ticket Status XML
        public string OrderStatusXML(string orderNumber)
        {
            return "<body><order merchantid=\"" + ConfigurationManager.AppSettings["SunAgentID"] + "\" orderno=\"" + orderNumber + "\" /></body>";
        }
        public string CheckOrderStatus(string orderNumber, out string ResponseString)
        {
            string cmd = "2015";
            string bodyXML = OrderStatusXML(orderNumber);
            string orderXML = PrepRequestXML(cmd, bodyXML, DateTime.Now.ToString("yyyyMMddHHmmss"));
            Stream resp = SendRequest(cmd, orderXML);
            using (StreamReader sr = new StreamReader(resp))
            {
                ResponseString = sr.ReadToEnd();
            }
            XmlDocument xDoc = new XmlDocument();
            XmlNodeList xNodes = null;
            xDoc.Load(new StringReader(ResponseString));
            xNodes = xDoc.GetElementsByTagName("response");
            if (xNodes.Count != 1)
                return null;
            if (xNodes[0].Attributes.Count < 1)
                return null;
            try{
                return xNodes[0].Attributes["errorcode"].Value;
            }
            catch{ return null; }
        }
        #endregion

        #region Other XML
        /// <summary>
        /// This method prepares the request xml for sending to server
        /// </summary>
        /// <param name="cmd">command code of the request</param>
        /// <param name="reqXML">body request xml</param>
        /// <param name="id">the unique id of the request xml</param>
        /// <returns>complete xml for sending the request</returns>
        public string PrepRequestXML(string cmd, string reqXML, string id)
        {
            //lg.Write("PrepRequestXML Started");
            MemoryStream ms = new MemoryStream();
            XmlTextWriter xtw = new XmlTextWriter(ms, Encoding.UTF8);
            xtw.WriteStartDocument();
            // <msg v="" id="">
            xtw.WriteStartElement("msg");
            xtw.WriteAttributeString("v", ConfigurationManager.AppSettings["SunVer"]);
            xtw.WriteAttributeString("id", id);
            // <ctrl>
            xtw.WriteStartElement("ctrl");
            // <agentID>Agent ID</agentID>
            string agentID = ConfigurationManager.AppSettings["SunAgentID"].ToString();
            xtw.WriteElementString("agentID", agentID);
            // <cmd>Command</cmd>
            xtw.WriteElementString("cmd", cmd);
            // <timestamp>yyyyMMddHHmmss</timestamp>
            xtw.WriteElementString("timestamp", DateTime.Now.ToString("yyyyMMddHHmmss"));
            // <md>md5(agent id + agent password + message)</md> - md5 all lower case
            string agentPWD = ConfigurationManager.AppSettings["SunAgentPwd"].ToString();
            reqXML = HttpUtility.HtmlDecode(reqXML).Replace("&amp;", "?").Replace("&", "?").Replace("?", "&amp;");
            string mdString = agentID + agentPWD + reqXML;
            MD5CryptoServiceProvider mdCrypto = new MD5CryptoServiceProvider();
            mdString = BitConverter.ToString(mdCrypto.ComputeHash(Encoding.GetEncoding(936).GetBytes(mdString))).Replace("-", "").ToLower();
            mdCrypto.Clear();
            xtw.WriteElementString("md", mdString);
            // </ctrl>
            xtw.WriteEndElement();
            // <body>
            xtw.WriteRaw(reqXML);
            // </msg>
            xtw.WriteEndElement();
            xtw.Flush();

            ms.Position = 0;
            StreamReader sr = new StreamReader(ms, Encoding.UTF8);
            string s = sr.ReadToEnd();
            xtw.Close();
            ms.Close();
            sr.Close();
            //lg.Write("PrepRequestXML Ended");
            return s;
        }

        #endregion

        #region Common functions

        /// <summary>
        /// Converts system's Lottery ID into eTicket's Lottery ID
        /// </summary>
        /// <param name="lotteryId">System's Lottery ID</param>
        /// <returns>Sun Lotto's Lottery ID</returns>
        private string GetLotteryID(int lotteryId)
        {
            string eId = "000";
            switch (lotteryId)
            {
                case SLS.Lottery.SSQ.ID: eId = SSQ.ID;
                    break;
                case SLS.Lottery.FC3D.ID: eId = FC3D.ID;
                    break;
                case SLS.Lottery.QLC.ID: eId = QLC.ID;
                    break;
                case SLS.Lottery.CQSSC.ID: eId = CQSSC.ID;
                    break;
            }
            return eId;
        }

        /// <summary>
        /// converts sun lottery id into system lottery id
        /// </summary>
        /// <param name="lottoID">sun lottery id</param>
        /// <returns>system lottery id</returns>
        public int GetSystemLotteryID(string lottoID)
        {
            int sysID = 0;
            switch (lottoID)
            {
                case SSQ.ID: sysID = SLS.Lottery.SSQ.ID;
                    break;
                case FC3D.ID: sysID = SLS.Lottery.FC3D.ID;
                    break;
                case CQSSC.ID: sysID = SLS.Lottery.CQSSC.ID;
                    break;
                case QLC.ID: sysID = SLS.Lottery.QLC.ID;
                    break;
            }
            return sysID;
        }

        private void GetPlayType(int lotteryId, int playType, string lottoNumber, out string subCat, out string pType)
        {
            subCat = "";
            pType = "";
            switch (lotteryId)
            {
                #region SSQ
                case SLS.Lottery.SSQ.ID:
                    switch (playType)
                    {
                        case SLS.Lottery.SSQ.PlayType_D:
                            subCat = SSQ.SubCategory.PuTong;
                            pType = SSQ.PlayType.DanShi;
                            break;
                        case SLS.Lottery.SSQ.PlayType_F:
                            subCat = SSQ.SubCategory.PuTong;
                            pType = SSQ.PlayType.FuShi;
                            break;
                    }
                    break;
                #endregion
                #region FC3D
                case SLS.Lottery.FC3D.ID:
                    switch (playType)
                    {
                        case SLS.Lottery.FC3D.PlayType_ZhiD:
                            subCat = FC3D.SubCategory.ZhiXuan;
                            pType = FC3D.PlayType.DanShi;
                            break;
                        case SLS.Lottery.FC3D.PlayType_ZhiF:
                            subCat = FC3D.SubCategory.ZhiXuan;
                            pType = FC3D.PlayType.FuShi;
                            break;
                        case SLS.Lottery.FC3D.PlayType_ZhiB:
                            subCat = FC3D.SubCategory.ZhiXuan;
                            pType = FC3D.PlayType.HeZhi;
                            break;
                        default:
                            subCat = null;
                            pType = null;
                            break;
                    }
                    break;
                #endregion
                #region QLC
                case SLS.Lottery.QLC.ID:
                    switch (playType)
                    {
                        case SLS.Lottery.QLC.PlayType_D:
                            subCat = QLC.SubCategory.PuTong;
                            pType = QLC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.QLC.PlayType_F:
                            subCat = QLC.SubCategory.PuTong;
                            pType = QLC.PlayType.FuShi;
                            break;
                    }
                    break;
                #endregion
                #region CQSSC
                case SLS.Lottery.CQSSC.ID:
                    int counter = lottoNumber.Replace("-", "_").Replace("\r", "").Replace("\n", "&").Split('&')[0].Count(o => o == '_');
                    switch (playType)
                    {
                        case SLS.Lottery.CQSSC.PlayType_D: //2801 单式
                            pType = CQSSC.PlayType.DanShi;
                            if (counter == 0)
                                subCat = CQSSC.SubCategory.WuXing;  // 5星单式
                            else if (counter == 2)
                                subCat = CQSSC.SubCategory.SanXing; // 3星单式
                            else if (counter == 3)
                                subCat = CQSSC.SubCategory.ErXing;  // 2星单式
                            else if (counter == 4)
                                subCat = CQSSC.SubCategory.YiXing;  // 1星单式
                            break;
                        case SLS.Lottery.CQSSC.PlayType_F: //2802 复选
                            pType = CQSSC.PlayType.FuXuan;
                            if (counter == 0)
                                subCat = CQSSC.SubCategory.WuXing;  // 5星复选
                            else if (counter == 2)
                                subCat = CQSSC.SubCategory.SanXing; // 3星复选
                            else if (counter == 3)
                                subCat = CQSSC.SubCategory.ErXing;  // 2星复选
                            break;
                        case SLS.Lottery.CQSSC.PlayType_ZH: //2803 组合玩法，传统彩票复式
                            pType = CQSSC.PlayType.FuShi;
                            if (counter == 0)
                                subCat = CQSSC.SubCategory.WuXing;  // 5星复式
                            else if (counter == 2)
                                subCat = CQSSC.SubCategory.SanXing; // 3星复式
                            else if (counter == 3)
                                subCat = CQSSC.SubCategory.ErXing;  // 2星复式
                            break;
                        case SLS.Lottery.CQSSC.PlayType_DX: //2804 大小单双
                            subCat = CQSSC.SubCategory.DaXiaoDanShuang;
                            pType = CQSSC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_5X_TXD: //2805 五星通选单式
                            subCat = CQSSC.SubCategory.WuXingTongXuan;
                            pType = CQSSC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_2X_ZuD: //2807 二星组选单式
                            subCat = CQSSC.SubCategory.ErXingZuXuan;
                            pType = CQSSC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_2X_ZuF: //2808 二星组选复式
                            subCat = CQSSC.SubCategory.ErXingZuXuan;
                            pType = CQSSC.PlayType.FuShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_2X_ZuB: //2810 二星组选包点（和值）
                            subCat = CQSSC.SubCategory.ErXingZuXuan;
                            pType = CQSSC.PlayType.HeZhi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_3X_Zu3D: // 2813 三星组3单式
                            subCat = CQSSC.SubCategory.SanXingZuSan;
                            pType = CQSSC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_3X_Zu3F: // 2814 三星组3复式
                            subCat = CQSSC.SubCategory.SanXingZuSan;
                            pType = CQSSC.PlayType.FuShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_3X_Zu6D: // 2815 三星组6单式
                            subCat = CQSSC.SubCategory.SanXingZuLiu;
                            pType = CQSSC.PlayType.DanShi;
                            break;
                        case SLS.Lottery.CQSSC.PlayType_3X_Zu6F: // 2816 三星组6复式
                            subCat = CQSSC.SubCategory.SanXingZuLiu;
                            pType = CQSSC.PlayType.FuShi;
                            break;
                    }
                    break;
                #endregion
            }
        }

        private string GetIssue(int lotteryId, string issue)
        {
            string sun_issue = null;
            switch (lotteryId)
            {
                case SLS.Lottery.SSQ.ID:
                    // 官方格式 2012074
                    sun_issue = issue;
                    break;
                case SLS.Lottery.FC3D.ID:
                    // 官方格式 2012171
                    sun_issue = issue;
                    break;
                case SLS.Lottery.QLC.ID:
                    // 官方格式 2012074
                    sun_issue = issue;
                    break;
                case SLS.Lottery.CQSSC.ID:
                    //120626073 (system = xxxxxxxx-xxx, system is full year, sun lotto only shows 2 digit year)
                    sun_issue = issue.Replace("-", "");
                    if (sun_issue.Length > 9)
                        sun_issue = sun_issue.Substring(sun_issue.Length - 9, 9);
                    break;
            }
            return sun_issue;
        }

        /// <summary>
        /// converts sun lottery drawing number into system drawing number format
        /// </summary>
        /// <param name="lotoid">sun lottery id</param>
        /// <param name="issue">sun issue id</param>
        /// <returns>system format drawing number</returns>
        public string ConvertIntoSystemIssue(string lotoid, string issue)
        {
            string sysIssue = null;
            switch (lotoid)
            {
                case SSQ.ID:
                    // 官方格式 2012074
                    sysIssue = issue;
                    break;
                case FC3D.ID:
                    // 官方格式 2012171
                    sysIssue = issue;
                    break;
                case QLC.ID:
                    // 官方格式 2012074
                    sysIssue = issue;
                    break;
                case CQSSC.ID:
                    //120626073 (system = xxxxxxxx-xxx, system is full year, sun lotto only shows 2 digit year)
                    sysIssue = DateTime.ParseExact(issue.Substring(2, 4) + issue.Substring(0, 2), "MMddyy", DateTimeFormatInfo.InvariantInfo).ToString("yyyyMMdd") + "-" + issue.Substring(6);
                    break;
            }
            return sysIssue;
        }

        private string[] SplitLotteryNumber(string Number)
        {
            string[] s = Number.Split('\n');
            if (s.Length == 0)
                return null;
            for (int i = 0; i < s.Length; i++)
                s[i] = s[i].Trim();
            return s;
        }

        private string ConvertLottoNum(int LotteryID, int PlayTypeID, string LotteryNumber)
        {
            string cNum = null;
            //if (!LotteryNumber.Contains("\r"))
            //{
            //    cNum = LotteryNumber.Replace("\n", "&amp;").Replace(" + ", "#").Replace("+", "#");
            //    return cNum;
            //}
            switch (LotteryID)
            {
                #region SSQ, QLC (same number format)
                case SLS.Lottery.SSQ.ID:
                case SLS.Lottery.QLC.ID:
                    cNum = LotteryNumber.Trim().Replace("\r", "").Replace("\n", "&");
                    break;
                #endregion
                #region CQSSC
                case SLS.Lottery.CQSSC.ID:
                    cNum = LotteryNumber.Replace("\r", "").Replace("\n", "&");
                    //if (LotteryNumber.Contains("小") || LotteryNumber.Contains("双") || LotteryNumber.Contains("单") || LotteryNumber.Contains("大"))
                    //{
                    //    // 大小单双 2804
                    //    cNum = cNum.Replace("大", "2,").Replace("小", "1,").Replace("单", "5,").Replace("双", "4,").Trim(',').Replace(",&", "&").Replace("&,", "&");
                    //}
                    //else if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuD)
                    //{
                    //    // 二星组选单式 2807
                    //    StringBuilder sb = new StringBuilder();
                    //    foreach (string s in cNum.Split('&'))
                    //    {
                    //        sb.Append("_,_,_," + s[0] + "," + s[1] + "&");
                    //    }
                    //    cNum = sb.ToString().Trim('&');
                    //}
                    //else if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu6D || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu3D)
                    //{
                    //    // 三星组6单式 2815
                    //    cNum = cNum.Replace("-", "");
                    //    StringBuilder sb = new StringBuilder();
                    //    foreach (string s in cNum.Split('&'))
                    //    {
                    //        sb.Append("_,_," + s[0] + "," + s[1] + "," + s[2] + "&");
                    //    }
                    //    cNum = sb.ToString().Trim('&');
                    //}
                    
                    if(PlayTypeID == 2803)
                    {
                        // 复式 2803
                        string aNum = cNum.Replace("-", "_");
                        string a = string.Empty;
                        for (int t = 0; t < aNum.Length; t++)
                        {
                            if (aNum[t] == ',')
                                a = a.Trim();
                            a += aNum[t];
                            if (aNum[t] != '_' && aNum[t] != ',')
                                a += " ";
                        }
                        cNum = a.Trim();
                    }
                    break;
                #endregion
                #region FC3D
                case SLS.Lottery.FC3D.ID:
                    cNum = LotteryNumber.Replace("\r", "").Replace("\n", "&");
                    if (PlayTypeID == SLS.Lottery.FC3D.PlayType_ZhiF)
                    {
                        string aNum = cNum.Replace("-", "_");
                        string a = string.Empty;
                        for (int t = 0; t < aNum.Length; t++)
                        {
                            if (aNum[t] == ',')
                                a = a.Trim();
                            a += aNum[t];
                            if (aNum[t] != '_' && aNum[t] != ',')
                                a += " ";
                        }
                        cNum = a.Trim();
                    }
                    break;
                #endregion
            }
            return cNum.Replace("&amp;", "&").Replace("&", "&amp;");
        }

        public string ConverToSystemLottoNum(string lottoid, string lottoNumber)
        {
            string sysLotto = null;
            switch (lottoid)
            {
                case SSQ.ID: sysLotto = lottoNumber.Replace(",", " ").Replace("#", " + ").Replace("+", " + ");
                    break;
                case FC3D.ID: sysLotto = lottoNumber.Replace(",", "");
                    break;
                case QLC.ID: sysLotto = lottoNumber.Replace(",", " ").Replace("#", " + ").Replace("+", " + ");
                    break;
                case CQSSC.ID: sysLotto = lottoNumber.Replace(",", "");
                    break;
            }
            return sysLotto;
        }

        /// <summary>
        /// The function to send out requests using HTTP Post to the server, and returns the response stream
        /// </summary>
        /// <param name="cmd">command code of the request</param>
        /// <param name="reqXML">xml string of the request</param>
        /// <returns>response stream</returns>
        private Stream SendRequest(string cmd, string reqXML)
        {
            string pData = "cmd=" + cmd + "&msg=" + HttpUtility.UrlEncode(reqXML);
            //lg.Write("SendRequest: " + reqXML);
            byte[] bData = Encoding.UTF8.GetBytes(pData);
            HttpWebRequest req = null;
            req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["SunPostAddr"]);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = bData.Length;
            req.Timeout = 5000;
            try{
                using (Stream writeStream = req.GetRequestStream()){
                    writeStream.Write(bData, 0, bData.Length);
                }
            }
            catch (Exception ex){
                req.Abort();
                throw ex;
            }
            Stream resp = null;
            try{
                resp = req.GetResponse().GetResponseStream();
            }
            catch(Exception ex){
                req.Abort();
                throw ex;
            }
            return resp;
        }

        private string GetErrorCode(string errCode)
        {
            string err = EtErrorNum.GeneralError;
            switch (errCode)
            {
                case "0": err = EtErrorNum.Success;
                    break;
                case "108": err = EtErrorNum.Message.SignatureErr;
                    break;
                case "112": err = EtErrorNum.AgentFailure;
                    break;
                case "301": err = EtErrorNum.Ticket.WrongFormat;
                    break;
                case "303": err = EtErrorNum.Ticket.WrongAmount;
                    break;
                case "304": err = EtErrorNum.Issue.Expired;
                    break;
            }
            return err;
        }

        public slTicket[] ToElectronicTicket(int LotteryID, int PlayTypeID, string LottoNumber, int Multiple, ref double Money)
        {

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_DX)
                return eTicket_CQSSC_DXDS(PlayTypeID, LottoNumber, Multiple, 50, ref Money);
            //else if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_D) // 2801
            //    return eTicket_CQSSC_DS(PlayTypeID, LottoNumber, Multiple, 50, ref Money);
            //else if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_ZH) // 2803
            //    return eTicket_CQSSC_DS(PlayTypeID, LottoNumber, Multiple, 50, ref Money);
            else if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_F) // 2802
                return eTicket_CQSSC_FX(PlayTypeID, LottoNumber, Multiple, 50, ref Money);
            else // 2801, 2803, 2810
                return eTicket_CQSSC_DS(PlayTypeID, LottoNumber, Multiple, 50, ref Money);
        }

        #region eTicket_CQSSC 的具体方法
        private slTicket[] eTicket_CQSSC_DXDS(int PlayTypeID, string LottoNumber, int Multiple, int MaxMultiple, ref double Money)
        {
            string[] strs = new SLS.Lottery.CQSSC().AnalyseScheme(LottoNumber, PlayTypeID).Split('\n');
            if (strs == null)
                return null;
            if (strs.Length == 0)
                return null;

            List<slTicket> totalTickets = new List<slTicket>();
            string subCat = null;
            string plType = null;
            GetPlayType(SLS.Lottery.CQSSC.ID, PlayTypeID, LottoNumber, out subCat, out plType);
            Money = 0;
            if (string.IsNullOrEmpty(subCat) || string.IsNullOrEmpty(plType))
                return null;
            int mulWalker = Multiple;
            while (mulWalker > 0)
            {
                string lottoNum = string.Empty;
                int curWalker = 0;
                if (mulWalker >= MaxMultiple)
                    curWalker = MaxMultiple;
                else
                    curWalker = mulWalker;
                for (int i = 0; i < strs.Length; i++)
                {
                    lottoNum = strs[i].Split('|')[0];
                    lottoNum = ConvertLottoNum(SLS.Lottery.CQSSC.ID, PlayTypeID, lottoNum);
                    slTicket tmp = new slTicket(subCat, plType, lottoNum, curWalker, curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]));
                    totalTickets.Add(tmp);
                    Money += curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]);
                }
                mulWalker = mulWalker - curWalker;
            }
            return totalTickets.ToArray();
        }
        private slTicket[] eTicket_CQSSC_DS(int PlayTypeID, string LottoNumber, int Multiple, int MaxMultiple, ref double Money)
        {
            string[] strs = new SLS.Lottery.CQSSC().AnalyseScheme(LottoNumber, PlayTypeID).Split('\n');
            if (strs == null)
                return null;
            if (strs.Length == 0)
                return null;

            List<slTicket> totalTickets = new List<slTicket>();
            string subCat = null;
            string plType = null;
            GetPlayType(SLS.Lottery.CQSSC.ID, PlayTypeID, LottoNumber, out subCat, out plType);
            Money = 0;
            if (string.IsNullOrEmpty(subCat) || string.IsNullOrEmpty(plType))
                return null;
            int mulWalker = Multiple;
            while (mulWalker > 0)
            {
                string lottoNum = string.Empty;
                int curWalker = 0;
                if (mulWalker >= MaxMultiple)
                    curWalker = MaxMultiple;
                else
                    curWalker = mulWalker;
                for (int i = 0; i < strs.Length; i++)
                {
                    lottoNum = strs[i].Split('|')[0];
                    lottoNum = ConvertLottoNum(SLS.Lottery.CQSSC.ID, PlayTypeID, lottoNum);
                    slTicket tmp = new slTicket(subCat, plType, lottoNum, curWalker, curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]));
                    totalTickets.Add(tmp);
                    Money += curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]);
                }
                mulWalker = mulWalker - curWalker;
            }
            return totalTickets.ToArray();
        }
        private slTicket[] eTicket_CQSSC_FX(int PlayTypeID, string LottoNumber, int Multiple, int MaxMultiple, ref double Money)
        {
            string[] strs = new SLS.Lottery.CQSSC().AnalyseScheme(LottoNumber, PlayTypeID).Split('\n');
            if (strs == null)
                return null;
            if (strs.Length == 0)
                return null;

            List<slTicket> totalTickets = new List<slTicket>();
            string subCat = null;
            string plType = null;
            GetPlayType(SLS.Lottery.CQSSC.ID, PlayTypeID, LottoNumber, out subCat, out plType);
            Money = 0;
            if (string.IsNullOrEmpty(subCat) || string.IsNullOrEmpty(plType))
                return null;
            int mulWalker = Multiple;
            while (mulWalker > 0)
            {
                string lottoNum = string.Empty;
                int curWalker = 0;
                if (mulWalker >= MaxMultiple)
                    curWalker = MaxMultiple;
                else
                    curWalker = mulWalker;
                for (int i = 0; i < strs.Length; i++)
                {
                    lottoNum = strs[i].Split('|')[0];
                    lottoNum = ConvertLottoNum(SLS.Lottery.CQSSC.ID, PlayTypeID, lottoNum);
                    slTicket tmp = new slTicket(subCat, plType, lottoNum, curWalker, curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]));
                    totalTickets.Add(tmp);
                    Money += curWalker * 2.00 * int.Parse(strs[i].Split('|')[1]);
                }
                mulWalker = mulWalker - curWalker;
            }
            return totalTickets.ToArray();
        }
        #endregion

        #region Fix for Calculation Problem on 2 or more drawings in a scheme on 2803
        
        public SLS.Lottery.Ticket[] ToElectronicTicket_ZH(int PlayTypeID, string Number, int Multiple, int MaxMultiple, ref double Money)
        {
            string[] strs = AnalyseScheme_ZH(Number, PlayTypeID).Split('\n');

            if (strs == null)
            {
                return null;
            }
            if (strs.Length == 0)
            {
                return null;
            }

            Money = 0;

            int MultipleNum = 0;

            if ((Multiple % MaxMultiple) != 0)
            {
                MultipleNum = (Multiple - (Multiple % MaxMultiple)) / MaxMultiple + 1;
            }
            else
            {
                MultipleNum = Multiple / MaxMultiple;
            }

            ArrayList al = new ArrayList();

            int EachMultiple = 1;
            double EachMoney = 0;

            for (int n = 1; n < MultipleNum + 1; n++)
            {
                if ((n * MaxMultiple) < Multiple)
                {
                    EachMultiple = MaxMultiple;
                }
                else
                {
                    EachMultiple = Multiple - (n - 1) * MaxMultiple;
                }

                for (int i = 0; i < strs.Length; i++)
                {
                    string Numbers = "";
                    EachMoney = 0;

                    Numbers = strs[i].ToString().Split('|')[0];
                    EachMoney += 2 * double.Parse(strs[i].ToString().Split('|')[1]);

                    Money += EachMoney * EachMultiple;

                    al.Add(new SLS.Lottery.Ticket(303, ConvertFormatToElectronTicket_HPCQ(PlayTypeID, Numbers), EachMultiple, EachMoney * EachMultiple));
                }
            }

            SLS.Lottery.Ticket[] Tickets = new SLS.Lottery.Ticket[al.Count];

            for (int i = 0; i < Tickets.Length; i++)
            {
                Tickets[i] = (SLS.Lottery.Ticket)al[i];
            }

            return Tickets;
        }
        private string AnalyseScheme_ZH(string Content, int PlayType)
        {
            string[] strs = Content.Split('\n');
            if (strs == null)
                return "";
            if (strs.Length == 0)
                return "";

            string Result = "";
            int Num = 1;
            string CanonicalNumber = "";

            string[] Locate = new string[5];

            Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            for (int i = 0; i < strs.Length; i++)
            {
                Match m = regex.Match(strs[i]);

                CanonicalNumber = "";
                Num = 1;
                for (int j = 0; j < 5; j++)
                {
                    Locate[j] = m.Groups["L" + j.ToString()].ToString().Trim();

                    if (Locate[j] == "")
                    {
                        CanonicalNumber = "";
                        continue;
                    }

                    if (Locate[j].Length > 1)
                    {
                        Locate[j] = Locate[j].Substring(1, Locate[j].Length - 2);

                        if (Locate[j].Length > 1)
                        {
                            Locate[j] = FilterRepeated(Locate[j]);
                        }

                        if (Locate[j] == "")
                        {
                            CanonicalNumber = "";
                            continue;
                        }
                    }

                    if (Locate[j].Length > 1)
                    {
                        CanonicalNumber += "(" + Locate[j] + ")";

                        Num = Num * Locate[j].Length;
                    }
                    else
                    {
                        CanonicalNumber += Locate[j];
                    }
                }

                Result += CanonicalNumber + "|" + Num.ToString() + "\n";
            }

            if (Result.EndsWith("\n"))
                Result = Result.Substring(0, Result.Length - 1);
            return Result;
        }
        private string FilterRepeated(string NumberPart)
        {
            string Result = "";
            for (int i = 0; i < NumberPart.Length; i++)
            {
                if ((Result.IndexOf(NumberPart.Substring(i, 1)) == -1) && ("0123456789-".IndexOf(NumberPart.Substring(i, 1)) >= 0))
                    Result += NumberPart.Substring(i, 1);
            }
            return Sort(Result);
        }
        protected string Sort(string str)
        {
            char[] ch = str.ToCharArray();
            Array.Sort(ch, new CompareToAscii());
            return new string(ch);
        }
        protected class CompareToAscii : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                return ((new CaseInsensitiveComparer()).Compare(x, y));
            }
        }
        private string ConvertFormatToElectronTicket_HPCQ(int PlayTypeID, string Number)
        {
            Number = Number.Trim();

            string Ticket = "";

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_D || PlayTypeID == SLS.Lottery.CQSSC.PlayType_F || PlayTypeID == SLS.Lottery.CQSSC.PlayType_5X_TXD || PlayTypeID == SLS.Lottery.CQSSC.PlayType_5X_TXF || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu3D || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu6D)
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    if (j % 5 == 0 && j > 0)
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        Ticket += "\n" + Number.Substring(j, 1) + ",";
                    }
                    else
                    {
                        Ticket += Number.Substring(j, 1) + ",";
                    }
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_ZH)
            {
                string[] Locate = new string[5];

                Regex regex = new Regex(@"^(?<L0>([\d-])|([(][\d]+?[)]))(?<L1>([\d-])|([(][\d]+?[)]))(?<L2>([\d-])|([(][\d]+?[)]))(?<L3>([\d-])|([(][\d]+?[)]))(?<L4>(\d)|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 5; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    }

                    Ticket += Locate[i].ToString() + ",";
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_DX)
            {
                Number = Number.Replace("大", "2").Replace("小", "1").Replace("单", "5").Replace("双", "4");

                for (int j = 0; j < Number.Length; j++)
                {
                    if (j % 2 == 0 && j > 0)
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                        Ticket += "\n" + Number.Substring(j, 1) + ",";
                    }
                    else
                    {
                        Ticket += Number.Substring(j, 1) + ",";
                    }
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuD)
            {
                string[] NumberList = Number.Split('\n');
                for (int i = 0; i < NumberList.Length; i++)   // 12\n34\n13\n36
                {
                    Ticket += "_,_,_,";
                    for (int j = 0; j < NumberList[i].Length; j++)
                    {
                        Ticket += NumberList[i].Substring(j, 1) + ",";
                    }

                    if (Ticket.EndsWith(","))
                    {
                        Ticket = Ticket.Substring(0, Ticket.Length - 1);
                    }

                    Ticket += "\n";
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuF || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu3F || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_Zu6F || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_ZHFS || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_ZuB)
            {
                for (int j = 0; j < Number.Length; j++)
                {
                    Ticket += Number.Substring(j, 1) + ",";
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuFW)
            {
                Ticket += "_,_,_,";
                string[] Locate = new string[2];

                Regex regex = new Regex(@"^(?<L0>([\d])|([(][\d]+?[)]))(?<L1>([\d])|([(][\d]+?[)]))", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Match m = regex.Match(Number);

                for (int i = 0; i < 2; i++)
                {
                    Locate[i] = m.Groups["L" + i.ToString()].ToString().Trim();

                    if (Locate[i].Length > 1)
                    {
                        Locate[i] = Locate[i].Substring(1, Locate[i].Length - 2);
                    }

                    Ticket += Locate[i].ToString() + ",";
                }
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuB)
            {
                Ticket += Number + ",";
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_2X_ZuBD || PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_ZuBD)
            {
                Ticket += Number + ",";
            }

            if (PlayTypeID == SLS.Lottery.CQSSC.PlayType_3X_B)
            {
                Ticket += Number + ",";
            }
            
            Ticket = Ticket.Substring(0, Ticket.Length - 1);
            Ticket = Ticket.Replace("-", "_");
            return Ticket;
        }

        #endregion

        #endregion
    }

}