using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Admin.ViewModel
{
    public class IndexViewModel : BaseViewModel
    {
        public decimal AllDonationAmount { get; set; }

        public int ChangeOfCareCount { get; set; }

        public decimal ChangeOfCareAmt { get; set; }

        public int DonationCount { get; set; }

        public decimal DonationAmt { get; set; }
    }
}
