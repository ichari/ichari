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

public partial class Admin_NotificationOptions : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Options));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Options,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        for (short i = 1; i <= 3; i++)
        {
            string SendStr = DAL.Functions.F_GetSiteSendNotificationTypes(_Site.ID, i);

            for (short j = 1; j <= 21; j++)
            {
                CheckBox cb = (CheckBox)this.FindControl("cb" + j.ToString() + "_" + i.ToString());

                if (cb == null)
                {
                    continue;
                }

                cb.Checked = (SendStr.IndexOf("[" + new NotificationTypes()[j] + "]") >= 0);
            }
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        for (short i = 1; i <= 3; i++)
        {
            string SendStr = "";

            for (short j = 1; j <= 21; j++)
            {
                CheckBox cb = (CheckBox)this.FindControl("cb" + j.ToString() + "_" + i.ToString());

                if (cb == null)
                {
                    continue;
                }

                if (cb.Checked)
                {
                    SendStr += ("[" + j.ToString() + "]");
                }
            }

            int ReturnValue = -1;
            string ReturnDescription = "";
            int Results = -1;
            Results = DAL.Procedures.P_SetSiteSendNotificationTypes(_Site.ID, i, SendStr, ref ReturnValue, ref ReturnDescription);


            if (Results < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_NotificationOptions");

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            Shove._Web.JavaScript.Alert(this.Page, "通知、消息的发送选项已经保存成功。");
        }
    }
}
