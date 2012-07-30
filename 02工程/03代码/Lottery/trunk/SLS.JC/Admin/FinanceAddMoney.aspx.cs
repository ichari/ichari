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

public partial class Admin_FinanceAddMoney : AdminPageBase
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
        RequestLoginPage = "Admin/FinanceAddMoney.aspx";

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

        int ReturnValue = -1;
        string ReturnDescription = "";

        DataSet ds = null;

        int Results = -1;
        Results =  DAL.Procedures.P_GetFinanceAddMoney(ref ds, _Site.ID, UserID, int.Parse(ddlYear.SelectedValue), int.Parse(ddlMonth.SelectedValue), ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }


        if ((ds == null) || (ds.Tables.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_FinanceAddMoney");

            return;
        }

        if (ReturnValue < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_FinanceAddMoney");

            return;
        }

        PF.DataGridBindData(g, ds.Tables[0], gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;

            e.Item.Cells[2].Text = getBankName(e.Item.Cells[2].Text);

            money = Shove._Convert.StrToDouble(e.Item.Cells[3].Text, 0);
            e.Item.Cells[3].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[4].Text, 0);
            e.Item.Cells[4].Text = (money == 0) ? "" : money.ToString("N");

            string strResult = e.Item.Cells[5].Text;

            if (strResult == "1")
            {
                strResult = "True";
            }

            e.Item.Cells[5].Text = Shove._Convert.StrToBool(strResult, false) ? "<font color='Red'>成功</font>" : "未成功";

            e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=" + e.Item.Cells[7].Text + "&ID=" + e.Item.Cells[6].Text + "'>" + e.Item.Cells[0].Text + "</a>";
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

    //根据支付方式来获取相应的中文说明
    private string getBankName(string bankCode)
    {

        string bankName = "";
        string[] banks = bankCode.Split('_');

        if (banks.Length < 2)
        {
            return "未知银行";
        }

        if (banks[0].ToUpper() == "ALIPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "ALIPAY":
                    bankName = "支付宝";
                    break;

                case "ICBCB2C":
                    bankName = "中国工商银行";
                    break;
                case "GDB":
                    bankName = "广东发展银行";
                    break;
                case "CEBBANK":
                    bankName = "中国光大银行";
                    break;
                case "CCB":
                    bankName = "中国建设银行";
                    break;
                case "COMM":
                    bankName = "中国交通银行";
                    break;
                case "ABC":
                    bankName = "中国农业银行";
                    break;
                case "SPDB":
                    bankName = "上海浦发银行";
                    break;
                case "SDB":
                    bankName = "深圳发展银行";
                    break;
                case "CIB":
                    bankName = "兴业银行";
                    break;
                case "HZCBB2C":
                    bankName = "杭州银行";
                    break;
                case "CMBC":
                    bankName = "杭州银行";
                    break;
                case "BOCB2C":
                    bankName = "中国银行";
                    break;
                case "CMB":
                    bankName = "中国招商银行";
                    break;
                case "CITIC":
                    bankName = "中信银行";
                    break;
                default:
                    bankName = "支付宝";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "TENPAY")
        {
            switch (banks[1].ToUpper())
            {
                case "0":
                    bankName = "财付通";

                    break;
                case "1001":
                    bankName = "招商银行";

                    break;
                case "1002":
                    bankName = "中国工商银行";

                    break;
                case "1003":
                    bankName = "中国建设银行";

                    break;
                case "1004":
                    bankName = "上海浦东发展银行";

                    break;
                case "1005":
                    bankName = "中国农业银行";

                    break;
                case "1006":
                    bankName = "中国民生银行";

                    break;
                case "1008":
                    bankName = "深圳发展银行";

                    break;
                case "1009":
                    bankName = "兴业银行";

                    break;
                case "1028":
                    bankName = "广州银联";

                    break;
                case "1032":
                    bankName = "   北京银行";

                    break;
                case "1020":
                    bankName = "   中国交通银行";

                    break;
                case "1022":
                    bankName = "   中国光大银行";

                    break;
                default:
                    bankName = "财付通";
                    break;
            }
        }
        else if (banks[0].ToUpper() == "51ZFK")
        {
            switch (banks[1].ToUpper())
            {
                case "SZX":
                    bankName = "神州行充值卡";
                    break;

                case "ZFK":
                    bankName = "51支付卡";
                    break;
                default:
                    bankName = "神州行充值卡";
                    break;
            }
        }






        return bankName;

    }
}
