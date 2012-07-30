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
using System.Data.SqlClient;

using Shove.Alipay;

public partial class OnlinePay_Alipay02_SendClient : RoomPageBase
{

    public string Balance;
    public string UserName;

    public string DefaultSendPage = "";


    SystemOptions so = new SystemOptions();
    protected void Page_Load(object sender, EventArgs e)
    {
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


        bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);
        bool OnlinePay_99Bill_Status_ON = so["OnlinePay_99Bill_Status_ON"].ToBoolean(false);
        bool OnlinePay_Tenpay_Status_ON = so["OnlinePay_Tenpay_Status_ON"].ToBoolean(false);
        bool OnlinePay_ZhiFuKa_Status_ON = so["OnlinePay_007Ka_Status_ON"].ToBoolean(false);
        bool OnlinePay_YeePay_Status_ON = so["OnlinePay_YeePay_Status_ON"].ToBoolean(false);
        bool OnlinePay_CnCard_Status_ON = so["OnlinePay_CnCard_Status_ON"].ToBoolean(false);

        if (!OnlinePay_Alipay_Status_ON)
        {
            td_Alipay.Visible = false;
            td_Alipay1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "../Alipay02/Send_Alipay.aspx";
            }
        }

        if (!OnlinePay_99Bill_Status_ON)
        {
            td_99Bill.Visible = false;
            td_99Bill1.Visible = false;
        }
        else
        {
            if (DefaultSendPage == "")
            {
                DefaultSendPage = "../Alipay02/Send_99Bill.aspx";
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
                DefaultSendPage = "../Alipay02/Send_CFT.aspx";
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
                DefaultSendPage = "../Alipay02/Send_YeePay.aspx";
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
                DefaultSendPage = "../007ka/Default.aspx";
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
                DefaultSendPage = "../CnCard/Send.aspx";
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


    #region

    //protected override void OnLoad(EventArgs e)
    //{
    //    isRequestLogin = true;

    //    RequestLoginPage = this.Request.Url.AbsoluteUri;

    //    isAllowPageCache = false;

    //    base.OnLoad(e);

    //    //isRequestLogin = true;
    //    //RequestLoginPage = "OnlinePay/Alipay02/SendClinet.aspx?UN=" + Shove._Web.Utility.GetRequest("UN") + "&UP=" + Shove._Web.Utility.GetRequest("UP") + "&Sign=" + Shove._Web.Utility.GetRequest("Sign");

    //    //base.OnLoad(e);
    //}

    //private void BindDataForPayList()
    //{
    //    double Money = double.Parse(PayMoney.Text) - double.Parse(tbFormalitiesFees.Text);
    //    // AliPay
    //    //bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);
    //    //if (OnlinePay_Alipay_Status_ON)
    //    //{
    //    //    OnlinePay_Alipay_HomePage = "../Alipay/Send.aspx";
    //    //    Panel_Alipay.Visible = false;
    //    //}
    //    //else
    //    //{
    //    //    OnlinePay_Alipay_HomePage = "https://www.alipay.com/";
    //    //    OnlinePay_Alipay_Status = "(暂未开通)";
    //    //    OnlinePay_Alipay_Target = "_blank";
    //    //    Panel_Alipay.Visible = false;
    //    //}

    //    // 99Bill
    //    bool OnlinePay_99Bill_Status_ON = so["OnlinePay_99Bill_Status_ON"].ToBoolean(false);
    //    if (OnlinePay_99Bill_Status_ON)
    //    {
    //        OnlinePay_99Bill_HomePage = "../99Bill/Send.aspx?PayMoney=" + Money.ToString();
    //        Panel_99Bill.Visible = false;
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
    //        OnlinePay_Tenpay_HomePage = "../Tenpay/Send.aspx?PayMoney=" + Money.ToString();
    //        Panel_Tenpay.Visible = false;
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
    //        OnlinePay_CBPayMent_HomePage = "../CBPayMent/Send.aspx?PayMoney=" + Money.ToString();
    //        Panel_CBPayMent.Visible = false;
    //    }
    //    else
    //    {
    //        OnlinePay_CBPayMent_HomePage = "http://www.chinabank.com.cn/";
    //        OnlinePay_CBPayMent_Status = "(暂未开通)";
    //        OnlinePay_CBPayMent_Target = "_blank";
    //        Panel_CBPayMent.Visible = false;
    //    }
    //}

    //protected void OnlinePayed_Click(object sender, System.EventArgs e)
    //{
    //    double money = 0;

    //    if (_User.Competences.CompetencesList.IndexOf(Competences.Administrator) > 0)
    //    {
    //        money = Shove._Convert.StrToDouble(this.PayMoney.Text.Trim(), 0);
    //        if (money < 0.01)
    //        {
    //            this.PayMoney.Enabled = true;
    //            //OnlinePayed.Enabled = true;
    //            //bt_Submit.Enabled = false;

    //            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！");
    //            return;
    //        }
    //        else
    //        {
    //            this.PayMoney.Enabled = false;
    //            //OnlinePayed.Enabled = false;
    //            //bt_Submit.Enabled = true;
    //        }
    //    }
    //    else
    //    {
    //        money = Shove._Convert.StrToInt(this.PayMoney.Text.Trim(), 0);
    //        if (money < 1)
    //        {
    //            this.PayMoney.Enabled = true;
    //            //OnlinePayed.Enabled = true;
    //            //bt_Submit.Enabled = false;

    //            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！");
    //            return;
    //        }
    //        else
    //        {
    //            this.PayMoney.Enabled = false;
    //            //OnlinePayed.Enabled = false;
    //            //bt_Submit.Enabled = true;
    //        }
    //    }

    //    double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
    //    double FormalitiesFees = Math.Round(money * FormalitiesFeesScale, 2);

    //    money += FormalitiesFees;
    //    this.PayMoney.Enabled = true;
    //    this.PayMoney.Text = money.ToString();
    //    this.tbFormalitiesFees.Text = FormalitiesFees.ToString();
    //    labFormalitiesFees.Text = "手续费 " + FormalitiesFees.ToString() + " 元由支付网关提供商收取。";

    //    this.PayMoney.Visible = false;
    //    //OnlinePayed.Visible = false;
    //    //bt_Submit.Enabled = true;

    //    PayBankList.Visible = true;
    //    PaySend.Visible = false;
    //}

    //private void AlipayParameter()
    //{
    //    //业务参数赋值；
    //    gateway = "https://www.alipay.com/cooperate/gateway.do?";	                //支付接口
    //    service = "create_direct_pay_by_user";
    //    sign_type = "MD5";
    //    payment_type = "1";                                                          //支付类型
    //    _input_charset = "utf-8";                                                    //编码类型
    //    //return_url = "http://" + _Site.Urls.Split(',')[0] + "/OnlinePay/Alipay/Receive.aspx"; //服务器通知返回接口
    //    //notify_url = "http://" + _Site.Urls.Split(',')[0] + "/OnlinePay/Alipay/Notify.aspx";  //服务器通知返回接口(暂时不用)
    //    return_url = Shove._Web.Utility.GetUrl() + "/OnlinePay/Alipay02/ReceiveClient.aspx"; //服务器通知返回接口
    //    notify_url = Shove._Web.Utility.GetUrl() + "/OnlinePay/Alipay02/AlipayNotify.aspx";  //服务器通知返回接口(暂时不用
    //    partner = so["OnlinePay_Alipay_UserNumber"].ToString("");  //卖家商户号
    //    show_url = "http://www.alipay.com";
    //    seller_email = so["OnlinePay_Alipay_UserName"].ToString("");//卖家支付宝名称(邮箱)
    //    key = so["OnlinePay_Alipay_MD5Key"].ToString("");
    //    paymethod = "bankPay";   //赋值:bankPay(网银);cartoon(卡通); directPay(余额) 三种付款方式都要，参数为空

    //    buyer_email = _User.Email;

    //    //构造商品名称
    //    subject = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString());	                //商品名称,作为加密的用户ID用

    //    body = "预付款";                                                             //商品描述
    //    double PayMoney = Convert.ToDouble(this.PayMoney.Text.Trim());
    //    total_fee = PayMoney.ToString();                                             //总金额 0.01～50000.00     

    //    long NewPayNumber = -1;
    //    string ReturnDescription = "";

    //    //if (DAL.Procedures.P_GetNewPayNumber.Call(_Site.ID, _User.ID, "Alipay", PayMoney, Shove._Convert.StrToDouble(this.tbFormalitiesFees.Text, 0), ref NewPayNumber, ref ReturnDescription) < 0)
    //    //{
    //    //    PublicFunction.GoError(ErrorNumber.DataReadWrite, "数据读取错误。", this.GetType().BaseType.FullName);

    //    //    return;
    //    //}

    //    //if ((NewPayNumber < 0) || (ReturnDescription != ""))
    //    //{
    //    //    PublicFunction.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

    //    //    return;
    //    //}

    //    out_trade_no = NewPayNumber.ToString();
    //}

    //protected void btnClick(string BankCode)
    //{
    //    AlipayParameter();

    //    //Alipay ap = new Alipay();

    //    //string aliay_url = ap.CreatUrl(
    //    //    gateway,
    //    //    service,
    //    //    partner,
    //    //    return_url,
    //    //    notify_url,
    //    //    out_trade_no,
    //    //    subject,
    //    //    payment_type,
    //    //    total_fee,
    //    //    seller_email,
    //    //    key,
    //    //    _input_charset,
    //    //    sign_type,
    //    //    "body",
    //    //    body,
    //    //    "defaultbank",
    //    //    BankCode,
    //    //    "paymethod",
    //    //    paymethod
    //    //    );

    //    //if (aliay_url == "")
    //    //{
    //    //    Shove._Web.JavaScript.Alert(this.Page, "地址构建出现错误");

    //    //    return;
    //    //}

    //    //this.Response.Write("<script language='javascript'>window.open ('" + aliay_url + "');window.top.location.href='../Result.aspx'</script>");
    //}

    //protected void bt_Submit_Click(object sender, EventArgs e)
    //{
    //    //AlipayParameter();

    //    //Alipay ap = new Alipay();

    //    //string aliay_url = ap.CreatUrl(
    //    //    gateway,
    //    //    service,
    //    //    partner,
    //    //    return_url,
    //    //    notify_url,
    //    //    out_trade_no,
    //    //    subject,
    //    //    payment_type,
    //    //    total_fee,
    //    //    seller_email,
    //    //    key,
    //    //    _input_charset,
    //    //    sign_type,
    //    //    "body",
    //    //    body,
    //    //    "show_url",
    //    //    show_url,
    //    //    "buyer_email",
    //    //    buyer_email,
    //    //    "paymethod",
    //    //    paymethod
    //    //    );

    //    //if (aliay_url == "")
    //    //{
    //    //    Shove._Web.JavaScript.Alert(this.Page, "地址构建出现错误");

    //    //    return;
    //    //}

    //    //this.Response.Write("<script language='javascript'>window.open ('" + aliay_url + "');window.top.location.href='../Result.aspx'</script>");
    //}

    //protected void btnICBC_Click(object sender, EventArgs e)
    //{
    //    btnClick("ICBCB2C");
    //}

    //protected void btnCMB_Click(object sender, EventArgs e)
    //{
    //    btnClick("CMB");
    //}

    //protected void btnCCB_Click(object sender, EventArgs e)
    //{
    //    btnClick("CCB");
    //}

    //protected void btnABC_Click(object sender, EventArgs e)
    //{
    //    btnClick("ABC");
    //}

    //protected void btnSPDB_Click(object sender, EventArgs e)
    //{
    //    btnClick("SPDB");
    //}

    //protected void btnCIB_Click(object sender, EventArgs e)
    //{
    //    btnClick("CIB");
    //}

    //protected void btnGDB_Click(object sender, EventArgs e)
    //{
    //    btnClick("GDB");
    //}

    //protected void btnSDB_Click(object sender, EventArgs e)
    //{
    //    btnClick("SDB");
    //}

    //protected void btnCMBC_Click(object sender, EventArgs e)
    //{
    //    btnClick("CMBC");
    //}

    //protected void btnCOMM_Click(object sender, EventArgs e)
    //{
    //    btnClick("COMM");
    //}

    //protected void btnPOSTGC_Click(object sender, EventArgs e)
    //{
    //    btnClick("POSTGC");
    //}

    //protected void btnCITIC_Click(object sender, EventArgs e)
    //{
    //    btnClick("CITIC");
    //}
    #endregion
}