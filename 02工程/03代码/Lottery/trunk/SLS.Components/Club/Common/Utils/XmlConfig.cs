using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace Discuz.Common
{
    /// <summary>
    ///XmlConfig 的摘要说明
    /// </summary>
    public class XmlConfig
    {
        static string cpsConfigPath;

        static XmlConfig()
        {
            cpsConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"config\config.xml";
        }

        public static string GetCpsClubUrl()
        {
            string[] property = GetCpsProperty("ClubUrl");

            if (property == null || property.Length == 0)
            {
                return Shove._Web.WebConfig.GetAppSettingsString("ClubUrl");
            }

            string url = property[0];

            if (!url.StartsWith("http://"))
            {
                url = "http://" + url;
            }

            return url;
        }

        public static string GetCpsProperty(string property, string defaultValue)
        {
            DataTable dt = GetCpsConfigAsDataTable();

            if (dt == null)
            {
                return defaultValue;
            }

            if (dt.Rows.Count == 0)
            {
                string s = Shove._Web.WebConfig.GetAppSettingsString(property);

                if (s == "")
                {
                    return defaultValue;
                }

                return s;
            }

            DataRow[] drCps = dt.Select("SiteUrl = '" + Shove._Web.Utility.GetUrl().ToLower() + "' or ClubUrl='" + Shove._Web.Utility.GetUrl().ToLower() + "'");

            if (drCps.Length == 0)
            {
                drCps = dt.Select("SiteUrl = 'http://mhb.shovesoft.com'");
            }

            string v = "";

            try
            { v = drCps[0][property].ToString(); }
            catch { }


            if (v == "")
            {
                return defaultValue;
            }

            return v;
        }

        public static string[] GetCpsProperty(string property)
        {
            DataTable dt = GetCpsConfigAsDataTable();

            if (dt == null)
            {
                return null;
            }

            if (dt.Rows.Count == 0)
            {
                string s = Shove._Web.WebConfig.GetAppSettingsString(property);

                if (s == "")
                {
                    return null;
                }

                return new string[] { s };
            }

            DataRow[] drCps = dt.Select("SiteUrl = '" + Shove._Web.Utility.GetUrl().ToLower() + "' or ClubUrl='" + Shove._Web.Utility.GetUrl().ToLower() + "'");

            if (drCps.Length == 0)
            {
                drCps = dt.Select("SiteUrl = 'http://mhb.shovesoft.com'");
            }

            int l = drCps.Length;

            if (l > 0)
            {
                string[] p = new string[l];

                for (int i = 0; i < l; i++)
                {
                    p[i] = drCps[i][property].ToString();
                }

                return p;
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetCpsConfigAsDataTable()
        {
            string appKey = "CpsConfig";
            DataTable dt = null;
            object obj = HttpContext.Current.Application[appKey];

            if (obj == null)
            {
                InitializationConfig(cpsConfigPath, appKey, dt, DateTime.Now);
            }
            else
            {
                object[] objArr = (object[])obj;
                dt = (DataTable)objArr[0];
                DateTime appTime = (DateTime)objArr[1];
                InitializationConfig(cpsConfigPath, appKey, dt, appTime);
            }

            return dt;
        }

        static void InitializationConfig(string configPath, string appKey, DataTable dt, DateTime appTime)
        {
            if (!File.Exists(configPath))
            {
                new Log("System").Write("未找到配置文件 " + configPath);

                return;
            }

            DateTime configTime = new FileInfo(configPath).LastWriteTime;

            if (appTime != configTime)
            {
                DataSet ds = new DataSet();
                ds.ReadXml(configPath);

                if (ds == null)
                {
                    new Log("System").Write("配置文件 " + configPath + " 的格式不符合规范");

                    return;
                }

                dt = ds.Tables["Cps"];

                object[] obj = new object[] { dt, configTime };

                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[appKey] = obj;
                HttpContext.Current.Application.UnLock();
            }
        }
    }
}