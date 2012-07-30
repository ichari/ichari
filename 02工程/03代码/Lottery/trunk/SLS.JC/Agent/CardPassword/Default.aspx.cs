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

public partial class Agent_CardPassword_Default : CardPasswordPageBase
{
    public string SubPage = "Welcome.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        SubPage = Shove._Web.Utility.GetRequest("SubPage");

        if (SubPage == "")
        {
            SubPage = "Welcome.aspx";
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion
}
