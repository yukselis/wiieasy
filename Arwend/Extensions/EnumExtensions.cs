using System;
using System.Collections.Generic;
using System.Linq;
using Arwend.Attributes;

namespace Arwend
{
    public static class EnumExtensions
    {
        public static string ToStringValue(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];

            return attributes?.Length > 0 ? attributes[0].StringValue : value.ToString();
        }

        public static int ToIntegerValue(this Enum value)
        {
            return value.ToIntegerValueAttribute()?.Value ?? value.ToInt32();
        }

        public static IntegerValueAttribute ToIntegerValueAttribute(this Enum value)
        {
            var type = value.GetType();
            var fieldInfo = type.GetField(value.ToString());
            var attributes = fieldInfo.GetCustomAttributes(typeof(IntegerValueAttribute), false) as IntegerValueAttribute[];
            return attributes?[0];
        }

        public static int ToInt32(this Enum value)
        {
            return Convert.ToInt32(value);
        }

        public static byte ToByte(this Enum value)
        {
            return Convert.ToByte(value);
        }

        public static string GetValue<T>(this Enum value)
        {
            return Convert.ChangeType(value, typeof(T)).ToString();
        }

        public static List<TEnum> ToList<TEnum>()
        {
            var type = typeof(TEnum);

            if (type.BaseType == typeof(Enum))
                throw new ArgumentException("T must be type of System.Enum");

            var values = Enum.GetValues(type);
            if (values.Length > 0)
                return values.Cast<TEnum>().ToList();
            return null;
        }
    }
}