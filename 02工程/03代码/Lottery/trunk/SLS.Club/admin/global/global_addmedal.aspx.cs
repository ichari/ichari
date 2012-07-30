using System;
using System.Data;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加勋章信息
    /// </summary>
    
#if NET1
    public class addmedal : AdminPage
#else
    public partial class addmedal : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox name;
        protected Discuz.Control.RadioButtonList available;
        protected Discuz.Control.UpFile image;
        protected Discuz.Control.Button AddMedalInfo;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetMedal();
                if (dt.Rows.Count >= 100)
                {
                    base.RegisterStartupScript( "", "<script>alert('勋章列表记录已经达到99枚,因此系统不再允许添加勋章');window.location.href='global_medalgrid.aspx';</script>");
                    return;
                }
                string path = Utils.GetMapPath("../../images/medals");
                image.UpFilePath = path;
            }
        }

        public void AddMedalInfo_Click(object sender, EventArgs e)
        {
            #region 添加勋章节

            if (this.CheckCookie())
            {
                if (image.Text == "")
                {
                    base.RegisterStartupScript( "", "<script>alert('上传图片不能为空');</script>");
                    return;
                }
                //string sqlstring = string.Format("INSERT INTO [" + BaseConfigs.GetTablePrefix + "medals] (medalid,name,available,image) Values ('{0}','{1}','{2}','{3}')",
                //                                 Convert.ToString(Convert.ToInt32(DbHelper.SelectMaxID("" + BaseConfigs.GetTablePrefix + "medals", "medalid")) + 1),
                //                                 name.Text,
                //                                 available.SelectedValue,
                //                                 image.Text);
                //DbHelper.ExecuteNonQuery(sqlstring);
                DatabaseProvider.GetInstance().AddMedal(
                     DatabaseProvider.GetInstance().GetMaxMedalId(),
                     name.Text,
                     int.Parse(available.SelectedValue),
                     image.Text);
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/UI/MedalsList");
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "勋章文件添加", name.Text);
                base.RegisterStartupScript( "PAGE", "window.location.href='global_medalgrid.aspx';");
            }

            #endregion
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