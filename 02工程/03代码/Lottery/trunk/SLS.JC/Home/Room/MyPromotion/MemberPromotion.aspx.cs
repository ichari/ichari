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

public partial class Home_Room_MyPromotion_MemberPromotion : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            tbUrl.Text = _User.GetPromotionURL(0);
            hidLink.Value = tbUrl.Text;
            BindDataForYearMonth();
            BindTransactionList();
            lbTotalBonus.Text = _User.Bonus.ToString("N");
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
        string cacheKey = "Home_Room_MyPromotion_MemberPromotion_BindData" + _User.ID.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(cacheKey);
        double AllTodayMoney = 0;
        double AllYesterdayMoney = 0;
        double AllMonthMoney = 0;
        double AllTotalMoney = 0;

        if (dt == null)
        {
            int returnValue = -1;
            string returnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_CpsGetCommendMemberList(ref ds, _User.ID, ref returnValue, ref returnDescription);
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

        DataTable dtData = dt.Clone();
        dtData.Columns.Add("Count", typeof(int));
        string Condition = "1=1";
        if (tbUserName.Text != "" && tbUserName.Text != "请输入用户名")
        {
            Condition = "Name like '%" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text) + "%'";
        }

        DataRow[] rows = dt.Select(Condition);
        for (int i = 0; i < rows.Length; i++)
        {
            dtData.Rows.Add(rows[i].ItemArray);
            dtData.Rows[i]["Count"] = i + 1;
            AllTodayMoney += Shove._Convert.StrToDouble(rows[i]["TodayTradeMoney"].ToString(), 0.0);
            AllYesterdayMoney += Shove._Convert.StrToDouble(rows[i]["YesterdayTradeMoney"].ToString(), 0.0);
            AllMonthMoney += Shove._Convert.StrToDouble(rows[i]["ThisMonthTradeMoney"].ToString(), 0.0);
            AllTotalMoney += Shove._Convert.StrToDouble(rows[i]["TotalTradeMoney"].ToString(), 0.0);

        }
        lbTodaySales1.Text = String.Format("{0:0.00}", AllTodayMoney);
        lbYesterdaySales1.Text = String.Format("{0:0.00}", AllYesterdayMoney);
        lbMonthSales1.Text = String.Format("{0:0.00}", AllMonthMoney);
        lbSales1.Text = String.Format("{0:0.00}", AllTotalMoney);
        PF.DataGridBindData(g, dtData, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        trLink.Style.Add("display", "");
        trMoney.Style.Add("display", "");

        if (rblMoney.SelectedValue == "0")
        {
            divOther.Style.Add("display", "");
        }

        int money = 0;
        if (rblMoney.SelectedValue == "0")
        {
            money = Shove._Convert.StrToInt(Request.Form["tbMoney"], -1);
        }
        else
        {
            money = Shove._Convert.StrToInt(rblMoney.SelectedValue, -1);
        }

        if (money < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "红包金额只能为整数金额！");
            return;
        }

        lbShowInfo.Text = "此链接包含" + money.ToString() + "元红包！";
        DAL.Tables.T_UserHongbaoPromotion uhp = new DAL.Tables.T_UserHongbaoPromotion();
        uhp.UserID.Value = _User.ID;
        uhp.Money.Value = money;
        uhp.ExpiryDate.Value = DateTime.Now.AddDays(Shove._Convert.StrToInt(ddlDate.SelectedValue, 7));
        long id = uhp.Insert();
        SynchronizeSessionID s = new SynchronizeSessionID(this.Page);
        string userID = _User.ID.ToString();
        string sign = s.GenSign(id.ToString(), userID);
        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/PromoteUserReg.aspx?id=" + id.ToString() + "&userID=" + userID + "&Sign=" + sign + "";
        uhp.URL.Value = url;
        uhp.Update("ID=" + id.ToString() + "");
        tbUrl.Text = url;
        Shove._Web.Cache.ClearCache("Home_Room_MyPromotion_HongBaoRecord" + _User.ID.ToString());
        lbShowCreateResult.Text = "生成红包链接成功！";
        return;
    }

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();
        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnSearch.Enabled = false;
            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
    }

    private void BindTransactionList()
    {
        DateTime StartTime, EndTime;

        if (ddlMonth.SelectedValue == "0")
        {
            StartTime = Convert.ToDateTime(DateTime.Now.Year.ToString()+"-01-01");
            EndTime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "-12-31");
        }
        else
        {
            StartTime = Convert.ToDateTime(ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue + "-" + "1");//所选月的第一天
            EndTime = StartTime.AddMonths(1).AddTicks(-1);//所选月的最后一天的最后时间
        }

        string CacheKey = "Home_Room_MyPromotion_MemberPromotion_BindTransactionList" + _User.ID.ToString() + StartTime.ToShortDateString() + EndTime.ToShortDateString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            DataSet ds = null;
            int ReturnValue = 0;
            string ReturnDescprtion = "";
            int Result = DAL.Procedures.P_GetCpsPopularizeAccountRevenue(ref ds, _Site.ID, _User.ID, StartTime, EndTime, 2, ref ReturnValue, ref ReturnDescprtion);
            if (Result < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescprtion);
                return;
            }

            if (ds == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
                return;
            }

            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
            }

            Shove._Web.Cache.SetCache(CacheKey, dt);
        }
        PF.DataGridBindData(g1, dt, gPager1);
        summation(dt);
        gPager1.Visible = g1.PageCount > 1;

        if (ddlMonth.SelectedValue == "0")
        {
            lbShow.Text = "本年";
        }
        else
        {
            lbShow.Text = "本月";
        }
    }

    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        BindTransactionList();
    }

    public void summation(DataTable dt)
    {
        double tradeSum = 0, bonusSum = 0;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            tradeSum += Shove._Convert.StrToDouble(dt.Rows[i]["DayTradeMoney"].ToString(), 0);
            bonusSum += Shove._Convert.StrToDouble(dt.Rows[i]["DayBonusMoney"].ToString(), 0);
        }
        lbTradeSum.Text = tradeSum.ToString("N2");
        lbBonusSum.Text = bonusSum.ToString("N2");
    }

    protected void gPager1_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindTransactionList();
    }

    protected void gPager1_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindTransactionList();
    }

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lbDetail = e.Item.FindControl("lbDetail") as Label;

            lbDetail.Attributes.Add("onclick", "return GetUserAccountDetail(" + e.Item.Cells[8].Text + ");");
        }
    }

    protected void g1_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem || e.Item.ItemType == ListItemType.Item)
        {
            Label lbDetail = e.Item.FindControl("lbDetail") as Label;

            if (ddlMonth.SelectedValue == "0")
            {
                lbDetail.Attributes.Add("onclick", "return GetUserPromotionDetailByYear('"+Shove._Convert.StrToInt(e.Item.Cells[0].Text.Split('-')[1],1)+"');");
            }
            else
            {
                lbDetail.Attributes.Add("onclick", "return GetUserPromotionDetail('" + Shove._Convert.StrToDateTime(e.Item.Cells[0].Text, DateTime.Now.ToString()).ToString("yyyy-MM-dd") + "');");
            }
        }

        if (e.Item.ItemType == ListItemType.Header)
        {
            if (ddlMonth.SelectedValue == "0")
            {
                e.Item.Cells[1].Text = "当月会员交易量";
            }
            else
            {
                e.Item.Cells[1].Text = "本日会员交易量";
            }
        }
    }
}
