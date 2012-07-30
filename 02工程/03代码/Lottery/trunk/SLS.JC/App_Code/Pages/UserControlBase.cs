using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
///UserControlBase 的摘要说明
/// </summary>
public class UserControlBase : System.Web.UI.UserControl
{
    public Sites _Site;
    public Users _User;
   
    public string AlipayLoginSrcUrl = "";
    public string LoginTopSrcUrl = "";
    public string LoginBackSrcUrl = "";
    public string RegisterUrl = "";
    public string ForgetPwdUrl = "";
    public string AlipayLoginUrl = "";
    public string LoginIframeUrl = "";
    protected override void OnLoad(EventArgs e)
    {
        AlipayLoginSrcUrl = ResolveUrl("~/Home/Room/images/zfb_button2.jpg");
        LoginTopSrcUrl = ResolveUrl("~/Home/Room/images/login_top.jpg");
        LoginBackSrcUrl = ResolveUrl("~/Home/Room/images/login_back.jpg");
        RegisterUrl = ResolveUrl("~/UserReg.aspx");
        ForgetPwdUrl = ResolveUrl("~/ForgetPassword.aspx");
        AlipayLoginUrl = ResolveUrl("~/Home/Room/Login.aspx");
        LoginIframeUrl = ResolveUrl("~/Home/Room/UserLoginDialog.aspx");

        #region 获取站点
        
        _Site = new Sites()[1];

        #endregion

        #region 获取用户

        _User = Users.GetCurrentUser(_Site.ID);

        #endregion

        base.OnLoad(e);
    }
}
