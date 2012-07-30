using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

public partial class Agent_CardPassword_QueryUseCardPassword : CardPasswordPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("CardPassword_QueryUseCardPassword_" + _CardPasswordAgentUser.ID.ToString());

        if (dt == null)
        {
            string Condition = "AgentID = " + _CardPasswordAgentUser.ID.ToString() + " and State = 1";

            if (tbCardPasswordID.Text.Trim() != "")
            {
                int _AgentID = -1;
                Condition += " and ID = " + new CardPassword().GetCardPasswordID(PF.GetCallCert(), Shove._Web.Utility.FilteSqlInfusion(tbCardPasswordID.Text.Trim()), ref _AgentID).ToString();
            }

            if (tbDateTime.Text.Trim() != "")
            {
                DateTime dtFrom = DateTime.Parse("1981-01-01");

                try
                {
                    dtFrom = DateTime.Parse(tbDateTime.Text.Trim());
                }
                catch
                {
                    Shove._Web.JavaScript.Alert(this.Page, "时间格式填写有错误！");

                    return;
                }

                Condition += " and UseDateTime > '" + dtFrom.ToString() + "'";
            }

            dt = new DAL.Views.V_CardPasswordDetails().Open("ID, Money, UseDateTime, RealityName, Name", Condition, "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "CardPassword_QueryUseCardPassword");

                return;
            }

            Shove._Web.Cache.SetCache("CardPassword_QueryUseCardPassword_" + _CardPasswordAgentUser.ID.ToString(), dt);
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

    protected void g_ItemDataBound(object sender, DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = new CardPassword().GenNumber(PF.GetCallCert(), Shove._Convert.StrToInt(_CardPasswordAgentUser.ID.ToString(), 0), Shove._Convert.StrToLong(e.Item.Cells[5].Text, 0));
            e.Item.Cells[1].Text = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0).ToString("N");
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache("CardPassword_QueryUseCardPassword_" + _CardPasswordAgentUser.ID.ToString());

        BindData();
    }

    #region 导出Excel相关
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("CardPassword_QueryUseCardPassword_" + _CardPasswordAgentUser.ID.ToString());

        if (dt == null)
        {
            string Condition = "AgentID = " + _CardPasswordAgentUser.ID.ToString() + " and State = 1";

            if (tbCardPasswordID.Text.Trim() != "")
            {
                int _AgentID = -1;
                Condition += " and ID = " + new CardPassword().GetCardPasswordID(PF.GetCallCert(), Shove._Web.Utility.FilteSqlInfusion(tbCardPasswordID.Text.Trim()), ref _AgentID).ToString();
            }

            if (tbDateTime.Text.Trim() != "")
            {
                DateTime dtFrom = DateTime.Parse("1981-01-01");

                try
                {
                    dtFrom = DateTime.Parse(tbDateTime.Text.Trim());
                }
                catch
                {
                    Shove._Web.JavaScript.Alert(this.Page, "时间格式填写有错误！");

                    return;
                }

                Condition += " and UseDateTime > '" + dtFrom.ToString() + "'";
            }

            dt = new DAL.Views.V_CardPasswordDetails().Open("ID, Money, UseDateTime, RealityName", Condition, "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "CardPassword_QueryUseCardPassword");

                return;
            }

            Shove._Web.Cache.SetCache("CardPassword_QueryUseCardPassword_" + _CardPasswordAgentUser.ID.ToString(), dt);
        }

        dt.Columns.Add("Number", typeof(System.String));

        CardPassword cp = new CardPassword();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Number"] = "[" + cp.GenNumber(PF.GetCallCert(), _CardPasswordAgentUser.ID, Shove._Convert.StrToLong(dt.Rows[i]["ID"].ToString(), -1)) + "]";

            dt.AcceptChanges();
        }

        dt.Columns.Remove(dt.Columns[0]);

        string FileName = "T_CardPassword.xls";

        HttpResponse response = Page.Response;

        response.AppendHeader("Content-Disposition", "attachment;filename=" + FileName);
        Response.ContentType = "application/ms-excel";
        response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");

        foreach (DataColumn dc in dt.Columns)
        {
            response.Write(dc.ColumnName + "\t");
        }

        response.Write("\n");

        foreach (DataRow dr in dt.Rows)
        {
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                response.Write(dr[i].ToString() + "\t");
            }

            response.Write("\n");
        }

        response.End();

    }
    #endregion
}