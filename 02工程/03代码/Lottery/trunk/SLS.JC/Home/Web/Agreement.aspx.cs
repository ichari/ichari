using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Home_Web_Agreement : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            int LotteryID = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("LotteryID"), 5);

            if (LotteryID != 5 && LotteryID != 6 && LotteryID != 13 && LotteryID != 29 && LotteryID != 58 && LotteryID != 59 && LotteryID != 60)
            {
                LotteryID = 5;
            }

            BindData(LotteryID);
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    #endregion

    private void BindData(int LotteryID)
    {
        string CacheKey = "LotteryAgreements";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Lotteries().Open("[ID],[Code],[Agreement]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "");

            if (dt == null || dt.Rows.Count < 1)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 6000);
        }

        DataRow[] drA = dt.Select("[ID] = " + LotteryID.ToString());

        if (drA.Length > 0)
        {
            lbAgreement.Text = drA[0]["Agreement"].ToString().ToLower().Replace("[sitename]", _Site.Name).Replace("[siteurl]", Shove._Web.Utility.GetUrl());
            imgLogo.ImageUrl = "images/" + drA[0]["Code"].ToString().ToLower() + ".jpg";
        }
    }
}
