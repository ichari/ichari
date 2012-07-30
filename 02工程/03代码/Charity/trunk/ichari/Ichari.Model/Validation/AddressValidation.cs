using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Validation
{
    public class AddressValidation
    {
        [Display(Name = "真实姓名：")]
        [Required(ErrorMessage = "请填姓名")]
        public string Name { get; set; }

        [Display(Name = "详细地址：")]
        [Required(ErrorMessage = "请填地址")]
        public string Street { get; set; }

        [Display(Name = "省份：")]
        [Required(ErrorMessage = "请填省份")]
        public string Province { get; set; }

        [Display(Name = "城市：")]
        [Required(ErrorMessage = "请填城市")]
        public string City { get; set; }

        [Display(Name = "地区：")]
        [Required(ErrorMessage = "请填地区")]
        public string Area { get; set; }

        [Display(Name = "邮编：")]
        [Required(ErrorMessage = "请填邮编")]
        public string Postal { get; set; }

        [Display(Name = "手机号码：")]
        [Required(ErrorMessage = "请填手机号码")]
        public string Cell { get; set; }

        [Display(Name = "电子邮件：")]
        [Required(ErrorMessage = "请填电子邮件")]
        [RegularExpression(@"^([A-Z0-9a-z_]([._-]?[A-Z0-9a-z])*@[A-Z0-9a-z]([-_]?[A-Z0-9a-z])*[.][A-Z0-9a-z]([._-]?[A-Z0-9a-z])*)", ErrorMessage = "邮箱格式不正确")]
        public string Email { get; set; }

        [Display(Name = "固定电话：")]
        public string Tel { get; set; }

        public bool DefaultAddr { get; set; }
        /// <summary>
        /// 抽奖记录ID
        /// </summary>
        public long? DrawId { get; set; }
    }
}
