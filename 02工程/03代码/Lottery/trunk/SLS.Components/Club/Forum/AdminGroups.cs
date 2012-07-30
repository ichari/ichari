using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 管理组操作类
    /// </summary>
    public class AdminGroups
    {

        /// <summary>
        /// 获得到指定管理组信息
        /// </summary>
        /// <returns>管理组信息</returns>
        public static AdminGroupInfo[] GetAdminGroupList()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            AdminGroupInfo[] admingroupArray = cache.RetrieveObject("/AdminGroupList") as AdminGroupInfo[];
            if (admingroupArray == null)
            {
                DataTable dt = DatabaseProvider.GetInstance().GetAdminGroupList();
                admingroupArray = new AdminGroupInfo[dt.Rows.Count];
                AdminGroupInfo admingroup;
                int Index = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    admingroup = new AdminGroupInfo();
                    admingroup.Admingid = short.Parse(dr["admingid"].ToString());
                    admingroup.Alloweditpost = byte.Parse(dr["alloweditpost"].ToString());
                    admingroup.Alloweditpoll = byte.Parse(dr["alloweditpoll"].ToString());
                    admingroup.Allowstickthread = byte.Parse(dr["allowstickthread"].ToString());
                    admingroup.Allowmodpost = byte.Parse(dr["allowmodpost"].ToString());
                    admingroup.Allowdelpost = byte.Parse(dr["allowdelpost"].ToString());
                    admingroup.Allowmassprune = byte.Parse(dr["allowmassprune"].ToString());
                    admingroup.Allowrefund = byte.Parse(dr["allowrefund"].ToString());
                    admingroup.Allowcensorword = byte.Parse(dr["allowcensorword"].ToString());
                    admingroup.Allowviewip = byte.Parse(dr["allowviewip"].ToString());
                    admingroup.Allowbanip = byte.Parse(dr["allowbanip"].ToString());
                    admingroup.Allowedituser = byte.Parse(dr["allowedituser"].ToString());
                    admingroup.Allowmoduser = byte.Parse(dr["allowmoduser"].ToString());
                    admingroup.Allowbanuser = byte.Parse(dr["allowbanuser"].ToString());
                    admingroup.Allowpostannounce = byte.Parse(dr["allowpostannounce"].ToString());
                    admingroup.Allowviewlog = byte.Parse(dr["allowviewlog"].ToString());
                    admingroup.Disablepostctrl = byte.Parse(dr["disablepostctrl"].ToString());
                    admingroupArray[Index] = admingroup;
                    Index++;
                }

                cache.AddObject("/AdminGroupList", admingroupArray);

                dt.Dispose();
            }
            return admingroupArray;
        }

        /// <summary>
        /// 获得到指定管理组信息
        /// </summary>
        /// <param name="admingid">管理组ID</param>
        /// <returns>组信息</returns>
        public static AdminGroupInfo GetAdminGroupInfo(int admingid)
        {
            // 如果管理组id大于0
            if (admingid > 0)
            {
                AdminGroupInfo[] admingroupArray = GetAdminGroupList();
                foreach (AdminGroupInfo admingroup in admingroupArray)
                {
                    // 如果存在该管理组则返回该组信息
                    if (admingroup.Admingid == admingid)
                    {
                        return admingroup;
                    }
                }
            }
            // 如果不存在该组则返回null
            return null;
        }


        /// <summary>
        /// 设置管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">管理组信息</param>
        /// <returns>更改记录数</returns>
        public static int SetAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            return DatabaseProvider.GetInstance().SetAdminGroupInfo(admingroupsInfo);
        }

        /// <summary>
        /// 创建一个新的管理组信息
        /// </summary>
        /// <param name="__admingroupsInfo">要添加的管理组信息</param>
        /// <returns>更改记录数</returns>
        public static int CreateAdminGroupInfo(AdminGroupInfo admingroupsInfo)
        {
            return DatabaseProvider.GetInstance().CreateAdminGroupInfo(admingroupsInfo);
        }

        /// <summary>
        /// 删除指定的管理组信息
        /// </summary>
        /// <param name="admingid">管理组ID</param>
        /// <returns>更改记录数</returns>
        public static int DeleteAdminGroupInfo(short admingid)
        {
            return DatabaseProvider.GetInstance().DeleteAdminGroupInfo(admingid);
        }
    }//class end


}
