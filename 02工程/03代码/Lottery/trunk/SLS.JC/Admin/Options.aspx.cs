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

public partial class Admin_Options : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

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
        cbisWriteLog.Checked = _Site.SiteOptions["Opt_isWriteLog"].ToBoolean(true);

        tbInitiateSchemeLimitLowerScaleMoney.Text = _Site.SiteOptions["Opt_InitiateSchemeLimitLowerScaleMoney"].ToString("");
        tbInitiateSchemeLimitLowerScale.Text = _Site.SiteOptions["Opt_InitiateSchemeLimitLowerScale"].ToString("");
        tbInitiateSchemeLimitUpperScaleMoney.Text = _Site.SiteOptions["Opt_InitiateSchemeLimitUpperScaleMoney"].ToString("");
        tbInitiateSchemeLimitUpperScale.Text = _Site.SiteOptions["Opt_InitiateSchemeLimitUpperScale"].ToString("");

        tbInitiateSchemeBonusScale.Text = _Site.SiteOptions["Opt_InitiateSchemeBonusScale"].ToString("");
        tbInitiateSchemeMinBuyAndAssureScale.Text = _Site.SiteOptions["Opt_InitiateSchemeMinBuyAndAssureScale"].ToString("");
        tbInitiateSchemeMaxNum.Text = _Site.SiteOptions["Opt_InitiateSchemeMaxNum"].ToString("");
        tbInitiateFollowSchemeMaxNum.Text = _Site.SiteOptions["Opt_InitiateFollowSchemeMaxNum"].ToString("");
        tbQuashSchemeMaxNum.Text = _Site.SiteOptions["Opt_QuashSchemeMaxNum"].ToString("");
        cbFullSchemeCanQuash.Checked = _Site.SiteOptions["Opt_FullSchemeCanQuash"].ToBoolean(true);

        tbMaxShowLotteryNumberRows.Text = _Site.SiteOptions["Opt_MaxShowLotteryNumberRows"].ToString("");

        tbScoringOfSelfBuy.Text = _Site.SiteOptions["Opt_ScoringOfSelfBuy"].ToString("");
        tbScoringOfCommendBuy.Text = _Site.SiteOptions["Opt_ScoringOfCommendBuy"].ToString("");
        tbScoringExchangeRate.Text = _Site.SiteOptions["Opt_ScoringExchangeRate"].ToString("");
        cbScoring_Status_ON.Checked = _Site.SiteOptions["Opt_Scoring_Status_ON"].ToBoolean(true);
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Shove._Convert.StrToDouble(tbInitiateSchemeBonusScale.Text, 0) > 1 || Shove._Convert.StrToDouble(tbInitiateSchemeBonusScale.Text, 0) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "方案制作佣金设置有错误，设置范围是( 0 - 1 ) 例如 0.03　！");
            return;
        }

        try
        {
            _Site.SiteOptions["Opt_isWriteLog"] = new OptionValue(cbisWriteLog.Checked);

            _Site.SiteOptions["Opt_InitiateSchemeLimitLowerScaleMoney"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeLimitLowerScaleMoney.Text, 0));
            _Site.SiteOptions["Opt_InitiateSchemeLimitLowerScale"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeLimitLowerScale.Text, 0));
            _Site.SiteOptions["Opt_InitiateSchemeLimitUpperScaleMoney"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeLimitUpperScaleMoney.Text, 0));
            _Site.SiteOptions["Opt_InitiateSchemeLimitUpperScale"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeLimitUpperScale.Text, 0));

            _Site.SiteOptions["Opt_InitiateSchemeBonusScale"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeBonusScale.Text, 0));
            _Site.SiteOptions["Opt_InitiateSchemeMinBuyAndAssureScale"] = new OptionValue(Shove._Convert.StrToDouble(tbInitiateSchemeMinBuyAndAssureScale.Text, 0));
            _Site.SiteOptions["Opt_InitiateSchemeMaxNum"] = new OptionValue(Shove._Convert.StrToShort(tbInitiateSchemeMaxNum.Text, 1000));
            _Site.SiteOptions["Opt_InitiateFollowSchemeMaxNum"] = new OptionValue(Shove._Convert.StrToShort(tbInitiateFollowSchemeMaxNum.Text, 1));
            _Site.SiteOptions["Opt_QuashSchemeMaxNum"] = new OptionValue(Shove._Convert.StrToShort(tbQuashSchemeMaxNum.Text, 3));
            _Site.SiteOptions["Opt_FullSchemeCanQuash"] = new OptionValue(cbFullSchemeCanQuash.Checked);
            _Site.SiteOptions["Opt_MaxShowLotteryNumberRows"] = new OptionValue(Shove._Convert.StrToShort(tbMaxShowLotteryNumberRows.Text, 5));
            
            _Site.SiteOptions["Opt_ScoringOfSelfBuy"] = new OptionValue(Shove._Convert.StrToDouble(tbScoringOfSelfBuy.Text, 0));
            _Site.SiteOptions["Opt_ScoringOfCommendBuy"] = new OptionValue(Shove._Convert.StrToDouble(tbScoringOfCommendBuy.Text, 0));
            _Site.SiteOptions["Opt_ScoringExchangeRate"] = new OptionValue(Shove._Convert.StrToDouble(tbScoringExchangeRate.Text, 100));
            _Site.SiteOptions["Opt_Scoring_Status_ON"] = new OptionValue(cbScoring_Status_ON.Checked);
        }
        catch (Exception exception)
        {
            PF.GoError(ErrorNumber.Unknow, exception.Message, this.GetType().BaseType.FullName);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "系统参数已经保存成功。");
    }
}
