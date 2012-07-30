using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

public partial class CPS_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            tdTGZN.InnerHtml = BindNews("CPS推广指南");
            tdXWGG.InnerHtml = BindNews("CPS新闻公告");
            BindUsers();
        }
    }

    private DataTable BindNews()
    {
        string Key = "CPS_Default_BindNews";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = @"select * from
                                (select top 4 ID,Title,Content,TypeName from V_News where isShow = 1  and [TypeName] = 'CPS新闻公告'
                                order by isCommend,ID desc)a
                            union
                            select * from
                                (select top 8 ID,Title,Content,TypeName from V_News where isShow = 1  and [TypeName] = 'CPS推广指南'
                                order by isCommend,ID desc)b";

            dt = Shove.Database.MSSQL.Select(sql);

            if (dt == null)
            {
                return null;
            }

            Shove._Web.Cache.SetCache(Key, dt, 1200);
        }

        return dt;
    }

    private string BindNews(string TypeName)
    {
        DataRow[] drs = BindNews().Select("TypeName='" + TypeName + "'", "ID desc");

        StringBuilder sb = new StringBuilder();
        sb.Append("<table width=\"95%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" style='margin-top:10px'>");

        string Title = "";
        bool IsClass = false;
        string color = "";

        foreach (DataRow dr in drs)
        {
            Title = dr["Title"].ToString();

            if ((Title.IndexOf("<font class=red12>") > -1 || Title.IndexOf("<font class=black12>") > -1))
            {
                if (Title.Contains("<font class=red12>"))
                {
                    color = "red";
                }
                if (Title.Contains("<font class=black12>"))
                {
                    color = "black12";
                }

                Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                IsClass = true;
            }

            sb.AppendLine("<tr>")
                    .AppendLine("<td width=\"5%\" height=\"26\" align=\"center\"><img src=\"images/dian.jpg\" width=\"3\" height=\"3\" /></td>")
                    .Append("<td width=\"95%\" height=\"26\" align=\"left\" class=\"hui\">").Append("<a href='");

            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                sb.Append(dr["Content"].ToString());
            }
            else
            {
                sb.Append("NewsShow.aspx?ID=" + dr["ID"].ToString());
            }

            sb.Append("' target='_blank'>");

            if (IsClass)
            {
                sb.Append("<font class='" + color + "'>").Append(Shove._String.Cut(Title, 24)).Append("</font>");
            }
            else
            {
                sb.Append(Shove._String.Cut(Title, 24));
            }

            sb.AppendLine("</a></td>")
            .AppendLine("</tr>");
        }

        sb.Append("</table>");

        return sb.ToString();
    }

    private void BindUsers()
    {
        string Key = "CPS_Default_BindUsers";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            dt = new DAL.Tables.T_Users().Open("top 9 Name,Bonus", "Bonus > 0", "Bonus desc");

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(Key, 3600);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width=\"96%\" border=\"0\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\">");

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            sb.Append("<tr>")
                .Append("<td width=\"12%\" height=\"24\" align=\"center\">")
                .Append("<img src=\"images/num_" + (i + 1).ToString() + ".gif\" width=\"13\" height=\"13\" />")
                .Append("</td>")
                .Append("<td width=\"57%\" height=\"24\">")
                .Append(dt.Rows[i]["Name"].ToString())
                .Append("</td>")
                .Append("<td width=\"31%\" height=\"24\">")
                .Append(Shove._Convert.StrToDouble(dt.Rows[i]["Bonus"].ToString(),0).ToString("N"))
                .Append("元</td>")
                .Append("</tr>");
        }

        sb.Append("</table>");

        tdUsers.InnerHtml = sb.ToString();
    }
}
