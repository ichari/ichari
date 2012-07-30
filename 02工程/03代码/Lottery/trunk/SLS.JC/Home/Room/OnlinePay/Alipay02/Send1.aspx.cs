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

public partial class Home_Room_OnlinePay_Alipay02_Send1 : RoomPageBase
{
    public string Balance;
    public string UserName;
    public string BankName;

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
    public long BuyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
      
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_OnlinePay_Alipay02_Send1), this.Page);

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
        BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), 0);
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

    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        string Money = this.PayMoney.Text;

        if (Shove._Convert.StrToDouble(Money, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！再提交，谢谢！");
            return;
        }

        if (Shove._Convert.StrToDouble(Money, 0) < 2)
        {
            Shove._Web.JavaScript.Alert(this.Page, "存入金额最少2元, 请输入正确的充值金额！再提交，谢谢！");
            return;
        }

        lbPayMoney.Text = this.PayMoney.Text;

        if (radZFB.Checked)
        {
            BankName = "zfb";
            hdBankCode.Value = "alipay";
        }
        else if (radCFT.Checked)
        {
            BankName = "cft";
            hdBankCode.Value = "0";
        }
        else if (radICBCB2C.Checked)
        {
            BankName = "hsyh";
            //hdBankCode.Value = "ICBCB2C";
            hdBankCode.Value = "1002";
        }
        else if (radGDB.Checked)
        {
            BankName = "gdfzyh";
            //hdBankCode.Value = "GDB";
            hdBankCode.Value = "GDB";
        }
        else if (radGDYH.Checked)
        {
            BankName = "gdyh";
            //hdBankCode.Value = "CEBBANK";
            hdBankCode.Value = "1022";
        }
        else if (radCCB.Checked)
        {
            BankName = "jsyh";
            //hdBankCode.Value = "CCB";
            hdBankCode.Value = "1003";
        }
        else if (radCOMM.Checked)
        {
            BankName = "jtyh";
            //hdBankCode.Value = "COMM";
            hdBankCode.Value = "1020";
        }
        else if (radABC.Checked)
        {
            BankName = "nyyh";
            //hdBankCode.Value = "ABC";
            hdBankCode.Value = "1005";
        }
        else if (radSPDB.Checked)
        {
            BankName = "shpd";
            //hdBankCode.Value = "SPDB";
            hdBankCode.Value = "1004";
        }
        else if (radSDB.Checked)
        {
            BankName = "szfzyh";
            //hdBankCode.Value = "SDB";
            hdBankCode.Value = "1008";
        }
        else if (radCIB.Checked)
        {
            BankName = "xyyh";
            //hdBankCode.Value = "CIB";
            hdBankCode.Value = "1009";
        }
        else if (radCMBC.Checked)
        {
            BankName = "zgms";
            //hdBankCode.Value = "CMBC";
            hdBankCode.Value = "1006";
        }
        else if (radBOCB2C.Checked)
        {
            BankName = "zgyh";
            //hdBankCode.Value = "BOCB2C";
            hdBankCode.Value = "BOCB2C";
        }
        else if (radCMB.Checked)
        {
            BankName = "zsyhj";
            //hdBankCode.Value = "CMB";
            hdBankCode.Value = "1001";
        }
        else if (radBCCBEB.Checked)
        {
            BankName = "bjyh";
            //hdBankCode.Value = "CITIC";
            hdBankCode.Value = "1032";
        }
        else if (radszx.Checked)
        {
            Response.Redirect("../ZhiFuKa/default.aspx?cardno=szx&BuyID=" + BuyID.ToString());

            return;
        }
        else if (radzfk.Checked)
        {
            Response.Redirect("../ZhiFuKa/default.aspx?cardno=zfk&BuyID=" + BuyID.ToString());
            return;
        }
        else if (rad007Ka.Checked)
        {
            Response.Redirect("../007ka/default.aspx?cardno=007Ka&BuyID="+BuyID.ToString());
            return;
        }
       

        if (hdBankCode.Value == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择银行，谢谢！");
            return;
        }



        if (Shove._Convert.StrToInt(hdBankCode.Value, -1) == -1)
        {
            hlOK.NavigateUrl = "Send2.aspx?PayMoney=" + Money + "&bankPay=" + this.hdBankCode.Value + "&BuyID=" + BuyID.ToString();
        }
        else
        {

            hlOK.NavigateUrl = "../Tenpay/Send.aspx?PayMoney=" + Money + "&bankPay=" + this.hdBankCode.Value + "&BuyID=" + BuyID.ToString();
        }

        pnlFirst.Visible = false;
        pnlSecond.Visible = true;
    }

    protected void lbReturn_Click(object sender, EventArgs e)
    {
        pnlFirst.Visible = true;
        pnlSecond.Visible = false;
    }

}