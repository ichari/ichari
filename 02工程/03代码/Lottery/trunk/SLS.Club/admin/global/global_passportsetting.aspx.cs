using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Forum;

using Discuz.Common;
using Discuz.Config;

namespace Discuz.Web.Admin
{
#if NET1
    public class global_passportsetting : AdminPage
#else
    public partial class global_passportsetting : AdminPage
#endif
    {
#if NET1
		protected System.Web.UI.HtmlControls.HtmlForm form1;

		protected Discuz.Control.TextBox appname;
		protected Discuz.Control.TextBox appurl;
		protected Discuz.Control.TextBox callbackurl;
		protected Discuz.Web.Admin.TextareaResize ipaddresses;
		protected System.Web.UI.HtmlControls.HtmlInputHidden apikeyhidd;
		protected Discuz.Control.Button savepassportinfo;
		protected Discuz.Control.Hint hint1;
#endif
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string apikey = DNTRequest.GetString("apikey");
                if (apikey != "")
                {
                    APIConfigInfo aci = APIConfigs.GetConfig();
                    foreach (ApplicationInfo ai in aci.AppCollection)
                    {
                        if (ai.APIKey == apikey)
                        {
                            appname.Text = ai.AppName;
                            appurl.Text = ai.AppUrl;
                            callbackurl.Text = ai.CallbackUrl;
                            ipaddresses.Text = ai.IPAddresses;
                            break;
                        }
                    }
                }
                apikeyhidd.Value = apikey;
            }
        }

        protected void savepassportinfo_Click(object sender, EventArgs e)
        {
            if (appname.Text.Trim() == "")
            {
                base.RegisterStartupScript("PAGE", "alert('整合程序名称不能为空!');");
                return;
            }
            if (appurl.Text.Trim() == "")
            {
                base.RegisterStartupScript("PAGE", "alert('整合程序 Url 地址不能为空!');");
                return;
            }
            if (callbackurl.Text.Trim() == "")
            {
                base.RegisterStartupScript("PAGE", "alert('登录完成后返回地址不能为空!');");
                return;
            }
            if (ipaddresses.Text.Trim() != "")
            {
                foreach (string ip in ipaddresses.Text.Replace("\r\n","").Replace(" ","").Split(','))
                {
                    if (!Utils.IsIP(ip))
                    {
                        base.RegisterStartupScript("PAGE", "alert('IP地址格式错误!');");
                        return;
                    }
                }
            }
            if (apikeyhidd.Value == "") //增加
            {
                ApplicationInfo ai = new ApplicationInfo();
                ai.AppName = appname.Text;
                ai.AppUrl = appurl.Text;
                ai.APIKey = Utils.MD5(System.Guid.NewGuid().ToString());
                ai.Secret = Utils.MD5(System.Guid.NewGuid().ToString());
                ai.CallbackUrl = callbackurl.Text;
                ai.IPAddresses = ipaddresses.Text.Replace("\r\n","").Replace(" ","");
                APIConfigInfo aci = APIConfigs.GetConfig();
                if (aci.AppCollection == null)
                    aci.AppCollection = new ApplicationInfoCollection();
                aci.AppCollection.Add(ai);
                APIConfigs.SaveConfig(aci);
            }
            else   //修改
            {
                APIConfigInfo aci = APIConfigs.GetConfig();
                foreach (ApplicationInfo ai in aci.AppCollection)
                {
                    if (ai.APIKey == apikeyhidd.Value)
                    {
                        ai.AppName = appname.Text;
                        ai.AppUrl = appurl.Text;
                        ai.CallbackUrl = callbackurl.Text;
                        ai.IPAddresses = ipaddresses.Text.Replace("\r\n","").Replace(" ","");
                        break;
                    }
                }
                APIConfigs.SaveConfig(aci);
            }
            Response.Redirect("global_passportmanage.aspx");
        }
    }
}
