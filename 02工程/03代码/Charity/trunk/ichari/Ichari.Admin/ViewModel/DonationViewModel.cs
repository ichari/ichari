using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Ichari.Model.Admin;
using Ichari.Model;

namespace Ichari.Admin.ViewModel
{
    public class DonationViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<Order> RecordList { get; set; }

        public IEnumerable<SelectListItem> OrderStatus { get; set; }

        public BaseQueryModel QueryModel { get; set; } 
    }

    
}
