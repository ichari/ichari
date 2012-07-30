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
using System.Xml;

public partial class Home_Room_OnlinePay_ZhiFuKa_Send : RoomPageBase
{

    private string customerid = "";                                                                 // 商户号（替换为自已的商户号）

    private string sdcustomno = "";                                                                 // 商户系统生成的订单号

    double ordermoney = 0;                                                                          //充值金额

    string cardno = "";                                                                          //充值类型(支付卡或神州行卡)

    string faceno = "";                                                                       //卡面值编号

    string cardnum = "";                                                                            //点卡卡号

    string cardpass = "";                                                                           //点卡密码

    private string remarks = "3gcpw";                                                              // 商户备注

    private string mark = "";                                                                       // 商户自定义，原样返回.

    private string noticeurl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/ZhiFuKa/Receive.aspx"; // 通知商户支付结果的商户系统地址

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    private string paygateurl = "http://202.75.218.94/gateway/zfgateway.asp";                       // 支付网关URL

    private string state = "0000";
    private string errcode = "0000";
    private string errmsg = "0000";


    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {

        bool OnlinePay_ZhiFuKa_Status_ON = so["OnlinePay_ZhiFuKa_Status_ON"].ToBoolean(false);

        ordermoney = Shove._Convert.StrToDouble(Shove._Web.Utility.GetRequest("ordermoney"), 0);
        cardno = Shove._Web.Utility.GetRequest("cardno");
        faceno = Shove._Web.Utility.GetRequest("faceno");
        cardnum = Shove._Web.Utility.GetRequest("cardnum");
        cardpass = Shove._Web.Utility.GetRequest("cardpass");

        string errorMessage = "";


        if (!IsPostBack)
        {

            //卖家商户号
            customerid = so["OnlinePay_ZhiFuKa_UserNumber"].Value.ToString();

            //卖家商户key
            key = so["OnlinePay_ZhiFuKa_MD5Key"].Value.ToString();

            //交易标识(用户ID+投注ID) 加密
            mark = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "|" + Shove._Web.Utility.GetRequest("BuyID"));

            long NewPayNumber = -1;
            string ReturnDescription = "";

            try
            {

                //产生内部充值编号
                if (DAL.Procedures.P_GetNewPayNumber(_Site.ID, _User.ID, "51ZFK_"+cardno,ordermoney , 0, ref NewPayNumber, ref ReturnDescription) < 0)
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
                sdcustomno = NewPayNumber.ToString();


                #region 验证参数是否齐全

                if (string.IsNullOrEmpty(cardno) || string.IsNullOrEmpty(remarks) || string.IsNullOrEmpty(customerid) || string.IsNullOrEmpty(sdcustomno) || string.IsNullOrEmpty(mark) || string.IsNullOrEmpty(noticeurl) || string.IsNullOrEmpty(key) || string.IsNullOrEmpty(cardnum) || string.IsNullOrEmpty(cardpass) || string.IsNullOrEmpty(faceno))
                {
                    errorMessage ="参数不齐全，无法充值！";

                    Response.Write("<script type=\"text/javascript\"> window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx?errMsg="+errorMessage+"';</script>");
                  
                    return;
                }

                #endregion

                string url = "";
                if (!GetPayUrl(out url))
                {
                    errorMessage =url;

                    Response.Write("<script type=\"text/javascript\"> window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx?errMsg=" + errorMessage + "';</script>");
                 

                    return;
                }
                else
                {

                    GetResponseContents(GetHtml(url, "GB2312", 200));

                    if (state == "1")
                    {

                        new Log("OnlinePay").Write("在线支付：充值申请已经提交，等待结果通知！支付号：" + sdcustomno);


                        errorMessage = "在线支付：充值申请已经提交，等待结果通知！支付号：" + sdcustomno;

                        Response.Write("<script type=\"text/javascript\"> window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?errMsg=" + errorMessage + "';</script>");

                        return;
                    }



                    new Log("OnlinePay").Write("在线支付：充值申请提交失败！描述：code=" + errcode + "； msg=" + errmsg + "； 支付号：" + sdcustomno);

                    errorMessage = "在线支付：充值申请提交失败！描述：code=" + errcode + "； msg=" + errmsg + "； 支付号：" + sdcustomno + "！";

                    Response.Write("<script type=\"text/javascript\"> window.top.location.href='" + Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx?errMsg=" + errorMessage + "';</script>");
                 
                    return;

                }
            }
            catch (Exception ex)
            {

                new Log("OnlinePay").Write(ex.Message);

                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().BaseType.FullName);

                return;

            }

        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;

        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAllowPageCache = false;

        base.OnLoad(e);
    }

    #endregion


    #region 内部函数

    /// <summary>
    /// 获取支付页面URL
    /// </summary>
    /// <param name="url">如果函数返回真,是支付URL,如果函数返回假,是错误信息</param>
    /// <returns>函数执行是否成功</returns>
    private bool GetPayUrl(out string url)
    {
        try
        {
            string sign = GetPaySign();

            url = paygateurl + "?customerid=" + customerid + "&sdcustomno=" + sdcustomno + "&ordermoney=" + ordermoney + "&cardno=" + cardno + "&faceno=" + faceno + "&cardnum="
                + cardnum + "&cardpass=" + cardpass + "&noticeurl=" + noticeurl + "&remarks=" + remarks
                + "&mark=" + mark + "&sign=" + sign;

            return true;
        }
        catch (Exception err)
        {
            url = "创建URL时出错,错误信息:" + err.Message;
            return false;
        }
    }

    /// <summary>
    /// 获取支付签名
    /// </summary>
    /// <returns>根据参数得到签名</returns>
    private string GetPaySign()
    {

        string sign_text = "customerid=" + customerid + "&sdcustomno=" + sdcustomno + "&noticeurl=" + noticeurl
            + "&mark=" + mark + "&key=" + key;

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

    // 获取 Url 返回的 Html 流
    public string GetHtml(string Url, string encodeing, int TimeoutSeconds)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        StreamReader reader = null;
        try
        {
            request = (HttpWebRequest)WebRequest.Create(Url);
            request.UserAgent = "www.svnhost.cn";
            if (TimeoutSeconds > 0)
            {
                request.Timeout = 1000 * TimeoutSeconds;
            }
            request.AllowAutoRedirect = false;

            response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(encodeing));
                string html = reader.ReadToEnd();
                return html;
            }
            else
            {
                return "";
            }
        }
        catch (SystemException)
        {
            return "";
        }
    }

    //获取返回内容列表
    private void GetResponseContents(string messageXml)
    {
        System.Xml.XmlDocument XmlDoc = new XmlDocument();
        System.Xml.XmlNodeList nodes = null;

        //先获取代理商的总账情况
        try
        {
            XmlDoc.Load(new StringReader(messageXml));
            nodes = XmlDoc.GetElementsByTagName("item");
        }
        catch { }

        if (nodes != null && nodes.Count > 0)
        {
            foreach (XmlNode item in nodes)
            {
                if (item.Attributes["name"] != null)
                {
                    switch (item.Attributes["name"].Value)
                    {
                        case "state":
                            state = item.Attributes["value"].Value;
                            break;
                        case "errcode":
                            errcode = item.Attributes["value"].Value;
                            break;
                        case "errmsg":
                            errmsg = item.Attributes["value"].Value;
                            break;
                        case "mark":
                            mark = item.Attributes["value"].Value;
                            break;

                    }
                }

            }

        }

    }

    #endregion

}
