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
using System.Data.OleDb;
using System.Net;

using Shove.Alipay;

public partial class Admin_OnlinePayment_Alipay_AlipayNotify :Page
{
    /// <summary>
    /// created by sunzhizhi 2006.5.21,sunzhizhi@msn.com。
    /// </summary>

    
    //获取远程服务器ATN结果
    public String Get_Http(String a_strUrl, int timeout)
    {
        string strResult;
        try
        {

            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
            myReq.Timeout = timeout;
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            Stream myStream = HttpWResp.GetResponseStream();
            StreamReader sr = new StreamReader(myStream, Encoding.Default);
            StringBuilder strBuilder = new StringBuilder();
            while (-1 != sr.Peek())
            {
                strBuilder.Append(sr.ReadLine());
            }

            strResult = strBuilder.ToString();
        }
        catch (Exception exp)
        {

            strResult = "错误：" + exp.Message;
        }

        return strResult;
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        //isRequestLogin = false;

        base.OnInit(e);
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

        string partner;
        string key;
        string alipayNotifyURL;
        string _input_charset;
        string responseTxt;

        _input_charset = "utf-8";

        ///当不知道https的时候，请使用http
        alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";  //"https://www.alipay.com/cooperate/gateway.do?";


        SystemOptions sysOptions = new SystemOptions();
        partner = sysOptions["OnlinePay_Alipay_ForUserDistill_UserNumber"].ToString(""); 		//partner合作伙伴id（必须填写）
        key = sysOptions["OnlinePay_Alipay_ForUserDistill_MD5Key_ForPayOut"].ToString("");       //partner 的对应交易安全校验码（必须填写）

        // alipayNotifyURL = alipayNotifyURL + "service=notify_verify" + "&partner=" + partner + "&notify_id=" + Request.Form["notify_id"];
        alipayNotifyURL = alipayNotifyURL + "partner=" + partner + "&notify_id=" + Request.Form["notify_id"];

        //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
        //Shove.Alipay.Alipay myAlipay = new Shove.Alipay.Alipay();
        responseTxt =Get_Http(alipayNotifyURL, 120000);


        //****************************************************************************************
        NameValueCollection coll;
        coll = Request.Form;

        // Get names of all forms into a string array.
        String[] requestarr = coll.AllKeys;

        //进行排序；
        //string[] Sortedstr = BubbleSort(requestarr);
        string[] Sortedstr = Shove.Alipay.Alipay.BubbleSort(requestarr);

        new Log("Admin\\AlipayPayment").Write("查询URL:" + alipayNotifyURL);//@@@@@@@@@
        //构造待md5摘要字符串 ；
        string prestr = "";
        for (int i = 0; i < Sortedstr.Length; i++)
        {
            if (Request.Form[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
            {
                if (i == Sortedstr.Length - 1)
                {
                    prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]];
                }
                else
                {
                    prestr = prestr + Sortedstr[i] + "=" + Request.Form[Sortedstr[i]] + "&";
                }
            }

        }

        prestr = prestr + key;
        string mysign = Shove.Alipay.Alipay.GetMD5(prestr, _input_charset);
        string sign = Request.Form["sign"];
        string success_details = Request.Form["success_details"];
        string fail_details = Request.Form["fail_details"];
        string batch_no = Request.Form["batch_no"];	            //批量付款订单号 日期(20070412)+16位序列号---客户流水号
        string pay_account_no = Request.Form["pay_account_no"];	//支付宝流水号

        //double Money = 0;


        new Log("Admin\\AlipayPayment").Write("签名验证:mysign=" + mysign + " sign=" + sign + " responseTxt=" + responseTxt + " batch_no=" + batch_no + "  pay_account_no=" + pay_account_no);//@@@@@@@@@

        if (mysign == sign && responseTxt == "true")   //验证支付宝发过来的消息，签名是否正确
        {
            //更新自己数据库的订单语句，请自己填写一下
            if ((success_details != null) && (success_details != ""))
            {
                new Log("Admin\\AlipayPayment").Write("success_details:" + success_details);//@@@@@@@@@
                
                success_details = success_details.Substring(0, success_details.Length - 1);

                string[] success_detailsLists = success_details.Split('|');

                for (int j = 0; j < success_detailsLists.Length; j++)
                {
                    string[] success_detailsList = success_detailsLists[j].Split('^');
                    long distillID = Shove._Convert.StrToLong(success_detailsList[0], -1);//商家流水号(这里为提款ID)

                    //Money = +Shove._Convert.StrToDouble(success_detailsList[3], 0);
                    //加入统计（Web服务未写）
                    int returnValue = 0;
                    string returnDescription = "";
                    if (DAL.Procedures.P_UserDistillPayByAlipaySuccess(1, distillID, 1, ref returnValue, ref returnDescription) < 0)
                    {

                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("出错!提款ID:" + distillID + "派款成功,但状态更新失败");

                        continue;
                    }
                    if (returnValue < 0)
                    {
                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("出错!提款ID:" + distillID + "派款成功，但状态更新失败:" + returnDescription);
                        continue;
                    }
                }
            }
            

            if ((fail_details != null) && (fail_details != ""))
            {
                new Log("Admin\\AlipayPayment").Write("fail_details:" + fail_details);//@@@@@@@@@
                
                fail_details = fail_details.Substring(0, fail_details.Length - 1);

                string[] fail_detailsLists = fail_details.Split('|');

                for (int j = 0; j < fail_detailsLists.Length; j++)
                {
                    string[] fail_detailsList = fail_detailsLists[j].Split('^');

                    long distillID=Shove._Convert.StrToLong(fail_detailsList[0], -1);//商家流水号(这里为提款ID)

                    //支付没有成功，返还会员提款的金额
                    int returnValue = 0;
                    string returnDescription = "";
                    if (DAL.Procedures.P_UserDistillPayByAlipayUnsuccess(1,distillID,"批量付款到支付宝失败,请财务人员确认,并另作付款.", ref returnValue ,ref returnDescription )< 0)
                    {
                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("出错!提款ID:" + distillID + "派款失败，状态更新失败。");
                        continue;
                    }
                    if (returnValue < 0)
                    {
                        DAL.Procedures.P_UserDistillPayByAlipayWriteLog("出错!提款ID:" + distillID + "派款失败，状态更新失败:" + returnDescription);
                        continue;
                    }
                }
            }

            Response.Write("success");     //返回给支付宝消息，成功
        }
        else
        {
            Response.Write("fail");
        }
    }

    //public static string GetMD5(string s, string _input_charset)
    //{
    //    /// <summary>
    //    /// 与ASP兼容的MD5加密算法
    //    /// </summary>

    //    MD5 md5 = new MD5CryptoServiceProvider();
    //    byte[] t = md5.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(s));
    //    StringBuilder sb = new StringBuilder(32);
    //    for (int i = 0; i < t.Length; i++)
    //    {
    //        sb.Append(t[i].ToString("x").PadLeft(2, '0'));
    //    }
    //    return sb.ToString();
    //}

    //public static string[] BubbleSort(string[] r)
    //{
    //    /// <summary>
    //    /// 冒泡排序法
    //    /// </summary>

    //    int i, j; //交换标志 
    //    string temp;

    //    bool exchange;

    //    for (i = 0; i < r.Length; i++) //最多做R.Length-1趟排序 
    //    {
    //        exchange = false; //本趟排序开始前，交换标志应为假

    //        for (j = r.Length - 2; j >= i; j--)
    //        {
    //            if (System.String.CompareOrdinal(r[j + 1], r[j]) < 0)　//交换条件
    //            {
    //                temp = r[j + 1];
    //                r[j + 1] = r[j];
    //                r[j] = temp;

    //                exchange = true; //发生了交换，故将交换标志置为真 
    //            }
    //        }

    //        if (!exchange) //本趟排序未发生交换，提前终止算法 
    //        {
    //            break;
    //        }

    //    }
    //    return r;
    //}
}