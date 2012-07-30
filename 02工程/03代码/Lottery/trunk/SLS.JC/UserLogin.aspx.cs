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

public partial class UserLogin : SitePageBase
{
    public string LoginIframeUrl = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        HomePage.Value = Shove._Web.Utility.GetUrl();

        AjaxPro.Utility.RegisterTypeForAjax(typeof(UserLogin), this.Page);
        LoginIframeUrl = ResolveUrl("~/Home/Room/UserLoginDialog.aspx");

        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
            CheckCode.Visible = isUseCheckCode;
            CheckCodeReg.Visible = isUseCheckCode;

            new Login().SetCheckCode(_Site, ShoveCheckCode1);
            new Login().SetCheckCode(_Site, ShoveCheckCode2);

            if (_User != null)
            {
                Response.Redirect("Default.aspx");
            }

            if (Shove._Web.Cache.GetCache("IsLoginFirst") != null)
            {
                Panel1.Visible = true;
                Shove._Web.Cache.ClearCache("IsLoginFirst");
            }
        }
    }

    protected void btnLogin_Click(object sender, System.EventArgs e)
    {
        string ReturnDescription = "";
        int Result = -1;

        if (hLogin.Value == "1")
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, tbFormCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);
        }
        else
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, ref ReturnDescription);
        }
       
        object fromURL = Shove._Web.Cache.GetCache("OnGotoLoginUrl");

        if (Result < 0)
        {
            this.Panel1.Visible = true;
            hLogin.Value = "1";
            
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        if (fromURL != null)
        {
            Shove._Web.Cache.ClearCache("OnGotoLoginUrl");
            Response.Redirect(fromURL.ToString());
        }
        else  if (Shove._Web.Utility.GetRequest("Rollback") != null || Shove._Web.Utility.GetRequest("Rollback") != "")
        {
            string strReturn = Shove._Web.Utility.GetRequest("Rollback");
            strReturn = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), strReturn);
            if (strReturn == "MyIcaile.aspx")
            {
                new Login().GoToRequestLoginPage("~/Home/Room/AccountDetail.aspx");
            }
            else
            {
                new Login().GoToRequestLoginPage("~/Default.aspx");
            }
        }
        else
        {
            new Login().GoToRequestLoginPage("~/Default.aspx");
        }
    }

    /// <summary>
    /// 特殊商家专版的用户登录
    /// </summary>
    private void LoginForSpecialCpsUser()
    {
        string curCpsUrl = Shove._Web.Utility.GetUrl();
        string ReturnDescription = "";
        int Result = -1;

        string userNamePrefix = ""; //商家会员的前缀
        if (curCpsUrl == "http://caipiao.tpy100.com")//太平洋购彩
        {
            userNamePrefix = "typ_";
        }
        else if (curCpsUrl == "http://caipiao1.58.com")//58购彩
        {
            userNamePrefix = "58_";
        }

        if (this.Panel1.Visible)
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, tbFormCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);
        }
        else
        {
            Result = new Login().LoginSubmit(this.Page, _Site, tbFormUserName.Text, tbFormUserPassword.Text, ref ReturnDescription);
        }

        if (Result < 0) //加上前缀再尝登录一次
        {
            if (this.Panel1.Visible)
            {
                Result = new Login().LoginSubmit(this.Page, _Site, userNamePrefix + tbFormUserName.Text, tbFormUserPassword.Text, tbFormCheckCode.Text, ShoveCheckCode1, ref ReturnDescription);
            }
            else
            {
                Result = new Login().LoginSubmit(this.Page, _Site, userNamePrefix + tbFormUserName.Text, tbFormUserPassword.Text, ref ReturnDescription);
            }
        }
        if (Result < 0)
        {
            this.Panel1.Visible = true;

            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        //判断参数.
        object fromURL = Shove._Web.Cache.GetCache("OnGotoLoginUrl");
        if (fromURL != null)
        {
            Shove._Web.Cache.ClearCache("OnGotoLoginUrl");
            Response.Redirect(fromURL.ToString());
        }
        else if (Shove._Web.Utility.GetRequest("Rollback") != null || Shove._Web.Utility.GetRequest("Rollback") != "")
        {
            string strReturn = Shove._Web.Utility.GetRequest("Rollback");
            strReturn = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), strReturn);
            if (strReturn == "MyIcaile.aspx")
            {
                new Login().GoToRequestLoginPage("~/Home/Room/AccountDetail.aspx");
            }
            else
            {
                new Login().GoToRequestLoginPage("~/Default.aspx");
            }
        }
        else
        {
            new Login().GoToRequestLoginPage("~/Default.aspx");
        }
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        if (CheckCodeReg.Visible)
        {
            if (tbRegCheckCode.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入验证码！");

                return;
            }
        }

        long CpsID = -1;
        long CommenderID = -1;
        string Memo = "";
        FirstUrl firstUrl = new FirstUrl();
        string URL = firstUrl.Get();

        if (!URL.StartsWith("http://"))
        {
            URL = "http://" + URL;
            URL = URL.Split('?'.ToString().ToCharArray())[0];
        }

        DataTable dt = new DAL.Tables.T_Cps().Open("id, [ON], [Name]", "SiteID = " + _Site.ID.ToString() + " and( DomainName = '" + URL + "' or DomainName='" + Shove._Web.Utility.GetUrl() + "')", "");

        if (Shove._Convert.StrToLong(firstUrl.CpsID, -1) > 0) //读取第一次访问页面时保存的CPS ID
        {
            CpsID = Shove._Convert.StrToLong(firstUrl.CpsID, -1);
        }
        else if ((dt != null) && (dt.Rows.Count > 0))
        {
            CpsID = Shove._Convert.StrToLong(dt.Rows[0]["ID"].ToString(), -1);
            Memo = firstUrl.PID;//联盟商推广URL的PID
        }

        System.Threading.Thread.Sleep(500);

        string Name = tbRegUserName.Text.Trim();
        string Password = tbFormPassword.Text.Trim();
        string Password2 = tbPassword2.Text.Trim();
        string Email = tbEmail.Text.Trim();
        string RealityName = tbRealityName.Text.Trim();
        //string IDCardNumber = tbIDCardNumber.Text.Trim();

        Users user = new Users(_Site.ID);

        user.Name = Name;
        user.Password = Password;
        user.Email = Email;
        user.RealityName = RealityName;
        //user.IDCardNumber = IDCardNumber;
        user.UserType = 2;
        user.CommenderID = CommenderID;
        user.CpsID = CpsID;
        user.Memo = Memo;

        string ReturnDescription = "";
        int Result = user.Add(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("会员注册不成功：" + ReturnDescription);
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }

        Result = user.Login(ref ReturnDescription);

        if (Result < 0)
        {
            new Log("Users").Write("注册成功后登录失败：" + ReturnDescription);
            Shove._Web.JavaScript.Alert(this, ReturnDescription);

            return;
        }
        //if (this.ckbHomePage.Checked == true)
        //{
 
        //}
        Response.Redirect("Home/Room/UserRegSuccess.aspx");
    }

    /// <summary>
    /// 校验用户是否可用
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public int CheckUserName(string name)
    {
        if (!PF.CheckUserName(name))
        {
            return -1;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name = '" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt != null && dt.Rows.Count > 0)
        {
            return -2;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return -3;
        }

        return 0;
    }

    /// <summary>
    /// 校验注册信息
    /// </summary>
    /// <returns></returns>
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
    public string CheckReg(string name, string password, string password2, string email, string realityName,string inputCheckCode,string CheckCode, string IDCardNumber)
    {
        name = name.Trim();
        password = password.Trim();
        password2 = password2.Trim();
        email = email.Trim();
        realityName = realityName.Trim();
        IDCardNumber = IDCardNumber.Trim();

        if (!PF.CheckUserName(name))
        {
            return "对不起用户名中含有禁止使用的字符";
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            return "用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。";
        }

        if (password != password2)
        {
            return "两次密码输入不一致，请仔细检查。";
        }

        if (password.Length < 6 || password.Length > 16)
        {
            return "密码长度必须在 6-16 位之间。";
        }

        if (!Shove._String.Valid.isEmail(email))
        {
            return "电子邮件地址格式不正确。";
        }

        //if (IDCardNumber.Trim() == "")
        //{
        //    return "请输入您的身份证号码。";
        //}

        if (IDCardNumber.Trim() != "")
        {
            if ((!Shove._String.Valid.isIDCardNumber(IDCardNumber.Trim())) && (!Shove._String.Valid.isIDCardNumber_Hongkong(IDCardNumber.Trim()))
            && (!Shove._String.Valid.isIDCardNumber_Macau(IDCardNumber.Trim())) && (!Shove._String.Valid.isIDCardNumber_Singapore(IDCardNumber.Trim()))
            && (!Shove._String.Valid.isIDCardNumber_Taiwan(IDCardNumber.Trim())))
            {
                return "身份证号码格式不正确。";
            }
        }

        Shove.Web.UI.ShoveCheckCode sccCheckCode = new Shove.Web.UI.ShoveCheckCode();

        if (!sccCheckCode.Valid(CheckCode,inputCheckCode))
        {
            return "验证码输入错误";
        }

        return "";
    }


}
