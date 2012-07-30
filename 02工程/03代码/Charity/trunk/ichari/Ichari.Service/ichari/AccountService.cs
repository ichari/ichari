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

	public class AccountService : BaseService<Account>, Ichari.IService.IAccountService
	{
	    private IAccountRepository _accountRepository;
	    
	    public AccountService()
	    {
	        this._accountRepository = new AccountRepository();
	        base._repository = this._accountRepository;
	    }
	    
	    public AccountService(System.Data.Objects.ObjectContext context)
	    {
	        this._accountRepository = new AccountRepository(context);
	        base._repository = this._accountRepository;
	    }
	}
}
