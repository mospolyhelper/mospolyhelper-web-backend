namespace Mospolyhelper.Domain.Schedule.Models
{
    public class Auditorium
    {
        public string Title { get; }
        public string Color { get; }

        public Auditorium(string title, string color)
        {
            this.Title = title;
            this.Color = color;
        }
    }

    class AuditoriumExt
    {
        public static string ReplaceEmojiByText(string raw)
        {
            if (raw.Contains("\uD83D\uDCF7"))
                return raw.Replace("\uD83D\uDCF7", "(Вебинар)"); // 📷
            if (raw.Contains("\uD83C\uDFE0"))
                return raw.Replace("\uD83C\uDFE0", "(LMS)"); // 🏠
            return raw;

        }
    }
}
