using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
public partial class Home_Room_OnlinePay_007ka_Receive1:RoomPageBase
{
    #region 接口参数
    private string MerID = "";
    private string MerAccount = "";
    private string OrderID = "";
    private string TranStat = "";
    private string TranInfo = "";
    private string CardType = "";
    private string Value = "";
    private string Command = "";
    private string InterfaceName = "";
    private string InterfaceNumber = "";
    private string Datetime = "";
    private string TranOrder = "";
    private string Attach = "";
    private string Orderinfo = "";
    private string SignCounterpart = "";
    private string key = "";
    string errorMessage = "";
    #endregion

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        string returnUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/";
        try
        {
            SettingParams();
        }
        catch (Exception ee)
        {
            errorMessage = "007在线充值：充值未完成。错误描述：参数有误 详细：" + ee.Message;
            new Log("OnlinePay").Write(errorMessage);

            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        key = so["OnlinePay_007Ka_Key"].ToString("");
        if (Request.QueryString.AllKeys.Length < 1)
        {
            errorMessage = "007在线充值：充值未完成。错误描述：数据传输错误！";
            new Log("OnlinePay").Write(errorMessage);

            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        if (SignCounterpart != GetMD5(Orderinfo))
        {
            errorMessage = "007在线充值：充值未完成。错误描述：认证签名失败！";
            new Log("OnlinePay").Write(errorMessage);
            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        if (MerID != so["OnlinePay_007Ka_MerchantId"].ToString("").Trim())
        {
            errorMessage = "007在线充值：充值未完成。错误描述：商户号错误，数据非法！";
            new Log("OnlinePay").Write(errorMessage);
            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        if (MerAccount != so["OnlinePay_007Ka_MerAccount"].ToString("").Trim())
        {
            errorMessage = "007在线充值：充值未完成。错误描述：商户银行账号错误，数据非法！";
            new Log("OnlinePay").Write(errorMessage);
            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        if (TranStat.ToString().Trim() != "1" && TranStat.ToString().Trim() != "29")
        {
            errorMessage = "007充值失败。";
            new Log("OnlinePay").Write(errorMessage + " 错误号：" + TranStat.ToString()+" 错误描述:"+TranInfo);
            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result,UserID", "ID=" + OrderID, "");
        if (dt == null || dt.Rows.Count < 1)
        {
            errorMessage = "007在线充值：充值未完成。错误描述：生成支付流水号未成功。";
            new Log("OnlinePay").Write(errorMessage);
            Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

            return;
        }
        else
        {
            if (Shove._Convert.StrToLong(dt.Rows[0][1].ToString(), 0) != 0)
            {
                if (Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0) == 1)
                {
                    errorMessage = OrderID + "订单充值已经成功!";
                    Response.Write("OK");
                    new Log("OnlinePay").Write(errorMessage);
                    Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                    return;
                }
                string returnDescription = "";
                double FormalitiesFeesScale = so["OnlinePay_007Ka_FormalitiesFees"].ToDouble(0) / 100;
                double FormalitiesFees = Math.Round((Shove._Convert.StrToDouble(Value, 0)/100) * FormalitiesFeesScale, 2);
                Users user = new Users(_Site.ID)[_Site.ID, Shove._Convert.StrToLong(dt.Rows[0][1].ToString(), 0)];
                int temp = user.AddUserBalance(Shove._Convert.StrToDouble(Value, 0)/100.0 - FormalitiesFees, FormalitiesFees, OrderID, "007Ka", "系统交易号：" + OrderID + "007Ka支付", ref returnDescription);
                if (temp < 0)
                {
                    errorMessage = "增加电子货币错误。错误原因：" + returnDescription;
                    new Log("OnlinePay").Write(errorMessage);
                    Response.Redirect(returnUrl + "Fail.aspx?errMsg=" + errorMessage);

                    return;
                }
                else
                {
                    Response.Write("OK");
                    errorMessage = "007在线充值：充值完成！";
                    new Log("OnlinePay").Write(errorMessage);
                    Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?BuyID=" + Attach);

                    return;
                }
            }
            else
            {
                errorMessage = "007在线充值：充值未完成。错误描述：用户不存在！";
                new Log("OnlinePay").Write(errorMessage);
                Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx?BuyID=" + Attach);

                return;
            }
        }
    }
    /// <summary>
    /// 获取回调数据
    /// </summary>
    private void SettingParams()
    {
        Orderinfo = Shove._Web.Utility.GetRequest("Orderinfo");
        SignCounterpart = Shove._Web.Utility.GetRequest("Sign");
        string[] strArray = Orderinfo.Split('|');
        MerID = strArray[0];
        MerAccount = strArray[1];
        OrderID = strArray[2];
        TranStat = strArray[3];
        TranInfo = strArray[4];
        CardType = strArray[5];
        Value = strArray[6];
        Command = strArray[7];
        InterfaceName = strArray[8];
        InterfaceNumber = strArray[9];
        Datetime = strArray[10];
        TranOrder = strArray[11];
        Attach = strArray[12];
    }
    /// <summary>
    /// 获取大写的MD5签名结果
    /// </summary>
    /// <param name="encypStr"></param>
    /// <returns></returns>
    private string GetMD5(string encypStr)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encypStr+"|"+key, "MD5").ToUpper();
    }
}

