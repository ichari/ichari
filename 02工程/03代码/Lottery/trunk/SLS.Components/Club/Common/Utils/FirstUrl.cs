using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Discuz.Common
{

    /// <summary>
    ///FirstUrl 的摘要说明
    /// </summary>
    public class FirstUrl
    {
        string DefaultUrl = Shove._Web.Utility.GetUrlWithoutHttp();

        string Key = "FirstUrl";
        //联盟推广地址参数
        private string Pid = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("PID"));
        private string PidKey = "SLS.TWZT.UnionPid";
        //站长非域名形式的推广地址参数:http://www.Icaile.com?cpsid=20
        private string cpsID = Shove._Convert.StrToLong(Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("cpsid")), -1).ToString();
        private string CpsIDKey = "SLS.TWZT.CpsID";

        private string userID = Shove._Web.Utility.FilteSqlInfusion(Shove._Web.Utility.GetRequest("userID"));
        private string userIDKey = "SLS.TWZT.UnionUserID";

        public void Save()
        {
            object FirstUrl = HttpContext.Current.Session[Key];
            if (FirstUrl == null) //只存在一次
            {

                if (!string.IsNullOrEmpty(Pid))
                    HttpContext.Current.Session[PidKey] = Pid;

                //站长非域名形式的推广地址的CPS ID参数
                if (!string.IsNullOrEmpty(cpsID))
                    HttpContext.Current.Session[CpsIDKey] = cpsID;

                if (!string.IsNullOrEmpty(userID))
                    HttpContext.Current.Session[userIDKey] = userID;

                if (DefaultUrl.StartsWith("club."))
                {
                    DefaultUrl = DefaultUrl.Substring(5);
                }
                HttpContext.Current.Session[Key] = DefaultUrl.Replace(".wzwbw.", ".icaile."); //

            }
        }

        public string Get()
        {
            object FirstUrl = HttpContext.Current.Session[Key];

            if ((FirstUrl == null) || (String.IsNullOrEmpty(FirstUrl.ToString())))
            {
                return DefaultUrl.Replace("wzwbw", "icaile");
            }

            return FirstUrl.ToString().Replace("wzwbw", "icaile");
        }

        public string PID
        {
            get
            {
                return HttpContext.Current.Session[PidKey] == null ? string.Empty : HttpContext.Current.Session[PidKey].ToString();
            }
        }
        public string CpsID
        {
            get
            {
                return HttpContext.Current.Session[CpsIDKey] == null ? string.Empty : HttpContext.Current.Session[CpsIDKey].ToString();
            }
        }
        public string USERID
        {
            get
            {
                return HttpContext.Current.Session[userIDKey] == null ? string.Empty : HttpContext.Current.Session[userIDKey].ToString();
            }
        }

    }


}