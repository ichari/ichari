using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class PrizeCategoryRepository : BaseRepository.Repository<PrizeCategory>, Ichari.IRepository.IPrizeCategoryRepository
	{
	    public PrizeCategoryRepository()
	    { }
	    
	    public PrizeCategoryRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}