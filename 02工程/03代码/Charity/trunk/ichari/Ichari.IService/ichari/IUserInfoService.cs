using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;

namespace Ichari.IService
{
    public interface IUserInfoService : IService<UserInfo>
    {
        UserInfo Get(int userId);
    }
}
