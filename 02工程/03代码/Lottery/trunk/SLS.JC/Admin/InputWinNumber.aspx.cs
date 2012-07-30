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

public partial class Admin_InputWinNumber : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            BindData();

            btnGO.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent)); 
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryListRestrictions == "" ? "-1" : _Site.UseLotteryListRestrictions) + ") and [ID] <> " + SLS.Lottery.ZCDC.sID, "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_InputWinNumber");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");

        if (ddlLottery.Items.Count < 1)
        {
            btnGO.Enabled = false;
            tbWinNumber.Enabled = false;
        }
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " +  Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and EndTime < GetDate() and isOpened = 0", "EndTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_InputWinNumber");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {
            btnGO.Enabled = true;
            tbWinNumber.Enabled = true;
        }
        else
        {
            btnGO.Enabled = false;
            tbWinNumber.Enabled = false;

            tbWinNumber.Text = "";
        }
    }

    private void BindData()
    {
        if (ddlIsuse.Items.Count < 1)
        {
            return;
        }

        string WinLotteryExemple = "格式：" + DAL.Functions.F_GetLotteryWinNumberExemple(int.Parse(ddlLottery.SelectedValue));
        labTip.Text = WinLotteryExemple;
        tbWinNumber.MaxLength = WinLotteryExemple.Length - 3;

        object obj = MSSQL.ExecuteScalar("select WinLotteryNumber from T_Isuses where [ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue));

        if (obj != null)
        {
            tbWinNumber.Text = Convert.ToString(obj);
        }

        // 判断是否开奖过
        bool isOpened = true;

        try
        {
            isOpened = Convert.ToBoolean(MSSQL.ExecuteScalar("select isOpened from T_Isuses where [ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue)));
        }
        catch { }

        if (isOpened)
        {
            btnGO.Enabled = false;
            tbWinNumber.Enabled = false;

            Shove._Web.JavaScript.Alert(this.Page, "此期已经开奖了，不能修改中奖号码。");

            return;
        }

        btnGO.Enabled = true;
        tbWinNumber.Enabled = true;
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();

        BindData();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnGO_Click(object sender, System.EventArgs e)
    {
        tbWinNumber.Text = tbWinNumber.Text.Trim();

        if (!new SLS.Lottery()[int.Parse(ddlLottery.SelectedValue)].AnalyseWinNumber(tbWinNumber.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "中奖号码格式不正确！");

            return;
        }

        DAL.Tables.T_Isuses T_Isuses = new DAL.Tables.T_Isuses();

        T_Isuses.WinLotteryNumber.Value = tbWinNumber.Text;

        if (T_Isuses.Update("[ID] = " +  Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue)) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_InputWinNumber");

            return;
        }

        Shove._Web.Cache.ClearCache(DataCache.IsusesInfo + ddlLottery.SelectedValue);
        Shove._Web.JavaScript.Alert(this.Page, "中奖号码填写成功。");
    }
}
