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

public partial class Home_Room_ViewChase : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            this.txtStartDate.Text = System.DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            this.txtEndDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            BindData();

            BindChaseComboData();
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
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(_Site.ID.ToString() + "MemberChase" + _User.ID.ToString());

        string Condition = "[UserID] = " + _User.ID.ToString() + " and SiteID = " + _Site.ID;

        if (isDateValid())
        {
            Condition += " and Convert(datetime,[DateTime]) between '" + txtStartDate.Text + " 0:0:0' and '" + txtEndDate.Text + " 23:59:59'";
        }

        if (dt == null)
        {
            dt = new DAL.Views.V_ChaseTasksTotal().Open("", Condition, "[DateTime] desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_ViewChase");

                return;
            }

            Shove._Web.Cache.SetCache(_Site.ID.ToString() + "MemberChase" + _User.ID.ToString(), dt);
        }

        PF.DataGridBindData(g1, dt, gPager1);

        gPager1.Visible = true;

        this.lblPageBuyMoney.Text = PF.GetSumByColumn(dt, 12, true, gPager1.PageSize, gPager1.PageIndex).ToString("N");

        this.lblTotalBuyMoney.Text = PF.GetSumByColumn(dt, 12, false, gPager1.PageSize, gPager1.PageIndex).ToString("N");
    }

    protected void g1_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            string str = e.Item.Cells[2].Text.Trim();
            if (str.Length > 8)
            {
                str = str.Substring(0, 7) + "..";
            }

            e.Item.Cells[2].Text = "<a href='ChaseDetail.aspx?id=" + e.Item.Cells[8].Text + "'><font color=\"#330099\">" + str + "</Font></a>";

            string LotteryName = e.Item.Cells[1].Text;
            e.Item.Cells[1].Text = "<a href='ChaseDetail.aspx?id=" + e.Item.Cells[8].Text + "'><font color=\"#330099\">" + LotteryName + "</Font></a>";

            double money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("N");

            int SumIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[4].Text, 0);
            int BuyedIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[5].Text, 0);
            int QuashedIsuseNum = Shove._Convert.StrToInt(e.Item.Cells[6].Text, 0);

            e.Item.Cells[7].Text = (SumIsuseNum > (BuyedIsuseNum + QuashedIsuseNum)) ? "<Font color=\'Red\'>进行中</font>" : "已终止";
        }
    }

    protected void gPager1_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager1_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache(_Site.ID.ToString() + "MemberChase" + _User.ID.ToString());
        if (!isDateValid())
        {
            return;
        }
        else
        {
            BindData();
        }
    }

    private bool isDateValid()
    {
        if (Shove._Convert.StrToDateTime(txtStartDate.Text, "2000-1-1") > Shove._Convert.StrToDateTime(txtEndDate.Text, "2000-1-1"))
        {
            Shove._Web.JavaScript.Alert(this.Page, "终止日期要大于等于起始日期！");
            return false;
        }
        else if (Shove._Convert.StrToDateTime(txtStartDate.Text, "2000-1-1") == Convert.ToDateTime("2000-1-1") || Shove._Convert.StrToDateTime(txtEndDate.Text, "2000-1-1") == Convert.ToDateTime("2000-1-1"))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的起始日期(格式: 2009-1-1)！");
            return false;
        }
        return true;
    }

    protected void g1_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    private void BindChaseComboData()
    {
        string Key = "Home_Room_ViewChase_BindChaseComboData" + _User.ID.ToString() + tbTitle.Text;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("select a.ID,Name, DateTime,IsuseCount,IsuseCount*Multiple*Nums*Price as SumMoney,Money,QuashStatus,ExecutedCount,")
                .Append("IsuseCount-ExecutedCount as NoExecutedCount,Title from T_Chases a inner join T_Lotteries b ")
                .Append("on a.LotteryID = b.ID  and a.UserID = " + _User.ID.ToString() + " ");
            if (tbTitle.Text != "")
            {
                sb.Append("and Title like '%" + Shove._Web.Utility.FilteSqlInfusion(tbTitle.Text) + "%'");
            }

            sb.Append("  left join (select ChaseID,count(SchemeID) as ExecutedCount from  T_ExecutedChases group by ChaseID)c on a.ID = c.ChaseID");

            dt = Shove.Database.MSSQL.Select(sb.ToString());

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt);
        }

        PF.DataGridBindData(g2, dt, gPager2);

        gPager2.Visible = true;

        double money = 0;
        foreach (DataRow dr in dt.Rows)
        {
            money += Shove._Convert.StrToDouble(dr["SumMoney"].ToString(), 0);
        }

        this.lblTotalMoney.Text = money.ToString("N");
    }

    protected void g2_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            int SumIsuseCount = Shove._Convert.StrToInt(e.Item.Cells[4].Text, 0);
            int ExecutedIsuseCount = Shove._Convert.StrToInt(e.Item.Cells[5].Text, 0);
            int QuashStatus = Shove._Convert.StrToInt(e.Item.Cells[7].Text, 0);

            e.Item.Cells[7].Text = QuashStatus == 1 ? "已终止" : (SumIsuseCount == ExecutedIsuseCount ? "已完成" : "<Font color=\'Red\'>进行中</font>");

            e.Item.Cells[2].Text = "<a style='TEXT-DECORATION: underline;color:red' href='ChaseExecutedSchemes.aspx?id=" + e.Item.Cells[8].Text + "' target='_blank'>" + e.Item.Cells[2].Text + "</a>";
        }
    }

    protected void gPager2_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindChaseComboData();
    }

    protected void gPager2_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindChaseComboData();
    }

    protected void g2_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        BindChaseComboData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindChaseComboData();
    }
}
