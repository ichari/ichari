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

public partial class Home_Room_OnlinePay_Tenpay_Receive : RoomPageBase
{

    private string bargainor_id = "";                                                               // 商户号（替换为自已的商户号）

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    private const int cmdno = 1;                                                                    // 支付命令.1

    private int fee_type = 0;                                                                       // 费用类型,现在暂只支持 1:人民币

    private string date = "";

    private string sp_billno = "";                                                                  //商户订单号,10位正整数

    private long total_fee = 0;                                                                     // 订单金额,以分为单位

    private string transaction_id = "";                                                             // 交易单号,商户号(10)+支付日期(8)+商户订单号(10,不足的话左补0)=28位.

    private string attach = "";                                                                     // 指令标识,每次指令都会有这个字段,财付通在处理完成后会原样返回.

    private long buyId = 0;                                                                         // 投注编号

    private long userId = -1;                                                                       // 用户编号

    private int pay_result;                                                                         //  支付結果

    public const int PAYOK = 0;
    public const int PAYSPERROR = 1;
    public const int PAYMD5ERROR = 2;
    public const int PAYERROR = 3;

    private string payerrmsg = "";

    double Money = 0;                                                                               //充值金额
    string bankPay = "0";                                                                           //充值类型

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            string errmsg = "";

            //卖家商户key
            key = so["OnlinePay_Tenpay_MD5Key"].Value.ToString();

            //卖家商户号
            bargainor_id = so["OnlinePay_Tenpay_UserNumber"].Value.ToString();


            //判断签名及相关参数是否正确
            if (GetPayValueFromUrl(Request.QueryString, out errmsg))
            {
                //认证签名成功
                //支付判断
                if (pay_result == PAYOK)
                {
                    //支付成功，同定单号md5pay.Transaction_id可能会多次通知，请务必注意判断订单是否重复的逻辑
                    //处理业务逻辑，处理db之类的
                    //errmsg = "支付成功";
                    //Response.Write(errmsg+"<br/>");
                    //Response.Write("财付通定单号:"+ md5pay.Transaction_id +"(请牢记定单号)"+"<br/>");	

                    Users user;

                    user = new Users(_Site.ID)[_Site.ID, userId];

                    if (user == null)
                    {

                        errmsg = "在线支付：异常用户数据！" + " 支付号：" + sp_billno;
                        //new Log("System").Write(errmsg);
                        Response.Write(errmsg + "<br/>");

                        return;
                    }

                    if (_User == null)
                    {
                        _User = new Users(_Site.ID)[_Site.ID, user.ID];

                        string ReturnDescription = "";

                        _User.LoginDirect(ref ReturnDescription);

                    }


                    if (WriteUserAccount("系统交易号：" + sp_billno + " 财付通交易号：" + transaction_id))
                    {
                        string suchtml = "<meta content=\"China TENCENT\" name=\"TENCENT_ONLINE_PAYMENT\">\n"
                         + "<script language=\"javascript\">\n"
                         + "window.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?BuyID=" + buyId.ToString() + "';\n"
                         + "</script>";

                        errmsg = "在线支付：获取通知并处理成功！" + " 支付号：" + sp_billno;
                        //new Log("System").Write(errmsg);

                        //跳转到成功页面，财付通收到<meta content=\"China TENCENT\" name=\"TENCENT_ONLINE_PAYMENT\">，认为通知成功
                        Response.Write(suchtml);
                    }
                    else
                    {

                        errmsg = "在线支付：写入返回数据出错！" + " 支付号：" + sp_billno;
                        //new Log("System").Write(errmsg);
                        Response.Write(errmsg + "<br/>");

                    }
                }
                else
                {

                    //支付失败，请不要按成功处理
                    errmsg = "在线支付：验证出错！商户或者密串有误，错误代码：" + pay_result.ToString() + " 支付号：" + sp_billno;

                    //new Log("System").Write(errmsg);

                    Response.Write(errmsg + "<br/>");
                }

            }
            else
            {
                //认证签名失败
                errmsg = "认证签名失败";
                Response.Write("认证签名失败" + "<br/>");
            }

        }
        catch
        {
            //支付失败，请不要按成功处理
            Response.Write("支付失败" + "<br/>");
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        isAllowPageCache = false;
       
        base.OnLoad(e);
    }

    #endregion


    private bool WriteUserAccount(string Memo)
    {

        if (Money == 0)
        {
            return false;
        }

        double FormalitiesFeesScale = so["OnlinePay_Tenpay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Money - Math.Round(Money / (FormalitiesFeesScale + 1), 2);
        Money -= FormalitiesFees;

        string ReturnDescription = "";
        bool ok = (_User.AddUserBalance(Money, FormalitiesFees, sp_billno, getBankName(bankPay), Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(sp_billno, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                //new Log("System").Write("在线支付：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                //new Log("System").Write("在线支付：对应的数据未处理");

                return false;
            }
        }

        return ok;
    }


    #region 内部函数

    /// <summary>
    /// 从支付结果页面的URL请求参数中获取结果信息
    /// </summary>
    /// <param name="querystring">支付结果页面的URL请求参数</param>
    /// <param name="errmsg">函数执行不成功的话,返回错误信息</param>
    /// <returns>函数执行是否成功</returns>
    private bool GetPayValueFromUrl(NameValueCollection querystring, out string errmsg)
    {
        //结果URL参数样例如下
        /*
        ?cmdno=1&pay_result=0&pay_info=OK&date=20070423&bargainor_id=1201143001&transaction_id=1201143001200704230000000013
        &sp_billno=13&total_fee=1&fee_type=1&attach=%D5%E2%CA%C7%D2%BB%B8%F6%B2%E2%CA%D4%BD%BB%D2%D7%B5%A5				
        &sign=ADD7475F2CAFA793A3FB35051869E301
        */

        #region 进行参数校验

        if (querystring == null || querystring.Count == 0)
        {
            errmsg = "参数为空";
            return false;
        }

        if (querystring["cmdno"] == null || querystring["cmdno"].ToString().Trim() != cmdno.ToString())
        {
            errmsg = "没有cmdno参数或cmdno参数不正确";
            return false;
        }

        if (querystring["pay_result"] == null)
        {
            errmsg = "没有pay_result参数";
            return false;
        }

        if (querystring["date"] == null)
        {
            errmsg = "没有date参数";
            return false;
        }

        if (querystring["pay_info"] == null)
        {
            errmsg = "没有pay_info参数";
            return false;
        }

        if (querystring["bargainor_id"] == null)
        {
            errmsg = "没有bargainor_id参数";
            return false;
        }

        if (querystring["transaction_id"] == null)
        {
            errmsg = "没有transaction_id参数";
            return false;
        }

        if (querystring["sp_billno"] == null)
        {
            errmsg = "没有sp_billno参数";
            return false;
        }

        if (querystring["total_fee"] == null)
        {
            errmsg = "没有total_fee参数";
            return false;
        }

        if (querystring["fee_type"] == null)
        {
            errmsg = "没有fee_type参数";
            return false;
        }

        if (querystring["attach"] == null)
        {
            errmsg = "没有attach参数";
            return false;
        }

        if (querystring["sign"] == null)
        {
            errmsg = "没有sign参数";
            return false;
        }

        #endregion

        errmsg = "";

        try
        {
            pay_result = Int32.Parse(querystring["pay_result"].Trim());

            payerrmsg = UrlDecode(querystring["pay_info"].Trim());
            date = querystring["date"];
            transaction_id = querystring["transaction_id"];
            sp_billno = querystring["sp_billno"];
            total_fee = Shove._Convert.StrToLong(querystring["total_fee"], 0);
            fee_type = Shove._Convert.StrToInt(querystring["fee_type"], 0);

            attach = UrlDecode(querystring["attach"]);

            //要进行解密
            userId = Shove._Convert.StrToLong((Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), attach)).Split('|')[0], -1);

            bankPay=Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), attach).Split('|')[1];

            buyId = Shove._Convert.StrToLong((Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), attach)).Split('|')[2], -1);

            

            Money = Shove._Convert.StrToDouble(querystring["total_fee"], 0) / 100;

            if (querystring["bargainor_id"] != bargainor_id)
            {
                pay_result = PAYSPERROR;
                return true;
            }

            string strsign = querystring["sign"];

            string sign = GetPayResultSign();

            if (sign != strsign)
            {
                pay_result = PAYMD5ERROR;
            }

            return true;
        }
        catch (Exception err)
        {
            errmsg = "解析参数出错:" + err.Message;
            return false;
        }
    }

    /// <summary>
    /// 对字符串进行URL解码
    /// </summary>
    /// <param name="instr">待解码的字符串</param>
    /// <returns>解码结果</returns>
    private static string UrlDecode(string instr)
    {
        if (instr == null || instr.Trim() == "")
            return "";
        else
        {
            return instr.Replace("%3d", "=").Replace("%26", "&").Replace("%22", "\"").Replace("%3f", "?")
                .Replace("%27", "'").Replace("%20", " ").Replace("%25", "%");
        }
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
    /// 获取支付结果签名
    /// </summary>
    /// <returns>根据参数得到签名</returns>
    private string GetPayResultSign()
    {
        string sign_text = "cmdno=" + cmdno + "&pay_result=" + pay_result + "&date=" + date + "&transaction_id=" + transaction_id
            + "&sp_billno=" + sp_billno + "&total_fee=" + total_fee + "&fee_type=" + fee_type + "&attach=" + UrlEncode(attach) + "&key=" + key;

        return GetMD5(sign_text);
    }

    /// <summary>
    /// 获取大写的MD5签名结果
    /// </summary>
    /// <param name="encypStr"></param>
    /// <returns></returns>
    private string GetMD5(string encypStr)
    {
        string retStr;
        MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

        //创建md5对象
        byte[] inputBye;
        byte[] outputBye;

        //使用GB2312编码方式把字符串转化为字节数组．
        inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);

        outputBye = m5.ComputeHash(inputBye);

        retStr = System.BitConverter.ToString(outputBye);
        retStr = retStr.Replace("-", "").ToUpper();
        return retStr;
    }

    //根据支付方式来获取相应的中文说明
    private string getBankName(string bankCode)
    {
        string bankName = "财付通";


        switch (bankCode.ToLower())
        {
            case "0":
                bankName = "财付通";

                break;
            case "1001":
                bankName = "招商银行";

                break;
            case "1002":
                bankName = "中国工商银行";

                break;
            case "1003":
                bankName = "中国建设银行";

                break;
            case "1004":
                bankName = "上海浦东发展银行";

                break;
            case "1005":
                bankName = "中国农业银行";

                break;
            case "1006":
                bankName = "中国民生银行";

                break;
            case "1008":
                bankName = "深圳发展银行";

                break;
            case "1009":
                bankName = "兴业银行";

                break;
            case "1028":
                bankName = "广州银联";

                break;
            case "1032":
                bankName = "   北京银行";

                break;
            case "1020":
                bankName = "   中国交通银行";

                break;
            case "1022":
                bankName = "   中国光大银行";

                break;
        }

        return bankName;

    }

    #endregion
}
