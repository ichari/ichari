using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Shove.Database;
using System.Data;

public partial class TrendCharts_Default : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private string BindNewsForLottery(int LotteryID, string TypeName)
    {
        if (TypeName == "福彩3D")
        {
            TypeName = "3D";
        }

        string Key ="Home_Room_Buy_BindNewsForLotterys" + LotteryID.ToString();

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(Key);

        if (dt == null)
        {
            string sql = "";

            sql = "select Top 9 Title,Content,DateTime,b.Name as TypeName from (select ID,Title,Content,TypeID,[DateTime],isCommend from dbo.T_News where isShow = 1 ) a "
                   + "inner join dbo.T_NewsTypes b "
                   + "on a.TypeID = b.ID "
                   + "where b.Name = @Name "
                   + "order by isCommend,DateTime desc";
            dt = Shove.Database.MSSQL.Select(sql,
                new Shove.Database.MSSQL.Parameter("Name", SqlDbType.VarChar, 0, ParameterDirection.Input, TypeName));

            if (dt == null || dt.Rows.Count == 0)
            {
                return "";
            }

            Shove._Web.Cache.SetCache(Key, dt, 6000);
        }

        StringBuilder sb = new StringBuilder();

        sb.Append("<table width='95%' border='0' cellspacing='0' cellpadding='0'>");

        string Title = "";
        string color = "";
        int i = 0;
        foreach (DataRow dr in dt.Rows)
        {
            i++;

            Title = dr["Title"].ToString().Trim();
            if (i <= 5)
            {
                if ((Title.IndexOf("<font class=red12>") > -1 || Title.IndexOf("<font class=black12>") > -1))
                {
                    if (Title.Contains("<font class=red12>"))
                    {
                        color = "red12";
                    }
                    if (Title.Contains("<font class=black12>"))
                    {
                        color = "black12";
                    }

                    Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                    sb.Append("<tr>")
                        .Append("<td width='2' height='28' align='left'>")
                        .Append("<span class='hui14'>&#9642;</span>")
                        .Append("</td>")
                        .Append("<td width='96%' align='left' class='blue12_3'>")
                        .Append("<a href='" + dr["Content"].ToString() + "' target='_blank'>")
                         .Append("<font class = '" + color + "'>")
                         .Append(Shove._String.Cut(Title, 15))
                         .Append("</font>")
                        .Append("</a></td></tr>");
                }
                else
                {
                    sb.Append("<tr>")
                       .Append("<td width='2' height='28' align='left'>")
                       .Append("<span class='hui14'>&#9642;</span>")
                       .Append("</td>")
                       .Append("<td width='96%' align='left' class='blue12_3'>")
                       .Append("<a href='" + dr["Content"].ToString() + "' target='_blank'>")
                        .Append(Shove._String.Cut(Title, 15))

                       .Append("</a></td></tr>");
                }
            }
            else
            {
                break;
            }
        }

        sb.Append("</table>");

        return sb.ToString();
    }
}
