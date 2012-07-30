using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;
using Ichari.Service;

namespace Ichari.Uow
{
    public partial class ContentUow : BaseUnitOfWork, IContentUow
    {
        
        public IContentService ContentService { get; set; }

        public ContentUow() 
            : base(string.Empty)
        {
            this.Initialize();
        }

        public override void Initialize()
        {
            this.ContentService = new ContentService(base._context);
        }
    }
}
