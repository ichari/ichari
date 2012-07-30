using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Home_Room_OnlinePay_ZhiFuKa_Default :RoomPageBase
{
    public string Balance;
    public string UserName;

    SystemOptions so = new SystemOptions();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (_User != null)
        {
            Balance = _User.Balance.ToString();
            UserName = _User.Name.ToString();
        }

        if (!IsPostBack)
        {
            if (Shove._Web.Utility.GetRequest("cardno") != "zfk")
            {
                radSZX.Checked = true;

                imgCardType.ImageUrl = "../Alipay02/images/bank_logo/logo_szx.gif";

                tb_szx.Visible =  true;
                tb_51zfk.Visible = false;
            }
            else
            {
                rad51ZFK.Checked = true;
                imgCardType.ImageUrl = "../Alipay02/images/bank_logo/logo_51zfk.gif";

                tb_szx.Visible = false;
                tb_51zfk.Visible = true;
            }

            labFeesScale_zfk.Text = labFeesScale_szx.Text = so["OnlinePay_ZhiFuKa_PayFormalitiesFeesScale"].Value.ToString() + " %";

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

    protected void lbReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("../default.aspx?BuyId=" + Shove._Web.Utility.GetRequest("BuyID"));

    }

    protected string getReallityMoney(int OrialMoney)
    {
        double reallityMoney = 0.00;

        double FormalitiesFeesScale = so["OnlinePay_ZhiFuKa_PayFormalitiesFeesScale"].ToDouble(0) / 100;

        reallityMoney = OrialMoney - OrialMoney * FormalitiesFeesScale;

        return reallityMoney.ToString();
    }

    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        string ordermoney = "0";
        string cardno = "";
        string faceno = "";
        string cardnum = "";
        string cardpass = "";

        if (radSZX.Checked)
        {
            ordermoney = rblist_szx.SelectedValue;

            cardno = "szx";

            faceno = cardno + ordermoney.ToString().PadLeft(3, '0');

            cardnum = txtCardNum_szx.Text.Trim();

            cardpass = txtCardPass_szx.Text.Trim();

        }
        else
        {
            ordermoney = rblist_zfk.SelectedValue;

            cardno = "zfk";

            faceno = cardno + ordermoney.ToString().PadLeft(3, '0');

            cardnum = txtCardNum_zfk.Text.Trim();

            cardpass = txtCardPass_zfk.Text.Trim();

            if (ordermoney == "1")
            {
                ordermoney = "0.01";
            }
        }

        Response.Redirect("send.aspx?ordermoney=" + ordermoney + "&cardno=" + cardno + "&faceno=" + faceno + "&cardnum=" + cardnum + "&cardpass=" + cardpass + "&BuyId=" + Shove._Web.Utility.GetRequest("BuyID"));

    }
}
