using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class FreeCardRepository : BaseRepository.Repository<FreeCard>, Ichari.IRepository.IFreeCardRepository
	{
	    public FreeCardRepository()
	    { }
	    
	    public FreeCardRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}