using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Common;
using Discuz.Entity;


namespace Discuz.Web.Admin
{

#if NET1
    public class option : AdminPage
#else
    public partial class option : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton Topicqueuestats_1;
        protected System.Web.UI.HtmlControls.HtmlInputRadioButton Topicqueuestats_0;
        protected Discuz.Control.TextBox topicqueuestatscount;
        protected Discuz.Control.RadioButtonList fullmytopics;
        protected Discuz.Control.RadioButtonList userstatusby;
        protected Discuz.Control.RadioButtonList modworkstatus;
        protected Discuz.Control.TextBox guestcachepagetimeout;
        protected Discuz.Control.TextBox topiccachemark;
        protected Discuz.Control.RadioButtonList reasonpm;
        protected Discuz.Control.TextBox losslessdel;
        protected Discuz.Control.TextBox edittimelimit;
        protected Discuz.Control.RadioButtonList editedby;
        protected Discuz.Control.TextBox tpp;
        protected Discuz.Control.TextBox ppp;
        protected Discuz.Control.TextBox hottopic;
        protected Discuz.Control.TextBox starthreshold;
        protected Discuz.Control.RadioButtonList defaulteditormode;
        protected Discuz.Control.RadioButtonList allowswitcheditor;
        protected Discuz.Control.RadioButtonList allowhtmltitle;
        protected Discuz.Control.RadioButtonList fastpost;
        protected Discuz.Control.RadioButtonList enabletag;
		protected Discuz.Control.TextBox ratevalveset1;
		protected Discuz.Control.TextBox ratevalveset2;
		protected Discuz.Control.TextBox ratevalveset3;
		protected Discuz.Control.TextBox ratevalveset4;
		protected Discuz.Control.TextBox ratevalveset5;
        protected Discuz.Control.Hint Hint1;
        protected Discuz.Control.Button SaveInfo;
        #endregion
#endif

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息

            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            fullmytopics.SelectedValue = __configinfo.Fullmytopics.ToString();
            modworkstatus.SelectedValue = __configinfo.Modworkstatus.ToString();
            userstatusby.SelectedValue = (__configinfo.Userstatusby.ToString() != "0") ? "1" : "0";
            guestcachepagetimeout.Text = __configinfo.Guestcachepagetimeout.ToString();
            topiccachemark.Text = __configinfo.Topiccachemark.ToString();

            if (__configinfo.TopicQueueStats == 1)
            {
                Topicqueuestats_1.Checked = true;
                Topicqueuestats_0.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:visible;");
            }
            else
            {
                Topicqueuestats_0.Checked = true;
                Topicqueuestats_1.Checked = false;
                topicqueuestatscount.AddAttributes("style", "visibility:hidden;");
            }

            topicqueuestatscount.Text = __configinfo.TopicQueueStatsCount.ToString();
            losslessdel.Text = __configinfo.Losslessdel.ToString();
            edittimelimit.Text = __configinfo.Edittimelimit.ToString();
            editedby.SelectedValue = __configinfo.Editedby.ToString();
            defaulteditormode.SelectedValue = __configinfo.Defaulteditormode.ToString();
            allowswitcheditor.SelectedValue = __configinfo.Allowswitcheditor.ToString();
            reasonpm.SelectedValue = __configinfo.Reasonpm.ToString();
            hottopic.Text = __configinfo.Hottopic.ToString();
            starthreshold.Text = __configinfo.Starthreshold.ToString();
            fastpost.SelectedValue = __configinfo.Fastpost.ToString();
            tpp.Text = __configinfo.Tpp.ToString();
            ppp.Text = __configinfo.Ppp.ToString();
            allowhtmltitle.SelectedValue = __configinfo.Htmltitle.ToString();
            enabletag.SelectedValue = __configinfo.Enabletag.ToString();
            string[] ratevalveset = __configinfo.Ratevalveset.Split(',');
            ratevalveset1.Text = ratevalveset[0];
            ratevalveset2.Text = ratevalveset[1];
            ratevalveset3.Text = ratevalveset[2];
            ratevalveset4.Text = ratevalveset[3];
            ratevalveset5.Text = ratevalveset[4];
            statstatus.SelectedValue = __configinfo.Statstatus.ToString();
            statscachelife.Text = __configinfo.Statscachelife.ToString();
            //pvfrequence.Text = __configinfo.Pvfrequence.ToString();
            hottagcount.Text = __configinfo.Hottagcount.ToString();
            oltimespan.Text = __configinfo.Oltimespan.ToString();
            maxmodworksmonths.Text = __configinfo.Maxmodworksmonths.ToString();
            disablepostad.SelectedValue = __configinfo.Disablepostad.ToString();
            disablepostad.Items[0].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='';");
            disablepostad.Items[1].Attributes.Add("onclick", "$('" + postadstatus.ClientID + "').style.display='none';");
            disablepostadregminute.Text = __configinfo.Disablepostadregminute.ToString();
            disablepostadpostcount.Text = __configinfo.Disablepostadpostcount.ToString();
            disablepostadregular.Text = __configinfo.Disablepostadregular.ToString();
            if (__configinfo.Disablepostad == 0)
            {
                postadstatus.Attributes.Add("style", "display:none");
            }
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息
            string[][] inputrule = new string[2][];
            inputrule[0] = new string[] { losslessdel.Text, edittimelimit.Text, tpp.Text, ppp.Text, starthreshold.Text, hottopic.Text, 
                guestcachepagetimeout.Text,disablepostadregminute.Text,disablepostadpostcount.Text};
            inputrule[1] = new string[] { "删帖不减金币时间", "编辑帖子时间", "每页主题数", "每页主题数", "星星升级阀值", "热门话题最低帖数",
                "缓存游客页面的失效时间","新用户广告强力屏蔽注册分钟数","新用户广告强力屏蔽发帖数" };
            for (int j = 0; j < inputrule[0].Length; j++)
            {
                if (!Utils.IsInt(inputrule[0][j].ToString()))
                {
                    base.RegisterStartupScript("", "<script>alert('输入错误:" + inputrule[1][j].ToString() + ",只能是0或者正整数');window.location.href='forum_option.aspx';</script>");
                    return;
                }
            }
            if (Convert.ToInt32(losslessdel.Text) > 9999 || Convert.ToInt32(losslessdel.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('删帖不减金币时间期限只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt32(edittimelimit.Text) > 9999 || Convert.ToInt32(edittimelimit.Text) < 0)
            {
                base.RegisterStartupScript( "", "<script>alert('编辑帖子时间限制只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(tpp.Text) > 100 || Convert.ToInt16(tpp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('每页主题数只能在1-100之间');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(ppp.Text) > 100 || Convert.ToInt16(ppp.Text) <= 0)
            {
                base.RegisterStartupScript("", "<script>alert('每页帖子数只能在1-100之间');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (Convert.ToInt16(starthreshold.Text) > 9999 || Convert.ToInt16(starthreshold.Text) < 0)
            {
                base.RegisterStartupScript( "", "<script>alert('星星升级阀值只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottopic.Text) > 9999 || Convert.ToInt16(hottopic.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('热门话题最低帖数只能在0-9999之间');window.location.href='forum_option.aspx';</script>");
                return;
            }

            if (Convert.ToInt16(hottagcount.Text) > 60 || Convert.ToInt16(hottagcount.Text) < 0)
            {
                base.RegisterStartupScript("", "<script>alert('首页热门标签(Tag)数量只能在0-60之间');window.location.href='forum_option.aspx';</script>");
            }

            if (!ValidateRatevalveset(ratevalveset1.Text)) return;
            if (!ValidateRatevalveset(ratevalveset2.Text)) return;
            if (!ValidateRatevalveset(ratevalveset3.Text)) return;
            if (!ValidateRatevalveset(ratevalveset4.Text)) return;
            if (!ValidateRatevalveset(ratevalveset5.Text)) return;
            if (!(Convert.ToInt16(ratevalveset1.Text) < Convert.ToInt16(ratevalveset2.Text) &&
                  Convert.ToInt16(ratevalveset2.Text) < Convert.ToInt16(ratevalveset3.Text) && 
                  Convert.ToInt16(ratevalveset3.Text) < Convert.ToInt16(ratevalveset4.Text) && 
                  Convert.ToInt16(ratevalveset4.Text) < Convert.ToInt16(ratevalveset5.Text)))
            {
                base.RegisterStartupScript("", "<script>alert('评分阀值不是递增取值');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (disablepostad.SelectedValue == "1" && disablepostadregular.Text == "")
            {
                base.RegisterStartupScript("", "<script>alert('新用户广告强力屏蔽正则表达式为空');window.location.href='forum_option.aspx';</script>");
                return;
            }
            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                __configinfo.Fullmytopics = Convert.ToInt16(fullmytopics.SelectedValue);
                __configinfo.Modworkstatus = Convert.ToInt16(modworkstatus.SelectedValue);
                __configinfo.Userstatusby = Convert.ToInt16(userstatusby.SelectedValue);
                if (Topicqueuestats_1.Checked == true)
                {
                    __configinfo.TopicQueueStats = 1;
                }
                else
                {
                    __configinfo.TopicQueueStats = 0;
                }
                __configinfo.TopicQueueStatsCount = Convert.ToInt32(topicqueuestatscount.Text);
                __configinfo.Guestcachepagetimeout = Convert.ToInt16(guestcachepagetimeout.Text);
                __configinfo.Topiccachemark = Convert.ToInt16(topiccachemark.Text);
                __configinfo.Losslessdel = Convert.ToInt16(losslessdel.Text);
                __configinfo.Edittimelimit = Convert.ToInt16(edittimelimit.Text);
                __configinfo.Editedby = Convert.ToInt16(editedby.SelectedValue);
                __configinfo.Defaulteditormode = Convert.ToInt16(defaulteditormode.SelectedValue);
                __configinfo.Allowswitcheditor = Convert.ToInt16(allowswitcheditor.SelectedValue);
                __configinfo.Reasonpm = Convert.ToInt16(reasonpm.SelectedValue);
                __configinfo.Hottopic = Convert.ToInt16(hottopic.Text);
                __configinfo.Starthreshold = Convert.ToInt16(starthreshold.Text);
                __configinfo.Fastpost = Convert.ToInt16(fastpost.SelectedValue);
                __configinfo.Tpp = Convert.ToInt16(tpp.Text);
                __configinfo.Ppp = Convert.ToInt16(ppp.Text);
                __configinfo.Htmltitle = Convert.ToInt32(allowhtmltitle.SelectedValue);
                __configinfo.Enabletag = Convert.ToInt32(enabletag.SelectedValue);
                __configinfo.Ratevalveset = ratevalveset1.Text + "," + ratevalveset2.Text + "," + ratevalveset3.Text + "," + ratevalveset4.Text + "," + ratevalveset5.Text;
                __configinfo.Statstatus = Convert.ToInt16(statstatus.SelectedValue);
                __configinfo.Statscachelife = Convert.ToInt16(statscachelife.Text);
                //__configinfo.Pvfrequence = Convert.ToInt16(pvfrequence.Text);
                __configinfo.Hottagcount = Convert.ToInt16(hottagcount.Text);
                __configinfo.Oltimespan = Convert.ToInt16(oltimespan.Text);
                __configinfo.Maxmodworksmonths = Convert.ToInt16(maxmodworksmonths.Text);
                __configinfo.Disablepostad = Convert.ToInt16(disablepostad.SelectedValue);
                __configinfo.Disablepostadregminute = Convert.ToInt16(disablepostadregminute.Text);
                __configinfo.Disablepostadpostcount = Convert.ToInt16(disablepostadpostcount.Text);
                __configinfo.Disablepostadregular = disablepostadregular.Text;

                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));
                TopicStats.SetQueueCount();
                AdminCaches.ReSetConfig();
                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "论坛功能常规选项设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='forum_option.aspx';");
            }
            #endregion
        }

        private bool ValidateRatevalveset(string val)
        {
            #region 验证值
            if (!Utils.IsNumeric(val))
            {
                base.RegisterStartupScript("", "<script>alert('评分各项阀值只能是数字');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            if (Convert.ToInt16(val) > 999 || Convert.ToInt16(val) < 1)
            {
                base.RegisterStartupScript("", "<script>alert('评分各项阀值只能在1-999之间');window.location.href='forum_option.aspx';</script>");
                return false;
            }
            else
                return true;
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion
    }
}
