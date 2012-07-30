using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Focus_Detailed : SitePageBase
{
    private long ID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!this.IsPostBack)
        {
            ID = Shove._Convert.StrToLong(Shove._Web.Utility.GetRequest("ID"), 1);

            DataTable dt = new DAL.Tables.T_FocusEvent().Open("[Content]", "ID=" + ID.ToString(), "");

            if (dt == null)
            {
                PF.GoError(ErrorNumber.NoData, "数据库繁忙，请重试", this.GetType().FullName);

                return;
            }

            if (dt.Rows.Count < 1)
            {
                return;
            }

            lbContent.Text = dt.Rows[0]["Content"].ToString();
        }
    }
}
