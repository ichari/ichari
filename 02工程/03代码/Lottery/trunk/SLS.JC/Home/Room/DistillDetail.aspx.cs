using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Room_DistillDetail : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
            BindDistills();
            isShowDistill.Visible = false;
        }
    }
    private void BindDistills()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Home_Room_DistillDetail_" + _User.ID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKeyName);

        if (dt == null)
        {
            dt = new DAL.Views.V_UserDistills().Open("ID,[DateTime],[Money],FormalitiesFees,Result,Memo", "[UserID] = " + _User.ID.ToString() + "", "[DateTime] desc, [ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(732)", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKeyName, dt, 60);
        }

        gUserDistills.DataSource = dt;
        gUserDistills.DataBind();

        this.lblDistillCount.Text = dt.Rows.Count.ToString();
        this.lblDistillMoney.Text = PF.GetSumByColumn(dt, 2, false, gUserDistills.PageSize, 0).ToString("N");
    }
    protected void gUserDistills_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double money;
            money = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0);
            e.Item.Cells[1].Text = (money == 0) ? "" : money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            e.Item.Cells[2].Text = (money == 0) ? "" : money.ToString("N");

            e.Item.Cells[3].Text = "提款";

            string result = e.Item.Cells[4].Text;
            if (result == "0")
            {
                e.Item.Cells[4].Text = "申请状态";
            }
            else if (result == "1")
            {
                e.Item.Cells[4].Text = "已付款";
            }
            else if (result == "-1")
            {
                e.Item.Cells[4].Text = "拒绝提款";
            }
            else if (result == "-2")
            {
                e.Item.Cells[4].Text = "用户撤销提款";
            }
            else
            {
                e.Item.Cells[4].Text = "处理中...";
            }
        }
    }
    /// <summary>
    /// 用户自行撤销提款
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void gUserDistills_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        HiddenField hidDistillID = (HiddenField)e.Item.FindControl("tdDistillID");
        int DistillID = Shove._Convert.StrToInt(hidDistillID.Value, 0);
        if (e.CommandName == "QuashDistills")
        {
            string CacheKeyName = "Home_Room_DistillDetail_" + _User.ID.ToString();
            //int DistillID1 = Shove._Convert.StrToInt(e.Item.Cells[4].Text, 0);
            string ReturnDescription = "";
            
            //int Result = DAL.Procedures.P_UserDistillCancel(1, _User.ID, -2, DistillID, "用户自行撤销提款", _User.ID, ref ReturnValue, ref ReturnDescription);
            int Result = _User.DistillQuash(DistillID, ref ReturnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误." + ReturnDescription);

                return;
            }

            if (ReturnDescription != "")
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            Shove._Web.JavaScript.Alert(this.Page, "撤销成功。");
            Shove._Web.Cache.ClearCache(CacheKeyName);
            BindDistills();
        }
        else if (e.CommandName == "ShowDistillDetail")
        {
            this.isShowDistill.Visible = true;
            string CardNum = "";
            string AlipayName = "";
            string time = "";
            DataTable dt = new DAL.Tables.T_UserDistills().Open("BankCardNumber,AlipayName,[DateTime], BankTypeName, BankName, BankInProvince, BankInCity", "id = " + DistillID, "");
            CardNum = dt.Rows[0]["BankCardNumber"].ToString();
            AlipayName = dt.Rows[0]["AlipayName"].ToString();
            time = dt.Rows[0]["DateTime"].ToString();
            if (CardNum == "")
            {
                this.lblDistillBankType.Text = "支付宝提款";
                this.lblDistillBanks.Text = "支付宝账号: ";
                this.lblDistillBankDetail.Text = AlipayName;
                divBankInfo.Visible = false;
            }
            else
            {

                this.lblDistillBankType.Text = "银行卡提款";
                this.lblDistillBankDetail.Text = CardNum;
                this.lblDistillBanks.Text = "银行卡号: ";

                divBankInfo.Visible = true;
                this.lbBankInProvince.Text = dt.Rows[0]["BankInProvince"].ToString();
                this.lbBankInCity.Text = dt.Rows[0]["BankInCity"].ToString();
                this.lbAccountBank.Text = dt.Rows[0]["BankName"].ToString();
                this.lbBankTypeName.Text = dt.Rows[0]["BankTypeName"].ToString();
            }
            this.lblDistillTime.Text = time.ToString();
            BindDistills();
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindDistills();
    }
}
