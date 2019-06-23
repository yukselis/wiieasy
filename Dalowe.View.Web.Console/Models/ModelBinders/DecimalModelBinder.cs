using System;
using System.Globalization;
using System.Web.Mvc;

namespace Dalowe.View.Web.Console.Models.ModelBinders
{
    //public class DecimalModelBinder : DefaultModelBinder
    //{
    //    public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
    //    {
    //        var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

    //        return valueProviderResult == null ? base.BindModel(controllerContext, bindingContext) : Decimal.Parse(valueProviderResult.AttemptedValue, NumberStyles.Currency); ;
    //        // of course replace with your custom conversion logic
    //    }
    //}

    public class DecimalModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            var modelState = new ModelState { Value = valueResult };

            object actualValue = null;

            if (valueResult.AttemptedValue != string.Empty)
                try
                {
                    actualValue = Convert.ToDecimal(valueResult.AttemptedValue, CultureInfo.CurrentCulture);
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