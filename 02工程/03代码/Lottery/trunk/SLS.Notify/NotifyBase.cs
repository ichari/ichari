using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;

using SLS.Common;

namespace SLS.Notify
{
    public abstract class NotifyBase
    {
        #region 属性
        /// <summary>
        /// 是否记录请求日志
        /// </summary>
        public bool IsWriteRequestLog { get; set; }
        /// <summary>
        /// 通知类型
        /// </summary>
        public string TransType { get; set; }
        /// <summary>
        /// 传递的XML字符串
        /// </summary>
        public string TransMsg { get; set; }
        /// <summary>
        /// 代理商ID
        /// </summary>
        public string AgentId { get; set; }
        /// <summary>
        /// 代理商密码
        /// </summary>
        public string AgentPwd { get; set; }
        /// <summary>
        /// 出票商接口URL
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// 出票接口IP地址
        /// </summary>
        public string InterfaceIp { get; set; }

        public string ConnectString { get; set; }

        /// <summary>
        /// 模拟接口收到的数据(方便用于测试)
        /// </summary>
        public string SimulateRecieveData { get; set; }
        #endregion

        public NotifyBase()
        {            
        }
        /// <summary>
        /// 接收通知
        /// </summary>
        public virtual void Recieve()
        {
            InitRequest();
        }
        /// <summary>
        /// 发送查询请求
        /// </summary>
        /// <param name="lotteryTypeId">彩种ID</param>
        /// <param name="issueNo">期号</param>
        /// <returns></returns>
        public abstract string Send(string lotteryTypeId,string issueNo);

        #region 私有方法        
        
        private void InitRequest()
        {
            this.TransType = Shove._Web.Utility.GetRequest(RequestCmdName.CmdName);
            this.TransMsg = Shove._Web.Utility.GetRequest(RequestCmdName.Message);
            this.InterfaceIp = SLS.Common.WebUtils.GetAppSettingValue("SunLotIpAddr");
            this.IsWriteRequestLog = true;
            if (IsWriteRequestLog) {
                WriteLog("Notify: " + "\tTransType: " + TransType + "\t" + TransMsg);
            }

            var cip = Common.WebUtils.GetClientIPAddress();
            if (!InterfaceIp.Contains(cip)) {
                var err = string.Format("电子票异常客户端 IP 请求:{0}", cip);
                WriteLog(err);
                throw new Common.ElectronicException(err);
            }
        }

        protected void WriteLog(string msg)
        { 
            new SLS.Common.Log(string.Format("{0}\\{1}"
                    , "ElectronTicket", "SunLot"))
                    .Write(msg);
        }
        #endregion

        #region 成员方法
        
        #endregion
    }
}
