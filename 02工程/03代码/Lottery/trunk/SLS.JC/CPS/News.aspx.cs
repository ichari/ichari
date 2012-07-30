using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text.RegularExpressions;

public partial class CPS_News : System.Web.UI.Page
{
    private string Type = "CPS新闻公告";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Shove._Web.Utility.GetRequest("TypeID") == "2")
            {
                Type = "CPS推广指南";
            }

            BindNews(Type);
        }
    }

    private void BindNews(string TypeName)
    {
        string CacheKey = "CPS_News_BindNews";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Views.V_News().Open("ID,Title,Content,TypeName", "isShow=1 and (TypeName='CPS新闻公告' or TypeName = 'CPS推广指南')", "");

            if (dt == null)
            {
                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 1200);
        }

        DataTable dtData = dt.Clone();

        DataRow[] drs = dt.Select("TypeName='"+TypeName+"'","ID desc");

        foreach (DataRow dr in drs)
        {
            dtData.Rows.Add(dr.ItemArray);
        }

        sdlNewsList.DataSource = dtData.DefaultView;
        sdlNewsList.DataBind();
    }

    protected void sdlNewsList_PageIndexChanged(object sender, EventArgs e)
    {
        BindNews(Type);
    }

    protected void sdlNewsList_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.EditItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataRow dr = drv.Row;

            HyperLink hl = (HyperLink)e.Item.FindControl("hlNews");

            string title = dr["Title"].ToString();
            bool IsClass = false;
            string color = "";

            if ((title.IndexOf("<font class=red12>") > -1 || title.IndexOf("<font class=black12>") > -1))
            {
                if (title.Contains("<font class=red12>"))
                {
                    color = "red";
                }
                if (title.Contains("<font class=black12>"))
                {
                    color = "black12";
                }

                Title = Title.Replace("<font class=red12>", "").Replace("<font class=black12>", "").Replace("</font>", "");

                IsClass = true;
            }

            if (IsClass)
            {
                hl.Text = "<font class='" + color + "'>" + Shove._String.HtmlTextCut(title, 20) + "</font>";
            }
            else
            {
                hl.Text = Shove._String.HtmlTextCut(title, 20);
            }

            Regex regex = new Regex(@"^http://([\w-]+\.)+[\w-]+(/[\w-./?%&=]*)?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            Match m = regex.Match(dr["Content"].ToString());

            if (m.Success)
            {
                hl.NavigateUrl = dr["Content"].ToString();
            }
            else
            {
                hl.NavigateUrl = "NewsShow.aspx?ID=" + dr["ID"].ToString();
            }
        }
    }
}
