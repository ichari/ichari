using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Model.Dto
{
    public class DrawDetailDto
    {
        public long Id { get; set; }
        public string PrizeName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Tel { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Street { get; set; }
        public string Email { get; set; }
        public string OrderNo { get; set; }
        public DateTime CreateTime { get; set; }
        public int Source { get; set; }
        public bool IsVirtual { get; set; }
    }
}
