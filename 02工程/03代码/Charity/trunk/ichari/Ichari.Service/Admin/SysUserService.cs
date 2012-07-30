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

	public class SysUserService : BaseService<SysUser>, Ichari.IService.ISysUserService
	{
	    private ISysUserRepository _sysuserRepository;
	    
	    public SysUserService()
	    {
	        this._sysuserRepository = new SysUserRepository();
	        base._repository = this._sysuserRepository;
	    }
	    
	    public SysUserService(System.Data.Objects.ObjectContext context)
	    {
	        this._sysuserRepository = new SysUserRepository(context);
	        base._repository = this._sysuserRepository;
	    }
	}
}
