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

public partial class Home_Room_BindAlipay : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }

        //tbPassword.Attributes.Add("value", tbPassword.Text);
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
        Label1.Text = _User.Name;
        labName.Text = _User.Name;

        if (_User.isAlipayNameValided)
        {

            labAlipayAccountOld.Text = GetAlipayNam();
            lbStatus.Text = "您已经绑定";
        }
        else
        {
            labBindState.Text = "(未绑定)";
            lbStatus.Text = "您一旦绑定";
        }

        if (_User.SecurityQuestion.StartsWith("自定义问题|"))
        {
            lbQuestion.Text = _User.SecurityQuestion.Remove(0, 6);
        }
        else
        {
            lbQuestion.Text = _User.SecurityQuestion;
        }

        //if (lbQuestion.Text == "")
        //{
        //    lbQuestionInfo.Text = "设置安全保护问题";
        //}
        //else
        //{
        //    lbQuestionInfo.Text = "修改安全保护问题";
        //}
    }

    private string GetAlipayNam()
    {
        string strAlipay = "";
        string strAliValue = "**********************";
        int iLength = _User.AlipayName.IndexOf("@");
        int iLengthSum = _User.AlipayName.Length;
        int iLengthShow = iLengthSum - iLength;
        try
        {
            if (_User.AlipayName.IndexOf("@") == -1)
            {
                strAlipay = _User.AlipayName.Substring(0, 3) + "********";
            }
            else
            {

                strAlipay = strAliValue.Substring(0, iLength - 1) + _User.AlipayName.Substring(_User.AlipayName.IndexOf("@") - 1, iLengthShow + 1);
            }
        }
        catch
        {
            strAlipay = "";
        }
        return strAlipay;

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (_User == null)
        {
            return;
        }

        if (_User.RealityName == "")
        {
            Response.Write("<script type='text/javascript'>alert('请完善您的基本资料，真实姓名不能为空，谢谢！');window.location='UserEdit.aspx?FromUrl=BindAlipay.aspx';</script>");
        }

        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');window.location='SafeSet.aspx?FromUrl=BindAlipay.aspx';</script>");
        }

        //if ((!Shove._String.Valid.isEmail(tbAlipayAccountNew.Text.Trim())) && (!Shove._String.Valid.isMobile(tbAlipayAccountNew.Text.Trim())))
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "输入的支付宝账号格式不正确(要求使用有效的 Email 账号、手机号码)。");

        //    return;
        //}

        if (this.tbRealityName.Text != _User.RealityName)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请核实您的真实姓名，谢谢！");
            return;
        }
        //if (tbMyA.Text.Trim() != _User.SecurityAnswer)
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "安全保护问题回答错误。");

        //    return;
        //}

        Shove._Web.Cache.SetCache("BindAlipay_" + _User.ID, _User.ID.ToString());
        Response.Redirect("Login.aspx");

    }
}