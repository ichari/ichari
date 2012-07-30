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

	public class FriendLinkService : BaseService<FriendLink>, Ichari.IService.IFriendLinkService
	{
	    private IFriendLinkRepository _friendlinkRepository;
	    
	    public FriendLinkService()
	    {
	        this._friendlinkRepository = new FriendLinkRepository();
	        base._repository = this._friendlinkRepository;
	    }
	    
	    public FriendLinkService(System.Data.Objects.ObjectContext context)
	    {
	        this._friendlinkRepository = new FriendLinkRepository(context);
	        base._repository = this._friendlinkRepository;
	    }
	}
}
