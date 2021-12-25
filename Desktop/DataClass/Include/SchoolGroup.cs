using System;

namespace Desktop.DataClass.Include
{
    [Flags]
    public enum SchoolGroup : long
    {
        Zadne = 0,
        Angielski = 1 << 0,
        Niemiecki = Angielski << 1,
        Wloski = Niemiecki << 1,
        Rosyjski = Wloski << 1,
        Szachy = Rosyjski << 1,
        Wf = Szachy << 1,
        Matematyka = Wf << 1,
        Fizyka = Matematyka << 1,
        Informatyka = Fizyka << 1,
        Inne = Informatyka << 1
    }
}