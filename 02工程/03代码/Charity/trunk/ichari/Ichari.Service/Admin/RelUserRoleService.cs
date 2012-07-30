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

	public class RelUserRoleService : BaseService<RelUserRole>, Ichari.IService.IRelUserRoleService
	{
	    private IRelUserRoleRepository _reluserroleRepository;
	    
	    public RelUserRoleService()
	    {
	        this._reluserroleRepository = new RelUserRoleRepository();
	        base._repository = this._reluserroleRepository;
	    }
	    
	    public RelUserRoleService(System.Data.Objects.ObjectContext context)
	    {
	        this._reluserroleRepository = new RelUserRoleRepository(context);
	        base._repository = this._reluserroleRepository;
	    }
	}
}
