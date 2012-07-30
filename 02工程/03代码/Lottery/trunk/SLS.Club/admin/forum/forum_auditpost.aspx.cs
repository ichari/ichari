using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;

using Discuz.Common;
using Discuz.Entity;
using Discuz.Forum;
using Discuz.Data;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using DropDownList = Discuz.Control.DropDownList;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 审核帖子
    /// </summary>
     
#if NET1
    public class auditpost : AdminPage
#else
    public partial class auditpost : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.DropDownList postlist;
        protected Discuz.Control.Button SelectPass;
        protected Discuz.Control.Button SelectDelete;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Web.Admin.ajaxpostinfo AjaxPostInfo1;

        protected System.Web.UI.WebControls.Literal msg;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (DatabaseProvider.GetInstance().GetCurrentPostTableRecordCount(int.Parse(postlist.SelectedValue)) == 0)
                {
                    msg.Visible = true;
                }
                BindData();
            }
        }

        public void BindData()
        {
            #region 绑定审核帖子
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "审核帖子列表";
            DataGrid1.DataKeyField = "pid";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetUnauditPostSQL(int.Parse(postlist.SelectedValue)));
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

        private void postslist_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }

        public void initPostTable()
        {
            #region 初始化分表控件

            postlist.AutoPostBack = true;

            //DataTable dt = DbHelper.ExecuteDataset("SELECT ID FROM [" + BaseConfigs.GetTablePrefix + "tablelist] Order BY ID ASC").Tables[0];
            DataTable dt = DatabaseProvider.GetInstance().GetDetachTableId();
            postlist.Items.Clear();
            foreach (DataRow r in dt.Rows)
            {
                postlist.Items.Add(new ListItem(BaseConfigs.GetTablePrefix + "posts" + r[0].ToString(), r[0].ToString()));
            }
            postlist.DataBind();
            postlist.SelectedValue = Posts.GetPostTableID();

            #endregion
        }

        private void SelectPass_Click(object sender, EventArgs e)
        {
            #region 对选中的帖子设置为通过审核

            string idlist = DNTRequest.GetString("pid");
            string pidlist = "";
            string tidlist = "";
            foreach (string doubleid in idlist.Split(','))
            {
                string[] idarray = doubleid.Split('|');
                pidlist += idarray[0] + ",";
                tidlist += idarray[1] + ",";
            }
            pidlist = pidlist.TrimEnd(',');
            tidlist = tidlist.TrimEnd(',');
            if (this.CheckCookie())
            {
                if (pidlist != "")
                {
                    UpdateUserCredits(tidlist, pidlist);
                    DatabaseProvider.GetInstance().PassPost(int.Parse(postlist.SelectedValue), pidlist);
                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_auditpost.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
                }
            }

            #endregion
        }

        /// <summary>
        /// 更新用户金币
        /// </summary>
        /// <param name="postTableId">分表id</param>
        /// <param name="pidlist">通过审核帖子的Pid列表</param>
        private void UpdateUserCredits(string tidlist, string pidlist)
        {
            string[] tidarray = tidlist.Split(',');
            string[] pidarray = pidlist.Split(',');
            float[] values = null;
            ForumInfo forum = null;
            PostInfo post = null;
            int fid = -1;
            for(int i = 0; i < pidarray.Length; i++)
            {
                //Topics.get
                post = Posts.GetPostInfo(int.Parse(tidarray[i]), int.Parse(pidarray[i]));  //获取帖子信息
                if (fid != post.Fid)    //当上一个和当前主题不在一个版块内时，重新读取版块的金币设置
                {
                    fid = post.Fid;
                    forum = Forums.GetForumInfo(fid);
                    if (!forum.Replycredits.Equals(""))
                    {
                        int index = 0;
                        float tempval = 0;
                        values = new float[8];
                        foreach (string ext in Utils.SplitString(forum.Replycredits, ","))
                        {

                            if (index == 0)
                            {
                                if (!ext.Equals("True"))
                                {
                                    values = null;
                                    break;
                                }
                                index++;
                                continue;
                            }
                            tempval = Utils.StrToFloat(ext, 0.0f);
                            values[index - 1] = tempval;
                            index++;
                            if (index > 8)
                            {
                                break;
                            }
                        }
                    }
                }

                if (values != null)
                {
                    ///使用版块内金币
                    UserCredits.UpdateUserCreditsByPosts(post.Posterid, values);
                }
                else
                {
                    ///使用默认金币
                    UserCredits.UpdateUserCreditsByPosts(post.Posterid);
                }
            }
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region 对选中的帖子进行删除

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("pid") != "")
                {
                    DataTable dt = new DataTable();
                    foreach (string pid in DNTRequest.GetString("pid").Split(','))
                    {
                        if (pid.Trim() != "")
                        {
                            //dt = DbHelper.ExecuteDataset("SELECT TOP 1 * [layer],[tid]  FROM [" + BaseConfigs.GetTablePrefix + "posts" + postlist.SelectedValue + "] WHERE [pid]=" + pid).Tables[0];
                            dt = DatabaseProvider.GetInstance().GetPostLayer(int.Parse(postlist.SelectedValue), int.Parse(pid));
                            if (dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["layer"].ToString().Trim() == "0")
                                {
                                    TopicAdmins.DeleteTopics(dt.Rows[0]["tid"].ToString(), false);
                                }
                                else
                                {
                                    Posts.DeletePost(postlist.SelectedValue, Convert.ToInt32(pid), false,false);
                                }
                            }
                        }
                    }
                    base.RegisterStartupScript( "PAGE", "window.location.href='forum_auditpost.aspx';");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_auditpost.aspx';</script>");
                }
            }

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
            this.postlist.SelectedIndexChanged += new EventHandler(this.postslist_SelectedIndexChanged);
            this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);

            this.Load += new EventHandler(this.Page_Load);

            DataGrid1.DataKeyField = "pid";
            DataGrid1.TableHeaderName = "审核帖子列表";
            DataGrid1.ColumnSpan = 7;

            initPostTable();
        }

        #endregion

    }
}