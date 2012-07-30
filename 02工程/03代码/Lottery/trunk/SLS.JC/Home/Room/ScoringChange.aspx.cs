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

public partial class Home_Room_ScoringChange : SitePageBase
{
    public double Opt_ScoringExchangeRate;
    protected double ScoringOfSelfBuy;
    protected double ScoringOfCommendBuy;

    protected void Page_Load(object sender, EventArgs e)
    {
        Opt_ScoringExchangeRate = _Site.SiteOptions["Opt_ScoringExchangeRate"].ToDouble(100);
        ScoringOfSelfBuy = _User.ScoringOfSelfBuy;
        ScoringOfCommendBuy = _User.ScoringOfCommendBuy;

        Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

        labScoringSum.Text = _User.Scoring.ToString();
        labScoring.Text = ((int)(_User.Scoring / Opt_ScoringExchangeRate) * Opt_ScoringExchangeRate).ToString();

        if (_User.Scoring < 10)
        {
            tbScoring.Enabled = false;
            btnOK.Enabled = false;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
        isRequestLogin = true;

        base.OnInit(e);
    }

    #endregion

    #region 积分兑换
    protected void btnOK_Click(object sender, EventArgs e)
    {
        string ReturnDescription = "";

        labScoringSum.Text = _User.Scoring.ToString();
       // labScoring.Text = ((int)(_User.Scoring / Opt_ScoringExchangeRate) * Opt_ScoringExchangeRate).ToString();
        double Scoring = Shove._Convert.StrToDouble(Shove._Web.Utility.FilteSqlInfusion(tbScoring.Text), 0);

        if ((Scoring < Opt_ScoringExchangeRate) || (Scoring > _User.Scoring) || (int)(Scoring / Opt_ScoringExchangeRate) * Opt_ScoringExchangeRate != Scoring)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的兑换积分.");

            return;
        }

        ReturnDescription = "";
        int Result = _User.ScoringExchange(Scoring, ref ReturnDescription);

        if ((ReturnDescription != "") || (Result != 0))
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.Page.GetType().BaseType.FullName);

            return;
        }

        labScoringSum.Text = _User.Scoring.ToString();
        labScoring.Text = (Convert.ToInt32(labScoring.Text) - Convert.ToInt32(Scoring)).ToString();
      //  labScoringSum.Text = ((int)(_User.Scoring / Opt_ScoringExchangeRate) * Opt_ScoringExchangeRate).ToString();
        tbScoring.Text = "";

        Shove._Web.JavaScript.Alert(this.Page, "积分兑换成功,兑换金额已经存到您的可用资金中。");

        return;
    }
    #endregion
}
