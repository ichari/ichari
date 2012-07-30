using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Ichari.Model
{
    public class DrawingsValidation
    {

    }
    [MetadataType(typeof(PrizeValidation))]
    public partial class Prize
    { 
    }
    public class PrizeValidation
    {   
        [Display(Name = "类别")]
        [Required(ErrorMessage = "请选类别")]
        public string CategoryId { get; set; }
       
        [Display(Name = "奖品名")]
        [Required(ErrorMessage = "奖品名不能为空")]
        public string Name { get; set; }

        [Display(Name = "图档位置")]
        public string ImgUrl { get; set; }
        
        [Display(Name = "几率")]
        [Required(ErrorMessage = "请填几率")]
        public int Probability { get; set; }
        
        [Display(Name = "角度")]
        [Required(ErrorMessage = "请填角度")]
        public int Angle { get; set; }

        [Display(Name = "是否使用")]
        public bool IsEnabled { get; set; }
    }

    public class PrizeCatValidation
    {
        [Required(ErrorMessage = "类别名不能为空")]
        [Display(Name = "类别名")]
        public string Name { get; set; }
    }
}
