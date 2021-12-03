using System;

namespace Desktop.DataClass.Other
{
    public class Where
    {
        public enum Operand
        {
            Eq, Less, Greater
        }
        public string Key;
        public IComparable Value;
        public Operand Op;
    }
}