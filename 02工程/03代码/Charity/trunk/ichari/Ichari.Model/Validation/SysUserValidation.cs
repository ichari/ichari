using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Validation
{
    
    public class SysUserValidation
    {
        [Display(Name="用户名")]
        [Required(ErrorMessage="用户名不能为空")]
        public string LogonName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        [Display(Name="真实姓名")]
        public string TrueName { get; set; }

        
        [RegularExpression(@"^(\w)+(\.\w+)*@(\w)+((\.\w+)+)$",ErrorMessage="Email格式不正确")]
        public string Email { get; set; }
    }
}
