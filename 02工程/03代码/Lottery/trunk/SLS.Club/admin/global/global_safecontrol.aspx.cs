using System;
using System.Web;
using System.Web.UI;
using System.Collections;
using System.IO;
using System.Web.UI.WebControls;

using Discuz.Control;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Common;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 安全控制
    /// </summary>
    
#if NET1
    public class safecontrol : AdminPage
#else
    public partial class safecontrol : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.TextBox seccodestatus;
        protected Discuz.Control.TextBox maxonlines;
        protected Discuz.Control.TextBox postinterval;
        protected Discuz.Control.TextBox searchctrl;
        protected Discuz.Control.TextBox maxspm;
        protected Discuz.Control.RadioButtonList secques;
        protected Discuz.Control.RadioButtonList admintools;
        protected Discuz.Control.Button SaveInfo;
        protected Discuz.Control.Hint Hint1;
		protected Discuz.Control.DropDownList VerifyImage;
        #endregion
#endif


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadConfigInfo();
                seccodestatus.Attributes.Add("style", "line-height:16px");
            }
        }

        public void LoadConfigInfo()
        {
            #region 加载配置信息
            //加载验证码的显示
            string[] dllFiles = System.IO.Directory.GetFiles(HttpRuntime.BinDirectory, "Discuz.Plugin.VerifyImage.*.dll");
            foreach (string dllFile in dllFiles)
            {
                string filename = dllFile.ToString().Substring(dllFile.ToString().IndexOf("Discuz.Plugin.VerifyImage")).Replace("Discuz.Plugin.VerifyImage.", "").Replace(".dll", "");
                VerifyImage.Items.Add(filename);
            }
            GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
            maxonlines.Text = __configinfo.Maxonlines.ToString();
            postinterval.Text = __configinfo.Postinterval.ToString();
            searchctrl.Text = __configinfo.Searchctrl.ToString();
            maxspm.Text = __configinfo.Maxspm.ToString();
            seccodestatus.AddAttributes("readonly", "");
            seccodestatus.Attributes.Add("onfocus", "this.className='';");
            seccodestatus.Attributes.Add("onblur", "this.className='';");
            admintools.SelectedValue = __configinfo.Admintools.ToString();
            VerifyImage.Items.Add(new ListItem("系统默认验证码", ""));            
            seccodestatus.Text = __configinfo.Seccodestatus.Replace(",", "\r\n");
            ViewState["Seccodestatus"] = __configinfo.Seccodestatus.ToString();
            VerifyImage.SelectedValue = __configinfo.VerifyImageAssemly;
            try
            {
                secques.SelectedValue = __configinfo.Secques.ToString();
            }
            catch
            {
                secques.SelectedValue = "1";
            }
            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                Hashtable HT = new Hashtable();
                HT.Add("最大在线人数", maxonlines.Text);
                HT.Add("发帖灌水预防", postinterval.Text);
                HT.Add("搜索时间限制", searchctrl.Text);
                HT.Add("60 秒最大搜索次数", maxspm.Text);
                foreach (DictionaryEntry de in HT)
                {
                    if (!Utils.IsInt(de.Value.ToString()))
                    {
                        base.RegisterStartupScript("", "<script>alert('输入错误:" + de.Key.ToString().Trim() + ",只能是0或者正整数');window.location.href='global_safecontrol.aspx';</script>");
                        return;
                    }
                }


                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));
                __configinfo.VerifyImageAssemly = VerifyImage.SelectedValue;
                __configinfo.Maxonlines = Convert.ToInt32(maxonlines.Text);
                __configinfo.Postinterval = Convert.ToInt32(postinterval.Text);
                __configinfo.Searchctrl = Convert.ToInt32(searchctrl.Text);
                __configinfo.Seccodestatus = seccodestatus.Text.Trim().Replace("\r\n", ",");
                __configinfo.Maxspm = Convert.ToInt32(maxspm.Text);
                __configinfo.Secques = Convert.ToInt32(secques.SelectedValue);
                __configinfo.Admintools = Convert.ToInt16(admintools.SelectedValue);
                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "安全控制设置", "");
                base.RegisterStartupScript( "PAGE","window.location.href='global_safecontrol.aspx';");
            }

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
        }

        #endregion

    }
}