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

public partial class Admin_SchemesFormulaeSet : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemesFormulaeSet");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindData()
    {
        tbMoney.Text = "";
        tbSchedule.Text = "";
        tbMinMoney.Text = "";

        DataTable dt = new DAL.Tables.T_SchemesFormulae().Open("", "LotteryID=" + ddlLottery.SelectedValue, "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SchemesFormulaeSet");

            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        tbMoney.Text = dt.Rows[0]["Money"].ToString();
        tbSchedule.Text = dt.Rows[0]["Schedule"].ToString();
        tbMinMoney.Text = dt.Rows[0]["MinMoney"].ToString();
        IsSet.Value = "1";
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnAdd_Click(object sender, System.EventArgs e)
    {
        if (string.IsNullOrEmpty(tbMoney.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入方案最低金额！");

            return;
        }

        if (Shove._Convert.StrToFloat(tbMoney.Text.Trim(), 0) == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入最低方案金额输入有误，请重新输入！");

            return;
        }

        if (string.IsNullOrEmpty(tbSchedule.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入方案进度加保底的最少金额！");

            return;
        }

        if (Shove._Convert.StrToFloat(tbSchedule.Text.Trim(), 0) == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入方案的最少进度加保底金额输入有误，请重新输入！");

            return;
        }

        if (string.IsNullOrEmpty(tbMinMoney.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入方案最小进度！");

            return;
        }

        if (Shove._Convert.StrToFloat(tbMinMoney.Text.Trim(), 0) == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入方案的最少进度输入有误，请重新输入！");

            return;
        }

        DAL.Tables.T_SchemesFormulae t_SchemesFormulae = new DAL.Tables.T_SchemesFormulae();

        t_SchemesFormulae.Money.Value = Shove._Convert.StrToFloat(tbMoney.Text.Trim(), 0);
        t_SchemesFormulae.Schedule.Value = Shove._Convert.StrToFloat(tbSchedule.Text.Trim(), 0);
        t_SchemesFormulae.MinMoney.Value = Shove._Convert.StrToFloat(tbMinMoney.Text.Trim(), 0);

        if (IsSet.Value != "1")
        {
            t_SchemesFormulae.LotteryID.Value = Shove._Convert.StrToShort(ddlLottery.SelectedValue, 0);

            t_SchemesFormulae.Insert();

            Shove._Web.JavaScript.Alert(this.Page, "彩种已经设置成功！");

            return;
        }

        t_SchemesFormulae.Update("LotteryID=" + ddlLottery.SelectedValue);

        Shove._Web.JavaScript.Alert(this.Page, "彩种已经设置成功！");

        return;
    }
}
