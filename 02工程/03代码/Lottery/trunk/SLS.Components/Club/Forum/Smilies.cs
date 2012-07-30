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
	/// 表情符操作类
	/// </summary>
	public class Smilies
	{

        public static Regex[] regexSmile = null;

        static Smilies()
        {
            //InitRegexSmilies();
        }

        /// <summary>
        /// 初始化表情正则对象数组
        /// </summary>
        public static void InitRegexSmilies()
        {
            SmiliesInfo[] smiliesList = Smilies.GetSmiliesListWithInfo();
            int smiliesCount = smiliesList.Length;

            regexSmile = new Regex[smiliesCount];

            for (int i = 0; i < smiliesCount; i++)
            {
                 regexSmile[i] = new Regex(@Regex.Escape(smiliesList[i].Code), RegexOptions.None);

            }
        }

        /// <summary>
        /// 重新加载并初始化表情正则对象数组
        /// </summary>
        /// <param name="smiliesList">表情对象数组</param>
        public static void ResetRegexSmilies(SmiliesInfo[] smiliesList)
        {
            int smiliesCount = smiliesList.Length;

            // 如果数目不同则重新创建数组, 以免发生数组越界
            if (regexSmile == null || regexSmile.Length != smiliesCount)
            {
                regexSmile = new Regex[smiliesCount];
            }

            for (int i = 0; i < smiliesCount; i++)
            {
                regexSmile[i] = new Regex(@Regex.Escape(smiliesList[i].Code), RegexOptions.None);

            }
        }

		/// <summary>
		/// 得到表情符数据
		/// </summary>
		/// <returns>表情符数据</returns>
		public static IDataReader GetSmiliesList()
		{
            return DatabaseProvider.GetInstance().GetSmiliesList();
		}


		/// <summary>
		/// 得到表情符数据,包括表情分类
		/// </summary>
		/// <returns>表情符表</returns>
		public static DataTable GetSmiliesListDataTable()
		{
            return DatabaseProvider.GetInstance().GetSmiliesListDataTable();
		}

		/// <summary>
		/// 得到不带分类的表情符数据
		/// </summary>
		/// <returns>表情符表</returns>
		public static DataTable GetSmiliesListWithoutType()
		{
            return DatabaseProvider.GetInstance().GetSmiliesListWithoutType();
		}

		/// <summary>
		/// 将缓存中的表情信息返回为SmiliesInfo[],不包括表情分类
		/// </summary>
		/// <returns></returns>
		public static SmiliesInfo[] GetSmiliesListWithInfo()
		{
			Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
			SmiliesInfo[] smilieslistinfo = cache.RetrieveObject("/UI/SmiliesListWithInfo") as SmiliesInfo[];
			if (smilieslistinfo == null)
			{
				DataTable dt = GetSmiliesListWithoutType();
				if (dt == null)
				{
					return null;
				}
				if (dt.Rows.Count <= 0)
				{
					return null;
				}

				smilieslistinfo = new SmiliesInfo[dt.Rows.Count];
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					smilieslistinfo[i] = new SmiliesInfo();
					smilieslistinfo[i].Id = Utils.StrToInt(dt.Rows[i]["id"],0);
					smilieslistinfo[i].Code = dt.Rows[i]["Code"].ToString();
					smilieslistinfo[i].Displayorder = Utils.StrToInt(dt.Rows[i]["Displayorder"],0);
					smilieslistinfo[i].Type = Utils.StrToInt(dt.Rows[i]["Type"],0);
					smilieslistinfo[i].Url = dt.Rows[i]["Url"].ToString();

				}
				cache.AddObject("/UI/SmiliesListWithInfo", smilieslistinfo);

                //表情缓存重新加载时重新初始化表情正则对象数组
                ResetRegexSmilies(smilieslistinfo);
			}
			return smilieslistinfo;
		}

		/// <summary>
		/// 获得表情分类列表
		/// </summary>
		/// <returns></returns>
		public static DataTable GetSmilieTypes()
		{
            return DatabaseProvider.GetInstance().GetSmilieTypes();
		}

        /// <summary>
        /// 获取表情分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		public static DataRow GetSmilieTypeById(string id)
		{
            return DatabaseProvider.GetInstance().GetSmilieTypeById(id);
		}

        /// <summary>
        /// 清理空的表情分类
        /// </summary>
        /// <returns>被清理掉的空表情分类列表</returns>
        public static string ClearEmptySmilieType()
        {
            string emptySmilieList = "";
            DataTable smilieType = GetSmilieTypes();
            foreach (DataRow dr in smilieType.Rows)
            {
                if (DatabaseProvider.GetInstance().GetSmiliesInfoByType(int.Parse(dr["id"].ToString())).Rows.Count == 0)
                {
                    emptySmilieList += dr["code"].ToString() + ",";
                    DbHelper.ExecuteNonQuery(DatabaseProvider.GetInstance().DeleteSmily(int.Parse(dr["id"].ToString())));
                }
            }
            return emptySmilieList.TrimEnd(',');
        }

	}//class
}
