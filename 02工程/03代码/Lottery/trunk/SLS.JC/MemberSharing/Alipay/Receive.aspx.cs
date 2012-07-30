using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.IO;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Net;
using System.Text.RegularExpressions;

public partial class MemberSharing_Alipay_Receive : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "接口调用失败，请重新登录。", "../../Default.aspx");

            return;
        }

        SystemOptions so = new SystemOptions();

        string alipayNotifyURL = "http://notify.alipay.com/trade/notify_query.do?";
        string partner = so["MemberSharing_Alipay_UserNumber"].ToString("");  //卖家商户号

        alipayNotifyURL = alipayNotifyURL + "partner=" + partner + "&notify_id=" + Request.QueryString["notify_id"];

        //获取支付宝ATN返回结果，true是正确的订单信息，false 是无效的
        string responseTxt = Get_Http(alipayNotifyURL, 120000);

        if (responseTxt == "false")
        {
            Shove._Web.JavaScript.Alert(this.Page, "接口调用失败，请重新登录。", "../../Home/Web/Default.aspx");

            return;
        }

        string url = this.Request.Url.AbsoluteUri.Replace("/MemberSharing/Alipay/Receive.aspx?", "/Home/Room/Receive.aspx?");
        this.Response.Redirect(url, true);
    }

    //获取远程服务器ATN结果
    private String Get_Http(String a_strUrl, int timeout)
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
}