using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperTreeViewExtensions
    {
        public static MvcHtmlString QTreeView<T>(this HtmlHelper htmlHelper
            ,IList<T> source)
        {
            var s = BuildRoot(source);
            var ms = new MvcHtmlString(s);
            return ms;
        }

        private static string BuildRoot<T>(IList<T> source)
        {
            var s = new StringBuilder();
            s.Append(string.Format("<div id=\"{0}\" class=\"{1}\">","qTreeView","q_TreeView"));

            s.Append("</div>");
            return s.ToString();
        }
    }
}
