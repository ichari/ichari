using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class PayLogRepository : BaseRepository.Repository<PayLog>, Ichari.IRepository.IPayLogRepository
	{
	    public PayLogRepository()
	    { }
	    
	    public PayLogRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}