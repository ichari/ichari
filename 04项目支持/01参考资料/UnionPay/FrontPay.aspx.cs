using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.unionpay.upop.sdk;

public partial class Home_Room_OnlinePay_UnionPay_FrontPay : RoomPageBase
{
    SystemOptions so = new SystemOptions();
   
    protected void Page_Load(object sender, EventArgs e)
    {
   
    }
    protected void btnNext_Click(object sender, System.EventArgs e)
    {
        string Money = this.PayMoney.Text;
        double dMoney = Shove._Convert.StrToDouble(Money, 0);
        if (dMoney < 2)
        {
            Shove._Web.JavaScript.Alert(this.Page, "存入金额最少2元, 请输入正确的充值金额！再提交，谢谢！");
            return;
        }
        //// calculate transaction fee and add to total cost
        double FormalitiesFeesScale = so["OnlinePay_Alipay_PayFormalitiesFeesScale"].ToDouble(0) / 100;
        double FormalitiesFees = Math.Round(dMoney * FormalitiesFeesScale, 2);

        string sURL = "/Home/Room/OnlinePay/UnionPay/FrontSend.aspx?m=" + dMoney.ToString() + "&f=" + FormalitiesFees.ToString();
        Response.Write("<script type='text/javascript'>window.top.location = '" + sURL + "';</script>");
    }
}