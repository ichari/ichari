using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model.Admin;
using Ichari.Model;

namespace Ichari.Admin.ViewModel
{
    public class LoveChangeViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<LoveChange> RecordList { get; set; }

        
    }
}
