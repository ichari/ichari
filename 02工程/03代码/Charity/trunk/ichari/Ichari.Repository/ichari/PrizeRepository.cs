using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class PrizeRepository : BaseRepository.Repository<Prize>, Ichari.IRepository.IPrizeRepository
	{
	    public PrizeRepository()
	    { }
	    
	    public PrizeRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}