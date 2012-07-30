using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Discuz.Forum;


namespace Discuz.Web.Admin
{
#if NET1
    public class editgroup : AdminPage
#else
    public partial class editgroup : AdminPage
#endif
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
