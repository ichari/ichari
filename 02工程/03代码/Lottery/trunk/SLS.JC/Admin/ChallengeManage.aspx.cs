using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_ChallengeManage : AdminPageBase
{
    private string BeginTime = "";
    private string EndTime = "";
    private string userName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            BeginTime = DateTime.Now.Year +"-"+ DateTime.Now.Month + "-1";
            EndTime = DateTime.Now.Year + "-" + DateTime.Now.Month + "-1";
            this.tbBeginTime.Text = BeginTime;
            this.tbEndTime.Text = EndTime;
            BindRanking();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        RequestCompetences = Competences.BuildCompetencesList(Competences.MemberManagement, Competences.QueryData);

        base.OnInit(e);
    }
    #endregion

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbUserName.Text.Trim() == "")
        {
            Shove._Web.JavaScript.Alert(this.Page, "请输入用户名称");

            return;
        }

        Users user = new Users(_Site.ID)[_Site.ID, tbUserName.Text.Trim()];

        if (user == null)
        {
            Shove._Web.JavaScript.Alert(this.Page, "用户不存在");

            return;
        }

        BindRanking();
    }

    #region Bind

    public void BindRanking()
    {

        if (!string.IsNullOrEmpty(tbBeginTime.Text) && !string.IsNullOrEmpty(tbEndTime.Text))
        {
            BeginTime = tbBeginTime.Text;
            EndTime = tbEndTime.Text;
        }

        userName = Shove._Web.Utility.FilteSqlInfusion(tbUserName.Text.Trim());

        string cacheKey = BeginTime + EndTime;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Admin_72_Ranking_" + cacheKey);

        if (dt == null || dt.Rows.Count == 0)
        {
            string sql = string.Format("select u.ID,row_number() over (order by (b.[Score]) desc) as Ranking,u.Name,b.BetCount,b.WinCount,b.Score,b.TotalWinMoney,Count(s.WinMoney) as sumMoney from T_ChallengeBetRed as b , T_Users as u,T_ChallengeScheme as s where b.UserId = u.ID and s.InitiateUserId = b.UserId and s.InitiateUserId = b.UserId and s.WinMoney > 1 and s.DateTime > '{0}' and s.DateTime < '{1}' group by u.Name,b.BetCount,b.WinCount,b.Score,b.TotalWinMoney,u.ID order by b.Score",
                BeginTime, EndTime);

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Admin_72_Ranking_" + cacheKey, dt, 1200);
            }
        }

        DataTable dtt = new DataTable();
        dtt = dt.Clone();//拷贝框架

        DataRow[] drs = dt.Select("Name like '%" + userName + "%'");
        for (int i = 0; i < drs.Length; i++)
        {
            dtt.ImportRow((DataRow)drs[i]);//这一句再确认一下。           
        }

        dt = dtt;

        try
        {
            dt.Columns.Add("Scale", Type.GetType("System.String"));
        }
        catch { }
        foreach (DataRow dr in dt.Rows)
        {
            double BetCount = Shove._Convert.StrToDouble(dr["BetCount"].ToString(), 1);
            double WinCount = Shove._Convert.StrToDouble(dr["WinCount"].ToString(), 0);
            double x = 0;
            try
            {
                x = ((WinCount / BetCount) * 100);
            }
            catch
            { //除数为0
            }
            string c = x.ToString().Split('.')[0];
            dr["Scale"] = x.ToString().Split('.')[0];
                        
            // 转换积分
            dr["Score"] = ((int)Shove._Convert.StrToDouble(dr["Score"].ToString(), 0)).ToString();
            try
            {
                dr["TotalWinMoney"] = Convert.ToDouble(dr["TotalWinMoney"]).ToString("F2");
            }catch{}

        }
        gRanking.DataSource = dt;
        gRanking.DataBind();
    }

    protected void gPager_PageWillChange(object Sender, Shove.Web.UI.PageChangeEventArgs e)
    {
        BindRanking();
    }

    protected void gPager_SortBefore(object source, System.Web.UI.WebControls.DataGridSortCommandEventArgs e)
    {
        BindRanking();
    }
    #endregion

    protected void btnSearchByRegDate_Click(object sender, EventArgs e)
    {
        BindRanking();
    }
}
