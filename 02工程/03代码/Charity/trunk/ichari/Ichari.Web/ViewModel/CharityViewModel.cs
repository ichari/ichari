using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

using Ichari.Model;
using Ichari.Model.Dto;

namespace Ichari.Web.ViewModel
{
    public class CharityViewModel : BaseViewModel
    {
        /// <summary>
        /// 捐赠金额
        /// </summary>
        [Required(ErrorMessage="捐款金额不能为空")]
        [RegularExpression(@"^[+-]?(([0-9]\d*[.]?)|(0.))(\d{0,2})?$",ErrorMessage="输入数字的格式不正确，小数点后最多两位")]
        [Range(0.1,99999999,ErrorMessage="捐赠金额至少大于0.1元")]
        public decimal Amount { get; set; }

        //public UserInfo LogonUser { get; set; }
        [Required(ErrorMessage="用户名不能为空")]
        public string UserName { get; set; }

        [Required(ErrorMessage="密码不能为空")]
        public string Password { get; set; }

        public bool IsSaveCookie { get; set; }
    }

    public class ChariDrawViewModel : BaseViewModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserName { get; set; }

        [Display(Name = "密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }

        [Display(Name = "捐款单号")]
        [Required(ErrorMessage = "单号不能为空")]
        public string DonatioinId { get; set; }

        [Display(Name = "捐款金额")]
        [Required(ErrorMessage = "金额不能为空")]
        [DataType(DataType.Currency, ErrorMessage = "金额不正确")]
        public decimal DonAmount { get; set; }
    }

    public class ChariGameViewModel : BaseViewModel<LoveChange>
    {
        public List<LoveChange> DList { get; set; }

        public List<Order> OList { get; set; }

        public int Count { get; set; }

        public string DonationId { get; set; }

        public decimal DonationAmount { get; set; }
    }

    public class ChariWinnerViewModel : BaseViewModel
    {
        public List<WinnerListDto> WinList { get; set; }
        /// <summary>
        /// 是否从银联跳转
        /// </summary>
        public bool IsFromChinaUnion { get; set; }
    }
}
