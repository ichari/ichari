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

public partial class Home_Room_OnlinePay_Default : RoomPageBase
{
    public long BuyID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), 0);

        string sign = Shove._Web.Utility.GetRequest("sign");
        string r = Shove._Web.Utility.GetRequest("r");

        if (!string.IsNullOrEmpty(sign) && _User == null && !string.IsNullOrEmpty(r))
        {
            string UserJoinDate = "";

            try
            {
                UserJoinDate = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), sign);
            }
            catch { }

            if (string.IsNullOrEmpty(UserJoinDate))
            {
                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }

            if (UserJoinDate.Split('|').Length != 2)
            {
                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }

            long UserID = Shove._Convert.StrToLong(UserJoinDate.Split('|')[0], 0);

            if (UserID < 1)
            {
                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }

            DateTime dt = Shove._Convert.StrToDateTime(UserJoinDate.Split('|')[1], "1987-01-01");

            if (dt.CompareTo(DateTime.Now) > 30 || dt.CompareTo(DateTime.Now) < -30)
            {
                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }

            _User = new Users(1)[1, UserID];

            if (_User == null)
            {
                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }

            if (r != Shove._Security.Encrypt.MD5(_User.Name + "|" + dt.ToString("yyyy-mm-dd HH:MM:ss")))
            {
                string ReturnDescptiong = "";
                _User.LoginDirect(ref ReturnDescptiong);

                Response.Redirect("Alipay01/Fail.aspx", true);

                return;
            }
        }

        int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType", 2);

        if (OnlinePayType == 1)
        {
            Response.Redirect("Alipay01/Send.aspx?BuyID=" + BuyID.ToString(), true);

            return;
        }
        else
        {
            Response.Redirect("Alipay02/Send_Default.aspx?BuyID=" + BuyID.ToString(), true);

            return;
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        RequestLoginPage = "~/Home/Room/OnlinePay/Default.aspx";

        base.OnLoad(e);
    }
}
