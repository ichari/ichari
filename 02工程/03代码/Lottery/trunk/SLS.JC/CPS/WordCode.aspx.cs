using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_WordCode : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbWordCode1.Text = BindImageCode("买彩票，就上"+ _Site.Name + "。");
            tbWordCode2.Text = BindImageCode("买彩票，就上" + _Site.Name + "。");
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private string BindImageCode(string Word)
    {
        string code = "<a style='line-height:22px;color:#c00; FONT-SIZE:14px; FONT-WEIGHT:600' href='" + Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid=" + _User.cps.ID.ToString() + "' target='_bank'>" + Word + "</a>";
       
        return code;
    }
}
