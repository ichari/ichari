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

public partial class Home_Room_UserEmailBind : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        //lblQuestion.Text = _User.SecurityQuestion;
        labName.Text = _User.Name;
        labUserType.Text = ((_User.UserType == 1) ? "普通用户" : ((_User.UserType == 3) ? "VIP用户" : "高级用户"));
        labLevel.Text = _User.Level.ToString();

        tbRealityName.Text = _User.RealityName;
        tbEmail.Text = _User.Email;
        labIsEmailVailded.Text = (_User.isEmailValided ? "<font color='red'>已激活</font>" : "未激活");

        btnBind.Visible = !_User.isEmailValided;
        btnReBind.Visible = _User.isEmailValided;
    }

    protected void btnBind_Click(object sender, EventArgs e)
    {
        string Email = Shove._Web.Utility.FilteSqlInfusion(Shove._Convert.ToDBC(tbEmail.Text.Trim()));
        //string Answer = Shove._Web.Utility.FilteSqlInfusion(Shove._Convert.ToDBC(tbAnswer.Text.Trim()));


        if (_User.SecurityAnswer == "" || _User.SecurityQuestion == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "您尚未设置安全问题不能进行邮箱绑定。", "SafeSet.aspx?FromUrl=UserEmailBind.aspx");

            return;
        }

        //if (Answer == "")
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "问题答案输入有误。");

        //    return;
        //}

        //if (Answer != _User.SecurityAnswer)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "问题答案输入有误。");

        //    return;
        //}
        if (Email == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入 Email 地址。");
            return;
        }

        if (!Shove._String.Valid.isEmail(Email))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入 Email 格式不正确。");
            return;
        }

        if ((Email == _User.Email) && (_User.isEmailValided))
        {
            Label1.Visible = true;
            Label1.Text = "你的 Email 已经激活了，不需要再次激活。";
            return;
        }
        string ReturnDescriptin = "";
        _User.Email = Email;
        _User.isEmailValided = false;
        int Result = _User.EditByID(ref ReturnDescriptin);
        if (Result < 0)
        {
            PF.GoError(-1, "数据库读写错误", this.GetType().FullName);
            return;
        }

        string key = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), _User.ID.ToString() + "," + DateTime.Now.ToString() + "," + Email);

        //key进行md5加密后转成16进制后得到一个32位的密文
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        string sign = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", "");
        key = sign.Substring(0, 16) + key + sign.Substring(16, 16);
        string url = Shove._Web.Utility.GetUrl() + "/Home/Room/EmailReg.aspx?emailvalidkey=" + key;
        StringBuilder sb = new StringBuilder();
        sb.Append("<div style='font-weight:bold;'>尊敬的"+_Site.Name+"客户(").Append(_User.Name).Append("):</div>")
            .Append("<div>您好!</div>")
            .Append("<div>系统已收到您的邮箱激活申请，请点击链接<a href='").Append(url).Append("' target='_top'>").Append(url).Append("</a>校验您的身份。</div>")
            .Append("<div>为了您的安全，该邮件通知地址将在 24 小时后失效，谢谢合作。</div>")
            .Append("<div>此邮件由系统发出，请勿直接回复!</div>")
            .Append("<div>").Append(Shove._Web.Utility.GetUrlWithoutHttp()).Append(" 版权所有(C) 2008-2009</div>");

        if (PF.SendEmail(_Site, Email, _Site.Name + "邮箱激活验证", sb.ToString()) == 0)
        {
            tbEmail.Enabled = false;
            Label1.Text = "您好，系统已经发送一封验证邮件您的邮箱，请到您的信箱点击链接完成激活。";
        }
        else
        {
            new Log("System").Write(this.GetType().FullName + "发送邮件失败");
        }
    }
}

