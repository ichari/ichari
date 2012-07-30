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

public partial class Admin_Questions : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindDataForType();

            BindData();

            g.Columns[5].Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.MemberService));
        }
    }

    #region Web 窗体设计器生成的代码

    protected override void OnLoad(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberService,Competences.QueryData);

        base.OnLoad(e);
    }

    #endregion

    private void BindDataForType()
    {
        DataTable dt = new DAL.Tables.T_QuestionTypes().Open("", "UseType = 1", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Questions");

            return;
        }

        ddlType.Items.Clear();
        ddlType.Items.Add(new ListItem("--选择问题类型--", "-1"));

        foreach (DataRow dr in dt.Rows)
        {
            ddlType.Items.Add(new ListItem(dr["Name"].ToString(), dr["ID"].ToString()));
        }
    }

    private void BindData()
    {
        string CmdStr = "select * from V_Questions";
        bool hasWhere = false;

        if (rb2.Checked)
        {
            CmdStr += " where AnswerStatus = " + HandleResult.Answered.ToString();
            hasWhere = true;
        }

        if (rb3.Checked)
        {
            CmdStr += " where AnswerStatus = " + HandleResult.Trying.ToString();
            hasWhere = true;
        }

        if (ddlType.SelectedIndex > 0)
        {
            if (!hasWhere)
            {
                CmdStr += " where ";
            }
            else
            {
                CmdStr += " and ";
            }

            CmdStr += " TypeID = " + ddlType.SelectedValue;
        }

        CmdStr += " order by UserID, [DateTime]";

        DataTable dt = MSSQL.Select(CmdStr);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Questions");

            return;
        }
        
        PF.DataGridBindData(g, dt, gPager);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            CheckBox cb = (CheckBox)e.Item.Cells[4].FindControl("cbAnswered");
            cb.Checked = (Shove._Convert.StrToShort(e.Item.Cells[8].Text, HandleResult.NoAcception) == HandleResult.Answered);
        }
    }

    protected void g_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
    {
        if (e.CommandName == "Answer")
        {
            this.Response.Redirect("QuestionAnswer.aspx?ID=" + e.Item.Cells[6].Text + "&UserID=" + e.Item.Cells[7].Text, true);

            return;
        }

        if (e.CommandName == "Del")
        {
            int ReturnValue = -1;
            string ReturnDescription = "";

            int Results = -1;
            Results = DAL.Procedures.P_QuestionsDelete(_Site.ID, long.Parse(e.Item.Cells[6].Text), ref ReturnValue, ref ReturnDescription);
            if (Results < 0)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Questions");

                return;
            }

            if (ReturnValue < 0)
            {
                PF.GoError(ErrorNumber.Unknow, ReturnDescription, "Admin_Questions");

                return;
            }

            BindData();

            return;
        }
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void rb1_CheckedChanged(object sender, System.EventArgs e)
    {
        BindData();
    }
}
