using System;

namespace Desktop.DataClass.Other
{
    public class Where
    {
        [Flags]
        public enum Operand
        {
            Eq, Less, Greater
        }
        public string Key;
        public IComparable Value;
        public Operand Op;

        public static string HumanOp(Where where) => where.HumanOp();
        public string HumanOp()
        {
            if (Op == Operand.Eq)
                return "=";
            if (Op == (Operand.Eq | Operand.Less))
                return "<=";
            if (Op == (Operand.Eq | Operand.Greater)) return ">=";

            if (Op == Operand.Greater)
                return "<";
            if (Op == Operand.Less)
                return ">";
            throw new ArgumentException("Out Of Operands!");
        }

        public string Human() => $"{HumanOp()} \"{Value.ToString()}\"";
        public static string Human(Where where) => where.Human();
    }
}