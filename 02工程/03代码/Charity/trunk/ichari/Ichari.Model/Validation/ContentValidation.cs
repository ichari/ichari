using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Ichari.Model
{
    [MetadataType(typeof(ContentValication))]
    public partial class Content
    {
        
    }

    public class ContentValication
    { 
        [Required(ErrorMessage="不能为空")]
        public string TagName { get; set; }

        [Required(ErrorMessage="不能为空")]
        public string Title { get; set; }
    }
}
