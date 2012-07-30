using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class AccountLogRepository : BaseRepository.Repository<AccountLog>, Ichari.IRepository.IAccountLogRepository
	{
	    public AccountLogRepository()
	    { }
	    
	    public AccountLogRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}