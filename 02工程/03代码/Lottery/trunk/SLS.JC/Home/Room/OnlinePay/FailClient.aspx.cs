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

public partial class OnlinePay_FailClient : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "OnlinePay/FailClient.aspx";

        base.OnLoad(e);
    }
}
