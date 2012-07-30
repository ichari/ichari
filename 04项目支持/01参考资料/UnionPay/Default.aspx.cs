using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_OnlinePay_UnionPay_Default : RoomPageBase
{
    public string Balance;
    public string UserName;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;

        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAllowPageCache = false;

        base.OnLoad(e);
    }
}