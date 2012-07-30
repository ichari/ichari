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

public partial class Home_Room_InvestHistory : RoomPageBase
{
    int[] arrLotteries = new int[] { 5, 6, 29, 39 };
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();
            BindDataForPlayType();
            BindDataForIsuse();


            BindHistoryData();
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
            if (arrLotteries.Contains(Shove._Convert.StrToInt(dtLotteries.Rows[i]["ID"].ToString(), 0)))
            {
                ddlLottery.Items.Add(new ListItem(dtPlayTypes.Rows[0]["LotteryName"].ToString(), dtPlayTypes.Rows[0]["LotteryID"].ToString()));
            }
        }

        if (ddlLottery.Items.Count > 0)
        {
            BindDataForPlayType(ddlLottery.Items[0].Value);
        }
    }


    private void BindDataForPlayType()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        ddlPlayType.Items.Clear();
        ddlPlayType.Items.Add(new ListItem("全部玩法", "-1"));

        if (ddlLottery.SelectedValue == "-1")
        {
            return;
        }

        SLS.Lottery.PlayType[] PlayTypes = new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].GetPlayTypeList();

        foreach (SLS.Lottery.PlayType PlayType in PlayTypes)
        {
            ddlPlayType.Items.Add(new ListItem(PlayType.Name, PlayType.ID.ToString()));
        }
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        ddlIsuse.Items.Clear();
        ddlIsuse.Items.Add(new ListItem("全部", "-1"));

        if (ddlLottery.SelectedValue == "-1")
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " + Shove._Convert.StrToInt(ddlLottery.SelectedValue, -1).ToString() + " and EndTime < GetDate()", "EndTime desc");

        if (dt == null)
        {
            //PF.GoError(ErrorNumber.NoIsuse, ddlLottery.Text + "暂无期号");

            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            ddlIsuse.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
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


    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForPlayType();

        BindDataForIsuse();
    }

    protected void btnGo_Click(object sender, System.EventArgs e)
    {
        if (ddlLottery.SelectedIndex < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择彩种。");

            return;
        }

        BindHistoryData();
    }


    private void BindHistoryData()
    {
        if (ddlIsuse.Items.Count < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请选择彩种、玩法和期号。");

            return;
        }

        string Condition = " [UserID] = " + _User.ID.ToString();

        if (ddlLottery.SelectedValue != "-1")
        {
            Condition += " and LotteryID=" + Shove._Convert.StrToInt(ddlLottery.SelectedValue, -1).ToString();
        }

        if (ddlIsuse.SelectedValue == "-1")
        {
            Condition += " and EndTime < GetDate()";
        }
        else
        {
            Condition += " and IsuseID=" + Shove._Convert.StrToLong(ddlIsuse.SelectedValue, -1).ToString();
        }

        if (ddlPlayType.SelectedValue != "-1")
        {
            Condition += " and PlayTypeID = " + Shove._Convert.StrToInt(ddlPlayType.SelectedValue, -1).ToString();
        }

        string CacheKey = "InvestHistory_" + Condition;
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Views.V_BuyDetailsWithQuashedAll().Open("*", Condition, "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 60);
        }

        PF.DataGridBindData(gHistory, dt, gPagerHistory);

        gPagerHistory.Visible = true;

        this.lblPageBuySum.Text = PF.GetSumByColumn(dt, 10, true, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
        this.lblPageRewardSum.Text = PF.GetSumByColumn(dt, 8, true, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");

        this.lblTotalBuySum.Text = PF.GetSumByColumn(dt, 10, false, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
        this.lblTotalRewardSum.Text = PF.GetSumByColumn(dt, 8, false, gPagerHistory.PageSize, gPagerHistory.PageIndex).ToString("N");
    }

    protected void gHistory_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[12].Text, 0);
            string str = e.Item.Cells[1].Text;

            if (str.Length > 6)
            {
                str = str.Substring(0, 5) + "..";
            }

            e.Item.Cells[1].Text = "<a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'><font color=\"#330099\">" + str + "</Font></a>";

            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "" : money.ToString("N");

            double i = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            double j = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);

            if (j >= i)
            {
                e.Item.Cells[6].Text = "100%";
            }
            else
            {
                if (i > 0)
                {
                    e.Item.Cells[6].Text = Math.Round(j / i * 100, 2).ToString() + "%";
                }
            }

            money = Shove._Convert.StrToDouble(e.Item.Cells[7].Text, 0);
            e.Item.Cells[7].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[8].Text, 0);
            e.Item.Cells[8].Text = (money == 0) ? "" : money.ToString("N");

            if (money == 0)
            {
                e.Item.Cells[9].Text = "未中奖";
                e.Item.Cells[9].Style.Add(HtmlTextWriterStyle.Color, "FFFFCC");
            }
            else
            {
                e.Item.Cells[9].Text = "中奖啦!";
            }

            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                DataRow dr = drv.Row;

                if (!Shove._Convert.StrToBool(dr["IsOpened"].ToString(), false))
                {
                    e.Item.Cells[9].Text = "未开奖";
                }
            }
            catch { }

            short QuashStatus = Shove._Convert.StrToShort(e.Item.Cells[13].Text, 0);

            if (QuashStatus != 0)
            {
                e.Item.Cells[11].Text = "已撤单";
            }
            else
            {
                bool Buyed = Shove._Convert.StrToBool(e.Item.Cells[14].Text, false);

                if (Buyed)
                {
                    e.Item.Cells[11].Text = "<Font color=\'Red\'>已成功</font>";
                }
                else
                {
                    int BuyedShare = Shove._Convert.StrToInt(e.Item.Cells[16].Text, 0);
                    int SchemeShare = Shove._Convert.StrToInt(e.Item.Cells[2].Text, 0);

                    if (BuyedShare >= SchemeShare)
                    {
                        e.Item.Cells[11].Text = "<Font color=\'Red\'>已满员</font>";
                    }
                    else
                    {
                        e.Item.Cells[11].Text = "未成功";
                    }
                }
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindHistoryData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindHistoryData();
    }
}
