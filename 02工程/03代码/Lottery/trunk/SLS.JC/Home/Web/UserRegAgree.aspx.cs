using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Web_UserRegAgree : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string type = Request["type"];
            string key = "Opt_UserRegisterAgreement";

            if((!string.IsNullOrEmpty(type)) && type.Trim().ToUpper() == "CPS")
                key = "Opt_CpsRegisterAgreement";

            labAgreement.Text = _Site.SiteOptions[key].ToString("").Replace("[SiteName]", _Site.Name).Replace("[SiteUrl]", Shove._Web.Utility.GetUrl()).Replace("http://www.icaile.com", Shove._Web.Utility.GetUrl());
        }
    }
}
