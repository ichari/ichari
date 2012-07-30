using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Data;

public partial class Home_Room_OnlinePay_Tenpay_QueryResult : RoomPageBase
{
    private string bargainor_id = "";                                                               // 商户号（替换为自已的商户号）

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    private const int querycmdno = 2;                                                                    // 支付命令.1

    private int fee_type = 0;                                                                       // 费用类型,现在暂只支持 1:人民币

    private string date = "";

    private string sp_billno = "";                                                                  //商户订单号,10位正整数

    private long total_fee = 0;                                                                     // 订单金额,以分为单位

    private string transaction_id = "";                                                             // 交易单号,商户号(10)+支付日期(8)+商户订单号(10,不足的话左补0)=28位.

    private string attach = "";                                                                     // 指令标识,每次指令都会有这个字段,财付通在处理完成后会原样返回.

    private int pay_result;                                                                         //  支付結果

    public const int PAYOK = 0;
    public const int PAYSPERROR = 1;
    public const int PAYMD5ERROR = 2;
    public const int PAYERROR = 3;

    private string payerrmsg = "";


    SystemOptions so = new SystemOptions();

    private void Page_Load(object sender, System.EventArgs e)
    {


        string errmsg = "";

        //卖家商户key
        key = so["OnlinePay_Tenpay_MD5Key"].Value.ToString();

        //卖家商户号
        bargainor_id = so["OnlinePay_Tenpay_UserNumber"].Value.ToString();


        if (GetQueryValueFromUrl(Request.QueryString, out errmsg))
        {

            if (pay_result == PAYOK)
            {
                //如果充值查询成功，那么去对记录进行处理
                try
                {
                    string Memo = "系统交易号：" + sp_billno + ",财付通交易号：" + transaction_id;
                    int ReturnValue = -1;
                    string ReturnDescription = "";
                    int Results = -1;

                    DAL.Tables.T_UserPayDetails t_paydetails = new DAL.Tables.T_UserPayDetails();

                    DataTable tmptTB = t_paydetails.Open("", "ID=" + sp_billno, "");

                    if (tmptTB == null || tmptTB.Rows.Count <= 0)
                    {
                        Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：充值处理失败，本条数据丢失。\");</script>");

                        return;
                    }

                    double Money = Shove._Convert.StrToDouble(tmptTB.Rows[0]["Money"].ToString(), 0);
                    long ID = Shove._Convert.StrToLong(tmptTB.Rows[0]["UserID"].ToString(), 0);
                    double FormalitiesFees = Shove._Convert.StrToDouble(tmptTB.Rows[0]["FormalitiesFees"].ToString(), 0);

                    string[] banks = tmptTB.Rows[0]["PayType"].ToString().Split('_');

                    string PayBank = banks.Length < 2 ? "" : banks[1];

                    Results = DAL.Procedures.P_UserAddMoney(_Site.ID, ID, Money, FormalitiesFees, sp_billno,getBankName(PayBank), Memo, ref ReturnValue, ref ReturnDescription);

                    if (Results < 0)
                    {
                        Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：数据库读写错误\");</script>");

                        return;
                    }
                    else
                    {
                        if (ReturnValue < 0)
                        {
                            Response.Write("<script type=\"text/javascript\">alert(\"" + ReturnDescription + "\");</script>");

                            return;
                        }

                        Response.Write("<script type=\"text/javascript\">alert(\"此笔充值记录已到帐并已处理成功！\");</script>");

                    }
                }
                catch
                {
                    Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：查询失败，可能是网络通讯故障。请重试一次。\");</script>");


                    return;
                }


            }
            else
            {
                Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：" + payerrmsg+"\");</script>");
            }
        }
        else
        {
            //认证签名失败
            errmsg = "认证签名失败";
            Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：" + errmsg + "\");</script>");
        }

    }

    #region 内部函数

    /// <summary>
    /// 从查询结果页面的URL请求参数中获取结果信息
    /// </summary>
    /// <param name="querystring">查询结果页面的URL请求参数</param>
    /// <param name="errmsg">函数执行不成功的话,返回错误信息</param>
    /// <returns>函数执行是否成功</returns>
    public bool GetQueryValueFromUrl(NameValueCollection querystring, out string errmsg)
    {
        //结果URL参数样例如下

        #region 进行参数校验

        if (querystring == null || querystring.Count == 0)
        {
            errmsg = "参数为空";
            return false;
        }

        if (querystring["cmdno"] == null || querystring["cmdno"].ToString().Trim() != querycmdno.ToString())
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
            total_fee = long.Parse(querystring["total_fee"]);
            fee_type = Int32.Parse(querystring["fee_type"]);
            attach = querystring["attach"];

            if (querystring["bargainor_id"] != bargainor_id)
            {
                pay_result = PAYSPERROR;
                return true;
            }

            string strsign = querystring["sign"];
            string sign = GetQueryResultSign();

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
    /// 获取查询结果签名
    /// </summary>
    /// <returns>根据参数得到签名</returns>
    private string GetQueryResultSign()
    {
        string sign_text = "cmdno=" + querycmdno + "&pay_result=" + pay_result + "&date=" + date + "&transaction_id=" + transaction_id
            + "&sp_billno=" + sp_billno + "&total_fee=" + total_fee + "&fee_type=" + fee_type + "&attach=" + attach + "&key=" + key;

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

    /// <summary>
    /// 对字符串进行URL编码
    /// </summary>
    /// <param name="instr">待编码的字符串</param>
    /// <returns>编码结果</returns>
    private static string UrlEncode(string instr)
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


    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        isAllowPageCache = false;
       
        base.OnLoad(e);
    }

    #endregion
}
