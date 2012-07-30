using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

public partial class Home_Room_OnlinePay_007ka_PayQuery : RoomPageBase
{
    #region 接口参数
    private string MerID = "";
    private string MerAccount = "";
    private string OrderID = "";
    private string ReplyFormat = "xml";
    private string Command = "1";
    private string InterfaceName = "007KA_SRH";
    private string InterfaceNumber = "1.0.0.1";
    private string MerURL = "";
    private string Attach = "";
    private string key = "";
    public string Sign = "";
    public string Orderinfo = "";
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        SystemOptions so=new SystemOptions ();
        MerID = so["OnlinePay_007Ka_MerchantId"].ToString("").Trim();
        MerAccount = so["OnlinePay_007Ka_MerAccount"].ToString("").Trim();
        OrderID =Shove._Web.Utility.GetRequest("OrderID");
        key = so["OnlinePay_007Ka_Key"].ToString("").Trim();
        MerURL = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/007ka/QueryResult.aspx";
        Attach = MerURL;
        SettingOrderInfo();
        Sign = GetMD5(Orderinfo+"|"+key);

        string return_Orderinfo = PF.GetHtml("http://www.007ka.cn/interface/cardpay/query.php?Orderinfo=" + Orderinfo + "&Sign=" + Sign, "GBK", 1);
        return_Orderinfo=System.Web.HttpUtility.UrlEncode(return_Orderinfo);
        Response.Redirect("QueryResult.aspx?Orderinfo=" + return_Orderinfo, true);
    }

    private void SettingOrderInfo()
    {
        Orderinfo = MerID+"|"+MerAccount+"|"+OrderID+"|"+ReplyFormat+"|"+Command+"|"+
             InterfaceName + "|" + InterfaceNumber + "|" + MerURL + "|" + Attach;
    }
    /// <summary>
    /// 获取大写的MD5签名结果
    /// </summary>
    /// <param name="encypStr"></param>
    /// <returns></returns>
    private string GetMD5(string encypStr)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encypStr,"MD5").ToUpper(); //  '先MD5 32 然后转大写

    }


}