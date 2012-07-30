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

public partial class Admin_Isuse : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindData();

            btnAdd.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.LotteryIsuseScheme,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (" + (_Site.UseLotteryList == "" ? "-1" : _Site.UseLotteryList) + ")", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");

        string LotteryID = Shove._Web.Utility.GetRequest("LotteryID");

        if (LotteryID != "")
        {
            Shove.ControlExt.SetDownListBoxTextFromValue(ddlLottery, LotteryID);
        }
    }

    private void BindData()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("", "LotteryID = " +  Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and EndTime > GetDate()", "EndTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        g.DataSource = dt;
        g.DataBind();

        // 读最后一期期号，以提示用户该继续输入哪一期
        DataTable dtLastIsuse = new DAL.Tables.T_Isuses().Open("top 1 [Name], StartTime, EndTime", "LotteryID = " +  Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue), "EndTime desc");

        if (dtLastIsuse == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dtLastIsuse.Rows.Count < 1)
        {
            labLastIsuseTip.Text = "此彩种还没有添加过任何期号。";
        }
        else
        {
            labLastIsuseTip.Text = "已添加的最后期号：" + dtLastIsuse.Rows[0]["Name"].ToString() + "，开始时间：" + Shove._Convert.StrToDateTime(dtLastIsuse.Rows[0]["StartTime"].ToString(), "0000-00-00 00:00:00").ToString("yyyy-MM-dd HH:mm:ss") + "，截止时间：" + Shove._Convert.StrToDateTime(dtLastIsuse.Rows[0]["EndTime"].ToString(), "0000-00-00 00:00:00").ToString("yyyy-MM-dd HH:mm:ss") + "。";
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        this.Response.Redirect("IsuseAdd.aspx?LotteryID=" + ddlLottery.SelectedValue, true);
    }
}
