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

public partial class Admin_LoginLog : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BindData();

            btnClear.Visible = _User.Competences.IsOwnedCompetences(Competences.BuildCompetencesList(Competences.Log));
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.Log,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        string strCmd = "select * from V_SystemLog where SiteID = " + _Site.ID.ToString();
        int QueryTime = Shove._Convert.StrToInt(ddlTime.SelectedValue, -1);

        if (QueryTime > 0)
        {
            strCmd += " and Datediff(day, [DateTime], GetDate()) <= " + QueryTime.ToString();
        }

        if (tbUserName.Text.Trim() != "")
        {
            strCmd += " and [Name] = '" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim()) + "'";
        }

        strCmd += " order by [DateTime]";

        DataTable dt = MSSQL.Select(strCmd);

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LoginLog");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);

        btnClear.Enabled = (dt.Rows.Count > 0);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            if (e.Item.Cells[7].Text == "0")
            {
                e.Item.Cells[7].Text = "正常登录";
            }
            else if (e.Item.Cells[7].Text == "1")
            {
                e.Item.Cells[7].Text = "<font color='red'>密码错误</font>";
            }
            else if (e.Item.Cells[7].Text == "2")
            {
                e.Item.Cells[7].Text = "<font color='blue'>冻结登录</font>";
            }
            else if (e.Item.Cells[7].Text == "3")
            {
                e.Item.Cells[7].Text = "取回密码";
            }
            else if (e.Item.Cells[7].Text == "4")
            {
                e.Item.Cells[7].Text = "清除日志";
            }
            else
            {
                e.Item.Cells[7].Text = "其它";
            }
        }
    }

    protected void btnGo_Click(object sender, System.EventArgs e)
    {
        BindData();
    }

    protected void btnClear_Click(object sender, System.EventArgs e)
    {
        string strCmd = "delete from T_SystemLog where SiteID = " + _Site.ID.ToString();
        int QueryTime = Shove._Convert.StrToInt(ddlTime.SelectedValue, -1);

        if (QueryTime > 0)
        {
            strCmd += " and Datediff(day, DateTime, GetDate()) <= " + QueryTime.ToString();
        }

        if (tbUserName.Text.Trim() != "")
        {
            strCmd += " and dbo.F_GetUserIDByName(" + _Site.ID.ToString() + ", Name) = '" + Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim()) + "'";
        }

        if (MSSQL.ExecuteNonQuery(strCmd) < 0)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_LoginLog");

            return;
        }

        BindData();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }
}
