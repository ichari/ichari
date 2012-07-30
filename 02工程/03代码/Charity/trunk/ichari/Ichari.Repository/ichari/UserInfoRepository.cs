using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Repository
{
    public class UserInfoRepository : BaseRepository.Repository<Ichari.Model.UserInfo> , Ichari.IRepository.IUserInfoRepository
    {
        public UserInfoRepository()
        { }

        public UserInfoRepository(System.Data.Objects.ObjectContext context)
            : base(context)
        {
 
        }
    }
}
