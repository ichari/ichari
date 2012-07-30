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
using System.Text;
using System.Security.Cryptography;

public partial class Home_Room_EmailReg : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            System.Threading.Thread.Sleep(500);
            string key = Shove._Web.Utility.GetRequest("emailvalidkey").Trim();
            if (key == "" || key.Length <= 32)
            {
                labValided.Text = "非法访问。";
                tbOk.Visible = false;
                tbFailure.Visible = true;

                return;
            }

            string sign = key.Substring(0, 16) + key.Substring(key.Length - 16, 16);

            key = key.Substring(16, key.Length - 32);

            try
            {
                if (sign != BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(key))).Replace("-", ""))
                {
                    labValided.Text = "非法访问。1";
                    tbOk.Visible = false;
                    tbFailure.Visible = true;

                    return;
                }

                key = Shove._Security.Encrypt.UnEncryptString(PF.GetCallCert(), key);

                long userID = -1;
                DateTime time = DateTime.Now.AddYears(-1);
                string email = "";
                try
                {
                    userID = Shove._Convert.StrToLong(key.Split(',')[0], 0);
                    time = Convert.ToDateTime(key.Split(',')[1]);
                    email = key.Split(',')[2];
                }
                catch { }

                if (time.AddDays(1).CompareTo(DateTime.Now) < 0)
                {
                    labValided.Text = "该地址已过期。";
                    tbOk.Visible = false;
                    tbFailure.Visible = true;

                    return;
                }

                if (userID <= 0)
                {
                    labValided.Text = "非法访问。2";
                    tbOk.Visible = false;
                    tbFailure.Visible = true;

                    return;
                }

                string ReturnDescription = "";

                Users user = new Users(1);
                user.ID = userID;
                user.Login(ref ReturnDescription);

                if (email != user.Email)
                {
                    labValided.Text = "您的邮箱地址不符，请到大厅，我的资料中重新发起激活。<br/><div class='blue12' style='color:black'>前往 <a href=\"Buy.aspx\">购买彩票</a>&nbsp;&nbsp; <a href=\"AccountDetail.aspx\">用户中心</a></div>";
                    tbOk.Visible = false;
                    tbFailure.Visible = true;

                    return;
                }

                if (user.isEmailValided)
                {
                    labValided.Text = "您的邮箱已激活，不需要再次激活。<br/><div class='blue12' style='color:black'>前往 <a href=\"../../JCZC/buy_spf.aspx\">购买彩票</a>&nbsp;&nbsp; <a href=\"AccountDetail.aspx\">我的账户</a></div>";
                    tbOk.Visible = false;
                    tbFailure.Visible = true;

                    return;
                }

                user.isEmailValided = true;

                int Result = user.EditByID(ref ReturnDescription);

                if (Result < 0)
                {
                    PF.GoError(-1, ReturnDescription, this.GetType().FullName);

                    return;
                }
                tbOk.Visible = true;
                tbFailure.Visible = false;
                labValided.Text = "邮箱激活成功。<br/><div class='blue12' color:black>前往 <a href=\"../../JCZC/buy_spf.aspx\">购买彩票</a> &nbsp; &nbsp; <a href=\"AccountDetail.aspx\">我的账户</a></div>";
            }
            catch
            {
                labValided.Text = "非法访问。3";
                tbOk.Visible = false;
                tbFailure.Visible = true;
                return;
            }
        }
    }


    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;
        this.isRequestLogin = false;
        base.OnInit(e);
    }

    #endregion
}