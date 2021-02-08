namespace Mospolyhelper.Data.Schedule.Converters
{
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;
    using Mospolyhelper.Domain.Schedule.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    public class ScheduleTeacherRemoteConverter
    {
        private readonly Regex regex0 = new Regex(@"\((.*?)\)");

        private readonly Regex regex1 = new Regex(@"(\p{L}|\))\(");
        private readonly Regex regex2 = new Regex(@"\)(\p{L}|\()");
        private readonly Regex regex3 = new Regex(@"(\p{L})-(\p{L})");

        private readonly Dictionary<string, int> map = new Dictionary<string, int>()
        {
            { "янв", 1 },
            { "фев", 2 },
            { "мар", 3 },
            { "апр", 4 },
            { "май", 5 },
            { "июн", 6 },
            { "июл", 7 },
            { "авг", 8 },
            { "сен", 9 },
            { "окт", 10 },
            { "ноя", 11 },
            { "дек", 12 }
        };


        private readonly ILogger logger;

        public ScheduleTeacherRemoteConverter(ILogger<ScheduleTeacherRemoteConverter> logger)
        {
            this.logger = logger;
        }

        public Domain.Schedule.Model.Schedule? Parse(string html)
        {
            this.logger.LogDebug("Parse");
            if (html == string.Empty)
            {
                return null;
            }
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var teacher = Teacher.FromFullName(
                doc.DocumentNode.Descendants("h3")
                .Where(it => it.HasClass("teacher-info__name"))
                .First().InnerText
                );

            var table = doc.DocumentNode.Descendants("table").First();

            var tempList = new List<List<Lesson>>() { 
                new List<Lesson>(), new List<Lesson>(), new List<Lesson>(), new List<Lesson>(),
                new List<Lesson>(), new List<Lesson>(), new List<Lesson>()
            };

            var rows = table.Descendants("tr");
            var rowIndex = 0;
            foreach (var row in rows)
            {
                var cells = row.Descendants("td");
                var cellIndex = 0;
                foreach (var cell in cells)
                {
                    var lessonDivs = cell.Elements("div");
                    foreach (var lessonDiv in lessonDivs)
                    {
                        var lesson = ParseLesson(lessonDiv, rowIndex, teacher);
                        tempList[(cellIndex + 1) % 7].Add(lesson);
                    }
                    cellIndex++;
                }
                rowIndex++;
            }
            return Domain.Schedule.Model.Schedule.From(
                tempList.Select(it => it as IList<Lesson>).ToList()
                );
        }

        private Lesson ParseLesson(HtmlNode node, int order, Teacher teacher)
        {
            var emoji = node.Elements("b")
                .Select(it => AuditoriumExt.ReplaceEmojiByText(it.InnerText))
                .FirstOrDefault() ?? string.Empty;

            var auditoriums = node.Descendants()
                .Where(it => it.HasClass("lesson__auditory"))
                .Select(it => new Auditorium(emoji + " " + it.InnerText, string.Empty));

            var (dateFrom, dateTo) = ParseDates(node);

            var lessonTitle = node.Descendants()
                .Where(it => it.HasClass("lesson__subject"))
                .FirstOrDefault()?.InnerText ?? "";
            var match = regex0.Matches(lessonTitle).LastOrDefault();
            var lessonType = match?.Groups?.Values.ElementAtOrDefault(1)?.Value ?? "Другое";
            if (match != null)
            {
                lessonTitle = lessonTitle.Replace($"({lessonType})", "");
            }
            lessonTitle = ProcessTitle(lessonTitle);
            lessonType = LessonExt.FixTeacherType(lessonType, lessonTitle);


            var groups = node.Descendants()
                .Where(it => it.HasClass("lesson__group"))
                .Select(it => new Domain.Schedule.Model.Group(it.InnerText, false));

            return new Lesson(
                order,
                lessonTitle,
                lessonType,
                new List<Teacher>() { teacher },
                auditoriums.ToList(),
                groups.ToList(),
                dateFrom,
                dateTo
                );
        }

        private (DateTime dateFrom, DateTime dateTo) ParseDates(HtmlNode node)
        {
            var dates = node.Descendants()
                .Where(it => it.HasClass("lesson__date"))
                .FirstOrDefault()
                ?.InnerText
                ?.Split('-')
                ?.Select(it => ParseDate(it))
                ?.ToList();

            DateTime dateFrom, dateTo;
            switch (dates?.Count)
            {
                case null:
                    {
                        dateFrom = DateTime.MinValue;
                        dateTo = DateTime.MaxValue;
                        break;
                    }
                case 0:
                    {
                        dateFrom = DateTime.MinValue;
                        dateTo = DateTime.MaxValue;
                        break;
                    }
                case 1:
                    {
                        dateFrom = dates[0];


                        var dateFromNextYear = dateFrom.AddYears(1);
                        var dateFromPrevYear = dateFrom.AddYears(-1);
                        var today = DateTime.Now;
                        var currentDifference = Math.Abs((dateFrom - today).Days);


                        if (Math.Abs((dateFromNextYear - today).Days) < currentDifference)
                        {
                            dateFrom = dateFromNextYear;
                        }
                        else if (Math.Abs((dateFromPrevYear - today).Days) < currentDifference)
                        {
                            dateFrom = dateFromPrevYear;
                        }
                        dateTo = dateFrom;
                        break;
                    }
                default:
                    {
                        dateFrom = dates[0];
                        dateTo = dates[1];

                        var dateFromNextYear = dateFrom.AddYears(1);
                        var dateToNextYear = dateTo.AddYears(1);
                        var currentDifference = (dateTo - dateFrom).Days;

                        // Add 1 year to dateTo and check if difference lower than current or not
                        // This condition for direct order of dates
                        if (Math.Abs(currentDifference) > Math.Abs((dateToNextYear - dateFrom).Days))
                        {
                            dateTo = dateToNextYear;
                        }
                        // If previous condition is false then try do same with dateFrom
                        // This condition for wrong (reversed) order of dates, e.g. jan 12 - sep 5
                        if (Math.Abs(currentDifference) > Math.Abs((dateTo - dateFromNextYear).Days))
                        {
                            dateFrom = dateFromNextYear;
                        }

                        // To fix wrong (reversed) order of dates
                        if ((dateTo - dateFrom).Days < 0)
                        {
                            var buf = dateTo;
                            dateTo = dateFrom;
                            dateFrom = buf;
                        }

                        // If date range is sep 5 - jan 12 and today is jan,
                        // then year of dates will displaced by 1
                        var days = (dateFrom - DateTime.Now).Days;
                        if ((DateTime.Now.AddDays(days)).Year - dateFrom.Year > 0)
                        {
                            dateTo = dateTo.AddYears(-1);
                            dateFrom = dateFrom.AddYears(-1);
                        }
                        break;
                    }
            }
            return (dateFrom, dateTo);
        }

        private string ProcessTitle(string rawTitle)
        {
            var res = rawTitle.Trim();
            res = regex1.Replace(res, "$1 (");
            res = regex2.Replace(res, ") $1");
            res = regex3.Replace(res, "$1\u200b-\u200b$2");
            res = res.First().ToString().ToUpper() + res.Substring(1);
            return res;
        }

        private DateTime ParseDate(string dateString)
        {
            var dayAndMonth = dateString.Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            int day = int.Parse(dayAndMonth[0].TrimStart('0'));
            var month = map[dayAndMonth[1].ToLower()];
            return new DateTime(DateTime.Now.Year, month, day);
        }
    }
}
