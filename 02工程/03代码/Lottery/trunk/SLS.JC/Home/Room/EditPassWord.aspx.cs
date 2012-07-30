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

public partial class Home_Room_EditPassWord : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
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
        tbName.Text = _User.Name;
        if (_User.SecurityQuestion.StartsWith("自定义问题|"))
        {
            lbQuestion.Text = _User.SecurityQuestion.Remove(0, 6);
        }
        else
        {
            lbQuestion.Text = _User.SecurityQuestion;
        }

        if (lbQuestion.Text == "")
        {
            lbQuestionInfo.Text = "设置安全保护问题";
        }
        else
        {
            lbQuestionInfo.Text = "修改安全保护问题";
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');window.location='SafeSet.aspx?FromUrl=EditPassWord.aspx';</script>");

            return;
        }

        if (tbOldPassWord.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入密码。");
            return;
        }

        if (tbNewPassWord.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入新密码。");
            return;
        }

        if (PF.EncryptPassword(tbOldPassWord.Text.Trim()) != _User.Password)
        {
            Shove._Web.JavaScript.Alert(this.Page, "密码有误，请重新输入。");
            return;
        }

        if (tbRePassWord.Text.Trim() != tbNewPassWord.Text.Trim())
        {
            Shove._Web.JavaScript.Alert(this.Page, "两次输入的密码不相同。");
            return;
        }

        if (tbMyA.Text.Trim() != _User.SecurityAnswer)
        {
            Shove._Web.JavaScript.Alert(this.Page, "安全保护问题回答错误。");

            return;
        }

        Users tu = new Users(_Site.ID);
        _User.Clone(tu);

        _User.Name = tbName.Text.Trim();
        _User.Password = tbNewPassWord.Text.Trim();

        string ReturnDescription = "";

        if (_User.EditByID(ref ReturnDescription) < 0)
        {
            tu.Clone(_User);
            new Log("Users").Write("会员修改密码失败：" + ReturnDescription);
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        string FromUrl = Shove._Web.Utility.GetRequest("FromUrl");
        if (FromUrl == "")
        {
            FromUrl = "EditPassWord.aspx";
        }
        Shove._Web.JavaScript.Alert(this.Page, "用户密码已经保存成功。", FromUrl);
    }
}