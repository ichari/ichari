using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class Admin_QuestionAnswer : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            long id = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);
            long UserID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("UserID"), -1);

            if ((id < 0) | (UserID < 0))
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_QuestionAnswer");

                return;
            }

            tbID.Text = id.ToString();
            tbUserID.Text = UserID.ToString();
            
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberService);

        base.OnLoad(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Questions().Open("", "SiteID = " + _Site.ID.ToString() + " and [ID] = " + Shove._Web.Utility.FilteSqlInfusion(tbID.Text), "");

        if ((dt == null) || (dt.Rows.Count < 1))
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_QuestionAnswer");

            return;
        }

        tbDateTime.Text = dt.Rows[0]["DateTime"].ToString();
        tbTelphone.Text = dt.Rows[0]["Telephone"].ToString();
        labContent.Text = dt.Rows[0]["Content"].ToString();
        short AnswerStatus = Shove._Convert.StrToShort(dt.Rows[0]["AnswerStatus"].ToString(), HandleResult.Trying);

        if (AnswerStatus == HandleResult.Answered)
        {
        	tbAnswer.Value = dt.Rows[0]["Answer"].ToString();

            btnAnswer.Enabled = false;
        }
    }

    protected void btnAnswer_Click(object sender, System.EventArgs e)
    {
        if ((tbUserID.Text == "") || (tbID.Text == ""))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", "Admin_QuestionAnswer");

            return;
        }

        if (tbAnswer.Value.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入答复内容。");

            return;
        }

        int ReturnValue = -1;
        string ReturnDescription = "";
        int Results = -1;
            Results = DAL.Procedures.P_QuestionsAnswer(_Site.ID, long.Parse(tbID.Text), tbAnswer.Value, _User.ID, ref ReturnValue, ref ReturnDescription);

        if (Results < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_QuestionAnswer");

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        PF.SendStationSMS(_Site, _User.ID, long.Parse(tbUserID.Text), StationSMSTypes.UserMessage, "您有问题已经被管理员答复，请到“我的问题列表”查看，谢谢。");

        Shove._Web.JavaScript.Alert(this.Page, "回复成功！", "Questions.aspx");

        return;
        //this.Response.Redirect("Questions.aspx", true);
    }
}
