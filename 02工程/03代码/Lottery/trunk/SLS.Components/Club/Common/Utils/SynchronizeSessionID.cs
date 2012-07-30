using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discuz.Common
{
    /// <summary>
    ///SessionCenter 的摘要说明
    /// </summary>
    public class SynchronizeSessionID
    {
        private System.Web.UI.Page page;

        public SynchronizeSessionID(System.Web.UI.Page _page)
        {
            if (_page == null)
            {
                throw new Exception("SynchronizeSessionID 类需要一个实例化的 System.Web.UI.Page 类型参数。");
            }

            page = _page;
        }

        public bool Synchronize()
        {
            if (page.IsPostBack)
            {
                return false;
            }

            HttpCookie hc = page.Request.Cookies["CenterSessionIDSynchronized"];

            if (hc != null)
            {
                if (hc.Value == "1")
                {
                    return false;
                }
            }

            string OriginalUrl = Shove._Security.Encrypt.Encrypt3DES(Common.Utils.GetCallCert(), page.Request.Url.AbsoluteUri.Replace("aspx/1/", ""), Discuz.Common.Utils.DesKey);
            string ReceiveUrl = Shove._Security.Encrypt.Encrypt3DES(Common.Utils.GetCallCert(), Shove._Web.Utility.GetUrl() + "/GetSessionID.aspx", Common.Utils.DesKey);
            string CenterUrl = Shove._Web.WebConfig.GetAppSettingsString("CenterUrl");

            CenterUrl += CenterUrl.EndsWith("/") ? "" : "/";

            page.Response.Redirect(CenterUrl + "GetSessionID.aspx?OriginalUrl=" + HttpUtility.UrlEncode(OriginalUrl) + "&ReceiveUrl=" + HttpUtility.UrlEncode(ReceiveUrl) + "&Sign=" + GenSign(OriginalUrl, ReceiveUrl));

            return true;
        }

        public string GenSign(params string[] inputs)
        {
            string SourceString = "";

            foreach (string str in inputs)
            {
                SourceString += str;
            }

            return Shove._Security.Encrypt.MD5(SourceString + Common.Utils.CenterMD5Key);
        }

        public bool ValidSign(System.Web.HttpRequest request)
        {
            string Sign = HttpUtility.UrlDecode(request["Sign"]);
            string OriginalUrl = HttpUtility.UrlDecode(request["OriginalUrl"]);
            string SessionID = HttpUtility.UrlDecode(request["SessionID"]);

            string SourceString = OriginalUrl + SessionID;

            string mysign = Shove._Security.Encrypt.MD5(SourceString + Common.Utils.CenterMD5Key);

            return (String.Compare(Sign, mysign, StringComparison.OrdinalIgnoreCase) == 0);
        }
    }
}
