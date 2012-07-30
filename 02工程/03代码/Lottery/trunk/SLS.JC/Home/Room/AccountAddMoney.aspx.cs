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

public partial class Home_Room_AccountAddMoney : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        BindUserPayData();
        isShowUserPay.Visible = false;
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

            case "gUserPay":
                BindUserPayData();
                break;

        }

    }
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
            }
        }

        else if (banks[0].ToLower() == "007KA")
        {
            bankName = "神州行充值卡";
            return bankName;
        }
        return bankName;

    }
    protected void gUserPay_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
            e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");

            e.Item.Cells[3].Text = getBankName(e.Item.Cells[3].Text);
        }
    }


    private void BindUserPayData()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_UserPayDetail_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_UserPayDetails().Open("ID,[DateTime],PayType,[Money],FormalitiesFees", "[UserID] = " + _User.ID.ToString() + " and Result = 1", "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 60);
        }

        gUserPay.DataSource = dt;
        gUserPay.DataBind();

        this.lblUserPayCount.Text = dt.Rows.Count.ToString();
        this.lblUserPayMoney.Text = PF.GetSumByColumn(dt, 3, false, gUserPay.PageSize, 0).ToString("N");
    }
    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {

            case "gPagerUserPay":
                hdCurDiv.Value = "divUserPay";
                BindUserPayData();
                break;

        }
    }


    protected void gUserPay_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        HiddenField tdUserPayID = (HiddenField)e.Item.FindControl("tdUserPayDetail");
        if (tdUserPayID == null)
        {
            return;
        }
        int PayID = Shove._Convert.StrToInt(tdUserPayID.Value.ToString(), 0);
        if (PayID == 0)
        {
            return;
        }
        string payType = "";
        string Money = "";
        string time = "";

        if (e.CommandName == "ShowUserPayDetail")
        {
            try
            {
                this.isShowUserPay.Visible = false;
                DataTable dt = new DAL.Tables.T_UserPayDetails().Open("PayType , [Money] , [DateTime]", "id = " + PayID, "");
                payType = dt.Rows[0]["PayType"].ToString();
                Money = dt.Rows[0]["Money"].ToString();
                time = dt.Rows[0]["DateTime"].ToString();
                this.lblUserPayBank.Text = getBankName(payType); //== "alipay" ? "支付宝充值" :"网银充值";
            }
            catch { }

            this.lblUserPayMoneys.Text = Money.Substring(0, Money.IndexOf('.')) + Money.Substring(Money.IndexOf('.'), 3);
            this.lblUserPayTime.Text = time;
        }
        BindUserPayData();
        //BindDistills();
    }
}