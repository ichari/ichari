using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Security.Cryptography;
using System.Text;

public partial class ForgetPassword : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {        
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(500);

        string name = Shove._Web.Utility.FilteSqlInfusion(tbFormUserName.Text.Trim());
        string email = Shove._Web.Utility.FilteSqlInfusion(tbEmail.Text.Trim());

        if (name == "")
        {
            Shove._Web.JavaScript.Alert(this, "用户名不能为空。");

            return;
        }

        if (email == "")
        {
            Shove._Web.JavaScript.Alert(this, "邮箱地址不能为空。");

            return;
        }

        if (!Shove._String.Valid.isEmail(email))
        {
            Shove._Web.JavaScript.Alert(this, "邮箱地址格式不正确。");

            return;
        }

        if (tbRegCheckCode.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入验证码！");

            return;
        }

        string RegCode = tbRegCheckCode.Text.Trim().ToLower();

        if (Shove._Web.Cache.GetCacheAsString("CheckCode_" + Request.Cookies["ASP.NET_SessionId"].Value, "") != Shove._Security.Encrypt.MD5(PF.GetCallCert() + RegCode))
        {
            Shove._Web.JavaScript.Alert(this.Page, "验证码输入错误，请重新输入！");

            return;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("", "Name = '" + name + "' and Email = '" + email + "'", "");

        if (dt == null || dt.Rows.Count < 1)
        {
            Shove._Web.JavaScript.Alert(this, "用户名或邮箱不正确。");

            return;
        }

        if (!Shove._Convert.StrToBool(dt.Rows[0]["isEmailValided"].ToString(), false))
        {
            Shove._Web.JavaScript.Alert(this, "您的邮箱当前还没有激活，不能使用密码找回功能，请联系客服人员帮你找回密码，谢谢合作。");

            return;
        }

        string key = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), dt.Rows[0]["ID"].ToString() + "," + DateTime.Now.ToString());

        //key进行md5加密后转成16进制后得到一个32位的密文
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "");

        key = sign.Substring(0, 16) + key + sign.Substring(16, 16);

        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/ResetPassword.aspx?key=" + key;

        StringBuilder sb = new StringBuilder();

        sb.Append("<div style='font-weight:bold;'>尊敬的" + _Site.Name + "客户(").Append(name).Append("):</div>")
            .Append("<div>您好!</div>")
            .Append("<div>系统已收到您的密码找回申请，请点击链接<a href='").Append(url).Append("' target='_top'>").Append(url).Append("</a>重设您的密码。</div>")
            .Append("<div>为了您的安全，该邮件通知地址将在 24 小时后失效，谢谢合作。</div>")
            .Append("<div>此邮件由系统发出，请勿直接回复!</div>")
            .Append("<div>").Append(Shove._Web.Utility.GetUrlWithoutHttp()).Append(" 版权所有(C) 2008-2009</div>");

        int Result = PF.SendEmail(_Site, email, "密码找回通知信", sb.ToString());

        if (Result < 0)
        {
            new Log("System").Write(this.GetType().FullName + "发送邮件失败");
            return;
        }
        // 修改客户端Html提示
        top_tishi.InnerHtml = "<div class=\"top_ok\"><img src=\"/Images/btn_regyes.gif\" />&nbsp;系统已收到您的密码找回申请，为了您的安全，该邮件通知地址将在 24 小时后失效，谢谢合作。</div>";

        this.tbEmail.ReadOnly = true;
        this.tbEmail.Text = "";
        this.tbFormUserName.ReadOnly = true;
        this.tbRegCheckCode.Text = "";
        this.tbRegCheckCode.ReadOnly = true;
        Shove._Web.JavaScript.Alert(this.Page, "系统已发送邮件到您的邮箱中");
    }
}
