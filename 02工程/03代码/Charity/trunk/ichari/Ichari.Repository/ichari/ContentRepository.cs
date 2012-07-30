using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model;
using Ichari.Model.Admin;

namespace Ichari.Repository  
{

	public class ContentRepository : BaseRepository.Repository<Content>, Ichari.IRepository.IContentRepository
	{
	    public ContentRepository()
	    { }
	    
	    public ContentRepository(System.Data.Objects.ObjectContext context)
	        : base(context)
	    { }
	}
}