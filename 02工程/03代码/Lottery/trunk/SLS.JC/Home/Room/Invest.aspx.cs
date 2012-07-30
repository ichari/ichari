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

public partial class Home_Room_Invest : RoomPageBase
{
    int[] arrLotteries = new int[] { 5, 6, 29, 39 };

    protected string Script = "ShowOrHiddenDiv(\"divReward\");clickTabMenu(document.getElementById(\"tdReward\"), 'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();
            BindHistoryData();
            BindDataReward();

            if (Shove._Web.Utility.GetRequest("Type") == "1")
            {
                hdCurDiv.Value = "divHistory";
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        ddlLottery.Items.Clear();
        ddlLottery.Items.Add(new ListItem("全部彩种", "-1"));

        if (_Site.UseLotteryList == "")
        {
            PF.GoError(ErrorNumber.Unknow, "暂无玩法", this.GetType().FullName);

            return;
        }

        string CacheKey = "dtLotteriesUseLotteryList";
        DataTable dtLotteries = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dtLotteries == null)
        {
            dtLotteries = new DAL.Tables.T_Lotteries().Open("[ID], [Name], [Code]", "[ID] in(" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[ID]");

            if (dtLotteries == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName + "(-46)");

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dtLotteries, 6000);
        }

        for (int i = 0; i < dtLotteries.Rows.Count; i++)
        {
            string LotteryID = dtLotteries.Rows[i]["ID"].ToString();

            //玩法信息缓存 6000 秒
            CacheKey = "dtVPlayTypes_" + LotteryID.ToString();
            DataTable dtPlayTypes = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dtPlayTypes == null)
            {
                dtPlayTypes = new DAL.Views.V_PlayTypes().Open("", "LotteryID = " + LotteryID.ToString(), "[ID]");

                if (dtPlayTypes == null || dtPlayTypes.Rows.Count < 1)
                {
                    PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                    return;
                }

                Shove._Web.Cache.SetCache(CacheKey, dtPlayTypes, 6000);
            }

            //if (arrLotteries.Contains(Shove._Convert.StrToInt(dtLotteries.Rows[i]["ID"].ToString(), 0)))
            //{
                ddlLottery.Items.Add(new ListItem(dtPlayTypes.Rows[0]["LotteryName"].ToString(), dtPlayTypes.Rows[0]["LotteryID"].ToString()));
            //}
        }

        if (ddlLottery.Items.Count > 0)
        {
            BindDataForPlayType(ddlLottery.Items[0].Value);
        }
    }

    private void BindDataForPlayType(string LotteryID)
    {
        ddlPlayType.Items.Clear();
        ddlPlayType.Items.Add(new ListItem("全部玩法", "-1"));

        DataTable dt;

        dt = new DAL.Views.V_PlayTypes().Open("ID,LotteryID,Name,LotteryName,BuyFileName", "LotteryID=" + LotteryID, "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "暂无玩法", this.GetType().FullName);

            return;
        }

        for (int j = 0; j < dt.Rows.Count; j++)
        {
            ddlPlayType.Items.Add(new ListItem(dt.Rows[j]["Name"].ToString(), dt.Rows[j]["ID"].ToString()));
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        Shove.Web.UI.ShoveGridPager gPager = (Shove.Web.UI.ShoveGridPager)Sender;

        if (gPager.ID == "gPagerHistory")
        {
            hdCurDiv.Value = "divHistory";
            BindHistoryData();
            Script = "ShowOrHiddenDiv(\"divHistory\");clickTabMenu(document.getElementById(\"tdHistory\"),'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";
        }
        else if (gPager.ID == "gPager_Reward")
        {
            hdCurDiv.Value = "divReward";
            BindDataReward();
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        Script = "ShowOrHiddenDiv(\"divHistory\");clickTabMenu(document.getElementById(\"tdHistory\"),'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";
        BindDataForPlayType(ddlLottery.SelectedValue);
    }

    protected void btnGo_Click(object sender, System.EventArgs e)
    {
        if (ddlLottery.SelectedIndex < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择彩种。");

            return;
        }
        Script = "ShowOrHiddenDiv(\"divHistory\");clickTabMenu(document.getElementById(\"tdHistory\"),'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";

        BindHistoryData();
    }

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

        string Condition = "1=1";

        if (ddlLottery.SelectedValue != "-1")
        {
            Condition += " and LotteryID=" + Shove._Convert.StrToInt(ddlLottery.SelectedValue, -1).ToString();
        }

        if (ddlPlayType.SelectedValue != "-1")
        {
            Condition += " and PlayTypeID = " + Shove._Convert.StrToInt(ddlPlayType.SelectedValue, -1).ToString();
        }

        DataTable dtData = dt.Clone();

        foreach (DataRow dr in dt.Select(Condition, "[DateTime] desc"))
        {
            dtData.Rows.Add(dr.ItemArray);
        }

        PF.DataGridBindData(gHistory, dtData, gPagerHistory);

        gPagerHistory.Visible = true;


        //页面总计
        this.lblPageBuySum.Text = PF.GetSumByColumn(dtData, 8 ,true, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
        this.lblPageSumWinMoney.Text = PF.GetSumByColumn(dtData, 10, true, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");

        //总计
        this.lblBuySum.Text = PF.GetSumByColumn(dtData, 8, false, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
        this.lblSumWinMoney.Text = PF.GetSumByColumn(dtData, 10, false, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
    }

    protected void gHistory_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

            string str1 = e.Item.Cells[0].Text;
            if (str1.Length > 6)
            {
                e.Item.Cells[0].Text = str1.Substring(0, 6) + "…";
            }

            e.Item.Cells[0].Attributes.Add("title", str1);

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
                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未中奖" + "</a>";
                e.Item.Cells[10].Style.Add(HtmlTextWriterStyle.Color, "FFFFCC");
            }
            else
            {
                e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<font color=\"red\">中奖啦!</font>" + "</a>";
                e.Item.Cells[10].Style.Add(HtmlTextWriterStyle.Color, "#FF0000");
            }

            bool IsOpened = false;

            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                DataRow dr = drv.Row;

                IsOpened = Shove._Convert.StrToBool(dr["IsOpened"].ToString(), false);

                if (!IsOpened)
                {
                    e.Item.Cells[10].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未开奖" + "</a>";
                    e.Item.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFCA");
                }
            }
            catch { }

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[14].Text, 0);

            if (QuashStatus != 0)
            {
                string StatusDescription = "已撤单";

                e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + StatusDescription + "</a>";
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
                            e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "<Font color=\'Red\'>已满员</font>" + "</a>";
                        }
                        else
                        {
                            e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "未满员" + "</a>";
                        }
                    }
                }
                else
                {
                    e.Item.Cells[12].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + "已成功" + "</a>";
                }
            }
        }
    }

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
            case "gHistory":
                Script = "ShowOrHiddenDiv(\"divHistory\");clickTabMenu(document.getElementById(\"tdHistory\"),'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";
                BindHistoryData();                
                break;
            case "gReward":
                Script = "ShowOrHiddenDiv(\"divReward\");clickTabMenu(document.getElementById(\"tdReward\"), 'url(images/admin_qh_100_1.jpg)', 'myIcaileTab');";
                BindDataReward();
                break;
        }
    }

    private void BindDataReward()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Invest_Reward_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("UserID, SchemeID, LotteryNumber, IsuseName, SchemeWinMoney, LotteryName, WinMoneyNoWithTax,DetailMoney"
                                                                , "[UserID] = " + _User.ID.ToString() + " and (EndTime < GetDate() or LotteryID in (72,73)) and WinMoneyNoWithTax > 0"
                                                                , "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 3600);
        }

        gReward.DataSource = dt;
        gReward.DataBind();

        this.lblRewardCount.Text = dt.Rows.Count.ToString();
        this.lblRewardMoney.Text = PF.GetSumByColumn(dt, 6, false, gReward.PageSize, 0).ToString("N");
    }

    protected void gReward_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;
            double BuyMoney;
            BuyMoney = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (BuyMoney == 0) ? "" : BuyMoney.ToString("N");

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
