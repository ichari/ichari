using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

/// <summary>
///Class1 的摘要说明
/// </summary>
public class CpsInterfacePageBase: System.Web.UI.Page
{
    public CpsInterfacePageBase()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    public Sites _Site;
    public Users _User;
    public static long BusinessID = 0;

    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isRequestLogin = true;

    protected override void OnLoad(EventArgs e)
    {
        string ReturnDescription = "";

        #region 获取站点

        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().BaseType.FullName);

            return;
        }

        #endregion

        #region 获取用户

        _User = Users.GetCurrentUser(_Site.ID);

        if (isRequestLogin&&_User == null)
        {
            GoLogin(RequestLoginPage);

            return;
        }

        if (isRequestLogin&& _User.cps.ID < 0)
        {
            _User.Logout(ref ReturnDescription);

            PF.GoError(ErrorNumber.Unknow, "您不是 CPS 用户", this.GetType().BaseType.FullName);

            return;
        }

        if (isRequestLogin && _User.cps.ID != BusinessID && _User.cps.ParentID != BusinessID)
        {
            _User.Logout(ref ReturnDescription);

            PF.GoError(ErrorNumber.DataReadWrite, "您不是本站的推广商家，禁止登录", this.GetType().BaseType.FullName);

            return;    
        }

        if (isRequestLogin&& !_User.cps.ON)
        {
            _User.Logout(ref ReturnDescription);

            PF.GoError(ErrorNumber.Unknow, "用户被冻结", this.GetType().BaseType.FullName);

            return;
        }

        #endregion

        HtmlMeta hm = new HtmlMeta();
        hm.HttpEquiv = "X-UA-Compatible";
        hm.Content = "IE=EmulateIE7";

        base.OnLoad(e);
    }

    protected void GoLogin(string RequestLoginPage)
    {
        string LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Cps/Interface/Login.aspx";

        HttpContext.Current.Response.Write("<script language=\"javascript\">try {window.top.frames[0].location=\"" + LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "\";} catch(ex) {window.location=\"" + LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "\";}</script>");
    }
}
