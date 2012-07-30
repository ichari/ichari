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
using Discuz.Data;
using Discuz.Entity;
using Discuz.Common;


namespace Discuz.Web.Admin
{

#if NET1
    public class dboptimize : AdminPage
#else
    public partial class dboptimize : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button Yh;
        #endregion
#endif
        public System.Collections.ArrayList FreeSpace;
        public void Page_Load(object sender, EventArgs e)
        {
            if (DbHelper.Provider.IsDbOptimize() == true)
            {
                FreeSpace = DatabaseProvider.GetInstance().CheckDbFree();
                DataGrid1.DataSource = FreeSpace;
                DataGrid1.DataKeyField = "index";
                DataGrid1.AllowCustomPaging = false;
                DataGrid1.TableHeaderName = "碎片优化";
                DataGrid1.DataBind();
            }
            else
            {
                Response.Write("<script>alert('您所使用的数据库不支持优化!');</script>");
                Response.Write("<script>history.go(-1)</script>");
                Response.End();
            }
        }

        private void Yh_Click(object sender, EventArgs e)
        {
            #region 优化数据库
            string tablelist = DNTRequest.GetFormString("tablename");
            DatabaseProvider.GetInstance().DbOptimize(tablelist);
            base.RegisterStartupScript( "PAGE",  "window.location.href='global_dboptimize.aspx';");
            #endregion
        }

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {

            // this.Load += new EventHandler(this.Page_Load);
            this.Yh.Click += new EventHandler(this.Yh_Click);
        }

        #endregion

    }
}
