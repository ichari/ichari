using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Home_Room_MyPromotion_UserPromotionDetail : RoomPageBase
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
        string strDay = Shove._Web.Utility.GetRequest("dt");
        DateTime fromDate = DateTime.Now;
        DateTime toDate = DateTime.Now;
        try
        {
             fromDate=Convert.ToDateTime(strDay+" 0:0:0");
             toDate = fromDate.AddDays(1);
        }
        catch
        {
            Shove._Web.JavaScript.Alert(this.Page, "参数出错!");
            return;
        }

        string cacheKey = "Home_Room_MyPromotion_UserPromotionDetail_" + _User.ID + "_" + fromDate.ToString("yyyyMMdd") + toDate.ToString("yyyyMMdd");
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);

        if (dt == null)
        {

            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_CpsGetCommendMemberBuyDetail(ref ds, _Site.ID, _User.ID, -1, fromDate, toDate, ref returnValue, ref returnDescription);

            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (returnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, returnDescription);

                return;
            }
            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            dt = ds.Tables[0];
            Shove._Web.Cache.SetCache(cacheKey, dt);
        }

        PF.DataGridBindData(g, dt, gPager);
        gPager.Visible = g.PageCount > 1;
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
