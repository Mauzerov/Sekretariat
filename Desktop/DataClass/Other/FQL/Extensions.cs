using System;

namespace Desktop.DataClass.Other.FQL
{
    public static class Extensions
    {
        public static bool Operand(this int value, Where.Operand where)
        {
            switch (where)
            {
                case Where.Operand.Eq:
                    return value == 0;
                case Where.Operand.Less:
                    return value < 0;
                case Where.Operand.Greater:
                    return value > 0;
            }

            throw new ArgumentException("Extensions: Unable To Compare!");
        }

        public static string TrimInside(this string s)
        {
            return s.Replace("\n", " ").Replace("\t", " ").Replace("  ", "");
        }
    }
}