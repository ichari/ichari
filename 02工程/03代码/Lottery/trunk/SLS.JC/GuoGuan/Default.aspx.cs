using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Shove;
using System.Data;
using System.Web.UI.HtmlControls;

public partial class GuoGuan_SFC_Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        ((HtmlInputHidden)WebHead1.FindControl("currentMenu")).Value = "mStat";
        if (!this.IsPostBack)
        {
            DataTable dt = Shove.Database.MSSQL.Select("select * from T_BuyWays");
            for (int i = 1; i < 16; i++)
            {
                ddl_Day.Items.Add(DateTime.Now.AddDays(i * -1).ToString("yyyy-MM-dd"));
            }
        }
    }
}
