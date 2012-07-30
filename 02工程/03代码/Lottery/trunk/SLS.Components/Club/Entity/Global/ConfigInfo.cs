using System;
using System.Text;

namespace Discuz.Entity
{
    public class ConfigInfo
    {
        public ConfigInfo()
        { }

        /// <summary>
        /// 自动编号
        /// </summary>
        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 针对general.config的字段
        /// </summary>
        private string _cfgGeneral;
        public string CfgGeneral
        {
            get { return _cfgGeneral.Trim(); }
            set { _cfgGeneral = value; }
        }

        /// <summary>
        /// 针对Cache.config
        /// </summary>
        private int _cfgCache;
        public int CfgCache
        {
            get { return _cfgCache; }
            set { _cfgCache = value; }
        }
    }
}
