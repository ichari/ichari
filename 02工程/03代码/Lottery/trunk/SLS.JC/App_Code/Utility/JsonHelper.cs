using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Newtonsoft.Json;
using System.Text;
using System.IO;


/// <summary>
///JsonHelper 的摘要说明
/// </summary>
public class JsonHelper
{
    public JsonHelper()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    public static string DataTableToJSON(DataTable dt, string dtName)
    {
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);

        using (JsonWriter jw = new JsonTextWriter(sw))
        {
            JsonSerializer ser = new JsonSerializer();
            jw.WriteStartObject();
            jw.WritePropertyName(dtName);
            jw.WriteStartArray();
            foreach (DataRow dr in dt.Rows)
            {
                jw.WriteStartObject();

                foreach (DataColumn dc in dt.Columns)
                {
                    jw.WritePropertyName(dc.ColumnName);
                    ser.Serialize(jw, dr[dc].ToString());
                }

                jw.WriteEndObject();
            }
            jw.WriteEndArray();
            jw.WriteEndObject();

            sw.Close();
            jw.Close();

        }

        return sb.ToString();
    }

    #region 把dataset数据转换成json的格式
    /// <summary>
    /// 把dataset数据转换成json的格式
    /// </summary>
    /// <param name="ds">dataset数据集</param>
    /// <returns>json格式的字符串</returns>
    public static string GetJsonByDataset(DataSet ds)
    {
        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0].Rows.Count <= 0)
        {
            //如果查询到的数据为空
            //return "{\"ok\":false}";
            return "[]";
        }
        StringBuilder sb = new StringBuilder();
        //sb.Append("{\"ok\":true,");
        //sb.Append("\"rCount\":"+ds.Tables[0].Rows.Count+",");

        sb.Append("[");
        foreach (DataTable dt in ds.Tables)
        {
            sb.Append("[");
            //sb.Append(string.Format("\"{0}\":[", dt.TableName));

            foreach (DataRow dr in dt.Rows)
            {
                sb.Append("{");
                for (int i = 0; i < dr.Table.Columns.Count; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\",", dr.Table.Columns[i].ColumnName.Replace("\"", "\\\"").Replace("\'", "\\\'"), ObjToStr(dr[i]).Replace("\"", "\\\"").Replace("\'", "\\\'")).Replace(Convert.ToString((char)13), " \\r\\n").Replace(Convert.ToString((char)10), " \\r\\n");
                }
                if (sb.ToString().Substring(sb.Length - 1) == ",")
                {
                    sb.Remove(sb.Length - 1, 1);
                }
                sb.Append("},");
            }
            if (sb.ToString().Substring(sb.Length - 1) == ",")
            {
                sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("],");
        }
        if (sb.ToString().Substring(sb.Length - 1) == ",")
        {
            sb.Remove(sb.Length - 1, 1);
        }
        sb.Append("]");
        return sb.ToString();
    }

    /// <summary>
    /// 将object转换成为string
    /// </summary>
    /// <param name="ob">obj对象</param>
    /// <returns></returns>
    public static string ObjToStr(object ob)
    {
        if (ob == null)
        {
            return string.Empty;
        }
        else
            return ob.ToString();
    }
    #endregion
}
