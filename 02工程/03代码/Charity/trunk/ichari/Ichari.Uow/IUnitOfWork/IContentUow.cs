using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.IService;

namespace Ichari.Uow
{
    public interface IContentUow : IUnitOfWork
    {
        IContentService ContentService { get; set; }
    }
}
