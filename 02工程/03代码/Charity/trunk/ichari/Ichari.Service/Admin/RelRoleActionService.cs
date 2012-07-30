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

	public class RelRoleActionService : BaseService<RelRoleAction>, Ichari.IService.IRelRoleActionService
	{
	    private IRelRoleActionRepository _relroleactionRepository;
	    
	    public RelRoleActionService()
	    {
	        this._relroleactionRepository = new RelRoleActionRepository();
	        base._repository = this._relroleactionRepository;
	    }
	    
	    public RelRoleActionService(System.Data.Objects.ObjectContext context)
	    {
	        this._relroleactionRepository = new RelRoleActionRepository(context);
	        base._repository = this._relroleactionRepository;
	    }
	}
}
