using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class Cps_Administrator_CpsTry : AdminPageBase
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
        string str = "SiteID = " + _Site.ID.ToString() + " and  HandleResult = " + HandleResult.Trying.ToString();

        string Key = "Cps_Administrator_CpsTry";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Views.V_CpsTrys().Open("", str, "[DateTime]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(Key, dt, 100000);
        }

        string Condition = "1=1";

        if (tbKeyWord.Text != "" && tbKeyWord.Text != "请输入代理商ID")
        {
            Condition += " and Name='" + Shove._Web.Utility.FilteSqlInfusion(tbKeyWord.Text.Trim()) + "'";
        }

        DataTable dtData = dt.Clone();

        DataRow[] drs = dt.Select(Condition);

        dtData.Columns.Add("Count");

        for (int i = 0; i < drs.Length; i++)
        {
            dtData.Rows.Add(drs[i].ItemArray);
            dtData.Rows[i]["Count"] = i + 1;
        }

        PF.DataGridBindData(g, dtData, gPager);
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        long TryID = Shove._Convert.StrToLong(e.Item.Cells[9].Text, -1);

        switch (e.CommandName)
        {
            case "HandleTry": //处理申请
                {
                    Response.Redirect("CpsTryHandle.aspx?id=" + TryID.ToString());
                } break;
            case "NoAccept":
                {
                    long ReturnValue = -1;
                    string ReturnDescription = "";

                    int Result = DAL.Procedures.P_CpsTryHandle(_Site.ID, TryID,_User.ID, (short)-1, 0, false, false, ref ReturnValue, ref ReturnDescription);

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

                    Shove._Web.JavaScript.Alert(this.Page, "代理商申请已经拒绝。");
                } break;
            case "Deletes":
                {
                    DAL.Tables.T_CpsTrys cps = new DAL.Tables.T_CpsTrys();

                    cps.Delete("ID=" + TryID.ToString());
                } break;

        }

        Shove._Web.Cache.ClearCache("Cps_Administrator_CpsTry");
        BindData();
    }
}
