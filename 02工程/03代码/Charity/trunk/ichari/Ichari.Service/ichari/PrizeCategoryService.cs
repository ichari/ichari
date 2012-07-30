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

	public class PrizeCategoryService : BaseService<PrizeCategory>, Ichari.IService.IPrizeCategoryService
	{
	    private IPrizeCategoryRepository _prizecategoryRepository;
	    
	    public PrizeCategoryService()
	    {
	        this._prizecategoryRepository = new PrizeCategoryRepository();
	        base._repository = this._prizecategoryRepository;
	    }
	    
	    public PrizeCategoryService(System.Data.Objects.ObjectContext context)
	    {
	        this._prizecategoryRepository = new PrizeCategoryRepository(context);
	        base._repository = this._prizecategoryRepository;
	    }
	}
}
