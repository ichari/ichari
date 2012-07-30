using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class LoveChangeRepository : BaseRepository.Repository<LoveChange>, Ichari.IRepository.ILoveChangeRepository
	{
	    public LoveChangeRepository()
	    { }
	    
	    public LoveChangeRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}