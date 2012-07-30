using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

public partial class Home_Room_AccountWinMoney : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindRewardData();
    }
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
          
            case "gReward":
                BindRewardData();
                break;
         
        }

    }
    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {
            case "gPagerReward":
                hdCurDiv.Value = "divReward";
                BindRewardData();
                break;
        }
        BindRewardData();
    }
    private void BindRewardData()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_Welcome_Reward_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("UserID, SchemeID, LotteryNumber, IsuseName, SchemeWinMoney, LotteryName, WinMoneyNoWithTax,DetailMoney", "[UserID] = " + _User.ID.ToString() + " and EndTime < GetDate() and WinMoneyNoWithTax > 0", "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 60);
        }

        gReward.DataSource = dt;
        gReward.DataBind();

        this.lblRewardCount.Text = dt.Rows.Count.ToString();
        this.lblRewardMoney.Text = PF.GetSumByColumn(dt, 6, false, gPagerReward.PageSize, gPagerReward.PageIndex).ToString("N");
    }
    protected void gReward_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");


            if (money > 0)
            {
                e.Item.Cells[7].Text = "<font color=\"red\">中奖啦!</font>";
            }
            else
            {
                e.Item.Cells[7].Text = "未中奖";
            }

            e.Item.ToolTip = e.Item.Cells[2].Text;

            e.Item.Cells[2].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + e.Item.Cells[8].Text + "' target='_blank'>投注内容</a></span>";

            double winMoneyNoWithTax = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);  //我的奖金
            double detailMoney = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);        //我投注的金额
            double winMoney = (winMoneyNoWithTax - detailMoney);

            double winRate = winMoney / detailMoney;

            if (winRate >= 1)
            {
                e.Item.Cells[6].Text = Math.Round(winRate, 2).ToString() + "倍";
            }
            else
            {
                e.Item.Cells[6].Text = (Math.Round(winRate, 2) * 100).ToString() + "%";
            }

            if (winMoney < 0)
            {
                e.Item.Cells[6].Text = "";
            }
        }
    }
}
