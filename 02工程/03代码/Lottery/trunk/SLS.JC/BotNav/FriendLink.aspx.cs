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
using Shove.Database;
using System.Text;

public partial class BotNav_FriendLink : SitePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            GetFriendLinks();
        }
    }

    private void GetFriendLinks()
    {
        StringBuilder sb = new StringBuilder();

        string CacheKey = "T_FriendshipLinks_Links";
        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = MSSQL.Select("select LinkName,Url,LogoUrl from T_FriendshipLinks where SiteID = @SiteID order by [Order] asc ",
            new MSSQL.Parameter("SiteID", SqlDbType.Int, 0, ParameterDirection.Input, _Site.ID));
        }
        
        if (dt == null)
        {
            PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

            return ;
        }

        if (dt.Rows.Count > 0)
        {
            Shove._Web.Cache.SetCache("T_FriendshipLinks_Links", dt);
        }

        DataTable dtFriendLinksWithImg = dt.Clone();
        DataRow []drTmp =  dt.Select("LogoUrl <> ''");
        int length = drTmp.Length;
        for (int i = 0; i < length; i++)
        {
            dtFriendLinksWithImg.Rows.Add(drTmp[i].ItemArray);
        }

        dlFriendLinks.DataSource = dtFriendLinksWithImg;
        dlFriendLinks.DataBind();


        DataTable dtFriendLinksWithOutImg = dt.Clone();
        drTmp = dt.Select("LogoUrl = ''");
        length = drTmp.Length;
        for (int i = 0; i < length; i++)
        {
            dtFriendLinksWithOutImg.Rows.Add(drTmp[i].ItemArray);
        }
        dlFrindLinksNoImg.DataSource = dtFriendLinksWithOutImg;
        dlFrindLinksNoImg.DataBind();
    }
}
