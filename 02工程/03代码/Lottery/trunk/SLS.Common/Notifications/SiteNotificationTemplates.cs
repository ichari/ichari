using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Shove.Database;

namespace SLS.Common
{
    /// <summary>
    /// SiteNotificationTemplates 的摘要说明
    /// </summary>
    public class SiteNotificationTemplates
    {
        private string _connStr;
        public Sites Site;

        public SiteNotificationTemplates()
        {
            Site = null;
            _connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }

        public SiteNotificationTemplates(Sites site)
        {
            Site = site;
            _connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
        }

        public string this[short Manner, string NotificationType]
        {
            get
            {
                if ((Manner < NotificationManners.Min) || (Manner > NotificationManners.Max))
                {
                    throw new Exception("SiteNotificationTemplates 类的通知方式 Manner 变量的值超出的范围，它的范围是：1 手机短信 2 Email 3 站内信");
                }

                return this[(Manner == NotificationManners.SMS) ? "SMS" : ((Manner == NotificationManners.Email) ? "Email" : "StationSMS"), NotificationType];
            }
            set
            {
                if ((Manner < NotificationManners.Min) || (Manner > NotificationManners.Max))
                {
                    throw new Exception("SiteNotificationTemplates 类的通知方式 Manner 变量的值超出的范围，它的范围是：1 手机短信 2 Email 3 站内信");
                }

                this[(Manner == NotificationManners.SMS) ? "SMS" : ((Manner == NotificationManners.Email) ? "Email" : "StationSMS"), NotificationType] = value;
            }
        }

        public string this[string Manner, string NotificationType]
        {
            get
            {
                if ((Site == null) || (Site.ID < 1))
                {
                    throw new Exception("没有初始化 SiteNotificationTemplates 类的 Site 变量");
                }

                if ((Manner != "SMS") && (Manner != "Email") && (Manner != "StationSMS"))
                {
                    throw new Exception("SiteNotificationTemplates 类的通知方式 Manner 变量的值超出的范围，它的范围是：1 (SMS)手机短信 2 Email 3 (StationSMS)站内信");
                }

                string SystemPreFix = "SiteOptions_";
                DataTable dt = null;
                bool InApplication = true;

                try
                {
                    dt = (DataTable)System.Web.HttpContext.Current.Application[SystemPreFix + this.Site.ID.ToString()];
                }
                catch { }

                if ((dt == null) || (dt.Rows.Count < 1))
                {
                    InApplication = false;

                    dt = new SLS.Dal.Tables.T_Sites().Open(_connStr,"", "[ID] = " + Site.ID.ToString(), "");
                }

                if (dt == null)
                {
                    throw new Exception("SiteNotificationTemplates 类读取数据错误，请检查数据库连接设置");
                }

                if (dt.Rows.Count < 1)
                {
                    throw new Exception("SiteNotificationTemplates 类的 Site 变量值不在有效范围之内");
                }

                if (!InApplication)
                {
                    try
                    {
                        System.Web.HttpContext.Current.Application.Lock();
                        System.Web.HttpContext.Current.Application.Add(SystemPreFix + this.Site.ID.ToString(), dt);
                    }
                    catch { }
                    finally
                    {
                        try
                        {
                            System.Web.HttpContext.Current.Application.UnLock();
                        }
                        catch { }
                    }
                }

                string Result = dt.Rows[0]["Template" + Manner + "_" + NotificationType].ToString().Replace("[SiteName]", Site.Name).Replace("[SiteUrl]", Site.Url);

                return Result;
            }
            set
            {
                if ((Site == null) || (Site.ID < 1))
                {
                    throw new Exception("没有初始化 SiteNotificationTemplates 类的 Site 变量");
                }

                string sql = @"update T_Sites set [Template" + Manner + "_" + NotificationType + "] = @Value where [ID]=@ID";
                int Result = MSSQL.ExecuteNonQuery(sql,
                            new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, value),
                            new MSSQL.Parameter("ID", SqlDbType.BigInt, 0, ParameterDirection.Input, Site.ID));
                if (Result < 0)
                {
                    throw new Exception("SiteNotificationTemplates 类读取数据错误，请检查数据库连接设置。如果数据库连接设置没有问题，可能是 NotificationType 变量的值不在有效范围之内");
                }
                //if (DAL.Procedures.P_SetSiteNotificationTemplate(Site.ID, iManner, NotificationType, value) < 0)
                //{
                //    throw new Exception("SiteNotificationTemplates 类读取数据错误，请检查数据库连接设置。如果数据库连接设置没有问题，可能是 NotificationType 变量的值不在有效范围之内");
                //}

                string SystemPreFix = "SiteOptions_";

                try
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application.Remove(SystemPreFix + this.Site.ID.ToString());
                }
                catch { }
                finally
                {
                    try
                    {
                        System.Web.HttpContext.Current.Application.UnLock();
                    }
                    catch { }
                }
            }
        }

        public void SplitEmailTemplate(string Content, ref string Subject, ref string Body)
        {
            Regex regex = new Regex(@"{Subject}(?<Subject>[^{]*?){/Subject}(?<Body>[\W\w]*)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            Match m = regex.Match(Content);

            Subject = m.Groups["Subject"].ToString().Trim();
            Body = m.Groups["Body"].ToString().Trim();
        }
    }
}