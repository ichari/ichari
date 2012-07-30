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

public partial class Admin_Users : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID, "1");
            DateTime DateTimeNow = System.DateTime.Now;
            tbBeginTime.Text = DateTimeNow.AddDays(-(DateTimeNow.Day-1)).ToString("yyyy-MM-dd");
            tbEndTime.Text = DateTimeNow.ToString("yyyy-MM-dd");
            BindData();
            GetCompetence();

        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement,Competences.QueryData);

        base.OnInit(e);
    }

    #endregion

    private void GetCompetence()
    {
  
        DataTable dt = Shove.Database.MSSQL.Select("select * from T_UserInGroups  where GroupID =1 and UserID=" + _User.ID + " ");


            if (dt != null && dt.Rows.Count > 0)
            {
                this.btnDownload.Visible = true;
                this.btnSelect.Visible = true;
            }
            else
            {
                this.btnDownload.Visible = false;
                this.btnSelect.Visible = false;
            }
    }

    private void BindData()
    {
        DataTable dt = null;
        string sql = "";
        if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID) != null)
        {
            if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "1")    //默认情况， 显示单月注册的会员
            {
                sql = "select * from V_Users where (CONVERT(datetime,RegisterTime) between DATEADD(DD,-(DatePart(D,GETDATE())),GETDATE()) and GETDATE()) AND SiteID = @SiteID order by RegisterTime desc";
                dt = MSSQL.Select(sql,new MSSQL.Parameter("SiteID",SqlDbType.Int,0,ParameterDirection.Input,_Site.ID));
            }
            else if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "2") //根据注册时间搜索
            {
                if (tbBeginTime.Text.Trim() == "" && tbEndTime.Text.Trim() == "")
                {
                    sql = "select * from V_Users where SiteID = @SiteID order by RegisterTime desc";
                    dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
                }
                else
                {
                    sql = "select * from V_Users where (CONVERT(datetime,RegisterTime) between @StartDate and @EndDate or CONVERT(datetime,RegisterTime) = @EndDate) and SiteID = @SiteID order by RegisterTime desc";
                    DateTime dtBegin = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbBeginTime.Text), DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dtEnd = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbEndTime.Text), DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1);
                    dt = MSSQL.Select(sql,
                        new MSSQL.Parameter("StartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, dtBegin),
                        new MSSQL.Parameter("EndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, dtEnd),
                        new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
                }
            }
            else if (Shove._Web.Cache.GetCache("FirstQuery_" + _User.ID).ToString() == "3") //注册未充值会员
            {
                if (this.tbBeginTime.Text.Trim() == "" || this.tbEndTime.Text.Trim() == "")
                {
                    sql = "select * from V_Users Users where not exists ( select UserID from T_UserPayDetails PayDetails where Users.ID=PayDetails.UserID and Result = 1 ) and SiteID = @SiteID order by RegisterTime desc";
                    dt = MSSQL.Select(sql,
                        new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
                }
                else
                {
                    sql = "select * from V_Users Users where not exists ( select UserID from T_UserPayDetails PayDetails where Users.ID=PayDetails.UserID and Result = 1 ) and SiteID = @SiteID and (CONVERT(datetime,RegisterTime) between @StartDate and @EndDate) order by RegisterTime desc";
                    DateTime dtBegin = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbBeginTime.Text), DateTime.Now.ToString("yyyy-MM-dd"));
                    DateTime dtEnd = Shove._Convert.StrToDateTime(Shove._Web.Utility.FilteSqlInfusion(tbEndTime.Text), DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1);
                    dt = MSSQL.Select(sql,
                        new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID),
                        new MSSQL.Parameter("StartDate", SqlDbType.DateTime, 0, ParameterDirection.Input,dtBegin ),
                        new MSSQL.Parameter("EndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, dtEnd));
                }

                
            }
            else   //所有会员
            {
                sql = "select * from V_Users where SiteID = @SiteID order by RegisterTime desc";
                dt = MSSQL.Select(sql,new MSSQL.Parameter("SiteID",SqlDbType.Int,0,ParameterDirection.Input,_Site.ID));
            }
        }
        else  //所有会员
        {
            sql = "select * from V_Users where SiteID = @SiteID order by RegisterTime desc";
            dt = MSSQL.Select(sql, new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
        }

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", "Admin_Users");

            return;
        }

        PF.DataGridBindData(g, dt, gPager);

        btnSearch.Enabled = (dt.Rows.Count > 0);
    }

    protected void g_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            e.Item.Cells[0].Text = "<a href='UserDetail.aspx?SiteID=" + e.Item.Cells[26].Text + "&ID=" + e.Item.Cells[24].Text + "'>" + e.Item.Cells[0].Text + "</a>";

            CheckBox cb = (CheckBox)e.Item.Cells[17].FindControl("cbisCanLogin");
            cb.Checked = Shove._Convert.StrToBool(e.Item.Cells[25].Text, true);

            CheckBox cbisEmailValided = (CheckBox)e.Item.Cells[4].FindControl("cbisEmailValided");
            if (cbisEmailValided != null)
            {
                cbisEmailValided.Checked = Shove._Convert.StrToBool(e.Item.Cells[27].Text, false);
            }

            CheckBox cbisMobileValided = (CheckBox)e.Item.Cells[6].FindControl("cbisMobileValided");
            if (cbisMobileValided != null)
            {
                cbisMobileValided.Checked = Shove._Convert.StrToBool(e.Item.Cells[28].Text, false);
            }

            e.Item.Cells[21].Text = e.Item.Cells[21].Text.Trim() == "2" ? "<font color='red'>高级</font>" : e.Item.Cells[21].Text.Trim() == "3" ? "<font color='blue'>VIP</font>" : "普通";

            double money = Shove._Convert.StrToDouble(e.Item.Cells[10].Text, 0);
            e.Item.Cells[10].Text = money.ToString("N");

            money = Shove._Convert.StrToDouble(e.Item.Cells[11].Text, 0);
            e.Item.Cells[11].Text = money.ToString("N");
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

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名称");

            return;
        }

        DataTable dt = null;
        string sql = "";

        sql = "select * from V_Users where name like '%"+ tbUserName.Text.Trim() +"%'";
        dt = MSSQL.Select(sql);

        PF.DataGridBindData(g, dt, gPager);

        btnSearch.Enabled = (dt.Rows.Count > 0);
    }

    protected void btnDownload_Click(object sender, EventArgs e)
    {
        DataTable dt = new DAL.Tables.T_Users().Open("", "", "[ID]");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.Page.GetType().BaseType.FullName);

            return;
        }

        string FileName = "T_Users.xls";

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
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.ClearCache("FirstQuery_" + _User.ID);
        BindData();
    }
    protected void btnSearchByRegDate_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID,"2");
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }
        BindData();
    }

    protected void btnSearchNoPay_Click(object sender, EventArgs e)
    {
        Shove._Web.Cache.SetCache("FirstQuery_" + _User.ID,"3");
        if (tbBeginTime.Text.Trim() != "" && tbEndTime.Text.Trim() != "")
        {
            DateTime fromDate = DateTime.Now;
            DateTime toDate = DateTime.Now;
            if (!DateTime.TryParse(tbBeginTime.Text, out fromDate) && !DateTime.TryParse(tbEndTime.Text, out toDate))
            {
                Shove._Web.JavaScript.Alert(this.Page, "请输入正确的日期格式:2008-8-8");
                return;
            }
        }
        BindData();
    }
}
