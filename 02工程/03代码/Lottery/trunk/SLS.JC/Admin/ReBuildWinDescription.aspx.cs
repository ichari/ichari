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

public partial class Admin_ReBuildWinDescription : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryWin);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");

        if (ddlLottery.Items.Count < 1)
        {
            btnGO.Enabled = false;
        }
        else
        {
            ddlLottery_SelectedIndexChanged(ddlLottery, new EventArgs());
        }
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and isOpened = 1", "EndTime desc");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {
            WinNumberOther.Visible = true;
            btnGO.Enabled = true;

            ddlIsuse_SelectedIndexChanged(ddlIsuse, new EventArgs());
        }
        else
        {
            WinNumberOther.Visible = true;
            btnGO.Enabled = false;
        }
    }

    private void BindDataForWinMoney()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_WinTypes().Open("", "LotteryID = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt.DefaultView;
        g.DataBind();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForWinMoney();
        BindDataForIsuse();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("WinLotteryNumber, isOpened", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        bool isOpened = Shove._Convert.StrToBool(dt.Rows[0]["isOpened"].ToString(), true);
        string WinLotteryNumber = dt.Rows[0]["WinLotteryNumber"].ToString();

        if (!isOpened)
        {
            btnGO.Enabled = false;

            PF.GoError(ErrorNumber.Unknow, "此期还没有开奖，不能重构中奖描述。", this.GetType().BaseType.FullName);

            return;
        }

        tbWinNumber.Text = WinLotteryNumber;
        btnGO.Enabled = true;
    }

    protected void g_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataKey key = g.DataKeys[g.DataKeys.Count - 1];

            double money = Shove._Convert.StrToDouble(key.Values[0].ToString(), 0);
            ((TextBox)e.Row.Cells[1].FindControl("tbMoney")).Text = (money == 0 ? "" : money.ToString());

            money = Shove._Convert.StrToDouble(key.Values[1].ToString(), 0);
            ((TextBox)e.Row.Cells[2].FindControl("tbMoneyNoWithTax")).Text = (money == 0 ? "" : money.ToString());
        }
    }

    protected void btnGO_Click(object sender, System.EventArgs e)
    {
        tbWinNumber.Text = Shove._Convert.ToDBC(tbWinNumber.Text.Trim().Replace("　", " ")).Trim();

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(tbWinNumber.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "开奖号码不正确！");

            return;
        }

        double[] WinMoneyList = new double[g.Rows.Count * 2];

        for (int i = 0; i < g.Rows.Count; i++)
        {
            WinMoneyList[i * 2] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[1].FindControl("tbMoney")).Text, 0);
            WinMoneyList[i * 2 + 1] = Shove._Convert.StrToDouble(((TextBox)g.Rows[i].Cells[2].FindControl("tbMoneyNoWithTax")).Text, 0);

            if (WinMoneyList[i * 2] < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "第 " + (i + 1).ToString() + " 项奖金输入错误！");

                return;
            }
        }

        DAL.Tables.T_Schemes T_Schemes = new DAL.Tables.T_Schemes();

        DataTable dt = T_Schemes.Open("", "IsuseID = " + ddlIsuse.SelectedValue + " and isOpened = 1", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            string LotteryNumber = dt.Rows[i]["LotteryNumber"].ToString();

            string Description = "";
            double WinMoneyNoWithTax = 0;

            double WinMoney = new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].ComputeWin(LotteryNumber, tbWinNumber.Text.Trim(), ref Description, ref WinMoneyNoWithTax, int.Parse(dt.Rows[i]["PlayTypeID"].ToString()), WinMoneyList);

            int Multiple = Shove._Convert.StrToInt(dt.Rows[i]["Multiple"].ToString(), 1);

            T_Schemes.WinDescription.Value = Description;
            T_Schemes.Update("[ID] = " + dt.Rows[i]["ID"].ToString());
        }

        //tbWinNumber.Text = "";

        Shove._Web.JavaScript.Alert(this.Page, "重构中奖描述成功。");
    }
}
