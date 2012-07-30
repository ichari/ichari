using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public partial class Home_Room_OnlinePay_Tenpay_PayQuery : RoomPageBase
{
    #region 财付通变量

    string querygateurl = "http://mch.tenpay.com/cgi-bin/cfbi_query_order.cgi";
    string queryreturn_url = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Tenpay/QueryResult.aspx";

    private string bargainor_id = "";                                                               // 商户号（替换为自已的商户号）

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    private const int cmdno = 2;                                                                    // 支付命令.1

    private string date = "";

    private string sp_billno = "";                                                                  //商户订单号,10位正整数

    private string transaction_id = "";                                                             // 交易单号,商户号(10)+支付日期(8)+商户订单号(10,不足的话左补0)=28位.

    private string attach =UrlEncode(HttpUtility.UrlEncode("充值查询", Encoding.GetEncoding("GBK")));                                                                   // 指令标识,每次指令都会有这个字段,财付通在处理完成后会原样返回.

    SystemOptions so = new SystemOptions();


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        //获取查询参数
        //Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + attach + "！\");window.location.href='';</script>");

        ////获取查询参数
        //Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " +UrlDecode(attach) + "！\");window.location.href='';</script>");

        //return;

        //卖家商户号
        bargainor_id = so["OnlinePay_Tenpay_UserNumber"].Value.ToString();

        //卖家商户key
        key = so["OnlinePay_Tenpay_MD5Key"].Value.ToString();

        date = Shove._Web.Utility.GetRequest("date");
        sp_billno= Shove._Web.Utility.GetRequest("sp_billno");

        if (string.IsNullOrEmpty(date) || string.IsNullOrEmpty(sp_billno) || string.IsNullOrEmpty(bargainor_id) || string.IsNullOrEmpty(key))
        {

            Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：参数不齐全，无法提交查询！\"); window.location.href='';</script>");

            return;
        }

        transaction_id  = CreatePayNumber(sp_billno);

        string url = "";
        if (!GetQueryUrl(out url))
        {
            Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：" + url + "！\");window.location.href='';</script>");

            return;
        }
        else
        {
            string returnUrl="";

            if (GetSus_Msg(PF.GetHtml(url, "GBK", 120), out returnUrl))
            {

                Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sp_billno + " 的支付记录没有充值成功，描述：" + returnUrl + "！\"); window.location.href='';</script>");

                return;
            }

            Response.Write(returnUrl);
        }
    }


    #region 财付通充值查询

    /// <summary>
    /// 获取查询页面URL
    /// </summary>
    /// <param name="url">如果函数返回真,是查询URL,如果函数返回假,是错误信息</param>
    /// <returns>函数执行是否成功</returns>
    private bool GetQueryUrl(out string url)
    {

        try
        {
            string sign = GetQuerySign();

            url = querygateurl + "?cmdno=" + cmdno + "&date=" + date + "&bargainor_id=" + bargainor_id + "&transaction_id="
            + transaction_id + "&sp_billno=" + sp_billno + "&return_url=" + queryreturn_url + "&attach=" + attach + "&sign=" + sign;

            return true;
        }
        catch (Exception err)
        {
            url = "创建URL时出错,错误信息:" + err.Message;
            return false;
        }
    }

    /// <summary>
    /// 获取查询签名
    /// </summary>
    /// <returns>根据参数得到签名</returns>
    private string GetQuerySign()
    {
        string sign_text = "cmdno=" + cmdno + "&date=" + date + "&bargainor_id=" + bargainor_id + "&transaction_id="
           + transaction_id + "&sp_billno=" + sp_billno + "&return_url=" + queryreturn_url + "&attach=" + UrlDecode(attach) + "&key=" + key;
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
    /// 产生一个交易号
    /// </summary>
    /// <param name="Number"></param>
    /// <returns></returns>
    private string CreatePayNumber(string Number)
    {
        string number = Number.PadLeft(10, '0');

        number = number.Substring(number.Length - 10);

        return bargainor_id + date + number;
    }

    //分析返回的地址,抓取错误信息
    private bool GetSus_Msg(string url, out string sus_Msg)
    {
        bool result = false;

        sus_Msg = url;

        //如果返回地址为指定接收，就不分析
        if (url.IndexOf("QueryResult.aspx") >= 0)
        {
            return result;
        }
        else
        {
            string[] strs = url.Split('&');

            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i].IndexOf("suc_msg=") >= 0)
                {
                    sus_Msg = strs[i].Substring(8);

                    result = true;

                    break;
                }
            }
        }

        return result;

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
