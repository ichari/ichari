using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text;

public partial class Home_Room_MyPromotion_SitePromotionDetail : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindUserAccount();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindUserAccount()
    {
        string strDateTime = Shove._Web.Utility.GetRequest("dt");

        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("SiteID={0} and Buyed=1 and replace(CONVERT(char(10),[DateTime],111),'/','-')='{1}' ", _Site.ID, strDateTime);
        sb.AppendFormat("and UserID in (Select ID from T_Users where CpsID in (Select ID from T_Cps where CommendID={0})) ", _User.ID);

        DataTable dt = new DAL.Views.V_BuyDetails().Open("IsuseName, SchemeNumber, LotteryName, PlayTypeName,Money,DetailMoney, DateTime", sb.ToString(), "DateTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");

            e.Item.Cells[6].Text = Shove._Convert.StrToDateTime(e.Item.Cells[6].Text, "2009-01-01").ToShortDateString();
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindUserAccount();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindUserAccount();
    }
}
