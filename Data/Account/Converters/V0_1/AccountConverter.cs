namespace Mospolyhelper.Data.Account.Converters.V0_1
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Web;
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;
    using Domain.Account.Model;
    using Domain.Account.Model.V0_1;
    using Model;
    using Students = Domain.Account.Model.V0_1.Students;

    public class AccountConverter
    {
        private readonly ILogger logger;

        public AccountConverter(ILogger<AccountConverter> logger)
        {
            this.logger = logger;
        }

        public Students ParsePortfolios(string portfolios, int page)
        {
            this.logger.LogDebug("ParsePortfolios");
            var doc = new HtmlDocument();
            doc.LoadHtml(portfolios);

            var maxPage = new Regex("p=portfolio.*?pg=(.*?)\"")
                .Matches(doc.DocumentNode.InnerHtml)
                .Select(it => {
                    if (int.TryParse(it.Groups[1].Value, out var res))
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
            

            var portfolioList = doc.DocumentNode.Descendants("tr")
                .Where(it =>
                    it.InnerText.Contains("Группа", StringComparison.InvariantCultureIgnoreCase)
                ).Select(it => ParsePortfolio(it))
                .ToList();

            return new Students(
                maxPage,
                page,
                portfolioList
                );
        }

        private Portfolio ParsePortfolio(HtmlNode portfolioTable)
        {
            this.logger.LogDebug("ParsePortfolio");

            var tds = portfolioTable.Descendants("td").GetEnumerator();
            tds.MoveNext();
            var imageUrl = tds.Current.Descendants("img").FirstOrDefault()
                ?.GetAttributeValue("src", "img/no_avatar.jpg") ?? "img/no_avatar.jpg";
            tds.MoveNext();
            var portfolio = tds.Current.Descendants("div");
            var id = int.Parse(
                portfolio.FirstOrDefault()?
                .GetAttributeValue("id", "myModal_-1")
                .Split('_', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? "-1"
                );
            var name = portfolio.FirstOrDefault()?.Descendants("h4")?.FirstOrDefault()?.InnerText ?? string.Empty;
            var body = portfolio.ElementAtOrDefault(1)?.InnerHtml ?? string.Empty;
            var group = new Regex(@"Группа: <b>(.+?)<\/b><br")
                .Match(body).Groups[1].Value.Trim();
            var direction = new Regex("Направление подготовки \\(специальность\\): <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value.Trim();
            var specialization = new Regex("Специализация: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value.Trim();
            var course = new Regex("Курс: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value.Trim();
            var educationForm = new Regex("Форма обучения: <b>(.+?)<\\/b><br")
                .Match(body).Groups[1].Value.Trim();

            return new Portfolio(
                id,
                name,
                imageUrl,
                group,
                direction,
                specialization,
                course,
                educationForm
            );
        }

        public AccountTeachers ParseTeachers(string teachers, int page)
        {
            this.logger.LogDebug("ParseTeachers");
            var doc = new HtmlDocument();
            doc.LoadHtml(teachers);

            var maxPage = new Regex("p=teachers.*?pg=(.*?)\"")
                .Matches(doc.DocumentNode.InnerHtml)
                .Select(it => {
                    if (int.TryParse(it.Groups[1].Value, out var res))
                    {
                        return res as int?;
                    }
                    else
                    {
                        return null;
                    }
                }).Where(it => it != null)
                .Append(1)
                .Max(it => it!.Value);

            IList<AccountTeacher> resList;
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                resList = Array.Empty<AccountTeacher>();
            }
            else
            {
                resList = table.Descendants("tr").Select(ParseTeacher).ToList();
            }

            return new AccountTeachers(
                    maxPage,
                    page,
                    resList
                    );
        }

        private AccountTeacher ParseTeacher(HtmlNode tr)
        {
            this.logger.LogDebug("ParseTeacher");
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
            { "?p=payments", "payments" },
            { "?p=rasp", "" },
            { "?p=marks", "marks" },
            { "?p=stud_stats", "grade-sheets" },
            { "?p=projects", "" },
            { "?p=phys", "" },
            { "?p=group", "classmates" },
            { "?p=teachers", "teachers" },
            { "?p=sprav", "applications" },
            { "?p=myportfolio", "myportfolio" },
            { "?p=portfolio", "portfolios" }
        };

        public IList<string> ParsePermissions(string permissions)
        {
            this.logger.LogDebug("ParsePermissions");
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
            this.logger.LogDebug("ParseInfo");
            var doc = new HtmlDocument();
            doc.LoadHtml(info);

            var divs = doc.DocumentNode.Descendants("div");

            foreach (var div in divs)
            {
                var h4s = div.Descendants("h4");
                if (!h4s.Any())
                {
                    continue;
                }
                var contentDivs = div.Descendants("div").Where(node => node.InnerText.Contains("рождения"));
                if (!contentDivs.Any())
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
                var group = new Regex(@"Группа: <b><a.*?>(.+?)<\/a><\/b><br")
                    .Match(content).Groups[1].Value;
                var dormitoryAndRoom = new Regex(@"Общежитие: № <b>(.*?)</b>, комната <b>(.*?)</b><br>")
                    .Match(content);
                var dormitory = dormitoryAndRoom.Groups.Count > 1 ? dormitoryAndRoom.Groups[1].Value : string.Empty;
                var dormitoryRoom = dormitoryAndRoom.Groups.Count > 2 ? dormitoryAndRoom.Groups[2].Value : string.Empty;
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
                    dormitory,
                    dormitoryRoom,
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
            this.logger.LogDebug("ParseMarks");
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

        public GradeSheets? ParseGradeSheets(string gradeSheets)
        {
            this.logger.LogDebug("ParseGradeSheets");
            var doc = new HtmlDocument();
            doc.LoadHtml(gradeSheets);
            var semestersNode = doc.DocumentNode.Descendants("select")
                .FirstOrDefault(it => it.GetAttributeValue("name", string.Empty) == "kvartal")
                ?.Descendants("option") ?? Array.Empty<HtmlNode>();
            var semester = semestersNode.FirstOrDefault(it => it.GetAttributeValue("selected", string.Empty) == "selected")
                ?.GetAttributeValue("value", string.Empty) ?? string.Empty;
            var semesterList = semestersNode.Select(it => it.GetAttributeValue("value", string.Empty)).ToList();

            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return null;
            }

            var sheetList = new List<GradeSheet>();

            var trs = table.Descendants("tr").GetEnumerator();
            trs.MoveNext();
            while (trs.MoveNext())
            {
                var tds = trs.Current.Descendants("td").GetEnumerator();
                tds.MoveNext();
                tds.MoveNext();
                var aNumber = tds.Current.Descendants("a").FirstOrDefault();
                var number = aNumber?.InnerText ?? string.Empty;
                var id = aNumber?.GetAttributeValue("onclick", string.Empty)?.Split('\'')?.ElementAt(1) ?? string.Empty;
                tds.MoveNext();
                var subject = tds.Current.InnerText;
                while (subject.EndsWith("\r\n"))
                {
                    subject = subject.Remove(subject.Length - "\r\n".Length);
                }
                tds.MoveNext();
                var sheetType = tds.Current.InnerText;
                tds.MoveNext();
                var loadType = tds.Current.InnerText;
                tds.MoveNext();
                var appraisalsDate = tds.Current.InnerHtml.Replace("<br>", " ").Trim();
                if (appraisalsDate.EndsWith("\r\n"))
                {
                    appraisalsDate = appraisalsDate.Remove(appraisalsDate.Length - "\r\n".Length);
                }
                tds.MoveNext();
                var grade = tds.Current.InnerText;
                tds.MoveNext();
                var courseAndSemester = tds.Current.InnerText;
                sheetList.Add(new GradeSheet(
                    id,
                    number,
                    subject,
                    sheetType,
                    loadType,
                    appraisalsDate,
                    grade,
                    courseAndSemester
                    ));
            }
            return new GradeSheets(semester, semesterList, sheetList);
        }

        public GradeSheetInfo ParseGradeSheetInfo(string gradeSheetInfo)
        {
            this.logger.LogDebug("ParseGradeSheetInfo");
            var info = JsonSerializer.Deserialize<GradeSheetInfoApiModel>(gradeSheetInfo);
            return info.ToModel();
        }

        public IList<GradeSheetMark> ParseGradeSheetMarks(string marks)
        {
            this.logger.LogDebug("ParseGradeSheetMarks");
            var allMarks = JsonSerializer.Deserialize<GradeSheetAllMarksApiModel>(marks);
            var studentMarks = allMarks.Students;
            var doc = new HtmlDocument();
            doc.LoadHtml(studentMarks);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<GradeSheetMark>();
            }
            var resList = new List<GradeSheetMark>();
            var trList = table.Descendants("tr").ToList();
            if (trList.Count == 0)
            {
                return Array.Empty<GradeSheetMark>();
            }
            var trs = trList.AsEnumerable().Skip(1);
            foreach (var tr in trs)
            {
                var tds = tr.Descendants("td").GetEnumerator();
                tds.MoveNext();
                tds.MoveNext();
                tds.MoveNext();
                var name = tds.Current.Descendants("label").FirstOrDefault()?.InnerText ?? string.Empty;
                tds.MoveNext();
                var mark = tds.Current.Descendants("label").FirstOrDefault()?.InnerText ?? string.Empty;
                resList.Add(new GradeSheetMark(name, mark));
            }

            return resList;
        }

        public IList<Application> ParseApplications(string application)
        {
            this.logger.LogDebug("ParseApplications");
            var doc = new HtmlDocument();
            doc.LoadHtml(application);
            var table = doc.DocumentNode.Descendants("table").LastOrDefault();
            if (table == null)
            {
                return Array.Empty<Application>();
            }
            var resList = new List<Application>();
            var trList = table.Descendants("tr").ToList();
            if (trList.Count == 0)
            {
                return Array.Empty<Application>();
            }
            var trs = trList.AsEnumerable().Skip(1);
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

        public Payments ParsePayments(string payments)
        {
            this.logger.LogDebug("ParseApplications");
            var doc = new HtmlDocument();
            doc.LoadHtml(payments);
            var content = doc.DocumentNode
                .Descendants("div")
                .FirstOrDefault(it => it.GetAttributeValue("id", string.Empty) == "content");
            if (content == null)
            {
                return new Payments(new Dictionary<string, Contract>());
            }
            var resDict = new Dictionary<string, Contract>();

            var tables = content.Descendants("table").GetEnumerator();
            var amounts = content.Descendants("p")
                .Where(it => it.InnerHtml.Contains("руб", StringComparison.InvariantCultureIgnoreCase))
                .GetEnumerator();
            var hasDormContract = content.Descendants("h4")
                .Any(it => it.InnerHtml.Contains("ОПЛАТА ЗА ОБЩЕЖИТИЕ", StringComparison.InvariantCultureIgnoreCase));
            if (hasDormContract && tables.MoveNext())
            {
                var table = tables.Current;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"сумма:\s*?(.+?)\s*?руб.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty, 
                    out var paidAmount
                    );
                var contractName = new Regex(@"<i>(.+?) г\.,").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"<b>(.+?)<\/b>\s*?руб").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty,
                    out var debt
                    );
                var debtDate = new Regex(@"на (.+?) г\.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"<b>(.+?)<\/b>\s*?руб").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty,
                    out var remainingAmount
                    );
                var expirationDate = new Regex(@"\(до (.+?) г\.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;

                var paymentList = new List<Payment>();
                foreach (var tr in table.Descendants("tr"))
                {
                    var tds = tr.Descendants("td").GetEnumerator();
                    tds.MoveNext();
                    var date = tds.Current.InnerHtml;
                    tds.MoveNext();
                    tds.MoveNext();
                    int.TryParse(
                        new string(
                            (tds.Current.Descendants("b").FirstOrDefault()?.InnerHtml ?? string.Empty)
                            .Where(it => char.IsDigit(it) || it == '-').ToArray()
                            ),
                        out var amount
                        );
                    paymentList.Add(new Payment(date, amount));
                }
                var sberQR = content.Descendants("img").ElementAtOrDefault(1)?.GetAttributeValue("src", string.Empty) ?? string.Empty;
                resDict["dormitory"] = new Contract(
                    contractName,
                    paidAmount,
                    debt,
                    debtDate,
                    remainingAmount,
                    expirationDate,
                    paymentList,
                    sberQR
                    );
            }

            var hasTuitionContract = content.Descendants("h4")
                .Any(it => it.InnerHtml.Contains("ОПЛАТА ЗА ОБУЧЕНИЕ", StringComparison.InvariantCultureIgnoreCase));
            if (hasTuitionContract && tables.MoveNext())
            {
                var table = tables.Current;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"сумма:\s*?(.+?)\s*?руб.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty,
                    out var paidAmount
                    );
                var contractName = new Regex(@"<i>(.+?) г\.,").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"<b>(.+?)<\/b>\s*?руб").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty,
                    out var debt
                    );
                var debtDate = new Regex(@"на (.+?) г\.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;
                amounts.MoveNext();
                int.TryParse(
                    new Regex(@"<b>(.+?)<\/b>\s*?руб").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty,
                    out var remainingAmount
                    );
                var expirationDate = new Regex(@"\(до (.+?) г\.").Match(amounts.Current.InnerHtml)
                    .Groups.Values.ElementAtOrDefault(1)?.Value ?? string.Empty;

                var paymentList = new List<Payment>();
                foreach (var tr in table.Descendants("tr"))
                {
                    var tds = tr.Descendants("td").GetEnumerator();
                    tds.MoveNext();
                    var date = tds.Current.InnerHtml;
                    tds.MoveNext();
                    tds.MoveNext();
                    int.TryParse(
                        new string(
                            (tds.Current.Descendants("b").FirstOrDefault()?.InnerHtml ?? string.Empty)
                            .Where(it => char.IsDigit(it) || it == '-').ToArray()
                            ),
                        out var amount
                        );
                    paymentList.Add(new Payment(date, amount));
                }
                var sberQR = string.Empty;
                resDict["tuition"] = new Contract(
                    contractName,
                    paidAmount,
                    debt,
                    debtDate,
                    remainingAmount,
                    expirationDate,
                    paymentList,
                    sberQR
                    );
            }

            return new Payments(resDict);
        }

        public IList<Classmate> ParseClassmates(string classmates)
        {
            this.logger.LogDebug("ParseClassmates");
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
            this.logger.LogDebug("ParseDialogs");
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

                var senderBlock = tds.Current.Descendants("b").FirstOrDefault();
                var senderName = senderBlock?.Descendants("b")?.FirstOrDefault()?.InnerText?.Trim() ?? string.Empty;
                var senderGroup = senderBlock?.Descendants("font")?.FirstOrDefault()?.InnerText?.Trim() ?? string.Empty;
                var dateTime = tds.Current.Descendants("font").LastOrDefault()?.InnerText ?? string.Empty;
                var message = tds.Current.InnerHtml.Split("<br>").LastOrDefault()?.Trim() ?? string.Empty;
                tds.MoveNext();
                var hasAttachments = tds.Current.InnerHtml.Contains("прикреп", StringComparison.InvariantCultureIgnoreCase);
                tds.MoveNext();
                var hasRead = !tds.Current.InnerHtml.Contains("нов", StringComparison.InvariantCultureIgnoreCase);
                resList.Add(
                    new DialogPreview(
                        id,
                        messageKey,
                        authorName,
                        authorGroup,
                        imageUrl,
                        message,
                        dateTime,
                        senderImageUrl,
                        senderName,
                        senderGroup,
                        hasAttachments,
                        hasRead
                        )
                    );
            }
            return resList;
        }

        public IList<AccountMessage> ParseDialog(string dialog)
        {
            this.logger.LogDebug("ParseDialog");
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
                        //url = HttpUtility.UrlDecode(url);
                        var name = it.InnerText;
                        return new AccountAttachment(url ?? string.Empty, name);
                    })
                    ?.ToList() as IList<AccountAttachment> ?? Array.Empty<AccountAttachment>();
                tds.MoveNext();
                var dateTime = tds.Current.Descendants("font").FirstOrDefault()?.InnerText ?? string.Empty;
                var removeUrl = (tds.Current.Descendants("a").FirstOrDefault()
                        ?.GetAttributeValue("onclick", string.Empty)
                        .Split("'")).FirstOrDefault(it => it.Contains("&dlg="))
                    ?.Split("&dlg=")?.LastOrDefault() ?? string.Empty;
                removeUrl = HttpUtility.UrlDecode(removeUrl);
                resList.Add(
                    new AccountMessage(
                        id,
                        imageUrl,
                        authorName,
                        message,
                        dateTime,
                        attachments,
                        removeUrl
                        )
                    );
            }
            return resList;
        }

        public MyPortfolio ParseMyPortfolio(string html)
        {
            this.logger.LogDebug("ParseMyPortfolio");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var otherInfo = doc.DocumentNode
                .Descendants("textarea"
                ).FirstOrDefault(it => 
                    it.GetAttributeValue("name", string.Empty) == "otherinfo"
                    )
                ?.InnerText ?? string.Empty; ;

            var otherInfoIsPublic = doc.DocumentNode
                .Descendants("input"
                ).FirstOrDefault(it => 
                    it.GetAttributeValue("name", string.Empty) == "acces_otherinfo"
                    )
                ?.GetAttributeValue("value", 0) == 1;
            
            return new MyPortfolio(
                Encoding.UTF8.GetString(Convert.FromBase64String(otherInfo)),
                otherInfoIsPublic
                );
        }
    }
}
