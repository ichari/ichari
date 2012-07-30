using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cps_Administrator_DefaultFrameTop : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            labUserName.Text = _User.Name;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    protected void linkLogout_Click(object sender, EventArgs e)
    {
        string ReturnDescription = "";

        if (_User != null)
        {
            if (_User.Logout(ref ReturnDescription) < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, this.GetType().FullName);

                return;
            }
        }

        Response.Write("<script language=\"javascript\">window.top.location.href=\"../Default.aspx\"</script>");
    }   
}
