using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using Discuz.Config;

using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 管理帖子列表
    /// </summary>

#if NET1
    public class postgridmanage : AdminPage
#else
    public partial class postgridmanage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.Button SetPostInfo;
        #endregion
#endif


        public string condition = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Discuz.Common.Utils.GetCookie("postswhere") != "")
                {
                    ViewState["condition"] = Discuz.Common.Utils.GetCookie("postswhere").ToString();
                    ViewState["fid"] = Discuz.Common.Utils.GetCookie("seachpost_fid").ToString();
                    if (Discuz.Common.Utils.GetCookie("posttablename") != "")
                    {
                        ViewState["posttablename"] = Discuz.Common.Utils.GetCookie("posttablename").ToString();
                    }
                    else
                    {
                        ViewState["posttablename"] = Posts.GetPostTableName();
                    }
                }
                else
                {
                    Response.Redirect("forum_searchpost.aspx");
                    return ;
                }
                BindData();
            }
        }

        public void BindData()
        {
            #region 数据绑定

            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "发帖列表";
            DataGrid1.BindData(DatabaseProvider.GetInstance().PostGridBind(ViewState["posttablename"].ToString(),ViewState["condition"].ToString()));

            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }


        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }


        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.Cells[2].Text.ToString().Length > 15)
            {
                e.Item.Cells[2].Text = e.Item.Cells[2].Text.Substring(0, 15) + "…";
            }

            #endregion
        }

        private void SetPostInfo_Click(object sender, EventArgs e)
        {
            #region 删除选中的帖子

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("pid") != "")
                {
                    string pidlist = DNTRequest.GetString("pid");
                    string posttableid = ViewState["posttablename"].ToString().Trim().Replace(BaseConfigs.GetTablePrefix + "posts", "");
                    posttableid = (posttableid == "" ? "1" : posttableid);
                    foreach (string idlist in DNTRequest.GetString("pid").Split(','))
                    {
                        int pid = int.Parse(idlist.Split('|')[0]);
                        int tid = int.Parse(idlist.Split('|')[1]);
                        
                        
                        #region 更新用户金币金币
                        PostInfo post = Posts.GetPostInfo(tid, pid);
                        ForumInfo forum = Forums.GetForumInfo(post.Fid);
                        int Losslessdel = Utils.StrDateDiffHours(post.Postdatetime, config.Losslessdel * 24);
                        

                        // 通过验证的用户可以删除帖子，如果是主题贴则另处理
                        if (post.Layer == 0)
                        {
                            TopicAdmins.DeleteTopics(tid.ToString(), byte.Parse(forum.Recyclebin.ToString()), false);
                            //重新统计论坛帖数
                            Forums.SetRealCurrentTopics(forum.Fid);
                        }
                        else
                        {
                            int reval = Posts.DeletePost(posttableid, Convert.ToInt32(pid), false,true);
                            if (reval > 0 && Losslessdel < 0)
                            {
                                UserCredits.UpdateUserCreditsByPosts(post.Posterid, -1);
                            }
                            // 删除主题游客缓存
                            ForumUtils.DeleteTopicCacheFile(tid.ToString());

                            //再次确保回复数精确
                            Topics.UpdateTopicReplies(tid);
                        }
                        #endregion
                    }

                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "批量删帖", "帖子ID:" + pidlist);

                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_searchpost.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_searchpost.aspx';</script>");
                }
            }

            #endregion
        }

        public string Invisible(string invisible)
        {
            if (invisible == "0")
            {
                return "<div align=center><img src=../images/OK.gif /></div>";
            }
            else
            {
                return "<div align=center><img src=../images/Cancel.gif /></div>";
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
            this.SetPostInfo.Click += new EventHandler(this.SetPostInfo_Click);
            DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.TableHeaderName = "发帖列表";
            DataGrid1.ColumnSpan = 8;
            DataGrid1.DataKeyField = "pid";
        }

        #endregion

    }
}