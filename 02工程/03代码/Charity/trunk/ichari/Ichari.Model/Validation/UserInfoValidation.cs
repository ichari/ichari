using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Ichari.Model
{
    [MetadataType(typeof(UserInfoValidation))]
    public partial class UserInfo
    {
        [Required(ErrorMessage = "请再次输入密码")]
        [Compare("Password", ErrorMessage = "密码不符合")]
        public string ConfirmPwd { get; set; }

        [Required(ErrorMessage = "请输入验证码")]
        public string Captcha { get; set; }

        public bool IsSaveCookie { get; set; }
    }

    public class UserAdditionalInfo
    {
        public string UserName { get; set; }

        public string Phone { get; set; }

        [Required(ErrorMessage = "请输入身份证号码")]
        [RegularExpression(@"^\d{17}(\d{1}|X|x)$",ErrorMessage="身份证格式不正确")]
        public string IdentityCardNo { get; set; }

        [Required(ErrorMessage = "请输入姓名")]
        public string TrueName { get; set; }
    }

    public class UserInfoValidation
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [StringLength(16, ErrorMessage = "用户名应 {2}-16 个字符", MinimumLength = 4)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [StringLength(16, ErrorMessage = "密码应 6-16 个字符", MinimumLength = 6)]
        public string Password { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [Compare("Password", ErrorMessage = "密码不符合")]
        public string ConfirmPwd { get; set; }

        [RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [RegularExpression(@"^1\d{10}",ErrorMessage="手机号码格式不正确")]
        public string Phone { get; set; }
    }

    public class UserChangePassword
    {
        [Required(ErrorMessage = "请输入原密码")]
        public string CurrentPwd { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [StringLength(16, ErrorMessage = "密码应 6-16 个字符", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [Compare("NewPassword", ErrorMessage = "密码不符合")]
        public string ConfirmNewPwd { get; set; }
    }

    public class UserResetPassword
    {
        //[Required(ErrorMessage = "*请输入用户名")]
        //public string UserName { get; set; }

        //[Required(ErrorMessage = "*请输入邮箱地址")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "请输入新密码")]
        [StringLength(16, ErrorMessage = "密码应 6-16 个字符", MinimumLength = 6)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "请再次输入密码")]
        [Compare("NewPassword", ErrorMessage = "密码不符合")]
        public string ConfirmNewPwd { get; set; }
    }

    public class UserNameEmail
    {
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "邮箱地址不能为空")]
        [RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }
    }
}
