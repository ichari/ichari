using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Validation
{
    
    public class SysRoleValidation
    {
        [Required(ErrorMessage="角色名不能为空")]
        [Display(Name="角色名称")]
        public string RoleName { get; set; }

        
    }
}
