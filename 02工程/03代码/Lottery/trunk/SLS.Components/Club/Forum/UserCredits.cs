using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// UserCreditsFactory 的摘要说明。
	/// </summary>
	public class UserCredits
	{

		/// <summary>
		/// 获得系统设置的总积分计算公式
		/// </summary>
		/// <returns>计算公式</returns>
		private static string GetCreditsArithmetic(int uid)
		{
            string ArithmeticStr = Scoresets.GetScoreCalFormula();
			if (ArithmeticStr.Equals(""))
			{
				return "0";
			}
			string[] para = {
								"digestposts",
								"posts",
								"oltime",
								"pageviews",
								"extcredits1",
								"extcredits2",
								"extcredits3",
								"extcredits4",
								"extcredits5",
								"extcredits6",
								"extcredits7",
								"extcredits8"
							};


			IDataReader  reader = Users.GetShortUserInfoToReader(uid);
			if (reader != null)
			{
				if (reader.Read())
				{
					for (int i = 0; i < para.Length; i++)
					{
						ArithmeticStr = ArithmeticStr.Replace(para[i],Utils.StrToFloat(reader[para[i]],0).ToString());
					}
				}
				reader.Close();
			}
			return ArithmeticStr;
		}


		/// <summary>
		/// 根据积分公式更新用户积分,并且受分数变动影响有可能会更改用户所属的用户组
		/// <param name="uid">用户ID</param>
		/// </summary>
		public static int UpdateUserCredits(int uid)
		{
			UserInfo tmpUserInfo = Users.GetUserInfo(uid);
			if (tmpUserInfo == null)
			{
				return 0;
			}

            DatabaseProvider.GetInstance().UpdateUserCredits(uid, Scoresets.GetScoreCalFormula());
			tmpUserInfo = Users.GetUserInfo(uid);
			UserGroupInfo tmpUserGroupInfo = UserGroups.GetUserGroupInfo(tmpUserInfo.Groupid);

            if (tmpUserGroupInfo != null && tmpUserGroupInfo.System == 0 && tmpUserGroupInfo.Radminid == 0)
            {
                tmpUserGroupInfo = GetCreditsUserGroupID(tmpUserInfo.Credits);
                DatabaseProvider.GetInstance().UpdateUserGroup(uid, tmpUserGroupInfo.Groupid);
                OnlineUsers.UpdateGroupid(uid, tmpUserGroupInfo.Groupid);
            }
            else 
            {
                //当用户是已删除的特殊组成员时，则运算相应积分，并更新该用户所属组信息
                if (tmpUserGroupInfo != null && tmpUserGroupInfo.Groupid == 7 && tmpUserInfo.Adminid == -1)
                {
                    tmpUserGroupInfo = GetCreditsUserGroupID(tmpUserInfo.Credits);
                    DatabaseProvider.GetInstance().UpdateUserGroup(uid, tmpUserGroupInfo.Groupid);
                    OnlineUsers.UpdateGroupid(uid, tmpUserGroupInfo.Groupid);
                }
            }
			return 1;

		}


		/// <summary>
		/// 更新用户积分(适用于单个操作)
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		private static int UpdateUserCredits(int uid, CreditsOperationType creditsOperationType, int pos)
		{
			return UpdateUserCredits(uid, 1, creditsOperationType, pos);
		}

		/// <summary>
		/// 通过指定值更新用户积分
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
		/// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
		/// <returns></returns>
		private static int UpdateUserCredits(int uid,float[] values, bool allowMinus)
		{
			UserInfo tmpUserInfo = Users.GetUserInfo(uid);
			if (tmpUserInfo == null)
			{
				return 0;
			}
			
			if (values.Length < 8)
			{
				return -1;
			}
			if (!allowMinus)//不允许扣成负分时要进行判断积分是否足够被减
			{
				// 如果要减扩展积分, 首先判断扩展积分是否足够被减
                if (!DatabaseProvider.GetInstance().CheckUserCreditsIsEnough(uid, values))
                {
                    return -1;
                }
			}

            DatabaseProvider.GetInstance().UpdateUserCredits(uid, values);
			
			///更新用户积分
			return UpdateUserCredits(uid);
		}

		/// <summary>
		/// 通过指定值更新用户积分(积分不够时不扣,返回-1)
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
		private static int UpdateUserCredits(int uid,float[] values)
		{
			return UpdateUserCredits(uid,values,false);
		}

        /// <summary>
        /// 检查用户积分是否足够被减(适用于单用户, 单个或多个积分)
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
        /// <param name="creditsOperationType">积分操作类型,如发帖等</param>
        /// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
        /// <returns></returns>
        public static bool CheckUserCreditsIsEnough(int uid, int mount, CreditsOperationType creditsOperationType, int pos)
        {
            DataTable dt = Scoresets.GetScoreSet();
            DataColumn[] keys = new DataColumn[1];
            keys[0] = dt.Columns["id"];
            dt.PrimaryKey = keys;
            DataRow dr = dt.Rows[(int)creditsOperationType];

            for (int i = 2; i < 10; i++)
            {
                if (Utils.StrToFloat(dr[i], 0) < 0)//只要任何一项要求减分,就去数据库检查
                {
                    return DatabaseProvider.GetInstance().CheckUserCreditsIsEnough(uid, dr, pos, mount);
                }
            }
            return true;           
        }

		/// <summary>
		/// 更新用户积分(适用于单用户,单个或多个操作)
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		/// <param name="allowMinus">是否允许被扣成负分,true允许,false不允许并且不进行扣分返回-1</param>
		/// <returns></returns>
		private static int UpdateUserCredits(int uid, int mount, CreditsOperationType creditsOperationType, int pos, bool allowMinus)
		{
			if (!Users.Exists(uid))
			{
				return 0;
			}

            DataTable dt = Scoresets.GetScoreSet();
			DataColumn[] keys = new DataColumn[1];
			keys[0] = dt.Columns["id"];
			dt.PrimaryKey = keys;
			DataRow dr = dt.Rows[(int)creditsOperationType];

			// 如果要减扩展积分, 首先判断扩展积分是否足够被减
            if (pos < 0)
            {
                //当不是删除主题或回复时
                if (creditsOperationType != CreditsOperationType.PostTopic && creditsOperationType != CreditsOperationType.PostReply)
                {
                    if (!allowMinus && !DatabaseProvider.GetInstance().CheckUserCreditsIsEnough(uid, dr, pos, mount))
                    {
                        return -1;
                    }
                }
            }
            DatabaseProvider.GetInstance().UpdateUserCredits(uid, dr, pos, mount);

			///更新用户积分
			return UpdateUserCredits(uid);

		}


		/// <summary>
		/// 更新用户积分(当扣分时,如果用户剩余分值不足,则不扣)
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="mount">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		private static int UpdateUserCredits(int uid,int mount, CreditsOperationType creditsOperationType, int pos)
		{
			return UpdateUserCredits(uid, mount, creditsOperationType, pos, false);
		}



		/// <summary>
		/// 根据用户列表,一次更新多个用户的积分
		/// </summary>
		/// <param name="uidlist">用户ID列表</param>
		/// <param name="values">扩展积分值</param>
		private static int UpdateUserCredits(string uidlist,float[] values)
		{
	

			if (Utils.IsNumericArray(Utils.SplitString(uidlist,","))){
				///根据公式计算用户的总积分,并更新
				int reval = 0;
				foreach(string uid in Utils.SplitString(uidlist,","))
				{
					if (Utils.StrToInt(uid,0) > 0)
					{
						reval = reval + UpdateUserCredits(Utils.StrToInt(uid,0),values, true);
					}
				}
				
				return reval;
			}
			return -1;

		}


		
		/// <summary>
		/// 根据用户列表,一次更新多个用户的积分
		/// </summary>
		/// <param name="uidlist">用户ID列表</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		private static int UpdateUserCredits(string uidlist, CreditsOperationType creditsOperationType,int pos)
		{
			if (Utils.IsNumericArray(Utils.SplitString(uidlist, ",")))
			{
				///根据公式计算用户的总积分,并更新
				int reval = 0;
				foreach(string uid in Utils.SplitString(uidlist, ","))
				{
					if (Utils.StrToInt(uid,0) > 0)
					{
						reval = reval + UpdateUserCredits(Utils.StrToInt(uid, 0), 1, creditsOperationType, pos);
					}
				}
				
				return reval;
			}
			return 0;

		}


		/// <summary>
		/// 根据用户列表,一次更新多个用户的积分(此方法只在删除主题时使用过)
		/// </summary>
		/// <param name="uidlist">用户ID列表</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		private static int UpdateUserCredits(int[] uidlist, CreditsOperationType creditsOperationType,int pos)
		{
			///根据公式计算用户的总积分,并更新
			int reval = 0;
			for (int i = 0; i < uidlist.Length; i++)
			{
				if (uidlist[i] > 0)
				{
					reval = reval + UpdateUserCredits(uidlist[i], 1, creditsOperationType, pos, true);
				}
			}
			
			return reval;
		}

		/// <summary>
		/// 根据用户列表,一次更新多个用户的积分(此方法只在删除主题时使用过)
		/// </summary>
		/// <param name="uidlist">用户ID列表</param>
		/// <param name="mountlist">更新数量,比如由上传2个附件引发此操作,那么此参数值应为2,数组长度应与uidlist相同</param>
		/// <param name="creditsOperationType">积分操作类型,如发帖等</param>
		/// <param name="pos">加或减标志(正数为加,负数为减,通常被传入1或者-1)</param>
		private static int UpdateUserCredits(int[] uidlist, int[] mountlist, CreditsOperationType creditsOperationType, int pos)
		{
			///根据公式计算用户的总积分,并更新
			int reval = 0;
			for (int i = 0; i < uidlist.Length; i++)
			{
				if (uidlist[i] > 0)
				{
					reval = reval + UpdateUserCredits(uidlist[i], mountlist[i], creditsOperationType, pos, true);
				}
			}
				
			return reval;
		}
		
		
		/// <summary>
		/// 根据积分获得积分用户组所应该匹配的用户组描述 (如果没有匹配项或用户非积分用户组则返回null)
		/// </summary>
		/// <param name="Credits">积分</param>
		/// <returns>用户组描述</returns>
		public static UserGroupInfo GetCreditsUserGroupID(float Credits)
		{
			UserGroupInfo[] usergroupinfo = UserGroups.GetUserGroupList();
			UserGroupInfo tmpitem = null;

			foreach (UserGroupInfo infoitem in usergroupinfo)
			{
				// 积分用户组的特征是radminid等于0
				if (infoitem.Radminid == 0 && infoitem.System == 0 && (Credits >= infoitem.Creditshigher && Credits <= infoitem.Creditslower))
				{
					if (tmpitem == null || infoitem.Creditshigher > tmpitem.Creditshigher)
					{
						tmpitem = infoitem;
					}
				}
			}

			return tmpitem == null ? new UserGroupInfo() : tmpitem;
		}



		/// <summary>
		/// 用户发表主题时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByPostTopic(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.PostTopic, 1);

		}

		/// <summary>
		/// 用户发表主题时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="values">积分变动值,应保证是一个长度为8的数组,对应8种扩展积分的变动值</param>
		public static int UpdateUserCreditsByPostTopic(int uid,float[] values)
		{
			return UpdateUserCredits(uid, values);

		}

		/// <summary>
		/// 用户发表回复时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByPosts(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.PostReply, 1);
		}

		/// <summary>
		/// 用户发表回复时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="values">自定义积分值列表</param>
		public static int UpdateUserCreditsByPosts(int uid,float[] values)
		{
			return UpdateUserCredits(uid, values);
		}

		
		/// <summary>
		/// 用户发表回复时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByPosts(int uid,int pos)
		{
			return UpdateUserCredits(uid, CreditsOperationType.PostReply, pos);
		}

		/// <summary>
		/// 用户所发表的主题或帖子被置为精华时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="mount">被置为精华的主题或帖子数量</param>
		public static int UpdateUserCreditsByDigest(int uid, int mount)
		{
			return UpdateUserCredits(uid, mount, CreditsOperationType.Digest, 1, true);
		}

		/// <summary>
		/// 用户所发表的主题或帖子被置为精华时更新用户的积分
		/// </summary>
		/// <param name="uidlist">用户id列表</param>
		/// <param name="mount">被置为精华的主题或帖子数量</param>
		public static int UpdateUserCreditsByDigest(string uidlist, int mount)
		{
			if (!uidlist.Equals(""))
			{
				if (Utils.IsNumericArray(uidlist.Split(',')))
				{
					return UpdateUserCredits(uidlist, CreditsOperationType.Digest, 1);
				}
			}
			return 0;
		}

		
		/// <summary>
		/// 用户所发表的主题或帖子被置为精华时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByDigest(int uid)
		{
			return UpdateUserCreditsByUploadAttachment(uid, 1);
		}

		/// <summary>
		/// 用户上传附件时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="mount">上传附件数量</param>
		public static int UpdateUserCreditsByUploadAttachment(int uid, int mount)
		{
			return UpdateUserCredits(uid, mount, CreditsOperationType.UploadAttachment, 1);
		}

		/// <summary>
		/// 用户上传附件时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByUploadAttachment(int uid)
		{
			return UpdateUserCreditsByUploadAttachment(uid, 1);
		}


		/// <summary>
		/// 用户下载附件时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="mount">下载附件数量</param>
		public static int UpdateUserCreditsByDownloadAttachment(int uid, int mount)
		{
			return UpdateUserCredits(uid, mount, CreditsOperationType.DownloadAttachment, -1);
		}

		/// <summary>
		/// 用户下载附件时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByDownloadAttachment(int uid)
		{
			return UpdateUserCreditsByDownloadAttachment(uid, 1);
		}
		

		/// <summary>
		/// 用户发送短消息时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsBySendpms(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.SendMessage, 1);
		}


		/// <summary>
		/// 用户搜索时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsBySearch(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.Search, -1);
		}



		/// <summary>
		/// 用户交易成功时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByTradefinished(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.TradeSucceed, 1);

		}

		/// <summary>
		/// 用户参与投票时更新用户的积分
		/// </summary>
		/// <param name="uid">用户id</param>
		public static int UpdateUserCreditsByVotepoll(int uid)
		{
			return UpdateUserCredits(uid, CreditsOperationType.Vote, 1);

		}

		/// <summary>
		/// 用户对帖子评分时更新用户的积分
		/// </summary>
		/// <param name="useridlist">用户id</param>
		public static int UpdateUserCreditsByRate(string useridlist,float[] extcreditslist)
		{
			return UpdateUserCredits(useridlist, extcreditslist);

		}

		/// <summary>
		/// 版主删除论坛主题
		/// </summary>
		/// <param name="tuidlist">要删除的主题用户id</param>
		/// <param name="auidlist">要删除的主题对应的的附件数量,应与tuidlist长度相同</param>
		public static int UpdateUserCreditsByDeleteTopic(int[] tuidlist,int[] auidlist,int pos)
		{
			return UpdateUserCredits(tuidlist, CreditsOperationType.PostTopic, pos) + UpdateUserCredits(tuidlist, auidlist, CreditsOperationType.UploadAttachment, pos);
		}

		/// <summary>
		/// 根据用户Id获取用户积分
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>用户积分</returns>
		public static int GetUserCreditsByUid(int uid)
		{
			///根据公式计算用户的总积分,并更新
			string ExpressionStr = GetCreditsArithmetic(uid);
            return Utils.StrToInt(Math.Floor(Utils.StrToFloat(Arithmetic.ComputeExpression(ExpressionStr), 0)), 0);
		}
    } // end class
}
