using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;

namespace Discuz.Web.UI
{
    public partial class StatusPage : PageBase
    {
        public StatusPage()
        {
            this.Load += new EventHandler(Status_Load);
        }

        void Status_Load(object sender, EventArgs e)
        {
            APIConfigInfo apiInfo = APIConfigs.GetConfig();
            if (!apiInfo.Enable)
                return;
            ApplicationInfo appInfo = null;
            ApplicationInfoCollection appcollection = apiInfo.AppCollection;
            foreach (ApplicationInfo newapp in appcollection)
            {
                if (newapp.APIKey == DNTRequest.GetString("api_key"))
                {
                    appInfo = newapp;
                }
            }
            if (appInfo == null)
                return;


            string next = DNTRequest.GetString("next");
            string reurl = string.Format("{0}{1}user_status={2}{3}", appInfo.CallbackUrl, appInfo.CallbackUrl.IndexOf("?") > 0 ? "&" : "?", userid > 0 ? "1" : "0", next == string.Empty ? next : "next=" + next);
            Response.Redirect(reurl);

        }
    }
}
