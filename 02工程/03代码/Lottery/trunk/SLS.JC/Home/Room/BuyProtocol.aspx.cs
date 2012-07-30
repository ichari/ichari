using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Room_BuyProtocol : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Request["LotteryID"], 5);
            bindData(LotteryID);
        }
    }

    private void bindData(int lotteryID)
    {
        
        DAL.Tables.T_Lotteries dt = new DAL.Tables.T_Lotteries();
        DataTable dtAgreement  = dt.Open("Agreement","ID = "+lotteryID+"","");

        if (dtAgreement != null && dtAgreement.Rows.Count > 0)
        {
            string aggrement = dtAgreement.Rows[0]["Agreement"].ToString();
            this.lbAgreement.Text = aggrement.Replace("[SiteName]", _Site.Name).Replace("[SiteUrl]",_Site.Url);
        }
    }
}
