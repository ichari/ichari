using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Web;

namespace Ichari.Common
{
    public static class WebUtils
    {
        private static object lockRoot = new object();

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 生成交易流水号，例如：2012-05-31-14-02-56-235-656
        /// 在多线程/多台服务器的时候，可能不够严谨
        /// </summary>
        /// <returns></returns>
        public static string GenTradeNo(Random r)
        {
            lock (lockRoot)
            {                
                
                var randomStr = r.Next(100, 999);
                return string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), randomStr);
                
            }
        }

        /// <summary>
        /// 将金额转换成单位为分的整数形式
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static string ConvertToFen(decimal amount)
        {
            var arr = amount.ToString().Split('.');
            if (arr[1].Length > 2)
                throw new Exception("金额只能带有两位小数");
            var r = (int)Math.Round(amount * 100);
            return r.ToString();
        }

        public static string Post(string Url, string RequestString, int TimeoutSeconds)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);

            if (TimeoutSeconds > 0)
            {
                request.Timeout = 1000 * TimeoutSeconds;
            }
            request.Method = "POST";
            request.AllowAutoRedirect = true;

            byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(RequestString);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "";
            Stream outstream = request.GetRequestStream();

            outstream.Write(data, 0, data.Length);
            outstream.Close();

            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            string ContentType = hwrp.Headers.Get("Content-Type");

            StreamReader sr = null;
            if (string.IsNullOrEmpty(ContentType))
                sr = new StreamReader(hwrp.GetResponseStream());
            else
            {
                if (ContentType.IndexOf("text/html") > -1)
                {
                    sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));
                }
                else
                {
                    sr = new StreamReader(new GZipStream(hwrp.GetResponseStream(), CompressionMode.Decompress), System.Text.Encoding.GetEncoding("GBK"));
                }
            }

            return sr.ReadToEnd();
        }

        public static string GetClientIPAddress()
        {
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
        }
    }
}
