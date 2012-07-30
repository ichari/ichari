using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Web.ViewModel
{
    public class DrawDetailViewModel : BaseViewModel
    {
        public Ichari.Model.Dto.DrawDetailDto DrawDetail {get;set;}
        public Ichari.Model.LoveChange LcModel { get; set; }
        public Ichari.Model.Order OrderModel { get; set; }
        public Ichari.Model.Drawings Draw { get; set; }
        public Ichari.Model.FreeCard FreeCard { get; set; }
        public Ichari.Model.UserInfo User { get; set; }
    }
}
