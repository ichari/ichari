using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class SysRoleRepository : BaseRepository.Repository<SysRole>, Ichari.IRepository.ISysRoleRepository
	{
	    public SysRoleRepository()
	    { }
	    
	    public SysRoleRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}