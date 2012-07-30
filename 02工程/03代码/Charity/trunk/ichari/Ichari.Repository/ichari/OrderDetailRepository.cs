using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class OrderDetailRepository : BaseRepository.Repository<OrderDetail>, Ichari.IRepository.IOrderDetailRepository
	{
	    public OrderDetailRepository()
	    { }
	    
	    public OrderDetailRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}