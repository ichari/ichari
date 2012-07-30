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

public partial class Admin_UserAddMoney : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = "Admin/UserAddMoney.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.AddMoney);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Sites().Open("[ID], [Name]", "", "[Level], [ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_UserAddMoney");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlSites, dt, "Name", "ID");
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        long SiteID = Shove._Convert.StrToLong(ddlSites.SelectedValue, -1);

        if (SiteID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的站点编号。");

            return;
        }
        
        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名称。");

            return;
        }

        double Money = Shove._Convert.StrToDouble(tbMoney.Text, 0);

        if (Money == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的金额。");

            return;
        }

        Users tu = new Users(SiteID)[SiteID, tbUserName.Text.Trim()];

        if (tu == null)
        {
            PF.GoError(ErrorNumber.Unknow, "用户不存在", "Admin_UserAddMoney");

            return;
        }

        string Message = "手工充值";
        string ReturnDescription = "";

        if (rb2.Checked)
        {
            Message += "，获得的奖励";
        }
        else if (rb3.Checked)
        {
            Message += "，购彩";
        }
        else if (rb4.Checked)
        {
            Message += "，预付款";
        }
        else if (rb5.Checked)
        {
            Message += "，转帐户";
        }
        else if (rb6.Checked)
        {
            Message += "，擂台奖励";
        }

        if (tbMessage.Text.Trim() != "")
        {
            Message += "，" + tbMessage.Text.Trim();
        }

        if (tu.AddUserBalanceManual(Money, Message, _User.ID, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_UserAddMoney");

            return;
        }

        if (rb6.Checked)
        {
            DAL.Tables.T_ChallengeBetRed t_ChallengeBetRed = new DAL.Tables.T_ChallengeBetRed();

            // 获取用户当前奖金
            string TotalMoney = Shove.Database.MSSQL.ExecuteScalar("select TotalWinMoney from T_ChallengeBetRed where UserId = " + tu.ID.ToString()) + "";

            t_ChallengeBetRed.TotalWinMoney.Value = Shove._Convert.StrToDouble(TotalMoney, 0) + Money;
            t_ChallengeBetRed.Update("UserId = " + tu.ID.ToString());
            // 清楚擂台缓存
            Shove._Web.Cache.ClearCache("DataCache_Challenge_72_BetHot");
        }

        tbUserName.Text = "";
        tbMoney.Text = "";
        tbMessage.Text = "";

        Shove._Web.JavaScript.Alert(this, "为用户充值成功。");
    }
}
