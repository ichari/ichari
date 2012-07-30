using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model.Enum
{
    /// <summary>
    /// 图文链类型
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// 文字链
        /// </summary>
        [Display(Name="文字链")]
        Text = 1,
        /// <summary>
        /// 图文链
        /// </summary>
        [Display(Name="图文链")]
        ImageText = 2,
        /// <summary>
        /// 图片链
        /// </summary>
        [Display(Name="图片链")]
        Image = 3
    }
}
