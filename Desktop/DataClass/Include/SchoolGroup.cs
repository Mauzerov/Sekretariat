using System;

namespace Desktop.DataClass.Include
{
    [Flags]
    public enum SchoolGroup : long
    {
        None = 0,
        English = 1 << 0,
        German = English << 1,
        Italian = German << 1,
        Russian = Italian << 1,
        Chess = Russian << 1,
        Pe = Chess << 1,
        Math = Pe << 1,
        Physics = Math << 1,
        ComputerScience = Physics << 1,
        Other = ComputerScience << 1
    }
}