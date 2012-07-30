using System;
using System.Collections.Generic;
using System.Text;
using Discuz.Config;
using System.IO;
using System.Web;

using Discuz.Forum;
using Discuz.Common;
using Discuz.Entity;

namespace Discuz.Web.UI
{
    public class PageElement
    {
        /// <summary>
        /// 模板id
        /// </summary>
        protected static int templateid;

        /// <summary>
        /// 当前模板路径
        /// </summary>
        protected static string templatepath = AppDomain.CurrentDomain.BaseDirectory + "templates/default/";

        
        public static string Bottom
        {
            get
            {
                string footer = ReadHtml("footer.htm");

                footer = footer.Replace("$SiteUrl$", Discuz.Common.XmlConfig.GetCpsProperty("SiteUrl","").ToString());
                footer = footer.Replace("$GovUrl$", Discuz.Common.XmlConfig.GetCpsProperty("GovUrl","").ToString());
                footer = footer.Replace("$GovName$", Discuz.Common.XmlConfig.GetCpsProperty("GovName", "晓风彩票软件-门户版"));
                footer = footer.Replace("$BottomLogo$", Discuz.Common.XmlConfig.GetCpsProperty("BottomLogo", "images/logo.gif"));

                return footer;
            }
        }

        //读取文件
        static string ReadFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return "";
            }

            string strValue = "";
            StreamReader sr = new StreamReader(filePath, System.Text.Encoding.GetEncoding("GBK"));
            strValue = sr.ReadToEnd();
            sr.Close();
            return strValue;
        }

        static string ReadHtml(string fileName)
        {
            GeneralConfigInfo config =  GeneralConfigs.GetConfig();
            if (Utils.InArray(DNTRequest.GetString("selectedtemplateid"), Templates.GetValidTemplateIDList()))
            {
                templateid = DNTRequest.GetInt("selectedtemplateid", 0);
            }
            else if (Utils.InArray(Utils.GetCookie(Utils.GetTemplateCookieName()), Templates.GetValidTemplateIDList()))
            {
                templateid = Utils.StrToInt(Utils.GetCookie(Utils.GetTemplateCookieName()), config.Templateid);
            }

            if (templateid == 0)
            {
                templateid = config.Templateid;
            }

            templatepath = Templates.GetTemplateItem(templateid).Directory;

            string filePath = AppDomain.CurrentDomain.BaseDirectory + "templates/"+templatepath + "/" + fileName;

            if (!File.Exists(filePath))
            {
                return "";
            }

            FileInfo file = new FileInfo(filePath);
            DateTime lastWriteTime = file.LastWriteTime;

            object[] obj = (object[])HttpContext.Current.Application[fileName];

            if (obj == null)
            {
                obj = new object[] { ReadFile(filePath), lastWriteTime };

                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[fileName] = obj;
                HttpContext.Current.Application.UnLock();
            }

            DateTime lastAppTime = (DateTime)obj[1];

            if (lastAppTime != lastWriteTime)
            {
                obj = new object[] { ReadFile(filePath), lastWriteTime };

                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[fileName] = obj;
                HttpContext.Current.Application.UnLock();
            }

            return obj[0].ToString();
        }
    }
}
