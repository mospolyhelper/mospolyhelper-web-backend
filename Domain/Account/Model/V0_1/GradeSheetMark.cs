namespace Mospolyhelper.Domain.Account.Model.V0_1
{
    public class GradeSheetMark
    {
        public GradeSheetMark(string name, string mark)
        {
            Name = name;
            Mark = mark;
        }

        public string Name { get; }
        public string Mark { get; }
    }
}
