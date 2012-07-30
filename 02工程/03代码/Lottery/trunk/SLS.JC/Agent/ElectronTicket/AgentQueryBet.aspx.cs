using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Agent_ElectronTicket_AgentQueryBet : ElectronTicketAgentsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            dataBindLottery();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAtFramePageLogin = true;
        base.OnInit(e);
    }

    #endregion

    private void dataBindLottery()
    {
        ddlLottery.Items.Clear();
        ddlLottery.Items.Add(new ListItem("------------请选择------------", "-1"));

        DataTable dtLotterys = Shove._Web.Cache.GetCacheAsDataTable(_ElectronTicketAgents.ID.ToString() + "Agent_ElectronTicket_AgentQueryBet_dtLotterys");

        if (dtLotterys == null)
        {
            dtLotterys = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_ElectronTicketAgents.UseLotteryList) + ")", "[Order]");

            if (dtLotterys == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_FrameLeft");

                return;
            }

            Shove._Web.Cache.SetCache(_ElectronTicketAgents.ID.ToString() + "Agent_ElectronTicket_AgentQueryBet_dtLotterys", dtLotterys);
        }

        for (int i = 0; i < dtLotterys.Rows.Count; i++)
        {
            ddlLottery.Items.Add(new ListItem(dtLotterys.Rows[i]["Name"].ToString(), dtLotterys.Rows[i]["ID"].ToString()));
        }
    }

    private void BindData()
    {
        string Condition = "1=1";

        if (!string.IsNullOrEmpty(tbIsuseName.Text.Trim()))
        {
            Condition += " and  IsuseName= '" + Shove._Web.Utility.FilteSqlInfusion(tbIsuseName.Text.Trim()) + "'";
        }

        if (int.Parse(ddlLottery.SelectedItem.Value) > 0)
        {
            Condition += " and LotteryID= " + ddlLottery.SelectedItem.Value;
        }

        if (int.Parse(ddlState.SelectedItem.Value) > 0)
        {
            if (int.Parse(ddlState.SelectedItem.Value) == 2)
            {
                Condition += " and state > 1";
            }
            else
            {
                Condition += " and state = 1";
            }
        }

        if (!string.IsNullOrEmpty(tbStartTime.Text.Trim()))
        {
            DateTime dtFrom = DateTime.Parse("1981-01-01");

            try
            {
                dtFrom = DateTime.Parse(tbStartTime.Text.Trim());
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "时间格式填写有错误！");

                return;
            }

            Condition += " and DateTime > '" + tbStartTime.Text.Trim() + "'";
        }

        if (!string.IsNullOrEmpty(tbEndTime.Text.Trim()))
        {
            DateTime dtFrom = DateTime.Parse("1981-01-01");

            try
            {
                dtFrom = DateTime.Parse(tbEndTime.Text.Trim());
            }
            catch
            {
                Shove._Web.JavaScript.Alert(this.Page, "时间格式填写有错误！");

                return;
            }

            Condition += " and DateTime < '" + tbEndTime.Text.Trim() + "'";
        }

        if (!string.IsNullOrEmpty(tbSchemeNumber.Text.Trim()))
        {
            Condition = "SchemeNumber= '" + Shove._Web.Utility.FilteSqlInfusion(tbSchemeNumber.Text.Trim()) + "'";
        }

        DataTable dt = new DAL.Views.V_ElectronTicketAgentSchemes().Open("ID, DateTime, SchemeNumber, Amount, LotteryName, PlayTypeName, State, Identifiers", Condition, "DateTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemeAtTop");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            double Money = Shove._Convert.StrToDouble(e.Item.Cells[2].Text, 0);
            e.Item.Cells[2].Text = Money.ToString("N");

            if (e.Item.Cells[5].Text == "1")
            {
                e.Item.Cells[5].Text = "成功";
            }
            else
            {
                e.Item.Cells[5].Text = "失败";
            }
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btn_ok_Click(object sender, EventArgs e)
    {
        BindData();
    }
}
