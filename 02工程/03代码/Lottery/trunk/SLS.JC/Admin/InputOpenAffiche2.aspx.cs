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

public partial class Admin_InputOpenAffiche2 : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForLottery();

            BindDataForIsuse();

            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.FillContent);

        base.OnInit(e);
    }

    #endregion

    private void BindDataForLottery()
    {
        DataTable dt = new DAL.Tables.T_Lotteries().Open("[ID], [Name]", "[ID] in (1,2,3,4,9,10,14,15,39)", "[Order]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlLottery, dt, "Name", "ID");
    }

    private void BindDataForIsuse()
    {
        if (ddlLottery.Items.Count < 1)
        {
            return;
        }

        DataTable dt = new DAL.Tables.T_Isuses().Open("[ID], [Name]", "LotteryID = " +  Shove._Web.Utility.FilteSqlInfusion(ddlLottery.SelectedValue) + " and EndTime < GetDate() and isnull(WinLotteryNumber,'')<>''", "EndTime");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        Shove.ControlExt.FillDropDownList(ddlIsuse, dt, "Name", "ID");

        if (ddlIsuse.Items.Count > 0)
        {
            btnOK.Enabled = true;
            tbOpenAffiche.Visible = false;
        }
        else
        {
            btnOK.Enabled = false;
            tbOpenAffiche.Visible = true;
        }

        BindData();
    }

    private void BindData()
    {
        if (ddlIsuse.Items.Count < 1)
        {
            return;
        }

        object oValue = MSSQL.ExecuteScalar("select OpenAffiche from T_Isuses where [ID] = " + Shove._Web.Utility.FilteSqlInfusion(ddlIsuse.SelectedValue));

        if (oValue == null)
        {
            tbOpenAffiche.Value = new OpenAfficheTemplates()[int.Parse(ddlLottery.SelectedValue)];

            return;
        }

        string str = oValue.ToString();

        if (str == "")
        {
            tbOpenAffiche.Value = new OpenAfficheTemplates()[int.Parse(ddlLottery.SelectedValue)];
        }
        else
        {
            tbOpenAffiche.Value = str;
        }
    }

    protected void ddlLottery_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindDataForIsuse();
    }

    protected void ddlIsuse_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (ddlIsuse.Items.Count < 1)
        {
            return;
        }

        if (fileVideo.Value != "")
        {
            if (Shove._IO.File.UploadFile(this.Page, fileVideo, "../Images/Video/", ddlIsuse.SelectedValue + ".avi", true, "video") < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, "文件上传错误。");

                return;
            }
        }

        if (MSSQL.ExecuteNonQuery("update T_Isuses set OpenAffiche = @p1 where [ID] = " + ddlIsuse.SelectedValue,
            new Shove.Database.MSSQL.Parameter("p1", SqlDbType.VarChar, 0, ParameterDirection.Input, tbOpenAffiche.Value)) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

            return;
        }

        Shove._Web.JavaScript.Alert(this.Page, "数据保存成功。");
    }
}
