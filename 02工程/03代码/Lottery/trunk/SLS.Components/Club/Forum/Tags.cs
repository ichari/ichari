using System;
using System.Text;
using System.IO;
using System.Data;

using Discuz.Common;
using Discuz.Config;
using Discuz.Entity;
#if NET1
#else
using Discuz.Common.Generic;
#endif
using Discuz.Data;
using Discuz.Forum;
using Discuz.Forum.ScheduledEvents;

namespace Discuz.Forum
{
    public class Tags
    {
        /// <summary>
        /// 将reader转化为实体类
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static TagInfo LoadSingleTagInfo(IDataReader reader)
        {
            TagInfo tag = new TagInfo();
            tag.Tagid = Utils.StrToInt(reader["tagid"], 0);
            tag.Tagname = reader["tagname"].ToString();
            tag.Userid = Utils.StrToInt(reader["userid"], 0);
            tag.Postdatetime = Convert.ToDateTime(reader["postdatetime"]);
            tag.Orderid = Utils.StrToInt(reader["orderid"], 0);
            tag.Color = reader["color"].ToString();
            tag.Count = Utils.StrToInt(reader["count"], 0);
            tag.Fcount = Utils.StrToInt(reader["fcount"], 0);
            tag.Pcount = Utils.StrToInt(reader["pcount"], 0);
            tag.Scount = Utils.StrToInt(reader["scount"], 0);
            tag.Vcount = Utils.StrToInt(reader["vcount"], 0);

            return tag;
        }

        /// <summary>
        /// 获取标签信息(不存在返回null)
        /// </summary>
        /// <param name="tagid">标签id</param>
        /// <returns></returns>
        public static TagInfo GetTagInfo(int tagid)
        {
            IDataReader reader = DatabaseProvider.GetInstance().GetTagInfo(tagid);
            TagInfo tag = null;
            if (reader.Read())
            {
                tag = Tags.LoadSingleTagInfo(reader);
            }
            reader.Close();

            return tag;
        }

        /// <summary>
        /// 写入标签缓存文件
        /// </summary>
        /// <param name="filename">文件绝对路径(mappath之后的)</param>
        /// <param name="tags">标签集合</param>
        /// <param name="jsonp_callback">jsonp的回调函数名, 如不使用, 请传入string.Empty</param>
        /// <param name="outputcountfield">是否输出计数统计字段</param>
        public static void WriteTagsCacheFile(string filename, List<TagInfo> tags, string jsonp_callback, bool outputcountfield)
        {
            if (tags.Count > 0)
            {
                if (!Directory.Exists(Path.GetDirectoryName(filename)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filename));
                }

                StringBuilder builder = new StringBuilder();
                if (jsonp_callback != string.Empty)
                {
                    builder.Append(jsonp_callback);
                    builder.Append("(");
                }

                builder.Append("[\r\n  ");
                foreach (TagInfo tag in tags)
                {
                    if (outputcountfield)
                    {
                        builder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}', 'fcount' : '{2}', 'pcount' : '{3}', 'scount' : '{4}', 'vcount' : '{5}', 'gcount' : '{6}'}}, ",
                                tag.Tagid, tag.Tagname, tag.Fcount, tag.Pcount, tag.Scount, tag.Vcount, tag.Gcount));
                    }
                    else
                    {
                        builder.Append(string.Format("{{'tagid' : '{0}', 'tagname' : '{1}'}}, ", tag.Tagid, tag.Tagname));
                    }
                }
                builder.Length = builder.Length - 2;
                builder.Append("\r\n]");
                if (jsonp_callback != string.Empty)
                {
                    builder.Append(")");
                }

                try
                {
                    using (FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        Byte[] info = System.Text.Encoding.UTF8.GetBytes(builder.ToString());
                        fs.Write(info, 0, info.Length);
                        fs.Close();
                        //EventLogs.WriteFailedLog(string.Format("标签任务成功: 文件 {0} 写入成功", filename));
                    }
                }
                catch
                {
                    //EventLogs.WriteFailedLog(string.Format("标签任务失败: 文件 {0} 写入失败", filename));
                }
            }
        }
    }
}
