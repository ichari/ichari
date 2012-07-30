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

public partial class Home_Room_OnlinePay_Alipay01_Receive : RoomPageBase
{
    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //***************************************************************     
            string _input_charset = "utf-8";
            string NotifyService = "notify_verify";
            string SellerEmail = so["OnlinePay_Alipay_UserName"].ToString("");//卖家支付宝名称(邮箱)

            string NotifyID = Request.QueryString["notify_id"];

            int NotifyType = 2;

            Shove.Alipay.Alipay ap = new Shove.Alipay.Alipay();

            //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
            string responseTxt = ap.Get_Http(NotifyService, NotifyID, SellerEmail, _input_charset, NotifyType, 120000);

            int i;
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestarr = coll.AllKeys;

            //进行排序；
            string[] Sortedstr = Alipay.Gateway.Utility.BubbleSort(requestarr);

            //构造待md5摘要字符串 ；

            StringBuilder prestr = new StringBuilder();

            for (i = 0; i < Sortedstr.Length; i++)
            {
                if (String.IsNullOrEmpty(Sortedstr[i]))
                {
                    continue;
                }

                if (Request.QueryString[Sortedstr[i]] != "" && Sortedstr[i] != "sign" && Sortedstr[i] != "sign_type")
                {
                    if (i == Sortedstr.Length - 1)
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]]);
                    }
                    else
                    {
                        prestr.Append(Sortedstr[i] + "=" + Request.QueryString[Sortedstr[i]] + "&");

                    }
                }
            }

            string mysign = ap.GetMD5(prestr.ToString(), SellerEmail, _input_charset);
            string sign = Request.QueryString["sign"];
            string trade_status = Request.QueryString["trade_status"];
            string trade_no = Request.QueryString["trade_no"];              //支付宝交易号
            string out_trade_no = Request.QueryString["out_trade_no"];      //自己交易号
            string payment_type = Request.QueryString["payment_type"];      //支付类型
            string subject = Request.QueryString["subject"];                //商品名称，用户存放用户ID
            string subject_UnEncrypt = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), subject);
            double total_fee = double.Parse(Request.QueryString["total_fee"].ToString());       //支付金额
            string seller_email = Request.QueryString["seller_email"];      //卖家账号

            if (seller_email != so["OnlinePay_Alipay_UserName"].ToString(""))
            {
                new Log("System").Write("在线支付：收款帐号不匹配！");

                PF.GoError(ErrorNumber.Unknow, "支付用户信息验证失败", this.GetType().BaseType.FullName);

                return;
            }

            if (mysign == sign && responseTxt == "true" && trade_status == "TRADE_FINISHED")   //验证支付发过来的消息，签名是否正确
            {
                Users user;

                if (_User == null)
                {
                    user = new Users(_Site.ID)[_Site.ID, Shove._Convert.StrToLong(subject_UnEncrypt, -1)];
                }
                else
                {
                    user = new Users(_Site.ID)[_Site.ID, _User.ID];
                }

                if (user == null)
                {
                    //PF.GoError(ErrorNumber.Unknow, "异常用户数据", this.GetType().BaseType.FullName);

                    new Log("System").Write("在线支付：异常用户数据！");

                    this.Response.Write("<script language='javascript'>window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/MyIcaile.aspx?SubPage=OnlinePay/Fail.aspx'</script>");

                    return;
                }

                if (_User == null)
                {
                    _User = new Users(_Site.ID)[_Site.ID, user.ID];

                    //string ReturnDescription = "";
                    //_User.LoginDirect(ref ReturnDescription);
                }

                if (WriteUserAccount(_User, out_trade_no.ToString(), total_fee.ToString(), "系统交易号：" + out_trade_no.ToString() + ",支付宝交易号：" + trade_no.ToString()))
                {
                    this.Response.Write("<script language='javascript'>window.top.location.href='http://" + Shove._Web.Utility.GetUrlWithoutHttp() + "/Home/Room/MyIcaile.aspx?SubPage=OnlinePay/OK.aspx'</script>");

                    return;
                }
                else
                {
                    new Log("System").Write("在线支付：写入返回数据出错！");

                    this.Response.Write("<script language='javascript'>window.top.location.href='http://" + Shove._Web.Utility.GetUrlWithoutHttp() + "/Home/Room/MyIcaile.aspx?SubPage=OnlinePay/Fail.aspx'</script>");

                    return;
                }
            }
            else
            {
                new Log("System").Write("在线支付：系统交易号：" + out_trade_no + " 支付宝交易号：" + trade_no + " 校验出错！responseTxt系统要求参数为true/false，实际返回：" + responseTxt.ToString() + " trade_status系统要求返回TRADE_FINISHED，实际返回： " + trade_status.ToString() + " 生成校验码：" + mysign.ToString() + "返回校验码：" + sign.ToString());
                this.Response.Write("<script language='javascript'>window.top.location.href='http://" + Shove._Web.Utility.GetUrlWithoutHttp() + "/Home/Room/MyIcaile.aspx?SubPage=OnlinePay/Fail.aspx'</script>");

                return;
            }
        }
        catch (Exception Ex)
        {
            new Log("System").Write("在线支付：" + Ex.Message + " -- 接收数据异常！");

            this.Response.Write("<script language='javascript'>window.top.location.href='http://" + Shove._Web.Utility.GetUrlWithoutHttp() + "/Home/Room/MyIcaile.aspx?SubPage=OnlinePay/Fail.aspx'</script>");

            return;
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

    ////获取远程服务器ATN结果
    //public String Get_Http(String a_strUrl, int timeout)
    //{
    //    string strResult;
    //    try
    //    {
    //        HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(a_strUrl);
    //        myReq.Timeout = timeout;
    //        HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
    //        Stream myStream = HttpWResp.GetResponseStream();
    //        StreamReader sr = new StreamReader(myStream, Encoding.Default);
    //        StringBuilder strBuilder = new StringBuilder();
    //        while (-1 != sr.Peek())
    //        {
    //            strBuilder.Append(sr.ReadLine());
    //        }

    //        strResult = strBuilder.ToString();
    //    }
    //    catch (Exception exp)
    //    {

    //        strResult = "错误：" + exp.Message;
    //    }

    //    return strResult;
    //}

    private bool WriteUserAccount(Users _User, string orderid, string amount, string Memo)
    {
        double Money = Shove._Convert.StrToDouble(amount, 0);
        if (Money == 0)
        {
            return false;
        }

        double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Money - Math.Round(Money / (FormalitiesFeesScale + 1), 2);
        Money -= FormalitiesFees;

        string ReturnDescription = "";
        bool ok = (_User.AddUserBalance(Money, FormalitiesFees, orderid, "支付宝支付," + so["OnlinePay_Alipay_UserName"].ToString(""), Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(orderid, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log("System").Write("在线支付：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log("System").Write("在线支付：对应的数据未处理");

                return false;
            }
        }

        return ok;
    }
}