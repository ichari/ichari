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

public partial class Home_Room_UserListFollowScheme : RoomPageBase
{
    public long UserID;

    public int LotteryID, PlayTypeID;
    public string LotteryName, PlayTypeName;
    public string Name;

    public int i = 1;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            tbLotteryID.Value = Shove._Web.Utility.GetRequest("LotteryID");
            tbPlayTypeID.Value = Shove._Web.Utility.GetRequest("PlayTypeID");
            tbUserID.Value = Shove._Web.Utility.GetRequest("UserID");

            GetID();

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void GetID()
    {
        LotteryID = Shove._Convert.StrToInt(tbLotteryID.Value, -1);
        PlayTypeID = Shove._Convert.StrToInt(tbPlayTypeID.Value, -1);
        UserID = Shove._Convert.StrToInt(tbUserID.Value, -1);

        SLS.Lottery lottery = new SLS.Lottery();

        if (!lottery.ValidID(LotteryID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().FullName);

            return;
        }

        if (!lottery[LotteryID].CheckPlayType(PlayTypeID))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误，系统异常。", this.GetType().FullName);

            return;
        }

        LotteryName = lottery[LotteryID].name;
        PlayTypeName = lottery.GetPlayTypeName(PlayTypeID);

        Name = new Users(1)[1, UserID].Name;
    }

    private void BindData()
    {
        DataTable dt = new DAL.Views.V_CustomFollowSchemes().Open("[UserName],MoneyStart,[MoneyEnd],BuyShareStart,BuyShareEnd,[TypeName],Convert(varchar,[DateTime],120)[DateTime]", "UsersForInitiateFollowSchemeUserID = " + Shove._Web.Utility.FilteSqlInfusion(tbUserID.Value) + " and PlayTypeID = " + Shove._Web.Utility.FilteSqlInfusion(tbPlayTypeID.Value), "[DateTime] desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        lbCountUser.Text = dt.Rows.Count.ToString();

        PF.DataGridBindData(g, dt, gPager);

        gPager.Visible = true;
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataRow dr = drv.Row;

            //认购金额
            e.Item.Cells[2].Text = Shove._Convert.StrToDouble(dr["MoneyStart"].ToString(), 0).ToString("N") + "&nbsp;至&nbsp;" + Shove._Convert.StrToDouble(dr["MoneyEnd"].ToString(), 0).ToString("N") + " 元";
            //END
        }
    }
}
