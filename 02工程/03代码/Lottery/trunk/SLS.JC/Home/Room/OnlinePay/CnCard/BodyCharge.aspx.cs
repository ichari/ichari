using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Globalization;

public partial class Home_Room_OnlinePay_CnCard_BodyCharge : System.Web.UI.Page
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

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_OnlinePay_CnCard_BodyCharge), this.Page);

        bool OnlinePay_CnCard_Status_ON = so["OnlinePay_CnCard_Status_ON"].ToBoolean(false);// && PF.ValidCert(so["OnlinePay_CnCard_UserNumber"].ToString(), so["OnlinePay_CnCard_ON_Cert"].ToString(), "SPAYC");

        if (!OnlinePay_CnCard_Status_ON)
        {
            Response.Write("暂未启用");
            Response.End();
            return;
        }

        BindInfo();
    }

    private void BindInfo()
    {
        System.DateTime dt = System.DateTime.Now;     
        c_memo2 = "";

        c_mid = so["OnlinePay_CnCard_UserNumber"].ToString("");
        c_ymd = dt.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);

        c_moneytype = "0";  //人民币
        c_retflag = "1";    //商户订单支付成功后是否需要返回商户指定的文件，0：不用返回 1：需要返回
        c_paygate = "";     //如果在商户网站选择银行则设置该值，具体值可参见《云网支付@网技术接口手册》附录一；如果来云网支付@网选择银行此项为空值。
        c_returl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/CnCard/Receive.aspx";
        c_language = "0";   //对启用了国际卡支付时，可使用该值定义消费者在银行支付时的页面语种，值为：0银行页面显示为中文/1银行页面显示为英文
        notifytype = "1";   //0普通通知方式/1服务器通知方式，空值为普通通知方式
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetUserInfoByName(string UserName)
    {
        if (string.IsNullOrEmpty(UserName))
        {
            return "";
        }

        Users u = new Users(1);

        u.Name = UserName;

        string ReturnDescption = "";

        int Result = u.GetUserInformationByName(ref ReturnDescption);

        if (Result < 0 || ReturnDescption != "")
        {
            return "获取会员信息错误!";
        }

        if (ReturnDescption != "")
        {
            return ReturnDescption;
        }

        return u.ID.ToString();
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetNewPayNumber(string UserID, string PayMoney)
    {
        if (string.IsNullOrEmpty(UserID))
        {
            return "";
        }

        long _UserID = 0;

        try
        {
            _UserID = long.Parse(UserID);
        }
        catch
        {
            return "";
        }

        double Money = 0;

        try
        {
            Money = double.Parse(PayMoney);
        }
        catch
        {
            return "";
        }

        long PayNumber = 0;
        string Return = "";

        if (DAL.Procedures.P_GetNewPayNumber(1, _UserID, "CnCard", Money, 0, ref PayNumber, ref Return) < 0)
        {
            return "";
        }

        if ((PayNumber < 0) || (Return != ""))
        {
            return "";
        }

        return PayNumber + "|" + Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), UserID);
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string GetSign(string c_mid, string c_order, string c_orderamount, string c_ymd, string c_moneytype, string c_retflag, string c_returl, string c_paygate,
        string c_memo1, string c_memo2, string notifytype, string c_language)
    {
        string srcStr = c_mid + c_order + c_orderamount + c_ymd + c_moneytype + c_retflag + c_returl + c_paygate + c_memo1 + c_memo2 + notifytype + c_language + so["OnlinePay_CnCard_MD5"].ToString("");

        c_signstr = FormsAuthentication.HashPasswordForStoringInConfigFile(srcStr, "MD5").ToLower();

        return c_signstr;
    }
}
