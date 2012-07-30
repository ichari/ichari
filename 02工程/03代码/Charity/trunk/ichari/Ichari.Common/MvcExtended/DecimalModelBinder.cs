using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ichari.Common.MvcExtended
{
    public class DecimalModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var v = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            decimal parsedValue = 0m;
            if (!decimal.TryParse(v.AttemptedValue, out parsedValue)) {
                var error = @"'{0}'不是正确的数值格式";
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, string.Format(error, v.AttemptedValue, bindingContext.ModelMetadata.DisplayName));
            }
            return base.BindModel(controllerContext, bindingContext);
        }
    }
}
