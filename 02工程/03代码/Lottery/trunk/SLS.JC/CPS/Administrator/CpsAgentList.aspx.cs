using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class CPS_Administrator_CpsAgentList : AdminPageBase
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
        string Key = "CPS_Administrator_CpsAgentList";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            int ReturnValue = -1;
            string ReturnDescription = "";
            DataSet ds = null;
            int Result = DAL.Procedures.P_GetCpsUnionBusinessList(ref ds, ref  ReturnValue, ref ReturnDescription);

            if (Result < 0 || (ds == null) || (ds.Tables.Count < 1))
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            if (ReturnValue < 0)
            {
                Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                return;
            }

            dt = ds.Tables[0];

            Shove._Web.Cache.SetCache(Key, dt, 100000);
        }

        if (hfCurFilterExpress.Value != "") //查询过滤条件
        {
            g.CurrentPageIndex = 0;
            DataView resultView = new DataView(dt, hfCurFilterExpress.Value, "ThisMonthTrade", DataViewRowState.OriginalRows);
            try
            {
                g.DataSource = resultView;
                g.DataBind();
            }
            catch
            {
                g.CurrentPageIndex = 0;
                g.DataSource = resultView;
                g.DataBind();
            }
        }
        else
        {
            PF.DataGridBindData(g, dt);
        }

    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindData();
    }

    protected void gPager_SortBefore(object source, DataGridSortCommandEventArgs e)
    {
        BindData();
    }

    protected void g_ItemCommand(object source, DataGridCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "MemberDetail":
                {
                    Response.Redirect("CpsUsersList.aspx?ID=" + e.Item.Cells[13].Text + "&UserID=" + e.Item.Cells[14].Text);      //Cps ID
                } break;
            case "IncomeDetail":
                {
                    Response.Redirect("CpsSiteBuyDetail.aspx?ID=" + e.Item.Cells[13].Text);  //Cps ID
                } break;
            case "Stop":
                {
                    string ReturnDescription = "";
                    Users user = new Users(_Site.ID);
                    user.cps.ID = Shove._Convert.StrToLong(e.Item.Cells[13].Text, -1);  //Cps ID
                    user.ID = Shove._Convert.StrToLong(e.Item.Cells[14].Text, -1);      //OwnerUserID
                    user.GetUserInformationByID(ref ReturnDescription);
                    user.isCanLogin = false;

                    if (user.EditByID(ref ReturnDescription) < 0)
                    {
                        Shove._Web.JavaScript.Alert(this.Page, ReturnDescription);

                        return;
                    }

                    Shove._Web.Cache.ClearCache("CPS_Administrator_CpsAgentList");
                    BindData();
                } break;
            case "SetInfo":
                {
                    Response.Redirect("BaseInfo.aspx?ID=" + e.Item.Cells[13].Text); //Cps ID
                } break;
            case "SiteLogin":
                {
                    Users users = new Users(_Site.ID);

                    users.ID = Shove._Convert.StrToLong(e.Item.Cells[14].Text, 0);

                    Session["CpsAdminPageBase_Users"] = users;
                    string url;
                    url = "Admin/Default.aspx";
                    string script = "<script>window.open('" + url + "')</script>";
                    Response.Write(script);
                } break;
        }
    }

    protected void btnSearchByID_Click(object sender, EventArgs e)
    {
        hfCurFilterExpress.Value = "";

        DateTime StartTime = DateTime.Now;
        DateTime EndTime = DateTime.Now;
        bool isSearchByDate = false;
        if (!string.IsNullOrEmpty(tbBeginTime.Text.Trim()))
        {
            StartTime = Shove._Convert.StrToDateTime(tbBeginTime.Text.Trim() + " 0:0:0", StartTime.ToString());
            isSearchByDate = true;
        }

        if (!string.IsNullOrEmpty(tbEndTime.Text.Trim()))
        {
            EndTime = Shove._Convert.StrToDateTime(tbEndTime.Text.Trim() + " 23:59:59", StartTime.ToString());
            isSearchByDate = true;
        }

        if (EndTime.CompareTo(StartTime) < 0)
        {
            Shove._Web.JavaScript.Alert(this.Page, "开始时间不能大于截止时间。");

            return;
        }

        //组合过滤条件
        string filterExpress = "";
        if (rblSearch.SelectedIndex == 0)
        {
            filterExpress = " CpsDateTime>='" + StartTime.ToString("yyyy-MM-dd") + "' and CpsDateTime<'" + EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
        }
        else if (isSearchByDate)
        {
            filterExpress = " CpsDateTime>='" + StartTime.ToString("yyyy-MM-dd") + "' and CpsDateTime<'" + EndTime.AddDays(1).ToString("yyyy-MM-dd") + "'";
        }
        if (tbUserName.Text.Trim() != "") //商家ID查询
        {
            filterExpress = " UserName like '" + tbUserName.Text.Trim() + "%'";
        }


        hfCurFilterExpress.Value = filterExpress;
        BindData();
    }

    protected void btnSearchBySiteName_Click(object sender, EventArgs e)
    {
        hfCurFilterExpress.Value = "";
        if (tbCpsSiteName.Text.Trim() != "") //保存数据绑定时的过滤条件
        {
            hfCurFilterExpress.Value = "CpsName like '%" + tbCpsSiteName.Text.Trim() + "%'";
        }

        BindData();
    }
}
