using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// UserFactoryAdmin 的摘要说明。
	/// 后台用户信息操作管理类
	/// </summary>
	public class AdminUsers : Users
	{
		public AdminUsers()
		{
		}


		/// <summary>
		/// 更新用户全部信息
		/// </summary>
		/// <param name="__userinfo"></param>
		/// <returns></returns>
		public static bool UpdateUserAllInfo(UserInfo __userinfo)
		{
            DatabaseProvider.GetInstance().UpdateUserAllInfo(__userinfo);

			//当用户不是版主(超级版主)或管理员
			if ((__userinfo.Adminid == 0) || (__userinfo.Adminid > 3))
			{
				//删除用户在版主列表中相关数据
                DatabaseProvider.GetInstance().DeleteModerator(__userinfo.Uid);				

				//同时更新版块相关的版主信息
				UpdateForumsFieldModerators(__userinfo.Username);
			}

			#region 以下为更新该用户的扩展信息

			string signature = Utils.HtmlEncode(ForumUtils.BanWordFilter(__userinfo.Signature));

			UserGroupInfo usergroupinfo = AdminUserGroups.AdminGetUserGroupInfo(__userinfo.Groupid);
            GeneralConfigInfo config = GeneralConfigs.GetConfig();

			PostpramsInfo _postpramsinfo = new PostpramsInfo();
			_postpramsinfo.Usergroupid = usergroupinfo.Groupid;
			_postpramsinfo.Attachimgpost = config.Attachimgpost;
			_postpramsinfo.Showattachmentpath = config.Showattachmentpath;
			_postpramsinfo.Hide = 0;
			_postpramsinfo.Price = 0;
			_postpramsinfo.Sdetail = __userinfo.Signature;
			_postpramsinfo.Smileyoff = 1;
			_postpramsinfo.Bbcodeoff = 1 - usergroupinfo.Allowsigbbcode;
			_postpramsinfo.Parseurloff = 1;
			_postpramsinfo.Showimages = usergroupinfo.Allowsigimgcode;
			_postpramsinfo.Allowhtml = 0;
			_postpramsinfo.Smiliesinfo = Smilies.GetSmiliesListWithInfo();
			_postpramsinfo.Customeditorbuttoninfo = Editors.GetCustomEditButtonListWithInfo();
			_postpramsinfo.Smiliesmax = config.Smiliesmax;
			_postpramsinfo.Signature = 1;
			_postpramsinfo.Onlinetimeout = config.Onlinetimeout;

            DatabaseProvider.GetInstance().UpdateUserField(__userinfo, signature, ForumUtils.CreateAuthStr(20), UBB.UBBToHTML(_postpramsinfo));

			#endregion

			Users.UpdateUserForumSetting(__userinfo);

			return true;

		}

		/// <summary>
		/// 更新用户名
		/// </summary>
		/// <param name="__userinfo">当前用户信息</param>
		/// <param name="oldusername">以前用户的名称</param>
		/// <returns></returns>
		public static bool UserNameChange(UserInfo __userinfo, string oldusername)
		{
			//将新主题表
            DatabaseProvider.GetInstance().UpdateTopicLastPoster(__userinfo.Uid, __userinfo.Username);
            DatabaseProvider.GetInstance().UpdateTopicPoster(__userinfo.Uid, __userinfo.Username);

			//更新帖子表
            foreach (DataRow dr in DatabaseProvider.GetInstance().GetTableListIds())
			{
                DatabaseProvider.GetInstance().UpdatePostPoster(__userinfo.Uid, __userinfo.Username, dr["id"].ToString());
			}

			//更新短消息
            DatabaseProvider.GetInstance().UpdatePMSender(__userinfo.Uid, __userinfo.Username);
            DatabaseProvider.GetInstance().UpdatePMReceiver(__userinfo.Uid, __userinfo.Username);

			//更新公告
            DatabaseProvider.GetInstance().UpdateAnnouncementPoster(__userinfo.Uid, __userinfo.Username);

			//更新统计表中的信息
            if (DatabaseProvider.GetInstance().HasStatisticsByLastUserId(__userinfo.Uid))
			{

                DatabaseProvider.GetInstance().UpdateStatisticsLastUserName(__userinfo.Uid, __userinfo.Username);
				//更新缓存
                Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Statistics");
			}


			//更新论坛版主相关信息
			foreach (DataRow dr in DatabaseProvider.GetInstance().GetModerators(oldusername))
			{
				string moderators = "," + dr["moderators"].ToString().Trim() + ",";
				if (moderators.IndexOf("," + oldusername + ",") >= 0)
				{
                    DatabaseProvider.GetInstance().UpdateModerators(Utils.StrToInt(dr["fid"], 0), dr["moderators"].ToString().Trim().Replace(oldusername, __userinfo.Username));
				}
			}
			return true;
		}


		/// <summary>
		/// 删除指定用户的所有信息
		/// </summary>
		/// <param name="uid">指定的用户uid</param>
		/// <param name="delposts">是否删除帖子</param>
		/// <param name="delpms">是否删除短消息</param>
		/// <returns></returns>
		public static bool DelUserAllInf(int uid, bool delposts, bool delpms)
		{            
            bool val = DatabaseProvider.GetInstance().DelUserAllInf(uid, delposts, delpms);
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Statistics");
            return val;
		}


		/// <summary>
		/// 更新当前用户名在版块属性中的版主信息
		/// </summary>
		/// <param name="username">当前用户的名称</param>
		public static void UpdateForumsFieldModerators(string username)
		{
			//删除版主表的相关用户信息
            DataTable dt = DatabaseProvider.GetInstance().GetModeratorsTable(username);
			if (dt.Rows.Count > 0)
			{
				string updatestr = "";
				foreach (DataRow dr in dt.Rows)
				{
					updatestr = dr["moderators"].ToString().Replace(username + ",", "");
					updatestr = updatestr.Replace("," + username, "");
					updatestr = updatestr.Replace(username, "");
                    DatabaseProvider.GetInstance().UpdateModerators(Utils.StrToInt(dr["fid"], 0), updatestr);
				}
			}
		}


		/// <summary>
		/// 合并用户
		/// </summary>
		/// <param name="srcuid">源用户ID</param>
		/// <param name="targetuid">目标用户ID</param>
		/// <returns></returns>
		public static bool CombinationUser(int srcuid, int targetuid)
		{
			try
			{
				//积分合并
				UserInfo __srcuserinfo = Users.GetUserInfo(srcuid);
				UserInfo __targetuserinfo = Users.GetUserInfo(targetuid);
                DatabaseProvider.GetInstance().UpdateUserCredits(targetuid, __srcuserinfo.Credits + __targetuserinfo.Credits, __srcuserinfo.Extcredits1 + __targetuserinfo.Extcredits1, __srcuserinfo.Extcredits2 + __targetuserinfo.Extcredits2,
                                                __srcuserinfo.Extcredits3 + __targetuserinfo.Extcredits3, __srcuserinfo.Extcredits4 + __targetuserinfo.Extcredits4, __srcuserinfo.Extcredits5 + __targetuserinfo.Extcredits5,
                                                __srcuserinfo.Extcredits6 + __targetuserinfo.Extcredits6, __srcuserinfo.Extcredits7 + __targetuserinfo.Extcredits7, __srcuserinfo.Extcredits8 + __targetuserinfo.Extcredits8);

                DatabaseProvider.GetInstance().CombinationUser(Posts.GetPostTableName(), __targetuserinfo, __srcuserinfo);

				//删除被合并用户的所有相关信息
				DelUserAllInf(srcuid, true, true);

				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 通过用户名得到UID
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public static int GetuidByusername(string username)
		{
            return DatabaseProvider.GetInstance().GetuidByusername(username);
		}
	}
}