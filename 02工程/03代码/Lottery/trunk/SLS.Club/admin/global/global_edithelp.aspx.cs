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
    public class edithelp : AdminPage
#else
    public partial class edithelp : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox title;
        protected Discuz.Control.DropDownList type;
        protected Discuz.Control.TextBox orderby;
        protected Discuz.Web.Admin.OnlineEditor message;
        protected Discuz.Control.Button updatehelp;
        protected Discuz.Control.TextBox poster;
        #endregion
#endif


        public int id = DNTRequest.GetInt("id",0);
        public HelpInfo helpinfo = new HelpInfo();
                 
        protected void Page_Load(object sender, EventArgs e)
        {
            helpinfo = Helps.getmessage(id);
            if (Helps.choosepage(helpinfo.Pid) == true)
            {
                Response.Redirect("global_edithelpclass.aspx?id="+id);
                return;
            }
            if (!Page.IsPostBack)
            {
                if ((this.username != null) && (this.username != ""))
                {
                    if (id == 0)
                    {
                        return;
                    }
                    else
                    {
                        poster.Text = this.username;
                        type.AddTableData(Helps.bindhelptype());
                        type.SelectedValue = helpinfo.Pid.ToString();
                        orderby.Text = helpinfo.Orderby.ToString();
                        tbtitle.Text = helpinfo.Title;
                        message.Text = helpinfo.Message;
                        updatehelp.ValidateForm = true;
                        tbtitle.AddAttributes("maxlength", "200");
                        tbtitle.AddAttributes("rows", "2");
                        type.DataBind();
                    }
                }
            }
        }

        protected void updatehelp_Click(object sender, EventArgs e)
        {
            Helps.updatehelp(this.id, tbtitle.Text, message.Text, int.Parse(type.SelectedValue), int.Parse(orderby.Text));
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
            this.updatehelp.Click += new EventHandler(this.updatehelp_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

     


    }
}
