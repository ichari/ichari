using System;
using System.Data;
using Discuz.Common;
using Discuz.Forum;
using System.Data.Common;
using Discuz.Config;
using Discuz.Web.UI;

namespace Discuz.Web
{
	/// <summary>
	/// 激活用户页面
	/// </summary>
	public partial class activationuser : PageBase
	{
		protected override void ShowPage()
		{
			
			pagetitle = "用户帐号激活";

			SetUrl("index.aspx");
			SetMetaRefresh();
			SetShowBackLink(false);
	
			string authstr = Utils.HtmlEncode(DNTRequest.GetString("authstr").Trim()).Replace("'","''");

			if(authstr != null && authstr != "")
			{
                DataTable dt = Discuz.Forum.Users.GetUserIdByAuthStr(authstr);
				if (dt.Rows.Count > 0)
				{
					int uid = Convert.ToInt32(dt.Rows[0][0].ToString());

					//将用户调整到相应的用户组
					if (UserCredits.GetCreditsUserGroupID(0) != null)
					{
						int tmpGroupID = UserCredits.GetCreditsUserGroupID(0).Groupid; //添加注册用户审核机制后需要修改
                        Discuz.Forum.Users.UpdateUserGroup(uid, tmpGroupID);                        
                    }

					//更新激活字段
                    Discuz.Forum.Users.UpdateAuthStr(uid, "", 0);
                   
					AddMsgLine("您当前的帐号已经激活,稍后您将以相应身份返回首页");

					ForumUtils.WriteUserCookie(uid, Utils.StrToInt(DNTRequest.GetString("expires"), -1), config.Passwordkey);
					OnlineUsers.UpdateAction(olid, UserAction.ActivationUser.ActionID, 0, config.Onlinetimeout);
	
				}
				else
				{
					AddMsgLine("您当前的激活链接无效,稍后您将以游客身份返回首页");
					OnlineUsers.DeleteRows(olid);
					ForumUtils.ClearUserCookie();
				}
			}
			else
			{
				AddMsgLine("您当前的激活链接无效,稍后您将以游客身份返回首页");
				OnlineUsers.DeleteRows(olid);
				ForumUtils.ClearUserCookie();
			}
		}

        override protected void OnLoad(EventArgs e)
        {

            /* 
                This page was created by Discuz!NT Template Engine at 2008/10/13 15:54:51.
                本页面代码由Discuz!NT模板引擎生成于 2008/10/13 15:54:51. 
            */

            base.OnLoad(e);

            templateBuilder.Append("<div id=\"foruminfo\">\r\n");
            templateBuilder.Append("	<div id=\"nav\">\r\n");
            templateBuilder.Append("		<a href=\"" + config.Forumurl.ToString().Trim() + "\">" + config.Forumtitle.ToString().Trim() + "</a>\r\n");
            templateBuilder.Append("	</div>\r\n");
            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("<br />\r\n");
            templateBuilder.Append("<br />\r\n");

            templateBuilder.Append("<div class=\"box message\">\r\n");
            templateBuilder.Append("	<h1>彩友提示信息</h1>\r\n");
            templateBuilder.Append("	<p>" + msgbox_text.ToString() + "</p>\r\n");

            if (msgbox_url != "")
            {

                templateBuilder.Append("	<p><a href=\"" + msgbox_url.ToString() + "\">如果浏览器没有转向, 请点击这里.</a></p>\r\n");

            }	//end if

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</div>\r\n");


            templateBuilder.Append("</div>\r\n");

            templateBuilder.Append("</div>\r\n");
            templateBuilder.Append("</body>\r\n");
            templateBuilder.Append("</html>\r\n");



            Response.Write(templateBuilder.ToString());
        }
	}
}
