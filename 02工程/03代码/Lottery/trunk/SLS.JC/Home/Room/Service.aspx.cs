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

public partial class Home_Room_Service : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        labUserName.Text = _User.Name;

        if (!this.IsPostBack)
        {
            FillQuestionType();

            tbUserTelphone.Text = _User.Telephone;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void FillQuestionType()
    {
        DataTable dt = new DAL.Tables.T_QuestionTypes().Open("", "UseType = 1", "[ID]");

        if (dt == null || dt.Rows.Count == 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_Service");

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlQuestionType, dt, "Name", "ID");
    }

    protected void btnSave_Click(object sender, System.EventArgs e)
    {
        //if (tbUserTelphone.Text.Trim() == "")
        //{
        //    Shove._Web.JavaScript.Alert(this.Page, "请输入联系电话。");

        //    return;
        //}

        if (tbContent.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入问题描述。");

            return;
        }

        long NewQuestionID = 0;
        string ReturnDescription = "";

        if (DAL.Procedures.P_QuestionsAdd(_Site.ID, _User.ID, short.Parse(ddlQuestionType.SelectedValue), tbUserTelphone.Text.Trim(), tbContent.Text.Trim(), ref NewQuestionID, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Room_Service");

            return;
        }

        if (NewQuestionID < 0)
        {
            PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Room_Service");

            return;
        }

        tbContent.Text = "";
        Shove._Web.Cache.ClearCache("MemberQuestionList_1_" + _User.ID.ToString());
        Shove._Web.Cache.ClearCache("MemberQuestionList_2_" + _User.ID.ToString());

        Shove._Web.JavaScript.Alert(this.Page, "问题提交成功。");
    }
}