using System;

namespace Arwend.Attributes
{
    public class IntegerValueAttribute : Attribute
    {
        public IntegerValueAttribute(string title, int value)
        {
            Title = title;
            Value = value;
        }

        public string Title { get; protected set; }
        public int Value { get; protected set; }
    }
}