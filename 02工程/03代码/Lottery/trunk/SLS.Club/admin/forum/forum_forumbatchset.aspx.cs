using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using Discuz.Common;
using Discuz.Forum;
using Button = Discuz.Control.Button;
using CheckBoxList = Discuz.Control.CheckBoxList;
using TextBox = Discuz.Control.TextBox;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 论坛版块批量设置
    /// </summary>
     
#if NET1
    public class forumbatchset : AdminPage
#else
    public partial class forumbatchset : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Web.Admin.forumtree Forumtree1;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setpassword;
        protected Discuz.Control.TextBox password;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setattachextensions;
        protected Discuz.Control.CheckBoxList attachextensions;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setpostcredits;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setreplycredits;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setsetting;
        protected System.Web.UI.WebControls.CheckBoxList setting;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setviewperm;
        protected Discuz.Control.CheckBoxList viewperm;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setpostperm;
        protected Discuz.Control.CheckBoxList postperm;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setreplyperm;
        protected Discuz.Control.CheckBoxList replyperm;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setgetattachperm;
        protected Discuz.Control.CheckBoxList getattachperm;
        protected System.Web.UI.HtmlControls.HtmlInputCheckBox setpostattachperm;
        protected Discuz.Control.CheckBoxList postattachperm;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SubmitBatchSet;
        #endregion
#endif


        public ForumInfo __foruminfo = new ForumInfo();

        protected void Page_Load(object sender, EventArgs e)
        { }


        public void LoadCurrentForumInfo(int fid)
        {
            #region 提取基本信息

            if (fid > 0)
            {
                __foruminfo = AdminForums.GetForumInfomation(fid);
            }
            else
            {
                return;
            }

            if (__foruminfo.Allowsmilies == 1) setting.Items[0].Selected = true;
            if (__foruminfo.Allowrss == 1) setting.Items[1].Selected = true;
            if (__foruminfo.Allowbbcode == 1) setting.Items[2].Selected = true;
            if (__foruminfo.Allowimgcode == 1) setting.Items[3].Selected = true;
            if (__foruminfo.Recyclebin == 1) setting.Items[4].Selected = true;
            if (__foruminfo.Modnewposts == 1) setting.Items[5].Selected = true;
            if (__foruminfo.Jammer == 1) setting.Items[6].Selected = true;
            if (__foruminfo.Disablewatermark == 1) setting.Items[7].Selected = true;
            if (__foruminfo.Inheritedmod == 1) setting.Items[8].Selected = true;
            if (__foruminfo.Allowthumbnail == 1) setting.Items[9].Selected = true;

            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupsTitle();
            viewperm.SetSelectByID(__foruminfo.Viewperm.Trim());
            postperm.SetSelectByID(__foruminfo.Postperm.Trim());
            replyperm.SetSelectByID(__foruminfo.Replyperm.Trim());
            getattachperm.SetSelectByID(__foruminfo.Getattachperm.Trim());
            postattachperm.SetSelectByID(__foruminfo.Postattachperm.Trim());

            dt = DatabaseProvider.GetInstance().GetAttachTypes();
                
            attachextensions.SetSelectByID(__foruminfo.Attachextensions.Trim());

            #endregion
        }

        public int BoolToInt(bool a)
        {
            if (a) return 1;
            else return 0;
        }

        private void SubmitBatchSet_Click(object sender, EventArgs e)
        {
            #region 写入批量论坛设置信息

            string targetlist = DNTRequest.GetString("Forumtree1");

            if ((targetlist == "") || (targetlist == ",") || (targetlist == "0"))
            {
                base.RegisterStartupScript( "", "<script>alert('您未选中任何版块, 系统无法提交! ');</script>");
                return;
            }

            __foruminfo = AdminForums.GetForumInfomation(DNTRequest.GetInt("fid", -1));
            __foruminfo.Allowsmilies = BoolToInt(setting.Items[0].Selected);
            __foruminfo.Allowrss = BoolToInt(setting.Items[1].Selected);
            __foruminfo.Allowhtml = 0;
            __foruminfo.Allowbbcode = BoolToInt(setting.Items[2].Selected);
            __foruminfo.Allowimgcode = BoolToInt(setting.Items[3].Selected);
            __foruminfo.Allowblog = 0;
            __foruminfo.Istrade = 0;
            __foruminfo.Alloweditrules = 0;
            __foruminfo.Recyclebin = BoolToInt(setting.Items[4].Selected);
            __foruminfo.Modnewposts = BoolToInt(setting.Items[5].Selected);
            __foruminfo.Jammer = BoolToInt(setting.Items[6].Selected);
            __foruminfo.Disablewatermark = BoolToInt(setting.Items[7].Selected);
            __foruminfo.Inheritedmod = BoolToInt(setting.Items[8].Selected);
            __foruminfo.Allowthumbnail = BoolToInt(setting.Items[9].Selected);
            __foruminfo.Password = password.Text;
            __foruminfo.Attachextensions = attachextensions.GetSelectString(",");
            __foruminfo.Viewperm = viewperm.GetSelectString(",");
            __foruminfo.Postperm = postperm.GetSelectString(",");
            __foruminfo.Replyperm = replyperm.GetSelectString(",");
            __foruminfo.Getattachperm = getattachperm.GetSelectString(",");
            __foruminfo.Postattachperm = postattachperm.GetSelectString(",");

            BatchSetParams bsp = new BatchSetParams();
            bsp.SetPassWord = setpassword.Checked;
            bsp.SetAttachExtensions = setattachextensions.Checked;
            bsp.SetPostCredits = setpostcredits.Checked;
            bsp.SetReplyCredits = setreplycredits.Checked;
            bsp.SetSetting = setsetting.Checked;
            bsp.SetViewperm = setviewperm.Checked;
            bsp.SetPostperm = setpostperm.Checked;
            bsp.SetReplyperm = setreplyperm.Checked;
            bsp.SetGetattachperm = setgetattachperm.Checked;
            bsp.SetPostattachperm = setpostattachperm.Checked;

            if (AdminForums.BatchSetForumInf(__foruminfo, bsp, targetlist))
            {
                base.RegisterStartupScript( "PAGE", "window.location.href='forum_ForumsTree.aspx';");
            }
            else
            {
                base.RegisterStartupScript( "", "<script>alert('提交不成功!');window.location.href='forum_ForumsTree.aspx';</script>");
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
            this.SubmitBatchSet.Click += new EventHandler(this.SubmitBatchSet_Click);
            this.Load += new EventHandler(this.Page_Load);

            #region 控件数据绑定

            DataTable dt = DatabaseProvider.GetInstance().GetUserGroupsTitle();
            viewperm.AddTableData(dt);
            postperm.AddTableData(dt);
            replyperm.AddTableData(dt);
            getattachperm.AddTableData(dt);
            postattachperm.AddTableData(dt);

            dt = DatabaseProvider.GetInstance().GetAttachTypes();
            attachextensions.AddTableData(dt);

            LoadCurrentForumInfo(DNTRequest.GetInt("fid", -1));

            #endregion
        }

        #endregion
    }
}