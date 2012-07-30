using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{

    /// <summary>
    ///	下拉分表控件
    /// </summary>

#if NET1
    public class DropDownPost : UserControl
#else
    public partial class DropDownPost : UserControl
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DropDownList postslist;    
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region 初始化分表控件

                postslist.Items.Clear();
                foreach (DataRow r in DatabaseProvider.GetInstance().GetDatechTableIds())
                {
                    postslist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + r[0].ToString(), r[0].ToString()));
                }
                postslist.DataBind();
                postslist.SelectedValue = Posts.GetPostTableID();

                #endregion
            }
        }

        public string SelectedValue
        {
            get { return postslist.SelectedValue; }
            set { postslist.SelectedValue = value; }
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}