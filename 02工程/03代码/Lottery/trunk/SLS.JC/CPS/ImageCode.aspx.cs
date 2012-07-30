using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_ImageCode : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbImageCode1.Text = BindImageCode("ad_tg_2.jpg", 619, 60);
            tbImageCode2.Text = BindImageCode("ad_tg_2.jpg", 619, 60);
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

    private string BindImageCode(string imageURL, int width, int height)
    {
        string code = "<a href='"+Shove._Web.Utility.GetUrl()+"/Default.aspx?cpsid="+_User.cps.ID.ToString()+"' target='_bank'>";
        imageURL = Shove._Web.Utility.GetUrl() + "/Cps/Images/" + imageURL;
        code += "<img src='" + imageURL + "' width='" + width + "' height='" + height + "' border='0'/>";
        code += "</a>";

        return code;
    }
}
