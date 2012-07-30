using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

using Ichari.Model;
using Ichari.IRepository;
using Ichari.Repository;

namespace Ichari.Service
{
    public class UserInfoService : BaseService<UserInfo>, Ichari.IService.IUserInfoService
    {
        private const string UserInfoCacheKey = "UserInfo.Get";

        private IUserInfoRepository _userInfoRepository;

        public UserInfoService()
        {
            this._userInfoRepository = new UserInfoRepository();
            base._repository = this._userInfoRepository;
        }



        public UserInfoService(System.Data.Objects.ObjectContext context)
        {
            this._userInfoRepository = new UserInfoRepository(context);
            base._repository = this._userInfoRepository;
        }

        public UserInfo Get(int userId)
        {
            var u = Ichari.Cache.CacheContainer<UserInfo>.GetByName(UserInfoCacheKey,userId.ToString());
            if (u != null)
                return u;

            u = base.Get(t => t.Id == userId);

            if(u != null)
                Ichari.Cache.CacheContainer<UserInfo>.SetByName(UserInfoCacheKey, u,userId.ToString());

            return u;
        }
    }
}
