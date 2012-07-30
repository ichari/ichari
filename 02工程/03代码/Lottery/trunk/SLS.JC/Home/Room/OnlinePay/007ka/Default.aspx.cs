using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

public partial class Home_Room_OnlinePay_007ka_Default : RoomPageBase
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
            labFeesScale.Text =so["OnlinePay_007Ka_FormalitiesFees"].ToString("")+"%";

            GetBlankURL();
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


    protected string getReallityMoney(int OrialMoney)
    {
        double reallityMoney = 0.00;

        double FormalitiesFeesScale = so["OnlinePay_007Ka_FormalitiesFees"].ToDouble(0) / 100;

        reallityMoney = OrialMoney - OrialMoney * FormalitiesFeesScale;

        return reallityMoney.ToString();
    }

    private void GetBlankURL()
    {
        string ordermoney = "0";
        string cardno = "007KA";
        ordermoney = rblist.SelectedValue;
        imgbtn_OK.NavigateUrl = Shove._Web.Utility.GetUrl() + "/Home/Room/OnlinePay/007ka/send.aspx?ordermoney=" + ordermoney + "&cardno=" + cardno + "&BuyId=" + Shove._Web.Utility.GetRequest("BuyID");
    }
    protected void rblist_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetBlankURL();
    }
}
