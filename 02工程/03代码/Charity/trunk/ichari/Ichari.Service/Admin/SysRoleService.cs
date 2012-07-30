using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;
using Ichari.IRepository;
using Ichari.Repository;

namespace Ichari.Service  
{

	public class SysRoleService : BaseService<SysRole>, Ichari.IService.ISysRoleService
	{
	    private ISysRoleRepository _sysroleRepository;
	    
	    public SysRoleService()
	    {
	        this._sysroleRepository = new SysRoleRepository();
	        base._repository = this._sysroleRepository;
	    }
	    
	    public SysRoleService(System.Data.Objects.ObjectContext context)
	    {
	        this._sysroleRepository = new SysRoleRepository(context);
	        base._repository = this._sysroleRepository;
	    }
	}
}
