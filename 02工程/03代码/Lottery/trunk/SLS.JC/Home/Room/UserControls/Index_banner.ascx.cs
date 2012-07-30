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

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public partial class Home_Room_UserControls_Index_banner : UserControlBase
{
    public string imagsBanner = "";
    public string linksBanner = "";
    public string textBanner = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        string CacheKey = "Home_Room_UserControls_Index_banner_ImagePlay";

        DataTable dt = Shove._Web.Cache.GetCacheAsDataTable(CacheKey);

        if (dt == null)
        {
            dt = new DAL.Tables.T_FocusImageNews().Open("", "", "ID Desc");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.DataReadWrite, "数据库繁忙，请重试", this.GetType().BaseType.FullName);

                return;
            }

            Shove._Web.Cache.SetCache(CacheKey, dt, 1200);
        }

        DataRow[] drs = dt.Select("IsBig=1","ID desc");

        for (int i = 0; i < drs.Length && i < 6; i++)
        {
            imagsBanner += "Private/" + _Site.ID.ToString() + "/NewsImages/" + drs[i]["ImageUrl"].ToString() + ",";

            textBanner += drs[i]["Title"].ToString() + ",";

            linksBanner += drs[i]["Url"].ToString() + ",";
        }
    }
}
