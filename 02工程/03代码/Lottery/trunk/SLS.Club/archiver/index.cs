using System;
using System.Web;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Web.UI;


namespace Discuz.Web.Archiver
{
	/// <summary>
	/// 简洁版首页
	/// </summary>
	public partial class index : ArchiverPage
	{
		private string FORUM_LINK = "<a href=\"showforum-{0}{1}\">{2}</a>";
        public index()
		{
            if (config.Aspxrewrite != 1)
            {
                FORUM_LINK = "<a href=\"showforum{1}?forumid={0}\">{2}</a>";
            }

            ShowTitle(config.Forumtitle + " - 首页");
			ShowBody();
			HttpContext.Current.Response.Write("<h1>" + config.Forumtitle + "</h1>");
			HttpContext.Current.Response.Write("<div id=\"wrap\">");
			DataTable dt = Forums.GetArchiverForumIndexList(config.Hideprivate, usergroupinfo.Groupid);
			foreach(DataRow dr in dt.Rows)
			{
				if (dr["layer"].ToString() == "0")
				{
					HttpContext.Current.Response.Write("<div class=\"cateitem\"><h2>");
					HttpContext.Current.Response.Write(string.Format(FORUM_LINK, dr["fid"].ToString(), config.Extname, Utils.HtmlDecode(dr["name"].ToString().Trim())));
					HttpContext.Current.Response.Write("</h2></div>\r\n");
				}
				else
				{
					HttpContext.Current.Response.Write("<div class=\"forumitem\"><h3>");
					HttpContext.Current.Response.Write(Utils.GetSpacesString(Utils.StrToInt(dr["layer"].ToString(), 0)));
					HttpContext.Current.Response.Write(string.Format(FORUM_LINK, dr["fid"].ToString(), config.Extname, Utils.HtmlDecode(dr["name"].ToString().Trim())));
					HttpContext.Current.Response.Write("</h3></div>\r\n");
				}
			}
			HttpContext.Current.Response.Write("</div>");
			HttpContext.Current.Response.Write("<div class=\"fullversion\">查看完整版本: <a href=\"../index.aspx\">" + config.Forumtitle + "</a></div>\r\n");
			ShowFooter();
			HttpContext.Current.Response.End();

		}
	}
}
