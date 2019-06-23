using System;
using Resources;

namespace Dalowe.View.Web.Framework.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredAttribute : System.ComponentModel.DataAnnotations.RequiredAttribute
    {
        public RequiredAttribute()
        {
            ErrorMessageResourceName = Messages.RequiredAttribute_ValidationError;
            ErrorMessageResourceType = typeof(Messages);
        }

        public override bool IsValid(object value)
        {
            return base.IsValid(value);
        }
    }
}