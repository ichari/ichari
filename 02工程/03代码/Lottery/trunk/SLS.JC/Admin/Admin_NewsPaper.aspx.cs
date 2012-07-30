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

public partial class Admin_Admin_NewsPaper : AdminPageBase
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
           BindDataForNewsPaperTypes();

            LinkButton1.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
            LinkButton2.Visible = LinkButton1.Visible;
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent,Competences.Administrator);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForNewsPaperTypes()
    {
        DataTable dt = new DAL.Tables.T_NewsPaperIsuses().Open("", "", "[ID]");
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);
            return;
        }
        Shove.ControlExt.FillDropDownList(ddlNewsTypes, dt, "Name", "ID");
    }

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("Admin_NPIsusesAdd.aspx?ID=" +ddlNewsTypes.SelectedValue , true);
    }
    
    protected void lbDelete_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlNewsTypes.SelectedValue))
        {
            int resDelete = Shove.Database.MSSQL.ExecuteNonQuery("delete T_NewsPaperIsuses where ID=" + ddlNewsTypes.SelectedValue + "");

            if (resDelete < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "删除失败！");
            }

            Shove._Web.JavaScript.Alert(this.Page, "删除成功！");

            BindDataForNewsPaperTypes();

        }
    }
}
