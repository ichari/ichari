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

public partial class UserReg : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
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

        string Name = TBUserName.Text.Trim();
        string Password = TBPwdOne.Text.Trim();
        string Password2 = TBPwdTwo.Text.Trim();
        string Email = TBUserMail.Text.Trim();
        string RealityName = tbRealityName.Text.Trim();
        string Mobile = TBMobile.Text.Trim();

        Users user = new Users(_Site.ID);

        user.Name = Name;
        user.Password = Password;
        user.Email = Email;
        user.RealityName = RealityName;
        user.UserType = 2;
        user.Mobile = Mobile;

        if (!string.IsNullOrEmpty(hidCity.Value))
        {
            DataTable dtCity = new DAL.Views.V_Citys().Open("ID", "City='" + hidCity.Value.Substring(0, hidCity.Value.Length - 1) + "'", "");

            if (dtCity != null && dtCity.Rows.Count == 1)
            {
                user.CityID = Shove._Convert.StrToInt(dtCity.Rows[0]["ID"].ToString(), 1);
            }
        }

        dt = new DAL.Tables.T_Users().Open("", "id=" + Shove._Web.Utility.GetRequest("CID") + " and Name='" + Shove._Web.Utility.GetRequest("CN") + "'", "");

        if ((dt != null) && (dt.Rows.Count == 1))
        {
            CommenderID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("CID"), -1);
        }

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
     
        Response.Redirect("Home/Room/UserRegSuccess.aspx");
    }
}
