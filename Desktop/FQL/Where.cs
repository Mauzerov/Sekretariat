using System;

namespace Desktop.DataClass.Other.FQL
{
    public class Where
    {
        [Flags]
        public enum Operand
        {
            Eq, Less, Greater, Neq
        }
        public string Key;
        public IComparable Value;
        public Operand Op;

        public static string HumanOp(Where where) => where.HumanOp();
        public string HumanOp()
        {
            if (Op == Operand.Neq)
                return "<>";
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

        public static Operand OperandFromString(string s)
        {
            switch (s)
            {
                case "<>":
                    return Operand.Neq;
                case "=":
                    return Operand.Eq;
                case "<=":
                    return Operand.Eq | Operand.Less;
                case ">=":
                    return Operand.Eq | Operand.Greater;
                case "<":
                    return Operand.Less;
                case ">":
                    return Operand.Greater;
                default:
                    throw new ArgumentException("Wrong Operand String");
            }
        }
    }
}
