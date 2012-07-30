using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using com.yeepay.bank;

public partial class Home_Room_OnlinePay_YeePay_Receive : RoomPageBase
{
    private static string p1_MerId;
    private static string keyValue;
    SystemOptions so = new SystemOptions();


    private string r0_Cmd;
    private string r1_Code;
    private string r2_TrxId;
    private string r3_Amt;
    private string r4_Cur;
    private string r5_Pid;
    private string r6_Order;
    private string r7_Uid;
    private string r8_MP;
    private string r9_BType;
    private string rp_PayDate;
    private string hmac;

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = false;
        isAllowPageCache = false;

        base.OnLoad(e);
    }

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    { //设置 log 文件地址
        Buy.LogFileName = "c:/YeePay_HTML.txt";
        if (!IsPostBack)
        {
            p1_MerId = so["OnlinePay_YeePay_UserNumber"].Value.ToString(); //"10000432521";                                     // 商家ID
            keyValue = so["OnlinePay_YeePay_MD5Key"].Value.ToString();//"8UPp0KE8sq73zVP370vko7C39403rtK1YwX40Td6irH216036H27Eb12792t";  // 商家密钥

            Buy.NodeAuthorizationURL = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/YeePay/Receive.aspx";//@"https://www.yeepay.com/app-merchant-proxy/node";   // 设置请求地址

            # region 获取易宝支付成功后的请求信息
            try
            {
                r0_Cmd = Request["r0_Cmd"].ToString().Trim();
                r1_Code = Request["r1_Code"].ToString().Trim();
                r2_TrxId = Request["r2_TrxId"].ToString().Trim();
                r3_Amt = Request["r3_Amt"].ToString().Trim();
                r4_Cur = Request["r4_Cur"].ToString().Trim();
                r5_Pid = Request["r5_Pid"].ToString().Trim();
                r6_Order = Request["r6_Order"].ToString().Trim();
                r7_Uid = Request["r7_Uid"].ToString().Trim();
                r8_MP = Request["r8_MP"].ToString().Trim();
                r9_BType = Request["r9_BType"].ToString().Trim();
                rp_PayDate = Request["rp_PayDate"].ToString().Trim();
                hmac = Request["hmac"].ToString().Trim();
            }
            catch
            {
                PF.GoError("YeePay_Receive", 101, "支付参数获取错误", "Home_Room_OnlinePay_YeePay_Receive");
                return;
            }
            #endregion

            Users user = new Users(_Site.ID)[_Site.ID, Shove._Convert.StrToLong(Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), r8_MP), 0)];
            
            //校验用户信息
            if (user == null || user.ID != _User.ID)
            {
                new Log("OnlinePay").Write("用户帐号信息不匹配！");
                return;
            }

            // 校验返回数据包
            BuyCallbackResult result = Buy.VerifyCallback(p1_MerId, keyValue, r0_Cmd, r1_Code, r2_TrxId, r3_Amt, r4_Cur, r5_Pid, r6_Order, r7_Uid, r8_MP, r9_BType, rp_PayDate, hmac);
            //Buy.VerifyCallback(p1_MerId, keyValue, Buy.GetQueryString("r0_Cmd"), Buy.GetQueryString("r1_Code"), Buy.GetQueryString("r2_TrxId"),
            //Buy.GetQueryString("r3_Amt"), Buy.GetQueryString("r4_Cur"), Buy.GetQueryString("r5_Pid"), Buy.GetQueryString("r6_Order"), Buy.GetQueryString("r7_Uid"),
            //Buy.GetQueryString("r8_MP"), Buy.GetQueryString("r9_BType"), Buy.GetQueryString("rp_PayDate"), Buy.GetQueryString("hmac"));

            if (string.IsNullOrEmpty(result.ErrMsg))
            {
                if (result.R1_Code == "1")
                {
                    bool isPaySuccess = WriteUserAccount(user, r6_Order, Shove._Convert.StrToDouble(result.R3_Amt, 0), "易宝支付：订单号：" + result.R6_Order + "，支付金额：" + result.R3_Amt);
                    if (result.R9_BType == "1")
                    {
                        if (isPaySuccess)
                        {
                            Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx");
                        }
                        else
                        {
                            Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx");
                        }
                        //  callback方式:浏览器重定向
                        // Response.Write("支付成功!<br>商品ID:" + result.R5_Pid + "<br>商户订单号:" + result.R6_Order + "<br>支付金额:" + result.R3_Amt + "<br>易宝支付交易流水号:" + result.R2_TrxId + "<BR>");
                    }
                    else if (result.R9_BType == "2")
                    {
                        // * 如果是服务器返回或者电话支付返回(result.R9_BType==2 or result.R9_BType==3)则需要回应一个特定字符串'SUCCESS',且在'SUCCESS'之前不可以有任何其他字符输出,保证首先输出的是'SUCCESS'字符串
                        Response.Write("SUCCESS");

                        if (isPaySuccess)
                        {
                            Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/OK.aspx");
                        }
                        else
                        {
                            Response.Redirect(Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/Fail.aspx");
                        }
                    }
                }
                else
                {
                    new Log("OnlinePay").Write("易宝支付失败!");
                }
            }
            else
            {
                new Log("OnlinePay").Write("交易签名无效" + result.ErrMsg);
            }
        }
    }

    private bool WriteUserAccount(Users thisUser, string PayNumber, double PayMoney, string Memo)
    {
        if (PayMoney <= 0)
        {
            return false;
        }

        double formalitiesFeesScale = so["OnlinePay_YeePay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double formalitiesFees = PayMoney - Math.Round(PayMoney / (formalitiesFeesScale + 1), 2);
        double addMoney = PayMoney - formalitiesFees;

        string ReturnDescription = "";
        bool ok = (thisUser.AddUserBalance(addMoney, formalitiesFees, PayNumber, "易宝", Memo, ref ReturnDescription) == 0);

        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(PayNumber, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log("OnlinePay").Write("[易宝]在线支付：返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);

            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log("OnlinePay").Write("[易宝]在线支付：对应的数据未处理");

                return false;
            }
        }

        return ok;
    }
}
