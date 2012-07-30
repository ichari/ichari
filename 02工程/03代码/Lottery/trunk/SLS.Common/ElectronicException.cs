using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLS.Common
{
    public class ElectronicException : Exception
    {
        public ElectronicException(string msg)
            : base(msg)
        { 
        }
    }
}
