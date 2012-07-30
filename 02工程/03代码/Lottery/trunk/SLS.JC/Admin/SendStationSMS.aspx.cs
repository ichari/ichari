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

public partial class Admin_SendStationSMS : AdminPageBase
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

    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (tbContent.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入短消息内容。");

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
            if (AimNames[i] == _User.Name)
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 不能发送消息给自己！<br />";

                continue;
            }

            Users temp_user = new Users(_Site.ID)[_Site.ID, AimNames[i]];

            if (temp_user == null)
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 不存在！<br />";

                continue;
            }

            if (PF.SendStationSMS(_Site, _User.ID, temp_user.ID, StationSMSTypes.SystemMessage, tbContent.Text.Trim()) < 0)
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
            tbContent.Text = "";
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
        if (PF.SendStationSMS(_Site, _User.ID, -1, StationSMSTypes.SystemMessage, tbContent.Text.Trim()) < 0)
        {
            labSendResult.Text = "系统消息发送失败。";

            return;
        }

        labSendResult.Text = "系统消息发送成功。";
        tbAim.Text = "";
        tbContent.Text = "";
        cbSystemMessage.Checked = false;
    }
}
