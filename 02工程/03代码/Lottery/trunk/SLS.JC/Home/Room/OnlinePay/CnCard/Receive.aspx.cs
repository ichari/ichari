using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.Security;

public partial class Home_Room_OnlinePay_CnCard_Receive : System.Web.UI.Page
{
    protected string c_mid;			//商户编号，在申请商户成功后即可获得，可以在申请商户成功的邮件中获取该编号
    protected string c_order;		//商户提供的订单号
    protected string c_orderamount; //商户提供的订单总金额，以元为单位，小数点后保留两位，如：13.05
    protected string c_ymd;			//商户传输过来的订单产生日期，格式为"yyyymmdd"，如20050102
    protected string c_transnum;	//云网支付网关提供的该笔订单的交易流水号，供日后查询、核对使用；
    protected string c_succmark;	//交易成功标志，Y-成功 N-失败
    protected string c_moneytype;	//支付币种，0为人民币
    protected string c_cause;		//如果订单支付失败，则该值代表失败原因
    protected string c_memo1;		//商户提供的需要在支付结果通知中转发的商户参数一
    protected string c_memo1_UnEncrypt;
    protected string c_memo2;		//商户提供的需要在支付结果通知中转发的商户参数二
    protected string c_signstr;		//云网支付网关对已上信息进行MD5加密后的字符串


    SystemOptions so = new SystemOptions();

    protected void Page_Load(object sender, System.EventArgs e)
    {
        bool OnlinePay_CnCard_Status_ON = so["OnlinePay_CnCard_Status_ON"].ToBoolean(false); //&& PF.ValidCert(so["OnlinePay_CnCard_UserNumber"].ToString(), so["OnlinePay_CnCard_ON_Cert"].ToString(), "SPAYC");
        
        if (!OnlinePay_CnCard_Status_ON)
        {
            Response.Write("暂未启用");
            Response.End();
            return;
        }

        if (!this.IsPostBack)
        {
            try
            {
                c_mid = Request["c_mid"].ToString();//1028390
                c_order = Request["c_order"].ToString();//8000000347
                c_orderamount = Request["c_orderamount"].ToString();//1
                c_ymd = Request["c_ymd"].ToString();//20100108
                c_transnum = Request["c_transnum"].ToString();//15256025
                c_succmark = Request["c_succmark"].ToString();//Y
                c_moneytype = Request["c_moneytype"].ToString();//0
                string cause = Request["c_cause"];
                if (cause != null && cause != "")
                {
                    c_cause = cause.ToString();
                }
                c_memo1 = Request["c_memo1"].ToString();//05D20F4166330505E3
                c_memo1_UnEncrypt = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), c_memo1);
                string c_memo2Detail = Request["c_memo2"];
                if(c_memo2Detail != null && c_memo2Detail != "")
                {
                    c_memo2 = c_memo2Detail.ToString();
                }
                c_signstr = Request["c_signstr"].ToString();//fa41a9a74a36b48ae43fbde3e0e85036
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "返回参数出现异常");

                return;
            } 

            if (c_mid != so["OnlinePay_CnCard_UserNumber"].ToString(""))
            {
                Shove._Web.JavaScript.Alert(this.Page, "账户校验发生异常");

                return;
            }

            if (c_succmark != "Y" && c_succmark != "N")
            {
                Shove._Web.JavaScript.Alert(this.Page, "订单交易状态返回结果错误");

                return;
            }

            Sites _Site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];

            if (_Site == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "域名无效，限制访问");

                return;
            }

            Users user = new Users(_Site.ID);
            user.ID = Shove._Convert.StrToInt(c_memo1_UnEncrypt, -1);

            if (user.ID < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "异常用户数据");

                return;
            }
            string description = "";
            int Result = user.GetUserInformationByID(ref description);
            if ((Result != 0) && (Result != -3))
            {
                Shove._Web.JavaScript.Alert(this.Page, "帐号信息异常");

                return;
            }
            else
            {
                new Log("System").Write(description);
            }

            string srcStr = c_mid + c_order + c_orderamount + c_ymd + c_transnum + c_succmark + c_moneytype + c_memo1 + c_memo2 + so["OnlinePay_CnCard_MD5"].ToString("");
            if (c_signstr != FormsAuthentication.HashPasswordForStoringInConfigFile(srcStr, "MD5").ToLower())
            {
                Shove._Web.JavaScript.Alert(this.Page, "云网支付平台返回信息不一致");
                return;
            }

            if (c_succmark == "Y")
            {
                WriteUserAccount(user);
            }
            else
            {
                return;
            }

            Response.Write("<result>1</result><reURL>" + Shove._Web.Utility.GetUrl() + "/Room/OnlinePay/OK.aspx</reURL>");
            Response.End();
        }
    }

    private bool WriteUserAccount(Users user)
    {
        double Money = Shove._Convert.StrToDouble(c_orderamount, 0);
        if (Money == 0)
        {
            return false;
        }

        double FormalitiesFeesScale = so["OnlinePay_CnCard_CommissionScale"].ToDouble(0) / 100;
        double FormalitiesFees = Money - Math.Round(Money / (FormalitiesFeesScale + 1), 2);
        Money -= FormalitiesFees;

        string ReturnDescription = "";
        bool ok = (user.AddUserBalance(Money, FormalitiesFees, c_order, "云网支付," + c_transnum, "", ref ReturnDescription) == 0);
        if (!ok)
        {
            DataTable dt = new DAL.Tables.T_UserPayDetails().Open("Result", "[id] = " + Shove._Convert.StrToLong(c_order, 0).ToString(), "");

            if (dt == null || dt.Rows.Count == 0)
            {
                new Log(this.GetType().FullName).Write("返回的交易号找不到对应的数据");

                return false;
            }

            int IsOK = Shove._Convert.StrToInt(dt.Rows[0][0].ToString(), 0);
            if (IsOK == 1)
            {
                return true;
            }
            else
            {
                new Log(this.GetType().FullName).Write("对应的数据未处理");

                return false;
            }
        }

        return ok;
    }
}