using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;

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
    /// 编辑论坛版块信息
    /// </summary>

#if NET1
    public class editmall : AdminPage
#else
    public partial class editmall : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.WebControls.Literal forumname;
        protected Discuz.Control.TabControl TabControl1;
        protected Discuz.Control.TabPage tabPage1;
        protected Discuz.Control.TextBox name;
        protected Discuz.Web.Admin.TextareaResize description;
        protected Discuz.Control.RadioButtonList status;
        protected Discuz.Control.RadioButtonList colcount;
        protected Discuz.Control.TextBox colcountnumber;
        protected Discuz.Web.Admin.TextareaResize moderators;
        protected System.Web.UI.WebControls.Literal inheritmoderators;
        protected Discuz.Control.TabPage tabPage2;
        protected Discuz.Control.TextBox password;
        protected Discuz.Control.TextBox icon;
        protected Discuz.Control.TextBox redirect;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected Discuz.Control.TextBox rules;
        protected Discuz.Control.RadioButtonList autocloseoption;
        protected Discuz.Control.TextBox autocloseday;
        protected Discuz.Control.CheckBoxList setting;
        protected Discuz.Control.TabPage tabPage3;
        protected System.Web.UI.HtmlControls.HtmlTable powerset;
        protected Discuz.Control.TabPage tabPage4;
        protected Discuz.Control.Button DelButton;
        protected Discuz.Control.DataGrid SpecialUserList;
        protected Discuz.Web.Admin.TextareaResize UserList;
        protected Discuz.Control.Button BindPower;
        protected Discuz.Control.TabPage tabPage5;
        protected Discuz.Control.RadioButtonList applytopictype;
        protected Discuz.Control.RadioButtonList postbytopictype;
        protected Discuz.Control.RadioButtonList viewbytopictype;
        protected Discuz.Control.RadioButtonList topictypeprefix;
        protected Discuz.Control.DataGrid TopicTypeDataGrid;
        protected Discuz.Control.TabPage tabPage6;
        protected System.Web.UI.WebControls.Label forumsstatic;
        protected Discuz.Control.Button RunForumStatic;
        protected Discuz.Control.TextBox displayorder;
        protected Discuz.Control.TextBox topictypes;
		protected System.Web.UI.HtmlControls.HtmlGenericControl templatestyle;
        protected Discuz.Control.DropDownList templateid;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SubmitInfo;
        protected System.Web.UI.HtmlControls.HtmlGenericControl showcolnum;
        protected System.Web.UI.HtmlControls.HtmlGenericControl showclose;
        #endregion
#endif

        public string runforumsstatic;
        public DataRow dr;
        public ForumInfo __foruminfo = new ForumInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.TabControl1.Items.Remove(this.TabControl1.Items[5]);
                this.TabControl1.Items.Remove(this.TabControl1.Items[4]);
                if (DNTRequest.GetString("fid") == "")
                {
                    return;
                }

                BindTopicType();
                DataGridBind("");
            }
        }

        public void LoadCurrentForumInfo(int fid)
        {
            #region 加载相关信息

            if (fid > 0)
            {
                __foruminfo = AdminForums.GetForumInfomation(fid);
            }
            else
            {
                return;
            }

            if (__foruminfo.Layer > 0)
            {
                tabPage2.Visible = true;
                tabPage6.Visible = true;
            }
            else
            {
                //删除掉"高级设置"属性页
                TabControl1.Items.Remove(tabPage2);
                tabPage2.Visible = false;

                //删除掉"特殊用户"属性页
                TabControl1.Items.Remove(tabPage4);
                tabPage4.Visible = false;

                //删除掉"主题分类"属性页
                TabControl1.Items.Remove(tabPage5);
                tabPage5.Visible = false;

                //删除掉"统计信息"属性页
                TabControl1.Items.Remove(tabPage6);
                tabPage6.Visible = false;
                templatestyle.Visible = false;
            }

            forumname.Text = __foruminfo.Name.Trim();
            name.Text = __foruminfo.Name.Trim();
            displayorder.Text = __foruminfo.Displayorder.ToString();

            status.SelectedValue = __foruminfo.Status.ToString();

            if (__foruminfo.Colcount == 1)
            {
                showcolnum.Attributes.Add("style", "display:none");
                colcount.SelectedIndex = 0;
            }
            else
            {
                showcolnum.Attributes.Add("style", "display:block");
                colcount.SelectedIndex = 1;
            }
            colcount.Attributes.Add("onclick", "javascript:document.getElementById('" + showcolnum.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage1_colcount_0').checked ? 'none' : 'block');");
            colcountnumber.Text = __foruminfo.Colcount.ToString();

            templateid.SelectedValue = __foruminfo.Templateid.ToString();

            forumsstatic.Text = string.Format("主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}",
                                              __foruminfo.Topics.ToString(),
                                              __foruminfo.Posts.ToString(),
                                              __foruminfo.Todayposts.ToString(),
                                              __foruminfo.Lastpost.ToString());

            ViewState["forumsstatic"] = forumsstatic.Text;

            if (__foruminfo.Allowsmilies == 1) setting.Items[0].Selected = true;
            if (__foruminfo.Allowrss == 1) setting.Items[1].Selected = true;
            if (__foruminfo.Allowbbcode == 1) setting.Items[2].Selected = true;
            if (__foruminfo.Allowimgcode == 1) setting.Items[3].Selected = true;
            if (__foruminfo.Recyclebin == 1) setting.Items[4].Selected = true;
            if (__foruminfo.Modnewposts == 1) setting.Items[5].Selected = true;
            //if (__foruminfo.Jammer == 1) setting.Items[6].Selected = true;
            if (__foruminfo.Disablewatermark == 1) setting.Items[6].Selected = true;
            if (__foruminfo.Inheritedmod == 1) setting.Items[7].Selected = true;
            if (__foruminfo.Allowthumbnail == 1) setting.Items[8].Selected = true;
            if (__foruminfo.Allowtag == 1) setting.Items[9].Selected = true;
            //if (__foruminfo.Istrade == 1) setting.Items[11].Selected = true;
            //if ((__foruminfo.Allowpostspecial & 1) != 0) setting.Items[11].Selected = true;
            //if ((__foruminfo.Allowpostspecial & 16) != 0) setting.Items[12].Selected = true;
            //if ((__foruminfo.Allowpostspecial & 4) != 0) setting.Items[13].Selected = true;
            allowspecialonly.SelectedValue = __foruminfo.Allowspecialonly.ToString();

            if (__foruminfo.Autoclose == 0)
            {
                showclose.Attributes.Add("style", "display:none");
                autocloseoption.SelectedIndex = 0;
            }
            else
            {
                autocloseoption.SelectedIndex = 1;
            }
            autocloseoption.Attributes.Add("onclick", "javascript:document.getElementById('" + showclose.ClientID + "').style.display= (document.getElementById('TabControl1_tabPage2_autocloseoption_0').checked ? 'none' : 'block');");
            autocloseday.Text = __foruminfo.Autoclose.ToString();

            //提取高级信息
            description.Text = __foruminfo.Description.Trim();
            password.Text = __foruminfo.Password.Trim();
            icon.Text = __foruminfo.Icon.Trim();
            redirect.Text = __foruminfo.Redirect.Trim();
            moderators.Text = __foruminfo.Moderators.Trim();

            string strusername = "";
            foreach (DataRow dr in DatabaseProvider.GetInstance().GetModerators(fid))
            {
                strusername = strusername + dr["username"].ToString().Trim() + ",";
            }
            if (strusername != "")
            {
                inheritmoderators.Text = strusername.Substring(0, strusername.Length - 1);
            }

            rules.Text = __foruminfo.Rules.Trim();
            topictypes.Text = __foruminfo.Topictypes.Trim();

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
                tr.Cells.Add(GetTD("viewperm", __foruminfo.Viewperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postperm", __foruminfo.Postperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("replyperm", __foruminfo.Replyperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("getattachperm", __foruminfo.Getattachperm.Trim(), dr["groupid"].ToString(), i));
                tr.Cells.Add(GetTD("postattachperm", __foruminfo.Postattachperm.Trim(), dr["groupid"].ToString(), i));
                powerset.Rows.Add(tr);
                i++;
            }


            dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.SetSelectByID(__foruminfo.Attachextensions.Trim());

            if (fid > 0)
            {
                __foruminfo = AdminForums.GetForumInfomation(fid);
            }
            else
            {
                return;
            }
            applytopictype.SelectedValue = __foruminfo.Applytopictype.ToString();
            postbytopictype.SelectedValue = __foruminfo.Postbytopictype.ToString();
            viewbytopictype.SelectedValue = __foruminfo.Viewbytopictype.ToString();
            topictypeprefix.SelectedValue = __foruminfo.Topictypeprefix.ToString();


            #endregion
        }

        private void BindTopicType()
        {
            #region 主题分类绑定
            TopicTypeDataGrid.BindData(DatabaseProvider.GetInstance().GetTopicTypeInfo());
            TopicTypeDataGrid.TableHeaderName = "当前版块:  " + __foruminfo.Name;
            #endregion
        }

        public void TopicTypeDataGrid_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            #region 主题分类列绑定
            string[] topictype = __foruminfo.Topictypes.Split('|');
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                string id = e.Item.Cells[0].Text;
                foreach (string type in topictype)
                {
                    if (type.Split(',')[0] == id)
                    {
                        e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='" + type + "|' /><input type='radio' name='type" + e.Item.ItemIndex + "' value='-1' />";
                        if ((type + "&").IndexOf(",0&") < 0)	//加上一个“&”可以指定是尾部，以防止type中出现"26,1111111111,0"，注意26后面就出现了“,1”，从而选取了“,1”样式
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                        else
                            e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                        if ((type + "&").IndexOf(",1&") < 0) //加上一个“&”可以指定是尾部，以防止type中出现"26,0111111111,0"，注意26后面就出现了“,0”，从而选取了“,0”样式
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
                        else
                            e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' checked value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
                        return;
                    }
                }
                e.Item.Cells[3].Text = "<input type='hidden' name='oldtopictype" + e.Item.ItemIndex + "' value='' /><input type='radio' name='type" + e.Item.ItemIndex + "' checked value='-1' />";
                e.Item.Cells[4].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",0|' />";
                e.Item.Cells[5].Text = "<input type='radio' name='type" + e.Item.ItemIndex + "' value='" + id + "," + e.Item.Cells[1].Text + ",1|' />";
            }
            #endregion
        }

        public void TopicTypeDataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            TopicTypeDataGrid.LoadCurrentPageIndex(e.NewPageIndex);
            BindTopicType();
            DataGridBind("");
            this.TabControl1.SelectedIndex = 4;
        }

        private HtmlTableCell GetTD(string strPerfix, string groupList, string groupId, int ctlId)
        {
            #region 生成组权限控制项

            groupList = "," + groupList + ",";
            string strTd = "<input type='checkbox' name='" + strPerfix + "' id='" + strPerfix + ctlId + "' value='" + groupId + "' "
                + (groupList.IndexOf("," + groupId + ",") == -1 ? "" : "checked='checked'") + ">";
            HtmlTableCell td = new HtmlTableCell("td");
            if (ctlId % 2 == 1)
                td.Attributes.Add("class", "td_alternating_item1");
            else
                td.Attributes.Add("class", "td_alternating_item2");
            td.Controls.Add(new LiteralControl(strTd));
            return td;

            #endregion
        }

        public void BindPower_Click(object sender, EventArgs e)
        {
            #region 特殊用户绑定
            if (UserList.Text != "")
            {
                string result = __foruminfo.Permuserlist;
                string[] userpowerlist = new string[1] { "" };
                if (result != null)
                {
                    userpowerlist = __foruminfo.Permuserlist.Split('|');
                }

                foreach (string adduser in UserList.Text.Split(','))
                {
                    string uid = Discuz.Forum.Users.GetUserID(adduser).ToString();
                    if (uid == "-1")
                        continue;
                    bool find = false;
                    foreach (string u in userpowerlist)
                    {
                        if (u.IndexOf(adduser + ",") == 0)
                        {
                            result = result.Replace(u, adduser + "," + uid + "," + 0);
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        result = adduser + "," + uid + "," + 0 + "|" + result;
                    }
                }
                if (result != "")
                {
                    if (result.Substring(result.Length - 1, 1) == "|")
                        result = result.Substring(0, result.Length - 1);
                }
                DatabaseProvider.GetInstance().UpdatePermUserListByFid(result, __foruminfo.Fid);
                __foruminfo.Permuserlist = result;
                UserList.Text = "";
            }
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            #endregion
        }

        public void DelButton_Click(object sender, EventArgs e)
        {
            #region 删除特殊用户
            int row = 0;
            ArrayList al = new ArrayList(__foruminfo.Permuserlist.Split('|'));
            foreach (object o in SpecialUserList.GetKeyIDArray())
            {
                if (SpecialUserList.GetCheckBoxValue(row, "userid"))
                {
                    string uid = o.ToString();
                    foreach (string user in __foruminfo.Permuserlist.Split('|'))
                    {
                        if (user.IndexOf("," + uid + ",") > 0)
                        {
                            al.Remove(user);
                            break;
                        }
                    }
                }
                row++;
            }
            string result = "";
            foreach (string user in al)
            {
                result += user + "|";
            }
            if (result != "")
                result = result.Substring(0, result.Length - 1);
            DatabaseProvider.GetInstance().UpdatePermUserListByFid(result, __foruminfo.Fid);
            if (SpecialUserList.Items.Count == 1 && SpecialUserList.CurrentPageIndex > 0)
            {
                SpecialUserList.CurrentPageIndex--;
            }

            __foruminfo.Permuserlist = result;
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            #endregion
        }

        private void DataGridBind(string userList)
        {
            #region 特殊用户设置
            SpecialUserList.TableHeaderName = "特殊用户权限设置";
            string Permuserlist = __foruminfo.Permuserlist;
            DataTable dt = new DataTable();
            dt.Columns.Add("id", System.Type.GetType("System.Int32"));
            dt.Columns.Add("uid", System.Type.GetType("System.Int32"));
            dt.Columns.Add("name", System.Type.GetType("System.String"));
            dt.Columns.Add("viewbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("postbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("replybyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("getattachbyuser", System.Type.GetType("System.Boolean"));
            dt.Columns.Add("postattachbyuser", System.Type.GetType("System.Boolean"));
            foreach (string user in userList.Split(','))
            {
                if (user.Trim() == "")
                    continue;
                int uid = Discuz.Forum.Users.GetUserID(user);
                if (uid != -1)
                {
                    DataRow dr = dt.NewRow();
                    dr["id"] = dt.Rows.Count + 1;
                    dr["uid"] = uid.ToString();
                    dr["name"] = user;
                    dr["viewbyuser"] = false;
                    dr["postbyuser"] = false;
                    dr["replybyuser"] = false;
                    dr["getattachbyuser"] = false;
                    dr["postattachbyuser"] = false;
                    dt.Rows.Add(dr);
                }
            }

            if (Permuserlist != null)
            {
                foreach (string p in Permuserlist.Split('|'))
                {
                    if (("," + userList + ",").IndexOf("," + p.Split(',')[0] + ",") >= 0)
                        continue;
                    int power = Convert.ToInt32(p.Split(',')[2]);
                    DataRow dr = dt.NewRow();
                    dr["id"] = dt.Rows.Count + 1;
                    dr["uid"] = p.Split(',')[1];
                    dr["name"] = p.Split(',')[0];
                    dr["viewbyuser"] = power & (int)Forums.ForumSpecialUserPower.ViewByUser;
                    dr["postbyuser"] = power & (int)Forums.ForumSpecialUserPower.PostByUser;
                    dr["replybyuser"] = power & (int)Forums.ForumSpecialUserPower.ReplyByUser;
                    dr["getattachbyuser"] = power & (int)Forums.ForumSpecialUserPower.DownloadAttachByUser;
                    dr["postattachbyuser"] = power & (int)Forums.ForumSpecialUserPower.PostAttachByUser;
                    dt.Rows.Add(dr);
                }
            }
            SpecialUserList.DataSource = dt;
            SpecialUserList.DataBind();
            #endregion
        }

        protected void DataGrid_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            #region 翻页

            SpecialUserList.LoadCurrentPageIndex(e.NewPageIndex);
            DataGridBind("");
            BindTopicType();
            this.TabControl1.SelectedIndex = 3;
            //base.CallBaseRegisterStartupScript("PAGE", "<script>Tab_OnSelectClientClick(document.getElementById('TabControl1:tabPage34_H2'),'TabControl1:tabPage34');</script>");

            #endregion
        }

        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }

        private void SubmitInfo_Click(object sender, EventArgs e)
        {
            #region 提交同级版块

            if (this.CheckCookie())
            {
                if (DNTRequest.GetString("fid") != "")
                {
                    __foruminfo = AdminForums.GetForumInfomation(DNTRequest.GetInt("fid", 0));
                    __foruminfo.Name = name.Text.Trim();
                    __foruminfo.Displayorder = Convert.ToInt32(displayorder.Text);
                    __foruminfo.Status = Convert.ToInt16(status.SelectedValue);

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

                    __foruminfo.Templateid = (Convert.ToInt32(templateid.SelectedValue) == config.Templateid ? 0 : Convert.ToInt32(templateid.SelectedValue));
                    __foruminfo.Allowhtml = 0;
                    __foruminfo.Allowblog = 0;
                    //__foruminfo.Istrade = 0;
                    //__foruminfo.Allowpostspecial = 0; //需要作与运算如下
                    //__foruminfo.Allowspecialonly = 0;　//需要作与运算如下
                    ////$allow辩论 = allowpostspecial & 16;
                    ////$allow悬赏 = allowpostspecial & 4;
                    ////$allow投票 = allowpostspecial & 1;

                    __foruminfo.Alloweditrules = 0;
                    __foruminfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
                    __foruminfo.Allowrss = BoolToInt(setting.Items[1].Selected);
                    __foruminfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
                    __foruminfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
                    __foruminfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
                    __foruminfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
                    //__foruminfo.Jammer = BoolToInt(setting.Items[6].Selected);
                    __foruminfo.Disablewatermark = BoolToInt(setting.Items[6].Selected);
                    __foruminfo.Inheritedmod = BoolToInt(setting.Items[7].Selected);
                    __foruminfo.Allowthumbnail = BoolToInt(setting.Items[8].Selected);
                    __foruminfo.Allowtag = BoolToInt(setting.Items[9].Selected);
                    //__foruminfo.Istrade = BoolToInt(setting.Items[11].Selected);
                    int temppostspecial = 0;
                    //temppostspecial = setting.Items[11].Selected ? temppostspecial | 1 : temppostspecial & ~1;
                    //temppostspecial = setting.Items[12].Selected ? temppostspecial | 16 : temppostspecial & ~16;
                    //temppostspecial = setting.Items[13].Selected ? temppostspecial | 4 : temppostspecial & ~4;
                    __foruminfo.Allowpostspecial = temppostspecial;
                    __foruminfo.Allowspecialonly = Convert.ToInt16(allowspecialonly.SelectedValue);

                    if (autocloseoption.SelectedValue == "0")
                        __foruminfo.Autoclose = 0;
                    else
                        __foruminfo.Autoclose = Convert.ToInt32(autocloseday.Text);

                    __foruminfo.Description = description.Text;
                    __foruminfo.Password = password.Text;
                    __foruminfo.Icon = icon.Text;
                    __foruminfo.Redirect = redirect.Text;
                    __foruminfo.Attachextensions = attachextensions.GetSelectString(",");

                    AdminForums.CompareOldAndNewModerator(__foruminfo.Moderators, moderators.Text.Replace("\r\n",","), DNTRequest.GetInt("fid", 0));

                    __foruminfo.Moderators = moderators.Text.Replace("\r\n", ",");
                    __foruminfo.Rules = rules.Text;
                    __foruminfo.Topictypes = topictypes.Text;
                    __foruminfo.Viewperm = Request.Form["viewperm"];
                    __foruminfo.Postperm = Request.Form["postperm"];
                    __foruminfo.Replyperm = Request.Form["replyperm"];
                    __foruminfo.Getattachperm = Request.Form["getattachperm"];
                    __foruminfo.Postattachperm = Request.Form["postattachperm"];

                    __foruminfo.Applytopictype = Convert.ToInt32(applytopictype.SelectedValue);
                    __foruminfo.Postbytopictype = Convert.ToInt32(postbytopictype.SelectedValue);
                    __foruminfo.Viewbytopictype = Convert.ToInt32(viewbytopictype.SelectedValue);
                    __foruminfo.Topictypeprefix = Convert.ToInt32(topictypeprefix.SelectedValue);
                    __foruminfo.Topictypes = GetTopicType();

                    __foruminfo.Permuserlist = GetPermuserlist();

                    Discuz.Aggregation.AggregationFacade.ForumAggregation.ClearDataBind();
                    string result = AdminForums.SaveForumsInf(__foruminfo).Replace("'", "’");
                    AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "编辑论坛版块", "编辑论坛版块,名称为:" + name.Text.Trim());

                    GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                    __configinfo.Specifytemplate = DatabaseProvider.GetInstance().GetSpecifyForumTemplateCount() > 0 ? 1: 0;
                    GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));
                    if (result == "")
                    {
                        Response.Redirect("forum_ForumsTree.aspx");
                    }
                    else
                    {
                        Response.Write("<script>alert('用户:" + result + "不存在或因为它们所属组为\"游客\",\"等待验证会员\",因为无法设为版主');window.location.href='forum_ForumsTree.aspx';</script>");
                        Response.End();
                    }
                }
            }

            #endregion
        }

        private string GetTopicType()
        {
            #region 获取主题分类
            string tmpType = __foruminfo.Topictypes;
            int i = 0;
            DataTable topicTypes = DbHelper.ExecuteDataset(DatabaseProvider.GetInstance().GetTopicTypes()).Tables[0];
            while (true)
            {
                #region
                if (DNTRequest.GetFormString("type" + i) == "") //循环处理选择的主题分类
                    break;
                else
                {
                    if (DNTRequest.GetFormString("type" + i) != "-1")   //-1不使用，0平板显示，1下拉显示
                    {
                        string oldtopictype = DNTRequest.GetFormString("oldtopictype" + i); //旧主题分类
                        string newtopictype = DNTRequest.GetFormString("type" + i); //新主题分类
                        if (oldtopictype == null || oldtopictype == "")
                        {
                            //tmpType += newtopictype;
                            int insertOrder = GetDisplayOrder(newtopictype.Split(',')[1], topicTypes);
                            ArrayList topictypesal = new ArrayList();
                            foreach (string topictype in tmpType.Split('|'))
                            {
                                if (topictype != "")
                                    topictypesal.Add(topictype);
                            }
                            bool isInsert = false;
                            for (int j = 0; j < topictypesal.Count; j++)
                            {
                                int curDisplayOrder = GetDisplayOrder(topictypesal[j].ToString().Split(',')[1], topicTypes);
                                if (curDisplayOrder > insertOrder)
                                {
                                    topictypesal.Insert(j, newtopictype);
                                    isInsert = true;
                                    break;
                                }
                            }
                            if (!isInsert)
                            {
                                topictypesal.Add(newtopictype);
                            }
                            tmpType = "";
                            foreach (object t in topictypesal)
                            {
                                tmpType += t.ToString() + "|";
                            }
                        }
                        else
                            tmpType = tmpType.Replace(oldtopictype, newtopictype);
                    }
                    else
                    {
                        if (DNTRequest.GetFormString("oldtopictype" + i) != "")
                            tmpType = tmpType.Replace(DNTRequest.GetFormString("oldtopictype" + i), "");
                    }
                }
                #endregion
                i++;

            }
            return tmpType;
            #endregion
        }

        private int GetDisplayOrder(string topicTypeName, DataTable topicTypes)
        {
            #region 返回显示顺序
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
            #region 获取主题分类
            foreach (string type in topicTypes.Split('|'))
            {
                if (type.IndexOf("," + topicName.Trim() + ",") != -1)
                    return type;
            }
            return "";
            #endregion
        }

        private string GetPermuserlist()
        {
            #region 获取特殊用户
            int row = 0;
            string result = __foruminfo.Permuserlist;
            if (result == null)
                return "";
            foreach (object o in SpecialUserList.GetKeyIDArray())
            {
                string uid = o.ToString();
                int power = 0;
                if (SpecialUserList.GetCheckBoxValue(row, "viewbyuser"))
                    power |= (int)Forums.ForumSpecialUserPower.ViewByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "postbyuser"))
                    power |= (int)Forums.ForumSpecialUserPower.PostByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "replybyuser"))
                    power |= (int)Forums.ForumSpecialUserPower.ReplyByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "getattachbyuser"))
                    power |= (int)Forums.ForumSpecialUserPower.DownloadAttachByUser;
                if (SpecialUserList.GetCheckBoxValue(row, "postattachbyuser"))
                    power |= (int)Forums.ForumSpecialUserPower.PostAttachByUser;
                string[] userpowerlist = __foruminfo.Permuserlist.Split('|');
                bool find = false;
                foreach (string u in userpowerlist)
                {
                    if (u.IndexOf("," + uid + ",") > 0)
                    {
                        result = result.Replace(u, u.Split(',')[0] + "," + uid + "," + power);
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    result = Discuz.Forum.Users.GetUserName(Convert.ToInt32(uid)).Trim() + "," + uid + "," + power + "|" + result;
                }
                row++;
            }
            if (result == "")
                return "";
            else
            {
                if (result.Substring(result.Length - 1, 1) == "|")
                    return result.Substring(0, result.Length - 1);
                else
                    return result;
            }
            #endregion
        }

        private void RunForumStatic_Click(object sender, EventArgs e)
        {
            #region 运行论坛统计

            if (this.CheckCookie())
            {
                forumsstatic.Text = ViewState["forumsstatic"].ToString();

                int fid = DNTRequest.GetInt("fid", -1);
                if (fid > 0)
                {
                    __foruminfo = AdminForums.GetForumInfomation(fid);
                }
                else
                {
                    return;
                }

                int topiccount = 0;
                int postcount = 0;
                int lasttid = 0;
                string lasttitle = "";
                string lastpost = "";
                int lastposterid = 0;
                string lastposter = "";
                int replypost = 0;
                AdminForumStats.ReSetFourmTopicAPost(fid, out topiccount, out postcount, out lasttid, out lasttitle, out lastpost, out lastposterid, out lastposter, out replypost);

                runforumsstatic = string.Format("<br /><br />运行结果<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />主题总数:{0}<br />帖子总数:{1}<br />今日回帖数总数:{2}<br />最后提交日期:{3}",
                                                topiccount,
                                                postcount,
                                                replypost,
                                                lastpost);

                if ((__foruminfo.Topics == topiccount) && (__foruminfo.Posts == postcount) && (__foruminfo.Todayposts == replypost) && (__foruminfo.Lastpost.Trim() == lastpost))
                {
                    runforumsstatic += "<br /><br /><br />结果一致";
                }
                else
                {
                    runforumsstatic += "<br /><br /><br />比较<hr style=\"height:1px; width:600; color:#CCCCCC; background:#CCCCCC; border: 0; \" align=\"left\" />";
                    if (__foruminfo.Topics != topiccount)
                    {
                        runforumsstatic += "主题总数有差异<br />";
                    }
                    if (__foruminfo.Posts != postcount)
                    {
                        runforumsstatic += "帖子总数有差异<br />";
                    }
                    if (__foruminfo.Todayposts != replypost)
                    {
                        runforumsstatic += "今日回帖数总数有差异<br />";
                    }
                    if (__foruminfo.Lastpost != lastpost)
                    {
                        runforumsstatic += "最后提交日期有差异<br />";
                    }
                }
            }
            this.TabControl1.SelectedIndex = 5;
            DataGridBind("");
            BindTopicType();
            #endregion
        }

        protected void Sort_Grid(Object sender, DataGridSortCommandEventArgs e)
        {
            #region 排序

            TopicTypeDataGrid.Sort = e.SortExpression.ToString();

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
            //this.TabControl1.IsFirstAddTabPage = true;
            this.TabControl1.InitTabPage();
            this.TabControl1.SelectedIndex = DNTRequest.GetInt("tabindex", 0);
            this.SpecialUserList.PageIndexChanged += new DataGridPageChangedEventHandler(this.DataGrid_PageIndexChanged);
            //this.autocloseoption.SelectedIndexChanged += new System.EventHandler(this.autocloseoption_SelectedIndexChanged);
            this.TopicTypeDataGrid.ItemDataBound += new DataGridItemEventHandler(this.TopicTypeDataGrid_ItemDataBound);
            this.TopicTypeDataGrid.SortCommand += new DataGridSortCommandEventHandler(this.Sort_Grid);
            this.TopicTypeDataGrid.PageIndexChanged += new DataGridPageChangedEventHandler(this.TopicTypeDataGrid_PageIndexChanged);
            //this.colcount.SelectedIndexChanged += new System.EventHandler(this.colcount_SelectedIndexChanged);


            TopicTypeDataGrid.AllowCustomPaging = false;
            TopicTypeDataGrid.DataKeyField = "id";
            TopicTypeDataGrid.ColumnSpan = 6;

            this.SubmitInfo.Click += new System.EventHandler(this.SubmitInfo_Click);
            this.RunForumStatic.Click += new System.EventHandler(this.RunForumStatic_Click);
            this.BindPower.Click += new EventHandler(this.BindPower_Click);
            this.DelButton.Click += new EventHandler(this.DelButton_Click);

            this.Load += new System.EventHandler(this.Page_Load);

            templateid.AddTableData(DatabaseProvider.GetInstance().GetTemplateName());
            DataTable dt = DatabaseProvider.GetInstance().GetAttachType();
            attachextensions.AddTableData(dt);

            LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));


            SpecialUserList.AllowPaging = true;
            SpecialUserList.DataKeyField = "id";
        }

        #endregion
    }
}