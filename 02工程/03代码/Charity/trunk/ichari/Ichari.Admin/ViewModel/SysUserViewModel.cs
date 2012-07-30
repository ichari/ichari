using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ichari.Model.Admin;

namespace Ichari.Admin.ViewModel
{
    public class SysUserViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<SysUser> SysUserList { get; set; }

        public SysUser UserModel { get; set; }

        public List<SysRole> RoleList { get; set; }

        public HashSet<int> HaveRoles { get; set; }
    }
}
