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

public partial class Admin_LotteryInformation : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
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
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LotteryInformation");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindData()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Lotteries().Open("Explain, SchemeExemple, Agreement, OpenAfficheTemplate", "[ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LotteryInformation");

            return;
        }
        
        tbExplain.Value = dt.Rows[0]["Explain"].ToString();
        tbLotteryExemple.Value = dt.Rows[0]["SchemeExemple"].ToString();
        tbAgreement.Value = dt.Rows[0]["Agreement"].ToString();
        tbOpenAfficheTemplate.Value = dt.Rows[0]["OpenAfficheTemplate"].ToString();
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }
        string sql = "update T_Lotteries set Explain = '" + Shove._Web.Utility.FilteSqlInfusion(tbExplain.Value) + "', SchemeExemple = '" + Shove._Web.Utility.FilteSqlInfusion(tbLotteryExemple.Value) + "', Agreement = '" + Shove._Web.Utility.FilteSqlInfusion(tbAgreement.Value) + "', OpenAfficheTemplate = '" + Shove._Web.Utility.FilteSqlInfusion(tbOpenAfficheTemplate.Value) + "' where [ID] = '" + Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + "'";
        if (MSSQL.ExecuteNonQuery(sql) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "保存失败。");

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "保存成功。");
    }
}
