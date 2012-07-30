using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    internal abstract class LotteryDataProvider : IGetOpenData
    {
        public string AgentId { get; set; }
        public string AgentPwd { get; set; }
        public string ApiUrl { get; set; }
        public string TransType { get; set; }

        public abstract string GetData(int lotoTypeId,string issueNo);
    }
}
