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

	public class AccountLogService : BaseService<AccountLog>, Ichari.IService.IAccountLogService
	{
	    private IAccountLogRepository _accountlogRepository;
	    
	    public AccountLogService()
	    {
	        this._accountlogRepository = new AccountLogRepository();
	        base._repository = this._accountlogRepository;
	    }
	    
	    public AccountLogService(System.Data.Objects.ObjectContext context)
	    {
	        this._accountlogRepository = new AccountLogRepository(context);
	        base._repository = this._accountlogRepository;
	    }
	}
}
