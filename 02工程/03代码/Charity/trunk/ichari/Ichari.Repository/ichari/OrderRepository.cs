using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class OrderRepository : BaseRepository.Repository<Order>, Ichari.IRepository.IOrderRepository
	{
	    public OrderRepository()
	    { }
	    
	    public OrderRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}