using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

/// <summary>
///CardPasswordPageBase 的摘要说明
/// </summary>
public class CardPasswordPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public CardPasswordAgentUsers _CardPasswordAgentUser;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isAtFramePageLogin = true;              // 是否框架内的页面转跳登录。

    public CardPasswordPageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    protected override void OnLoad(EventArgs e)
    {
        #region 获取站点

        //_Site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];
        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().FullName);

            return;
        }

        #endregion

        #region 获取用户

        _CardPasswordAgentUser = CardPasswordAgentUsers.GetCurrentUser();

        if (isRequestLogin && (_CardPasswordAgentUser == null))
        {
            Response.Redirect("Login.aspx");

            return;
        }

        #endregion

        HtmlMeta hm = new HtmlMeta();
        hm.HttpEquiv = "X-UA-Compatible";
        hm.Content = "IE=EmulateIE7";

        base.OnLoad(e);
    }
}
