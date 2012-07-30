using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Ichari.Common;

namespace Ichari.Sync
{
    public class UserSync
    {
        public bool IsEnableSync {
            get {
                if (ConfigurationManager.AppSettings[StaticConfig.AkIsEnableSyncToLottery] == "1")
                    return true;
                return false;
            }
        }
        /// <summary>
        /// 主站注册用户同步到彩票频道
        /// </summary>
        public long RegistToLottery(string userName,string pwd,string email,string trueName)
        {
            if (!IsEnableSync)
                return -1;
            return SyncUserToLottery(userName, pwd, email, trueName);
        }

        private long SyncUserToLottery(string userName,string pwd,string email,string trueName)
        {
            var reqstr = new StringBuilder();
            reqstr.Append(string.Format("userName={0}&pwd={1}&email={2}&trueName={3}",userName,pwd,email,trueName));
            var formatter = System.Configuration.ConfigurationManager.AppSettings[StaticConfig.AkLotteryApiUrlFormatter];
            var apiUrl = string.Format(formatter, LotteryApiConfig.RegisterUser);
            var r = WebUtils.Post(apiUrl, reqstr.ToString(), 5);
            long id = 0;
            long.TryParse(r,out id);
            return id;
        }

        /// <summary>
        /// 主站更新密码同步更新彩票频道
        /// </summary>
        /// <param name="lotteryUserId"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public string UpdatePassword(long lotteryUserId, string newPwd)
        {
            if (!IsEnableSync)
                return "同步功能被禁用";
            var reqstr = new StringBuilder();
            reqstr.Append(string.Format("lotUserId={0}&pwd={1}",lotteryUserId,newPwd));
            var formatter = System.Configuration.ConfigurationManager.AppSettings[StaticConfig.AkLotteryApiUrlFormatter];
            var apiUrl = string.Format(formatter, LotteryApiConfig.UpdatePassword);
            var r = WebUtils.Post(apiUrl, reqstr.ToString(), 5);
            return r;
        }
    }
}
