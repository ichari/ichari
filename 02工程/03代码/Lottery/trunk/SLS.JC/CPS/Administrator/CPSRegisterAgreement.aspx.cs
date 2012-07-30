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

public partial class Cps_Administrator_CPSRegisterAgreement : AdminPageBase
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
        try
        {
            tbContent.Value = new DAL.Tables.T_Sites().Open("Opt_CPSRegisterAgreement", "", "").Rows[0][0].ToString();
            System.Threading.Thread.Sleep(500);
        }
        catch (Exception exception)
        {
            PF.GoError(ErrorNumber.Unknow, exception.Message, "Administrator_CPSRegisterAgreement");

            return;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        Shove.Database.MSSQL.ExecuteSQLScript("update T_Sites set Opt_CPSRegisterAgreement = '"+tbContent.Value.Trim()+"' where ID = "+ _Site.ID.ToString());
       
        Shove._Web.JavaScript.Alert(this.Page, "保存成功。");
    }
}
