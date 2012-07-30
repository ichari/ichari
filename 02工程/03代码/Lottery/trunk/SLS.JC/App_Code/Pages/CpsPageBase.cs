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
///CpsPageBase 的摘要说明
/// </summary>
public class CpsPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isCpsRequestLogin = false;               // 是否需要 Cps 用户登录

    public bool isAllowPageCache = false;                // 是否允许缓存该页面

    public DateTime StartTime;
    public string PageUrl;

    public CpsPageBase()
    {
        StartTime = DateTime.Now;
    }

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

        if (isRequestLogin && (_User == null))
        {
            GoLogin(RequestLoginPage);

            return;
        }

        if (isCpsRequestLogin && (_User.cps.ID < 0))
        {
            _User.Logout(ref ReturnDescription);

            PF.GoError(ErrorNumber.Unknow, "您不是 CPS 用户", this.GetType().BaseType.FullName);

            return;
        }

        if (isCpsRequestLogin && !_User.cps.ON)
        {
            _User.Logout(ref ReturnDescription);

            PF.GoError(ErrorNumber.Unknow, "用户被冻结", this.GetType().BaseType.FullName);

            return;
        }

        #endregion

        HtmlMeta hm = new HtmlMeta();
        hm.HttpEquiv = "X-UA-Compatible";
        hm.Content = "IE=EmulateIE7";

        PageUrl = this.Request.Url.AbsoluteUri;

        base.OnLoad(e);
    }

    protected  void GoLogin(string RequestLoginPage)
    {
        string LoginPageFileName = Shove._Web.Utility.GetUrl() + "/Cps/" + "Login.aspx";

        HttpContext.Current.Response.Write("<script language=\"javascript\">window.top.location.href=\"" + LoginPageFileName + "?RequestLoginPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "\";</script>");
    }

    public override void Dispose()
    {
        TimeSpan ts = DateTime.Now - StartTime;

        if (ts.Seconds >= 10)
        {
            new Log("Page").Write("耗时：" + ts.Minutes.ToString("00") + "分" + ts.Seconds.ToString("00") + "秒" + ts.Milliseconds.ToString("000") + "毫秒，" + PageUrl);
        }

        base.Dispose();
    }
}
