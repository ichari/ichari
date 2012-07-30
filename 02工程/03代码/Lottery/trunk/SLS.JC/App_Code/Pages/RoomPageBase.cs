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
using System.Text;

/// <summary>
/// PageBase 的摘要说明
/// </summary>
public class RoomPageBase : System.Web.UI.Page
{
    public Sites _Site;
    public Users _User;

    public bool isRequestLogin = true;                  // 是否需要登录
    public string RequestLoginPage = "";                // 需要登录后，转跳到 Login.aspx 页面，登录后，会按此页面自动定位回来
    public bool isAtFramePageLogin = true;              // 是否框架内的页面转跳登录。

    public bool isAllowPageCache = false;                // 是否允许缓存该页面
    public int PageCacheSeconds = 0;

    public DateTime StartTime;
    public string PageUrl;

    //弹出广告页面列表
    public static string FloatNotifyPageList = Shove._Web.WebConfig.GetAppSettingsString("FloatNotifyPageList");
    //弹出广告显示时间（单位秒）
    public static int FloatNotifyTimeOut = Shove._Web.WebConfig.GetAppSettingsInt("FloatNotifyTimeOut", 0);

    public RoomPageBase()
    {
        StartTime = DateTime.Now;
    }

    protected override void OnLoad(EventArgs e)
    {
        if (!this.IsPostBack)
        {
            new FirstUrl().Save();
        }

        PageUrl = this.Request.Url.AbsoluteUri;

        #region 获取站点

        //_Site = new Sites()[Shove._Web.Utility.GetUrlWithoutHttp()];
        _Site = new Sites()[1];

        if (_Site == null)
        {
            PF.GoError(ErrorNumber.Unknow, "域名无效，限制访问", "SitePageBase");

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

        #region 加载头部和底部
        try
        {
            PlaceHolder phHead = Page.FindControl("phHead") as PlaceHolder;

            if (phHead != null)
            {
                UserControl Head = Page.LoadControl("~/Home/Room/UserControls/WebHead.ascx") as UserControl;
                Head.ID = "WebHead1";
                phHead.Controls.Add(Head);
            }


            PlaceHolder phFoot = Page.FindControl("phFoot") as PlaceHolder;

            if (phFoot != null)
            {
                UserControl Foot = Page.LoadControl("~/Home/Room/UserControls/WebFoot.ascx") as UserControl;
                phFoot.Controls.Add(Foot);
            }
        }
        catch { }

        #endregion

        #region 弹出广告

        string ClassName = (this.Request.Url.Query == "" ? this.Request.Url.AbsoluteUri : this.Request.Url.AbsoluteUri.Replace(this.Request.Url.Query, "")).Replace(Shove._Web.Utility.GetUrl(), "").Replace("/", "_").Replace(".aspx", "");

        if (ClassName.StartsWith("_"))
        {
            ClassName = ClassName.Remove(0, 1);
        }

        if (FloatNotifyPageList.IndexOf("," + ClassName + ",") > -1)
        {
            StringBuilder sb = new StringBuilder();

            string CacheKey = "FloatNotifyContent";
            DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

            if (dt == null)
            {
                dt = new DAL.Tables.T_FloatNotify().Open("", "", "[Order] asc,[DateTime] desc");

                if (dt != null && dt.Rows.Count > 0)
                {
                    Shove._Web.Cache.SetCache(CacheKey, dt, 60000);
                }
            }

            bool IsShow = false;
            int FloatNotifiesType = _Site.SiteOptions["Opt_FloatNotifiesTime"].ToInt(3);
            string LastLoginFloatNotifyKey = "LastLoginShowFloatNotifyUserID";

            switch (FloatNotifiesType)
            {
                case 1:
                    {
                        HttpCookie cookieLastLoginFloatNotify = Request.Cookies[LastLoginFloatNotifyKey];

                        if (cookieLastLoginFloatNotify != null && !String.IsNullOrEmpty(cookieLastLoginFloatNotify.Value))
                        {
                            IsShow = true;
                        }
                    } break;
                case 2:
                    {
                        IsShow = (DateTime.Now.Minute == 0);
                    } break;
                case 3:
                    {
                        IsShow = true;
                    } break;
            }

            if (dt != null && dt.Rows.Count > 0 && IsShow)
            {
                DataRow[] drs = dt.Select("isShow=1");

                for (int i = 0; i < drs.Length; i++)
                {
                    if (i == 2)
                    {
                        break;
                    }

                    sb.Append("<font color='" + drs[i]["Color"].ToString() + "'>").Append((i + 1).ToString()).Append(".</font>").Append("<a href='").Append(drs[i]["Url"].ToString()).Append("' target='_blank' style='text-decoration: none;color:").Append(drs[i]["Color"].ToString()).Append(";' onmouseover=\"this.style.color='#ff6600';\" onmouseout=\"this.style.color='").Append(drs[i]["Color"].ToString()).Append("';\">").Append(drs[i]["Title"].ToString()).Append("</a><br />");
                }

                string FloatNotify = HmtlManage.GetHtml(AppDomain.CurrentDomain.BaseDirectory + "Home/Web/Template/FloatNotify.html");

                Label label = new Label();

                label.Text = FloatNotify.Replace("$FloatNotifyTimeOut$", (FloatNotifyTimeOut * 100).ToString()).Replace("$FloatNotifyContent$", sb.ToString());

                try
                {
                    this.Form.Controls.AddAt(0, label);
                }
                catch (Exception ex)
                {
                    new Log("Page").Write(ClassName + ex.Message);
                }

                // 从 Cookie 中移除 浮出广告的UserID
                HttpCookie cookieLastLoginFloatNotify = new HttpCookie(LastLoginFloatNotifyKey);
                cookieLastLoginFloatNotify.Value = "";
                cookieLastLoginFloatNotify.Expires = DateTime.Now.AddYears(-20);

                try
                {
                    HttpContext.Current.Response.Cookies.Add(cookieLastLoginFloatNotify);
                }
                catch { }
            }
        }

        #endregion

        #region 缓存

        if (isAllowPageCache)
        {
            if (PageCacheSeconds > 0)
            {
                this.Response.Cache.SetExpires(DateTime.Now.AddSeconds(PageCacheSeconds));
                this.Response.Cache.SetCacheability(HttpCacheability.Server);
                this.Response.Cache.VaryByParams["*"] = true;
                this.Response.Cache.SetValidUntilExpires(true);
                this.Response.Cache.SetVaryByCustom("SitePage");
            }
        }

        #endregion

        //HtmlMeta hm = new HtmlMeta();
        //hm.HttpEquiv = "X-UA-Compatible";
        //hm.Content = "IE=EmulateIE7";

        //Page.Header.Controls.Add(hm);

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
