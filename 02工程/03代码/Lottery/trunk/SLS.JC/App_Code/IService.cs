using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 如果更改此处的接口名称 "IService"，也必须更新 Web.config 中对 "IService" 的引用。
[ServiceContract]
public interface IService
{
    [OperationContract]
    void DoWork();

    [OperationContract]
    List<T_PassRate> GetPassRate(string MatchID);
}
