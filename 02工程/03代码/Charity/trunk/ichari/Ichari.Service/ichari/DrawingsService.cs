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

	public class DrawingsService : BaseService<Drawings>, Ichari.IService.IDrawingsService
	{
	    private IDrawingsRepository _drawingsRepository;
	    
	    public DrawingsService()
	    {
	        this._drawingsRepository = new DrawingsRepository();
	        base._repository = this._drawingsRepository;
	    }
	    
	    public DrawingsService(System.Data.Objects.ObjectContext context)
	    {
	        this._drawingsRepository = new DrawingsRepository(context);
	        base._repository = this._drawingsRepository;
	    }
	}
}
