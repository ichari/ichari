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

public partial class Agent_ElectronTicket_Login : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        new Login().SetCheckCode(_Site, CheckCode);
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        base.OnInit(e);
    }

    #endregion

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string ReturnDescription = "";

        int Result = LoginSubmit(this.Page, _Site, tbID.Text, tbPassword.Text, tbCheckCode.Text, CheckCode, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        new Login().GoToRequestLoginPage("Default.aspx");
    }

    public int LoginSubmit(Page page, Sites site, string ID, string Password, string InputCheckCode, Shove.Web.UI.ShoveCheckCode sccCheckCode, ref string ReturnDescription)
    {
        ReturnDescription = "";

        bool Opt_isUseCheckCode = site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

        ID = ID.Trim();
        Password = Password.Trim();

        if ((ID == "") || (Password == ""))
        {
            ReturnDescription = "用户名和密码都不能为空";

            return -1;
        }

        if ((Opt_isUseCheckCode) && (!sccCheckCode.Valid(InputCheckCode)))
        {
            ReturnDescription = "验证码输入错误";

            return -2;
        }

        System.Threading.Thread.Sleep(500);

        ElectronTicketAgents electronTicketAgents = new ElectronTicketAgents();
        electronTicketAgents.ID = Shove._Convert.StrToInt(ID, 0);
        electronTicketAgents.Password = Password;

        return electronTicketAgents.Login(ref ReturnDescription);
    }
}
