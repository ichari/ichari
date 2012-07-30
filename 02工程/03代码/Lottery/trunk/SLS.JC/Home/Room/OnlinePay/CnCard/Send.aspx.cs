using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web.Security;

public partial class Home_Room_OnlinePay_CnCard_Send : RoomPageBase
{
    protected string c_mid;
    protected string c_order;
    protected string c_orderamount;
    protected string c_ymd;
    protected string c_moneytype;
    protected string c_retflag;
    protected string c_paygate;
    protected string c_returl;
    protected string c_memo1;
    protected string c_memo2;
    protected string c_language;
    protected string notifytype;
    protected string c_signstr;

    protected string c_name;
    protected string c_address;
    protected string c_post;
    protected string c_tel;
    protected string c_email;
    SystemOptions so = new SystemOptions();
    protected void Page_Load(object sender, System.EventArgs e)
    {
        bool OnlinePay_CnCard_Status_ON = so["OnlinePay_CnCard_Status_ON"].ToBoolean(false);// && PF.ValidCert(so["OnlinePay_CnCard_UserNumber"].ToString(), so["OnlinePay_CnCard_ON_Cert"].ToString(), "SPAYC");
        if (!OnlinePay_CnCard_Status_ON)
        {
            Response.Write("暂未启用");
            Response.End();
            return;
        }

        double money = Shove._Convert.StrToDouble(Request["PayMoney"], 0);
        if (money > 0)
        {
            lbPayMoney.Text = money.ToString();

            btnNext_Click(this.Page, new EventArgs());
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Request.Url.AbsolutePath;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindInfo()
    {
        System.DateTime dt = System.DateTime.Now;

        c_name = _User.Name;
        c_address = _User.Address;
        c_post = ""; //user.PostCode;
        c_tel = _User.Telephone;
        c_email = _User.Email;

        c_memo1 = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString());            //私有参数,放的充值的用户ID
        c_memo2 = "";

        c_mid = so["OnlinePay_CnCard_UserNumber"].ToString("");
        c_ymd = dt.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);

        double PayMoney = Convert.ToDouble(this.lbPayMoney.Text.Trim());
        c_orderamount = PayMoney.ToString();

        long PayNumber = 0;
        string Return = "";

        double CommissionScale = so["OnlinePay_CnCard_CommissionScale"].ToDouble(0) / 100;
        double Commission = Math.Round(PayMoney * CommissionScale, 2);

        if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "CnCard", PayMoney, Commission, ref PayNumber, ref Return) < 0)
        {
            PF.GoError(-333, "数据读取错误。", "Home_Room_OnlinePay_CnCard_Send.aspx.cs");
            return;
        }
        if ((PayNumber < 0) || (Return != ""))
        {
            PF.GoError(-333, Return, "Home_Room_OnlinePay_CnCard_Send.aspx.cs");
            return;
        }

        c_order = PayNumber.ToString();
        c_moneytype = "0";  //人民币
        c_retflag = "1";    //商户订单支付成功后是否需要返回商户指定的文件，0：不用返回 1：需要返回
        c_paygate = "";     //如果在商户网站选择银行则设置该值，具体值可参见《云网支付@网技术接口手册》附录一；如果来云网支付@网选择银行此项为空值。
        c_returl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/CnCard/Receive.aspx";
        c_language = "0";   //对启用了国际卡支付时，可使用该值定义消费者在银行支付时的页面语种，值为：0银行页面显示为中文/1银行页面显示为英文
        notifytype = "1";   //0普通通知方式/1服务器通知方式，空值为普通通知方式

        string srcStr = c_mid + c_order + c_orderamount + c_ymd + c_moneytype + c_retflag + c_returl + c_paygate + c_memo1 + c_memo2 + notifytype + c_language + so["OnlinePay_CnCard_MD5"].ToString("");
        c_signstr = FormsAuthentication.HashPasswordForStoringInConfigFile(srcStr, "MD5").ToLower();
    }

    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        double money = 0;

        money = Shove._Convert.StrToInt(this.PayMoney.Text.Trim(), 0);
        if (money < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！");
            return;
        }

        this.PayMoney.Text = money.ToString();

        this.PayMoney.Enabled = false;

        BindInfo();

        this.Panel1.Visible = true;
        Panel2.Visible = false;
    }
}