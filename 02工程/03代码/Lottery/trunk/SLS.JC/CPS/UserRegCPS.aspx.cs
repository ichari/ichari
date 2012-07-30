using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class CPS_UserRegCPS : CpsPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        AjaxPro.Utility.RegisterTypeForAjax(typeof(CPS_UserRegCPS), this.Page);

        if (!IsPostBack)
        {
            bool isUseCheckCode = _Site.SiteOptions["Opt_isUseCheckCode"].ToBoolean(true);
            CheckCode.Visible = isUseCheckCode;

            new Login().SetCheckCode(_Site, ShoveCheckCode1);

            BindUsers();
            BindNews();

            hType.Value = Shove._Web.Utility.GetRequest("type");
            if (hType.Value == "2")
            {
                spanType.InnerHtml = "注册代理商";
            }

            if (_User != null)
            {
                hUserID.Value = _User.ID.ToString();
                tbUser.Visible = false;
                tbPhone.Text = _User.Mobile;
                tbEmail.Text = _User.Email;
                tbQQ.Text = _User.QQ;

                if (_User.cps.ID > 0)
                {
                    if (_User.cps.Type == 1)
                    {
                        tdCpsApply.InnerHtml = "<span  class='cheng12'>您已经是推广员</span>";
                    }
                    else
                    {
                        tdCpsApply.InnerHtml = "<span  class='cheng12'>您已经是代理商</span>";
                    }
                }
                else
                {
                    if (new DAL.Tables.T_CpsTrys().GetCount("HandleResult = 0 and UserID=" + _User.ID.ToString()) > 0)
                    {
                        tdCpsApply.InnerHtml = "您已申请过CPS推广联盟，正在审核中。";
                    }
                }
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        isRequestLogin = false;

        base.OnInit(e);
    }

    #endregion

    private void BindUsers()
    {
        string Key = "CPS_Default_BindUsers";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Users().Open("top 9 Name,Bonus", "Bonus > 0", "Bonus desc");

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, 3600);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"96%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sb.Append("<tr>")
                .Append("<td width=\"16%\" height=\"26\" align=\"center\">")
                .Append("<img src=\"images/num_" + (i + 1).ToString() + ".gif\" width=\"13\" height=\"13\" />")
                .Append("</td>")
                .Append("<td width=\"53%\" height=\"26\" class=\"hui\">")
                .Append(dt.Rows[i]["Name"].ToString())
                .Append("</td>")
                .Append("<td width=\"31%\" height=\"26\" class=\"hui\">")
                .Append(Shove._Convert.StrToDouble(dt.Rows[i]["Bonus"].ToString(), 0).ToString("N"))
                .Append("元</td>")
                .Append("</tr>");
        }

        sb.Append("</table>");

        tdUsers.InnerHtml = sb.ToString();
    }

    private void BindNews()
    {
        string Key = "CPS_Default_BindNews";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = @"select * from
                                (select top 5 ID,Title,Content,TypeName from V_News where isShow = 1  and [TypeName] = 'CPS新闻公告'
                                order by isCommend,ID desc)a
                            union
                            select * from
                                (select top 8 ID,Title,Content,TypeName from V_News where isShow = 1  and [TypeName] = 'CPS推广指南'
                                order by isCommend,ID desc)b";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 3600);
        }

        DataRow[] drs = dt.Select("TypeName='CPS推广指南'", "ID desc");

        StringBuilder sb = new StringBuilder();
        sb.Append("<table width=\"96%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">");

        string Title = "";
        bool IsClass = false;
        string color = "";

        foreach (DataRow dr in drs)
        {
            Title = dr["Title"].ToString();

            if ((Title.IndexOf("<font class=red12>") > -1 || Title.IndexOf("<font class=black12>") > -1))
            {
                if (Title.Contains("<font class=red12>"))
                {
                    color = "red12";
                }
                if (Title.Contains("<font class=black12>"))
                {
                    color = "black12";
                }

                Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                IsClass = true;
            }

            sb.AppendLine("<tr>")
                    .AppendLine("<td width=\"5%\" height=\"30\" align=\"center\"><img src=\"images/dian.jpg\" width=\"3\" height=\"3\" /></td>")
                    .Append("<td width=\"95%\" height=\"30\" align=\"left\" class=\"hui\">").Append("<a href='");

            sb.Append(dr["Content"].ToString());

            sb.Append("' target='_blank'>");

            if (IsClass)
            {
                sb.Append("<font class='" + color + "'>").Append(Shove._String.Cut(Title, 24)).Append("</font>");
            }
            else
            {
                sb.Append(Shove._String.Cut(Title, 24));
            }

            sb.AppendLine("</a></td>")
            .AppendLine("</tr>");
        }

        sb.Append("</table>");

        tdNews.InnerHtml = sb.ToString();
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
    public string CheckReginfo(string name, string password, string realyName, string siteName, string webUrl, string phone, string email, string userID)
    {
        name = name.Trim();
        password = password.Trim();
        siteName = siteName.Trim();
        webUrl = webUrl.Trim();
        phone = phone.Trim();
        email = email.Trim();

        if (Shove._Convert.StrToLong(userID, -1) < 1)
        {
            if (!PF.CheckUserName(name))
            {
                return "对不起用户名中含有禁止使用的字符";
            }

            if (Shove._String.GetLength(name) < 5 || Shove._String.GetLength(name) > 16)
            {
                return "用户名长度在 5-16 个英文字符或数字、中文 3-8 之间。";
            }

            if (password.Length < 6 || password.Length > 16)
            {
                return "密码长度必须在 6-16 位之间。";
            }

            if (realyName == "")
            {
                return "真实姓名不能为空。";
            }
        }

        if (!Shove._String.Valid.isUrl(webUrl))
        {
            return "网址不正确！";
        }

        if (string.IsNullOrEmpty(siteName))
        {
            return "请输入网站名称。";
        }

        if (!Shove._String.Valid.isMobile(phone))
        {
            return "请输入正确的手机号码。";
        }

        if (!Shove._String.Valid.isEmail(email))
        {
            return "请输入正确的电子信箱。";
        }

        return "";
    }

    protected void btnReg_Click(object sender, EventArgs e)
    {
        if (!cbAgree.Checked)
        {
            Shove._Web.JavaScript.Alert(this.Page, "必须同意注册协议！");

            return;
        }

        if (CheckCode.Visible)
        {
            if (tbCheckCode.Text.Trim() == "")
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入验证码！");

                return;
            }
            else
            {
                if (!ShoveCheckCode1.Valid(tbCheckCode.Text.Trim()))
                {
                    Shove._Web.JavaScript.Alert(this.Page, "验证码输入有误！");

                    return;
                }
            }
        }

        string url = tbWebUrl.Text;

        if (!url.StartsWith("http://"))
        {
            url = "http://" + url;
        }

        Regex regex = new Regex(@"([\w-]+\.)+[\w-]+.([^a-z])(/[\w- ./?%&=]*)?|[a-zA-Z0-9\-\.][\w-]+.([^a-z])(/[\w- ./?%&=]*)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        Match m = regex.Match(url);

        if (!m.Success && url.IndexOf("http") == 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "网址填写错误");

            return;
        }

        double scale = scale = _Site.SiteOptions["Opt_CpsBonusScale"].ToDouble(0);

        if (_User == null)  //没有登录注册
        {
            Users user = new Users(_Site.ID);

            user.Name = tbUserName.Text.Trim();
            user.Password = tbPassword.Text.Trim();
            user.Email = tbEmail.Text.Trim();
            user.RealityName = tbRealyName.Text.Trim();
            user.Mobile = tbPhone.Text.Trim();
            user.QQ = tbQQ.Text.Trim();
            user.UserType = 2;

            string ReturnDescription = "";
            long Result = user.Add(ref ReturnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this, ReturnDescription);

                return;
            }

            //登录
            Result = user.Login(ref ReturnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this, ReturnDescription);

                return;
            }

            user.cps.BonusScale = scale;
            user.cps.ON = true;
            user.cps.ResponsiblePerson = tbRealyName.Text.Trim();
            user.cps.ContactPerson = tbRealyName.Text.Trim();
            user.cps.Mobile = tbPhone.Text.Trim();
            user.cps.Email = tbEmail.Text.Trim();
            user.cps.QQ = tbQQ.Text.Trim();
            user.cps.Type = Shove._Convert.StrToShort(ddlCpsType.SelectedValue, 1);
            user.cps.OwnerUserID = user.ID;
            user.cps.Url = tbWebUrl.Text.Trim();
            user.cps.Name = tbSiteName.Text.Trim();
            user.cps.MD5Key = tbMD5.Text.Trim();
            user.cps.Content = "";

            ReturnDescription = "";
            Result = user.cps.Try(ref ReturnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this, ReturnDescription);

                return;
            }

            Shove._Web.Cache.ClearCache("Cps_Administrator_CpsTry");
            Response.Redirect("Default.aspx");
        }
        else
        {
            if (_User.cps.ID != -1)
            {

                Shove._Web.JavaScript.Alert(this.Page, "您已注册成商家！");

                return;
            }

            if (new DAL.Tables.T_CpsTrys().GetCount("HandleResult = 0 and UserID=" + _User.ID.ToString()) > 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "您已申请过CPS，正在审核中。");

                return;
            }

            _User.cps.BonusScale = scale;
            _User.cps.ON = true;
            _User.cps.ResponsiblePerson = _User.RealityName;
            _User.cps.ContactPerson = _User.RealityName;
            _User.cps.Mobile = tbPhone.Text.Trim();
            _User.cps.Email = tbEmail.Text.Trim();
            _User.cps.QQ = tbQQ.Text.Trim();
            _User.cps.Type = Shove._Convert.StrToShort(ddlCpsType.SelectedValue, 1);
            _User.cps.OwnerUserID = _User.ID;
            _User.cps.Url = tbWebUrl.Text.Trim();
            _User.cps.Name = tbSiteName.Text.Trim();
            _User.cps.MD5Key = tbMD5.Text.Trim();
            _User.cps.Content = "";

            string ReturnDescription = "";
            int Result = _User.cps.Try(ref ReturnDescription);

            if (Result < 0)
            {
                Shove._Web.JavaScript.Alert(this, ReturnDescription);

                return;
            }

            Shove._Web.Cache.ClearCache("Cps_Administrator_CpsTry");

            Shove._Web.JavaScript.Alert(this.Page, "已经提交申请成功，工作人员会尽快处理！", "Default.aspx");
        }
    }
}
