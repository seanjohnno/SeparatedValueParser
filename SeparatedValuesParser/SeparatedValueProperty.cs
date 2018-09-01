using System;

namespace SeparatedValuesParser
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SeparatedValueProperty : Attribute
    {
        public SeparatedValueProperty(string title)
        {
            Title = title;
        }

        public string Title { get; }
    }
}
