using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Net;

public partial class Home_Room_OnlinePay_99Bill_Receive : RoomPageBase
{

    //初始化结果及地址
    //返回给快钱的结果代码:0失败,1 成功
    public int rtnOk = 0;
    //返回给快钱的让它重定向的URL
    public string rtnUrl = "";

    SystemOptions so = new SystemOptions();

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        isAllowPageCache = false;
    
        base.OnLoad(e);
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            try
            {
                new Log("OnlinePay").Write("快钱在线支付收到通知：" + Request.RawUrl);
            }
            catch{}

            //设置人民币网关密钥(区分大小写)
            string key = so["OnlinePay_99Bill_MD5Key"].Value.ToString();
            //string myMerchantAcctId = so["OnlinePay_99Bill_UserNumber"].Value.ToString();

            # region 获取快钱支付成功后的请求信息

            //获取人民币网关账户号
            string merchantAcctId = Request["merchantAcctId"].ToString().Trim();

            //获取网关版本.固定值
            ///快钱会根据版本号来调用对应的接口处理程序。
            ///本代码版本号固定为v2.0
            string version = Request["version"].ToString().Trim();

            //获取语言种类.固定选择值。
            ///只能选择1、2、3
            ///1代表中文；2代表英文
            ///默认值为1
            string language = Request["language"].ToString().Trim();

            //签名类型.固定值
            ///1代表MD5签名
            ///当前版本固定为1
            string signType = Request["signType"].ToString().Trim();

            //获取支付方式
            ///值为：10、11、12、13、14
            ///00：组合支付（网关支付页面显示快钱支持的各种支付方式，推荐使用）10：银行卡支付（网关支付页面只显示银行卡支付）.11：电话银行支付（网关支付页面只显示电话支付）.12：快钱账户支付（网关支付页面只显示快钱账户支付）.13：线下支付（网关支付页面只显示线下支付方式）.14：B2B支付（网关支付页面只显示B2B支付，但需要向快钱申请开通才能使用）
            string payType = Request["payType"].ToString().Trim();

            //获取银行代码
            ///参见银行代码列表
            string bankId = Request["bankId"].ToString().Trim();

            //获取商户订单号
            string orderId = Request["orderId"].ToString().Trim();//存在用户冲值的充值编号,关系T_UserPayDetatil.ID

            //获取订单提交时间
            ///获取商户提交订单时的时间.14位数字。年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]
            ///如：20080101010101
            string orderTime = Request["orderTime"].ToString().Trim();

            //获取原始订单金额
            ///订单提交到快钱时的金额，单位为分。
            ///比方2 ，代表0.02元
            string orderAmount = Request["orderAmount"].ToString().Trim();

            //获取快钱交易号
            ///获取该交易在快钱的交易号
            string dealId = Request["dealId"].ToString().Trim();

            //获取银行交易号
            ///如果使用银行卡支付时，在银行的交易号。如不是通过银行支付，则为空
            string bankDealId = Request["bankDealId"].ToString().Trim();

            //获取在快钱交易时间
            ///14位数字。年[4位]月[2位]日[2位]时[2位]分[2位]秒[2位]
            ///如；20080101010101
            string dealTime = Request["dealTime"].ToString().Trim();

            //获取实际支付金额
            ///单位为分
            ///比方 2 ，代表0.02元
            string payAmount = Request["payAmount"].ToString().Trim();//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!-----注意单位为(分)---------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //获取交易手续费
            ///单位为分
            ///比方 2 ，代表0.02元
            string fee = Request["fee"].ToString().Trim();

            //获取扩展字段1; 
            string ext1 = Request["ext1"].ToString().Trim();//这里存放用户ID  加密后信息

            //获取扩展字段2
            string ext2 = Request["ext2"].ToString().Trim();

            //获取处理结果
            ///10代表 成功; 11代表 失败
            string payResult = Request["payResult"].ToString().Trim();

            //获取错误代码
            ///详细见文档错误代码列表
            string errCode = Request["errCode"].ToString().Trim();

            //获取加密签名串
            string signMsg = Request["signMsg"].ToString().Trim();

            #endregion

            # region 组成MD5加密串
            //生成加密串。必须保持如下顺序。
            string merchantSignMsgVal = "";
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "merchantAcctId", merchantAcctId);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "version", version);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "language", language);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "signType", signType);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "payType", payType);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "bankId", bankId);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "orderId", orderId);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "orderTime", orderTime);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "orderAmount", orderAmount);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "dealId", dealId);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "bankDealId", bankDealId);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "dealTime", dealTime);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "payAmount", payAmount);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "fee", fee);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "ext1", ext1);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "ext2", ext2);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "payResult", payResult);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "errCode", errCode);
            merchantSignMsgVal = appendParam(merchantSignMsgVal, "key", key);

            //如果在web.config文件中设置了编码方式，例如<globalization requestEncoding="utf-8" responseEncoding="utf-8"/>（如未设则默认为utf-8），
            //那么，GetMD5()方法中所传递的编码方式也必须与此保持一致。
            string merchantSignMsg = GetMD5(merchantSignMsgVal, "utf-8");

            #endregion

            #region 处理结果

            //商家进行数据处理，并跳转会商家显示支付结果的页面
            ///首先进行签名字符串验证
            if (signMsg.ToUpper() != merchantSignMsg.ToUpper())//签名验证失败
            {
                try
                {
                    new Log("OnlinePay").Write("快钱在线支付：校验出错！快钱支付返回时请求信息：" + merchantSignMsgVal + "  生成校验码：" + merchantSignMsg + "返回校验码：" + signMsg + " 支付号：" + orderId);
                    this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx'</script>");
                }
                catch { }
                return;
            }
            else//签名验证成功
            {
                if (payResult == "11")//支付失败
                {
                }
                else if (payResult == "10")//支付成功
                {
                    /*  
                     ' 商户网站逻辑处理，比方更新订单支付状态为成功
                    ' 特别注意：只有signMsg.ToUpper() == merchantSignMsg.ToUpper()，且payResult=10，才表示支付成功！同时将订单金额与提交订单前的订单金额进行对比校验。
                    */
                    //从扩展参数ext1中进行解密,获取请求时的冲保用户ID信息
                    string[] userInfos = (Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), ext1)).Split('|');
                    long userID = Shove._Convert.StrToLong(userInfos[0], -1);
                    string bankPay = userInfos[1];
                    long buyId = Shove._Convert.StrToLong(userInfos[2], -1);

                    double payMoney = Shove._Convert.StrToDouble(payAmount, 0) / 100.0; //把快钱返回的实际支付金额单位为(分)转为(元)



                    Users user;
                    user = new Users(_Site.ID)[_Site.ID, userID];


                    if (user == null)
                    {
                        try
                        {
                            new Log("OnlinePay").Write("快钱在线支付：异常用户数据！" + " 支付号：" + orderId);
                        }
                        catch { }
                        //this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx'</script>");
                        rtnOk = 0;
                        rtnUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx";
                        return;
                    }

                    //if (_User == null)
                    //{
                    //    _User = new Users(_Site.ID)[_Site.ID, user.ID];

                    //    string ReturnDescription = "";
                    //    _User.LoginDirect(ref ReturnDescription);
                    //}

                    //用户账户操作
                    if (WriteUserAccount(user, orderId, payMoney, "系统交易号：" + orderId + " 快钱的交易号：" + dealId))
                    {
                        long buyID = 0;

                        if (this.Request.Url.AbsoluteUri.IndexOf("?BuyID") > 0 && this.Request.Url.AbsoluteUri.IndexOf("&") > 0)
                        {
                            buyID = Shove._Convert.StrToLong(HttpUtility.UrlDecode(this.Request.Url.AbsoluteUri).Split('?')[1].Split('&')[0].Replace("BuyID=", ""), -1);
                        }

                        //this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?BuyID=" + buyID.ToString() + "'</script>");
                        rtnOk = 1;
                        rtnUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?BuyID=" + buyID.ToString();
                        return;
                    }
                    else
                    {
                        new Log("OnlinePay").Write("在线支付：写入返回数据出错！" + " 支付号：" + orderId);

                        //this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx'</script>");
                        rtnOk = 0;
                        rtnUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx";
                        return;
                    }

                    //报告给快钱处理结果，并提供将要重定向的地址。
                    

                }

            }
            #endregion

        }
        catch(Exception ex)
        {
            new Log("OnlinePay").Write("[快钱]在线支付：" + ex.Message + " -- 接收数据异常！");

            //this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx'</script>");

            return;
        }


    }


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
    //==============================================================================

    private bool WriteUserAccount(Users thisUser, string PayNumber, double PayMoney, string Memo)
    {
        if (PayMoney <= 0)
        {
            return false;
        }

        double formalitiesFeesScale = so["OnlinePay_99Bill_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double formalitiesFees = PayMoney - Math.Round(PayMoney / (formalitiesFeesScale + 1), 2);
        double addMoney = PayMoney - formalitiesFees;

        string ReturnDescription = "";
        bool ok = (thisUser.AddUserBalance(addMoney, formalitiesFees, PayNumber,"快钱", Memo, ref ReturnDescription) == 0);
        //new Log("OnlinePay").Write("ReturnDescription:" + ReturnDescription);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(PayNumber, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log("OnlinePay").Write("[快钱]在线支付：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log("OnlinePay").Write("[快钱]在线支付：对应的数据未处理");

                return false;
            }
        }

        return ok;
    }

}