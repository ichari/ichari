using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_Login : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

            trCheckCode.Visible = isUseCheckCode;

            new Login().SetCheckCode(_Site, CheckCode);

            if (_User != null)
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = false;

        base.OnInit(e);
    }

    #endregion

    protected void btnOK_Click(object sender, ImageClickEventArgs e)
    {
        string ReturnDescription = "";
        int Result = -1;

        if (this.trCheckCode.Visible)
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbUserName.Text, tbPwd.Text, tbCheckCode.Text, CheckCode, ref ReturnDescription);
        }
        else
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbUserName.Text, tbPwd.Text, ref ReturnDescription);
        }

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Response.Redirect(Shove._Web.Utility.GetRequest("RequestLoginPage"));
    }

}
