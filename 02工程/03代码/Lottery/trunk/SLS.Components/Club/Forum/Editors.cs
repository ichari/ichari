using System;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;

using Discuz.Common;
using Discuz.Data;
using Discuz.Forum;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{
    /// <summary>
    /// 编辑器操作类
    /// </summary>
    public class Editors
    {

        public static Regex[] regexCustomTag = null;

        static Editors()
        {
            InitRegexCustomTag();
        }

        /// <summary>
        /// 初始化自定义标签正则对象数组
        /// </summary>
        public static void InitRegexCustomTag()
        {
            CustomEditorButtonInfo[] tagList = Editors.GetCustomEditButtonListWithInfo();
            if (tagList != null)
            {
                int tagCount = tagList.Length;

                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < tagCount; i++)
                {
                    if (builder.Length > 0)
                    {
                        builder.Remove(0, builder.Length);
                    }
                    builder.Append(@"(\[");
                    builder.Append(tagList[i].Tag);
                    if (tagList[i].Params > 1)
                    {
                        builder.Append("=");
                        for (int j = 2; j <= tagList[i].Params; j++)
                        {
                            builder.Append(@"(.*?)");
                            if (j < tagList[i].Params)
                            {
                                builder.Append(",");
                            }
                        }
                    }

                    builder.Append(@"\])([\s\S]+?)\[\/");
                    builder.Append(tagList[i].Tag);
                    builder.Append(@"\]");

                    regexCustomTag[i] = new Regex(builder.ToString(), RegexOptions.IgnoreCase);
                }
            }
        }

        /// <summary>
        /// 重新加载并初始化自定义标签正则对象数组
        /// </summary>
        /// <param name="smiliesList">自定义标签对象数组</param>
        public static void ResetRegexCustomTag(CustomEditorButtonInfo[] tagList)
        {
            int tagCount = tagList.Length;

            // 如果数目不同则重新创建数组, 以免发生数组越界
            if (regexCustomTag == null || tagCount != regexCustomTag.Length)
            {
                regexCustomTag = new Regex[tagCount];
            }

            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < tagCount; i++)
            {

                if (builder.Length > 0)
                {
                    builder.Remove(0, builder.Length);
                }
                builder.Append(@"(\[");
                builder.Append(tagList[i].Tag);
                if (tagList[i].Params > 1)
                {
                    builder.Append("=");
                    for (int j = 2; j <= tagList[i].Params; j++)
                    {
                        builder.Append(@"(.*?)");
                        if (j < tagList[i].Params)
                        {
                            builder.Append(",");
                        }

                    }

                }

                builder.Append(@"\])([\s\S]+?)\[\/");
                builder.Append(tagList[i].Tag);
                builder.Append(@"\]");

                regexCustomTag[i] = new Regex(builder.ToString(), RegexOptions.IgnoreCase);

            }
        }


        /// <summary>
        /// 以DataReader返回自定义编辑器按钮列表
        /// </summary>
        /// <returns></returns>
        public static IDataReader GetCustomEditButtonList()
        {
            return DatabaseProvider.GetInstance().GetCustomEditButtonList();
        }

        /// <summary>
        /// 以DataTable返回自定义按钮列表
        /// </summary>
        /// <returns></returns>
        public static DataTable GetCustomEditButtonListWithTable()
        {
            return DatabaseProvider.GetInstance().GetCustomEditButtonListWithTable();
        }



        /// <summary>
        /// 以CustomEditorButtonInfo数组形式返回自定义按钮
        /// </summary>
        /// <returns></returns>
        public static CustomEditorButtonInfo[] GetCustomEditButtonListWithInfo()
        {
            Discuz.Cache.DNTCache cache = Discuz.Cache.DNTCache.GetCacheService();
            CustomEditorButtonInfo[] buttonArray = cache.RetrieveObject("/UI/CustomEditButtonInfo") as CustomEditorButtonInfo[];
            if (buttonArray == null)
            {
                DataTable dt = GetCustomEditButtonListWithTable();
                if (dt == null)
                {
                    return null;
                }

                if (dt.Rows.Count <= 0)
                {
                    return null;
                }

                buttonArray = new CustomEditorButtonInfo[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    buttonArray[i] = new CustomEditorButtonInfo();
                    buttonArray[i].Id = Utils.StrToInt(dt.Rows[i]["id"], 0);
                    buttonArray[i].Tag = dt.Rows[i]["Tag"].ToString();
                    buttonArray[i].Icon = dt.Rows[i]["Icon"].ToString();
                    buttonArray[i].Available = Utils.StrToInt(dt.Rows[i]["Available"], 0);
                    buttonArray[i].Example = dt.Rows[i]["Example"].ToString();
                    buttonArray[i].Explanation = dt.Rows[i]["Explanation"].ToString();
                    buttonArray[i].Params = Utils.StrToInt(dt.Rows[i]["Params"], 0);
                    buttonArray[i].Nest = Utils.StrToInt(dt.Rows[i]["Nest"], 0);
                    buttonArray[i].Paramsdefvalue = dt.Rows[i]["Paramsdefvalue"].ToString();
                    buttonArray[i].Paramsdescript = dt.Rows[i]["Paramsdescript"].ToString();
                    buttonArray[i].Replacement = dt.Rows[i]["Replacement"].ToString();
                }
                dt.Dispose();
                cache.AddObject("/UI/CustomEditButtonInfo", buttonArray);

                // 表情缓存重新加载时重新初始化表情正则对象数组
                ResetRegexCustomTag(buttonArray);
            }
            return buttonArray;
        }


    }
}
