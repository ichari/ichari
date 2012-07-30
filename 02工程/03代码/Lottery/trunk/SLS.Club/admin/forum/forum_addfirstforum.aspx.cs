using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using DropDownList = Discuz.Control.DropDownList;
using RadioButtonList = Discuz.Control.RadioButtonList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;

using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 添加第一个分类页
    /// 说明: 当论坛版块表中没有记录时,则会运行该页面
    /// </summary>

#if NET1
    public class addfirstforum : AdminPage
#else
    public partial class addfirstforum : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.TextBox name;
        protected Discuz.Web.Admin.TextareaResize description;
        protected Discuz.Control.RadioButtonList status;
        protected Discuz.Web.Admin.TextareaResize moderators;
        protected Discuz.Control.TabPage tabPage22;
        protected Discuz.Control.TextBox password;
        protected Discuz.Control.TextBox icon;
        protected Discuz.Control.TextBox redirect;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.TextBox rules;
        protected Discuz.Control.RadioButtonList autocloseoption;
        protected Discuz.Control.TextBox autocloseday;
        protected Discuz.Control.CheckBoxList setting;
        protected Discuz.Control.TabPage tabPage33;
        protected System.Web.UI.HtmlControls.HtmlTable powerset;
        protected Discuz.Control.TextBox topictypes;
        protected Discuz.Control.DropDownList templateid;
        protected Discuz.Control.Button Submit;
		protected System.Web.UI.HtmlControls.HtmlGenericControl showclose;
        #endregion
#endif


        public ForumInfo __foruminfo = new ForumInfo();

        protected void Page_Load(object sender, EventArgs e)
        { }


        public void InitInfo()
        {
            #region 加载初始化信息
            //绑定模板
            templateid.AddTableData(DatabaseProvider.GetInstance().GetTemplates());
            //绑定用户组
            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupsTitle();
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell td = new HtmlTableCell("td");
                if (i % 2 == 1)
                    td.Attributes.Add("class", "td_alternating_item1");
                else
                    td.Attributes.Add("class", "td_alternating_item2");
                td.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + i + "' onclick='selectRow(" + i + ",this.checked)'>"));
                tr.Cells.Add(td);
                td = new HtmlTableCell("td");
                if (i % 2 == 1)
                    td.Attributes.Add("class", "td_alternating_item1");
                else
                    td.Attributes.Add("class", "td_alternating_item2");
                td.Controls.Add(new LiteralControl("<label for='r" + i + "'>" + dr["grouptitle"].ToString() + "</lable>"));
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("viewperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }
            //绑定附件类型
            dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);

            showclose.Attributes.Add("style", "display:none");
            autocloseoption.SelectedIndex = 0;
			autocloseoption.Attributes.Add("onclick","javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");

            #endregion
        }

        private HtmlTableCell GetTD(string strPerfix, string groupId, int ctlId)
        {
            #region 生成组权限控制项

            string strTd = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "'>";
            HtmlTableCell td = new HtmlTableCell("td");
            if (ctlId % 2 == 1)
                td.Attributes.Add("class", "td_alternating_item1");
            else
                td.Attributes.Add("class", "td_alternating_item2");
            td.Controls.Add(new LiteralControl(strTd));
            return td;

            #endregion
        }


        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }


        private void SubmitSame_Click(object sender, EventArgs e)
        {
            if (this.CheckCookie())
            {
                InsertForum("0", "0", "0", "0", "1");
            }
        }

        /// <summary>
        /// 插入论坛版块
        /// </summary>
        /// <param name="parentid"></param>
        /// <param name="layer"></param>
        /// <param name="parentidlist"></param>
        /// <param name="subforumcount"></param>
        /// <param name="systemdisplayorder"></param>
        public void InsertForum(string parentid, string layer, string parentidlist, string subforumcount, string systemdisplayorder)
        {
            #region 插入论坛版块记录

            __foruminfo.Parentid = Convert.ToInt32(parentid);
            __foruminfo.Layer = Convert.ToInt32(layer);
            __foruminfo.Parentidlist = parentidlist;
            __foruminfo.Subforumcount = Convert.ToInt32(subforumcount);
            __foruminfo.Name = name.Text.Trim();
            __foruminfo.Status = Convert.ToInt32(status.SelectedValue);
            __foruminfo.Colcount = 1;
            __foruminfo.Displayorder = Convert.ToInt32(systemdisplayorder);
            __foruminfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
            __foruminfo.Allowhtml = 0;
            __foruminfo.Allowblog = 0;
            __foruminfo.Istrade = 0;

            __foruminfo.Alloweditrules = 0;
            __foruminfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            __foruminfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            __foruminfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            __foruminfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            __foruminfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
            __foruminfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
            __foruminfo.Jammer = BoolToInt(setting.Items[6].Selected);
            __foruminfo.Disablewatermark = BoolToInt(setting.Items[7].Selected);
            __foruminfo.Inheritedmod = BoolToInt(setting.Items[8].Selected);
            __foruminfo.Allowthumbnail = BoolToInt(setting.Items[9].Selected);
            __foruminfo.Allowtag = BoolToInt(setting.Items[10].Selected);
            __foruminfo.Istrade = 0;// BoolToInt(setting.Items[11].Selected);
            int temppostspecial = 0;
            temppostspecial = setting.Items[11].Selected ? temppostspecial | 1 : temppostspecial & ~1;
            temppostspecial = setting.Items[12].Selected ? temppostspecial | 16 : temppostspecial & ~16;
            temppostspecial = setting.Items[13].Selected ? temppostspecial | 4 : temppostspecial & ~4;
            __foruminfo.Allowpostspecial = temppostspecial;
            __foruminfo.Allowspecialonly = Convert.ToInt16(allowspecialonly.SelectedValue);

            if (autocloseoption.SelectedValue == "0")
                __foruminfo.Autoclose = 0;
            else
                __foruminfo.Autoclose = Convert.ToInt32(autocloseday.Text);

            __foruminfo.Description = description.Text;
            __foruminfo.Password = password.Text;
            __foruminfo.Icon = icon.Text;
            __foruminfo.Postcredits = "";
            __foruminfo.Replycredits = "";
            __foruminfo.Redirect = redirect.Text;
            __foruminfo.Attachextensions = attachextensions.GetSelectString(",");
            __foruminfo.Moderators = moderators.Text;
            __foruminfo.Rules = rules.Text;
            __foruminfo.Topictypes = topictypes.Text;
            __foruminfo.Viewperm = Request.Form["viewperm"];
            __foruminfo.Postperm = Request.Form["postperm"];
            __foruminfo.Replyperm = Request.Form["replyperm"];
            __foruminfo.Getattachperm = Request.Form["getattachperm"];
            __foruminfo.Postattachperm = Request.Form["postattachperm"];

            string result = AdminForums.InsertForumsInf(__foruminfo).Replace("'", "’");

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加论坛版块", "添加论坛版块,名称为:" + name.Text.Trim());

            if (result == "")
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_ForumsTree.aspx';");
            else
                base.RegisterStartupScript( "PAGE", "alert('用户:" + result + "不存在,因为无法设为版主');window.location.href='forum_ForumsTree.aspx';");

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
            this.TabControl1.InitTabPage();
            TabControl1.Items.Remove(tabPage22);
            tabPage22.Visible = false;
           
            this.Submit.Click += new EventHandler(this.SubmitSame_Click);
            //this.autocloseoption.SelectedIndexChanged += new EventHandler(this.autocloseoption_SelectedIndexChanged);
            this.Load += new EventHandler(this.Page_Load);

            InitInfo();
        }

        #endregion

    }
}