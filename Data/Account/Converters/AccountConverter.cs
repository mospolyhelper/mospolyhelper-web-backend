using HtmlAgilityPack;
using Mospolyhelper.Domain.Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mospolyhelper.Data.Account.Converters
{
    public class AccountConverter
    {
        public IList<AccountPortfolio> ParsePortfolios(string portfolios)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(portfolios);

            return doc.DocumentNode.Descendants("td")
                .Where(it =>
                    it.InnerText.Contains("Группа", StringComparison.InvariantCultureIgnoreCase)
                ).Select(it => ParsePortfolio(it.Descendants("div")))
                .ToList();
        }

        private AccountPortfolio ParsePortfolio(IEnumerable<HtmlNode> portfolio)
        {
            var name = portfolio.FirstOrDefault()?.Descendants("h4")?.FirstOrDefault()?.InnerText ?? string.Empty;
            var body = portfolio.ElementAtOrDefault(1)?.InnerHtml ?? string.Empty;
            var group = new Regex("Группа: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value;
            var direction = new Regex("Направление подготовки \\(специальность\\): <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value;
            var specialization = new Regex("Специализация: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value;
            var course = new Regex("Курс: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value;
            var educationForm = new Regex("Форма обучения: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value;

            return new AccountPortfolio(
                name,
                group,
                direction,
                specialization,
                course,
                educationForm
                );
        }


        public AccountInfo? ParseInfo(string info)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(info);

            var divs = doc.DocumentNode.Descendants("div");

            foreach (var div in divs)
            {
                var h4s = div.Descendants("h4");
                if (h4s.Count() == 0)
                {
                    continue;
                }
                var contentDivs = div.Descendants("div").Where(node => node.InnerText.Contains("рождения"));
                if (contentDivs.Count() == 0)
                {
                    continue;
                }

                var name = h4s.First().InnerText;

                var content = contentDivs.First().InnerHtml;

                var status = new Regex("Статус: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var sex = new Regex("Пол: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var birthDate = new Regex("Дата рождения: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var studentCode = new Regex("Код студента: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var faculty = new Regex("Факультет: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var course = new Regex("Курс: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var group = new Regex("Группа: <b><a.*?>(.+?)<\\/a><\\/b><br")
                    .Match(content).Groups[1].Value;
                var direction = new Regex("Специальность: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var specialization = new Regex("Специализация: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var educationPeriod = new Regex("Срок обучения: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var educationForm = new Regex("Форма обучения: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var financingType = new Regex("Вид финансирования: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var educationLevel = new Regex("Уровень образования: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;
                var admissionYear = new Regex("Год набора: <b>(.+?)<\\/b><br")
                    .Match(content).Groups[1].Value;

                var ordersString = new Regex("<h4.*?>Приказы</h4>(.*)")
                    .Match(div.InnerHtml).Groups[1].Value;

                var htmlTagsClear = new Regex("<.*?>");
                var orders = new Regex("<p>(.*?)</p>")
                    .Matches(ordersString)
                    .Select(it => htmlTagsClear.Replace(it.Groups[1].Value, string.Empty))
                    .ToList();

                return new AccountInfo(
                    name,
                    status,
                    sex,
                    birthDate,
                    studentCode,
                    faculty,
                    course,
                    group,
                    direction,
                    specialization,
                    educationPeriod,
                    educationForm,
                    financingType,
                    educationLevel,
                    admissionYear,
                    orders
                    );
            }
            return null;
        }

        public AccountMarks? ParseMarks(string marks)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(marks);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return null;
            }

            var tempListOfList = new SortedDictionary<string, IDictionary<string, IList<AccountMark>>>();

            var tds = table.Descendants("td").GetEnumerator();
            while (tds.MoveNext())
            {
                var rowCourseCount = tds.Current.GetAttributeValue("rowspan", 0);
                if (rowCourseCount > 1)
                {
                    var course = tds.Current.InnerText;
                    tempListOfList[course] = new SortedDictionary<string, IList<AccountMark>>();
                    while (rowCourseCount > 0)
                    {
                        tds.MoveNext();
                        var rowSemesterCount = tds.Current.GetAttributeValue("rowspan", 0);
                        if (rowSemesterCount > 1)
                        {
                            var semester = tds.Current.InnerText;

                            var tempList = new AccountMark[rowSemesterCount];
                            for (var j = 0; j < tempList.Length && tds.MoveNext(); j++)
                            {
                                var subject = tds.Current.InnerText;
                                tds.MoveNext();
                                var loadType = tds.Current.InnerText;
                                tds.MoveNext();
                                var mark = tds.Current.InnerText;
                                tempList[j] = new AccountMark(subject, loadType, mark);
                            }
                            tempListOfList[course][semester] = tempList;
                            rowCourseCount -= rowSemesterCount;
                        }
                    }
                }
            }
            return new AccountMarks(tempListOfList);
        }

        //public IList<Account>
    }
}
