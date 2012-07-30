using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {

        public static void RenderPager(this HtmlHelper htmlHelper, string partialViewName,
            int totalCount,int pageSize,int pageIndex,string url,Dictionary<string, object> pars)
        {
            if(string.IsNullOrEmpty(partialViewName))
                partialViewName = "_PageBar";

            var bar = new Ichari.Common.PageBar();
            bar.TotalCount = totalCount;
            bar.PageSize = pageSize;
            bar.PageIndex = pageIndex;
            bar.Url = url;
            bar.Params = pars;

            htmlHelper.RenderPartial(partialViewName, bar);
        }

        public static MvcHtmlString TipPartial(this HtmlHelper htmlHelper,bool alwaysDisplay = false)
        {
            htmlHelper.ViewData["alwaysDisplay"] = alwaysDisplay;            
            return htmlHelper.Partial("_Info");
        }

       
    }
}
