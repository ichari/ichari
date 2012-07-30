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

public partial class Home_Room_ResetPassword : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string key = Shove._Web.Utility.GetRequest("key").Trim();

            if (key == "" || key.Length <= 32)
            {
                pSetp1.Visible = false;
                pStep2.Visible = true;

                lbError.Text = "非法访问。";

                return;
            }

            string sign = key.Substring(0, 16) + key.Substring(key.Length - 16, 16);

            key = key.Substring(16, key.Length - 32);

            try
            {
                if (sign != BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", ""))
                {
                    pSetp1.Visible = false;
                    pStep2.Visible = true;

                    lbError.Text = "非法访问。";

                    return;
                }

                key = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), key);

                long userID = Shove._Convert.StrToLong(key.Split(',')[0], 0);
                DateTime time = Convert.ToDateTime(key.Split(',')[1]);

                if (time.AddDays(1).CompareTo(DateTime.Now) < 0)
                {
                    pSetp1.Visible = false;
                    pStep2.Visible = true;

                    lbError.Text = "该地址已过期。";

                    return;
                }

                if (userID <= 0)
                {
                    pSetp1.Visible = false;
                    pStep2.Visible = true;

                    lbError.Text = "非法访问。";

                    return;
                }

                pSetp1.Visible = true;
                pStep2.Visible = false;

                bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
                CheckCode.Visible = isUseCheckCode;

                new Login().SetCheckCode(_Site, ShoveCheckCode1);
            }
            catch
            {
                pSetp1.Visible = false;
                pStep2.Visible = true;

                lbError.Text = "非法访问。";

                return;
            }
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    #endregion

    protected void btnResetPassword_Click(object sender, System.EventArgs e)
    {
        System.Threading.Thread.Sleep(500);

        string key = Shove._Web.Utility.GetRequest("key").Trim();

        if (key == "" || key.Length <= 32)
        {
            pSetp1.Visible = false;
            pStep2.Visible = true;

            lbError.Text = "非法访问。";

            return;
        }

        string sign = key.Substring(0, 16) + key.Substring(key.Length - 16, 16);

        key = key.Substring(16, key.Length - 32);

        try
        {
            if (sign != BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", ""))
            {
                pSetp1.Visible = false;
                pStep2.Visible = true;

                lbError.Text = "非法访问。";

                return;
            }

            key = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), key);

            long userID = Shove._Convert.StrToLong(key.Split(',')[0], 0);
            DateTime time = Convert.ToDateTime(key.Split(',')[1]);

            if (time.AddDays(1).CompareTo(DateTime.Now) < 0)
            {
                pSetp1.Visible = false;
                pStep2.Visible = true;

                lbError.Text = "该地址已过期。";

                return;
            }

            if (userID <= 0)
            {
                pSetp1.Visible = false;
                pStep2.Visible = true;

                lbError.Text = "非法访问。";

                return;
            }

            string password = tbUserPassword.Text.Trim();
            string password2 = tbUserPassword2.Text.Trim();

            if (password == "")
            {
                Shove._Web.JavaScript.Alert(this, "新密码不能为空。");

                return;
            }

            if (password != password2)
            {
                Shove._Web.JavaScript.Alert(this, "两次密码输入不一致，请重新输入。");

                return;
            }

            if (password.Length < 6 || password.Length > 16)
            {
                Shove._Web.JavaScript.Alert(this, "密码长度必须为 6-16 位，请重新输入。");

                return;
            }

            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);

            if ((isUseCheckCode) && !ShoveCheckCode1.Valid(tbCheckCode.Text.Trim()))
            {
                Shove._Web.JavaScript.Alert(this, "验证码输入错误。");

                return;
            }

            Users user = new Users(_Site.ID);

            user.ID = userID;
            user.Password = password;

            string ReturnDescription = "";

            int Result = user.EditByID(ref ReturnDescription);

            if (Result < 0)
            {
                PF.GoError(-1, ReturnDescription, this.GetType().FullName);

                return;
            }

            user.Login(ref ReturnDescription);

            pSetp1.Visible = false;
            pStep2.Visible = true;

            lbError.Text = "密码修改成功。<div class='blue' style='margin-top:10px;'>您现在可以前往 <a href='/Home/Room/ViewAccount.aspx'>【用户中心】</a> <a href='/Default.aspx'>【官网首页】</a></div>";
        }
        catch
        {
            pSetp1.Visible = false;
            pStep2.Visible = true;

            lbError.Text = "非法访问。";

            return;
        }
    }
}
