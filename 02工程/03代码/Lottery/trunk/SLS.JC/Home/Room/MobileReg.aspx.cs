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

public partial class Home_Room_MobileReg : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //填充用户基本资料
            tbUserName.Text = _User.Name;
            tbUserMobile.Text = _User.Mobile;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

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

    protected void btnMobileValid_Click(object sender, System.EventArgs e)
    {
        string Mobile = Shove._Web.Utility.FilteSqlInfusion(Shove._Convert.ToDBC(tbUserMobile.Text.Trim()));

        if (Mobile == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入手机号码。");

            return;
        }

        if (!Shove._String.Valid.isMobile(Mobile))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入手机号码格式不正确。");

            return;
        }

        if ((Mobile == _User.Mobile) && (_User.isMobileValided))
        {
            Label3.Visible = true;
            Label3.Text = "&nbsp;&nbsp;&nbsp;&nbsp;你的手机已经通过验证了，不需要再次验证。";

            Shove._Web.JavaScript.Alert(this.Page, "输入手机号码已经被校验通过，不需要重复校验。");

            return;
        }

        if (new DAL.Tables.T_Users().GetCount("Mobile = '" + Mobile + "' and isMobileValided = 1 and [ID] <> " + _User.ID.ToString()) > 0)
        {
            Label3.Visible = true;
            Label3.Text = "&nbsp;&nbsp;&nbsp;&nbsp;此手机号码已经被其他用户验证，请重新输入一个手机号码。";

            Shove._Web.JavaScript.Alert(this.Page, "此手机号码已经被其他用户验证，请重新输入一个手机号码。");

            return;
        }

        string ValidNumber = GetValidNumber();
        ViewState["MobileValidNumber_" + _User.ID.ToString()] = Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ValidNumber), PF.DesKey);

        string Body = _Site.SiteNotificationTemplates[NotificationManners.SMS, NotificationTypes.MobileValid];

        if (Body != "")
        {
            Body = Body.Replace("[晓风彩票软件门户版]","["+_Site.Name+"客服中心]");
            Body = Body.Replace("[UserName]", _User.Name);
            Body = Body.Replace("[ValidNumber]", ValidNumber);
        }

        btnGO.Visible = (PF.SendSMS(_Site, _User.ID, tbUserMobile.Text.Trim(), Body) == 0);

        _User.Mobile = Mobile;
        _User.isMobileValided = false;

        string ReturnDescription="";
        int Result = _User.EditByID(ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(-1, "数据库读写错误", this.GetType().FullName);

            return;
        }

        tbUserMobile.Enabled = false;
        btnMobileValid.Enabled = false;
        Label3.Text = "&nbsp;&nbsp;&nbsp;&nbsp;您好，系统已经发送一串验证密码到你的手机，请将接收到的字串输入到验证密码框内，再点击确定按钮完成验证。";
        panelValid.Visible = true;
        Label3.Visible = true;
        tbValidPassword.Enabled = true;
        tbValidPassword.ReadOnly = false;
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

        _User.Mobile = tbUserMobile.Text;
        _User.isMobileValided = true;

        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            temp_user.Clone(_User);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "手机绑定成功。", "UserMobileBind.aspx");
    }

}