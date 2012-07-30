using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Notify
{
    /// <summary>
    /// 获取开奖数据的接口定义
    /// </summary>
    internal interface IGetOpenData
    {
        /// <summary>
        /// 获取开奖数据
        /// </summary>
        /// <param name="lotoTypeId">彩种ID</param>
        /// <param name="issueNo">期号</param>
        /// <returns></returns>
        string GetData(int lotoTypeId,string issueNo);
    }
}
