namespace Mospolyhelper.Data.Account.Model
{
    using System.Text.Json.Serialization;

    public class GradeSheetAllMarksApiModel
    {
        [JsonPropertyName("students")]
        public string Students { get; set; }

        [JsonPropertyName("html")]
        public string Html { get; set; }
    }
}
