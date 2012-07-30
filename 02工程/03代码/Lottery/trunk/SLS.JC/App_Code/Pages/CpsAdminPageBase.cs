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
public class CpsAdminPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来

    public string RequestCompetences = "CpsManage";              // 需要的权限列表，用 [Administrator][Competence][...]... 表示

    public DateTime StartTime;
    public string PageUrl;

    public CpsAdminPageBase()
    {
        StartTime = DateTime.Now;
    }

    protected override void OnLoad(EventArgs e)
    {
        #region 获取站点

        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", this.GetType().FullName);

            return;
        }

        #endregion

        #region 获取用户

        Users users = Users.GetCurrentUser(1);

        if (users == null)
        {
            PF.GoLogin(RequestLoginPage, false);

            return;
        }

        if (_User == null)
        {
            _User = Session["CpsAdminPageBase_Users"] as Users;
        }

        if (_User == null)
        {
            PF.GoLogin(RequestLoginPage, false);

            return;
        }

        #endregion

        #region 判断权限

        if (!users.Competences.IsOwnedCompetences(RequestCompetences))
        {
            PF.GoError(ErrorNumber.NotEnoughCompetence, "对不起，您没有足够的权限访问此页面", this.GetType().FullName);

            return;
        }

        #endregion

        HtmlMeta hm = new HtmlMeta();
        hm.HttpEquiv = "X-UA-Compatible";
        hm.Content = "IE=EmulateIE7";

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
