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

	public class PayLogService : BaseService<PayLog>, Ichari.IService.IPayLogService
	{
	    private IPayLogRepository _paylogRepository;
	    
	    public PayLogService()
	    {
	        this._paylogRepository = new PayLogRepository();
	        base._repository = this._paylogRepository;
	    }
	    
	    public PayLogService(System.Data.Objects.ObjectContext context)
	    {
	        this._paylogRepository = new PayLogRepository(context);
	        base._repository = this._paylogRepository;
	    }
	}
}
