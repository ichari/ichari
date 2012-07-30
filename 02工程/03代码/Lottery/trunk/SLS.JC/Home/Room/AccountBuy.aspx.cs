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
using System.Text;

public partial class Home_Room_BuyAccount : RoomPageBase
{
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindHistoryData()
    {
        string CacheKey = "Home_Room_Invest_BindHistoryData" + _User.ID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
 
        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(@"select a.LotteriesID as LotteryID, a.LotteriesName as LotteryName, a.ID as PlayTypeID, tu.Name as InitiateName, a.PlayTypesName as PlayTypeName, ts.Share as SchemeShare, ts.Money, tb.Share, tb.DetailMoney, ts.WinMoney as SchemeWinMoney, tb.WinMoneyNoWithTax as WinMoneyNoWithTax, tb.DateTime, ts.ID as SchemeID,tb.QuashStatus, ts.Buyed, ts.AssureMoney, ts.BuyedShare as BuyedShare, ts.isOpened as IsOpened
                        from T_BuyDetails tb inner join 
                        T_Schemes ts on ts.ID = tb.SchemeID
                        inner join (
                        select T_PlayTypes.ID, T_PlayTypes.Name as PlayTypesName, T_Lotteries.Name as LotteriesName, T_Lotteries.ID as LotteriesID from T_PlayTypes inner join
                        T_Lotteries on T_PlayTypes.LotteryID = T_Lotteries.ID) as a on ts.PlayTypeID = a.ID
                        inner join T_Users tu on ts.InitiateUserID = tu.ID and tb.UserID = " + _User.ID.ToString() + " order by tb.DateTime desc");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 60);
        }
       
        PF.DataGridBindData(gHistory, dt, gPagerHistory);

        int count = 0;
        double money = 0;
        foreach (DataRow dr in dt.Rows)
        {
            if (dr["QuashStatus"].ToString() == "0")
            {
                count++;
                money += Shove._Convert.StrToDouble(dr["DetailMoney"].ToString(), 0);
            }
        } 
        lblBuyOutCount.Text = count.ToString();
        lblBuyOutMoney.Text = money.ToString("N");
        gPagerHistory.Visible = true;
    }
    protected void gHistory_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[13].Text, 0);
            string str = e.Item.Cells[2].Text;

            if (str.Length > 6)
            {
                str = str.Substring(0, 5) + "..";
            }

            e.Item.Cells[2].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + str + "</a></span>";

            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[6].Text, 0);
            e.Item.Cells[6].Text = (money == 0) ? "" : money.ToString("N");

            double i = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            double j = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);

            if (j >= i)
            {
                e.Item.Cells[7].Text = "100%";
            }
            else
            {
                if (i > 0)
                {
                    e.Item.Cells[7].Text = Math.Round(j / i * 100, 2).ToString() + "%";
                }
            }

            money = Shove._Convert.StrToDouble(e.Item.Cells[8].Text, 0);
            e.Item.Cells[8].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[9].Text, 0);
            e.Item.Cells[9].Text = (money == 0) ? "" : money.ToString("N");

            if (money == 0)
            {

                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"blue\"> 未中奖 </font>" + "</a>";
                e.Item.Cells[10].Style.Add(HtmlTextWriterStyle.Color, "FFFFCC");

            }
            else
            {
                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"red\">中奖啦!</font>" + "</a>";
            }

            bool IsOpened = false;

            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                DataRow dr = drv.Row;

                IsOpened = Shove._Convert.StrToBool(dr["IsOpened"].ToString(), false);

                if (!IsOpened)
                {
                    e.Item.BackColor = System.Drawing.Color.FromName("#FFFFCA");
                    e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"blue\">未开奖</font>" + "</a>";
                    e.Item.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFCA");
                }
            }
            catch { }

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[14].Text, 1);

            if (QuashStatus != 0)
            {
                string StatusDescription = "已撤单";

                e.Item.Cells[12].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + StatusDescription + "</a></span>";
            }
            else
            {
                if (!IsOpened)
                {

                    bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[15].Text, false);

                    if (Buyed)
                    {
                        e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<Font color=\'Red\'>已成功</font>" + "</a>";
                    }
                    else
                    {
                        int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[17].Text, 0);
                        int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[3].Text, 0);

                        if (BuyedShare >= SchemeShare)
                        {
                            e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"blue\">" + "<Font color=\'Red\'>已满员</font>" + "</a>";
                        }
                        else
                        {
                            e.Item.Cells[12].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未满员" + "</a></span>";
                        }
                    }
                }
                else
                {
                    e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"red\">已成功</font>" + "</a>";
                }
            }
        }
    }
    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;

                BindHistoryData();

    }
    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {

            case "gPagerHistory":
                hdCurDiv.Value = "divBuy";
                BindHistoryData();
                break;
          
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindHistoryData();
    }
}
