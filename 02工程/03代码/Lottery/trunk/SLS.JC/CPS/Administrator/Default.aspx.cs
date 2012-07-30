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

public partial class Cps_Administrator_Default : AdminPageBase
{
    public string SubPage = "CpsPromoterList.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (_User.Competences.CompetencesList == "")
        {
            PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", "Admin_Default");

            return;
        }

        SubPage = Shove._Web.Utility.GetRequest("SubPage");

        if (SubPage == "")
        {
            SubPage = "CpsPromoterList.aspx";
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
}
