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

public partial class Agent_ElectronTicket_AgentQueryTicket : ElectronTicketAgentsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        div_Ticket.Visible = (tbTicket.Text.Trim() != "");
    }

    protected void btn_ok_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Views.V_ElectronTicketAgentSchemesElectronTickets().Open("LotteryID, IsuseID, PlayTypeID, Ticket, SchemeNumber, WinLotteryNumber, DateTime, Money, Multiple", "Identifiers='" + tbTicket.Text.Trim() + "'", "");

        if (dt == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误！");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "没有找到此票标识！");

            return;
        }

        DataTable dtWinList = new DAL.Tables.T_WinTypes().Open("","LotteryID=" + dt.Rows[0]["LotteryID"].ToString(),"");

        if ((dtWinList == null) || (dtWinList.Rows.Count < 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误！");

            return;
        }

        double[] WinMoneyList = new double[dtWinList.Rows.Count * 2];

        for (int i = 0; i < dtWinList.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(dtWinList.Rows[i]["defaultMoney"].ToString(), 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(dtWinList.Rows[i]["DefaultMoneyNoWithTax"].ToString(), 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误！");

                return;
            }

            if (WinMoneyList[i * 2] < WinMoneyList[i * 2 + 1])
            {
                Shove._Web.JavaScript.Alert(this.Page, "数据库读写错误！");

                return;
            }
        }

        string t_LotteryNumber = dt.Rows[0]["Ticket"].ToString();
        string LotteryNumber = "";

        int LotteryID = Shove._Convert.StrToInt(dt.Rows[0]["LotteryID"].ToString(), 0);
        int PlayTypeID = Shove._Convert.StrToInt(dt.Rows[0]["PlayTypeID"].ToString(), 0);
        string WinLotteryNumber = dt.Rows[0]["WinLotteryNumber"].ToString();

        try
        {
            new SLS.Lottery()[LotteryID].HPSH_ToElectronicTicket(PlayTypeID, t_LotteryNumber, ref LotteryNumber, ref PlayTypeID);
        }
        catch
        {
        }

        string Description = "";
        double WinMoneyNoWithTax = 0;
        double WinMoney = 0;

        try
        {
            WinMoney = new SLS.Lottery()[LotteryID].ComputeWin(LotteryNumber, WinLotteryNumber, ref Description, ref WinMoneyNoWithTax, PlayTypeID, WinMoneyList);
        }
        catch
        {
        }

        lbDescription.Text = "";

        if (!string.IsNullOrEmpty(Description.Trim()))
        {
            lbDescription.Text = Description;
        }

        lbSchemeNumber.Text = dt.Rows[0]["SchemeNumber"].ToString();
        lbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
        lbAmount.Text = dt.Rows[0]["Money"].ToString();
        lbMultiple.Text = dt.Rows[0]["Multiple"].ToString();

        if (WinMoney > 0)
        {
            lbWinMoney.Text = WinMoney.ToString();
        }
        else
        {
            lbWinMoney.Text = "<color='red'>" + WinMoney + "</color>";
        }
    }
}
