using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using RadioButtonList = Discuz.Control.RadioButtonList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;

using Discuz.Cache;

namespace Discuz.Web.Admin
{

#if NET1
    public class topictypesgrid : AdminPage
#else
    public partial class topictypesgrid : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.Button delButton;
        protected Discuz.Control.DataGrid DataGrid1;
        protected Discuz.Control.TextBox typename;
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.TextBox description;
        protected Discuz.Control.Button AddNewRec;
		protected Discuz.Control.Button SaveTopicType;
        #endregion
#endif
        private DataTable ForumNameIncludeTopicType;
        private void Page_Load(object sender, System.EventArgs e)
        {
            ForumNameIncludeTopicType = DatabaseProvider.GetInstance().GetForumNameIncludeTopicType();
            // 在此处放置用户代码以初始化页面
            if (!Page.IsPostBack)
            {
                BindData();	//绑定主题分类
            }
        }

        /// <summary>
        /// 绑定主题
        /// </summary>
        public void BindData()
        {
            #region 绑定主题
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "主题分类";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetTopicTypes());
            #endregion
        }

        /// <summary>
        /// 翻页
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            #region 返回主题的序号
            foreach (DataRow dr in topicTypes.Rows)
            {
                if (dr["name"].ToString().Trim() == topicTypeName.Trim())
                {
                    return int.Parse(dr["displayorder"].ToString());
                }
            }
            return -1;
            #endregion
        }

        private string GetTopicTypeString(string topicTypes, string topicName)
        {
            #region 返回主题是否在其内
            foreach (string type in topicTypes.Split('|'))
            {
                if (type.IndexOf("," + topicName.Trim() + ",") != -1)
                    return type;
            }
            return "";
            #endregion
        }


        /// <summary>
        /// 增加新主题分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddNewRec_Click(object sender, EventArgs e)
        {
            #region 增加新主题分类
            //检查输入是否合法
            if (!CheckValue(typename.Text, displayorder.Text, description.Text)) return;

            //检查是否有同名分类存在
            if(DatabaseProvider.GetInstance().IsExistTopicType(typename.Text))
            {
                base.RegisterStartupScript( "", "<script>alert('数据库中已存在相同的主题分类名称');window.location.href='forum_topictypesgrid.aspx';</script>");
                return;
            }

            //增加分类到dnt_topictypes,并写日志
            DatabaseProvider.GetInstance().AddTopicTypes(typename.Text, int.Parse(displayorder.Text), description.Text);
            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加主题分类", "添加主题分类,名称为:" + typename.Text);

            //更新分类缓存
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/TopicTypes");
            base.RegisterStartupScript("", "<script>window.location.href='forum_topictypesgrid.aspx';</script>");
            return;
            #endregion
        }

        /// <summary>
        /// 删除主题分类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void delButton_Click(object sender, EventArgs e)
        {
            #region 删除主题分类
            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("id") != "")
                {
                    //取得要删除的ID列表，以“，”分隔
                    string idlist = DNTRequest.GetString("id");

                    //调用更新版块的方法
                    DeleteForumTypes(idlist);

                    //从主题分类表(dnt_topictypes)中删除相应的分类并写日志
                    DatabaseProvider.GetInstance().DeleteTopicTypesByTypeidlist(idlist);
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "删除主题分类", "删除主题分类,ID为:" + DNTRequest.GetString("id").Replace("0 ", ""));

                    //更新主题分类缓存
                    DNTCache cache = DNTCache.GetCacheService();
                    cache.RemoveObject("/Forum/TopicTypes");
                    Response.Redirect("forum_topictypesgrid.aspx");
                }
                else
                {
                    base.RegisterStartupScript( "", "<script>alert('您未选中任何选项');window.location.href='forum_attachtypesgrid.aspx';</script>");
                }
            }
            #endregion
        }

        /// <summary>
        /// 检查参数是否合法
        /// </summary>
        /// <param name="typename">主题分类名称</param>
        /// <param name="displayorder">排序次序</param>
        /// <param name="description">描述</param>
        /// <returns>合法返回treu，否则返回false</returns>
        private bool CheckValue(string typename, string displayorder, string description)
        {
            #region 检查参数是否合法
            if (typename == "" || typename.Length > 100 )
            {
                base.RegisterStartupScript("", "<script>alert('主题分类名称不能为空');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if ((displayorder == "") || (Convert.ToInt32(displayorder) < 0))
            {
                base.RegisterStartupScript("", "<script>alert('显示顺序不能为空 ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }

            if (description.Length > 500)
            {
                base.RegisterStartupScript("", "<script>alert('描述不能长于500个符');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            if( typename.IndexOf("|") > 0 )
            {
                base.RegisterStartupScript("", "<script>alert('不能含有非法字符 | ');window.location.href='forum_topictypesgrid.aspx';</script>");
                return false;
            }
            return true;
            #endregion
        }

        /// <summary>
        /// 前台绑定“关联论坛”的方法
        /// </summary>
        /// <param name="id">主题分类的ID</param>
        /// <returns>返回关联论坛的链接字符串</returns>
        public string LinkForum(string id)
        {
            #region 返回主题分类绑定的论坛名称
            string str = "";

            //处理每个论坛版块
            foreach (DataRow dr in ForumNameIncludeTopicType.Rows)
            {
                //将主题分类列表用间隔符“|”切开
                foreach (string type in dr["topictypes"].ToString().Split('|'))
                {
                    //查找主题ID（Id+","的做法是为了方便匹配，因为主题ID在版块中保存在每一项的开始，如果等于0，就说明找到了，小于0表示未找到，大于0表示并非主题ID）
                    if (type.IndexOf(id + ",") == 0)
                    {
                        //形成字符串
                        str += "<a href='/showforum.aspx?forumid=" + dr["fid"] + "&typeid=" + id + "&search=1' target='_blank'>" + dr["name"].ToString().Trim() + "</a>";
                        str += "[<a href='forum_editforums.aspx?fid=" + dr["fid"] + "&tabindex=4'>编辑</a>],";
                        //每一个主题分类只能存在于一个版块中，找到后就不必再向下查找，所以跳出本版块，接着查找下一版块
                        break;
                    }
                }
            }

            //如果有str不为空说明有包含该主题ID的版块，所以去掉最后的一个“,”
            if (str != "")
                return str.Substring(0, str.Length - 1);
            else
                return "";
            #endregion
        }

        /// <summary>
        /// 删除版块中的主题分类
        /// </summary>
        /// <param name="idlist">要删除主题分类的ID列表</param>
        private void DeleteForumTypes(string idlist)
        {
            #region 删除所选的主题分类
            //取得ID的数组
            string[] ids = idlist.Split(',');

            //取得主题分类的缓存
#if NET1            
            System.Collections.SortedList __topictypearray = new SortedList();
#else
            Discuz.Common.Generic.SortedList<int, object> __topictypearray = new Discuz.Common.Generic.SortedList<int, object>();
#endif
            __topictypearray = Caches.GetTopicTypeArray();

            //取得版块的fid,topictypes字段
            DataTable dt = DatabaseProvider.GetInstance().GetForumTopicType();

            //处理每一个版块
            foreach (DataRow dr in dt.Rows)
            {
                //如果版块的主题分类字段为空（topictypes==""），则处理下一个
                if (dr["topictypes"].ToString() == "") continue;

                string topictypes = dr["topictypes"].ToString();
                //处理每一个要删除的ID
                foreach (string id in ids)
                {
                    //将删除的ID拼成相应的格式串后，将原来的剔除掉，形成一个新的主题分类的字段
                    topictypes = topictypes.Replace(id + "," + __topictypearray[Int32.Parse(id)].ToString() + ",0|", "");
                    topictypes = topictypes.Replace(id + "," + __topictypearray[Int32.Parse(id)].ToString() + ",1|", "");
                    //将帖子列表（dnt_topics）中typeid为当前要删除的Id更新为0
                    DatabaseProvider.GetInstance().ClearTopicTopicType(int.Parse(id));
                }
                //用剔除了要删除的主题ID的主题列表值更新数据库
                DatabaseProvider.GetInstance().UpdateTopicTypeForForum(topictypes, int.Parse(dr["fid"].ToString()));
            }
            #endregion
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 设置编辑框宽度
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[1].Controls[0]).Width = 150;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0]).Width = 30;
                ((System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0]).Width = 250;
            }
            #endregion
        }

        private void SaveTopicType_Click(object sender, EventArgs e)
        {
            #region 保存主题分类编辑
            //下四行取编辑行的更新值
            int rowid = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                string id = o.ToString();
                string name = DataGrid1.GetControlValue(rowid, "name");
                string displayorder = DataGrid1.GetControlValue(rowid, "displayorder");
                string description = DataGrid1.GetControlValue(rowid, "description");

                //检查参数的合法性
                //if (!CheckValue(name, displayorder, description)) return;

                //判断主题分类表中是否有与要更新的重名
                if (!CheckValue(name, displayorder, description) || DatabaseProvider.GetInstance().IsExistTopicType(name, int.Parse(id)))
                {
                    //base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的主题分类名称');window.location.href='forum_topictypesgrid.aspx';</script>");
                    //return;
                    error = true;
                    continue;
                }

                //取得主题分类的缓存
#if NET1
            System.Collections.SortedList __topictypearray = new SortedList();
#else
                Discuz.Common.Generic.SortedList<int, object> __topictypearray = new Discuz.Common.Generic.SortedList<int, object>();
#endif
                __topictypearray = Caches.GetTopicTypeArray();

                DataTable dt = DatabaseProvider.GetInstance().GetExistTopicTypeOfForum();
                DataTable topicTypes = DbHelper.ExecuteDataset(DatabaseProvider.GetInstance().GetTopicTypes()).Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    //用新名更新dnt_forumfields表的topictypes字段
                    string topictypes = dr["topictypes"].ToString();
                    if (topictypes.Trim() == "")    //如果主题列表为空则不处理
                        continue;
                    string oldTopicType = GetTopicTypeString(topictypes, __topictypearray[Int32.Parse(id)].ToString().Trim()); //获取修改名字前的旧主题列表
                    if (oldTopicType == "")    //如果主题列表中不包含当前要修改的主题，则不处理
                        continue;
                    string newTopicType = oldTopicType.Replace("," + __topictypearray[Int32.Parse(id)].ToString().Trim() + ",", "," + name + ",");
                    topictypes = topictypes.Replace(oldTopicType + "|", ""); //将旧的主题列表从论坛主题列表中删除
                    ArrayList topictypesal = new ArrayList();
                    foreach (string topictype in topictypes.Split('|'))
                    {
                        if (topictype != "")
                            topictypesal.Add(topictype);
                    }
                    bool isInsert = false;
                    for (int i = 0; i < topictypesal.Count; i++)
                    {
                        int curDisplayOrder = GetDisplayOrder(topictypesal[i].ToString().Split(',')[1], topicTypes);
                        if (curDisplayOrder > int.Parse(displayorder))
                        {
                            topictypesal.Insert(i, newTopicType);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        topictypesal.Add(newTopicType);
                    }
                    topictypes = "";
                    foreach (object t in topictypesal)
                    {
                        topictypes += t.ToString() + "|";
                    }
                    DatabaseProvider.GetInstance().UpdateTopicTypeForForum(topictypes, int.Parse(dr["fid"].ToString()));
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesOption" + dr["fid"].ToString());
                    Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Forum/TopicTypesLink" + dr["fid"].ToString());
                }

                //更新主题分类表(dnt_topictypes)
                DatabaseProvider.GetInstance().UpdateTopicTypes(name, int.Parse(displayorder), description, int.Parse(id));
                rowid++;
            }

            //更新缓存
            DNTCache cache = DNTCache.GetCacheService();
            cache.RemoveObject("/Forum/TopicTypes");
            if(error)
                base.RegisterStartupScript("", "<script>alert('数据库中已存在相同的主题分类名称或为空，该记录不能被更新！');window.location.href='forum_topictypesgrid.aspx';</script>");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_topictypesgrid.aspx';");
            return;
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
            this.Load += new System.EventHandler(this.Page_Load);
            this.AddNewRec.Click += new EventHandler(this.AddNewRec_Click);
            this.delButton.Click += new EventHandler(this.delButton_Click);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            this.SaveTopicType.Click += new EventHandler(this.SaveTopicType_Click);
            DataGrid1.ColumnSpan = 5;
        }
        #endregion
    }
}
