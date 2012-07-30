using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class RelRoleActionRepository : BaseRepository.Repository<RelRoleAction>, Ichari.IRepository.IRelRoleActionRepository
	{
	    public RelRoleActionRepository()
	    { }
	    
	    public RelRoleActionRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}