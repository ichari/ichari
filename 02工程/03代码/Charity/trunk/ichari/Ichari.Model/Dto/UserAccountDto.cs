using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Model.Dto
{
    public class UserAccountDto
    {
        public Int64 Id { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public decimal FrozenAmount { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Int64 UserId { get; set; }
    }
}
