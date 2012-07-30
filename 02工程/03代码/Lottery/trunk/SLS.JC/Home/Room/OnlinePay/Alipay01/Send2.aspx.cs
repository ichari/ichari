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

public partial class Home_Room_OnlinePay_Alipay01_Send2 : RoomPageBase
{
    private string gateway = "";
    private string service = "";
    private string partner = "";
    private string sign_type = "";
    private string out_trade_no = "";
    private string subject = "";
    private string body = "";
    private string payment_type = "";
    private string total_fee = "";
    private string show_url = "";
    private string seller_email = "";
    private string key = "";
    private string return_url = "";
    private string _input_charset = "";
    private string notify_url = "";
    private string buyer_email = "";
    private string paymethod = "";

    public double Money = 0;

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Alipay_Status_ON"].ToBoolean(false);

        int OnlinePayType = Shove._Web.WebConfig.GetAppSettingsInt("OnlinePayType", 2);

        if (OnlinePayType == 2)
        {
            Response.Redirect("../Alipay02/Default.aspx", true);

            return;
        }

        double Money = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("PayMoney"), 0);
        string BankCode = Shove._Web.Utility.GetRequest("BankCode");

        if (!IsPostBack)
        {
            if (_User.Competences.CompetencesList.IndexOf(Competences.Administrator) > 0)
            {
                if (Money < 0.01)
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"请输入正确的充值金额！再提交，谢谢！\"); window.close();</script>");
                    //Shove._Web.JavaScript.Alert(this.Page, "请输入正确的充值金额！");

                    return;
                }
            }
            else
            {
                if (Money < 1)
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"请输入正确的充值金额！再提交，谢谢！\"); window.close();</script>");

                    return;
                }
            }

            double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
            double FormalitiesFees = Math.Round(Money * FormalitiesFeesScale, 2);

            Money += FormalitiesFees;

            //业务参数赋值；
            gateway = "https://www.alipay.com/cooperate/gateway.do?";	                //支付接口
            service = "create_direct_pay_by_user";
            sign_type = "MD5";
            payment_type = "1";                                                          //支付类型
            _input_charset = "utf-8";                                                    //编码类型
            return_url = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Alipay01/Receive.aspx"; //服务器通知返回接口
            notify_url = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Alipay01/AlipayNotify.aspx";  //服务器通知返回接口(暂时不用
            partner = so["OnlinePay_Alipay_UserNumber"].ToString("");  //卖家商户号
            show_url = "http://www.alipay.com";
            seller_email = so["OnlinePay_Alipay_UserName"].ToString("");//卖家支付宝名称(邮箱)
            key = so["OnlinePay_Alipay_MD5Key"].ToString("");
            paymethod = "bankPay";   //赋值:bankPay(网银);cartoon(卡通); directPay(余额) 三种付款方式都要，参数为空

            buyer_email = _User.Email;

            //构造商品名称
            subject = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString());	                //商品名称,作为加密的用户ID用

            body = "TicketMoney";                                                             //商品描述
            double PayMoney = Convert.ToDouble(Money.ToString());
            total_fee = PayMoney.ToString();                                             //总金额 0.01～50000.00     

            long NewPayNumber = -1;
            string ReturnDescription = "";

            if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "Alipay", (PayMoney - FormalitiesFees), FormalitiesFees, ref NewPayNumber, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if ((NewPayNumber < 0) || (ReturnDescription != ""))
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;
            }

            out_trade_no = NewPayNumber.ToString();

            if (BankCode.ToLower() == "alipay")
            {
                AlipayPay();
            }
            else
            {
                BankClick(BankCode);
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

    protected void BankClick(string BankCode)
    {
        Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();

        string aliay_url = ap.CreatUrl(
            gateway,
            service,
            partner,
            return_url,
            notify_url,
            out_trade_no,
            subject,
            payment_type,
            total_fee,
            seller_email,
            key,
            _input_charset,
            sign_type,
            "body",
            body,
            "defaultbank",
            BankCode,
            "paymethod",
            paymethod
            );

        if (aliay_url == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "地址构建出现错误");

            return;
        }

        this.Response.Write("<script language='javascript'>window.top.location.href='" + aliay_url + "'</script>");
    }

    protected void AlipayPay()
    {
        Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();

        string aliay_url = ap.CreatUrl(
            gateway,
            service,
            partner,
            return_url,
            notify_url,
            out_trade_no,
            subject,
            payment_type,
            total_fee,
            seller_email,
            key,
            _input_charset,
            sign_type,
            "body",
            body,
            "show_url",
            show_url,
            "paymethod",
            paymethod
            );

        if (aliay_url == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "地址构建出现错误");

            return;
        }

        this.Response.Write("<script language='javascript'>window.top.location.href='" + aliay_url + "'</script>");
    }
}