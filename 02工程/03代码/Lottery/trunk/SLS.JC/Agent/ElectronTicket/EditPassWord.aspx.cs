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

public partial class Agent_ElectronTicket_EditPassWord : ElectronTicketAgentsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
 
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion


    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (tbOldPassWord.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入密码。");
            return;
        }

        if (tbNewPassWord.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入新密码。");
            return;
        }

        if (PF.EncryptPassword(tbOldPassWord.Text.Trim()) != _ElectronTicketAgents.Password)
        {
            Shove._Web.JavaScript.Alert(this.Page, "密码有误，请重新输入。");
            return;
        }

        if (tbRePassWord.Text.Trim() != tbNewPassWord.Text.Trim())
        {
            Shove._Web.JavaScript.Alert(this.Page, "两次输入的密码不相同。");
            return;
        }

        ElectronTicketAgents t_User = new ElectronTicketAgents();
        _ElectronTicketAgents.Clone(t_User);

        _ElectronTicketAgents.Password = PF.EncryptPassword(tbNewPassWord.Text.Trim());
        
        string ReturnDescription = "";

        if (_ElectronTicketAgents.EditByID(ref ReturnDescription) < 0)
        {
            t_User.Clone(_ElectronTicketAgents);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "用户密码已经保存成功。");
    }
}
