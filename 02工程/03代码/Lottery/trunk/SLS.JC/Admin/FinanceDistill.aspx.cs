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

public partial class Admin_FinanceDistill : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            tbID.Text = Shove._Web.Utility.GetRequest("id");

            if (tbID.Text == "")
            {
                tbID.Text = "-1";
            }

            BindDataForYearMonth();

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "Admin/FinanceDistill.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForYearMonth()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        if (Year < PF.SystemStartYear)
        {
            btnRead.Enabled = false;

            return;
        }

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;

        ddlMonth.SelectedIndex = Month - 1;
    }

    private void BindData()
    {
        if (ddlYear.Items.Count == 0)
        {
            return;
        }

        long UserID = Shove._Convert.StrToLong(tbID.Text, -1);

        DataTable dt;

        if (tbID.Text == "-1")
        {
            dt = new DAL.Views.V_UserDistills().Open("", "SiteID = " + _Site.ID.ToString() + " and year([DateTime]) = " +  Shove._Web.Utility.FilteSqlInfusion(ddlYear.SelectedValue) + " and month([DateTime]) = " +  Shove._Web.Utility.FilteSqlInfusion(ddlMonth.SelectedValue), "[DateTime] desc");
        }
        else
        {
            dt = new DAL.Views.V_UserDistills().Open("", "SiteID = " + _Site.ID.ToString() + " and UserID = " + UserID.ToString() + " and year([DateTime]) = " +  Shove._Web.Utility.FilteSqlInfusion(ddlYear.SelectedValue) + " and month([DateTime]) = " +  Shove._Web.Utility.FilteSqlInfusion(ddlMonth.SelectedValue), "[DateTime] desc");
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceDistill");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {

            // 提款结果 0 申请状态  -2用户撤销提款 -1 不受理 1 已付款成功  11支付宝处理中  12支付宝付款失败
            short Result = Shove._Convert.StrToShort(e.Item.Cells[6].Text, -100);
            if (Result == 0)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>申请中</font>";
            }
            else if (Result == -1)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>已拒绝</font>";
            }
            else if (Result == -2)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>用户撤销提款</font>";
            }
            else if (Result == 1)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>付款成功</font>";
            }
            else if (Result == 10)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>已接受提款</font>";
            }
            else if (Result == 11)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>支付宝处理中</font>";
            }
            else if (Result == 12)
            {
                e.Item.Cells[6].Text = "<font color='Blue'>支付宝付款失败</font>";
            }

            

            //string HandleDateTime = e.Item.Cells[7].Text;
            //e.Item.Cells[7].Text = ((Result == 0) ? "" : HandleDateTime);
            //用户资料链接
            e.Item.Cells[1].Text = "<a href='UserDetail.aspx?SiteID=1&ID=" + e.Item.Cells[11].Text + "'>" + e.Item.Cells[1].Text + "</a>";
        }
    }

    protected void btnRead_Click(object sender, System.EventArgs e)
    {
        if (tbUserName.Text.Trim() != "")
        {
            Users tu = new Users(_Site.ID)[_Site.ID, tbUserName.Text.Trim()];

            if (tu == null)
            {
                Shove._Web.JavaScript.Alert(this.Page, "用户名不存在。");

                return;
            }

            tbID.Text = tu.ID.ToString();
        }
        else
        {
            tbID.Text = "-1";
        }

        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }
}
