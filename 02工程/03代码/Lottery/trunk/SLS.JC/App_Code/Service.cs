using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// 注意: 如果更改此处的类名 "Service"，也必须更新 Web.config 中对 "Service" 的引用。
public class Service : IService
{
    public void DoWork()
    {
    }

    #region IService 成员


    public List<T_PassRate> GetPassRate(string MatchID)
    {
        if (MatchID.EndsWith(","))
        {
            MatchID = MatchID.Substring(0, MatchID.Length - 1);
        }

        string[] StrIds = MatchID.Split(',');

        PassRateDataContext db1 = new PassRateDataContext(Shove._Web.WebConfig.GetAppSettingsString("ConnectionString"));
        var PassRate = from s in db1.T_PassRate
                       where StrIds.Contains(s.MatchID.ToString()) orderby s.Day, s.MatchNumber ascending
                       select s;
        return PassRate.ToList<T_PassRate>();
    }

    #endregion
}
