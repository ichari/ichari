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

public class SystemOptions
{
    public OptionValue this[string Key]
    {
        get
        {
            string SystemPreFix = "SystemOptions_";
            DataTable dt = null;
            bool InApplication = true;

            try
            {
                dt = (DataTable)System.Web.HttpContext.Current.Application[SystemPreFix];
            }
            catch { }

            if (dt == null)
            {
                InApplication = false;

                dt = new DAL.Tables.T_Options().Open("[Key], [Value]", "", "");
            }

            if (dt == null)
            {
                throw new Exception("T_Options 表读取发生错误，请检查数据连接或者数据库是否完整");
            }

            DataRow[] dr = dt.Select("Key='" + Key + "'");

            if ((dr == null) || (dr.Length < 1))
            {
                throw new Exception("T_Options 表读取发生错误，请检查数据连接或者是否该表拥有 Key 值为 " + Key + " 记录");
            }

            if (!InApplication)
            {
                try
                {
                    System.Web.HttpContext.Current.Application.Lock();
                    System.Web.HttpContext.Current.Application.Add(SystemPreFix, dt);
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
            
            return new OptionValue(dr[0]["Value"]);
        }

        set
        {
            DAL.Procedures.P_SetOptions(Key, value.Value.ToString());

            string SystemPreFix = "SystemOptions_";

            try
            {
                System.Web.HttpContext.Current.Application.Lock();
                System.Web.HttpContext.Current.Application.Remove(SystemPreFix);
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

    public SystemOptions()
    {
    }
}
