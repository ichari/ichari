using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;


namespace Discuz.Web.Admin
{

#if NET1
    public class edithelpclass : AdminPage
#else
    public partial class edithelpclass : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox title;
        protected Discuz.Control.TextBox orderby;
        protected Discuz.Control.Button updateclass;
        protected Discuz.Control.TextBox poster;
        #endregion
#endif

        public int id = DNTRequest.GetInt("id", 0);
        public HelpInfo helpinfo = new HelpInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                helpinfo = Helps.getmessage(id);

                if ((this.username != null) && (this.username != ""))
                {
                    if (id == 0)
                    {
                        return;
                    }
                    else
                    {
                        poster.Text = this.username;
                        orderby.Text = helpinfo.Orderby.ToString();
                        tbtitle.Text = helpinfo.Title;
                    }
                }
            }
        }

        protected void updateclass_Click(object sender, EventArgs e)
        {
            Helps.updatehelp(this.id, tbtitle.Text, "", 0, int.Parse(orderby.Text));
            Response.Redirect("global_helplist.aspx");
        }


        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.updateclass.Click += new EventHandler(this.updateclass_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion



    }
}
