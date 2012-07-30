using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections;

/// <summary>
/// New Interface for AliPay
/// </summary>
namespace Alipay.Gateway
{
    /// <summary>
    /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
    /// </summary>
    public class Utility
    {
        public static string GetMD5(string s, string _input_charset)
        {
            /// <summary>
            /// 与ASP兼容的MD5加密算法
            /// </summary>

            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
            {
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string[] BubbleSort(string[] r)
        {
            /// <summary>
            /// 冒泡排序法
            /// </summary>

            int i, j; //交换标志 
            string temp;

            bool exchange;

            for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
            {
                exchange = false; //本趟排序开始前，交换标志应为假

                for (j = r.Length - 2; j >= i; j--)
                {
                    if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)//交换条件
                    {
                        temp = r[j + 1];
                        r[j + 1] = r[j];
                        r[j] = temp;

                        exchange = true; //发生了交换，故将交换标志置为真 
                    }
                }

                if (!exchange) //本趟排序未发生交换，提前终止算法 
                {
                    break;
                }

            }
            return r;
        }

        public string CreatUrl(
            string gateway,
            string service,
            string partner,
            string sign_type,
            string out_trade_no,
            string subject,
            string body,
            string payment_type,
            string total_fee,
            string show_url,
            string seller_email,
            string key,
            string return_url,
            string _input_charset,
            string notify_url,
            string agentID,
            string buyer_email
            )
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "subject=" + subject, 
                "body=" + body, 
                "out_trade_no=" + out_trade_no, 
                "total_fee=" + total_fee, 
                "show_url=" + show_url,  
                "payment_type=" + payment_type, 
                "seller_email=" + seller_email, 
                "notify_url=" + notify_url,
                "_input_charset="+_input_charset,          
                "return_url=" + return_url,
                "agent=" + agentID
                //"buyer_email=" + buyer_email
                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);

            for (i = 0; i < Sortedstr.Length; i++)
            {

                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
                //parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + Sortedstr[i].Split(delimiterChars)[1] + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }

        public string CreatUrl(
            string gateway,
            string service,
            string partner,
            string sign_type,
            string out_trade_no,
            string subject,
            string body,
            string payment_type,
            string total_fee,
            string show_url,
            string seller_email,
            string key,
            string return_url,
            string _input_charset,
            string notify_url,
            string agentID,
            string BankCode,
            string buyer_email
            )
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr = { 
                "service=" + service, 
                "partner=" + partner, 
                "subject=" + subject, 
                "body=" + body, 
                "out_trade_no=" + out_trade_no, 
                "total_fee=" + total_fee, 
                "show_url=" + show_url,  
                "payment_type=" + payment_type, 
                "seller_email=" + seller_email, 
                "notify_url=" + notify_url,
                "_input_charset="+_input_charset,          
                "return_url=" + return_url,
                "defaultbank=" + BankCode,
                "agent=" + agentID
                //"buyer_email=" + buyer_email
                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);


            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);

            for (i = 0; i < Sortedstr.Length; i++)
            {

                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
                //parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + Sortedstr[i].Split(delimiterChars)[1] + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }

        /// <summary>
        /// 会员共享
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="service"></param>
        /// <param name="partner"></param>
        /// <param name="sign_type"></param>
        /// <param name="key"></param>
        /// <param name="return_url"></param>
        /// <param name="_input_charset"></param>
        /// <returns></returns>
        public string CreatUrl(
            string gateway,
            string service,
            string partner,
            string sign_type,
            string key,
            string return_url,
            string _input_charset,
            string target_service
            )
        {
            if (!gateway.EndsWith("?"))
            {
                gateway += "?";
            }

            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "service="+service, 
                "partner=" + partner, 
                "_input_charset="+_input_charset,          
                "return_url=" + return_url,
                "target_service=" + target_service
                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);

            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {
                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), _input_charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }

        public string Createurl(string gateway, string service, string partner, string Key, string sign_type, string Charset, params string[] ParamsAndValue)
        {
            if (!gateway.EndsWith("?"))
            {
                gateway += "?";
            }

            ArrayList al = new ArrayList();

            //////////////////固定参数///////////

            al.Add("_input_charset=" + Charset);
            al.Add("partner=" + partner);
            al.Add("service=" + service);

            ////////可变参数/////////////////
            for (int i = 0; i < ParamsAndValue.Length / 2; i++)
            {
                if ((ParamsAndValue[i * 2] != "") && (ParamsAndValue[i * 2 + 1]) != "")
                {
                    al.Add(ParamsAndValue[i * 2].ToLower() + "=" + ParamsAndValue[i * 2 + 1]);
                }
            }
            ///////////////////////////////////////////////////////////////////

            //初始数组
            string[] InitialOristr = new string[al.Count];
            string[] Oristr = new string[al.Count];

            for (int i = 0; i < al.Count; i++)
            {
                Oristr[i] = al[i].ToString();
                InitialOristr[i] = al[i].ToString();
            }

            //进行排序
            string[] Sortedstr = BubbleSort(Oristr);

            //构造待md5摘要字符串

            StringBuilder prestr = new StringBuilder();

            for (int i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {

                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(Key);

            //生成Md5摘要；
            string sign = GetMD5(prestr.ToString(), Charset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(gateway);

            for (int i = 0; i < InitialOristr.Length; i++)
            {
                parameter.Append(InitialOristr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(InitialOristr[i].Split(delimiterChars)[1]) + "&");
                //parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + Sortedstr[i].Split(delimiterChars)[1] + "&");
            }

            parameter.Append("sign=" + sign + "&sign_type=" + sign_type);

            //返回支付Url；
            return parameter.ToString();
        }

        public string CreateLoginUrl(string Gateway, string Partner, string SignType, string ReturnUrl, string Key, string InputCharset, string parameters)
        {
            /// <summary>
            /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
            /// </summary>
            int i;

            //构造数组；
            string[] Oristr ={ 
                "Partner=" + Partner, 
                "InputCharset=" + InputCharset,          
                "ReturnUrl=" + ReturnUrl,
                "parameters=" + parameters
                };

            //进行排序；
            string[] Sortedstr = BubbleSort(Oristr);

            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr.Append(Sortedstr[i]);

                }
                else
                {
                    prestr.Append(Sortedstr[i] + "&");
                }
            }

            prestr.Append(Partner + Key);

            //生成Md5摘要；
            string Sign = Shove.Alipay.Alipay.GetMD5(prestr.ToString(), InputCharset);

            //构造支付Url；
            char[] delimiterChars = { '=' };
            StringBuilder parameter = new StringBuilder();
            parameter.Append(Gateway);
            for (i = 0; i < Sortedstr.Length; i++)
            {
                parameter.Append(Sortedstr[i].Split(delimiterChars)[0] + "=" + HttpUtility.UrlEncode(Sortedstr[i].Split(delimiterChars)[1]) + "&");
            }

            parameter.Append("Sign=" + Sign + "&SignType=" + SignType);

            //返回支付Url；
            return parameter.ToString();
        }
    }
}