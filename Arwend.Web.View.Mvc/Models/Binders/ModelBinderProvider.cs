using System;
using System.Web.Mvc;
namespace Arwend.Web.View.Mvc.Models.Binders
{
    public class ModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(Type modelType)
        {
            if (modelType == typeof(byte))
            {
                return new ByteModelBinder();
            }
            return null;
        }
    }
}
