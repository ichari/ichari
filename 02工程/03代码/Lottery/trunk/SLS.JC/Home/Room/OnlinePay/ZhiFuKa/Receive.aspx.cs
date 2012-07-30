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

public partial class Home_Room_OnlinePay_ZhiFuKa_Receive : RoomPageBase
{

    private string customerid = "";                                                                 // 商户号（替换为自已的商户号）

    private string sd51no = "";                                                                     //51支付平台的订单ID

    private string sdcustomno = "";                                                                 // 商户系统生成的订单号

    double ordermoney = 0;                                                                          //充值金额

    string cardno = "";                                                                          //充值类型(支付卡或神州行卡)

    private string mark = "";                                                                       // 商户自定义，原样返回.

    private long buyId = 0;                                                                         // 投注编号

    private long userId = -1;                                                                       // 用户编号

    private string noticeurl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/ZhiFuKa/Receive.aspx"; // 通知商户支付结果的商户系统地址

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    public const int PAYOK = 1;                                                                    //支付成功
    public const int PAYERROR = 2;                                                                 //支付失败
    public const int PAYSPERROR = 3;                                                               //商户不匹配
    public const int PAYMD5ERROR = 4;                                                               //密钥不正确

    private int state = PAYERROR;


    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            string errmsg = "";

            //卖家商户key
            key = so["OnlinePay_ZhiFuKa_MD5Key"].Value.ToString();

            //卖家商户号
            customerid = so["OnlinePay_ZhiFuKa_UserNumber"].Value.ToString();


            //判断签名及相关参数是否正确
            if (GetPayValueFromUrl(Request.QueryString, out errmsg))
            {
                //认证签名成功
                //支付判断
                if (state == PAYOK || state == PAYERROR)
                {
                    Users user;

                    user = new Users(_Site.ID)[_Site.ID, userId];

                    if (user == null)
                    {

                        errmsg = "在线支付：充值未完成。错误描述：异常用户数据！" + " 支付号：" + sdcustomno;
                        //new Log("OnlinePay").Write(errmsg);
                        Response.Write(errmsg + "<br/>");

                        return;
                    }

                    if (_User == null)
                    {
                        _User = new Users(_Site.ID)[_Site.ID, user.ID];

                        string ReturnDescription = "";

                        _User.LoginDirect(ref ReturnDescription);

                    }

                    if (state == PAYOK)
                    {
                        //如果是充值成功的通知
                        if (WriteUserAccount("系统交易号：" + sdcustomno+" 51支付交易号："+sd51no))
                        {

                            //errmsg = "在线支付：充值完成。" + " 支付号：" + sdcustomno;
                            //new Log("OnlinePay").Write(errmsg);

                            //给接口提供商返回通知结果(1000:表示获取到了充值成功的通知，并成功处理。返回：1，下次不用再通知了。)

                            ReturnResult("1");

                            return;

                        }
                        else
                        {

                            //给接口提供商返回通知结果(1001:表示获取到了充值成功的通知，但是在完成充值的过程中出现了问题。返回：非1，希望下次继续发送。)

                            ReturnResult("1001");

                            return;


                        }
                    }
                    else
                    {
                        //如果是充值失败的通知

                        //errmsg = "在线支付：充值失败！" + " 支付号：" + sdcustomno;
                        //new Log("OnlinePay").Write(errmsg);

                        //给接口提供商返回通知结果(2001:表示获取了充值失败通知，这种通知我们本地无需处理。返回：1，下次不用再通知了。)

                        ReturnResult("1");

                        return;

                    }

                }
                else
                {

                    //支付失败
                    //errmsg = "在线支付：充值未完成。错误描述：验证出错！商户或者密串有误，错误代码：" + state.ToString() + " 支付号：" + sdcustomno;

                    //new Log("OnlinePay").Write(errmsg);

                    //给接口提供商返回通知结果(3001:表示获取了充值通知，但是参数信息有错误，无法通过验证。返回：3001，希望下次继续发送。)

                    ReturnResult("3001");

                }

            }
            else
            {
                //认证签名失败
                //errmsg = "在线支付：充值未完成。错误描述：认证签名失败！";

                //new Log("OnlinePay").Write(errmsg);


                //给接口提供商返回通知结果(3001:表示获取了充值通知，但是参数信息有错误，无法通过验证。返回：3001，希望下次继续发送。)

                ReturnResult("3001");
            }

        }
        catch
        {
            //支付失败

            //new Log("OnlinePay").Write("在线支付：充值未完成。错误描述：出现未知异常！");

            //给接口提供商返回通知结果(5001:表示获取了充值通知，但是出现了未知异常。返回：5001，希望下次继续发送。)

            ReturnResult("5001");
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
        //这里手续费的计算与网银的不一样，网银充值10元，那么实充就是10,但是手机卡充值10元，实充就要小于10元
        double FormalitiesFeesScale = so["OnlinePay_ZhiFuKa_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Math.Round(ordermoney * FormalitiesFeesScale, 2);

        ordermoney -= FormalitiesFees;

        string ReturnDescription = "";
        bool ok = (_User.AddUserBalance(ordermoney, FormalitiesFees, sdcustomno,getBankName(cardno), Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(sdcustomno, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                //new Log("OnlinePay").Write("在线支付：充值未完成。错误描述：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                //new Log("OnlinePay").Write("在线支付：充值未完成。错误描述：对应的数据未处理");

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

        #region 进行参数校验


        if (querystring == null || querystring.Count == 0)
        {
            errmsg = "参数为空";
            return false;
        }

        if (querystring["state"] == null)
        {
            errmsg = "没有state参数或state参数不正确";
            return false;
        }

        if (querystring["customerid"] == null)
        {
            errmsg = "没有customerid参数";
            return false;
        }

        if (querystring["sd51no"] == null)
        {
            errmsg = "没有sd51no参数";
            return false;
        }

        if (querystring["sdcustomno"] == null)
        {
            errmsg = "没有sdcustomno参数";
            return false;
        }

        if (querystring["ordermoney"] == null)
        {
            errmsg = "没有ordermoney参数";
            return false;
        }

        if (querystring["cardno"] == null)
        {
            errmsg = "没有cardno参数";
            return false;
        }

        if (querystring["mark"] == null)
        {
            errmsg = "没有mark参数";
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
            state = Shove._Convert.StrToInt(querystring["state"].Trim(), 2);

            sd51no = querystring["sd51no"];

            sdcustomno = querystring["sdcustomno"];

            ordermoney = Shove._Convert.StrToDouble(querystring["ordermoney"], 0);

            cardno = querystring["cardno"];

            mark = querystring["mark"];

            //要进行解密
            userId = Shove._Convert.StrToLong((Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), mark)).Split('|')[0], -1);

            buyId = Shove._Convert.StrToLong((Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), mark)).Split('|')[1], -1);

            if (querystring["customerid"] != customerid)
            {
                state = PAYSPERROR;
                return true;
            }

            string strsign = querystring["sign"];



            string sign = GetPayResultSign();

            if (sign != strsign)
            {
                state = PAYMD5ERROR;
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
    /// 获取支付结果签名
    /// </summary>
    /// <returns>根据参数得到签名</returns>
    private string GetPayResultSign()
    {

        string sign_text = "customerid=" + customerid + "&sd51no=" + sd51no + "&sdcustomno=" + sdcustomno + "&mark=" + mark
            + "&key=" + key;

       

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


    private void ReturnResult(string resultCode)
    {
        Response.Write("<result>" + resultCode + "</result>");
    }

    //根据支付编号来获取相应的中文说明
    private string getBankName(string bankCode)
    {
        string bankName = "神州行充值卡";
        switch (bankCode.ToLower())
        {
            case "szx":
                bankName = "神州行充值卡";
                break;

            case "zfk":
                bankName = "51支付卡";
                break;
        }

        return bankName;

    }

    #endregion
}
