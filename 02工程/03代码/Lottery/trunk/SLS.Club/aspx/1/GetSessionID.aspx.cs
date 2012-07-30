using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Discuz.Common;
using Discuz.Forum;

namespace Discuz.Web
{
    public partial class GetSessionID : PageBase
    {
        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            this.isSynchronizeSessionID = false;

            base.OnInit(e);
        }

        #endregion

        protected override void ShowPage()
        {

        }

        override protected void OnLoad(EventArgs e)
        {
            SynchronizeSessionID ssid = new SynchronizeSessionID(this.Page);

            if (!ssid.ValidSign(this.Request))
            {
                return;
            }

            string OriginalUrl = Shove._Security.Encrypt.Decrypt3DES(Common.Utils.GetCallCert(), HttpUtility.UrlDecode(this.Request.QueryString["OriginalUrl"]), Common.Utils.DesKey);
            string SessionID = Shove._Security.Encrypt.Decrypt3DES(Common.Utils.GetCallCert(), HttpUtility.UrlDecode(this.Request.QueryString["SessionID"]), Common.Utils.DesKey);

            if (String.IsNullOrEmpty(OriginalUrl) || String.IsNullOrEmpty(SessionID))
            {
                return;
            }

            HttpCookie hc1 = new HttpCookie("CenterSessionIDSynchronized", "1");
            this.Response.Cookies.Add(hc1);

            HttpCookie hc2 = new HttpCookie("ASP.NET_SessionId", SessionID);
            this.Response.Cookies.Add(hc2);

            Response.Write("<script type='text/javascript' language='javascript'>top.location.href='" + OriginalUrl + "';</script>");

            base.OnLoad(e);
        }
    }
}