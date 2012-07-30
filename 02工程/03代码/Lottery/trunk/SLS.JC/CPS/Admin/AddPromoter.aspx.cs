using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class CPS_Admin_AddPromoter : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isCpsRequestLogin = true;
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Admin/Default.aspx?SubPage=AddPromoter.aspx";

        base.OnInit(e);
    }

    #endregion

    protected bool CheckInput()
    {
        string name = tbUserName.Text;

        if (!PF.CheckUserName(name))
        {
            Shove._Web.JavaScript.Alert(this.Page, "对不起用户名中含有禁止使用的字符");

            return false;
        }

        if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。");

            return false;
        }

        if (tbPassword.Text != tbPwd.Text)
        {
            Shove._Web.JavaScript.Alert(this.Page, "两次密码输入不一致，请仔细检查。");

            return false;
        }

        if (tbPassword.Text.Length < 6 || tbPassword.Text.Length > 16)
        {
            Shove._Web.JavaScript.Alert(this.Page, "密码长度必须在 6-16 位之间。");

            return false;
        }

        DataTable dt = new DAL.Tables.T_Users().Open("ID", "Name='" + Shove._Web.Utility.FilteSqlInfusion(name) + "'", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return false;
        }

        if (dt.Rows.Count > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户名已存在！");

            return false;
        }

        return true;
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (!CheckInput())
        {
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

        if(!Shove._String.Valid.isMobile(tbMobile.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "手机号码填写错误");

            return;
        }

        if (!Shove._String.Valid.isEmail(tbEmail.Text.Trim()))
        {
            Shove._Web.JavaScript.Alert(this.Page, "Email填写错误");

            return;
        }

        string urlName = tbUrlName.Text;
        string contactPerson = tbContactPerson.Text;
        string telephone = tbPhone.Text;
        string mobile = tbMobile.Text;
        string qq = tbQQNum.Text;
        string email = tbEmail.Text;
        long parentID = _User.cps.ID;

        string ReturnDescription = "";

        Users user = new Users(_Site.ID);

        user.Name = tbUserName.Text;
        user.Password = tbPassword.Text;
        user.RealityName = contactPerson;
        user.Email = email;
        user.QQ = qq;
        user.Telephone = telephone;
        user.Mobile = mobile;
        user.CpsID = parentID;
        user.UserType = 2;

        if (user.Add(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }
        else
        {
            user.cps.DomainName = "";
            user.cps.Url = url;
            user.cps.Type = 1;
            user.cps.Name = urlName;
            user.cps.ContactPerson = contactPerson;
            user.cps.Telephone = telephone;
            user.cps.Mobile = mobile;
            user.cps.QQ = qq;
            user.cps.Email = email;
            user.cps.ParentID = parentID;
            user.cps.BonusScale = 0.02;
            user.cps.ON = true;

            long newCpsID = user.cps.Add(ref ReturnDescription);
            if (newCpsID < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            //设置推广链接
            user.cps.DomainName = Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid=" + newCpsID.ToString();
            
            if (user.cps.EditByID(ref ReturnDescription) < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }
        }

        Shove._Web.Cache.ClearCache("CPS_Admin_PromoterList" + _User.cps.ID.ToString());

        Response.Redirect("PromoterList.aspx");
    }
}
