using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Cryptography;
using System.Text;
public partial class Home_Room_OnlinePay_007ka_QueryResult : System.Web.UI.Page
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
        key = so["OnlinePay_007Ka_Key"].ToString("");
        try
        {
            SettingParams();
        }
        catch
        {
            errorMessage = "007订单查询结果：回调参数有误！";
            new Log("OnlinePay").Write(errorMessage);
            Shove._Web.JavaScript.Alert(this.Page, errorMessage);

            return;
        }
        try
        {
            if (Request.QueryString.AllKeys.Length < 1)
            {
                errorMessage = OrderID+"订单查询结果：该数据传输丢失！";
                new Log("OnlinePay").Write(errorMessage);
                Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                return;
            }
            if (SignCounterpart != GetMD5(Orderinfo + "|" + key))
            {
                errorMessage = OrderID + "订单查询结果：认证签名失败！";
                new Log("OnlinePay").Write(errorMessage);
                Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                return;
            }
            if (TranStat.ToString().Trim() != "1" && TranStat.ToString().Trim() != "29")
            {
                errorMessage = OrderID + "订单查询结果：007手机卡充值失败！错误号：" + TranStat.ToString() + " 错误描述：" + TranInfo + "(" + GetTranStatToStringCH(Shove._Convert.StrToInt(TranStat.ToString(),0)) + ")";
                new Log("OnlinePay").Write(errorMessage);
                Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                return;
            }

            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result,UserID", "ID=" + OrderID, "");
            if (dt == null || dt.Rows.Count < 1)
            {
                errorMessage = OrderID+"订单查询结果：支付号不存在，数据可能已近丢失！";
                new Log("OnlinePay").Write(errorMessage);
                Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                return;
            }
            else
            {
                if (Shove._Convert.StrToInt(dt.Rows[0][1].ToString(), 0) != 0)
                {
                    if (Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0) == 1)
                    {
                        errorMessage = OrderID + "订单充值已经成功!";
                        Response.Write("OK");
                        new Log("OnlinePay").Write(errorMessage);
                        Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                        return;
                    }
                    int returnValue = -20000;
                    string returnDescription = "";
                    double FormalitiesFeesScale = so["OnlinePay_007Ka_FormalitiesFees"].ToDouble(0) / 100;
                    double FormalitiesFees = Math.Round((Shove._Convert.StrToDouble(Value, 0) / 100) * FormalitiesFeesScale, 2);
                    long userid = Shove._Convert.StrToLong(dt.Rows[0][1].ToString(), 0);
                    DAL.Procedures.P_UserAddMoney(1, userid, Shove._Convert.StrToDouble(Value, 0) / 100.00 - FormalitiesFees, FormalitiesFees, OrderID, "007KA", "007KA系统交易号" + OrderID, ref returnValue, ref returnDescription);
                    if (returnValue < 0)
                    {
                        errorMessage = OrderID+"订单查询结果：增加电子货币错误。错误原因：" + returnDescription;
                        new Log("OnlinePay").Write(errorMessage);
                        Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                        return;
                    }
                    else
                    {
                        errorMessage = OrderID+"订单充值成功!";
                        Response.Write("OK");
                        new Log("OnlinePay").Write(errorMessage);
                        Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                        return;
                    }
                }
                else
                {
                    errorMessage = OrderID+"订单查询结果：充值未完成！错误原因：用户不存在";
                    new Log("OnlinePay").Write(errorMessage);
                    Shove._Web.JavaScript.Alert(this.Page, errorMessage);

                    return;
                }
            }
        }
        catch
        {
            errorMessage = OrderID+"订单查询结果：错误描述：出现未知异常！";
            new Log("OnlinePay").Write(errorMessage);
            Shove._Web.JavaScript.Alert(this.Page, errorMessage);

            return;
        }
    }
    /// <summary>
    /// 获取回调数据
    /// </summary>
    private void SettingParams()
    {
        Orderinfo = Shove._Web.Utility.GetRequest("Orderinfo");
        Orderinfo=System.Web.HttpUtility.UrlDecode(Orderinfo);
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
        SignCounterpart = strArray[13];
        Orderinfo = Orderinfo.Replace("|"+SignCounterpart, "");
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

    //充值状态
    private string GetTranStatToStringCH(int state)
    {
        switch (state)
        { 
            case 1:
                return "交易成功";
            case 4:
                return "交易处理中";
            case 5:
                return "错误交易指令";
            case 6:
                return "接口版本号错误";
            case 7:
                return "代理商校验错误";
            case 8:
                return "代理商不存在";
            case 9:
                return "本次查询出现未知错误";
            case 14:
                return "交易已经过期";
            case 18:
                return "交易结果不能确定";
            case 19:
                return "无效充值卡";
            case 21:
               return  "代理商已经暂停交易";
            case 24:
               return "不能为该用户充值";
            case 28:
               return "成功金额小于申报金额";
            case 29:
               return "成功金额大于申报价格，交易成功";
            case 31:
               return "本条交易信息不存在";
            default:
               return state.ToString();
        }
    }
}
