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

public partial class Admin_CardPasswordManage :AdminPageBase
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
        isRequestLogin = true;
        RequestLoginPage = "Admin/CardPasswordManage.aspx";

        RequestCompetences = Competences.BuildCompetencesList(Competences.Finance);

        base.OnInit(e);
    }

    #endregion

    private void BindData()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("CardPassword_QueryCardPassword_All");

        if (dt == null)
        {
            string Condition = "";

            if (rbExp.Checked)
            {
                Condition += "state = -1";
            }
            else if (rbUse.Checked)
            {
                Condition += "state = 1";
            }
            else if (rbNoUse.Checked)
            {
                Condition += "state = 0";
            }

            if (tbCardPasswordID.Text.Trim() != "")
            {
                int _AgentID = -1;
                Condition += " and ID = " + new CardPassword().GetCardPasswordID(PF.GetCallCert(),  Shove._Web.Utility.FilteSqlInfusion(tbCardPasswordID.Text.Trim()), ref _AgentID).ToString();
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

                Condition += " and DateTime > '" + dtFrom.ToString() + "'";
            }

            dt = new DAL.Views.V_CardPasswordDetails().Open("ID, Money, Period, State, AgentID, UseDateTime, RealityName", Condition, "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "CardPassword_QueryCardPassword");

                return;
            }

            Shove._Web.Cache.SetCache("CardPassword_QueryCardPassword_All", dt, 1200);
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
            e.Item.Cells[0].Text = new CardPassword().GenNumber(PF.GetCallCert(), Shove._Convert.StrToInt(e.Item.Cells[7].Text, 0),Shove._Convert.StrToLong(e.Item.Cells[6].Text, 0));
            e.Item.Cells[1].Text = Shove._Convert.StrToDouble(e.Item.Cells[1].Text, 0).ToString("N");

            string state = e.Item.Cells[2].Text;

            if (state == "-1")
            {
                e.Item.Cells[2].Text = "<font color='blue'>已过期</font>";
            }
            else if (state == "0")
            {
                e.Item.Cells[2].Text = "未使用";
            }
            else if (state == "1")
            {
                e.Item.Cells[2].Text = "<font color='red'>已使用</font>";
            }
        }
    }

    protected void btnQuery_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache("CardPassword_QueryCardPassword_All");

        BindData();
    }


    #region 导出Excel相关
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.ClearContent();
        Response.Charset = "GB2312";    //设置输出流的http字符集
        Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");//设置输出流为简体中文
        Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

        Response.ContentType = "application/excel";

        StringWriter sw = new StringWriter();

        HtmlTextWriter htw = new HtmlTextWriter(sw);

        g.RenderControl(htw);

        Response.Write(sw.ToString());

        Response.End();

    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        //不能少
    }
    #endregion
}
