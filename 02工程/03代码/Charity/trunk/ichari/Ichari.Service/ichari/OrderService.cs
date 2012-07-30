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

	public class OrderService : BaseService<Order>, Ichari.IService.IOrderService
	{
	    private IOrderRepository _orderRepository;
	    
	    public OrderService()
	    {
	        this._orderRepository = new OrderRepository();
	        base._repository = this._orderRepository;
	    }
	    
	    public OrderService(System.Data.Objects.ObjectContext context)
	    {
	        this._orderRepository = new OrderRepository(context);
	        base._repository = this._orderRepository;
	    }
	}
}
