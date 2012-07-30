using System;
using System.Web.UI;
using System.Data;
using System.Xml;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;
using Discuz.Common.Xml;

using System.Text;

namespace Discuz.Web.Admin
{
#if NET1
    public class managemainmenu : AdminPage
    {
		protected Discuz.Control.DataGrid DataGrid1;
#else
    public partial class managemainmenu : AdminPage
	{
#endif
        private string configPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            configPath = Page.Server.MapPath("../xml/navmenu.config");
            string menuid = DNTRequest.GetString("menuid");
            string mode = DNTRequest.GetString("mode");
            if (mode != "")
            {
                if (mode == "del")
                {
                    MenuManage.DeleteMainMenu(int.Parse(menuid));
                }
                else
                {
                    if (menuid == "0")
                    {
                        MenuManage.NewMainMenu(DNTRequest.GetString("menutitle"), DNTRequest.GetString("defaulturl"));
                    }
                    else
                    {
                        MenuManage.EditMainMenu(int.Parse(menuid), DNTRequest.GetString("menutitle"), DNTRequest.GetString("defaulturl"));
                    }
                }
                Response.Redirect("managemainmenu.aspx", true);
            }
            else
            {
                BindDataGrid();
            }
        }

        /*private void SaveManagerBody()
        {
            string url = Context.Request.Url.ToString().ToLower().Substring(0, Context.Request.Url.ToString().ToLower().IndexOf("rapidset")) + "framepage/managerbody.aspx";
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            req.Method = "GET";
            WebResponse wr = req.GetResponse();
            StreamReader sr = new StreamReader(wr.GetResponseStream(), System.Text.Encoding.GetEncoding("gb2312"));
            string html = sr.ReadToEnd();
            FileStream fs = new FileStream(Utils.GetMapPath("../framepage/managerbody.htm"), FileMode.Create);
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(html);
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }*/

        private void BindDataGrid()
        {
            DataGrid1.TableHeaderName = "菜单管理";
            XmlDocumentExtender doc = new XmlDocumentExtender();
            doc.Load(configPath);
            XmlNodeList mainmenus = doc.SelectNodes("/dataset/toptabmenu");
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("id"));
            dt.Columns.Add(new DataColumn("title"));
            dt.Columns.Add(new DataColumn("defaulturl"));
            dt.Columns.Add(new DataColumn("system"));
            dt.Columns.Add(new DataColumn("delitem"));
            foreach (XmlNode menuitem in mainmenus)
            {
                DataRow dr = dt.NewRow();
                dr["id"] = menuitem["id"].InnerText;
                dr["title"] = menuitem["title"].InnerText;
                dr["defaulturl"] = menuitem["defaulturl"].InnerText;
                dr["system"] = menuitem["system"].InnerText != "0" ? "是" : "否";
                if (menuitem["mainmenulist"].InnerText != "")
                    dr["delitem"] = "删除";
                else
                    dr["delitem"] = "<a href='managemainmenu.aspx?mode=del&menuid=" + menuitem["id"].InnerText + "' onclick='return confirm(\"您确认要删除此菜单项吗?\")'>删除</a>";
                dt.Rows.Add(dr);
            }
            DataGrid1.DataSource = dt;
            DataGrid1.DataBind();
        }

        protected void createMenu_Click(object sender, EventArgs e)
        {
            MenuManage.CreateMenuJson();
        }

        

        #region Web Form Designer generated code
        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
            this.createMenu.Click += new EventHandler(this.createMenu_Click);
        }

        #endregion
    }
}
