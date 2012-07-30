using System.Collections;
using System;
using System.Web.UI;

using Discuz.Control;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;


namespace Discuz.Web.Admin
{
    /// <summary>
    /// 注册与访问控制
    /// </summary>
 
#if NET1
    public class registerandvisit : AdminPage
#else
    public partial class registerandvisit : AdminPage
#endif
    {

#if NET1
        #region 控件声明
        protected Discuz.Control.RadioButtonList regstatus;
        protected Discuz.Control.RadioButtonList regadvance;
        protected Discuz.Control.RadioButtonList realnamesystem;
        protected Discuz.Control.RadioButtonList doublee;
        protected Discuz.Web.Admin.TextareaResize censoruser;
        protected Discuz.Web.Admin.TextareaResize ipregctrl;
        protected Discuz.Control.RadioButtonList regverify;
        protected Discuz.Web.Admin.TextareaResize accessemail;
        protected Discuz.Web.Admin.TextareaResize censoremail;
        protected Discuz.Control.TextBox regctrl;
        protected Discuz.Control.TextBox newbiespan;
        protected Discuz.Control.RadioButtonList welcomemsg;
        protected Discuz.Web.Admin.TextareaResize welcomemsgtxt;
        protected Discuz.Control.RadioButtonList rules;
        protected Discuz.Web.Admin.TextareaResize rulestxt;
        protected Discuz.Control.RadioButtonList hideprivate;
        protected Discuz.Web.Admin.TextareaResize ipdenyaccess;
        protected Discuz.Web.Admin.TextareaResize ipaccess;
        protected Discuz.Web.Admin.TextareaResize adminipaccess;
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

            regstatus.SelectedValue = __configinfo.Regstatus.ToString();
            censoruser.Text = __configinfo.Censoruser.ToString();
            doublee.SelectedValue = __configinfo.Doublee.ToString();
            regverify.SelectedValue = __configinfo.Regverify.ToString();
            accessemail.Text = __configinfo.Accessemail.ToString();
            censoremail.Text = __configinfo.Censoremail.ToString();
            hideprivate.SelectedValue = __configinfo.Hideprivate.ToString();
            ipdenyaccess.Text = __configinfo.Ipdenyaccess.ToString();
            ipaccess.Text = __configinfo.Ipaccess.ToString();
            regctrl.Text = __configinfo.Regctrl.ToString();
            ipregctrl.Text = __configinfo.Ipregctrl.ToString();
            adminipaccess.Text = __configinfo.Adminipaccess.ToString();
            welcomemsg.SelectedValue = __configinfo.Welcomemsg.ToString();
            welcomemsgtxt.Text = __configinfo.Welcomemsgtxt.ToString();
            rules.SelectedValue = __configinfo.Rules.ToString();
            rulestxt.Text = __configinfo.Rulestxt.ToString();
            newbiespan.Text = __configinfo.Newbiespan.ToString();
            regadvance.SelectedValue = __configinfo.Regadvance.ToString();
            realnamesystem.SelectedValue = __configinfo.Realnamesystem.ToString();

            #endregion
        }

        private void SaveInfo_Click(object sender, EventArgs e)
        {
            #region 保存设置信息

            if (this.CheckCookie())
            {
                GeneralConfigInfo __configinfo = GeneralConfigs.Deserialize(Server.MapPath("../../config/general.config"));

                __configinfo.Regstatus = Convert.ToInt16(regstatus.SelectedValue);
                __configinfo.Censoruser = DelNullRowOrSpace(censoruser.Text);
                __configinfo.Doublee = Convert.ToInt16(doublee.SelectedValue);
                __configinfo.Regverify = Convert.ToInt16(regverify.SelectedValue);
                __configinfo.Accessemail = accessemail.Text;
                __configinfo.Censoremail = censoremail.Text;
                __configinfo.Hideprivate = Convert.ToInt16(hideprivate.SelectedValue);
                __configinfo.Ipdenyaccess = ipdenyaccess.Text;
                __configinfo.Ipaccess = ipaccess.Text;
                __configinfo.Regctrl = Convert.ToInt16(regctrl.Text);
                __configinfo.Ipregctrl = ipregctrl.Text;
                __configinfo.Adminipaccess = adminipaccess.Text;
                __configinfo.Welcomemsg = Convert.ToInt16(welcomemsg.SelectedValue);
                __configinfo.Welcomemsgtxt = welcomemsgtxt.Text;
                __configinfo.Rules = Convert.ToInt16(rules.SelectedValue);
                __configinfo.Rulestxt = rulestxt.Text;
                __configinfo.Newbiespan = Convert.ToInt16(newbiespan.Text);
                __configinfo.Regadvance = Convert.ToInt16(regadvance.SelectedValue);
                __configinfo.Realnamesystem = Convert.ToInt16(realnamesystem.SelectedValue);

             
                    //  foreach (DictionaryEntry str in Emailhash)
               // {

               //     try
               //     {
               //         string[] singlemail = str.Value.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

               //         foreach (string mail in singlemail)
               //{
               //   if(mail!="")
               //    if (Utils.IsValidEmail(mail.ToString()) == false)
               //         throw new Exception();
                    
               //     }
               //}
               //     catch
               //     {

               //         base.RegisterStartupScript("erro", "<script>alert('" + str.Key.ToString() + ",EMAIL格式错误');</script>");
               //         return;
               //     }
               //} 


                 Hashtable IPHash = new Hashtable();
                 IPHash.Add("特殊 IP 注册限制", ipregctrl.Text);
                 IPHash.Add("IP 禁止访问列表", ipdenyaccess.Text);
                 IPHash.Add("IP 访问列表", ipaccess.Text);
                 IPHash.Add("管理员后台IP访问列表", adminipaccess.Text);

                 //foreach (DictionaryEntry str in IPHash)
                 //{

                 //    try
                 //    {
                 //        string[] singleip = str.Value.ToString().Split(new string[] { "\r\n" }, StringSplitOptions.None);

                 //        foreach (string ip in singleip)
                 //        {
                 //            if(ip!="")
                 //            if (Utils.IsIP(ip.ToString()) == false)
                 //                throw new Exception();

                 //        }
                 //    }
                 //    catch
                 //    {

                 //        base.RegisterStartupScript("erro", "<script>alert('" + str.Key.ToString() + ",IP格式错误');</script>");
                 //        return;
                 //    }
                 //} 
                 string ipkey = "";
                 if (Utils.IsRuleTip(IPHash, "ip", out ipkey) == false)
                 {
                     base.RegisterStartupScript("erro", "<script>alert('" + ipkey.ToString() + ",IP格式错误');</script>");
                     return;
                 }

                 Hashtable Emailhash = new Hashtable();
                 Emailhash.Add("Email 允许地址", accessemail.Text);
                 Emailhash.Add("Email 禁止地址", censoremail.Text);

                 string key = "";
                 if (Utils.IsRuleTip(Emailhash, "email", out key) == false)
                 {
                     base.RegisterStartupScript("erro", "<script>alert('" + key.ToString() + ",Email格式错误');</script>");
                     return;
                 }
                
                GeneralConfigs.Serialiaze(__configinfo, Server.MapPath("../../config/general.config"));

                //AdminCaches.ReSetSiteUrls();

                AdminVistLogs.InsertLog(this.userid, this.username, this.usergroupid, this.grouptitle, this.ip, "注册与访问控制设置", "");
                base.RegisterStartupScript( "PAGE",  "window.location.href='global_registerandvisit.aspx';");
            }

            #endregion
        }

        public string DelNullRowOrSpace(string desStr)
        {
            #region 删除空行
            string result = "";
            foreach (string str in Utils.SplitString(desStr.Replace(" ", ""), "\r\n"))
            {
                if (str.Trim() != "")
                {
                    if (result == "")
                    {
                        result = str;
                    }
                    else
                    {
                        result = result + "\r\n" + str;
                    }
                }
            }
            return result;
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
            this.SaveInfo.Click += new EventHandler(this.SaveInfo_Click);
            this.Load += new EventHandler(this.Page_Load);

        }

        #endregion

    }
}