using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Admin
{
    public static class StaticKey
    {
        #region Session Key
        public const string SessionUser = "CurrentUser";
        public const string SessionIsSuper = "IsSuper";
        public const string SessionUserActionsList = "UserActionsList";
        public const string IsSuper = "IsSuper";
        #endregion

        #region Error Info
        public const string InfoTimeOut = "登录超时";
        public const string InfoNoAuth = "未授权";
        public const string ErrorLogon = "用户名或密码错误";
        #endregion

        #region Message
        public const string MsgOpSuccess = "操作成功";
        
        #endregion

        #region ViewData Key
        public const string TempGlobalError = "TempGlobalError";
        public const string TempGlobalInfo = "TempGlobalInfo";
        #endregion

        #region AppSetting Key
        public const string AppDefaultUserPwd = "DefaultUserPwd";
        public const string AppSuperUserName = "SuperUser";
        #endregion
    }
}
