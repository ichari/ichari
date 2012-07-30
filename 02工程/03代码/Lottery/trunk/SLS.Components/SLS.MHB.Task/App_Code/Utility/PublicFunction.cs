using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.MHB.Task
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Net;
    using System.IO;
    using System.IO.Compression;
    public class PublicFunction
    {
        // 获取 Url 返回的 Html 流
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

            sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));

            return sr.ReadToEnd();
        }
    }
}
