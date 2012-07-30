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

public partial class Admin_JclcggEdit : AdminPageBase
{
    private long ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), 0);

            hID.Value = ID.ToString();

            BindData();

            btnEdit.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.LotteryIsuseScheme));
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

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_PassRateBasket().Open("", "ID=" + ID.ToString(), "[Day], MatchNumber");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        lbMatchNumber.Text = dt.Rows[0]["MatchNumber"].ToString();
        tbGame.Text = dt.Rows[0]["Game"].ToString();
        tbStopSellTime.Text = Shove._Convert.StrToDateTime(dt.Rows[0]["StopSellTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
        tbMainTeam.Text = dt.Rows[0]["MainTeam"].ToString();
        tbGuestTeam.Text = dt.Rows[0]["GuestTeam"].ToString();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        string Game = tbGame.Text.Trim();

        if (Game == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入联赛名称。");

            return;
        }

        string MainTeam = tbMainTeam.Text.Trim();

        if (MainTeam == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入主队名称。");

            return;
        }

        string GuestTeam = tbGuestTeam.Text.Trim();

        if (Game == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入客队名称。");

            return;
        }

        DAL.Tables.T_PassRateBasket t_PassRateBasket = new DAL.Tables.T_PassRateBasket();

        t_PassRateBasket.Game.Value = Game;
        t_PassRateBasket.MainTeam.Value = MainTeam;
        t_PassRateBasket.GuestTeam.Value = GuestTeam;
        t_PassRateBasket.StopSellTime.Value = tbStopSellTime.Text.Trim();

        long Result = t_PassRateBasket.Update("ID=" + hID.Value);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "修改失败。");

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "修改成功", "Jclcgg.aspx");
    }
}