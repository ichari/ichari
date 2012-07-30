using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


using Discuz.Control;
using Discuz.Common;
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
    /// 添加版块
    /// </summary>

#if NET1
    public class addforums : AdminPage
#else
    public partial class addforums : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage51;
        protected Discuz.Control.TextBox name;
        protected Discuz.Web.Admin.TextareaResize description;
        protected Discuz.Control.RadioButtonList status;
        protected Discuz.Control.RadioButtonList addtype;
        protected Discuz.Control.DropDownTreeList targetforumid;
        protected Discuz.Control.RadioButtonList colcount;
        protected Discuz.Control.TextBox colcountnumber;
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
        protected Discuz.Control.TextBox topictypes;
        protected Discuz.Control.DropDownList templateid;
        protected Discuz.Control.Button SubmitAdd;
        protected Discuz.Control.Hint Hint1;

        protected System.Web.UI.HtmlControls.HtmlTable powerset;
        protected System.Web.UI.HtmlControls.HtmlGenericControl showcolnum;
		protected System.Web.UI.HtmlControls.HtmlGenericControl showclose;
        protected System.Web.UI.HtmlControls.HtmlGenericControl showtargetforum;
        #endregion
#endif


        public ForumInfo __foruminfo = new ForumInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetForums();

                //如果版块表中没有任何版块, 则跳转到"添加第一个版块"页面. 
                if (dt.Rows.Count == 0)
                {
                    Server.Transfer("forum_AddFirstForum.aspx");
                }
            }
        }

        public void InitInfo()
        {
            #region 初始化信息绑定

            targetforumid.BuildTree(DatabaseProvider.GetInstance().GetForumsTree());

            templateid.AddTableData(DatabaseProvider.GetInstance().GetTemplates());

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
                td.Controls.Add(new LiteralControl("<input type='checkbox' id='r" + i + "' onclick='selectRow(" + i + ",this.checked)'><label for='r" + i + "'>" + dr["grouptitle"].ToString() + "</lable>"));
                tr.Cells.Add(td);
                tr.Cells.Add(GetTD("viewperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }

            dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);


            if (DNTRequest.GetString("fid") != "")
            {
                targetforumid.SelectedValue = DNTRequest.GetString("fid");
                addtype.SelectedValue = "1";
                targetforumid.Visible = true;
            }

            showcolnum.Attributes.Add("style", "display:none");
            colcount.SelectedIndex = 0;
            colcount.Attributes.Add("onclick","javascript:document.getElementById('" + showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_colcount_0').checked ? 'none' : 'block');");

            showclose.Attributes.Add("style", "display:none");
            autocloseoption.SelectedIndex = 0;

            showtargetforum.Attributes.Add("style", "display:block");
            addtype.Attributes.Add("onclick", "javascript:document.getElementById('" + showtargetforum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage51_addtype_0').checked ? 'none' : 'block');");

			autocloseoption.Attributes.Add("onclick","javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage22_autocloseoption_0').checked ? 'none' : 'block');");

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


        private void SubmitSameAfter()
        {
            if (this.CheckCookie())
            {
                int maxdisplayorder = DatabaseProvider.GetInstance().GetForumsMaxDisplayOrder();
                InsertForum("0", "0", "0", "0", maxdisplayorder.ToString());
            }
        }


        public string SetAfterDisplayOrder(int currentdisplayorder)
        {
            #region 在当前节点之后加入同级论坛时的displayorder字段值

            DatabaseProvider.GetInstance().UpdateForumsDisplayOrder(currentdisplayorder);
            return Convert.ToString(currentdisplayorder + 1);

            #endregion
        }

        public void SetSubForumCount(int fid)
        {
            #region 在当前节点之后加入子论坛时的displayorder字段值

            DatabaseProvider.GetInstance().UpdateSubForumCount(fid);
            #endregion
        }

        private void SubmitAddChild()
        {
            #region 保存新增论坛信息
            if (this.CheckCookie())
            {
                if (name.Text.Trim() == "")
                {
                    base.RegisterStartupScript("", "<script>alert('论坛名称不能为空');</script>");
                    return;
                }
                if (targetforumid.SelectedValue != "0")
                {
                    #region 添加与当前论坛同级的论坛

                    //添加与当前论坛同级的论坛
                    DataRow dr = DatabaseProvider.GetInstance().GetForum(Utils.StrToInt(targetforumid.SelectedValue, 0));
                        
                    //找出当前要插入的记录所用的FID
                    string parentidlist = null;
                    if (dr["parentidlist"].ToString().Trim() == "0")
                    {
                        parentidlist = dr["fid"].ToString();
                    }
                    else
                    {
                        parentidlist = dr["parentidlist"].ToString().Trim() + "," + dr["fid"].ToString();
                    }

                    int maxdisplayorder = 0;
                    DataTable dt = DatabaseProvider.GetInstance().GetForumsMaxDisplayOrder(Utils.StrToInt(targetforumid.SelectedValue, 0));
                    if ((dt.Rows.Count > 0) && (dt.Rows[0][0].ToString() != ""))
                    {
                        maxdisplayorder = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        maxdisplayorder = Convert.ToInt32(dr["displayorder"].ToString());
                    }

                    InsertForum(dr["fid"].ToString(),
                                Convert.ToString((Convert.ToInt32(dr["layer"].ToString()) + 1)),
                                parentidlist,
                                "0",
                                SetAfterDisplayOrder(maxdisplayorder));

                    SetSubForumCount(Convert.ToInt32(dr["fid"].ToString()));

                    #endregion
                }
                else
                {
                    #region 按根论坛插入

                    int maxdisplayorder = DatabaseProvider.GetInstance().GetForumsMaxDisplayOrder();
                    InsertForum("0", "0", "0", "0", maxdisplayorder.ToString());

                    #endregion
                }
            }
            #endregion
        }


        private void SubmitAdd_Click(object sender, EventArgs e)
        {
            if (addtype.SelectedValue == "0")
            {
                SubmitSameAfter();
            }
            else
            {
                if (targetforumid.SelectedValue == "0")
                {
                    base.RegisterStartupScript("", "<script>alert('请选择所属论坛版块');</script>");
                    return;
                }

                SubmitAddChild();
            }
        }

        public void InsertForum(string parentid, string layer, string parentidlist, string subforumcount, string systemdisplayorder)
        {
            #region 添加新论坛

            __foruminfo.Parentid = Convert.ToInt32(parentid);
            __foruminfo.Layer = Convert.ToInt32(layer);
            __foruminfo.Parentidlist = parentidlist;
            __foruminfo.Subforumcount = Convert.ToInt32(subforumcount);
            __foruminfo.Name = name.Text.Trim();

            __foruminfo.Status = Convert.ToInt16(status.SelectedValue);

            __foruminfo.Displayorder = Convert.ToInt32(systemdisplayorder);

            __foruminfo.Templateid = Convert.ToInt32(templateid.SelectedValue);
            __foruminfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            __foruminfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            __foruminfo.Allowhtml = 0;
            __foruminfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            __foruminfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            __foruminfo.Allowblog = 0;
            __foruminfo.Istrade = 0;

            //__foruminfo.Allowpostspecial = 0; //需要作与运算如下
            //__foruminfo.Allowspecialonly = 0;　//需要作与运算如下
            //$allow辩论 = allowpostspecial & 16;
            //$allow悬赏 = allowpostspecial & 4;
            //$allow投票 = allowpostspecial & 1;

            __foruminfo.Alloweditrules = 0;
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
            if (colcount.SelectedValue == "1") //传统模式[默认]
            {
                __foruminfo.Colcount = 1;
            }
            else
            {
                if (Convert.ToInt16(colcountnumber.Text) < 1 || Convert.ToInt16(colcountnumber.Text) > 9)
                {
                    base.RegisterStartupScript("", "<script>alert('列值必须在2~9范围内');</script>");
                    return;
                }
                __foruminfo.Colcount = Convert.ToInt16(colcountnumber.Text);
            }
            __foruminfo.Viewperm = Request.Form["viewperm"];
            __foruminfo.Postperm = Request.Form["postperm"];
            __foruminfo.Replyperm = Request.Form["replyperm"];
            __foruminfo.Getattachperm = Request.Form["getattachperm"];
            __foruminfo.Postattachperm = Request.Form["postattachperm"];

            string result = AdminForums.InsertForumsInf(__foruminfo).Replace("'", "’");

            AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "添加论坛版块", "添加论坛版块,名称为:" + name.Text.Trim());

            if (result == "")
                base.RegisterStartupScript( "PAGE", "self.location.href='forum_ForumsTree.aspx';");
            else
                base.RegisterStartupScript( "PAGE", "alert('用户:" + result + "不存在,因为无法设为版主');self.location.href='forum_ForumsTree.aspx';");

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
            this.TabControl1.InitTabPage();
            this.SubmitAdd.Click += new EventHandler(this.SubmitAdd_Click);
            //this.autocloseoption.SelectedIndexChanged += new EventHandler(this.autocloseoption_SelectedIndexChanged);
            //this.colcount.SelectedIndexChanged += new EventHandler(this.colcount_SelectedIndexChanged);
            //this.addtype.SelectedIndexChanged += new EventHandler(this.addtype_SelectedIndexChanged);
            this.Load += new EventHandler(this.Page_Load);
            InitInfo();
        }

        #endregion

    }
}