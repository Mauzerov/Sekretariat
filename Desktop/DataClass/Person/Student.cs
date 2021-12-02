using Desktop.DataClass.Include;

namespace Desktop.DataClass.Person
{
    public class Student : Person
    {
        public string Class;
        public SchoolGroup Group;

        public Student(params object[] args) : base(args)
        {
            Class = (string)args[index++];
            Group = (SchoolGroup)args[index++];
        }
    }
}