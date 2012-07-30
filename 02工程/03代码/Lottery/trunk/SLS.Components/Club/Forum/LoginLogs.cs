using System;
using System.Data;
using System.Data.Common;

using Discuz.Common;
using Discuz.Data;
using Discuz.Config;
using Discuz.Entity;

namespace Discuz.Forum
{

	/// <summary>
	/// 登录日志操作类
	/// </summary>
	public class LoginLogs
	{
		/// <summary>
		/// 增加错误次数并返回错误次数, 如不存在登录错误日志则建立
		/// </summary>
		/// <param name="ip">ip地址</param>
        /// <returns>int</returns>
		public static int UpdateLoginLog(string ip, bool update)
		{			
            DataTable dt = DatabaseProvider.GetInstance().GetErrLoginRecordByIP(ip);
			if(dt.Rows.Count > 0)
			{
				int errcount = Utils.StrToInt(dt.Rows[0][0].ToString(), 0);
				if (Utils.StrDateDiffMinutes(dt.Rows[0][1].ToString(), 0) < 15)
				{
					if ((errcount >= 5) || (!update))
					{
						return errcount;
					}
					else
					{
						DatabaseProvider.GetInstance().AddErrLoginCount(ip);
                        return errcount + 1;
					}
				}
                DatabaseProvider.GetInstance().ResetErrLoginCount(ip);
                return 1;
			}
			else
			{
				if (update)
				{
                    DatabaseProvider.GetInstance().AddErrLoginRecord(ip);	
                }
				return 1;
			}
		}

		/// <summary>
		/// 删除指定ip地址的登录错误日志
		/// </summary>
		/// <param name="ip">ip地址</param>
        /// <returns>int</returns>
		public static int DeleteLoginLog(string ip)
		{
            return DatabaseProvider.GetInstance().DeleteErrLoginRecord(ip);	
        }
	}
}
