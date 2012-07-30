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

public partial class Admin_SendEmail : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SendMessage);

        base.OnInit(e);
    }

    #endregion

    protected void btnSend_Click(object sender, System.EventArgs e)
    {
        if (tbSubject.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入邮件标题。");

            return;
        }

        if (tbContent.Value.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入邮件内容。");

            return;
        }

        if (cbSystemMessage.Checked)
        {
            SendSystemMessage();

            return;
        }

        if (tbAim.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入接收方用户名。");

            return;
        }

        string[] AimNames = GetAimNames(tbAim.Text.Trim());

        if (AimNames.Length < 1)
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入正确的接收方用户名。");

            return;
        }

        int CountOK = 0, CountFail = 0;	//发送计数统计
        string SendResult = "";	//发送结果

        for (int i = 0; i < AimNames.Length; i++)
        {
            Users tu = new Users(_Site.ID)[_Site.ID, AimNames[i]];

            if (tu == null)
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 不存在！<br />";

                continue;
            }

            if (tu.Email.Trim() == "")
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 没有输入邮件地址！<br />";

                continue;
            }

            if (PF.SendEmail(_Site, tu.Email, tbSubject.Text.Trim(), tbContent.Value.Trim()) < 0)
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 发送错误！<br />";

                continue;
            }

            CountOK++;
            SendResult += "用户 " + AimNames[i] + " 发送成功。<br />";
        }

        labSendResult.Text = "发送结果：成功 " + CountOK.ToString() + " 个，失败 " + CountFail.ToString() + " 个。<br />" + SendResult;

        if (CountFail == 0)
        {
            tbAim.Text = "";
            tbContent.Value = "";
            cbSystemMessage.Checked = false;
        }
    }

    private string[] GetAimNames(string str)
    {
        string[] strs = str.Split(',');
        ArrayList AimNames = new ArrayList();

        for (int i = 0; i < strs.Length; i++)
        {
            strs[i] = strs[i].Trim();

            if (strs[i] == "")
            {
                continue;
            }

            bool isExist = false;

            for (int j = 0; j < AimNames.Count; j++)
            {
                if (AimNames[j].ToString() == strs[i])
                {
                    isExist = true;

                    break;
                }
            }

            if (!isExist)
            {
                AimNames.Add(strs[i]);
            }
        }

        while (AimNames.Count > 10)
        {
            AimNames.RemoveAt(AimNames.Count - 1);
        }

        string[] Result = new string[AimNames.Count];

        for (int i = 0; i < AimNames.Count; i++)
        {
            Result[i] = AimNames[i].ToString();
        }

        return Result;
    }

    private void SendSystemMessage()
    {
        DataTable dt = new DAL.Tables.T_Users().Open("", "SiteID = " + _Site.ID.ToString() + " and Email <> ''", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_SendEmail");

            return;
        }

        int CountOK = 0, CountFail = 0;	//发送计数统计
        string SendResult = "";	//发送结果

        foreach (DataRow dr in dt.Rows)
        {
            if (PF.SendEmail(_Site, dr["Email"].ToString(), tbSubject.Text.Trim(), tbContent.Value.Trim()) < 0)
            {
                CountFail++;
                SendResult += "用户 " + dr["Name"].ToString() + " 发送错误！<br />";
            }
            else
            {
                CountOK++;
                SendResult += "用户 " + dr["Name"].ToString() + " 发送成功。<br />";
            }
        }

        labSendResult.Text = "发送结果：成功 " + CountOK.ToString() + " 个，失败 " + CountFail.ToString() + " 个。<br />" + SendResult;

        if (CountFail == 0)
        {
            tbAim.Text = "";
            tbContent.Value = "";
            cbSystemMessage.Checked = false;
        }
    }
}
