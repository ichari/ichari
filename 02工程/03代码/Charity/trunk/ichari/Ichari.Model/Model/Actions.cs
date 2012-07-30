using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Ichari.Model.Validation;

namespace Ichari.Model.Admin
{
    //[MetadataType(typeof(SysUserValidation))]
    public partial class Actions
    {
        public Actions ParentNode { get; set; }

        public List<Actions> SubNodes { get; set; }
    }
}
