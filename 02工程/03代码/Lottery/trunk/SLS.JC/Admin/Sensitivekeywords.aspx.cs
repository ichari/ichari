using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Sensitivekeywords : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnOK.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.FillContent));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent, Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = new DAL.Tables.T_Sensitivekeywords().Open("","","");

        if (dt == null)
        {
            return;
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        tbContent.Value = dt.Rows[0]["KeyWords"].ToString();
        hidID.Value = dt.Rows[0]["ID"].ToString();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        DAL.Tables.T_Sensitivekeywords t_Sensitivekeywords = new DAL.Tables.T_Sensitivekeywords();

        t_Sensitivekeywords.KeyWords.Value = tbContent.Value.Replace("，",",");

        if (Shove._Convert.StrToShort(hidID.Value, 0) > 1)
        {
            t_Sensitivekeywords.Update("1=1");
        }
        else
        {
            t_Sensitivekeywords.Insert();
        }

        Shove._Web.JavaScript.Alert(this.Page, "保存成功。");
    }
}
