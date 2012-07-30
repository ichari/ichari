using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Home_Room_UserQQBind : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
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
        lbName.Text = _User.Name;
        labName.Text = _User.Name;
        //tbQQ.Text = _User.QQ;

        DataTable dt = new DAL.Tables.T_Users().Open("IsQQValided", "ID=" + _User.ID.ToString(), "");

        if (dt == null || dt.Rows.Count == 0)
        {
            return;
        }

        if (Shove._Convert.StrToBool(dt.Rows[0]["IsQQValided"].ToString(), false))
        {
            labQQ.Text = _User.QQ.Length > 3 ? (_User.QQ.Substring(0, 3) + "********") : _User.QQ;
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
        if (_User == null)
        {
            return;
        }

        if (_User.RealityName == "")
        {
            Response.Write("<script type='text/javascript'>alert('请完善您的基本资料，真实姓名不能为空，谢谢！');window.location='UserEdit.aspx?FromUrl=UserQQBind.aspx';</script>");
        }

        if (this.lbQuestion.Text == "")
        {
            Response.Write("<script type='text/javascript'>alert('为了您的账户安全，请先设置安全保护问题，谢谢！');window.location='SafeSet.aspx?FromUrl=UserQQBind.aspx';</script>");
        }

        if (this.tbRealityName.Text != _User.RealityName)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请核实您的真实姓名，谢谢！");
            return;
        }
        if (tbMyA.Text.Trim() != _User.SecurityAnswer)
        {
            Shove._Web.JavaScript.Alert(this.Page, "安全保护问题回答错误。");

            return;
        }

        Shove._Web.Cache.SetCache("UserQQBind_"+_User.ID.ToString(),_User.ID.ToString());
        Response.Redirect("TencentLogin.aspx");
    }
}
