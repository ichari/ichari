using System;
using System.Data;
using System.Configuration;

using Shove.Database;

namespace SLS.MHB.Task
{
    public class SystemOptions
    {
        private string ConnectionString;

        public SystemOptions(string connectionstring)
        {
            ConnectionString = connectionstring;
        }

        public OptionValue this[string Key]
        {
            get
            {
                DataTable dt = new DAL.Tables.T_Options().Open(ConnectionString, "[Value]", "[Key] = '" + Key + "'", "");

                if (dt == null)
                {
                    throw new Exception("T_Options 表读取发生错误，请检查数据连接或者数据库是否完整");
                }


                if (dt.Rows.Count < 1)
                {
                    throw new Exception("T_Options 表读取发生错误，请检查数据连接或者是否该表拥有 Key 值为 " + Key + " 记录");
                }

                return new OptionValue(dt.Rows[0]["Value"]);
            }
        }
    }
}
