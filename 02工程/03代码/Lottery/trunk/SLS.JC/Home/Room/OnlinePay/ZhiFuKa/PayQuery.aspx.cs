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
using System.Xml;
using System.Data;

public partial class Home_Room_OnlinePay_ZhiFuKa_PayQuery : RoomPageBase
{
    #region 51支付卡变量

    string querygateurl = "http://202.75.218.94/gateway/orderquery.asp";

    private string customerid = "";                                                                 // 商户号（替换为自已的商户号）

    private string sdcustomno = "";                                                                 // 商户系统生成的订单号

    private string mark = "";                                                                       // 指令标识,每次指令都会有这个字段,51支付卡在处理完成后会原样返回.                                                                    

    private string key = "";                                                                        // 商户KEY（替换为自已的KEY）

    double ordermoney = -1;                                                                          // 实际充值的金额

    private string sd51no = "";                                                                     //51支付平台的订单ID

    private string state = "0000";

    SystemOptions so = new SystemOptions();


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

        //卖家商户号
        customerid = so["OnlinePay_ZhiFuKa_UserNumber"].Value.ToString();

        //卖家商户key
        key = so["OnlinePay_ZhiFuKa_MD5Key"].Value.ToString();

        sdcustomno = Shove._Web.Utility.GetRequest("sdcustomno");

        if (string.IsNullOrEmpty(sdcustomno) || string.IsNullOrEmpty(customerid) || string.IsNullOrEmpty(key))
        {

            Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：参数不齐全，无法提交查询！\"); window.location.href='';</script>");

            return;
        }

        string url = "";
        if (!GetQueryUrl(out url))
        {
            Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：" + url + "！\");window.location.href='';</script>");

            return;
        }
        else
        {
            string msg = "";

            if (GetResponseContents(GetHtml(url, "GB2312", 200), out msg))
            {

                Response.Write("<script type=\"text/javascript\">alert(\"支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：" + msg + "！\"); window.location.href='';</script>");

                return;
            }

            //如果，充值成功，那么我们完成充值

            try
            {
                string Memo = "系统交易号：" + sdcustomno + ",51支付交易号：" + sd51no;
                int ReturnValue = -1;
                string ReturnDescription = "";
                int Results = -1;

                DAL.Tables.T_UserPayDetails t_paydetails = new DAL.Tables.T_UserPayDetails();

                DataTable tmptTB = t_paydetails.Open("", "ID=" + sdcustomno, "");

                if (tmptTB == null || tmptTB.Rows.Count <= 0)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：充值处理失败，本条数据丢失。");

                    return;
                }

                double Money = (ordermoney == -1 ? ordermoney : Shove._Convert.StrToDouble(tmptTB.Rows[0]["Money"].ToString(), 0));
                long ID = Shove._Convert.StrToLong(tmptTB.Rows[0]["UserID"].ToString(), 0);
                double FormalitiesFees = Shove._Convert.StrToDouble(tmptTB.Rows[0]["FormalitiesFees"].ToString(), 0);

                string[] banks = tmptTB.Rows[0]["PayType"].ToString().Split('_');

                string PayBank = banks.Length < 2 ? "" : banks[1];

                Results = DAL.Procedures.P_UserAddMoney(_Site.ID, ID, Money, FormalitiesFees, sdcustomno,getBankName(PayBank), Memo, ref ReturnValue, ref ReturnDescription);

                if (Results < 0)
                {
                    Shove._Web.JavaScript.Alert(this.Page, "支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：数据库读写错误");

                    return;
                }
                else
                {
                    if (ReturnValue < 0)
                    {
                        Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                        return;
                    }

                    Shove._Web.JavaScript.Alert(this.Page, "此笔充值记录已到帐并已处理成功！");
                }
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "支付号为 " + sdcustomno + " 的支付记录没有充值成功，描述：查询失败，可能是网络通讯故障。请重试一次。");

                return;
            }

        }

    }


    #region 内部函数

    /// <summary>
    /// 获取支付页面URL
    /// </summary>
    /// <param name="url">如果函数返回真,是支付URL,如果函数返回假,是错误信息</param>
    /// <returns>函数执行是否成功</returns>
    private bool GetQueryUrl(out string url)
    {
        try
        {
            string sign = GetPaySign();

            url = querygateurl + "?customerid=" + customerid + "&sdcustomno=" + sdcustomno + "&mark=" + mark + "&sign=" + sign;

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

        string sign_text = "customerid=" + customerid + "&sdcustomno=" + sdcustomno + "&mark=" + mark + "&key=" + key;

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
    private bool GetResponseContents(string messageXml, out string sus_Msg)
    {
        bool result = false;
        sus_Msg = "";

        if (string.IsNullOrEmpty(messageXml))
        {
            result = true;
            sus_Msg = "不存在的充值记录！";

            return result;
        }


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
                        case "sd51no":
                            sd51no = item.Attributes["value"].Value;
                            break;
                        case "ordermoney":
                            ordermoney = Shove._Convert.StrToDouble(item.Attributes["value"].Value, -1);
                            break;
                        case "mark":
                            mark = item.Attributes["value"].Value;
                            break;

                    }
                }

            }

        }
        else
        {
            result = true;
            sus_Msg = "不存在的充值记录！";

            return result;
        }

        if (state == "0")
        {
            result = true;
            sus_Msg = "正在处理中……！";

            return result;
        }

        if (state == "2")
        {
            result = true;
            sus_Msg = "充值失败！";

            return result;
        }

        return result;

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




    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        isAllowPageCache = false;
    
        base.OnLoad(e);
    }

    #endregion

}
