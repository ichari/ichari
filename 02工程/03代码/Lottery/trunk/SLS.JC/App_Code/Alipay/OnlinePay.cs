using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;

namespace Alipay.Gateway
{
    /// <summary>
    ///OnlinePay 的摘要说明
    /// </summary>
    public class OnlinePay
    {
        public OnlinePay()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public int Query(string PayType, string PaymentNumber, ref string AlipayPaymentNumber, ref string ReturnDescription)
        {
            SystemOptions so = new SystemOptions();

            string gateway = so["QueryAddMoney_Alipay_Gateway"].ToString("");
            string service = "single_trade_query";
            string partner = "";  //卖家商户号
            string Key = "";
            string _input_charset = "utf-8";
            string sign_type = "MD5";

            if (PayType == "ALIPAY_alipay")
            {
                partner = so["OnlinePay_Alipay_UserNumber"].ToString("");  //卖家商户号
                Key = so["OnlinePay_Alipay_MD5Key"].ToString("");
            }
            else
            {
                partner = so["OnlinePay_Alipay_UserNumber1"].ToString("");  //卖家商户号
                Key = so["OnlinePay_Alipay_MD5Key1"].ToString("");
            }

            if ((gateway == "") || (partner == "") || (Key == ""))
            {
                ReturnDescription = "系统设置信息错误";

                return -1;
            }

            Utility utility = new Utility();

            string aliay_url = utility.Createurl(gateway, service, partner, Key, sign_type, _input_charset, "out_trade_no", PaymentNumber);

            string AlipayResult = "";

            try
            {
                AlipayResult = PF.GetHtml(aliay_url, "utf-8", 120);
            }
            catch
            {
                ReturnDescription = "数据获取异常，请重新审核";

                return -2;
            }

            if (string.IsNullOrEmpty(AlipayResult))
            {
                ReturnDescription = "数据获取异常，请重新审核";

                return -3;
            }

            XmlDocument XmlDoc = new XmlDocument();

            try
            {
                XmlDoc.Load(new StringReader(AlipayResult));
            }
            catch
            {
                ReturnDescription = "数据获取异常，请重新审核";

                return -4;
            }

            System.Xml.XmlNodeList nodesIs_success = XmlDoc.GetElementsByTagName("is_success");

            if ((nodesIs_success == null) || (nodesIs_success.Count < 1))
            {
                ReturnDescription = "查询信息获取异常，请重新查询";

                return -5;
            }

            string is_success = nodesIs_success[0].InnerText;

            if (is_success.ToUpper() != "T")
            {
                ReturnDescription = "该支付记录未支付成功";

                return -6;
            }

            System.Xml.XmlNodeList nodesTrade_no = XmlDoc.GetElementsByTagName("trade_no");

            if ((nodesTrade_no == null) || (nodesTrade_no.Count < 1))
            {
                ReturnDescription = "没有对应的支付信息";

                return -7;
            }

            AlipayPaymentNumber = nodesTrade_no[0].InnerText;

            System.Xml.XmlNodeList nodesTrade_Status = XmlDoc.GetElementsByTagName("trade_status");

            if ((nodesTrade_Status == null) || (nodesTrade_Status.Count < 1))
            {
                ReturnDescription = "没有对应的支付信息";

                return -8;
            }

            string Trade_Status = nodesTrade_Status[0].InnerText.ToUpper();

            if (Trade_Status == "WAIT_BUYER_PAY")
            {
                ReturnDescription = "等待买家付款";

                return -9;
            }

            if (Trade_Status == "WAIT_SELLER_SEND_GOODS")
            {
                ReturnDescription = "买家付款成功(担保交易，未确定支付给商家)";

                return -10;
            }

            if (Trade_Status == "WAIT_BUYER_CONFIRM_GOODS")
            {
                ReturnDescription = "卖家发货成功(未确定支付给商家)";

                return -11;
            }

            if (Trade_Status == "TRADE_CLOSED")
            {
                ReturnDescription = "交易被关闭，未成功付款";

                return -12;
            }

            if (Trade_Status != "TRADE_FINISHED" && Trade_Status != "TRADE_SUCCESS")
            {
                ReturnDescription = "其他未成功支付的错误";

                return -9999;
            }

            return 0;
        }
    }
}