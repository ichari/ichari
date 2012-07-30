using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.Common;
using System.Collections;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;


namespace Discuz.Web.Admin
{

#if NET1
    public class helplist : AdminPage
#else
    public partial class helplist : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button DelRec;
        protected Discuz.Control.Button Orderby;
        #endregion
#endif


        protected DataGrid DataGrid1;
        public IDataReader ddr;
        public ArrayList _arrayList=null;
        protected void Page_Load(object sender, EventArgs e)
        {
            _arrayList = Helps.GetHelpList();
        }

        private void DelRec_Click(object sender, EventArgs e)
        {
            #region 删除

            string idlist = DNTRequest.GetFormString("id");
   
            if (this.CheckCookie())
            {
                if (idlist != "")
                {
                    del(idlist);
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('您未选中任何选项');window.location.href='global_helplist.aspx';</script>");
                }
            }

            #endregion
        }

        protected void del(string idlist)
        {
            #region 删除帮助
            Helps.delhelp(idlist);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/helplist");
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除帮助", "删除帮助,帮助ID为: " + DNTRequest.GetString("id"));
            Response.Redirect("global_helplist.aspx");
            #endregion
        }

        private void Orderby_Click(object sender, EventArgs e)
        {
            #region 排序
            string[] orderlist = DNTRequest.GetFormString("orderbyid").Split(',');
            string[] idlist = DNTRequest.GetFormString("hidid").Split(',');
            foreach (string s in orderlist)
            {
                if (Utils.IsNumeric(s) == false)
                {
                    base.RegisterStartupScript("", "<script>alert('输入错误,排序号只能是数字');window.location.href='global_helplist.aspx';</script>");
                    return;

                }
            }

            for (int i = 0; i < idlist.Length; i++)
            {
                DatabaseProvider.GetInstance().UpOrder(orderlist[i].ToString(), idlist[i].ToString());

            }

            base.RegisterStartupScript("", "<script>window.location.href='global_helplist.aspx';</script>");
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
            this.DelRec.Click += new EventHandler(this.DelRec_Click);
            this.Orderby.Click += new EventHandler(this.Orderby_Click);
            this.Load += new EventHandler(this.Page_Load);
           
        }

        #endregion


    }
}
