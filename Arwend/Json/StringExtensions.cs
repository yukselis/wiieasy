using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arwend.Json
{
    public static class StringExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static dynamic ToJson(this string source)
        {
            var jObject = JObject.Parse(source); 
            return new DynamicJsonObject(jObject);
        }

    }
}
