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

	public class LoveChangeService : BaseService<LoveChange>, Ichari.IService.ILoveChangeService
	{
	    private ILoveChangeRepository _lovechangeRepository;
	    
	    public LoveChangeService()
	    {
	        this._lovechangeRepository = new LoveChangeRepository();
	        base._repository = this._lovechangeRepository;
	    }
	    
	    public LoveChangeService(System.Data.Objects.ObjectContext context)
	    {
	        this._lovechangeRepository = new LoveChangeRepository(context);
	        base._repository = this._lovechangeRepository;
	    }
	}
}
