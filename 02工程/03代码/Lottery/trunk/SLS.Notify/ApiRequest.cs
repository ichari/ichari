using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    /// <summary>
    /// 针对API接口请求的参数包装类
    /// </summary>
    public class ApiRequest
    {
        public string TransType { get; set; }
        public string AgentId { get; set; }
        public string AgentPwd { get; set; }
        public string ApiUrl { get; set; }
    }
}
