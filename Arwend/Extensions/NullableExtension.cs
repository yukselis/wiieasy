﻿using System;

namespace Arwend
{
    public static class NullableExtension
    {
        public static Nullable<T> AsNullable<T>(this T value) where T : struct
        {
            return new Nullable<T>(value);
        }
    }
}
