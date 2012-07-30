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

	public class AddressService : BaseService<Address>, Ichari.IService.IAddressService
	{
	    private IAddressRepository _addressRepository;
	    
	    public AddressService()
	    {
	        this._addressRepository = new AddressRepository();
	        base._repository = this._addressRepository;
	    }
	    
	    public AddressService(System.Data.Objects.ObjectContext context)
	    {
	        this._addressRepository = new AddressRepository(context);
	        base._repository = this._addressRepository;
	    }
	}
}
