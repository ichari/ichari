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

public partial class Home_Room_AccountDrawMoney : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindDistills();
    }    
    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion
    private void BindDistills()
    {
        if (_User == null)
        {
            return;
        }

        string CacheKeyName = "Room_UserDistills_" + _User.ID.ToString();

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
            else if (result == "-1")
            {
                e.Item.Cells[4].Text = "不受理";
            }
            else if (result == "1")
            {
                e.Item.Cells[4].Text = "已受理";
            }
            else if (result == "-2")
            {
                e.Item.Cells[4].Text = "已取消";
            }
            else
            {
                e.Item.Cells[4].Text = "";
            }
        }
    }
    protected void gUserDistills_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        HiddenField hidDistillID = (HiddenField)e.Item.FindControl("tdDistillID");
        if (hidDistillID == null)
        {
            return;
        }
        int DistillID = Shove._Convert.StrToInt(hidDistillID.Value, 0);
        if (DistillID == 0) 
        {
            return;
        }
        if (e.CommandName == "QuashDistills")
        {
            string CacheKeyName = "Room_UserDistills_" + _User.ID.ToString();
            //int ReturnValue = 0;
            string ReturnDescription = "";

            //int Result = DAL.Procedures.P_UserDistillCancel(1, _User.ID, -2, DistillID, "用户自行撤销提款", _User.ID, ref ReturnValue, ref ReturnDescription);
            int Result = _User.DistillQuash(DistillID, ref ReturnDescription);
            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误.");

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
            DataTable dt = new DAL.Tables.T_UserDistills().Open("BankCardNumber,AlipayName,[DateTime],BankName", "id = " + DistillID, "");
            CardNum = dt.Rows[0]["BankCardNumber"].ToString();
            AlipayName = dt.Rows[0]["AlipayName"].ToString();
            time = dt.Rows[0]["DateTime"].ToString();
            if (CardNum == "")
            {
                this.lblDistillBankType.Text = "支付宝提款";
                this.lblDistillBanks.Text = "支付宝账号: ";
                this.lblDistillBankDetail.Text = AlipayName;
                this.lbAccountBank.Visible = false;
                this.lbAccountBankDetail.Visible = false;
            }
            else
            {

                this.lblDistillBankType.Text = "银行卡提款";
                this.lblDistillBankDetail.Text = CardNum;
                this.lblDistillBanks.Text = "银行卡号: ";
                this.lbAccountBank.Visible = true;
                this.lbAccountBankDetail.Visible = true;
                this.lbAccountBank.Text = dt.Rows[0]["BankName"].ToString();
            }
            this.lblDistillTime.Text = time.ToString();
        }

        BindDistills();
    }
    protected void g_SortCommand(object source, DataGridSortCommandEventArgs e)
    {
        DataGrid grid = (DataGrid)source;
        switch (grid.ID)
        {
          
            case "gUserDistills":
                BindDistills();
                break;
      
        }
    }
    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        string gPagerId = ((Shove.Web.UI.ShoveGridPager)(Sender)).ID;
        switch (gPagerId)
        {
          
            case "gPagergDistills":
                hdCurDiv.Value = "divUserDistills";
                BindDistills();
                break;
       
        }
    }

}
