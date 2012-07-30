using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.IO.Compression;

namespace SLS.Task
{
    public class PublicFunction
    {
        public static string GetCallCert()
        {
            string Result = "";

            Result = "ShoveSoft";
            Result += " ";
            Result += "CO.,Ltd ";

            string Result2 = Shove._String.Reverse(Result);

            Result = "--";
            Result += " by Shove ";

            Result = Shove._String.Reverse(Result2) + Result;

            Result2 = Shove._String.Reverse(Result);

            Result = "20050709";
            Result += Shove._String.Reverse("圳深 ");
            Result += Shove._String.Reverse("安宝");

            Result = Shove._String.Reverse(Result);

            Result = Shove._String.Reverse(Result2) + Shove._String.Reverse(Result);

            return Result;
        }

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

            if (ContentType.IndexOf("text/html") > -1)
            {
                sr = new StreamReader(hwrp.GetResponseStream(), System.Text.Encoding.GetEncoding("GBK"));
            }
            else
            {
                sr = new StreamReader(new GZipStream(hwrp.GetResponseStream(), CompressionMode.Decompress), System.Text.Encoding.GetEncoding("GBK"));
            }

            return sr.ReadToEnd();
        }
    }
}
