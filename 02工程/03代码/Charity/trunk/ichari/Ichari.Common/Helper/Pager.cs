using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ichari.Common.Helper
{
    /// <summary>
    /// LINQ分页封装类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageList<T> : List<T>
    {
        private int _pageSize;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }
        public new int Count { get; private set; }
        public int PageCount { get; private set; }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 延迟加载
        /// </summary>
        /// <param name="source"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        public PageList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            this._pageIndex = pageIndex;
            this._pageSize = pageSize;
            Count = source.Count();
            PageCount = (int)Math.Ceiling(Count / (double)PageSize);
            if (Count > 0)
            {
                var r = source.Skip<T>((pageIndex - 1) * PageSize).Take<T>(PageSize).ToList();
                this.AddRange(r);
            }
        }
        

        public PageList()
        {
        }
    }
}
