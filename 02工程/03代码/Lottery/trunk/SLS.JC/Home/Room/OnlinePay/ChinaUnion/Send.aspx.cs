using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

using com.unionpay.upop.sdk;

public partial class Home_Room_OnlinePay_ChinaUnion_Send : RoomPageBase
{

    private string bargainor_id = "";                                                               // 商户号（替换为自已的商户号）

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    private string paygateurl = "https://www.tenpay.com/cgi-bin/v1.0/pay_gate.cgi";                 // 财付通支付网关URL

    private string return_url = "";                                                                 // 支付结果回跳页面,推荐使用ip地址的方式(最长255个字符)

    private const int cmdno = 1;                                                                    // 支付命令.1

    private int fee_type = 1;                                                                       // 费用类型,现在暂只支持 1:人民币

    private string date = DateTime.Now.ToString("yyyyMMdd");

    private string sp_billno = "";                                                                  //商户订单号,10位正整数

    private long total_fee = 0;                                                                     // 订单金额,以分为单位

    private string transaction_id = "";                                                             // 交易单号,商户号(10)+支付日期(8)+商户订单号(10,不足的话左补0)=28位.

    private string desc = "";                                                                       // 商品名称

    private string attach = "";                                                                     // 指令标识,每次指令都会有这个字段,财付通在处理完成后会原样返回.

    private string purchaser_id = "";                                                               // 买家财付通帐号

    private string spbill_create_ip = "";                                                           // 用户ip(当在服务器上再设置)


    double Money = 0;                                                                               //充值金额
    string bankPay = "0";                                                                           //充值类型
    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {

        //bool OnlinePay_Alipay_Status_ON = so["OnlinePay_Tenpay_Status_ON"].ToBoolean(false);

        Money = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("PayMoney"), 0);
        bankPay = Shove._Web.Utility.GetRequest("bankPay");

    
        if (!IsPostBack)
        {
            //管理员和会员的充值的最低金额不一样
            if (_User.Competences.CompetencesList.IndexOf(Competences.Administrator) > 0)
            {
                if (Money < 0.01)
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"请输入正确的充值金额！再提交，谢谢！\"); window.close();</script>");

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

            //Money = 0.01;
            

            //手续费用
            double FormalitiesFeesScale = so["OnlinePay_ChinaUnion_CommissionScale"].ToDouble(0) / 100;
            double FormalitiesFees = Math.Round(Money * FormalitiesFeesScale, 2);

            //最后的充值额
            Money += FormalitiesFees;

            //卖家商户号
            bargainor_id = so["OnlinePay_ChinaUnion_UserName"].Value.ToString();

            //卖家商户key
            key = so["OnlinePay_ChinaUnion_MD5"].Value.ToString();

            //商品名称
            desc = HttpUtility.UrlEncode("预付款", Encoding.GetEncoding("GBK"));

            //商品总金额,以分为单位（传过的是以元为单位）.
            total_fee = long.Parse((Money * 100).ToString());

            //买家帐号
            purchaser_id = "";

            //商户后台回调url
            return_url = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/ChinaUnion/Receive.aspx";
            
            //交易标识(用户ID+投注ID+充值方式编号)
            attach = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "|" + bankPay + "|" + Shove._Web.Utility.GetRequest("BuyID"));
            
            double PayMoney = Convert.ToDouble(Money.ToString());
            long NewPayNumber = -1;
            string ReturnDescription = "";

            //产生内部充值编号
            if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "TENPAY_" + bankPay, (PayMoney - FormalitiesFees), FormalitiesFees, ref NewPayNumber, ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if ((NewPayNumber < 0) || (ReturnDescription != ""))
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;
            }

            //商户订单号（内部充值编号）
            sp_billno = NewPayNumber.ToString();

            //財付通交易号,需保证此订单号每天唯一,切不能重复！
            transaction_id = CreatePayNumber(NewPayNumber);

            //用户IP(暂停)
            spbill_create_ip = "";// Page.Request.UserHostAddress;

            #region 验证参数是否齐全

            if (string.IsNullOrEmpty(bankPay) || string.IsNullOrEmpty(desc) || string.IsNullOrEmpty(bargainor_id) || string.IsNullOrEmpty(transaction_id) || string.IsNullOrEmpty(sp_billno) || string.IsNullOrEmpty(return_url) || string.IsNullOrEmpty(attach))
            {
                Response.Write("<script type=\"text/javascript\">alert(\"参数不齐全，无法充值！\"); window.close();</script>");

                return;
            }

            #endregion


            //string url = "";
            //if (!GetPayUrl(out url))
            //{
            //    Response.Write("<script type=\"text/javascript\">alert(\"" + url + "\"); window.close();</script>");

            //    return;
            //}
            //else
            //{

            //    this.Response.Write("<script language='javascript'>window.top.location.href='" + url + "'</script>");
            //}

            UPOPSrv.LoadConf(Server.MapPath("conf.xml.config"));
            var srv = new FrontPaySrv(GenParams());
            Response.ContentEncoding = srv.Charset;
            Response.Write(srv.CreateHtml());

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

    #region 内部函数

    private Dictionary<string, string> GenParams()
    {
        var dict = new Dictionary<string, string>();
        // 交易类型，前台只支持CONSUME 和 PRE_AUTH
        dict.Add("transType", UPOPSrv.TransType.CONSUME);
        // 商品URL
        dict.Add("commodityUrl",string.Empty);
        // 商品名称
        dict.Add("commodityName","预付款");
        // 商品单价，分为单位
        dict.Add("commodityUnitPrice",Money.ToString());
        // 商品数量
        dict.Add("commodityQuantity","1");
        // 订单号，必须唯一
        dict.Add("orderNumber",sp_billno);
        // 交易金额
        dict.Add("orderAmount", total_fee.ToString());
        // 币种
        dict.Add("orderCurrency",UPOPSrv.CURRENCY_CNY);
        // 交易时间
        dict.Add("orderTime",DateTime.Now.ToString("yyyyMMddHHmmss"));
        // 用户IP
        dict.Add("customerIp",Request.UserHostAddress);
        // 前台回调URL
        var redirectUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Ok.aspx";
        dict.Add("frontEndUrl",redirectUrl);
        // 后台回调URL（前台请求时可为空）
        dict.Add("backEndUrl",return_url);

        return dict;
    }

    

    /// <summary>
    /// 产生一个交易号
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    private string CreatePayNumber(long Number)
    {
        string number = Number.ToString().PadLeft(10, '0');

        number = number.Substring(number.Length - 10);

        return bargainor_id + date + number;
    }

    /// <summary>
    /// 对字符串进行URL编码
    /// </summary>
    /// <param name="instr">待编码的字符串</param>
    /// <returns>编码结果</returns>
    private string UrlEncode(string instr)
    {
        if (instr == null || instr.Trim() == "")
            return "";
        else
        {
            return instr.Replace("%", "%25").Replace("=", "%3d").Replace("&", "%26").
                Replace("\"", "%22").Replace("?", "%3f").Replace("'", "%27").Replace(" ", "%20");
        }
    }

    /// <summary>
    /// 对字符串进行URL解码
    /// </summary>
    /// <param name="instr">待解码的字符串</param>
    /// <returns>解码结果</returns>
    private string UrlDecode(string instr)
    {
        if (instr == null || instr.Trim() == "")
            return "";
        else
        {
            return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?")
                .Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
        }
    }

    #endregion

}
