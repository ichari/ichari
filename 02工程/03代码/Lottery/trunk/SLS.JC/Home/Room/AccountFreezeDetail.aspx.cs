using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Home_Room_AccountFreezeDetail : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(_Site.ID.ToString() + "AccountFreezeDetail_" + _User.ID.ToString());

        if (dt == null)
        {
            int ReturnValue = 0;
            string ReturnDescription = "";

            DataSet ds = null;

            DAL.Procedures.P_GetUserFreezeDetail(ref ds, _Site.ID, _User.ID, ref ReturnValue, ref ReturnDescription);

            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_AccountFreezeDetail");

                return;
            }

            dt = ds.Tables[0];

            Shove._Web.Cache.SetCache(_Site.ID.ToString() + "AccountFreezeDetail_" + _User.ID.ToString(), dt);
        }

        PF.DataGridBindData(g, dt);

        //页面总计
        if (((gPager.PageIndex + 1) * gPager.PageSize) > dt.Rows.Count)
        {
            this.lblPageFreezeCount.Text = (dt.Rows.Count % gPager.PageSize).ToString();
        }
        else
        {
            this.lblPageFreezeCount.Text = gPager.PageSize.ToString();
        }
        this.lblPageFreezeSum.Text = PF.GetSumByColumn(dt, 1, true, gPager.PageSize,gPager.PageIndex).ToString("N");

        //总计
        this.lblTotalFreezeCount.Text = dt.Rows.Count.ToString();
        this.lblTotalFreezeSum.Text = PF.GetSumByColumn(dt, 1, false, gPager.PageSize,gPager.PageIndex).ToString("N");
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[1].Text = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0).ToString("N");
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
