using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CPS_UserControls_Head : System.Web.UI.UserControl
{
    public string ImgUrl = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ImgUrl = "'" + ResolveUrl("~/Cps/Images/") + "'";
        }
    }
}
