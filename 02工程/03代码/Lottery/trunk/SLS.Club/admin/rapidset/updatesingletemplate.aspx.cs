using System;

using Discuz.Common;
using Discuz.Config;
using Discuz.Forum;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 单个生成模板页面
    /// </summary>
    public partial class updatesingletemplate : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int result = -1;
                string filename = DNTRequest.GetString("filename");
                string path = DNTRequest.GetString("path");

                int templateid = Convert.ToInt32(AdminTemplates.GetAllTemplateList(AppDomain.CurrentDomain.BaseDirectory + "templates/").Select("directory='" + path + "'")[0]["templateid"].ToString());

                if (filename != "")
                {
                    ForumPageTemplate forumpagetemplate = new ForumPageTemplate();
                    forumpagetemplate.GetTemplate("/",path, filename, 1, templateid);
                    result = 1;
                }

                Response.Write(result);
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.Expires = -1;
                Response.End();
            }
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