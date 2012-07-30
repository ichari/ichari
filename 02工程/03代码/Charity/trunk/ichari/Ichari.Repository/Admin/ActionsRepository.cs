using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model.Admin;

namespace Ichari.Repository
{
    public class ActionsRepository : BaseRepository.Repository<Actions> , Ichari.IRepository.IActionsRepository
    {
        public ActionsRepository()
        { }

        public ActionsRepository(System.Data.Objects.ObjectContext context)
            : base(context)
        {
 
        }
    }
}
