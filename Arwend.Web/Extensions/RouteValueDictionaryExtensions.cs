using System;
using System.Web.Routing;
namespace Arwend.Web.Extensions
{
    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary MergeAttributes(this RouteValueDictionary source, RouteValueDictionary target)
        {
            foreach (string key in target.Keys)
            {
                if (key.Equals("styles", StringComparison.InvariantCultureIgnoreCase))
                {
                    source = source.MergeStyles(target[key].ToString()); 
                    continue;
                }
                if (source.ContainsKey(key))
                    source[key] += " " + target[key];
                else
                    source.Add(key, target[key]);
            }
            return source;
        }
        public static RouteValueDictionary MergeStyles(this RouteValueDictionary source, string styles)
        {
            if (source.ContainsKey("style"))
                source["style"] += "; " + styles;
            else
                source.Add("style", styles);
            return source;
        }
    }
}
