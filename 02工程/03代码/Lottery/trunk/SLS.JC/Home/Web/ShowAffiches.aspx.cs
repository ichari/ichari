using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;

public partial class Home_Web_ShowAffiches : SitePageBase
{
    protected static string NewsHtml_Fx = "";
    protected static string NewsHtml_Jq = "";
    public string WinUserList = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindData();
            BindWin();
        }
    }

    /// <summary>
    /// 绑定最新中奖
    /// </summary>
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

    private void BindData()
    {
        long id = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("id"), -1);

        if (id < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "您访问的数据不存在，可能是参数错误或内容已经被删除！", this.GetType().BaseType.FullName);

            return;
        }

        DataTable dt = dt = new DAL.Tables.T_SiteAffiches().Open("", "ID=" + id.ToString() + "", "");

        if (dt == null)
        {
            PF.GoError(ErrorNumber.Unknow, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return;
        }

        if (dt.Rows.Count < 0)
        {
            PF.GoError(ErrorNumber.Unknow, "您访问的数据不存在，可能是参数错误或内容已经被删除！", this.GetType().BaseType.FullName);

            return;
        }

        DataRow dr = dt.Rows[0];

        lbTitle.Text = dr["Title"].ToString().Replace("<p>","").Replace("</p>","");
        lbDateTime.Text = dr["DateTime"].ToString();
        lbContent.Text = dr["Content"].ToString().Replace("<p>","").Replace("</p>","");
    }
}
