using System.Text.Json.Serialization;

namespace TB.Shared.Enums
{
    public class Enums
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FileExtensions
        {
            Csv = 1,
            Pdf,
            Doc,
            Docx,
            Xls,
            Xlsx,
            Jpg,
            Jpeg,
            Png,
            Gif

        }
    }
}
