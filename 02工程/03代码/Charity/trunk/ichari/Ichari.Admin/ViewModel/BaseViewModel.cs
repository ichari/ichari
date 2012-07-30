using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

using Ichari.Model.Admin;

namespace Ichari.Admin.ViewModel
{
    public class BaseViewModel
    {
        public  List<Actions> SubMenuList { get; set; }

        int pageIndex = 1;
        /// <summary>
        /// 当前分页的序号，从1开始
        /// </summary>
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; } }
        /// <summary>
        /// 每页显示的记录数
        /// </summary>
        public int PageCount { get; set; }

        public int TotalCount { get; set; }
        

        public IEnumerable<SelectListItem> YesOrNo
        {
            get;
            set;
        }

        public BaseViewModel()
        {
            this.PageCount = 10;
            this.YesOrNo = new List<SelectListItem>() { 
                new SelectListItem(){Text = "",Value=""},
                new SelectListItem(){Text = "是",Value="true"},
                new SelectListItem(){Text = "否",Value="false"}
            };
        }
    }

    public class BaseViewModel<T> : BaseViewModel
    {
        public Ichari.Common.Helper.PageList<T> PageList { get; set; }
    }
}
