using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Pay.ViewModel
{
    public class BaseViewModel
    {
        
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

        public BaseViewModel()
        {
            this.PageCount = 5;
        }
    }
}
