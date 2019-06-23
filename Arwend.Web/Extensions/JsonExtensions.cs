using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Arwend.Web.Extensions
{
    public static class JsonExtensions
    {
        //public static string ToJson(this object obj, int recursionDepth)
        //{
        //   return new JavaScriptSerializer { RecursionLimit = recursionDepth }.Serialize(obj);
        //}
        public static string ToJson(this object obj)
        {
            if (obj != null)
                return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            //return new JavaScriptSerializer().Serialize(obj);
            return string.Empty;
            //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            //using (MemoryStream stream = new MemoryStream())
            //{
            //    serializer.WriteObject(stream, obj);
            //    string json = Encoding.UTF8.GetString(stream.ToArray());
            //    stream.Close();
            //    return json;
            //}
        }
        public static T FromJson<T>(this string value)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(value);
            //return new JavaScriptSerializer().Deserialize<T>(value);
            //DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(T));
            //using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(value)))
            //{
            //    T obj = (T)deserializer.ReadObject(stream);
            //    stream.Close();
            //    return obj;
            //}
        }
    }
}
