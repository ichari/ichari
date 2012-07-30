using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Model.Dto
{
    public class DrawListDto
    {
        /// <summary>
        /// draw id
        /// </summary>
        public long Id { get; set; }
        public long? PrizeId { get; set; }
        public string PrizeName { get; set; }
        public string OrderNo { get; set; }
        public int OrderState { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public bool? IsConfirmed { get; set; }
        public bool IsWin { get; set; }
        /// <summary>
        /// 是否领奖
        /// </summary>
        public bool IsHandled { get; set; }
        public int Source { get; set; }
        public DateTime DrawTime { get; set; }
    }
}
