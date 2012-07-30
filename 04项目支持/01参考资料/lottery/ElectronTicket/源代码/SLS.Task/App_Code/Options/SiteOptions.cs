using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Configuration;

using Shove.Database;

namespace SLS.Task
{
    /// <summary>
    /// SiteOptions 的摘要说明
    /// </summary>
    public class SiteOptions
    {
        private long SiteID;
        private string ConnectionString;

        public SiteOptions()
        {
            SiteID = -1;
        }

        public SiteOptions(string connectionstring, long siteid)
        {
            ConnectionString = connectionstring;
            SiteID = siteid;
        }

        public OptionValue this[string Key]
        {
            get
            {
                DataTable dt = null;

                try
                {
                    dt = new DAL.Tables.T_Sites().Open(ConnectionString, "[" + Key + "]", "[ID] = " + SiteID.ToString(), "");
                }
                catch { }

                if (dt == null)
                {
                    throw new Exception("T_Sites 表读取发生错误，请检查数据连接或者数据库是否完整");
                }

                if (dt.Rows.Count < 1)
                {
                    throw new Exception("没有读到站点 ID 为 " + SiteID.ToString() + " 的站点信息");
                }

                return new OptionValue(dt.Rows[0][Key]);
            }
        }
    }
}