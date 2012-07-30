using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class Home_Room_OnlinePay_CardPassword_CardPasswordValid : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tbCardPassword.Text = Shove._Web.Utility.GetRequest("Num");
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

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string ValidNumber = "LtnyeFVjxGloveshove19791130ea8g502shove!@#$%^&*()__";

        try
        {
            ValidNumber = ViewState["CardPasswordValidNumber_" + _User.ID.ToString()].ToString();
            ValidNumber = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), Shove._Security.Encrypt.Decrypt3DES(PF.GetCallCert(), ValidNumber, PF.DesKey));
        }
        catch { }

        if (ValidNumber != tbCode.Text.Trim())
        {
            Shove._Web.JavaScript.Alert(this.Page, "验证串错误。");

            return;
        }

        string ReturnDescription = "";

        if (new CardPassword().Use(ValidNumber, _Site.ID, _User.ID, ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        else
        {
            DAL.Tables.T_CardPasswordsValid cpv = new DAL.Tables.T_CardPasswordsValid();

            cpv.UserID.Value = _User.ID;
            cpv.Mobile.Value = tbMobile.Text.Trim();
            cpv.CardPasswordsNum.Value = tbCardPassword.Text.Trim();

            cpv.Insert();
        }

        Shove._Web.JavaScript.Alert(this.Page, "卡密充值成功, 请点击“查看我的账户”查看投注卡账户余额情况。");
    }

    protected void btnValid_Click(object sender, EventArgs e)
    {
        int Freeze = 0;
        int ReturnValue = 0;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_CardPasswordTryErrorFreeze(_Site.ID, _User.ID, ref Freeze, ref ReturnValue, ref ReturnDescription);

        if ((Result < 0) || (ReturnValue < 0))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_ViewChase");

            return;
        }

        if (Freeze > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "您输入错误的卡密号码次数过多，系统已经暂时锁定您的卡密支付功能。");

            return;
        }

        string Number = Shove._Web.Utility.FilteSqlInfusion(Shove._Convert.ToDBC(tbCardPassword.Text.Trim()));

        if (String.IsNullOrEmpty(Number))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入充值卡密。");

            return;
        }

        System.Threading.Thread.Sleep(1000);

        if (!Regex.IsMatch(Number, @"^[\d]{20}$", RegexOptions.Compiled | RegexOptions.IgnoreCase))
        {
            Shove._Web.JavaScript.Alert(this.Page, "您输入的卡密号码错误!");

            return;
        }

        string Mobile = Shove._Web.Utility.FilteSqlInfusion(Shove._Convert.ToDBC(tbMobile.Text.Trim()));

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

        if (new DAL.Tables.T_CardPasswordsValid().GetCount("Mobile = '" + Mobile + "'") > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "此手机号码已经被验证，请重新输入一个手机号码。");

            return;
        }

        string ValidNumber = GetValidNumber();
        ViewState["CardPasswordValidNumber_" + _User.ID.ToString()] = Shove._Security.Encrypt.Encrypt3DES(PF.GetCallCert(), Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), ValidNumber), PF.DesKey);

        string Body = "尊敬的" + _User.Name + "的会员您好！您在<%=_Site.Name %>的卡密充值较验码是：" + ValidNumber + "，请回网页输入较验码以完成卡密充值的过程！";

        if (PF.SendSMS(_Site, _User.ID, Mobile, Body) == 0)
        {
            trInfo.Visible = true;
            trValid.Visible = true;
            tbCardPassword.ReadOnly = true;
            tbMobile.ReadOnly = true;
            btnValid.Enabled = false;
        }
    }
}
