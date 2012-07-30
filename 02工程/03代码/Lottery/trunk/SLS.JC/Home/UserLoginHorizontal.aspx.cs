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
using System.Security.Cryptography;
using System.Text;

public partial class Home_UserLoginHorizontal : SitePageBase
{
    public string labCheckCode = "验证码：";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

            new Login().SetCheckCode(_Site, ShoveCheckCode1);

            if (!isUseCheckCode)
            {
                labCheckCode = "";
                tbCheckCode.Visible = false;
                ShoveCheckCode1.Visible = false;
            }
        }

        _User = Users.GetCurrentUser(_Site.ID);

        if (_User != null)
        {
            Panel1.Visible = false;
            Panel2.Visible = true;

            lbUserName.Text = _User.Name + "";

            if (_User.Competences.CompetencesList != "")
            {
                lbUserType.Text = "级别:管理员";
            }
            else if (_User.OwnerSites != "")
            {
                lbUserType.Text = "级别:代理商";
            }
            else
            {
                switch (_User.UserType)
                {
                    case 1:
                        lbUserType.Text = "级别:普通";
                        break;
                    case 2:
                        lbUserType.Text = "级别:高级";
                        break;
                    case 3:
                        lbUserType.Text = "级别:VIP";
                        break;
                }
            }

            lbBalance.Text = "余额：<font color=\"red\">" + _User.Balance.ToString("N") + "</font> 元";

            btnAdmin.Visible = (_User.Competences.CompetencesList != "");
        }
        else
        {
            Panel1.Visible = true;
            Panel2.Visible = false;
        }
     
        tbUserPassword.Attributes.Add("value", tbUserPassword.Text);
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion


    protected void btnLogin_Click(object sender, System.EventArgs e)
    {
        string ReturnDescription = "";

        int Result = new Login().LoginSubmit(this.Page, _Site, tbUserName.Text, tbUserPassword.Text, tbCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);

        if (Result < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Response.Redirect(this.Request.Url.AbsoluteUri, true);
    }

    protected void btnLogout_Click(object sender, System.EventArgs e)
    {
        if (_User != null)
        {
            string ReturnDescription = "";
            _User.Logout(ref ReturnDescription);
        }
        else
        {
            this.Response.Redirect(this.Request.Url.AbsoluteUri, true);
        }

        Response.Redirect(this.Request.Url.AbsoluteUri, true);
    }

    protected void btnAdmin_Click(object sender, System.EventArgs e)
    {
        Response.Write(" <script   language=javascript> parent.location.href= '../Admin/Default.aspx'; </script> ");
        Response.End();
    }
}