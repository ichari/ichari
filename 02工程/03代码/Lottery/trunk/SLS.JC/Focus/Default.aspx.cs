using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Data;
using Shove.Database;

public partial class Focus_Default : SitePageBase
{
    private int Month = DateTime.Now.Month;
    protected static string innerHtml = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            Month = Shove._Convert.StrToInt(Shove._Web.Utility.GetRequest("Month"), DateTime.Now.Month);

            BindDataForYear();
            DataBind();
        }
    }

    private void BindDataForYear()
    {
        ddlYear.Items.Clear();

        DateTime dt = DateTime.Now;
        int Year = dt.Year;
        int Month = dt.Month;

        for (int i = PF.SystemStartYear; i <= Year; i++)
        {
            ddlYear.Items.Add(new ListItem(i.ToString() + "年", i.ToString()));
        }

        ddlYear.SelectedIndex = ddlYear.Items.Count - 1;
    }

    private void DataBind()
    {
        string CacheKey = "Focus_Default" + "_Year_" + ddlYear.SelectedValue + "_Month_" + Month.ToString();
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);
        if (dt == null)
        {
            dt = new DAL.Tables.T_FocusEvent().Open("ID, Title, [Content], ImageUrl", "Year=" + ddlYear.SelectedValue + " and Month = " + Month.ToString() + " and IsShow=1", "ID");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 1200);
        }

        StringBuilder sb = new StringBuilder();

        if (dt.Rows.Count > 1)
        {

            sb.Append("<div class=\"BigImgBox\">");
            sb.Append("<div class=\"BigImg\">");
            sb.Append("<a href=\"Detailed.aspx?ID=" + dt.Rows[0]["ID"].ToString() + "\" target=\"_blank\">");
            sb.Append("<img src=\"/Private/1/NewsImages/" + dt.Rows[0]["ImageUrl"].ToString() + "\" width=\"300px\" height=\"218\"></a></div>");
            sb.Append("<a href=\"Detailed.aspx?ID=" + dt.Rows[0]["ID"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dt.Rows[0]["Title"].ToString(), 22) + "</a>");
            sb.Append("</div>");

            for (int i = 1; i < dt.Rows.Count; i++)
            {
                sb.Append("<div class=\"SmallImgBox\">");
                sb.Append("<div class=\"SmallImg\">");
                sb.Append("<a href=\"Detailed.aspx?ID=" + dt.Rows[i]["ID"].ToString() + "\" target=\"_blank\">");
                sb.Append("<img src=\"/Private/1/NewsImages/" + dt.Rows[i]["ImageUrl"].ToString() + "\" width=\"135\" height=\"90\"></a></div>");
                sb.Append("<a href=\"Detailed.aspx?ID=" + dt.Rows[i]["ID"].ToString() + "\" target=\"_blank\">" + Shove._String.Cut(dt.Rows[i]["Title"].ToString(), 11) + "</a>");
                sb.Append("</div>");
            }

            lbContent.Text = sb.ToString();

            sb.Remove(0, sb.Length);

            sb.Append("&nbsp;");
        }
        else
        {
            lbContent.Text = "";
        }
        for (int i = 1; i < 13; i++)
        {
            sb.Append("&nbsp;<span ");

            if (i == Month)
            {
                sb.Append("class=\"month2\"");
            }

            sb.Append(">");

            if ((ddlYear.SelectedValue.Equals(DateTime.Now.Year.ToString()) && i <= Month) || !ddlYear.SelectedValue.Equals(DateTime.Now.Year.ToString()))
            {
                sb.Append("<a href=\"?Month=" + i.ToString() + "\">");
            }
            else
            {
                sb.Append("<a href=\"?Month=" + i.ToString() + "\">");
            }

            sb.Append(i.ToString() + "月</a></span>&nbsp;");

            if (i != 12)
            {
                sb.Append("|");
            }
        }
        innerHtml = sb.ToString();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataBind();
    }
}
