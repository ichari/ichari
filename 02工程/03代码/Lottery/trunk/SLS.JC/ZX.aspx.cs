using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class ZX : System.Web.UI.Page
{
    public string New = "";
    public string Title = "";
    public string WinUserList = "";
    protected string pageHtml = "";
    string _NewTypeName = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            string NewsName = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("NewsName"));

            switch (NewsName)
            {
                case "jczq":
                    NewsName = "竞彩足球";
                    _NewTypeName = "jczq";
                    break;
                case "jclq":
                    NewsName = "竞彩篮球";
                    _NewTypeName = "jclq";
                    break;
                case "dlt":
                    NewsName = "超级大乐透";
                    _NewTypeName = "dlt";
                    break;
                case "szpl":
                    NewsName = "排列3/5";
                    _NewTypeName = "szpl";
                    break;
                case "qxc":
                    NewsName = "七星彩";
                    _NewTypeName = "qxc";
                    break;
                case "22x5":
                    NewsName = "22选5";
                    _NewTypeName = "22x5";
                    break;
                case "ctzc":
                    NewsName = "足彩资讯";
                    _NewTypeName = "ctzc";
                    break;
                case "ogzx":
                    NewsName = "欧冠资讯";
                    _NewTypeName = "ogzx";
                    break;
                case "yczx":
                    NewsName = "英超资讯";
                    _NewTypeName = "yczx";
                    break;
                case "xjzx":
                    NewsName = "西甲资讯";
                    _NewTypeName = "xjzx";
                    break;
                case "yjsz":
                    NewsName = "意甲资讯";
                    _NewTypeName = "yjsz";
                    break;
                case "djsz":
                    NewsName = "德甲资讯";
                    _NewTypeName = "djsz";
                    break;
                case "jjc":
                    NewsName = "竞技彩资讯";
                    _NewTypeName = "jjc";
                    break;
                case "szc":
                    NewsName = "数字彩资讯";
                    _NewTypeName = "szc";
                    break;
                default:
                    NewsName = "竞彩足球";
                    _NewTypeName = "jczq";
                    break;
            }

            Title = NewsName;

            New = BindNewsForLottery(NewsName);

            BindWin();
        }
    }

    private string BindNewsForLottery(string TypeName)
    {
        string Key = "ForeCast_BindNewsForLottery" + TypeName;

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = "";

            sql = "select a.ID, Title,Content,DateTime,b.Name as TypeName from (select ID,Title,Content,TypeID,[DateTime],isCommend from dbo.T_News where isShow = 1 ) a "
                   + "inner join dbo.T_NewsTypes b "
                   + "on a.TypeID = b.ID "
                   + "where b.Name = '" + TypeName + "'"
                   + "order by isCommend,DateTime desc";
            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return "";
            }

            Shove._Web.Cache.SetCache(Key, dt, 1200);
        }

        // 获取当前页数
        int pageindex = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("p").ToString(), 1);
        // 每页行数
        int PageNum = 20;

        int perPageRowCount = PageNum;

        // 总行数
        int rowCount = dt.Rows.Count;

        int pageCount = rowCount % perPageRowCount == 0 ? rowCount / perPageRowCount : rowCount / perPageRowCount + 1;

        if (pageindex > pageCount)
        {
            pageindex = pageCount;
        }

        if (pageindex < 1)
        {
            pageindex = 1;
        }

        int Count = 0;

        if (perPageRowCount * pageindex > rowCount)
        {
            Count = rowCount;
        }
        else
        {
            Count = perPageRowCount * pageindex;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<span class=\"jilu\">共" + pageCount.ToString() + "页，" + dt.Rows.Count + "条记录</span><span id=\"first\"><a href=\"/ZX.aspx?NewsName=" + _NewTypeName + "&p=1\">首页</a></span>");

        if (pageindex == 1)
        {
            sb.Append("<span class=\"disabled\">« 上一页</span>");
        }
        else
        {
            sb.Append("<span><a href=\"/ZX.aspx?NewsName=" + _NewTypeName + "&p=" + (pageindex - 1).ToString() + "\">« 上一页</a></span>");
        }

        for (int i = 0; i < pageCount; i++)
        {
            if (i == pageindex - 1)
            {
                sb.Append("<span class=\"current\">" + (i + 1).ToString() + "</span>");
                continue;
            }

            if ((i < pageindex + 4 || i < 9) && (i > pageindex - 6 || i > pageCount - 10))
            {
                sb.Append("<a href=\"/ZX.aspx?NewsName=" + _NewTypeName + "&p=" + (i + 1).ToString() + "\">" + (i + 1).ToString() + "</a>");
            }
        }

        if (pageindex == pageCount)
        {
            sb.Append("<span class=\"disabled\">下一页 »</span>");
        }
        else
        {
            sb.Append("<span><a href=\"/ZX.aspx?NewsName=" + _NewTypeName + "&p=" + (pageindex + 1).ToString() + "\">下一页 »</a></span>");
        }

        sb.Append("<span id=\"last\" value=\"" + pageCount.ToString() + "\"><a href=\"/ZX.aspx?NewsName=" + _NewTypeName + "&p=" + (pageCount).ToString() + "\">尾页</a></span>");

        // 分页
        pageHtml = sb.ToString();



        DataRow[] dr = dt.Select("1=1");


        // 分页结束，开始正文
        sb = new StringBuilder();

        sb.Append("<ul>");

        string Title = "";

        for (int i = (pageindex - 1) * perPageRowCount; i < Count; i++)
        {
            Title = dr[i]["Title"].ToString().Trim();

            sb.Append("<li><span class=\"newsdate\">")
               .Append(Shove._Convert.StrToDateTime(dr[i]["DateTime"].ToString(), DateTime.Now.ToString()).ToString("yyyy-MM-dd HH:mm:ss"))
               .Append("</span>")
                .Append("<a href='");

            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr[i]["Content"].ToString());

            if (m.Success)
            {
                sb.Append(dr[i]["Content"].ToString());
            }
            else
            {
                sb.Append("../Home/Web/ShowNews.aspx?ID=" + dr[i]["ID"].ToString());
            }

            sb.Append("' target='_blank'>" + Shove._String.Cut(Title, 30) + "</a></li>");
        }
        
        sb.Append("</ul>");

        return sb.ToString();
    }

    private void BindWin()
    {
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable("DataCache_ZX");

        if (dt == null)
        {

            string sql = "select  top 10 u.ID as UserID,l.ID as LotteryID, u.Name as UserName,l.Name as Name,i.Name as IsuseName ,s.Multiple, b.WinMoneyNoWithTax as [Money]"
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
                Shove._Web.Cache.SetCache("DataCache_ZX", dt, 1200);
            }
        }

        if (dt.Rows.Count < 1)
        {
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr><td class=\"text_l\">" + dr["UserName"].ToString().Substring(0, 1) + "***</td>");
            sb.Append("<td class=\"text_c\">" + dr["Name"].ToString() + "</td>");
            sb.Append("<td class=\"text_c\">" + dr["Multiple"].ToString() + "</td>");
            sb.Append("<td class=\"red text_r\">" + Shove._Convert.StrToDouble(dr["Money"].ToString(), 0).ToString("N") + "<span class=\"gray\">元</span></td></tr>");
        }

        WinUserList = sb.ToString();
    }
}
