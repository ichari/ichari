using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data;

public partial class Home_Room_OnlinePay_007ka_Receive : System.Web.UI.Page
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
    private string Sign = "";
    private string key="";
    private string Orderinfo="";
    string errorMessage = "";
    #endregion

    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SettingParams();
        }
        catch (Exception ee)
        {
            errorMessage = "通知错误：错误错误描述：007Ka回调页面参数列表！";
            new Log("OnlinePay").Write(errorMessage + "      错误信息:" + ee.Message);
            Response.End();

            return;
        }
        Response.Write("OK");
        key = so["OnlinePay_007Ka_Key"].ToString("").Trim();
        if (Sign != GetMD5(Orderinfo))
        {
            errorMessage = "通知错误：充值未完成。错误描述：认证签名失败！";
            new Log("OnlinePay").Write(errorMessage);
            Response.End();

            return;
        }
        if (MerID != so["OnlinePay_007Ka_MerchantId"].ToString("").Trim())
        {
            errorMessage = "通知错误：充值未完成。错误描述：商户号错误，数据非法！";
            new Log("OnlinePay").Write(errorMessage);
            Response.End();

            return;
        }
        if (MerAccount != so["OnlinePay_007Ka_MerAccount"].ToString("").Trim())
        {
            errorMessage = "通知错误：充值未完成。错误描述：银行账号错误，数据非法！";
            new Log("OnlinePay").Write(errorMessage);
            Response.End();

            return;
        }
        if (TranStat.ToString().Trim() != "1" && TranStat.ToString().Trim() != "29")
        {
            errorMessage = "通知错误：充值未完成。错误号："+TranStat+"错误描述："+TranInfo;
            new Log("OnlinePay").Write(errorMessage);
            Response.End();

            return;
        }
        DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result,UserID", "ID=" + OrderID, "");
        if (dt == null || dt.Rows.Count < 1)
        {
            errorMessage = "通知错误：充值未完成。错误描述：数据库错误。关联错误：获取支付流水号未成功。";
            new Log("OnlinePay").Write(errorMessage);
            Response.End();

            return;
        }
        else
        {
            if (Shove._Convert.StrToLong(dt.Rows[0][0].ToString(), 0) == 0)
            {
                string returnDescription = "";
                int returnValue = 1;
                double FormalitiesFeesScale = so["OnlinePay_007Ka_FormalitiesFees"].ToDouble(0) / 100;
                double FormalitiesFees = Math.Round((Shove._Convert.StrToDouble(Value, 0) / 100) * FormalitiesFeesScale, 2);
                DAL.Procedures.P_UserAddMoney(1, Shove._Convert.StrToLong(dt.Rows[0][1].ToString(), 0), Shove._Convert.StrToDouble(Value, 0) / 100.00 - FormalitiesFees, FormalitiesFees, OrderID, "007Ka", "系统交易号：" + OrderID + "007Ka支付", ref returnValue, ref returnDescription);
                if (returnValue < 0)
                {
                    errorMessage = "通知错误：充值未完成。错误描述：增加电子货币错误。错误原因：" + returnDescription;
                    new Log("OnlinePay").Write(errorMessage);
                    Response.End();

                    return;
                }
                else
                {
                    errorMessage = "007在线充值：充值完成";
                    new Log("OnlinePay").Write(errorMessage);
                    Response.End();

                    return;
                }
            }
            else
            {
                errorMessage = "007在线充值：充值完成";
                new Log("OnlinePay").Write(errorMessage);
                Response.End();

                return;
            }
        }
    }
    /// <summary>
    /// 获取回调数据
    /// </summary>
    private void SettingParams()
    {
        Orderinfo = Request.Params["Orderinfo"];
        Sign = Request.Form["Sign"];

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
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(encypStr + "|" + key, "MD5").ToUpper();
    }
}
