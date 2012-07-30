using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class AddressRepository : BaseRepository.Repository<Address>, Ichari.IRepository.IAddressRepository
	{
	    public AddressRepository()
	    { }
	    
	    public AddressRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}