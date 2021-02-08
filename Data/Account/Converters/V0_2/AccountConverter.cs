namespace Mospolyhelper.Data.Account.Converters.V0_2
{
    using HtmlAgilityPack;
    using Microsoft.Extensions.Logging;
    using Domain.Account.Model.V0_2;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AccountConverter
    {
        private readonly ILogger logger;
        private const string site = "https://e.mospolytech.ru/";


        public AccountConverter(ILogger<AccountConverter> logger)
        {
            this.logger = logger;
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
            { "?p=portfolio", "students" }
        };

        public UserAuth PasrseAuth(string html, string sessionId)
        {
            this.logger.LogDebug("PasrseAuth");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var user = doc.DocumentNode.Descendants("form")
                .Where(it => it.GetAttributeValue("id", string.Empty) == "newsletter-form")
                .FirstOrDefault();
            var avatarUrl = user?.Descendants("img")?.FirstOrDefault()?.GetAttributeValue("src", string.Empty) 
                ?? "img/no_avatar.jpg";
            avatarUrl = site + avatarUrl;
            var name = user?.Descendants("p")?.FirstOrDefault()?.Descendants("b")?.FirstOrDefault()?.InnerText ?? "Ошибка";

            var premissions = doc.DocumentNode.Descendants("ul")
                .Where(it => it.GetAttributeValue("class", string.Empty) == "categories")
                ?.FirstOrDefault()
                ?.Descendants("li")
                ?.Select(it =>
                it.Descendants("a")
                .FirstOrDefault()
                ?.GetAttributeValue("href", null)
                )
                .Select(it => it != null && urlDict.ContainsKey(it) ? urlDict[it] : null)
                .Where(it => !string.IsNullOrEmpty(it))
                .ToList() as IList<string> ?? Array.Empty<string>();

            return new UserAuth(
                sessionId,
                name,
                avatarUrl,
                premissions
                );
        }
    }
}
