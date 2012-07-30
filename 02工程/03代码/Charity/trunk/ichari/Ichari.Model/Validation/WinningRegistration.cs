using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ichari.Model.Validation
{
    public class WinningRegistration
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请填用户名")]
        [StringLength(16, ErrorMessage = "用户名应 {2}-16 个字符", MinimumLength = 4)]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "请填密码")]
        [StringLength(16, ErrorMessage = "密码应 6-16 个字符", MinimumLength = 6)]
        public string Password { get; set; }

        [Display(Name = "确认密码")]
        [Required(ErrorMessage = "请再次输入密码")]
        [Compare("Password", ErrorMessage = "密码不符合")]
        public string ConfirmPwd { get; set; }

        [Display(Name = "验证码")]
        [Required(ErrorMessage = "请输入验证码")]
        public string Captcha { get; set; }

        [Display(Name = "电子邮件")]
        [Required(ErrorMessage = "请填电子邮件")]
        [RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "真实姓名")]
        [Required(ErrorMessage = "请填姓名")]
        public string Name { get; set; }

        [Display(Name = "详细地址")]
        [Required(ErrorMessage = "请填地址")]
        public string Street { get; set; }

        [Display(Name = "省份")]
        [Required(ErrorMessage = "请填省份")]
        public string Province { get; set; }

        [Display(Name = "城市")]
        [Required(ErrorMessage = "请填城市")]
        public string City { get; set; }

        [Display(Name = "地区")]
        [Required(ErrorMessage = "请填地区")]
        public string Area { get; set; }

        [Display(Name = "邮编")]
        [Required(ErrorMessage = "请填邮编")]
        public string Postal { get; set; }

        [Display(Name = "手机号码")]
        [Required(ErrorMessage = "请填手机号码")]
        public string Cell { get; set; }

        [Display(Name = "固定电话")]
        public string Tel { get; set; }

        [Display(Name = "默认地址")]
        public bool DefaultAddr { get; set; }
    }
}
