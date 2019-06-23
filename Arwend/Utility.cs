using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Arwend
{
    public static class Utility
    {
        public static string GenerateRandomPassword(int maxSize)
        {
            char[] chars = new char[62];
            chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
        public static string GetQueryStringValue(string url, string queryString)
        {
            return System.Web.HttpUtility.ParseQueryString(url).Get(queryString);
        }
        public static string GetQueryStringValue(string url, string key, string seperator)
        {
            if (url.IndexOf(key,StringComparison.InvariantCultureIgnoreCase) > -1 && url.Contains(seperator))
            {
                url = (new Regex(seperator)).Replace(url, "?", 1);
                return GetQueryStringValue(url, key);
            }
            return string.Empty;
        }
        public static object Clone(object Original)
        {
            byte[] bytes = null;
            object clonedObject = null;
            var formatter = new BinaryFormatter();
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    formatter.Serialize(stream, Original);
                    bytes = stream.ToArray();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    clonedObject = formatter.Deserialize(stream);
                }
            }
            catch { }
            return clonedObject;
        }

        public static string EncodeUrl(string strIn)
        {
            if (string.IsNullOrEmpty(strIn)) return strIn;
            StringBuilder sbOut = new StringBuilder(strIn.Trim());
            sbOut = sbOut.Replace("-", "");
            sbOut = sbOut.Replace(" ", "-");
            sbOut = sbOut.Replace("  ", "-");
            sbOut = sbOut.Replace("ğ", "g");
            sbOut = sbOut.Replace("Ğ", "G");
            sbOut = sbOut.Replace("ü", "u");
            sbOut = sbOut.Replace("Ü", "U");
            sbOut = sbOut.Replace("ş", "s");
            sbOut = sbOut.Replace("Ş", "S");
            sbOut = sbOut.Replace("ç", "c");
            sbOut = sbOut.Replace("Ç", "C");
            sbOut = sbOut.Replace("ö", "o");
            sbOut = sbOut.Replace("Ö", "O");
            sbOut = sbOut.Replace("ı", "i");
            sbOut = sbOut.Replace("İ", "I");
            sbOut = sbOut.Replace("$", "-24");
            sbOut = sbOut.Replace("&", "-26");
            sbOut = sbOut.Replace("+", "-2B");
            sbOut = sbOut.Replace(",", "-2C");
            sbOut = sbOut.Replace("/", "-2F");
            sbOut = sbOut.Replace(":", "-3A");
            sbOut = sbOut.Replace(";", "-3B");
            sbOut = sbOut.Replace("=", "-3D");
            sbOut = sbOut.Replace("?", "-3F");
            sbOut = sbOut.Replace("@", "-40");
            sbOut = sbOut.Replace("'", "-22");
            sbOut = sbOut.Replace("<", "-3C");
            sbOut = sbOut.Replace(">", "-3E");
            sbOut = sbOut.Replace("#", "-23");
            sbOut = sbOut.Replace("%", "-25");
            sbOut = sbOut.Replace("{", "-7B");
            sbOut = sbOut.Replace("}", "-7D");
            sbOut = sbOut.Replace("|", "-7C");
            sbOut = sbOut.Replace("\\", "-5C");
            sbOut = sbOut.Replace("^", "-5E");
            sbOut = sbOut.Replace("~", "-7E");
            sbOut = sbOut.Replace("[", "-5B");
            sbOut = sbOut.Replace("]", "-5D");
            sbOut = sbOut.Replace("`", "-60");
            sbOut = sbOut.Replace(".", "-2E");
            sbOut = sbOut.Replace("\"", "-22");
            sbOut = sbOut.Replace("‘", "-91");
            sbOut = sbOut.Replace("’", "-92");
            sbOut = sbOut.Replace("ˆ", "-88");
            sbOut = sbOut.Replace("‚", "-83");
            return sbOut.ToString();
        }
    }
}
