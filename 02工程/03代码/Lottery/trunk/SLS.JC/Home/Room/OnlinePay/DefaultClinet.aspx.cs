using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

//public partial class OnlinePay_DefaultClinet : PageBase
//{
    #region
    //public string OnlinePay_99Bill_HomePage = "", OnlinePay_99Bill_Status = "", OnlinePay_99Bill_Target = "_self";
    //public string OnlinePay_CBPayMent_HomePage = "", OnlinePay_CBPayMent_Status = "", OnlinePay_CBPayMent_Target = "_self";
    //public string OnlinePay_Tenpay_HomePage = "", OnlinePay_Tenpay_Status = "", OnlinePay_Tenpay_Target = "_self";
    //public string OnlinePay_Alipay_HomePage = "", OnlinePay_Alipay_Status = "", OnlinePay_Alipay_Target = "_self";
    //public string OnlinePay_CnCard_HomePage = "", OnlinePay_CnCard_Status = "", OnlinePay_CnCard_Target = "_self";
    //public string OnlinePay_ICBC_HomePage = "", OnlinePay_ICBC_Status = "", OnlinePay_ICBC_Target = "_self";
    //public string OnlinePay_CMBChina_HomePage = "", OnlinePay_CMBChina_Status = "", OnlinePay_CMBChina_Target = "_self";

    //SystemOptions so = new SystemOptions();

    //private string Sign;
    //private string UserName;
    //private string UserPassword;

    //protected void Page_Load(object sender, EventArgs e)
    //{
    //    // 获取用户
    //    Sign = Shove._Security.Encrypt.UnEncryptString(PublicFunction.GetCallCert(), Shove._Web.Utility.GetRequest("Sign"));
    //    UserName = Shove._Security.Encrypt.UnEncryptString(PublicFunction.GetCallCert(), Shove._Web.Utility.GetRequest("UN"));
    //    UserPassword = Shove._Security.Encrypt.UnEncryptString(PublicFunction.GetCallCert(), Shove._Web.Utility.GetRequest("UP"));

    //    if (Sign == FormsAuthentication.HashPasswordForStoringInConfigFile(UserName + UserPassword + "7ien.shovesoft.shove 中国深圳 2007-10-25", "MD5"))
    //    {
    //        _User = new Users(_Site.ID)[_Site.ID, UserName];

    //        if (_User == null)
    //        {
    //            // 不存在

    //            return;
    //        }

    //        if (_User.Password != UserPassword)
    //        {
    //            // 

    //            return;
    //        }

    //        string ReturnDescription = "";

    //        if (_User.LoginDirect(ref ReturnDescription) < 0)
    //        {
    //            //

    //            return;
    //        }
    //    }

    //    int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType");

    //    if (OnlinePayType == 1)
    //    {
    //        Response.Redirect("Alipay02/SendClient.aspx", true);

    //        return;
    //    }

    //    //BindDataForPayList();
    //}

    //#region Web 窗体设计器生成的代码
    //override protected void OnInit(EventArgs e)
    //{
    //    RequestLoginPage = "OnlinePay/DefaultClinet.aspx?UN=" + Shove._Web.Utility.GetRequest("UN") + "&UP=" + Shove._Web.Utility.GetRequest("UP") + "&Sign=" + Shove._Web.Utility.GetRequest("Sign");
    //    base.OnInit(e);
    //}
    //#endregion

    //private void BindDataForPayList()
    //{
    //    // AliPay
    //    bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);
    //    if (OnlinePay_Alipay_Status_ON)
    //    {
    //        OnlinePay_Alipay_HomePage = "Alipay02/SendClient.aspx?UN=" + UserName + "&UP=" + UserPassword + "&Sign=" + Sign;
    //    }
    //    else
    //    {
    //        OnlinePay_Alipay_HomePage = "https://www.alipay.com/";
    //        OnlinePay_Alipay_Status = "(暂未开通)";
    //        OnlinePay_Alipay_Target = "_blank";
    //        Panel_Alipay.Visible = false;
    //    }

    //    // 99Bill
    //    bool OnlinePay_99Bill_Status_ON = so["OnlinePay_99Bill_Status_ON"].ToBoolean(false);
    //    if (OnlinePay_99Bill_Status_ON)
    //    {
    //        OnlinePay_99Bill_HomePage = "99Bill/SendClient.aspx?UN=" + UserName + "&UP=" + UserPassword + "&Sign=" + Sign;
    //    }
    //    else
    //    {
    //        OnlinePay_99Bill_HomePage = "https://www.99bill.com/website/";
    //        OnlinePay_99Bill_Status = "(暂未开通)";
    //        OnlinePay_99Bill_Target = "_blank";
    //        Panel_99Bill.Visible = false;
    //    }

    //    // TenPay
    //    bool OnlinePay_Tenpay_Status_ON = so["OnlinePay_Tenpay_Status_ON"].ToBoolean(false);
    //    if (OnlinePay_Tenpay_Status_ON)
    //    {
    //        OnlinePay_Tenpay_HomePage = "Tenpay/SendClient.aspx?UN=" + UserName + "&UP=" + UserPassword + "&Sign=" + Sign;
    //    }
    //    else
    //    {
    //        OnlinePay_Tenpay_HomePage = "https://www.tenpay.com/";
    //        OnlinePay_Tenpay_Status = "(暂未开通)";
    //        OnlinePay_Tenpay_Target = "_blank";
    //        Panel_Tenpay.Visible = false;
    //    }

    //    // CBPayMent
    //    bool OnlinePay_CBPayMent_Status_ON = so["OnlinePay_CBPayMent_Status_ON"].ToBoolean(false);
    //    if (OnlinePay_CBPayMent_Status_ON)
    //    {
    //        OnlinePay_CBPayMent_HomePage = "CBPayMent/SendClient.aspx?UN=" + UserName + "&UP=" + UserPassword + "&Sign=" + Sign;
    //    }
    //    else
    //    {
    //        OnlinePay_CBPayMent_HomePage = "http://www.chinabank.com.cn/";
    //        OnlinePay_CBPayMent_Status = "(暂未开通)";
    //        OnlinePay_CBPayMent_Target = "_blank";
    //        Panel_CBPayMent.Visible = false;
    //    }
    //}
    #endregion
public partial class OnlinePay_DefaultClinet : RoomPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string sign = Shove._Web.Utility.GetRequest("sign");
        string r = Shove._Web.Utility.GetRequest("r");

        if (!string.IsNullOrEmpty(sign) && _User == null && !string.IsNullOrEmpty(r))
        {
            string UserJoinDate = "";

            try
            {
                UserJoinDate = Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), sign, "tfgyNQ56Gkp97otH33yu4Tta");
            }
            catch { }

            if (string.IsNullOrEmpty(UserJoinDate))
            {
                Response.Redirect("FailClient.aspx", true);

                return;
            }

            if (UserJoinDate.Split('|').Length != 2)
            {
                Response.Redirect("FailClient.aspx", true);

                return;
            }

            long UserID = Shove._Convert.StrToLong(UserJoinDate.Split('|')[0], 0);

            if (UserID < 1)
            {
                Response.Redirect("FailClient.aspx", true);

                return;
            }

            DateTime dt = Shove._Convert.StrToDateTime(UserJoinDate.Split('|')[1], "1987-01-01");

            //if (dt.Subtract(DateTime.Now).TotalMinutes > 5 || dt.Subtract(DateTime.Now).TotalMinutes < -5)
            //{
            //    Response.Redirect("FailClient.aspx", true);

            //    return;
            //}

            _User = new Users(1)[1, UserID];

            string ReturnDescption = "";

            _User.Login(ref ReturnDescption);

            if (_User == null)
            {
                Response.Redirect("FailClient.aspx", true);

                return;
            }

            if (r != FormsAuthentication.HashPasswordForStoringInConfigFile(_User.Name + "|" + dt.ToString("yyyy-MM-dd HH:mm:ss"), "MD5"))
            {
                string ReturnDescptiong = "";
                _User.LoginDirect(ref ReturnDescptiong);

                Response.Redirect("FailClient.aspx", true);

                return;
            }
        }

        int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType", 2);

        if (OnlinePayType == 1)
        {
            Response.Redirect("Alipay01/Send.aspx", true);

            return;
        }
        else
        {
            //Response.Redirect("Alipay02/Send_Default.aspx", true);
            Response.Redirect("ClientPay/SendClient.aspx", true);

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
