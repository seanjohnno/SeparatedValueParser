using System;

namespace SeparatedValuesParser
{
    public class StringToType
    {
        public static object ToType(string value, Type type)
        {
            if (type == typeof(string))
            {
                return value;
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                return bool.Parse(value);
            }
            else if(type == typeof(byte) || type == typeof(byte?))
            {
                return byte.Parse(value);
            }
            else if(type == typeof(char) || type == typeof(char?))
            {
                return char.Parse(value);
            }
            else if(type == typeof(decimal) || type == typeof(decimal?))
            {
                return decimal.Parse(value);
            }
            else if(type == typeof(double) || type == typeof(double?))
            {
                return double.Parse(value);
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return float.Parse(value);
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                return int.Parse(value);
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                return long.Parse(value);
            }
            else if (type == typeof(sbyte) || type == typeof(sbyte?))
            {
                return sbyte.Parse(value);
            }
            else if (type == typeof(uint) || type == typeof(uint?))
            {
                return uint.Parse(value);
            }
            else if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return ulong.Parse(value);
            }
            else if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return ushort.Parse(value);
            }

            throw new NotSupportedException("Value type not supported");
        }
    }
}
