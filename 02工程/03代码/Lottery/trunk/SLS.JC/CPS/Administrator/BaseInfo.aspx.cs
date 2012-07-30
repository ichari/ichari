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
using System.Text.RegularExpressions;

public partial class Cps_Administrator_BaseInfo : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Cps cps = new Cps();
            cps.SiteID = _Site.ID;
            cps.ID = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("ID")), -1);

            string returndescription = "";
            cps.GetCpsInformationByID(ref returndescription);

            tbUrlName.Text = cps.Name;
            tbUrl.Text = cps.Url;
            tbMD5Key.Text = cps.MD5Key;
            tbContactPerson.Text = cps.ContactPerson;
            tbPhone.Text = cps.Telephone;
            tbMobile.Text = cps.Mobile;
            tbQQNum.Text = cps.QQ;
            tbEmail.Text = cps.Email;
            tbDomainName.Text = cps.DomainName;
            linkBonusScale.NavigateUrl = "BonusScaleSetupForCps.aspx?ID=" + Shove._Web.Utility.GetRequest("ID");

            if (cps.Type == 2)
            {
                lbType.Text = "代理商";
                trMD5Key.Visible = true;
            }
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    protected void btnOK_Click(object sender, EventArgs e)
    {
        string Cpsid = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("ID")), -1).ToString();
        
        Cps cps = new Cps();
        cps.ID = Shove._Convert.StrToLong(Cpsid, -1);
        cps.SiteID = _Site.ID;
        string ReturnDescription = "";
        cps.GetCpsInformationByID(ref ReturnDescription);

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

        if (tbUrlName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入网站名称!");

            return;
        }

        if (!Shove._String.Valid.isMobile(tbMobile.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入手机号码格式不正确。");

            return;
        }

        if (!Shove._String.Valid.isEmail(tbEmail.Text))
        {
            Shove._Web.JavaScript.Alert(this.Page, "输入Email格式不正确。");

            return;
        }

        string DomainName = tbDomainName.Text;
        cps.Name = tbUrlName.Text;
        cps.Url = url;
        cps.MD5Key = tbMD5Key.Text;
        cps.ContactPerson = tbContactPerson.Text;
        cps.Telephone = tbPhone.Text;
        cps.Mobile = tbMobile.Text;
        cps.QQ = tbQQNum.Text;
        cps.Email = tbEmail.Text;
        cps.DomainName = DomainName;

        if (cps.DomainName.Trim() == "")
        {
            cps.DomainName = Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid=" + Shove._Web.Utility.GetRequest("ID");
        }

        if (cps.EditByID(ref ReturnDescription) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "基本信息修改成功!");
    }
}
