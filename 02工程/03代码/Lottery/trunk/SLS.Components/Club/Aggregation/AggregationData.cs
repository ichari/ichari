using System;
using System.IO;
using System.Data;
using System.Text;
using System.Xml;

using Discuz.Config;
using Discuz.Common.Xml;

namespace Discuz.Aggregation
{
    /// <summary>
    /// 聚合数据基类
    /// </summary>
    public class  AggregationData
    {
        
        /// <summary>
        /// 聚合数据页面路径
        /// </summary>
        private static string filepath = AppDomain.CurrentDomain.BaseDirectory + "config/aggregation.config";

        /// <summary>
        /// 图片轮换字符串
        /// </summary>
        private static StringBuilder picRotateData = null;

        /// <summary>
        /// xmldoc对象,用于数据文件的信息操作
        /// </summary>
        protected static XmlDocumentExtender __xmlDoc = new XmlDocumentExtender();


        static AggregationData()
		{
            __xmlDoc.Load(filepath);
		}


        /// <summary>
        /// 读取聚合页面数据信息
        /// </summary>
        public static void ReadAggregationConfig()
        {
            __xmlDoc = new XmlDocumentExtender();
            __xmlDoc.Load(filepath);
        }
      
        /// <summary>
        /// 获取聚合页面数据文件路径
        /// </summary>
        public static string DataFilePath
        {
            get
            { 
                return filepath; 
            }
        }
		

        
        /// <summary>
        /// 从XML中检索出指定的轮换广告信息
        /// </summary>
        /// <returns></returns>
        public string GetRotatePicData()
        {
            //当文件未被修改时将直接返回相关记录
            if (picRotateData != null)
            {
                return picRotateData.ToString();
            }

            picRotateData = new StringBuilder();
            picRotateData.Append(this.GetRotatePicStr("Website"));

            return picRotateData.ToString();
        }

        /// <summary>
        /// 从相应的节点下检索轮显数据
        /// </summary>
        /// <param name="nodename">节点名称,如:Website,Spaceindex,Albumindex(区分大小写)</param>
        /// <returns></returns>
        protected string GetRotatePicStr(string nodename)
        {
            __xmlDoc.Load(filepath);
            XmlNodeList xmlnodelist = __xmlDoc.DocumentElement.SelectNodes("/Aggregationinfo/Aggregationpage/" + nodename + "/" + nodename + "_rotatepiclist/" + nodename + "_rotatepic");

            StringBuilder picRotate = new StringBuilder();
            for (int i = 0; i < xmlnodelist.Count; i++)
            {
                picRotate.Append("data[\"-1_" + (i + 1) + "\"] = \"img: " + __xmlDoc.GetSingleNodeValue(xmlnodelist[i], "img").Replace("\"", "\\\"") + "; url: " + __xmlDoc.GetSingleNodeValue(xmlnodelist[i], "url").Replace("\"", "\\\"") + "; target: _blank; alt:" + __xmlDoc.GetSingleNodeValue(xmlnodelist[i], "titlecontent").Replace("\"", "\\\"") + " ; titlecontent: " + __xmlDoc.GetSingleNodeValue(xmlnodelist[i], "titlecontent").Replace("\"", "\\\"") + ";\"\r\n");
            }

            return picRotate.ToString().Trim();
        }


        /// <summary>
        /// 清空数据绑定
        /// </summary>
        public virtual void ClearDataBind()
        {
            picRotateData = null;
        }


        /// <summary>
        /// 清空内存及缓存中所有聚合数据绑定
        /// </summary>
        public void ClearAllDataBind()
        {
            ClearDataBind();
            AggregationFacade.ForumAggregation.ClearDataBind();
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/HotForumList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumNewTopicList");
            Discuz.Cache.DNTCache.GetCacheService().RemoveObject("/Aggregation/ForumHotTopicList");

            //更新文件的最后修改时间
            FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "config/aggregation.config");
            fi.LastWriteTime = DateTime.Now;

            ReadAggregationConfig();
        }
    }
}
