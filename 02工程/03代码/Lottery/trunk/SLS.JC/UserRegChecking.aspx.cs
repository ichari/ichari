using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public partial class UserRegChecking : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            if (_User == null)
            {
                Response.Redirect("/Default.aspx");
            }
        }
    }
    /// <summary>
    ///  验证Email
    /// </summary>
    protected void btnEmail_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            Response.Redirect("/Default.aspx");
        }

        string email = Shove._Web.Utility.FilteSqlInfusion(tbEmail.Text.Trim());

        if (string.IsNullOrEmpty(email))
        {
            Shove._Web.JavaScript.Alert(this.Page, "邮箱地址格式不正确");
            return;
        }

        if (!Shove._String.Valid.isEmail(email))
        {
            Shove._Web.JavaScript.Alert(this, "邮箱地址格式不正确");

            return;
        }

        if (_User.isEmailValided)
        {
            Shove._Web.JavaScript.Alert(this.Page, "你的邮箱已经通过验证了，不需要再次验证。");
        }

        string ReturnDescriptin = "";

        _User.Email = email;
        _User.isEmailValided = false;
        int Result = _User.EditByID(ref ReturnDescriptin);

        if (Result < 0)
        {
            PF.GoError(-1, "数据库读写错误", this.GetType().FullName);

            return;
        }

        string key = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "," + DateTime.Now.ToString() + "," + email);

        //key进行md5加密后转成16进制后得到一个32位的密文
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "");

        key = sign.Substring(0, 16) + key + sign.Substring(16, 16);

        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/EmailReg.aspx?emailvalidkey=" + key;

        StringBuilder sb = new StringBuilder();

        sb.Append("<div style='font-weight:bold;'>尊敬的" + _Site.Name + "客户(").Append(_User.Name).Append("):</div>")
            .Append("<div>您好!</div>")
            .Append("<div>系统已收到您的邮箱激活申请，请点击链接<a href='").Append(url).Append("' target='_top'>").Append(url).Append("</a>校验您的身份。</div>")
            .Append("<div>为了您的安全，该邮件通知地址将在 24 小时后失效，谢谢合作。</div>")
            .Append("<div>此邮件由系统发出，请勿直接回复!</div>")
            .Append("<div>").Append(Shove._Web.Utility.GetUrlWithoutHttp()).Append(" 版权所有(C) 2008-2011</div>");

        if (PF.SendEmail(_Site, email, _Site.Name + "邮箱激活验证", sb.ToString()) == 0)
        {
            tbEmail.Enabled = false;
            rzTab_Content0.InnerHtml = "<br/><br/><div style=\"width:600px; margin:0 auto; padding:5px 10px 4px 10px; color:#b54800; font-size:12px; border:1px solid #ffd9ba; background:#FFFFDD;\"><img src=\"/Images/btn_regyes.gif\" />&nbsp;正确填写通行证和邮箱后，系统会发送一封邮件到您的邮箱，您可以点击邮件中的链接进行确认邮箱激活。</div>";
        }
        else
        {
            new Log("System").Write(this.GetType().FullName + "发送邮件失败");
        }
        
        // 邮件发送成功
    }

    /// <summary>
    /// 验证手机
    /// </summary>
    protected void btnMobile_Click(object sender, EventArgs e)
    {
        String Mobile = tbMobile.Text.Trim();

        if (String.IsNullOrEmpty(Mobile))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入手机号码不正确");
            return;
        }

        if(!Shove._String.Valid.isMobile(Mobile))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入手机号码不正确");
            return;
        }

        if (_User.isMobileValided)
        {
            Shove._Web.JavaScript.Alert(this.Page, "你的手机已经通过验证了，不需要再次验证。");
        }

        if (new DAL.Tables.T_Users().GetCount("Mobile = '" + Mobile + "' and isMobileValided = 1 and [ID] <> " + _User.ID.ToString()) > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "此手机号码已经被其他用户验证，请重新输入一个手机号码。");
            return;
        }
        
        string ValidNumber = GetValidNumber();
        ViewState["MobileValidNumber_" + _User.ID.ToString()] = Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ValidNumber), PF.DesKey);

        string Body = _Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.MobileValid];

        if (Body != "")
        {
            Body = Body.Replace("[晓风彩票软件门户版]", "[" + _Site.Name + "客服中心]");
            Body = Body.Replace("[UserName]", _User.Name);
            Body = Body.Replace("[ValidNumber]", ValidNumber);
        }


        pnInputMobile.Visible = (PF.SendSMS(_Site, _User.ID, Mobile, Body) == 0);

        pnMobile.Visible = false;

        _User.Mobile = Mobile;
        _User.isMobileValided = false;

        string ReturnDescription = "";
        int Result = _User.EditByID(ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(-1, "数据库读写错误", this.GetType().FullName);

            return;
        }

        Tip_Mobile.InnerHtml = "<br/><div style=\"width:600px; margin:0 auto; padding:5px 10px 4px 10px; color:#b54800; font-size:12px; border:1px solid #ffd9ba; background:#FFFFDD;\"><img src=\"/Images/btn_regyes.gif\" />&nbsp;您好，系统已经发送一串验证密码到你的手机，请将接收到的字串输入到验证密码框内，再点击确定按钮完成验证。</div>";
        pnInputMobile.Visible = true;
    }

    protected void btnGO_Click(object sender, System.EventArgs e)
    {
        string ValidNumber = "LtnyeFVjxGloveshove19791130ea8g502shove!@#$%^&*()__";

        try
        {
            ValidNumber = ViewState["MobileValidNumber_" + _User.ID.ToString()].ToString();
            ValidNumber = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), ValidNumber, PF.DesKey));
        }
        catch { }

        if (ValidNumber != tbValidPassword.Text.Trim())
        {
            Shove._Web.JavaScript.Alert(this.Page, "验证串错误。");

            return;
        }

        Users temp_user = new Users(_Site.ID);
        _User.Clone(temp_user);

        _User.Mobile = tbMobile.Text;
        _User.isMobileValided = true;

        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            temp_user.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "手机绑定成功。", "/Default.aspx");
    }

    #region Tool
    /// <summary>
    /// 手机验证相关
    /// </summary>
    private string GetValidNumber()
    {
        int MobileCheckCharset = _Site.SiteOptions["Opt_MobileCheckCharset"].ToInt(1);
        int MobileCheckStringLength = _Site.SiteOptions["Opt_MobileCheckStringLength"].ToInt(6);

        if ((MobileCheckCharset < 1) || (MobileCheckCharset > 4))
        {
            MobileCheckCharset = 1;
        }

        if ((MobileCheckStringLength < 1) || (MobileCheckStringLength > 20))
        {
            MobileCheckStringLength = 6;
        }

        string strs;
        switch (MobileCheckCharset)
        {
            case 1:
                strs = "0123456789";
                break;
            case 2:
                strs = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                break;
            case 3:
                strs = "abcdefghijklmnopqrstuvwxyz";
                break;
            default:
                strs = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                break;
        }

        System.Random rd = new Random();

        string str = "";

        for (int i = 0; i < MobileCheckStringLength; i++)
        {
            str += strs[rd.Next(strs.Length - 1)].ToString();
        }

        return str;
    }

    #endregion
}
