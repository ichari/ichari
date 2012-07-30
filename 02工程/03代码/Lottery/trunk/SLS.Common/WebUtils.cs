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

namespace SLS.Common
{
    public static class WebUtils
    {
        private static object lockRoot = new object();

        public static string GetAppSettingValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
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

            //byte[] data = System.Text.Encoding.GetEncoding("GBK").GetBytes(RequestString);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(RequestString);

            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "";
            Stream outstream = request.GetRequestStream();

            outstream.Write(data, 0, data.Length);
            outstream.Close();

            HttpWebResponse hwrp = (HttpWebResponse)request.GetResponse();
            string ContentType = hwrp.Headers.Get("Content-Type");

            StreamReader sr = null;

            if (ContentType.IndexOf("text/html") > -1 || ContentType.IndexOf("application/xml") > -1)
            {
                sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));
            }
            else
            {
                sr = new StreamReader(new GZipStream(hwrp.GetResponseStream(), CompressionMode.Decompress), System.Text.Encoding.GetEncoding("GBK"));
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

        public static string GetMd5(string source)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            var bs = System.Text.Encoding.Default.GetBytes(source);
            var hash = md5.ComputeHash(bs);

            StringBuilder s = new StringBuilder();
            foreach (var item in hash) {
                s.Append(item.ToString("x2"));
            }

            return s.ToString();
        }
    }
}
