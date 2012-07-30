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

using Shove.Database;

public partial class Admin_LotteryGateway : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            BindDataForLottery();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        SystemOptions so = new SystemOptions();
        //恒朋江西电子票参数
        tb1.Text = so["ElectronTicket_HPJX_Getway"].ToString("");
        tb2.Text = so["ElectronTicket_HPJX_UserName"].ToString("");
        tb3.Attributes.Add("value", so["ElectronTicket_HPJX_UserPassword"].ToString(""));

        cb1.Checked = so["ElectronTicket_HPJX_Status_ON"].ToBoolean(false);

        //恒朋上海电子票参数
        tb_HPSH_Getway.Text = so["ElectronTicket_HPSH_Getway"].ToString("");
        tb_AgentID.Text = so["ElectronTicket_HPSH_UserName"].ToString("");
        tb_AgentPwd.Attributes.Add("value", so["ElectronTicket_HPSH_UserPassword"].ToString(""));

        cb_HPSH.Checked = so["ElectronTicket_HPSH_Status_ON"].ToBoolean(false);

        //恒朋上海电子票参数
        tb_HPSD_Getway.Text = so["ElectronTicket_HPSD_Getway"].ToString("");
        tb_HPSD_AgentID.Text = so["ElectronTicket_HPSD_UserName"].ToString("");
        tb_HPSD_AgentPwd.Attributes.Add("value", so["ElectronTicket_HPSD_UserPassword"].ToString(""));

        cb_HPSD.Checked = so["ElectronTicket_HPSD_Status_ON"].ToBoolean(false);
    }

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name], PrintOutType", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt;
        g.DataBind();

        g.Columns[2].Visible = false;
        g.Columns[3].Visible = false;

        // 设置出票方式的下拉框
        for (int i = 0; i < g.Rows.Count; i++)
        {
            DropDownList ddlElectronTicket = (DropDownList)g.Rows[i].Cells[1].FindControl("ddlElectronTicket");

            int LotteryID = Shove._Convert.StrToInt(g.Rows[i].Cells[2].Text, -1);

            if (LotteryID < 1)
            {
                continue;
            }

            short PrintOutType = Shove._Convert.StrToShort(g.Rows[i].Cells[3].Text, 0);

            Shove.ControlExt.SetDownListBoxTextFromValue(ddlElectronTicket, PrintOutType.ToString());
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        SystemOptions so = new SystemOptions();
        //恒朋江西电子票参数
        so["ElectronTicket_HPJX_Getway"] = new OptionValue(tb1.Text);
        so["ElectronTicket_HPJX_UserName"] = new OptionValue(tb2.Text);
        so["ElectronTicket_HPJX_UserPassword"] = new OptionValue(tb3.Text);
        so["ElectronTicket_HPJX_Status_ON"] = new OptionValue(cb1.Checked);

        //恒朋上海电子票参数
        so["ElectronTicket_HPSH_Getway"] = new OptionValue(tb_HPSH_Getway.Text);
        so["ElectronTicket_HPSH_UserName"] = new OptionValue(tb_AgentID.Text);
        so["ElectronTicket_HPSH_UserPassword"] = new OptionValue(tb_AgentPwd.Text);
        so["ElectronTicket_HPSH_Status_ON"] = new OptionValue(cb_HPSH.Checked);

        //恒朋山东电子票参数
        so["ElectronTicket_HPSD_Getway"] = new OptionValue(tb_HPSD_Getway.Text);
        so["ElectronTicket_HPSD_UserName"] = new OptionValue(tb_HPSD_AgentID.Text);
        so["ElectronTicket_HPSD_UserPassword"] = new OptionValue(tb_HPSD_AgentPwd.Text);
        so["ElectronTicket_HPSD_Status_ON"] = new OptionValue(cb_HPSD.Checked);

        for (int i = 0; i < g.Rows.Count; i++)
        {
            DropDownList ddlElectronTicket = (DropDownList)g.Rows[i].Cells[1].FindControl("ddlElectronTicket");

            MSSQL.ExecuteNonQuery("update T_Lotteries set PrintOutType = " + ddlElectronTicket.SelectedValue + " where [ID] = " + g.Rows[i].Cells[2].Text);
        }

        tb3.Attributes.Add("value", tb3.Text);

        Shove._Web.JavaScript.Alert(this.Page, "站点资料已经保存成功。");
    }
}
