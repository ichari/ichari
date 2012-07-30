using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class RelUserRoleRepository : BaseRepository.Repository<RelUserRole>, Ichari.IRepository.IRelUserRoleRepository
	{
	    public RelUserRoleRepository()
	    { }
	    
	    public RelUserRoleRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}