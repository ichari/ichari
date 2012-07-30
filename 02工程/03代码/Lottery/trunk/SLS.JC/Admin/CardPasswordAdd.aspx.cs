using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class Admin_CardPasswordAdd : AdminPageBase
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
        RequestLoginPage = "Admin/CardPasswordAdd.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_CardPasswordAgents().Open("[ID], [Name]", "", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_CardPasswordAdd");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlCardPasswordAgents, dt, "Name", "ID");
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        int AgentsID = Shove._Convert.StrToInt(ddlCardPasswordAgents.SelectedValue, -1);

        if (AgentsID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的代理商编号。");
            ddlCardPasswordAgents.Focus();

            return;
        }

        int Period = Shove._Convert.StrToInt(tbDateTime.Text, 0);

        if (Period < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的过期时间。");
            tbDateTime.Focus();

            return;
        }

        double Money = Shove._Convert.StrToDouble(tbMoney.Text, 0);

        if (Money == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入有效的金额。");
            tbMoney.Focus();

            return;
        }

        int Count = Shove._Convert.StrToInt(tbCount.Text, 0);

        if (Count < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的卡密总数。");
            tbCount.Focus();

            return;
        }

        string ReturnDescription = "";

        if (new CardPassword().Add(AgentsID, Period, Money, Count, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_CardPasswordAdd");

            return;
        }

        if (ReturnDescription != "")
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "卡密增加成功!");
    }
}
