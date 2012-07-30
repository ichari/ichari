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

public partial class Home_Room_MessageSend : RoomPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    protected void btnSend_Click(object sender, System.EventArgs e)
    {
        tbAim.Text = "admin";

        if (tbContent.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入短消息内容。");
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

        int CountFail = 0;

        string SendResult = "";	//发送结果

        for (int i = 0; i < AimNames.Length; i++)
        {
            if (AimNames[i] == _User.Name)
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 不能发送消息给自己！";
                continue;
            }

            Users temp_user = new Users(_Site.ID);
            temp_user.Name = AimNames[i];

            string Return = "";
            int Result = temp_user.GetUserInformationByName(ref Return);
            if ((Result != 0) && (Result != -3) && Return != "")
            {
                CountFail++;
                SendResult += "用户 " + AimNames[i] + " 不存在！<br />";
                continue;
            }

            PF.SendStationSMS(_Site, _User.ID, temp_user.ID, 1, tbContent.Text.Trim());


        }

        if (CountFail > 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, SendResult);

            return;
        }

        tbAim.Text = "";
        tbContent.Text = "";
        Shove._Web.JavaScript.Alert(this.Page, "发送成功。");
    }

    private string[] GetAimNames(string str)
    {
        string[] strs = str.Split(',');
        ArrayList AimNames = new ArrayList();

        for (int i = 0; i < strs.Length; i++)
        {
            strs[i] = strs[i].Trim();
            if (strs[i] == "")
                continue;

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
}
