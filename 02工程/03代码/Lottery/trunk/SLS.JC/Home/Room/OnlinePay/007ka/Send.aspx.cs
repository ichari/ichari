using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

public partial class Home_Room_OnlinePay_007ka_Send : RoomPageBase
{
    //定义请求所需要的参数
    #region 
    public string type = "";        //业务类型
    public string version = "";     //版本号
    public string merchantId = "";    //商户编号
    public string meraccount = "";  //商户账号
    public string orderId = "";     //订单号  唯一标识
    public string orderDate = "";    //当前系统时间
    public string amount = "";      //金额
    public string curId = "";       //货币类型
    public string payInterfaceId = "";//接口名称
    public string productName = "";
    public string productDesc = "";
    public string reserved = "";
    public string merName = "";
    public string backUrl = "";     //响应地址
    public string hmac = "";        //签名
    public long BuyID = 0;
    string key = "";

    SystemOptions so = new SystemOptions();
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        string bankPay = Shove._Web.Utility.GetRequest("cardno");
        amount = Shove._Web.Utility.GetRequest("ordermoney");
        if (!IsPostBack)
        {
            BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), 0);
            long NewPayNumber = -1;
            string ReturnDescription = "";

            try
            {
                //产生内部充值编号
                if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "007Ka_" + bankPay, Shove._Convert.StrToDouble(amount, 0), so["OnlinePay_007Ka_FormalitiesFees"].ToDouble(0) / 100 * Shove._Convert.StrToDouble(amount, 0), ref NewPayNumber, ref ReturnDescription) < 0)
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
                orderId = NewPayNumber.ToString();
            }
            catch
            {
                new Log("OnlinePay").Write("007在线充值：充值未完成。错误描述：生成订单号出现错误.");
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，生成订单号出错，请重试", this.GetType().BaseType.FullName);

                return;
            }
            SettingParam();
            SettingURLEncode();
        }
    }

    /// <summary>
    /// 设置请求参数
    /// </summary>
    private void SettingParam()
    {
        type = "PAY";
        version = "1.0.0";
        merchantId = so["OnlinePay_007Ka_MerchantId"].ToString("").Trim();
        meraccount = so["OnlinePay_007Ka_MerAccount"].ToString("").Trim();
        orderDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        curId = "CNY";
        payInterfaceId = "SZXPAY";
        productName =HttpUtility.UrlEncode("<%=_Site.Name %>充值",Encoding.GetEncoding("GB2312"));
        productDesc = HttpUtility.UrlEncode("<%=_Site.Name %>上短信代投注系统充值", Encoding.GetEncoding("GB2312")) ;
        reserved = Shove._Web.Utility.GetRequest("BuyID");
        merName = HttpUtility.UrlEncode("<%=_Site.Url %>", Encoding.GetEncoding("GB2312"));
        backUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/007ka/Receive1.aspx";
        key = so["OnlinePay_007Ka_Key"].ToString("").Trim();
        hmac = GetMD5(GetSignString());//System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(GetSignString(), "MD5").ToUpper();
    }

    /// <summary>
    /// 生成签名字符串
    /// </summary>
    /// <returns></returns>
    private string GetSignString()
    {
        string sign = type + "|" + version + "|" + merchantId + "|" + meraccount + "|" +
            orderId + "|" + orderDate + "|" + amount + "|" + curId + "|" + payInterfaceId +
            "|" + productName + "|" + productDesc + "|" + reserved + "|" + merName + "|" +
            backUrl + "|" + key;
        return sign;
    }

    /// <summary>
    /// 参数编码
    /// </summary>
    private void SettingURLEncode()
    {
        type = Server.UrlEncode(type);
        version = Server.UrlEncode(version);
        merchantId = Server.UrlEncode(merchantId);
        meraccount = Server.UrlEncode(meraccount);
        orderId = Server.UrlEncode(orderId);
        orderDate = Server.UrlEncode(orderDate);
        amount = Server.UrlEncode(amount);
        curId = Server.UrlEncode(curId);
        payInterfaceId = Server.UrlEncode(payInterfaceId);
        productName = Server.UrlEncode(productName);
        productDesc = Server.UrlEncode(productDesc);
        reserved = Server.UrlEncode(reserved);
        merName = Server.UrlEncode(merName);
        backUrl = Server.UrlEncode(backUrl);
        hmac = Server.UrlEncode(hmac);

    }

    private string GetMD5(string encypStr)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encypStr, "MD5").ToUpper(); //  '先MD5 32 然后转大写
    }
}
