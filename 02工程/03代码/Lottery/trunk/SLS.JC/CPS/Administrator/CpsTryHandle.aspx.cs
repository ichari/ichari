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

public partial class Cps_Administrator_CpsTryHandle : AdminPageBase
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
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/Cps/Administrator/Default.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        long TryID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (TryID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        DataTable dt = new DAL.Views.V_CpsTrys().Open("", "SiteID = " + _Site.ID.ToString() + " and [ID] = " + TryID.ToString() + " and HandleResult = " + HandleResult.Trying.ToString(), "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (dt.Rows.Count < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "代理商申请不存在", this.GetType().FullName);

            return;
        }

        tbID.Text = TryID.ToString();

        tbName.Text = dt.Rows[0]["Name"].ToString();
        tbContactPerson.Text = dt.Rows[0]["ContactPerson"].ToString();
        tbTelephone.Text = dt.Rows[0]["Telephone"].ToString();
        tbUrl.Text = dt.Rows[0]["Url"].ToString();
        tbUrlName.Text = dt.Rows[0]["CpsTrysName"].ToString();
        tbMobile.Text = dt.Rows[0]["Mobile"].ToString();
        tbEmail.Text = dt.Rows[0]["Email"].ToString();
        tbQQ.Text = dt.Rows[0]["QQ"].ToString();
        Shove.ControlExt.SetDownListBoxTextFromValue(ddlCpsType, dt.Rows[0]["Type"].ToString());
        tbMD5Key.Text = dt.Rows[0]["MD5Key"].ToString();
        tbBonusScale.Text = dt.Rows[0]["BonusScale"].ToString();
        Shove.ControlExt.SetDownListBoxTextFromValue(ddlCom, dt.Rows[0]["IsIntimeBonus"].ToString());
    }

    protected void btnAccept_Click(object sender, EventArgs e)
    {
        long TryID = Shove._Convert.StrToLong(tbID.Text, -1);

        if (TryID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        double BonusScale = Shove._Convert.StrToDouble(tbBonusScale.Text, -1);

        if ((BonusScale < 0) || (BonusScale >= 1))
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入佣金比例。");

            return;
        }

        long NewCpsID = 0;
        string ReturnDescription = "";

        if (DAL.Procedures.P_CpsTryHandle(_Site.ID, TryID, _User.ID, (short)1, BonusScale, Shove._Convert.StrToBool(ddlCom.SelectedValue, false), cbON.Checked, ref NewCpsID, ref ReturnDescription) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (NewCpsID < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        string domainName = Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid=" + NewCpsID.ToString();

        //保存修改的申请的推广域名等资料
        int ReturnValue = -1;

        int Result = DAL.Procedures.P_CpsEdit(_Site.ID, NewCpsID, tbUrlName.Text, tbUrl.Text, "", BonusScale, cbON.Checked, "", "", "", "", tbContactPerson.Text.Trim(), Shove._Convert.ToDBC(tbTelephone.Text.Trim()).Trim(), "", Shove._Convert.ToDBC(tbMobile.Text.Trim()).Trim(),
            tbEmail.Text.Trim(), Shove._Convert.ToDBC(tbQQ.Text.Trim()).Trim(), "", tbMD5Key.Text, Shove._Convert.StrToShort(ddlCpsType.SelectedValue, 2), domainName, true, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {

            Shove._Web.JavaScript.Alert(this.Page, "代理商已经通过审核并生效。但保存推广域名等资料失败!");

            return;
        }
        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache("Cps_Administrator_CpsTry");
        Shove._Web.Cache.ClearCache("CPS_Administrator_CpsAgentList");
        Shove._Web.JavaScript.Alert(this.Page, "代理商已经通过审核并生效。", "CpsTry.aspx");
    }

    protected void btnNoAccept_Click(object sender, EventArgs e)
    {
        long TryID = Shove._Convert.StrToLong(tbID.Text, -1);

        if (TryID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        long ReturnValue = -1;
        string ReturnDescription = "";

        int Result = DAL.Procedures.P_CpsTryHandle(_Site.ID, TryID, _User.ID, (short)-1, 0, Shove._Convert.StrToBool(ddlCom.SelectedValue, false), false, ref ReturnValue, ref ReturnDescription);

        if (Result < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        if (ReturnValue < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

            return;
        }

        Shove._Web.Cache.ClearCache("Cps_Administrator_CpsTry");
        Shove._Web.JavaScript.Alert(this.Page, "代理商申请已经拒绝。", "Cps.aspx");
    }
}
