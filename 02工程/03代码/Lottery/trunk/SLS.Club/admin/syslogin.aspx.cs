using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using Discuz.Common;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// login 的摘要说明. 
    /// </summary>
    public partial class syslogin : Page
    {
        /// <summary>
        /// 当前登陆用户的在线ID
        /// </summary>
        public int olid;

        /// <summary>
        /// 论坛配置文件变量
        /// </summary>
        protected internal GeneralConfigInfo config;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region 进行权限判断

            int userid = Discuz.Forum.Users.GetUserIDFromCookie();

            if (userid > 0)
            {
                UserInfo u = Discuz.Forum.Users.GetUserInfo(userid);

                if (u.Adminid > 0 && u.Groupid > 0)
                {
                    Response.Redirect("index.aspx");

                    return;
                }
            }

            #endregion

            UserName.Attributes.Remove("class");
            PassWord.Attributes.Remove("class");
            UserName.AddAttributes("style", "width:200px");
           
            PassWord.AddAttributes("style", "width:200px");
           
            config = GeneralConfigs.GetConfig();

            OnlineUserInfo oluserinfo = OnlineUsers.UpdateInfo(config.Passwordkey, config.Onlinetimeout);

            olid = oluserinfo.Olid;


            if (!Page.IsPostBack)
            {

                #region 如果IP访问列表有设置则进行判断

                if (config.Adminipaccess.Trim() != "")
                {
                    string[] regctrl = Utils.SplitString(config.Adminipaccess, "\n");
                    if (!Utils.InIPArray(DNTRequest.GetIP(), regctrl))
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<br /><br /><div style=\"width:100%\" align=\"center\"><div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\">");
                        sb.Append("<img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" />&nbsp; 您的IP地址不在系统允许的范围之内</div></div>");
                        Response.Write(sb.ToString());
                        Response.End();
                        return;
                    }
                }

                #endregion


                #region 用户身份判断

                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(oluserinfo.Groupid);
                if (oluserinfo.Userid <= 0 || usergroupinfo.Radminid != 1)
                {
                    string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                    message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>无法确认您的身份</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                    message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><script type=\"text/javascript\">if(top.location!=self.location){top.location.href = \"syslogin.aspx\";}</script><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                    message += "<div align=\"center\" style=\"width:600px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                    message += "无法确认您的身份, 请<a href=\"../login.aspx\">登录</a></div></div></body></html>";
                    Response.Write(message);
                    Response.End();
                    return;
                }

                #endregion


                #region 判断安装目录文件信息

                if (IsExistsSetupFile())
                {
                    string message = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
                    message += "<html xmlns=\"http://www.w3.org/1999/xhtml\"><head><title>请将您的安装目录即install/目录下的文件全部删除, 以免其它用户运行安装该程序!</title><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">";
                    message += "<link href=\"styles/default.css\" type=\"text/css\" rel=\"stylesheet\"></head><script type=\"text/javascript\">if(top.location!=self.location){top.location.href = \"syslogin.aspx\";}</script><body><br /><br /><div style=\"width:100%\" align=\"center\">";
                    message += "<div align=\"center\" style=\"width:660px; border:1px dotted #FF6600; background-color:#FFFCEC; margin:auto; padding:20px;\"><img src=\"images/hint.gif\" border=\"0\" alt=\"提示:\" align=\"absmiddle\" width=\"11\" height=\"13\" /> &nbsp;";
                    message += "请将您的安装目录(install/)下和升级目录(upgrade/)下的.aspx文件全部删除, 以免其它用户运行安装或升级程序!</div></div></body></html>";
                    Response.Write(message);
                    Response.End();
                    return;
                }

                #endregion

               

                #region 显示相关页面登陆提交信息

                if (Context.Request.Cookies["dntadmin"] == null || Context.Request.Cookies["dntadmin"]["key"] == null || ForumUtils.GetCookiePassword(Context.Request.Cookies["dntadmin"]["key"].ToString(), config.Passwordkey) != (oluserinfo.Password + Discuz.Forum.Users.GetUserInfo(oluserinfo.Userid).Secques + oluserinfo.Userid.ToString()))
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\">请重新进行管理员登录";
                }

                if (oluserinfo.Userid > 0 && usergroupinfo.Radminid == 1 && oluserinfo.Username.Trim() != "")
                {
                    UserName.Text = oluserinfo.Username;
                    UserName.AddAttributes("readonly", "true");
                    UserName.CssClass = "nofocus";
                    UserName.Attributes.Add("onfocus", "this.className='nofocus';");
                    UserName.Attributes.Add("onblur", "this.className='nofocus';");
                }

                if (DNTRequest.GetString("result") == "1")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不存在或密码错误</font>";
                    return;
                }

                if (DNTRequest.GetString("result") == "2")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">用户不是管理员身分,因此无法登陆后台</font>";
                    return;
                }

                if (DNTRequest.GetString("result") == "3")
                {
                    Msg.Text = "<IMG alt=\"提示:\" src=\"images/warning.gif\" align=\"absMiddle\" border=\"0\" width=\"16\" height=\"16\"><font color=\"red\">验证码错误,请重新输入</font>";
                    return;
                }


                if (DNTRequest.GetString("result") == "4")
                {
                    Msg.Text = "";
                    return;
                }

                #endregion

            }

            if (Page.IsPostBack)
            {
                //对提供的信息进行验证
                VerifyLoginInf();
            }
            else
            {
                Response.Redirect("syslogin.aspx?result=4");
            }
        }


        /// <summary>
        /// 检查安装用录下是否有安装文件,但有就删除,如删除出问题就返回true
        /// </summary>
        /// <returns></returns>
        public bool IsExistsSetupFile()
        {
            //初始值为false表示无安装文件
            bool flag = false;

            #region 检查安装目录

            string[] installFiles = {"install/index.aspx","install/step2.aspx","install/step3.aspx","install/step4.aspx","install/succeed.aspx",
                                     "install/systemfile.aspx","install/pluginsetup.aspx","install/album.xml","install/space.xml","install/mall.xml"
                                    };
            foreach(string file in installFiles)
            {
                string f = AppDomain.CurrentDomain.BaseDirectory + file;

                flag = CheckAndDeleteFile(f);
                if (flag)
                {
                    return true;
                }
            }
          
            #endregion

            #region 检查升级目录

            string[] upgradeFiles = { "upgrade/index.aspx", "upgrade/finish.aspx", "upgrade/upgrade.aspx", "upgrade/systemfile.aspx" };

            foreach (string file in upgradeFiles)
            {
                string f = AppDomain.CurrentDomain.BaseDirectory + file;

                flag = CheckAndDeleteFile(f);
                if (flag)
                {
                    return true;
                } 
            }

            #endregion

            return flag;
        }


        //检查并删除指定路径的文件
        private bool CheckAndDeleteFile(string path)
        {
            if (Utils.FileExists(path))
            {
                try
                {
                    File.Delete(path);
                    return false;
                }
                catch
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }


        public void VerifyLoginInf()
        {
            if (!OnlineUsers.CheckUserVerifyCode(olid, DNTRequest.GetString("vcode")))
            {
                Response.Redirect("syslogin.aspx?result=3");
                return;
            }

            DataTable dt = new DataTable();

            if (config.Passwordmode == 1)
            {
                int uid = Discuz.Forum.Users.CheckDvBbsPassword(DNTRequest.GetString("username"), DNTRequest.GetString("password"));

                dt = DatabaseProvider.GetInstance().GetUserInfo(uid);
            }
            else
            {
                
                dt = DatabaseProvider.GetInstance().GetUserInfo(UserName.Text.Trim(), Utils.MD5(PassWord.Text.Trim()));
            }


            if (dt.Rows.Count > 0)
            {
                UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(Convert.ToInt32(dt.Rows[0]["groupid"].ToString()));

                if (usergroupinfo.Radminid == 1)
                {
                    ForumUtils.WriteUserCookie(Convert.ToInt32(dt.Rows[0]["uid"].ToString().Trim()), 1440, GeneralConfigs.GetConfig().Passwordkey);

                    int userid = Convert.ToInt32(dt.Rows[0]["uid"].ToString().Trim());
                    string username = UserName.Text.Trim();
                    int usergroupid = Convert.ToInt16(dt.Rows[0]["groupid"].ToString().Trim());
                    string secques = dt.Rows[0]["secques"].ToString().Trim();
                    string ip = DNTRequest.GetIP();

                    UserGroupInfo __usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(usergroupid);

                    string grouptitle = __usergroupinfo.Grouptitle;


                    HttpCookie cookie = new HttpCookie("dntadmin");
                    cookie.Values["key"] = ForumUtils.SetCookiePassword(Utils.MD5(PassWord.Text.Trim()) + secques + userid.ToString(), config.Passwordkey);
                    cookie.Expires = DateTime.Now.AddMinutes(30);
                    HttpContext.Current.Response.AppendCookie(cookie);

                    AdminVistLogs.InsertLog(userid, username, usergroupid, grouptitle, ip, "后台管理员登陆", "");

                    try
                    {
                        SoftInfo.LoadSoftInfo();
                    }
                    catch
                    {
                        Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                        Response.End();
                    }

                    //升级general.config文件
                    try
                    {
                        GeneralConfigs.Serialiaze(GeneralConfigs.GetConfig(), Server.MapPath("../config/general.config"));
                    }
                    catch { }


                    Response.Write("<script type=\"text/javascript\">top.location.href='index.aspx';</script>");
                    Response.End();
                    return;
                }
                else
                {
                    Response.Redirect("syslogin.aspx?result=2");
                    return;
                }
            }
            else
            {
                Response.Redirect("syslogin.aspx?result=1");
                return;
            }
        }

        #region Web 窗体设计器生成的代码

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        private void InitializeComponent()
        {
        }

        #endregion


        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
            base.SavePageStateToPersistenceMedium(viewState);
        }

        protected override object LoadPageStateFromPersistenceMedium()
        {
            object o = new object();
            try
            {
                o = base.LoadPageStateFromPersistenceMedium();
            }
            catch
            {
                o = null;
            }
            return o;
        }


    }
}