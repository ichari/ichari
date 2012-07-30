using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    public class NotifyWrapper
    {
        public static string ConnectString { get; set; }

        /// <summary>
        /// 是否从第三方网站抓取开奖数据
        /// </summary>
        private bool _isUseSniffer;
        
        /// <summary>
        /// 模拟开奖的XML数据，开奖使用
        /// </summary>
        public string MoniOpenXml { get; set; }

        public ApiRequest ApiRequestWrap { get; set; }

        public NotifyWrapper(bool isUseSniffer)
        {
            _isUseSniffer = isUseSniffer;

            if (string.IsNullOrEmpty(ConnectString)) {
                throw new Exception("数据库连接字符串未初始化");
            }
        }


        public NotifyBase Current {
            get {
                if (_isUseSniffer) {
                    return new LotteryOpenNotify(true);
                }
                if (ApiRequestWrap == null)
                    throw new SLS.Common.ElectronicException("如果使用API接口获取数据，请初始化ApiRequest参数");
                
                if (ApiRequestWrap.TransType == CommandType.LotterySchema || ApiRequestWrap.TransType == CommandType.IssueQuery) {
                    return new LotterySchemaNotify() { 
                        TransType = ApiRequestWrap.TransType,
                        AgentPwd = ApiRequestWrap.AgentPwd,
                        AgentId = ApiRequestWrap.AgentId,
                        ApiUrl = ApiRequestWrap.ApiUrl,
                        ConnectString = ConnectString
                    };
                }
                else if (ApiRequestWrap.TransType == CommandType.LotteryOpen || ApiRequestWrap.TransType == CommandType.OpenIssueQuery) {
                    return new LotteryOpenNotify(false) { 
                        TransType = ApiRequestWrap.TransType,
                        AgentPwd = ApiRequestWrap.AgentPwd,
                        AgentId = ApiRequestWrap.AgentId,
                        ApiUrl = ApiRequestWrap.ApiUrl,
                        ConnectString = ConnectString,
                        SimulateRecieveData = MoniOpenXml
                    };
                }
                throw new Common.ElectronicException("TransType格式不正确");
            }
        }
    }
}
