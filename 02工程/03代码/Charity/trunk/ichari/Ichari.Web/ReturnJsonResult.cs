using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Web
{
    public class ReturnJsonResult
    {
        public bool IsSuccess { get; set; }

        public string Info { get; set; }

        public string ErrorMessage { get; set; }
    }
}
