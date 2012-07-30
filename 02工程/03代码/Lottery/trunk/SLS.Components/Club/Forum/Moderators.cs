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
	/// 版主操作类
	/// </summary>
	public class Moderators
	{
		/// <summary>
		/// 获得所有版主信息
		/// </summary>
		/// <returns>所有版主信息</returns>
		public static ModeratorInfo[] GetModeratorList()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			ModeratorInfo[] infoArray = cache.RetrieveObject("/ModeratorList") as ModeratorInfo[];
			if(infoArray == null)
			{
                DataTable dt = DatabaseProvider.GetInstance().GetModeratorList();
				infoArray = new ModeratorInfo[dt.Rows.Count];
				ModeratorInfo info;
				// id为索引
				int id = 0;
				foreach(DataRow dr in dt.Rows)
				{
					info = new ModeratorInfo();
					info.Uid = Int32.Parse(dr["uid"].ToString());
					info.Fid = Int16.Parse(dr["fid"].ToString());
					info.Displayorder = Int16.Parse(dr["Displayorder"].ToString());
					info.Inherited = Int16.Parse(dr["inherited"].ToString());
					infoArray[id] = info;
					id++;
				}

				cache.AddObject("/ModeratorList", infoArray);
			}
			return infoArray;
		}


		/// <summary>
		/// 判断指定用户是否是指定版块的版主
		/// </summary>
		/// <param name="admingid">管理组id</param>
		/// <param name="uid">用户id</param>
		/// <param name="fid">论坛id</param>
		/// <returns>如果是版主返回true, 如果不是则返回false</returns>
		public static bool IsModer(int admingid, int uid, int fid)
		{
			if (admingid == 0)
			{
				return false;
			}
			// 用户为管理员或总版主直接返回真
			if (admingid == 1 || admingid == 2)
			{
				return true;
			}
			if (admingid == 3)
			{

				// 如果是管理员或总版主, 或者是普通版主且在该版块有版主权限
				ModeratorInfo[] infosinfoArray = GetModeratorList();
				foreach(ModeratorInfo moder in infosinfoArray)
				{
					// 论坛版主表中存在,则返回真
					if (moder.Uid == uid && moder.Fid == fid)
					{
						return true;
					}
				}
			}
			return false;
		}

	}
}
