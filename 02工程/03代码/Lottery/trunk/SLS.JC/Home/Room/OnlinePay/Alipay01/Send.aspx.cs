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

using Shove.Alipay;

public partial class Home_Room_OnlinePay_Alipay01_Send : RoomPageBase
{
    public string Balance;
    public string UserName;

    public string OnlinePay_99Bill_HomePage = "", OnlinePay_99Bill_Status = "", OnlinePay_99Bill_Target = "_self";
    public string OnlinePay_CBPayMent_HomePage = "", OnlinePay_CBPayMent_Status = "", OnlinePay_CBPayMent_Target = "_self";
    public string OnlinePay_Tenpay_HomePage = "", OnlinePay_Tenpay_Status = "", OnlinePay_Tenpay_Target = "_self";
    public string OnlinePay_Alipay_HomePage = "", OnlinePay_Alipay_Status = "", OnlinePay_Alipay_Target = "_self";
    public string OnlinePay_CnCard_HomePage = "", OnlinePay_CnCard_Status = "", OnlinePay_CnCard_Target = "_self";
    public string OnlinePay_ICBC_HomePage = "", OnlinePay_ICBC_Status = "", OnlinePay_ICBC_Target = "_self";
    public string OnlinePay_CMBChina_HomePage = "", OnlinePay_CMBChina_Status = "", OnlinePay_CMBChina_Target = "_self";

    public double Money = 0;
    public double RealPayMoney = 0;

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
        }

        bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);

        //if (!OnlinePay_Alipay_Status_ON)
        //{
        //    PF.GoError(ErrorNumber.Unknow, "未启用此项服务", this.GetType().BaseType.FullName);

        //    return;
        //}

        int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType", 2);

        if (OnlinePayType == 2)
        {
            Response.Redirect("../Alipay02/Default.aspx", true);

            return;
        }

        //int Num = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("Num"), 0);

        //if (Num <= 0)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "电话卡的购买数量输入有误", "Default.aspx");

        //    return;
        //}

        //this.PayMoney.Enabled = false;
        //Money = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("CardType"), 0) * Num;

        Money = Shove._Convert.StrToDouble(this.PayMoney.Text, 0);

        RealPayMoney = Money;

        //if (Money <= 0)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "请输入选择电话卡的类型", "Default.aspx");

        //    return;
        //}

        //labBalance.Text = "现购卡总额：<font color='red'>" + Money.ToString("N") + "</font> 元，账户余额：<font color='red'>" + _User.Balance.ToString("N") + "</font> 元";

        double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Math.Round(Money * FormalitiesFeesScale, 2);

        Money += FormalitiesFees;
        //this.PayMoney.Enabled = true;
        this.PayMoney.Text = Money.ToString();
        //this.tbFormalitiesFees.Text = FormalitiesFees.ToString();
        //labFormalitiesFees.Text = "手续费 " + FormalitiesFees.ToString() + " 元由支付网关提供商收取。";
        //labBalance.Text += " " + (labFormalitiesFees.Text != "" ? labFormalitiesFees.Text : "");

        //this.PayMoney.Enabled = false;

        BindDataForPayList(RealPayMoney);

        string Description = "金额：" + (Money - FormalitiesFees).ToString() + " " ;
        //lab1.Text = Description;
        //lab5.Text = Description;
        //lab6.Text = Description;
        //lab7.Text = Description;
        //lab8.Text = Description;
        //lab9.Text = Description;
        //lab10.Text = Description;
        //lab11.Text = Description;
        //lab12.Text = Description;
        //lab13.Text = Description;
        //lab14.Text = Description;
        //lab15.Text = Description;
        //lab16.Text = Description;
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

    private void BindDataForPayList(double Money)
    {
        //double BillPayMoney = double.Parse(PayMoney.Text) - double.Parse(tbFormalitiesFees.Text);
        string money = Money.ToString();
        hl1.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=Alipay";
        hl2.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=Alipay";
        hl3.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=ICBCB2C";
        hl4.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=CMB";
        hl5.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=CCB";
        hl6.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=ABC";
        hl7.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=SPDB";
        hl8.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=CIB";
        hl9.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=GDB";
        hl10.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=SDB";
        hl11.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=CMBC";
        hl12.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=COMM";
        hl13.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=POSTGC";
        hl14.NavigateUrl = "Send2.aspx?PayMoney=" + Money.ToString() + "&BankCode=CITIC";
    }

    protected void btnHiddenClick_Click(object sender, EventArgs e)
    {
        BindDataForPayList(double.Parse(this.RealPayMoneyValue.Value));
    }
}