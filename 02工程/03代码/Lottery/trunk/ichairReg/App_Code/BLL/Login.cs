using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Login 的摘要说明
/// </summary>
public class Login
{
    public Login()
    {
        //
        // TODO: 在此处添加构造函数逻辑
        //
    }

    public void SetCheckCode(Sites site, Shove.Web.UI.ShoveCheckCode sccCheckCode)
    {
        switch (site.SiteOptions["Opt_CheckCodeCharset"].ToShort(0))
        {
            case 0:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.All;
                break;

            case 1:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.OnlyLetter;
                break;

            case 2:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.OnlyLetterLower;
                break;

            case 3:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.OnlyLetterUpper;
                break;

            case 4:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.OnlyNumeric;
                break;

            default:
                sccCheckCode.Charset = Shove.Web.UI.ShoveCheckCode.CharSet.All;
                break;
        }
    }

    /// <summary>
    /// 登录，用于 Ajax 传入情况
    /// </summary>
    /// <param name="page"></param>
    /// <param name="site"></param>
    /// <param name="Name"></param>
    /// <param name="Password"></param>
    /// <param name="InputCheckCode">用户输入的验证码</param>
    /// <param name="CheckCode">验证码控件中记录的加密的原始验证码，用于校验</param>
    /// <param name="ReturnDescription"></param>
    /// <returns></returns>
    public int LoginSubmit(Page page, Sites site, string Name, string Password, string InputCheckCode, string CheckCode, ref string ReturnDescription)
    {
        ReturnDescription = "";

        bool Opt_isUseCheckCode = site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

        Name = Name.Trim();
        Password = Password.Trim();

        if ((Name == "") || (Password == ""))
        {
            ReturnDescription = "用户名和密码都不能为空";

            return -1;
        }

        if (Opt_isUseCheckCode)
        {
            Shove.Web.UI.ShoveCheckCode sccCheckCode = new Shove.Web.UI.ShoveCheckCode();

            if (!sccCheckCode.Valid(InputCheckCode, CheckCode))
            {
                ReturnDescription = "验证码输入错误";

                return -2;
            }
        }

        System.Threading.Thread.Sleep(500);

        Users user = new Users(site.ID);
        user.Name = Name;
        user.Password = Password;

        return user.Login(ref ReturnDescription);
    }

    /// <summary>
    /// 登录，用于页面普通 PostBack 情况
    /// </summary>
    /// <param name="page"></param>
    /// <param name="site"></param>
    /// <param name="Name"></param>
    /// <param name="Password"></param>
    /// <param name="InputCheckCode">用户输入的验证码</param>
    /// <param name="sccCheckCode">页面上的验证码控件</param>
    /// <param name="ReturnDescription"></param>
    /// <returns></returns>
    public int LoginSubmit(Page page, Sites site, string Name, string Password, string InputCheckCode, Shove.Web.UI.ShoveCheckCode sccCheckCode, ref string ReturnDescription)
    {
        ReturnDescription = "";

        bool Opt_isUseCheckCode = site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

        Name = Name.Trim();
        Password = Password.Trim();

        if ((Name == "") || (Password == ""))
        {
            ReturnDescription = "用户名和密码都不能为空";

            return -1;
        }

        if (Opt_isUseCheckCode)
        {
            if (sccCheckCode == null)
            {
                ReturnDescription = "验证码内部错误";

                return -2;
            }
            else
            {
                if (!sccCheckCode.Valid(InputCheckCode))
                {
                    ReturnDescription = "验证码输入错误";

                    return -3;
                }
            }
        }

        System.Threading.Thread.Sleep(500);

        Users user = new Users(site.ID);
        user.Name = Name;
        user.Password = Password;

        return user.Login(ref ReturnDescription);
    }

    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="page">page</param>
    /// <param name="site">site</param>
    /// <param name="Name">用户名</param>
    /// <param name="Password">密码</param>
    /// <param name="ReturnDescription">错误描述</param>
    /// <returns></returns>
    public int LoginSubmit(Page page, Sites site, string Name, string Password,  ref string ReturnDescription)
    {
        ReturnDescription = "";

        Name = Name.Trim();
        Password = Password.Trim();

        if ((Name == "") || (Password == ""))
        {
            ReturnDescription = "用户名和密码都不能为空";

            return -1;
        }

        System.Threading.Thread.Sleep(500);

        Users user = new Users(site.ID);
        user.Name = Name;
        user.Password = Password;

        return user.Login(ref ReturnDescription);
    }

    public void GoToRequestLoginPage(string DefaultPage)
    {
        string RequestLoginPage = Shove._Web.Utility.GetRequest("RequestLoginPage");

        if (RequestLoginPage == null)
        {
            RequestLoginPage = "";
        }

        if ((RequestLoginPage == "") && (DefaultPage.Trim() == ""))
        {
            if (Shove._Web.WebConfig.GetAppSettingsBool("GotoRoomWhenLogin", false))
            {
                RequestLoginPage = "~/Default.aspx";
            }
        }

        if (RequestLoginPage != "")
        {
            if (RequestLoginPage.StartsWith("Home/Room/"))
            {
                RequestLoginPage = RequestLoginPage.Substring(5, RequestLoginPage.Length - 5);

                if (!RequestLoginPage.StartsWith("MyIcaile.aspx?SubPage="))
                {
                    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>window.top.location.href='Home/Room/MyIcaile.aspx?SubPage=" + System.Web.HttpUtility.UrlEncode(RequestLoginPage) + "'</script>");
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Write("<script language='javascript'>window.top.location.href='Home/Room/" + RequestLoginPage + "'</script>");
                }

                return;
            }

            System.Web.HttpContext.Current.Response.Redirect(RequestLoginPage, true);
        }
        else
        {
            if (DefaultPage.Trim() == "")
            {
                return;
            }

            System.Web.HttpContext.Current.Response.Redirect(DefaultPage, true);
        }
    }
}
