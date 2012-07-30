using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Shove.Database;

namespace SLS.Common
{
    /// <summary>
    /// SiteOptions 的摘要说明
    /// </summary>
    public class SiteOptions
    {
        private string _connStr;

        public Sites Site;

        public SiteOptions()
        {
            Site = null;
            _connStr = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
            if (string.IsNullOrEmpty(_connStr))
                _connStr = PF.ConnectString;
        }

        public SiteOptions(Sites site)
        {
            Site = site;
        }

        public OptionValue this[string Key]
        {
            get
            {
                //if (Site == null)
                //{
                //    throw new Exception("没有初始化 SiteOptions 类的 Site 变量");
                //}

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
                    throw new Exception("T_Sites 表读取发生错误，请检查数据连接或者数据库是否完整");
                }

                if (dt.Rows.Count < 1)
                {
                    throw new Exception("没有读到站点 ID 为 " + Site.ID.ToString() + " 的站点信息");
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

                return new OptionValue(dt.Rows[0][Key]);
            }
            set
            {
                if ((Site == null) || (Site.ID < 1))
                {
                    throw new Exception("没有初始化 SiteOptions 类的 Site 变量");
                }

                DataTable dt = new SLS.Dal.Tables.T_Sites().Open(_connStr,Key, "[ID] = " + Site.ID.ToString(), "");

                if (dt == null)
                {
                    throw new Exception("T_Sites 表读取发生错误，请检查数据连接或者是否该表拥有 " + Key + " 字段");
                }

                if (dt.Rows.Count < 1)
                {
                    throw new Exception("没有读到站点 ID 为 " + Site.ID.ToString() + " 的站点信息");
                }

                switch (dt.Columns[0].DataType.Name)
                {
                    case "Byte[]":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, value.Value.ToString())) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "String":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.VarChar, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Int16":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.SmallInt, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Int32":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.Int, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Int64":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.BigInt, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Decimal":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.Money, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Boolean":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.Bit, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    case "Double":
                        if (MSSQL.ExecuteNonQuery("update T_Sites set " + Key + " = @Value where [ID] = " + Site.ID.ToString(),
                            new MSSQL.Parameter("Value", SqlDbType.Float, 0, ParameterDirection.Input, value.Value)) < 0)
                        {
                            throw new Exception("设置站点属性 " + Key + " 发生异常");
                        }
                        break;

                    default:
                        throw new Exception("设置站点属性 " + Key + " 发生异常");
                }

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
    }
}