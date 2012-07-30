using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using DataGrid = Discuz.Control.DataGrid;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;

//using Discuz.DbProvider.SqlServer;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.IO;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 审核用户
    /// </summary>

#if NET1
    public class forum_tagmanage : AdminPage
#else
    public partial class forum_tagmanage : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox regbefore;
        protected Discuz.Control.TextBox regip;
        protected Discuz.Control.Button clearuser;
        protected Discuz.Control.Button SelectPass;
        protected Discuz.Control.Button SelectDelete;
        protected Discuz.Control.Button AllPass;
        protected Discuz.Control.Button AllDelete;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox sendemail;
        protected Discuz.Control.DataGrid DataGrid1;
        #endregion
#endif
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        public void BindData()
        {
            DataGrid1.AllowCustomPaging = false;
            DataGrid1.TableHeaderName = "标签列表";
            DataGrid1.DataKeyField = "tagid";
            DataGrid1.BindData(DatabaseProvider.GetInstance().GetForumTagsSql("",Convert.ToInt32(radstatus.SelectedValue)));
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            DataGrid1.Sort = e.SortExpression.ToString();
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            DataGrid1.LoadCurrentPageIndex(e.NewPageIndex);
        }


        protected void savetags_Click(object sender, EventArgs e)
        {
            #region 保存标签修改
            int row = 0;
            bool error = false;
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                string orderid = DataGrid1.GetControlValue(row, "orderid").Trim();
                string color = DataGrid1.GetControlValue(row, "color").Trim().ToUpper();
                Regex r = new Regex("^#?([0-9|A-F]){6}$");
                if (orderid == "" || !Utils.IsNumeric(orderid) || (color != "" && !r.IsMatch(color)))
                {
                    error = true;
                    continue;
                }
                DatabaseProvider.GetInstance().UpdateForumTags(id,int.Parse(orderid),color.Replace("#",""));
                row++;
            }
            Topics.NeatenRelateTopics();
            WriteTagsStatus();
            if (error)
                base.RegisterStartupScript("PAGE", "alert('某些记录输入错误，未能被更新！');window.location.href='forum_tagmanage.aspx';");
            else
                base.RegisterStartupScript("PAGE", "window.location.href='forum_tagmanage.aspx';");
            #endregion
        }

        private void SelectDelete_Click(object sender, EventArgs e)
        {
            #region 删除选中的用户信息

            if (this.CheckCookie())
            {
                string uidlist = DNTRequest.GetString("uid");
                if (uidlist != "")
                {
                    DatabaseProvider.GetInstance().DeleteUserByUidlist(uidlist);
                    base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
                }
                else
                {
                    base.RegisterStartupScript("", "<script>alert('请选择相应的用户!');window.location='forum_audituser.aspx';</script>");
                }
            }

            #endregion
        }

        private void AllPass_Click(object sender, EventArgs e)
        {
            #region 将用户调整到相应的用户组

            if (this.CheckCookie())
            {
                if (UserCredits.GetCreditsUserGroupID(0) != null)
                {
                    int tmpGroupID = UserCredits.GetCreditsUserGroupID(0).Groupid; //添加注册用户审核机制后需要修改
                    DatabaseProvider.GetInstance().ChangeUsergroup(8, tmpGroupID);
                    foreach (DataRow dr in DatabaseProvider.GetInstance().GetAudituserUid().Tables[0].Rows)
                    {
                        UserCredits.UpdateUserCredits(Convert.ToInt32(dr["uid"].ToString()));
                    }
                    DatabaseProvider.GetInstance().ClearAllUserAuthstr();
                }
                base.RegisterStartupScript("PAGE", "window.location='forum_audituser.aspx';");
            }

            #endregion
        }

        private static void WriteTextFile(string filename,string content)
        {
            FileStream fs = new FileStream(Utils.GetMapPath("../../cache/tag/" + filename), FileMode.Create);
            byte[] bt = System.Text.Encoding.UTF8.GetBytes(content);
            fs.Write(bt, 0, bt.Length);
            fs.Close();
        }
        private void WriteTagsStatus()
        {
            string closedtags = "";
            string colorfultags = "";
            DataTable tags = DbHelper.ExecuteDataset(DatabaseProvider.GetInstance().GetForumTagsSql("",Convert.ToInt32(radstatus.SelectedValue))).Tables[0];
            foreach (DataRow dr in tags.Rows)
            {
                if (dr["orderid"].ToString() == "-1")
                {
                    closedtags += "'" + dr["tagid"].ToString() + "',";
                }
                if (dr["color"].ToString().Trim() != "")
                {
                    colorfultags += "'" + dr["tagid"].ToString() + "':{'tagid' : '" + dr["tagid"].ToString() + "', 'color' : '" + dr["color"].ToString() + "'},";
                }
            }
            closedtags = "var closedtags = [" + closedtags.TrimEnd(',') + "];";
            WriteTextFile("closedtags.txt", closedtags);
            colorfultags = "var colorfultags = {" + colorfultags.TrimEnd(',') + "};";
            WriteTextFile("colorfultags.txt", colorfultags);
        }

        protected void search_Click(object sender, System.EventArgs e)
        {
            //if (this.CheckCookie())
            //{
            //    DataGrid1.BindData(DatabaseProvider.GetInstance().GetForumTagsSql(tagname.Text.Trim()));
            //}
        }

        protected void searchtag_Click(object sender, System.EventArgs e)
        {
            BindData();
            string tag_name = this.tagname.Text.Trim();
            int from = Utils.StrToInt(txtfrom.Text.Trim(),101);
            int end = Utils.StrToInt(txtend.Text.Trim(),102);

            //三个文本框都为空，返回
            if(from==101 && end==102 && tag_name=="")
            {
                return;
            }

            if (from == 101 && end != 102)
            {
                return;
            }

            if (from != 101 && end == 102)
            {
                return;
            }


            //当名称不为空，范围为空，按名字搜索

            int selected = Convert.ToInt32(this.radstatus.SelectedValue);

            if (tag_name != "" && ((from == 101) && (end == 102)))
            {
                if (this.CheckCookie())
                {
                    DataGrid1.BindData(DatabaseProvider.GetInstance().GetForumTagsSql(tag_name, selected));
                }
            }
            else
            {
                DataGrid1.BindData(DatabaseProvider.GetInstance().GetTopicNumber(tag_name, from, end, selected));
            }
        }

        protected void DisableRec_Click(object sender, System.EventArgs e)
        {
            int row = 0;
            string tagid = DNTRequest.GetString("tagid");
            foreach (object o in DataGrid1.GetKeyIDArray())
            {
                int id = int.Parse(o.ToString());
                if (("," + tagid + ",").IndexOf("," + id + ",") == -1)
                    continue;
                string orderid = "-1";
                string color = DataGrid1.GetControlValue(row, "color").Trim().ToUpper();
                Regex r = new Regex("^#?([0-9|A-F]){6}$");
                if (color != "" && !r.IsMatch(color))
                {
                    continue;
                }
                DatabaseProvider.GetInstance().UpdateForumTags(id, int.Parse(orderid), color.Replace("#", ""));
                row++;
            }
            Topics.NeatenRelateTopics();
            WriteTagsStatus();
            base.RegisterStartupScript("PAGE", "window.location.href='forum_tagmanage.aspx';");
        }

        private void DataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 数据绑定显示长度控制

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                System.Web.UI.WebControls.TextBox t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[2].Controls[0];
                t.Attributes.Add("maxlength", "3");
                t.Attributes.Add("size", "3");

                t = (System.Web.UI.WebControls.TextBox)e.Item.Cells[3].Controls[0];
                t.Attributes.Add("maxlength", "6");
                t.Attributes.Add("size", "6");
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
            //this.SelectPass.Click += new EventHandler(this.SelectPass_Click);
            //this.SelectDelete.Click += new EventHandler(this.SelectDelete_Click);
            //this.AllPass.Click += new EventHandler(this.AllPass_Click);
            //this.AllDelete.Click += new EventHandler(this.AllDelete_Click);
           // this.Load += new EventHandler(this.Page_Load);
            this.DataGrid1.ItemDataBound += new DataGridItemEventHandler(this.DataGrid_ItemDataBound);
            DataGrid1.DataKeyField = "tagid";
            DataGrid1.TableHeaderName = "审核用户列表";
            DataGrid1.ColumnSpan = 8;
        }

        #endregion
    }
}