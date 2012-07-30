using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class CPS_Admin_AdvermentLink : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            spanLinkUrl.InnerHtml = Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid="+_User.cps.ID.ToString();
            tdRealyName.InnerHtml = _User.RealityName;
            tdUserName.InnerHtml = _User.Name;
            tbUrlName.Text = _User.cps.Name;
            tbUrl.Text = _User.cps.Url;
            tbMD5Key.Text = _User.cps.MD5Key;
            tbContactPerson.Text = _User.cps.ContactPerson;
            tbPhone.Text = _User.cps.Telephone;
            tbMobile.Text = _User.cps.Mobile;
            tbQQNum.Text = _User.cps.QQ;
            tbEmail.Text = _User.cps.Email;

            if (_User.cps.Type == 2)
            {
                tdMD51.Visible = true;
                tdMD52.Visible = true;
                hidType.Value = "2";
                tdType.InnerHtml = "代理商";
            }
            else
            {
                tdUrl.ColSpan = 3;
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=AdvermentLink.aspx";

        base.OnInit(e);
    }

    #endregion

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string email = tbEmail.Text.Trim();

        if (!Shove._String.Valid.isEmail(email))
        {
            Shove._Web.JavaScript.Alert(this.Page, "Email填写错误");

            return;
        }

        string mobile = tbMobile.Text.Trim();

        if (!Shove._String.Valid.isMobile(mobile))
        {
            Shove._Web.JavaScript.Alert(this.Page, "手机号码填写错误");

            return;
        }

        string url = tbUrl.Text;

        if (!url.StartsWith("http://"))
        {
            url = "http://" + url;
        }

        Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(url);

        if (!m.Success)
        {
            Shove._Web.JavaScript.Alert(this.Page, "网址填写错误");

            return;
        }

        _User.cps.Name = tbUrlName.Text;
        _User.cps.Url = url;
        _User.cps.MD5Key = tbMD5Key.Text;
        _User.cps.ContactPerson = tbContactPerson.Text;
        _User.cps.Telephone = tbPhone.Text;
        _User.cps.Mobile = tbMobile.Text;
        _User.cps.QQ = tbQQNum.Text;
        _User.cps.Email = tbEmail.Text;

        string ReturnDescription = "";

        if (_User.cps.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "基本信息修改成功!");
    }
}
