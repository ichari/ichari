using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ichari.Model;


namespace Ichari.Admin.ViewModel
{
    public class DrawingsViewModel : BaseViewModel<Ichari.Model.Dto.DrawListDto>
    {
        //public Ichari.Common.Helper.PageList<Drawings> DrawingsList { get; set; }
        public Ichari.Model.Dto.DrawDetailDto Detail { get; set; }

        public Order ChariOrder { get; set; }
        public LoveChange UnionOrder { get; set; }
    }

    public class PrizeViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<Prize> PrizeList { get; set; }
    }

    public class PrizeCategoryViewModel : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<PrizeCategory> PrizeCategoryList { get; set; }
    }
}
