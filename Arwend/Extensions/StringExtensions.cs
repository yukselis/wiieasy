using Arwend.Cryptography;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arwend
{
    public static class StringExtensions
    {
        private static readonly Encoding Encoding = Encoding.GetEncoding("Cyrillic");
        public static readonly string JavascriptEmailAddressRegex = "^[_a-z0-9-]+(\\.[_a-z0-9-]+)*@[a-z0-9-]+(\\.[a-z0-9-]+)*(\\.[a-z]{2,4})$";

        public static string Right(this string value, int length)
        {
            return Microsoft.VisualBasic.Strings.Right(value, length);
        }

        public static string Left(this string value, int length)
        {
            return Microsoft.VisualBasic.Strings.Left(value, length);
        }

        public static bool In(this string value, params string[] stringValues)
        {
            foreach (string comparedValue in stringValues)
                if (string.Compare(value, comparedValue) == 0)
                    return true;

            return false;
        }

        public static string Format(this string value, params object[] args)
        {
            return string.Format(value, args);
        }

        public static bool IsMatch(this string value, string pattern)
        {
            Regex regex = new Regex(pattern);
            return value.IsMatch(regex);
        }

        public static bool IsMatch(this string value, Regex regex)
        {
            return regex.IsMatch(value);
        }

        public static string ChangeInvalidSpaces(this string value)
        {
            return value.Replace((char)160, (char)32);
        }
        /// <summary>
        /// 64 digit tabanlı string veriyi 8 bitlik unsigned integer diziye dönüştürür.
        /// </summary>
        public static byte[] ToByteArray(this string text)
        {
            return System.Convert.FromBase64String(text);
        }

        /// <summary>
        /// 8 bitlik unsigned integer diziyi 64 digit tabanlı string'e dönüştürür.
        /// </summary>
        public static string ToText(this byte[] buffer)
        {
            return System.Convert.ToBase64String(buffer);
        }

        public static Nullable<T> ToNullable<T>(this string source) where T : struct
        {

            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(source) && source.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFromInvariantString(source);
                }
            }
            catch { }
            return result;
        }

        /// <summary>
        /// Decimal'i belirtilen formata ve kültürel bilgiye göre string'e çevirir.
        /// </summary>
        public static string ToPriceString(this decimal source)
        {
            return source.ToPriceString("tr-TR");
        }

        public static string ToPriceString(this decimal source, string cultureInfo)
        {
            return source.ToString("F", CultureInfo.GetCultureInfo(cultureInfo));
        }

        /// <summary>
        /// Decimal'i belirtilen kültürel bilgiye göre string'e çevirir.
        /// </summary>
        public static string ToPriceString(this string source)
        {
            return source.ToPriceString("tr-TR");
        }

        public static string ToPriceString(this string source, string cultureInfo)
        {
            return source.ToString(System.Globalization.CultureInfo.GetCultureInfo(cultureInfo));
        }

        /// <summary>
        /// Ondalıklı sayı biçimindeki string ifadedyi bir üst sayıya yuvarlayıp, sayının tam sayı kısmını alır.
        /// </summary>
        public static string ToRoundedValue(this string value)
        {
            var left = System.Math.Round(value.ToDecimal());
            return string.Format("{0:0}", left);
        }

        public static long ToInt16(this string value)
        {
            Int16 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int16.TryParse(value, out result);

            return result;
        }

        public static long ToInt32(this string value)
        {
            Int32 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int32.TryParse(value, out result);

            return result;
        }

        public static long ToInt64(this string value)
        {
            Int64 result = 0;

            if (!string.IsNullOrEmpty(value))
                Int64.TryParse(value, out result);

            return result;
        }

        /// <summary>
        /// Ondalıklı sayı biçimindeki string ifadeyi kontrollü biçimde decimal değerine çevirir.
        /// </summary>
        public static decimal ToDecimal(this string value)
        {
            var numberFormat = new NumberFormatInfo();
            numberFormat.NumberDecimalSeparator = ",";
            return Convert.ToDecimal(value.Replace(".", ","), numberFormat);
        }

        public static T ToEnum<T>(this string name) where T : struct
        {
            if (Enum.IsDefined(typeof(T), name))
                return (T)Enum.Parse(typeof(T), name, true);
            else return default(T);
        }

        public static string ToDashCase(this string input)
        {
            string pattern = "[A-Z]";
            string dash = "-";
            return Regex.Replace(input, pattern,
                m => ((m.Index == 0) ? string.Empty : dash) + m.Value.ToLowerInvariant());
        }

        public static string ToSlug(this string value)
        {

            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            var str = String.Join("", value.Normalize(NormalizationForm.FormD)
            .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark));

            str = value.RemoveAccent().ClearTurkishChars().ToLowerInvariant();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            str = Regex.Replace(str, @"\s+", " ").Trim();
            str = str.Substring(0, str.Length <= 200 ? str.Length : 200).Trim();
            str = Regex.Replace(str, @"\s", "-");
            str = Regex.Replace(str, @"-+", "-");
            return str;
        }

        /// <summary>
        /// String'i parçalar.
        /// </summary>
        public static string[] SplitString(this string value, string regexPattern, int maxLength)
        {
            string[] splitted = new string[3];

            if (string.IsNullOrEmpty(value))
                return splitted;

            value = value.Trim();

            if (value.Length > maxLength)
                value = value.Substring(0, maxLength);

            Match matchResults = null;
            Regex paragraphs = new Regex(regexPattern, RegexOptions.Singleline);
            matchResults = paragraphs.Match(value);
            if (matchResults.Success)
            {
                splitted[0] = matchResults.Groups[1].Value;
                splitted[1] = matchResults.Groups[2].Value;
                splitted[2] = matchResults.Groups[3].Value;
            }

            return splitted;

        }

        public static string AddLeadingZeros(this long value, int totalLength)
        {
            return value.AddLeadingZeros(totalLength, string.Empty);
        }

        public static string AddLeadingZeros(this long value, int totalLength, string prefix)
        {
            totalLength = totalLength - prefix.Length;
            return prefix + value.ToString().PadLeft(totalLength, '0');
        }

        public static string AddLeadingZeros(this string value, int totalLength, string prefix)
        {
            totalLength = totalLength - prefix.Length;
            return prefix + value.ToString().PadLeft(totalLength, '0');
        }

        public static string AddLeadingZeros(this int value, int totalLength, string prefix)
        {
            return ((long)value).AddLeadingZeros(totalLength, prefix);
        }

        public static string AddLeadingZeros(this byte value, int totalLength, string prefix)
        {
            return ((long)value).AddLeadingZeros(totalLength, prefix);
        }

        /// <summary>
        /// Clear metodunu çalıştırarak kaynak içindeki belirtilen karakterleri siler.
        /// </summary>
        public static string Clear(this string source)
        {
            if (string.IsNullOrEmpty(source)) return string.Empty;
            return source.Trim('_').Clear(new[] { ' ', '(', ')', '_', '-' });
        }

        /// <summary>
        /// Belirtilen karakterleri kaldırır string'den.
        /// </summary>
        public static string Clear(this string source, params char[] removeChars)
        {
            Guard.ArgumentNullException(removeChars, "removeChars");

            var destination = source;
            if (!string.IsNullOrEmpty(destination))
            {
                var splittedData = source.Split(removeChars, StringSplitOptions.RemoveEmptyEntries);
                destination = string.Concat(splittedData);
            }

            return destination;
        }

        public static string RemoveAccent(this string value)
        {
            byte[] bytes = Encoding.GetBytes(value);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string Encrypt(this string chipperText, string encryptionKey = "")
        {
            return Cryptography.CryptoManager.Encrypt(chipperText, encryptionKey);
        }
        public static string Decrypt(this string richText, string decryptionKey = "")
        {
            return Cryptography.CryptoManager.Decrypt(richText, decryptionKey);
        }
        public static string ComputeHash(this string plainText, string saltText = "", HashAlgorithms algorithm = HashAlgorithms.SHA1, Encoding encoding = null)
        {
            return Cryptography.CryptoManager.ComputeHash(plainText, saltText, algorithm, encoding);
        }
        public static string GetMd5Hash(this string plainText, string saltText = "")
        {
            return Cryptography.CryptoManager.GetMd5Hash(plainText, saltText);
        }
        
        /// <summary>
        /// Özel türkçe harfleri latin harflere çevirir.
        /// </summary>
        public static string ClearTurkishChars(this string value)
        {
            StringBuilder sb = new StringBuilder(value);
            sb = sb.Replace("ı", "i")
                   .Replace("ğ", "g")
                   .Replace("ü", "u")
                   .Replace("ş", "s")
                   .Replace("ö", "o")
                   .Replace("ç", "c")
                   .Replace("İ", "I")
                   .Replace("Ğ", "G")
                   .Replace("Ü", "U")
                   .Replace("Ş", "S")
                   .Replace("Ö", "O")
                   .Replace("Ç", "C");

            return sb.ToString();
        }

        public static bool HasValue(this string value, string[] array)
        {
            foreach (var item in array)
            {
                if (value.IndexOf(item, StringComparison.InvariantCultureIgnoreCase) > -1)
                    return true;
            }
            return false;
        }

        public static bool EndsWith(this string value, string[] array)
        {
            foreach (var item in array)
            {
                if (value.EndsWith(item, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
