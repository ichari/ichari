using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace SLS.Notify
{
    /// <summary>
    /// 出票商接口数据提供类
    /// </summary>
    internal class ApiLotteryDataProvider : LotteryDataProvider
    {
        

        public override string GetData(int lotoTypeId,string issueNo)
        {
            var ass = Assembly.GetExecutingAssembly();
            //载入请求消息格式            
            var sm = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.Request.xml"));
            var reqFormatter = sm.ReadToEnd().Replace("\r\n", "");
            //载入XML请求数据
            var xmlReader = new StreamReader(ass.GetManifestResourceStream("SLS.Notify.Xml.Schema.xml"));
            var xmlMsg = string.Format(xmlReader.ReadToEnd().Replace("\r\n",""), lotoTypeId, issueNo);
            //格式化请求数据
            var tick = (DateTime.Now - new DateTime(1900, 1, 1)).Ticks;
            var md5Msg = SLS.Common.WebUtils.GetMd5(this.AgentId + this.AgentPwd + xmlMsg);
            var regex = new System.Text.RegularExpressions.Regex("^<body>(.*)</body>$");
            var reqMsg = regex.Match(xmlMsg).Groups[1].Value;
            var formattedReq = string.Format(reqFormatter
                , tick
                , TransType
                , DateTime.Now.ToString("yyyyMMddHHmmss")
                , md5Msg
                , reqMsg);


            //http post请求
            var req = string.Format("cmd={0}&msg={1}", TransType, formattedReq);
            var rs = SLS.Common.WebUtils.Post(this.ApiUrl, req, 10);

            return rs;
        }
    }
}
