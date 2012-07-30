using System;

namespace Discuz.Config
{
    /// <summary>
    /// 基本设置描述类, 加[Serializable]标记为可序列化
    /// </summary>
    [Serializable]
    public class BaseConfigInfo : IConfigInfo
    {
        public string Dbconnectstring;
        public string Tableprefix = "dnt_";		// 数据库中表的前缀
        public string Forumpath = "/";    // 论坛在站点内的路径
        public string Dbtype = "SqlServer";
        public int Founderuid = 1;              // 创始人


        public BaseConfigInfo()
        {
            Dbconnectstring = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"];

            if (Dbconnectstring.StartsWith("0x78AD"))
            {
                Dbconnectstring = Shove._Security.Encrypt.Decrypt3DES(Common.Utils.GetCallCert(), Dbconnectstring.Substring(6), Common.Utils.DesKey);
            }

            Tableprefix = "dnt_";       // 数据库中表的前缀
            Forumpath = "/";      // 论坛在站点内的路径
            Dbtype = "SqlServer";
        }
    }
}
