using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Admin.ViewModel
{
    /// <summary>
    /// 分页工具条
    /// </summary>
    public class PageBar
    {
        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 每页显示记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                    throw new ArgumentException("PageBar : PageSize can't 0");
                if (TotalCount == 0)
                    return 0;
                return TotalCount % PageSize > 0 ? TotalCount / PageSize + 1 : TotalCount / PageSize;
            }
        }
        public int PageIndex { get; set; }

        public string Url { get; set; }

        public string CurrentPageInfo
        {
            get
            {
                int s = (PageIndex - 1) * PageSize + 1;
                int e = s + PageSize - 1;
                if (e > TotalCount)
                {
                    e = TotalCount;
                }
                return string.Format("{0} - {1}", s, e);
            }
        }
        /// <summary>
        /// 查询参数
        /// </summary>
        public IDictionary<string, string> Params { get; set; }

        public bool IsAjax { get; set; }
        public string JsMethod { get; set; }
    }
}
