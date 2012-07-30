using System;
using System.Web;
using Discuz.Common;
using Discuz.Forum;

using Discuz.Config;
using Discuz.Data;
using System.Data;
using Discuz.Entity;

namespace Discuz.Web.Admin
{
    /// <summary>
    /// 进行论坛统计
    /// </summary>
    
    public partial class ajaxcall : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                int pertask = DNTRequest.GetInt("pertask", 0);
                int lastnumber = DNTRequest.GetInt("lastnumber", 0);
                int startvalue = DNTRequest.GetInt("startvalue", 0);
                int endvalue = DNTRequest.GetInt("endvalue", 0);
                string resultmessage = "";
                switch (Request.Params["opname"])
                {
                    case "ReSetFourmTopicAPost":
                        AdminForumStats.ReSetFourmTopicAPost(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetUserDigestPosts":
                        AdminForumStats.ReSetUserDigestPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetUserPosts":
                        AdminForumStats.ReSetUserPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetTopicPosts":
                        AdminForumStats.ReSetTopicPosts(pertask, ref lastnumber);
                        resultmessage = lastnumber.ToString();
                        break;
                    case "ReSetFourmTopicAPost_StartEnd":
                        AdminForumStats.ReSetFourmTopicAPost(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserDigestPosts_StartEnd":
                        AdminForumStats.ReSetUserDigestPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetUserPosts_StartEnd":
                        AdminForumStats.ReSetUserPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ReSetTopicPosts_StartEnd":
                        AdminForumStats.ReSetTopicPosts(startvalue, endvalue);
                        resultmessage = "1";
                        break;
                    case "ftptest":
                        string serveraddress = DNTRequest.GetString("serveraddress");
                        string serverport = DNTRequest.GetString("serverport");
                        string username = DNTRequest.GetString("username");
                        string password = DNTRequest.GetString("password");
                        string timeout = DNTRequest.GetString("timeout");
                        string uploadpath = DNTRequest.GetString("uploadpath");
                        FTPs ftps = new FTPs();
                        string message = "";
                        bool ok = ftps.TestConnect(serveraddress, int.Parse(serverport), username, password, int.Parse(timeout), uploadpath, ref message);
                        if (ok)
                        {
                            resultmessage = "ok";
                        }
                        else
                        {
                            resultmessage = "远程附件设置测试出现错误！\n描述：" + message;
                        }
                        break;
                    case "setapp":
                        string allowpassport = DNTRequest.GetString("allowpassport");
                        APIConfigInfo aci = APIConfigs.GetConfig();
                        aci.Enable = allowpassport == "1";
                        APIConfigs.SaveConfig(aci);
                        resultmessage = "ok";
                        break;
                    
                }
                Response.Write(resultmessage);
                Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
                Response.Expires = -1;
                Response.End();
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
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion
    }
}