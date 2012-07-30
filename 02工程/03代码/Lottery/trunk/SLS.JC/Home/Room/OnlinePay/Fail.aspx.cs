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
using System.Text;

public partial class Home_Room_OnlinePay_Fail : RoomPageBase
{
    protected void Page_Load(object sender, System.EventArgs e)
    {
        if (!IsPostBack)
        {
            string errMsg =Shove._Web.Utility.GetRequest("errMsg");

            lab1.Text = string.IsNullOrEmpty(errMsg) == true ? _Site.Name : HttpUtility.UrlDecode(errMsg, Encoding.GetEncoding("GB2312"));

        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        isRequestLogin = true;
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isAllowPageCache = false;
       
        base.OnLoad(e);
    }

    #endregion
}
