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

public partial class Agent_ElectronTicket_Scheme : ElectronTicketAgentsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
           long SchemeID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

            if (SchemeID < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Agent_ElectronTicket_Scheme");

                return;
            }

            BindData(SchemeID);
        }
    }

    private void BindData(long SchemeID)
    {
        DataTable dt = new DAL.Tables.T_ElectronTicketAgentSchemes().Open("SchemeNumber, DateTime, Amount, LotteryNumber, Multiple, state", " ID =" + SchemeID.ToString(), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Agent_ElectronTicket_Scheme");

            return;
        }

        string t_str = dt.Rows[0]["LotteryNumber"].ToString();

        lbSchemeNumber.Text = dt.Rows[0]["SchemeNumber"].ToString();
        lbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
        lbAmount.Text = dt.Rows[0]["Amount"].ToString();
        lbMultiple.Text = dt.Rows[0]["Multiple"].ToString();

        string state = dt.Rows[0]["state"].ToString();

        lbState.Text = "失败";
        if (state == "1")
        {
            lbState.Text = "成功";
        }


        DataTable dtLotteryNumber = new DAL.Tables.T_ElectronTicketAgentSchemesElectronTickets().Open("Ticket, identifiers", " SchemeID=" + SchemeID.ToString(), "");

        if ((dtLotteryNumber == null) || dtLotteryNumber.Rows.Count < 1)
        {
            dtLotteryNumber = new DAL.Tables.T_ElectronTicketAgentSchemesElectronTickets().Open("Ticket, identifiers", " SchemeID=" + SchemeID.ToString(), "");

            if ((dtLotteryNumber == null) || dtLotteryNumber.Rows.Count < 1)
            {
                return;
            }
        }

        dtLotteryNumber.Columns.Add("count", typeof(System.String));

        for (int i = 0; i < dtLotteryNumber.Rows.Count; i++)
        {
            dtLotteryNumber.Rows[i]["count"] = (i + 1).ToString();
            dtLotteryNumber.AcceptChanges();
        }

        RpElectronTicket.DataSource = dtLotteryNumber;
        RpElectronTicket.DataBind();
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAtFramePageLogin = true;
        base.OnInit(e);
    }

    #endregion
}
