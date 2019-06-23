using System;
using System.Linq;
using System.Reflection;

namespace Arwend.Reflection
{
    public static class ReflectionExtensions
    {
        public static object GetPropertyValue<TResult>(this TResult source, string property) where TResult: class
        {
            return source.GetType().GetProperty(property, (BindingFlags.Public | BindingFlags.Instance)).GetValue(source);
        }
    }
}
