using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_OnlinePay_Alipay02_Send_99Bill : RoomPageBase
{
    public string Balance;
    public string UserName;
    public string BankName;

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

        if (radKQ.Checked)
        {
            BankName = "99Bill";
            hdBankCode.Value = "KQ";
        }
        else if (radICBCB2C.Checked)
        {
            BankName = "hsyh";
            hdBankCode.Value = "ICBC";
        }
        else if (radGDB.Checked)
        {
            BankName = "gdfzyh";
            hdBankCode.Value = "GDB";
        }
        else if (radGDYH.Checked)
        {
            BankName = "gdyh";
            hdBankCode.Value = "CEB";
        }
        else if (radCCB.Checked)
        {
            BankName = "jsyh";
            hdBankCode.Value = "CCB";
        }
        else if (radCOMM.Checked)
        {
            BankName = "jtyh";
            hdBankCode.Value = "BCOM";
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

        hlOK.NavigateUrl = "../99Bill/Send.aspx?PayMoney=" + Money + "&bankPay=" + this.hdBankCode.Value + "&BuyID=" + BuyID.ToString();

        pnlFirst.Visible = false;
        pnlSecond.Visible = true;
    }

    protected void lbReturn_Click(object sender, EventArgs e)
    {
        pnlFirst.Visible = true;
        pnlSecond.Visible = false;
    }
}