using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;
using Ichari.IRepository;
using Ichari.Repository;

namespace Ichari.Service  
{

	public class OrderDetailService : BaseService<OrderDetail>, Ichari.IService.IOrderDetailService
	{
	    private IOrderDetailRepository _orderdetailRepository;
	    
	    public OrderDetailService()
	    {
	        this._orderdetailRepository = new OrderDetailRepository();
	        base._repository = this._orderdetailRepository;
	    }
	    
	    public OrderDetailService(System.Data.Objects.ObjectContext context)
	    {
	        this._orderdetailRepository = new OrderDetailRepository(context);
	        base._repository = this._orderdetailRepository;
	    }
	}
}
