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
    /// 公告编辑
    /// </summary>
    public partial class editannounce : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DNTRequest.GetString("id") == "")
                {
                    Response.Redirect("global_announcegrid.aspx");
                }
                else
                {
                    LoadAnnounceInf(DNTRequest.GetInt("id", -1));
                    UpdateAnnounceInfo.ValidateForm = true;
                    tbtitle.AddAttributes("maxlength", "200");
                    tbtitle.AddAttributes("rows", "2");
                }
            }
        }

        public void LoadAnnounceInf(int id)
        {
            #region 装载公告信息

            DataTable dt = DatabaseProvider.GetInstance().GetAnnouncement(id);
            if (dt.Rows.Count > 0)
            {
                displayorder.Text = dt.Rows[0]["displayorder"].ToString();
                tbtitle.Text = dt.Rows[0]["title"].ToString();
                poster.Text = dt.Rows[0]["poster"].ToString();
                starttime.Text = Utils.GetStandardDateTime(dt.Rows[0]["starttime"].ToString());
                endtime.Text = Utils.GetStandardDateTime(dt.Rows[0]["endtime"].ToString());
                message.Text = dt.Rows[0]["message"].ToString().Trim();
            }

            #endregion
        }

        private void UpdateAnnounceInfo_Click(object sender, EventArgs e)
        {
            #region 更新公告相关信息

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().UpdateAnnouncement(DNTRequest.GetInt("id", 0), poster.Text, tbtitle.Text, Utils.StrToInt(displayorder.Text, 0), starttime.Text, endtime.Text, message.Text); 
                //移除公告缓存
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");

                //记录日志
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "更新公告", "更新公告,标题为:" + tbtitle.Text);

                base.RegisterStartupScript( "PAGE", "window.location.href='global_announcegrid.aspx';");
            }

            #endregion
        }

        private void DeleteAnnounce_Click(object sender, EventArgs e)
        {
            #region 删除公告

            if (this.CheckCookie())
            {
                DatabaseProvider.GetInstance().DeleteAnnouncement(DNTRequest.GetInt("id", 0));
                //移除公告缓存
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/AnnouncementList");

                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/SimplifiedAnnouncementList");

                //记录日志
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除公告", "删除公告,标题为:" + tbtitle.Text);

                base.RegisterStartupScript( "PAGE", "window.location.href='global_announcegrid.aspx';");
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
            this.UpdateAnnounceInfo.Click += new EventHandler(this.UpdateAnnounceInfo_Click);
            this.DeleteAnnounce.Click += new EventHandler(this.DeleteAnnounce_Click);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion


    }
}