using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.UI.HtmlControls;
using System.Text;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using System.Data.Common;
namespace Discuz.Web.Admin
{
    public partial class searchuser : System.Web.UI.UserControl
    {
        public StringBuilder sb = new StringBuilder();

        public int menucount = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            //查找用户
            System.Data.DataSet dsSrc = new System.Data.DataSet();
            dsSrc.ReadXml(Page.Server.MapPath("xml/navmenu.config"));
            //得到顶部菜单相关全局项
            DataRow toptabmenudr = dsSrc.Tables["toptabmenu"].Rows[0];

            string searchinfo = DNTRequest.GetString("searchinf");
            if (searchinfo != "")
            {

                IDataReader idr = DatabaseProvider.GetInstance().GetUserInfoByName(searchinfo);
                int count = 0;
                bool isexist = false;

                sb.Append("<table width=\"100%\" style=\"align:center\"><tr>");
                while(idr.Read())
                {
                    //先找出子菜单表中的相关菜单
                    isexist = true;

                    if (count >= 3)
                    {
                        count = 0;
                        sb.Append("</tr><tr>");
                    }
                    count++;//javascript:resetindexmenu('7','3','7,8','global/global_usergrid.aspx');
                    sb.Append("<td width=\"33%\" style=\"align:left\"><a href=\"#\" onclick=\"javascript:resetindexmenu('7','3','7,8','global/global_edituser.aspx?uid=" + idr["uid"] + "');\">" + idr["username"].ToString().ToString() + "</a></td>");
                }
                idr.Close();
                if (!isexist)
                {
                    sb.Append("没有找到相匹配的结果");
                }
                sb.Append("</tr></table>");
            }
            else
            { 
                sb.Append("您未输入任何搜索关键字"); 
            }

            dsSrc.Dispose();
               
        }
    }
}