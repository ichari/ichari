using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.IO;

namespace Alipay.Gateway
{
    /// <summary>
    ///Member 的摘要说明
    /// </summary>
    public class Member
    {
        public Member()
        {
            //
            //TODO: 在此处添加构造函数逻辑
            //
        }

        public long Query(string Account, ref string RealityName)
        {
            SystemOptions so = new SystemOptions();

            string gateway = so["MemberRegister_Alipay_Gateway"].ToString("");
            string service = "user_query";
            string partner = so["MemberRegister_Alipay_UserNumber"].ToString("");  //卖家商户号
            string Key = so["MemberRegister_Alipay_MD5"].ToString("");
            string _input_charset = "utf-8";
            string sign_type = "MD5";

            if ((gateway == "") || (partner == "") || (Key == ""))
            {
                return -1;
            }

            Utility utility = new Utility();

            string aliay_url = utility.Createurl(gateway, service, partner, Key, sign_type, _input_charset, "email", Account);

            string AlipayResult = "";

            try
            {
                AlipayResult = PF.GetHtml(aliay_url, "utf-8", 20);
            }
            catch
            {
                return -2;
            }

            if (string.IsNullOrEmpty(AlipayResult))
            {
                return -3;
            }

            XmlDocument XmlDoc = new XmlDocument();

            try
            {
                XmlDoc.Load(new StringReader(AlipayResult));
            }
            catch
            {
                return -4;
            }

            System.Xml.XmlNodeList nodesIs_success = XmlDoc.GetElementsByTagName("is_success");

            if ((nodesIs_success == null) || (nodesIs_success.Count < 1))
            {
                return -5;
            }

            string is_success = nodesIs_success[0].InnerText;

            if (is_success.ToUpper() != "T")
            {
                return -6;
            }

            System.Xml.XmlNodeList nodesUserID = XmlDoc.GetElementsByTagName("user_id");

            if ((nodesUserID == null) || (nodesUserID.Count < 1))
            {
                return -7;
            }

            System.Xml.XmlNodeList nodesUserRealityName = XmlDoc.GetElementsByTagName("real_name");

            if ((nodesUserRealityName != null) && (nodesUserRealityName.Count >= 1))
            {
                RealityName = nodesUserRealityName[0].InnerText;
            }
            else
            {
                RealityName = "";
            }

            return Shove._Convert.StrToLong(nodesUserID[0].InnerText, -8);
        }

        public string BuildLoginUrl()
        {
            new Log("bbb").Write("----------------1");

            SystemOptions so = new SystemOptions();

            string key = so["MemberSharing_Alipay_MD5"].ToString("");
            string return_url = Shove._Web.Utility.GetUrl() + "/MemberSharing/Alipay/Receive.aspx"; //服务器通知返回接口
            string gateway = "https://mapi.alipay.com/gateway.do";             //支付接口
            string _input_charset = "utf-8";
            string service = "alipay.auth.authorize";
            string sign_type = "MD5";
            string partner = so["MemberSharing_Alipay_UserNumber"].ToString("");          //卖家商户号
            string target_service = "user.auth.quick.login";

            Alipay.Gateway.Utility ap = new Alipay.Gateway.Utility();
            string aliay_url = ap.CreatUrl(
                gateway,
                service,
                partner,
                sign_type,
                key,
                return_url,
                _input_charset,
                target_service
            );

            new Log("bbb").Write(aliay_url);

            return aliay_url; //utility.CreateLoginUrl(Gateway, Partner, SignType, ReturnUrl, Key, InputCharset, parameters);

        }
    }
}