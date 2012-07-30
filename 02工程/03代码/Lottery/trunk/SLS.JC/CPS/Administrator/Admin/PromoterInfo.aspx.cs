using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Administrator_Admin_PromoterInfo : CpsAdminPageBase
{
    protected long CpsID = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CpsID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

            if (CpsID < 1)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

                return;
            }

            DataTable dt = new DAL.Views.V_Cps().Open("", "ID=" + CpsID.ToString() + " and ParentID=" + _User.cps.ID.ToString(), "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count == 0)
            {
                PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().BaseType.FullName);

                return;
            }

            DataRow dr = dt.Rows[0];

            spanLinkUrl.InnerHtml = Shove._Web.Utility.GetUrl() + "/Default.aspx?cpsid=" + CpsID.ToString();
            tdRealyName.InnerHtml = dr["RealityName"].ToString();
            tdUserName.InnerHtml = dr["UserName"].ToString();
            tbUrlName.Text = dr["Name"].ToString();
            tbUrl.Text = dr["Url"].ToString();
            tbContactPerson.Text = dr["ContactPerson"].ToString();
            tbPhone.Text = dr["Telephone"].ToString();
            tbMobile.Text = dr["Mobile"].ToString();
            tbQQNum.Text = dr["QQ"].ToString();
            tbEmail.Text = dr["Email"].ToString();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = Shove._Web.Utility.GetUrl() + "/CPS/Administrator/Admin/Default.aspx";
        RequestCompetences = Competences.BuildCompetencesList(Competences.CpsManage);

        base.OnInit(e);
    }

    #endregion
}
