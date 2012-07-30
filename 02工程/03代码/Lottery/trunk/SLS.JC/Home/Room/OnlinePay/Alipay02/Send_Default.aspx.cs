using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_OnlinePay_Alipay02_Send_Default : RoomPageBase
{
    public string Balance;
    public string UserName;

    public string DefaultSendPage="";
    public long BuyID = 0;

    SystemOptions so = new SystemOptions();
    protected void Page_Load(object sender, EventArgs e)
    {
        BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), 0);

        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
        }

        int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType", 2);
        if (OnlinePayType == 1)
        {
            Response.Redirect("../Alipay01/Default.aspx", true);

            return;
        }

        bool OnlinePay_ChinaUnion_Status_ON = so["OnlinePay_ChinaUnion_Status_ON"].ToBoolean(false);
        bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);
        bool OnlinePay_99Bill_Status_ON = so["OnlinePay_99Bill_Status_ON"].ToBoolean(false);
        bool OnlinePay_Tenpay_Status_ON = so["OnlinePay_Tenpay_Status_ON"].ToBoolean(false);
        bool OnlinePay_ZhiFuKa_Status_ON = so["OnlinePay_007Ka_Status_ON"].ToBoolean(false);
        bool OnlinePay_YeePay_Status_ON = so["OnlinePay_YeePay_Status_ON"].ToBoolean(false);
        bool OnlinePay_CnCard_Status_ON = so["OnlinePay_CnCard_Status_ON"].ToBoolean(false);

        if (!OnlinePay_ChinaUnion_Status_ON)
        {
            td_ChinaUnion.Visible = false;
            td_ChinaUnion1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "Send_ChinaUnion.aspx?BuyID=" + BuyID.ToString();
            }
        }

        if (!OnlinePay_Alipay_Status_ON)
        {
            td_Alipay.Visible = false;
            td_Alipay1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "Send_Alipay.aspx?BuyID=" + BuyID.ToString();
            }
        }
        
        if(!OnlinePay_99Bill_Status_ON)
        {
            td_99Bill.Visible = false;
            td_99Bill1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "Send_99Bill.aspx?BuyID=" + BuyID.ToString();
            }
        }

        if (!OnlinePay_Tenpay_Status_ON)
        {
            td_CFT.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "Send_CFT.aspx?BuyID=" + BuyID.ToString();
            }
        }

        if (!OnlinePay_YeePay_Status_ON)
        {
            td_Yeepay.Visible = false;
            td_Yeepay1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "Send_YeePay.aspx?BuyID=" + BuyID.ToString();
            }
        }

        if (!OnlinePay_ZhiFuKa_Status_ON)
        {
            td_SZX.Visible = false;
            td_SZX1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "../007ka/Default.aspx?BuyID=" + BuyID.ToString();
            }
        }

        if (!OnlinePay_CnCard_Status_ON)
        {
            td_CnCrad.Visible = false;
            td_CnCrad1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "../CnCard/Send.aspx?BuyID=" + BuyID.ToString();
            }
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;

        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAllowPageCache = false;

        base.OnLoad(e);
    }

    #endregion
}