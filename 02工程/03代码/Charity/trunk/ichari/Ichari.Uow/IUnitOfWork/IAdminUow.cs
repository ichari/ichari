using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Model.Admin;

namespace Ichari.Uow
{
    public interface IAdminUow : IUnitOfWork
    {
        IActionsService ActionsService { get; set; }
        ISysUserService SysUserService { get; set; }
        ISysRoleService SysRoleService { get; set; }
        IRelRoleActionService RelRoleActionService { get; set; }
        IRelUserRoleService RelUserRoleService { get; set; }

        /// <summary>
        /// 获取某用户的功能权限列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IEnumerable<Actions> GetUserActionList(int userId);
    }
}
