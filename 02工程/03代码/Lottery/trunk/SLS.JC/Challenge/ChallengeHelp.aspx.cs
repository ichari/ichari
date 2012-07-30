using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class Challenge_ChallengeHelp : RoomPageBase
{
    private string userName = "";
    protected bool state = false;
    protected int indexs = 0;

    protected static string NewsHTML = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            // 绑定新闻
            BindNews();
            // 绑定排行(10天)
            BindRankingScoreToDay();
            // 绑定月 排行
            BindRankingScoreMonth();
            // 绑定总 排行
            BindRankingScoreToMain();
            // 绑定  红人
            BindBetHot();
            // 绑定 用户信息(已登录)
            BindUserDetails();
        }
    }

    #region Web 窗体设计器生成的代码

    override protected void OnInit(EventArgs e)
    {
        RequestLoginPage = this.Request.Url.AbsoluteUri;

        isRequestLogin = false;
        isAtFramePageLogin = false;

        base.OnInit(e);
    }

    #endregion

    #region Bind

    private void BindUserDetails()
    {
        if (_User == null)
        {
               pUserDetails.Visible = false;
            return;
        }
        if (_User.ID < 1)
        {
            return;
        }
        string sql = "select c.UserId,u.Name,c.BetCount,c.WinCount,c.Score,c.TotalWinMoney from T_ChallengeBetRed as c ,T_Users as u where c.UserId = u.ID and c.UserId = " + _User.ID;
        DataTable dtUserDetails = Shove.Database.MSSQL.Select(sql);

        if (dtUserDetails == null)
        {
            return;
        }

        if (dtUserDetails.Rows.Count < 1)
        {
            return;
        }

        // 计算积分
        foreach (DataRow dr in dtUserDetails.Rows)
        {
            dr["Score"] = ((int)Shove._Convert.StrToDouble(dr["Score"].ToString(),0)).ToString();
        }

          gUserDetails.DataSource = dtUserDetails;
          gUserDetails.DataBind();
          pUserDetails.Visible = true;
    }
    
    /// <summary>
    /// 绑定积分(天)
    /// </summary>
    private void BindRankingScoreToDay()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToDay");

        if (dt == null)
        {

            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score from T_ChallengeScheme as s,T_Users as u where DateTime > dateadd(DD,-10,getdate()) and u.ID = s.InitiateUserId and s.WinMoney > 0 group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToDay", dt, 600);
            }
        }
        gSchemesToDay.DataSource = dt;
        gSchemesToDay.DataBind();
    }
    /// <summary>
    /// 绑定积分(月)
    /// </summary>
    private void BindRankingScoreMonth()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToMonth");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score,Count(s.ID) as Counts from T_ChallengeScheme as s,T_Users as u where u.ID = s.InitiateUserId and DateTime > dateadd(MM,-1,getdate()) and s.WinMoney > 0 group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(131)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToMonth", dt, 600);
            }
        }
        gSchemesToMonth.DataSource = dt;
        gSchemesToMonth.DataBind();
    }
    /// <summary>
    /// 绑定积分(总)
    /// </summary>
    private void BindRankingScoreToMain()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_ScoreToMain");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 10 row_number() over (order by (SUM(s.WinMoney)) desc) as Ranking,u.Name ,s.InitiateUserId as UserId,Convert(int,SUM(s.WinMoney)) as Score from T_ChallengeScheme as s,T_Users as u where u.ID = s.InitiateUserId and s.WinMoney > 0 group by s.InitiateUserId,u.Name order by (SUM(s.WinMoney)) desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(132)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_ScoreToMain", dt, 600);
            }
        }
        gSchemesToMain.DataSource = dt;
        gSchemesToMain.DataBind();
    }
    /// <summary>
    /// 绑定擂台红人
    /// </summary>
    private void BindBetHot()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_BetHot");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select top 4 c.UserId,u.Name,c.BetCount,c.WinCount,c.TotalWinMoney from T_ChallengeBetRed as c ,T_Users as u where c.UserId = u.ID order by BetCount desc ,c.WinCount desc , c.TotalWinMoney desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(133)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            if (dt != null)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_BetHot", dt, 600);
            }
        }
        foreach (DataRow dr in dt.Rows)
        {
            dr["TotalWinMoney"] = Shove._Convert.StrToDouble(dr["TotalWinMoney"].ToString(), 0.00).ToString("F2");
        }
        gBetHot.DataSource = dt;
        gBetHot.DataBind();
    }

    private void BindNews()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_Challenge_72_News");

        if (dt == null)
        {
            dt = new DAL.Tables.T_News().Open("top 5 [ID],[Title],[Content]", "TypeID = 9", "DateTime desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(129)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 1)
            {
                Shove._Web.Cache.SetCache("DataCache_Challenge_72_News", dt, 1200);
            }
        }
        if (dt == null)
        {
            return;
        }
        if (dt.Rows.Count < 1)
        {
            return;
        }

        Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendLine("<li>");
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append("<a href=\"" + dr["Content"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
            }
            else
            {
                sb.Append("<a href=\"../Home/Web/ShowNews.aspx?Id=" + dr["ID"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dr["Title"].ToString(), 16) + "</a>");
            }
            sb.AppendLine("</li>");
        }

        NewsHTML = sb.ToString();
    }

    #endregion
}
