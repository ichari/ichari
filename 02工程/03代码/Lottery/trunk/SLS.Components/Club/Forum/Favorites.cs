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
	/// 收藏夹操作类
	/// </summary>
	public class Favorites
	{

		/// <summary>
		/// 创建收藏信息
		/// </summary>
		/// <param name="uid">用户ID</param>
		/// <param name="tid">主题ID</param>
		/// <returns>创建成功返回 1 否则返回 0</returns>	
		public static int CreateFavorites(int uid,int tid)
		{
            return DatabaseProvider.GetInstance().CreateFavorites(uid, tid);
		}

        /// <summary>
        /// 创建收藏信息
        /// </summary>
        /// <param name="uid">用户ID</param>
        /// <param name="tid">主题ID</param>
        /// <param name="type">收藏类型</param>
        /// <returns>创建成功返回 1 否则返回 0</returns>	
        public static int CreateFavorites(int uid, int tid, FavoriteType type)
        {
            return DatabaseProvider.GetInstance().CreateFavorites(uid, tid, (byte)type);
        }
	
		/// <summary>
		/// 删除指定用户的收藏信息
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="fitemid">要删除的收藏信息id列表</param>
		/// <returns>删除的条数．出错时返回 -1</returns>
        public static int DeleteFavorites(int uid, string[] fitemid, FavoriteType type)
		{
			foreach (string id in fitemid)
			{
				if (!Utils.IsNumeric(id))
				{
					return -1;
				}
			}

			string fidlist = String.Join(",",fitemid);

            return DatabaseProvider.GetInstance().DeleteFavorites(uid, fidlist, (byte)type);
		}

		/// <summary>
		/// 得到用户收藏信息列表
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="pagesize">分页时每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <returns>用户信息列表</returns>
        public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex)
        {
            return DatabaseProvider.GetInstance().GetFavoritesList(uid, pagesize, pageindex);
        }
        
		/// <summary>
		/// 得到用户收藏信息列表
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="pagesize">分页时每页的记录数</param>
		/// <param name="pageindex">当前页码</param>
		/// <param name="type">收藏类型id</param>
		/// <returns>用户信息列表</returns>
        public static DataTable GetFavoritesList(int uid, int pagesize, int pageindex, FavoriteType type)
		{
            return DatabaseProvider.GetInstance().GetFavoritesList(uid, pagesize, pageindex, (int)type);
		}

		/// <summary>
		/// 得到用户收藏的总数
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <returns>收藏总数</returns>
		public static int GetFavoritesCount(int uid)
		{
            return DatabaseProvider.GetInstance().GetFavoritesCount(uid);	
        }


        /// <summary>
        /// 得到用户单个类型收藏的总数
        /// </summary>
        /// <param name="uid">用户id</param>
        /// <returns>收藏总数</returns>
        public static int GetFavoritesCount(int uid, FavoriteType type)
        {
            return DatabaseProvider.GetInstance().GetFavoritesCount(uid, (int)type);
        }

        /// <summary>
        /// 收藏夹里是否包含了指定的主题
        /// </summary>
        /// <param name="uid">用户Id</param>
        /// <param name="tid">主题Id</param>
        /// <returns></returns>
        public static int CheckFavoritesIsIN(int uid, int tid)
        {
            return CheckFavoritesIsIN(uid, tid, FavoriteType.ForumTopic);
        }

		/// <summary>
		/// 收藏夹里是否包含了指定的项
		/// </summary>
		/// <param name="uid">用户id</param>
		/// <param name="tid">项Id</param>
        /// <param name="type">类型: 相册, 日志, 主题</param>
		/// <returns></returns>
		public static int CheckFavoritesIsIN(int uid,int tid, FavoriteType type)
		{
            return DatabaseProvider.GetInstance().CheckFavoritesIsIN(uid, tid, (byte)type);	
        }
	}//class end
}
