using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Web.UI;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class AdminPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isAtFramePageLogin = true;              // 是否框架内的页面转跳登录。

    public string RequestCompetences = "";              // 需要的权限列表，用 [Administrator][Competence][...]... 表示

    public DateTime StartTime;
    public string PageUrl;

    public AdminPageBase()
    {
        StartTime = DateTime.Now;
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

        _User = Users.GetCurrentUser(_Site.ID);

        if (isRequestLogin && (_User == null))
        {
            PF.GoLogin(RequestLoginPage, isAtFramePageLogin);

            return;
        }

        #endregion

        #region 判断权限

        if (isRequestLogin && (RequestCompetences != "") && (!_User.Competences.IsOwnedCompetences(RequestCompetences)))
        {
            PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", this.GetType().FullName);

            return;
        }

        #endregion

        PageUrl = this.Request.Url.AbsoluteUri;

        base.OnLoad(e);
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
