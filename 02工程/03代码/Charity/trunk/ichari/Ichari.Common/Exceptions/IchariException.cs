using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Common
{
    public class IchariException : Exception
    {
        public IchariException(string msg)
            : base(msg)
        { }
    }
}
