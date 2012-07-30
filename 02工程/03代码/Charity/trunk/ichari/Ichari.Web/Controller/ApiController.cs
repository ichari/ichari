using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ichari.Uow;

namespace Ichari.Web
{
    public class ApiController : BaseController
    {
        private IChariUow _uow;

        public ApiController(IChariUow uow)
        {
            _uow = uow;
        }

        public void UpdateUserInfo()
        { 
            if (Request.UserHostAddress != System.Configuration.ConfigurationManager.AppSettings["SyncRequestIp"])
                return;

            var lotUserId = Request.Form["lotUserId"];
            var trueName = Request.Form["tn"];
            var idCardNo = Request.Form["cardno"];
            var email = Request.Form["email"];

            var lid = 0L;
            long.TryParse(lotUserId, out lid);
            if (lid <= 0) {
                var err = string.Format("彩票频道更新用户信息失败，彩票用户ID：{0}", lid);
                _log.Error(err);
                Response.Write(err);
                return;
            }
            try
            {
                var oldUser = _uow.UserInfoService.GetQueryList(t => t.LotteryUserId == lid).Single();
                oldUser.TrueName = trueName;
                oldUser.IdentityCardNo = idCardNo;
                oldUser.Email = email;
                oldUser.UpdateTime = DateTime.Now;
                _uow.Commit();
                Response.Write("1");
            }
            catch (InvalidOperationException ex)
            {
                var err = string.Format("彩票频道更新用户信息失败，{0}，彩票用户ID：{1}",ex.Message, lid);
                _log.Error(err);
                Response.Write(err);
                return;
            }
        }
    }
}
