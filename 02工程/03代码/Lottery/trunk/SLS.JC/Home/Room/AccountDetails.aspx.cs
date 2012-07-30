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

public partial class Home_Room_AccountDetails : AdminPageBase
{
    int outCount = 0;
    int inCount = 0;
    double outMoney = 0;
    double inMoney = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForYearMonth();
            BindDataForYearMonth1();
            BindData();
            BinDataForDay();
            BinDataForDay1();
        }
    }

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        if (Month > 2)
        {
            ddlMonth.SelectedIndex = Month - 2;
        }
        else
        {
            ddlMonth.SelectedIndex = 0;
        }
    }

    private void BindDataForYearMonth1()
    {
        ddlYear1.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnGO.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear1.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear1.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth1.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "AccountDetail_" + _User.ID.ToString());
        string start = ddlYear.SelectedValue + "-" + ddlMonth.SelectedValue + "-" + ddlDay.SelectedValue + " 00:00:00";
        DateTime dtStart = Shove._Convert.StrToDateTime(start, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        string end = ddlYear1.SelectedValue + "-" + ddlMonth1.SelectedValue + "-" + ddlDay1.SelectedValue + " 23:59:59";
        DateTime dtEnd = Shove._Convert.StrToDateTime(end, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        if (dt == null)
        {
            if (ddlYear.Items.Count < 1)
            {
                return;
            }

            //一下2个判断为时间判断
            //如果当前时间小于选择时间 则把 选择时间改成当前时间
            if (DateTime.Now.CompareTo(dtEnd) <= 0)
            {
                dtEnd = DateTime.Now;
            }

            if (dtEnd.CompareTo(dtStart) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "开始时间不能小于结束时间.");

                return;
            }

            int ReturnValue = 0;
            string ReturnDescription = "";

            DataSet ds = null;

            DAL.Procedures.P_GetUserAccountDetails(ref ds, 1, _User.ID, dtStart, dtEnd, ref ReturnValue, ref ReturnDescription);

            if ((ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_AccountDetail");

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            dt = ds.Tables[0];

            Shove._Web.Cache.SetCache(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "AccountDetail_" + _User.ID.ToString(), dt);
        }

        PF.DataGridBindData(g, dt, gPager);

        this.lblInCount.Text = inCount.ToString();
        this.lblOutCount.Text = outCount.ToString();
        this.lblInMoney.Text = inMoney.ToString("N");
        this.lblOutMoney.Text = outMoney.ToString("N");
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {
            case "gPager":
                hdCurDiv.Value = "divAccountDetail";
                BindData();
                break;
        }

    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache(Shove._Web.WebConfig.GetAppSettingsString("SystemPreFix") + _Site.ID.ToString() + "AccountDetail_" + _User.ID.ToString());

        BindData();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");
            if (money != 0)
            {
                inCount++;
                inMoney += money;
            }

            money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("N");
            if (money != 0)
            {
                outCount++;
                outMoney += money;
            }

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[5].Text, 0);
            e.Item.Cells[5].Text = (money == 0) ? "0.00" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[6].Text, 0);
            e.Item.Cells[6].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[7].Text, 0);
            e.Item.Cells[7].Text = (money == 0) ? "" : money.ToString("N");

            long SchemeID = Shove._Convert.StrToLong(e.Item.Cells[8].Text, 0);

            if (SchemeID > 0)
            {
                e.Item.Cells[1].Text = "<span class='red12_2'><a href='Scheme.aspx?id=" + SchemeID.ToString() + "' target='_blank'>" + e.Item.Cells[1].Text + "</a></span>";
            }
        }
    }

    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
            case "g":
                BindData();

                break;
        }
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BinDataForDay();
    }

    private void BinDataForDay()
    {
        ddlDay.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth.SelectedValue);
        int MaxDay = 0;

        foreach (int j in Month)
        {
            if (i == j)
            {
                MaxDay = 31;
                break;
            }
            else if (i == 2)
            {
                if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
                {
                    MaxDay = 29;
                    break;
                }
                else
                {
                    MaxDay = 28;
                    break;
                }
            }
            else
            {
                MaxDay = 30;
            }
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay.SelectedIndex = Day - 1;
    }

    private void BinDataForDay1()
    {
        ddlDay1.Items.Clear();

        int[] Month = new Int32[7];
        Month[0] = 1;
        Month[1] = 3;
        Month[2] = 5;
        Month[3] = 7;
        Month[4] = 8;
        Month[5] = 10;
        Month[6] = 12;

        DateTime dtTime = DateTime.Now;
        int Year = dtTime.Year;
        int Day = dtTime.Day;
        int i = int.Parse(ddlMonth.SelectedValue);
        int MaxDay = 0;

        foreach (int j in Month)
        {
            if (i == j)
            {
                MaxDay = 31;
                break;
            }
            else if (i == 2)
            {
                if (((Year % 4) == 0) && ((Year % 100) != 0) && ((Year % 400) == 0))
                {
                    MaxDay = 29;
                    break;
                }
                else
                {
                    MaxDay = 28;
                    break;
                }
            }
            else
            {
                MaxDay = 30;
            }
        }

        for (int j = 1; j <= MaxDay; j++)
        {
            ddlDay1.Items.Add(new ListItem(j.ToString() + "日", j.ToString()));
        }

        if (Day > MaxDay)
        {
            Day = MaxDay;
        }

        ddlDay1.SelectedIndex = Day - 1;
    }
}
