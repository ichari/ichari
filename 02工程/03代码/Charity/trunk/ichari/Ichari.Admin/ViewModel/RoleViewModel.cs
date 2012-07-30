using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model.Admin;

namespace Ichari.Admin.ViewModel
{
    public class RoleViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<SysRole> RoleList { get; set; }

        public SysRole RoleModel { get; set; }
    }
}
