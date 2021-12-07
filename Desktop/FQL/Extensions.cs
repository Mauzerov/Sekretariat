using System;

namespace Desktop.DataClass.Other.FQL
{
    public static class Extensions
    {
        public static bool Operand(this int value, Where.Operand where, long newValue) =>
            Operand((long) value, where, (long) newValue);
        
        public static bool Operand(this long value, Where.Operand where, long newValue)
        {
            if (@where == Where.Operand.Eq)
                return value == newValue;
            if (@where == (Where.Operand.Less | Where.Operand.Eq))
                return value <= newValue;
            if (@where == (Where.Operand.Greater | Where.Operand.Eq))
                return value >= newValue;
            if (@where == Where.Operand.Less)
                return value < newValue;
            if (@where == Where.Operand.Greater)
                return value > newValue;
            if (@where == Where.Operand.Neq) return value != newValue;

            throw new ArgumentException("Extensions: Unable To Compare!");
        }

        public static string TrimInside(this string s)
        {
            return s.Replace("\n", " ").Replace("\t", " ").Replace("  ", "");
        }
    }
}