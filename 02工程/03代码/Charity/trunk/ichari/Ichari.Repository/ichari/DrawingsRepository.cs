using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class DrawingsRepository : BaseRepository.Repository<Drawings>, Ichari.IRepository.IDrawingsRepository
	{
	    public DrawingsRepository()
	    { }
	    
	    public DrawingsRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}