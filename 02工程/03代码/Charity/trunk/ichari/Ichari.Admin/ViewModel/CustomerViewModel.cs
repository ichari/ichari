using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ichari.Model;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Admin.ViewModel
{
    public class CustomerViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<UserInfo> CustomerList { get; set; }
    }
    public class CustomerEditModel : BaseViewModel
    {
        public long Id { get; set; }

        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(16, ErrorMessage = "用户名应 {2}-16 个字符", MinimumLength = 4)]
        public string UserName { get; set; }
        
        [Display(Name = "邮箱地址")]
        [RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "姓名")]
        public string TrueName { get; set; }
        
        [Display(Name = "身份证号码")]
        public string IdentityCardNo { get; set; }
        
        [Display(Name = "电话")]
        public string Tel { get; set; }
        
        [Display(Name = "手机")]
        public string Phone { get; set; }
    }
}
