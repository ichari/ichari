using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Service;
using Ichari.Model.Admin;

namespace Ichari.Uow
{
    public partial class AdminUow : BaseUnitOfWork , IAdminUow
    {
        public IActionsService ActionsService { get; set;}
        public ISysUserService SysUserService { get; set; }
        public ISysRoleService SysRoleService { get; set; }
        public IRelRoleActionService RelRoleActionService { get; set; }
        public IRelUserRoleService RelUserRoleService { get; set; }
        


        public AdminUow([Microsoft.Practices.Unity.Dependency]string entityName)
            :base(entityName)
        {
            
            this.Initialize();
        }

        public override void Initialize()
        {
            this.ActionsService = new ActionsService(base._context);
            this.SysUserService = new SysUserService(base._context);
            this.SysRoleService = new SysRoleService(base._context);
            this.RelRoleActionService = new RelRoleActionService(base._context);
            this.RelUserRoleService = new RelUserRoleService(base._context);
        }

        public IEnumerable<Actions> GetUserActionList(int userId)
        {
            var r = from u in SysUserService.GetQueryList()
                    join ur in RelUserRoleService.GetQueryList()
                    on u.Id equals ur.UserId
                    join ra in RelRoleActionService.GetQueryList()
                    on ur.RoleId equals ra.RoleId
                    join act in ActionsService.GetQueryList()
                    on ra.ActionId equals act.ID
                    where u.Id == userId
                    select act;
            return r.ToList();
        }
    }
}
