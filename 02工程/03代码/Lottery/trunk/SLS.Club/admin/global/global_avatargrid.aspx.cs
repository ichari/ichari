using System;
using System.IO;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;

using Button = Discuz.Control.Button;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 用户头像列表
    /// </summary>
    
#if NET1
    public class avatargrid : AdminPage
#else
    public partial class avatargrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.WebControls.Literal vatarshow;

        protected Discuz.Control.Button UpdateAvatarCache;
        protected Discuz.Control.Button DeleteAvatar;
        #endregion
#endif


        public string avatar;
        public DataTable avatarfilelist;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadAvatarInfo();
            }
        }


        public void LoadAvatarInfo()
        {
            #region 加载头像数据

            avatarfilelist = new DataTable("avatarlist");
            avatarfilelist.Columns.Add("filenamepath", Type.GetType("System.String"));
            avatarfilelist.Columns.Add("filename", Type.GetType("System.String"));
            avatarfilelist.Columns.Add("_id", Type.GetType("System.Int32"));

            DirectoryInfo dirinfo = new DirectoryInfo(Server.MapPath("../../avatars/common/"));
            int i = 1;
            foreach (FileSystemInfo file in dirinfo.GetFileSystemInfos())
            {
                if (file != null)
                {
                    string extname = file.Extension.ToLower();

                    if (extname.Equals(".jpg") || extname.Equals(".gif") || extname.Equals(".png"))
                    {
                        DataRow dr = avatarfilelist.NewRow();
                        if (DNTRequest.GetString("path") == "1")
                        {
                            dr["filename"] = file.Name.Split('.')[0];
                        }
                        else
                        {
                            dr["filename"] = file.Name;
                        }
                        dr["filenamepath"] = "avatars\\common\\" + file.Name;
                        dr["_id"] = i;
                        i++;
                        avatarfilelist.Rows.Add(dr);
                    }
                }
            }

            foreach (DataRow avatarfile in avatarfilelist.Rows)
            {
                vatarshow.Text += "			<td width=\"25%\" height=\"130px\" align=\"center\"><img width=100 height=100 src=\"../../" + avatarfile["filenamepath"].ToString().Trim() + "\" title=\"" + avatarfile["filename"].ToString() + "\" \r\n";
                if (avatarfile["filename"].ToString().Trim() == "")
                {
                    vatarshow.Text += " style=\"border-style:dashed;border-width:2px;border-color:#FF0000\"\r\n";
                }
                vatarshow.Text += "			/><br />\r\n";
                vatarshow.Text += "			<input id=\"id\" onclick=\"checkedEnabledButton(this.form,'id','DeleteAvatar')\" type=\"checkbox\" value=\"" + avatarfile["filenamepath"].ToString() + "\" name=\"id\"\r\n";
                vatarshow.Text += "			   />&nbsp;<font style=\"font-size:12px\">" + avatarfile["filename"].ToString() + "</font></td>\r\n";
                if (Utils.StrToInt(avatarfile["_id"].ToString().Trim(), 0) % 4 == 0)
                {
                    vatarshow.Text += "			  </tr>\r\n";
                    vatarshow.Text += "			  <tr>\r\n";
                }
            }

            #endregion
        }

        private void DeleteAvatar_Click(object sender, EventArgs e)
        {
            #region 删除选中的头像

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    foreach (string filepathname in DNTRequest.GetString("id").Split(','))
                    {
                        if (Utils.FileExists(Utils.GetMapPath(@"..\..\" + filepathname)))
                        {
                            File.Delete(Utils.GetMapPath(@"..\..\" + filepathname));
                        }
                    }

                    Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                    cache.RemoveObject("/Forum/CommonAvatarList");

                    base.RegisterStartupScript( "PAGE", "window.location.href='global_avatargrid.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');</script>");
                }
            }

            #endregion
        }

        private void UpdateAvatarCache_Click(object sender, EventArgs e)
        {
            #region 更新头像缓存

            if (this.CheckCookie())
            {
                Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
                cache.RemoveObject("/Forum/CommonAvatarList");
                base.RegisterStartupScript( "PAGE", "window.location.href='global_avatargrid.aspx';");
            }

            #endregion
        }

        #region 把VIEWSTATE写入容器

        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.DiscuzForumSavePageState(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            return base.DiscuzForumLoadPageState();
        }

        #endregion

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.UpdateAvatarCache.Click += new EventHandler(this.UpdateAvatarCache_Click);
            this.DeleteAvatar.Click += new EventHandler(this.DeleteAvatar_Click);
            //this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}