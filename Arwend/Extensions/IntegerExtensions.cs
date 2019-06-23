﻿using System;
using System.Globalization;

namespace Arwend
{
    public static class IntegerExtensions
    {
        public static int Half(this int source)
        {
            return source / 2;
        }

        public static int Cube(this int source)
        {
            return (int)Math.Pow(source, 3);
        }

        public static int Square(this int source)
        {
            return (int)Math.Pow(source, 2);
        }

        public static bool IsEvenNumber(this int number)
        {
            return ((number % 2) == 0);
        }

        public static bool IsOddNumber(this int number)
        {
            return ((number % 2) == 1);
        }

        public static string ToMonthString(this int value)
        {
            return (value >= 1 && value <= 12) ? CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(value) : "";
        }

        public static decimal ToPixel(this int point)
        {
            return Decimal.Truncate((point * 4) / 5) + 1;
        }

        public static int Seconds(this int source)
        {
            return source * 1000;
        }
    }
}
