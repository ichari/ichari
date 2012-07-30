using System;
using System.Data;
using System.Data.Common;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
	/// <summary>
	/// AdminUserGroupFactory 的摘要说明。
	/// 后台用户组管理操作类
	/// </summary>
	public class AdminUserGroups : UserGroups
	{
		public static string opresult = ""; //存储操作结果或返回给用户的信息


		public AdminUserGroups()
		{
		}


		/// <summary>
		/// 通过指定的用户组id得到相关的用户组信息
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static UserGroupInfo AdminGetUserGroupInfo(int groupid)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetUserGroup(groupid);
			UserGroupInfo __info = new UserGroupInfo();
			if (dt.Rows.Count > 0)
			{
				foreach (DataRow dr in dt.Rows)
				{
					__info.Groupid = Int16.Parse(dr["groupid"].ToString());
					__info.Radminid = Int32.Parse(dr["radminid"].ToString());
					__info.Type = Int16.Parse(dr["type"].ToString());
					__info.System = Int16.Parse(dr["system"].ToString());
					__info.Color = dr["color"].ToString().Trim();
					__info.Grouptitle = dr["grouptitle"].ToString().Trim();
					__info.Creditshigher = Int32.Parse(dr["creditshigher"].ToString());
					__info.Creditslower = Int32.Parse(dr["creditslower"].ToString());
					__info.Stars = Int32.Parse(dr["stars"].ToString());
					__info.Groupavatar = dr["groupavatar"].ToString();
					__info.Readaccess = Int32.Parse(dr["readaccess"].ToString());
					__info.Allowvisit = Int32.Parse(dr["allowvisit"].ToString());
					__info.Allowpost = Int32.Parse(dr["allowpost"].ToString());
					__info.Allowreply = Int32.Parse(dr["allowreply"].ToString());
					__info.Allowpostpoll = Int32.Parse(dr["allowpostpoll"].ToString());
					__info.Allowdirectpost = Int32.Parse(dr["allowdirectpost"].ToString());
					__info.Allowgetattach = Int32.Parse(dr["allowgetattach"].ToString());
					__info.Allowpostattach = Int32.Parse(dr["allowpostattach"].ToString());
					__info.Allowvote = Int32.Parse(dr["allowvote"].ToString());
					__info.Allowmultigroups = Int32.Parse(dr["allowmultigroups"].ToString());
					__info.Allowsearch = Int32.Parse(dr["allowsearch"].ToString());
					__info.Allowavatar = Int32.Parse(dr["allowavatar"].ToString());
					__info.Allowcstatus = Int32.Parse(dr["allowcstatus"].ToString());
					__info.Allowuseblog = Int32.Parse(dr["allowuseblog"].ToString());
					__info.Allowinvisible = Int32.Parse(dr["allowinvisible"].ToString());
					__info.Allowtransfer = Int32.Parse(dr["allowtransfer"].ToString());
					__info.Allowsetreadperm = Int32.Parse(dr["allowsetreadperm"].ToString());
					__info.Allowsetattachperm = Int32.Parse(dr["allowsetattachperm"].ToString());
					__info.Allowhidecode = Int32.Parse(dr["allowhidecode"].ToString());
					__info.Allowhtml = Int32.Parse(dr["allowhtml"].ToString());
					__info.Allowcusbbcode = Int32.Parse(dr["allowcusbbcode"].ToString());
					__info.Allownickname = Int32.Parse(dr["allownickname"].ToString());
					__info.Allowsigbbcode = Int32.Parse(dr["allowsigbbcode"].ToString());
					__info.Allowsigimgcode = Int32.Parse(dr["allowsigimgcode"].ToString());
					__info.Allowviewpro = Int32.Parse(dr["allowviewpro"].ToString());
					__info.Allowviewstats = Int32.Parse(dr["allowviewstats"].ToString());
                    __info.Allowtrade = Int32.Parse(dr["allowtrade"].ToString());
                    __info.Allowdiggs = Int32.Parse(dr["allowdiggs"].ToString());
					__info.Disableperiodctrl = Int32.Parse(dr["disableperiodctrl"].ToString());
                    __info.Allowdebate = Int32.Parse(dr["allowdebate"].ToString());
                    __info.Allowbonus = Int32.Parse(dr["allowbonus"].ToString());
                    __info.Minbonusprice = Int32.Parse(dr["minbonusprice"].ToString());
                    __info.Maxbonusprice = Int32.Parse(dr["maxbonusprice"].ToString());
					__info.Reasonpm = Int32.Parse(dr["reasonpm"].ToString());
					__info.Maxprice = Int16.Parse(dr["maxprice"].ToString());
					__info.Maxpmnum = Int16.Parse(dr["maxpmnum"].ToString());
					__info.Maxsigsize = Int16.Parse(dr["maxsigsize"].ToString());
					__info.Maxattachsize = Int32.Parse(dr["maxattachsize"].ToString());
					__info.Maxsizeperday = Int32.Parse(dr["maxsizeperday"].ToString());
					__info.Attachextensions = dr["attachextensions"].ToString().Trim();
					__info.Raterange = dr["raterange"].ToString().Trim();
                    __info.Maxspaceattachsize = Int32.Parse(dr["maxspaceattachsize"].ToString());
                    __info.Maxspacephotosize = Int32.Parse(dr["maxspacephotosize"].ToString());
				}
			}
			return __info;
		}


		/// <summary>
		/// 得到管理组字段信息
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static AdminGroupInfo AdminGetAdminGroupInfo(int groupid)
		{
            DataTable dt = DatabaseProvider.GetInstance().GetAdminGroup(groupid);
			if (dt.Rows.Count > 0)
			{
				DataRow dr = dt.Rows[0];
				AdminGroupInfo __info;
				__info = new AdminGroupInfo();
				__info.Admingid = short.Parse(dr["admingid"].ToString());
				__info.Alloweditpost = byte.Parse(dr["alloweditpost"].ToString());
				__info.Alloweditpoll = byte.Parse(dr["alloweditpoll"].ToString());
				__info.Allowstickthread = byte.Parse(dr["allowstickthread"].ToString());
				__info.Allowmodpost = byte.Parse(dr["allowmodpost"].ToString());
				__info.Allowdelpost = byte.Parse(dr["allowdelpost"].ToString());
				__info.Allowmassprune = byte.Parse(dr["allowmassprune"].ToString());
				__info.Allowrefund = byte.Parse(dr["allowrefund"].ToString());
				__info.Allowcensorword = byte.Parse(dr["allowcensorword"].ToString());
				__info.Allowviewip = byte.Parse(dr["allowviewip"].ToString());
				__info.Allowbanip = byte.Parse(dr["allowbanip"].ToString());
				__info.Allowedituser = byte.Parse(dr["allowedituser"].ToString());
				__info.Allowmoduser = byte.Parse(dr["allowmoduser"].ToString());
				__info.Allowbanuser = byte.Parse(dr["allowbanuser"].ToString());
				__info.Allowpostannounce = byte.Parse(dr["allowpostannounce"].ToString());
				__info.Allowviewlog = byte.Parse(dr["allowviewlog"].ToString());
				__info.Disablepostctrl = byte.Parse(dr["disablepostctrl"].ToString());
                __info.Allowviewrealname = byte.Parse(dr["allowviewrealname"].ToString());
				return __info;

			}
			return null;
		}


		/// <summary>
		/// 添加用户组信息
		/// </summary>
		/// <param name="__usergroupinfo"></param>
		/// <returns></returns>
		public static bool AddUserGroupInfo(UserGroupInfo __usergroupinfo)
		{
			try
			{
				int Creditshigher = __usergroupinfo.Creditshigher;
				int Creditslower = __usergroupinfo.Creditslower;
                DataTable dt = DatabaseProvider.GetInstance().GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
                if (dt.Rows.Count > 0)
                {

                    return false;

                }

				if (__usergroupinfo.Radminid == 0)
				{
					if (!SystemCheckCredits("add", ref Creditshigher, ref Creditslower, 0))
					{
						return false;
					}
				}

                DatabaseProvider.GetInstance().AddUserGroup(__usergroupinfo, Creditshigher, Creditslower);

                DatabaseProvider.GetInstance().AddOnlineList(__usergroupinfo.Grouptitle);

				AdminCaches.ReSetAdminGroupList();

				AdminCaches.ReSetUserGroupList();

				return true;
			}
			catch
			{
				return false;
			}
		}


		/// <summary>
		/// 校验指定积分用户组的上下限
		/// </summary>
		/// <param name="opname">操作名称</param>
		/// <param name="Creditshigher">积分下限</param>
		/// <param name="Creditslower">积分上限</param>
		/// <param name="groupid"></param>
		public static bool SystemCheckCredits(string opname, ref int Creditshigher, ref int Creditslower, int groupid)
		{
			opresult = "";

			switch (opname.ToLower())
			{
				case "add":
					{
						#region

                        DataTable dt = DatabaseProvider.GetInstance().GetMinCreditHigher();
						if (dt.Rows.Count > 0)
						{
							int systemMiniCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (Creditslower <= systemMiniCredits) //当系统最小积分下限组还大于等于当前添加的积分组的上限时
							{
								Creditslower = systemMiniCredits;
								opresult = "由您所输入的积分下限小于或等于系统最小值,因些系统已将其调整为" + systemMiniCredits;
								break;
							}
						}

                        dt = DatabaseProvider.GetInstance().GetMaxCreditLower();
						if (dt.Rows.Count > 0)
						{
							int systemMaxCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (Creditshigher >= systemMaxCredits) //当系统最大积分上限组还小于等于当前添加的积分组的下限时
							{
								Creditshigher = systemMaxCredits;
								opresult = "由您所输入的积分上限大于或等于系统最大值,因些系统已将其调整为" + systemMaxCredits;
								break;
							}
						}

                        dt = DatabaseProvider.GetInstance().GetUserGroupByCreditshigher(Creditshigher);
						if (dt.Rows.Count > 0)
						{
							int currentGroupID = Convert.ToInt32(dt.Rows[0][0].ToString());
							int currentCreditsHigher = Convert.ToInt32(dt.Rows[0][1].ToString());
							int currentCreditsLower = Convert.ToInt32(dt.Rows[0][2].ToString());

							if (Creditslower > currentCreditsLower)
							{
								return false;
							}
							else
							{
								if (Creditshigher == currentCreditsHigher)
								{
									if (Creditslower < currentCreditsLower)
									{
                                        DatabaseProvider.GetInstance().UpdateUserGroupCreditsHigher(currentGroupID, Creditslower);
										break;
									}
									else
									{
										opresult = "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + currentCreditsLower + ",因些系统无效提交您的数据!";
										return false;
									}
								}
								else
								{
									Creditslower = currentCreditsLower;
									//更新当前查询积分组,也就是要添加的组的积分上限位于当前查询组的积份上下限之间
                                    DatabaseProvider.GetInstance().UpdateUserGroupCreidtsLower(currentCreditsHigher, Creditshigher);

									break;
								}
							}
						}
						else
						{
							opresult = "系统未提到合适的位置保存您提交的信息!";
							return false;
						}

						#endregion
					}

				case "delete":
					{
                        if (DatabaseProvider.GetInstance().GetGroupCountByCreditsLower(Creditshigher) > 0)
						{
                            DatabaseProvider.GetInstance().UpdateUserGroupsCreditsLowerByCreditsLower(Creditslower, Creditshigher);	
                        }
						else
						{
                            DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(Creditshigher, Creditslower);
                        }
						break;
					}

				case "update":
					{
						#region

                        DataTable olddt = DatabaseProvider.GetInstance().GetUserGroupCreditsLowerAndHigher(groupid);
						int currentGroupOldCreatesHigher = Convert.ToInt32(olddt.Rows[0]["creditshigher"].ToString()); //要更新的当前用户组的老的下限积分
						int currentGroupOldCreatesLower = Convert.ToInt32(olddt.Rows[0]["creditslower"].ToString()); //要更新的当前用户组的老的上限积分

                        DataTable dt = DatabaseProvider.GetInstance().GetMinCreditHigher();
						if (dt.Rows.Count > 0)
						{
							int systemMiniCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (Creditslower <= systemMiniCredits) //当系统最小积分下限组还大于等于当前添加的积分组的上限时
							{
								Creditslower = systemMiniCredits;
								opresult = "由您所输入的积分下限小于或等于系统最大值,因些系统已将其调整为" + systemMiniCredits;
                                DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(currentGroupOldCreatesHigher, currentGroupOldCreatesLower);								
								break;
							}
						}

                        dt = DatabaseProvider.GetInstance().GetMaxCreditLower();
						if (dt.Rows.Count > 0)
						{
							int systemMaxCredits = Convert.ToInt32(dt.Rows[0][0].ToString());
							if (Creditshigher >= systemMaxCredits) //当系统最大积分上限组还小于等于当前添加的积分组的下限时
							{
								Creditshigher = systemMaxCredits;
								opresult = "由您所输入的积分上限大于或等于系统最大值,因些系统已将其调整为" + systemMaxCredits;
                                DatabaseProvider.GetInstance().UpdateUserGroupsCreditsLowerByCreditsLower(currentGroupOldCreatesLower, currentGroupOldCreatesHigher);								
								break;
							}
						}

                        dt = DatabaseProvider.GetInstance().GetUserGroupByCreditshigher(Creditshigher);
						if (dt.Rows.Count > 0)
						{
							int currentGroupID = Convert.ToInt32(dt.Rows[0][0].ToString());
							int currentCreditsHigher = Convert.ToInt32(dt.Rows[0][1].ToString());
							int currentCreditsLower = Convert.ToInt32(dt.Rows[0][2].ToString());

							if (Creditslower > currentCreditsLower)
							{
								opresult = "由您所输入的积分上限大于或等于所属有效积分上限的最大值" + currentCreditsLower + ",因些系统无效提交您的数据!";
								return false;
							}
							else
							{
								if (Creditshigher == currentCreditsHigher)
								{
									if (Creditslower < currentCreditsLower)
									{
										//提升以当前老的积分下限为上限的用户组的上限值为老的积分上限
                                        DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(Creditslower, currentCreditsLower);
										break;
									}
								}
								else
								{
									opresult = "系统已自动将您提交的积分上限调整为" + currentCreditsLower;

									//提升以当前老的积分下限为上限的用户组的上限值为老的积分上限
                                    DatabaseProvider.GetInstance().UpdateUserGroupsCreditsHigherByCreditsHigher(Creditslower, currentCreditsLower);
                                    DatabaseProvider.GetInstance().UpdateUserGroupsCreditsLowerByCreditsLower(Creditshigher, currentCreditsHigher);
    								break;
								}
							}
						}
						else
						{
							opresult = "系统未提到合适的位置保存您提交的信息!";
							return false;
						}

						#endregion

						break;
					}
			}
			return true;
		}


		/// <summary>
		/// 更新用户组信息
		/// </summary>
		/// <param name="__usergroupinfo"></param>
		/// <returns></returns>
		public static bool UpdateUserGroupInfo(UserGroupInfo __usergroupinfo)
		{
			int Creditshigher = __usergroupinfo.Creditshigher;
			int Creditslower = __usergroupinfo.Creditslower;

			if ((__usergroupinfo.Groupid >= 9) && (__usergroupinfo.Radminid == 0))
			{
				//当已存在的用户组积分上下限不是当前组的时候,则不允许编辑
                DataTable dt = DatabaseProvider.GetInstance().GetUserGroupByCreditsHigherAndLower(Creditshigher, Creditslower);
				if (dt.Rows.Count > 0)
				{
					if (__usergroupinfo.Groupid.ToString() != dt.Rows[0][0].ToString())
					{
						return false;
					}
				}

				if (!SystemCheckCredits("update", ref Creditshigher, ref Creditslower, __usergroupinfo.Groupid))
				{
					return false;
				}
			}

            DatabaseProvider.GetInstance().UpdateUserGroup(__usergroupinfo, Creditshigher, Creditslower);

            DatabaseProvider.GetInstance().UpdateOnlineList(__usergroupinfo);

			AdminCaches.ReSetAdminGroupList();

			AdminCaches.ReSetUserGroupList();

			return true;

		}
 
		/// <summary>
		/// 删除指定用户组
		/// </summary>
		/// <param name="groupid"></param>
		/// <returns></returns>
		public static bool DeleteUserGroupInfo(int groupid)
		{
			try
			{
                if (DatabaseProvider.GetInstance().IsSystemOrTemplateUserGroup(groupid))
				{
					//当为系统初始组或模板组时,则不允许删除
					return false;
				}

				//当为用户组时
				if (groupid >= 9)
				{
                    DataTable dt = DatabaseProvider.GetInstance().GetOthersCommonUserGroup(groupid);
					if (dt.Rows.Count > 1)
					{
                        if (DatabaseProvider.GetInstance().GetUserGroupRAdminId(groupid) == "0")
						{
                            dt = DatabaseProvider.GetInstance().GetUserGroupCreditsLowerAndHigher(groupid);
							int creditshigher = Convert.ToInt32(dt.Rows[0]["creditshigher"].ToString());
							int creditslower = Convert.ToInt32(dt.Rows[0]["creditslower"].ToString());
							SystemCheckCredits("delete", ref creditshigher, ref creditslower, groupid);
						}
					}
					else
					{
						if (dt.Rows.Count == 1)
						{
							//当系统删除当前组后只有一个组存在时则直接设置唯一组下限,但不修改唯一组上限的值
                            DatabaseProvider.GetInstance().UpdateUserGroupLowerAndHigherToLimit(Utils.StrToInt(dt.Rows[0][0], 0));
						    	
                        }
						else
						{ //系统中用户组只有一个时
							opresult = "当前用户组为系统中唯一的用户组,因此系统无法删除";
							return false;
						}
					}
				}
                DatabaseProvider.GetInstance().DeleteUserGroup(groupid);

                DatabaseProvider.GetInstance().DeleteAdminGroup(groupid);

                DatabaseProvider.GetInstance().DeleteOnlineList(groupid);

				AdminCaches.ReSetAdminGroupList();

				AdminCaches.ReSetUserGroupList();

				return true;
			}
			catch
			{
				return false;
			}
		}

	}
}