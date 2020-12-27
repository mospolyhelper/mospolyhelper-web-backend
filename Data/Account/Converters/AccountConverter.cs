using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Mospolyhelper.Domain.Account.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Mospolyhelper.Data.Account.Converters
{
    public class AccountConverter
    {
        private readonly ILogger logger;

        public AccountConverter(ILogger<AccountConverter> logger)
        {
            this.logger = logger;
        }

        public Students ParsePortfolios(string portfolios, int page)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(portfolios);

            var maxPage = new Regex("p=portfolio.*?pg=(.*?)\"")
                .Matches(doc.DocumentNode.InnerHtml)
                .Select(it => {
                    if (int.TryParse(it.Groups[1].Value, out int res))
                    {
                        return res as int?;
                    }
                    else
                    {
                        return null;
                    }
                }).Where(it => it is int)
                .Append(1)
                .Max(it => it!.Value);
            

            var portfolioList = doc.DocumentNode.Descendants("td")
                .Where(it =>
                    it.InnerText.Contains("Группа", StringComparison.InvariantCultureIgnoreCase)
                ).Select(it => ParsePortfolio(it.Descendants("div")))
                .ToList();

            return new Students(
                maxPage,
                page,
                portfolioList
                );
        }

        private Portfolio ParsePortfolio(IEnumerable<HtmlNode> portfolio)
        {
            var id = int.Parse(
                portfolio.FirstOrDefault()?
                .GetAttributeValue("id", "myModal_-1")
                .Split('_', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? "-1"
                );
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

            return new Portfolio(
                id,
                name,
                group,
                direction,
                specialization,
                course,
                educationForm
                );
        }

        public AccountTeachers ParseTeachers(string teachers, int page)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(teachers);

            var maxPage = new Regex("p=teachers.*?pg=(.*?)\"")
                .Matches(doc.DocumentNode.InnerHtml)
                .Select(it => {
                    if (int.TryParse(it.Groups[1].Value, out int res))
                    {
                        return res as int?;
                    }
                    else
                    {
                        return null;
                    }
                }).Where(it => it is int)
                .Append(1)
                .Max(it => it!.Value);

            IList<AccountTeacher> resList = new List<AccountTeacher>();
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                resList = Array.Empty<AccountTeacher>();
            }
            else
            {
                resList = table.Descendants("tr").Select(tr => ParseTeacher(tr)).ToList();
            }

            return new AccountTeachers(
                    maxPage,
                    page,
                    resList
                    );
        }

        private AccountTeacher ParseTeacher(HtmlNode tr)
        {
            var id = int.Parse(
                   tr.GetAttributeValue("id", "t_-1")
                   .Split('_', StringSplitOptions.RemoveEmptyEntries)
                   .LastOrDefault() ?? "-1"
                   );
            var tds = tr.Descendants("td").GetEnumerator();
            tds.MoveNext();
            var imageUrl = tds.Current.Descendants("img").FirstOrDefault()
                ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "img/no_avatar.jpg";
            tds.MoveNext();
            var status = tds.Current.Descendants("img").FirstOrDefault()
                ?.GetAttributeValue("title", "offline") ?? "offline";
            tds.MoveNext();
            var name = tds.Current.Descendants("b").FirstOrDefault()?.InnerText ?? "Имя не найдено";
            var info = tds.Current.Descendants("font").FirstOrDefault()?.InnerText ?? string.Empty;
            tds.MoveNext();
            var messageKey = tds.Current.Descendants("a").FirstOrDefault()
                ?.GetAttributeValue("onclick", string.Empty)
                ?.Split("'", StringSplitOptions.RemoveEmptyEntries)
                ?.ElementAtOrDefault(1)
                ?.Split("&u=", StringSplitOptions.RemoveEmptyEntries)
                ?.LastOrDefault() ?? string.Empty;
            messageKey = HttpUtility.UrlDecode(messageKey);

            return new AccountTeacher(
                        id,
                        name,
                        info,
                        imageUrl,
                        status,
                        messageKey
                        );
        }

        private readonly Dictionary<string, string> urlDict = new Dictionary<string, string> 
        { 
            { "?", "info" },
            { "?p=about", "" },
            { "?p=alerts", "" },
            { "?p=messages", "dialogs" },
            { "?p=payments", "" },
            { "?p=rasp", "" },
            { "?p=marks", "marks" },
            { "?p=stud_stats", "" },
            { "?p=projects", "" },
            { "?p=phys", "" },
            { "?p=group", "classmates" },
            { "?p=teachers", "teachers" },
            { "?p=sprav", "applications" },
            { "?p=myportfolio", "myportfolio" },
            { "?p=portfolio", "portfolios" },
        };

        public IList<string> ParsePermissions(string permissions)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(permissions);

            var premissions = doc.DocumentNode.Descendants("ul")
                .Where(it => it.GetAttributeValue("class", string.Empty) == "categories")
                ?.FirstOrDefault()
                ?.Descendants("li")
                ?.Select(it => 
                it.Descendants("a")
                .FirstOrDefault()
                ?.GetAttributeValue("href", null)
                //?.Split("p=")
                //?.LastOrDefault()
                )
                .Select(it => it != null && urlDict.ContainsKey(it) ? urlDict[it] : null)
                .Where(it => !string.IsNullOrEmpty(it))
                .ToList() as IList<string> ?? Array.Empty<string>();

            return premissions;
        }

        public Info? ParseInfo(string info)
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

                return new Info(
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

        public IList<Application> ParseApplications(string application)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(application);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<Application>();
            }
            var resList = new List<Application>();
            var trs = table.Descendants("tr").Skip(1);
            foreach (var tr in trs)
            {
                var tds = tr.Descendants("td").GetEnumerator();
                tds.MoveNext();
                var dateTime = tds.Current.InnerHtml;
                tds.MoveNext();
                var regNumber = tds.Current.InnerHtml;
                tds.MoveNext();
                var name = tds.Current.Descendants("a").First().InnerHtml;
                var info = tds.Current.Descendants("div").First().InnerHtml;
                tds.MoveNext();
                var status = tds.Current.InnerHtml;
                tds.MoveNext();
                var department = tds.Current.InnerHtml;
                tds.MoveNext();
                var note = tds.Current.InnerHtml;
                resList.Add(
                    new Application(
                        regNumber,
                        name,
                        dateTime,
                        status,
                        department,
                        note,
                        info
                        )
                    );
            }
            return resList;
        }

        public IList<Classmate> ParseClassmates(string classmates)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(classmates);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<Classmate>();
            }
            var resList = new List<Classmate>();
            var trs = table.Descendants("tr");
            foreach (var tr in trs)
            {
                var id = int.Parse(
                    tr.GetAttributeValue("id", "t_-1")
                    .Split('_', StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault() ?? "-1"
                    );
                var tds = tr.Descendants("td").GetEnumerator();
                tds.MoveNext();
                var imageUrl = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "img/no_avatar.jpg";
                tds.MoveNext();
                var status = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("title", "offline") ?? "offline";
                tds.MoveNext();
                var name = tds.Current.InnerText;
                tds.MoveNext();
                var messageKey = tds.Current.Descendants("a").FirstOrDefault()
                    ?.GetAttributeValue("onclick", string.Empty)
                    ?.Split("'", StringSplitOptions.RemoveEmptyEntries)
                    ?.ElementAtOrDefault(1)
                    ?.Split("&u=", StringSplitOptions.RemoveEmptyEntries)
                    ?.LastOrDefault() ?? string.Empty;
                messageKey = HttpUtility.UrlDecode(messageKey);
                resList.Add(
                    new Classmate(
                        id,
                        name,
                        imageUrl,
                        status,
                        messageKey
                        )
                    );
            }
            return resList;
        }

        public IList<DialogPreview> ParseDialogs(string messages)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(messages);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<DialogPreview>();
            }
            var resList = new List<DialogPreview>();
            var trs = table.Descendants("tr");
            foreach (var tr in trs)
            {
                var id = int.Parse(
                    tr.GetAttributeValue("id", "t_-1")
                    .Split('_', StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault() ?? "-1"
                    );
                var messageKey = tr.GetAttributeValue("onclick", string.Empty)
                    ?.Split("'", StringSplitOptions.RemoveEmptyEntries)
                    ?.ElementAtOrDefault(1)
                    ?.Split("&dlg=", StringSplitOptions.RemoveEmptyEntries)
                    ?.LastOrDefault() ?? string.Empty;
                messageKey = HttpUtility.UrlDecode(messageKey);

                var tds = tr.Descendants("td").GetEnumerator();
                tds.MoveNext();
                var imageUrl = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "img/no_avatar.jpg";
                tds.MoveNext();
                var authorName = tds.Current.Descendants("b").FirstOrDefault()
                    ?.Descendants("b")?.FirstOrDefault()?.InnerText ??
                    tds.Current.Descendants("b").FirstOrDefault()?.InnerText ?? string.Empty;
                var authorGroup = tds.Current.Descendants("font").FirstOrDefault()?.InnerText ?? string.Empty;
                tds.MoveNext();
                var senderImageUrl = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "";
                var senderName = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("title", "") ?? "";
                var date = tds.Current.Descendants("font").LastOrDefault()?.InnerText ?? string.Empty;
                var message = tds.Current.InnerHtml.Split("<br>").LastOrDefault()?.Trim() ?? string.Empty;
                tds.MoveNext();
                var hasAttachments = tds.Current.InnerHtml.Contains("прикреп", StringComparison.InvariantCultureIgnoreCase);
                tds.MoveNext();
                var hasRead = tds.Current.InnerHtml.Contains("нов", StringComparison.InvariantCultureIgnoreCase);
                resList.Add(
                    new DialogPreview(
                        id,
                        messageKey,
                        authorName,
                        authorGroup,
                        imageUrl,
                        message,
                        date,
                        senderImageUrl,
                        senderName,
                        hasAttachments,
                        hasRead
                        )
                    );
            }
            return resList;
        }

        public IList<AccountMessage> ParseDialog(string dialog)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(dialog);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<AccountMessage>();
            }
            var resList = new List<AccountMessage>();
            var trs = table.Descendants("tr");
            foreach (var tr in trs)
            {
                var id = int.Parse(
                    tr.GetAttributeValue("id", "t_-1")
                    .Split('_', StringSplitOptions.RemoveEmptyEntries)
                    .LastOrDefault() ?? "-1"
                    );

                var tds = tr.Descendants("td").GetEnumerator();
                tds.MoveNext();
                var imageUrl = tds.Current.Descendants("img").FirstOrDefault()
                    ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "img/no_avatar.jpg";
                tds.MoveNext();
                var authorNameAndTarget = tds.Current.Descendants("b");
                var authorName = authorNameAndTarget.FirstOrDefault()?.InnerText ?? string.Empty;
                var messageTo = authorNameAndTarget.ElementAtOrDefault(1)?.InnerText;
                var message = tds.Current.Descendants("div").FirstOrDefault()
                    ?.InnerHtml ?? string.Empty;
                var index = message.LastIndexOf("<div");
                if (index != -1 && message.Substring(index).Contains("креп"))
                {
                    message = message.Substring(0, index);
                }
                var attachments = tds.Current.Descendants("div").FirstOrDefault()
                    ?.Descendants("div").FirstOrDefault()
                    ?.Descendants("a")
                    ?.Select(it => 
                    {
                        var url = it.GetAttributeValue("href", null)?.Split("f=")?.LastOrDefault();
                        url = HttpUtility.UrlDecode(url);
                        var name = it.InnerText;
                        return new AccountAttachment(url, name);
                    })
                    ?.ToList() as IList<AccountAttachment> ?? Array.Empty<AccountAttachment>();
                tds.MoveNext();
                var date = tds.Current.Descendants("font").FirstOrDefault()?.InnerText ?? string.Empty;
                var removeUrl = tds.Current.Descendants("a").FirstOrDefault()
                    ?.GetAttributeValue("onclick", string.Empty)
                    .Split("'").Where(it => it.Contains("&dlg=")).FirstOrDefault()
                    ?.Split("&dlg=")?.LastOrDefault() ?? string.Empty;
                removeUrl = HttpUtility.UrlDecode(removeUrl);
                resList.Add(
                    new AccountMessage(
                        id,
                        imageUrl,
                        authorName,
                        message,
                        attachments,
                        removeUrl
                        )
                    );
            }
            return resList;
        }

        public MyPortfolio ParseMyPortfolio(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var otherInfo = doc.DocumentNode
                .Descendants("textarea")
                .Where(it =>
                it.GetAttributeValue("name", string.Empty) == "otherinfo"
                ).FirstOrDefault()?.InnerText ?? string.Empty; ;

            var otherInfoIsPublic = doc.DocumentNode
                .Descendants("input")
                .Where(it => 
                it.GetAttributeValue("name", string.Empty) == "acces_otherinfo"
                ).FirstOrDefault()?.GetAttributeValue("value", 0) == 1;
            
            return new MyPortfolio(
                Encoding.UTF8.GetString(Convert.FromBase64String(otherInfo)),
                otherInfoIsPublic
                );
        }
    }
}
