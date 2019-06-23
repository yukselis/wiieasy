using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Resources;

namespace Dalowe.View.Web.Framework.AttributeAdapters
{
    public class RequiredAttributeAdapter : System.Web.Mvc.RequiredAttributeAdapter
    {
        public RequiredAttributeAdapter(ModelMetadata metadata, ControllerContext context, RequiredAttribute attribute)
            : base(metadata, context, attribute)
        {
        }

        public override IEnumerable<ModelClientValidationRule> GetClientValidationRules()
        {
            var className = Metadata.ContainerType.Name;
            var propertyName = Metadata.PropertyName;

            var specificKey = string.Format("{0}_{1}_Required", className, propertyName);
            var errorMessage = Messages.ResourceManager.GetObject(specificKey) as string;


            if (string.IsNullOrEmpty(errorMessage))
            {
                var genericMessageWithPlaceHolder =
                    (string)Messages.ResourceManager.GetObject("RequiredAttribute_ValidationError");

                if (!string.IsNullOrEmpty(genericMessageWithPlaceHolder))
                    errorMessage = string.Format(genericMessageWithPlaceHolder, Metadata.DisplayName);
            }

            if (string.IsNullOrEmpty(errorMessage))
                errorMessage = ErrorMessage;
            return new[] { new ModelClientValidationRequiredRule(errorMessage) };
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            return base.Validate(container);
        }
    }
}