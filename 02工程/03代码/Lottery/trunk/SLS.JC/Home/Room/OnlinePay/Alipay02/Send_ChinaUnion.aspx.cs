using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Room_OnlinePay_Alipay02_Send_ChinaUnion : RoomPageBase
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

        if (BuyID > 0)
        {
            DataTable dt = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

            if (dt == null)
            {
                return;
            }

            double Money = Shove._Convert.StrToDouble(dt.Rows[0]["Money"].ToString(), 0);

            if (_User != null)
            {
                if (_User.Balance < Money)
                {
                    PayMoney.Text = (Money - _User.Balance).ToString();
                }
            }
            else
            {
                PayMoney.Text = Money.ToString();
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

    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        string Money = this.PayMoney.Text;

        if (Shove._Convert.StrToDouble(Money, 0) <= 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！再提交，谢谢！");
            return;
        }

        if (Shove._Convert.StrToDouble(Money, 0) < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "存入金额最少1元, 请输入正确的充值金额！再提交，谢谢！");
            return;
        }

        lbPayMoney.Text = this.PayMoney.Text;

        if (radChinaUnion.Checked)
        {
            BankName = "chinaunion";
            hdBankCode.Value = "chinaunion";
        }
        else if (radICBCB2C.Checked)
        {
            BankName = "hsyh";
            hdBankCode.Value = "ICBCB2C";
        }
        else if (radGDB.Checked)
        {
            BankName = "gdfzyh";
            hdBankCode.Value = "GDB";
        }
        else if (radGDYH.Checked)
        {
            BankName = "gdyh";
            hdBankCode.Value = "CEBBANK";
        }
        else if (radCCB.Checked)
        {
            BankName = "jsyh";
            hdBankCode.Value = "CCB";
        }
        else if (radCOMM.Checked)
        {
            BankName = "jtyh";
            hdBankCode.Value = "COMM";
        }
        else if (radABC.Checked)
        {
            BankName = "nyyh";
            hdBankCode.Value = "ABC";
        }
        else if (radSPDB.Checked)
        {
            BankName = "shpd";
            hdBankCode.Value = "SPDB";
        }
        else if (radSDB.Checked)
        {
            BankName = "szfzyh";
            hdBankCode.Value = "SDB";
        }
        else if (radCIB.Checked)
        {
            BankName = "xyyh";
            hdBankCode.Value = "CIB";
        }
        else if (radHZCBB2C.Checked)
        {
            BankName = "hzyh";
            hdBankCode.Value = "HZCBB2C";
        }
        else if (radCMBC.Checked)
        {
            BankName = "zgms";
            hdBankCode.Value = "CMBC";
        }
        else if (radBOCB2C.Checked)
        {
            BankName = "zgyh";
            hdBankCode.Value = "BOCB2C";
        }
        else if (radCMB.Checked)
        {
            BankName = "zsyhj";
            hdBankCode.Value = "CMB";
        }
        else if (radCITIC.Checked)
        {
            BankName = "zxyh";
            hdBankCode.Value = "CITIC";
        }
        if (hdBankCode.Value == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择银行，谢谢！");
            return;
        }

        hlOK.NavigateUrl = "../ChinaUnion/Send.aspx?PayMoney=" + Money + "&bankPay=" + this.hdBankCode.Value + "&BuyID=" + BuyID.ToString();


        pnlFirst.Visible = false;
        pnlSecond.Visible = true;
    }

    protected void lbReturn_Click(object sender, EventArgs e)
    {
        pnlFirst.Visible = true;
        pnlSecond.Visible = false;
    }
}