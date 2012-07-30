using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加表情
    /// </summary>
    
#if NET1
    public class addsmile : AdminPage
#else
    public partial class addsmile : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.TextBox code;
        protected Discuz.Control.UpFile url;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button AddSmileInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                url.UpFilePath = Server.MapPath("../../editor/images/smilies/" + Smilies.GetSmilieTypeById(Request.QueryString["typeid"])["url"].ToString() + "/");
            }
        }

        private void AddSmileInfo_Click(object sender, EventArgs e)
        {
            #region 添加表情记录

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().AddSmiles(DatabaseProvider.GetInstance().GetMaxSmiliesId(), int.Parse(displayorder.Text), 
                    DNTRequest.GetInt("typeid", 0), code.Text,
                    Smilies.GetSmilieTypeById(DNTRequest.GetInt("typeid", 0).ToString())["url"].ToString() + "/" + url.UpdateFile());

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "表情文件添加", code.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';");

                //if (this.CheckCookie())
                //{
                //    string sqlstring = string.Format("INSERT INTO [" + BaseConfigFactory.GetTablePrefix + "smilies] (displayorder,type,code,url) Values ('{0}','{1}','{2}','{3}')",
                //                                     displayorder.Text,
                                                     
                //                                     code.Text,
                //                                     SmiliesFactory.GetSmilieTypeById(DNTRequest.GetInt("typeid", 0).ToString())["url"].ToString() + "/" + url.UpdateFile());
                //    AdminDatabase.ExecuteNonQuery(sqlstring);

                //    DatabaseProvider.GetInstance().AddSmiles(DatabaseProvider.GetInstance().GetMaxSmiliesId(),int.Parse(displayorder.Text),DNTRequest.GetInt("typeid", 0),code.Text,
                //        Smilies.GetSmilieTypeById(DNTRequest.GetInt("typeid", 0).ToString())["url"].ToString() + "/" + url.UpdateFile());

                //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesList");
                //    DNTCache.GetCacheService().RemoveObject("/Forum/UI/SmiliesListWithInfo");
                //    AdminVistLogFactory.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "表情文件添加", code.Text);
                //    Page.RegisterStartupScript("PAGE", "window.location.href='forum_smilegrid.aspx?typeid=" + DNTRequest.GetInt("typeid", 0) + "';");
                //}
            }

            #endregion
        }

        #region Web 窗体设计器生成的代码

        override protected void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.AddSmileInfo.Click += new EventHandler(this.AddSmileInfo_Click);
        }

        #endregion


    }
}