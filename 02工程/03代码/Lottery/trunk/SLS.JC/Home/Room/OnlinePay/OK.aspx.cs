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
using System.Text;

public partial class Home_Room_OnlinePay_OK : RoomPageBase
{
    public string script = "";
    public string Url = "";
    protected void Page_Load(object sender, System.EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(Home_Room_OnlinePay_OK), this.Page);

        if (!IsPostBack)
        {
            BindData();
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

    private void BindData()
    {
        string errMsg = Shove._Web.Utility.GetRequest("errMsg");

        lab1.Text = string.IsNullOrEmpty(errMsg) == true ? _Site.Name : HttpUtility.UrlDecode(errMsg, Encoding.GetEncoding("GB2312"));

        long BuyID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("BuyID"), 0);
        if (BuyID > 0)
        {
            GoBuy(BuyID);
        }
        else
        {
            object fromURL = Shove._Web.Cache.GetCache("OnAlipayFromUrl");
            if (fromURL != null)
            {
                Shove._Web.Cache.ClearCache("OnAlipayFromUrl");
                Url = fromURL.ToString();
            }
            else
            {
                Url = "/Default.aspx";
            }
        }
    }

    private void GoBuy(long BuyID)
    {
        DataTable dtBuy = new DAL.Tables.T_AlipayBuyTemp().Open("", "ID=" + BuyID.ToString(), "");

        if (dtBuy != null && dtBuy.Rows.Count == 1)
        {
            string LotteryID = dtBuy.Rows[0]["LotteryID"].ToString();
            string Number = dtBuy.Rows[0]["Number"].ToString();

            if (LotteryID.Equals("72"))
            {
                Url = "/JCZC/BuyConfirm.aspx?BuyID=" + BuyID.ToString();
            }
            else
            {
                Url = "/Lottery/" + DataCache.LotterieBuyPage[Shove._Convert.StrToInt(LotteryID, 74)] + "?BuyID=" + BuyID.ToString();
            }

            HidBuyID.Value = "1";
        }
    }
}
