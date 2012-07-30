using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

public partial class CelebrityHall_Default : RoomPageBase
{

    //本期推荐
    protected string UserID = "";
    protected string UserName = "";
    protected string UserLotteryID = "";
    private string dingZhi = "";
    // html
    protected static string innerHTML = "";
    // 名人汇聚Html
    protected static string innerHTMLStar = "";
    // 新闻Html
    protected static string NewsHTML = "";

    public static Dictionary<int, string> Lotteries = new Dictionary<int, string>();

    static CelebrityHall_Default()
    {
        Lotteries[59] = "15X5";
        Lotteries[9] = "22X5";
        Lotteries[65] = "31X7";
        Lotteries[6] = "3D";
        Lotteries[39] = "CJDLT";
        Lotteries[58] = "DF6J1";
        Lotteries[2] = "JQC";
        Lotteries[15] = "LCBQC";
        Lotteries[63] = "PL3";
        Lotteries[64] = "PL5";
        Lotteries[13] = "QLC";
        Lotteries[3] = "QXC";
        Lotteries[1] = "SFC";
        Lotteries[61] = "SSC";
        Lotteries[29] = "SSL";
        Lotteries[5] = "SSQ";
        Lotteries[62] = "SYYDJ";
        Lotteries[72] = "JCZQ";
        Lotteries[73] = "JCLQ";
        Lotteries[74] = "SFC";
        Lotteries[75] = "SFC_9_D";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            //绑定潜力推荐
            BindPotential();
            //绑定本期推荐
            BindRecommend();
            //绑定英雄榜
            BindHero();
            //绑定名人合买
            BindCollect();
            //绑定名人汇聚
            BindStar();
            // 绑定新闻
            BindNews();
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

    /// <summary>
    ///  绑定潜力推荐
    /// </summary>
    private void BindPotential()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Potentials");

        if (dt == null)
        {

            dt = Shove.Database.MSSQL.Select("select top 7 row_number() over (order by (u.Level) desc) as Ranking, s.ID as SchemeID,u.ID as UserID,i.Name as IsuseName,l.Name,u.Name as UserName,s.Money "
                    +"from T_Schemes s "
                    +"inner join T_Isuses  i on i.ID = s.IsuseID "
                    +"inner join T_Users u on s.InitiateUserID = u.ID "
                    +"inner join T_Lotteries l on i.LotteryID = l.ID "
                    +"where s.QuashStatus = 0 and s.isOpened = 0 and s.Share > s.BuyedShare order by u.Level desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(128)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Potentials", dt, 1200);
            }
        }

        gPotential.DataSource = dt;
        gPotential.DataBind();        
    }

    /// <summary>
    ///  绑定本期推荐
    /// </summary>
    private void BindRecommend()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Recommends");

        if (dt == null)
        {

            string sql = "select top 4 u.ID,l.ID as LotteryID, u.Name,l.Name as LotteryName,i.Name as IsuseName ,s.IsuseID, b.WinMoneyNoWithTax as WinMoney "
                + "from T_BuyDetails b "
                + "inner join T_Users u on b.UserID = u.ID "
                + "inner join T_Schemes s on b.SchemeID = s.ID "
                + "inner join T_Isuses i on s.IsuseID = i.ID "
                + "inner join T_Lotteries l on i.LotteryID = l.ID "
                + "where u.ID = (select top 1 UserID from T_Personages where IsRecommend = 1) "
                + "order by  b.WinMoneyNoWithTax desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(149)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Recommends", dt, 1200);
            }
        }

        if (dt == null)
        {
            gPotential.DataSource = null;
            return;
        }

        if (dt.Rows.Count < 1)
        {
            gPotential.DataSource = null;
            return;
        }

        // 对本期推荐人赋值
        UserID = dt.Rows[0]["ID"].ToString();
        // 保存推荐人物昵称
        UserName = dt.Rows[0]["Name"].ToString();

        // 查询后台设置定制彩种ID
        UserLotteryID = Shove.Database.MSSQL.ExecuteScalar("select LotteryID from T_Personages where IsRecommend = 1 and UserID = " + UserID).ToString();

        if (Shove._Convert.StrToInt(UserLotteryID, -1) < 1)
        {
            hffLotteryID.Value = "-1";
        }

        // 过滤竞彩足、篮球的定制跟单
        if (UserLotteryID.Equals("72") || UserLotteryID.Equals("73"))
        {
            btn_Single.Visible = false;
        }

        hfUserID.Value = UserID;
        hffLotteryID.Value = UserLotteryID;
        
        // 过滤中奖为零的
        DataTable dtt = dt.Clone();

        foreach (DataRow dr in dt.Rows)
        {
            if (Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0.00) > 1)
            {
                dtt.ImportRow(dr);
            }
        }

        gRecommend.DataSource = dtt;
        gRecommend.DataBind();     
    }

    /// <summary>
    /// 绑定英雄榜
    /// </summary>
    private void BindHero()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Hero");

        if (dt == null)
        {

            string sql = "select  top 20 row_number() over (order by (b.WinMoneyNoWithTax) desc) as Ranking, u.ID as UserID,l.ID as LotteryID, u.Name as UserName,l.Name as Name,i.Name as IsuseName ,s.IsuseID, b.WinMoneyNoWithTax as [Money]"
                + "from T_BuyDetails b "
                + "inner join T_Users u on b.UserID = u.ID "
                + "inner join T_Schemes s on b.SchemeID = s.ID "
                + "inner join T_Isuses i on s.IsuseID = i.ID "
                + "inner join T_Lotteries l on i.LotteryID = l.ID "
                + "where b.[WinMoneyNoWithTax] > 1 "
                + "order by  b.WinMoneyNoWithTax desc";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(130)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Hero", dt, 1200);
            }
        }
        gHero.DataSource = dt;
        gHero.DataBind();
    }

    /// <summary>
    /// 绑定名人合买
    /// </summary>
    private void BindCollect()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Collects");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("select s.ID,u.ID as UserID,s.ID as SchemeID,l.ID as LotteryID ,s.[Money],s.Share,s.BuyedShare,i.Name,l.Name as LotteryName,u.Name as UserName,[WinMoney]=(select SUM(WinMoneyNoWithTax) from T_Schemes where InitiateUserID = s.InitiateUserID and WinMoney > 0 and DateTime > dateAdd(mm,-1,getdate())) from T_Schemes as s ,T_Isuses as i,T_PlayTypes as p,T_Lotteries as l,T_Users as u where s.Share > s.BuyedShare and s.QuashStatus = 0 and s.PlayTypeID = p.ID  and s.IsuseID = i.ID and l.ID = p.LotteryID and s.InitiateUserID in (select UserID from T_Personages group by UserID) and u.ID in (select UserID from T_Personages group by UserID) and u.ID = s.initiateUserID");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(131)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Collects", dt, 1200);
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

        // 区分UserID
        string UserIDState = "";

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<div class=\"mrhm_list\" id=\"_mr_info\"><div class=\"LeftBotton\" id=\"LeftArr\"></div><div></div><div class=\"Cont\" id=\"ISL_Cont_1\"><ul>");

        int index = 0;
        bool state = false;

        foreach (DataRow dr in dt.Rows)
        {


            if (!UserIDState.Equals(dr["UserID"].ToString()))
            {
                index = 0;
                if (state)
                {
                    sb.AppendLine("</div></li>");
                }
                else
                {
                    state = true;
                }

                sb.AppendLine("<li><div class=\"mr_info\"><h2><img src=\"images/icon.gif\" width=\"58\" height=\"58\" /></h2>")
                  .AppendLine("<dl><h3><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\">" + dr["UserName"] + "</a></h3><dt>")
                  .AppendLine("<span></span>近期中奖 <em>" + Shove._Convert.StrToDouble(dr["WinMoney"].ToString(), 0).ToString("F2") + "</em>元</dt><dt></dt></dl>")
                  .AppendLine("<div class=\"mr_fangan_tit\"><dl><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\">更多&gt;&gt;</a></dl><h3>他的方案</h3></div>")
                  .AppendLine("<div class=\"mr_fangan_list\">");

                // 计算进度
                double sacle = Shove._Convert.StrToDouble(dr["BuyedShare"].ToString(), 0) / Shove._Convert.StrToDouble(dr["Share"].ToString(), 0) * 100;

                sb.AppendLine("<dl style=\"width:210px\" ><span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">抢购</a></span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">" + dr["LotteryName"] + "" + dr["Name"] + "期</a> <em>￥" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("F2") + "</em> 进度<em>" + sacle.ToString("F2") + "%</em></dl>");

            }
            else
            {
                if (index < 2)
                {
                    double sacle = Shove._Convert.StrToDouble(dr["BuyedShare"].ToString(), 0) / Shove._Convert.StrToDouble(dr["Share"].ToString(), 0) * 100;

                    sb.AppendLine("<dl style=\"width:210px\" ><span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\" >抢购</a></span><a href=\"../Home/Room/Scheme.aspx?id=" + dr["SchemeID"] + "\" target=\"_blank\">" + dr["LotteryName"] + "" + dr["Name"] + "期</a> <em>￥" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("F2") + "</em> 进度<em>" + sacle.ToString("F1") + "%</em></dl>");

                }
                index++;
            }

            UserIDState = dr["UserID"].ToString();


        }
        sb.AppendLine("</div></div></li>");
        sb.AppendLine("</ul></div><div class=\"RightBotton\" id=\"RightArr\"></div></div>");
        innerHTML = sb.ToString();
    }

    /// <summary>
    /// 绑定名人汇聚
    /// </summary>
    private void BindStar()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_Star");

        if (dt == null)
        {
            dt = Shove.Database.MSSQL.Select("Select b.Name,a.* from T_Personages a inner join T_Lotteries  b on b.ID = a.LotteryID order by a.[Order] asc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(132)", this.GetType().BaseType.FullName);

                return;
            }

            if (dt.Rows.Count > 0)
            {
                // 1200M
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_Star", dt, 1200);
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

        ArrayList list = new ArrayList();

        System.Text.StringBuilder sb = new StringBuilder();

        int _index = 0;

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            _index++;

            string LotteryID = dt.Rows[i]["LotteryID"].ToString();

            if (list.Contains(LotteryID) || list.Count >= 4)
            {
                continue;
            }

            //获取彩种名称
            string LotteryName = dt.Rows[i]["Name"].ToString();
            if (list.Count == 3)
            {
                sb.AppendLine("<div class=\"allren no_line\"><h3><span>" + LotteryName + "</span></h3><ul>");
            }
            else
            {
                sb.AppendLine("<div class=\"allren\"><h3><span>" + LotteryName + "</span></h3><ul>");
            }

            // 通过LotteryID 查找所有该彩种的人物
            DataRow[] drs = dt.Select("LotteryID = " + LotteryID);

            for (int j = 0; j < drs.Length && j < 8; j++)
            {
                //LotteryID UserID
                DataRow dr = drs[j];
                sb.AppendLine("<li><span class=\"m_icon\"><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\"><img src=\"images/icon.gif\" width=\"58\" height=\"58\" /></a></span><span><a href=\"../Home/Web/Score.aspx?id=" + dr["UserID"] + "&LotteryID=" + dr["LotteryID"] + " \" target=\"_blank\">" + dr["UserName"] + "</a></span></li>");
            }
            if (_index < 4)
            {
            }
            sb.AppendLine("</ul><div class=\"cl\"></div></div>");

            list.Add(LotteryID);
        }

        innerHTMLStar = sb.ToString();
    }

    /// <summary>
    /// 绑定新闻
    /// </summary>
    private void BindNews()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_CelebrityHall_News");

        if (dt == null)
        {
            dt = new DAL.Tables.T_News().Open("top 12 [ID],[Title],[Content]", "TypeID = 10", "[ID]");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试(133)", this.GetType().BaseType.FullName);
                return;
            }

            if (dt.Rows.Count > 0)
            {
                Shove._Web.Cache.SetCache("DataCache_CelebrityHall_News", dt, 1200);
            }
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

    private void ResponseTailor(bool b)
    {
        if (_User.ID < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "请重新登陆", this.GetType().FullName);

            return;
        }

        long userid = Shove._Convert.StrToLong(hfUserID.Value, -1);

        int lotteryid = Shove._Convert.StrToInt(hffLotteryID.Value, -1);

        if (lotteryid < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }

        int temp = -1;
        if (b)
        {
            temp = lotteryid;
        }
        string headMethod = "followScheme(" + temp + ");$Id(\"iframeFollowScheme\").src=\"../Home/Room/FollowFriendSchemeAdd.aspx?LotteryID=" + temp + "&DzLotteryID=" + temp;

        if (userid < 1)
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误", this.GetType().FullName);

            return;
        }
        if (!new SLS.Lottery().ValidID(lotteryid))
        {
            PF.GoError(ErrorNumber.Unknow, "参数错误！(彩种)", this.GetType().FullName);

            return;
        }

        Users tu = new Users(1);
        tu.ID = userid;
        dingZhi = "&FollowUserID=" + userid + "&FollowUserName=" + HttpUtility.UrlEncode(tu.Name) + "\"";
        string ReturnDescription = "";

        if (tu.GetUserInformationByID(ref ReturnDescription) != 0)
        {

            return;
        }

        dingZhi = Shove._Security.Encrypt.EncryptString(PF.GetCallCert(), headMethod + dingZhi);

        if ((lotteryid != 72) || (lotteryid != 73))
        {
            Response.Redirect("../Lottery/Buy_" + Lotteries[lotteryid] + ".aspx?DZ=" + dingZhi + "");
        }
    }

    protected void btn_Single_Click(object sender, ImageClickEventArgs e)
    {
        ResponseTailor(true);
    }
}
