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

using Shove.Database;

public partial class Admin_ReceiveSMS : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // ReceiveMessage(); // 请不要打开，否则会将投注的短信接收到 T_SMS 表，而是短信投注的表 T_SmsBettings 收不到短信

        if (!this.IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.SendMessage);

        base.OnInit(e);
    }

    #endregion

    private void ReceiveMessage()
    {
        // 以下代码请不要打开，否则会将投注的短信接收到 T_SMS 表，而是短信投注的表 T_SmsBettings 收不到短信

        #region 接收短信的代码

        //string SMS_UserID = _Site.SiteOptions["Opt_ISP_UserID"].Value.ToString();
        //string SMS_UserPassword = _Site.SiteOptions["Opt_ISP_UserPassword"].Value.ToString();

        //if ((SMS_UserID == "") || (SMS_UserPassword == ""))
        //{
        //    labTip.Text = "接收短信失败，可能是参数设置错误。";

        //    return;
        //}

        //SMS.Eucp.Gateway.Gateway segg = new SMS.Eucp.Gateway.Gateway(SMS_UserID, SMS_UserPassword);
        //SMS.Eucp.Gateway.CallResult Result = segg.ReceiveSMS();

        //if (Result.Code < 0)
        //{
        //    labTip.Text = Result.Description;

        //    return;
        //}

        //if (segg.rsc.Count < 1)
        //{
        //    return;
        //}

        //labTip.Text = "接收到 " + segg.rsc.Count.ToString() + " 条新短信。";

        //// 写入数据库
        //long NewSMSID = 0;

        //for (int i = 0; i < segg.rsc.Count; i++)
        //{
        //    DAL.Procedures.P_WriteSMS(_Site.ID, 0, segg.rsc[i].FromMobile, "", Shove._Convert.ToDBC(segg.rsc[i].Content), ref NewSMSID);
        //}

        #endregion
    }

    private void BindData()
    {
        int days = int.Parse(ddlDateTime.SelectedValue);
        string CmdStr;

        if (days == -1)
        {
            CmdStr = "select * from T_SMS where SiteID = " + _Site.ID.ToString() + " and SMSID <> -1 order by [DateTime] desc";
        }
        else
        {
            CmdStr = "select * from T_SMS where SiteID = " + _Site.ID.ToString() + " and Datediff(day, [DateTime], GetDate()) <= " + days.ToString() + " and SMSID <> -1 order by [DateTime] desc";
        }

        DataTable dt = MSSQL.Select(CmdStr);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Questions");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void ddlDateTime_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }
}
