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
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

public partial class Home_Room_OnlinePay_99Bill_Send : RoomPageBase
{
    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;

        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAllowPageCache = false;

        base.OnLoad(e);
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            double payMoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("PayMoney"), 0);
            string bankPay = Shove._Web.Utility.GetRequest("bankPay");
            string buyID = Shove._Web.Utility.GetRequest("BuyID");
            SystemOptions so = new SystemOptions();

            //管理员和会员的充值的最低金额不一样
            if (_User.Competences.CompetencesList.IndexOf(Competences.Administrator) > 0)
            {
                if (payMoney < 0.01)
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"请输入正确的充值金额！再提交，谢谢！\"); window.close();</script>");

                    return;
                }
            }
            else
            {
                if (payMoney < 0.01)
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"请输入正确的充值金额！再提交，谢谢！\"); window.close();</script>");

                    return;
                }
            }


            //手续费用
            double formalitiesFeesScale = so["OnlinePay_99Bill_PayFormalitiesFeesScale"].ToDouble(0) / 100;
            double formalitiesFees = Math.Round(payMoney * formalitiesFeesScale, 2);

            if (formalitiesFeesScale > 0.09)//手续费比例有误
            {
                PF.GoError(ErrorNumber.DataReadWrite, "手续费比例有误", this.GetType().BaseType.FullName);
                return;
            }
            //最后的充值额
            payMoney += formalitiesFees;

            long newPayNumber = -1;
            string returnDescription = "";
            //产生内部充值编号
            if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "99Bill_" + bankPay, (payMoney - formalitiesFees), formalitiesFees, ref newPayNumber, ref returnDescription) < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if ((newPayNumber < 0) || (returnDescription != ""))
            {
                PF.GoError(ErrorNumber.Unknow, returnDescription, this.GetType().BaseType.FullName);

                return;
            }

            //快钱接口请求参数
            string inputCharset = "";
            string pageUrl = "";
            string bgUrl = "";
            string version = "";
            string language = "";
            string signType = "";
            string merchantAcctId = "";

            string payerName = "";
            string payerContactType = "";
            string payerContact = "";
            string orderId = "";
            string orderAmount = "";
            string orderTime = "";
            string productName = "";

            string productNum = "";
            string productId = "";
            string productDesc = "";
            string ext1 = "";

            string ext2 = "";
            string payType = "";
            string bankId = "";
            string redoFlag = "";
            string pid = "";
            string signMsg = "";


            //人民币网关密钥
            ///区分大小写.请与快钱联系索取
            string key = so["OnlinePay_99Bill_MD5Key"].Value.ToString();

            //字符集.固定选择值。可为空。
            ///只能选择1、2、3.
            ///1代表UTF-8; 2代表GBK; 3代表gb2312
            ///默认值为1
            inputCharset = "1";

            //接受支付结果的页面地址.与[bgUrl]不能同时为空。必须是绝对地址。
            ///如果[bgUrl]为空，快钱将支付结果Post到[pageUrl]对应的地址。
            ///如果[bgUrl]不为空，并且[bgUrl]页面指定的<redirecturl>地址不为空，则转向到<redirecturl>对应的地址
            pageUrl = "";

            //服务器接受支付结果的后台地址.[bgUrl]与[pageUrl]不能同时为空。必须是绝对地址。
            ///快钱通过服务器连接的方式将交易结果发送到[bgUrl]对应的页面地址，在商户处理完成后输出的<result>如果为1，页面会转向到<redirecturl>对应的地址。
            ///如果快钱未接收到<redirecturl>对应的地址，快钱将把支付结果post到[pageUrl]对应的页面。
            bgUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/99Bill/Receive.aspx";

            //网关版本.固定值
            ///快钱会根据版本号来调用对应的接口处理程序。
            ///本代码版本号固定为v2.0
            version = "v2.0";

            //语言种类.固定选择值。
            ///只能选择1、2、3
            ///1代表中文；2代表英文
            ///默认值为1
            language = "1";

            //签名类型.固定值
            ///1代表MD5签名
            ///当前版本固定为1
            signType = "1";

            //人民币网关账户号
            ///请登录快钱系统获取用户编号，用户编号后加01即为人民币网关账户号。
            merchantAcctId = so["OnlinePay_99Bill_UserNumber"].Value.ToString();

            //支付人姓名
            ///可为中文或英文字符
            payerName = _User.Name;

            //支付人联系方式类型.固定选择值
            ///只能选择1
            ///1代表Email
            payerContactType = "1";

            //支付人联系方式
            ///只能选择Email或手机号
            payerContact = "";

            //商户订单号
            ///由字母、数字、或[-][_]组成
            orderId = newPayNumber.ToString();//内部充值编号

            //订单金额
            ///以分为单位，必须是整型数字
            ///比方2，代表0.02元
            ///商品总金额,以分为单位（传过的是以元为单位）.
            long total_fee = long.Parse((payMoney * 100).ToString());//!!!!!!!!!!!!!!!!!!!!!------注意单位是分----!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            orderAmount = total_fee.ToString();

            //订单提交时间
            ///14位数字。年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]
            ///如；20080101010101
            orderTime = DateTime.Now.ToString("yyyyMMddHHmmss");

            //商品名称
            ///可为中文或英文字符
            productName = "购彩预付款";

            //商品数量
            ///可为空，非空时必须为数字
            productNum = "1";

            //商品代码
            ///可为字符或者数字
            productId = "200599";

            //商品描述
            productDesc = "";

            //扩展字段1
            ///在支付结束后原样返回给商户
            ///交易标识(用户ID+投注ID+充值方式编号)
            string attachInfo = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "|" + bankPay + "|" + buyID);
            ext1 = attachInfo;

            //扩展字段2
            ///在支付结束后原样返回给商户
            ext2 = "";

            //支付方式.固定选择值
            ///只能选择00、10、11、12、13、14
            ///00：组合支付（网关支付页面显示快钱支持的各种支付方式，推荐使用）10：银行卡支付（网关支付页面只显示银行卡支付）.11：电话银行支付（网关支付页面只显示电话支付）.12：快钱账户支付（网关支付页面只显示快钱账户支付）.13：线下支付（网关支付页面只显示线下支付方式）.14：B2B支付（网关支付页面只显示B2B支付，但需要向快钱申请开通才能使用）
            payType = "00";

            //银行代码
            ///实现直接跳转到银行页面去支付,只在payType=10时才需设置参数
            ///具体代码参见 接口文档银行代码列表
            bankId = "";

            if (bankPay != "KQ")
            {
                //支付方式.固定选择值
                ///只能选择00、10、11、12、13、14
                ///00：组合支付（网关支付页面显示快钱支持的各种支付方式，推荐使用）10：银行卡支付（网关支付页面只显示银行卡支付）.11：电话银行支付（网关支付页面只显示电话支付）.12：快钱账户支付（网关支付页面只显示快钱账户支付）.13：线下支付（网关支付页面只显示线下支付方式）.14：B2B支付（网关支付页面只显示B2B支付，但需要向快钱申请开通才能使用）
                payType = "10";

                //银行代码
                ///实现直接跳转到银行页面去支付,只在payType=10时才需设置参数
                ///具体代码参见 接口文档银行代码列表
                bankId = bankPay;
            }

            //同一订单禁止重复提交标志
            ///固定选择值： 1、0
            ///1代表同一订单号只允许提交1次；0表示同一订单号在没有支付成功的前提下可重复提交多次。默认为0建议实物购物车结算类商户采用0；虚拟产品类商户采用1
            redoFlag = "1";

            //快钱的合作伙伴的账户号
            ///如未和快钱签订代理合作协议，不需要填写本参数
            pid = "";


            #region 验证参数是否齐全
            if (string.IsNullOrEmpty(merchantAcctId) || string.IsNullOrEmpty(ext1) || string.IsNullOrEmpty(orderId))
            {
                Response.Write("<script type=\"text/javascript\">alert(\"参数不齐全，无法充值！\"); window.close();</script>");

                return;
            }
            #endregion

            //生成加密签名串
            ///请务必按照如下顺序和规则组成加密串！
            String signMsgVal = "";
            signMsgVal = appendParam(signMsgVal, "inputCharset", inputCharset);
            signMsgVal = appendParam(signMsgVal, "pageUrl", pageUrl);
            signMsgVal = appendParam(signMsgVal, "bgUrl", bgUrl);
            signMsgVal = appendParam(signMsgVal, "version", version);
            signMsgVal = appendParam(signMsgVal, "language", language);
            signMsgVal = appendParam(signMsgVal, "signType", signType);
            signMsgVal = appendParam(signMsgVal, "merchantAcctId", merchantAcctId);
            signMsgVal = appendParam(signMsgVal, "payerName", payerName);
            signMsgVal = appendParam(signMsgVal, "payerContactType", payerContactType);
            signMsgVal = appendParam(signMsgVal, "payerContact", payerContact);
            signMsgVal = appendParam(signMsgVal, "orderId", orderId);
            signMsgVal = appendParam(signMsgVal, "orderAmount", orderAmount);
            signMsgVal = appendParam(signMsgVal, "orderTime", orderTime);
            signMsgVal = appendParam(signMsgVal, "productName", productName);
            signMsgVal = appendParam(signMsgVal, "productNum", productNum);
            signMsgVal = appendParam(signMsgVal, "productId", productId);
            signMsgVal = appendParam(signMsgVal, "productDesc", productDesc);
            signMsgVal = appendParam(signMsgVal, "ext1", ext1);
            signMsgVal = appendParam(signMsgVal, "ext2", ext2);
            signMsgVal = appendParam(signMsgVal, "payType", payType);
            signMsgVal = appendParam(signMsgVal, "bankId", bankId);
            signMsgVal = appendParam(signMsgVal, "redoFlag", redoFlag);
            signMsgVal = appendParam(signMsgVal, "pid", pid);
            signMsgVal = appendParam(signMsgVal, "key", key);

            //如果在web.config文件中设置了编码方式，例如<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>（如未设则默认为utf-8），
            //那么，inputCharset的取值应与已设置的编码方式相一致；
            //同时，GetMD5()方法中所传递的编码方式也必须与此保持一致。
            signMsg = GetMD5(signMsgVal, "utf-8").ToUpper();



            //生成调用接口的URL
            string requestURL = "";
            ///生成请求参数键值串
            requestURL = appendParam(requestURL, "inputCharset", inputCharset);
            requestURL = appendParam(requestURL, "pageUrl", pageUrl);
            requestURL = appendParam(requestURL, "bgUrl", bgUrl);
            requestURL = appendParam(requestURL, "version", version);
            requestURL = appendParam(requestURL, "language", language);
            requestURL = appendParam(requestURL, "signType", signType);
            requestURL = appendParam(requestURL, "merchantAcctId", merchantAcctId);
            requestURL = appendParam(requestURL, "payerName", HttpUtility.UrlEncode(payerName));
            requestURL = appendParam(requestURL, "payerContactType", payerContactType);
            requestURL = appendParam(requestURL, "payerContact", payerContact);
            requestURL = appendParam(requestURL, "orderId", orderId);
            requestURL = appendParam(requestURL, "orderAmount", orderAmount);
            requestURL = appendParam(requestURL, "orderTime", orderTime);
            requestURL = appendParam(requestURL, "productName", HttpUtility.UrlEncode(productName));
            requestURL = appendParam(requestURL, "productNum", productNum);
            requestURL = appendParam(requestURL, "productId", productId);
            requestURL = appendParam(requestURL, "productDesc", productDesc);
            requestURL = appendParam(requestURL, "ext1", ext1);
            requestURL = appendParam(requestURL, "ext2", ext2);
            requestURL = appendParam(requestURL, "payType", payType);
            requestURL = appendParam(requestURL, "bankId", bankId);
            requestURL = appendParam(requestURL, "redoFlag", redoFlag);
            requestURL = appendParam(requestURL, "pid", pid);

            requestURL = "https://www.99bill.com/gateway/recvMerchantInfoAction.htm?" + requestURL + "&signMsg=" + signMsg;
            new Log("OnlinePay").Write("快钱冲值请求：" + requestURL);

            this.Response.Write("<script language='javascript'>window.top.location.href='" + requestURL + "'</script>");

        }
    }


    //=====================================================================================
    //功能函数。将变量值不为空的参数组成字符串
    string appendParam(string returnStr, string paramId, string paramValue)
    {
        if (returnStr != "")
        {
            if (paramValue != "")
            {

                returnStr += "&" + paramId + "=" + paramValue;
            }
        }
        else
        {
            if (paramValue != "")
            {
                returnStr = paramId + "=" + paramValue;
            }
        }

        return returnStr;
    }
    //功能函数。将变量值不为空的参数组成字符串。结束
    //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。开始
    private static string GetMD5(string dataStr, string codeType)
    {
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(System.Text.Encoding.GetEncoding(codeType).GetBytes(dataStr));
        System.Text.StringBuilder sb = new System.Text.StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }
    //功能函数。将字符串进行编码格式转换，并进行MD5加密，然后返回。结束
}
