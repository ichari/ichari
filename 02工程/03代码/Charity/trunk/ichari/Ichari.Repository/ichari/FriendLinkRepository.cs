using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class FriendLinkRepository : BaseRepository.Repository<FriendLink>, Ichari.IRepository.IFriendLinkRepository
	{
	    public FriendLinkRepository()
	    { }
	    
	    public FriendLinkRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}