using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Arwend.Web.View.Mvc.Models.Binders
{
    public class ByteModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            var modelState = new ModelState { Value = valueResult };
            byte actualValue = default(byte);
            try
            {
                if (!Byte.TryParse(valueResult.AttemptedValue, out actualValue))
                    actualValue = (byte)(valueResult.AttemptedValue.Equals("true", StringComparison.InvariantCultureIgnoreCase) ? 1 : 0);
            }
            catch (FormatException e)
            {
                modelState.Errors.Add(e);
            }

            bindingContext.ModelState.Add(bindingContext.ModelName, modelState);
            return actualValue;
        }
    }
}
