using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Model.Dto
{
    [Serializable()]
    public class WinnerListDto
    {
        public string UserName { get; set; }
        public DateTime GameDate { get; set; }
        public string PrizeName { get; set; }
        public string TimePassed { get; set; }
    }
}
