using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class SysUserRepository : BaseRepository.Repository<SysUser>, Ichari.IRepository.ISysUserRepository
	{
	    public SysUserRepository()
	    { }
	    
	    public SysUserRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}